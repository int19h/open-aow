// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class SuicidalAIGroupControl : AIGroupControl
{
    public SuicidalAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // MainAG.pas:750
    public override void MoveExecuterDone(object Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == AIMoveExecutor.Status.OutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == AIMoveExecutor.Status.Invalidated)
                    {
                        Done();
                    }
                }

                break;
        }
    }

    // MainAG.pas:738
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        var execute = FCombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // MainAG.pas:759
    public override void ProcessStates()
    {
        AIGroupTargetFinder Finder;
        switch (FState)
        {
            case 0:
                Finder = new AIGroupTargetFinder(null);
                Finder.Setup(this);
                Finder.FSelector.FMovePointsMax = 100;
                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    var target = FTargets.GetTargets(0);
                    ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
                    FTargets.Delete(0);
                }
                else
                {
                    Done();
                }

                break;
        }
    }
}
