// Layout translated from recovered Delphi forms.
#nullable disable warnings

namespace Aow.Game.UI.Dialogs;

public sealed partial class LeaderSetupWindow
{
    private bool _hasLayout;

    public override void Dispose()
    {
        Destroying();
        try
        {
            if (_hasLayout)
            {
                LeaderSetupWinDestroy();
            }
        }
        finally
        {
            base.Dispose();
        }
    }
}
