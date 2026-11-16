#nullable disable
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
    public AutoProduceAIPlayerAction(CustomNode owner) : base(owner) => _productions = new QuadItemList<ProductionControl>();

    private int _productionIndex;
    private int _state;
    private int _goldUpkeep;
    private int _manaUpkeep;
    private readonly QuadItemList<ProductionControl> _productions;

    // AI.pas:1208
    public override void Dispose()
    {
        _productions?.Dispose();
        base.Dispose();
    }

    // AI.pas:1532
    public override bool Process()
    {
        int i = default;
        int priority = default;
        AIBudget budget = default;
        ProductionControlList controls = default;
        switch (_state)
        {
            case 0:
                {
                    Player player = Control.Player;
                    if (player.PlayerIndex != 0)
                    {
                        if (FindAvailableTransporter() == null && !ProducingTransporter() && StrategicMap.Current.Day > 5)
                        {
                            budget = player.AIBudget.Budgets.FindChild<AIBudget>(131742);
                            if (budget == null)
                            {
                                budget = new AITransporterBudget(null) { Holder = CreationHolder() };
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

                        _goldUpkeep = player.AIBudget.GoldUpkeep - Round(player.Income * 0.1);
                        if (_goldUpkeep < 0)
                        {
                            _goldUpkeep = 0;
                        }

                        int manaIncome = player.MagicControl.GetManaIncome();
                        _manaUpkeep = player.AIBudget.ManaUpkeep - Round(manaIncome * 0.1);
                        if (_manaUpkeep < 0)
                        {
                            _manaUpkeep = 0;
                        }

                        if (player.PlayerIndex != 0 && (player.AIBudget.Gold > 25 || player.AIBudget.Mana > 25) && (_goldUpkeep > 0 || _manaUpkeep > 0))
                        {
                            _productionIndex = -1;
                            _state += 1;
                        }
                        else
                        {
                            _state = -1;
                        }
                    }
                    else
                    {
                        _state = -1;
                    }
                }

                break;
            case 1:
                while (_state == 1)
                {
                    _productionIndex += 1;
                    controls = Control.Player.ProductionControls;
                    if (_productionIndex < controls.Count)
                    {
                        UpdateProductions(controls.GetPC(_productionIndex));
                    }
                    else
                    {
                        _productions.Sort(CompareAutoProductions);
                        if (_productions.Count != 0)
                        {
                            _state += 1;
                        }
                        else
                        {
                            _state = -1;
                        }
                    }
                }

                break;
            case 2:
                while (_state == 2 && !Control.Player.GetBusy())
                {
                    i = 0;
                    priority = Round(_productions[0].Fourth * 0.8);
                    while (i + 1 < _productions.Count && _productions[i + 1].Fourth >= priority)
                    {
                        i += 1;
                    }

                    i = StrategicMap.Current.Random(i + 1);
                    QuadItem<ProductionControl> item = _productions[i];
                    ExecuteProduce(_productions[i].First, item.Second);
                    _productions.Delete(i);
                    if (!(_productions.Count != 0 && (_goldUpkeep > 2 || _manaUpkeep > 2) && (Control.Player.AIBudget.Gold > 25 || Control.Player.AIBudget.Mana > 25)))
                    {
                        _state = -1;
                    }
                }

                break;
        }

        return _state != -1;
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
            for (int i = 0, last = control.UnitProductions.Count - 1; i <= last; i++)
            {
                if (control.UnitProductions.GetItem(i).Researched)
                {
                    unitIndex = control.UnitProductions.GetItem(i).UnitIndex;
                    if (unitIndex != -1)
                    {
                        priority = Control.UnitIndexToPriority(unitIndex);
                        if (priority != -1)
                        {
                            _productions.AddABCD(control, i, 0, priority + bonus);
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
            if (goldUpkeep <= _goldUpkeep && manaUpkeep <= _manaUpkeep)
            {
                if (Control.Player.AIBudget.RequestProductionExpense(gold, goldUpkeep, mana, manaUpkeep))
                {
                    if (control.ProduceUnit(index, 0, false))
                    {
                        int unitProductionManaUpkeep = control.UnitProductionManaUpkeep(index);
                        int unitProductionMana = control.UnitProductionMana(index);
                        int unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(index);
                        int unitProductionGold = control.UnitProductionGold(index);
                        Control.Player.AIBudget.ProductionExpenseMade(unitProductionGold, unitProductionGoldUpkeep, unitProductionMana, unitProductionManaUpkeep);
                        _goldUpkeep -= goldUpkeep;
                        _manaUpkeep -= manaUpkeep;
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
                    int unitResearchMana = control.UnitResearchMana(index);
                    int unitResearchGold = control.UnitResearchGold(index);
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
        resource = GamePack.Current.UnitResources.GetUnitResource(unitIndex);
        if (resource != null && resource.GetTransportCapacity() != 0)
        {
            unit = new Unit(null) { Holder = CreationHolder() };
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
        int i = default;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        Player player = default;
        ProductionControl control = default;
        UnitResource resource = default;
        int priority = default;
        player = Control.Player;
        if (budget.Gold > 50)
        {
            candidates = new QuadItemList<ProductionControl>();
            i = player.ProductionControls.Count;
            while (i != 0)
            {
                i -= 1;
                control = player.ProductionControls.GetPC(i);
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
                    {
                        if (control.UnitProductions.GetItem(j).UnitIndex != -1 && !control.UnitProductions.GetItem(j).Researched)
                        {
                            UnitProduction unitProduction = control.UnitProductions.GetItem(j);
                            resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                int unitResearchMana = control.UnitResearchMana(j);
                                int unitResearchGold = control.UnitResearchGold(j);
                                if (player.AIBudget.RequestBudgetExpense(unitResearchGold, unitResearchMana, budget, false))
                                {
                                    MapCoordinate coordinate2 = control.GetLocation();
                                    x = coordinate2.X;
                                    y = coordinate2.Y;
                                    l = coordinate2.MapLevel;
                                    structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                                    priority = TransporterPriority(control.UnitProductions.GetItem(j).UnitIndex);
                                    if (priority != -1)
                                    {
                                        if (structure != null && structure.ProductionPriority > 0)
                                        {
                                            priority = structure.ProductionPriority > 10
                                                ? priority * 5
                                                : Round((double)(priority * 0.5) * structure.ProductionPriority);
                                        }

                                        candidates.AddABCD(control, j, 0, priority);
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
                QuadItem<ProductionControl> item = candidates[0];
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
        int i = default;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        Player player = default;
        ProductionControl control = default;
        UnitResource resource = default;
        int priority = default;
        player = Control.Player;
        if (budget.Gold > 50)
        {
            candidates = new QuadItemList<ProductionControl>();
            i = player.ProductionControls.Count;
            while (i != 0)
            {
                i -= 1;
                control = player.ProductionControls.GetPC(i);
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
                    {
                        if (control.UnitProductions.GetItem(j).UnitIndex != -1 && control.UnitProductions.GetItem(j).Researched)
                        {
                            UnitProduction unitProduction = control.UnitProductions.GetItem(j);
                            resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                int unitProductionMana = control.UnitProductionMana(j);
                                int unitProductionGold = control.UnitProductionGold(j);
                                if (player.AIBudget.RequestBudgetExpense(unitProductionGold, unitProductionMana, budget, false))
                                {
                                    int unitProductionManaUpkeep = control.UnitProductionManaUpkeep(j);
                                    int controlUnitProductionMana = control.UnitProductionMana(j);
                                    int unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(j);
                                    int controlUnitProductionGold = control.UnitProductionGold(j);
                                    if (player.AIBudget.RequestProductionExpense(controlUnitProductionGold, unitProductionGoldUpkeep, controlUnitProductionMana, unitProductionManaUpkeep, budget))
                                    {
                                        MapCoordinate coordinate3 = control.GetLocation();
                                        x = coordinate3.X;
                                        y = coordinate3.Y;
                                        l = coordinate3.MapLevel;
                                        structure = (PlayerStructure)StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite(typeof(PlayerStructure));
                                        priority = TransporterPriority(control.UnitProductions.GetItem(j).UnitIndex);
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
                                                    priority += Round((double)(priority * 0.5) * structure.ProductionPriority);
                                                }
                                            }

                                            candidates.AddABCD(control, j, 0, priority);
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
                QuadItem<ProductionControl> item = candidates[0];
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
        int i;
        Player player = Control.Player;
        Troop result = null;
        i = player.Armies.Count;
        while (i != 0 && result == null)
        {
            i -= 1;
            result = player.Armies.GetArmy(i).Transporter(255);
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
        int i;
        ProductionControl control;
        // { locals follow the original stack order }
        Player player = Control.Player;
        bool result = false;
        i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            control = player.ProductionControls.GetPC(i);
            for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
            {
                if (control.UnitProductions.GetItem(j).UnitIndex != -1 && control.UnitProductions.GetItem(j).Researched)
                {
                    UnitProduction unitProduction = control.UnitProductions.GetItem(j);
                    resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                    result = result || resource.GetTransportCapacity() != 0;
                }
            }
        }

        return result;
    }

    // AI.pas:1487
    public bool ProducingTransporter()
    {
        int i;
        ProductionControl control;
        Player player = Control.Player;
        bool result = false;
        i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            control = player.ProductionControls.GetPC(i);
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
        int i;
        ProductionControl control;
        UnitResource resource;
        Player player = Control.Player;
        bool result = false;
        i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            control = player.ProductionControls.GetPC(i);
            if (control.Status == ProductionControlStatus.Producing)
            {
                if (control.Queue.GetItem(0).Order is ResearchSpell)
                {
                    UnitProduction unitProduction = control.UnitProductions.GetItem(control.Settings.UnitProductionIndex);
                    resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
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

    private static int CompareAutoProductions(in QuadItem<ProductionControl> item1, in QuadItem<ProductionControl> item2) => item2.Fourth - item1.Fourth;

}
