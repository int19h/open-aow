// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Combat.Actions;

public partial class UpdateHeaderCombatAction : CombatAction
{
    // Combat2.pas:960
    public new const int ClassID = 131766;

    public UpdateHeaderCombatAction(ECustomNode owner) : base(owner)
    { }

    public string F_UpdateHeaderCA_0C = "";

    // Combat2.pas:963
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref F_UpdateHeaderCA_0C, 20);
    }

    // Combat2.pas:969
    public override void Execute(Battle Combat)
    {
        Combat.FCombatData.FHeader = F_UpdateHeaderCA_0C;
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.FCombat); return false; }
}
