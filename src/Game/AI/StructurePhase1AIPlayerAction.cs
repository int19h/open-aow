// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

public partial class StructurePhase1AIPlayerAction : StructureAIPlayerAction, IAIPlayerActionClass
{
    // Structur.PAS:3361
    // (inherited phase)
    public StructurePhase1AIPlayerAction(ECustomNode owner) : base(owner)
    {
        Phase = 1;
    }

    // intf_P1_12.inc:16

    // Structur.PAS:3367
    public static new int Priority => 21;

}
