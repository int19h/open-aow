#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI;

// production index, state, gold upkeep, mana upkeep, productions
public class AutoProduceAIPlayerAction(CustomNode? owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    private int _productionIndex;
    private int _state;
    private int _goldUpkeep;
    private int _manaUpkeep;
    private readonly QuadItemList<ProductionControl> _productions = [];

    // AI.pas:1208
    public override void Dispose() => base.Dispose();

    // AI.pas:1532
    public override bool Process()
    {
        switch (_state)
        {
            case 0:
                {
                    Player player = Control.Player;
                    if (player.PlayerIndex != 0)
                    {
                        if (FindAvailableTransporter() is null && !ProducingTransporter() && StrategicMap.Current.Day > 5)
                        {
                            AIBudget budget = player.AIBudget.Budgets.FindChild<AIBudget>(AITransporterBudget.ClassId);
                            if (budget is null)
                            {
                                budget = new AITransporterBudget(null) { Holder = CreationHolder() };
                                budget.SetOwner(player.AIBudget.Budgets);
                            }

                            if (budget.RequiredGold < 100)
                            {
                                budget.RequiredGold += 15;
                            }

                            _ = ProduceTransporter(budget);
                            if (!TransporterProductionControlExists())
                            {
                                _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
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
                    ProductionControlList controls = Control.Player.ProductionControls;
                    if (_productionIndex < controls.Count)
                    {
                        UpdateProductions(controls[_productionIndex]);
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
                    int i = 0;
                    int priority = Round(_productions[0].Fourth * 0.8);
                    while (i + 1 < _productions.Count && _productions[i + 1].Fourth >= priority)
                    {
                        i += 1;
                    }

                    i = StrategicMap.Current.Random(i + 1);
                    QuadItem<ProductionControl> item = _productions[i];
                    _ = ExecuteProduce(_productions[i].First, index: item.Second);
                    _productions.Delete(i);
                    if (!(_productions.Count != 0 && (_goldUpkeep > 2 || _manaUpkeep > 2) && (Control.Player.AIBudget.Gold > 25 || Control.Player.AIBudget.Mana > 25)))
                    {
                        _state = -1;
                    }
                }

                break;
            default:
                break;
        }

        return _state != -1;
    }

    // AI.pas:1222
    public void UpdateProductions(ProductionControl control)
    {
        if (control.Status == ProductionControlStatus.Idle)
        {
            int bonus = 0;
            for (int i = 0, last = control.UnitProductions.Count - 1; i <= last; i++)
            {
                if (control.UnitProductions.GetItem(i).Researched)
                {
                    int unitIndex = control.UnitProductions.GetItem(i).UnitIndex;
                    if (unitIndex != -1)
                    {
                        int priority = Control.UnitIndexToPriority(unitIndex);
                        if (priority != -1)
                        {
                            _ = _productions.AddABCD(control, i, C: 0, D: priority + bonus);
                        }
                    }
                }
            }
        }
    }

    // AI.pas:1243
    public bool ExecuteProduce(ProductionControl control, int index)
    {
        if (Control.Player.ProductionControls.IndexOf(control) != -1 && control.Status == ProductionControlStatus.Idle)
        {
            int gold = control.UnitProductionGold(index);
            int mana = control.UnitProductionMana(index);
            int goldUpkeep = control.UnitProductionGoldUpkeep(index);
            int manaUpkeep = control.UnitProductionManaUpkeep(index);
            if (goldUpkeep <= _goldUpkeep && manaUpkeep <= _manaUpkeep)
            {
                if (Control.Player.AIBudget.RequestProductionExpense(gold, goldUpkeep, mana, manaUpkeep))
                {
                    if (control.ProduceUnit(index, groupId: 0, queue: false))
                    {
                        int unitProductionManaUpkeep = control.UnitProductionManaUpkeep(index);
                        int unitProductionMana = control.UnitProductionMana(index);
                        int unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(index);
                        int unitProductionGold = control.UnitProductionGold(index);
                        _ = Control.Player.AIBudget.ProductionExpenseMade(unitProductionGold, goldUpkeep: unitProductionGoldUpkeep, mana: unitProductionMana, manaUpkeep: unitProductionManaUpkeep);
                        _goldUpkeep -= goldUpkeep;
                        _manaUpkeep -= manaUpkeep;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // AI.pas:1269
    public bool ExecuteResearch(ProductionControl control, int index)
    {
        if (Control.Player.ProductionControls.IndexOf(control) != -1 && control.Status == ProductionControlStatus.Idle)
        {
            // { Original behavior: affordability uses production prices for research. }
            int gold = control.UnitProductionGold(index);
            int mana = control.UnitProductionMana(index);
            if (Control.Player.AIBudget.RequestProductionExpense(gold, goldUpkeep: 0, mana, manaUpkeep: 0))
            {
                if (control.ResearchUnit(index, queue: false))
                {
                    int unitResearchMana = control.UnitResearchMana(index);
                    int unitResearchGold = control.UnitResearchGold(index);
                    _ = Control.Player.AIBudget.ExpenseMade(unitResearchGold, mana: unitResearchMana);
                    return true;
                }
            }
        }

        return false;
    }

    // AI.pas:1290
    public int TransporterPriority(int unitIndex)
    {
        UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitIndex);
        int result;
        if (resource is not null && resource.GetTransportCapacity() != 0)
        {
            Unit unit = new(null) { Holder = CreationHolder() };
            unit.SetUnitResource(resource);
            unit.UpdateOffensiveStrength();
            result = unit.MaxOffensiveStrength;
            _ = unit.Release();
            if (result <= 0)
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // AI.pas:1308
    public bool ResearchTransporter(AIBudget budget)
    {
        Player player = Control.Player;
        if (budget.Gold > 50)
        {
            QuadItemList<ProductionControl> candidates = [];
            int i = player.ProductionControls.Count;
            while (i != 0)
            {
                i -= 1;
                ProductionControl control = player.ProductionControls[i];
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
                    {
                        if (control.UnitProductions.GetItem(j).UnitIndex != -1 && !control.UnitProductions.GetItem(j).Researched)
                        {
                            UnitProduction? unitProduction = control.UnitProductions.GetItem(j);
                            UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                int unitResearchMana = control.UnitResearchMana(j);
                                int unitResearchGold = control.UnitResearchGold(j);
                                if (player.AIBudget.RequestBudgetExpense(unitResearchGold, mana: unitResearchMana, budget, usePool: false))
                                {
                                    MapCoordinate coordinate2 = control.Location;
                                    int x = coordinate2.X;
                                    int y = coordinate2.Y;
                                    int l = coordinate2.MapLevel;
                                    PlayerStructure structure = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<PlayerStructure>();
                                    int priority = TransporterPriority(control.UnitProductions.GetItem(j).UnitIndex);
                                    if (priority != -1)
                                    {
                                        if (structure is not null && structure.ProductionPriority > 0)
                                        {
                                            priority = structure.ProductionPriority > 10
                                                ? priority * 5
                                                : Round((double)(priority * 0.5) * structure.ProductionPriority);
                                        }

                                        _ = candidates.AddABCD(control, j, C: 0, D: priority);
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
                _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
                QuadItem<ProductionControl> item = candidates[0];
                if (ExecuteResearch(candidates[0].First, index: item.Second))
                {
                    budget.Gold = 0;
                    budget.Mana = 0;
                    budget.RequiredGold = 0;
                    budget.RequiredMana = 0;
                }
                else
                {
                    _ = player.AIBudget.ExpenseMade(budget.Gold, budget.Mana);
                }
            }
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1374
    public bool ProduceTransporter(AIBudget budget)
    {
        Player player = Control.Player;
        if (budget.Gold > 50)
        {
            QuadItemList<ProductionControl> candidates = [];
            int i = player.ProductionControls.Count;
            while (i != 0)
            {
                i -= 1;
                ProductionControl control = player.ProductionControls[i];
                if (control.Status == ProductionControlStatus.Idle)
                {
                    for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
                    {
                        if (control.UnitProductions.GetItem(j).UnitIndex != -1 && control.UnitProductions.GetItem(j).Researched)
                        {
                            UnitProduction? unitProduction = control.UnitProductions.GetItem(j);
                            UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                            if (resource.GetTransportCapacity() != 0)
                            {
                                int unitProductionMana = control.UnitProductionMana(j);
                                int unitProductionGold = control.UnitProductionGold(j);
                                if (player.AIBudget.RequestBudgetExpense(unitProductionGold, mana: unitProductionMana, budget, usePool: false))
                                {
                                    int unitProductionManaUpkeep = control.UnitProductionManaUpkeep(j);
                                    int controlUnitProductionMana = control.UnitProductionMana(j);
                                    int unitProductionGoldUpkeep = control.UnitProductionGoldUpkeep(j);
                                    int controlUnitProductionGold = control.UnitProductionGold(j);
                                    if (player.AIBudget.RequestProductionExpense(controlUnitProductionGold, goldUpkeep: unitProductionGoldUpkeep, mana: controlUnitProductionMana, manaUpkeep: unitProductionManaUpkeep, budget))
                                    {
                                        MapCoordinate coordinate3 = control.Location;
                                        int x = coordinate3.X;
                                        int y = coordinate3.Y;
                                        int l = coordinate3.MapLevel;
                                        PlayerStructure structure = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<PlayerStructure>();
                                        int priority = TransporterPriority(control.UnitProductions.GetItem(j).UnitIndex);
                                        if (priority != -1)
                                        {
                                            if (structure is not null && structure.ProductionPriority > 0)
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

                                            _ = candidates.AddABCD(control, j, C: 0, D: priority);
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
                _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
                QuadItem<ProductionControl> item = candidates[0];
                if (ExecuteProduce(candidates[0].First, index: item.Second))
                {
                    budget.Gold = 0;
                    budget.Mana = 0;
                    budget.RequiredGold = 0;
                    budget.RequiredMana = 0;
                }
                else
                {
                    _ = player.AIBudget.ExpenseMade(budget.Gold, budget.Mana);
                }
            }
            else
            {
                if (!ResearchingTransporter())
                {
                    _ = ResearchTransporter(budget);
                }
            }
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1444
    public Troop? FindAvailableTransporter()
    {
        Player player = Control.Player;
        Troop? result = null;
        int i = player.Armies.Count;
        while (i != 0 && result is null)
        {
            i -= 1;
            result = player.Armies.GetArmy(i).Transporter(255);
            if (result is not null && result.GroupId != 0)
            {
                result = null;
            }
        }

        return result;
    }

    // AI.pas:1461
    public bool TransporterProductionControlExists()
    {
        // { locals follow the original stack order }
        Player player = Control.Player;
        bool result = false;
        int i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            ProductionControl control = player.ProductionControls[i];
            for (int j = 0, last = control.UnitProductions.Count - 1; j <= last; j++)
            {
                if (control.UnitProductions.GetItem(j).UnitIndex != -1 && control.UnitProductions.GetItem(j).Researched)
                {
                    UnitProduction? unitProduction = control.UnitProductions.GetItem(j);
                    UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                    result = result || resource.GetTransportCapacity() != 0;
                }
            }
        }

        return result;
    }

    // AI.pas:1487
    public bool ProducingTransporter()
    {
        Player player = Control.Player;
        bool result = false;
        int i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            ProductionControl control = player.ProductionControls[i];
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
        Player player = Control.Player;
        bool result = false;
        int i = player.ProductionControls.Count;
        while (i != 0 && !result)
        {
            i -= 1;
            ProductionControl control = player.ProductionControls[i];
            if (control.Status == ProductionControlStatus.Producing)
            {
                if (control.Queue.GetItem(0).Order is ResearchSpell)
                {
                    UnitProduction? unitProduction = control.UnitProductions.GetItem(control.Settings.UnitProductionIndex);
                    UnitResource? resource = GamePack.Current.UnitResources.GetUnitResource(unitProduction.UnitIndex);
                    if (resource is not null)
                    {
                        result = resource.GetTransportCapacity() != 0;
                    }
                }
            }
        }

        return result;
    }

    // AI.pas:1214
    public static new int Priority => 12;

    private static int CompareAutoProductions(in QuadItem<ProductionControl> item1, in QuadItem<ProductionControl> item2) => item2.Fourth - item1.Fourth;

}
