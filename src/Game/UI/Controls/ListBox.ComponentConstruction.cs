// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.UI.Controls;

public partial class ListBox
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
