// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Ui.Controls;

public partial class AOWComboBox
{
    public LayoutStrings LayoutItems
    {
        set
        {
            value.CopyTo(Items);
        }
    }
}
