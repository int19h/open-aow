#nullable disable
// Translated from AoWE.pas.
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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// Status. Mode. Player. location. IndependentRelation. UnfogRange. Upkeep. OnUpdate.
// OldAIBehavior. destination. UserMode. VisibilityRanges. ConcealedUnits
// The original's TArmy derives from TUnitList. The port's army owns its troops as its node
// children (it routes messages to them, notifies their changes, frees them and stores them) and
// reads them through Units. The members below that carry a Unit.PAS line are TUnitList's part.
public partial class Army : ObservableNode, IChildList<Troop>, IUnitHolder
{
    public new const int ClassId = 131606;

    /// <summary>The troops of this army, in their stored order.</summary>
    public UnitCollection Units => new(this);

    // An army takes troops only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop;

    Troop IOwner<Troop>.this[int index] => Units[index];

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    // Army.PAS:324
    public Army(CustomNode owner) : base(owner)
    {
        Location = new MapCoordinate(-1, -1, -1);
        Destination = new MapCoordinate(-1, -1, -1);
        IndependentRelation = -1;
    }

    public Set<ArmyStatusElement> Status { get; set; }

    public Set<ArmyModeElement> Mode { get; set; }
    public sbyte Player { get; set; }
    public MapCoordinate Location { get; set; }

    public sbyte IndependentRelation { get; private set; }

    private byte _unfogRange;
    private byte _upkeep;

    public event NotifyEvent<object> OnUpdate;

    internal void SetUpdateHandler(NotifyEvent<object> handler) => OnUpdate = handler;

    internal void ClearUpdateHandler() => OnUpdate = null;

    public GroupBehavior OldAIBehavior { get; set; }
    public MapCoordinate Destination { get; set; }
    public Set<ArmyUserModeElement> UserMode { get; set; }

    private byte _visibilityRanges;

    public byte ConcealedUnits { get; private set; }

