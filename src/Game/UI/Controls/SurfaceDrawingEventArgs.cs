using Aow.Game.Draw;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class SurfaceDrawingEventArgs : EventArgs
{
    public required Surface? Surface { get; init; }

    /// <summary>True once a handler drew the surface itself. A handler only ever sets it to true, so a true value stays for the handlers after it and the window skips its own drawing.</summary>
    public bool Handled { get; set; }
}
