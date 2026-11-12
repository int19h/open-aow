// Translated from AoWE.pas.

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// control, mode, completion, costs, upkeep, productions, candidates, remaining strengths,
// remainder, requested strength, damage types, requested strengths, priority range, multiple,
// group ID, superiority, move-point priority, allow research, research priority, move types,
// minimum moves, path-strength factor
public partial class AIGroupProductionControl : AIGroupTask
{
    // AIGroup.pas:2440
    public AIGroupProductionControl(ECustomNode owner) : base(owner)
    {
        Productions = new AIGroupUnitProductionList(null) { Holder = CreationHolder() };
        _producibleUnits = new AIGroupUnitProductionList(null) { Holder = CreationHolder() };
    }

    private AIGroupControl _control;

    public AIGroupProductionControlMode Mode { get; set; }
    public bool Complete { get; private set; }

    private int _gold;
    private int _mana;
    private int _goldUpkeep;
    private int _manaUpkeep;

    public AIGroupUnitProductionList Productions { get; private set; }

    private AIGroupUnitProductionList _producibleUnits;
    private readonly int[] _remainingStrengths = new int[10];
    private int _remainingStrength;

    public int ProduceStrength { get; set; }

    private Set<DamageType> _damageTypes;
    private readonly int[] _produceStrengths = new int[10];
    private readonly int _priorityRange;
    private bool _multiple;
    private int _groupId;

    public int Superiority { get; set; }

    private int _movePointPriority;
    private bool _allowResearch;
    private int _researchPriority;
    private Set<MoveType> _moveTypes;
    private int _movesMin;
    private float _pathStrengthFactor;

    // AIGroup.pas:2447
    public override void Dispose()
    {
        Productions?.Dispose();
        Productions = null;
        _producibleUnits?.Dispose();
        _producibleUnits = null;
        base.Dispose();
    }

    // AIGroup.pas:2471
    public override bool GetBusy() => State != -1;

    // AIGroup.pas:2654
    public override bool Activate(AIGroupControl control)
    {
        _control = control;
        _remainingStrength = ProduceStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:2772
    public override bool Process()
    {
        int index;
        AIGroupUnitProduction production;
        bool finished;
        switch (State)
        {
            case 0:
                if (_control.Group.Productions.Count == 0)
                {
                    StrategicMap.Current.AIExecutor.UseProcessTime(500);
                    finished = false;
                    _control.ProductionLocations.Validate(_control);
                    while (!Complete && !finished)
                    {
                        UpdateProducibleUnits();
                        if (_producibleUnits.Count != 0)
                        {
                            index = 0;
                            while (true)
                            {
                                bool condition = _producibleUnits.Count > index + 1;
                                if (condition)
                                {
                                    var unitProductions = _producibleUnits.GetUnitProductions(index + 1);
                                    var fProducibleUnitsUnitProductions = _producibleUnits.GetUnitProductions(0);
                                    condition = unitProductions.Priority > fProducibleUnitsUnitProductions.Priority - _priorityRange;
                                }

                                if (!condition)
                                    break;
                                index += 1;
                            }

                            // { Original passes the last eligible index, not the candidate count. }
                            index = StrategicMap.Current.Random(index);
                            AddProduction(_producibleUnits.GetUnitProductions(index));
                            if (!_multiple)
                            {
                                finished = true;
                            }
                        }
                        else
                        {
                            finished = true;
                        }
                    }

                    if ((Productions.Count != 0 && Mode == AIGroupProductionControlMode.AutoProduce) || (Mode == AIGroupProductionControlMode.CompleteProduceOnly && Complete))
                    {
                        ExecuteProductions(_groupId);
                    }

                    Done();
                }
                else
                {
                    for (int I = 0, last = _control.Group.Productions.Count - 1; I <= last; I++)
                    {
                        production = new AIGroupUnitProduction(null) { Holder = CreationHolder() };
                        production.SetU(_control.Group.Productions.GetProductions(I).Control.Settings.Unit);
                        if (production.Unit != null)
                        {
                            production.Gold = 0;
                            production.Mana = 0;
                            production.GoldUpkeep = 0;
                            production.ManaUpkeep = 0;
                            var productions = _control.Group.Productions.GetProductions(I);
                            production.SetProductionLocation(_control.ProductionLocations.FindPC(productions.Control));
                            production.Priority = 0;
                            production.Research = false;
                            if (production.ProductionLocation != null)
                            {
                                AddProduction(production);
                            }
                        }

                        production.Release();
                    }

                    Done();
                }

                break;
            default:
                Done();
                break;
        }

        return false;
    }

    // AIGroup.pas:2647
    public virtual bool Setup(AIGroupControl control)
    {
        Reset();
        _groupId = control.Group.GroupId;
        return true;
    }

    // AIGroup.pas:2456
    public int GetProducedStrength() => ProduceStrength - _remainingStrength;

    // AIGroup.pas:2461
    public int GetProduceStrength(DamageType damageType) => _produceStrengths[(int)damageType];

    // AIGroup.pas:2466
    public void SetProduceStrength(DamageType damageType, int value)
    {
        _produceStrengths[(int)damageType] = value;
    }

    // AIGroup.pas:2476
    public int UnitProductionPriority(Unit unit)
    {
        bool useful;
        DamageType D;
        int result = -1;
        useful = unit.GetMoves() > _movesMin && (unit.GetMoveTypes() & _moveTypes) != Set<MoveType>.Of();
        useful = useful && unit.GetUnitMoraleValue(_control.Group.Player, _control.Group.IndependentRelation) >= 50;
        if (useful && _control.Group.Player == 0)
        {
            useful = _control.Group.IndependentRelation < 0 || unit.GetIndependentRelation() < 0 || unit.GetIndependentRelation() == _control.Group.IndependentRelation;
        }

        if (useful)
        {
            useful = _remainingStrength > 0 && unit.GetStrengthMax(_damageTypes) > 0;
            if (!useful)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    useful = _remainingStrengths[(int)D] > 0 && unit.GetStrength(D) > 0;
                    // { Original bug: a useful per-type strength returns the initial -1 priority. }
                    if (useful)
                    {
                        return result;
                    }
                }

                if (!useful && Superiority != -1)
                {
                    useful = (_control.CombatPredictor.Usefullness(unit) & 8388608) != 0;
                    if (useful)
                    {
                        result += 50;
                    }
                }
            }
        }

