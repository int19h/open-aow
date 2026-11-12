// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class FastBattleSpellCommand : FastBattleCommand
{
    public FastBattleSpellCommand(int abilityId, EngineObject creator) : base(abilityId)
    {
        Creator = creator;
    }

    // The spell that created this command. The command reaches the engine through it.
    public EngineObject Creator { get; }

    public int SpellId { get; set; }

    // Spells.PAS:1
    public override string GetName()
    {
        var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
        string result = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), s_castFormat), [spell.SpellName]);
        if (Target != null)
        {
            result = result + " on " + Target.IdString();
        }

        return result;
    }

    private static readonly string s_castFormat = "Cast %s";

    // intf_P5_05.inc:10

    // intf_P5_05.inc:10

}
