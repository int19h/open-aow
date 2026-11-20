#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class HuntAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{
    public new HuntAIGroup Group => GetGroup<HuntAIGroup>();

    // AIGroup.pas:1479
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location);
        if (field.GetPlayer() != Group.Player)
        {
            ArmyHexSprite army = field.GetArmyHexSprite();
            if (army is not null)
            {
                HuntAIGroup group = Group;
                bool found = false;
                int i = group.TargetUnitIds.Count;
                while (!found && i != 0)
                {
                    i -= 1;
                    int item = group.TargetUnitIds[i];
                    found = army.Army.Units.IndexOfId(item) != -1;
                }

                if (found)
                {
                    SetupCP(target);
                    CombatResult execute = CombatPredictor.Execute();
                    if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
                    {
                        if (CombatPredictor.GetEfficiency() != 0)
                        {
                            AITargetData data = target.Data;
                            data.Priority = 1000 - target.MovePoints;
                            target.Data = data;
                            base.TargetFoundEvent(sender, target, ref searchDone);
                        }
                    }
                }
            }
        }
    }

    // AIGroup.pas:1515
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                HuntAIGroup group = Group;
                {
                    for (int i = group.TargetUnitIds.Count - 1; i >= 0; i--)
                    {
                        int item = group.TargetUnitIds[i];
                        Troop troop = StrategicMap.Current.UnitControl.FindUnit(item);
                        if (troop is null || troop.Player == 0)
                        {
                            group.TargetUnitIds.Delete(i);
                        }
                    }
                }

                if (group.TargetUnitIds.Count != 0)
                {
                    AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
                    _ = finder.Setup(this);
                    AITargetInfo targetInfo = finder.Selector.TargetInfo;
                    targetInfo.Mode = AITargetMode.Hunt;
                    finder.Selector.TargetInfo = targetInfo;
                    ActivatePlugin(finder);
                    _ = finder.Release();
                    State += 1;
                }
                else
                {
                    AIGroup newGroup = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Patrol);
                    _ = newGroup.AddUnits(Group.Units);
                    _ = newGroup.Release();
                    Done();
                }

                break;
            case 1:
                if (Targets.Count != 0)
                {
                    SetupME();
                    AIGroupControlTarget targets = Targets.GetTargets(0);
                    AIGroupControlTarget fTargetsTargets = Targets.GetTargets(0);
                    ExecuteME(Targets.GetTargets(0).Location.X, fTargetsTargets.Location.Y, targets.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    Done();
                }

                break;
            default:
                break;
        }
    }
}
