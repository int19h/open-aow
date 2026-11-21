// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;

namespace Aow.Game.UI.Controls;

public sealed partial class ComboBox
{
    public LayoutStrings LayoutItems
    {
        set => value.CopyTo(Items);
    }
}
