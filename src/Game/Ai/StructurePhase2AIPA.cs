// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Ai;

public partial class StructurePhase2AIPA : StructureAIPA, IAIPlayerActionClass
{
    // Structur.PAS:3373
    // inherited FPhase
    public StructurePhase2AIPA(ECustomNode owner) : base(owner)
    {
        FPhase = 2;
    }

    // intf_P1_12.inc:23

    // Structur.PAS:3379
    public static new int Priority => 15;

}
