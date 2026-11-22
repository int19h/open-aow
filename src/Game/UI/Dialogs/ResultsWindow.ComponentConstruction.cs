// Layout translated from recovered Delphi forms.
#nullable disable warnings

namespace Aow.Game.UI.Dialogs;

public sealed partial class ResultsWindow
{
    private bool _hasLayout;

    public override void Dispose()
    {
        Destroying();
        try
        {
            if (_hasLayout)
            {
                ResultsWinDestroy();
            }
        }
        finally
        {
            base.Dispose();
        }
    }
}
