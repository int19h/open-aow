using Aow.Game.Combat;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.UI;

// The input of the player at this machine. It reads the dialog modules at call time, so the
// host installs it on the engine before the dialogs load.
public sealed class LocalPlayerInput(Func<UIContext> ui) : IPlayerInput
{
    private UIContext UI => ui();

    // The call hands the caster to this input. A dialog that keeps the caster takes its own
    // reference, and this input releases the handed reference when the dialog opens.
    public void ChooseSpellTargets(SpellCaster caster)
    {
        try
        {
            UI.MapEvents.OpenSpellDialog(caster);
        }
        finally
        {
            _ = caster.Release();
        }
    }

    public void ChooseCombatSpellTarget(CombatSpellCaster caster)
    {
        try
        {
            UI.CombatMapEvents.CombatMapCastSpell(caster);
        }
        finally
        {
            _ = caster.Release();
        }
    }
}
