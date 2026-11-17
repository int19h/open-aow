#nullable disable
// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class SuicidalAIGroupControl : AIGroupControl
{
    public SuicidalAIGroupControl(CustomNode owner) : base(owner)
    { }

    // MainAG.pas:750
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

    // MainAG.pas:738
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
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

    // MainAG.pas:759
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        switch (State)
        {
            case 0:
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                finder.Selector.MovePointsMax = 100;
                ActivatePlugin(finder);
                _ = finder.Release();
                State += 1;
                break;
            case 1:
                _ = Targets.Validate();
                if (Targets.Count != 0)
                {
                    Targets.SortOnPriority();
                    SetupME();
                    AIGroupControlTarget targetLocation = Targets.GetTargets(0);
                    AIGroupControlTarget target = Targets.GetTargets(0);
                    ExecuteME(Targets.GetTargets(0).Location.X, target.Location.Y, targetLocation.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    Done();
                }

                break;
        }
    }
}
