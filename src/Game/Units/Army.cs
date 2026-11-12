// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
public partial class Army : UnitList
{
    public new const int ClassId = 131606;

    // Army.PAS:324
    public Army(ECustomNode owner) : base(owner)
    {
        X = -1;
        Y = -1;
        MapLevel = -1;
        DestinationX = -1;
        DestinationY = -1;
        DestinationMapLevel = -1;
        IndependentRelation = -1;
    }

    public Set<ArmyStatusElement> Status { get; set; }

    public Set<ArmyModeElement> Mode;
    public sbyte Player;
    public sbyte X;
    public sbyte Y;
    public sbyte MapLevel;

    public sbyte IndependentRelation { get; private set; }

    private byte _unfogRange;
    private byte _upkeep;

    public event NotifyEvent<object> OnUpdate;

    internal void SetUpdateHandler(NotifyEvent<object> handler) => OnUpdate = handler;

    internal void ClearUpdateHandler() => OnUpdate = null;

    public GroupBehavior OldAIBehavior;
    public sbyte DestinationX;
    public sbyte DestinationY;
    public sbyte DestinationMapLevel;
    public Set<ArmyUserModeElement> UserMode;
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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 4);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref X), 5);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Y), 6);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 7);
        if (!stream.StorageMode().Contains(StorageModeElement.Write) || StrategicMap.Current == null || StrategicMap.Current.ConnectionType != (PlayerConnectionType)1)
        {
            stream.Byte(ref Unsafe.As<sbyte, byte>(ref DestinationX), 8);
            stream.Byte(ref Unsafe.As<sbyte, byte>(ref DestinationY), 9);
            stream.Byte(ref Unsafe.As<sbyte, byte>(ref DestinationMapLevel), 10);
            TransferSet(ref UserMode, 1, 13, stream.Bytes, 0);
        }

        stream.Bytes(Bytes.Of(ref OldAIBehavior, 1), 1, 11);
        TransferSet(ref Mode, 1, 12, stream.Bytes, 0);
        if (stream.StorageMode().Contains(StorageModeElement.Read) && Mode.Contains(ArmyModeElement.Moving))
        {
            Mode = Mode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        X = PersistSupport.UnsignedByte(persister, name: "X", value: X);
        Y = PersistSupport.UnsignedByte(persister, name: "Y", value: Y);
        MapLevel = PersistSupport.UnsignedByte(persister, name: "MapLevel", value: MapLevel);
        if (persister.Loading || StrategicMap.Current == null || StrategicMap.Current.ConnectionType != (PlayerConnectionType)1)
        {
            DestinationX = PersistSupport.UnsignedByte(persister, name: "DestinationX", value: DestinationX);
            DestinationY = PersistSupport.UnsignedByte(persister, name: "DestinationY", value: DestinationY);
            DestinationMapLevel = PersistSupport.UnsignedByte(persister, name: "DestinationMapLevel", value: DestinationMapLevel);
            UserMode = persister.Set("UserMode", value: UserMode);
        }

        OldAIBehavior = persister.Enum("OldAIBehavior", value: OldAIBehavior);
        Mode = persister.Set("Mode", value: Mode);
        if (persister.Loading && Mode.Contains(ArmyModeElement.Moving))
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

    // Army.PAS:1844
    public override int RemoveChild(EngineObject child)
    {
        BeginUpdate();
        var unit = child;
        unit.AddRef();

        int result = base.RemoveChild(child);
        var unit2 = child;
        unit2.Release();

        EndUpdate();
        return result;
    }

    // Army.PAS:1853
    public override int AddChild(EngineObject child)
    {
        int result;
        // The check refuses a child that is not a troop before the troop is read.
        CheckChild(child);
        var troop = (Troop)child;
        if (!Status.Contains(ArmyStatusElement.Activated) || troop.CanActivate())
        {
            result = base.AddChild(child);
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

    // Army.PAS:716
    public override void Changed()
    {
        Update();
    }

    // Army.PAS:1241
    public override sbyte GetPlayer() => Player;

    // Army.PAS:1874
    public override void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel)
    {
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            hexX = X;
            hexY = Y;
            hexLevel = MapLevel;
        }
        else
        {
            hexX = -1;
            hexY = -1;
            hexLevel = -1;
        }
    }

    // Army.PAS:1719
    public override bool CanAddUnit(Troop troop)
    {
        bool result;
        bool hasTransporter;
        MapField field;
        if (Status.Contains(ArmyStatusElement.Activated))
        {
            result = GetCount() < MaxSize() && troop.CanAddToList(this);
            hasTransporter = IndexOfTransporter(255) != -1;
            if (result && !hasTransporter && X != -1 && X != 0 && !Mode.Contains(ArmyModeElement.DisableTerrainValidation))
            {
                // { Original skips terrain validation on the zero X column. }
                field = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel);
                result = troop.ValidTerrain(field.TerrainType, field.OverlayType);
            }

            if (result && troop.GetTransporter())
            {
                result = !hasTransporter && troop.GetTransportCapacity() >= GetCount();
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:1836
    public override bool CanDeleteUnit(Troop troop)
    {
        bool result;
        if (IndexOfTransporter(255) != -1)
        {
            if (troop.GetTransporter() && UnitsInTransporter() != 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Army.PAS:1868
    public override bool AddUnit(Troop troop)
    {
        troop.SetOwner(this);
        return this == troop.Owner;
    }

    // Army.PAS:1770
    public override bool AddUnits(UnitList units)
    {
        bool result = CanAddUnits(units) == AddUnitsResult.Ok;
        if (result)
        {
            BeginUpdate();
            for (int I = units.GetCount() - 1; I >= 0; I--)
            {
                AddUnit(units.GetUnit(I));
            }

            EndUpdate();
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
            level = StrategicMap.Current.MapContainer.MapLevels[MapLevel];
            message.MessageId = 4363;
            message.Result = 0;
            message.Param[1] = player;
            HexNumberWalkData.InitializeHexNumberWalk(out walk, X, Y, 0);
            while (result && walk.Radius < 8)
            {
                if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                {
                    field = level.GetField(walk.X, walk.Y);
                    if ((field.FieldFlags & 4096) != 0)
                    {
                        message.Param[0] = walk.Radius;
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
        BeginUpdate();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            if ((((int)(((uint)selection) >> I)) & 1) != 0)
            {
                this[I].Killed();
            }
        }

        EndUpdate();
    }

    // Army.PAS:352
    public int GetStrength(byte selection, DamageType damageType)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (((1 << I) & selection) != 0)
            {
                result += this[I].GetStrength(damageType);
            }
        }

        return result;
    }

    // Army.PAS:361
    public int GetStrengthMax(byte selection, Set<DamageType> damageTypes)
    {
        int result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (((1 << I) & selection) != 0)
            {
                result += this[I].GetStrengthMax(damageTypes);
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
        for (int I = Count - 1; I >= 0; I--)
        {
            troop = this[I];
            if (troop.Group != null)
            {
                behavior = troop.Group.GroupClass.Behavior;
            }
            else
            {
                behavior = GroupBehavior.Passive;
            }

            includeUnit = info.Units == null || info.Units.IndexOfChild(troop) == -1;
            if (includeUnit && info.Requests.Contains(BattleFieldInfoRequestsElement.IgnorePassiveIndependents))
            {
                includeUnit = troop.Player != 0 || troop.Group != null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive && troop.Group.GroupClass.Behavior != GroupBehavior.Guard;
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
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                troop = this[I];
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
        int I;
        I = 0;
        while ((selection & 1) == 0 && I != Count)
        {
            I += 1;
            selection = (byte)(int)(((uint)selection) >> 1);
        }

        if (I != Count)
        {
            result = this[I].UnitId;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Army.PAS:423
    public AIGroup GetGroup(byte selection)
    {
        bool different;
        int I;
        bool found;
        AIGroup result = null;
        different = false;
        I = 0;
        found = false;
        while (I != Count && !different)
        {
            if ((selection & 1) != 0)
            {
                if (!found)
                {
                    result = this[I].Group;
                    found = true;
                }
                else
                {
                    different = result != this[I].Group;
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                group.AddUnit(this[I]);
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // Army.PAS:455
    public byte GetGroupSelection(int groupId)
    {
        byte result = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            result = (byte)(result << 1);
            if (groupId == this[I].GroupId)
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
        if (group != null)
        {
            result = group.GroupClass.Behavior;
        }
        else
        {
            result = GroupBehavior.None;
        }

        return result;
    }

    // Army.PAS:473
    public void SetBehavior(byte selection, GroupBehavior behavior)
    {
        AIGroup group;
        group = StrategicMap.Current.AIGroupManager.CreateGroup(Player, IndependentRelation, behavior);
        if (group != null)
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if ((selection & 1) != 0)
                {
                    group.AddUnit(this[I]);
                }

                selection = (byte)(int)(((uint)selection) >> 1);
            }

            group.Release();
        }
    }

    // Army.PAS:488
    public GroupBehavior GetOldAIBehavior()
    {
        int I;
        GroupBehavior result = GroupBehavior.None;
        I = GetCount();
        while (result == GroupBehavior.None && I != 0)
        {
            I -= 1;
            result = (GroupBehavior)GetUnit(I).LegacyGroupBehavior;
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
            index = IndexOf(hero);
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
                if (Player == 0 && StrategicMap.Current.GetIndependentAIMode() != StrategicMapIndependentAIMode.Off)
                {
                    if (X != -1 && StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) == null)
                    {
                        if (IndependentRelation >= 0)
                        {
                            behavior = GroupBehavior.Patrol;
                        }
                        else
                        {
                            behavior = GroupBehavior.GuardArea;
                        }
                    }
                    else
                    {
                        behavior = GroupBehavior.Passive;
                    }
                }
                else
                {
                    behavior = StrategicMap.Current.Players.GetPlayers(Player).AIControl.GetRandomGroupBehavior();
                }
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
                if (this[index].GroupId == 0)
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
                if (Player == 0 && StrategicMap.Current.GetIndependentAIMode() != StrategicMapIndependentAIMode.Off && X != -1)
                {
                    if (StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).FindNoneTransparentHexSprite(typeof(PlayerStructure)) == null)
                    {
                        if (IndependentRelation >= 0)
                        {
                            behavior = GroupBehavior.Patrol;
                        }
                        else
                        {
                            behavior = GroupBehavior.GuardArea;
                        }
                    }
                }
            }

            if (selection != 0)
            {
                index = IndexOfBehavior(behavior);
                if (index == -1)
                {
                    SetBehavior(selection, behavior);
                }
                else
                {
                    SetGroup(selection, this[index].Group);
                }
            }
        }
    }

    // Army.PAS:560
    public ArmyId CreateId(byte selection)
    {
        ArmyId id;
        id = new ArmyId(null) { Holder = CreationHolder() };
        id.X = X;
        id.Y = Y;
        id.MapLevel = MapLevel;
        id.Player = Player;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                id.UnitIds.Add(this[I].UnitId);
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        return id;
    }

    // Army.PAS:579
    public ArmyHexSprite GetArmyHexSprite()
    {
        ArmyHexSprite result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(out X, out Y, out L);
        if (X != -1)
        {
            result = (ArmyHexSprite)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131607);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Army.PAS:591
    public void AssignSettings(Army army)
    {
        DestinationX = army.DestinationX;
        DestinationY = army.DestinationY;
        DestinationMapLevel = army.DestinationMapLevel;
        Player = army.Player;
    }

    // Army.PAS:599
    public void SetFullyConcealed(bool value)
    {
        if (value != Status.Contains(ArmyStatusElement.FullyConcealed))
        {
            if (value)
            {
                Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed);
            }
            else
            {
                Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed);
            }

            if (Status.Contains(ArmyStatusElement.Activated) && X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(X, Y, MapLevel, 0);
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
            if (ConcealedUnits != 0 && StrategicMap.Current.SeatedPlayer != Player && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                concealed = GetConcealedForPlayer(StrategicMap.Current.SeatedPlayer);
            }
            else
            {
                concealed = false;
            }

            leader = Leader(255);
            showCrown = leader != null;
            if (showCrown && concealed)
            {
                var indexOf = IndexOf(leader);
                if ((((int)(((uint)ConcealedUnits) >> indexOf)) & 1) != 0)
                {
                    showCrown = false;
                }
            }

            changed = false;
            if (showCrown)
            {
                Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown);
            }
            else
            {
                Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown);
            }

            if (concealed != Status.Contains(ArmyStatusElement.Concealed))
            {
                if (concealed)
                {
                    Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed);
                }
                else
                {
                    Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed);
                }

                changed = true;
            }

            concealed = concealed && GetCount() == ConcealedUnitsCount();
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
    public void SetMapCoordinate(sbyte X, sbyte Y, sbyte L)
    {
        this.X = X;
        this.Y = Y;
        MapLevel = L;
        if (UpdateCount == 0)
        {
            UpdateConcealedUnits();
        }
    }

    // Army.PAS:647
    public int GetRace(byte selection)
    {
        bool same;
        int race;
        int result = -1;
        same = true;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                race = this[I].GetRace();
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
    public void ResetDestinationPosition()
    {
        DestinationX = -1;
        DestinationY = -1;
        DestinationMapLevel = -1;
    }

    // Army.PAS:672
    public void SetDestinationPosition(sbyte X, sbyte Y, sbyte L)
    {
        DestinationX = X;
        DestinationY = Y;
        DestinationMapLevel = L;
    }

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
        int I;
        MapField field;
        if (X != -1 && MapLevel != -1 && Status.Contains(ArmyStatusElement.Activated))
        {
            ConcealedUnits = 0;
            I = Count;
            field = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel);
            while (I != 0)
            {
                I -= 1;
                ConcealedUnits = (byte)(ConcealedUnits << 1);
                if (this[I].ConcealedOnMapField(field))
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
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            upkeep = (byte)(upkeep + this[I].GetUpkeep());
        }

        previous = _upkeep;
        if (previous != upkeep && Status.Contains(ArmyStatusElement.Activated))
        {
            delta = upkeep - previous;
            var players = StrategicMap.Current.Players.GetPlayers(Player);
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
            for (int I = GetCount() - 1; I >= 0; I--)
            {
                selection = (byte)(selection << 1);
                if (!this[I].ValidTerrain(terrainType, overlayType))
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
                BeginUpdate();
                log = new ArmyEventLog(null) { Holder = CreationHolder() };
                log._player = player;
                if (Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)terrainType))
                {
                    log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsDrownText);
                    var unitsDrown = AbilityRules.TranslateResourceString(GetEngine(), s_unitsDrownText);
                    log.ArmyMessages.Add(unitsDrown);
                    log.ArmyIcon = 3;
                }
                else
                {
                    switch (terrainType)
                    {
                        case 9:
                            log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsBurnedText);
                            var unitsBurned = AbilityRules.TranslateResourceString(GetEngine(), s_unitsBurnedText);
                            log.ArmyMessages.Add(unitsBurned);
                            log.ArmyIcon = 0;
                            break;
                        default:
                            log.ArmyText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsCrushedText);
                            var translatedText = AbilityRules.TranslateResourceString(GetEngine(), s_unitsCrushedText);
                            log.ArmyMessages.Add(translatedText);
                            log.ArmyIcon = 2;
                            break;
                    }
                }

                log.ArmyView.Setup(this, 255);
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    troop = GetUnit(I);
                    if ((((int)(((uint)selection) >> I)) & 1) != 0)
                    {
                        var hitPoints = troop.GetHitPoints();
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
                EventLog.DistributeLocationEventLog(log, Player, X, Y, MapLevel, true);
                log.Release();
                EndUpdate();
            }

            Validate();
        }
    }

    // Army.PAS:785
    public void ValidateTerrain(sbyte terrainType, sbyte overlayType)
    {
        ValidateTerrain(terrainType, overlayType, Player);
    }

    // Army.PAS:790
    public void Validate()
    {
        Troop troop;
        Unit newUnit;
        StrategicMap.Current.LockGameOverEvent();
        BeginUpdate();
        for (int I = Count - 1; I >= 0; I--)
        {
            troop = this[I];
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

        EndUpdate();
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
        for (int I = Count - 1; I >= 0; I--)
        {
            troop = this[I];
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
            for (int I = Count - 1; I >= 0; I--)
            {
                troop = this[I];
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
            SortUnits();
            UpdateVisibilityRanges();
            UpdateUnfog();
            UpdateUpkeep();
            UpdateConcealedUnits();
            UpdateConcealment();
            UpdateFormation();
            UpdateIndependentRelation();
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && X != -1)
            {
                StrategicMap.Current.ScannerIconsChangedRadius(X, Y, MapLevel, 0);
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
            if (OnUpdate != null)
            {
                OnUpdate(this);
            }
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

        for (int I = Count - 1; I >= 0; I--)
        {
            troop = this[I];
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
        if (X == 0)
        {
            X = -1;
            Y = -1;
            MapLevel = -1;
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
                StrategicMap.Current.FogArea(X, Y, MapLevel, _unfogRange);
                _unfogRange = 0;
            }

            for (int I = Count - 1; I >= 0; I--)
            {
                troop = this[I];
                if (troop.UnitStatus.Contains(UnitStatusElement.Activated))
                {
                    troop.Deactivate();
                }
            }

            if (Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                var players = StrategicMap.Current.Players.GetPlayers(Player);
                StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
            }

            Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        }
    }

    // Army.PAS:1211
    public byte ValidateSelection(byte selection) => (byte)(selection & AbilityRules.ArmySelectionMasks[Count]);

    // Army.PAS:1216
    public int SelectionCount(byte selection)
    {
        selection = (byte)(selection & AbilityRules.ArmySelectionMasks[Count]);
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
            var transporter = Transporter(255);
            result = (byte)(result & ~(1 << IndexOfChild(transporter)));
        }

        return result;
    }

    // Army.PAS:1235
    public void Select()
    {
        Status = Status + Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);
    }

    // Army.PAS:1238
    public void Unselect()
    {
        Status = Status - Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);
    }

    // Army.PAS:1244
    public bool GetUnitConcealed(int index)
    {
        MapField field;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel);
        return this[index].ConcealedOnMapField(field);
    }

    // Army.PAS:1262
    public bool GetUnitsConcealedForPlayer(byte selection, sbyte player)
    {
        bool result;
        selection = ValidateSelection(selection);
        if (selection == (ConcealedUnits & selection))
        {
            result = GetConcealedForPlayer(player);
        }
        else
        {
            result = false;
        }

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
            BeginUpdate();
            if (Status.Contains(ArmyStatusElement.Activated))
            {
                if (Player != -1)
                {
                    var players = StrategicMap.Current.Players.GetPlayers(Player);
                    StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep - _upkeep);
                    StrategicMap.Current.Players.GetPlayers(Player).Armies.RemoveChild(this);
                }

                Player = player;
                if (Player != -1)
                {
                    var players = StrategicMap.Current.Players.GetPlayers(Player);
                    StrategicMap.Current.Players.GetPlayers(Player).SetArmyUpkeep(players.ArmyUpkeep + _upkeep);
                    StrategicMap.Current.Players.GetPlayers(Player).Armies.AddChild(this);
                }

                for (int I = Count - 1; I >= 0; I--)
                {
                    this[I].SetPlayer(Player);
                }

                if (X != -1)
                {
                    StrategicMap.Current.ScannerIconsChangedRadius(X, Y, MapLevel, 0);
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

            EndUpdate();
        }
    }

    // Army.PAS:1332
    public Set<DamageType> ImmunityTypes(byte selection)
    {
        int I;
        Set<DamageType> result = AbilityRules.s_allDamageTypes;
        I = 0;
        while (result != Set<DamageType>.Of() && I != GetCount())
        {
            if ((selection & 1) != 0)
            {
                result = result & this[I].GetImmunityTypes();
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
        }

        return result;
    }

    // Army.PAS:1346
    public int ObtainValue(byte selection)
    {
        int result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                result += this[I].GetObtainValue();
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
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if ((selection & 1) != 0)
                    {
                        troop = this[I];
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
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if ((selection & 1) != 0)
                    {
                        troop = this[I];
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
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((selection & 1) != 0)
                {
                    result = result + this[I].GetMoveTypes();
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
            common = Set<MoveType>.Of().WithRange(World.Movement.MoveType.Walking, World.Movement.MoveType.Tunneling);
            moveType = (int)World.Movement.MoveType.Flying;
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if (((1 << I) & selection) != 0)
                {
                    moves = this[I].GetMoveTypes();
                    common = common & moves;
                    if (moveType != -1)
                    {
                        if ((MoveType)moveType == World.Movement.MoveType.Flying)
                        {
                            if (!moves.Contains(World.Movement.MoveType.Flying))
                            {
                                if (moves.Contains(World.Movement.MoveType.Swimming))
                                {
                                    moveType = (int)World.Movement.MoveType.Swimming;
                                }
                                else
                                {
                                    moveType = (int)World.Movement.MoveType.Walking;
                                }
                            }
                        }
                        else
                        {
                            if ((MoveType)moveType == World.Movement.MoveType.Swimming)
                            {
                                if ((Set<MoveType>.Of(World.Movement.MoveType.Flying, World.Movement.MoveType.Swimming) & moves) == Set<MoveType>.Of())
                                {
                                    moveType = (int)World.Movement.MoveType.Walking;
                                }
                            }
                            else
                            {
                                if ((MoveType)moveType == World.Movement.MoveType.Walking)
                                {
                                    if ((Set<MoveType>.Of(World.Movement.MoveType.Walking, World.Movement.MoveType.Flying).WithRange(World.Movement.MoveType.Forestry, World.Movement.MoveType.LavaCrossing) & moves) == Set<MoveType>.Of())
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
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((selection & 1) != 0)
                {
                    this[I].CreateMovePointTable(out unitTable);
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
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((selection & 1) != 0)
                {
                    this[I].CreateMovePointTable(out unitTable);
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
        int I;
        Troop transport;
        transport = Transporter(selection);
        if (transport == null)
        {
            I = 0;
            result = true;
            while (I != GetCount() && result)
            {
                if (((1 << I) & selection) != 0)
                {
                    result = this[I].ValidTerrain(terrainType, overlayType);
                }

                I += 1;
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
        int I;
        Troop transport;
        transport = Transporter(255);
        if (transport == null)
        {
            I = 0;
            result = true;
            while (I != GetCount() && result)
            {
                result = this[I].ValidTerrain(terrainType, overlayType);
                I += 1;
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
        int I;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        bool result = ValidTerrain(field.TerrainType, field.OverlayType);
        I = 0;
        if (!Status.Contains(ArmyStatusElement.Activated))
        {
            while (result && I != Count)
            {
                result = this[I].CanActivate();
                I += 1;
            }
        }

        return result;
    }

    // Army.PAS:1543
    public void MovedTo(MapField oldField, MapField newField, int maxCost)
    {
        Troop troop;
        BeginUpdate();
        UserMode = UserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
        troop = Transporter(255);
        if (troop == null)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                this[I].MovedTo(oldField, newField, maxCost);
            }
        }
        else
        {
            troop.MovedTo(oldField, newField, maxCost);
            for (int I = Count - 1; I >= 0; I--)
            {
                troop = this[I];
                if (!troop.GetTransporter())
                {
                    troop.MovedTo(oldField, newField, 0);
                }
            }
        }

        UpdateExploration();
        EndUpdate();
    }

    // Army.PAS:1565
    public int MaxSize()
    {
        int capacity;
        int result = 8;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            capacity = this[I].GetTransportCapacity();
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
        int I;
        Troop troop;
        I = 0;
        Hero result = null;
        while (I != GetCount() && result == null)
        {
            if ((selection & 1) != 0)
            {
                troop = this[I];
                if (troop is Hero && types.Contains(((Hero)troop).GetHeroType()))
                {
                    result = (Hero)troop;
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
        }

        return result;
    }

    // Army.PAS:1593
    public Leader Leader(byte selection)
    {
        int I;
        I = 0;
        Leader result = null;
        while (I != GetCount() && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (this[I].EngineClass.ClassId == 131634)
                {
                    result = (Leader)this[I];
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
        }

        return result;
    }

    // Army.PAS:1608
    public Troop Transporter(byte selection)
    {
        int I;
        I = 0;
        Troop result = null;
        while (I != GetCount() && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (this[I].GetTransporter())
                {
                    result = this[I];
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
        }

        return result;
    }

    // Army.PAS:1623
    public int IndexOfTransporter(byte selection)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            if (((1 << result) & selection) != 0 && this[result].GetTransporter())
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
        int I;
        MapField field;
        if (X != -1)
        {
            field = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel);
            I = 0;
            result = 0;
            while (I != GetCount())
            {
                if (!this[I].ValidTerrain(field.TerrainType, field.OverlayType))
                {
                    result = (byte)(result + (1 << I));
                }

                I += 1;
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            range = (byte)this[I].VisibilityRange();
            if (range > vision)
            {
                vision = range;
            }

            range = (byte)this[I].TrueVisionRange();
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
                StrategicMap.Current.FogArea(X, Y, MapLevel, range);
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
                    var visibilityRange = VisibilityRange();
                    StrategicMap.Current.ExploreArea(X, Y, MapLevel, (sbyte)player, visibilityRange);
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
            if (StrategicMap.Current.SeatedPlayer != -1 && (StrategicMap.Current.SeatedPlayer == Player || StrategicMap.Current.Players.GetPlayers(Player).DiplomaticRelations.GetDiplomaticRelation(StrategicMap.Current.SeatedPlayer) == DiplomaticRelation.Alliance))
            {
                range = VisibilityRange();
            }
            else
            {
                range = 0;
            }

            if (range != _unfogRange)
            {
                if (_unfogRange != 0)
                {
                    StrategicMap.Current.FogArea(X, Y, MapLevel, _unfogRange);
                }

                _unfogRange = (byte)range;
                if (X != -1)
                {
                    if (_unfogRange != 0)
                    {
                        StrategicMap.Current.UnFogArea(X, Y, MapLevel, _unfogRange);
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
    public AddUnitsResult CanAddUnits(UnitList units)
    {
        AddUnitsResult result;
        int I;
        int transportCount;
        Troop transport;
        transportCount = 0;
        I = 0;
        int last = units.GetCount() - 1;
        for (; I <= last; I += 1)
        {
            if (units.GetUnit(I).GetTransporter())
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
                    I = 0;
                    int limit = units.GetCount() - 1;
                    for (; I <= limit; I += 1)
                    {
                        if (units.GetUnit(I).GetTransporter())
                        {
                            transport = units.GetUnit(I);
                        }
                    }
                }

                if (units.GetCount() + GetCount() <= transport.GetTransportCapacity() + 1)
                {
                    result = AddUnitsResult.Ok;
                }
                else
                {
                    result = AddUnitsResult.TooManyUnits;
                }
            }
            else
            {
                if (units.GetCount() + GetCount() <= 8)
                {
                    I = 0;
                    // { Original repeats the list count test before testing each candidate. }
                    if (I != units.GetCount())
                    {
                        while (I != units.GetCount() && CanAddUnit(units.GetUnit(I)))
                        {
                            I += 1;
                        }
                    }

                    if (I != units.GetCount())
                    {
                        result = AddUnitsResult.InvalidMoveType;
                    }
                    else
                    {
                        result = AddUnitsResult.Ok;
                    }
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
    public AddUnitsResult CanAddUnitSelection(UnitList units, byte selection)
    {
        AddUnitsResult result;
        DelphiList<Troop> list;
        int I;
        int transportCount;
        Troop transport;
        list = new DelphiList<Troop>();
        I = 0;
        int last = units.GetCount() - 1;
        for (; I <= last; I += 1)
        {
            if ((selection & 1) != 0)
            {
                list.Add(units.GetUnit(I));
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        transportCount = 0;
        I = 0;
        int limit = list.Count - 1;
        for (; I <= limit; I += 1)
        {
            if (list[I].GetTransporter())
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
                    I = 0;
                    int last2 = list.Count - 1;
                    for (; I <= last2; I += 1)
                    {
                        if (list[I].GetTransporter())
                        {
                            transport = list[I];
                        }
                    }
                }

                var count = GetCount();
                var transportCapacity = transport.GetTransportCapacity();
                if (list.Count + count <= transportCapacity + 1)
                {
                    result = AddUnitsResult.Ok;
                }
                else
                {
                    result = AddUnitsResult.TooManyUnits;
                }
            }
            else
            {
                var count = GetCount();
                if (list.Count + count <= 8)
                {
                    I = 0;
                    while (I != list.Count && CanAddUnit(list[I]))
                    {
                        I += 1;
                    }

                    if (I != list.Count)
                    {
                        result = AddUnitsResult.InvalidMoveType;
                    }
                    else
                    {
                        result = AddUnitsResult.Ok;
                    }
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
    public AddUnitsResult AddUnitSelection(UnitList units, byte selection)
    {
        UnitUserList list;
        AddUnitsResult result = CanAddUnitSelection(units, selection);
        if (result == AddUnitsResult.Ok)
        {
            units.BeginUpdate();
            list = new UnitUserList(null) { Holder = CreationHolder() };
            for (int I = units.GetCount() - 1; I >= 0; I--)
            {
                if (((1 << I) & selection) != 0)
                {
                    if (!units.GetUnit(I).GetTransporter())
                    {
                        list.AddUnit(units.GetUnit(I));
                    }
                    else
                    {
                        AddUnit(units.GetUnit(I));
                    }
                }
            }

            for (int I = list.GetCount() - 1; I >= 0; I--)
            {
                AddUnit(list.GetUnit(I));
            }

            units.EndUpdate();
            list?.Dispose();
        }

        return result;
    }

    // Army.PAS:1881
    public Troop SelectionUnit(byte selection, int index)
    {
        int I;
        Troop result = null;
        I = 0;
        while (I != Count && result == null)
        {
            if ((selection & 1) != 0)
            {
                if (index == 0)
                {
                    result = this[I];
                }

                index -= 1;
            }

            selection = (byte)(int)(((uint)selection) >> 1);
            I += 1;
        }

        return result;
    }

    // Army.PAS:1919
    public void MakeIndependent(byte selection, ref ArmyHSList armies)
    {
        sbyte X;
        sbyte Y;
        sbyte L;
        Army goodArmy;
        Army evilArmy;
        DelphiList<Army> races;
        DelphiList<Troop> units;
        int Race;
        Troop troop;
        Alignment alignment;
        ArmyHexSprite armyHexSprite;
        ArmyHexSprite PlaceIndependentArmy(sbyte x, sbyte y, sbyte l, Army army)
        {
            ArmyHexSprite result;
            try
            {
                if (army.GetCount() != 0)
                {
                    result = new ArmyHexSprite(null) { Holder = CreationHolder() };
                    result.Army.BeginUpdate();
                    result.Army.AddUnits(army);
                    if (result.Place(X, Y, L, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
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

        BeginUpdate();
        goodArmy = null;
        evilArmy = null;
        races = new DelphiList<Army>();
        units = new DelphiList<Troop>();
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                units.Add(this[I]);
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }

        for (int I = units.Count - 1; I >= 0; I--)
        {
            troop = units.List[I];
            if (troop.GetRace() != -1 && !troop.GetAbilityEnabled(107))
            {
                if (troop.GetRace() >= races.Count)
                {
                    races.Count = troop.GetRace() + 1;
                }

                var race = troop.GetRace();
                if (races.List[race] == null)
                {
                    var army = new Army(null) { Holder = CreationHolder() };
                    var aUnitRace = troop.GetRace();
                    races.List[aUnitRace] = army;
                }

                var race2 = troop.GetRace();
                races.List[race2].AddUnit(troop);
                units.Delete(I);
            }
        }

        for (int I = units.Count - 1; I >= 0; I--)
        {
            troop = units.List[I];
            alignment = troop.GetAlignment();
            Race = -1;
            for (int J = races.Count - 1; J >= 0; J--)
            {
                if (races.List[J] != null && GamePack.Current.RaceResources.GetRaceResource(J).Alignments.Contains(alignment))
                {
                    // { Original chooses the largest compatible race group. }
                    if (Race == -1 || races.List[Race].GetCount() <= races.List[J].GetCount())
                    {
                        Race = J;
                    }
                }
            }

            if (Race != -1)
            {
                races.List[Race].AddUnit(troop);
                units.Delete(I);
            }
            else
            {
                if (AlignmentTables.GoodAlignments.Contains(alignment) && (!AlignmentTables.EvilAlignments.Contains(alignment) || (goodArmy != null && evilArmy == null) || (goodArmy == null && evilArmy == null) || goodArmy != null && evilArmy != null && goodArmy.GetCount() >= evilArmy.GetCount()))
                {
                    if (goodArmy == null)
                    {
                        goodArmy = new Army(null) { Holder = CreationHolder() };
                    }

                    goodArmy.AddUnit(troop);
                    units.Delete(I);
                }
                else
                {
                    if (evilArmy == null)
                    {
                        evilArmy = new Army(null) { Holder = CreationHolder() };
                    }

                    evilArmy.AddUnit(troop);
                    units.Delete(I);
                }
            }
        }

        X = this.X;
        Y = this.Y;
        L = MapLevel;
        EndUpdate();
        for (int I = 0, last = races.Count - 1; I <= last; I++)
        {
            if (races.List[I] != null)
            {
                armyHexSprite = PlaceIndependentArmy(X, Y, L, races.List[I]);
                if (armies != null && armyHexSprite != null)
                {
                    armies.AddChild(armyHexSprite);
                }

                races.List[I]?.Dispose();
                races.List[I] = null;
            }
        }

        if (goodArmy != null)
        {
            armyHexSprite = PlaceIndependentArmy(X, Y, L, goodArmy);
            if (armies != null && armyHexSprite != null)
            {
                armies.AddChild(armyHexSprite);
            }

            goodArmy?.Dispose();
        }

        if (evilArmy != null)
        {
            armyHexSprite = PlaceIndependentArmy(X, Y, L, evilArmy);
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
        int I;
        int unitRace;
        Troop troop;
        Alignment Alignment;
        if (Player == 0)
        {
            race = -1;
            valid = true;
            I = Count;
            while (valid && I != 0)
            {
                I -= 1;
                if (!this[I].GetAbilityEnabled(107))
                {
                    unitRace = this[I].GetRace();
                }
                else
                {
                    unitRace = -1;
                }

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
                if (X != -1)
                {
                    race = IndependentRelation;
                    IndependentRelation = -1;
                    IndependentRelation = (sbyte)StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel).GetIndependentRelation();
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

            I = Count;
            while (valid && I != 0)
            {
                I -= 1;
                bool condition = race == this[I].GetRace();
                if (!condition)
                {
                    var alignment = this[I].GetAlignment();
                    condition = GamePack.Current.RaceResources.GetRaceResource(race).Alignments.Contains(alignment);
                }

                valid = condition;
            }

            if (!valid)
            {
                evil = true;
                good = true;
                neutral = true;
                I = 0;
                int last = Count - 1;
                for (; I <= last; I += 1)
                {
                    troop = this[I];
                    Alignment = troop.GetAlignment();
                    evil = evil && AlignmentTables.EvilAlignments.Contains(Alignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Units.Alignment.Evil, Units.Alignment.PureEvil).Contains(Alignment));
                    good = good && AlignmentTables.GoodAlignments.Contains(Alignment) && (troop.GetRace() == -1 || Set<Alignment>.Of(Units.Alignment.PureGood, Units.Alignment.Good).Contains(Alignment));
                    neutral = neutral && AlignmentTables.NeutralAlignments.Contains(Alignment);
                }

                if (good && !evil)
                {
                    IndependentRelation = -2;
                }
                else
                {
                    if (evil && !good)
                    {
                        IndependentRelation = -4;
                    }
                    else
                    {
                        IndependentRelation = -3;
                    }
                }
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
        log._player = Player;
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
        if (count == 1)
        {
            log.Text = AbilityRules.TranslateResourceString(GetEngine(), s_oneUnitDesertedText);
        }
        else
        {
            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsDesertedFormat), [IntToStr(count)]);
        }

        if (StrategicMap.Current.Players.GetPlayers(Player).Gems == 0 && StrategicMap.Current.Players.GetPlayers(Player).GetUpkeep() > StrategicMap.Current.Players.GetPlayers(Player).Income)
        {
            if (count == 1)
            {
                var desertUnitNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitNoUpkeepText);
                log.Messages.Text = desertUnitNoUpkeep;
            }
            else
            {
                var desertUnitsNoUpkeep = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsNoUpkeepText);
                log.Messages.Text = desertUnitsNoUpkeep;
            }
        }
        else
        {
            if (ValidateSelection(255) == selection)
            {
                if (count == 1)
                {
                    var desertUnitLowMorale = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitLowMoraleText);
                    log.Messages.Text = desertUnitLowMorale;
                }
                else
                {
                    var desertUnitsLowMorale = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsLowMoraleText);
                    log.Messages.Text = desertUnitsLowMorale;
                }
            }
            else
            {
                if (count == 1)
                {
                    var desertUnitLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitLowMoraleSplitText);
                    log.Messages.Text = desertUnitLowMoraleSplit;
                }
                else
                {
                    var desertUnitsLowMoraleSplit = AbilityRules.TranslateResourceString(GetEngine(), s_desertUnitsLowMoraleSplitText);
                    log.Messages.Text = desertUnitsLowMoraleSplit;
                }
            }
        }

        EventLog.DistributeLocationEventLog(log, -1, X, Y, MapLevel, true);
        log.Release();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            bit = 1 << I;
            if ((bit & selection) != 0 && GetUnit(I).GetAbilityEnabled(155))
            {
                units.Add(GetUnit(I));
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

        for (int I = 0, last = armies.GetCount() - 1; I <= last; I++)
        {
            if (AbilityRules.DiplomaticRelation(0, armies.GetArmyHexSprite(I).Army.IndependentRelation, player, 0) == DiplomaticRelation.War)
            {
                armies.GetArmyHexSprite(I).Army.SetBehavior(255, GroupBehavior.Raid);
            }
            else
            {
                armies.GetArmyHexSprite(I).Army.SetBehavior(255, GroupBehavior.Scout);
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                troop = this[I];
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

        if (predictor.Execute() == CombatResult.Defeat)
        {
            if (predictor.GetSuperiority() < 50)
            {
                result = PartyStatus.Unruly;
            }
            else
            {
                result = PartyStatus.Unrest;
            }
        }
        else
        {
            if (count == 0 || morale <= 60)
            {
                result = PartyStatus.Stable;
            }
            else
            {
                if (morale <= 80)
                {
                    result = PartyStatus.Content;
                }
                else
                {
                    result = PartyStatus.Cheerful;
                }
            }
        }

        predictor?.Dispose();
        return result;
    }

    // Army.PAS:2185
    public void UpdateDesertion()
    {
        PartyStatus status;
        int chance;
        byte selection;
        if (X != -1 && Player != 0)
        {
            status = GetPartyStatus(255);
            if (Set<PartyStatus>.Of(PartyStatus.Unruly, PartyStatus.Unrest).Contains(status))
            {
                if (status == PartyStatus.Unrest)
                {
                    chance = 10;
                }
                else
                {
                    chance = 50;
                }

                if (chance > StrategicMap.Current.Random(100))
                {
                    selection = 0;
                    for (int I = Count - 1; I >= 0; I--)
                    {
                        selection = (byte)(selection << 1);
                        if (this[I].MoraleValue <= 40)
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
        int I;
        I = GetCount();
        bool result = false;
        while (I != 0 && !result)
        {
            I -= 1;
            if (this[I].GetAbilityEnabled(101))
            {
                result = true;
            }
        }

        if (result)
        {
            BeginUpdate();
            I = GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                if (this[I].GetAbilityEnabled(101))
                {
                    this[I].ValidateCrusader();
                }
            }

            result = GetCount() != 0;
            EndUpdate();
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
            if (X != -1 && Player > 0)
            {
                var visibilityRange = VisibilityRange();
                StrategicMap.Current.ExploreArea(X, Y, MapLevel, Player, visibilityRange);
                var visibilityRange2 = VisibilityRange();
                var trueVisionRange = TrueVisionRange();
                StrategicMap.Current.UpdatePlayerSeen(Player, X, Y, MapLevel, visibilityRange2, trueVisionRange);
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
                    if (Player == 0 && GetJoinSelection(player) != 0)
                    {
                        result = MoveAction.Negotiate;
                    }
                    else
                    {
                        result = MoveAction.Attack;
                    }
                }
            }
            else
            {
                if (Player == 0 && GetJoinSelection(player) != 0)
                {
                    result = MoveAction.Negotiate;
                }
                else
                {
                    result = MoveAction.Attack;
                }
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
            for (int I = Count - 1; I >= 0; I--)
            {
                result = (byte)(result << 1);
                troop = this[I];
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
        int I;
        byte result = 0;
        if (player > 0)
        {
            wantsJoin = false;
            I = Count;
            while (!wantsJoin && I != 0)
            {
                I -= 1;
                wantsJoin = this[I].OfferToJoin(player);
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
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                result += this[I].JoinAmount(player);
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
            log.SetupJoinArmy(this, log._player, JoinMode.Default);
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
        int I;
        int J;
        int direction;
        int X = default;
        int Y = default;
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
                    BeginUpdate();
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
                            var name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitJoinedForcesFormat), [name]);
                            log.Messages.Text = format;
                        }
                        else
                        {
                            log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsJoinedFormat), [IntToStr(joinCount)]);
                            var selectionCount = SelectionCount(selection);
                            var name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_unitsJoinedForcesFormat), [selectionCount, name]);
                            log.Messages.Text = format;
                        }
                    }
                    else
                    {
                        log.Text = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_joinedFormat), [hero.HeroName]);
                        var name = StrategicMap.Current.Players.GetPlayers(offer.Player).GetName();
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), s_joinedForcesFormat), [hero.HeroName, name]);
                        log.Messages.Text = format;
                    }

                    EventLog.DistributeLocationEventLog(log, -1, this.X, this.Y, MapLevel, false);
                    log.Release();
                    if (ValidateSelection(255) != selection)
                    {
                        locations = new MapCoordinateList();
                        if (offer.ArmyHexSprite != null)
                        {
                            var hexY = offer.ArmyHexSprite.GetHexY();
                            var hexX = offer.ArmyHexSprite.GetHexX();
                            direction = HexGeometry.DeltaHexToHexNumberFast(this.X, this.Y, hexX, hexY);
                            if (direction > 0 && direction <= 6)
                            {
                                locations.AddMapCoordinate(this.X, this.Y, MapLevel);
                                side = direction + 1;
                                if (side > 6)
                                {
                                    side = 1;
                                }

                                // { Original sign extends the wrapped byte coordinates here. }
                                HexGeometry.CenterHexNumberToHex((sbyte)offer.X, (sbyte)offer.Y, side, ref X, ref Y);
                                locations.AddMapCoordinate((sbyte)X, (sbyte)Y, MapLevel);
                                side = direction - 1;
                                if (side == 0)
                                {
                                    side = 6;
                                }

                                HexGeometry.CenterHexNumberToHex((sbyte)offer.X, (sbyte)offer.Y, side, ref X, ref Y);
                                locations.AddMapCoordinate((sbyte)X, (sbyte)Y, MapLevel);
                            }
                        }

                        J = 0;
                        I = Count;
                        while (I != 0)
                        {
                            I -= 1;
                            if ((selection & (1 << I)) != 0)
                            {
                                placed = false;
                                while (!placed && J < locations.Count)
                                {
                                    var item = locations[J];
                                    var locationsItem = locations[J];
                                    var item2 = locations[J];
                                    placed = GetUnit(I).Place(item2.X, locationsItem.Y, item.MapLevel, offer.Player, Set<UnitPlaceModeElement>.Of()) != null;
                                    if (!placed)
                                    {
                                        J += 1;
                                    }
                                }

                                if (!placed)
                                {
                                    GetUnit(I).Place(this.X, this.Y, MapLevel, offer.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                                }
                            }
                        }

                        locations?.Dispose();
                    }
                    else
                    {
                        SetPlayer(offer.Player);
                    }

                    EndUpdate();
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
        log._player = GetArmyHexSprite().Army.Player;
        var fireDamage = AbilityRules.TranslateResourceString(GetEngine(), s_fireDamageText);
        log.ArmyMessages.Add(fireDamage);
        log.ArmyIcon = 0;
        var armyHexSprite = GetArmyHexSprite();
        log.ArmyView.Setup(armyHexSprite.Army, 255);
        BeginUpdate();
        damaged = false;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            troop = GetUnit(I);
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
            EventLog.DistributeLocationEventLog(log, -1, X, Y, MapLevel, true);
        }

        log.Release();
        Validate();
        EndUpdate();
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

            for (int I = Count - 1; I >= 0; I--)
            {
                troop = GetUnit(I);
                if (troop.Player != 0 || troop.Group != null && troop.Group.GroupClass.Behavior != GroupBehavior.Passive)
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
        log._player = player;
        // { Original appends the storm message. }
        var stormType = AbilityRules.TranslateResourceString(GetEngine(), GameTables.StormTypeTexts[(int)storm]);
        log.ArmyMessages.Add(stormType);
        log.ArmyIcon = 2;
        log.ArmyView.Setup(this, 255);
        BeginUpdate();
        damaged = false;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            damage = this[I].ExecuteStormDamage(storm, distance);
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
            EventLog.DistributeLocationEventLog(log, Player, X, Y, MapLevel, true);
        }

        log.Release();
        Validate();
        EndUpdate();
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
                        if (AbilityRules.ValidAttackRelation(info.Player, info.IndependentRelation, Player, IndependentRelation) || info.Mode == AITargetMode.Raid)
                        {
                            if (info.Player != 0 && Leader(255) != null)
                            {
                                data.Priority = 300;
                            }
                            else
                            {
                                if (info.Player != 0 && Hero(255, Set<HeroType>.Of(HeroType.Hero)) != null)
                                {
                                    data.Priority = 100;
                                }
                                else
                                {
                                    data.Priority = 50;
                                }
                            }
                        }
                        else
                        {
                            data.Priority = 0;
                        }
                    }
                    else
                    {
                        if (Player == 0)
                        {
                            selection = GetJoinSelection(info.Player);
                            if (selection != 0)
                            {
                                var joinAmount = JoinAmount(info.Player, selection);
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
                if (!GetUnitsConcealedForPlayer(255, info.Player) && info.Player != Player)
                {
                    data.Priority = 50;
                }
                else
                {
                    data.Priority = 0;
                }

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
        for (int I = Count - 1; I >= 0; I--)
        {
            troop = this[I];
            troop.UnitStatus = troop.UnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.MoveDone);
        }
    }

    // Army.PAS:2623
    public bool CanCombine(Army army, byte selection)
    {
        bool result;
        int I;
        int maxCount;
        byte temporary;
        Troop ownTransporter;
        Troop otherTransporter;
        selection = army.ValidateSelection(selection);
        I = 0;
        temporary = selection;
        while (temporary != 0)
        {
            if ((temporary & 1) != 0)
            {
                I += 1;
            }

            temporary = (byte)(int)(((uint)temporary) >> 1);
        }

        ownTransporter = Transporter(255);
        if (ownTransporter != null)
        {
            maxCount = ownTransporter.GetTransportCapacity() + 1;
        }
        else
        {
            maxCount = 8;
        }

        I += Count;
        if (I <= maxCount)
        {
            otherTransporter = army.Transporter(selection);
            if (otherTransporter != null && (I > otherTransporter.GetTransportCapacity() + 1 || ownTransporter != null))
            {
                result = false;
            }
            else
            {
                result = true;
            }
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
        if (player == Player || player == -1 || X == -1) return false;
        var field = StrategicMap.Current.MapContainer.GetField(X, Y, MapLevel);
        return GetUnit(index).ConcealedOnMapField(field) && GetConcealedForPlayer(player);
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
