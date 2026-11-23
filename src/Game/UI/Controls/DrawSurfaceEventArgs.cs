using Aow.Game.Draw;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class DrawSurfaceEventArgs : EventArgs
{
    public required Surface? Surface { get; init; }

    public required int Left { get; init; }

    public required int Top { get; init; }
}
