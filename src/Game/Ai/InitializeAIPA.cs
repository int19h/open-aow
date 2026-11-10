// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Ai;

public partial class InitializeAIPA : AIPlayerAction, IAIPlayerActionClass
{
    public InitializeAIPA(ECustomNode owner) : base(owner)
    { }

    // AI.pas:1017
    public override bool Process()
    {
        FControl.FPlayer.FAIBudget.UpdateBudgets();
        if (FControl.FPlayer.FPlayerIndex != 0)
        {
            FControl.UpdateSkirmishGroups();
        }

        return false;
    }

    // AI.pas:1014
    public static new int Priority => 1000;

}