        if (useful)
        {
            result += RosterTables.GetUnitIndexAIPriority(unit.UnitResource.UnitResourceIndex(), _control.Group.Player, _control.Group.IndependentRelation, engine: GetEngine());
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // AIGroup.pas:2514
    public int GetCurrentProductionTime()
    {
        int result = -1;
        for (int I = 0, last = Productions.Count - 1; I <= last; I++)
        {
            if (Productions.GetUnitProductions(I).ProductionTime > result)
            {
                result = Productions.GetUnitProductions(I).ProductionTime;
            }
        }

        return result;
    }

    // AIGroup.pas:2524
    public int GetCompleteProductionTime()
    {
        int result = GetCurrentProductionTime();
        if (result != -1 && _remainingStrength > 0 && GetProducedStrength() > 0)
        {
            var producedStrength = GetProducedStrength();
            result += (int)((double)((double)((double)_remainingStrength / producedStrength) * result) + 0.999);
        }

        return result;
    }

    // AIGroup.pas:2531
    public void UpdateCompletion()
    {
        DamageType D;
        Complete = _remainingStrength <= 0;
        if (Complete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (_remainingStrengths[(int)D] > 0)
                {
                    Complete = false;
                    return;
                }
            }

            if (Superiority != -1)
            {
                _control.CombatPredictor.Execute();
                if (_control.CombatPredictor.GetSuperiority() < Superiority)
                {
                    Complete = false;
                }
            }
        }
    }

