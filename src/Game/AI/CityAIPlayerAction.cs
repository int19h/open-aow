// Translated from City.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// CityCount, ProductionLevelCounts, MaxProductionLevel, ProductionRatio
public partial class CityAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    public CityAIPlayerAction(ECustomNode owner) : base(owner)
    { }

    private int _cityCount;
    private Int32Array4 _productionLevelCounts;
    private int _maxProductionLevel;

    // City.pas:3130
    public override bool Process()
    {
        Player player;
        AIBudgetManager manager;
        AIBudget upgradeBudget;
        AIBudget researchBudget;
        AIBudget fortifyBudget;
        UpdateStatistics();
        player = Control.Player;
        if (player.PlayerIndex != 0)
        {
            manager = player.AIBudget;
            upgradeBudget = (AIBudget)manager.Budgets.FindChild(131349);
            if (upgradeBudget == null)
            {
                upgradeBudget = new AICityUpgradeBudget(null);
                upgradeBudget.Owner = manager.Budgets;
                upgradeBudget.RequiredGold = 250;
                upgradeBudget.Days = 7;
            }

            researchBudget = (AIBudget)manager.Budgets.FindChild(131350);
            if (researchBudget != null)
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
                researchBudget = new AICityResearchBudget(null);
                researchBudget.Days = 7;
                researchBudget.RequiredGold = 125;
                researchBudget.Owner = manager.Budgets;
            }

            ValidateResearchBudget(researchBudget);
            if (upgradeBudget.GetReady())
            {
                if (UpgradeCity(upgradeBudget))
                {
                    upgradeBudget.RequiredGold = 250;
                    upgradeBudget.Days = 7;
                }
            }

            ValidateUpgradeBudget(upgradeBudget);
            fortifyBudget = (AIBudget)manager.Budgets.FindChild(131351);
            if (fortifyBudget != null)
            {
                if (fortifyBudget.GetReady())
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
                if (AoWHSMap.Current.Day > 5)
                {
                    fortifyBudget = new AICityFortifyBudget(null);
                    fortifyBudget.Days = 7;
                    fortifyBudget.RequiredGold = 80;
                    fortifyBudget.Owner = manager.Budgets;
                }
            }

            if (fortifyBudget != null)
            {
                ValidateFortifyBudget(fortifyBudget);
            }
        }

        return false;
    }

    // City.pas:2795
    public bool ValidResearchCity(City city)
    {
        bool Result = city.Production.GetProduceUnitLevelMax() < city.UpgradeLevel;
        bool result = Result;
        if (result)
        {
            var activeAction = city.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = city.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:2802
    public bool ValidResearchBudgetCity(City city) => city.Production.GetProduceUnitLevelMax() != city.UpgradeLevel || city.GetActiveAction() == CityAction.Upgrade;

    // City.pas:2807
    public void ValidateResearchBudget(AIBudget budget)
    {
        Player player;
        int count;
        int total;
        int priority;
        PlayerStructure structure;
        player = Control.Player;
        count = 0;
        total = 0;
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            structure = player.Structures.GetStructure(I);
            if (structure is City)
            {
                total += 1;
                if (ValidResearchBudgetCity((City)structure))
                {
                    count += 1;
                }
            }
        }

        if (count == 0)
        {
            player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 125;
            priority = 7 - count * total / 3;
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
        DoubleItemList<City> cities;
        bool valid;
        Player player;
        int unitIndex;
        int index;
        int value;
        int priority;
        PlayerStructure structure;
        City city;
        ProductionItem production;
        bool result = false;
        player = Control.Player;
        cities = new DoubleItemList<City>();
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            structure = player.Structures.GetStructure(I);
            if (structure is City && ValidResearchCity((City)structure))
            {
                city = (City)structure;
                valid = false;
                for (int J = city.Production.UnitProductions.Count - 1; J >= 0; J--)
                {
                    unitIndex = city.Production.UnitProductions.GetItem(J).UnitIndex;
                    if (unitIndex != -1)
                    {
                        valid = valid || AoWHSSet.Current.UnitResources.GetUnitResource(unitIndex).Level == city.UpgradeLevel && !city.Production.UnitProductions.GetItem(J).Researched && city.ResearchCost(J) <= researchBudget.Gold + upgradeBudget.Gold;
                    }
                }

                if (valid)
                {
                    priority = city.GetSize() * 2 + (city.ProductionPriority + city.WallType * 3);
                    cities.AddAbility(city, priority);
                }
            }
        }

        cities.SortDescendingItemB();
        if (cities.Count != 0)
        {
            index = 0;
            value = Round((double)cities[0].Second * 0.75);
            while (index + 1 < cities.Count && value <= cities[index + 1].Second)
            {
                index += 1;
            }

            index = AoWHSMap.Current.Random(index + 1);
            city = cities[index].First;
            RandSeed = AoWHSMap.Current.Random(16777215);
            index = cities[index].First.Production.FindOptimalResearchProduction(null, researchBudget.Gold + upgradeBudget.Gold);
            if (index != -1)
            {
                city.Production.ExecuteDeleteProductions();
                production = new ProductionItem(null);
                production.Order = new ResearchSpell(index);
                city.Production.ExecuteAddProduction(production);
                production?.Dispose();
                index = city.ResearchCost(index);
                if (index > researchBudget.Gold)
                {
                    index -= researchBudget.Gold;
                    player.AIBudget.BudgetExpenseMade(researchBudget.Gold, 0, researchBudget, AIBudgetManager.ExpenseMode.Budget);
                    player.AIBudget.BudgetExpenseMade(index, 0, upgradeBudget, AIBudgetManager.ExpenseMode.Budget);
                }
                else
                {
                    player.AIBudget.BudgetExpenseMade(index, 0, researchBudget, AIBudgetManager.ExpenseMode.Budget);
                }

                result = true;
            }
        }

        cities?.Dispose();
        return result;
    }

    // City.pas:2909
    public bool ValidUpgradeCity(City city)
    {
        string reason = "";
        int level;
        level = city.Production.GetProduceUnitLevelMax();
        bool Result = city.CanUpgrade(false, ref reason);
        Result = Result && city.GetSize() > city.UpgradeLevel && !(level == _maxProductionLevel && _productionLevelCounts[level - 1] == 1) && level == city.UpgradeLevel;
        bool result = Result;
        if (result)
        {
            var activeAction = city.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = city.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:2921
    public bool ValidUpgradeBudgetCity(City city)
    {
        bool result;
        int level;
        level = city.Production.GetProduceUnitLevelMax();
        if (city.GetSize() <= city.UpgradeLevel || level != city.UpgradeLevel)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // City.pas:2928
    public bool UpgradeCity(AIBudget budget)
    {
        bool result;
        Player player;
        DoubleItemList<City> cities;
        int priority;
        PlayerStructure structure;
        City city;
        ProductionItem production;
        player = Control.Player;
        cities = new DoubleItemList<City>();
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            // { Preserve the original runtime class check. }
            structure = player.Structures.GetStructure(I);
            if (structure is City && ValidUpgradeCity((City)structure))
            {
                city = (City)structure;
                priority = city.GetSize() * 2 + (city.ProductionPriority + city.WallType * 3);
                cities.AddAbility(city, priority);
            }
        }

        cities.SortDescendingItemB();
        if (cities.Count != 0)
        {
            city = cities[0].First;
            city.Production.ExecuteDeleteProductions();
            production = new ProductionItem(null);
            production.Order = new Upgrade();
            city.Production.ExecuteAddProduction(production);
            production?.Dispose();
            player.AIBudget.BudgetExpenseMade(250, 0, budget, AIBudgetManager.ExpenseMode.Budget);

            result = true;
        }
        else
        {
            result = false;
        }

        cities?.Dispose();
        return result;
    }

    // City.pas:2964
    public void ValidateUpgradeBudget(AIBudget budget)
    {
        Player player;
        int count;
        int priority;
        PlayerStructure structure;
        player = Control.Player;
        count = 0;
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            structure = player.Structures.GetStructure(I);
            if (structure is City)
            {
                if (ValidUpgradeBudgetCity((City)structure))
                {
                    count += 1;
                }
            }
        }

        if (count == 0)
        {
            player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 250;
            priority = 8 - count / 2;
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
        bool Result = city.CanFortify(false, ref reason);
        Result = Result && city.WallType == 0 || city.WallType == 1 && city.GetSize() > 2 && city.UpgradeLevel > 1 && AoWHSMap.Current.Day >= 30;
        bool result = Result;
        if (result)
        {
            var activeAction = city.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = city.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && city.Production.Settings.TurnsLeft == city.Production.Settings.TurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:3009
    public bool ValidFortifyBudgetCity(City city) => city.WallType == 0 || city.WallType == 1 && city.GetSize() > 2 && city.UpgradeLevel > 1 && AoWHSMap.Current.Day >= 30;

    // City.pas:3015
    public bool FortifyCity(AIBudget budget)
    {
        bool result;
        Player player;
        DoubleItemList<City> cities;
        int priority;
        PlayerStructure structure;
        City city;
        ProductionItem production;
        player = Control.Player;
        cities = new DoubleItemList<City>();
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            // { Preserve the original runtime class check. }
            structure = player.Structures.GetStructure(I);
            if (structure is City && ValidFortifyCity((City)structure))
            {
                city = (City)structure;
                priority = city.GetSize() * 2 + city.ProductionPriority;
                if (city.WallType == 0)
                {
                    priority += 3;
                }

                cities.AddAbility(city, priority);
            }
        }

        cities.SortDescendingItemB();
        if (cities.Count != 0)
        {
            city = cities[0].First;
            city.Production.ExecuteDeleteProductions();
            var fortifyCost = city.FortifyCost();
            player.AIBudget.BudgetExpenseMade(fortifyCost, 0, budget, AIBudgetManager.ExpenseMode.Budget);
            production = new ProductionItem(null);
            production.Order = new Fortify();
            city.Production.ExecuteAddProduction(production);
            production?.Dispose();

            result = true;
        }
        else
        {
            result = false;
        }

        cities?.Dispose();
        return result;
    }

    // City.pas:3052
    public void ValidateFortifyBudget(AIBudget budget)
    {
        Player player;
        int count;
        int priority;
        PlayerStructure structure;
        player = Control.Player;
        count = 0;
        for (int I = player.Structures.Count - 1; I >= 0; I--)
        {
            structure = player.Structures.GetStructure(I);
            if (structure is City)
            {
                if (ValidFortifyBudgetCity((City)structure))
                {
                    count += 1;
                }
            }
        }

        if (count == 0)
        {
            player.AIBudget.ReturnExpense(budget.Gold, budget.Mana);
            budget.Gold = 0;
            budget.Mana = 0;
            budget.RequiredGold = 0;
            // { Original limits days, not priority. }
            budget.Days = 7;
        }
        else
        {
            budget.RequiredGold = 80;
            priority = 8 - count / 2;
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
    public int UpgradeableCities()
    {
        int count;
        PlayerStructure structure;
        count = 0;
        for (int I = Control.Player.Structures.Count - 1; I >= 0; I--)
        {
            structure = Control.Player.Structures.GetStructure(I);
            if (structure is City)
            {
                _ = (City)structure;
                var city = (City)structure;
                if (city.GetSize() > city.UpgradeLevel)
                {
                    count += 1;
                }
            }
        }

        return count;
    }

    // City.pas:3102
    public void UpdateStatistics()
    {
        int level;
        int available;
        int busy;
        City city;
        _cityCount = 0;
        _maxProductionLevel = 0;
        available = 0;
        busy = 0;
        _productionLevelCounts = default;
        for (int I = Control.Player.Structures.Count - 1; I >= 0; I--)
        {
            if (Control.Player.Structures.GetStructure(I) is City)
            {
                _cityCount += 1;
                city = (City)Control.Player.Structures.GetStructure(I);
                if (Set<CityAction>.Of(CityAction.None, CityAction.Trade, CityAction.Produce).Contains(city.GetActiveAction()))
                {
                    available += (city.GetSize() + 1) / 2;
                    level = city.Production.GetProduceUnitLevelMax();
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
                    busy += (city.GetSize() + 1) / 2;
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
