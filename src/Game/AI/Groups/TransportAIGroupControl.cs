// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class TransportAIGroupControl : AIGroupControl
{
    public TransportAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // MainAG.pas:822
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

    // MainAG.pas:810
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        var execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
        {
            if (CombatPredictor.GetEfficiency() > 75)
            {
                var efficiency = CombatPredictor.GetEfficiency();
                target.Data.Priority = efficiency + (target.Data.Priority * 100 - target.MovePoints * 5);
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    // MainAG.pas:849
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIGroupWanderControl wander;
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
                    State += 1;
                }

                break;
            case 2:
                wander = new AIGroupWanderControl(null) { Holder = CreationHolder() };
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

    // MainAG.pas:831
    public override void SetupME()
    {
        Troop transporter;
        Army army;
        base.SetupME();
        transporter = Group.Leader;
        if (transporter != null && transporter.Owner is Army)
        {
            army = transporter.ArmyOwner;
            for (int I = 0, last = army.Count - 1; I <= last; I++)
            {
                bool condition = transporter != army.GetUnit(I);
                if (condition)
                {
                    var troop = army.GetUnit(I);
                    condition = MoveExecutor.Units.IndexOfUnit(troop) == -1;
                }

                if (condition)
                {
                    var troop = army.GetUnit(I);
                    MoveExecutor.AddUnit(troop);
                }
            }
        }
    }
}
