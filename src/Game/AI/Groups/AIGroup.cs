// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

public partial class AIGroup : UnitUserList, IAIGroupClass
{
    // AIGroup.pas:101
    public new const int ClassId = 136272;

    // AIGroup.pas:57
    public AIGroup(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        Status = Set<AIGroupStatusElement>.Of();
        Productions = new AIGroupProductionList(null) { Holder = CreationHolder() };
        GroupId = -1;
        Player = -1;
        IndependentRelation = -1;
        TransporterId = -1;
        LeaderId = -1;
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    public Set<AIGroupStatusElement> Status { get; set; }

    private int _refCount;

    public Troop Leader { get; private set; }
    public Troop Transporter { get; private set; }
    public AIGroupProductionList Productions { get; }

    public int GroupId { get; set; }
    public sbyte Player { get; set; }
    public int LeaderId { get; set; }
    public int TransporterId { get; set; }
    public sbyte IndependentRelation { get; set; }

    public AITargetLocation TargetMapCoordinate { get; set; }

    public AIGroupTargetStatus TargetType { get; set; }
    public int GoldBudget { get; set; }
    public int ManaBudget { get; set; }
    public int TargetMovePoints { get; set; }
    public byte TargetMode { get; set; }
    public byte ProcessPriority { get; set; }
    public byte RecruitmentPriority { get; set; }

    // AIGroup.pas:72
    public override void Dispose()
    {
        if (StrategicMap.Current != null && StrategicMap.Current.UnitControl == null && Count != 0)
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

        if (Status.Contains(AIGroupStatusElement.Activated))
        {
            Deactivate();
        }

        Clear();
        Productions?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:104
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        GroupId = persister.Int32("GroupId", value: GroupId);
        TransporterId = persister.Int32("TransporterId", value: TransporterId, fallback: -1);
        LeaderId = persister.Int32("LeaderId", value: LeaderId, fallback: -1);
        int target = TargetMapCoordinate.ToStored().ToInt32();
        target = persister.Int32("TargetMapCoordinate", value: target);
        TargetMapCoordinate = AITargetLocation.FromStored(StoredCoordinateImage.FromInt32(target));
        TargetType = persister.Enum("TargetType", value: TargetType);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        IndependentRelation = persister.RawSByte("IndependentRelation", value: IndependentRelation);
        ProcessPriority = persister.Byte("ProcessPriority", value: ProcessPriority);
        RecruitmentPriority = persister.Byte("RecruitmentPriority", value: RecruitmentPriority);
        GoldBudget = persister.Int32("GoldBudget", value: GoldBudget);
        ManaBudget = persister.Int32("ManaBudget", value: ManaBudget);
        TargetMovePoints = persister.Int32("TargetMovePoints", value: TargetMovePoints);
        TargetMode = persister.RawByte("TargetMode", value: TargetMode);
    }

    // AIGroup.pas:88
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:94
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:127
    public override void Clear()
    {
        Troop transporter;
        while (Count != 0)
        {
            DeleteUnit(this[Count - 1]);
        }

        transporter = Transporter;
        if (transporter != null)
        {
            DeleteUnit(transporter);
        }

        Leader = null;
        LeaderId = -1;
    }

    // AIGroup.pas:515
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986)
        {
            NewTurn((sbyte)message.Payload.Int);
        }
        else
        {
            if (message.MessageId == 536870920)
            {
                Activate();
                ValidateEmpty();
            }
        }
    }

    // AIGroup.pas:162
    public override void GetLocation(out sbyte hexX, out sbyte hexY, out sbyte hexLevel)
    {
        MapCoordinate location = default;
        location = GetLocation();
        hexX = location.X;
        hexY = location.Y;
        hexLevel = location.MapLevel;
    }

    // AIGroup.pas:365
    public override bool AddUnit(Troop troop)
    {
        bool result;
        AIGroup group;
        if (IndexOf(troop) == -1)
        {
            group = troop.Group;
            if (group != null)
            {
                group.DeleteUnit(troop);
            }

            if (troop.UnitId != TransporterId || !troop.GetTransporter())
            {
                AddChild(troop);
                if (LeaderId == -1)
                {
                    SetLeader(troop);
                }
            }
            else
            {
                Transporter = troop;
                Transporter.AddRef();
            }

            troop.GroupId = GroupId;
            troop.Group = this;
            Status = Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
            result = true;
        }
        else
        {
            result = false;
        }

        ValidateEmpty();
        return result;
    }

    // AIGroup.pas:401
    public override bool DeleteUnit(Troop troop)
    {
        bool result;
        int I;
        sbyte x1 = default;
        sbyte y1 = default;
        sbyte l1 = default;
        sbyte x2 = default;
        sbyte y2 = default;
        sbyte l2 = default;
        Troop leader;
        troop.AddRef();
        if (RemoveChild(troop) != -1)
        {
            troop.GroupId = 0;
            troop.Group = null;
            leader = Leader;
            if (leader == troop)
            {
                leader.GetLocation(out x1, out y1, out l1);
                Leader = null;
                LeaderId = -1;
                I = Count;
                while (I != 0 && Leader == null)
                {
                    I -= 1;
                    this[I].GetLocation(out x2, out y2, out l2);
                    if (x1 == x2 && y1 == y2 && l1 == l2)
                    {
                        SetLeader(this[I]);
                    }
                }

                if (Leader == null && Count != 0)
                {
                    SetLeader(this[0]);
                }
            }

            Status = Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
            result = true;
        }
        else
        {
            if (troop == Transporter)
            {
                troop.GroupId = 0;
                troop.Group = null;
                TransporterId = -1;
                Transporter.Release();
                Transporter = null;
                Status = Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
                result = true;
            }
            else
            {
                result = false;
            }
        }

        ValidateEmpty();
        troop.Release();
        return result;
    }

    // AIGroup.pas:391
    public override bool AddUnits(UnitList units)
    {
        AddRef();
        for (int I = units.Count - 1; I >= 0; I--)
        {
            AddUnit(units.GetUnit(I));
        }

        Release();
        return true;
    }

    // AIGroup.pas:212
    public virtual int GetGoldBudget() => GoldBudget;

    // AIGroup.pas:216
    public virtual int GetManaBudget() => ManaBudget;

    // AIGroup.pas:148
    public virtual MapCoordinate GetLocation()
    {
        MapCoordinate location = default;
        // { Reach the leader through its field. }
        if (Leader != null)
        {
            Leader.GetLocation(out var locationX, out var locationY, out var locationMapLevel);
            location = new MapCoordinate(X: locationX, Y: locationY, MapLevel: locationMapLevel);
        }
        else
        {
            location = MapCoordinate.None;
        }

        return location;
    }

    // AIGroup.pas:510
    public virtual void NewTurn(sbyte player)
    {
        if (player == Player)
        {
            Status = Status - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        }
    }

    // AIGroup.pas:138
    public virtual AIGroupControl CreateGroupControl()
    {
        AIGroupControl result = (AIGroupControl)GroupControlClass().CreateInstance(null, holder: CreationHolder());
        result.Group = this;
        AddRef();
        return result;
    }

    // AIGroup.pas:463
    public virtual bool DefendLocation(MapCoordinate location, DefensePriority priority)
    {
        bool result;
        MapCoordinate groupLocation = default;
        groupLocation = GetLocation();
        if (location.MapLevel == groupLocation.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, groupLocation.X, groupLocation.Y) <= 1)
        {
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:459
    public virtual bool CooperateWithGroup(AIGroup group, AIGroupControlTarget target) => group.GroupClass.Behavior == GroupClass.Behavior;

    // AIGroup.pas:473
    public virtual bool GetRecruitableUnits(AIGroup recruiter, UnitList units, UnitList recruitable) => false;

    // AIGroup.pas:144
    public int GetProcessPriority() => Count + (ProcessPriority << 16);

    // AIGroup.pas:171
    public bool GetAdjacent()
    {
        MapCoordinateList locations;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        locations = new MapCoordinateList();
        for (int I = Count - 1; I >= 0; I--)
        {
            GetUnit(I).GetLocation(out X, out Y, out L);
            locations.Add(HexGeometry.PackMapCoordinate(X, Y, L));
        }

        bool result = MapCoordinateList.LocationsConnected(locations);
        locations?.Dispose();
        return result;
    }

    // AIGroup.pas:186
    public void SetLeader(Troop troop)
    {
        if (IndexOf(troop) != -1)
        {
            Leader = troop;
            LeaderId = troop.UnitId;
        }
    }

    // AIGroup.pas:194
    public int GetMovePointsMin()
    {
        int result = 50;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (result > GetUnit(I).GetMovePoints())
            {
                result = GetUnit(I).GetMovePoints();
            }
        }

        return result;
    }

    // AIGroup.pas:203
    public int GetMovesMin()
    {
        int result = 50;
        for (int I = Count - 1; I >= 0; I--)
        {
            if (result > GetUnit(I).GetMoves())
            {
                result = GetUnit(I).GetMoves();
            }
        }

        return result;
    }

    // AIGroup.pas:220
    public void ResetTargetExpense()
    {
        GoldBudget = 0;
        ManaBudget = 0;
    }

    // AIGroup.pas:225
    public void ReturnTargetExpense()
    {
        if (GoldBudget > 0 || ManaBudget > 0)
        {
            StrategicMap.Current.Players.GetPlayers(Player).AIBudget.ReturnExpense(GoldBudget, ManaBudget);
            GoldBudget = 0;
            ManaBudget = 0;
        }
    }

    // AIGroup.pas:234
    public bool RequestTargetExpense(int gold, int mana)
    {
        int G;
        int M;
        G = gold - GoldBudget;
        M = mana - ManaBudget;
        var players = StrategicMap.Current.Players.GetPlayers(Player);
        return StrategicMap.Current.Players.GetPlayers(Player).AIBudget.RequestBudgetExpense(G, M, players.AIBudget.DefaultBudget, true);
    }

    // AIGroup.pas:243
    public bool MakeTargetExpense(int gold, int mana)
    {
        bool result;
        if (RequestTargetExpense(gold, mana))
        {
            ReturnTargetExpense();
            var players = StrategicMap.Current.Players.GetPlayers(Player);
            result = StrategicMap.Current.Players.GetPlayers(Player).AIBudget.BudgetExpenseMade(gold, mana, players.AIBudget.DefaultBudget, AIBudgetManager.ExpenseMode.PoolThenBudget);
            if (result)
            {
                GoldBudget = gold;
                ManaBudget = mana;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:258
    public bool SetupTarget(MapCoordinate location, int movePoints, AIGroupTargetStatus targetType, byte mode, int transporterId, int gold, int mana)
    {
        bool result;
        Troop transporter;
        if (location != MapCoordinate.None)
        {
            TargetMapCoordinate = AITargetLocation.At(location);
            TargetType = targetType;
            TargetMovePoints = movePoints;
            TargetMode = (byte)(mode << 0);
            if (targetType == AIGroupTargetStatus.Execute)
            {
                var movePointsMin = GetMovePointsMin();
                TargetMovePoints -= movePointsMin;
            }

            result = MakeTargetExpense(gold, mana);
        }
        else
        {
            TargetMapCoordinate = AITargetLocation.None;
            TargetType = targetType;
            TargetMode = (byte)(mode << 0);
            TargetMovePoints = 0;
            ReturnTargetExpense();
            result = true;
        }

        if (location != MapCoordinate.None && transporterId != -1)
        {
            AddTransporter(StrategicMap.Current.UnitControl.FindUnit(transporterId));
        }
        else
        {
            transporter = Transporter;
            if (transporter != null)
            {
                DeleteUnit(transporter);
            }
        }

        return result;
    }

    // AIGroup.pas:290
    public bool SetupTarget(AIGroupControlTarget target, AIGroupTargetStatus targetType, byte mode)
    {
        bool result;
        Troop transporter;
        if (target != null)
        {
            TargetMapCoordinate = AITargetLocation.At(target.Location);
            TargetType = targetType;
            TargetMovePoints = target.MovePoints;
            TargetMode = mode;
            if (targetType == AIGroupTargetStatus.Execute)
            {
                var movePointsMin = GetMovePointsMin();
                TargetMovePoints -= movePointsMin;
            }

            var targetMana = target.GetTargetMana();
            result = MakeTargetExpense(target.GetTargetGold(), targetMana);
        }
        else
        {
            TargetMapCoordinate = AITargetLocation.None;
            TargetType = targetType;
            TargetMode = mode;
            TargetMovePoints = 0;
            ReturnTargetExpense();
            result = true;
        }

        if (target != null && target.TransporterId != -1)
        {
            AddTransporter(StrategicMap.Current.UnitControl.FindUnit(target.TransporterId));
        }
        else
        {
            transporter = Transporter;
            if (transporter != null)
            {
                DeleteUnit(transporter);
            }
        }

        return result;
    }

    // AIGroup.pas:321
    public void ValidateEmpty()
    {
        bool empty;
        empty = Count == 0 && TransporterId == -1;
        if (empty && !Status.Contains(AIGroupStatusElement.Empty))
        {
            Status = Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Empty);
            Release();
        }
        else
        {
            if (!empty && Status.Contains(AIGroupStatusElement.Empty))
            {
                Status = Status - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Empty);
                AddRef();
            }
        }
    }

    // AIGroup.pas:337
    public void AddTransporter(Troop troop)
    {
        Troop oldTransporter;
        AIGroup group;
        oldTransporter = Transporter;
        if (oldTransporter != troop)
        {
            if (oldTransporter != null)
            {
                oldTransporter.Group = null;
                oldTransporter.GroupId = 0;
                oldTransporter.Release();
            }

            group = troop.Group;
            if (group != null)
            {
                group.DeleteUnit(troop);
                troop.GroupId = 0;
            }

            TransporterId = troop.UnitId;
            Transporter = troop;
            Transporter.AddRef();
            troop.GroupId = GroupId;
            troop.Group = this;
            ValidateEmpty();
        }
    }

    // AIGroup.pas:446
    public void UnitProduced(ProductionControl control, Troop troop)
    {
        sbyte x1 = default;
        sbyte y1 = default;
        sbyte l1 = default;
        sbyte x2 = default;
        sbyte y2 = default;
        sbyte l2 = default;
        if (Leader != null)
        {
            GetLocation(out x1, out y1, out l1);
            troop.GetLocation(out x2, out y2, out l2);
            if (l1 == l2 && HexGeometry.DeltaHexToRadius(x1, y1, x2, y2) <= 2)
            {
                AddUnit(troop);
            }

            Productions.DeleteUnit(troop);
        }
    }

    // AIGroup.pas:475
    public void Activate()
    {
        sbyte player;
        Assert(!Status.Contains(AIGroupStatusElement.Activated), "", "AIGroup.pas", 479);
        Status = Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Activated);
        player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).AIGroups.AddChild(this);
        }

        Leader = FindId(LeaderId);
        Assert(Transporter != null || TransporterId == -1, "", "AIGroup.pas", 485);
    }

    // AIGroup.pas:488
    public void Deactivate()
    {
        sbyte player;
        if (!Status.Contains(AIGroupStatusElement.Activated))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Assertion failure", "D:\\AoWDev\\AoWE\\inc\\AIGroup.pas", 24094);
        }

        Status = Status - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Activated);
        player = Player;
        if (player != -1)
        {
            StrategicMap.Current.Players.GetPlayers(player).AIGroups.RemoveChild(this);
        }
    }

    // AIGroup.pas:498
    public void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Status.Contains(AIGroupStatusElement.Activated) && Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).AIGroups.RemoveChild(this);
            }

            Player = player;
            if (Status.Contains(AIGroupStatusElement.Activated) && Player != -1)
            {
                StrategicMap.Current.Players.GetPlayers(Player).AIGroups.AddChild(this);
            }
        }
    }

    // intf_P5_07.inc:54

    // AIGroup.pas:123
    public virtual EngineClass GroupControlClass() => EngineClass.Of<AIGroupControl>();

    // AIGroup.pas:121
    public static GroupBehavior Behavior => GroupBehavior.None;

}
