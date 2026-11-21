using Aow.Game.Magic.Abilities;
namespace Aow.Game.UI.Grids;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class UnitDestroyedEventArgs(Troop troop) : EventArgs
{
    public Troop Troop { get; } = troop;
}
