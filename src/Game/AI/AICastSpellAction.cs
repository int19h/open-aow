// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI;

public partial class AICastSpellAction : EngineObject
{
    public AICastSpellAction(ECustomNode owner) : base(owner)
    { }

    public int FSourceUnitID;
    public int FSpellID;
    public int FPriority;
}
