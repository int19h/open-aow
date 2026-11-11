// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class CombatObjectTurnDoneCombatAction : CombatAction
{
    // Combat2.pas:1038
    public new const int ClassID = 131772;

    public CombatObjectTurnDoneCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte F_CombatObjectTurnDoneCA_0C;

    // Combat2.pas:1041
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref F_CombatObjectTurnDoneCA_0C, 1), 1, 20);
    }

    // Combat2.pas:1047
    public override void Execute(Battle Combat)
    {
        CombatObject Obj;
        Obj = Combat.FCombatData.FindID(F_CombatObjectTurnDoneCA_0C);
        Obj.FStatus = Obj.FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.TurnDone);
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { if (viewer.FTime == 0) Execute(viewer.FCombat); return viewer.FDelay > viewer.FTime; }
}
