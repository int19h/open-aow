#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.AI.Groups;

// The original's TAIGroup derives from TUnitUserList. The port's group holds its troops as
// references in its node children (it routes messages to them and notifies their changes) and
// reads them through Units. It does not own the troops. Their owner stays the army or the unit
// list that holds them. The members below that carry a Unit.PAS line are TUnitList's part.
public partial class AIGroup : ObservableNode, IChildList<Troop>, IUnitHolder, IAIGroupClass
{
    // AIGroup.pas:101
    public new const int ClassId = 136_272;

    /// <summary>The troops of this group, in their stored order.</summary>
    public UnitCollection Units => new(this);

    // A group takes troops only.
    protected sealed override bool AcceptsChild(EngineObject child) => child is Troop;

    Troop IOwner<Troop>.this[int index] => Units[index];

    int IChildSink<Troop>.AddChild(Troop child) => AddChild(child);

    // Unit.PAS:2188
    public virtual bool CanAddUnit(Troop? troop) => troop.CanAddToList(Units);

    // AIGroup.pas:57
    public AIGroup(CustomNode? owner) : base(owner)
    {
        _refCount = 1;
        Status = AIGroupStatuses.None;
        Productions = new AIGroupProductionList(null) { Holder = CreationHolder() };
        GroupId = -1;
        Player = -1;
        IndependentRelation = -1;
        TransporterId = -1;
        LeaderId = -1;
        ProcessPriority = 10;
        RecruitmentPriority = 10;
    }

    public AIGroupStatuses Status { get; set; }

    private int _refCount;

    public Troop? Leader { get; private set; }
    public Troop? Transporter { get; private set; }
    public AIGroupProductionList Productions { get; }

    public int GroupId { get; set; }
    public sbyte Player { get; set; }
    public int LeaderId { get; set; }
    public int TransporterId { get; set; }
    public sbyte IndependentRelation { get; set; }

    // A fresh group is the Delphi zero value, location (0, 0, 0). The initializer runs before the
    // base constructor, so a construction observer sees that location. Null is only the cleared target.
    public MapCoordinate? TargetLocation { get; set; } = new MapCoordinate(0, 0, 0);

    public AIGroupTargetStatus TargetType { get; set; }
    public int GoldBudget { get; set; }
    public int ManaBudget { get; set; }
    public int TargetMovePoints { get; set; }
    public byte TargetMode { get; set; }
    public int ProcessPriority { get; set; }
    public int RecruitmentPriority { get; set; }

    // AIGroup.pas:72
    public override void Dispose()
    {
        if (StrategicMap.Current is not null && StrategicMap.Current.UnitControl is null && Units.Count != 0)
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

        if (Status.Contains(AIGroupStatuses.Activated))
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
        // Property id 23. A location stores through StoredCoordinateImage with fourth byte 0, and the
        // cleared target stores as -1. A loaded value with any other fourth byte is refused.
        int target = TargetLocation is MapCoordinate location
            ? StoredCoordinateImage.From(location).ToInt32()
            : -1;
        target = persister.Int32("TargetLocation", value: target);
        if (persister.Loading)
        {
            TargetLocation = ReadTargetLocation(target);
        }
        TargetType = persister.Enum("TargetType", value: TargetType);
        Player = persister.UnsignedByte(name: "Player", value: Player);
        IndependentRelation = persister.RawSByte("IndependentRelation", value: IndependentRelation);
        ProcessPriority = persister.Byte("ProcessPriority", value: ProcessPriority.TruncateToByte());
        RecruitmentPriority = persister.Byte("RecruitmentPriority", value: RecruitmentPriority.TruncateToByte());
        GoldBudget = persister.Int32("GoldBudget", value: GoldBudget);
        ManaBudget = persister.Int32("ManaBudget", value: ManaBudget);
        TargetMovePoints = persister.Int32("TargetMovePoints", value: TargetMovePoints);
        TargetMode = persister.RawByte("TargetMode", value: TargetMode);
    }

    private static MapCoordinate? ReadTargetLocation(int stored) => stored == -1
        ? null
        : StoredCoordinateImage.FromInt32(stored).ReadCheckedLocation("TargetLocation");

