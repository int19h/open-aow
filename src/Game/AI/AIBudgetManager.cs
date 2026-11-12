// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// player, budgets, income/reserve factors, production upkeep, available upkeep, pool, savings
// budget
public partial class AIBudgetManager : EngineObject
{
    // AI.pas:1712
    public AIBudgetManager(ECustomNode owner) : base(owner)
    {
        Budgets = new AIBudgetList(null);
        GoldIncomeFactor = (float)0.75;
        ManaIncomeFactor = (float)0.75;
        GoldReserveFactor = (float)0.5;
        ManaReserveFactor = (float)0.5;
    }

    public Player Player;
    public AIBudgetList Budgets;
    public float GoldIncomeFactor;
    public float ManaIncomeFactor;
    public float GoldReserveFactor;
    public float ManaReserveFactor;
    public int ProductionGoldUpkeep;
    public int ProductionManaUpkeep;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public int Gold;
    public int Mana;
    public AIBudget DefaultBudget;

    // AI.pas:1722
    public override void Dispose()
    {
        Budgets?.Dispose();
        base.Dispose();
    }

    // AI.pas:1728
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(Budgets, 20);
        stream.rwInteger(ref Gold, 21);
        stream.rwInteger(ref Mana, 22);
        stream.rwInteger(ref GoldUpkeep, 23);
        stream.rwInteger(ref ManaUpkeep, 24);
        stream.rwInteger(ref ProductionGoldUpkeep, 25);
        stream.rwInteger(ref ProductionManaUpkeep, 26);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Budgets", Budgets);
        persister.Int32("Gold", ref Gold);
        persister.Int32("Mana", ref Mana);
        persister.Int32("GoldUpkeep", ref GoldUpkeep);
        persister.Int32("ManaUpkeep", ref ManaUpkeep);
        persister.Int32("ProductionGoldUpkeep", ref ProductionGoldUpkeep);
        persister.Int32("ProductionManaUpkeep", ref ProductionManaUpkeep);
    }

    // AI.pas:2166
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (Player.PlayerType != PlayerType.Human && message.MessageId == 536870920)
        {
            DefaultBudget = (AIBudget)Budgets.FindChild(131741);
            if (DefaultBudget == null)
            {
                DefaultBudget = new AISavingsBudget(null);
                DefaultBudget.SetOwner(Budgets);
            }
        }
    }

    // AI.pas:1740
    public int GetBudgetGoldUpkeep()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).GoldUpkeep;
        }

        return result;
    }

    // AI.pas:1749
    public int GetBudgetManaUpkeep()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).ManaUpkeep;
        }

        return result;
    }

    // AI.pas:1758
    public int GetReservedBudgetGold()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).Gold;
        }

        return result;
    }

    // AI.pas:1767
    public int GetReservedBudgetMana()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).Mana;
        }

        return result;
    }

    // AI.pas:1776
    public int GetRequiredBudgetDayGold()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).DayGold;
        }

        return result;
    }

    // AI.pas:1785
    public int GetRequiredBudgetDayMana()
    {
        int result = 0;
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            result += Budgets.GetBudgets(I).DayMana;
        }

        return result;
    }

    // AI.pas:1794
    public void UpdateBudgetDayRequirements()
    {
        for (int I = Budgets.Count - 1; I >= 0; I--)
        {
            Budgets.GetBudgets(I).UpdateDayRequirements();
        }
    }

    // AI.pas:1802
    public void UpdateProductionUpkeep()
    {
        ProductionControl control;
        ProductionManaUpkeep = 0;
        ProductionGoldUpkeep = 0;
        for (int I = this.Player.ProductionControls.Count - 1; I >= 0; I--)
        {
            control = this.Player.ProductionControls.GetPC(I);
            if (control.Status == ProductionControlStatus.Producing && control.Queue.GetItem(0).Order is ProduceUnit && control.Settings.Unit != null)
            {
                ProductionGoldUpkeep += control.Settings.GoldUpkeep;
                ProductionManaUpkeep += control.Settings.ManaUpkeep;
            }
        }
    }

    // AI.pas:1825
    public int GetGroupsGoldBudget()
    {
        int result = 0;
        for (int I = this.Player.AIGroups.Count - 1; I >= 0; I--)
        {
            result += this.Player.AIGroups.GetGroups(I).GetGoldBudget();
        }

        return result;
    }

    // AI.pas:1837
    public int GetGroupsManaBudget()
    {
        int result = 0;
        for (int I = this.Player.AIGroups.Count - 1; I >= 0; I--)
        {
            result += this.Player.AIGroups.GetGroups(I).GetManaBudget();
        }

        return result;
    }

    // AI.pas:1849
    public void ReserveForBudgets(int gold, int mana)
    {
        int I;
        int M;
        int G;
        int totalGold;
        int totalMana;
        int goldLeft;
        int manaLeft;
        goldLeft = gold;
        manaLeft = mana;
        totalGold = GetRequiredBudgetDayGold();
        totalMana = GetRequiredBudgetDayMana();
        I = Budgets.Count;
        while (I != 0 && (goldLeft > 0 || manaLeft > 0))
        {
            I -= 1;
            if (totalGold != 0)
            {
                G = Round((double)(Budgets.GetBudgets(I).DayGold * gold) / totalGold);
                if (G > Budgets.GetBudgets(I).DayGold)
                {
                    G = Budgets.GetBudgets(I).DayGold;
                }

                if (G > goldLeft)
                {
                    G = goldLeft;
                }
            }
            else
            {
                G = 0;
            }

            if (totalMana != 0)
            {
                M = Round((double)(Budgets.GetBudgets(I).DayMana * mana) / totalMana);
                if (M > Budgets.GetBudgets(I).DayMana)
                {
                    M = Budgets.GetBudgets(I).DayMana;
                }

                if (M > manaLeft)
                {
                    M = manaLeft;
                }
            }
            else
            {
                M = 0;
            }

            var aiBudget = Budgets.GetBudgets(I);
            aiBudget.Gold += G;
            aiBudget.Mana += M;
            aiBudget.DayGold -= G;
            aiBudget.DayMana -= M;

            goldLeft -= G;
            manaLeft -= M;
        }
    }

    // AI.pas:1887
    public void ReturnFromBudgets(int gold, int mana)
    {
        int I;
        int G;
        int M;
        int totalGold;
        int totalMana;
        int goldLeft;
        int manaLeft;
        goldLeft = gold;
        manaLeft = mana;
        totalGold = GetReservedBudgetGold();
        totalMana = GetReservedBudgetMana();
        I = Budgets.Count;
        while (I != 0 && (goldLeft > 0 || manaLeft > 0))
        {
            I -= 1;
            if (totalGold != 0 && goldLeft > 0)
            {
                G = Round((double)(Budgets.GetBudgets(I).Gold * gold) / totalGold) + 1;
                if (G > Budgets.GetBudgets(I).Gold)
                {
                    G = Budgets.GetBudgets(I).Gold;
                }

                if (G > goldLeft)
                {
                    G = goldLeft;
                }
            }
            else
            {
                G = 0;
            }

            if (totalMana != 0 && manaLeft > 0)
            {
                // { Original bug: mana returns are weighted by reserved gold. }
                M = Round((double)(Budgets.GetBudgets(I).Gold * mana) / totalMana) + 1;
                if (M > Budgets.GetBudgets(I).Mana)
                {
                    M = Budgets.GetBudgets(I).Mana;
                }

                if (M > manaLeft)
                {
                    M = manaLeft;
                }
            }
            else
            {
                M = 0;
            }

            var aiBudget = Budgets.GetBudgets(I);
            aiBudget.Gold -= G;
            aiBudget.Mana -= M;

            goldLeft -= G;
            manaLeft -= M;
        }
    }

    // AI.pas:1924
    public void ValidateReservations()
    {
        int gold;
        int mana;
        gold = GetReservedBudgetGold() - Player.Gems;
        mana = GetReservedBudgetMana() - Player.MagicControl.Mana;
        if (gold < 0)
        {
            gold = 0;
        }

        if (mana < 0)
        {
            mana = 0;
        }

        if (gold > 0 || mana > 0)
        {
            GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_temporaryDebugChannel, "RETURNING FROM BUDGETS (GOLD:" + IntToStr(gold) + "/MANA:" + IntToStr(mana) + ")");
            ReturnFromBudgets(gold, mana);
        }
    }

    // AI.pas:1940
    public void OutputDebugInfo()
    {
        for (int I = 0, last = Budgets.Count - 1; I <= last; I++)
        {
            Budgets.GetBudgets(I).OutputDebugInfo();
        }

        var groupsGoldBudget = GetGroupsGoldBudget();
        var groupsManaBudget = GetGroupsManaBudget();
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_temporaryDebugChannel, "GROUPS BUDGET (GOLD:" + IntToStr(groupsGoldBudget) + "/MANA:" + IntToStr(groupsManaBudget) + ")");
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_temporaryDebugChannel, "POOL (GOLD:" + IntToStr(Gold) + "/MANA:" + IntToStr(Mana) + ")");
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_temporaryDebugChannel, "REAL (GOLD:" + IntToStr(Player.Gems) + "/MANA:" + IntToStr(Player.MagicControl.Mana) + ")");
    }

    // AI.pas:1954
    public void ValidateManaUpkeep()
    {
        if (ManaUpkeep < 0)
        {
            Player.MagicControl.AIOverrideUpkeep(16777215, Math.Abs(ManaUpkeep));
        }
    }

    // AI.pas:1960
    public void UpdateBudgets()
    {
        int requiredGold;
        int requiredMana;
        int gold;
        int mana;
        int goldUpkeep;
        int ManaUpkeep;
        int reserveGold;
        int reserveMana;
        ValidateReservations();
        UpdateProductionUpkeep();
        UpdateBudgetDayRequirements();
        var reservedBudgetGold = GetReservedBudgetGold();
        var groupsGoldBudget = GetGroupsGoldBudget();
        gold = Player.Gems - reservedBudgetGold - groupsGoldBudget;
        var reservedBudgetMana = GetReservedBudgetMana();
        var groupsManaBudget = GetGroupsManaBudget();
        mana = Player.MagicControl.Mana - reservedBudgetMana - groupsManaBudget;
        var upkeep = Player.GetUpkeep();
        var budgetGoldUpkeep = GetBudgetGoldUpkeep();
        goldUpkeep = upkeep + ProductionGoldUpkeep + budgetGoldUpkeep;
        var manaUpkeep = Player.MagicControl.GetManaUpkeep();
        var budgetManaUpkeep = GetBudgetManaUpkeep();
        ManaUpkeep = manaUpkeep + ProductionManaUpkeep + budgetManaUpkeep;
        GoldUpkeep = Round((double)Player.Income * GoldIncomeFactor) - goldUpkeep;
        this.ManaUpkeep = Round((double)Player.MagicControl.GetManaIncome() * ManaIncomeFactor) - ManaUpkeep;
        if (this.ManaUpkeep < 0)
        {
            ValidateManaUpkeep();
        }

        requiredGold = GetRequiredBudgetDayGold();
        requiredMana = GetRequiredBudgetDayMana();
        reserveGold = Round((double)gold * GoldReserveFactor);
        reserveMana = Round((double)mana * ManaReserveFactor);
        if (reserveGold > requiredGold)
        {
            reserveGold = requiredGold;
        }

        if (reserveMana > requiredMana)
        {
            reserveMana = requiredMana;
        }

        ReserveForBudgets(reserveGold, reserveMana);
        Gold = gold - reserveGold;
        Mana = mana - reserveMana;
    }

    // AI.pas:1985
    public bool RequestProductionExpense(int gold, int goldUpkeep, int mana, int manaUpkeep) => (gold == 0 || gold <= Gold) && (mana == 0 || mana <= Mana) && (goldUpkeep == 0 || goldUpkeep <= GoldUpkeep) && (manaUpkeep == 0 || manaUpkeep <= ManaUpkeep);

    // AI.pas:1993
    public bool RequestProductionExpense(int gold, int goldUpkeep, int mana, int manaUpkeep, AIBudget budget)
    {
        int G;
        int M;
        G = Gold;
        M = Mana;
        if (budget != null)
        {
            G += budget.Gold;
            M += budget.Mana;
        }

        return (gold == 0 || G >= gold) && (mana == 0 || mana <= M) && (goldUpkeep == 0 || GoldUpkeep >= goldUpkeep) && (manaUpkeep == 0 || ManaUpkeep >= manaUpkeep);
    }

    // AI.pas:2010
    public bool ProductionExpenseMade(int gold, int goldUpkeep, int mana, int manaUpkeep)
    {
        bool result;
        if (RequestProductionExpense(gold, goldUpkeep, mana, manaUpkeep))
        {
            if (gold < 0)
            {
                gold = 0;
            }

            if (mana < 0)
            {
                mana = 0;
            }

            if (manaUpkeep < 0)
            {
                manaUpkeep = 0;
            }

            if (goldUpkeep < 0)
            {
                goldUpkeep = 0;
            }

            Gold -= gold;
            Mana -= mana;
            GoldUpkeep -= goldUpkeep;
            ManaUpkeep -= manaUpkeep;
            ProductionGoldUpkeep += goldUpkeep;
            ProductionManaUpkeep += manaUpkeep;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:2029
    public bool RequestBudgetExpense(int gold, int mana, AIBudget budget, bool usePool)
    {
        int G;
        int M;
        G = 0;
        M = 0;
        if (budget != null)
        {
            G += budget.Gold;
            M += budget.Mana;
        }

        if (usePool)
        {
            G += Gold;
            M += Mana;
        }

        return (gold == 0 || gold <= G) && (mana == 0 || mana <= M);
    }

    // AI.pas:2049
    public bool BudgetExpenseMade(int gold, int mana, AIBudget budget, ExpenseMode mode)
    {
        bool result;
        if (RequestBudgetExpense(gold, mana, budget, mode != ExpenseMode.Budget))
        {
            if (gold < 0)
            {
                gold = 0;
            }

            if (mana < 0)
            {
                mana = 0;
            }

            if (mode == ExpenseMode.PoolThenBudget)
            {
                if (gold < Gold)
                {
                    Gold -= gold;
                    gold = 0;
                }
                else
                {
                    gold -= Gold;
                    this.Gold = 0;
                }

                if (mana < Mana)
                {
                    Mana -= mana;
                    mana = 0;
                }
                else
                {
                    mana -= Mana;
                    this.Mana = 0;
                }
            }

            if (budget != null)
            {
                if (gold < budget.Gold)
                {
                    budget.Gold -= gold;
                    gold = 0;
                }
                else
                {
                    gold -= budget.Gold;
                    budget.Gold = 0;
                }

                if (mana < budget.Mana)
                {
                    budget.Mana -= mana;
                    mana = 0;
                }
                else
                {
                    mana -= budget.Mana;
                    budget.Mana = 0;
                }
            }

            if (mode == ExpenseMode.BudgetThenPool)
            {
                if (gold < Gold)
                {
                    Gold -= gold;
                }
                else
                {
                    Gold = 0;
                }

                if (mana < Mana)
                {
                    Mana -= mana;
                }
                else
                {
                    Mana = 0;
                }
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:2113
    public bool RequestBudgetExpense(int gold, int mana, int goldUpkeep, int manaUpkeep, AIBudget budget, bool usePool)
    {
        if (gold < 0)
        {
            gold = 0;
        }

        if (mana < 0)
        {
            mana = 0;
        }

        if (manaUpkeep < 0)
        {
            manaUpkeep = 0;
        }

        if (goldUpkeep < 0)
        {
            goldUpkeep = 0;
        }

        return RequestBudgetExpense(gold, mana, budget, usePool) && (goldUpkeep == 0 || goldUpkeep <= GoldUpkeep) && (manaUpkeep == 0 || manaUpkeep <= ManaUpkeep);
    }

    // AI.pas:2124
    public bool BudgetExpenseMade(int gold, int mana, int goldUpkeep, int manaUpkeep, AIBudget budget, ExpenseMode mode)
    {
        bool result;
        if (RequestBudgetExpense(gold, mana, goldUpkeep, manaUpkeep, budget, mode != ExpenseMode.Budget))
        {
            if (gold < 0)
            {
                gold = 0;
            }

            if (mana < 0)
            {
                mana = 0;
            }

            if (manaUpkeep < 0)
            {
                manaUpkeep = 0;
            }

            if (goldUpkeep < 0)
            {
                goldUpkeep = 0;
            }

            result = BudgetExpenseMade(gold, mana, budget, mode);
            GoldUpkeep -= goldUpkeep;
            ManaUpkeep -= manaUpkeep;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:2139
    public bool RequestExpense(int gold, int mana) => gold >= 0 && mana >= 0 && gold <= Gold && mana <= Mana;

    // AI.pas:2144
    public bool ExpenseMade(int gold, int mana)
    {
        bool result;
        if (RequestExpense(gold, mana))
        {
            if (gold < 0)
            {
                gold = 0;
            }

            if (mana < 0)
            {
                mana = 0;
            }

            Gold -= gold;
            Mana -= mana;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:2157
    public bool ReturnExpense(int gold, int mana)
    {
        if (gold < 0)
        {
            gold = 0;
        }

        if (mana < 0)
        {
            mana = 0;
        }

        Gold += gold;
        Mana += mana;
        return true;
    }

    public enum ExpenseMode : byte
    {
        Budget = 0, BudgetThenPool = 1, PoolThenBudget = 2
    }

    // intf_P5_06.inc:593

}
