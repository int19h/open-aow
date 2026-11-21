using Aow.Game.Units;
namespace Aow.Game.UI.Grids;

/// <summary>The data of an event (issue 326, the standard event pattern).</summary>
public sealed class MapObjectEventArgs(Army army) : EventArgs
{
    public Army Army { get; } = army;
}
