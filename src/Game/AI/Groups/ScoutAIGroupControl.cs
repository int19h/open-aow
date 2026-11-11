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

    // IndependentAG.pas:659
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        var execute = FCombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.NoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 50)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // IndependentAG.pas:680
    public override void ProcessStates()
    {
        AIGroupTargetFinder Finder;
        AIGroupWanderControl Wander;
        switch (FState)
        {
            case 0:
                bool condition = FGroup.FPlayer != 0;
                if (!condition)
                {
                    var independentAIMode = AoWHSMap.Current.GetIndependentAIMode();
                    condition = AoWHSMap.Current.Random(3) < (int)independentAIMode;
                }

                if (condition)
                {
                    Finder = new AIGroupTargetFinder(null);
                    Finder.Setup(this);
                    var movePointsMin = FGroup.GetMovePointsMin();
                    Finder.FSelector.FMovePointsMax = movePointsMin;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                }
                else
                {
                    Done();
                }

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
                    FState += 1;
                }

                break;
            case 2:
                Wander = new AIGroupWanderControl(null);
                Wander.Setup(this);
                Wander.FMovePointsMax = 125;
                ActivatePlugin(Wander);
                Wander.Release();
                FState += 1;
                break;
            case 3:
                Done();
                break;
        }
    }
}
