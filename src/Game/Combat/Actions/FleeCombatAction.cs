// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Combat.Actions;

public class FleeCombatAction(CustomNode owner) : CombatAction(owner)
{
    // Combat2.pas:1099
    public new const int ClassId = 131_776;

    public byte ObjectId { get; set; }

    // Combat2.pas:1102
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ObjectId = persister.RawByte("ObjectId", value: ObjectId);
    }

    // Combat2.pas:1108
    public override void Execute(Battle combat)
    {
        CombatObject objectValue = combat.CombatData.FindId(ObjectId);
        objectValue.Status |= CombatObjectStatuses.Fled;
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer)
    {
        Execute(viewer.Combat);
        return false;
    }
}
