#nullable disable warnings
// Translated from SingleCombatHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public class SingleCombatHex(CustomNode owner) : AbstractHexagon(owner)
{

    // SingleCombatHex.pas:41
    public new const int ClassId = 2_229_534;

    // SingleCombatHex.pas:47
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
        x = x - pixelXSurface - 8;
        y -= pixelYSurface;
        ImageSequence? sequence = base.Resource.ImageSequenceList.GetImageSequence(0);
        sequence.ImageLibrary.Get(sequence.ImageIndex).Show(x, y, lockedSurface);
    }

    // SingleCombatHex.pas:44
    public override MapObjectResourceList GetResourceList(GameEngine? engine) => GamePack.Current.CombatResources;

    public new class Resource(CustomNode owner) : AbstractHexagonResource(owner)
    {

        // SingleCombatHex.pas:59
        public new const int ClassId = 2_229_535;

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
        public override int ResourceUserClassId() => SingleCombatHex.ClassId;

        // SingleCombatHex.pas:65
        public override void SetLevel(int value) => base.SetLevel(0);

        // SingleCombatHex.pas:70
        public override HexControl? HexControl => null;

        // SingleCombatHex.pas:62
        public override string ResourceEditName() => "Combat Single Hexagon";

    }
}
