// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai;

// player, budgets, income/reserve factors, production upkeep, available upkeep, pool, savings
// budget
public partial class AIBudgetManager : EObject
{
    // AI.pas:1712
    public AIBudgetManager(ECustomNode owner) : base(owner)
    {
        FBudgets = new AIBudgetList(null);
        FGoldIncomeFactor = (float)0.75;
        FManaIncomeFactor = (float)0.75;
        FGoldReserveFactor = (float)0.5;
        FManaReserveFactor = (float)0.5;
    }

    public Player FPlayer;
    public AIBudgetList FBudgets;
    public float FGoldIncomeFactor;
    public float FManaIncomeFactor;
    public float FGoldReserveFactor;
    public float FManaReserveFactor;
    public int FProductionGoldUpkeep;
    public int FProductionManaUpkeep;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public int FGold;
    public int FMana;
    public AIBudget FDefaultBudget;

    // AI.pas:1722
    public override void Dispose()
    {
        FBudgets?.Dispose();
        base.Dispose();
    }

    // AI.pas:1728
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FBudgets, 20);
        Stream.rwInteger(ref FGold, 21);
        Stream.rwInteger(ref FMana, 22);
        Stream.rwInteger(ref FGoldUpkeep, 23);
        Stream.rwInteger(ref FManaUpkeep, 24);
        Stream.rwInteger(ref FProductionGoldUpkeep, 25);
        Stream.rwInteger(ref FProductionManaUpkeep, 26);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Budgets", FBudgets);
        persister.Int("Gold", ref FGold);
        persister.Int("Mana", ref FMana);
        persister.Int("GoldUpkeep", ref FGoldUpkeep);
        persister.Int("ManaUpkeep", ref FManaUpkeep);
        persister.Int("ProductionGoldUpkeep", ref FProductionGoldUpkeep);
        persister.Int("ProductionManaUpkeep", ref FProductionManaUpkeep);
    }

    // AI.pas:2166
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (FPlayer.FPlayerType != PlayerType.Human && Msg.MsgID == 536870920)
        {
            FDefaultBudget = (AIBudget)FBudgets.FindChild(131741);
            if (FDefaultBudget == null)
            {
                FDefaultBudget = new AISavingsBudget(null);
                FDefaultBudget.SetOwner(FBudgets);
            }
        }
    }

    // AI.pas:1740
    public int GetBudgetGoldUpkeep()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FGoldUpkeep;
        }

        return Result;
    }

    // AI.pas:1749
    public int GetBudgetManaUpkeep()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FManaUpkeep;
        }

        return Result;
    }

    // AI.pas:1758
    public int GetReservedBudgetGold()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FGold;
        }

        return Result;
    }

    // AI.pas:1767
    public int GetReservedBudgetMana()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FMana;
        }

        return Result;
    }

    // AI.pas:1776
    public int GetRequiredBudgetDayGold()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FDayGold;
        }

        return Result;
    }

    // AI.pas:1785
    public int GetRequiredBudgetDayMana()
    {
        int Result = 0;
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            Result += FBudgets.GetBudgets(I).FDayMana;
        }

        return Result;
    }

    // AI.pas:1794
    public void UpdateBudgetDayRequirements()
    {
        for (int I = FBudgets.Count - 1; I >= 0; I--)
        {
            FBudgets.GetBudgets(I).UpdateDayRequirements();
        }
    }

    // AI.pas:1802
    public void UpdateProductionUpkeep()
    {
        ProductionControl Control;
        FProductionManaUpkeep = 0;
        FProductionGoldUpkeep = 0;
        for (int I = this.FPlayer.FProductionControls.Count - 1; I >= 0; I--)
        {
            Control = this.FPlayer.FProductionControls.GetPC(I);
            if (Control.FStatus == ProductionControl.Status.Producing && Control.FQueue.GetItem(0).Order is ProduceUnit && Control.FSettings.FUnit != null)
            {
                FProductionGoldUpkeep += Control.FSettings.FGoldUpkeep;
                FProductionManaUpkeep += Control.FSettings.FManaUpkeep;
            }
        }
    }

    // AI.pas:1825
    public int GetGroupsGoldBudget()
    {
        int Result = 0;
        for (int I = this.FPlayer.FAIGroups.Count - 1; I >= 0; I--)
        {
            Result += this.FPlayer.FAIGroups.GetGroups(I).GetGoldBudget();
        }

        return Result;
    }

    // AI.pas:1837
    public int GetGroupsManaBudget()
    {
        int Result = 0;
        for (int I = this.FPlayer.FAIGroups.Count - 1; I >= 0; I--)
        {
            Result += this.FPlayer.FAIGroups.GetGroups(I).GetManaBudget();
        }

        return Result;
    }

    // AI.pas:1849
    public void ReserveForBudgets(int Gold, int Mana)
    {
        int I;
        int M;
        int G;
        int TotalGold;
        int TotalMana;
        int GoldLeft;
        int ManaLeft;
        GoldLeft = Gold;
        ManaLeft = Mana;
        TotalGold = GetRequiredBudgetDayGold();
        TotalMana = GetRequiredBudgetDayMana();
        I = FBudgets.Count;
        while (I != 0 && (GoldLeft > 0 || ManaLeft > 0))
        {
            I -= 1;
            if (TotalGold != 0)
            {
                G = Round((double)(FBudgets.GetBudgets(I).FDayGold * Gold) / TotalGold);
                if (G > FBudgets.GetBudgets(I).FDayGold)
                {
                    G = FBudgets.GetBudgets(I).FDayGold;
                }

                if (G > GoldLeft)
                {
                    G = GoldLeft;
                }
            }
            else
            {
                G = 0;
            }

            if (TotalMana != 0)
            {
                M = Round((double)(FBudgets.GetBudgets(I).FDayMana * Mana) / TotalMana);
                if (M > FBudgets.GetBudgets(I).FDayMana)
                {
                    M = FBudgets.GetBudgets(I).FDayMana;
                }

                if (M > ManaLeft)
                {
                    M = ManaLeft;
                }
            }
            else
            {
                M = 0;
            }

            var aiBudget = FBudgets.GetBudgets(I);
            aiBudget.FGold += G;
            aiBudget.FMana += M;
            aiBudget.FDayGold -= G;
            aiBudget.FDayMana -= M;

            GoldLeft -= G;
            ManaLeft -= M;
        }
    }

    // AI.pas:1887
    public void ReturnFromBudgets(int Gold, int Mana)
    {
        int I;
        int G;
        int M;
        int TotalGold;
        int TotalMana;
        int GoldLeft;
        int ManaLeft;
        GoldLeft = Gold;
        ManaLeft = Mana;
        TotalGold = GetReservedBudgetGold();
        TotalMana = GetReservedBudgetMana();
        I = FBudgets.Count;
        while (I != 0 && (GoldLeft > 0 || ManaLeft > 0))
        {
            I -= 1;
            if (TotalGold != 0 && GoldLeft > 0)
            {
                G = Round((double)(FBudgets.GetBudgets(I).FGold * Gold) / TotalGold) + 1;
                if (G > FBudgets.GetBudgets(I).FGold)
                {
                    G = FBudgets.GetBudgets(I).FGold;
                }

                if (G > GoldLeft)
                {
                    G = GoldLeft;
                }
            }
            else
            {
                G = 0;
            }

            if (TotalMana != 0 && ManaLeft > 0)
            {
                // { Original bug: mana returns are weighted by reserved gold. }
                M = Round((double)(FBudgets.GetBudgets(I).FGold * Mana) / TotalMana) + 1;
                if (M > FBudgets.GetBudgets(I).FMana)
                {
                    M = FBudgets.GetBudgets(I).FMana;
                }

                if (M > ManaLeft)
                {
                    M = ManaLeft;
                }
            }
            else
            {
                M = 0;
            }

            var aiBudget = FBudgets.GetBudgets(I);
            aiBudget.FGold -= G;
            aiBudget.FMana -= M;

            GoldLeft -= G;
            ManaLeft -= M;
        }
    }

    // AI.pas:1924
    public void ValidateReservations()
    {
        int Gold;
        int Mana;
        Gold = GetReservedBudgetGold() - FPlayer.FGems;
        Mana = GetReservedBudgetMana() - FPlayer.FMagicControl.FMana;
        if (Gold < 0)
        {
            Gold = 0;
        }

        if (Mana < 0)
        {
            Mana = 0;
        }

        if (Gold > 0 || Mana > 0)
        {
            GameTables.AoWEngine.OutputDebugMessage(AbilityRules.TMPDID, "RETURNING FROM BUDGETS (GOLD:" + IntToStr(Gold) + "/MANA:" + IntToStr(Mana) + ")");
            ReturnFromBudgets(Gold, Mana);
        }
    }

    // AI.pas:1940
    public void OutputDebugInfo()
    {
        for (int I = 0, last = FBudgets.Count - 1; I <= last; I++)
        {
            FBudgets.GetBudgets(I).OutputDebugInfo();
        }

        var groupsGoldBudget = GetGroupsGoldBudget();
        var groupsManaBudget = GetGroupsManaBudget();
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.TMPDID, "GROUPS BUDGET (GOLD:" + IntToStr(groupsGoldBudget) + "/MANA:" + IntToStr(groupsManaBudget) + ")");
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.TMPDID, "POOL (GOLD:" + IntToStr(FGold) + "/MANA:" + IntToStr(FMana) + ")");
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.TMPDID, "REAL (GOLD:" + IntToStr(FPlayer.FGems) + "/MANA:" + IntToStr(FPlayer.FMagicControl.FMana) + ")");
    }

    // AI.pas:1954
    public void ValidateManaUpkeep()
    {
        if (FManaUpkeep < 0)
        {
            FPlayer.FMagicControl.AIOverrideUpkeep(16777215, Math.Abs(FManaUpkeep));
        }
    }

    // AI.pas:1960
    public void UpdateBudgets()
    {
        int RequiredGold;
        int RequiredMana;
        int Gold;
        int Mana;
        int GoldUpkeep;
        int ManaUpkeep;
        int ReserveGold;
        int ReserveMana;
        ValidateReservations();
        UpdateProductionUpkeep();
        UpdateBudgetDayRequirements();
        var reservedBudgetGold = GetReservedBudgetGold();
        var groupsGoldBudget = GetGroupsGoldBudget();
        Gold = FPlayer.FGems - reservedBudgetGold - groupsGoldBudget;
        var reservedBudgetMana = GetReservedBudgetMana();
        var groupsManaBudget = GetGroupsManaBudget();
        Mana = FPlayer.FMagicControl.FMana - reservedBudgetMana - groupsManaBudget;
        var upkeep = FPlayer.GetUpkeep();
        var budgetGoldUpkeep = GetBudgetGoldUpkeep();
        GoldUpkeep = upkeep + FProductionGoldUpkeep + budgetGoldUpkeep;
        var manaUpkeep = FPlayer.FMagicControl.GetManaUpkeep();
        var budgetManaUpkeep = GetBudgetManaUpkeep();
        ManaUpkeep = manaUpkeep + FProductionManaUpkeep + budgetManaUpkeep;
        FGoldUpkeep = Round((double)FPlayer.FIncome * FGoldIncomeFactor) - GoldUpkeep;
        FManaUpkeep = Round((double)FPlayer.FMagicControl.GetManaIncome() * FManaIncomeFactor) - ManaUpkeep;
        if (FManaUpkeep < 0)
        {
            ValidateManaUpkeep();
        }

        RequiredGold = GetRequiredBudgetDayGold();
        RequiredMana = GetRequiredBudgetDayMana();
        ReserveGold = Round((double)Gold * FGoldReserveFactor);
        ReserveMana = Round((double)Mana * FManaReserveFactor);
        if (ReserveGold > RequiredGold)
        {
            ReserveGold = RequiredGold;
        }

        if (ReserveMana > RequiredMana)
        {
            ReserveMana = RequiredMana;
        }

        ReserveForBudgets(ReserveGold, ReserveMana);
        FGold = Gold - ReserveGold;
        FMana = Mana - ReserveMana;
    }

    // AI.pas:1985
    public bool RequestProductionExpense(int Gold, int GoldUpkeep, int Mana, int ManaUpkeep) => (Gold == 0 || Gold <= FGold) && (Mana == 0 || Mana <= FMana) && (GoldUpkeep == 0 || GoldUpkeep <= FGoldUpkeep) && (ManaUpkeep == 0 || ManaUpkeep <= FManaUpkeep);

    // AI.pas:1993
    public bool RequestProductionExpenseEx(int Gold, int GoldUpkeep, int Mana, int ManaUpkeep, AIBudget Budget)
    {
        int G;
        int M;
        G = FGold;
        M = FMana;
        if (Budget != null)
        {
            G += Budget.FGold;
            M += Budget.FMana;
        }

        return (Gold == 0 || G >= Gold) && (Mana == 0 || Mana <= M) && (GoldUpkeep == 0 || FGoldUpkeep >= GoldUpkeep) && (ManaUpkeep == 0 || FManaUpkeep >= ManaUpkeep);
    }

    // AI.pas:2010
    public bool ProductionExpenseMade(int Gold, int GoldUpkeep, int Mana, int ManaUpkeep)
    {
        bool Result;
        if (RequestProductionExpense(Gold, GoldUpkeep, Mana, ManaUpkeep))
        {
            if (Gold < 0)
            {
                Gold = 0;
            }

            if (Mana < 0)
            {
                Mana = 0;
            }

            if (ManaUpkeep < 0)
            {
                ManaUpkeep = 0;
            }

            if (GoldUpkeep < 0)
            {
                GoldUpkeep = 0;
            }

            FGold -= Gold;
            FMana -= Mana;
            FGoldUpkeep -= GoldUpkeep;
            FManaUpkeep -= ManaUpkeep;
            FProductionGoldUpkeep += GoldUpkeep;
            FProductionManaUpkeep += ManaUpkeep;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:2029
    public bool RequestBudgetExpense(int Gold, int Mana, AIBudget Budget, bool UsePool)
    {
        int G;
        int M;
        G = 0;
        M = 0;
        if (Budget != null)
        {
            G += Budget.FGold;
            M += Budget.FMana;
        }

        if (UsePool)
        {
            G += FGold;
            M += FMana;
        }

        return (Gold == 0 || Gold <= G) && (Mana == 0 || Mana <= M);
    }

    // AI.pas:2049
    public bool BudgetExpenseMade(int Gold, int Mana, AIBudget Budget, BudgetExpenseMode Mode)
    {
        bool Result;
        if (RequestBudgetExpense(Gold, Mana, Budget, Mode != BudgetExpenseMode.Budget))
        {
            if (Gold < 0)
            {
                Gold = 0;
            }

            if (Mana < 0)
            {
                Mana = 0;
            }

            if (Mode == BudgetExpenseMode.PoolThenBudget)
            {
                if (Gold < FGold)
                {
                    FGold -= Gold;
                    Gold = 0;
                }
                else
                {
                    Gold -= FGold;
                    FGold = 0;
                }

                if (Mana < FMana)
                {
                    FMana -= Mana;
                    Mana = 0;
                }
                else
                {
                    Mana -= FMana;
                    FMana = 0;
                }
            }

            if (Budget != null)
            {
                if (Gold < Budget.FGold)
                {
                    Budget.FGold -= Gold;
                    Gold = 0;
                }
                else
                {
                    Gold -= Budget.FGold;
                    Budget.FGold = 0;
                }

                if (Mana < Budget.FMana)
                {
                    Budget.FMana -= Mana;
                    Mana = 0;
                }
                else
                {
                    Mana -= Budget.FMana;
                    Budget.FMana = 0;
                }
            }

            if (Mode == BudgetExpenseMode.BudgetThenPool)
            {
                if (Gold < FGold)
                {
                    FGold -= Gold;
                }
                else
                {
                    FGold = 0;
                }

                if (Mana < FMana)
                {
                    FMana -= Mana;
                }
                else
                {
                    FMana = 0;
                }
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:2113
    public bool RequestBudgetExpenseEx(int Gold, int Mana, int GoldUpkeep, int ManaUpkeep, AIBudget Budget, bool UsePool)
    {
        if (Gold < 0)
        {
            Gold = 0;
        }

        if (Mana < 0)
        {
            Mana = 0;
        }

        if (ManaUpkeep < 0)
        {
            ManaUpkeep = 0;
        }

        if (GoldUpkeep < 0)
        {
            GoldUpkeep = 0;
        }

        return RequestBudgetExpense(Gold, Mana, Budget, UsePool) && (GoldUpkeep == 0 || GoldUpkeep <= FGoldUpkeep) && (ManaUpkeep == 0 || ManaUpkeep <= FManaUpkeep);
    }

    // AI.pas:2124
    public bool BudgetExpenseMadeEx(int Gold, int Mana, int GoldUpkeep, int ManaUpkeep, AIBudget Budget, BudgetExpenseMode Mode)
    {
        bool Result;
        if (RequestBudgetExpenseEx(Gold, Mana, GoldUpkeep, ManaUpkeep, Budget, Mode != BudgetExpenseMode.Budget))
        {
            if (Gold < 0)
            {
                Gold = 0;
            }

            if (Mana < 0)
            {
                Mana = 0;
            }

            if (ManaUpkeep < 0)
            {
                ManaUpkeep = 0;
            }

            if (GoldUpkeep < 0)
            {
                GoldUpkeep = 0;
            }

            Result = BudgetExpenseMade(Gold, Mana, Budget, Mode);
            FGoldUpkeep -= GoldUpkeep;
            FManaUpkeep -= ManaUpkeep;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:2139
    public bool RequestExpense(int Gold, int Mana) => Gold >= 0 && Mana >= 0 && Gold <= FGold && Mana <= FMana;

    // AI.pas:2144
    public bool ExpenseMade(int Gold, int Mana)
    {
        bool Result;
        if (RequestExpense(Gold, Mana))
        {
            if (Gold < 0)
            {
                Gold = 0;
            }

            if (Mana < 0)
            {
                Mana = 0;
            }

            FGold -= Gold;
            FMana -= Mana;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:2157
    public bool ReturnExpense(int Gold, int Mana)
    {
        if (Gold < 0)
        {
            Gold = 0;
        }

        if (Mana < 0)
        {
            Mana = 0;
        }

        FGold += Gold;
        FMana += Mana;
        return true;
    }

    public enum BudgetExpenseMode : byte
    {
        Budget = 0, BudgetThenPool = 1, PoolThenBudget = 2
    }

    // intf_P5_06.inc:593

}
