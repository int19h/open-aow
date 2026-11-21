#nullable disable
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Units;

// Status. Mode. Player. location. IndependentRelation. UnfogRange. Upkeep. OnUpdate.
// OldAIBehavior. destination. UserMode. VisibilityRanges. ConcealedUnits
// The original's TArmy derives from TUnitList. The port's army owns its troops as its node
// children (it routes messages to them, notifies their changes, frees them and stores them) and
// reads them through Units. The members below that carry a Unit.PAS line are TUnitList's part.
public class Army(CustomNode owner) : ObservableNode(owner), IChildList<Troop>, IUnitHolder
{
    private const MoveTypes AllMoveTypes = World.Movement.MoveTypes.Walking | World.Movement.MoveTypes.Flying | World.Movement.MoveTypes.Swimming | World.Movement.MoveTypes.Forestry | World.Movement.MoveTypes.CaveCrawling | World.Movement.MoveTypes.Mountaineering | World.Movement.MoveTypes.LavaCrossing | World.Movement.MoveTypes.Tunneling;

    private const MoveTypes ForestryThroughLavaCrossing = World.Movement.MoveTypes.Forestry | World.Movement.MoveTypes.CaveCrawling | World.Movement.MoveTypes.Mountaineering | World.Movement.MoveTypes.LavaCrossing;

    public new const int ClassId = 131_606;

    /// <summary>The troops of this army, in their stored order.</summary>
    public UnitCollection Units => new(this);

    // An army takes troops only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop;

    Troop IOwner<Troop>.this[int index] => Units[index];

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    public ArmyStatuses Status { get; set; }

    public ArmyModes Mode { get; set; }
    public sbyte Player { get; set; }
    public MapCoordinate Location { get; set; } = new MapCoordinate(-1, -1, -1);

    public sbyte IndependentRelation { get; private set; } = -1;

    private byte _unfogRange;
    private byte _upkeep;

    private NotifyEvent<object> _update;

    internal void SetUpdateHandler(NotifyEvent<object> handler) => _update = handler;

    public GroupBehavior OldAIBehavior { get; set; }
    public MapCoordinate Destination { get; set; } = new MapCoordinate(-1, -1, -1);
    public ArmyUserModes UserMode { get; set; }

    private byte _visibilityRanges;

    public byte ConcealedUnits { get; private set; }

