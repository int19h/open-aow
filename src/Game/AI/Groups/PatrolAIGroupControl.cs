// Translated from IndependentAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class PatrolAIGroupControl : AIGroupControl
{
    // Heading 0 is the centre, 1 to 6 the hex sides (fixed-arrays.md, named constant).
    public const int HexDirectionSlotCount = 7;

    public PatrolAIGroupControl(CustomNode owner) : base(owner)
    { }

    // IndependentAG.pas:415
    public override void MoveExecutorDone(object sender)
    {
        switch (MoveExecutor.Status)
        {
            case AIMoveExecutorStatus.Done:
                Done();
                break;
            case AIMoveExecutorStatus.Failed:
                if (State == 7)
                {
                    Group.OnControlMoveFailed();
                }
                else
                {
                    Done();
                }

                break;
        }
    }

    // IndependentAG.pas:426
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        MapField mapField;
        mapField = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location);
        if (mapField.GetPlayer() != Group.Player)
        {
            SetupCP(target);
            CombatResult execute = CombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
            {
                if (CombatPredictor.GetSuperiority() > 80)
                {
                    if (CombatPredictor.GetEfficiency() > 50)
                    {
                        int efficiency = CombatPredictor.GetEfficiency();
                        AITargetData data = target.Data;
                        data.Priority = efficiency + (data.Priority * 100);
                        target.Data = data;
                        base.TargetFoundEvent(sender, target, ref searchDone);
                    }
                }
            }
        }
    }

    // IndependentAG.pas:444
    public override void ProcessStates() => Group.ProcessControlStates(this);
}
