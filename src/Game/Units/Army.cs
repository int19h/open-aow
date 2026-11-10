// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Ai;
using Aow.Game.Ai.Groups;

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

// FStatus. FMode. FPlayer. location. FIndependentRelation. FUnfogRange. FUpkeep. FOnUpdate.
// FOldAIBehavior. destination. FUserMode. FVisibilityRanges. FConcealedUnits
public partial class Army : UnitList
{
    public new const int ClassID = 131606;

    // Army.PAS:324
    public Army(ECustomNode owner) : base(owner)
    {
        FX = -1;
        FY = -1;
        FL = -1;
        FDestinationX = -1;
        FDestinationY = -1;
        FDestinationL = -1;
        FIndependentRelation = -1;
    }

    public Set<ArmyStatusElement> FStatus;
    public Set<ArmyModeElement> FMode;
    public sbyte FPlayer;
    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public sbyte FIndependentRelation;
    public byte FUnfogRange;
    public byte FUpkeep;
    public NotifyEvent<object> FOnUpdate;
    public GroupBehavior FOldAIBehavior;
    public sbyte FDestinationX;
    public sbyte FDestinationY;
    public sbyte FDestinationL;
    public Set<ArmyUserModeElement> FUserMode;
    public byte FVisibilityRanges;
    public byte FConcealedUnits;

