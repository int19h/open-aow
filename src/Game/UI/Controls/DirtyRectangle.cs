// Translated from AoWComp.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// one entry of the dirty rectangle lists. R, Owner (the component that asked for the redraw.
// AddDrawRect only)
public partial class DirtyRectangle
{
    public Rect Rectangle;

    public UIComponent Owner { get; set; }
}
