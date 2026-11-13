// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

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
        Priority = persister.Int32("Priority", value: Priority);
        RequiredGold = persister.Int32("RequiredGold", value: RequiredGold);
        RequiredMana = persister.Int32("RequiredMana", value: RequiredMana);
        GoldUpkeep = persister.Int32("GoldUpkeep", value: GoldUpkeep);
        ManaUpkeep = persister.Int32("ManaUpkeep", value: ManaUpkeep);
        Gold = persister.Int32("Gold", value: Gold);
        Mana = persister.Int32("Mana", value: Mana);
        Days = persister.Int32("Days", value: Days);
        DayGoldMax = persister.Int32("DayGoldMax", value: DayGoldMax);
        DayManaMax = persister.Int32("DayManaMax", value: DayManaMax);
        DayGold = persister.Int32("DayGold", value: DayGold);
        DayMana = persister.Int32("DayMana", value: DayMana);
    }

    // AI.pas:1702
    public virtual bool GetReady() => RequiredGold == Gold && RequiredMana == Mana;

    // AI.pas:1678
    public virtual void OutputDebugInfo()
    {
        GetEngine().OutputDebugMessage(DebugChannel.Temporary, this.GetType().Name + " Gold:" + IntToStr(Gold) + "/" + IntToStr(RequiredGold) + " Mana:" + IntToStr(Mana) + "/" + IntToStr(RequiredMana));
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
