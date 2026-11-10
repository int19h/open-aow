// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;

namespace Aow.Game.Ui.Controls;

public partial class AOWCheckBox
{
    public LayoutStrings LayoutToolTipText
    {
        set
        {
            value.CopyTo(ToolTipText);
        }
    }
}
