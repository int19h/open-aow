// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// cycles, group, status, location, army, army sprite, selection, targets, strengths, max
// strength, move table, immunity, predictor, move executer, recruitment, production locations,
// plugin, state
public partial class AIGroupControl : EngineObject
{

    // AIGroup.pas:973
    public AIGroupControl(ECustomNode owner) : base(owner)
    {
        Targets = new AIGroupControlTargetList(null);
        Targets.Control = this;
        RecruitmentGroups = new AIRecruitmentGroupList(null);
        ProductionLocations = new AIGroupProductionLocationList(null);
        CombatPredictor = new CombatPredictor();
    }

    private int _processCycles;

    public AIGroup Group { get; set; }

    private Set<StatusElement> _status;
    public MapCoordinate Location;
    public Army Army;

    public ArmyHexSprite ArmyHexSprite { get; private set; }

    public byte Selection;

    public AIGroupControlTargetList Targets { get; }

    private readonly int[] _strength = new int[10];

    public int StrengthMax { get; private set; }

    public MovePointTable MovePointTable;
    private Set<DamageType> _immunityTypes;

    public CombatPredictor CombatPredictor { get; }
    public AIMoveExecutor MoveExecutor { get; private set; }
    public AIRecruitmentGroupList RecruitmentGroups { get; }
    public AIGroupProductionLocationList ProductionLocations { get; }

    private AIGroupTask _aiGroupTask;

    public int State { get; set; }

    // AIGroup.pas:983
    public override void Dispose()
    {
        DeactivatePlugin();
        CombatPredictor?.Dispose();
        Targets?.Dispose();
        RecruitmentGroups?.Dispose();
        ProductionLocations?.Dispose();
        Group.Release();
        base.Dispose();
    }

    // AIGroup.pas:1084
    public virtual void MoveExecutorDone(object sender)
    {
        if (Group.Transporter != null && !MoveExecutor.GetTransporting())
        {
            Group.DeleteUnit(Group.Transporter);
        }

        if (MoveExecutor.Status == AIMoveExecutorStatus.Invalidated)
        {
            State = 0;
        }
        else
        {
            if (MoveExecutor.Status == AIMoveExecutorStatus.Failed)
            {
                if (!Group.Status.Contains(AIGroupStatusElement.Split))
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
    public virtual void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        Targets.Add(target);
    }

    // AIGroup.pas:1155
    public virtual void RecruitmentGroupFoundEvent(AIRecruitmentGroup group, ref bool searchDone)
    {
        RecruitmentGroups.Add(group);
    }

    // AIGroup.pas:1160
    public virtual void ProductionLocationFoundEvent(AIGroupProductionLocation location, ref bool searchDone)
    {
        ProductionLocations.Add(location);
    }

    // AIGroup.pas:1266
    public virtual bool Validate()
    {
        bool result;
        if (!Group.Status.Contains(AIGroupStatusElement.Done))
        {
            Group.GetLocation(ref Location.X, ref Location.Y, ref Location.MapLevel);
            if (Location.X != -1)
            {
                ArmyHexSprite = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref Location)).GetArmyHexSprite();
                RetrieveGroupArmy(Location.X, Location.Y, Location.MapLevel, ref Army, ref Selection);
                UpdateGroupChanges();
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:1246
    public virtual void ProcessStates()
    {
        Done();
    }

    // AIGroup.pas:1251
    public virtual void ProcessStatesLooped()
    {
        Done();
    }

    // AIGroup.pas:1170
    public virtual bool Split()
    {
        int I;
        AIGroup group;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        sbyte unitX = default;
        sbyte unitY = default;
        sbyte unitL = default;
        bool finished;
        bool found;
        bool result = false;
        finished = false;
        while (!finished)
        {
            I = Group.Count;
            found = false;
            while (!found && I != 0)
            {
                I -= 1;
                Group.GetUnit(I).GetLocation(ref X, ref Y, ref L);
                found = X != Location.X || Y != Location.Y || L != Location.MapLevel;
            }

            if (found)
            {
                var behavior = Group.GroupClass.Behavior;
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, behavior);
                group.Status = group.Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Split);
                I = Group.Count - 1;
                for (; I >= 0; I -= 1)
                {
                    Group.GetUnit(I).GetLocation(ref unitX, ref unitY, ref unitL);
                    if (X == unitX && Y == unitY && L == unitL)
                    {
                        group.AddUnit(Group.GetUnit(I));
                    }
                }

                if (group.Count != 0)
                {
                    Group.Status = Group.Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Split);
                    result = true;
                }

                group.Release();
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
        _status = _status + Set<StatusElement>.Of(StatusElement.Activated);
    }

    // AIGroup.pas:1222
    public virtual void Deactivate()
    {
        MoveExecutor.ClearDoneHandler();
        _status = _status - Set<StatusElement>.Of(StatusElement.Activated);
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
        Troop troop;
        ResetME();
        troop = Group.Leader;
        if (troop != null)
        {
            MoveExecutor.AddUnit(troop);
        }

        for (int I = 0, last = Group.Count - 1; I <= last; I++)
        {
            if (Group.GetUnit(I) != Group.Leader)
            {
                var unit = Group.GetUnit(I);
                MoveExecutor.AddUnit(unit);
            }
        }

        if (!Group.GetAdjacent())
        {
            MoveExecutor.Mode = MoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
        }

        troop = Group.Transporter;
        if (troop != null)
        {
            MoveExecutor.AddTransporter(troop);
        }
    }

    // AIGroup.pas:1124
    public virtual void ExecuteME(sbyte X, sbyte Y, sbyte L)
    {
        MapField field;
        field = StrategicMap.Current.MapContainer.GetField(X, Y, L);
        if (field.GetPlayer() != -1 && field.GetPlayer() != Group.Player)
        {
            MoveExecutor.Mode = MoveExecutor.Mode + Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent, AIMoveExecutorModeElement.Attack);
        }

        MoveExecutor.ExecuteMove(X, Y, L);
    }

    // AIGroup.pas:994
    public void Done()
    {
        Group.Status = Group.Status + Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Done);
        State = -1;
    }

