// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class HuntAIGroupControl : AIGroupControl
{
    public HuntAIGroupControl(CustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1479
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField field;
        ArmyHexSprite army;
        int i;
        HuntAIGroup group;
        bool found;
        field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location);
        if (field.GetPlayer() != Group.Player)
        {
            army = field.GetArmyHexSprite();
            if (army != null)
            {
                group = (HuntAIGroup)Group;
                found = false;
                i = group.TargetUnitIds.Count;
                while (!found && i != 0)
                {
                    i -= 1;
                    var item = group.TargetUnitIds[i];
                    found = army.Army.Units.IndexOfId(item) != -1;
                }

                if (found)
                {
                    SetupCP(target);
                    var execute = CombatPredictor.Execute();
                    if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
                    {
                        if (CombatPredictor.GetEfficiency() != 0)
                        {
                            var data = target.Data;
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
        AIGroupTargetFinder finder;
        HuntAIGroup group;
        Troop troop;
        AIGroup newGroup;
        switch (State)
        {
            case 0:
                group = (HuntAIGroup)Group;
                {
                    for (int i = group.TargetUnitIds.Count - 1; i >= 0; i--)
                    {
                        var item = group.TargetUnitIds[i];
                        troop = StrategicMap.Current.UnitControl.FindUnit(item);
                        if (troop == null || troop.Player == 0)
                        {
                            group.TargetUnitIds.Delete(i);
                        }
                    }
                }

                if (group.TargetUnitIds.Count != 0)
                {
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    finder.Setup(this);
                    var targetInfo = finder.Selector.TargetInfo;
                    targetInfo.Mode = AITargetMode.Hunt;
                    finder.Selector.TargetInfo = targetInfo;
                    ActivatePlugin(finder);
                    finder.Release();
                    State += 1;
                }
                else
                {
                    newGroup = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Patrol);
                    newGroup.AddUnits(Group.Units);
                    newGroup.Release();
                    Done();
                }

                break;
            case 1:
                if (Targets.Count != 0)
                {
                    SetupME();
                    var targets = Targets.GetTargets(0);
                    var fTargetsTargets = Targets.GetTargets(0);
                    ExecuteME(Targets.GetTargets(0).Location.X, fTargetsTargets.Location.Y, targets.Location.MapLevel);
                    Targets.Delete(0);
                }
                else
                {
                    Done();
                }

                break;
        }
    }
}
