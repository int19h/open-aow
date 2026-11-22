using Aow.Rtl;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class MouseEventArgs : EventArgs
{
    public required MouseButton Button { get; init; }

    public required ShiftStates Shift { get; init; }

    public required int X { get; init; }

    public required int Y { get; init; }
}
