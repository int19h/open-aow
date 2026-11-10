// Translated from AoWComp.pas.
using Aow.Rtl;

namespace Aow.Game.Ui.Controls;

// how a component sizes and places itself inside its owner (ReAlign). FWidthPercent,
// FHeightPercent, FLeftOffPercent, FRightOffPercent, FTopOffPercent, FBottomOffPercent,
// FLeftOffset, FRightOffset, FTopOffset, FBottomOffset, FAlignWidth, FAlignHeight. It does not
// override Assign, so TAoWComponent.SetAlign raises FormatException (original bug)
public partial class AOWAlignment : Persistent
{
    internal byte FWidthPercent;
    internal byte FHeightPercent;
    internal int FLeftOffPercent;
    internal int FRightOffPercent;
    internal int FTopOffPercent;
    internal int FBottomOffPercent;
    internal int FLeftOffset;
    internal int FRightOffset;
    internal int FTopOffset;
    internal int FBottomOffset;
    internal AOWAlignWidth FAlignWidth;
    internal AOWAlignHeight FAlignHeight;

    [Published]
    public AOWAlignWidth AlignWidth { get => FAlignWidth; set => FAlignWidth = value; }

    [Published]
    public AOWAlignHeight AlignHeight { get => FAlignHeight; set => FAlignHeight = value; }

    [Published]
    public byte HeightPercent { get => FHeightPercent; set => FHeightPercent = value; }

    [Published]
    public byte WidthPercent { get => FWidthPercent; set => FWidthPercent = value; }

    [Published]
    public int LeftOffPercent { get => FLeftOffPercent; set => FLeftOffPercent = value; }

    [Published]
    public int RightOffPercent { get => FRightOffPercent; set => FRightOffPercent = value; }

    [Published]
    public int TopOffPercent { get => FTopOffPercent; set => FTopOffPercent = value; }

    [Published]
    public int BottomOffPercent { get => FBottomOffPercent; set => FBottomOffPercent = value; }

    [Published]
    public int LeftOffset { get => FLeftOffset; set => FLeftOffset = value; }

    [Published]
    public int RightOffset { get => FRightOffset; set => FRightOffset = value; }

    [Published]
    public int TopOffset { get => FTopOffset; set => FTopOffset = value; }

    [Published]
    public int BottomOffset { get => FBottomOffset; set => FBottomOffset = value; }

}
