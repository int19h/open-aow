namespace Aow.Game.UI.Grids;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class PlayerEventArgs(sbyte player) : EventArgs
{
    public sbyte Player { get; } = player;
}
