// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat.Actions;

public partial class NewCombatRoundCombatAction : CombatAction
{
    // Combat2.pas:1009
    public new const int ClassId = 131771;

    public NewCombatRoundCombatAction(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:1012
    public override void Execute(Battle combat)
    {
        var combatData = combat.CombatData;
        combatData.Round += 1;

        for (int I = combat.CombatData.GetCount() - 1; I >= 0; I--)
        {
            combat.CombatData.GetObjects(I).NewRound();
        }
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { if (viewer.Time == 0) Execute(viewer.Combat); return viewer.Delay > viewer.Time; }
}
