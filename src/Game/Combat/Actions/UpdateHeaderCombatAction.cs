// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Combat.Actions;

public partial class UpdateHeaderCombatAction : CombatAction
{
    // Combat2.pas:960
    public new const int ClassId = 131766;

    public UpdateHeaderCombatAction(ECustomNode owner) : base(owner)
    { }

    public string Header = "";

    // Combat2.pas:963
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref Header, 20);
    }

    // Combat2.pas:969
    public override void Execute(Battle combat)
    {
        combat.CombatData.Header = Header;
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.Combat); return false; }
}
