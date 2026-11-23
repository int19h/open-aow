#nullable disable warnings
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

// cycles, group, status, location, army, army sprite, selection, targets, strengths, max
// strength, move table, immunity, predictor, move executer, recruitment, production locations,
// plugin, state
public class AIGroupControl : EngineObject
{

    // AIGroup.pas:973
    public AIGroupControl(CustomNode owner) : base(owner)
    {
        Targets = new AIGroupControlTargetList(null)
        {
            Holder = CreationHolder(),
            Control = this
        };
        RecruitmentGroups = new AIRecruitmentGroupList(null) { Holder = CreationHolder() };
        ProductionLocations = new AIGroupProductionLocationList(null) { Holder = CreationHolder() };
        CombatPredictor = new CombatPredictor(this);
    }

    private int _processCycles;

    public AIGroup Group { get; set; }

    // GroupControlClass constructs a control matching its AIGroup subclass.
    protected T GetGroup<T>() where T : AIGroup => (T)Group;

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private Statuses _status;

    public MapCoordinate Location { get; private set; }
    public Army? Army { get; private set; }

    public ArmyHexSprite ArmyHexSprite { get; private set; }

    public int Selection { get; private set; }

    public AIGroupControlTargetList Targets { get; }

    private readonly int[] _strength = new int[10];

    public int StrengthMax { get; private set; }

    public MovePointTable MovePointTable { get; private set; }

    private DamageTypes _immunityTypes;

    public CombatPredictor CombatPredictor { get; }
    public AIMoveExecutor? MoveExecutor { get; private set; }
    public AIRecruitmentGroupList RecruitmentGroups { get; }
    public AIGroupProductionLocationList ProductionLocations { get; }

    private AIGroupTask? _aiGroupTask;

    public int State { get; set; }

    // AIGroup.pas:983
    public override void Dispose()
    {
        DeactivatePlugin();
        CombatPredictor?.Dispose();
        Targets?.Dispose();
        RecruitmentGroups?.Dispose();
        ProductionLocations?.Dispose();
        _ = Group.Release();
        base.Dispose();
    }

    // AIGroup.pas:1084
    public virtual void MoveExecutorDone(object sender)
    {
        if (Group.Transporter is not null && !MoveExecutor.GetTransporting())
        {
            _ = Group.DeleteUnit(Group.Transporter);
        }

        if (MoveExecutor.Status == AIMoveExecutorStatus.Invalidated)
        {
            State = 0;
        }
        else
        {
            if (MoveExecutor.Status == AIMoveExecutorStatus.Failed)
            {
                if (!Group.Status.Contains(AIGroupStatuses.Split))
                {
                    if (!Split())
                    {
                        Done();
                    }
                }
                else
                {
                    Done();
                }
            }
        }
    }

