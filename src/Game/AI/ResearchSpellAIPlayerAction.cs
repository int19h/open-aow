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
    public ResearchSpellAIPlayerAction(CustomNode owner) : base(owner)
    { }

    // Spells.PAS:1504
    public override bool Process()
    {
        DoubleItemList choices;
        SpellList spells;
        int value;
        int index;
        if (Control.Player.PlayerIndex != 0 && Control.Player.MagicControl.ResearchSpellId == 0)
        {
            spells = new SpellList();
            choices = new DoubleItemList();
            Control.Player.MagicControl.ListResearchSpells(ref spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
            for (int i = 0, last = spells.GetCount() - 1; i <= last; i++)
            {
                value = spells.GetSpell(i).CastDuration * 100 + 100;
                var spell = spells.GetSpell(i);
                var netPowerToResearch = Control.Player.MagicControl.GetNetPowerToResearch();
                value = value - Control.Player.MagicControl.ResearchTurns(netPowerToResearch, spell.ResearchCost);
                if (value <= 0)
                {
                    value = 1;
                }

                choices.AddAbility(spells.GetSpell(i).SpellId, value);
            }

            if (choices.Count != 0)
            {
                RandSeed = StrategicMap.Current.Random(16777215);
                index = choices.RandomChanceItemB(0);
                var item = choices[index];
                Control.Player.MagicControl.ExecuteResearchSpell(item.First);
            }

            choices?.Dispose();
            spells?.Dispose();
        }

        return false;
    }

    // Spells.PAS:1502
    // inherited player control
    public static new int Priority => 22;

}
