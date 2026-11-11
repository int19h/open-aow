// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

// No new fields
public partial class BudgetManagerAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // AI.pas:2201
    public BudgetManagerAIPlayerAction(ECustomNode owner) : base(owner)
    {
    }

    // AI.pas:2210
    public override bool Process() => false;

    // intf_P5_06.inc:627

    // AI.pas:2207
    public static new int Priority => 25;

}