    // AIGroup.pas:1000
    public int GetStrengthMax(Set<DamageType> damageTypes)
    {
        DamageType damageType;
        int result = 0;
        for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
        {
            damageType = (DamageType)damageTypeOrdinal;
            if (damageTypes.Contains(damageType) && result < _strength[(int)damageType])
            {
                result = _strength[(int)damageType];
            }
        }

        return result;
    }

    // AIGroup.pas:1010
    public void UpdateGroupChanges()
    {
        DamageType damageType;
        Troop troop;
        MovePointTable movePointTable = default;
        if (Group.Status.Contains(AIGroupStatusElement.Changed) || _processCycles == 1)
        {
            _immunityTypes = AbilityRules.s_allDamageTypes;
            StrengthMax = 0;
            Array.Clear(_strength);
            if (Group.Count != 0)
            {
                FillBytes(Bytes.Of(ref MovePointTable, 256), 1);
            }
            else
            {
                FillBytes(Bytes.Of(ref MovePointTable, 256), -1);
            }

            for (int I = Group.Count - 1; I >= 0; I--)
            {
                troop = Group.GetUnit(I);
                _immunityTypes = troop.GetImmunityTypes() & _immunityTypes;
                troop.CreateMovePointTable(ref movePointTable);
                AbilityRules.MaximizeMovePointTable(Bytes.Of(ref MovePointTable), Bytes.Of(ref movePointTable));
                for (int damageTypeOrdinal = (int)(DamageType)0; damageTypeOrdinal <= (int)(DamageType)9; damageTypeOrdinal++)
                {
                    damageType = (DamageType)damageTypeOrdinal;
                    var strength = troop.GetStrength(damageType);
                    _strength[(int)damageType] += strength;
                }

                var strengthMax = troop.GetStrengthMax(AbilityRules.s_allDamageTypes);
                StrengthMax += strengthMax;
            }

            Group.Status = Group.Status - Set<AIGroupStatusElement>.Of(AIGroupStatusElement.Changed);
        }
    }

    // AIGroup.pas:1040
    public bool RetrieveGroupArmy(sbyte X, sbyte Y, sbyte L, ref Army army, ref byte selection)
    {
        bool result;
        ArmyHexSprite armyHexSprite;
        armyHexSprite = StrategicMap.Current.MapContainer.GetField(X, Y, L).GetArmyHexSprite();
        if (armyHexSprite != null)
        {
            army = armyHexSprite.Army;
            selection = armyHexSprite.Army.GetGroupSelection(Group.GroupId);
            result = true;
        }
        else
        {
            selection = 0;
            army = null;
            result = false;
        }

        return result;
    }

