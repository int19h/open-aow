// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai;

// production index, state, gold upkeep, mana upkeep, productions
public partial class AutoProduceAIPA : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:1202
    public AutoProduceAIPA(ECustomNode owner) : base(owner)
    {
        FProductions = new QuadItemList<ProductionControl>();
    }

    public int FProductionIndex;
    public int FState;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public QuadItemList<ProductionControl> FProductions;

    // AI.pas:1208
    public override void Dispose()
    {
        FProductions?.Dispose();
        base.Dispose();
    }

    // AI.pas:1532
    public override bool Process()
    {
        int I = default;
        int Priority = default;
        AIBudget Budget = default;
        ProductionControlList Controls = default;
        switch (FState)
        {
            case 0:
                {
                    var player = FControl.FPlayer;
                    if (player.FPlayerIndex != 0)
                    {
                        if (FindAvailableTransporter() == null && !ProducingTransporter() && AoWHSMap.Current.FDay > 5)
                        {
                            Budget = (AIBudget)player.FAIBudget.FBudgets.FindChild(131742);
                            if (Budget == null)
                            {
                                Budget = new AITransporterBudget(null);
                                Budget.SetOwner(player.FAIBudget.FBudgets);
                            }

                            if (Budget.FRequiredGold < 100)
                            {
                                Budget.FRequiredGold += 15;
                            }

                            ProduceTransporter(Budget);
                            if (!TransporterProductionControlExists())
                            {
                                player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                                Budget.FGold = 0;
                                Budget.FMana = 0;
                                Budget.FRequiredGold = 0;
                            }
                        }

                        FGoldUpkeep = player.FAIBudget.FGoldUpkeep - Round((double)player.FIncome * 0.1);
                        if (FGoldUpkeep < 0)
                        {
                            FGoldUpkeep = 0;
                        }

                        var manaIncome = player.FMagicControl.GetManaIncome();
                        FManaUpkeep = player.FAIBudget.FManaUpkeep - Round((double)manaIncome * 0.1);
                        if (FManaUpkeep < 0)
                        {
                            FManaUpkeep = 0;
                        }

                        if (player.FPlayerIndex != 0 && (player.FAIBudget.FGold > 25 || player.FAIBudget.FMana > 25) && (FGoldUpkeep > 0 || FManaUpkeep > 0))
                        {
                            FProductionIndex = -1;
                            FState += 1;
                        }
                        else
                        {
                            FState = -1;
                        }
                    }
                    else
                    {
                        FState = -1;
                    }
                }

                break;
            case 1:
                while (FState == 1)
                {
                    FProductionIndex += 1;
                    Controls = FControl.FPlayer.FProductionControls;
                    if (FProductionIndex < Controls.Count)
                    {
                        UpdateProductions(Controls.GetPC(FProductionIndex));
                    }
                    else
                    {
                        FProductions.Sort(CompareAutoProductions);
                        if (FProductions.Count != 0)
                        {
                            FState += 1;
                        }
                        else
                        {
                            FState = -1;
                        }
                    }
                }

                break;
            case 2:
                while (FState == 2 && !FControl.FPlayer.GetBusy())
                {
                    I = 0;
                    Priority = Round((double)FProductions[0].D * 0.8);
                    while (I + 1 < FProductions.Count && FProductions[I + 1].D >= Priority)
                    {
                        I += 1;
                    }

                    I = AoWHSMap.Current.Random(I + 1);
                    var item = FProductions[I];
                    ExecuteProduce(FProductions[I].A, item.B);
                    FProductions.Delete(I);
                    if (!(FProductions.Count != 0 && (FGoldUpkeep > 2 || FManaUpkeep > 2) && (FControl.FPlayer.FAIBudget.FGold > 25 || FControl.FPlayer.FAIBudget.FMana > 25)))
                    {
                        FState = -1;
                    }
                }

                break;
        }

        return FState != -1;
    }

    // AI.pas:1222
    public void UpdateProductions(ProductionControl Control)
    {
        int UnitIndex;
        int Priority;
        int Bonus;
        if (Control.FStatus == ProductionControl.Status.Idle)
        {
            Bonus = 0;
            for (int I = 0, last = Control.FUnitProductions.Count - 1; I <= last; I++)
            {
                if (Control.FUnitProductions.GetItem(I).FResearched)
                {
                    UnitIndex = Control.FUnitProductions.GetItem(I).FUnitIndex;
                    if (UnitIndex != -1)
                    {
                        Priority = FControl.UnitIndexToPriority(UnitIndex);
                        if (Priority != -1)
                        {
                            FProductions.AddABCD(Control, I, 0, Priority + Bonus);
                        }
                    }
                }
            }
        }
    }

    // AI.pas:1243
    public bool ExecuteProduce(ProductionControl Control, int Index)
    {
        int Gold;
        int Mana;
        int GoldUpkeep;
        int ManaUpkeep;
        bool Result = false;
        if (FControl.FPlayer.FProductionControls.IndexOf(Control) != -1 && Control.FStatus == ProductionControl.Status.Idle)
        {
            Gold = Control.UnitProductionGold(Index);
            Mana = Control.UnitProductionMana(Index);
            GoldUpkeep = Control.UnitProductionGoldUpkeep(Index);
            ManaUpkeep = Control.UnitProductionManaUpkeep(Index);
            if (GoldUpkeep <= FGoldUpkeep && ManaUpkeep <= FManaUpkeep)
            {
                if (FControl.FPlayer.FAIBudget.RequestProductionExpense(Gold, GoldUpkeep, Mana, ManaUpkeep))
                {
                    if (Control.ProduceUnitEx(Index, 0, false))
                    {
                        var unitProductionManaUpkeep = Control.UnitProductionManaUpkeep(Index);
                        var unitProductionMana = Control.UnitProductionMana(Index);
                        var unitProductionGoldUpkeep = Control.UnitProductionGoldUpkeep(Index);
                        var unitProductionGold = Control.UnitProductionGold(Index);
                        FControl.FPlayer.FAIBudget.ProductionExpenseMade(unitProductionGold, unitProductionGoldUpkeep, unitProductionMana, unitProductionManaUpkeep);
                        FGoldUpkeep -= GoldUpkeep;
                        FManaUpkeep -= ManaUpkeep;
                        Result = true;
                    }
                }
            }
        }

        return Result;
    }

    // AI.pas:1269
    public bool ExecuteResearch(ProductionControl Control, int Index)
    {
        int Gold;
        int Mana;
        bool Result = false;
        if (FControl.FPlayer.FProductionControls.IndexOf(Control) != -1 && Control.FStatus == ProductionControl.Status.Idle)
        {
            // { Original behavior: affordability uses production prices for research. }
            Gold = Control.UnitProductionGold(Index);
            Mana = Control.UnitProductionMana(Index);
            if (FControl.FPlayer.FAIBudget.RequestProductionExpense(Gold, 0, Mana, 0))
            {
                if (Control.ResearchUnit(Index, false))
                {
                    var unitResearchMana = Control.UnitResearchMana(Index);
                    var unitResearchGold = Control.UnitResearchGold(Index);
                    FControl.FPlayer.FAIBudget.ExpenseMade(unitResearchGold, unitResearchMana);
                    Result = true;
                }
            }
        }

        return Result;
    }

    // AI.pas:1290
    public int TransporterPriority(int UnitIndex)
    {
        int Result;
        UnitResource Resource;
        Unit AUnit;
        Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(UnitIndex);
        if (Resource != null && Resource.GetTransportCapacity() != 0)
        {
            AUnit = new Unit(null);
            AUnit.SetUnitResource(Resource);
            AUnit.UpdateOffensiveStrength();
            Result = AUnit.FMaxOffensiveStrength;
            AUnit.Release();
            if (Result <= 0)
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // AI.pas:1308
    public bool ResearchTransporter(AIBudget Budget)
    {
        QuadItemList<ProductionControl> Candidates = default;
        PlayerStructure Structure = default;
        int I = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Player Player = default;
        ProductionControl Control = default;
        UnitResource Resource = default;
        int Priority = default;
        Player = FControl.FPlayer;
        if (Budget.FGold > 50)
        {
            Candidates = new QuadItemList<ProductionControl>();
            I = Player.FProductionControls.Count;
            while (I != 0)
            {
                I -= 1;
                Control = Player.FProductionControls.GetPC(I);
                if (Control.FStatus == ProductionControl.Status.Idle)
                {
                    for (int J = 0, last = Control.FUnitProductions.Count - 1; J <= last; J++)
                    {
                        if (Control.FUnitProductions.GetItem(J).FUnitIndex != -1 && !Control.FUnitProductions.GetItem(J).FResearched)
                        {
                            var unitProduction = Control.FUnitProductions.GetItem(J);
                            Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
                            if (Resource.GetTransportCapacity() != 0)
                            {
                                var unitResearchMana = Control.UnitResearchMana(J);
                                var unitResearchGold = Control.UnitResearchGold(J);
                                if (Player.FAIBudget.RequestBudgetExpense(unitResearchGold, unitResearchMana, Budget, false))
                                {
                                    Control.GetLocation(ref X, ref Y, ref L);
                                    Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(PlayerStructure));
                                    Priority = TransporterPriority(Control.FUnitProductions.GetItem(J).FUnitIndex);
                                    if (Priority != -1)
                                    {
                                        if (Structure != null && Structure.FProductionPriority > 0)
                                        {
                                            if (Structure.FProductionPriority > 10)
                                            {
                                                Priority = Priority * 5;
                                            }
                                            else
                                            {
                                                Priority = Round((double)((double)Priority * 0.5) * Structure.FProductionPriority);
                                            }
                                        }

                                        Candidates.AddABCD(Control, J, 0, Priority);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Candidates.Count != 0)
            {
                Candidates.Sort(CompareAutoProductions);
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                var item = Candidates[0];
                if (ExecuteResearch(Candidates[0].A, item.B))
                {
                    Budget.FGold = 0;
                    Budget.FMana = 0;
                    Budget.FRequiredGold = 0;
                    Budget.FRequiredMana = 0;
                }
                else
                {
                    Player.FAIBudget.ExpenseMade(Budget.FGold, Budget.FMana);
                }
            }

            Candidates?.Dispose();
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1374
    public bool ProduceTransporter(AIBudget Budget)
    {
        QuadItemList<ProductionControl> Candidates = default;
        PlayerStructure Structure = default;
        int I = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Player Player = default;
        ProductionControl Control = default;
        UnitResource Resource = default;
        int Priority = default;
        Player = FControl.FPlayer;
        if (Budget.FGold > 50)
        {
            Candidates = new QuadItemList<ProductionControl>();
            I = Player.FProductionControls.Count;
            while (I != 0)
            {
                I -= 1;
                Control = Player.FProductionControls.GetPC(I);
                if (Control.FStatus == ProductionControl.Status.Idle)
                {
                    for (int J = 0, last = Control.FUnitProductions.Count - 1; J <= last; J++)
                    {
                        if (Control.FUnitProductions.GetItem(J).FUnitIndex != -1 && Control.FUnitProductions.GetItem(J).FResearched)
                        {
                            var unitProduction = Control.FUnitProductions.GetItem(J);
                            Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
                            if (Resource.GetTransportCapacity() != 0)
                            {
                                var unitProductionMana = Control.UnitProductionMana(J);
                                var unitProductionGold = Control.UnitProductionGold(J);
                                if (Player.FAIBudget.RequestBudgetExpense(unitProductionGold, unitProductionMana, Budget, false))
                                {
                                    var unitProductionManaUpkeep = Control.UnitProductionManaUpkeep(J);
                                    var controlUnitProductionMana = Control.UnitProductionMana(J);
                                    var unitProductionGoldUpkeep = Control.UnitProductionGoldUpkeep(J);
                                    var controlUnitProductionGold = Control.UnitProductionGold(J);
                                    if (Player.FAIBudget.RequestProductionExpenseEx(controlUnitProductionGold, unitProductionGoldUpkeep, controlUnitProductionMana, unitProductionManaUpkeep, Budget))
                                    {
                                        Control.GetLocation(ref X, ref Y, ref L);
                                        Structure = (PlayerStructure)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(PlayerStructure));
                                        Priority = TransporterPriority(Control.FUnitProductions.GetItem(J).FUnitIndex);
                                        if (Priority != -1)
                                        {
                                            if (Structure != null && Structure.FProductionPriority > 0)
                                            {
                                                if (Structure.FProductionPriority > 10)
                                                {
                                                    Priority += Priority * 5;
                                                }
                                                else
                                                {
                                                    Priority += Round((double)((double)Priority * 0.5) * Structure.FProductionPriority);
                                                }
                                            }

                                            Candidates.AddABCD(Control, J, 0, Priority);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Candidates.Count != 0)
            {
                Candidates.Sort(CompareAutoProductions);
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                var item = Candidates[0];
                if (ExecuteProduce(Candidates[0].A, item.B))
                {
                    Budget.FGold = 0;
                    Budget.FMana = 0;
                    Budget.FRequiredGold = 0;
                    Budget.FRequiredMana = 0;
                }
                else
                {
                    Player.FAIBudget.ExpenseMade(Budget.FGold, Budget.FMana);
                }
            }
            else
            {
                if (!ResearchingTransporter())
                {
                    ResearchTransporter(Budget);
                }
            }

            Candidates?.Dispose();
        }

        // { Original behavior: both transporter routines always return False. }
        return false;
    }

    // AI.pas:1444
    public AbstractUnit FindAvailableTransporter()
    {
        int I;
        var player = FControl.FPlayer;
        AbstractUnit Result = null;
        I = player.FArmies.Count;
        while (I != 0 && Result == null)
        {
            I -= 1;
            Result = player.FArmies.GetArmy(I).Transporter(255);
            if (Result != null && Result.FGroupID != 0)
            {
                Result = null;
            }
        }

        return Result;
    }

    // AI.pas:1461
    public bool TransporterProductionControlExists()
    {
        UnitResource Resource;
        int I;
        ProductionControl Control;
        // { locals follow the original stack order }
        var player = FControl.FPlayer;
        bool Result = false;
        I = player.FProductionControls.Count;
        while (I != 0 && !Result)
        {
            I -= 1;
            Control = player.FProductionControls.GetPC(I);
            for (int J = 0, last = Control.FUnitProductions.Count - 1; J <= last; J++)
            {
                if (Control.FUnitProductions.GetItem(J).FUnitIndex != -1 && Control.FUnitProductions.GetItem(J).FResearched)
                {
                    var unitProduction = Control.FUnitProductions.GetItem(J);
                    Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
                    Result = Result || Resource.GetTransportCapacity() != 0;
                }
            }
        }

        return Result;
    }

    // AI.pas:1487
    public bool ProducingTransporter()
    {
        int I;
        ProductionControl Control;
        var player = FControl.FPlayer;
        bool Result = false;
        I = player.FProductionControls.Count;
        while (I != 0 && !Result)
        {
            I -= 1;
            Control = player.FProductionControls.GetPC(I);
            if (Control.FStatus == ProductionControl.Status.Producing)
            {
                if (Control.FQueue.GetItem(0).Order is ProduceUnit)
                {
                    Result = Control.FSettings.FUnit.GetTransportCapacity() != 0;
                }
            }
        }

        return Result;
    }

    // AI.pas:1507
    public bool ResearchingTransporter()
    {
        int I;
        ProductionControl Control;
        UnitResource Resource;
        var player = FControl.FPlayer;
        bool Result = false;
        I = player.FProductionControls.Count;
        while (I != 0 && !Result)
        {
            I -= 1;
            Control = player.FProductionControls.GetPC(I);
            if (Control.FStatus == ProductionControl.Status.Producing)
            {
                if (Control.FQueue.GetItem(0).Order is ResearchSpell)
                {
                    var unitProduction = Control.FUnitProductions.GetItem(Control.FSettings.FUnitProductionIndex);
                    Resource = AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
                    if (Resource != null)
                    {
                        Result = Resource.GetTransportCapacity() != 0;
                    }
                }
            }
        }

        return Result;
    }

    // intf_P5_06.inc:642

    // AI.pas:1214
    public static new int Priority => 12;

    private static int CompareAutoProductions(in QuadItem<ProductionControl> Item1, in QuadItem<ProductionControl> Item2)
    {
        return Item2.D - Item1.D;
    }

}
