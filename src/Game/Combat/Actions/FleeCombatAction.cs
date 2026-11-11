// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class FleeCombatAction : CombatAction
{
    // Combat2.pas:1099
    public new const int ClassID = 131776;

    public FleeCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte FObjectID;

    // Combat2.pas:1102
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FObjectID, 1), 1, 20);
    }

    // Combat2.pas:1108
    public override void Execute(Battle Combat)
    {
        CombatObject Obj;
        Obj = Combat.FCombatData.FindID(FObjectID);
        Obj.FStatus = Obj.FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Fled);
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.FCombat); return false; }
}
