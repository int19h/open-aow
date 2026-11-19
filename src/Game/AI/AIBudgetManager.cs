#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Session;

using Aow.Game.World.Structures;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI;

// player, budgets, income/reserve factors, production upkeep, available upkeep, pool, savings
// budget
public partial class AIBudgetManager : EngineObject
{
    // AI.pas:1712
    public AIBudgetManager(CustomNode owner) : base(owner)
    {
        Budgets = new AIBudgetList(null) { Holder = CreationHolder() };
        _goldIncomeFactor = (float)0.75;
        _manaIncomeFactor = (float)0.75;
        _goldReserveFactor = (float)0.5;
        _manaReserveFactor = (float)0.5;
    }

    public Player Player { get; init; }
    public AIBudgetList Budgets { get; }

    private readonly float _goldIncomeFactor;
    private readonly float _manaIncomeFactor;
    private readonly float _goldReserveFactor;
    private readonly float _manaReserveFactor;

    public int ProductionGoldUpkeep { get; set; }
    public int ProductionManaUpkeep { get; set; }
    public int GoldUpkeep { get; set; }
    public int ManaUpkeep { get; set; }
    public int Gold { get; set; }
    public int Mana { get; set; }

    public AIBudget DefaultBudget { get; private set; }

    // AI.pas:1722
    public override void Dispose()
    {
        Budgets?.Dispose();
        base.Dispose();
    }

