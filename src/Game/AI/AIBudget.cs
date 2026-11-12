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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref Priority, 30);
        stream.Int32(ref RequiredGold, 31);
        stream.Int32(ref RequiredMana, 32);
        stream.Int32(ref GoldUpkeep, 33);
        stream.Int32(ref ManaUpkeep, 34);
        stream.Int32(ref Gold, 35);
        stream.Int32(ref Mana, 36);
        stream.Int32(ref Days, 37);
        stream.Int32(ref DayGoldMax, 38);
        stream.Int32(ref DayManaMax, 39);
        stream.Int32(ref DayGold, 40);
        stream.Int32(ref DayMana, 41);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Priority = persister.Int32("Priority", Priority);
        RequiredGold = persister.Int32("RequiredGold", RequiredGold);
        RequiredMana = persister.Int32("RequiredMana", RequiredMana);
        GoldUpkeep = persister.Int32("GoldUpkeep", GoldUpkeep);
        ManaUpkeep = persister.Int32("ManaUpkeep", ManaUpkeep);
        Gold = persister.Int32("Gold", Gold);
        Mana = persister.Int32("Mana", Mana);
        Days = persister.Int32("Days", Days);
        DayGoldMax = persister.Int32("DayGoldMax", DayGoldMax);
        DayManaMax = persister.Int32("DayManaMax", DayManaMax);
        DayGold = persister.Int32("DayGold", DayGold);
        DayMana = persister.Int32("DayMana", DayMana);
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
