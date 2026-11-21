#nullable disable warnings
// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

namespace Aow.Game.AI.Groups;

public class TransportAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{

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
            default:
                break;
        }
    }

    // MainAG.pas:810
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        CombatResult execute = CombatPredictor.Execute();
        if (execute is CombatResult.Victory or CombatResult.Defeat or CombatResult.NoDefenders)
        {
            if (CombatPredictor.GetEfficiency() > 75)
            {
                int efficiency = CombatPredictor.GetEfficiency();
                AITargetData data = target.Data;
                data.Priority = efficiency + ((data.Priority * 100) - (target.MovePoints * 5));
                target.Data = data;
                base.TargetFoundEvent(sender, target, ref searchDone);
            }
        }
    }

    // MainAG.pas:849
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
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

    // MainAG.pas:831
    public override void SetupME()
    {
        base.SetupME();
        Troop? transporter = Group.Leader;
        if (transporter is not null && transporter.Owner is Army)
        {
            Army? army = transporter.ArmyOwner;
            for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
            {
                bool condition = transporter != army.Units.GetUnit(i);
                if (condition)
                {
                    Troop? troop = army.Units.GetUnit(i);
                    condition = MoveExecutor.Units.IndexOfUnit(troop) == -1;
                }

                if (condition)
                {
                    Troop? troop = army.Units.GetUnit(i);
                    _ = MoveExecutor.AddUnit(troop);
                }
            }
        }
    }
}
