#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.AI.Groups;

public class HuntAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{
    public new HuntAIGroup Group => GetGroup<HuntAIGroup>();

    // AIGroup.pas:1479
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField field = GetEngine().StrategicMap.MapContainer.GetFieldMapCoordinate(target.Location);
        if (field.GetPlayer() != Group.Player)
        {
            ArmyHexSprite army = field.GetArmyHexSprite();
            if (army is not null)
            {
                HuntAIGroup group = Group;
                bool found = false;
                found = group.TargetUnitIds.Any(item => army.Army.Units.IndexOfId(item) != -1);
                if (found)
                {
                    SetupCP(target);
                    CombatResult execute = CombatPredictor.Execute();
                    if (execute is CombatResult.Victory or CombatResult.Defeat or CombatResult.NoDefenders)
                    {
                        if (CombatPredictor.GetEfficiency() != 0)
                        {
                            AITargetData data = target.Data;
                            data.Priority = 1_000 - target.MovePoints;
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
                        Troop? troop = GetEngine().StrategicMap.UnitControl.FindUnit(item);
                        if (troop is null || troop.Player == PlayerId.Independent)
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
                    AIGroup? newGroup = GetEngine().StrategicMap.AIGroupManager.CreateGroup(Group.Player, relation: Group.IndependentRelation, behavior: GroupBehavior.Patrol);
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
                    ExecuteME(Targets.GetTargets(0).Location.X, fTargetsTargets.Location.Y, L: targets.Location.MapLevel);
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
