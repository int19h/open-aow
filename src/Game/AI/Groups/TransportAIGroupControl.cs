#nullable disable
// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class TransportAIGroupControl(CustomNode owner) : AIGroupControl(owner)
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
        }
    }

    // MainAG.pas:810
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        SetupCP(target);
        CombatResult execute = CombatPredictor.Execute();
        if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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
        AIGroupTargetFinder finder;
        AIGroupWanderControl wander;
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
            for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
            {
                bool condition = transporter != army.Units.GetUnit(i);
                if (condition)
                {
                    Troop troop = army.Units.GetUnit(i);
                    condition = MoveExecutor.Units.IndexOfUnit(troop) == -1;
                }

                if (condition)
                {
                    Troop troop = army.Units.GetUnit(i);
                    _ = MoveExecutor.AddUnit(troop);
                }
            }
        }
    }
}
