// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.Draw;

// Inherited single-hexagon sprite layout
public partial class COozeHS : SingleHS
{
    public COozeHS(ECustomNode owner) : base(owner)
    { }

    // AoWTC.pas:4092
    public new const int ClassID = 2228489;

    // AoWTC.pas:4097
    public override sbyte GetOverlay(int HN) => 11;

    // AoWTC.pas:4121
    public override byte GetLevel() => 26;

    // AoWTC.pas:4126
    public override int GetShowPriority() => 1744830464 + (GetYhx() << 13) + GetXhx();

    // AoWTC.pas:4102
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Frame;
        int X = default;
        int Y = default;
        ImageSequence Sequence;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
        XhpSurf = X - XhpSurf - 16;
        YhpSurf = Y - 48 - YhpSurf;
        switch (((MapField)Owner).TerrainType)
        {
            case 3:
            case 6:
            case 13:
                Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[26];
                break;
            case 12:
                Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[27];
                break;
            default:
                Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[25];
                break;
        }

        while (Sequence != null)
        {
            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
            Frame = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
            if (Frame != -1)
            {
                Sequence.ImageLibrary[Frame].Show(XhpSurf, YhpSurf, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // AoWTC.pas:4131
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        ((MapField)Owner).UpdateTerrain();
        return Result;
    }

    // Engine.pas:524

}
