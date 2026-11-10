// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

public partial class MoveWallCA : CombatAction
{
    // Combat2.pas:1072
    public new const int ClassID = 131767;

    public MoveWallCA(ECustomNode owner) : base(owner)
    { }

    public byte F_MoveWallCA_0C;
    public byte F_MoveWallCA_0D;

    // Combat2.pas:1075
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref F_MoveWallCA_0C, 1), 1, 20);
        Stream.rwData(Bytes.Of(ref F_MoveWallCA_0D, 1), 1, 21);
    }

    // Combat2.pas:1082
    public override void Execute(Battle Combat)
    {
        FastCombatUnit Obj;
        Obj = (FastCombatUnit)Combat.FCombatData.FindID(F_MoveWallCA_0C);
        if (F_MoveWallCA_0D == 0)
        {
            Obj.FState = Obj.FState + Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.OutsideWall);
        }

        if (F_MoveWallCA_0D == 1)
        {
            Obj.FState = Obj.FState - Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.OutsideWall);
        }
    }

    // intf_P3_02.inc:38
    public override bool Play(CombatViewer viewer) { Execute(viewer.FCombat); return false; }
}
