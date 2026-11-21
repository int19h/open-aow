#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Combat.Actions;

public class CombatObjectTurnDoneCombatAction(CustomNode owner) : CombatAction(owner)
{
    // Combat2.pas:1038
    public new const int ClassId = 131_772;

    public byte CombatObjectId { get; set; }

    // Combat2.pas:1041
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CombatObjectId = persister.RawByte("CombatObjectId", value: CombatObjectId);
    }

    // Combat2.pas:1047
    public override void Execute(Battle combat)
    {
        CombatObject objectValue = combat.CombatData.FindId(CombatObjectId);
        objectValue.Status |= CombatObjectStatuses.TurnDone;
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay > viewer.Time;
    }
}
