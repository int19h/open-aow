// Translated from IsoHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

public partial class UpperIsometricHex : IsometricHex
{
    public UpperIsometricHex(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131440;

    // IsoHex.pas:355
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        CustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        if (FDynamic == null)
        {
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
    }

    public new partial class Resource : IsometricHexResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 131441;

        // IsoHex.pas:443
        public override int ResourceUserClassID() => 131440;

        // IsoHex.pas:448
        public override string ResourceEditName() => UpperIsometricHexagonStr;

        private static readonly string UpperIsometricHexagonStr = "Upper Isometric Hexagon";

    }

}
