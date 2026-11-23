using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class DrawItemEventArgs : EventArgs
{
    public required Rect Rectangle { get; init; }

    public required int Item { get; init; }

    public required LockedSurface Surface { get; set; }
}
