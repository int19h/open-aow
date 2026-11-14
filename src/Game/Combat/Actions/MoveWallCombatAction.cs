// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class MoveWallCombatAction : CombatAction
{
    // Combat2.pas:1072
    public new const int ClassId = 131767;

    public MoveWallCombatAction(ECustomNode owner) : base(owner)
    { }

    public byte CombatObjectId { get; set; }
    public byte InsideWall { get; set; }

    // Combat2.pas:1075
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        CombatObjectId = persister.RawByte("CombatObjectId", value: CombatObjectId);
        InsideWall = persister.RawByte("InsideWall", value: InsideWall);
    }

    // Combat2.pas:1082
    public override void Execute(Battle combat)
    {
        FastBattleUnit objectValue;
        objectValue = (FastBattleUnit)combat.CombatData.FindId(CombatObjectId);
        if (InsideWall == 0)
        {
            objectValue.State = objectValue.State + Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
        }

        if (InsideWall == 1)
        {
            objectValue.State = objectValue.State - Set<FastBattleUnitStateElement>.Of(FastBattleUnitStateElement.OutsideWall);
        }
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.Combat); return false; }
}
