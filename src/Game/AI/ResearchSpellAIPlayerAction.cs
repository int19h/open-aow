// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.AI;

public class ResearchSpellAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    // Spells.PAS:1504
    public override bool Process()
    {
        if (Control.Player.PlayerIndex != 0 && Control.Player.MagicControl.ResearchSpellId == 0)
        {
            SpellList spells = new();
            DoubleItemList choices = new();
            Control.Player.MagicControl.ListResearchSpells(spells, Set<SpellType>.Of(SpellType.Combat, SpellType.Unit, SpellType.Global));
            for (int i = 0, last = spells.GetCount() - 1; i <= last; i++)
            {
                int value = (spells.GetSpell(i).CastDuration * 100) + 100;
                Spell spell = spells.GetSpell(i);
                int netPowerToResearch = Control.Player.MagicControl.GetNetPowerToResearch();
                value -= Control.Player.MagicControl.ResearchTurns(netPowerToResearch, spell.ResearchCost);
                if (value <= 0)
                {
                    value = 1;
                }

                _ = choices.AddAbility(spells.GetSpell(i).SpellId, value);
            }

            if (choices.Count != 0)
            {
                RandSeed = StrategicMap.Current.Random(16777215);
                int index = choices.RandomChanceItemB(0);
                DoubleItemList.DoubleItem item = choices[index];
                _ = Control.Player.MagicControl.ExecuteResearchSpell(item.First);
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
