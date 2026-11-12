// Translated from AoWE.pas.
using Aow.Engine.Buffers;

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
        Productions = new AIGroupUnitProductionList(null);
        ProducibleUnits = new AIGroupUnitProductionList(null);
    }

    public AIGroupControl Control;
    public AIGroupProductionControlMode Mode;
    public bool Complete;
    public int Gold;
    public int Mana;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public AIGroupUnitProductionList Productions;
    public AIGroupUnitProductionList ProducibleUnits;
    public Int32Array10 RemainingStrengths;
    public int RemainingStrength;
    public int ProduceStrength;
    public Set<DamageType> DamageTypes;
    public Int32Array10 ProduceStrengths;
    public int PriorityRange;
    public bool Multiple;
    public int GroupId;
    public int Superiority;
    public int MovePointPriority;
    public bool AllowResearch;
    public int ResearchPriority;
    public Set<MoveType> MoveTypes;
    public int MovesMin;
    public float PathStrengthFactor;

    // AIGroup.pas:2447
    public override void Dispose()
    {
        Productions?.Dispose();
        Productions = null;
        ProducibleUnits?.Dispose();
        ProducibleUnits = null;
        base.Dispose();
    }

    // AIGroup.pas:2471
    public override bool GetBusy() => State != -1;

    // AIGroup.pas:2654
    public override bool Activate(AIGroupControl control)
    {
        Control = control;
        RemainingStrength = ProduceStrength;
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
                if (Control.Group.Productions.Count == 0)
                {
                    AoWHSMap.Current.AIExecutor.UseProcessTime(500);
                    finished = false;
                    Control.ProductionLocations.Validate(Control);
                    while (!Complete && !finished)
                    {
                        UpdateProducibleUnits();
                        if (ProducibleUnits.Count != 0)
                        {
                            index = 0;
                            while (true)
                            {
                                bool condition = ProducibleUnits.Count > index + 1;
                                if (condition)
                                {
                                    var unitProductions = ProducibleUnits.GetUnitProductions(index + 1);
                                    var fProducibleUnitsUnitProductions = ProducibleUnits.GetUnitProductions(0);
                                    condition = unitProductions.Priority > fProducibleUnitsUnitProductions.Priority - PriorityRange;
                                }

                                if (!condition)
                                    break;
                                index += 1;
                            }

                            // { Original passes the last eligible index, not the candidate count. }
                            index = AoWHSMap.Current.Random(index);
                            AddProduction(ProducibleUnits.GetUnitProductions(index));
                            if (!Multiple)
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
                        ExecuteProductions(GroupId);
                    }

                    Done();
                }
                else
                {
                    for (int I = 0, last = Control.Group.Productions.Count - 1; I <= last; I++)
                    {
                        production = new AIGroupUnitProduction(null);
                        production.SetU(Control.Group.Productions.GetProductions(I).Control.Settings.Unit);
                        if (production.Unit != null)
                        {
                            production.Gold = 0;
                            production.Mana = 0;
                            production.GoldUpkeep = 0;
                            production.ManaUpkeep = 0;
                            var productions = Control.Group.Productions.GetProductions(I);
                            production.SetProductionLocation(Control.ProductionLocations.FindPC(productions.Control));
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
        GroupId = control.Group.GroupId;
        return true;
    }

    // AIGroup.pas:2456
    public int GetProducedStrength() => ProduceStrength - RemainingStrength;

    // AIGroup.pas:2461
    public int GetProduceStrength(DamageType damageType) => ProduceStrengths[(int)damageType];

    // AIGroup.pas:2466
    public void SetProduceStrength(DamageType damageType, int value)
    {
        ProduceStrengths[(int)damageType] = value;
    }

    // AIGroup.pas:2476
    public int UnitProductionPriority(Unit unit)
    {
        bool useful;
        DamageType D;
        int result = -1;
        useful = unit.GetMoves() > MovesMin && (unit.GetMoveTypes() & MoveTypes) != Set<MoveType>.Of();
        useful = useful && unit.GetUnitMoraleValue(Control.Group.Player, Control.Group.IndependentRelation) >= 50;
        if (useful && Control.Group.Player == 0)
        {
            useful = Control.Group.IndependentRelation < 0 || unit.GetIndependentRelation() < 0 || unit.GetIndependentRelation() == Control.Group.IndependentRelation;
        }

        if (useful)
        {
            useful = RemainingStrength > 0 && unit.GetStrengthMax(DamageTypes) > 0;
            if (!useful)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    useful = RemainingStrengths[(int)D] > 0 && unit.GetStrength(D) > 0;
                    // { Original bug: a useful per-type strength returns the initial -1 priority. }
                    if (useful)
                    {
                        return result;
                    }
                }

                if (!useful && Superiority != -1)
                {
                    useful = (Control.CombatPredictor.Usefullness(unit) & 8388608) != 0;
                    if (useful)
                    {
                        result += 50;
                    }
                }
            }
        }

        if (useful)
        {
            result += RosterTables.GetUnitIndexAIPriority(unit.UnitResource.UnitResourceIndex(), Control.Group.Player, Control.Group.IndependentRelation);
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
        if (result != -1 && RemainingStrength > 0 && GetProducedStrength() > 0)
        {
            var producedStrength = GetProducedStrength();
            result += (int)((double)((double)((double)RemainingStrength / producedStrength) * result) + 0.999);
        }

        return result;
    }

    // AIGroup.pas:2531
    public void UpdateCompletion()
    {
        DamageType D;
        Complete = RemainingStrength <= 0;
        if (Complete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (RemainingStrengths[(int)D] > 0)
                {
                    Complete = false;
                    return;
                }
            }

            if (Superiority != -1)
            {
                Control.CombatPredictor.Execute();
                if (Control.CombatPredictor.GetSuperiority() < Superiority)
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
                    AoWHSMap.Current.Players.GetPlayers(Control.Group.Player).AIBudget.ProductionExpenseMade(production.Gold, production.GoldUpkeep, production.Mana, production.ManaUpkeep);
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
                    AoWHSMap.Current.Players.GetPlayers(Control.Group.Player).AIBudget.ProductionExpenseMade(production.Gold, production.GoldUpkeep, production.Mana, production.ManaUpkeep);
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
        Gold += production.Gold;
        Mana += production.Mana;
        // { Original bug: upkeep is based on total costs rather than accumulated upkeep. }
        GoldUpkeep = Gold + production.GoldUpkeep;
        ManaUpkeep = Mana + production.ManaUpkeep;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = production.Unit.GetStrength(D);
            RemainingStrengths[(int)D] -= strength;
        }

        var strengthMax = production.Unit.GetStrengthMax(DamageTypes);
        RemainingStrength -= strengthMax;
        if (Superiority != -1)
        {
            Control.CombatPredictor.AddUnit(production.Unit);
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
            Gold -= production.Gold;
            Mana -= production.Mana;
            GoldUpkeep -= production.GoldUpkeep;
            ManaUpkeep -= production.ManaUpkeep;
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = production.Unit.GetStrength(D);
                RemainingStrengths[(int)D] += strength;
            }

            var strengthMax = production.Unit.GetStrengthMax(DamageTypes);
            RemainingStrength += strengthMax;
            if (Superiority != -1)
            {
                Control.CombatPredictor.DeleteUnit(production.Unit);
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
        MovesMin = 0;
        Superiority = -1;
        // { Original clears the production list twice and leaves the candidate list alone. }
        Productions.Clear();
        Gold = 0;
        Mana = 0;
        GoldUpkeep = 0;
        ManaUpkeep = 0;
        ProduceStrength = 0;
        DamageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        ProduceStrengths = default;
        MoveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        Complete = false;
        Mode = AIGroupProductionControlMode.AutoProduce;
        Multiple = false;
        GroupId = 0;
        MovePointPriority = -5;
        ResearchPriority = -500;
        AllowResearch = true;
        PathStrengthFactor = (float)0.8;
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
            testUnit = new Unit(null);
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
                        if (AoWHSMap.Current.Players.GetPlayers(Control.Group.Player).AIBudget.RequestProductionExpense(unitProductionGold + Gold, unitProductionGoldUpkeep + GoldUpkeep, unitProductionMana + Mana, unitProductionManaUpkeep + ManaUpkeep))
                        {
                            var unitProduction = location.ProductionControl.UnitProductions.GetItem(I);
                            testUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex));
                            priority = UnitProductionPriority(testUnit);
                            if (priority != -1)
                            {
                                newUnit = new Unit(null);
                                var fUnitProductionsItem = location.ProductionControl.UnitProductions.GetItem(I);
                                newUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(fUnitProductionsItem.UnitIndex));
                                production = new AIGroupUnitProduction(null);
                                production.SetU(newUnit);
                                production.Gold = location.ProductionControl.UnitProductionGold(I);
                                production.Mana = location.ProductionControl.UnitProductionMana(I);
                                production.GoldUpkeep = location.ProductionControl.UnitProductionGoldUpkeep(I);
                                production.ManaUpkeep = location.ProductionControl.UnitProductionManaUpkeep(I);
                                production.SetProductionLocation(location);
                                production.Priority = MovePointPriority * location.MovePoints + priority;
                                production.Research = false;
                                production.ProductionTime = location.ProductionControl.UnitProductionTime(I);
                                ProducibleUnits.AddChild(production);
                                production.Release();
                                newUnit.Release();
                                result = true;
                            }
                        }
                    }
                    else
                    {
                        if (AllowResearch)
                        {
                            var unitResearchMana = location.ProductionControl.UnitResearchMana(I);
                            var unitResearchGold = location.ProductionControl.UnitResearchGold(I);
                            if (AoWHSMap.Current.Players.GetPlayers(Control.Group.Player).AIBudget.RequestProductionExpense(unitResearchGold + Gold, GoldUpkeep, unitResearchMana + Mana, ManaUpkeep))
                            {
                                var item = location.ProductionControl.UnitProductions.GetItem(I);
                                testUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(item.UnitIndex));
                                priority = UnitProductionPriority(testUnit);
                                if (priority != -1)
                                {
                                    newUnit = new Unit(null);
                                    var fUnitProductionsItem = location.ProductionControl.UnitProductions.GetItem(I);
                                    newUnit.SetUnitResource(AoWHSSet.Current.UnitResources.GetUnitResource(fUnitProductionsItem.UnitIndex));
                                    production = new AIGroupUnitProduction(null);
                                    production.SetU(newUnit);
                                    production.Gold = location.ProductionControl.UnitResearchGold(I);
                                    production.Mana = location.ProductionControl.UnitResearchMana(I);
                                    production.GoldUpkeep = location.ProductionControl.UnitProductionGoldUpkeep(I);
                                    production.ManaUpkeep = location.ProductionControl.UnitProductionManaUpkeep(I);
                                    production.SetProductionLocation(location);
                                    production.Priority = MovePointPriority * location.MovePoints + priority + ResearchPriority;
                                    production.Research = true;
                                    ProducibleUnits.AddChild(production);
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
            result = Control.Group.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)location.PathStrength * PathStrengthFactor) || Control.Group.GetMovePointsMin() >= location.MovePoints;
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
        ProducibleUnits.Clear();
        for (int I = 0, last = Control.ProductionLocations.Count - 1; I <= last; I++)
        {
            if (ValidProductionLocation(Control.ProductionLocations.GetProductionLocation(I)))
            {
                ListProducibleUnits(Control.ProductionLocations.GetProductionLocation(I));
            }
        }

        ProducibleUnits._children.Sort(CompareProducibleUnits);
    }

    private static int CompareProducibleUnits(object item1, object item2)
    {
        return ((AIGroupUnitProduction)item2).Priority - ((AIGroupUnitProduction)item1).Priority;
    }

    // intf_P5_07.inc:537

    // intf_P5_07.inc:537

}

public enum AIGroupProductionControlMode : byte
{
    None = 0, AutoProduce = 1, CompleteProduceOnly = 2
}
