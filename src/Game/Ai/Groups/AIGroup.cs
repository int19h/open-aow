// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

public partial class AIGroup : UnitUserList, IAIGroupClass
{
    // AIGroup.pas:101
    public new const int ClassID = 136272;

    // AIGroup.pas:57
    public AIGroup(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FStatus = Set<AIGroupStatusElement>.Of();
        FProductions = new AIGroupProductionList(null);
        FGroupID = -1;
        FPlayer = -1;
        FIndependentRelation = -1;
        FTransporterID = -1;
        FLeaderID = -1;
        FProcessPriority = 10;
        FRecruitmentPriority = 10;
    }

    public Set<AIGroupStatusElement> FStatus;
    public int FRefCount;
    public AbstractUnit FLeader;
    public AbstractUnit FTransporter;
    public AIGroupProductionList FProductions;
    public int FGroupID;
    public sbyte FPlayer;
    public int FLeaderID;
    public int FTransporterID;
    public sbyte FIndependentRelation;
    public XYL FTargetXYL;
    public AIGroupTargetStatus FTargetType;
    public int FGoldBudget;
    public int FManaBudget;
    public int FTargetMovePoints;
    public byte FTargetMode;
    public byte FProcessPriority;
    public byte FRecruitmentPriority;

    // AIGroup.pas:72
    public override void Dispose()
    {
        if (AoWHSMap.Current != null && AoWHSMap.Current.FUnitControl == null && Count != 0)
        {
            // { Original diagnostic raises and immediately swallows its exception. }
            try
            {
                throw new InvalidOperationException("oohps");
            }
            catch
            {
            }
        }

        if (FStatus.Contains(AIGroupStatusElement.Activated))
        {
            Deactivate();
        }

        Clear();
        FProductions?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:104
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FGroupID, 20);
        Stream.rwIntegerEx(ref FTransporterID, -1, 21);
        Stream.rwIntegerEx(ref FLeaderID, -1, 22);
        Stream.rwInteger(ref Unsafe.As<XYL, int>(ref FTargetXYL), 23);
        Stream.rwData(Bytes.Of(ref FTargetType, 1), 1, 24);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 25);
        Stream.rwData(Bytes.Of(ref FIndependentRelation, 1), 1, 26);
        Stream.rwByte(ref FProcessPriority, 27);
        Stream.rwByte(ref FRecruitmentPriority, 28);
        Stream.rwInteger(ref FGoldBudget, 29);
        Stream.rwInteger(ref FManaBudget, 30);
        Stream.rwInteger(ref FTargetMovePoints, 31);
        Stream.rwData(Bytes.Of(ref FTargetMode, 1), 1, 32);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("GroupID", ref FGroupID);
        persister.Int("TransporterID", ref FTransporterID, -1);
        persister.Int("LeaderID", ref FLeaderID, -1);
        int target = FTargetXYL.Packed;
        persister.Int("TargetXYL", ref target);
        if (persister.Loading) FTargetXYL.Packed = target;
        persister.Enum("TargetType", ref FTargetType);
        PersistSupport.UnsignedByte(persister, "Player", ref FPlayer);
        persister.RawSByte("IndependentRelation", ref FIndependentRelation);
        persister.Byte("ProcessPriority", ref FProcessPriority);
        persister.Byte("RecruitmentPriority", ref FRecruitmentPriority);
        persister.Int("GoldBudget", ref FGoldBudget);
        persister.Int("ManaBudget", ref FManaBudget);
        persister.Int("TargetMovePoints", ref FTargetMovePoints);
        persister.RawByte("TargetMode", ref FTargetMode);
    }

    // AIGroup.pas:88
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:94
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // AIGroup.pas:127
    public override void Clear()
    {
        AbstractUnit Transporter;
        while (FChildren.Count != 0)
        {
            DeleteUnit((AbstractUnit)FChildren.List[FChildren.Count - 1]);
        }

        Transporter = FTransporter;
        if (Transporter != null)
        {
            DeleteUnit(Transporter);
        }

        FLeader = null;
        FLeaderID = -1;
    }

    // AIGroup.pas:515
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986)
        {
            NewTurn((sbyte)Msg.Payload.Int);
        }
        else
        {
            if (Msg.MsgID == 536870920)
            {
                Activate();
                ValidateEmpty();
            }
        }
    }

    // AIGroup.pas:162
    public override void GetLocation(ref sbyte Xhx, ref sbyte Yhx, ref sbyte Lhx)
    {
        XYL Location = default;
        Location.Packed = GetLoc();
        Xhx = Location.X;
        Yhx = Location.Y;
        Lhx = Location.L;
    }

    // AIGroup.pas:365
    public override bool AddUnit(AbstractUnit AUnit)
    {
        bool Result;
        AIGroup Group;
        if (IndexOf(AUnit) == -1)
        {
            Group = AUnit.FGroup;
            if (Group != null)
            {
                Group.DeleteUnit(AUnit);
            }

            if (AUnit.FUnitID != FTransporterID || !AUnit.GetTransporter())
            {
                AddChild(AUnit);
                if (FLeaderID == -1)
                {
                    SetLeader(AUnit);
                }
            }
            else
            {
                FTransporter = AUnit;
                FTransporter.AddRef();
            }

            AUnit.FGroupID = FGroupID;
            AUnit.FGroup = this;
            FStatus = FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
            Result = true;
        }
        else
        {
            Result = false;
        }

        ValidateEmpty();
        return Result;
    }

    // AIGroup.pas:401
    public override bool DeleteUnit(AbstractUnit AUnit)
    {
        bool Result;
        int I;
        sbyte X1 = default;
        sbyte Y1 = default;
        sbyte L1 = default;
        sbyte X2 = default;
        sbyte Y2 = default;
        sbyte L2 = default;
        AbstractUnit Leader;
        AUnit.AddRef();
        if (RemoveChild(AUnit) != -1)
        {
            AUnit.FGroupID = 0;
            AUnit.FGroup = null;
            Leader = FLeader;
            if (Leader == AUnit)
            {
                Leader.GetLocation(ref X1, ref Y1, ref L1);
                FLeader = null;
                FLeaderID = -1;
                I = Count;
                while (I != 0 && FLeader == null)
                {
                    I -= 1;
                    ((AbstractUnit)FChildren.List[I]).GetLocation(ref X2, ref Y2, ref L2);
                    if (X1 == X2 && Y1 == Y2 && L1 == L2)
                    {
                        SetLeader((AbstractUnit)FChildren.List[I]);
                    }
                }

                if (FLeader == null && Count != 0)
                {
                    SetLeader((AbstractUnit)FChildren.List[0]);
                }
            }

            FStatus = FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
            Result = true;
        }
        else
        {
            if (AUnit == FTransporter)
            {
                AUnit.FGroupID = 0;
                AUnit.FGroup = null;
                FTransporterID = -1;
                FTransporter.Release();
                FTransporter = null;
                FStatus = FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
                Result = true;
            }
            else
            {
                Result = false;
            }
        }

        ValidateEmpty();
        AUnit.Release();
        return Result;
    }

    // AIGroup.pas:391
    public override bool AddUnits(UnitList Units)
    {
        AddRef();
        for (int I = Units.Count - 1; I >= 0; I--)
        {
            AddUnit(Units.GetUnit(I));
        }

        Release();
        return true;
    }

    // AIGroup.pas:212
    public virtual int GetGoldBudget() => FGoldBudget;

    // AIGroup.pas:216
    public virtual int GetManaBudget() => FManaBudget;

    // AIGroup.pas:148
    public virtual int GetLoc()
    {
        XYL Location = default;
        // { Reach the leader through its field. }
        if (FLeader != null)
        {
            FLeader.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
            Location.Flag = 0;
        }
        else
        {
            Location.Packed = 16777215;
        }

        return Location.Packed;
    }

    // AIGroup.pas:510
    public virtual void NewTurn(sbyte Player)
    {
        if (Player == FPlayer)
        {
            FStatus = FStatus - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        }
    }

    // AIGroup.pas:138
    public virtual AIGroupControl CreateGroupControl()
    {
        AIGroupControl Result = (AIGroupControl)GroupControlClass().Create(null);
        Result.FGroup = this;
        AddRef();
        return Result;
    }

    // AIGroup.pas:463
    public virtual bool DefendLocation(int Location, DefensePriority Priority)
    {
        bool Result;
        XYL GroupLocation = default;
        GroupLocation.Packed = GetLoc();
        if (Unsafe.As<int, XYL>(ref Location).L == GroupLocation.L && HexGeometry.dHXtoRad(Unsafe.As<int, XYL>(ref Location).X, Unsafe.As<int, XYL>(ref Location).Y, GroupLocation.X, GroupLocation.Y) <= 1)
        {
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:459
    public virtual bool CooperateWithGroup(AIGroup Group, AIGroupControlTarget Target) => Group.GroupClass.Behavior == GroupClass.Behavior;

    // AIGroup.pas:473
    public virtual bool GetRecruitableUnits(AIGroup Recruiter, UnitList Units, UnitList Recruitable) => false;

    // AIGroup.pas:144
    public int GetProcessPriority() => Count + (FProcessPriority << 16);

    // AIGroup.pas:171
    public bool GetAdjacent()
    {
        XYLList Locations;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Locations = new XYLList();
        for (int I = Count - 1; I >= 0; I--)
        {
            GetUnit(I).GetLocation(ref X, ref Y, ref L);
            Locations.Add(HexGeometry.PackXYL(X, Y, L));
        }

        bool Result = XYLList.LocationsConnected(Locations);
        Locations?.Dispose();
        return Result;
    }

    // AIGroup.pas:186
    public void SetLeader(AbstractUnit AUnit)
    {
        if (IndexOf(AUnit) != -1)
        {
            FLeader = AUnit;
            FLeaderID = AUnit.FUnitID;
        }
    }

    // AIGroup.pas:194
    public int GetMovePointsMin()
    {
        int Result = 50;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (Result > GetUnit(I).GetMovePoints())
            {
                Result = GetUnit(I).GetMovePoints();
            }
        }

        return Result;
    }

    // AIGroup.pas:203
    public int GetMovesMin()
    {
        int Result = 50;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (Result > GetUnit(I).GetMoves())
            {
                Result = GetUnit(I).GetMoves();
            }
        }

        return Result;
    }

    // AIGroup.pas:220
    public void ResetTargetExpense()
    {
        FGoldBudget = 0;
        FManaBudget = 0;
    }

    // AIGroup.pas:225
    public void ReturnTargetExpense()
    {
        if (FGoldBudget > 0 || FManaBudget > 0)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIBudget.ReturnExpense(FGoldBudget, FManaBudget);
            FGoldBudget = 0;
            FManaBudget = 0;
        }
    }

    // AIGroup.pas:234
    public bool RequestTargetExpense(int Gold, int Mana)
    {
        int G;
        int M;
        G = Gold - FGoldBudget;
        M = Mana - FManaBudget;
        var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
        return AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIBudget.RequestBudgetExpense(G, M, players.FAIBudget.FDefaultBudget, true);
    }

    // AIGroup.pas:243
    public bool MakeTargetExpense(int Gold, int Mana)
    {
        bool Result;
        if (RequestTargetExpense(Gold, Mana))
        {
            ReturnTargetExpense();
            var players = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer);
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIBudget.BudgetExpenseMade(Gold, Mana, players.FAIBudget.FDefaultBudget, AIBudgetManager.BudgetExpenseMode.PoolThenBudget);
            if (Result)
            {
                FGoldBudget = Gold;
                FManaBudget = Mana;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:258
    public bool SetupTargetEx(XYL Location, int MovePoints, AIGroupTargetStatus TargetType, byte Mode, int TransporterID, int Gold, int Mana)
    {
        bool Result;
        AbstractUnit Transporter;
        if (Unsafe.As<XYL, int>(ref Location) != 16777215)
        {
            Unsafe.As<XYL, int>(ref FTargetXYL) = Unsafe.As<XYL, int>(ref Location) + 0;
            FTargetType = TargetType;
            FTargetMovePoints = MovePoints;
            FTargetMode = (byte)(Mode << 0);
            if (TargetType == AIGroupTargetStatus.Execute)
            {
                var movePointsMin = GetMovePointsMin();
                FTargetMovePoints -= movePointsMin;
            }

            Result = MakeTargetExpense(Gold, Mana);
        }
        else
        {
            Unsafe.As<XYL, int>(ref FTargetXYL) = 16777215;
            FTargetType = TargetType;
            FTargetMode = (byte)(Mode << 0);
            FTargetMovePoints = 0;
            ReturnTargetExpense();
            Result = true;
        }

        if (Unsafe.As<XYL, int>(ref Location) != 16777215 && TransporterID != -1)
        {
            AddTransporter(AoWHSMap.Current.FUnitControl.FindUnit(TransporterID));
        }
        else
        {
            Transporter = FTransporter;
            if (Transporter != null)
            {
                DeleteUnit(Transporter);
            }
        }

        return Result;
    }

    // AIGroup.pas:290
    public bool SetupTarget(AIGroupControlTarget Target, AIGroupTargetStatus TargetType, byte Mode)
    {
        bool Result;
        AbstractUnit Transporter;
        if (Target != null)
        {
            FTargetXYL = Target.FLocation;
            FTargetType = TargetType;
            FTargetMovePoints = Target.FMovePoints;
            FTargetMode = Mode;
            if (TargetType == AIGroupTargetStatus.Execute)
            {
                var movePointsMin = GetMovePointsMin();
                FTargetMovePoints -= movePointsMin;
            }

            var targetMana = Target.GetTargetMana();
            Result = MakeTargetExpense(Target.GetTargetGold(), targetMana);
        }
        else
        {
            Unsafe.As<XYL, int>(ref FTargetXYL) = 16777215;
            FTargetType = TargetType;
            FTargetMode = Mode;
            FTargetMovePoints = 0;
            ReturnTargetExpense();
            Result = true;
        }

        if (Target != null && Target.FTransporterID != -1)
        {
            AddTransporter(AoWHSMap.Current.FUnitControl.FindUnit(Target.FTransporterID));
        }
        else
        {
            Transporter = FTransporter;
            if (Transporter != null)
            {
                DeleteUnit(Transporter);
            }
        }

        return Result;
    }

    // AIGroup.pas:321
    public void ValidateEmpty()
    {
        bool Empty;
        Empty = Count == 0 && FTransporterID == -1;
        if (Empty && !FStatus.Contains(AIGroupStatusElement.Empty))
        {
            FStatus = FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Empty);
            Release();
        }
        else
        {
            if (!Empty && FStatus.Contains(AIGroupStatusElement.Empty))
            {
                FStatus = FStatus - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Empty);
                AddRef();
            }
        }
    }

    // AIGroup.pas:337
    public void AddTransporter(AbstractUnit AUnit)
    {
        AbstractUnit OldTransporter;
        AIGroup Group;
        OldTransporter = FTransporter;
        if (OldTransporter != AUnit)
        {
            if (OldTransporter != null)
            {
                OldTransporter.FGroup = null;
                OldTransporter.FGroupID = 0;
                OldTransporter.Release();
            }

            Group = AUnit.FGroup;
            if (Group != null)
            {
                Group.DeleteUnit(AUnit);
                AUnit.FGroupID = 0;
            }

            FTransporterID = AUnit.FUnitID;
            FTransporter = AUnit;
            FTransporter.AddRef();
            AUnit.FGroupID = FGroupID;
            AUnit.FGroup = this;
            ValidateEmpty();
        }
    }

    // AIGroup.pas:446
    public void UnitProduced(ProductionControl Control, AbstractUnit AUnit)
    {
        sbyte X1 = default;
        sbyte Y1 = default;
        sbyte L1 = default;
        sbyte X2 = default;
        sbyte Y2 = default;
        sbyte L2 = default;
        if (FLeader != null)
        {
            GetLocation(ref X1, ref Y1, ref L1);
            AUnit.GetLocation(ref X2, ref Y2, ref L2);
            if (L1 == L2 && HexGeometry.dHXtoRad(X1, Y1, X2, Y2) <= 2)
            {
                AddUnit(AUnit);
            }

            FProductions.DeleteUnit(AUnit);
        }
    }

    // AIGroup.pas:475
    public void Activate()
    {
        sbyte Player;
        Assert(!FStatus.Contains(AIGroupStatusElement.Activated), "", "AIGroup.pas", 479);
        FStatus = FStatus + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Activated);
        Player = FPlayer;
        if (Player != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIGroups.AddChild(this);
        }

        FLeader = FindID(FLeaderID);
        Assert(FTransporter != null || FTransporterID == -1, "", "AIGroup.pas", 485);
    }

    // AIGroup.pas:488
    public void Deactivate()
    {
        sbyte Player;
        if (!FStatus.Contains(AIGroupStatusElement.Activated))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AIGroup.pas", 24094);
        }

        FStatus = FStatus - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Activated);
        Player = FPlayer;
        if (Player != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIGroups.RemoveChild(this);
        }
    }

    // AIGroup.pas:498
    public void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            if (FStatus.Contains(AIGroupStatusElement.Activated) && FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIGroups.RemoveChild(this);
            }

            FPlayer = Player;
            if (FStatus.Contains(AIGroupStatusElement.Activated) && FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIGroups.AddChild(this);
            }
        }
    }

    // intf_P5_07.inc:54

    // AIGroup.pas:123
    public virtual EClass GroupControlClass() => EClass.Of<AIGroupControl>();

    // AIGroup.pas:121
    public static GroupBehavior Behavior => GroupBehavior.None;

}
