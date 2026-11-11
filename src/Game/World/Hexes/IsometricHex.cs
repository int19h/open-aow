// Translated from IsoHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// FDynamic, FOpenSides (bit HN-1: the neighbour HN is no hexagon of this kind), FSides (bits
// 0.5: the neighbour HN has another terrain, bits 6.7: the image variant)
public partial class IsometricHex : AbstractHexagon
{
    public IsometricHex(ECustomNode owner) : base(owner)
    {
        FSides = (byte)(Random(4) << 6);
    }

    // IsoHex.pas:214
    public new const int ClassID = 131437;

    protected internal DynamicIsometricHex FDynamic;
    protected internal byte FOpenSides;
    protected internal byte FSides;

    // IsoHex.pas:204
    public override void Dispose()
    {
        if (FDynamic != null)
        {
            FDynamic?.Dispose();
            FDynamic = null;
        }

        base.Dispose();
    }

    // IsoHex.pas:198

    // IsoHex.pas:224
    public override void Disconnect()
    {
        if (FDynamic != null)
        {
            FDynamic?.Dispose();
            FDynamic = null;
        }

        base.Disconnect();
    }

    // IsoHex.pas:234
    public override void NeighbourTerrainChanged(int HN, in HexSpriteTerrainInfo Info)
    {
        UpdateTransitions();
    }

    // IsoHex.pas:239
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        CustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
        Xhp -= 6;
        Yhp -= 9;
        if ((FSides & 1) != 0)
        {
            IL[31].Show(Xhp, Yhp, SurfaceDesc);
        }

        if ((FSides & 2) != 0)
        {
            IL[32].Show(Xhp, Yhp, SurfaceDesc);
        }

        if ((FSides & 32) != 0)
        {
            IL[30].Show(Xhp, Yhp, SurfaceDesc);
        }
    }

    // IsoHex.pas:256
    public virtual void ShowDynamic(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
    }

    // IsoHex.pas:50

}
