// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// production index, state, gold upkeep, mana upkeep, productions
public partial class AutoProduceAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1202
    public AutoProduceAIPlayerAction(ECustomNode owner) : base(owner)
    {
        Productions = new QuadItemList<ProductionControl>();
    }

    public int ProductionIndex;
    public int State;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public QuadItemList<ProductionControl> Productions;

    // AI.pas:1208
    public override void Dispose()
    {
        Productions?.Dispose();
        base.Dispose();
    }

    // AI.pas:1532
    public override bool Process()
    {
        int I = default;
        int priority = default;
        AIBudget budget = default;
        ProductionControlList controls = default;
        switch (State)
        {
            case 0:
                {
                    var player = Control.Player;
                    if (player.PlayerIndex != 0)
                    {
                        if (FindAvailableTransporter() == null && !ProducingTransporter() && AoWHSMap.Current.Day > 5)
                        {
                            budget = (AIBudget)player.AIBudget.Budgets.FindChild(131742);
                            if (budget == null)
                            {
                                budget = new AITransporterBudget(null);
                                budget.SetOwner(player.AIBudget.Budgets);
                            }

                            if (budget.RequiredGold < 100)
                            {
                                budget.RequiredGold += 15;
                            }

                            ProduceTransporter(budget);
                            if (!TransporterProductionControlExists())
                            {
                                player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
                                budget.Gold = 0;
                                budget.Mana = 0;
                                budget.RequiredGold = 0;
                            }
                        }

                        GoldUpkeep = player.AIBudget.GoldUpkeep - Round((double)player.Income * 0.1);
                        if (GoldUpkeep < 0)
                        {
                            GoldUpkeep = 0;
                        }

                        var manaIncome = player.MagicControl.GetManaIncome();
                        ManaUpkeep = player.AIBudget.ManaUpkeep - Round((double)manaIncome * 0.1);
                        if (ManaUpkeep < 0)
                        {
                            ManaUpkeep = 0;
                        }

                        if (player.PlayerIndex != 0 && (player.AIBudget.Gold > 25 || player.AIBudget.Mana > 25) && (GoldUpkeep > 0 || ManaUpkeep > 0))
                        {
                            ProductionIndex = -1;
                            State += 1;
                        }
                        else
                        {
                            State = -1;
                        }
                    }
                    else
                    {
                        State = -1;
                    }
                }

                break;
            case 1:
                while (State == 1)
                {
                    ProductionIndex += 1;
                    controls = Control.Player.ProductionControls;
                    if (ProductionIndex < controls.Count)
                    {
                        UpdateProductions(controls.GetPC(ProductionIndex));
                    }
                    else
                    {
                        Productions.Sort(CompareAutoProductions);
                        if (Productions.Count != 0)
                        {
                            State += 1;
                        }
                        else
                        {
                            State = -1;
                        }
                    }
                }

                break;
            case 2:
                while (State == 2 && !Control.Player.GetBusy())
                {
                    I = 0;
                    priority = Round((double)Productions[0].Fourth * 0.8);
                    while (I + 1 < Productions.Count && Productions[I + 1].Fourth >= priority)
                    {
                        I += 1;
                    }

                    I = AoWHSMap.Current.Random(I + 1);
                    var item = Productions[I];
                    ExecuteProduce(Productions[I].First, item.Second);
                    Productions.Delete(I);
                    if (!(Productions.Count != 0 && (GoldUpkeep > 2 || ManaUpkeep > 2) && (Control.Player.AIBudget.Gold > 25 || Control.Player.AIBudget.Mana > 25)))
                    {
                        State = -1;
                    }
                }

                break;
        }

        return State != -1;
    }

    // AI.pas:1222
    public void UpdateProductions(ProductionControl control)
    {
        int unitIndex;
        int priority;
        int bonus;
        if (control.Status == ProductionControlStatus.Idle)
        {
            bonus = 0;
            for (int I = 0, last = control.UnitProductions.Count - 1; I <= last; I++)
            {
                if (control.UnitProductions.GetItem(I).Researched)
                {
                    unitIndex = control.UnitProductions.GetItem(I).UnitIndex;
                    if (unitIndex != -1)
                    {
                        priority = Control.UnitIndexToPriority(unitIndex);
                        if (priority != -1)
                        {
                            Productions.AddABCD(control, I, 0, priority + bonus);
                        }
                    }
                }
            }
        }
    }

    // AI.pas:1243
    public bool ExecuteProduce(ProductionControl control, int index)
    {
        int gold;
        int mana;
        int goldUpkeep;
        int manaUpkeep;
        bool result = false;
        if (Control.Player.ProductionControls.IndexOf(control) != -1 && control.Status == ProductionControlStatus.Idle)
        {
            gold = control.UnitProductionGold(index);
            mana = control.UnitProductionMana(index);
            goldUpkeep = control.UnitProductionGoldUpkeep(index);
            manaUpkeep = control.UnitProductionManaUpkeep(index);
            if (goldUpkeep <= GoldUpkeep && manaUpkeep <= ManaUpkeep)
            {
                if (Control.Player.AIBudget.RequestProductionExpense(gold, goldUpkeep, mana, manaUpkeep))
                {
                    if (control.ProduceUnit(index, 0, false))
                    {
                        var unitProductionManaUpkeep = control.UnitProductionManaUpkeep(index);
                        var unitProductionMana = control.UnitProductionMana(index);
                        var unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(index);
                        var unitProductionGold = control.UnitProductionGold(index);
                        Control.Player.AIBudget.ProductionExpenseMade(unitProductionGold, unitProductionGoldUpkeep, unitProductionMana, unitProductionManaUpkeep);
                        GoldUpkeep -= goldUpkeep;
                        ManaUpkeep -= manaUpkeep;
                        result = true;
                    }
                }
            }
        }

        return result;
    }

    // AI.pas:1269
    public bool ExecuteResearch(ProductionControl control, int index)
    {
        int gold;
        int mana;
        bool result = false;
        if (Control.Player.ProductionControls.IndexOf(control) != -1 && control.Status == ProductionControlStatus.Idle)
        {
            // { Original behavior: affordability uses production prices for research. }
            gold = control.UnitProductionGold(index);
            mana = control.UnitProductionMana(index);
            if (Control.Player.AIBudget.RequestProductionExpense(gold, 0, mana, 0))
            {
                if (control.ResearchUnit(index, false))
                {
                    var unitResearchMana = control.UnitResearchMana(index);
                    var unitResearchGold = control.UnitResearchGold(index);
                    Control.Player.AIBudget.ExpenseMade(unitResearchGold, unitResearchMana);
                    result = true;
                }
            }
        }

        return result;
    }

    // AI.pas:1290
    public int TransporterPriority(int unitIndex)
    {
        int result;
        UnitResource resource;
        Unit unit;
        resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex);
        if (resource != null && resource.GetTransportCapacity() != 0)
        {
            unit = new Unit(null);
            unit.SetUnitResource(resource);
            unit.UpdateOffensiveStrength();
            result = unit.MaxOffensiveStrength;
            unit.Release();
            if (result <= 0)
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // AI.pas:1308
    public bool ResearchTransporter(AIBudget budget)
    {
        QuadItemList<ProductionControl> candidates = default;
        PlayerStructure structure = default;
        int I = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Player player = default;
        ProductionControl control = default;
        UnitResource resource = default;
        int priority = default;
        player = Control.Player;
        if (budget.Gold > 50)
        {
            candidates = new QuadItemList<ProductionControl>();
            I = player.ProductionControls.Count;
            while (I != 0)
            {
                I -= 1;
                control = player.ProductionControls.GetPC(I);
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int J = 0, last = control.UnitProductions.Count - 1; J <= last; J++)
                    {
                        if (control.UnitProductions.GetItem(J).UnitIndex != -1 && !control.UnitProductions.GetItem(J).Researched)
                        {
                            var unitProduction = control.UnitProductions.GetItem(J);
                            resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                var unitResearchMana = control.UnitResearchMana(J);
                                var unitResearchGold = control.UnitResearchGold(J);
                                if (player.AIBudget.RequestBudgetExpense(unitResearchGold, unitResearchMana, budget, false))
                                {
                                    control.GetLocation(ref X, ref Y, ref L);
                                    structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                                    priority = TransporterPriority(control.UnitProductions.GetItem(J).UnitIndex);
                                    if (priority != -1)
                                    {
                                        if (structure != null && structure.ProductionPriority > 0)
                                        {
                                            if (structure.ProductionPriority > 10)
                                            {
                                                priority = priority * 5;
                                            }
                                            else
                                            {
                                                priority = Round((double)((double)priority * 0.5) * structure.ProductionPriority);
                                            }
                                        }

                                        candidates.AddABCD(control, J, 0, priority);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (candidates.Count != 0)
            {
                candidates.Sort(CompareAutoProductions);
                player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
                var item = candidates[0];
                if (ExecuteResearch(candidates[0].First, item.Second))
                {
                    budget.Gold = 0;
                    budget.Mana = 0;
                    budget.RequiredGold = 0;
                    budget.RequiredMana = 0;
                }
                else
                {
                    player.AIBudget.ExpenseMade(budget.Gold, budget.Mana);
                }
            }

            candidates?.Dispose();
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1374
    public bool ProduceTransporter(AIBudget budget)
    {
        QuadItemList<ProductionControl> candidates = default;
        PlayerStructure structure = default;
        int I = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Player player = default;
        ProductionControl control = default;
        UnitResource resource = default;
        int priority = default;
        player = Control.Player;
        if (budget.Gold > 50)
        {
            candidates = new QuadItemList<ProductionControl>();
            I = player.ProductionControls.Count;
            while (I != 0)
            {
                I -= 1;
                control = player.ProductionControls.GetPC(I);
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int J = 0, last = control.UnitProductions.Count - 1; J <= last; J++)
                    {
                        if (control.UnitProductions.GetItem(J).UnitIndex != -1 && control.UnitProductions.GetItem(J).Researched)
                        {
                            var unitProduction = control.UnitProductions.GetItem(J);
                            resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                var unitProductionMana = control.UnitProductionMana(J);
                                var unitProductionGold = control.UnitProductionGold(J);
                                if (player.AIBudget.RequestBudgetExpense(unitProductionGold, unitProductionMana, budget, false))
                                {
                                    var unitProductionManaUpkeep = control.UnitProductionManaUpkeep(J);
                                    var controlUnitProductionMana = control.UnitProductionMana(J);
                                    var unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(J);
                                    var controlUnitProductionGold = control.UnitProductionGold(J);
                                    if (player.AIBudget.RequestProductionExpense(controlUnitProductionGold, unitProductionGoldUpkeep, controlUnitProductionMana, unitProductionManaUpkeep, budget))
                                    {
                                        control.GetLocation(ref X, ref Y, ref L);
                                        structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                                        priority = TransporterPriority(control.UnitProductions.GetItem(J).UnitIndex);
                                        if (priority != -1)
                                        {
                                            if (structure != null && structure.ProductionPriority > 0)
                                            {
                                                if (structure.ProductionPriority > 10)
                                                {
                                                    priority += priority * 5;
                                                }
                                                else
                                                {
                                                    priority += Round((double)((double)priority * 0.5) * structure.ProductionPriority);
                                                }
                                            }

                                            candidates.AddABCD(control, J, 0, priority);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (candidates.Count != 0)
            {
                candidates.Sort(CompareAutoProductions);
                player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
                var item = candidates[0];
                if (ExecuteProduce(candidates[0].First, item.Second))
                {
                    budget.Gold = 0;
                    budget.Mana = 0;
                    budget.RequiredGold = 0;
                    budget.RequiredMana = 0;
                }
                else
                {
                    player.AIBudget.ExpenseMade(budget.Gold, budget.Mana);
                }
            }
            else
            {
                if (!ResearchingTransporter())
                {
                    ResearchTransporter(budget);
                }
            }

            candidates?.Dispose();
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1444
    public Troop FindAvailableTransporter()
    {
        int I;
        var player = Control.Player;
        Troop result = null;
        I = player.Armies.Count;
        while (I != 0 && result == null)
        {
            I -= 1;
            result = player.Armies.GetArmy(I).Transporter(255);
            if (result != null && result.GroupId != 0)
            {
                result = null;
            }
        }

        return result;
    }

    // AI.pas:1461
    public bool TransporterProductionControlExists()
    {
        UnitResource resource;
        int I;
        ProductionControl control;
        // { locals follow the original stack order }
        var player = Control.Player;
        bool result = false;
        I = player.ProductionControls.Count;
        while (I != 0 && !result)
        {
            I -= 1;
            control = player.ProductionControls.GetPC(I);
            for (int J = 0, last = control.UnitProductions.Count - 1; J <= last; J++)
            {
                if (control.UnitProductions.GetItem(J).UnitIndex != -1 && control.UnitProductions.GetItem(J).Researched)
                {
                    var unitProduction = control.UnitProductions.GetItem(J);
                    resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                    result = result || resource.GetTransportCapacity() != 0;
                }
            }
        }

        return result;
    }

    // AI.pas:1487
    public bool ProducingTransporter()
    {
        int I;
        ProductionControl control;
        var player = Control.Player;
        bool result = false;
        I = player.ProductionControls.Count;
        while (I != 0 && !result)
        {
            I -= 1;
            control = player.ProductionControls.GetPC(I);
            if (control.Status == ProductionControlStatus.Producing)
            {
                if (control.Queue.GetItem(0).Order is ProduceUnit)
                {
                    result = control.Settings.Unit.GetTransportCapacity() != 0;
                }
            }
        }

        return result;
    }

    // AI.pas:1507
    public bool ResearchingTransporter()
    {
        int I;
        ProductionControl control;
        UnitResource resource;
        var player = Control.Player;
        bool result = false;
        I = player.ProductionControls.Count;
        while (I != 0 && !result)
        {
            I -= 1;
            control = player.ProductionControls.GetPC(I);
            if (control.Status == ProductionControlStatus.Producing)
            {
                if (control.Queue.GetItem(0).Order is ResearchSpell)
                {
                    var unitProduction = control.UnitProductions.GetItem(control.Settings.UnitProductionIndex);
                    resource = AoWHSSet.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                    if (resource != null)
                    {
                        result = resource.GetTransportCapacity() != 0;
                    }
                }
            }
        }

        return result;
    }

    // intf_P5_06.inc:642

    // AI.pas:1214
    public static new int Priority => 12;

    private static int CompareAutoProductions(in QuadItem<ProductionControl> item1, in QuadItem<ProductionControl> item2)
    {
        return item2.Fourth - item1.Fourth;
    }

}
