// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Ai;

public partial class StructurePhase1AIPA : StructureAIPA, IAIPlayerActionClass
{
    // Structur.PAS:3361
    // (inherited phase)
    public StructurePhase1AIPA(ECustomNode owner) : base(owner)
    {
        FPhase = 1;
    }

    // intf_P1_12.inc:16

    // Structur.PAS:3367
    public static new int Priority => 21;

}
