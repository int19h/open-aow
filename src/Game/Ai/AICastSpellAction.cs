// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai;

public partial class AICastSpellAction : EObject
{
    public AICastSpellAction(ECustomNode owner) : base(owner)
    { }

    public int FSourceUnitID;
    public int FSpellID;
    public int FPriority;
}
