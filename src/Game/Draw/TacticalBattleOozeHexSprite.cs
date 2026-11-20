// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// Inherited single-hexagon sprite layout
public class TacticalBattleOozeHexSprite(CustomNode owner) : SingleHexSprite(owner)
{

    // AoWTC.pas:4092
    public new const int ClassId = 2228489;

    // AoWTC.pas:4097
    public override sbyte GetOverlay(int hexNumber) => 11;

    // AoWTC.pas:4121
    public override byte Level => 26;

    // AoWTC.pas:4126
    public override int GetShowPriority() => 1744830464 + (HexY << 13) + HexX;

    // AoWTC.pas:4102
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
        pixelXSurface = x - pixelXSurface - 16;
        pixelYSurface = y - 48 - pixelYSurface;
        ImageSequence sequence = Owner.TerrainType switch
        {
            3 or 6 or 13 => GamePack.Current.ImageSequences.ImageSequences[26],
            12 => GamePack.Current.ImageSequences.ImageSequences[27],
            _ => GamePack.Current.ImageSequences.ImageSequences[25],
        };
        while (sequence is not null)
        {
            int frame = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(divisor: sequence.TotalSequenceFrames, instance: this));
            if (frame != -1)
            {
                sequence.ImageLibrary[frame].Show(pixelXSurface, pixelYSurface, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    // AoWTC.pas:4131
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        Owner.UpdateTerrain();
        return result;
    }
}
