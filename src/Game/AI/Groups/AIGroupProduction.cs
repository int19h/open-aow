#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

namespace Aow.Game.AI.Groups;

public partial class AIGroupProduction(CustomNode owner) : EngineObject(owner)
{
    public Troop Troop { get; init; }
    public ProductionControl Control { get; init; }
    // Engine.pas:524

}
