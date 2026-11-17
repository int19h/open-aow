#nullable disable
// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class ScoutAIGroupControl : AIGroupControl
{
    public ScoutAIGroupControl(CustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:671
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

    // IndependentAG.pas:659
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        CombatResult execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 50)
            {
                int efficiency = CombatPredictor.GetEfficiency();
                AITargetData data = target.Data;
                data.Priority = efficiency + ((data.Priority * 100) - (target.MovePoints * 5));
                target.Data = data;
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    // IndependentAG.pas:680
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIGroupWanderControl wander;
        switch (State)
        {
            case 0:
                bool condition = Group.Player != 0;
                if (!condition)
                {
                    StrategicMapIndependentAIMode independentAIMode = StrategicMap.Current.GetIndependentAIMode();
                    condition = StrategicMap.Current.Random(3) < (int)independentAIMode;
                }

                if (condition)
                {
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    _ = finder.Setup(this);
                    int movePointsMin = Group.GetMovePointsMin();
                    finder.Selector.MovePointsMax = movePointsMin;
                    ActivatePlugin(finder);
                    _ = finder.Release();
                    State += 1;
                }
                else
                {
                    Done();
                }

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
                    State += 1;
                }

                break;
            case 2:
                wander = new AIGroupWanderControl(null) { Holder = CreationHolder() };
                _ = wander.Setup(this);
                wander.MovePointsMax = 125;
                ActivatePlugin(wander);
                _ = wander.Release();
                State += 1;
                break;
            case 3:
                Done();
                break;
        }
    }
}
