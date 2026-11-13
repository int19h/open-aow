// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class FleeCombatAction : CombatAction
{
    // Combat2.pas:1099
    public new const int ClassId = 131776;

    public FleeCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte ObjectId;

    // Combat2.pas:1102
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref ObjectId, 1), 1, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ObjectId = persister.RawByte("ObjectId", value: ObjectId);
    }

    // Combat2.pas:1108
    public override void Execute(Battle combat)
    {
        CombatObject objectValue;
        objectValue = combat.CombatData.FindId(ObjectId);
        objectValue.Status = objectValue.Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Fled);
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.Combat); return false; }
}
