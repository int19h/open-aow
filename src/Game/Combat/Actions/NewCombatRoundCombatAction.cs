// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat.Actions;

public partial class NewCombatRoundCombatAction : CombatAction
{
    // Combat2.pas:1009
    public new const int ClassID = 131771;

    public NewCombatRoundCombatAction(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:1012
    public override void Execute(Battle Combat)
    {
        var combatData = Combat.FCombatData;
        combatData.FRound += 1;

        for (int I = Combat.FCombatData.GetCount() - 1; I >= 0; I--)
        {
            Combat.FCombatData.GetObjects(I).NewRound();
        }
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { if (viewer.FTime == 0) Execute(viewer.FCombat); return viewer.FDelay > viewer.FTime; }
}
