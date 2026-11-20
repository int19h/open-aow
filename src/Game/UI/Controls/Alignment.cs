// Translated from AoWComp.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// how a component sizes and places itself inside its owner (ReAlign). _widthPercent,
// _heightPercent, _leftOffsetPercent, _rightOffsetPercent, _topOffsetPercent, _bottomOffsetPercent,
// _leftOffset, _rightOffset, _topOffset, _bottomOffset, _alignWidth, _alignHeight. It does not
// override Assign, so TAoWComponent.SetAlign raises FormatException (original bug)
public sealed class Alignment : Persistent
{
    [Published]
    public AlignWidth AlignWidth { get; set; }

    [Published]
    public AlignHeight AlignHeight { get; set; }

    [Published]
    public byte HeightPercent { get; set; }

    [Published]
    public byte WidthPercent { get; set; }

    [Published]
    public int LeftOffsetPercent { get; set; }

    [Published]
    public int RightOffsetPercent { get; set; }

    [Published]
    public int TopOffsetPercent { get; set; }

    [Published]
    public int BottomOffsetPercent { get; set; }

    [Published]
    public int LeftOffset { get; set; }

    [Published]
    public int RightOffset { get; set; }

    [Published]
    public int TopOffset { get; set; }

    [Published]
    public int BottomOffset { get; set; }
}
