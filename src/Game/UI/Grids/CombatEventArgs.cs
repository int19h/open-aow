namespace Aow.Game.UI.Grids;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class CombatEventArgs(object value) : EventArgs
{
    public object Value { get; } = value;
}
