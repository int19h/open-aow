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

// FCityCount, FProductionLevelCounts, FMaxProductionLevel, FProductionRatio
public partial class CityAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    public CityAIPlayerAction(ECustomNode owner) : base(owner)
    { }

    public int FCityCount;
    public Int32Array4 FProductionLevelCounts;
    public int FMaxProductionLevel;
    public float FProductionRatio;

    // City.pas:3130
    public override bool Process()
    {
        Player Player;
        AIBudgetManager Manager;
        AIBudget UpgradeBudget;
        AIBudget ResearchBudget;
        AIBudget FortifyBudget;
        UpdateStatistics();
        Player = FControl.FPlayer;
        if (Player.FPlayerIndex != 0)
        {
            Manager = Player.FAIBudget;
            UpgradeBudget = (AIBudget)Manager.FBudgets.FindChild(131349);
            if (UpgradeBudget == null)
            {
                UpgradeBudget = new AICityUpgradeBudget(null);
                UpgradeBudget.Owner = Manager.FBudgets;
                UpgradeBudget.FRequiredGold = 250;
                UpgradeBudget.FDays = 7;
            }

            ResearchBudget = (AIBudget)Manager.FBudgets.FindChild(131350);
            if (ResearchBudget != null)
            {
                if (ResearchBudget.FGold + UpgradeBudget.FGold > 125)
                {
                    if (ResearchProduction(ResearchBudget, UpgradeBudget))
                    {
                        ResearchBudget.FDays = 7;
                        ResearchBudget.FRequiredGold = 125;
                    }
                }
            }
            else
            {
                ResearchBudget = new AICityResearchBudget(null);
                ResearchBudget.FDays = 7;
                ResearchBudget.FRequiredGold = 125;
                ResearchBudget.Owner = Manager.FBudgets;
            }

            ValidateResearchBudget(ResearchBudget);
            if (UpgradeBudget.GetReady())
            {
                if (UpgradeCity(UpgradeBudget))
                {
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }
            }

            ValidateUpgradeBudget(UpgradeBudget);
            FortifyBudget = (AIBudget)Manager.FBudgets.FindChild(131351);
            if (FortifyBudget != null)
            {
                if (FortifyBudget.GetReady())
                {
                    if (FortifyCity(FortifyBudget))
                    {
                        FortifyBudget.FDays = 7;
                        FortifyBudget.FRequiredGold = 80;
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.FDay > 5)
                {
                    FortifyBudget = new AICityFortifyBudget(null);
                    FortifyBudget.FDays = 7;
                    FortifyBudget.FRequiredGold = 80;
                    FortifyBudget.Owner = Manager.FBudgets;
                }
            }

            if (FortifyBudget != null)
            {
                ValidateFortifyBudget(FortifyBudget);
            }
        }

        return false;
    }

    // City.pas:2795
    public bool ValidResearchCity(City City)
    {
        bool Result = City.FProduction.GetProduceUnitLevelMax() < City.FUpgradeLevel;
        bool result = Result;
        if (result)
        {
            var activeAction = City.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = City.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:2802
    public bool ValidResearchBudgetCity(City City) => City.FProduction.GetProduceUnitLevelMax() != City.FUpgradeLevel || City.GetActiveAction() == CityAction.Upgrade;

    // City.pas:2807
    public void ValidateResearchBudget(AIBudget Budget)
    {
        Player Player;
        int Count;
        int Total;
        int Priority;
        PlayerStructure Structure;
        Player = FControl.FPlayer;
        Count = 0;
        Total = 0;
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City)
            {
                Total += 1;
                if (ValidResearchBudgetCity((City)Structure))
                {
                    Count += 1;
                }
            }
        }

        if (Count == 0)
        {
            Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
            Budget.FGold = 0;
            Budget.FMana = 0;
            Budget.FRequiredGold = 0;
            // { Original limits days, not priority. }
            Budget.FDays = 7;
        }
        else
        {
            Budget.FRequiredGold = 125;
            Priority = 7 - Count * Total / 3;
            if (Priority < 3)
            {
                Priority = 3;
            }

            if (Priority < Budget.FDays)
            {
                Budget.FDays = Priority;
            }
        }
    }

    // City.pas:2842
    public bool ResearchProduction(AIBudget ResearchBudget, AIBudget UpgradeBudget)
    {
        DoubleItemList<City> Cities;
        bool Valid;
        Player Player;
        int UnitIndex;
        int Index;
        int Value;
        int Priority;
        PlayerStructure Structure;
        City City;
        ProductionItem Production;
        bool Result = false;
        Player = FControl.FPlayer;
        Cities = new DoubleItemList<City>();
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City && ValidResearchCity((City)Structure))
            {
                City = (City)Structure;
                Valid = false;
                for (int J = City.FProduction.FUnitProductions.Count - 1; J >= 0; J--)
                {
                    UnitIndex = City.FProduction.FUnitProductions.GetItem(J).FUnitIndex;
                    if (UnitIndex != -1)
                    {
                        Valid = Valid || AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex).FLevel == City.FUpgradeLevel && !City.FProduction.FUnitProductions.GetItem(J).FResearched && City.ResearchCost(J) <= ResearchBudget.FGold + UpgradeBudget.FGold;
                    }
                }

                if (Valid)
                {
                    Priority = City.GetSize() * 2 + (City.FProductionPriority + City.FWallType * 3);
                    Cities.AddAB(City, Priority);
                }
            }
        }

        Cities.SortDescendingItemB();
        if (Cities.Count != 0)
        {
            Index = 0;
            Value = Round((double)Cities[0].B * 0.75);
            while (Index + 1 < Cities.Count && Value <= Cities[Index + 1].B)
            {
                Index += 1;
            }

            Index = AoWHSMap.Current.Random(Index + 1);
            City = Cities[Index].A;
            RandSeed = AoWHSMap.Current.Random(16777215);
            Index = Cities[Index].A.FProduction.FindOptimalResearchProduction(null, ResearchBudget.FGold + UpgradeBudget.FGold);
            if (Index != -1)
            {
                City.FProduction.ExecuteDeleteProductions();
                Production = new ProductionItem(null);
                Production.Order = new ResearchSpell(Index);
                City.FProduction.ExecuteAddProduction(Production);
                Production?.Dispose();
                Index = City.ResearchCost(Index);
                if (Index > ResearchBudget.FGold)
                {
                    Index -= ResearchBudget.FGold;
                    Player.FAIBudget.BudgetExpenseMade(ResearchBudget.FGold, 0, ResearchBudget, AIBudgetManager.ExpenseMode.Budget);
                    Player.FAIBudget.BudgetExpenseMade(Index, 0, UpgradeBudget, AIBudgetManager.ExpenseMode.Budget);
                }
                else
                {
                    Player.FAIBudget.BudgetExpenseMade(Index, 0, ResearchBudget, AIBudgetManager.ExpenseMode.Budget);
                }

                Result = true;
            }
        }

        Cities?.Dispose();
        return Result;
    }

    // City.pas:2909
    public bool ValidUpgradeCity(City City)
    {
        string Reason = "";
        int Level;
        Level = City.FProduction.GetProduceUnitLevelMax();
        bool Result = City.CanUpgrade(false, ref Reason);
        Result = Result && City.GetSize() > City.FUpgradeLevel && !(Level == FMaxProductionLevel && FProductionLevelCounts[Level - 1] == 1) && Level == City.FUpgradeLevel;
        bool result = Result;
        if (result)
        {
            var activeAction = City.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = City.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:2921
    public bool ValidUpgradeBudgetCity(City City)
    {
        bool Result;
        int Level;
        Level = City.FProduction.GetProduceUnitLevelMax();
        if (City.GetSize() <= City.FUpgradeLevel || Level != City.FUpgradeLevel)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // City.pas:2928
    public bool UpgradeCity(AIBudget Budget)
    {
        bool Result;
        Player Player;
        DoubleItemList<City> Cities;
        int Priority;
        PlayerStructure Structure;
        City City;
        ProductionItem Production;
        Player = FControl.FPlayer;
        Cities = new DoubleItemList<City>();
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            // { Preserve the original runtime class check. }
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City && ValidUpgradeCity((City)Structure))
            {
                City = (City)Structure;
                Priority = City.GetSize() * 2 + (City.FProductionPriority + City.FWallType * 3);
                Cities.AddAB(City, Priority);
            }
        }

        Cities.SortDescendingItemB();
        if (Cities.Count != 0)
        {
            City = Cities[0].A;
            City.FProduction.ExecuteDeleteProductions();
            Production = new ProductionItem(null);
            Production.Order = new Upgrade();
            City.FProduction.ExecuteAddProduction(Production);
            Production?.Dispose();
            Player.FAIBudget.BudgetExpenseMade(250, 0, Budget, AIBudgetManager.ExpenseMode.Budget);

            Result = true;
        }
        else
        {
            Result = false;
        }

        Cities?.Dispose();
        return Result;
    }

    // City.pas:2964
    public void ValidateUpgradeBudget(AIBudget Budget)
    {
        Player Player;
        int Count;
        int Priority;
        PlayerStructure Structure;
        Player = FControl.FPlayer;
        Count = 0;
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City)
            {
                if (ValidUpgradeBudgetCity((City)Structure))
                {
                    Count += 1;
                }
            }
        }

        if (Count == 0)
        {
            Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
            Budget.FGold = 0;
            Budget.FMana = 0;
            Budget.FRequiredGold = 0;
            // { Original limits days, not priority. }
            Budget.FDays = 7;
        }
        else
        {
            Budget.FRequiredGold = 250;
            Priority = 8 - Count / 2;
            if (Priority < 5)
            {
                Priority = 5;
            }

            if (Priority < Budget.FDays)
            {
                Budget.FDays = Priority;
            }
        }
    }

    // City.pas:2999
    public bool ValidFortifyCity(City City)
    {
        string Reason = "";
        bool Result = City.CanFortify(false, ref Reason);
        Result = Result && City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && AoWHSMap.Current.FDay >= 30;
        bool result = Result;
        if (result)
        {
            var activeAction = City.GetActiveAction();
            bool contains = Set<CityAction>.Of(CityAction.None, CityAction.Trade).Contains(activeAction);
            if (!contains)
            {
                var cityActiveAction = City.GetActiveAction();
                contains = Set<CityAction>.Of(CityAction.Produce).Contains(cityActiveAction) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
            }

            result = contains;
        }

        return result;
    }

    // City.pas:3009
    public bool ValidFortifyBudgetCity(City City) => City.FWallType == 0 || City.FWallType == 1 && City.GetSize() > 2 && City.FUpgradeLevel > 1 && AoWHSMap.Current.FDay >= 30;

    // City.pas:3015
    public bool FortifyCity(AIBudget Budget)
    {
        bool Result;
        Player Player;
        DoubleItemList<City> Cities;
        int Priority;
        PlayerStructure Structure;
        City City;
        ProductionItem Production;
        Player = FControl.FPlayer;
        Cities = new DoubleItemList<City>();
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            // { Preserve the original runtime class check. }
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City && ValidFortifyCity((City)Structure))
            {
                City = (City)Structure;
                Priority = City.GetSize() * 2 + City.FProductionPriority;
                if (City.FWallType == 0)
                {
                    Priority += 3;
                }

                Cities.AddAB(City, Priority);
            }
        }

        Cities.SortDescendingItemB();
        if (Cities.Count != 0)
        {
            City = Cities[0].A;
            City.FProduction.ExecuteDeleteProductions();
            var fortifyCost = City.FortifyCost();
            Player.FAIBudget.BudgetExpenseMade(fortifyCost, 0, Budget, AIBudgetManager.ExpenseMode.Budget);
            Production = new ProductionItem(null);
            Production.Order = new Fortify();
            City.FProduction.ExecuteAddProduction(Production);
            Production?.Dispose();

            Result = true;
        }
        else
        {
            Result = false;
        }

        Cities?.Dispose();
        return Result;
    }

    // City.pas:3052
    public void ValidateFortifyBudget(AIBudget Budget)
    {
        Player Player;
        int Count;
        int Priority;
        PlayerStructure Structure;
        Player = FControl.FPlayer;
        Count = 0;
        for (int I = Player.FStructures.Count - 1; I >= 0; I--)
        {
            Structure = Player.FStructures.GetStructure(I);
            if (Structure is City)
            {
                if (ValidFortifyBudgetCity((City)Structure))
                {
                    Count += 1;
                }
            }
        }

        if (Count == 0)
        {
            Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
            Budget.FGold = 0;
            Budget.FMana = 0;
            Budget.FRequiredGold = 0;
            // { Original limits days, not priority. }
            Budget.FDays = 7;
        }
        else
        {
            Budget.FRequiredGold = 80;
            Priority = 8 - Count / 2;
            if (Priority < 4)
            {
                Priority = 4;
            }

            if (Priority < Budget.FDays)
            {
                Budget.FDays = Priority;
            }
        }
    }

    // City.pas:3087
    public int UpgradeableCities()
    {
        int Count;
        PlayerStructure Structure;
        Count = 0;
        for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
        {
            Structure = FControl.FPlayer.FStructures.GetStructure(I);
            if (Structure is City)
            {
                _ = (City)Structure;
                var city = (City)Structure;
                if (city.GetSize() > city.FUpgradeLevel)
                {
                    Count += 1;
                }
            }
        }

        return Count;
    }

    // City.pas:3102
    public void UpdateStatistics()
    {
        int Level;
        int Available;
        int Busy;
        City City;
        FCityCount = 0;
        FMaxProductionLevel = 0;
        Available = 0;
        Busy = 0;
        FProductionLevelCounts = default;
        for (int I = FControl.FPlayer.FStructures.Count - 1; I >= 0; I--)
        {
            if (FControl.FPlayer.FStructures.GetStructure(I) is City)
            {
                FCityCount += 1;
                City = (City)FControl.FPlayer.FStructures.GetStructure(I);
                if (Set<CityAction>.Of(CityAction.None, CityAction.Trade, CityAction.Produce).Contains(City.GetActiveAction()))
                {
                    Available += (City.GetSize() + 1) / 2;
                    Level = City.FProduction.GetProduceUnitLevelMax();
                    if (Level > 0)
                    {
                        FProductionLevelCounts[Level - 1] += 1;
                    }

                    if (Level > FMaxProductionLevel)
                    {
                        FMaxProductionLevel = Level;
                    }
                }
                else
                {
                    Busy += (City.GetSize() + 1) / 2;
                }
            }
        }

        if (FCityCount != 0)
        {
            // { Original rounds the fraction before storing it. }
            FProductionRatio = Round((double)Available / (Available + Busy));
        }
        else
        {
            FProductionRatio = 0;
        }
    }

    // City.pas:2790
    public static new int Priority => 13;

}
