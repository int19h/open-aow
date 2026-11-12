// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

public partial class StructurePhase2AIPlayerAction : StructureAIPlayerAction, IAIPlayerActionClass
{
    // Structur.PAS:3373
    // inherited FPhase
    public StructurePhase2AIPlayerAction(ECustomNode owner) : base(owner)
    {
        Phase = 2;
    }

    // intf_P1_12.inc:23

    // Structur.PAS:3379
    public static new int Priority => 15;

}
