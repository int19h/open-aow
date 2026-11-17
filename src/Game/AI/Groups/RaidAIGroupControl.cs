// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class RaidAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{

    // IndependentAG.pas:589
    public override void MoveExecutorDone(object sender)
    {
        switch (State)
        {
            case 1:
                if (MoveExecutor.Status == AIMoveExecutorStatus.OutOfMovePoints)
                {
                    Done();
                }
                else
                {
                    if (MoveExecutor.Status == AIMoveExecutorStatus.Invalidated)
                    {
                        Done();
                    }
                }

                break;
            default:
                break;
        }
    }

    // IndependentAG.pas:576
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        // { Original also accepts crDefeat when the efficiency test passes. }
        CombatResult execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 0)
            {
                int efficiency = CombatPredictor.GetEfficiency();
                AITargetData data = target.Data;
                data.Priority = (efficiency / 10) + ((data.Priority * 100) - (target.MovePoints * 5));
                target.Data = data;
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                AIGroupTargetFinder finder = new(null!) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                finder.Selector.MovePointsMax = 100;
                AITargetInfo targetInfo = finder.Selector.TargetInfo;
                targetInfo.Mode = AITargetMode.Raid;
                finder.Selector.TargetInfo = targetInfo;
                ActivatePlugin(finder);
                _ = finder.Release();
                State++;
                break;
            case 1:
                _ = Targets.Validate();
                if (Targets.Count != 0)
                {
                    Targets.SortOnPriority();
                    SetupME();
                    AIGroupControlTarget target = Targets.GetTargets(0);
                    ExecuteME(target.Location.X, target.Location.Y, target.Location.MapLevel);
                    Targets.Delete(0);
                    State++;
                }
                else
                {
                    Done();
                }

                break;
            case 2:
                AIGroupRazeControl raze = new(null!) { Holder = CreationHolder() };
                _ = raze.Setup(this);
                // loads the set byte 04 from in the original
                raze.Mode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.Always);
                ActivatePlugin(raze);
                _ = raze.Release();
                State = 1;
                break;
            default:
                break;
        }
    }
}
