using Aow.Game.Combat;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;

namespace Aow.Headless;

// The input of the headless host. A directed cast stays accepted, no dialog opens and the
// handed caster is released, as a raise with no subscriber behaved before the input existed.
public sealed class HeadlessPlayerInput : IPlayerInput
{
    public void ChooseSpellTargets(SpellCaster caster) => _ = caster.Release();

    public void ChooseCombatSpellTarget(CombatSpellCaster caster) => _ = caster.Release();
}
