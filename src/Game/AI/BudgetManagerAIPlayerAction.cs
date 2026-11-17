// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

// No new fields
public partial class BudgetManagerAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    // AI.pas:2210
    public override bool Process() => false;

    // intf_P5_06.inc:627

    // AI.pas:2207
    public static new int Priority => 25;

}
