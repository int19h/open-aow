using Aow.Game.Combat;
using Aow.Game.Magic.Spells;

namespace Aow.Game.Session;

// The input of one player. The rules call it when a player choice is needed.
public interface IPlayerInput
{
    // Asks for the targets of a strategic spell. The call hands the caster to the input.
    void ChooseSpellTargets(SpellCaster caster);

    // Asks for the target of a combat spell. The call hands the caster to the input.
    void ChooseCombatSpellTarget(CombatSpellCaster caster);
}
