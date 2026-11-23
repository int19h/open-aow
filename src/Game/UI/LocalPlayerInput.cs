using Aow.Game.Combat;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Rtl;

namespace Aow.Game.UI;

// The input of the player at this machine. It reads the dialog modules at call time, so the
// host installs it on the engine before the dialogs load.
public sealed class LocalPlayerInput(Func<UIContext?> ui) : IPlayerInput
{
    private UIContext UI => ui() ?? throw new InvalidOperationException("No UI context is loaded for a spell choice.");

    private static void ShowMessageLogged(string text)
    {
        HostLog.CatchDialogAtCallSite(RtlHost.Logger, text: text);
        LegacyApplication.ShowMessage(text);
    }

    // The call hands the caster to this input. A dialog that keeps the caster takes its own
    // reference, and this input releases the handed reference when the dialog opens.
    public void ChooseSpellTargets(SpellCaster caster)
    {
        try
        {
            UIContext context = UI;
            try
            {
                context.MapEvents.OpenSpellDialog(caster);
            }
            catch
            {
                ShowMessageLogged("Exception occurred during OnCastSpell event (TAoWMapControl)");
            }
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
            UIContext context = UI;
            try
            {
                context.CombatMapEvents.CombatMapCastSpell(caster);
            }
            catch
            {
                ShowMessageLogged("Exception occurred during OnCastSpell event (TAoWCombatMapControl)");
            }
        }
        finally
        {
            _ = caster.Release();
        }
    }
}
