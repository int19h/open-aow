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
    public new const int ClassId = 2229534;

    // SingleCombatHex.pas:47
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        ImageSequence sequence;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref X, ref Y);
        X = X - pixelXSurface - 8;
        Y = Y - pixelYSurface;
        sequence = _resource.ImageSequenceList.GetImageSequence(0);
        sequence.ImageLibrary.Get(sequence.ImageIndex).Show(X, Y, lockedSurface);
    }

    // SingleCombatHex.pas:44
    public override MapObjectResourceList GetResourceList(GameEngine engine) => AoWHSSet.Current.CombatResources;

    public new partial class Resource : AbstractHexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // SingleCombatHex.pas:59
        public new const int ClassId = 2229535;

        // SingleCombatHex.pas:73
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            base.MessageProc(ref message, sender);
            if (message.MessageId == 65621)
            {
                message.Result = 5;
            }
        }

        // SingleCombatHex.pas:79
        public override int ResourceUserClassId() => 2229534;

        // SingleCombatHex.pas:65
        public override void SetLevel(byte value)
        {
            base.SetLevel(0);
        }

        // SingleCombatHex.pas:70
        public override HexControl GetHexControl() => null;

        // SingleCombatHex.pas:62
        public override string ResourceEditName() => "Combat Single Hexagon";

    }

}
