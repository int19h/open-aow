// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat.Actions;

public partial class NewCombatRoundCombatAction : CombatAction
{
    // Combat2.pas:1009
    public new const int ClassId = 131771;

    public NewCombatRoundCombatAction(CustomNode owner) : base(owner)
    { }

    // Combat2.pas:1012
    public override void Execute(Battle combat)
    {
        CombatData combatData = combat.CombatData;
        combatData.Round += 1;

        for (int i = combat.CombatData.GetCount() - 1; i >= 0; i--)
        {
            combat.CombatData.GetObjects(i).NewRound();
        }
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { if (viewer.Time == 0) { Execute(viewer.Combat); } return viewer.Delay > viewer.Time; }
}
