#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

public sealed class ArmyList(CustomNode? owner) : ObservableEventList(owner)
{
    public Player Player { get; set; }

    // Player.PAS:6
    public Army this[int index] => GetChild<Army>(index);
}
