using Aow.Game.Draw;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class SurfaceDrawingEventArgs : EventArgs
{
    public required Surface? Surface { get; init; }

    public bool Handled { get; set; }
}