    // Unit.PAS:2294. The load releases the reference each loaded child took when it was created.
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (persister.Loading)
        {
            for (int index = 0, last = Units.Count - 1; index <= last; index++)
            {
                _ = Units[index].Release();
            }
        }
    }

    // Unit.PAS:2184
    public override void DestroyAllChildren() => Clear();

    // Unit.PAS:2162
    public override int RemoveChild(EngineObject? child)
    {
        int result = base.RemoveChild(child);
        if (result != -1)
        {
            _ = child.Release();
        }

        return result;
    }

    // Unit.PAS:2167
    public override int AddChild(EngineObject? child)
    {
        // The check refuses a child that is not a troop before the troop is read.
        CheckChild(child);
        if (CanAddUnit((Troop?)child))
        {
            _ = child.AddRef();
            return base.AddChild(child);
        }

        return -1;
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
        while (Units.Count != 0)
        {
            _ = DeleteUnit(Units[^1]);
        }

        Troop? transporter = Transporter;
        if (transporter is not null)
        {
            _ = DeleteUnit(transporter);
        }

        Leader = null;
        LeaderId = -1;
    }

    // AIGroup.pas:515
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
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

    // AIGroup.pas:365
    public virtual bool AddUnit(Troop? troop)
    {
        bool result;
        if (Units.IndexOf(troop) == -1)
        {
            AIGroup? group = troop.Group;
            _ = (group?.DeleteUnit(troop));

            if (troop.UnitId != TransporterId || !troop.GetTransporter())
            {
                _ = AddChild(troop);
                if (LeaderId == -1)
                {
                    SetLeader(troop);
                }
            }
            else
            {
                Transporter = troop;
                _ = Transporter.AddRef();
            }

            troop.GroupId = GroupId;
            troop.Group = this;
            Status |= AIGroupStatuses.Changed;
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
    public virtual bool DeleteUnit(Troop troop)
    {
        _ = troop.AddRef();
        bool result;
        if (RemoveChild(troop) != -1)
        {
            troop.GroupId = 0;
            troop.Group = null;
            Troop? leader = Leader;
            if (leader == troop)
            {
                MapCoordinate coordinate91 = leader.GetLocation();
                sbyte x1 = coordinate91.X;
                sbyte y1 = coordinate91.Y;
                sbyte l1 = coordinate91.MapLevel;
                Leader = null;
                LeaderId = -1;
                int i = Units.Count;
                while (i != 0 && Leader is null)
                {
                    i -= 1;
                    MapCoordinate coordinate92 = Units[i].GetLocation();
                    sbyte x2 = coordinate92.X;
                    sbyte y2 = coordinate92.Y;
                    sbyte l2 = coordinate92.MapLevel;
                    if (x1 == x2 && y1 == y2 && l1 == l2)
                    {
                        SetLeader(Units[i]);
                    }
                }

                if (Leader is null && Units.Count != 0)
                {
                    SetLeader(Units[0]);
                }
            }

            Status |= AIGroupStatuses.Changed;
            result = true;
        }
        else
        {
            if (troop == Transporter)
            {
                troop.GroupId = 0;
                troop.Group = null;
                TransporterId = -1;
                _ = Transporter.Release();
                Transporter = null;
                Status |= AIGroupStatuses.Changed;
                result = true;
            }
            else
            {
                result = false;
            }
        }

        ValidateEmpty();
        _ = troop.Release();
        return result;
    }

    // AIGroup.pas:391
    public virtual bool AddUnits(UnitCollection units)
    {
        _ = AddRef();
        for (int i = units.Count - 1; i >= 0; i--)
        {
            _ = AddUnit(units.GetUnit(i));
        }

        _ = Release();
        return true;
    }

    // AIGroup.pas:212
    public virtual int GetGoldBudget() => GoldBudget;

    // AIGroup.pas:216
    public virtual int GetManaBudget() => ManaBudget;

    // AIGroup.pas:148
    void IUnitHolder.NotifyChildChanged(Troop child) => NotifyChildChanged(child);

    public virtual MapCoordinate GetLocation()
    {
        MapCoordinate location;
        // { Reach the leader through its field. }
        if (Leader is not null)
        {
            MapCoordinate coordinate93 = Leader.GetLocation();
            sbyte locationX = coordinate93.X;
            sbyte locationY = coordinate93.Y;
            sbyte locationMapLevel = coordinate93.MapLevel;
            location = new MapCoordinate(locationX, locationY, locationMapLevel);
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
            Status &= ~AIGroupStatuses.Done;
        }
    }

    // AIGroup.pas:138
    public virtual AIGroupControl CreateGroupControl()
    {
        AIGroupControl result = GroupControlClass().Create<AIGroupControl>(null, holder: CreationHolder());
        result.Group = this;
        _ = AddRef();
        return result;
    }

    // AIGroup.pas:463
    public virtual bool DefendLocation(MapCoordinate location, DefensePriority priority)
    {
        MapCoordinate groupLocation = GetLocation();
        return location.MapLevel == groupLocation.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, groupLocation.X, groupLocation.Y) <= 1;
    }

    // AIGroup.pas:459
    public virtual bool CooperateWithGroup(AIGroup group, AIGroupControlTarget target) => group.GroupClass.Behavior == GroupClass.Behavior;

    // AIGroup.pas:473
    public virtual bool GetRecruitableUnits(AIGroup recruiter, UnitUserList units, UnitUserList recruitable) => false;

    // AIGroup.pas:144
    public int GetProcessPriority() => Units.Count + (ProcessPriority << 16);

    // AIGroup.pas:171
    public bool GetAdjacent()
    {
        MapCoordinateList locations = new();
        for (int i = Units.Count - 1; i >= 0; i--)
        {
            MapCoordinate coordinate94 = Units.GetUnit(i).GetLocation();
            sbyte x = coordinate94.X;
            sbyte y = coordinate94.Y;
            sbyte l = coordinate94.MapLevel;
            _ = locations.Add(new MapCoordinate(x, y, l));
        }

        bool result = MapCoordinateList.LocationsConnected(locations);
        locations?.Dispose();
        return result;
    }

    // AIGroup.pas:186
    public void SetLeader(Troop? troop)
    {
        if (Units.IndexOf(troop) != -1)
        {
            Leader = troop;
            LeaderId = troop.UnitId;
        }
    }

    // AIGroup.pas:194
    public int GetMovePointsMin()
    {
        int result = 50;
        foreach (Troop? troop in Units.Reverse())
        {
            if (result > troop.GetMovePoints())
            {
                result = troop.GetMovePoints();
            }
        }

        return result;
    }

    // AIGroup.pas:203
    public int GetMovesMin()
    {
        int result = 50;
        foreach (Troop? troop in Units.Reverse())
        {
            if (result > troop.Moves)
            {
                result = troop.Moves;
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
            _ = StrategicMap.Current.Players.GetPlayers(Player).AIBudget.ReturnExpense(GoldBudget, mana: ManaBudget);
            GoldBudget = 0;
            ManaBudget = 0;
        }
    }

    // AIGroup.pas:234
    public bool RequestTargetExpense(int gold, int mana)
    {
        int g = gold - GoldBudget;
        int m = mana - ManaBudget;
        Player? players = StrategicMap.Current.Players.GetPlayers(Player);
        return StrategicMap.Current.Players.GetPlayers(Player).AIBudget.RequestBudgetExpense(g, mana: m, budget: players.AIBudget.DefaultBudget, usePool: true);
    }

    // AIGroup.pas:243
    public bool MakeTargetExpense(int gold, int mana)
    {
        bool result;
        if (RequestTargetExpense(gold, mana))
        {
            ReturnTargetExpense();
            Player? players = StrategicMap.Current.Players.GetPlayers(Player);
            result = StrategicMap.Current.Players.GetPlayers(Player).AIBudget.BudgetExpenseMade(gold, mana, budget: players.AIBudget.DefaultBudget, mode: AIBudgetManager.ExpenseMode.PoolThenBudget);
            if (result)
            {
                GoldBudget = gold;
                ManaBudget = mana;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // AIGroup.pas:258
    public bool SetupTarget(MapCoordinate location, int movePoints, AIGroupTargetStatus targetType, byte mode, int transporterId, int gold, int mana)
    {
        bool result;
        if (location != MapCoordinate.None)
        {
            TargetLocation = location;
            TargetType = targetType;
            TargetMovePoints = movePoints;
            TargetMode = (byte)(mode << 0);
            if (targetType == AIGroupTargetStatus.Execute)
            {
                int movePointsMin = GetMovePointsMin();
                TargetMovePoints -= movePointsMin;
            }

            result = MakeTargetExpense(gold, mana);
        }
        else
        {
            TargetLocation = MapCoordinate.None;
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
            Troop? transporter = Transporter;
            if (transporter is not null)
            {
                _ = DeleteUnit(transporter);
            }
        }

        return result;
    }

    // AIGroup.pas:290
    public bool SetupTarget(AIGroupControlTarget? target, AIGroupTargetStatus targetType, byte mode)
    {
        bool result;
        if (target is not null)
        {
            TargetLocation = target.Location;
            TargetType = targetType;
            TargetMovePoints = target.MovePoints;
            TargetMode = mode;
            if (targetType == AIGroupTargetStatus.Execute)
            {
                int movePointsMin = GetMovePointsMin();
                TargetMovePoints -= movePointsMin;
            }
            result = MakeTargetExpense(mana: target.TargetMana, gold: target.TargetGold);
        }
        else
        {
            TargetLocation = MapCoordinate.None;
            TargetType = targetType;
            TargetMode = mode;
            TargetMovePoints = 0;
            ReturnTargetExpense();
            result = true;
        }

        if (target is not null && target.TransporterId != -1)
        {
            AddTransporter(StrategicMap.Current.UnitControl.FindUnit(target.TransporterId));
        }
        else
        {
            Troop? transporter = Transporter;
            if (transporter is not null)
            {
                _ = DeleteUnit(transporter);
            }
        }

        return result;
    }

    // AIGroup.pas:321
    public void ValidateEmpty()
    {
        bool empty = Units.Count == 0 && TransporterId == -1;
        if (empty && !Status.Contains(AIGroupStatuses.Empty))
        {
            Status |= AIGroupStatuses.Empty;
            _ = Release();
        }
        else
        {
            if (!empty && Status.Contains(AIGroupStatuses.Empty))
            {
                Status &= ~AIGroupStatuses.Empty;
                _ = AddRef();
            }
        }
    }

    // AIGroup.pas:337
    public void AddTransporter(Troop? troop)
    {
        Troop? oldTransporter = Transporter;
        if (oldTransporter != troop)
        {
            if (oldTransporter is not null)
            {
                oldTransporter.Group = null;
                oldTransporter.GroupId = 0;
                _ = oldTransporter.Release();
            }

            AIGroup? group = troop.Group;
            if (group is not null)
            {
                _ = group.DeleteUnit(troop);
                troop.GroupId = 0;
            }

            TransporterId = troop.UnitId;
            Transporter = troop;
            _ = Transporter.AddRef();
            troop.GroupId = GroupId;
            troop.Group = this;
            ValidateEmpty();
        }
    }

    // AIGroup.pas:446
    public void UnitProduced(ProductionControl control, Troop? troop)
    {
        if (Leader is not null)
        {
            MapCoordinate coordinate95 = GetLocation();
            sbyte x1 = coordinate95.X;
            sbyte y1 = coordinate95.Y;
            sbyte l1 = coordinate95.MapLevel;
            MapCoordinate coordinate96 = troop.GetLocation();
            sbyte x2 = coordinate96.X;
            sbyte y2 = coordinate96.Y;
            sbyte l2 = coordinate96.MapLevel;
            if (l1 == l2 && HexGeometry.DeltaHexToRadius(x1, y1, x2, y2) <= 2)
            {
                _ = AddUnit(troop);
            }

            Productions.DeleteUnit(troop);
        }
    }

    // AIGroup.pas:475
    public void Activate()
    {
        Assert(!Status.Contains(AIGroupStatuses.Activated), message: "", file: "AIGroup.pas", line: 479);
        Status |= AIGroupStatuses.Activated;
        sbyte player = Player;
        if (player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(player).AIGroups.AddChild(this);
        }

        Leader = Units.FindId(LeaderId);
        Assert(Transporter is not null || TransporterId == -1, message: "", file: "AIGroup.pas", line: 485);
    }

    // AIGroup.pas:488
    public void Deactivate()
    {
        if (!Status.Contains(AIGroupStatuses.Activated))
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, message: "Assertion failure", file: "D:\\AoWDev\\AoWE\\inc\\AIGroup.pas", line: 24094);
        }

        Status &= ~AIGroupStatuses.Activated;
        sbyte player = Player;
        if (player != -1)
        {
            _ = StrategicMap.Current.Players.GetPlayers(player).AIGroups.RemoveChild(this);
        }
    }

    // AIGroup.pas:498
    public void SetPlayer(sbyte player)
    {
        if (player != Player)
        {
            if (Status.Contains(AIGroupStatuses.Activated) && Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).AIGroups.RemoveChild(this);
            }

            Player = player;
            if (Status.Contains(AIGroupStatuses.Activated) && Player != -1)
            {
                _ = StrategicMap.Current.Players.GetPlayers(Player).AIGroups.AddChild(this);
            }
        }
    }

    // AIGroup.pas:123
    public virtual EngineClass GroupControlClass() => EngineClass.Of<AIGroupControl>();

    public virtual bool HuntingPlayer(sbyte player) => false;

    // AIGroup.pas:121
    public static GroupBehavior Behavior => GroupBehavior.None;

}
