// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;

namespace Aow.Game.UI.Grids;

public sealed partial class TurnButton
{
    public LayoutStrings LayoutToolTipText
    {
        set => value.CopyTo(ToolTipText);
    }
}
