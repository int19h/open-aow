// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

// No new fields
public class BudgetManagerAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    // AI.pas:2210
    public override bool Process() => false;

    // AI.pas:2207
    public static new int Priority => 25;

}
