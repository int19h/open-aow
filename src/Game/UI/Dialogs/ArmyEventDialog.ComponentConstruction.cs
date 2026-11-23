// Layout translated from recovered Delphi forms.
#nullable disable warnings

namespace Aow.Game.UI.Dialogs;

public sealed partial class ArmyEventDialog
{
    private bool _hasLayout;

    public override void Dispose()
    {
        Destroying();
        try
        {
            if (_hasLayout)
            {
                ArmyEventDialogDestroy();
            }
        }
        finally
        {
            base.Dispose();
        }
    }
}