    // AIGroup.pas:1165
    public virtual void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone) => Targets.Add(target);

    // AIGroup.pas:1155
    public virtual void RecruitmentGroupFoundEvent(AIRecruitmentGroup group, in bool searchDone) => RecruitmentGroups.Add(group);

    // AIGroup.pas:1160
    public virtual void ProductionLocationFoundEvent(AIGroupProductionLocation location, in bool searchDone) => ProductionLocations.Add(location);

    // AIGroup.pas:1266
    public virtual bool Validate()
    {
        if (!Group.Status.Contains(AIGroupStatuses.Done))
        {
            MapCoordinate coordinate97 = Group.GetLocation();
            int locationX = coordinate97.X;
            int locationY = coordinate97.Y;
            int locationMapLevel = coordinate97.MapLevel;
            Location = new MapCoordinate((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
            if (Location.X != -1)
            {
                ArmyHexSprite = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location).GetArmyHexSprite();
                _ = Army;
                _ = (byte)Selection;
                _ = RetrieveGroupArmy(Location.X, Location.Y, L: Location.MapLevel, out Army army, out int selection);
                Army = army;
                Selection = selection;
                UpdateGroupChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // AIGroup.pas:1246
    public virtual void ProcessStates() => Done();

    // AIGroup.pas:1251
    public virtual void ProcessStatesLooped() => Done();

    // AIGroup.pas:1170
    public virtual bool Split()
    {
        int x = default;
        int y = default;
        int l = default;
        bool result = false;
        bool finished = false;
        while (!finished)
        {
            int i = Group.Units.Count;
            bool found = false;
            while (!found && i != 0)
            {
                i -= 1;
                MapCoordinate coordinate98 = Group.Units.GetUnit(i).GetLocation();
                x = coordinate98.X;
                y = coordinate98.Y;
                l = coordinate98.MapLevel;
                found = x != Location.X || y != Location.Y || l != Location.MapLevel;
            }

            if (found)
            {
                GroupBehavior behavior = Group.GroupClass.Behavior;
                AIGroup? group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, relation: Group.IndependentRelation, behavior);
                group.Status |= AIGroupStatuses.Split;
                i = Group.Units.Count - 1;
                for (; i >= 0; i -= 1)
                {
                    MapCoordinate coordinate99 = Group.Units.GetUnit(i).GetLocation();
                    int unitX = coordinate99.X;
                    int unitY = coordinate99.Y;
                    int unitL = coordinate99.MapLevel;
                    if (x == unitX && y == unitY && l == unitL)
                    {
                        _ = group.AddUnit(Group.Units.GetUnit(i));
                    }
                }

                if (group.Units.Count != 0)
                {
                    Group.Status |= AIGroupStatuses.Split;
                    result = true;
                }

                _ = group.Release();
            }
            else
            {
                finished = true;
            }
        }

        return result;
    }

    // AIGroup.pas:1214
    public virtual void Activate()
    {
        State = 0;
        MoveExecutor = StrategicMap.Current.Players.GetPlayers(Group.Player).AIControl.MoveExecutor;
        StrategicMap.Current.AIExecutor.UseProcessTime(100);
        _status |= Statuses.Activated;
    }

    // AIGroup.pas:1222
    public virtual void Deactivate()
    {
        MoveExecutor.SetDoneHandler(null);
        _status &= ~Statuses.Activated;
    }

    // AIGroup.pas:1101
    public virtual void ResetME()
    {
        MoveExecutor.Reset();
        MoveExecutor.SetDoneHandler(MoveExecutorDone);
    }

    // AIGroup.pas:1107
    public virtual void SetupME()
    {
        ResetME();
        Troop? troop = Group.Leader;
        if (troop is not null)
        {
            _ = MoveExecutor.AddUnit(troop);
        }

        for (int i = 0, last = Group.Units.Count - 1; i <= last; i++)
        {
            if (Group.Units.GetUnit(i) != Group.Leader)
            {
                Troop? unit = Group.Units.GetUnit(i);
                _ = MoveExecutor.AddUnit(unit);
            }
        }

        if (!Group.GetAdjacent())
        {
            MoveExecutor.Mode &= ~AIMoveExecutorModes.Adjacent;
        }

        troop = Group.Transporter;
        if (troop is not null)
        {
            MoveExecutor.AddTransporter(troop);
        }
    }

    // AIGroup.pas:1124
    public virtual void ExecuteME(int X, int Y, int L)
    {
        MapField field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        if (field.GetPlayer() != -1 && field.GetPlayer() != Group.Player)
        {
            MoveExecutor.Mode |= AIMoveExecutorModes.Adjacent | AIMoveExecutorModes.Attack;
        }

        _ = MoveExecutor.ExecuteMove(X, Y, L);
    }

    // AIGroup.pas:994
    public void Done()
    {
        Group.Status |= AIGroupStatuses.Done;
        State = -1;
    }

    // AIGroup.pas:1000
    public int GetStrengthMax(DamageTypes damageTypes)
    {
        int result = 0;
        for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.NonSpecific; damageTypeOrdinal++)
        {
            DamageType damageType = (DamageType)damageTypeOrdinal;
            if (damageTypes.Contains(damageType.ToFlag()) && result < _strength[(int)damageType])
            {
                result = _strength[(int)damageType];
            }
        }

        return result;
    }

    // AIGroup.pas:1010
    public void UpdateGroupChanges()
    {
        if (Group.Status.Contains(AIGroupStatuses.Changed) || _processCycles == 1)
        {
            _immunityTypes = AbilityRules.AllDamageTypes;
            StrengthMax = 0;
            Array.Clear(_strength);
            if (Group.Units.Count != 0)
            {
                MovePointTable movePointTable2 = MovePointTable;
                movePointTable2.Fill(1);
                MovePointTable = movePointTable2;
            }
            else
            {
                MovePointTable movePointTable3 = MovePointTable;
                movePointTable3.Fill(-1);
                MovePointTable = movePointTable3;
            }

            for (int i = Group.Units.Count - 1; i >= 0; i--)
            {
                Troop? troop = Group.Units.GetUnit(i);
                _immunityTypes = troop.GetImmunityTypes() & _immunityTypes;
                troop.CreateMovePointTable(out MovePointTable movePointTable);
                MovePointTable movePointTable4 = MovePointTable;
                movePointTable4.Maximize(movePointTable);
                MovePointTable = movePointTable4;
                for (int damageTypeOrdinal = (int)DamageType.Fire; damageTypeOrdinal <= (int)DamageType.NonSpecific; damageTypeOrdinal++)
                {
                    DamageType damageType = (DamageType)damageTypeOrdinal;
                    int strength = troop.GetStrength(damageType);
                    _strength[(int)damageType] += strength;
                }

                int strengthMax = troop.GetStrengthMax(AbilityRules.AllDamageTypes);
                StrengthMax += strengthMax;
            }

            Group.Status &= ~AIGroupStatuses.Changed;
        }
    }

    // AIGroup.pas:1040
    public bool RetrieveGroupArmy(int X, int Y, int L, out Army? army, out int selection)
    {
        ArmyHexSprite armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
        if (armyHexSprite is not null)
        {
            army = armyHexSprite.Army;
            selection = armyHexSprite.Army.GetGroupSelection(Group.GroupId);
            return true;
        }
        else
        {
            selection = 0;
            army = null;
            return false;
        }
    }

    // AIGroup.pas:1059
    public void CombineIdleGroups()
    {
        Player? player = StrategicMap.Current.Players.GetPlayers(Group.Player);
        for (int i = player.AIGroups.Count - 1; i >= 0; i--)
        {
            AIGroup group = player.AIGroups.GetGroups(i);
            if (group != Group && group.EngineClass.ClassId == Group.EngineClass.ClassId && group.Status.Contains(AIGroupStatuses.Done))
            {
                switch (group.TargetType)
                {
                    case AIGroupTargetStatus.Idle:
                        MapCoordinate location = group.GetLocation();
                        if (location.MapLevel == Location.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, Location.X, Location.Y) <= 1)
                        {
                            _ = Group.AddUnits(group.Units);
                        }

                        break;
                    case AIGroupTargetStatus.None:
                    case AIGroupTargetStatus.Setup:
                    case AIGroupTargetStatus.Execute:
                    default:
                        break;
                }
            }
        }
    }

    // AIGroup.pas:1134
    public void ResetCP()
    {
        CombatPredictor.DeleteOpponents();
        for (int i = CombatPredictor.Sides[0].Count - 1; i >= 0; i--)
        {
            CombatPredictorUnit? units = CombatPredictor.Sides[0].GetUnits(i);
            if (Group.Units.IndexOf(units.Troop) == -1)
            {
                CombatPredictorUnit? units2 = CombatPredictor.Sides[0].GetUnits(i);
                CombatPredictor.DeleteUnit(units2.Troop);
            }
        }

        for (int i = Group.Units.Count - 1; i >= 0; i--)
        {
            Troop? troop = Group.Units.GetUnit(i);
            if (CombatPredictor.Sides[0].IndexOfUnit(troop) == -1)
            {
                Troop? fGroupUnit = Group.Units.GetUnit(i);
                CombatPredictor.AddUnit(fGroupUnit);
            }
        }
    }

    // AIGroup.pas:1147
    public void SetupCP(AIGroupControlTarget target)
    {
        ResetCP();
        if (StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location).GetMoveAction(Group.Player) != MoveAction.Negotiate)
        {
            CombatPredictor.InitializeAdjacentOpponents((byte)Group.Player, relation: Group.IndependentRelation.TruncateToByte(), target.Location.X, target.Location.Y, L: target.Location.MapLevel);
        }
    }

    // AIGroup.pas:1228
    public void ActivatePlugin(AIGroupTask aiGroupTask)
    {
        DeactivatePlugin();
        _aiGroupTask = aiGroupTask;
        _ = _aiGroupTask.AddRef();
        _ = _aiGroupTask.Activate(this);
    }

    // AIGroup.pas:1236
    public void DeactivatePlugin()
    {
        _ = (_aiGroupTask?.Release());
        _aiGroupTask = null;
    }

    // AIGroup.pas:1256
    public void MainProcessStates(bool looped)
    {
        try
        {
            UpdateGroupChanges();
            if (looped)
            {
                ProcessStatesLooped();
            }
            else
            {
                ProcessStates();
            }
        }
        catch
        {
            throw;
        }
    }

    // AIGroup.pas:1285
    public bool Process(bool looped)
    {
        _processCycles += 1;
        if (Validate())
        {
            try
            {
                bool busy = false;
                while (!busy && !Group.Status.Contains(AIGroupStatuses.Done) && StrategicMap.Current.AIExecutor.ProcessTimeLeft >= 0 && MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                {
                    if (_aiGroupTask is not null)
                    {
                        busy = _aiGroupTask.MainProcess();
                        if (!busy)
                        {
                            DeactivatePlugin();
                            if (StrategicMap.Current.AIExecutor.ProcessTimeLeft >= 0 && !Group.Status.Contains(AIGroupStatuses.Done))
                            {
                                MainProcessStates(looped);
                            }
                        }
                    }
                    else
                    {
                        MainProcessStates(looped);
                    }
                }
            }
            catch
            {
                Done();
            }

            return !Group.Status.Contains(AIGroupStatuses.Done);
        }
        else
        {
            Done();
            return false;
        }
    }

    // AIGroup.pas:1323
    public Troop? FindAvailableTransporter()
    {
        DoubleItemList<Troop> list = [];
        Player? player = StrategicMap.Current.Players.GetPlayers(Group.Player);
        for (int i = 0, last = player.Armies.Count - 1; i <= last; i++)
        {
            Troop? troop = player.Armies.GetArmy(i).Transporter(255);
            if (troop is not null && (troop.Group is null || troop.Group.EngineClass.ClassId == TransportAIGroup.ClassId))
            {
                MapCoordinate coordinate100 = troop.GetLocation();
                int locationX = coordinate100.X;
                int locationY = coordinate100.Y;
                int locationMapLevel = coordinate100.MapLevel;
                MapCoordinate location = new((sbyte)locationX, (sbyte)locationY, (sbyte)locationMapLevel);
                int distance = HexGeometry.DeltaHexToRadius(location.X, location.Y, Location.X, Location.Y);
                if (Location.MapLevel == location.MapLevel && distance < 30)
                {
                    _ = list.AddAbility(troop, distance);
                }
            }
        }

        Troop? result;
        if (list.Count != 0)
        {
            list.SortAscendingItemB();
            result = list[0].First;
        }
        else
        {
            result = null;
        }

        list?.Dispose();
        return result;
    }

    [Flags]
    public enum Statuses : byte
    {
        None = 0,
        Activated = 1 << 0,
        Loop = 1 << 1,
    }
}
