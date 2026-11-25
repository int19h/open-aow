#nullable disable warnings
// Translated from City.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI;

// CityCount, ProductionLevelCounts, MaxProductionLevel, ProductionRatio
public class CityAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{
    private int _cityCount;
    private readonly int[] _productionLevelCounts = new int[4];
    private int _maxProductionLevel;

    // City.pas:3130
    public override bool Process()
    {
        UpdateStatistics();
        Player player = Control.Player;
        if (player.PlayerIndex != PlayerId.Independent)
        {
            AIBudgetManager manager = player.AIBudget;
            AIBudget upgradeBudget = manager.Budgets.FindChild<AIBudget>(AICityUpgradeBudget.ClassId);
            upgradeBudget ??= new AICityUpgradeBudget(null)
            {
                Holder = CreationHolder(),
                Owner = manager.Budgets,
                RequiredGold = 250,
                Days = 7
            };

            AIBudget researchBudget = manager.Budgets.FindChild<AIBudget>(AICityResearchBudget.ClassId);
            if (researchBudget is not null)
            {
                if (researchBudget.Gold + upgradeBudget.Gold > 125)
                {
                    if (ResearchProduction(researchBudget, upgradeBudget))
                    {
                        researchBudget.Days = 7;
                        researchBudget.RequiredGold = 125;
                    }
                }
            }
            else
            {
                researchBudget = new AICityResearchBudget(null)
                {
                    Holder = CreationHolder(),
                    Days = 7,
                    RequiredGold = 125,
                    Owner = manager.Budgets
                };
            }

            ValidateResearchBudget(researchBudget);
            if (upgradeBudget.Ready)
            {
                if (UpgradeCity(upgradeBudget))
                {
                    upgradeBudget.RequiredGold = 250;
                    upgradeBudget.Days = 7;
                }
            }

            ValidateUpgradeBudget(upgradeBudget);
            AIBudget fortifyBudget = manager.Budgets.FindChild<AIBudget>(AICityFortifyBudget.ClassId);
            if (fortifyBudget is not null)
            {
                if (fortifyBudget.Ready)
                {
                    if (FortifyCity(fortifyBudget))
                    {
                        fortifyBudget.Days = 7;
                        fortifyBudget.RequiredGold = 80;
                    }
                }
            }
            else
            {
                if (GetEngine().StrategicMap.Day > 5)
                {
                    fortifyBudget = new AICityFortifyBudget(null)
                    {
                        Holder = CreationHolder(),
                        Days = 7,
                        RequiredGold = 80,
                        Owner = manager.Budgets
                    };
                }
            }

            if (fortifyBudget is not null)
            {
                ValidateFortifyBudget(fortifyBudget);
            }
        }

        return false;
    }

