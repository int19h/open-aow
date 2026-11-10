// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class TransportAGC : AIGroupControl
{
    public TransportAGC(ECustomNode owner) : base(owner)
    { }

    // MainAG.pas:822
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

    // MainAG.pas:810
    public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
    {
        SetupCP(Target);
        var execute = FCombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 75)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // MainAG.pas:849
    public override void ProcessStates()
    {
        AIGroupTargetFinder Finder;
        AIGroupWanderControl Wander;
        switch (FState)
        {
            case 0:
                Finder = new AIGroupTargetFinder(null);
                Finder.Setup(this);
                Finder.FSelector.FMovePointsMax = 100;
                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
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

    // MainAG.pas:831
    public override void SetupME()
    {
        AbstractUnit Transporter;
        Army Army;
        base.SetupME();
        Transporter = FGroup.FLeader;
        if (Transporter != null && Transporter.Owner is Army)
        {
            Army = Transporter.ArmyOwner;
            for (int I = 0, last = Army.Count - 1; I <= last; I++)
            {
                bool condition = Transporter != Army.GetUnit(I);
                if (condition)
                {
                    var unit = Army.GetUnit(I);
                    condition = FMoveExecuter.FUnits.IndexOfUnit(unit) == -1;
                }

                if (condition)
                {
                    var unit = Army.GetUnit(I);
                    FMoveExecuter.AddUnit(unit);
                }
            }
        }
    }
}