    // Army.PAS:336
    public override void Dispose()
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            StrategicMap.Current.NotifyArmyDestroyed(this);
        }

        _update = null;
        base.Dispose();
    }

    // Army.PAS:1898
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Byte);
        if (persister.Loading || StrategicMap.Current is null || StrategicMap.Current.ConnectionType != PlayerConnectionType.Network)
        {
            Destination = persister.Coordinate(name: "Destination", value: Destination, width: CoordinateWidth.Byte);
            UserMode = persister.Set("UserMode", value: UserMode);
        }

        OldAIBehavior = persister.Enum("OldAIBehavior", value: OldAIBehavior);
        Mode = persister.Set("Mode", value: Mode);
        // The translate stream's mode holds read, so the original clears the flag there too.
        if ((persister.Loading || persister.Translating()) && Mode.Contains(ArmyModes.Moving))
        {
            Mode &= ~ArmyModes.Moving;
        }
    }

    // Army.PAS:2259
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 537001987)
        {
            NewTurnDone((sbyte)message.Payload.Int);
        }

        if (message.MessageId == 537001986)
        {
            LockChanged();
        }

        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            UnlockChanged();
        }

        if (message.MessageId == 537001989)
        {
            SetPlayer(Player);
        }
        else
        {
            if (message.MessageId == 537001990 && StrategicMap.Current.SeatedPlayer != -1)
            {
                UpdateUnfog();
                UpdateConcealment();
            }
            else
            {
                if (message.MessageId == 537001992)
                {
                    UpdateVisibilityRanges();
                    UpdateExploration();
                    if (StrategicMap.Current.SeatedPlayer != -1)
                    {
                        UpdateUnfog();
                        UpdateConcealment();
                    }
                }
                else
                {
                    if (message.MessageId == 537001986)
                    {
                        NewTurn((sbyte)message.Payload.Int);
                    }
                    else
                    {
                        if (message.MessageId == 537001985)
                        {
                            NewDay();
                        }
                        else
                        {
                            if (message.MessageId == 537001987 && message.Payload.Int == Player)
                            {
                                _ = ValidateCrusaders();
                            }
                        }
                    }
                }
            }
        }
    }

    // Unit.PAS:2294
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                _ = Units[i].Release();
            }
        }
    }

    // Unit.PAS:2177
    public override void Clear()
    {
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            Units[i].SetOwner(null);
        }

        ClearChildren();
    }

    // Unit.PAS:2184
    public override void DestroyAllChildren() => Clear();

    // Army.PAS:1844
    public override int RemoveChild(EngineObject child)
    {
        int result;
        using (BeginUpdate())
        {
            EngineObject unit = child;
            _ = unit.AddRef();

            result = RemoveUnitChild(child);
            EngineObject unit2 = child;
            _ = unit2.Release();

        }
        return result;
    }

    // Unit.PAS:2162, the unit list's part of RemoveChild.
    private int RemoveUnitChild(EngineObject child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            _ = child.Release();
        }

        return result;
    }

    // Army.PAS:1853
    public override int AddChild(EngineObject child)
    {
        // The check refuses a child that is not a troop before the troop is read.
        CheckChild(child);
        Troop troop = (Troop)child;
        int result;
        if (!Status.Contains(ArmyStatuses.Activated) || troop.CanActivate())
        {
            result = AddUnitChild(troop);
            if (result != -1)
            {
                if (Status.Contains(ArmyStatuses.Activated) && !troop.UnitStatus.Contains(UnitStatuses.Activated))
                {
                    troop.Activate();
                }

                troop.SetPlayer(Player);
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // Unit.PAS:2167, the unit list's part of AddChild.
    private int AddUnitChild(Troop troop)
    {
        if (CanAddUnit(troop))
        {
            _ = troop.AddRef();
            return base.AddChild(troop);
        }

        return -1;
    }

    // Army.PAS:716
    protected override void OnChange() => Update();

    // Army.PAS:1241
    public sbyte GetPlayer() => Player;

    // Army.PAS:1874
    public virtual MapCoordinate GetLocation() => Status.Contains(ArmyStatuses.Activated) ? Location : MapCoordinate.None;

    // Army.PAS:1719
    public virtual bool CanAddUnit(Troop troop)
    {
        bool result;
        if (Status.Contains(ArmyStatuses.Activated))
        {
            result = Units.Count < MaxSize() && troop.CanAddToList(Units);
            bool hasTransporter = IndexOfTransporter(255) != -1;
            if (result && !hasTransporter && Location.X != -1 && Location.X != 0 && !Mode.Contains(ArmyModes.DisableTerrainValidation))
            {
                // { Original skips terrain validation on the zero X column. }
                MapField field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
                result = troop.ValidTerrain(field.TerrainType, field.OverlayType);
            }

            if (result && troop.GetTransporter())
            {
                return !hasTransporter && troop.GetTransportCapacity() >= Units.Count;
            }
        }
        else
        {
            return true;
        }

        return result;
    }

    // Army.PAS:1836
    public virtual bool CanDeleteUnit(Troop troop) => IndexOfTransporter(255) == -1 || !troop.GetTransporter() || UnitsInTransporter() == 0;

    // Army.PAS:1868
    public bool AddUnit(Troop troop)
    {
        troop.SetOwner(this);
        return this == troop.Owner;
    }

    // Army.PAS:1770
    public bool AddUnits(UnitCollection units)
    {
        bool result = CanAddUnits(units) == AddUnitsResult.Ok;
        if (result)
        {
            using (BeginUpdate())
            {
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    _ = AddUnit(units.GetUnit(i));
                }
            }
        }

        return result;
    }

    // Unit.PAS:2208
    public bool DeleteUnit(Troop troop)
    {
        if (CanDeleteUnit(troop))
        {
            troop.SetOwner(null);
        }
        else
        {
            return false;
        }

        return true;
    }

    // Army.PAS:1269
    public virtual bool GetConcealedForPlayer(sbyte player)
    {
        bool result;
        MapFieldMessage message = default;
        if (player != -1)
        {
            result = true;
            MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
            message.MessageId = 4363;
            message.Result = 0;
            MapFieldMessageParameters param = message.Param;
            param[1] = player;
            message.Param = param;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
            while (result && walk.Radius < 8)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    MapField field = level.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & MapField.ViewerFlag) != 0)
                    {
                        MapFieldMessageParameters param2 = message.Param;
                        param2[0] = walk.Radius;
                        message.Param = param2;
                        field.SendMapFieldMessage(ref message);
                        result = message.Result.Int == 0;
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Army.PAS:343
    public void Kill(byte selection)
    {
        using (BeginUpdate())
        {
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                if (((selection >>> i) & 1) != 0)
                {
                    Units[i].Killed();
                }
            }
        }
    }

    // Army.PAS:352
    public int GetStrength(byte selection, DamageType damageType)
    {
        int result = 0;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            if (((1 << i) & selection) != 0)
            {
                result += Units[i].GetStrength(damageType);
            }
        }

        return result;
    }

    // Army.PAS:361
    public int GetStrengthMax(byte selection, DamageTypes damageTypes)
    {
        int result = 0;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            if (((1 << i) & selection) != 0)
            {
                result += Units[i].GetStrengthMax(damageTypes);
            }
        }

        return result;
    }

    // Army.PAS:370
    public void ReturnBattleFieldInfo(ref BattleFieldInfo info)
    {
        DiplomaticRelation relation = AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, otherPlayer: Player, otherIndependentRelation: IndependentRelation);
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            Troop troop = Units[i];
            GroupBehavior behavior = troop.Group is not null ? troop.Group.GroupClass.Behavior : GroupBehavior.Passive;

            // A default collection has no holder and counts every troop, as a missing list did.
            bool includeUnit = info.Units.Holder is null || info.Units.IndexOf(troop) == -1;
            if (includeUnit && info.Requests.Contains(BattleFieldInfoRequests.IgnorePassiveIndependents))
            {
                includeUnit = troop.Player != 0 || (troop.Group is not null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive && troop.Group.GroupClass.Behavior != GroupBehavior.Guard);
            }

            if (includeUnit && info.Requests.Contains(BattleFieldInfoRequests.IgnoreIndependents) && troop.Player == 0)
            {
                includeUnit = false;
            }

            if (includeUnit)
            {
                int strength = troop.GetStrengthMax(info.DamageTypes);
                if (info.FriendlyRelations.Contains(relation.ToFlag()))
                {
                    info.FriendlyStrength += strength;
                }

                if (info.EnemyRelations.Contains(relation.ToFlag()) || behavior == GroupBehavior.GuardArea)
                {
                    info.Strength += strength;
                }
            }
        }
    }

    // Army.PAS:394
    public bool HealUnits(byte selection)
    {
        bool result = false;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                Troop troop = Units[i];
                if (troop.GetHitPoints() != troop.GetHits())
                {
                    result = true;
                    troop.SetHitPoints(troop.GetHits());
                }
            }

            selection = (byte)(selection >>> 1);
        }

        return result;
    }

    // Army.PAS:410
    public int CreateGroupId(byte selection)
    {
        int i = 0;
        while ((selection & 1) == 0 && i != Units.Count)
        {
            i += 1;
            selection = (byte)(selection >>> 1);
        }

        return i != Units.Count ? Units[i].UnitId : 0;
    }

    // Army.PAS:423
    public AIGroup GetGroup(byte selection)
    {
        AIGroup result = null;
        bool different = false;
        int i = 0;
        bool found = false;
        while (i != Units.Count && !different)
        {
            if ((selection & 1) != 0)
            {
                if (!found)
                {
                    result = Units[i].Group;
                    found = true;
                }
                else
                {
                    different = result != Units[i].Group;
                }
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return !found || different ? null : result;
    }

    // Army.PAS:445
    public void SetGroup(byte selection, AIGroup group)
    {
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                _ = group.AddUnit(Units[i]);
            }

            selection = (byte)(selection >>> 1);
        }
    }

    // Army.PAS:455
    public byte GetGroupSelection(int groupId)
    {
        byte result = 0;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            result = (byte)(result << 1);
            if (groupId == Units[i].GroupId)
            {
                result = (byte)(result | 1);
            }
        }

        return result;
    }

    // Army.PAS:466
    public GroupBehavior GetBehavior(byte selection)
    {
        AIGroup group = GetGroup(selection);
        return group is not null ? group.GroupClass.Behavior : GroupBehavior.None;
    }

    // Army.PAS:473
    public void SetBehavior(byte selection, GroupBehavior behavior)
    {
        AIGroup group = StrategicMap.Current.AIGroupManager.CreateGroup(Player, relation: IndependentRelation, behavior);
        if (group is not null)
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    _ = group.AddUnit(Units[i]);
                }

                selection = (byte)(selection >>> 1);
            }

            _ = group.Release();
        }
    }

    // Army.PAS:488
    public GroupBehavior GetOldAIBehavior()
    {
        GroupBehavior result = GroupBehavior.None;
        int i = Units.Count;
        while (result == GroupBehavior.None && i != 0)
        {
            i -= 1;
            result = (GroupBehavior)Units.GetUnit(i).LegacyGroupBehavior;
        }

        return result;
    }

    // Army.PAS:500
    public void InitializeGroupBehavior()
    {
        byte selection = 255;
        Hero hero = Hero(selection, types: HeroTypes.Leader | HeroTypes.HeroLeader);
        int index;
        if (hero is not null)
        {
            index = Units.IndexOf(hero);
            if (index != -1)
            {
                SetBehavior((byte)(1 << index), behavior: GroupBehavior.Fortify);
                selection = (byte)(selection & ~(1 << index));
            }
        }

        index = IndexOfTransporter(selection);
        if (index != -1 && OldAIBehavior == GroupBehavior.None)
        {
            selection = (byte)(selection & ~(1 << index));
        }

        if (selection != 0)
        {
            GroupBehavior behavior = OldAIBehavior;
            if (behavior == GroupBehavior.None)
            {
                behavior = Player == 0 && StrategicMap.Current.IndependentAIMode != StrategicMapIndependentAIMode.Off
                    ? Location.X != -1 && StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) is null
                        ? IndependentRelation >= 0 ? GroupBehavior.Patrol : GroupBehavior.GuardArea
                        : GroupBehavior.Passive
                    : StrategicMap.Current.Players.GetPlayers(Player).AIControl.GetRandomGroupBehavior();
            }

            SetBehavior(selection, behavior);
        }
    }

    // Army.PAS:533
    public void UpdateGroupBehavior()
    {
        byte selection = GetGroupSelection(0);
        if (selection != 0)
        {
            int index = IndexOfTransporter(selection);
            if (index != -1)
            {
                selection = (byte)(selection & ~(1 << index));
                if (Units[index].GroupId == 0)
                {
                    SetBehavior((byte)(1 << index), behavior: GroupBehavior.Transport);
                }
            }

            GroupBehavior behavior = GroupBehavior.Passive;
            if (Player == 0 && Hero(selection, types: HeroTypes.Hero) is not null)
            {
                behavior = GroupBehavior.Scout;
            }
            else
            {
                if (Player == 0 && StrategicMap.Current.IndependentAIMode != StrategicMapIndependentAIMode.Off && Location.X != -1)
                {
                    if (StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) is null)
                    {
                        behavior = IndependentRelation >= 0 ? GroupBehavior.Patrol : GroupBehavior.GuardArea;
                    }
                }
            }

            if (selection != 0)
            {
                index = Units.IndexOfBehavior(behavior);
                if (index == -1)
                {
                    SetBehavior(selection, behavior);
                }
                else
                {
                    SetGroup(selection, Units[index].Group);
                }
            }
        }
    }

    // Army.PAS:560
    public ArmyId CreateId(byte selection)
    {
        ArmyId id = new(null)
        {
            Holder = CreationHolder(),
            Location = new MapCoordinate(Location.X, Location.Y, Location.MapLevel),
            Player = Player
        };
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                _ = id.UnitIds.Add(Units[i].UnitId);
            }

            selection = (byte)(selection >>> 1);
        }

        return id;
    }

    // Army.PAS:579
    public ArmyHexSprite GetArmyHexSprite()
    {
        MapCoordinate coordinate26 = GetLocation();
        sbyte x = coordinate26.X;
        sbyte y = coordinate26.Y;
        sbyte l = coordinate26.MapLevel;
        return x != -1 ? StrategicMap.Current.MapContainer.GetField(x, y, l).GetArmyHexSprite() : null;
    }

    // Army.PAS:591
    public void AssignSettings(Army army)
    {
        Destination = new MapCoordinate(army.Destination.X, army.Destination.Y, army.Destination.MapLevel);
        Player = army.Player;
    }

    // Army.PAS:599
    public void SetFullyConcealed(bool value)
    {
        if (value != Status.Contains(ArmyStatuses.FullyConcealed))
        {
            Status = value
                ? Status | ArmyStatuses.FullyConcealed
                : Status & ~ArmyStatuses.FullyConcealed;

            if (Status.Contains(ArmyStatuses.Activated) && Location.X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, radius: 0);
            }
        }
    }

    // Army.PAS:610
    public bool FullyConcealed => Status.Contains(ArmyStatuses.FullyConcealed);

    // Army.PAS:613
    public void UpdateConcealment()
    {
        if (Status.Contains(ArmyStatuses.Activated))
        {
            bool concealed = ConcealedUnits != 0 && StrategicMap.Current.SeatedPlayer != Player && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && GetConcealedForPlayer(StrategicMap.Current.SeatedPlayer);

            Leader leader = Leader(255);
            bool showCrown = leader is not null;
            if (showCrown && concealed)
            {
                int indexOf = Units.IndexOf(leader);
                if (((ConcealedUnits >>> indexOf) & 1) != 0)
                {
                    showCrown = false;
                }
            }

            bool changed = false;
            Status = showCrown
                ? Status | ArmyStatuses.ShowCrown
                : Status & ~ArmyStatuses.ShowCrown;

            if (concealed != Status.Contains(ArmyStatuses.Concealed))
            {
                Status = concealed
                    ? Status | ArmyStatuses.Concealed
                    : Status & ~ArmyStatuses.Concealed;

                changed = true;
            }

            concealed = concealed && Units.Count == ConcealedUnitsCount();
            if (concealed != FullyConcealed)
            {
                SetFullyConcealed(concealed);
                changed = true;
            }

            if (changed)
            {
                TriggerOnUpdateEvent();
            }
        }
    }

    // Army.PAS:639
    public void SetMapCoordinate(MapCoordinate location)
    {
        Location = location;
        if (UpdateCount == 0)
        {
            UpdateConcealedUnits();
        }
    }

    public void SetMapCoordinate(sbyte x, sbyte y, sbyte level)
        => SetMapCoordinate(new MapCoordinate(x, y, level));

    // Army.PAS:647
    public int GetRace(byte selection)
    {
        int result = -1;
        bool same = true;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                int race = Units[i].GetRace();
                if (race != -1)
                {
                    if (result != -1)
                    {
                        same = same && race == result;
                    }
                    else
                    {
                        result = race;
                    }
                }
            }

            selection = (byte)(selection >>> 1);
        }

        return !same ? -1 : result;
    }

    // Army.PAS:666
    public byte GetUpkeep() => _upkeep;

    // Army.PAS:669
    public void ResetDestinationPosition() => Destination = new MapCoordinate(-1, -1, -1);

    // Army.PAS:672
    public void SetDestinationPosition(sbyte X, sbyte Y, sbyte L) => Destination = new MapCoordinate(X, Y, L);

    // Army.PAS:675
    public int ConcealedUnitsCount()
    {
        byte selection = ConcealedUnits;
        int result = 0;
        while (selection != 0)
        {
            if ((selection & 1) != 0)
            {
                result += 1;
            }

            selection = (byte)(selection >>> 1);
        }

        return result;
    }

    // Army.PAS:687
    public void UpdateConcealedUnits()
    {
        if (Location.X != -1 && Location.MapLevel != -1 && Status.Contains(ArmyStatuses.Activated))
        {
            ConcealedUnits = 0;
            int i = Units.Count;
            MapField field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            while (i != 0)
            {
                i -= 1;
                ConcealedUnits = (byte)(ConcealedUnits << 1);
                if (Units[i].ConcealedOnMapField(field))
                {
                    ConcealedUnits = (byte)(ConcealedUnits | 1);
                }
            }
        }
    }

    // Army.PAS:705
    public void UpdateUpkeep()
    {
        byte upkeep = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            upkeep = (byte)(upkeep + Units[i].GetUpkeep());
        }

        byte previous = _upkeep;
        if (previous != upkeep && Status.Contains(ArmyStatuses.Activated))
        {
            int delta = upkeep - previous;
            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep + delta);
        }

        _upkeep = upkeep;
    }

    // Army.PAS:719
    public void ValidateTerrain(sbyte terrainType, sbyte overlayType, sbyte player)
    {
        Troop transport = Transporter(255);
        byte selection;
        if (transport is null || !transport.ValidTerrain(terrainType, overlayType))
        {
            selection = 0;
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                selection = (byte)(selection << 1);
                if (!Units[i].ValidTerrain(terrainType, overlayType))
                {
                    selection = (byte)(selection | 1);
                }
            }
        }
        else
        {
            selection = 0;
        }

        if (selection != 0)
        {
            if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && StrategicMap.Current.Day != 0)
            {
                using (BeginUpdate())
                {
                    ArmyEventLog log = new(null)
                    {
                        Holder = CreationHolder(),
                        Player = player
                    };
                    if (terrainType is (sbyte)TerrainType.Water or (sbyte)TerrainType.UndergroundWater)
                    {
                        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsDrownText);
                        string unitsDrown = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsDrownText);
                        _ = log.ArmyMessages.Add(unitsDrown);
                        log.ArmyIcon = 3;
                    }
                    else
                    {
                        switch (terrainType)
                        {
                            case (sbyte)TerrainType.Laval:
                                log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsBurnedText);
                                string unitsBurned = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsBurnedText);
                                _ = log.ArmyMessages.Add(unitsBurned);
                                log.ArmyIcon = 0;
                                break;
                            default:
                                log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsCrushedText);
                                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsCrushedText);
                                _ = log.ArmyMessages.Add(translatedText);
                                log.ArmyIcon = 2;
                                break;
                        }
                    }

                    log.ArmyView.Setup(this, selection: 255);
                    for (int i = 0, last = Units.Count - 1; i <= last; i++)
                    {
                        Troop troop = Units.GetUnit(i);
                        if (((selection >>> i) & 1) != 0)
                        {
                            sbyte hitPoints = troop.GetHitPoints();
                            _ = log.DamageValues.Add((byte)hitPoints);
                            _ = log.DamageTypes.Add(9);
                            troop.SetHitPoints(0);
                        }
                        else
                        {
                            _ = log.DamageValues.Add(0);
                            _ = log.DamageTypes.Add(9);
                        }
                    }

                    log.ValidateGameOver();
                    EventLog.DistributeLocationEventLog(log, Player, Location.X, Location.Y, Location.MapLevel, execute: true);
                    _ = log.Release();
                }
            }

            Validate();
        }
    }

    // Army.PAS:785
    public void ValidateTerrain(sbyte terrainType, sbyte overlayType) => ValidateTerrain(terrainType, overlayType, Player);

    // Army.PAS:790
    public void Validate()
    {
        StrategicMap.Current.LockGameOverEvent();
        using (BeginUpdate())
        {
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                Troop troop = Units[i];
                if (troop.GetHitPoints() == 0)
                {
                    if (troop.GetAbilityEnabled(107))
                    {
                        Unit newUnit = new(null) { Holder = CreationHolder() };
                        newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(152));
                        troop.Killed();
                        _ = AddUnit(newUnit);
                    }
                    else
                    {
                        troop.Killed();
                    }
                }
            }
        }
        StrategicMap.Current.UnlockGameOverEvent();
    }

    // Army.PAS:812
    public void UpdateFormation()
    {
        string sourceName = "";
        int level = 0;
        Troop troop;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            troop = Units[i];
            _ = troop.UpdateMoraleValue();
            if (troop.GetAbilityEnabled(46) || troop.GetAbilityLevel(46) > 0)
            {
                GamePack.Current.AbilityControl.GetAbility<LeadershipAbility>(46).ResetExternalSource(troop);
                int unitLevel = troop.GetAbilityLevel(46);
                if (unitLevel > level)
                {
                    level = unitLevel;
                    sourceName = troop.GetAbilityOwner(46).GetOwnerName();
                    if (sourceName == AbilityRules.TranslateResourceString(GetEngine(), S: s_armyInherentText))
                    {
                        sourceName = troop.GetName();
                    }
                }
            }
        }

        if (level > 0)
        {
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                troop = Units[i];
                if (troop.GetAbilityLevel(46) < level)
                {
                    GamePack.Current.AbilityControl.GetAbility<LeadershipAbility>(46).SetExternalSource(troop, level: (byte)level, sourceName);
                }
            }
        }
    }

    // Army.PAS:841
    public void Update()
    {
        if (UpdateCount == 0 && Status.Contains(ArmyStatuses.Activated))
        {
            LockChanged();
            Units.Sort();
            UpdateVisibilityRanges();
            UpdateUnfog();
            UpdateUpkeep();
            UpdateConcealedUnits();
            UpdateConcealment();
            UpdateFormation();
            UpdateIndependentRelation();
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && Location.X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, radius: 0);
            }

            UnlockChanged();
            TriggerOnUpdateEvent();
        }
    }

    // Army.PAS:861
    public void TriggerOnUpdateEvent()
    {
        if (UpdateCount == 0 && Status.Contains(ArmyStatuses.Activated))
        {
            StrategicMap.Current.NotifyArmyChanged(this);
            _update?.Invoke(this);
        }
    }

    // Army.PAS:870
    public void Activate()
    {
        Assert(!Status.Contains(ArmyStatuses.Activated), message: "Army already activated", file: "Army.PAS", line: 1171);
        if (OldAIBehavior == GroupBehavior.None)
        {
            OldAIBehavior = GetOldAIBehavior();
        }

        for (int i = Units.Count - 1; i >= 0; i--)
        {
            Troop troop = Units[i];
            if (!troop.UnitStatus.Contains(UnitStatuses.Activated))
            {
                troop.Activate();
            }
        }

        Status |= ArmyStatuses.Activated;
        _upkeep = 0;
        if (Player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(Player).Armies.AddChild(this);
        }

        // { Original treats X = 0 as an unset location. }
        if (Location.X == 0)
        {
            Location = new MapCoordinate(-1, -1, -1);
        }

        Update();
    }

    // Army.PAS:1186
    public void Deactivate()
    {
        if (Status.Contains(ArmyStatuses.Activated))
        {
            if (_unfogRange != 0)
            {
                _ = StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, range: _unfogRange);
                _unfogRange = 0;
            }

            for (int i = Units.Count - 1; i >= 0; i--)
            {
                Troop troop = Units[i];
                if (troop.UnitStatus.Contains(UnitStatuses.Activated))
                {
                    troop.Deactivate();
                }
            }

            if (Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                Player players = StrategicMap.Current.Players.GetPlayers(Player);
                StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
            }

            Status &= ~ArmyStatuses.Activated;
        }
    }

    // Army.PAS:1211
    public byte ValidateSelection(byte selection) => (byte)(selection & AbilityRules.ArmySelectionMasks[Units.Count]);

    // Army.PAS:1216
    public int SelectionCount(byte selection)
    {
        selection = (byte)(selection & AbilityRules.ArmySelectionMasks[Units.Count]);
        int result = 0;
        while (selection != 0)
        {
            if ((selection & 1) != 0)
            {
                result += 1;
            }

            selection = (byte)(selection >>> 1);
        }

        return result;
    }

    // Army.PAS:1227
    public byte ValidateMoveSelection(byte selection)
    {
        byte result = ValidateSelection(selection);
        if (IndexOfTransporter(255) != -1 && (UnitsInTransporter() & ~selection) != 0 && Transporter(selection) is not null)
        {
            Troop transporter = Transporter(255);
            return (byte)(result & ~(1 << IndexOfChild(transporter)));
        }

        return result;
    }

    // Army.PAS:1235
    public void Select() => Status |= ArmyStatuses.Selected;

    // Army.PAS:1238
    public void Unselect() => Status &= ~ArmyStatuses.Selected;

    // Army.PAS:1244
    public bool GetUnitConcealed(int index)
    {
        MapField field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        return Units[index].ConcealedOnMapField(field);
    }

    // Army.PAS:1262
    public bool GetUnitsConcealedForPlayer(byte selection, sbyte player)
    {
        selection = ValidateSelection(selection);
        return selection == (ConcealedUnits & selection) && GetConcealedForPlayer(player);
    }

    // Army.PAS:1298
    public void SetPlayer(sbyte player)
    {
        if (StrategicMap.Current.Players.GetPlayers(player) is null)
        {
            player = 0;
        }

        if (player != Player)
        {
            using (BeginUpdate())
            {
                if (Status.Contains(ArmyStatuses.Activated))
                {
                    if (Player != -1)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Player);
                        StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
                        _ = StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                    }

                    Player = player;
                    if (Player != -1)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Player);
                        StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep + _upkeep);
                        _ = StrategicMap.Current.Players.GetPlayers(Player).Armies.AddChild(this);
                    }

                    for (int i = Units.Count - 1; i >= 0; i--)
                    {
                        Units[i].SetPlayer(Player);
                    }

                    if (Location.X != -1)
                    {
                        StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, radius: 0);
                    }

                    if (Player > 0 && StrategicMap.Current.Day > 0)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                        {
                            SetBehavior(255, behavior: GroupBehavior.Passive);
                        }
                    }

                    UpdateUnfog();
                    UpdateExploration();
                }
                else
                {
                    Player = player;
                }
            }
        }
    }

    // Army.PAS:1332
    public DamageTypes ImmunityTypes(byte selection)
    {
        DamageTypes result = AbilityRules.AllDamageTypes;
        int i = 0;
        while (result != DamageTypes.None && i != Units.Count)
        {
            if ((selection & 1) != 0)
            {
                result &= Units[i].GetImmunityTypes();
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1346
    public int ObtainValue(byte selection)
    {
        int result = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                result += Units[i].GetObtainValue();
            }

            selection = (byte)(selection >>> 1);
        }

        return result;
    }

    // Army.PAS:1357
    public sbyte MovePoints(byte selection)
    {
        Troop transport = Transporter(selection);
        sbyte result;
        if (transport is not null)
        {
            return transport.GetMovePoints();
        }
        else
        {
            if (selection != 0)
            {
                result = 50;
                for (int i = 0, last = Units.Count - 1; i <= last; i++)
                {
                    if ((selection & 1) != 0)
                    {
                        Troop troop = Units[i];
                        if (troop.GetMovePoints() < result)
                        {
                            result = troop.GetMovePoints();
                        }
                    }

                    selection = (byte)(selection >>> 1);
                }
            }
            else
            {
                return 0;
            }
        }

        return result;
    }

    // Army.PAS:1378
    public sbyte Moves(byte selection)
    {
        Troop transport = Transporter(selection);
        sbyte result;
        if (transport is not null)
        {
            return transport.Moves;
        }
        else
        {
            if (selection != 0)
            {
                result = 50;
                for (int i = 0, last = Units.Count - 1; i <= last; i++)
                {
                    if ((selection & 1) != 0)
                    {
                        Troop troop = Units[i];
                        if (troop.Moves < result)
                        {
                            result = troop.Moves;
                        }
                    }

                    selection = (byte)(selection >>> 1);
                }
            }
            else
            {
                return 0;
            }
        }

        return result;
    }

    // Army.PAS:1399
    public MoveTypes MoveTypes(byte selection)
    {
        Troop transport = Transporter(selection);
        MoveTypes result;
        if (transport is not null)
        {
            return transport.GetMoveTypes();
        }
        else
        {
            result = World.Movement.MoveTypes.None;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    result |= Units[i].GetMoveTypes();
                }

                selection = (byte)(selection >>> 1);
            }
        }

        return result;
    }

    // Army.PAS:1416
    public MoveTypes GetPartyMoves(byte selection)
    {
        MoveTypes result = default;
        Troop transport = Transporter(selection);
        if (transport is null)
        {
            MoveTypes common = AllMoveTypes;
            int moveType = (int)MoveType.Flying;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if (((1 << i) & selection) != 0)
                {
                    MoveTypes moves = Units[i].GetMoveTypes();
                    common &= moves;
                    if (moveType != -1)
                    {
                        if ((MoveType)moveType == MoveType.Flying)
                        {
                            if (!moves.Contains(World.Movement.MoveTypes.Flying))
                            {
                                moveType = moves.Contains(World.Movement.MoveTypes.Swimming) ? (int)MoveType.Swimming : (int)MoveType.Walking;
                            }
                        }
                        else
                        {
                            if ((MoveType)moveType == MoveType.Swimming)
                            {
                                if (((World.Movement.MoveTypes.Flying | World.Movement.MoveTypes.Swimming) & moves) == World.Movement.MoveTypes.None)
                                {
                                    moveType = (int)MoveType.Walking;
                                }
                            }
                            else
                            {
                                if ((MoveType)moveType == MoveType.Walking)
                                {
                                    if (((World.Movement.MoveTypes.Walking | World.Movement.MoveTypes.Flying | ForestryThroughLavaCrossing) & moves) == World.Movement.MoveTypes.None)
                                    {
                                        moveType = -1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // { Original leaves Result uninitialized when the units share no movement mode. }
            if (moveType != -1)
            {
                result = ((MoveType)moveType).ToFlag();
            }

            result |= common;
        }
        else
        {
            return transport.GetMoveTypes();
        }

        return result;
    }

    // Army.PAS:1450
    public int MoveOnMovePoints(byte selection, sbyte terrainType, sbyte overlayType)
    {
        MovePointTable table = default;
        table.Fill(1);
        Troop transport = Transporter(selection);
        if (transport is not null)
        {
            transport.CreateMovePointTable(out table);
        }
        else
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    Units[i].CreateMovePointTable(out MovePointTable unitTable);
                    table.Maximize(unitTable);
                }

                selection = (byte)(selection >>> 1);
            }
        }

        return table[terrainType][overlayType - MovementTables.FirstOverlay];
    }

    // Army.PAS:1469
    public MovePointTable CreateMovePointTable(byte selection)
    {
        int count = 0;
        MovePointTable table = default;
        table.Fill(1);
        Troop transport = Transporter(selection);
        if (transport is not null)
        {
            transport.CreateMovePointTable(out table);
            count += 1;
        }
        else
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    Units[i].CreateMovePointTable(out MovePointTable unitTable);
                    table.Maximize(unitTable);
                    count += 1;
                }

                selection = (byte)(selection >>> 1);
            }
        }

        if (count == 0)
        {
            table.Fill(-1);
        }
        return table;
    }

    // Army.PAS:1494
    public bool ValidTerrain(byte selection, sbyte terrainType, sbyte overlayType)
    {
        Troop transport = Transporter(selection);
        bool result;
        if (transport is null)
        {
            int i = 0;
            result = true;
            while (i != Units.Count && result)
            {
                if (((1 << i) & selection) != 0)
                {
                    result = Units[i].ValidTerrain(terrainType, overlayType);
                }

                i += 1;
            }
        }
        else
        {
            return transport.ValidTerrain(terrainType, overlayType);
        }

        return result;
    }

    // Army.PAS:1512
    public bool ValidTerrain(sbyte terrainType, sbyte overlayType)
    {
        Troop transport = Transporter(255);
        bool result;
        if (transport is null)
        {
            int i = 0;
            result = true;
            while (i != Units.Count && result)
            {
                result = Units[i].ValidTerrain(terrainType, overlayType);
                i += 1;
            }
        }
        else
        {
            return transport.ValidTerrain(terrainType, overlayType);
        }

        return result;
    }

    // Army.PAS:1529
    public bool CanPlace(sbyte X, sbyte Y, sbyte L)
    {
        MapField field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        bool result = ValidTerrain(field.TerrainType, field.OverlayType);
        if (!Status.Contains(ArmyStatuses.Activated))
        {
            int i = 0;
            while (result && i != Units.Count)
            {
                result = Units[i].CanActivate();
                i += 1;
            }
        }

        return result;
    }

    // Army.PAS:1543
    public void MovedTo(MapField oldField, MapField newField, int maxCost)
    {
        using (BeginUpdate())
        {
            UserMode &= ~ArmyUserModes.Guard;
            Troop troop = Transporter(255);
            if (troop is null)
            {
                for (int i = Units.Count - 1; i >= 0; i--)
                {
                    Units[i].MovedTo(oldField, newField, maxCost);
                }
            }
            else
            {
                troop.MovedTo(oldField, newField, maxCost);
                for (int i = Units.Count - 1; i >= 0; i--)
                {
                    troop = Units[i];
                    if (!troop.GetTransporter())
                    {
                        troop.MovedTo(oldField, newField, maxCost: 0);
                    }
                }
            }

            UpdateExploration();
        }
    }

    // Army.PAS:1565
    public int MaxSize()
    {
        int result = 8;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            int capacity = Units[i].GetTransportCapacity();
            if (capacity != 0 && result > capacity + 1)
            {
                result = capacity + 1;
            }
        }

        return result;
    }

    // Army.PAS:1576
    public Hero Hero(byte selection, HeroTypes types)
    {
        int i = 0;
        Hero result = null;
        while (i != Units.Count && result is null)
        {
            if ((selection & 1) != 0)
            {
                Troop troop = Units[i];
                if (troop is Hero hero && types.Contains(hero.HeroType.ToFlag()))
                {
                    result = hero;
                }
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1593
    public Leader Leader(byte selection)
    {
        int i = 0;
        Leader result = null;
        while (i != Units.Count && result is null)
        {
            if ((selection & 1) != 0)
            {
                // Class id 131634 is registered only for Leader. GetType matches that class exactly.
                // A pattern match on its own also matches a subclass. No subclass exists today.
                // A null unit fails at GetType.
                Troop unit = Units[i];
                if (unit.GetType() == typeof(Leader) && unit is Leader leader)
                {
                    result = leader;
                }
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1608
    public Troop Transporter(byte selection)
    {
        int i = 0;
        Troop result = null;
        while (i != Units.Count && result is null)
        {
            if ((selection & 1) != 0)
            {
                if (Units[i].GetTransporter())
                {
                    result = Units[i];
                }
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1623
    public int IndexOfTransporter(byte selection)
    {
        int result = Units.Count;
        while (result != 0)
        {
            result -= 1;
            if (((1 << result) & selection) != 0 && Units[result].GetTransporter())
            {
                return result;
            }
        }

        return -1;
    }

    // Army.PAS:1634
    public byte UnitsInTransporter()
    {
        byte result;
        if (Location.X != -1)
        {
            MapField field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            int i = 0;
            result = 0;
            while (i != Units.Count)
            {
                if (!Units[i].ValidTerrain(field.TerrainType, field.OverlayType))
                {
                    result = (byte)(result + (1 << i));
                }

                i += 1;
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Army.PAS:1652
    public void UpdateVisibilityRanges()
    {
        byte vision = 0;
        byte trueVision = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            byte range = (byte)Units[i].VisibilityRange();
            if (range > vision)
            {
                vision = range;
            }

            range = (byte)Units[i].TrueVisionRange();
            if (range > trueVision)
            {
                trueVision = range;
            }
        }

        _visibilityRanges = (byte)((trueVision << 4) + vision);
    }

    // Army.PAS:1667
    public byte VisibilityRange() => (byte)(_visibilityRanges & 0xF);

    // Army.PAS:1670
    public byte TrueVisionRange() => (byte)(_visibilityRanges >>> 4);

    // Army.PAS:1673
    public void TurnUnFogOff()
    {
        if (Status.Contains(ArmyStatuses.Activated))
        {
            byte range = _unfogRange;
            if (range != 0)
            {
                _ = StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, range);
                _unfogRange = 0;
            }
        }
    }

    // Army.PAS:1687
    public void UpdateExploration()
    {
        if (Status.Contains(ArmyStatuses.Activated))
        {
            for (int player = 1, last = StrategicMap.Current.Players.GetCount() - 1; player <= last; player++)
            {
                if (player == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.Alliance)
                {
                    byte visibilityRange = VisibilityRange();
                    _ = StrategicMap.Current.ExploreArea(Location.X, Location.Y, Location.MapLevel, player: (sbyte)player, range: visibilityRange);
                }
            }
        }
    }

    // Army.PAS:1697
    public void UpdateUnfog()
    {
        if (Status.Contains(ArmyStatuses.Activated))
        {
            int range = StrategicMap.Current.SeatedPlayer != -1 && (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(StrategicMap.Current.SeatedPlayer) == DiplomaticRelation.Alliance)
                ? VisibilityRange()
                : 0;

            if (range != _unfogRange)
            {
                if (_unfogRange != 0)
                {
                    _ = StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, range: _unfogRange);
                }

                _unfogRange = (byte)range;
                if (Location.X != -1)
                {
                    if (_unfogRange != 0)
                    {
                        _ = StrategicMap.Current.UnFogArea(Location.X, Location.Y, Location.MapLevel, range: _unfogRange);
                    }
                }
                else
                {
                    _unfogRange = 0;
                }
            }
        }
    }

    // Army.PAS:1739
    public AddUnitsResult CanAddUnits(UnitCollection units)
    {
        int transportCount = 0;
        int i = 0;
        int last = units.Count - 1;
        for (; i <= last; i += 1)
        {
            if (units.GetUnit(i).GetTransporter())
            {
                transportCount += 1;
            }
        }

        if (IndexOfTransporter(255) != -1)
        {
            transportCount += 1;
        }
        if (transportCount <= 1)
        {
            if (transportCount == 1)
            {
                Troop transport = Transporter(255);
                if (transport is null)
                {
                    i = 0;
                    int limit = units.Count - 1;
                    for (; i <= limit; i += 1)
                    {
                        if (units.GetUnit(i).GetTransporter())
                        {
                            transport = units.GetUnit(i);
                        }
                    }
                }

                return units.Count + Units.Count <= transport.GetTransportCapacity() + 1 ? AddUnitsResult.Ok : AddUnitsResult.TooManyUnits;
            }
            else
            {
                if (units.Count + Units.Count <= 8)
                {
                    i = 0;
                    // { Original repeats the list count test before testing each candidate. }
                    if (i != units.Count)
                    {
                        while (i != units.Count && CanAddUnit(units.GetUnit(i)))
                        {
                            i += 1;
                        }
                    }

                    return i != units.Count ? AddUnitsResult.InvalidMoveType : AddUnitsResult.Ok;
                }
                else
                {
                    return AddUnitsResult.TooManyUnits;
                }
            }
        }
        else
        {
            return AddUnitsResult.MultipleTransporters;
        }
    }

    // Army.PAS:1782
    public AddUnitsResult CanAddUnitSelection(UnitCollection units, byte selection)
    {
        DelphiList<Troop> list = new();
        int i = 0;
        int last = units.Count - 1;
        for (; i <= last; i += 1)
        {
            if ((selection & 1) != 0)
            {
                _ = list.Add(units.GetUnit(i));
            }

            selection = (byte)(selection >>> 1);
        }

        int transportCount = 0;
        i = 0;
        int limit = list.Count - 1;
        for (; i <= limit; i += 1)
        {
            if (list[i].GetTransporter())
            {
                transportCount += 1;
            }
        }

        if (IndexOfTransporter(255) != -1)
        {
            transportCount += 1;
        }

        AddUnitsResult result;
        if (transportCount <= 1)
        {
            if (transportCount == 1)
            {
                Troop transport = Transporter(255);
                if (transport is null)
                {
                    i = 0;
                    int last2 = list.Count - 1;
                    for (; i <= last2; i += 1)
                    {
                        if (list[i].GetTransporter())
                        {
                            transport = list[i];
                        }
                    }
                }

                int count = Units.Count;
                byte transportCapacity = transport.GetTransportCapacity();
                result = list.Count + count <= transportCapacity + 1 ? AddUnitsResult.Ok : AddUnitsResult.TooManyUnits;
            }
            else
            {
                int count = Units.Count;
                if (list.Count + count <= 8)
                {
                    i = 0;
                    while (i != list.Count && CanAddUnit(list[i]))
                    {
                        i += 1;
                    }

                    result = i != list.Count ? AddUnitsResult.InvalidMoveType : AddUnitsResult.Ok;
                }
                else
                {
                    result = AddUnitsResult.TooManyUnits;
                }
            }
        }
        else
        {
            result = AddUnitsResult.MultipleTransporters;
        }

        list?.Dispose();
        return result;
    }

    // Army.PAS:1818
    public AddUnitsResult AddUnitSelection(UnitCollection units, byte selection)
    {
        AddUnitsResult result = CanAddUnitSelection(units, selection);
        if (result == AddUnitsResult.Ok)
        {
            UnitUserList list;
            using (units.Holder.BeginUpdate())
            {
                list = new UnitUserList(null) { Holder = CreationHolder() };
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (((1 << i) & selection) != 0)
                    {
                        _ = !units.GetUnit(i).GetTransporter() ? list.AddUnit(units.GetUnit(i)) : AddUnit(units.GetUnit(i));
                    }
                }

                for (int i = list.GetCount() - 1; i >= 0; i--)
                {
                    _ = AddUnit(list.GetUnit(i));
                }
            }
            list?.Dispose();
        }

        return result;
    }

    // Army.PAS:1881
    public Troop SelectionUnit(byte selection, int index)
    {
        Troop result = null;
        int i = 0;
        while (i != Units.Count && result is null)
        {
            if ((selection & 1) != 0)
            {
                if (index == 0)
                {
                    result = Units[i];
                }

                index -= 1;
            }

            selection = (byte)(selection >>> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1919
    public void MakeIndependent(byte selection, ArmyHSList armies)
    {
        sbyte x2;
        sbyte y2;
        sbyte l2;
        ArmyHexSprite PlaceIndependentArmy(sbyte x, sbyte y, sbyte l, Army army)
        {
            ArmyHexSprite result;
            try
            {
                if (army.Units.Count != 0)
                {
                    result = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    _ = result.Army.BeginUpdate();
                    _ = result.Army.AddUnits(army.Units);
                    if (result.Place(x2, y2, l2, mode: ArmyPlaceModes.Safe | ArmyPlaceModes.ScanArea))
                    {
                        result.Activate();
                        result.Army.EndUpdate();
                    }
                    else
                    {
                        result.Army.Kill(255);
                        result?.Dispose();
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                // { Original suppresses placement failures without releasing a partly built army. }
                return null;
            }

            return result;
        }

        Army goodArmy;
        Army evilArmy;
        DelphiList<Army> races;
        DelphiList<Troop> units;
        using (BeginUpdate())
        {
            goodArmy = null;
            evilArmy = null;
            races = new DelphiList<Army>();
            units = new DelphiList<Troop>();
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    _ = units.Add(Units[i]);
                }

                selection = (byte)(selection >>> 1);
            }

            Troop troop;
            for (int i = units.Count - 1; i >= 0; i--)
            {
                troop = units.List[i];
                if (troop.GetRace() != -1 && !troop.GetAbilityEnabled(107))
                {
                    if (troop.GetRace() >= races.Count)
                    {
                        races.Count = troop.GetRace() + 1;
                    }

                    sbyte race = troop.GetRace();
                    if (races.List[race] is null)
                    {
                        Army army = new(null) { Holder = CreationHolder() };
                        sbyte aUnitRace = troop.GetRace();
                        races.List[aUnitRace] = army;
                    }

                    sbyte race2 = troop.GetRace();
                    _ = races.List[race2].AddUnit(troop);
                    units.Delete(i);
                }
            }

            for (int i = units.Count - 1; i >= 0; i--)
            {
                troop = units.List[i];
                Alignment alignment = troop.GetAlignment();
                int race3 = -1;
                for (int j = races.Count - 1; j >= 0; j--)
                {
                    if (races.List[j] is not null && GamePack.Current.RaceResources.GetRaceResource(j).Alignments.Contains(alignment.ToFlag()))
                    {
                        // { Original chooses the largest compatible race group. }
                        if (race3 == -1 || races.List[race3].Units.Count <= races.List[j].Units.Count)
                        {
                            race3 = j;
                        }
                    }
                }

                if (race3 != -1)
                {
                    _ = races.List[race3].AddUnit(troop);
                    units.Delete(i);
                }
                else
                {
                    if (AlignmentTables.GoodAlignments.Contains(alignment.ToFlag()) && (!AlignmentTables.EvilAlignments.Contains(alignment.ToFlag()) || (goodArmy is not null && evilArmy is null) || (goodArmy is null && evilArmy is null) || (goodArmy is not null && evilArmy is not null && goodArmy.Units.Count >= evilArmy.Units.Count)))
                    {
                        goodArmy ??= new Army(null) { Holder = CreationHolder() };

                        _ = goodArmy.AddUnit(troop);
                        units.Delete(i);
                    }
                    else
                    {
                        evilArmy ??= new Army(null) { Holder = CreationHolder() };

                        _ = evilArmy.AddUnit(troop);
                        units.Delete(i);
                    }
                }
            }

            x2 = Location.X;
            y2 = Location.Y;
            l2 = Location.MapLevel;
        }
        ArmyHexSprite armyHexSprite;
        for (int i = 0, last = races.Count - 1; i <= last; i++)
        {
            if (races.List[i] is not null)
            {
                armyHexSprite = PlaceIndependentArmy(x2, y2, l: l2, army: races.List[i]);
                if (armies is not null && armyHexSprite is not null)
                {
                    _ = armies.AddChild(armyHexSprite);
                }

                races.List[i]?.Dispose();
                races.List[i] = null;
            }
        }

        if (goodArmy is not null)
        {
            armyHexSprite = PlaceIndependentArmy(x2, y2, l: l2, army: goodArmy);
            if (armies is not null && armyHexSprite is not null)
            {
                _ = armies.AddChild(armyHexSprite);
            }

            goodArmy?.Dispose();
        }

        if (evilArmy is not null)
        {
            armyHexSprite = PlaceIndependentArmy(x2, y2, l: l2, army: evilArmy);
            if (armies is not null && armyHexSprite is not null)
            {
                _ = armies.AddChild(armyHexSprite);
            }

            evilArmy?.Dispose();
        }

        units?.Dispose();
        races?.Dispose();
    }

    // Army.PAS:2029
    public void UpdateIndependentRelation()
    {
        if (Player == 0)
        {
            sbyte race = -1;
            bool valid = true;
            int i = Units.Count;
            while (valid && i != 0)
            {
                i -= 1;
                int unitRace = !Units[i].GetAbilityEnabled(107) ? Units[i].GetRace() : -1;

                if (unitRace != -1 && unitRace != race)
                {
                    if (race == -1)
                    {
                        race = (sbyte)unitRace;
                    }
                    else
                    {
                        valid = false;
                    }
                }
            }

            if (valid && race == -1)
            {
                if (Location.X != -1)
                {
                    race = IndependentRelation;
                    IndependentRelation = -1;
                    IndependentRelation = (sbyte)StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).GetIndependentRelation();
                    if (IndependentRelation >= 0)
                    {
                        race = IndependentRelation;
                    }
                    else
                    {
                        IndependentRelation = race;
                        race = -1;
                    }
                }

                if (race == -1)
                {
                    if (IndependentRelation >= 0)
                    {
                        race = IndependentRelation;
                    }
                    else
                    {
                        valid = false;
                    }
                }
            }

            i = Units.Count;
            while (valid && i != 0)
            {
                i -= 1;
                bool condition = race == Units[i].GetRace();
                if (!condition)
                {
                    Alignment alignment = Units[i].GetAlignment();
                    condition = GamePack.Current.RaceResources.GetRaceResource(race).Alignments.Contains(alignment.ToFlag());
                }

                valid = condition;
            }

            if (!valid)
            {
                bool evil = true;
                bool good = true;
                bool neutral = true;
                i = 0;
                int last = Units.Count - 1;
                for (; i <= last; i += 1)
                {
                    Troop troop = Units[i];
                    Alignment alignmentAlignment = troop.GetAlignment();
                    evil = evil && AlignmentTables.EvilAlignments.Contains(alignmentAlignment.ToFlag()) && (troop.GetRace() == -1 || alignmentAlignment is Alignment.Evil or Alignment.PureEvil);
                    good = good && AlignmentTables.GoodAlignments.Contains(alignmentAlignment.ToFlag()) && (troop.GetRace() == -1 || alignmentAlignment is Alignment.PureGood or Alignment.Good);
                    neutral = neutral && AlignmentTables.NeutralAlignments.Contains(alignmentAlignment.ToFlag());
                }

                IndependentRelation = good && !evil ? (sbyte)-2 : evil && !good ? (sbyte)-4 : (sbyte)-3;
            }
            else
            {
                IndependentRelation = race;
            }
        }
        else
        {
            IndependentRelation = -1;
        }
    }

    // Army.PAS:2091
    public void Desert(byte selection)
    {
        StrategicMap.Current.LockGameOverEvent();
        DelphiList<Troop> units = new();
        int count = SelectionCount(selection);
        ArmyInfoEventLog log = new(null) { Holder = CreationHolder() };
        log.ArmyView.Setup(this, selection: 255);
        log.Selection = selection;
        log.Player = Player;
        log.Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertionText);
        log.Triggers = ExecuteTriggers.User;
        if (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvents.Rebellion))
        {
            log.Triggers |= ExecuteTriggers.Event;
            if (StrategicMap.Current.RequestCombatType)
            {
                log.Triggers |= ExecuteTriggers.NewTurn;
            }
        }

        if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvents.Rebellion))
        {
            log.Triggers |= ExecuteTriggers.Event | ExecuteTriggers.NewTurn;
        }

        log.Description = AbilityRules.TranslateResourceString(GetEngine(), S: s_leaderOfPartyStepsForwardText);
        log.Text = count == 1
            ? AbilityRules.TranslateResourceString(GetEngine(), S: s_oneUnitDesertedText)
            : AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsDesertedFormat), arguments: [count.ToString(CultureInfo.InvariantCulture)]);

        if (StrategicMap.Current.Players.GetPlayers(Player).Gems == 0 && StrategicMap.Current.Players.GetPlayers(Player).GetUpkeep() > StrategicMap.Current.Players.GetPlayers(Player).Income)
        {
            if (count == 1)
            {
                string desertUnitNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitNoUpkeepText);
                log.Messages.Text = desertUnitNoUpkeep;
            }
            else
            {
                string desertUnitsNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitsNoUpkeepText);
                log.Messages.Text = desertUnitsNoUpkeep;
            }
        }
        else
        {
            if (ValidateSelection(255) == selection)
            {
                if (count == 1)
                {
                    string desertUnitLowMorale = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitLowMoraleText);
                    log.Messages.Text = desertUnitLowMorale;
                }
                else
                {
                    string desertUnitsLowMorale = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitsLowMoraleText);
                    log.Messages.Text = desertUnitsLowMorale;
                }
            }
            else
            {
                if (count == 1)
                {
                    string desertUnitLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitLowMoraleSplitText);
                    log.Messages.Text = desertUnitLowMoraleSplit;
                }
                else
                {
                    string desertUnitsLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), S: s_desertUnitsLowMoraleSplitText);
                    log.Messages.Text = desertUnitsLowMoraleSplit;
                }
            }
        }

        EventLog.DistributeLocationEventLog(log, player: -1, Location.X, Location.Y, Location.MapLevel, execute: true);
        _ = log.Release();
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            int bit = 1 << i;
            if ((bit & selection) != 0 && Units.GetUnit(i).GetAbilityEnabled(155))
            {
                _ = units.Add(Units.GetUnit(i));
                selection = (byte)(selection & ~(byte)bit);
            }
        }

        ArmyHSList armies = new(null) { Holder = CreationHolder() };
        sbyte player = Player;
        MakeIndependent(selection, armies);
        while (units.Count != 0)
        {
            Troop troop = units[0];
            _ = troop.AddRef();
            troop.Killed();
            troop.SetOwner(null);
            _ = troop.Release();
            units.Delete(0);
        }

        for (int i = 0, last = armies.GetCount() - 1; i <= last; i++)
        {
            if (AbilityRules.DiplomaticRelation(0, armies[i].Army.IndependentRelation, otherPlayer: player, otherIndependentRelation: 0) == DiplomaticRelation.War)
            {
                armies[i].Army.SetBehavior(255, behavior: GroupBehavior.Raid);
            }
            else
            {
                armies[i].Army.SetBehavior(255, behavior: GroupBehavior.Scout);
            }
        }

        armies?.Dispose();
        units?.Dispose();
        StrategicMap.Current.UnlockGameOverEvent();
    }

    // Army.PAS:2155
    public PartyStatus GetPartyStatus(byte selection)
    {
        int morale = 100;
        int count = 0;
        CombatPredictor predictor = new(this);
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                Troop troop = Units[i];
                int unitMorale = troop.MoraleValue;
                if (troop.GetUnitType() != UnitType.Machine)
                {
                    if (unitMorale < morale)
                    {
                        morale = unitMorale;
                    }

                    count += 1;
                }

                if (unitMorale <= 40)
                {
                    predictor.AddOpponent(troop, behindWall: 0);
                }
                else
                {
                    predictor.AddUnit(troop);
                }
            }

            selection = (byte)(selection >>> 1);
        }

        PartyStatus result = predictor.Execute() == CombatResult.Defeat
            ? predictor.GetSuperiority() < 50 ? PartyStatus.Unruly : PartyStatus.Unrest
            : count == 0 || morale <= 60 ? PartyStatus.Stable : morale <= 80 ? PartyStatus.Content : PartyStatus.Cheerful;

        predictor?.Dispose();
        return result;
    }

    // Army.PAS:2185
    public void UpdateDesertion()
    {
        if (Location.X != -1 && Player != 0)
        {
            PartyStatus status = GetPartyStatus(255);
            if (status is PartyStatus.Unruly or PartyStatus.Unrest)
            {
                int chance = status == PartyStatus.Unrest ? 10 : 50;

                if (chance > StrategicMap.Current.Random(100))
                {
                    byte selection = 0;
                    for (int i = Units.Count - 1; i >= 0; i--)
                    {
                        selection = (byte)(selection << 1);
                        if (Units[i].MoraleValue <= 40)
                        {
                            selection = (byte)(selection | 1);
                        }
                    }

                    if (selection != 0)
                    {
                        Desert(selection);
                    }
                }
            }
        }
    }

    // Army.PAS:2208
    public bool ValidateCrusaders()
    {
        int i = Units.Count;
        bool result = false;
        while (i != 0 && !result)
        {
            i -= 1;
            if (Units[i].GetAbilityEnabled(101))
            {
                result = true;
            }
        }

        if (result)
        {
            using (BeginUpdate())
            {
                i = Units.Count - 1;
                for (; i >= 0; i -= 1)
                {
                    if (Units[i].GetAbilityEnabled(101))
                    {
                        Units[i].ValidateCrusader();
                    }
                }

                return Units.Count != 0;
            }
        }
        else
        {
            return true;
        }
    }

    // Army.PAS:2230
    public void NewTurn(sbyte player)
    {
        if (player == Player)
        {
            UserMode &= ~ArmyUserModes.TurnDone;
            if (ValidateCrusaders())
            {
                UpdateDesertion();
            }
        }
    }

    // Army.PAS:2239
    public void NewTurnDone(sbyte player)
    {
        if (player == Player)
        {
            if (StrategicMap.Current.Players.GetPlayers(Player).PlayerType != PlayerType.Human)
            {
                UpdateGroupBehavior();
            }
        }
    }

    // Army.PAS:2245
    public void NewDay()
    {
        if (StrategicMap.Current.Day == 1)
        {
            ResetDestinationPosition();
            if (Location.X != -1 && Player > 0)
            {
                byte visibilityRange = VisibilityRange();
                _ = StrategicMap.Current.ExploreArea(Location.X, Location.Y, Location.MapLevel, Player, range: visibilityRange);
                byte visibilityRange2 = VisibilityRange();
                byte trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(Player, Location.X, Location.Y, L: Location.MapLevel, sightRadius: visibilityRange2, radius: trueVisionRange);
            }

            InitializeGroupBehavior();
        }
    }

    // Army.PAS:2283
    public bool CanActivate() => true;

    // Army.PAS:2288
    public MoveAction GetMoveAction(sbyte player)
    {
        if (player != Player)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (!Status.Contains(ArmyStatuses.FullyConcealed))
                {
                    return Player == 0 && GetJoinSelection(player) != 0 ? MoveAction.Negotiate : MoveAction.Attack;
                }
            }
            else
            {
                return Player == 0 && GetJoinSelection(player) != 0 ? MoveAction.Negotiate : MoveAction.Attack;
            }
        }
        else
        {
            return MoveAction.Combine;
        }

        return MoveAction.Normal;
    }

    // Army.PAS:2306
    public byte GetCanJoinSelection(sbyte player)
    {
        byte result;
        if (player > 0)
        {
            result = 0;
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                result = (byte)(result << 1);
                Troop troop = Units[i];
                if (troop.CanJoin(player))
                {
                    result = (byte)(result | 1);
                }
            }

            if (IndexOfTransporter(255) != -1)
            {
                if (result != ValidateSelection(255))
                {
                    return 0;
                }
            }
        }
        else
        {
            return 0;
        }

        return result;
    }

    // Army.PAS:2324
    public byte GetJoinSelection(sbyte player)
    {
        if (player > 0)
        {
            bool wantsJoin = false;
            int i = Units.Count;
            while (!wantsJoin && i != 0)
            {
                i -= 1;
                wantsJoin = Units[i].OfferToJoin(player);
            }

            if (wantsJoin)
            {
                return GetCanJoinSelection(player);
            }
        }

        return 0;
    }

    // Army.PAS:2341
    public int JoinAmount(sbyte player, byte selection)
    {
        int result = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                result += Units[i].JoinAmount(player);
            }

            selection = (byte)(selection >>> 1);
        }

        return result;
    }

    // Army.PAS:2353
    public bool IncomingNegotiateRequest(NegotiateRequestEventLog log)
    {
        if (Player == 0)
        {
            log.SetupJoinArmy(this, log.Player, joinMode: JoinMode.Default);
            log.Object = NegotiateObject.Army;
            log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_partyJoinHeaderText);
            return log.Selection != 0;
        }

        return false;
    }

    // Army.PAS:2365
    public bool IncomingNegotiateOffer(NegotiateTokenEvent offer)
    {
        if (offer.Object == NegotiateObject.Army)
        {
            if (offer.Result == NegotiateTokenEvent.NegotiateResult.Accept)
            {
                byte selection = GetJoinSelection(offer.Player);
                int joinCount = SelectionCount(selection);
                if (offer.Mode.Contains(NegotiateModes.PrisonerJoin) || JoinAmount(offer.Player, selection) <= offer.Gold)
                {
                    using (BeginUpdate())
                    {
                        offer.Mode |= NegotiateModes.MoveOnAccept;
                        ArmyInfoEventLog log = new(null) { Holder = CreationHolder() };
                        log.ArmyView.Setup(this, selection);
                        log.Selection = 255;
                        log.Description = "";
                        log.Title = AbilityRules.TranslateResourceString(GetEngine(), S: s_partyJoinText);
                        log.Triggers = ExecuteTriggers.User;
                        Hero hero = Hero(selection, types: HeroTypes.Hero);
                        if (hero is null)
                        {
                            if (joinCount == 1)
                            {
                                log.Text = AbilityRules.TranslateResourceString(GetEngine(), S: s_oneUnitJoinedText);
                                string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_unitJoinedForcesFormat), arguments: [name]);
                                log.Messages.Text = format;
                            }
                            else
                            {
                                log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsJoinedFormat), arguments: [joinCount.ToString(CultureInfo.InvariantCulture)]);
                                int selectionCount = SelectionCount(selection);
                                string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_unitsJoinedForcesFormat), arguments: [selectionCount, name]);
                                log.Messages.Text = format;
                            }
                        }
                        else
                        {
                            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_joinedFormat), arguments: [hero.HeroName]);
                            string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_joinedForcesFormat), arguments: [hero.HeroName, name]);
                            log.Messages.Text = format;
                        }

                        EventLog.DistributeLocationEventLog(log, player: -1, Location.X, Location.Y, Location.MapLevel, execute: false);
                        _ = log.Release();
                        if (ValidateSelection(255) != selection)
                        {
                            MapCoordinateList locations = new();
                            if (offer.ArmyHexSprite is not null)
                            {
                                sbyte hexY = offer.ArmyHexSprite.HexY;
                                sbyte hexX = offer.ArmyHexSprite.HexX;
                                int direction = HexGeometry.DeltaHexToHexNumberFast(Location.X, Location.Y, hexX, hexY);
                                if (direction is > 0 and <= 6)
                                {
                                    _ = locations.AddMapCoordinate(Location.X, Location.Y, Location.MapLevel);
                                    int side = direction + 1;
                                    if (side > 6)
                                    {
                                        side = 1;
                                    }

                                    // { Original sign extends the wrapped byte coordinates here. }
                                    int x = default;
                                    int y = default;
                                    HexGeometry.CenterHexNumberToHex(offer.Location.X, offer.Location.Y, hexNumber: side, ref x, ref y);
                                    _ = locations.AddMapCoordinate((sbyte)x, (sbyte)y, Location.MapLevel);
                                    side = direction - 1;
                                    if (side == 0)
                                    {
                                        side = 6;
                                    }

                                    HexGeometry.CenterHexNumberToHex(offer.Location.X, offer.Location.Y, hexNumber: side, ref x, ref y);
                                    _ = locations.AddMapCoordinate((sbyte)x, (sbyte)y, Location.MapLevel);
                                }
                            }

                            int j = 0;
                            int i = Units.Count;
                            while (i != 0)
                            {
                                i -= 1;
                                if ((selection & (1 << i)) != 0)
                                {
                                    bool placed = false;
                                    while (!placed && j < locations.Count)
                                    {
                                        MapCoordinate item = locations[j];
                                        MapCoordinate locationsItem = locations[j];
                                        MapCoordinate item2 = locations[j];
                                        placed = Units.GetUnit(i).Place(item2.X, locationsItem.Y, L: item.MapLevel, offer.Player, mode: UnitPlaceModes.None) is not null;
                                        if (!placed)
                                        {
                                            j += 1;
                                        }
                                    }

                                    if (!placed)
                                    {
                                        _ = Units.GetUnit(i).Place(Location.X, Location.Y, L: Location.MapLevel, offer.Player, mode: UnitPlaceModes.ScanArea);
                                    }
                                }
                            }

                            locations?.Dispose();
                        }
                        else
                        {
                            SetPlayer(offer.Player);
                        }
                    }
                    offer.Accept();
                }
                else
                {
                    offer.Mode = NegotiateModes.AttackOnReject;
                    offer.Reject();
                }
            }
            else
            {
                if (offer.Result == NegotiateTokenEvent.NegotiateResult.Reject && !offer.Mode.Contains(NegotiateModes.PrisonerJoin))
                {
                    offer.Reject();
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Army.PAS:2468
    public void TriggerFireDamage()
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        ArmyEventLog log = new(null)
        {
            Holder = CreationHolder(),
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: s_fireDamageText),
            Player = GetArmyHexSprite().Army.Player
        };
        string fireDamage = AbilityRules.TranslateResourceString(GetEngine(), S: s_fireDamageText);
        _ = log.ArmyMessages.Add(fireDamage);
        log.ArmyIcon = 0;
        ArmyHexSprite armyHexSprite = GetArmyHexSprite();
        log.ArmyView.Setup(armyHexSprite.Army, selection: 255);
        using (BeginUpdate())
        {
            bool damaged = false;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                Troop troop = Units.GetUnit(i);
                if (!troop.GetImmunityTypes().Contains(DamageTypes.Fire))
                {
                    int damage = troop.ExecuteDamageRole(4, damage: 4, damageTypes: DamageTypes.Fire);
                    if (damage > troop.GetHitPoints())
                    {
                        damage = troop.GetHitPoints();
                    }

                    if (damage != 0)
                    {
                        damaged = true;
                        troop.SetHitPoints((sbyte)(troop.GetHitPoints() - (sbyte)damage));
                    }

                    _ = log.DamageValues.Add((byte)damage);
                    _ = log.DamageTypes.Add((int)DamageType.Fire);
                }
                else
                {
                    _ = log.DamageValues.Add(0);
                    _ = log.DamageTypes.Add((int)DamageType.NonSpecific);
                }
            }

            log.ValidateGameOver();
            if (damaged)
            {
                EventLog.DistributeLocationEventLog(log, player: -1, Location.X, Location.Y, Location.MapLevel, execute: true);
            }

            _ = log.Release();
            Validate();
        }
    }

    // Army.PAS:2508
    public void RetrieveStormDamageInfo(StormType storm, int distance, ref StormDamageInfo info)
    {
        byte relation = (byte)(int)AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, otherPlayer: Player, otherIndependentRelation: IndependentRelation);
        if (info.FriendlyRelations.Contains(((DiplomaticRelation)relation).ToFlag()) || info.EnemyRelations.Contains(((DiplomaticRelation)relation).ToFlag()))
        {
            int attack;
            int damage;
            switch (storm)
            {
                case StormType.Ice:
                    attack = 7;
                    damage = 5;
                    break;
                case StormType.Lightning:
                    attack = 8;
                    damage = 7;
                    break;
                case StormType.Death:
                    attack = 7;
                    damage = 4;
                    break;
                case StormType.Holy:
                    attack = 7;
                    damage = 5;
                    break;
                case StormType.Fire:
                    attack = 7;
                    damage = 5;
                    break;
                case StormType.Pestilence:
                    attack = 4;
                    damage = 3;
                    break;
                case StormType.None:
                default:
                    attack = 0;
                    damage = 0;
                    break;
            }

            for (int i = Units.Count - 1; i >= 0; i--)
            {
                Troop troop = Units.GetUnit(i);
                if (troop.Player != 0 || (troop.Group is not null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive))
                {
                    troop.RetrieveDamageInfo((sbyte)attack, damage: (sbyte)damage, damageTypes: StormDamageType[(int)storm].ToFlag(), damageValue: out int value, obtainValue: out int amount);
                    if (info.FriendlyRelations.Contains(((DiplomaticRelation)relation).ToFlag()))
                    {
                        info.FriendlyDamage += amount;
                        // { Original assigns the second return to damage. }
                        info.FriendlyValue += value;
                    }

                    if (info.EnemyRelations.Contains(((DiplomaticRelation)relation).ToFlag()))
                    {
                        info.EnemyDamage += amount;
                        info.EnemyValue += value;
                    }
                }
            }
        }
    }

    // Army.PAS:2538
    public void IncomingStorm(StormType storm, int distance, sbyte player)
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        ArmyEventLog log = new(null)
        {
            Holder = CreationHolder(),
            ArmyText = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.StormTypeTexts[(int)storm]),
            Player = player
        };
        // { Original appends the storm message. }
        string stormType = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.StormTypeTexts[(int)storm]);
        _ = log.ArmyMessages.Add(stormType);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(this, selection: 255);
        using (BeginUpdate())
        {
            bool damaged = false;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                int damage = Units[i].ExecuteStormDamage(storm, distance);
                if (damage != 0)
                {
                    damaged = true;
                }

                _ = log.DamageValues.Add((byte)damage);
                _ = log.DamageTypes.Add((byte)(int)StormDamageType[(int)storm]);
            }

            if (damaged)
            {
                log.ValidateGameOver();
                EventLog.DistributeLocationEventLog(log, Player, Location.X, Location.Y, Location.MapLevel, execute: true);
            }

            _ = log.Release();
            Validate();
        }
    }

    // Army.PAS:2567
    public void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        switch (info.Mode)
        {
            case AITargetMode.Attack:
            case AITargetMode.Raid:
            case AITargetMode.AttackArmies:
                if (!GetUnitsConcealedForPlayer(255, info.Player) && info.Player != Player)
                {
                    if (Player > 0)
                    {
                        data.Priority = AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: IndependentRelation) || info.Mode == AITargetMode.Raid
                            ? info.Player != 0 && Leader(255) is not null
                                ? 300
                                : info.Player != 0 && Hero(255, types: HeroTypes.Hero) is not null ? 100 : 50
                            : 0;
                    }
                    else
                    {
                        if (Player == 0)
                        {
                            byte selection = GetJoinSelection(info.Player);
                            if (selection != 0)
                            {
                                int joinAmount = JoinAmount(info.Player, selection);
                                data.Cost = joinAmount;
                                data.TargetType = AITargetType.Negotiate;
                            }
                            else
                            {
                                if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, targetPlayer: Player, targetRace: IndependentRelation))
                                {
                                    data.Priority = 50;
                                }
                            }
                        }
                    }
                }

                break;
            case AITargetMode.GuardArea:
                data.Priority = !GetUnitsConcealedForPlayer(255, info.Player) && info.Player != Player ? 50 : 0;

                break;
            case AITargetMode.Hunt:
                data.Priority = 10;
                break;
            case AITargetMode.Recruit:
                if (info.Player == Player && (Player != 0 || AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, otherPlayer: Player, otherIndependentRelation: IndependentRelation) != DiplomaticRelation.War))
                {
                    data.Priority |= 1;
                }

                break;
            case AITargetMode.Produce:
            case AITargetMode.Fortify:
            case AITargetMode.Defend:
            default:
                break;
        }
    }

    // Army.PAS:2605
    public int AIMovePriority()
    {
        int result = 0;
        if (Leader(255) is not null)
        {
            result += 50_000;
        }

        if (Hero(255, types: HeroTypes.Hero | HeroTypes.HeroLeader) is not null)
        {
            result += 10_000;
        }

        result += ObtainValue(255);
        return result;
    }

    // Army.PAS:2613
    public void ResetMoveDone()
    {
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            Troop troop = Units[i];
            troop.UnitStatus &= ~UnitStatuses.MoveDone;
        }
    }

    // Army.PAS:2623
    public bool CanCombine(Army army, byte selection)
    {
        selection = army.ValidateSelection(selection);
        int i = 0;
        byte temporary = selection;
        while (temporary != 0)
        {
            if ((temporary & 1) != 0)
            {
                i += 1;
            }

            temporary = (byte)(temporary >>> 1);
        }

        Troop ownTransporter = Transporter(255);
        int maxCount = ownTransporter is not null ? ownTransporter.GetTransportCapacity() + 1 : 8;

        i += Units.Count;
        if (i <= maxCount)
        {
            Troop otherTransporter = army.Transporter(selection);
            return otherTransporter is null || (i <= otherTransporter.GetTransportCapacity() + 1 && ownTransporter is null);
        }
        else
        {
            return false;
        }
    }

    // intf_P2_07.inc:445
    public virtual bool GetUnitConcealedForPlayer(int index, sbyte player)
    {
        if (player == Player || player == -1 || Location.X == -1)
        {
            return false;
        }

        MapField field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        return Units.GetUnit(index).ConcealedOnMapField(field) && GetConcealedForPlayer(player);
    }

    internal static ReadOnlySpan<DamageType> StormDamageType => [
        DamageType.NonSpecific,
        DamageType.Fire,
        DamageType.Death,
        DamageType.Holy,
        DamageType.Cold,
        DamageType.Lightning,
        DamageType.Poison
    ];

    private static readonly string s_unitsDrownText = "Units Drowned";
    private static readonly string s_unitsBurnedText = "Units Burned";
    private static readonly string s_unitsCrushedText = "Units Crushed";
    private static readonly string s_armyInherentText = "Inherent";
    private static readonly string s_desertionText = "Desertion";
    private static readonly string s_leaderOfPartyStepsForwardText = "The leader of the party steps forward";
    private static readonly string s_oneUnitDesertedText = "1 unit deserted";
    private static readonly string s_unitsDesertedFormat = "%s units deserted";
    private static readonly string s_desertUnitNoUpkeepText = "This one abandons you, refusing a voucher for pay.";
    private static readonly string s_desertUnitsNoUpkeepText = "Without sufficient funds for support, these troops desert you.";
    private static readonly string s_desertUnitLowMoraleText = "With no common cause, this one deserts you.";
    private static readonly string s_desertUnitsLowMoraleText = "No longer sharing your cause, these desert you.";
    private static readonly string s_desertUnitLowMoraleSplitText = "Lacking faith, this one abandons your ranks";
    private static readonly string s_desertUnitsLowMoraleSplitText = "Troubled and weary, these abandon your ranks.";
    private static readonly string s_partyJoinHeaderText = "The leader of the party steps forward:";
    private static readonly string s_partyJoinText = "Party Join";
    private static readonly string s_oneUnitJoinedText = "1 unit joined";
    private static readonly string s_unitJoinedForcesFormat = "A unit joined the forces of %s";
    private static readonly string s_unitsJoinedFormat = "%s units joined";
    private static readonly string s_unitsJoinedForcesFormat = "%d units joined the forces of %s";
    private static readonly string s_joinedFormat = "%s joined";
    private static readonly string s_joinedForcesFormat = "%0:s joined the forces of %1:s";
    private static readonly string s_fireDamageText = "Fire Damage";

    public enum PartyStatus : byte
    {
        Unruly = 0, Unrest = 1, Stable = 2, Content = 3, Cheerful = 4
    }
}
