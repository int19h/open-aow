// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

public partial class ResearchSpellAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    public ResearchSpellAIPlayerAction(ECustomNode owner) : base(owner)
    { }

    // Spells.PAS:1504
    public override bool Process()
    {
        DoubleItemList Choices;
        SpellList Spells;
        int Value;
        int Index;
        if (FControl.FPlayer.FPlayerIndex != 0 && FControl.FPlayer.FMagicControl.FResearchSpellID == 0)
        {
            Spells = new SpellList();
            Choices = new DoubleItemList();
            FControl.FPlayer.FMagicControl.ListResearchSpells(ref Spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
            for (int I = 0, last = Spells.GetCount() - 1; I <= last; I++)
            {
                Value = Spells.GetSpell(I).FCastDuration * 100 + 100;
                var spell = Spells.GetSpell(I);
                var netPowerToResearch = FControl.FPlayer.FMagicControl.GetNetPowerToResearch();
                Value = Value - FControl.FPlayer.FMagicControl.ResearchTurns(netPowerToResearch, spell.FResearchCost);
                if (Value <= 0)
                {
                    Value = 1;
                }

                Choices.AddAB(Spells.GetSpell(I).FSpellID, Value);
            }

            if (Choices.Count != 0)
            {
                RandSeed = AoWHSMap.Current.Random(16777215);
                Index = Choices.RandomChanceItemB(0);
                var item = Choices[Index];
                FControl.FPlayer.FMagicControl.ExecuteResearchSpell(item.A);
            }

            Choices?.Dispose();
            Spells?.Dispose();
        }

        return false;
    }

    // Spells.PAS:1502
    // inherited player control
    public static new int Priority => 22;

}