    // AIGroup.pas:1059
    public void CombineIdleGroups()
    {
        Player player;
        AIGroup group;
        MapCoordinate location = default;
        player = StrategicMap.Current.Players.GetPlayers(Group.Player);
        for (int I = player.AIGroups.Count - 1; I >= 0; I--)
        {
            group = player.AIGroups.GetGroups(I);
            if (group != Group && group.EngineClass.ClassId == Group.EngineClass.ClassId && group.Status.Contains(AIGroupStatusElement.Done))
            {
                switch (group.TargetType)
                {
                    case AIGroupTargetStatus.Idle:
                        Unsafe.As<MapCoordinate, int>(ref location) = group.GetLocation();
                        if (location.MapLevel == Location.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, Location.X, Location.Y) <= 1)
                        {
                            Group.AddUnits(group);
                        }

                        break;
                }
            }
        }
    }

    // AIGroup.pas:1134
    public void ResetCP()
    {
        CombatPredictor.DeleteOpponents();
        for (int I = CombatPredictor.Sides[0].Count - 1; I >= 0; I--)
        {
            var units = CombatPredictor.Sides[0].GetUnits(I);
            if (Group.IndexOf(units.Troop) == -1)
            {
                var units2 = CombatPredictor.Sides[0].GetUnits(I);
                CombatPredictor.DeleteUnit(units2.Troop);
            }
        }

        for (int I = Group.Count - 1; I >= 0; I--)
        {
            var troop = Group.GetUnit(I);
            if (CombatPredictor.Sides[0].IndexOfUnit(troop) == -1)
            {
                var fGroupUnit = Group.GetUnit(I);
                CombatPredictor.AddUnit(fGroupUnit);
            }
        }
    }

    // AIGroup.pas:1147
    public void SetupCP(AIGroupControlTarget target)
    {
        ResetCP();
        if (StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref target.Location)).GetMoveAction(Group.Player) != MoveAction.Negotiate)
        {
            CombatPredictor.InitializeAdjacentOpponents((byte)Group.Player, (byte)Group.IndependentRelation, target.Location.X, target.Location.Y, target.Location.MapLevel);
        }
    }

    // AIGroup.pas:1228
    public void ActivatePlugin(AIGroupTask aiGroupTask)
    {
        DeactivatePlugin();
        _aiGroupTask = aiGroupTask;
        _aiGroupTask.AddRef();
        _aiGroupTask.Activate(this);
    }

    // AIGroup.pas:1236
    public void DeactivatePlugin()
    {
        if (this._aiGroupTask != null)
        {
            this._aiGroupTask.Release();
            this._aiGroupTask = null;
        }
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
        bool result;
        bool busy;
        _processCycles += 1;
        if (Validate())
        {
            try
            {
                busy = false;
                while (!busy && !Group.Status.Contains(AIGroupStatusElement.Done) && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() >= 0 && MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                {
                    if (_aiGroupTask != null)
                    {
                        busy = _aiGroupTask.MainProcess();
                        if (!busy)
                        {
                            DeactivatePlugin();
                            if (StrategicMap.Current.AIExecutor.GetProcessTimeLeft() >= 0 && !Group.Status.Contains(AIGroupStatusElement.Done))
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

            result = !Group.Status.Contains(AIGroupStatusElement.Done);
        }
        else
        {
            Done();
            result = false;
        }

        return result;
    }

    // AIGroup.pas:1323
    public Troop FindAvailableTransporter()
    {
        Troop result;
        int distance;
        Troop troop;
        DoubleItemList<Troop> list;
        MapCoordinate location = default;
        Player player;
        list = new DoubleItemList<Troop>();
        player = StrategicMap.Current.Players.GetPlayers(Group.Player);
        for (int I = 0, last = player.Armies.Count - 1; I <= last; I++)
        {
            troop = player.Armies.GetArmy(I).Transporter(255);
            if (troop != null && (troop.Group == null || troop.Group.EngineClass.ClassId == 136295))
            {
                troop.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
                distance = HexGeometry.DeltaHexToRadius(location.X, location.Y, Location.X, Location.Y);
                if (Location.MapLevel == location.MapLevel && distance < 30)
                {
                    list.AddAbility(troop, distance);
                }
            }
        }

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

    public enum StatusElement : byte
    {
        Activated = 0, Loop = 1
    }

    // intf_P5_07.inc:259

}
