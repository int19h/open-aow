#nullable disable warnings
// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public class ScoutAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{

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
            default:
                break;
        }
    }

    // IndependentAG.pas:659
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        CombatResult execute = CombatPredictor.Execute();
        if (execute is CombatResult.Victory or CombatResult.NoDefenders)
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
        switch (State)
        {
            case 0:
                bool condition = Group.Player != PlayerId.Independent;
                if (!condition)
                {
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    StrategicMapIndependentAIMode independentAIMode = strategicMap.IndependentAIMode;
                    condition = strategicMap.Random(3) < (int)independentAIMode;
                }

                if (condition)
                {
                    AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
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
                    ExecuteME(Targets.GetTargets(0).Location.X, target.Location.Y, L: targetLocation.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    State += 1;
                }

                break;
            case 2:
                AIGroupWanderControl wander = new(null) { Holder = CreationHolder() };
                _ = wander.Setup(this);
                wander.MovePointsMax = 125;
                ActivatePlugin(wander);
                _ = wander.Release();
                State += 1;
                break;
            case 3:
                Done();
                break;
            default:
                break;
        }
    }
}
