// Translated from SingleCombatHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class SingleCombatHex : AbstractHexagon
{
    public SingleCombatHex(ECustomNode owner) : base(owner)
    { }

    // SingleCombatHex.pas:41
    public new const int ClassID = 2229534;

    // SingleCombatHex.pas:47
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int X = default;
        int Y = default;
        ImageSequence Sequence;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        X = X - XhpSurf - 8;
        Y = Y - YhpSurf;
        Sequence = FResource.ImageSequenceList.GetImageSequence(0);
        Sequence.ImageLibrary.Get(Sequence.ImageIndex).Show(X, Y, SurfaceDesc);
    }

    // SingleCombatHex.pas:44
    public override MapObjectResourceList GetResourceList(GameEngine AEngine) => AoWHSSet.Current.FCombatResources;

    public new partial class Resource : AbstractHexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // SingleCombatHex.pas:59
        public new const int ClassID = 2229535;

        // SingleCombatHex.pas:73
        public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // SingleCombatHex.pas:79
        public override int ResourceUserClassID() => 2229534;

        // SingleCombatHex.pas:65
        public override void SetLevel(byte Value)
        {
            base.SetLevel(0);
        }

        // SingleCombatHex.pas:70
        public override HexControl GetHexagonControl() => null;

        // SingleCombatHex.pas:62
        public override string ResourceEditName() => "Combat Single Hexagon";

    }

}
