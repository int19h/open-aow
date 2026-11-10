// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class RaidAGC : AIGroupControl
{
    public RaidAGC(ECustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:589
    public override void MoveExecuterDone(object Sender)
    {
        switch (FState)
        {
            case 1:
                if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                    {
                        Done();
                    }
                }

                break;
        }
    }

    // IndependentAG.pas:576
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        // { Original also accepts crDefeat when the efficiency test passes. }
        var execute = FCombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency / 10 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    public override void ProcessStates()
    {
        switch (FState)
        {
            case 0:
                var finder = new AIGroupTargetFinder(null!);
                finder.Setup(this);
                finder.FSelector.FMovePointsMax = 100;
                finder.FSelector.FTargetInfo.Mode = AITargetMode.Raid;
                ActivatePlugin(finder);
                finder.Release();
                FState++;
                break;
            case 1:
                FTargets.Validate();
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var target = FTargets.GetTargets(0);
                    ExecuteME(target.FLocation.X, target.FLocation.Y, target.FLocation.L);
                    FTargets.Delete(0);
                    FState++;
                }
                else Done();
                break;
            case 2:
                var raze = new AIGroupRazeControl(null!);
                raze.Setup(this);
                // loads the set byte 04 from in the original
                raze.FMode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.Always);
                ActivatePlugin(raze);
                raze.Release();
                FState = 1;
                break;
        }
    }
}