    // Army.PAS:336
    public override void Dispose()
    {
        if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Destroying))
        {
            AoWHSMap.Current.ArmyDestroyed(this);
        }

        FOnUpdate = null;
        base.Dispose();
    }

    // Army.PAS:588
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // Army.PAS:1898
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 4);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 5);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 6);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 7);
        if (!Stream.StorageMode().Contains(StorageModeElement.Write) || AoWHSMap.Current == null || AoWHSMap.Current.FConnectionType != (PlayerConnectionType)1)
        {
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationX), 8);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationY), 9);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDestinationL), 10);
            RwSet(ref FUserMode, 1, 13, Stream.rwData, 0);
        }

        Stream.rwData(Bytes.Of(ref FOldAIBehavior, 1), 1, 11);
        RwSet(ref FMode, 1, 12, Stream.rwData, 0);
        if (Stream.StorageMode().Contains(StorageModeElement.Read) && FMode.Contains(ArmyModeElement.Moving))
        {
            FMode = FMode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnsignedByte(persister, "Player", ref FPlayer);
        PersistSupport.UnsignedByte(persister, "X", ref FX);
        PersistSupport.UnsignedByte(persister, "Y", ref FY);
        PersistSupport.UnsignedByte(persister, "L", ref FL);
        if (persister.Loading || AoWHSMap.Current == null || AoWHSMap.Current.FConnectionType != (PlayerConnectionType)1)
        {
            PersistSupport.UnsignedByte(persister, "DestinationX", ref FDestinationX);
            PersistSupport.UnsignedByte(persister, "DestinationY", ref FDestinationY);
            PersistSupport.UnsignedByte(persister, "DestinationL", ref FDestinationL);
            persister.Set("UserMode", ref FUserMode);
        }

        persister.Enum("OldAIBehavior", ref FOldAIBehavior);
        persister.Set("Mode", ref FMode);
        if (persister.Loading && FMode.Contains(ArmyModeElement.Moving))
        {
            FMode = FMode - Set<ArmyModeElement>.Of(ArmyModeElement.Moving);
        }
    }

    // Army.PAS:2259
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        if (Msg.MsgID == 537001987)
        {
            NewTurnDone((sbyte)Msg.Payload.Int);
        }

        if (Msg.MsgID == 537001986)
        {
            LockChanged();
        }

        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            UnlockChanged();
        }

        if (Msg.MsgID == 537001989)
        {
            SetPlayer(FPlayer);
        }
        else
        {
            if (Msg.MsgID == 537001990 && AoWHSMap.Current.FSeatedPlayer != -1)
            {
                UpdateUnfog();
                UpdateConcealment();
            }
            else
            {
                if (Msg.MsgID == 537001992)
                {
                    UpdateVisibilityRanges();
                    UpdateExploration();
                    if (AoWHSMap.Current.FSeatedPlayer != -1)
                    {
                        UpdateUnfog();
                        UpdateConcealment();
                    }
                }
                else
                {
                    if (Msg.MsgID == 537001986)
                    {
                        NewTurn((sbyte)Msg.Payload.Int);
                    }
                    else
                    {
                        if (Msg.MsgID == 537001985)
                        {
                            NewDay();
                        }
                        else
                        {
                            if (Msg.MsgID == 537001987 && Msg.Payload.Int == FPlayer)
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
    public override int RemoveChild(EObject Child)
    {
        BeginUpdate();
        var unit = Child;
        unit.AddRef();

        int Result = base.RemoveChild(Child);
        var unit2 = Child;
        unit2.Release();

        EndUpdate();
        return Result;
    }

    // Army.PAS:1853
    public override int AddChild(EObject Child)
    {
        int Result;
        if (!FStatus.Contains(ArmyStatusElement.Activated) || ((AbstractUnit)Child).CanActivate())
        {
            Result = base.AddChild(Child);
            if (Result != -1)
            {
                if (FStatus.Contains(ArmyStatusElement.Activated) && !((AbstractUnit)Child).FUnitStatus.Contains(UnitStatusElement.Activated))
                {
                    ((AbstractUnit)Child).Activate();
                }

                ((AbstractUnit)Child).SetPlayer(FPlayer);
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Army.PAS:716
    public override void Changed()
    {
        Update();
    }

    // Army.PAS:1241
    public override sbyte GetPlayer() => FPlayer;

    // Army.PAS:1874
    public override void GetLocation(ref sbyte Xhx, ref sbyte Yhx, ref sbyte Lhx)
    {
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            Xhx = FX;
            Yhx = FY;
            Lhx = FL;
        }
        else
        {
            Xhx = -1;
            Yhx = -1;
            Lhx = -1;
        }
    }

    // Army.PAS:1719
    public override bool CanAddUnit(AbstractUnit AUnit)
    {
        bool Result;
        bool HasTransporter;
        MapField Field;
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            Result = GetCount() < MaxSize() && AUnit.CanAddToList(this);
            HasTransporter = IndexOfTransporter(255) != -1;
            if (Result && !HasTransporter && FX != -1 && FX != 0 && !FMode.Contains(ArmyModeElement.DisableTerrainValidation))
            {
                // { Original skips terrain validation on the zero X column. }
                Field = AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
                Result = AUnit.ValidTerrain(Field.TerrainType, Field.OverlayType);
            }

            if (Result && AUnit.GetTransporter())
            {
                Result = !HasTransporter && AUnit.GetTransportCapacity() >= GetCount();
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:1836
    public override bool CanDeleteUnit(AbstractUnit AUnit)
    {
        bool Result;
        if (IndexOfTransporter(255) != -1)
        {
            if (AUnit.GetTransporter() && UnitsInTransporter() != 0)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:1868
    public override bool AddUnit(AbstractUnit AUnit)
    {
        AUnit.SetOwner(this);
        return this == AUnit.Owner;
    }

    // Army.PAS:1770
    public override bool AddUnits(UnitList Units)
    {
        bool Result = CanAddUnits(Units) == AddUnitsResult.OK;
        if (Result)
        {
            BeginUpdate();
            for (int I = Units.GetCount() - 1; I >= 0; I--)
            {
                AddUnit(Units.GetUnit(I));
            }

            EndUpdate();
        }

        return Result;
    }

    // Army.PAS:1269
    public virtual bool GetConcealedForPlayer(sbyte Player)
    {
        bool Result;
        MapLevel Level;
        AoWMapField Field;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        if (Player != -1)
        {
            Result = true;
            Level = AoWHSMap.Current.MapContainer.MapLevels[FL];
            Msg.MsgID = 4363;
            Msg.Result = 0;
            Msg.Param[1] = Player;
            HNWalkData.InitHNWalk(ref Walk, FX, FY, 0);
            while (Result && Walk.Radius < 8)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                    if ((Field.FFieldFlags & 4096) != 0)
                    {
                        Msg.Param[0] = Walk.Radius;
                        Field.SendMapFieldMsg(ref Msg);
                        Result = Msg.Result.Int == 0;
                    }
                }

                HNWalkData.WalkToNextHN(ref Walk);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:343
    public void Kill(byte Selection)
    {
        BeginUpdate();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            if ((((int)(((uint)Selection) >> I)) & 1) != 0)
            {
                ((AbstractUnit)FChildren.List[I]).Killed();
            }
        }

        EndUpdate();
    }

    // Army.PAS:352
    public int GetStrengthEx(byte Selection, DamageType DamageType)
    {
        int Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            if (((1 << I) & Selection) != 0)
            {
                Result += ((AbstractUnit)FChildren.List[I]).GetStrength(DamageType);
            }
        }

        return Result;
    }

    // Army.PAS:361
    public int GetStrengthMaxEx(byte Selection, Set<DamageType> DamageTypes)
    {
        int Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            if (((1 << I) & Selection) != 0)
            {
                Result += ((AbstractUnit)FChildren.List[I]).GetStrengthMax(DamageTypes);
            }
        }

        return Result;
    }

    // Army.PAS:370
    public void ReturnBattleFieldInfo(ref BattleFieldInfo Info)
    {
        int Strength;
        AbstractUnit AUnit;
        DiplomaticRelation Relation;
        GroupBehavior Behavior;
        bool IncludeUnit;
        Relation = AbilityRules.DiplomaticRelation(Info.Player, Info.IndependentRelation, FPlayer, FIndependentRelation);
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)FChildren.List[I];
            if (AUnit.FGroup != null)
            {
                Behavior = AUnit.FGroup.GroupClass.Behavior;
            }
            else
            {
                Behavior = GroupBehavior.Passive;
            }

            IncludeUnit = Info.Units == null || Info.Units.FChildren.IndexOf(AUnit) == -1;
            if (IncludeUnit && Info.Requests.Contains(BattleFieldInfoRequestsElement.IgnorePassiveIndependents))
            {
                IncludeUnit = AUnit.FPlayer != 0 || AUnit.FGroup != null && AUnit.FGroup.GroupClass.Behavior != GroupBehavior.Passive && AUnit.FGroup.GroupClass.Behavior != GroupBehavior.Guard;
            }

            if (IncludeUnit && Info.Requests.Contains(BattleFieldInfoRequestsElement.IgnoreIndependents) && AUnit.FPlayer == 0)
            {
                IncludeUnit = false;
            }

            if (IncludeUnit)
            {
                Strength = AUnit.GetStrengthMax(Info.DamageTypes);
                if (Info.FriendlyRelations.Contains((byte)(int)Relation))
                {
                    Info.FriendlyStrength += Strength;
                }

                if (Info.EnemyRelations.Contains((byte)(int)Relation) || Behavior == GroupBehavior.GuardArea)
                {
                    Info.Strength += Strength;
                }
            }
        }
    }

    // Army.PAS:394
    public bool HealUnits(byte Selection)
    {
        AbstractUnit AUnit;
        bool Result = false;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                if (AUnit.GetHitPoints() != AUnit.GetHits())
                {
                    Result = true;
                    AUnit.SetHitPoints(AUnit.GetHits());
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // Army.PAS:410
    public int CreateGroupID(byte Selection)
    {
        int Result;
        int I;
        I = 0;
        while ((Selection & 1) == 0 && I != FChildren.Count)
        {
            I += 1;
            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (I != FChildren.Count)
        {
            Result = ((AbstractUnit)FChildren.List[I]).FUnitID;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Army.PAS:423
    public AIGroup GetGroup(byte Selection)
    {
        bool Different;
        int I;
        bool Found;
        AIGroup Result = null;
        Different = false;
        I = 0;
        Found = false;
        while (I != FChildren.Count && !Different)
        {
            if ((Selection & 1) != 0)
            {
                if (!Found)
                {
                    Result = ((AbstractUnit)FChildren.List[I]).FGroup;
                    Found = true;
                }
                else
                {
                    Different = Result != ((AbstractUnit)FChildren.List[I]).FGroup;
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        if (!Found || Different)
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:445
    public void SetGroup(byte Selection, AIGroup Group)
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                Group.AddUnit((AbstractUnit)FChildren.List[I]);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }
    }

    // Army.PAS:455
    public byte GetGroupSelection(int GroupID)
    {
        byte Result = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            Result = (byte)(Result << 1);
            if (GroupID == ((AbstractUnit)FChildren.List[I]).FGroupID)
            {
                Result = (byte)(Result | 1);
            }
        }

        return Result;
    }

    // Army.PAS:466
    public GroupBehavior GetBehavior(byte Selection)
    {
        GroupBehavior Result;
        AIGroup Group;
        Group = GetGroup(Selection);
        if (Group != null)
        {
            Result = Group.GroupClass.Behavior;
        }
        else
        {
            Result = GroupBehavior.None;
        }

        return Result;
    }

    // Army.PAS:473
    public void SetBehavior(byte Selection, GroupBehavior Behavior)
    {
        AIGroup Group;
        Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FPlayer, FIndependentRelation, Behavior);
        if (Group != null)
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if ((Selection & 1) != 0)
                {
                    Group.AddUnit((AbstractUnit)FChildren.List[I]);
                }

                Selection = (byte)(int)(((uint)Selection) >> 1);
            }

            Group.Release();
        }
    }

    // Army.PAS:488
    public GroupBehavior GetOldAIBehavior()
    {
        int I;
        GroupBehavior Result = GroupBehavior.None;
        I = GetCount();
        while (Result == GroupBehavior.None && I != 0)
        {
            I -= 1;
            Result = (GroupBehavior)GetUnit(I).F_AbstractUnit_38;
        }

        return Result;
    }

    // Army.PAS:500
    public void InitializeGroupBehavior()
    {
        byte Selection;
        int Index;
        GroupBehavior Behavior;
        Hero AHero;
        Selection = 255;
        AHero = Hero(Selection, Set<HeroType>.Of(HeroType.Leader, HeroType.HeroLeader));
        if (AHero != null)
        {
            Index = IndexOf(AHero);
            if (Index != -1)
            {
                SetBehavior((byte)(1 << Index), GroupBehavior.Fortify);
                Selection = (byte)(Selection & ~(1 << Index));
            }
        }

        Index = IndexOfTransporter(Selection);
        if (Index != -1 && FOldAIBehavior == GroupBehavior.None)
        {
            Selection = (byte)(Selection & ~(1 << Index));
        }

        if (Selection != 0)
        {
            Behavior = FOldAIBehavior;
            if (Behavior == GroupBehavior.None)
            {
                if (FPlayer == 0 && AoWHSMap.Current.GetIndependentAIMode() != AoWHSMap.IndependentAIMode.Off)
                {
                    if (FX != -1 && AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindNoneTransparentHS(typeof(PlayerStructure)) == null)
                    {
                        if (FIndependentRelation >= 0)
                        {
                            Behavior = GroupBehavior.Patrol;
                        }
                        else
                        {
                            Behavior = GroupBehavior.GuardArea;
                        }
                    }
                    else
                    {
                        Behavior = GroupBehavior.Passive;
                    }
                }
                else
                {
                    Behavior = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.GetRandomGroupBehavior();
                }
            }

            SetBehavior(Selection, Behavior);
        }
    }

    // Army.PAS:533
    public void UpdateGroupBehavior()
    {
        byte Selection;
        int Index;
        GroupBehavior Behavior;
        Selection = GetGroupSelection(0);
        if (Selection != 0)
        {
            Index = IndexOfTransporter(Selection);
            if (Index != -1)
            {
                Selection = (byte)(Selection & ~(1 << Index));
                if (((AbstractUnit)FChildren.List[Index]).FGroupID == 0)
                {
                    SetBehavior((byte)(1 << Index), GroupBehavior.Transport);
                }
            }

            Behavior = GroupBehavior.Passive;
            if (FPlayer == 0 && Hero(Selection, Set<HeroType>.Of(HeroType.Hero)) != null)
            {
                Behavior = GroupBehavior.Scout;
            }
            else
            {
                if (FPlayer == 0 && AoWHSMap.Current.GetIndependentAIMode() != AoWHSMap.IndependentAIMode.Off && FX != -1)
                {
                    if (AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindNoneTransparentHS(typeof(PlayerStructure)) == null)
                    {
                        if (FIndependentRelation >= 0)
                        {
                            Behavior = GroupBehavior.Patrol;
                        }
                        else
                        {
                            Behavior = GroupBehavior.GuardArea;
                        }
                    }
                }
            }

            if (Selection != 0)
            {
                Index = IndexOfBehavior(Behavior);
                if (Index == -1)
                {
                    SetBehavior(Selection, Behavior);
                }
                else
                {
                    SetGroup(Selection, ((AbstractUnit)FChildren.List[Index]).FGroup);
                }
            }
        }
    }

    // Army.PAS:560
    public ArmyID CreateID(byte Selection)
    {
        ArmyID ID;
        ID = new ArmyID(null);
        ID.FX = FX;
        ID.FY = FY;
        ID.FL = FL;
        ID.FPlayer = FPlayer;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                ID.FUnitIDs.Add(((AbstractUnit)FChildren.List[I]).FUnitID);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return ID;
    }

    // Army.PAS:579
    public ArmyHS GetArmyHS()
    {
        ArmyHS Result;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        GetLocation(ref X, ref Y, ref L);
        if (X != -1)
        {
            Result = (ArmyHS)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(131607);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:591
    public void AssignSettings(Army Army)
    {
        FDestinationX = Army.FDestinationX;
        FDestinationY = Army.FDestinationY;
        FDestinationL = Army.FDestinationL;
        FPlayer = Army.FPlayer;
    }

    // Army.PAS:599
    public void SetFullyConcealed(bool Value)
    {
        if (Value != FStatus.Contains(ArmyStatusElement.FullyConcealed))
        {
            if (Value)
            {
                FStatus = FStatus + Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed);
            }
            else
            {
                FStatus = FStatus - Set<ArmyStatusElement>.Of(ArmyStatusElement.FullyConcealed);
            }

            if (FStatus.Contains(ArmyStatusElement.Activated) && FX != -1)
            {
                AoWHSMap.Current.ScannerIconsChangedRad(FX, FY, FL, 0);
            }
        }
    }

    // Army.PAS:610
    public bool GetFullyConcealed() => FStatus.Contains(ArmyStatusElement.FullyConcealed);

    // Army.PAS:613
    public void UpdateConcealment()
    {
        bool Concealed;
        Leader ALeader;
        bool Changed;
        bool ShowCrown;
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            if (FConcealedUnits != 0 && AoWHSMap.Current.FSeatedPlayer != FPlayer && !AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
            {
                Concealed = GetConcealedForPlayer(AoWHSMap.Current.FSeatedPlayer);
            }
            else
            {
                Concealed = false;
            }

            ALeader = Leader(255);
            ShowCrown = ALeader != null;
            if (ShowCrown && Concealed)
            {
                var indexOf = IndexOf(ALeader);
                if ((((int)(((uint)FConcealedUnits) >> indexOf)) & 1) != 0)
                {
                    ShowCrown = false;
                }
            }

            Changed = false;
            if (ShowCrown)
            {
                FStatus = FStatus + Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown);
            }
            else
            {
                FStatus = FStatus - Set<ArmyStatusElement>.Of(ArmyStatusElement.ShowCrown);
            }

            if (Concealed != FStatus.Contains(ArmyStatusElement.Concealed))
            {
                if (Concealed)
                {
                    FStatus = FStatus + Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed);
                }
                else
                {
                    FStatus = FStatus - Set<ArmyStatusElement>.Of(ArmyStatusElement.Concealed);
                }

                Changed = true;
            }

            Concealed = Concealed && GetCount() == ConcealedUnitsCount();
            if (Concealed != GetFullyConcealed())
            {
                SetFullyConcealed(Concealed);
                Changed = true;
            }

            if (Changed)
            {
                TriggerOnUpdateEvent();
            }
        }
    }

    // Army.PAS:639
    public void SetXYL(sbyte X, sbyte Y, sbyte L)
    {
        FX = X;
        FY = Y;
        FL = L;
        if (UpdateCount == 0)
        {
            UpdateConcealedUnits();
        }
    }

    // Army.PAS:647
    public int GetRace(byte Selection)
    {
        bool Same;
        int Race;
        int Result = -1;
        Same = true;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                Race = ((AbstractUnit)FChildren.List[I]).GetRace();
                if (Race != -1)
                {
                    if (Result != -1)
                    {
                        Same = Same && Race == Result;
                    }
                    else
                    {
                        Result = Race;
                    }
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (!Same)
        {
            Result = -1;
        }

        return Result;
    }

    // Army.PAS:666
    public byte GetUpkeep() => FUpkeep;

    // Army.PAS:669
    public void ResetDestinationPos()
    {
        FDestinationX = -1;
        FDestinationY = -1;
        FDestinationL = -1;
    }

    // Army.PAS:672
    public void SetDestinationPos(sbyte X, sbyte Y, sbyte L)
    {
        FDestinationX = X;
        FDestinationY = Y;
        FDestinationL = L;
    }

    // Army.PAS:675
    public int ConcealedUnitsCount()
    {
        byte Selection;
        Selection = FConcealedUnits;
        int Result = 0;
        while (Selection != 0)
        {
            if ((Selection & 1) != 0)
            {
                Result += 1;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // Army.PAS:687
    public void UpdateConcealedUnits()
    {
        int I;
        AoWMapField Field;
        if (FX != -1 && FL != -1 && FStatus.Contains(ArmyStatusElement.Activated))
        {
            FConcealedUnits = 0;
            I = FChildren.Count;
            Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
            while (I != 0)
            {
                I -= 1;
                FConcealedUnits = (byte)(FConcealedUnits << 1);
                if (((AbstractUnit)FChildren.List[I]).ConcealedOnMapF(Field))
                {
                    FConcealedUnits = (byte)(FConcealedUnits | 1);
                }
            }
        }
    }

    // Army.PAS:705
    public void UpdateUpkeep()
    {
        byte Upkeep;
        byte Previous;
        int Delta;
        Upkeep = 0;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            Upkeep = (byte)(Upkeep + ((AbstractUnit)FChildren.List[I]).GetUpkeep());
        }

        Previous = FUpkeep;
        if (Previous != Upkeep && FStatus.Contains(ArmyStatusElement.Activated))
        {
            Delta = Upkeep - Previous;
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetArmyUpkeep(players.FArmyUpkeep + Delta);
        }

        FUpkeep = Upkeep;
    }

    // Army.PAS:719
    public void ValidateTerrainEx(sbyte TerrainType, sbyte OverlayType, sbyte Player)
    {
        AbstractUnit Transport;
        byte Selection;
        ArmyEventLog Log;
        AbstractUnit AUnit;
        Transport = Transporter(255);
        if (Transport == null || !Transport.ValidTerrain(TerrainType, OverlayType))
        {
            Selection = 0;
            for (int I = GetCount() - 1; I >= 0; I--)
            {
                Selection = (byte)(Selection << 1);
                if (!((AbstractUnit)FChildren.List[I]).ValidTerrain(TerrainType, OverlayType))
                {
                    Selection = (byte)(Selection | 1);
                }
            }
        }
        else
        {
            Selection = 0;
        }

        if (Selection != 0)
        {
            if (!AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && AoWHSMap.Current.FDay != 0)
            {
                BeginUpdate();
                Log = new ArmyEventLog(null);
                Log.FPlayer = Player;
                if (Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)TerrainType))
                {
                    Log.FArmyText = AbilityRules.TranslateRStr(UnitsDrownRStr);
                    var unitsDrown = AbilityRules.TranslateRStr(UnitsDrownRStr);
                    Log.FArmyMessages.Add(unitsDrown);
                    Log.FArmyIcon = 3;
                }
                else
                {
                    switch (TerrainType)
                    {
                        case 9:
                            Log.FArmyText = AbilityRules.TranslateRStr(UnitsBurnedRStr);
                            var unitsBurned = AbilityRules.TranslateRStr(UnitsBurnedRStr);
                            Log.FArmyMessages.Add(unitsBurned);
                            Log.FArmyIcon = 0;
                            break;
                        default:
                            Log.FArmyText = AbilityRules.TranslateRStr(UnitsCrushedRStr);
                            var aoWETranslateRStr = AbilityRules.TranslateRStr(UnitsCrushedRStr);
                            Log.FArmyMessages.Add(aoWETranslateRStr);
                            Log.FArmyIcon = 2;
                            break;
                    }
                }

                Log.FArmyView.Setup(this, 255);
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    AUnit = GetUnit(I);
                    if ((((int)(((uint)Selection) >> I)) & 1) != 0)
                    {
                        var hitPoints = AUnit.GetHitPoints();
                        Log.FDamageValues.Add((byte)hitPoints);
                        Log.FDamageTypes.Add(9);
                        AUnit.SetHitPoints(0);
                    }
                    else
                    {
                        Log.FDamageValues.Add(0);
                        Log.FDamageTypes.Add(9);
                    }
                }

                Log.ValidateGameOver();
                EventLog.DistributeLocationEventLog(Log, FPlayer, FX, FY, FL, true);
                Log.Release();
                EndUpdate();
            }

            Validate();
        }
    }

    // Army.PAS:785
    public void ValidateTerrain(sbyte TerrainType, sbyte OverlayType)
    {
        ValidateTerrainEx(TerrainType, OverlayType, FPlayer);
    }

    // Army.PAS:790
    public void Validate()
    {
        AbstractUnit AUnit;
        Unit NewUnit;
        AoWHSMap.Current.LockGameOverEvent();
        BeginUpdate();
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)FChildren.List[I];
            if (AUnit.GetHitPoints() == 0)
            {
                if (AUnit.GetAbilityEnabled(107))
                {
                    NewUnit = new Unit(null);
                    NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(152));
                    AUnit.Killed();
                    AddUnit(NewUnit);
                }
                else
                {
                    AUnit.Killed();
                }
            }
        }

        EndUpdate();
        AoWHSMap.Current.UnlockGameOverEvent();
    }

    // Army.PAS:812
    public void UpdateFormation()
    {
        int Level;
        int UnitLevel;
        AbstractUnit AUnit;
        string SourceName = "";
        Level = 0;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)FChildren.List[I];
            AUnit.UpdateMoraleValue();
            if (AUnit.GetAbilityEnabled(46) || AUnit.GetAbilityLevel(46) > 0)
            {
                ((LeadershipAbility)AoWHSSet.Current.FAbilityControl.GetAbility(46)).ResetExternalSource(AUnit);
                UnitLevel = AUnit.GetAbilityLevel(46);
                if (UnitLevel > Level)
                {
                    Level = UnitLevel;
                    SourceName = AUnit.GetAbilityOwner(46).GetOwnerName();
                    if (SourceName == AbilityRules.TranslateRStr(ArmyInherentRStr))
                    {
                        SourceName = AUnit.GetName();
                    }
                }
            }
        }

        if (Level > 0)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                if (AUnit.GetAbilityLevel(46) < Level)
                {
                    ((LeadershipAbility)AoWHSSet.Current.FAbilityControl.GetAbility(46)).SetExternalSource(AUnit, (byte)Level, SourceName);
                }
            }
        }
    }

    // Army.PAS:841
    public void Update()
    {
        if (UpdateCount == 0 && FStatus.Contains(ArmyStatusElement.Activated))
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
            if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting) && FX != -1)
            {
                AoWHSMap.Current.ScannerIconsChangedRad(FX, FY, FL, 0);
            }

            UnlockChanged();
            TriggerOnUpdateEvent();
        }
    }

    // Army.PAS:861
    public void TriggerOnUpdateEvent()
    {
        if (UpdateCount == 0 && FStatus.Contains(ArmyStatusElement.Activated))
        {
            AoWHSMap.Current.ArmyChanged(this);
            if (FOnUpdate != null)
            {
                FOnUpdate(this);
            }
        }
    }

    // Army.PAS:870
    public void Activate()
    {
        AbstractUnit AUnit;
        Assert(!FStatus.Contains(ArmyStatusElement.Activated), "Army already activated", "Army.PAS", 1171);
        if (FOldAIBehavior == GroupBehavior.None)
        {
            FOldAIBehavior = GetOldAIBehavior();
        }

        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)FChildren.List[I];
            if (!AUnit.FUnitStatus.Contains(UnitStatusElement.Activated))
            {
                AUnit.Activate();
            }
        }

        FStatus = FStatus + Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        FUpkeep = 0;
        if (FPlayer != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FArmies.AddChild(this);
        }

        // { Original treats X = 0 as an unset location. }
        if (FX == 0)
        {
            FX = -1;
            FY = -1;
            FL = -1;
        }

        Update();
    }

    // Army.PAS:1186
    public void Deactivate()
    {
        AbstractUnit AUnit;
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            if (FUnfogRange != 0)
            {
                AoWHSMap.Current.FogArea(FX, FY, FL, FUnfogRange);
                FUnfogRange = 0;
            }

            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                if (AUnit.FUnitStatus.Contains(UnitStatusElement.Activated))
                {
                    AUnit.Deactivate();
                }
            }

            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FArmies.RemoveChild(this);
                var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetArmyUpkeep(players.FArmyUpkeep - FUpkeep);
            }

            FStatus = FStatus - Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        }
    }

    // Army.PAS:1211
    public byte ValidateSelection(byte Selection) => (byte)(Selection & AbilityRules.ArmySelectionMasks[FChildren.Count]);

    // Army.PAS:1216
    public int SelectionCount(byte Selection)
    {
        Selection = (byte)(Selection & AbilityRules.ArmySelectionMasks[FChildren.Count]);
        int Result = 0;
        while (Selection != 0)
        {
            if ((Selection & 1) != 0)
            {
                Result += 1;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // Army.PAS:1227
    public byte ValidateMoveSelection(byte Selection)
    {
        byte Result = ValidateSelection(Selection);
        if (IndexOfTransporter(255) != -1 && (UnitsInTransporter() & ~Selection) != 0 && Transporter(Selection) != null)
        {
            var transporter = Transporter(255);
            Result = (byte)(Result & ~(1 << FChildren.IndexOf(transporter)));
        }

        return Result;
    }

    // Army.PAS:1235
    public void Select()
    {
        FStatus = FStatus + Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);
    }

    // Army.PAS:1238
    public void Unselect()
    {
        FStatus = FStatus - Set<ArmyStatusElement>.Of(ArmyStatusElement.Selected);
    }

    // Army.PAS:1244
    public bool GetUnitConcealed(int Index)
    {
        AoWMapField Field;
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
        return ((AbstractUnit)FChildren.List[Index]).ConcealedOnMapF(Field);
    }

    // Army.PAS:1262
    public bool GetUnitsConcealedForPlayer(byte Selection, sbyte Player)
    {
        bool Result;
        Selection = ValidateSelection(Selection);
        if (Selection == (FConcealedUnits & Selection))
        {
            Result = GetConcealedForPlayer(Player);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:1298
    public void SetPlayer(sbyte Player)
    {
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player) == null)
        {
            Player = 0;
        }

        if (Player != FPlayer)
        {
            BeginUpdate();
            if (FStatus.Contains(ArmyStatusElement.Activated))
            {
                if (FPlayer != -1)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetArmyUpkeep(players.FArmyUpkeep - FUpkeep);
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FArmies.RemoveChild(this);
                }

                FPlayer = Player;
                if (FPlayer != -1)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).SetArmyUpkeep(players.FArmyUpkeep + FUpkeep);
                    AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FArmies.AddChild(this);
                }

                for (int I = FChildren.Count - 1; I >= 0; I--)
                {
                    ((AbstractUnit)FChildren.List[I]).SetPlayer(FPlayer);
                }

                if (FX != -1)
                {
                    AoWHSMap.Current.ScannerIconsChangedRad(FX, FY, FL, 0);
                }

                if (FPlayer > 0 && AoWHSMap.Current.FDay > 0)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                    {
                        SetBehavior(255, GroupBehavior.Passive);
                    }
                }

                UpdateUnfog();
                UpdateExploration();
            }
            else
            {
                FPlayer = Player;
            }

            EndUpdate();
        }
    }

    // Army.PAS:1332
    public Set<DamageType> ImmunityTypes(byte Selection)
    {
        int I;
        Set<DamageType> Result = AbilityRules.dtAll;
        I = 0;
        while (Result != Set<DamageType>.Of() && I != GetCount())
        {
            if ((Selection & 1) != 0)
            {
                Result = Result & ((AbstractUnit)FChildren.List[I]).GetImmunityTypes();
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        return Result;
    }

    // Army.PAS:1346
    public int ObtainValue(byte Selection)
    {
        int Result = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                Result += ((AbstractUnit)FChildren.List[I]).GetObtainValue();
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // Army.PAS:1357
    public sbyte MovePoints(byte Selection)
    {
        sbyte Result;
        AbstractUnit Transport;
        AbstractUnit AUnit;
        Transport = Transporter(Selection);
        if (Transport != null)
        {
            Result = Transport.GetMovePoints();
        }
        else
        {
            if (Selection != 0)
            {
                Result = 50;
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if ((Selection & 1) != 0)
                    {
                        AUnit = (AbstractUnit)FChildren.List[I];
                        if (AUnit.GetMovePoints() < Result)
                        {
                            Result = AUnit.GetMovePoints();
                        }
                    }

                    Selection = (byte)(int)(((uint)Selection) >> 1);
                }
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // Army.PAS:1378
    public sbyte Moves(byte Selection)
    {
        sbyte Result;
        AbstractUnit Transport;
        AbstractUnit AUnit;
        Transport = Transporter(Selection);
        if (Transport != null)
        {
            Result = Transport.GetMoves();
        }
        else
        {
            if (Selection != 0)
            {
                Result = 50;
                for (int I = 0, last = GetCount() - 1; I <= last; I++)
                {
                    if ((Selection & 1) != 0)
                    {
                        AUnit = (AbstractUnit)FChildren.List[I];
                        if (AUnit.GetMoves() < Result)
                        {
                            Result = AUnit.GetMoves();
                        }
                    }

                    Selection = (byte)(int)(((uint)Selection) >> 1);
                }
            }
            else
            {
                Result = 0;
            }
        }

        return Result;
    }

    // Army.PAS:1399
    public Set<MoveType> MoveTypes(byte Selection)
    {
        Set<MoveType> Result;
        AbstractUnit Transport;
        Transport = Transporter(Selection);
        if (Transport != null)
        {
            Result = Transport.GetMoveTypes();
        }
        else
        {
            Result = Set<MoveType>.Of();
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((Selection & 1) != 0)
                {
                    Result = Result + ((AbstractUnit)FChildren.List[I]).GetMoveTypes();
                }

                Selection = (byte)(int)(((uint)Selection) >> 1);
            }
        }

        return Result;
    }

    // Army.PAS:1416
    public Set<MoveType> GetPartyMoves(byte Selection)
    {
        Set<MoveType> Result = default;
        AbstractUnit Transport;
        Set<MoveType> Common;
        Set<MoveType> Moves;
        int MoveType;
        Transport = Transporter(Selection);
        if (Transport == null)
        {
            Common = Set<MoveType>.Of().WithRange(World.Movement.MoveType.Walking, World.Movement.MoveType.Tunneling);
            MoveType = (int)World.Movement.MoveType.Flying;
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if (((1 << I) & Selection) != 0)
                {
                    Moves = ((AbstractUnit)FChildren.List[I]).GetMoveTypes();
                    Common = Common & Moves;
                    if (MoveType != -1)
                    {
                        if ((MoveType)MoveType == World.Movement.MoveType.Flying)
                        {
                            if (!Moves.Contains(World.Movement.MoveType.Flying))
                            {
                                if (Moves.Contains(World.Movement.MoveType.Swimming))
                                {
                                    MoveType = (int)World.Movement.MoveType.Swimming;
                                }
                                else
                                {
                                    MoveType = (int)World.Movement.MoveType.Walking;
                                }
                            }
                        }
                        else
                        {
                            if ((MoveType)MoveType == World.Movement.MoveType.Swimming)
                            {
                                if ((Set<MoveType>.Of(World.Movement.MoveType.Flying, World.Movement.MoveType.Swimming) & Moves) == Set<MoveType>.Of())
                                {
                                    MoveType = (int)World.Movement.MoveType.Walking;
                                }
                            }
                            else
                            {
                                if ((MoveType)MoveType == World.Movement.MoveType.Walking)
                                {
                                    if ((Set<MoveType>.Of(World.Movement.MoveType.Walking, World.Movement.MoveType.Flying).WithRange(World.Movement.MoveType.Forestry, World.Movement.MoveType.LavaCrossing) & Moves) == Set<MoveType>.Of())
                                    {
                                        MoveType = -1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // { Original leaves Result uninitialized when the units share no movement mode. }
            if (MoveType != -1)
            {
                Result = Set<MoveType>.Of((MoveType)MoveType);
            }

            Result = Result + Common;
        }
        else
        {
            Result = Transport.GetMoveTypes();
        }

        return Result;
    }

    // Army.PAS:1450
    public int MoveOnMovePoints(byte Selection, sbyte TerrainType, sbyte OverlayType)
    {
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 UnitTable = default;
        AbstractUnit Transport;
        FillBytes(Bytes.Of(ref Table, 256), 1);
        Transport = Transporter(Selection);
        if (Transport != null)
        {
            Transport.CreateMovePointTable(ref Table);
        }
        else
        {
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((Selection & 1) != 0)
                {
                    ((AbstractUnit)FChildren.List[I]).CreateMovePointTable(ref UnitTable);
                    AbilityRules.MaximizeMovePointTable(Bytes.Of(ref Table), Bytes.Of(ref UnitTable));
                }

                Selection = (byte)(int)(((uint)Selection) >> 1);
            }
        }

        return Table[TerrainType][OverlayType - -1];
    }

    // Army.PAS:1469
    public void CreateMovePointTable(byte Selection, Span<byte> Table)
    {
        int Count;
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 UnitTable = default;
        AbstractUnit Transport;
        Count = 0;
        FillBytes(Table.Slice(0, 256), 1);
        Transport = Transporter(Selection);
        if (Transport != null)
        {
            Transport.CreateMovePointTable(ref MemoryMarshal.Cast<byte, FixedArray_Aow_Units_FixedArray_sbyte_16_16>(Table)[0]);
            Count += 1;
        }
        else
        {
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                if ((Selection & 1) != 0)
                {
                    ((AbstractUnit)FChildren.List[I]).CreateMovePointTable(ref UnitTable);
                    AbilityRules.MaximizeMovePointTable(Table, Bytes.Of(ref UnitTable));
                    Count += 1;
                }

                Selection = (byte)(int)(((uint)Selection) >> 1);
            }
        }

        if (Count == 0)
        {
            FillBytes(Table.Slice(0, 256), -1);
        }
    }

    // Army.PAS:1494
    public bool ValidTerrainEx(byte Selection, sbyte TerrainType, sbyte OverlayType)
    {
        bool Result;
        int I;
        AbstractUnit Transport;
        Transport = Transporter(Selection);
        if (Transport == null)
        {
            I = 0;
            Result = true;
            while (I != GetCount() && Result)
            {
                if (((1 << I) & Selection) != 0)
                {
                    Result = ((AbstractUnit)FChildren.List[I]).ValidTerrain(TerrainType, OverlayType);
                }

                I += 1;
            }
        }
        else
        {
            Result = Transport.ValidTerrain(TerrainType, OverlayType);
        }

        return Result;
    }

    // Army.PAS:1512
    public bool ValidTerrain(sbyte TerrainType, sbyte OverlayType)
    {
        bool Result;
        int I;
        AbstractUnit Transport;
        Transport = Transporter(255);
        if (Transport == null)
        {
            I = 0;
            Result = true;
            while (I != GetCount() && Result)
            {
                Result = ((AbstractUnit)FChildren.List[I]).ValidTerrain(TerrainType, OverlayType);
                I += 1;
            }
        }
        else
        {
            Result = Transport.ValidTerrain(TerrainType, OverlayType);
        }

        return Result;
    }

    // Army.PAS:1529
    public bool CanPlace(sbyte X, sbyte Y, sbyte L)
    {
        MapField Field;
        int I;
        Field = AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        bool Result = ValidTerrain(Field.TerrainType, Field.OverlayType);
        I = 0;
        if (!FStatus.Contains(ArmyStatusElement.Activated))
        {
            while (Result && I != FChildren.Count)
            {
                Result = ((AbstractUnit)FChildren.List[I]).CanActivate();
                I += 1;
            }
        }

        return Result;
    }

    // Army.PAS:1543
    public void MovedTo(MapField OldField, MapField NewField, int MaxCost)
    {
        AbstractUnit AUnit;
        BeginUpdate();
        FUserMode = FUserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.Guard);
        AUnit = Transporter(255);
        if (AUnit == null)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                ((AbstractUnit)FChildren.List[I]).MovedTo(OldField, NewField, MaxCost);
            }
        }
        else
        {
            AUnit.MovedTo(OldField, NewField, MaxCost);
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                if (!AUnit.GetTransporter())
                {
                    AUnit.MovedTo(OldField, NewField, 0);
                }
            }
        }

        UpdateExploration();
        EndUpdate();
    }

    // Army.PAS:1565
    public int MaxSize()
    {
        int Capacity;
        int Result = 8;
        for (int I = 0, last = GetCount() - 1; I <= last; I++)
        {
            Capacity = ((AbstractUnit)FChildren.List[I]).GetTransportCapacity();
            if (Capacity != 0 && Result > Capacity + 1)
            {
                Result = Capacity + 1;
            }
        }

        return Result;
    }

    // Army.PAS:1576
    public Hero Hero(byte Selection, Set<HeroType> Types)
    {
        int I;
        AbstractUnit AUnit;
        I = 0;
        Hero Result = null;
        while (I != GetCount() && Result == null)
        {
            if ((Selection & 1) != 0)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                if (AUnit is Hero && Types.Contains(((Hero)AUnit).GetHeroType()))
                {
                    Result = (Hero)AUnit;
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        return Result;
    }

    // Army.PAS:1593
    public Leader Leader(byte Selection)
    {
        int I;
        I = 0;
        Leader Result = null;
        while (I != GetCount() && Result == null)
        {
            if ((Selection & 1) != 0)
            {
                if (((EObject)FChildren.List[I]).EClass.ClassID == 131634)
                {
                    Result = (Leader)FChildren.List[I];
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        return Result;
    }

    // Army.PAS:1608
    public AbstractUnit Transporter(byte Selection)
    {
        int I;
        I = 0;
        AbstractUnit Result = null;
        while (I != GetCount() && Result == null)
        {
            if ((Selection & 1) != 0)
            {
                if (((AbstractUnit)FChildren.List[I]).GetTransporter())
                {
                    Result = (AbstractUnit)FChildren.List[I];
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        return Result;
    }

    // Army.PAS:1623
    public int IndexOfTransporter(byte Selection)
    {
        int Result = GetCount();
        while (Result != 0)
        {
            Result -= 1;
            if (((1 << Result) & Selection) != 0 && ((AbstractUnit)FChildren.List[Result]).GetTransporter())
            {
                return Result;
            }
        }

        return -1;
    }

    // Army.PAS:1634
    public byte UnitsInTransporter()
    {
        byte Result;
        int I;
        MapField Field;
        if (FX != -1)
        {
            Field = AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
            I = 0;
            Result = 0;
            while (I != GetCount())
            {
                if (!((AbstractUnit)FChildren.List[I]).ValidTerrain(Field.TerrainType, Field.OverlayType))
                {
                    Result = (byte)(Result + (1 << I));
                }

                I += 1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Army.PAS:1652
    public void UpdateVisibilityRanges()
    {
        byte TrueVision;
        byte Vision;
        byte Range;
        Vision = 0;
        TrueVision = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            Range = (byte)((AbstractUnit)FChildren.List[I]).VisibilityRange();
            if (Range > Vision)
            {
                Vision = Range;
            }

            Range = (byte)((AbstractUnit)FChildren.List[I]).TrueVisionRange();
            if (Range > TrueVision)
            {
                TrueVision = Range;
            }
        }

        FVisibilityRanges = (byte)((TrueVision << 4) + Vision);
    }

    // Army.PAS:1667
    public byte VisibilityRange() => (byte)(FVisibilityRanges & 15);

    // Army.PAS:1670
    public byte TrueVisionRange() => (byte)(int)(((uint)FVisibilityRanges) >> 4);

    // Army.PAS:1673
    public void TurnUnFogOff()
    {
        byte Range;
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            Range = FUnfogRange;
            if (Range != 0)
            {
                AoWHSMap.Current.FogArea(FX, FY, FL, Range);
                FUnfogRange = 0;
            }
        }
    }

    // Army.PAS:1687
    public void UpdateExploration()
    {
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            for (int Player = 1, last = AoWHSMap.Current.FPlayers.GetCount() - 1; Player <= last; Player++)
            {
                if (Player == FPlayer || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.Alliance)
                {
                    var visibilityRange = VisibilityRange();
                    AoWHSMap.Current.ExploreArea(FX, FY, FL, (sbyte)Player, visibilityRange);
                }
            }
        }
    }

    // Army.PAS:1697
    public void UpdateUnfog()
    {
        int Range;
        if (FStatus.Contains(ArmyStatusElement.Activated))
        {
            if (AoWHSMap.Current.FSeatedPlayer != -1 && (AoWHSMap.Current.FSeatedPlayer == FPlayer || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FDiplomaticRelations.GetDiplomaticRelation(AoWHSMap.Current.FSeatedPlayer) == DiplomaticRelation.Alliance))
            {
                Range = VisibilityRange();
            }
            else
            {
                Range = 0;
            }

            if (Range != FUnfogRange)
            {
                if (FUnfogRange != 0)
                {
                    AoWHSMap.Current.FogArea(FX, FY, FL, FUnfogRange);
                }

                FUnfogRange = (byte)Range;
                if (FX != -1)
                {
                    if (FUnfogRange != 0)
                    {
                        AoWHSMap.Current.UnFogArea(FX, FY, FL, FUnfogRange);
                    }
                }
                else
                {
                    FUnfogRange = 0;
                }
            }
        }
    }

    // Army.PAS:1739
    public AddUnitsResult CanAddUnits(UnitList Units)
    {
        AddUnitsResult Result;
        int I;
        int TransportCount;
        AbstractUnit Transport;
        TransportCount = 0;
        I = 0;
        int last = Units.GetCount() - 1;
        for (; I <= last; I += 1)
        {
            if (Units.GetUnit(I).GetTransporter())
            {
                TransportCount += 1;
            }
        }

        if (IndexOfTransporter(255) != -1)
        {
            TransportCount += 1;
        }

        if (TransportCount <= 1)
        {
            if (TransportCount == 1)
            {
                Transport = Transporter(255);
                if (Transport == null)
                {
                    I = 0;
                    int limit = Units.GetCount() - 1;
                    for (; I <= limit; I += 1)
                    {
                        if (Units.GetUnit(I).GetTransporter())
                        {
                            Transport = Units.GetUnit(I);
                        }
                    }
                }

                if (Units.GetCount() + GetCount() <= Transport.GetTransportCapacity() + 1)
                {
                    Result = AddUnitsResult.OK;
                }
                else
                {
                    Result = AddUnitsResult.TooManyUnits;
                }
            }
            else
            {
                if (Units.GetCount() + GetCount() <= 8)
                {
                    I = 0;
                    // { Original repeats the list count test before testing each candidate. }
                    if (I != Units.GetCount())
                    {
                        while (I != Units.GetCount() && CanAddUnit(Units.GetUnit(I)))
                        {
                            I += 1;
                        }
                    }

                    if (I != Units.GetCount())
                    {
                        Result = AddUnitsResult.InvalidMoveType;
                    }
                    else
                    {
                        Result = AddUnitsResult.OK;
                    }
                }
                else
                {
                    Result = AddUnitsResult.TooManyUnits;
                }
            }
        }
        else
        {
            Result = AddUnitsResult.MultipleTransporters;
        }

        return Result;
    }

    // Army.PAS:1782
    public AddUnitsResult CanAddUnitSelection(UnitList Units, byte Selection)
    {
        AddUnitsResult Result;
        DelphiList List;
        int I;
        int TransportCount;
        AbstractUnit Transport;
        List = new DelphiList();
        I = 0;
        int last = Units.GetCount() - 1;
        for (; I <= last; I += 1)
        {
            if ((Selection & 1) != 0)
            {
                List.Add(Units.GetUnit(I));
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        TransportCount = 0;
        I = 0;
        int limit = List.Count - 1;
        for (; I <= limit; I += 1)
        {
            if (((AbstractUnit)List[I]).GetTransporter())
            {
                TransportCount += 1;
            }
        }

        if (IndexOfTransporter(255) != -1)
        {
            TransportCount += 1;
        }

        if (TransportCount <= 1)
        {
            if (TransportCount == 1)
            {
                Transport = Transporter(255);
                if (Transport == null)
                {
                    I = 0;
                    int last2 = List.Count - 1;
                    for (; I <= last2; I += 1)
                    {
                        if (((AbstractUnit)List[I]).GetTransporter())
                        {
                            Transport = (AbstractUnit)List[I];
                        }
                    }
                }

                var count = GetCount();
                var transportCapacity = Transport.GetTransportCapacity();
                if (List.Count + count <= transportCapacity + 1)
                {
                    Result = AddUnitsResult.OK;
                }
                else
                {
                    Result = AddUnitsResult.TooManyUnits;
                }
            }
            else
            {
                var count = GetCount();
                if (List.Count + count <= 8)
                {
                    I = 0;
                    while (I != List.Count && CanAddUnit((AbstractUnit)List[I]))
                    {
                        I += 1;
                    }

                    if (I != List.Count)
                    {
                        Result = AddUnitsResult.InvalidMoveType;
                    }
                    else
                    {
                        Result = AddUnitsResult.OK;
                    }
                }
                else
                {
                    Result = AddUnitsResult.TooManyUnits;
                }
            }
        }
        else
        {
            Result = AddUnitsResult.MultipleTransporters;
        }

        List?.Dispose();
        return Result;
    }

    // Army.PAS:1818
    public AddUnitsResult AddUnitSelection(UnitList Units, byte Selection)
    {
        UnitUserList List;
        AddUnitsResult Result = CanAddUnitSelection(Units, Selection);
        if (Result == AddUnitsResult.OK)
        {
            Units.BeginUpdate();
            List = new UnitUserList(null);
            for (int I = Units.GetCount() - 1; I >= 0; I--)
            {
                if (((1 << I) & Selection) != 0)
                {
                    if (!Units.GetUnit(I).GetTransporter())
                    {
                        List.AddUnit(Units.GetUnit(I));
                    }
                    else
                    {
                        AddUnit(Units.GetUnit(I));
                    }
                }
            }

            for (int I = List.GetCount() - 1; I >= 0; I--)
            {
                AddUnit(List.GetUnit(I));
            }

            Units.EndUpdate();
            List?.Dispose();
        }

        return Result;
    }

    // Army.PAS:1881
    public AbstractUnit SelectionUnit(byte Selection, int Index)
    {
        int I;
        AbstractUnit Result = null;
        I = 0;
        while (I != FChildren.Count && Result == null)
        {
            if ((Selection & 1) != 0)
            {
                if (Index == 0)
                {
                    Result = (AbstractUnit)FChildren.List[I];
                }

                Index -= 1;
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
            I += 1;
        }

        return Result;
    }

    // Army.PAS:1919
    public void MakeIndependent(byte Selection, ref ArmyHSList Armies)
    {
        sbyte X;
        sbyte Y;
        sbyte L;
        Army GoodArmy;
        Army EvilArmy;
        DelphiList Races;
        DelphiList Units;
        int Race;
        AbstractUnit AUnit;
        Alignment Alignment;
        ArmyHS ArmyHS;
        ArmyHS PlaceIndependentArmy(sbyte AX, sbyte AY, sbyte AL, Army Army)
        {
            ArmyHS Result;
            try
            {
                if (Army.GetCount() != 0)
                {
                    Result = new ArmyHS(null);
                    Result.FArmy.BeginUpdate();
                    Result.FArmy.AddUnits(Army);
                    if (Result.PlaceEx(X, Y, L, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
                    {
                        Result.Activate();
                        Result.FArmy.EndUpdate();
                    }
                    else
                    {
                        Result.FArmy.Kill(255);
                        Result?.Dispose();
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }
            catch
            {
                // { Original suppresses placement failures without releasing a partly built army. }
                Result = null;
            }

            return Result;
        }

        BeginUpdate();
        GoodArmy = null;
        EvilArmy = null;
        Races = new DelphiList();
        Units = new DelphiList();
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                Units.Add(FChildren.List[I]);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        for (int I = Units.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)Units.List[I];
            if (AUnit.GetRace() != -1 && !AUnit.GetAbilityEnabled(107))
            {
                if (AUnit.GetRace() >= Races.Count)
                {
                    Races.Count = AUnit.GetRace() + 1;
                }

                var race = AUnit.GetRace();
                if (Races.List[race] == null)
                {
                    var army = new Army(null);
                    var aUnitRace = AUnit.GetRace();
                    Races.List[aUnitRace] = army;
                }

                var race2 = AUnit.GetRace();
                ((UnitList)Races.List[race2]).AddUnit(AUnit);
                Units.Delete(I);
            }
        }

        for (int I = Units.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)Units.List[I];
            Alignment = AUnit.GetAlignment();
            Race = -1;
            for (int J = Races.Count - 1; J >= 0; J--)
            {
                if (Races.List[J] != null && AoWHSSet.Current.FRaceResources.GetRaceResource(J).FAlignments.Contains(Alignment))
                {
                    // { Original chooses the largest compatible race group. }
                    if (Race == -1 || ((ECustomNode)Races.List[Race]).GetCount() <= ((ECustomNode)Races.List[J]).GetCount())
                    {
                        Race = J;
                    }
                }
            }

            if (Race != -1)
            {
                ((UnitList)Races.List[Race]).AddUnit(AUnit);
                Units.Delete(I);
            }
            else
            {
                if (AlignmentTables.alsGood.Contains(Alignment) && (!AlignmentTables.alsEvil.Contains(Alignment) || (GoodArmy != null && EvilArmy == null) || (GoodArmy == null && EvilArmy == null) || GoodArmy != null && EvilArmy != null && GoodArmy.GetCount() >= EvilArmy.GetCount()))
                {
                    if (GoodArmy == null)
                    {
                        GoodArmy = new Army(null);
                    }

                    GoodArmy.AddUnit(AUnit);
                    Units.Delete(I);
                }
                else
                {
                    if (EvilArmy == null)
                    {
                        EvilArmy = new Army(null);
                    }

                    EvilArmy.AddUnit(AUnit);
                    Units.Delete(I);
                }
            }
        }

        X = FX;
        Y = FY;
        L = FL;
        EndUpdate();
        for (int I = 0, last = Races.Count - 1; I <= last; I++)
        {
            if (Races.List[I] != null)
            {
                ArmyHS = PlaceIndependentArmy(X, Y, L, (Army)Races.List[I]);
                if (Armies != null && ArmyHS != null)
                {
                    Armies.AddChild(ArmyHS);
                }

                ((Army)Races.List[I])?.Dispose();
                Races.List[I] = null;
            }
        }

        if (GoodArmy != null)
        {
            ArmyHS = PlaceIndependentArmy(X, Y, L, GoodArmy);
            if (Armies != null && ArmyHS != null)
            {
                Armies.AddChild(ArmyHS);
            }

            GoodArmy?.Dispose();
        }

        if (EvilArmy != null)
        {
            ArmyHS = PlaceIndependentArmy(X, Y, L, EvilArmy);
            if (Armies != null && ArmyHS != null)
            {
                Armies.AddChild(ArmyHS);
            }

            EvilArmy?.Dispose();
        }

        Units?.Dispose();
        Races?.Dispose();
    }

    // Army.PAS:2029
    public void UpdateIndependentRelation()
    {
        sbyte Race;
        bool Valid;
        bool Evil;
        bool Good;
        bool Neutral;
        int I;
        int UnitRace;
        AbstractUnit AUnit;
        Alignment Alignment;
        if (FPlayer == 0)
        {
            Race = -1;
            Valid = true;
            I = FChildren.Count;
            while (Valid && I != 0)
            {
                I -= 1;
                if (!((AbstractUnit)FChildren.List[I]).GetAbilityEnabled(107))
                {
                    UnitRace = ((AbstractUnit)FChildren.List[I]).GetRace();
                }
                else
                {
                    UnitRace = -1;
                }

                if (UnitRace != -1 && UnitRace != Race)
                {
                    if (Race == -1)
                    {
                        Race = (sbyte)UnitRace;
                    }
                    else
                    {
                        Valid = false;
                    }
                }
            }

            if (Valid && Race == -1)
            {
                if (FX != -1)
                {
                    Race = FIndependentRelation;
                    FIndependentRelation = -1;
                    FIndependentRelation = (sbyte)((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL)).GetIndependentRelation();
                    if (FIndependentRelation >= 0)
                    {
                        Race = FIndependentRelation;
                    }
                    else
                    {
                        FIndependentRelation = Race;
                        Race = -1;
                    }
                }

                if (Race == -1)
                {
                    if (FIndependentRelation >= 0)
                    {
                        Race = FIndependentRelation;
                    }
                    else
                    {
                        Valid = false;
                    }
                }
            }

            I = FChildren.Count;
            while (Valid && I != 0)
            {
                I -= 1;
                bool condition = Race == ((AbstractUnit)FChildren.List[I]).GetRace();
                if (!condition)
                {
                    var alignment = ((AbstractUnit)FChildren.List[I]).GetAlignment();
                    condition = AoWHSSet.Current.FRaceResources.GetRaceResource(Race).FAlignments.Contains(alignment);
                }

                Valid = condition;
            }

            if (!Valid)
            {
                Evil = true;
                Good = true;
                Neutral = true;
                I = 0;
                int last = FChildren.Count - 1;
                for (; I <= last; I += 1)
                {
                    AUnit = (AbstractUnit)FChildren.List[I];
                    Alignment = AUnit.GetAlignment();
                    Evil = Evil && AlignmentTables.alsEvil.Contains(Alignment) && (AUnit.GetRace() == -1 || Set<Alignment>.Of(Units.Alignment.Evil, Units.Alignment.PureEvil).Contains(Alignment));
                    Good = Good && AlignmentTables.alsGood.Contains(Alignment) && (AUnit.GetRace() == -1 || Set<Alignment>.Of(Units.Alignment.PureGood, Units.Alignment.Good).Contains(Alignment));
                    Neutral = Neutral && AlignmentTables.alsNeutral.Contains(Alignment);
                }

                if (Good && !Evil)
                {
                    FIndependentRelation = -2;
                }
                else
                {
                    if (Evil && !Good)
                    {
                        FIndependentRelation = -4;
                    }
                    else
                    {
                        FIndependentRelation = -3;
                    }
                }
            }
            else
            {
                FIndependentRelation = Race;
            }
        }
        else
        {
            FIndependentRelation = -1;
        }
    }

    // Army.PAS:2091
    public void Desert(byte Selection)
    {
        DelphiList Units;
        sbyte Player;
        ArmyHSList Armies;
        int Count;
        int Bit;
        ArmyInfoEventLog Log;
        AbstractUnit AUnit;
        AoWHSMap.Current.LockGameOverEvent();
        Units = new DelphiList();
        Count = SelectionCount(Selection);
        Log = new ArmyInfoEventLog(null);
        Log.FArmyView.Setup(this, 255);
        Log.FSelection = Selection;
        Log.FPlayer = FPlayer;
        Log.FTitle = AbilityRules.TranslateRStr(DesertionRStr);
        Log.FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.User);
        if (AoWHSMap.Current.FPlayMode == PlayMode.Turn && GameTables.AoWEngine.FRegistry.GetClassicWindowPopupEvents().Contains(WindowPopupEvent.Rebellion))
        {
            Log.FTriggers = Log.FTriggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
            if (AoWHSMap.Current.FRequestCombatType)
            {
                Log.FTriggers = Log.FTriggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.NewTurn);
            }
        }

        if (AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous && GameTables.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents().Contains(WindowPopupEvent.Rebellion))
        {
            Log.FTriggers = Log.FTriggers + Set<ExecuteTrigger>.Of(ExecuteTrigger.Event, ExecuteTrigger.NewTurn);
        }

        Log.FDescription = AbilityRules.TranslateRStr(TheLeaderOfThePartyStepsForwardRStr);
        if (Count == 1)
        {
            Log.FText = AbilityRules.TranslateRStr(OneUnitDesertedRStr);
        }
        else
        {
            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(XUnitsDesertedRStr), [IntToStr(Count)]);
        }

        if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FGems == 0 && AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetUpkeep() > AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FIncome)
        {
            if (Count == 1)
            {
                var desertUnitNoUpkeep = AbilityRules.TranslateRStr(DesertUnitNoUpkeepRStr);
                Log.FMessages.Text = desertUnitNoUpkeep;
            }
            else
            {
                var desertUnitsNoUpkeep = AbilityRules.TranslateRStr(DesertUnitsNoUpkeepRStr);
                Log.FMessages.Text = desertUnitsNoUpkeep;
            }
        }
        else
        {
            if (ValidateSelection(255) == Selection)
            {
                if (Count == 1)
                {
                    var desertUnitLowMorale = AbilityRules.TranslateRStr(DesertUnitLowMoraleRStr);
                    Log.FMessages.Text = desertUnitLowMorale;
                }
                else
                {
                    var desertUnitsLowMorale = AbilityRules.TranslateRStr(DesertUnitsLowMoraleRStr);
                    Log.FMessages.Text = desertUnitsLowMorale;
                }
            }
            else
            {
                if (Count == 1)
                {
                    var desertUnitLowMoraleSplit = AbilityRules.TranslateRStr(DesertUnitLowMoraleSplitRStr);
                    Log.FMessages.Text = desertUnitLowMoraleSplit;
                }
                else
                {
                    var desertUnitsLowMoraleSplit = AbilityRules.TranslateRStr(DesertUnitsLowMoraleSplitRStr);
                    Log.FMessages.Text = desertUnitsLowMoraleSplit;
                }
            }
        }

        EventLog.DistributeLocationEventLog(Log, -1, FX, FY, FL, true);
        Log.Release();
        for (int I = GetCount() - 1; I >= 0; I--)
        {
            Bit = 1 << I;
            if ((Bit & Selection) != 0 && GetUnit(I).GetAbilityEnabled(155))
            {
                Units.Add(GetUnit(I));
                Selection = (byte)(Selection & ~(byte)Bit);
            }
        }

        Armies = new ArmyHSList(null);
        Player = FPlayer;
        MakeIndependent(Selection, ref Armies);
        while (Units.Count != 0)
        {
            AUnit = (AbstractUnit)Units[0];
            AUnit.AddRef();
            AUnit.Killed();
            AUnit.SetOwner(null);
            AUnit.Release();
            Units.Delete(0);
        }

        for (int I = 0, last = Armies.GetCount() - 1; I <= last; I++)
        {
            if (AbilityRules.DiplomaticRelation(0, Armies.GetArmyHS(I).FArmy.FIndependentRelation, Player, 0) == DiplomaticRelation.War)
            {
                Armies.GetArmyHS(I).FArmy.SetBehavior(255, GroupBehavior.Raid);
            }
            else
            {
                Armies.GetArmyHS(I).FArmy.SetBehavior(255, GroupBehavior.Scout);
            }
        }

        Armies?.Dispose();
        Units?.Dispose();
        AoWHSMap.Current.UnlockGameOverEvent();
    }

    // Army.PAS:2155
    public PartyStatus GetPartyStatus(byte Selection)
    {
        PartyStatus Result;
        int Morale;
        int Count;
        CombatPredictor Predictor;
        int UnitMorale;
        AbstractUnit AUnit;
        Morale = 100;
        Count = 0;
        Predictor = new CombatPredictor();
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                AUnit = (AbstractUnit)FChildren.List[I];
                UnitMorale = AUnit.FMoraleValue;
                if (AUnit.GetUnitType() != UnitType.Machine)
                {
                    if (UnitMorale < Morale)
                    {
                        Morale = UnitMorale;
                    }

                    Count += 1;
                }

                if (UnitMorale <= 40)
                {
                    Predictor.AddOpponent(AUnit, 0);
                }
                else
                {
                    Predictor.AddUnit(AUnit);
                }
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        if (Predictor.Execute() == CombatResult.Defeat)
        {
            if (Predictor.GetSuperiority() < 50)
            {
                Result = PartyStatus.Unruly;
            }
            else
            {
                Result = PartyStatus.Unrest;
            }
        }
        else
        {
            if (Count == 0 || Morale <= 60)
            {
                Result = PartyStatus.Stable;
            }
            else
            {
                if (Morale <= 80)
                {
                    Result = PartyStatus.Content;
                }
                else
                {
                    Result = PartyStatus.Cheerful;
                }
            }
        }

        Predictor?.Dispose();
        return Result;
    }

    // Army.PAS:2185
    public void UpdateDesertion()
    {
        PartyStatus Status;
        int Chance;
        byte Selection;
        if (FX != -1 && FPlayer != 0)
        {
            Status = GetPartyStatus(255);
            if (Set<PartyStatus>.Of(PartyStatus.Unruly, PartyStatus.Unrest).Contains(Status))
            {
                if (Status == PartyStatus.Unrest)
                {
                    Chance = 10;
                }
                else
                {
                    Chance = 50;
                }

                if (Chance > AoWHSMap.Current.Random(100))
                {
                    Selection = 0;
                    for (int I = FChildren.Count - 1; I >= 0; I--)
                    {
                        Selection = (byte)(Selection << 1);
                        if (((AbstractUnit)FChildren.List[I]).FMoraleValue <= 40)
                        {
                            Selection = (byte)(Selection | 1);
                        }
                    }

                    if (Selection != 0)
                    {
                        Desert(Selection);
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
        bool Result = false;
        while (I != 0 && !Result)
        {
            I -= 1;
            if (((AbstractUnit)FChildren.List[I]).GetAbilityEnabled(101))
            {
                Result = true;
            }
        }

        if (Result)
        {
            BeginUpdate();
            I = GetCount() - 1;
            for (; I >= 0; I -= 1)
            {
                if (((AbstractUnit)FChildren.List[I]).GetAbilityEnabled(101))
                {
                    ((AbstractUnit)FChildren.List[I]).ValidateCrusader();
                }
            }

            Result = GetCount() != 0;
            EndUpdate();
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:2230
    public void NewTurn(sbyte Player)
    {
        if (Player == FPlayer)
        {
            FUserMode = FUserMode - Set<ArmyUserModeElement>.Of(ArmyUserModeElement.TurnDone);
            if (ValidateCrusaders())
            {
                UpdateDesertion();
            }
        }
    }

    // Army.PAS:2239
    public void NewTurnDone(sbyte Player)
    {
        if (Player == FPlayer)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerType != PlayerType.Human)
            {
                UpdateGroupBehavior();
            }
        }
    }

    // Army.PAS:2245
    public void NewDay()
    {
        if (AoWHSMap.Current.FDay == 1)
        {
            ResetDestinationPos();
            if (FX != -1 && FPlayer > 0)
            {
                var visibilityRange = VisibilityRange();
                AoWHSMap.Current.ExploreArea(FX, FY, FL, FPlayer, visibilityRange);
                var visibilityRange2 = VisibilityRange();
                var trueVisionRange = TrueVisionRange();
                AoWHSMap.Current.UpdatePlayerSeen(FPlayer, FX, FY, FL, visibilityRange2, trueVisionRange);
            }

            InitializeGroupBehavior();
        }
    }

    // Army.PAS:2283
    public bool CanActivate() => true;

    // Army.PAS:2288
    public MoveAction GetMoveAction(sbyte Player)
    {
        MoveAction Result = MoveAction.Normal;
        if (Player != FPlayer)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType == PlayerType.Human)
            {
                if (!FStatus.Contains(ArmyStatusElement.FullyConcealed))
                {
                    if (FPlayer == 0 && GetJoinSelection(Player) != 0)
                    {
                        Result = MoveAction.Negotiate;
                    }
                    else
                    {
                        Result = MoveAction.Attack;
                    }
                }
            }
            else
            {
                if (FPlayer == 0 && GetJoinSelection(Player) != 0)
                {
                    Result = MoveAction.Negotiate;
                }
                else
                {
                    Result = MoveAction.Attack;
                }
            }
        }
        else
        {
            Result = MoveAction.Combine;
        }

        return Result;
    }

    // Army.PAS:2306
    public byte GetCanJoinSelection(sbyte Player)
    {
        byte Result;
        AbstractUnit AUnit;
        if (Player > 0)
        {
            Result = 0;
            for (int I = Count - 1; I >= 0; I--)
            {
                Result = (byte)(Result << 1);
                AUnit = (AbstractUnit)FChildren.List[I];
                if (AUnit.CanJoin(Player))
                {
                    Result = (byte)(Result | 1);
                }
            }

            if (IndexOfTransporter(255) != -1)
            {
                if (Result != ValidateSelection(255))
                {
                    Result = 0;
                }
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Army.PAS:2324
    public byte GetJoinSelection(sbyte Player)
    {
        bool WantsJoin;
        int I;
        byte Result = 0;
        if (Player > 0)
        {
            WantsJoin = false;
            I = Count;
            while (!WantsJoin && I != 0)
            {
                I -= 1;
                WantsJoin = ((AbstractUnit)FChildren.List[I]).OfferToJoin(Player);
            }

            if (WantsJoin)
            {
                Result = GetCanJoinSelection(Player);
            }
        }

        return Result;
    }

    // Army.PAS:2341
    public int JoinAmount(sbyte Player, byte Selection)
    {
        int Result = 0;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if ((Selection & 1) != 0)
            {
                Result += ((AbstractUnit)FChildren.List[I]).JoinAmount(Player);
            }

            Selection = (byte)(int)(((uint)Selection) >> 1);
        }

        return Result;
    }

    // Army.PAS:2353
    public bool IncommingNegotiateRequest(NegotiateRequestEventLog Log)
    {
        bool Result = false;
        if (FPlayer == 0)
        {
            Log.SetupJoinArmy(this, Log.FPlayer, JoinMode.Default);
            Log.FObject = NegotiateObject.Army;
            Log.FText = AbilityRules.TranslateRStr(P2PartyJoinHeaderRStr);
            Result = Log.FSelection != 0;
        }

        return Result;
    }

    // Army.PAS:2365
    public bool IncommingNegotiateOffer(NegotiateTE Offer)
    {
        bool Result;
        ArmyInfoEventLog Log;
        byte Selection;
        Hero AHero;
        XYLList Locations;
        int I;
        int J;
        int Direction;
        int X = default;
        int Y = default;
        int JoinCount;
        int Side;
        bool Placed;
        if (Offer.FObject == NegotiateObject.Army)
        {
            if (Offer.FResult == NegotiateTE.NegotiateResult.Accept)
            {
                Selection = GetJoinSelection(Offer.FPlayer);
                JoinCount = SelectionCount(Selection);
                if (Offer.FMode.Contains(NegotiateModeElement.PrisonerJoin) || JoinAmount(Offer.FPlayer, Selection) <= Offer.FGold)
                {
                    BeginUpdate();
                    Offer.FMode = Offer.FMode + Set<NegotiateModeElement>.Of(NegotiateModeElement.MoveOnAccept);
                    Log = new ArmyInfoEventLog(null);
                    Log.FArmyView.Setup(this, Selection);
                    Log.FSelection = 255;
                    Log.FDescription = "";
                    Log.FTitle = AbilityRules.TranslateRStr(P2PartyJoinRStr);
                    Log.FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.User);
                    AHero = Hero(Selection, Set<HeroType>.Of(HeroType.Hero));
                    if (AHero == null)
                    {
                        if (JoinCount == 1)
                        {
                            Log.FText = AbilityRules.TranslateRStr(P2OneUnitJoinedRStr);
                            var name = AoWHSMap.Current.FPlayers.GetPlayers(Offer.FPlayer).GetName();
                            var format = AbilityRules.Format(AbilityRules.TranslateRStr(P2AUnitJoinedTheForcesofYRStr), [name]);
                            Log.FMessages.Text = format;
                        }
                        else
                        {
                            Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(P2XUnitsJoinedRStr), [IntToStr(JoinCount)]);
                            var selectionCount = SelectionCount(Selection);
                            var name = AoWHSMap.Current.FPlayers.GetPlayers(Offer.FPlayer).GetName();
                            var format = AbilityRules.Format(AbilityRules.TranslateRStr(P2XUnitsJoinedTheForcesofYRStr), [selectionCount, name]);
                            Log.FMessages.Text = format;
                        }
                    }
                    else
                    {
                        Log.FText = AbilityRules.Format(AbilityRules.TranslateRStr(P2XJoinedRStr), [AHero.FHeroName]);
                        var name = AoWHSMap.Current.FPlayers.GetPlayers(Offer.FPlayer).GetName();
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(P2XJoinedTheForcesOfYRStr), [AHero.FHeroName, name]);
                        Log.FMessages.Text = format;
                    }

                    EventLog.DistributeLocationEventLog(Log, -1, FX, FY, FL, false);
                    Log.Release();
                    if (ValidateSelection(255) != Selection)
                    {
                        Locations = new XYLList();
                        if (Offer.FArmyHS != null)
                        {
                            var yhx = Offer.FArmyHS.GetYhx();
                            var xhx = Offer.FArmyHS.GetXhx();
                            Direction = HexGeometry.dHXtoHNfast(FX, FY, xhx, yhx);
                            if (Direction > 0 && Direction <= 6)
                            {
                                Locations.AddXYL(FX, FY, FL);
                                Side = Direction + 1;
                                if (Side > 6)
                                {
                                    Side = 1;
                                }

                                // { Original sign extends the wrapped byte coordinates here. }
                                HexGeometry.CenterHNtoHX((sbyte)Offer.FX, (sbyte)Offer.FY, Side, ref X, ref Y);
                                Locations.AddXYL((sbyte)X, (sbyte)Y, FL);
                                Side = Direction - 1;
                                if (Side == 0)
                                {
                                    Side = 6;
                                }

                                HexGeometry.CenterHNtoHX((sbyte)Offer.FX, (sbyte)Offer.FY, Side, ref X, ref Y);
                                Locations.AddXYL((sbyte)X, (sbyte)Y, FL);
                            }
                        }

                        J = 0;
                        I = Count;
                        while (I != 0)
                        {
                            I -= 1;
                            if ((Selection & (1 << I)) != 0)
                            {
                                Placed = false;
                                while (!Placed && J < Locations.Count)
                                {
                                    var item = Locations[J];
                                    var locationsItem = Locations[J];
                                    var item2 = Locations[J];
                                    Placed = GetUnit(I).Place(item2.X, locationsItem.Y, item.L, Offer.FPlayer, Set<UnitPlaceModeElement>.Of()) != null;
                                    if (!Placed)
                                    {
                                        J += 1;
                                    }
                                }

                                if (!Placed)
                                {
                                    GetUnit(I).Place(FX, FY, FL, Offer.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                                }
                            }
                        }

                        Locations?.Dispose();
                    }
                    else
                    {
                        SetPlayer(Offer.FPlayer);
                    }

                    EndUpdate();
                    Offer.Accept();
                }
                else
                {
                    Offer.FMode = Set<NegotiateModeElement>.Of(NegotiateModeElement.AttackOnReject);
                    Offer.Reject();
                }
            }
            else
            {
                if (Offer.FResult == NegotiateTE.NegotiateResult.Reject && !Offer.FMode.Contains(NegotiateModeElement.PrisonerJoin))
                {
                    Offer.Reject();
                }
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:2468
    public void TriggerFireDamage()
    {
        ArmyEventLog Log;
        AbstractUnit AUnit;
        int Damage;
        bool Damaged;
        RandSeed = AoWHSMap.Current.Random(16777215);
        Log = new ArmyEventLog(null);
        Log.FArmyText = AbilityRules.TranslateRStr(P2FireDamageRStr);
        Log.FPlayer = GetArmyHS().FArmy.FPlayer;
        var p2FireDamage = AbilityRules.TranslateRStr(P2FireDamageRStr);
        Log.FArmyMessages.Add(p2FireDamage);
        Log.FArmyIcon = 0;
        var armyHS = GetArmyHS();
        Log.FArmyView.Setup(armyHS.FArmy, 255);
        BeginUpdate();
        Damaged = false;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            AUnit = GetUnit(I);
            if (!AUnit.GetImmunityTypes().Contains(DamageType.Fire))
            {
                Damage = AUnit.ExecuteDamageRole(4, 4, Set<DamageType>.Of(DamageType.Fire));
                if (Damage > AUnit.GetHitPoints())
                {
                    Damage = AUnit.GetHitPoints();
                }

                if (Damage != 0)
                {
                    Damaged = true;
                    AUnit.SetHitPoints((sbyte)(AUnit.GetHitPoints() - (sbyte)Damage));
                }

                Log.FDamageValues.Add((byte)Damage);
                Log.FDamageTypes.Add((int)DamageType.Fire);
            }
            else
            {
                Log.FDamageValues.Add(0);
                Log.FDamageTypes.Add((int)DamageType.None);
            }
        }

        Log.ValidateGameOver();
        if (Damaged)
        {
            EventLog.DistributeLocationEventLog(Log, -1, FX, FY, FL, true);
        }

        Log.Release();
        Validate();
        EndUpdate();
    }

    // Army.PAS:2508
    public void RetrieveStormDamageInfo(StormType Storm, int Distance, ref StormDamageInfo Info)
    {
        int Attack;
        int Damage;
        byte Relation;
        int Value = default;
        int Amount = default;
        AbstractUnit AUnit;
        Relation = (byte)(int)AbilityRules.DiplomaticRelation(Info.Player, Info.IndependentRelation, FPlayer, FIndependentRelation);
        if (Info.FriendlyRelations.Contains(Relation) || Info.EnemyRelations.Contains(Relation))
        {
            switch (Storm)
            {
                case StormType.Ice:
                    Attack = 7;
                    Damage = 5;
                    break;
                case StormType.Lightning:
                    Attack = 8;
                    Damage = 7;
                    break;
                case StormType.Death:
                    Attack = 7;
                    Damage = 4;
                    break;
                case StormType.Holy:
                    Attack = 7;
                    Damage = 5;
                    break;
                case StormType.Fire:
                    Attack = 7;
                    Damage = 5;
                    break;
                case StormType.Pestilence:
                    Attack = 4;
                    Damage = 3;
                    break;
                default:
                    Attack = 0;
                    Damage = 0;
                    break;
            }

            for (int I = Count - 1; I >= 0; I--)
            {
                AUnit = GetUnit(I);
                if (AUnit.FPlayer != 0 || AUnit.FGroup != null && AUnit.FGroup.GroupClass.Behavior != GroupBehavior.Passive)
                {
                    AUnit.RetrieveDamageInfo((sbyte)Attack, (sbyte)Damage, Set<DamageType>.Of(StormDamageType[(int)Storm]), ref Value, ref Amount);
                    if (Info.FriendlyRelations.Contains(Relation))
                    {
                        Info.FriendlyDamage += Amount;
                        // { Original assigns the second return to damage. }
                        Info.FriendlyValue += Value;
                    }

                    if (Info.EnemyRelations.Contains(Relation))
                    {
                        Info.EnemyDamage += Amount;
                        Info.EnemyValue += Value;
                    }
                }
            }
        }
    }

    // Army.PAS:2538
    public void IncommingStorm(StormType Storm, int Distance, sbyte Player)
    {
        ArmyEventLog Log;
        bool Damaged;
        int Damage;
        RandSeed = AoWHSMap.Current.Random(16777215);
        Log = new ArmyEventLog(null);
        Log.FArmyText = AbilityRules.TranslateRStr(GameTables.TStormTypeRStr[(int)Storm]);
        Log.FPlayer = Player;
        // { Original appends the storm message. }
        var stormType = AbilityRules.TranslateRStr(GameTables.TStormTypeRStr[(int)Storm]);
        Log.FArmyMessages.Add(stormType);
        Log.FArmyIcon = 2;
        Log.FArmyView.Setup(this, 255);
        BeginUpdate();
        Damaged = false;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            Damage = ((AbstractUnit)FChildren.List[I]).ExecuteStormDamage(Storm, Distance);
            if (Damage != 0)
            {
                Damaged = true;
            }

            Log.FDamageValues.Add((byte)Damage);
            Log.FDamageTypes.Add((byte)(int)StormDamageType[(int)Storm]);
        }

        if (Damaged)
        {
            Log.ValidateGameOver();
            EventLog.DistributeLocationEventLog(Log, FPlayer, FX, FY, FL, true);
        }

        Log.Release();
        Validate();
        EndUpdate();
    }

    // Army.PAS:2567
    public void UpdateAITarget(in AITargetInfo Info, ref AITargetData Data)
    {
        byte Selection;
        switch (Info.Mode)
        {
            case AITargetMode.Attack:
            case AITargetMode.Raid:
            case AITargetMode.AttackArmies:
                if (!GetUnitsConcealedForPlayer(255, Info.Player) && Info.Player != FPlayer)
                {
                    if (FPlayer > 0)
                    {
                        if (AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, FIndependentRelation) || Info.Mode == AITargetMode.Raid)
                        {
                            if (Info.Player != 0 && Leader(255) != null)
                            {
                                Data.Priority = 300;
                            }
                            else
                            {
                                if (Info.Player != 0 && Hero(255, Set<HeroType>.Of(HeroType.Hero)) != null)
                                {
                                    Data.Priority = 100;
                                }
                                else
                                {
                                    Data.Priority = 50;
                                }
                            }
                        }
                        else
                        {
                            Data.Priority = 0;
                        }
                    }
                    else
                    {
                        if (FPlayer == 0)
                        {
                            Selection = GetJoinSelection(Info.Player);
                            if (Selection != 0)
                            {
                                var joinAmount = JoinAmount(Info.Player, Selection);
                                Data.Cost = joinAmount;
                                Data.TargetType = AITargetType.Negotiate;
                            }
                            else
                            {
                                if (AbilityRules.ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, FIndependentRelation))
                                {
                                    Data.Priority = 50;
                                }
                            }
                        }
                    }
                }

                break;
            case AITargetMode.GuardArea:
                if (!GetUnitsConcealedForPlayer(255, Info.Player) && Info.Player != FPlayer)
                {
                    Data.Priority = 50;
                }
                else
                {
                    Data.Priority = 0;
                }

                break;
            case AITargetMode.Hunt:
                Data.Priority = 10;
                break;
            case AITargetMode.Recruit:
                if (Info.Player == FPlayer && (FPlayer != 0 || AbilityRules.DiplomaticRelation(Info.Player, Info.IndependentRelation, FPlayer, FIndependentRelation) != DiplomaticRelation.War))
                {
                    Data.Priority = Data.Priority | 1;
                }

                break;
        }
    }

    // Army.PAS:2605
    public int AIMovePriority()
    {
        int Result = 0;
        if (Leader(255) != null)
        {
            Result += 50000;
        }

        if (Hero(255, Set<HeroType>.Of(HeroType.Hero, HeroType.HeroLeader)) != null)
        {
            Result += 10000;
        }

        Result += ObtainValue(255);
        return Result;
    }

    // Army.PAS:2613
    public void ResetMoveDone()
    {
        AbstractUnit AUnit;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            AUnit = (AbstractUnit)FChildren.List[I];
            AUnit.FUnitStatus = AUnit.FUnitStatus - Set<UnitStatusElement>.Of(UnitStatusElement.MoveDone);
        }
    }

    // Army.PAS:2623
    public bool CanCombine(Army Army, byte Selection)
    {
        bool Result;
        int I;
        int MaxCount;
        byte Temp;
        AbstractUnit OwnTransporter;
        AbstractUnit OtherTransporter;
        Selection = Army.ValidateSelection(Selection);
        I = 0;
        Temp = Selection;
        while (Temp != 0)
        {
            if ((Temp & 1) != 0)
            {
                I += 1;
            }

            Temp = (byte)(int)(((uint)Temp) >> 1);
        }

        OwnTransporter = Transporter(255);
        if (OwnTransporter != null)
        {
            MaxCount = OwnTransporter.GetTransportCapacity() + 1;
        }
        else
        {
            MaxCount = 8;
        }

        I += Count;
        if (I <= MaxCount)
        {
            OtherTransporter = Army.Transporter(Selection);
            if (OtherTransporter != null && (I > OtherTransporter.GetTransportCapacity() + 1 || OwnTransporter != null))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // intf_P2_07.inc:445
    public virtual bool GetUnitConcealedForPlayer(int index, sbyte player)
    {
        if (player == FPlayer || player == -1 || FX == -1) return false;
        var field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
        return GetUnit(index).ConcealedOnMapF(field) && GetConcealedForPlayer(player);
    }

    internal static FixedArray_Aow_Units_AoWE_TDamageType_7 StormDamageType = FixedArray_Aow_Units_AoWE_TDamageType_7.From((ReadOnlySpan<DamageType>)new DamageType[7]
    {
        DamageType.None,
        DamageType.Fire,
        DamageType.Death,
        DamageType.Holy,
        DamageType.Cold,
        DamageType.Lightning,
        DamageType.Poison
    });

    private static readonly string UnitsDrownRStr = "Units Drowned";
    private static readonly string UnitsBurnedRStr = "Units Burned";
    private static readonly string UnitsCrushedRStr = "Units Crushed";
    private static readonly string ArmyInherentRStr = "Inherent";
    private static readonly string DesertionRStr = "Desertion";
    private static readonly string TheLeaderOfThePartyStepsForwardRStr = "The leader of the party steps forward";
    private static readonly string OneUnitDesertedRStr = "1 unit deserted";
    private static readonly string XUnitsDesertedRStr = "%s units deserted";
    private static readonly string DesertUnitNoUpkeepRStr = "This one abandons you, refusing a voucher for pay.";
    private static readonly string DesertUnitsNoUpkeepRStr = "Without sufficient funds for support, these troops desert you.";
    private static readonly string DesertUnitLowMoraleRStr = "With no common cause, this one deserts you.";
    private static readonly string DesertUnitsLowMoraleRStr = "No longer sharing your cause, these desert you.";
    private static readonly string DesertUnitLowMoraleSplitRStr = "Lacking faith, this one abandons your ranks";
    private static readonly string DesertUnitsLowMoraleSplitRStr = "Troubled and weary, these abandon your ranks.";
    private static readonly string P2PartyJoinHeaderRStr = "The leader of the party steps forward:";
    private static readonly string P2PartyJoinRStr = "Party Join";
    private static readonly string P2OneUnitJoinedRStr = "1 unit joined";
    private static readonly string P2AUnitJoinedTheForcesofYRStr = "A unit joined the forces of %s";
    private static readonly string P2XUnitsJoinedRStr = "%s units joined";
    private static readonly string P2XUnitsJoinedTheForcesofYRStr = "%d units joined the forces of %s";
    private static readonly string P2XJoinedRStr = "%s joined";
    private static readonly string P2XJoinedTheForcesOfYRStr = "%0:s joined the forces of %1:s";
    private static readonly string P2FireDamageRStr = "Fire Damage";

    public enum PartyStatus : byte
    {
        Unruly = 0, Unrest = 1, Stable = 2, Content = 3, Cheerful = 4
    }
}
