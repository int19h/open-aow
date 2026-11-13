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
    public HuntAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:1479
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField field;
        ArmyHexSprite army;
        int I;
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
                I = group.TargetUnitIds.Count;
                while (!found && I != 0)
                {
                    I -= 1;
                    var item = group.TargetUnitIds[I];
                    found = army.Army.IndexOfId(item) != -1;
                }

                if (found)
                {
                    SetupCP(target);
                    var execute = CombatPredictor.Execute();
                    if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
                    {
                        if (CombatPredictor.GetEfficiency() != 0)
                        {
                            target.Data.Priority = 1000 - target.MovePoints;
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
                    for (int I = group.TargetUnitIds.Count - 1; I >= 0; I--)
                    {
                        var item = group.TargetUnitIds[I];
                        troop = StrategicMap.Current.UnitControl.FindUnit(item);
                        if (troop == null || troop.Player == 0)
                        {
                            group.TargetUnitIds.Delete(I);
                        }
                    }
                }

                if (group.TargetUnitIds.Count != 0)
                {
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    finder.Setup(this);
                    finder.Selector.TargetInfo.Mode = AITargetMode.Hunt;
                    ActivatePlugin(finder);
                    finder.Release();
                    State += 1;
                }
                else
                {
                    newGroup = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Patrol);
                    newGroup.AddUnits(Group);
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