    // AIGroup.pas:2553
    public bool ExecuteProductions(int groupId)
    {
        AIGroupUnitProduction production;
        ProductionControl control;
        bool result = true;
        for (int I = 0, last = Productions.Count - 1; I <= last; I++)
        {
            production = Productions.GetUnitProductions(I);
            control = production.ProductionLocation.ProductionControl;
            if (!production.Research)
            {
                var unitResourceIndex = production.Unit.UnitResource.UnitResourceIndex();
                if (control.ExecuteAIProduction(control.UnitProductions.IndexOf(unitResourceIndex), groupId))
                {
                    StrategicMap.Current.Players.GetPlayers(_control.Group.Player).AIBudget.ProductionExpenseMade(production.Gold, production.GoldUpkeep, production.Mana, production.ManaUpkeep);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                var unitResourceIndex = production.Unit.UnitResource.UnitResourceIndex();
                if (control.ExecuteAIResearch(control.UnitProductions.IndexOf(unitResourceIndex)))
                {
                    StrategicMap.Current.Players.GetPlayers(_control.Group.Player).AIBudget.ProductionExpenseMade(production.Gold, production.GoldUpkeep, production.Mana, production.ManaUpkeep);
                }
            }
        }

        return result;
    }

    // AIGroup.pas:2581
    public void AddProduction(AIGroupUnitProduction production)
    {
        DamageType D;
        Productions.AddChild(production);
        _gold += production.Gold;
        _mana += production.Mana;
        // { Original bug: upkeep is based on total costs rather than accumulated upkeep. }
        _goldUpkeep = _gold + production.GoldUpkeep;
        _manaUpkeep = _mana + production.ManaUpkeep;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = production.Unit.GetStrength(D);
            _remainingStrengths[(int)D] -= strength;
        }

        var strengthMax = production.Unit.GetStrengthMax(_damageTypes);
        _remainingStrength -= strengthMax;
        if (Superiority != -1)
        {
            _control.CombatPredictor.AddUnit(production.Unit);
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2599
    public void DeleteProduction(int index)
    {
        AIGroupUnitProduction production;
        DamageType D;
        if (index >= 0 && index < Productions.Count)
        {
            production = Productions.GetUnitProductions(index);
            _gold -= production.Gold;
            _mana -= production.Mana;
            _goldUpkeep -= production.GoldUpkeep;
            _manaUpkeep -= production.ManaUpkeep;
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = production.Unit.GetStrength(D);
                _remainingStrengths[(int)D] += strength;
            }

            var strengthMax = production.Unit.GetStrengthMax(_damageTypes);
            _remainingStrength += strengthMax;
            if (Superiority != -1)
            {
                _control.CombatPredictor.DeleteUnit(production.Unit);
            }

            Productions.RemoveChild(production);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2621
    public void Reset()
    {
        Productions.Clear();
        State = 0;
        _movesMin = 0;
        Superiority = -1;
        // { Original clears the production list twice and leaves the candidate list alone. }
        Productions.Clear();
        _gold = 0;
        _mana = 0;
        _goldUpkeep = 0;
        _manaUpkeep = 0;
        ProduceStrength = 0;
        _damageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        Array.Clear(_produceStrengths);
        _moveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        Complete = false;
        Mode = AIGroupProductionControlMode.AutoProduce;
        _multiple = false;
        _groupId = 0;
        _movePointPriority = -5;
        _researchPriority = -500;
        _allowResearch = true;
        _pathStrengthFactor = (float)0.8;
    }

    // AIGroup.pas:2662
    public bool ListProducibleUnits(AIGroupProductionLocation location)
    {
        AIGroupUnitProduction production;
        int priority;
        Unit testUnit;
        Unit newUnit;
        bool result = false;
        if (location.ProductionControl.Status == ProductionControlStatus.Idle)
        {
            testUnit = new Unit(null) { Holder = CreationHolder() };
            for (int I = 0, last = location.ProductionControl.UnitProductions.Count - 1; I <= last; I++)
            {
                if (location.ProductionControl.UnitProductions.GetItem(I).UnitIndex != -1)
                {
                    if (location.ProductionControl.UnitProductions.GetItem(I).Researched)
                    {
                        var unitProductionManaUpkeep = location.ProductionControl.UnitProductionManaUpkeep(I);
                        var unitProductionMana = location.ProductionControl.UnitProductionMana(I);
                        var unitProductionGoldUpkeep = location.ProductionControl.UnitProductionGoldUpkeep(I);
                        var unitProductionGold = location.ProductionControl.UnitProductionGold(I);
                        if (StrategicMap.Current.Players.GetPlayers(_control.Group.Player).AIBudget.RequestProductionExpense(unitProductionGold + _gold, unitProductionGoldUpkeep + _goldUpkeep, unitProductionMana + _mana, unitProductionManaUpkeep + _manaUpkeep))
                        {
                            var unitProduction = location.ProductionControl.UnitProductions.GetItem(I);
                            testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
                            priority = UnitProductionPriority(testUnit);
                            if (priority != -1)
                            {
                                newUnit = new Unit(null) { Holder = CreationHolder() };
                                var fUnitProductionsItem = location.ProductionControl.UnitProductions.GetItem(I);
                                newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(fUnitProductionsItem.UnitIndex));
                                production = new AIGroupUnitProduction(null) { Holder = CreationHolder() };
                                production.SetU(newUnit);
                                production.Gold = location.ProductionControl.UnitProductionGold(I);
                                production.Mana = location.ProductionControl.UnitProductionMana(I);
                                production.GoldUpkeep = location.ProductionControl.UnitProductionGoldUpkeep(I);
                                production.ManaUpkeep = location.ProductionControl.UnitProductionManaUpkeep(I);
                                production.SetProductionLocation(location);
                                production.Priority = _movePointPriority * location.MovePoints + priority;
                                production.Research = false;
                                production.ProductionTime = location.ProductionControl.UnitProductionTime(I);
                                _producibleUnits.AddChild(production);
                                production.Release();
                                newUnit.Release();
                                result = true;
                            }
                        }
                    }
                    else
                    {
                        if (_allowResearch)
                        {
                            var unitResearchMana = location.ProductionControl.UnitResearchMana(I);
                            var unitResearchGold = location.ProductionControl.UnitResearchGold(I);
                            if (StrategicMap.Current.Players.GetPlayers(_control.Group.Player).AIBudget.RequestProductionExpense(unitResearchGold + _gold, _goldUpkeep, unitResearchMana + _mana, _manaUpkeep))
                            {
                                var item = location.ProductionControl.UnitProductions.GetItem(I);
                                testUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(item.UnitIndex));
                                priority = UnitProductionPriority(testUnit);
                                if (priority != -1)
                                {
                                    newUnit = new Unit(null) { Holder = CreationHolder() };
                                    var fUnitProductionsItem = location.ProductionControl.UnitProductions.GetItem(I);
                                    newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(fUnitProductionsItem.UnitIndex));
                                    production = new AIGroupUnitProduction(null) { Holder = CreationHolder() };
                                    production.SetU(newUnit);
                                    production.Gold = location.ProductionControl.UnitResearchGold(I);
                                    production.Mana = location.ProductionControl.UnitResearchMana(I);
                                    production.GoldUpkeep = location.ProductionControl.UnitProductionGoldUpkeep(I);
                                    production.ManaUpkeep = location.ProductionControl.UnitProductionManaUpkeep(I);
                                    production.SetProductionLocation(location);
                                    production.Priority = _movePointPriority * location.MovePoints + priority + _researchPriority;
                                    production.Research = true;
                                    _producibleUnits.AddChild(production);
                                    production.Release();
                                    newUnit.Release();
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }

            testUnit?.Dispose();
        }

        return result;
    }

    // AIGroup.pas:2743
    public bool ValidProductionLocation(AIGroupProductionLocation location)
    {
        bool result;
        if (location.PathStrength != 0)
        {
            result = _control.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)location.PathStrength * _pathStrengthFactor) || _control.Group.GetMovePointsMin() >= location.MovePoints;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AIGroup.pas:2761
    public void UpdateProducibleUnits()
    {
        _producibleUnits.Clear();
        for (int I = 0, last = _control.ProductionLocations.Count - 1; I <= last; I++)
        {
            if (ValidProductionLocation(_control.ProductionLocations.GetProductionLocation(I)))
            {
                ListProducibleUnits(_control.ProductionLocations.GetProductionLocation(I));
            }
        }

        _producibleUnits.SortChildren<AIGroupUnitProduction>(CompareProducibleUnits);
    }

    private static int CompareProducibleUnits(AIGroupUnitProduction item1, AIGroupUnitProduction item2)
    {
        return item2.Priority - item1.Priority;
    }

    // intf_P5_07.inc:537

    // intf_P5_07.inc:537

}

public enum AIGroupProductionControlMode : byte
{
    None = 0, AutoProduce = 1, CompleteProduceOnly = 2
}
