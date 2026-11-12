// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// Inherited single-hexagon sprite layout
public partial class TacticalBattleOozeHexSprite : SingleHexSprite
{
    public TacticalBattleOozeHexSprite(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:4092
    public new const int ClassId = 2228489;

    // AoWTC.pas:4097
    public override sbyte GetOverlay(int hexNumber) => 11;

    // AoWTC.pas:4121
    public override byte GetLevel() => 26;

    // AoWTC.pas:4126
    public override int GetShowPriority() => 1744830464 + (GetHexY() << 13) + GetHexX();

    // AoWTC.pas:4102
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int frame;
        int X = default;
        int Y = default;
        ImageSequence sequence;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref X, ref Y);
        pixelXSurface = X - pixelXSurface - 16;
        pixelYSurface = Y - 48 - pixelYSurface;
        switch (((MapField)Owner).TerrainType)
        {
            case 3:
            case 6:
            case 13:
                sequence = AoWHSSet.Current.ImageSequences.ImageSequences[26];
                break;
            case 12:
                sequence = AoWHSSet.Current.ImageSequences.ImageSequences[27];
                break;
            default:
                sequence = AoWHSSet.Current.ImageSequences.ImageSequences[25];
                break;
        }

        while (sequence != null)
        {
            var totalSequenceFrames = sequence.GetTotalSequenceFrames();
            frame = sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
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
        ((MapField)Owner).UpdateTerrain();
        return result;
    }

    // Engine.pas:524

}