    // AI.pas:1728
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Budgets", objectValue: Budgets);
        Gold = persister.Int32("Gold", value: Gold);
        Mana = persister.Int32("Mana", value: Mana);
        GoldUpkeep = persister.Int32("GoldUpkeep", value: GoldUpkeep);
        ManaUpkeep = persister.Int32("ManaUpkeep", value: ManaUpkeep);
        ProductionGoldUpkeep = persister.Int32("ProductionGoldUpkeep", value: ProductionGoldUpkeep);
        ProductionManaUpkeep = persister.Int32("ProductionManaUpkeep", value: ProductionManaUpkeep);
    }

    // AI.pas:2166
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (Player.PlayerType != PlayerType.Human && message.MessageId == 536870920)
        {
            DefaultBudget = Budgets.FindChild<AIBudget>(131741);
            if (DefaultBudget == null)
            {
                DefaultBudget = new AISavingsBudget(null) { Holder = CreationHolder() };
                DefaultBudget.SetOwner(Budgets);
            }
        }
    }

    // AI.pas:1740
    public int GetBudgetGoldUpkeep()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).GoldUpkeep;
        }

        return result;
    }

    // AI.pas:1749
    public int GetBudgetManaUpkeep()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).ManaUpkeep;
        }

        return result;
    }

    // AI.pas:1758
    public int GetReservedBudgetGold()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).Gold;
        }

        return result;
    }

    // AI.pas:1767
    public int GetReservedBudgetMana()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).Mana;
        }

        return result;
    }

    // AI.pas:1776
    public int GetRequiredBudgetDayGold()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).DayGold;
        }

        return result;
    }

    // AI.pas:1785
    public int GetRequiredBudgetDayMana()
    {
        int result = 0;
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            result += Budgets.GetBudgets(i).DayMana;
        }

        return result;
    }

    // AI.pas:1794
    public void UpdateBudgetDayRequirements()
    {
        for (int i = Budgets.Count - 1; i >= 0; i--)
        {
            Budgets.GetBudgets(i).UpdateDayRequirements();
        }
    }

    // AI.pas:1802
    public void UpdateProductionUpkeep()
    {
        ProductionControl control;
        ProductionManaUpkeep = 0;
        ProductionGoldUpkeep = 0;
        for (int i = Player.ProductionControls.Count - 1; i >= 0; i--)
        {
            control = Player.ProductionControls.GetPC(i);
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
        for (int i = Player.AIGroups.Count - 1; i >= 0; i--)
        {
            result += Player.AIGroups.GetGroups(i).GetGoldBudget();
        }

        return result;
    }

    // AI.pas:1837
    public int GetGroupsManaBudget()
    {
        int result = 0;
        for (int i = Player.AIGroups.Count - 1; i >= 0; i--)
        {
            result += Player.AIGroups.GetGroups(i).GetManaBudget();
        }

        return result;
    }

    // AI.pas:1849
    public void ReserveForBudgets(int gold, int mana)
    {
        int i;
        int m;
        int g;
        int totalGold;
        int totalMana;
        int goldLeft;
        int manaLeft;
        goldLeft = gold;
        manaLeft = mana;
        totalGold = GetRequiredBudgetDayGold();
        totalMana = GetRequiredBudgetDayMana();
        i = Budgets.Count;
        while (i != 0 && (goldLeft > 0 || manaLeft > 0))
        {
            i -= 1;
            if (totalGold != 0)
            {
                g = Round((double)(Budgets.GetBudgets(i).DayGold * gold) / totalGold);
                if (g > Budgets.GetBudgets(i).DayGold)
                {
                    g = Budgets.GetBudgets(i).DayGold;
                }

                if (g > goldLeft)
                {
                    g = goldLeft;
                }
            }
            else
            {
                g = 0;
            }

            if (totalMana != 0)
            {
                m = Round((double)(Budgets.GetBudgets(i).DayMana * mana) / totalMana);
                if (m > Budgets.GetBudgets(i).DayMana)
                {
                    m = Budgets.GetBudgets(i).DayMana;
                }

                if (m > manaLeft)
                {
                    m = manaLeft;
                }
            }
            else
            {
                m = 0;
            }

            AIBudget aiBudget = Budgets.GetBudgets(i);
            aiBudget.Gold += g;
            aiBudget.Mana += m;
            aiBudget.DayGold -= g;
            aiBudget.DayMana -= m;

            goldLeft -= g;
            manaLeft -= m;
        }
    }

    // AI.pas:1887
    public void ReturnFromBudgets(int gold, int mana)
    {
        int i;
        int g;
        int m;
        int totalGold;
        int totalMana;
        int goldLeft;
        int manaLeft;
        goldLeft = gold;
        manaLeft = mana;
        totalGold = GetReservedBudgetGold();
        totalMana = GetReservedBudgetMana();
        i = Budgets.Count;
        while (i != 0 && (goldLeft > 0 || manaLeft > 0))
        {
            i -= 1;
            if (totalGold != 0 && goldLeft > 0)
            {
                g = Round((double)(Budgets.GetBudgets(i).Gold * gold) / totalGold) + 1;
                if (g > Budgets.GetBudgets(i).Gold)
                {
                    g = Budgets.GetBudgets(i).Gold;
                }

                if (g > goldLeft)
                {
                    g = goldLeft;
                }
            }
            else
            {
                g = 0;
            }

            if (totalMana != 0 && manaLeft > 0)
            {
                // { Original bug: mana returns are weighted by reserved gold. }
                m = Round((double)(Budgets.GetBudgets(i).Gold * mana) / totalMana) + 1;
                if (m > Budgets.GetBudgets(i).Mana)
                {
                    m = Budgets.GetBudgets(i).Mana;
                }

                if (m > manaLeft)
                {
                    m = manaLeft;
                }
            }
            else
            {
                m = 0;
            }

            AIBudget aiBudget = Budgets.GetBudgets(i);
            aiBudget.Gold -= g;
            aiBudget.Mana -= m;

            goldLeft -= g;
            manaLeft -= m;
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
            GetEngine().OutputDebugMessage(DebugChannel.Temporary, $"RETURNING FROM BUDGETS (GOLD:{IntToStr(gold)}/MANA:{IntToStr(mana)})");
            ReturnFromBudgets(gold, mana);
        }
    }

    // AI.pas:1940
    public void OutputDebugInfo()
    {
        for (int i = 0, last = Budgets.Count - 1; i <= last; i++)
        {
            Budgets.GetBudgets(i).OutputDebugInfo();
        }

        int groupsGoldBudget = GetGroupsGoldBudget();
        int groupsManaBudget = GetGroupsManaBudget();
        GetEngine().OutputDebugMessage(DebugChannel.Temporary, $"GROUPS BUDGET (GOLD:{IntToStr(groupsGoldBudget)}/MANA:{IntToStr(groupsManaBudget)})");
        GetEngine().OutputDebugMessage(DebugChannel.Temporary, $"POOL (GOLD:{IntToStr(Gold)}/MANA:{IntToStr(Mana)})");
        GetEngine().OutputDebugMessage(DebugChannel.Temporary, $"REAL (GOLD:{IntToStr(Player.Gems)}/MANA:{IntToStr(Player.MagicControl.Mana)})");
    }

    // AI.pas:1954
    public void ValidateManaUpkeep()
    {
        if (ManaUpkeep < 0)
        {
            _ = Player.MagicControl.AIOverrideUpkeep(16777215, Math.Abs(ManaUpkeep));
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
        int manaUpkeep2;
        int reserveGold;
        int reserveMana;
        ValidateReservations();
        UpdateProductionUpkeep();
        UpdateBudgetDayRequirements();
        int reservedBudgetGold = GetReservedBudgetGold();
        int groupsGoldBudget = GetGroupsGoldBudget();
        gold = Player.Gems - reservedBudgetGold - groupsGoldBudget;
        int reservedBudgetMana = GetReservedBudgetMana();
        int groupsManaBudget = GetGroupsManaBudget();
        mana = Player.MagicControl.Mana - reservedBudgetMana - groupsManaBudget;
        int upkeep = Player.GetUpkeep();
        int budgetGoldUpkeep = GetBudgetGoldUpkeep();
        goldUpkeep = upkeep + ProductionGoldUpkeep + budgetGoldUpkeep;
        int manaUpkeep = Player.MagicControl.GetManaUpkeep();
        int budgetManaUpkeep = GetBudgetManaUpkeep();
        manaUpkeep2 = manaUpkeep + ProductionManaUpkeep + budgetManaUpkeep;
        GoldUpkeep = Round((double)Player.Income * _goldIncomeFactor) - goldUpkeep;
        ManaUpkeep = Round((double)Player.MagicControl.GetManaIncome() * _manaIncomeFactor) - manaUpkeep2;
        if (ManaUpkeep < 0)
        {
            ValidateManaUpkeep();
        }

        requiredGold = GetRequiredBudgetDayGold();
        requiredMana = GetRequiredBudgetDayMana();
        reserveGold = Round((double)gold * _goldReserveFactor);
        reserveMana = Round((double)mana * _manaReserveFactor);
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
        int g;
        int m;
        g = Gold;
        m = Mana;
        if (budget != null)
        {
            g += budget.Gold;
            m += budget.Mana;
        }

        return (gold == 0 || g >= gold) && (mana == 0 || mana <= m) && (goldUpkeep == 0 || GoldUpkeep >= goldUpkeep) && (manaUpkeep == 0 || ManaUpkeep >= manaUpkeep);
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
        int g;
        int m;
        g = 0;
        m = 0;
        if (budget != null)
        {
            g += budget.Gold;
            m += budget.Mana;
        }

        if (usePool)
        {
            g += Gold;
            m += Mana;
        }

        return (gold == 0 || gold <= g) && (mana == 0 || mana <= m);
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
                    Gold = 0;
                }

                if (mana < Mana)
                {
                    Mana -= mana;
                    mana = 0;
                }
                else
                {
                    mana -= Mana;
                    Mana = 0;
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
