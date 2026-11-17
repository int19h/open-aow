// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.AI;

public partial class InitializeAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{

    // AI.pas:1017
    public override bool Process()
    {
        Control.Player.AIBudget.UpdateBudgets();
        if (Control.Player.PlayerIndex != 0)
        {
            Control.UpdateSkirmishGroups();
        }

        return false;
    }

    // AI.pas:1014
    public static new int Priority => 1000;

}
