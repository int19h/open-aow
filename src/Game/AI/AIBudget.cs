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
    public new const int ClassId = 131740;

    public AIBudget(ECustomNode owner) : base(owner)
    { }

    public int Priority;
    public int RequiredGold;
    public int RequiredMana;
    public int GoldUpkeep;
    public int ManaUpkeep;
    public int Days;
    public int DayGoldMax;
    public int DayManaMax;
    public int DayGold;
    public int DayMana;
    public int Gold;
    public int Mana;

    // AI.pas:1658
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Priority, 30);
        stream.rwInteger(ref RequiredGold, 31);
        stream.rwInteger(ref RequiredMana, 32);
        stream.rwInteger(ref GoldUpkeep, 33);
        stream.rwInteger(ref ManaUpkeep, 34);
        stream.rwInteger(ref Gold, 35);
        stream.rwInteger(ref Mana, 36);
        stream.rwInteger(ref Days, 37);
        stream.rwInteger(ref DayGoldMax, 38);
        stream.rwInteger(ref DayManaMax, 39);
        stream.rwInteger(ref DayGold, 40);
        stream.rwInteger(ref DayMana, 41);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("Priority", ref Priority);
        persister.Int32("RequiredGold", ref RequiredGold);
        persister.Int32("RequiredMana", ref RequiredMana);
        persister.Int32("GoldUpkeep", ref GoldUpkeep);
        persister.Int32("ManaUpkeep", ref ManaUpkeep);
        persister.Int32("Gold", ref Gold);
        persister.Int32("Mana", ref Mana);
        persister.Int32("Days", ref Days);
        persister.Int32("DayGoldMax", ref DayGoldMax);
        persister.Int32("DayManaMax", ref DayManaMax);
        persister.Int32("DayGold", ref DayGold);
        persister.Int32("DayMana", ref DayMana);
    }

    // AI.pas:1702
    public virtual bool GetReady() => RequiredGold == Gold && RequiredMana == Mana;

    // AI.pas:1678
    public virtual void OutputDebugInfo()
    {
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.TemporaryDebugChannel, this.GetType().Name + " Gold:" + IntToStr(Gold) + "/" + IntToStr(RequiredGold) + " Mana:" + IntToStr(Mana) + "/" + IntToStr(RequiredMana));
    }

    // AI.pas:1675
    public virtual void Done()
    {
    }

    // AI.pas:1685
    public virtual void UpdateDayRequirements()
    {
        DayGold = RequiredGold - Gold;
        DayMana = RequiredMana - Mana;
        if (Days != 0)
        {
            if (Days > 1)
            {
                Days -= 1;
            }

            DayGold = Round((double)DayGold / Days);
            DayMana = Round((double)DayMana / Days);
        }
        else
        {
            if (DayGoldMax < DayGold)
            {
                DayGold = DayGoldMax;
            }

            if (DayManaMax < DayMana)
            {
                DayMana = DayManaMax;
            }
        }
    }

}
