#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

public class ArmyList(CustomNode owner) : ObservableEventList(owner)
{
    public Player Player { get; set; }

    // Player.PAS:6
    public Army GetArmy(int index) => GetChild<Army>(index);
}
