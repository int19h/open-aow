// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class RaidAIGroupControl : AIGroupControl
{
    public RaidAIGroupControl(CustomNode owner) : base(owner)
    { }

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
        }
    }

    // IndependentAG.pas:576
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        // { Original also accepts crDefeat when the efficiency test passes. }
        var execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = CombatPredictor.GetEfficiency();
                var data = target.Data;
                data.Priority = efficiency / 10 + (data.Priority * 100 - target.MovePoints * 5);
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
                var finder = new AIGroupTargetFinder(null!) { Holder = CreationHolder() };
                finder.Setup(this);
                finder.Selector.MovePointsMax = 100;
                var targetInfo = finder.Selector.TargetInfo;
                targetInfo.Mode = AITargetMode.Raid;
                finder.Selector.TargetInfo = targetInfo;
                ActivatePlugin(finder);
                finder.Release();
                State++;
                break;
            case 1:
                Targets.Validate();
                if (Targets.Count != 0)
                {
                    Targets.SortOnPriority();
                    SetupME();
                    var target = Targets.GetTargets(0);
                    ExecuteME(target.Location.X, target.Location.Y, target.Location.MapLevel);
                    Targets.Delete(0);
                    State++;
                }
                else Done();
                break;
            case 2:
                var raze = new AIGroupRazeControl(null!) { Holder = CreationHolder() };
                raze.Setup(this);
                // loads the set byte 04 from in the original
                raze.Mode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.Always);
                ActivatePlugin(raze);
                raze.Release();
                State = 1;
                break;
        }
    }
}
