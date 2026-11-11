// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// priority, required gold/mana, upkeep, days, daily limits, daily requirements, gold/mana
public partial class AIBudget : EngineObject
{
    // AI.pas:1653
    public new const int ClassID = 131740;

    public AIBudget(ECustomNode owner) : base(owner)
    { }

    public int FPriority;
    public int FRequiredGold;
    public int FRequiredMana;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public int FDays;
    public int FDayGoldMax;
    public int FDayManaMax;
    public int FDayGold;
    public int FDayMana;
    public int FGold;
    public int FMana;

    // AI.pas:1658
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FPriority, 30);
        Stream.rwInteger(ref FRequiredGold, 31);
        Stream.rwInteger(ref FRequiredMana, 32);
        Stream.rwInteger(ref FGoldUpkeep, 33);
        Stream.rwInteger(ref FManaUpkeep, 34);
        Stream.rwInteger(ref FGold, 35);
        Stream.rwInteger(ref FMana, 36);
        Stream.rwInteger(ref FDays, 37);
        Stream.rwInteger(ref FDayGoldMax, 38);
        Stream.rwInteger(ref FDayManaMax, 39);
        Stream.rwInteger(ref FDayGold, 40);
        Stream.rwInteger(ref FDayMana, 41);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("Priority", ref FPriority);
        persister.Int("RequiredGold", ref FRequiredGold);
        persister.Int("RequiredMana", ref FRequiredMana);
        persister.Int("GoldUpkeep", ref FGoldUpkeep);
        persister.Int("ManaUpkeep", ref FManaUpkeep);
        persister.Int("Gold", ref FGold);
        persister.Int("Mana", ref FMana);
        persister.Int("Days", ref FDays);
        persister.Int("DayGoldMax", ref FDayGoldMax);
        persister.Int("DayManaMax", ref FDayManaMax);
        persister.Int("DayGold", ref FDayGold);
        persister.Int("DayMana", ref FDayMana);
    }

    // AI.pas:1702
    public virtual bool GetReady() => FRequiredGold == FGold && FRequiredMana == FMana;

    // AI.pas:1678
    public virtual void OutputDebugInfo()
    {
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.TMPDID, this.GetType().Name + " Gold:" + IntToStr(FGold) + "/" + IntToStr(FRequiredGold) + " Mana:" + IntToStr(FMana) + "/" + IntToStr(FRequiredMana));
    }

    // AI.pas:1675
    public virtual void Done()
    {
    }

    // AI.pas:1685
    public virtual void UpdateDayRequirements()
    {
        FDayGold = FRequiredGold - FGold;
        FDayMana = FRequiredMana - FMana;
        if (FDays != 0)
        {
            if (FDays > 1)
            {
                FDays -= 1;
            }

            FDayGold = Round((double)FDayGold / FDays);
            FDayMana = Round((double)FDayMana / FDays);
        }
        else
        {
            if (FDayGoldMax < FDayGold)
            {
                FDayGold = FDayGoldMax;
            }

            if (FDayManaMax < FDayMana)
            {
                FDayMana = FDayManaMax;
            }
        }
    }

}
