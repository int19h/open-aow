// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Ui.Controls;

public partial class AOWMenuBar
{
    public LayoutStrings LayoutItems
    {
        set
        {
            value.CopyTo(Items);
        }
    }

    public LayoutStrings LayoutToolTipText
    {
        set
        {
            value.CopyTo(ToolTipText);
        }
    }
}