    // City.pas:2795
    public bool ValidResearchCity(City city)
    {
        bool result2 = city.Production.GetProduceUnitLevelMax() < city.UpgradeLevel;
        bool result = result2;
        if (result)
        {
            CityAction activeAction = city.ActiveAction;
            bool contains = activeAction is CityAction.None or CityAction.Trade;
            if (!contains)
            {
                CityAction cityActiveAction = city.ActiveAction;
                contains = cityActiveAction == CityAction.Produce && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            return contains;
        }

        return result;
    }

    // City.pas:2802
    public bool ValidResearchBudgetCity(City city) => city.Production.GetProduceUnitLevelMax() != city.UpgradeLevel || city.ActiveAction == CityAction.Upgrade;

    // City.pas:2807
    public void ValidateResearchBudget(AIBudget budget)
    {
        Player player = Control.Player;
        // GetProduceUnitLevelMax and the city fields only read, so the totals do not depend on direction.
        int total = player.Structures.Count(static structure => structure is City);
        int count = player.Structures.Count(structure => structure is City city && ValidResearchBudgetCity(city));

        if (count == 0)
        {
            _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 125;
            int priority = 7 - (count * total / 3);
            if (priority < 3)
            {
                priority = 3;
            }

            if (priority < budget.Days)
            {
                budget.Days = priority;
            }
        }
    }

    // City.pas:2842
    public bool ResearchProduction(AIBudget researchBudget, AIBudget upgradeBudget)
    {
        bool result = false;
        Player player = Control.Player;
        DoubleItemList<City> cities = [];
        City city;
        for (int i = player.Structures.Count - 1; i >= 0; i--)
        {
            PlayerStructure structure = player.Structures.GetStructure(i);
            if (structure is City found && ValidResearchCity(found))
            {
                city = found;
                bool valid = false;
                for (int j = city.Production.UnitProductions.Count - 1; j >= 0; j--)
                {
                    int unitIndex = city.Production.UnitProductions.GetItem(j).UnitIndex;
                    if (unitIndex != -1)
                    {
                        valid = valid || (GetEngine().GamePack.UnitResources.GetUnitResource(unitIndex).Level == city.UpgradeLevel && !city.Production.UnitProductions.GetItem(j).Researched && city.ResearchCost(j) <= researchBudget.Gold + upgradeBudget.Gold);
                    }
                }

                if (valid)
                {
                    int priority = (city.Size * 2) + city.ProductionPriority + (city.WallType * 3);
                    _ = cities.AddAbility(city, priority);
                }
            }
        }

        cities.SortDescendingItemB();
        if (cities.Count != 0)
        {
            int index = 0;
            int value = Round(cities[0].Second * 0.75);
            while (index + 1 < cities.Count && value <= cities[index + 1].Second)
            {
                index += 1;
            }

            index = GetEngine().StrategicMap.Random(index + 1);
            city = cities[index].First;
            RandSeed = GetEngine().StrategicMap.Random(16777215);
            index = cities[index].First.Production.FindOptimalResearchProduction(null, gold: researchBudget.Gold + upgradeBudget.Gold);
            if (index != -1)
            {
                city.Production.ExecuteDeleteProductions();
                ProductionItem production = new(null)
                {
                    Holder = CreationHolder(),
                    Order = new ResearchSpell(index)
                };
                city.Production.ExecuteAddProduction(production);
                production?.Dispose();
                index = city.ResearchCost(index);
                if (index > researchBudget.Gold)
                {
                    index -= researchBudget.Gold;
                    _ = player.AIBudget.BudgetExpenseMade(researchBudget.Gold, mana: 0, budget: researchBudget, mode: AIBudgetManager.ExpenseMode.Budget);
                    _ = player.AIBudget.BudgetExpenseMade(index, mana: 0, budget: upgradeBudget, mode: AIBudgetManager.ExpenseMode.Budget);
                }
                else
                {
                    _ = player.AIBudget.BudgetExpenseMade(index, mana: 0, budget: researchBudget, mode: AIBudgetManager.ExpenseMode.Budget);
                }

                result = true;
            }
        }
        return result;
    }

    // City.pas:2909
    public bool ValidUpgradeCity(City city)
    {
        string reason = "";
        int level = city.Production.GetProduceUnitLevelMax();
        bool result2 = city.CanUpgrade(false, ref reason);
        result2 = result2 && city.Size > city.UpgradeLevel && !(level == _maxProductionLevel && _productionLevelCounts[level - 1] == 1) && level == city.UpgradeLevel;
        bool result = result2;
        if (result)
        {
            CityAction activeAction = city.ActiveAction;
            bool contains = activeAction is CityAction.None or CityAction.Trade;
            if (!contains)
            {
                CityAction cityActiveAction = city.ActiveAction;
                contains = cityActiveAction == CityAction.Produce && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            return contains;
        }

        return result;
    }

    // City.pas:2921
    public bool ValidUpgradeBudgetCity(City city)
    {
        int level = city.Production.GetProduceUnitLevelMax();
        return city.Size > city.UpgradeLevel && level == city.UpgradeLevel;
    }

    // City.pas:2928
    public bool UpgradeCity(AIBudget budget)
    {
        Player player = Control.Player;
        DoubleItemList<City> cities = [];
        City city;
        for (int i = player.Structures.Count - 1; i >= 0; i--)
        {
            // { Preserve the original runtime class check. }
            PlayerStructure structure = player.Structures.GetStructure(i);
            if (structure is City found && ValidUpgradeCity(found))
            {
                city = found;
                int priority = (city.Size * 2) + city.ProductionPriority + (city.WallType * 3);
                _ = cities.AddAbility(city, priority);
            }
        }

        cities.SortDescendingItemB();
        bool result;
        if (cities.Count != 0)
        {
            city = cities[0].First;
            city.Production.ExecuteDeleteProductions();
            ProductionItem production = new(null)
            {
                Holder = CreationHolder(),
                Order = new Upgrade()
            };
            city.Production.ExecuteAddProduction(production);
            production?.Dispose();
            _ = player.AIBudget.BudgetExpenseMade(250, mana: 0, budget, mode: AIBudgetManager.ExpenseMode.Budget);

            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    // City.pas:2964
    public void ValidateUpgradeBudget(AIBudget budget)
    {
        Player player = Control.Player;
        int count = player.Structures.Count(structure => structure is City city && ValidUpgradeBudgetCity(city));

        if (count == 0)
        {
            _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 250;
            int priority = 8 - (count / 2);
            if (priority < 5)
            {
                priority = 5;
            }

            if (priority < budget.Days)
            {
                budget.Days = priority;
            }
        }
    }

    // City.pas:2999
    public bool ValidFortifyCity(City city)
    {
        string reason = "";
        bool result2 = city.CanFortify(false, ref reason);
        result2 = (result2 && city.WallType == 0) || (city.WallType == 1 && city.Size > 2 && city.UpgradeLevel > 1 && GetEngine().StrategicMap.Day >= 30);
        bool result = result2;
        if (result)
        {
            CityAction activeAction = city.ActiveAction;
            bool contains = activeAction is CityAction.None or CityAction.Trade;
            if (!contains)
            {
                CityAction cityActiveAction = city.ActiveAction;
                contains = cityActiveAction == CityAction.Produce && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            return contains;
        }

        return result;
    }

    // City.pas:3009
    public bool ValidFortifyBudgetCity(City city) => city.WallType == 0 || (city.WallType == 1 && city.Size > 2 && city.UpgradeLevel > 1 && GetEngine().StrategicMap.Day >= 30);

    // City.pas:3015
    public bool FortifyCity(AIBudget budget)
    {
        Player player = Control.Player;
        DoubleItemList<City> cities = [];
        City city;
        for (int i = player.Structures.Count - 1; i >= 0; i--)
        {
            // { Preserve the original runtime class check. }
            PlayerStructure structure = player.Structures.GetStructure(i);
            if (structure is City found && ValidFortifyCity(found))
            {
                city = found;
                int priority = (city.Size * 2) + city.ProductionPriority;
                if (city.WallType == 0)
                {
                    priority += 3;
                }

                _ = cities.AddAbility(city, priority);
            }
        }

        cities.SortDescendingItemB();
        bool result;
        if (cities.Count != 0)
        {
            city = cities[0].First;
            city.Production.ExecuteDeleteProductions();
            int fortifyCost = city.FortifyCost();
            _ = player.AIBudget.BudgetExpenseMade(fortifyCost, mana: 0, budget, mode: AIBudgetManager.ExpenseMode.Budget);
            ProductionItem production = new(null)
            {
                Holder = CreationHolder(),
                Order = new Fortify()
            };
            city.Production.ExecuteAddProduction(production);
            production?.Dispose();

            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    // City.pas:3052
    public void ValidateFortifyBudget(AIBudget budget)
    {
        Player player = Control.Player;
        int count = player.Structures.Count(structure => structure is City city && ValidFortifyBudgetCity(city));

        if (count == 0)
        {
            _ = player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 80;
            int priority = 8 - (count / 2);
            if (priority < 4)
            {
                priority = 4;
            }

            if (priority < budget.Days)
            {
                budget.Days = priority;
            }
        }
    }

    // City.pas:3087
    // Size and UpgradeLevel are fields.
    public int UpgradeableCities() => Control.Player.Structures.Count(static structure => structure is City city && city.Size > city.UpgradeLevel);

    // City.pas:3102
    public void UpdateStatistics()
    {
        _cityCount = 0;
        _maxProductionLevel = 0;
        int available = 0;
        int busy = 0;
        Array.Clear(_productionLevelCounts);
        foreach (PlayerStructure structure in Control.Player.Structures.Reverse())
        {
            if (structure is City found)
            {
                _cityCount += 1;
                City city = found;
                if (city.ActiveAction is CityAction.None or CityAction.Trade or CityAction.Produce)
                {
                    available += (city.Size + 1) / 2;
                    int level = city.Production.GetProduceUnitLevelMax();
                    if (level > 0)
                    {
                        _productionLevelCounts[level - 1] += 1;
                    }

                    if (level > _maxProductionLevel)
                    {
                        _maxProductionLevel = level;
                    }
                }
                else
                {
                    busy += (city.Size + 1) / 2;
                }
            }
        }

        if (_cityCount != 0)
        {
        }
        else
        {
        }
    }

    // City.pas:2790
    public static new int Priority => 13;

}
