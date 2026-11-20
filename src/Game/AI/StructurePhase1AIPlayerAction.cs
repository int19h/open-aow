// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

public class StructurePhase1AIPlayerAction : StructureAIPlayerAction, IAIPlayerActionClass
{
    // Structur.PAS:3361
    // (inherited phase)
    public StructurePhase1AIPlayerAction(CustomNode owner) : base(owner) => Phase = 1;

    // Structur.PAS:3367
    public static new int Priority => 21;

}
