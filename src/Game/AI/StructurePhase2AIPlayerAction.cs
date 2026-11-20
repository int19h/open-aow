// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

public class StructurePhase2AIPlayerAction : StructureAIPlayerAction, IAIPlayerActionClass
{
    // Structur.PAS:3373
    // inherited FPhase
    public StructurePhase2AIPlayerAction(CustomNode owner) : base(owner) => Phase = 2;

    // Structur.PAS:3379
    public static new int Priority => 15;

}
