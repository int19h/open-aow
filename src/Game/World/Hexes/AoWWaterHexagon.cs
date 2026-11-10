// Translated from AoWHex.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// FAnimationFrame, FAnimationIndex, FShoreTerrain[1.6]
public partial class AoWWaterHexagon : LowerIsometricHexagon
{
    public AoWWaterHexagon(ECustomNode owner) : base(owner)
    {
        FAnimationFrame = (sbyte)(-1 - Random(20));
        var random = Random(4);
        this.FSides = (byte)(this.FSides | (random << 6));
    }

    // AoWHex.pas:275
    public new const int ClassID = 132925;

    public sbyte FAnimationFrame;
    public byte FAnimationIndex;
    public FixedArray_byte_6 FShoreTerrain;

    // AoWHex.pas:268

    // AoWHex.pas:48
    // AoWHex.pas:315. The base texture includes an owner-address phase.
    public override void Show(int xSurface, int ySurface, in SurfaceDesc surface)
    {
        var field = (MapField)Owner;
        int x = 0, y = 0;
        HexGeometry.HXtoHP(field.Xhx, field.Yhx, ref x, ref y);
        x -= xSurface + 8;
        y -= ySurface;
        var library = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int phase = FrameFixtureInputs.WaterBasePhase(Owner);
        library[((phase + field.Xhx + field.Yhx * 2 + (FSides >> 6)) & 15) + 140].Show(x, y, surface);
        x -= 6;
        y -= 9;
        if ((FSides & 1) != 0) library[36].Show(x, y, surface);
        if ((FSides & 2) != 0) library[37].Show(x, y, surface);
        if ((FSides & 32) != 0) library[35].Show(x, y, surface);
    }

    public override void UpdateTransitions() => UpdateTransitionCore(typeof(LowerIsometricHexagon), true,
        (hn, terrain) => FShoreTerrain[hn - 1] = (byte)terrain);

    public new partial class Resource : LowerIsometricHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            this.FDynamicLevel = 23;
        }

        // AoWHex.pas:452
        public new const int ClassID = 132926;

        // AoWHex.pas:446

        // AoWHex.pas:458
        public override int ResourceUserClassID() => 132925;

        // AoWHex.pas:58

        // AoWHex.pas:455
        public override string ResourceEditName() => AbilityRules.TranslateRStr(WaterHexagonRStr);

        private static readonly string WaterHexagonRStr = "Water/Ice Hexagon";

    }
}
