// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class GuardAreaAIGroupControl : AIGroupControl
{
    public GuardAreaAIGroupControl(CustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:189
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
            case 2:
                if (MoveExecutor.Status == AIMoveExecutorStatus.Failed)
                {
                    Group.OnControlMoveFailed();
                    Done();
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:176
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        // { Original also accepts crDefeat when the subsequent efficiency tests pass. }
        CombatResult execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 0)
            {
                int efficiency = CombatPredictor.GetEfficiency();
                AITargetData data = target.Data;
                data.Priority = efficiency + ((data.Priority * 100) - (target.MovePoints * 5));
                target.Data = data;
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    // IndependentAG.pas:205
    public override void ProcessStates() => Group.ProcessControlStates(this);

    // IndependentAG.pas:280
    public override void ProcessStatesLooped() => ProcessStates();
}
