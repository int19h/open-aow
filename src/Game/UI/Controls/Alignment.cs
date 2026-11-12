// Translated from AoWComp.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// how a component sizes and places itself inside its owner (ReAlign). _widthPercent,
// _heightPercent, _leftOffsetPercent, _rightOffsetPercent, _topOffsetPercent, _bottomOffsetPercent,
// _leftOffset, _rightOffset, _topOffset, _bottomOffset, _alignWidth, _alignHeight. It does not
// override Assign, so TAoWComponent.SetAlign raises FormatException (original bug)
public partial class Alignment : Persistent
{
    private byte _widthPercent;
    private byte _heightPercent;
    private int _leftOffsetPercent;
    private int _rightOffsetPercent;
    private int _topOffsetPercent;
    private int _bottomOffsetPercent;
    private int _leftOffset;
    private int _rightOffset;
    private int _topOffset;
    private int _bottomOffset;
    private AlignWidth _alignWidth;
    private AlignHeight _alignHeight;

    [Published]
    public AlignWidth AlignWidth { get => _alignWidth; set => _alignWidth = value; }

    [Published]
    public AlignHeight AlignHeight { get => _alignHeight; set => _alignHeight = value; }

    [Published]
    public byte HeightPercent { get => _heightPercent; set => _heightPercent = value; }

    [Published]
    public byte WidthPercent { get => _widthPercent; set => _widthPercent = value; }

    [Published]
    public int LeftOffsetPercent { get => _leftOffsetPercent; set => _leftOffsetPercent = value; }

    [Published]
    public int RightOffsetPercent { get => _rightOffsetPercent; set => _rightOffsetPercent = value; }

    [Published]
    public int TopOffsetPercent { get => _topOffsetPercent; set => _topOffsetPercent = value; }

    [Published]
    public int BottomOffsetPercent { get => _bottomOffsetPercent; set => _bottomOffsetPercent = value; }

    [Published]
    public int LeftOffset { get => _leftOffset; set => _leftOffset = value; }

    [Published]
    public int RightOffset { get => _rightOffset; set => _rightOffset = value; }

    [Published]
    public int TopOffset { get => _topOffset; set => _topOffset = value; }

    [Published]
    public int BottomOffset { get => _bottomOffset; set => _bottomOffset = value; }

}
