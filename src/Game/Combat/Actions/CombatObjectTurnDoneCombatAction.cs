// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class CombatObjectTurnDoneCombatAction : CombatAction
{
    // Combat2.pas:1038
    public new const int ClassId = 131772;

    public CombatObjectTurnDoneCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte CombatObjectId;

    // Combat2.pas:1041
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(Bytes.Of(ref CombatObjectId, 1), 1, 20);
    }

    // Combat2.pas:1047
    public override void Execute(Battle combat)
    {
        CombatObject objectValue;
        objectValue = combat.CombatData.FindId(CombatObjectId);
        objectValue.Status = objectValue.Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.TurnDone);
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { if (viewer.Time == 0) Execute(viewer.Combat); return viewer.Delay > viewer.Time; }
}
