// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;

namespace Aow.Game.UI.Controls;

public sealed partial class Gauge
{
    public LayoutStrings LayoutToolTipText
    {
        set => value.CopyTo(ToolTipText);
    }
}