    // Army.PAS:336
    public override void Dispose()
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            StrategicMap.Current.NotifyArmyDestroyed(this);
        }

        OnUpdate = null;
        base.Dispose();
    }

    // Army.PAS:1898
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Byte);
        if (persister.Loading || StrategicMap.Current == null || StrategicMap.Current.ConnectionType != (PlayerConnectionType)1)
        {
            Destination = PersistSupport.Coordinate(persister, name: "Destination", value: Destination, width: CoordinateWidth.Byte);
            UserMode = persister.Set("UserMode", value: UserMode);
        }

        OldAIBehavior = persister.Enum("OldAIBehavior", value: OldAIBehavior);
        Mode = persister.Set("Mode", value: Mode);
        // The translate stream's mode holds read, so the original clears the flag there too.
        if ((persister.Loading || PersistSupport.Translating(persister)) && Mode.Contains(ArmyModeElement.Moving))
        {
            Mode = Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
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
                                ValidateCrusaders();
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
                Units[i].Release();
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
            unit.AddRef();

            result = RemoveUnitChild(child);
            EngineObject unit2 = child;
            unit2.Release();

        }
        return result;
    }

    // Unit.PAS:2162, the unit list's part of RemoveChild.
    private int RemoveUnitChild(EngineObject child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            child.Release();
        }

        return result;
    }

    // Army.PAS:1853
    public override int AddChild(EngineObject child)
    {
        int result;
        // The check refuses a child that is not a troop before the troop is read.
        CheckChild(child);
        Troop troop = (Troop)child;
        if (!Status.Contains(ArmyStatusElement.Activated) || troop.CanActivate())
        {
            result = AddUnitChild(troop);
            if (result != -1)
            {
                if (Status.Contains(ArmyStatusElement.Activated) && !troop.UnitStatus.Contains(UnitStatusElement.Activated))
                {
                    troop.Activate();
                }

                troop.SetPlayer(Player);
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Unit.PAS:2167, the unit list's part of AddChild.
    private int AddUnitChild(Troop troop)
    {
        if (CanAddUnit(troop))
        {
            troop.AddRef();
            return base.AddChild(troop);
        }

        return -1;
    }

    // Army.PAS:716
    protected override void OnChange() => Update();

    // Army.PAS:1241
    public sbyte GetPlayer() => Player;

    // Army.PAS:1874
    public virtual MapCoordinate GetLocation() => Status.Contains(ArmyStatusElement.Activated) ? Location : MapCoordinate.None;

    // Army.PAS:1719
    public virtual bool CanAddUnit(Troop troop)
    {
        bool result;
        bool hasTransporter;
        MapField field;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            result = Units.Count < MaxSize() && troop.CanAddToList(Units);
            hasTransporter = IndexOfTransporter(255) != -1;
            if (result && !hasTransporter && Location.X != -1 && Location.X != 0 && !Mode.Contains(ArmyModeElement.DisableTerrainValidation))
            {
                // { Original skips terrain validation on the zero X column. }
                field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
                result = troop.ValidTerrain(field.TerrainType, field.OverlayType);
            }

            if (result && troop.GetTransporter())
            {
                result = !hasTransporter && troop.GetTransportCapacity() >= Units.Count;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:1836
    public virtual bool CanDeleteUnit(Troop troop)
    {
        bool result = IndexOfTransporter(255) == -1 || !troop.GetTransporter() || UnitsInTransporter() == 0;
        return result;
    }

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
                    AddUnit(units.GetUnit(i));
                }

            }
        }

        return result;
    }

    // Unit.PAS:2208
    public bool DeleteUnit(Troop troop)
    {
        bool result;
        if (CanDeleteUnit(troop))
        {
            result = true;
            troop.SetOwner(null);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Army.PAS:1269
    public virtual bool GetConcealedForPlayer(sbyte player)
    {
        bool result;
        MapLevel level;
        MapField field;
        MapFieldMessage message = default;
        HexNumberWalkData walk = default;
        if (player != -1)
        {
            result = true;
            level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
            message.MessageId = 4363;
            message.Result = 0;
            MapFieldMessageParameters param = message.Param;
            param[1] = player;
            message.Param = param;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, Location.X, Location.Y, 0);
            while (result && walk.Radius < 8)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    field = level.GetField(walk.X, walk.Y);
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
            result = false;
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
                if ((((int)(((uint)selection) >> i)) & 1) != 0)
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
    public int GetStrengthMax(byte selection, Set<DamageType> damageTypes)
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
        int strength;
        Troop troop;
        DiplomaticRelation relation;
        GroupBehavior behavior;
        bool includeUnit;
        relation = AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, Player, IndependentRelation);
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            troop = Units[i];
            behavior = troop.Group != null ? troop.Group.GroupClass.Behavior : GroupBehavior.Passive;

            // A default collection has no holder and counts every troop, as a missing list did.
            includeUnit = info.Units.Holder is null || info.Units.IndexOf(troop) == -1;
            if (includeUnit && info.Requests.Contains(BattleFieldInfoRequestsElement.IgnorePassiveIndependents))
            {
                includeUnit = troop.Player != 0 || (troop.Group != null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive && troop.Group.GroupClass.Behavior != GroupBehavior.Guard);
            }

            if (includeUnit && info.Requests.Contains(BattleFieldInfoRequestsElement.IgnoreIndependents) && troop.Player == 0)
            {
                includeUnit = false;
            }

            if (includeUnit)
            {
                strength = troop.GetStrengthMax(info.DamageTypes);
                if (info.FriendlyRelations.Contains((byte)(int)relation))
                {
                    info.FriendlyStrength += strength;
                }

                if (info.EnemyRelations.Contains((byte)(int)relation) || behavior == GroupBehavior.GuardArea)
                {
                    info.Strength += strength;
                }
            }
        }
    }

    // Army.PAS:394
    public bool HealUnits(byte selection)
    {
        Troop troop;
        bool result = false;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                troop = Units[i];
                if (troop.GetHitPoints() != troop.GetHits())
                {
                    result = true;
                    troop.SetHitPoints(troop.GetHits());
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // Army.PAS:410
    public int CreateGroupId(byte selection)
    {
        int result;
        int i;
        i = 0;
        while ((selection & 1) == 0 && i != Units.Count)
        {
            i += 1;
            selection = (byte)(int)(((uint)selection) >> 1);
        }

        result = i != Units.Count ? Units[i].UnitId : 0;

        return result;
    }

    // Army.PAS:423
    public AIGroup GetGroup(byte selection)
    {
        bool different;
        int i;
        bool found;
        AIGroup result = null;
        different = false;
        i = 0;
        found = false;
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

            selection = (byte)(int)(((uint)selection) >> 1);
            i += 1;
        }

        if (!found || different)
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:445
    public void SetGroup(byte selection, AIGroup group)
    {
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                group.AddUnit(Units[i]);
            }

            selection = (byte)(int)(((uint)selection) >> 1);
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
        GroupBehavior result;
        AIGroup group;
        group = GetGroup(selection);
        result = group != null ? group.GroupClass.Behavior : GroupBehavior.None;

        return result;
    }

    // Army.PAS:473
    public void SetBehavior(byte selection, GroupBehavior behavior)
    {
        AIGroup group;
        group = StrategicMap.Current.AIGroupManager.CreateGroup(Player, IndependentRelation, behavior);
        if (group != null)
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    group.AddUnit(Units[i]);
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }

            group.Release();
        }
    }

    // Army.PAS:488
    public GroupBehavior GetOldAIBehavior()
    {
        int i;
        GroupBehavior result = GroupBehavior.None;
        i = Units.Count;
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
        byte selection;
        int index;
        GroupBehavior behavior;
        Hero hero;
        selection = 255;
        hero = Hero(selection, Set<HeroType>.Of(HeroType.Leader, HeroType.HeroLeader));
        if (hero != null)
        {
            index = Units.IndexOf(hero);
            if (index != -1)
            {
                SetBehavior((byte)(1 << index), GroupBehavior.Fortify);
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
            behavior = OldAIBehavior;
            if (behavior == GroupBehavior.None)
            {
                behavior = Player == 0 && StrategicMap.Current.GetIndependentAIMode() != StrategicMapIndependentAIMode.Off
                    ? Location.X != -1 && StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) == null
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
        byte selection;
        int index;
        GroupBehavior behavior;
        selection = GetGroupSelection(0);
        if (selection != 0)
        {
            index = IndexOfTransporter(selection);
            if (index != -1)
            {
                selection = (byte)(selection & ~(1 << index));
                if (Units[index].GroupId == 0)
                {
                    SetBehavior((byte)(1 << index), GroupBehavior.Transport);
                }
            }

            behavior = GroupBehavior.Passive;
            if (Player == 0 && Hero(selection, Set<HeroType>.Of(HeroType.Hero)) != null)
            {
                behavior = GroupBehavior.Scout;
            }
            else
            {
                if (Player == 0 && StrategicMap.Current.GetIndependentAIMode() != StrategicMapIndependentAIMode.Off && Location.X != -1)
                {
                    if (StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) == null)
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
        ArmyId id;
        id = new ArmyId(null) { Holder = CreationHolder() };
        id.Location = new MapCoordinate(Location.X, Location.Y, Location.MapLevel);
        id.Player = Player;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                id.UnitIds.Add(Units[i].UnitId);
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return id;
    }

    // Army.PAS:579
    public ArmyHexSprite GetArmyHexSprite()
    {
        ArmyHexSprite result;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        MapCoordinate coordinate26 = GetLocation();
        x = coordinate26.X;
        y = coordinate26.Y;
        l = coordinate26.MapLevel;
        result = x != -1 ? StrategicMap.Current.MapContainer.GetField(x, y, l).GetArmyHexSprite() : null;

        return result;
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
        if (value != Status.Contains(ArmyStatusElement.FullyConcealed))
        {
            Status = value
                ? Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed)
                : Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed);

            if (Status.Contains(ArmyStatusElement.Activated) && Location.X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, 0);
            }
        }
    }

    // Army.PAS:610
    public bool GetFullyConcealed() => Status.Contains(ArmyStatusElement.FullyConcealed);

    // Army.PAS:613
    public void UpdateConcealment()
    {
        bool concealed;
        Leader leader;
        bool changed;
        bool showCrown;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            concealed = ConcealedUnits != 0 && StrategicMap.Current.SeatedPlayer != Player && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && GetConcealedForPlayer(StrategicMap.Current.SeatedPlayer);

            leader = Leader(255);
            showCrown = leader != null;
            if (showCrown && concealed)
            {
                int indexOf = Units.IndexOf(leader);
                if ((((int)(((uint)ConcealedUnits) >> indexOf)) & 1) != 0)
                {
                    showCrown = false;
                }
            }

            changed = false;
            Status = showCrown
                ? Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown)
                : Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown);

            if (concealed != Status.Contains(ArmyStatusElement.Concealed))
            {
                Status = concealed
                    ? Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed)
                    : Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed);

                changed = true;
            }

            concealed = concealed && Units.Count == ConcealedUnitsCount();
            if (concealed != GetFullyConcealed())
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

    public void SetMapCoordinate(sbyte x, sbyte y, sbyte level) =>
        SetMapCoordinate(new MapCoordinate(x, y, level));

    // Army.PAS:647
    public int GetRace(byte selection)
    {
        bool same;
        int race;
        int result = -1;
        same = true;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                race = Units[i].GetRace();
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

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        if (!same)
        {
            result = -1;
        }

        return result;
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
        byte selection;
        selection = ConcealedUnits;
        int result = 0;
        while (selection != 0)
        {
            if ((selection & 1) != 0)
            {
                result += 1;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // Army.PAS:687
    public void UpdateConcealedUnits()
    {
        int i;
        MapField field;
        if (Location.X != -1 && Location.MapLevel != -1 && Status.Contains(ArmyStatusElement.Activated))
        {
            ConcealedUnits = 0;
            i = Units.Count;
            field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
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
        byte upkeep;
        byte previous;
        int delta;
        upkeep = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            upkeep = (byte)(upkeep + Units[i].GetUpkeep());
        }

        previous = _upkeep;
        if (previous != upkeep && Status.Contains(ArmyStatusElement.Activated))
        {
            delta = upkeep - previous;
            Player players = StrategicMap.Current.Players.GetPlayers(Player);
            StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep + delta);
        }

        _upkeep = upkeep;
    }

    // Army.PAS:719
    public void ValidateTerrain(sbyte terrainType, sbyte overlayType, sbyte player)
    {
        Troop transport;
        byte selection;
        ArmyEventLog log;
        Troop troop;
        transport = Transporter(255);
        if (transport == null || !transport.ValidTerrain(terrainType, overlayType))
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
            if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && StrategicMap.Current.Day != 0)
            {
                using (BeginUpdate())
                {
                    log = new ArmyEventLog(null) { Holder = CreationHolder() };
                    log.Player = player;
                    if (Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)terrainType))
                    {
                        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsDrownText);
                        string unitsDrown = AbilityRules.TranslateResourceString(GetEngine(), s_unitsDrownText);
                        log.ArmyMessages.Add(unitsDrown);
                        log.ArmyIcon = 3;
                    }
                    else
                    {
                        switch (terrainType)
                        {
                            case 9:
                                log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsBurnedText);
                                string unitsBurned = AbilityRules.TranslateResourceString(GetEngine(), s_unitsBurnedText);
                                log.ArmyMessages.Add(unitsBurned);
                                log.ArmyIcon = 0;
                                break;
                            default:
                                log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsCrushedText);
                                string translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsCrushedText);
                                log.ArmyMessages.Add(translatedText);
                                log.ArmyIcon = 2;
                                break;
                        }
                    }

                    log.ArmyView.Setup(this, 255);
                    for (int i = 0, last = Units.Count - 1; i <= last; i++)
                    {
                        troop = Units.GetUnit(i);
                        if ((((int)(((uint)selection) >> i)) & 1) != 0)
                        {
                            sbyte hitPoints = troop.GetHitPoints();
                            log.DamageValues.Add((byte)hitPoints);
                            log.DamageTypes.Add(9);
                            troop.SetHitPoints(0);
                        }
                        else
                        {
                            log.DamageValues.Add(0);
                            log.DamageTypes.Add(9);
                        }
                    }

                    log.ValidateGameOver();
                    EventLog.DistributeLocationEventLog(log, Player, Location.X, Location.Y, Location.MapLevel, true);
                    log.Release();
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
        Troop troop;
        Unit newUnit;
        StrategicMap.Current.LockGameOverEvent();
        using (BeginUpdate())
        {
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                troop = Units[i];
                if (troop.GetHitPoints() == 0)
                {
                    if (troop.GetAbilityEnabled(107))
                    {
                        newUnit = new Unit(null) { Holder = CreationHolder() };
                        newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(152));
                        troop.Killed();
                        AddUnit(newUnit);
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
        int level;
        int unitLevel;
        Troop troop;
        string sourceName = "";
        level = 0;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            troop = Units[i];
            troop.UpdateMoraleValue();
            if (troop.GetAbilityEnabled(46) || troop.GetAbilityLevel(46) > 0)
            {
                ((LeadershipAbility)GamePack.Current.AbilityControl.GetAbility(46)).ResetExternalSource(troop);
                unitLevel = troop.GetAbilityLevel(46);
                if (unitLevel > level)
                {
                    level = unitLevel;
                    sourceName = troop.GetAbilityOwner(46).GetOwnerName();
                    if (sourceName == AbilityRules.TranslateResourceString(GetEngine(), s_armyInherentText))
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
                    ((LeadershipAbility)GamePack.Current.AbilityControl.GetAbility(46)).SetExternalSource(troop, (byte)level, sourceName);
                }
            }
        }
    }

    // Army.PAS:841
    public void Update()
    {
        if (UpdateCount == 0 && Status.Contains(ArmyStatusElement.Activated))
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
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && Location.X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, 0);
            }

            UnlockChanged();
            TriggerOnUpdateEvent();
        }
    }

    // Army.PAS:861
    public void TriggerOnUpdateEvent()
    {
        if (UpdateCount == 0 && Status.Contains(ArmyStatusElement.Activated))
        {
            StrategicMap.Current.NotifyArmyChanged(this);
            OnUpdate?.Invoke(this);
        }
    }

    // Army.PAS:870
    public void Activate()
    {
        Troop troop;
        Assert(!Status.Contains(ArmyStatusElement.Activated), "Army already activated", "Army.PAS", 1171);
        if (OldAIBehavior == GroupBehavior.None)
        {
            OldAIBehavior = GetOldAIBehavior();
        }

        for (int i = Units.Count - 1; i >= 0; i--)
        {
            troop = Units[i];
            if (!troop.UnitStatus.Contains(UnitStatusElement.Activated))
            {
                troop.Activate();
            }
        }

        Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        _upkeep = 0;
        if (Player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(Player).Armies.AddChild(this);
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
        Troop troop;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            if (_unfogRange != 0)
            {
                StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, _unfogRange);
                _unfogRange = 0;
            }

            for (int i = Units.Count - 1; i >= 0; i--)
            {
                troop = Units[i];
                if (troop.UnitStatus.Contains(UnitStatusElement.Activated))
                {
                    troop.Deactivate();
                }
            }

            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                Player players = StrategicMap.Current.Players.GetPlayers(Player);
                StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
            }

            Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
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

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // Army.PAS:1227
    public byte ValidateMoveSelection(byte selection)
    {
        byte result = ValidateSelection(selection);
        if (IndexOfTransporter(255) != -1 && (UnitsInTransporter() & ~selection) != 0 && Transporter(selection) != null)
        {
            Troop transporter = Transporter(255);
            result = (byte)(result & ~(1 << IndexOfChild(transporter)));
        }

        return result;
    }

    // Army.PAS:1235
    public void Select() => Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);

    // Army.PAS:1238
    public void Unselect() => Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);

    // Army.PAS:1244
    public bool GetUnitConcealed(int index)
    {
        MapField field;
        field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
        return Units[index].ConcealedOnMapField(field);
    }

    // Army.PAS:1262
    public bool GetUnitsConcealedForPlayer(byte selection, sbyte player)
    {
        bool result;
        selection = ValidateSelection(selection);
        result = selection == (ConcealedUnits & selection) && GetConcealedForPlayer(player);

        return result;
    }

    // Army.PAS:1298
    public void SetPlayer(sbyte player)
    {
        if (StrategicMap.Current.Players.GetPlayers(player) == null)
        {
            player = 0;
        }

        if (player != Player)
        {
            using (BeginUpdate())
            {
                if (Status.Contains(ArmyStatusElement.Activated))
                {
                    if (Player != -1)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Player);
                        StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
                        StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                    }

                    Player = player;
                    if (Player != -1)
                    {
                        Player players = StrategicMap.Current.Players.GetPlayers(Player);
                        StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep + _upkeep);
                        StrategicMap.Current.Players.GetPlayers(Player).Armies.AddChild(this);
                    }

                    for (int i = Units.Count - 1; i >= 0; i--)
                    {
                        Units[i].SetPlayer(Player);
                    }

                    if (Location.X != -1)
                    {
                        StrategicMap.Current.ScannerIconsChangedRadius(Location.X, Location.Y, Location.MapLevel, 0);
                    }

                    if (Player > 0 && StrategicMap.Current.Day > 0)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                        {
                            SetBehavior(255, GroupBehavior.Passive);
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
    public Set<DamageType> ImmunityTypes(byte selection)
    {
        int i;
        Set<DamageType> result = AbilityRules.AllDamageTypes;
        i = 0;
        while (result != Set<DamageType>.Of() && i != Units.Count)
        {
            if ((selection & 1) != 0)
            {
                result = result & Units[i].GetImmunityTypes();
            }

            selection = (byte)(int)(((uint)selection) >> 1);
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

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // Army.PAS:1357
    public sbyte MovePoints(byte selection)
    {
        sbyte result;
        Troop transport;
        Troop troop;
        transport = Transporter(selection);
        if (transport != null)
        {
            result = transport.GetMovePoints();
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
                        troop = Units[i];
                        if (troop.GetMovePoints() < result)
                        {
                            result = troop.GetMovePoints();
                        }
                    }

                    selection = (byte)(int)(((uint)selection) >> 1);
                }
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    // Army.PAS:1378
    public sbyte Moves(byte selection)
    {
        sbyte result;
        Troop transport;
        Troop troop;
        transport = Transporter(selection);
        if (transport != null)
        {
            result = transport.GetMoves();
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
                        troop = Units[i];
                        if (troop.GetMoves() < result)
                        {
                            result = troop.GetMoves();
                        }
                    }

                    selection = (byte)(int)(((uint)selection) >> 1);
                }
            }
            else
            {
                result = 0;
            }
        }

        return result;
    }

    // Army.PAS:1399
    public Set<MoveType> MoveTypes(byte selection)
    {
        Set<MoveType> result;
        Troop transport;
        transport = Transporter(selection);
        if (transport != null)
        {
            result = transport.GetMoveTypes();
        }
        else
        {
            result = Set<MoveType>.Of();
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    result = result + Units[i].GetMoveTypes();
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }
        }

        return result;
    }

    // Army.PAS:1416
    public Set<MoveType> GetPartyMoves(byte selection)
    {
        Set<MoveType> result = default;
        Troop transport;
        Set<MoveType> common;
        Set<MoveType> moves;
        int moveType;
        transport = Transporter(selection);
        if (transport == null)
        {
            common = Set<MoveType>.Of().WithRange(MoveType.Walking, MoveType.Tunneling);
            moveType = (int)MoveType.Flying;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if (((1 << i) & selection) != 0)
                {
                    moves = Units[i].GetMoveTypes();
                    common = common & moves;
                    if (moveType != -1)
                    {
                        if ((MoveType)moveType == MoveType.Flying)
                        {
                            if (!moves.Contains(MoveType.Flying))
                            {
                                moveType = moves.Contains(MoveType.Swimming) ? (int)MoveType.Swimming : (int)MoveType.Walking;
                            }
                        }
                        else
                        {
                            if ((MoveType)moveType == MoveType.Swimming)
                            {
                                if ((Set<MoveType>.Of(MoveType.Flying, MoveType.Swimming) & moves) == Set<MoveType>.Of())
                                {
                                    moveType = (int)MoveType.Walking;
                                }
                            }
                            else
                            {
                                if ((MoveType)moveType == MoveType.Walking)
                                {
                                    if ((Set<MoveType>.Of(MoveType.Walking, MoveType.Flying).WithRange(MoveType.Forestry, MoveType.LavaCrossing) & moves) == Set<MoveType>.Of())
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
                result = Set<MoveType>.Of((MoveType)moveType);
            }

            result = result + common;
        }
        else
        {
            result = transport.GetMoveTypes();
        }

        return result;
    }

    // Army.PAS:1450
    public int MoveOnMovePoints(byte selection, sbyte terrainType, sbyte overlayType)
    {
        MovePointTable table = default;
        MovePointTable unitTable = default;
        Troop transport;
        table.Fill(1);
        transport = Transporter(selection);
        if (transport != null)
        {
            transport.CreateMovePointTable(out table);
        }
        else
        {
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if ((selection & 1) != 0)
                {
                    Units[i].CreateMovePointTable(out unitTable);
                    table.Maximize(unitTable);
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }
        }

        return table[terrainType][overlayType - MovementTables.FirstOverlay];
    }

    // Army.PAS:1469
    public void CreateMovePointTable(byte selection, out MovePointTable table)
    {
        int count;
        MovePointTable unitTable = default;
        Troop transport;
        count = 0;
        table = default;
        table.Fill(1);
        transport = Transporter(selection);
        if (transport != null)
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
                    Units[i].CreateMovePointTable(out unitTable);
                    table.Maximize(unitTable);
                    count += 1;
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }
        }

        if (count == 0)
        {
            table.Fill(-1);
        }
    }

    // Army.PAS:1494
    public bool ValidTerrain(byte selection, sbyte terrainType, sbyte overlayType)
    {
        bool result;
        int i;
        Troop transport;
        transport = Transporter(selection);
        if (transport == null)
        {
            i = 0;
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
            result = transport.ValidTerrain(terrainType, overlayType);
        }

        return result;
    }

    // Army.PAS:1512
    public bool ValidTerrain(sbyte terrainType, sbyte overlayType)
    {
        bool result;
        int i;
        Troop transport;
        transport = Transporter(255);
        if (transport == null)
        {
            i = 0;
            result = true;
            while (i != Units.Count && result)
            {
                result = Units[i].ValidTerrain(terrainType, overlayType);
                i += 1;
            }
        }
        else
        {
            result = transport.ValidTerrain(terrainType, overlayType);
        }

        return result;
    }

    // Army.PAS:1529
    public bool CanPlace(sbyte X, sbyte Y, sbyte L)
    {
        MapField field;
        int i;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        bool result = ValidTerrain(field.TerrainType, field.OverlayType);
        i = 0;
        if (!Status.Contains(ArmyStatusElement.Activated))
        {
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
        Troop troop;
        using (BeginUpdate())
        {
            UserMode = UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
            troop = Transporter(255);
            if (troop == null)
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
                        troop.MovedTo(oldField, newField, 0);
                    }
                }
            }

            UpdateExploration();
        }
    }

    // Army.PAS:1565
    public int MaxSize()
    {
        int capacity;
        int result = 8;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            capacity = Units[i].GetTransportCapacity();
            if (capacity != 0 && result > capacity + 1)
            {
                result = capacity + 1;
            }
        }

        return result;
    }

    // Army.PAS:1576
    public Hero Hero(byte selection, Set<HeroType> types)
    {
        int i;
        Troop troop;
        i = 0;
        Hero result = null;
        while (i != Units.Count && result == null)
        {
            if ((selection & 1) != 0)
            {
                troop = Units[i];
                if (troop is Hero hero && types.Contains(hero.GetHeroType()))
                {
                    result = hero;
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1593
    public Leader Leader(byte selection)
    {
        int i;
        i = 0;
        Leader result = null;
        while (i != Units.Count && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (Units[i].EngineClass.ClassId == 131634)
                {
                    result = (Leader)Units[i];
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1608
    public Troop Transporter(byte selection)
    {
        int i;
        i = 0;
        Troop result = null;
        while (i != Units.Count && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (Units[i].GetTransporter())
                {
                    result = Units[i];
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
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
        int i;
        MapField field;
        if (Location.X != -1)
        {
            field = StrategicMap.Current.MapContainer.GetField(Location.X, Location.Y, Location.MapLevel);
            i = 0;
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
            result = 0;
        }

        return result;
    }

    // Army.PAS:1652
    public void UpdateVisibilityRanges()
    {
        byte trueVision;
        byte vision;
        byte range;
        vision = 0;
        trueVision = 0;
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            range = (byte)Units[i].VisibilityRange();
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
    public byte VisibilityRange() => (byte)(_visibilityRanges & 15);

    // Army.PAS:1670
    public byte TrueVisionRange() => (byte)(int)(((uint)_visibilityRanges) >> 4);

    // Army.PAS:1673
    public void TurnUnFogOff()
    {
        byte range;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            range = _unfogRange;
            if (range != 0)
            {
                StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, range);
                _unfogRange = 0;
            }
        }
    }

    // Army.PAS:1687
    public void UpdateExploration()
    {
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            for (int player = 1, last = StrategicMap.Current.Players.GetCount() - 1; player <= last; player++)
            {
                if (player == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.Alliance)
                {
                    byte visibilityRange = VisibilityRange();
                    StrategicMap.Current.ExploreArea(Location.X, Location.Y, Location.MapLevel, (sbyte)player, visibilityRange);
                }
            }
        }
    }

    // Army.PAS:1697
    public void UpdateUnfog()
    {
        int range;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            range = StrategicMap.Current.SeatedPlayer != -1 && (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(StrategicMap.Current.SeatedPlayer) == DiplomaticRelation.Alliance)
                ? VisibilityRange()
                : 0;

            if (range != _unfogRange)
            {
                if (_unfogRange != 0)
                {
                    StrategicMap.Current.FogArea(Location.X, Location.Y, Location.MapLevel, _unfogRange);
                }

                _unfogRange = (byte)range;
                if (Location.X != -1)
                {
                    if (_unfogRange != 0)
                    {
                        StrategicMap.Current.UnFogArea(Location.X, Location.Y, Location.MapLevel, _unfogRange);
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
        AddUnitsResult result;
        int i;
        int transportCount;
        Troop transport;
        transportCount = 0;
        i = 0;
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
                transport = Transporter(255);
                if (transport == null)
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

                result = units.Count + Units.Count <= transport.GetTransportCapacity() + 1 ? AddUnitsResult.Ok : AddUnitsResult.TooManyUnits;
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

                    result = i != units.Count ? AddUnitsResult.InvalidMoveType : AddUnitsResult.Ok;
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

        return result;
    }

    // Army.PAS:1782
    public AddUnitsResult CanAddUnitSelection(UnitCollection units, byte selection)
    {
        AddUnitsResult result;
        DelphiList<Troop> list;
        int i;
        int transportCount;
        Troop transport;
        list = new DelphiList<Troop>();
        i = 0;
        int last = units.Count - 1;
        for (; i <= last; i += 1)
        {
            if ((selection & 1) != 0)
            {
                list.Add(units.GetUnit(i));
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        transportCount = 0;
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

        if (transportCount <= 1)
        {
            if (transportCount == 1)
            {
                transport = Transporter(255);
                if (transport == null)
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
        UnitUserList list;
        AddUnitsResult result = CanAddUnitSelection(units, selection);
        if (result == AddUnitsResult.Ok)
        {
            using (units.Holder.BeginUpdate())
            {
                list = new UnitUserList(null) { Holder = CreationHolder() };
                for (int i = units.Count - 1; i >= 0; i--)
                {
                    if (((1 << i) & selection) != 0)
                    {
                        if (!units.GetUnit(i).GetTransporter())
                        {
                            list.AddUnit(units.GetUnit(i));
                        }
                        else
                        {
                            AddUnit(units.GetUnit(i));
                        }
                    }
                }

                for (int i = list.GetCount() - 1; i >= 0; i--)
                {
                    AddUnit(list.GetUnit(i));
                }

            }
            list?.Dispose();
        }

        return result;
    }

    // Army.PAS:1881
    public Troop SelectionUnit(byte selection, int index)
    {
        int i;
        Troop result = null;
        i = 0;
        while (i != Units.Count && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (index == 0)
                {
                    result = Units[i];
                }

                index -= 1;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            i += 1;
        }

        return result;
    }

    // Army.PAS:1919
    public void MakeIndependent(byte selection, ref ArmyHSList armies)
    {
        sbyte x2;
        sbyte y2;
        sbyte l2;
        Army goodArmy;
        Army evilArmy;
        DelphiList<Army> races;
        DelphiList<Troop> units;
        int race3;
        Troop troop;
        Alignment alignment;
        ArmyHexSprite armyHexSprite;
        ArmyHexSprite PlaceIndependentArmy(sbyte x, sbyte y, sbyte l, Army army)
        {
            ArmyHexSprite result;
            try
            {
                if (army.Units.Count != 0)
                {
                    result = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    result.Army.BeginUpdate();
                    result.Army.AddUnits(army.Units);
                    if (result.Place(x2, y2, l2, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
                    {
                        result.Activate();
                        result.Army.EndUpdate();
                    }
                    else
                    {
                        result.Army.Kill(255);
                        result?.Dispose();
                        result = null;
                    }
                }
                else
                {
                    result = null;
                }
            }
            catch
            {
                // { Original suppresses placement failures without releasing a partly built army. }
                result = null;
            }

            return result;
        }

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
                    units.Add(Units[i]);
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }

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
                    if (races.List[race] == null)
                    {
                        Army army = new(null) { Holder = CreationHolder() };
                        sbyte aUnitRace = troop.GetRace();
                        races.List[aUnitRace] = army;
                    }

                    sbyte race2 = troop.GetRace();
                    races.List[race2].AddUnit(troop);
                    units.Delete(i);
                }
            }

            for (int i = units.Count - 1; i >= 0; i--)
            {
                troop = units.List[i];
                alignment = troop.GetAlignment();
                race3 = -1;
                for (int j = races.Count - 1; j >= 0; j--)
                {
                    if (races.List[j] != null && GamePack.Current.RaceResources.GetRaceResource(j).Alignments.Contains(alignment))
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
                    races.List[race3].AddUnit(troop);
                    units.Delete(i);
                }
                else
                {
                    if (AlignmentTables.GoodAlignments.Contains(alignment) && (!AlignmentTables.EvilAlignments.Contains(alignment) || (goodArmy != null && evilArmy == null) || (goodArmy == null && evilArmy == null) || (goodArmy != null && evilArmy != null && goodArmy.Units.Count >= evilArmy.Units.Count)))
                    {
                        if (goodArmy == null)
                        {
                            goodArmy = new Army(null) { Holder = CreationHolder() };
                        }

                        goodArmy.AddUnit(troop);
                        units.Delete(i);
                    }
                    else
                    {
                        if (evilArmy == null)
                        {
                            evilArmy = new Army(null) { Holder = CreationHolder() };
                        }

                        evilArmy.AddUnit(troop);
                        units.Delete(i);
                    }
                }
            }

            x2 = Location.X;
            y2 = Location.Y;
            l2 = Location.MapLevel;
        }
        for (int i = 0, last = races.Count - 1; i <= last; i++)
        {
            if (races.List[i] != null)
            {
                armyHexSprite = PlaceIndependentArmy(x2, y2, l2, races.List[i]);
                if (armies != null && armyHexSprite != null)
                {
                    armies.AddChild(armyHexSprite);
                }

                races.List[i]?.Dispose();
                races.List[i] = null;
            }
        }

        if (goodArmy != null)
        {
            armyHexSprite = PlaceIndependentArmy(x2, y2, l2, goodArmy);
            if (armies != null && armyHexSprite != null)
            {
                armies.AddChild(armyHexSprite);
            }

            goodArmy?.Dispose();
        }

        if (evilArmy != null)
        {
            armyHexSprite = PlaceIndependentArmy(x2, y2, l2, evilArmy);
            if (armies != null && armyHexSprite != null)
            {
                armies.AddChild(armyHexSprite);
            }

            evilArmy?.Dispose();
        }

        units?.Dispose();
        races?.Dispose();
    }

    // Army.PAS:2029
    public void UpdateIndependentRelation()
    {
        sbyte race;
        bool valid;
        bool evil;
        bool good;
        bool neutral;
        int i;
        int unitRace;
        Troop troop;
        Alignment alignmentAlignment;
        if (Player == 0)
        {
            race = -1;
            valid = true;
            i = Units.Count;
            while (valid && i != 0)
            {
                i -= 1;
                unitRace = !Units[i].GetAbilityEnabled(107) ? Units[i].GetRace() : -1;

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
                    condition = GamePack.Current.RaceResources.GetRaceResource(race).Alignments.Contains(alignment);
                }

                valid = condition;
            }

            if (!valid)
            {
                evil = true;
                good = true;
                neutral = true;
                i = 0;
                int last = Units.Count - 1;
                for (; i <= last; i += 1)
                {
                    troop = Units[i];
                    alignmentAlignment = troop.GetAlignment();
                    evil = evil && AlignmentTables.EvilAlignments.Contains(alignmentAlignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Alignment.Evil, Alignment.PureEvil).Contains(alignmentAlignment));
                    good = good && AlignmentTables.GoodAlignments.Contains(alignmentAlignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Alignment.PureGood, Alignment.Good).Contains(alignmentAlignment));
                    neutral = neutral && AlignmentTables.NeutralAlignments.Contains(alignmentAlignment);
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
        DelphiList<Troop> units;
        sbyte player;
        ArmyHSList armies;
        int count;
        int bit;
        ArmyInfoEventLog log;
        Troop troop;
        StrategicMap.Current.LockGameOverEvent();
        units = new DelphiList<Troop>();
        count = SelectionCount(selection);
        log = new ArmyInfoEventLog(null) { Holder = CreationHolder() };
        log.ArmyView.Setup(this, 255);
        log.Selection = selection;
        log.Player = Player;
        log.Title = AbilityRules.TranslateResourceString(GetEngine(), s_desertionText);
        log.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.User);
        if (StrategicMap.Current.PlayMode == PlayMode.Turn && GetEngine().Registry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion))
        {
            log.Triggers = log.Triggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
            if (StrategicMap.Current.RequestCombatType)
            {
                log.Triggers = log.Triggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.NewTurn);
            }
        }

        if (StrategicMap.Current.PlayMode == PlayMode.Simultaneous && GetEngine().Registry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion))
        {
            log.Triggers = log.Triggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.Event, ExecuteTrigger.NewTurn);
        }

        log.Description = AbilityRules.TranslateResourceString(GetEngine(), s_leaderOfPartyStepsForwardText);
        log.Text = count == 1
            ? AbilityRules.TranslateResourceString(GetEngine(), s_oneUnitDesertedText)
            : AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsDesertedFormat), [IntToStr(count)]);

        if (StrategicMap.Current.Players.GetPlayers(Player).Gems == 0 && StrategicMap.Current.Players.GetPlayers(Player).GetUpkeep() > StrategicMap.Current.Players.GetPlayers(Player).Income)
        {
            if (count == 1)
            {
                string desertUnitNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitNoUpkeepText);
                log.Messages.Text = desertUnitNoUpkeep;
            }
            else
            {
                string desertUnitsNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsNoUpkeepText);
                log.Messages.Text = desertUnitsNoUpkeep;
            }
        }
        else
        {
            if (ValidateSelection(255) == selection)
            {
                if (count == 1)
                {
                    string desertUnitLowMorale = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitLowMoraleText);
                    log.Messages.Text = desertUnitLowMorale;
                }
                else
                {
                    string desertUnitsLowMorale = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsLowMoraleText);
                    log.Messages.Text = desertUnitsLowMorale;
                }
            }
            else
            {
                if (count == 1)
                {
                    string desertUnitLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitLowMoraleSplitText);
                    log.Messages.Text = desertUnitLowMoraleSplit;
                }
                else
                {
                    string desertUnitsLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsLowMoraleSplitText);
                    log.Messages.Text = desertUnitsLowMoraleSplit;
                }
            }
        }

        EventLog.DistributeLocationEventLog(log, -1, Location.X, Location.Y, Location.MapLevel, true);
        log.Release();
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            bit = 1 << i;
            if ((bit & selection) != 0 && Units.GetUnit(i).GetAbilityEnabled(155))
            {
                units.Add(Units.GetUnit(i));
                selection = (byte)(selection & ~(byte)bit);
            }
        }

        armies = new ArmyHSList(null) { Holder = CreationHolder() };
        player = Player;
        MakeIndependent(selection, ref armies);
        while (units.Count != 0)
        {
            troop = units[0];
            troop.AddRef();
            troop.Killed();
            troop.SetOwner(null);
            troop.Release();
            units.Delete(0);
        }

        for (int i = 0, last = armies.GetCount() - 1; i <= last; i++)
        {
            if (AbilityRules.DiplomaticRelation(0, armies.GetArmyHexSprite(i).Army.IndependentRelation, player, 0) == DiplomaticRelation.War)
            {
                armies.GetArmyHexSprite(i).Army.SetBehavior(255, GroupBehavior.Raid);
            }
            else
            {
                armies.GetArmyHexSprite(i).Army.SetBehavior(255, GroupBehavior.Scout);
            }
        }

        armies?.Dispose();
        units?.Dispose();
        StrategicMap.Current.UnlockGameOverEvent();
    }

    // Army.PAS:2155
    public PartyStatus GetPartyStatus(byte selection)
    {
        PartyStatus result;
        int morale;
        int count;
        CombatPredictor predictor;
        int unitMorale;
        Troop troop;
        morale = 100;
        count = 0;
        predictor = new CombatPredictor(this);
        for (int i = 0, last = Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                troop = Units[i];
                unitMorale = troop.MoraleValue;
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
                    predictor.AddOpponent(troop, 0);
                }
                else
                {
                    predictor.AddUnit(troop);
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        result = predictor.Execute() == CombatResult.Defeat
            ? predictor.GetSuperiority() < 50 ? PartyStatus.Unruly : PartyStatus.Unrest
            : count == 0 || morale <= 60 ? PartyStatus.Stable : morale <= 80 ? PartyStatus.Content : PartyStatus.Cheerful;

        predictor?.Dispose();
        return result;
    }

    // Army.PAS:2185
    public void UpdateDesertion()
    {
        PartyStatus status;
        int chance;
        byte selection;
        if (Location.X != -1 && Player != 0)
        {
            status = GetPartyStatus(255);
            if (Set<PartyStatus>.Of(PartyStatus.Unruly, PartyStatus.Unrest).Contains(status))
            {
                chance = status == PartyStatus.Unrest ? 10 : 50;

                if (chance > StrategicMap.Current.Random(100))
                {
                    selection = 0;
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
        int i;
        i = Units.Count;
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

                result = Units.Count != 0;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:2230
    public void NewTurn(sbyte player)
    {
        if (player == Player)
        {
            UserMode = UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
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
                StrategicMap.Current.ExploreArea(Location.X, Location.Y, Location.MapLevel, Player, visibilityRange);
                byte visibilityRange2 = VisibilityRange();
                byte trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(Player, Location.X, Location.Y, Location.MapLevel, visibilityRange2, trueVisionRange);
            }

            InitializeGroupBehavior();
        }
    }

    // Army.PAS:2283
    public bool CanActivate() => true;

    // Army.PAS:2288
    public MoveAction GetMoveAction(sbyte player)
    {
        MoveAction result = MoveAction.Normal;
        if (player != Player)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).PlayerType == PlayerType.Human)
            {
                if (!Status.Contains(ArmyStatusElement.FullyConcealed))
                {
                    result = Player == 0 && GetJoinSelection(player) != 0 ? MoveAction.Negotiate : MoveAction.Attack;
                }
            }
            else
            {
                result = Player == 0 && GetJoinSelection(player) != 0 ? MoveAction.Negotiate : MoveAction.Attack;
            }
        }
        else
        {
            result = MoveAction.Combine;
        }

        return result;
    }

    // Army.PAS:2306
    public byte GetCanJoinSelection(sbyte player)
    {
        byte result;
        Troop troop;
        if (player > 0)
        {
            result = 0;
            for (int i = Units.Count - 1; i >= 0; i--)
            {
                result = (byte)(result << 1);
                troop = Units[i];
                if (troop.CanJoin(player))
                {
                    result = (byte)(result | 1);
                }
            }

            if (IndexOfTransporter(255) != -1)
            {
                if (result != ValidateSelection(255))
                {
                    result = 0;
                }
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Army.PAS:2324
    public byte GetJoinSelection(sbyte player)
    {
        bool wantsJoin;
        int i;
        byte result = 0;
        if (player > 0)
        {
            wantsJoin = false;
            i = Units.Count;
            while (!wantsJoin && i != 0)
            {
                i -= 1;
                wantsJoin = Units[i].OfferToJoin(player);
            }

            if (wantsJoin)
            {
                result = GetCanJoinSelection(player);
            }
        }

        return result;
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

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return result;
    }

    // Army.PAS:2353
    public bool IncomingNegotiateRequest(NegotiateRequestEventLog log)
    {
        bool result = false;
        if (Player == 0)
        {
            log.SetupJoinArmy(this, log.Player, JoinMode.Default);
            log.Object = NegotiateObject.Army;
            log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_partyJoinHeaderText);
            result = log.Selection != 0;
        }

        return result;
    }

    // Army.PAS:2365
    public bool IncomingNegotiateOffer(NegotiateTokenEvent offer)
    {
        bool result;
        ArmyInfoEventLog log;
        byte selection;
        Hero hero;
        MapCoordinateList locations;
        int i;
        int j;
        int direction;
        int x = default;
        int y = default;
        int joinCount;
        int side;
        bool placed;
        if (offer.Object == NegotiateObject.Army)
        {
            if (offer.Result == NegotiateTokenEvent.NegotiateResult.Accept)
            {
                selection = GetJoinSelection(offer.Player);
                joinCount = SelectionCount(selection);
                if (offer.Mode.Contains(NegotiateModeElement.PrisonerJoin) || JoinAmount(offer.Player, selection) <= offer.Gold)
                {
                    using (BeginUpdate())
                    {
                        offer.Mode = offer.Mode + Set<NegotiateModeElement>.Of(NegotiateModeElement.MoveOnAccept);
                        log = new ArmyInfoEventLog(null) { Holder = CreationHolder() };
                        log.ArmyView.Setup(this, selection);
                        log.Selection = 255;
                        log.Description = "";
                        log.Title = AbilityRules.TranslateResourceString(GetEngine(), s_partyJoinText);
                        log.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.User);
                        hero = Hero(selection, Set<HeroType>.Of(HeroType.Hero));
                        if (hero == null)
                        {
                            if (joinCount == 1)
                            {
                                log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_oneUnitJoinedText);
                                string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitJoinedForcesFormat), [name]);
                                log.Messages.Text = format;
                            }
                            else
                            {
                                log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsJoinedFormat), [IntToStr(joinCount)]);
                                int selectionCount = SelectionCount(selection);
                                string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                                string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsJoinedForcesFormat), [selectionCount, name]);
                                log.Messages.Text = format;
                            }
                        }
                        else
                        {
                            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_joinedFormat), [hero.HeroName]);
                            string name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                            string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_joinedForcesFormat), [hero.HeroName, name]);
                            log.Messages.Text = format;
                        }

                        EventLog.DistributeLocationEventLog(log, -1, Location.X, Location.Y, Location.MapLevel, false);
                        log.Release();
                        if (ValidateSelection(255) != selection)
                        {
                            locations = new MapCoordinateList();
                            if (offer.ArmyHexSprite != null)
                            {
                                sbyte hexY = offer.ArmyHexSprite.GetHexY();
                                sbyte hexX = offer.ArmyHexSprite.GetHexX();
                                direction = HexGeometry.DeltaHexToHexNumberFast(Location.X, Location.Y, hexX, hexY);
                                if (direction > 0 && direction <= 6)
                                {
                                    locations.AddMapCoordinate(Location.X, Location.Y, Location.MapLevel);
                                    side = direction + 1;
                                    if (side > 6)
                                    {
                                        side = 1;
                                    }

                                    // { Original sign extends the wrapped byte coordinates here. }
                                    HexGeometry.CenterHexNumberToHex(offer.Location.X, offer.Location.Y, side, ref x, ref y);
                                    locations.AddMapCoordinate((sbyte)x, (sbyte)y, Location.MapLevel);
                                    side = direction - 1;
                                    if (side == 0)
                                    {
                                        side = 6;
                                    }

                                    HexGeometry.CenterHexNumberToHex(offer.Location.X, offer.Location.Y, side, ref x, ref y);
                                    locations.AddMapCoordinate((sbyte)x, (sbyte)y, Location.MapLevel);
                                }
                            }

                            j = 0;
                            i = Units.Count;
                            while (i != 0)
                            {
                                i -= 1;
                                if ((selection & (1 << i)) != 0)
                                {
                                    placed = false;
                                    while (!placed && j < locations.Count)
                                    {
                                        MapCoordinate item = locations[j];
                                        MapCoordinate locationsItem = locations[j];
                                        MapCoordinate item2 = locations[j];
                                        placed = Units.GetUnit(i).Place(item2.X, locationsItem.Y, item.MapLevel, offer.Player, Set<UnitPlaceModeElement>.Of()) != null;
                                        if (!placed)
                                        {
                                            j += 1;
                                        }
                                    }

                                    if (!placed)
                                    {
                                        Units.GetUnit(i).Place(Location.X, Location.Y, Location.MapLevel, offer.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
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
                    offer.Mode = Set<NegotiateModeElement>.Of(NegotiateModeElement.AttackOnReject);
                    offer.Reject();
                }
            }
            else
            {
                if (offer.Result == NegotiateTokenEvent.NegotiateResult.Reject && !offer.Mode.Contains(NegotiateModeElement.PrisonerJoin))
                {
                    offer.Reject();
                }
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Army.PAS:2468
    public void TriggerFireDamage()
    {
        ArmyEventLog log;
        Troop troop;
        int damage;
        bool damaged;
        RandSeed = StrategicMap.Current.Random(16777215);
        log = new ArmyEventLog(null) { Holder = CreationHolder() };
        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_fireDamageText);
        log.Player = GetArmyHexSprite().Army.Player;
        string fireDamage = AbilityRules.TranslateResourceString(GetEngine(), s_fireDamageText);
        log.ArmyMessages.Add(fireDamage);
        log.ArmyIcon = 0;
        ArmyHexSprite armyHexSprite = GetArmyHexSprite();
        log.ArmyView.Setup(armyHexSprite.Army, 255);
        using (BeginUpdate())
        {
            damaged = false;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                troop = Units.GetUnit(i);
                if (!troop.GetImmunityTypes().Contains(DamageType.Fire))
                {
                    damage = troop.ExecuteDamageRole(4, 4, Set<DamageType>.Of(DamageType.Fire));
                    if (damage > troop.GetHitPoints())
                    {
                        damage = troop.GetHitPoints();
                    }

                    if (damage != 0)
                    {
                        damaged = true;
                        troop.SetHitPoints((sbyte)(troop.GetHitPoints() - (sbyte)damage));
                    }

                    log.DamageValues.Add((byte)damage);
                    log.DamageTypes.Add((int)DamageType.Fire);
                }
                else
                {
                    log.DamageValues.Add(0);
                    log.DamageTypes.Add((int)DamageType.None);
                }
            }

            log.ValidateGameOver();
            if (damaged)
            {
                EventLog.DistributeLocationEventLog(log, -1, Location.X, Location.Y, Location.MapLevel, true);
            }

            log.Release();
            Validate();
        }
    }

    // Army.PAS:2508
    public void RetrieveStormDamageInfo(StormType storm, int distance, ref StormDamageInfo info)
    {
        int attack;
        int damage;
        byte relation;
        int value = default;
        int amount = default;
        Troop troop;
        relation = (byte)(int)AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, Player, IndependentRelation);
        if (info.FriendlyRelations.Contains(relation) || info.EnemyRelations.Contains(relation))
        {
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
                default:
                    attack = 0;
                    damage = 0;
                    break;
            }

            for (int i = Units.Count - 1; i >= 0; i--)
            {
                troop = Units.GetUnit(i);
                if (troop.Player != 0 || (troop.Group != null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive))
                {
                    troop.RetrieveDamageInfo((sbyte)attack, (sbyte)damage, Set<DamageType>.Of(StormDamageType[(int)storm]), out value, out amount);
                    if (info.FriendlyRelations.Contains(relation))
                    {
                        info.FriendlyDamage += amount;
                        // { Original assigns the second return to damage. }
                        info.FriendlyValue += value;
                    }

                    if (info.EnemyRelations.Contains(relation))
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
        ArmyEventLog log;
        bool damaged;
        int damage;
        RandSeed = StrategicMap.Current.Random(16777215);
        log = new ArmyEventLog(null) { Holder = CreationHolder() };
        log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), GameTables.StormTypeTexts[(int)storm]);
        log.Player = player;
        // { Original appends the storm message. }
        string stormType = AbilityRules.TranslateResourceString(GetEngine(), GameTables.StormTypeTexts[(int)storm]);
        log.ArmyMessages.Add(stormType);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(this, 255);
        using (BeginUpdate())
        {
            damaged = false;
            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                damage = Units[i].ExecuteStormDamage(storm, distance);
                if (damage != 0)
                {
                    damaged = true;
                }

                log.DamageValues.Add((byte)damage);
                log.DamageTypes.Add((byte)(int)StormDamageType[(int)storm]);
            }

            if (damaged)
            {
                log.ValidateGameOver();
                EventLog.DistributeLocationEventLog(log, Player, Location.X, Location.Y, Location.MapLevel, true);
            }

            log.Release();
            Validate();
        }
    }

    // Army.PAS:2567
    public void UpdateAITarget(in AITargetInfo info, ref AITargetData data)
    {
        byte selection;
        switch (info.Mode)
        {
            case AITargetMode.Attack:
            case AITargetMode.Raid:
            case AITargetMode.AttackArmies:
                if (!GetUnitsConcealedForPlayer(255, info.Player) && info.Player != Player)
                {
                    if (Player > 0)
                    {
                        data.Priority = AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, IndependentRelation) || info.Mode == AITargetMode.Raid
                            ? info.Player != 0 && Leader(255) != null
                                ? 300
                                : info.Player != 0 && Hero(255, Set<HeroType>.Of(HeroType.Hero)) != null ? 100 : 50
                            : 0;
                    }
                    else
                    {
                        if (Player == 0)
                        {
                            selection = GetJoinSelection(info.Player);
                            if (selection != 0)
                            {
                                int joinAmount = JoinAmount(info.Player, selection);
                                data.Cost = joinAmount;
                                data.TargetType = AITargetType.Negotiate;
                            }
                            else
                            {
                                if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, IndependentRelation))
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
                if (info.Player == Player && (Player != 0 || AbilityRules.DiplomaticRelation(info.Player, info.IndependentRelation, Player, IndependentRelation) != DiplomaticRelation.War))
                {
                    data.Priority = data.Priority | 1;
                }

                break;
        }
    }

    // Army.PAS:2605
    public int AIMovePriority()
    {
        int result = 0;
        if (Leader(255) != null)
        {
            result += 50000;
        }

        if (Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.HeroLeader)) != null)
        {
            result += 10000;
        }

        result += ObtainValue(255);
        return result;
    }

    // Army.PAS:2613
    public void ResetMoveDone()
    {
        Troop troop;
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            troop = Units[i];
            troop.UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.MoveDone);
        }
    }

    // Army.PAS:2623
    public bool CanCombine(Army army, byte selection)
    {
        bool result;
        int i;
        int maxCount;
        byte temporary;
        Troop ownTransporter;
        Troop otherTransporter;
        selection = army.ValidateSelection(selection);
        i = 0;
        temporary = selection;
        while (temporary != 0)
        {
            if ((temporary & 1) != 0)
            {
                i += 1;
            }

            temporary = (byte)(int)(((uint)temporary) >> 1);
        }

        ownTransporter = Transporter(255);
        maxCount = ownTransporter != null ? ownTransporter.GetTransportCapacity() + 1 : 8;

        i += Units.Count;
        if (i <= maxCount)
        {
            otherTransporter = army.Transporter(selection);
            result = otherTransporter == null || (i <= otherTransporter.GetTransportCapacity() + 1 && ownTransporter == null);
        }
        else
        {
            result = false;
        }

        return result;
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
        DamageType.None,
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
