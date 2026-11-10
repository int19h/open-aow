// Translated from AoWComp.pas.
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

// one entry of the dirty rectangle lists. R, Owner (the component that asked for the redraw.
// AddDrawRect only)
public partial class TaowRect
{
    public Rect R;
    public AoWComponent Owner;
}
