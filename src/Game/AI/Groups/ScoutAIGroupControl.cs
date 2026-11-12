// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class ScoutAIGroupControl : AIGroupControl
{
    public ScoutAIGroupControl(ECustomNode owner) : base(owner)
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
        var execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 50)
            {
                var efficiency = CombatPredictor.GetEfficiency();
                target.Data.Priority = efficiency + (target.Data.Priority * 100 - target.MovePoints * 5);
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
                    var independentAIMode = StrategicMap.Current.GetIndependentAIMode();
                    condition = StrategicMap.Current.Random(3) < (int)independentAIMode;
                }

                if (condition)
                {
                    finder = new AIGroupTargetFinder(null);
                    finder.Setup(this);
                    var movePointsMin = Group.GetMovePointsMin();
                    finder.Selector.MovePointsMax = movePointsMin;
                    ActivatePlugin(finder);
                    finder.Release();
                    State += 1;
                }
                else
                {
                    Done();
                }

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
                    State += 1;
                }

                break;
            case 2:
                wander = new AIGroupWanderControl(null);
                wander.Setup(this);
                wander.MovePointsMax = 125;
                ActivatePlugin(wander);
                wander.Release();
                State += 1;
                break;
            case 3:
                Done();
                break;
        }
    }
}
