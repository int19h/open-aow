using Aow.Game.Draw;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class SurfaceEventArgs : EventArgs
{
    public required LockedSurface Surface { get; init; }
}
