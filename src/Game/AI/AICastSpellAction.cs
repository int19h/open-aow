// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AICastSpellAction(CustomNode owner) : EngineObject(owner)
{
    public int SourceUnitId { get; set; }
    public int SpellId { get; set; }
    public int Priority { get; set; }
}
