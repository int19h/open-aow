// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Ui.Windows;

public partial class AOWMenu
{
    public LayoutStrings LayoutItems
    {
        set
        {
            value.CopyTo(Items);
        }
    }
}
