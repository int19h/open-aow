using Aow.Rtl;

namespace Aow.Game.UI.Controls;

/// <summary>The data of a UI event (issue 326, the UI families).</summary>
public sealed class KeyEventArgs : EventArgs
{
    public required ushort Key { get; set; }

    public required ShiftStates Shift { get; init; }
}
