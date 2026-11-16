// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Draw;

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
        field = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location);
        if (field.GetPlayer() != Group.Player)
        {
            army = field.GetArmyHexSprite();
            if (army != null && Group.IsHuntingArmy(army.Army))
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

    // AIGroup.pas:1515
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIGroup newGroup;
        switch (State)
        {
            case 0:
                if (Group.PruneAndHasHuntTargets())
                {
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    finder.Setup(this);
                    AITargetInfo targetInfo = finder.Selector.TargetInfo;
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
        }
    }
}
