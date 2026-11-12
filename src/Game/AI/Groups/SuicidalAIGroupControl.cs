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
        var execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = CombatPredictor.GetEfficiency();
                target.Data.Priority = efficiency + (target.Data.Priority * 100 - target.MovePoints * 5);
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
                finder.Setup(this);
                finder.Selector.MovePointsMax = 100;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 1:
                Targets.Validate();
                if (Targets.Count != 0)
                {
                    Targets.SortOnPriority();
                    SetupME();
                    var targetLocation = Targets.GetTargets(0);
                    var target = Targets.GetTargets(0);
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
