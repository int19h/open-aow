// Translated from AoWHex.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

// AnimationFrame, AnimationIndex, ShoreTerrain[1.6]
public partial class WaterHex : LowerIsometricHex
{
    public WaterHex(ECustomNode owner) : base(owner)
    {
        _animationFrame = (sbyte)(-1 - Random(20));
        var random = Random(4);
        this.Sides = (byte)(this.Sides | (random << 6));
    }

    // AoWHex.pas:275
    public new const int ClassId = 132925;

    private sbyte _animationFrame;
    private byte _animationIndex;
    public ByteArray6 ShoreTerrain;

    // AoWHex.pas:268

    // AoWHex.pas:48
    // AoWHex.pas:315. The base texture includes an owner-address phase.
    public override void Show(int xSurface, int ySurface, in LockedSurface surface)
    {
        var field = (MapField)Owner;
        int x = 0, y = 0;
        HexGeometry.HexToPixel(field.HexX, field.HexY, ref x, ref y);
        x -= xSurface + 8;
        y -= ySurface;
        var library = _resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int phase = FrameFixtureInputs.WaterBasePhase(Owner);
        library[((phase + field.HexX + field.HexY * 2 + (Sides >> 6)) & 15) + 140].Show(x, y, surface);
        x -= 6;
        y -= 9;
        if ((Sides & 1) != 0) library[36].Show(x, y, surface);
        if ((Sides & 2) != 0) library[37].Show(x, y, surface);
        if ((Sides & 32) != 0) library[35].Show(x, y, surface);
    }

    public override void UpdateTransitions() => UpdateTransitionCore(typeof(LowerIsometricHex), true,
        (hexNumber, terrain) => ShoreTerrain[hexNumber - 1] = (byte)terrain);

    public new partial class Resource : LowerIsometricHex.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            this.DynamicLevel = 23;
        }

        // AoWHex.pas:452
        public new const int ClassId = 132926;

        // AoWHex.pas:446

        // AoWHex.pas:458
        public override int ResourceUserClassId() => 132925;

        // AoWHex.pas:58

        // AoWHex.pas:455
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_waterHexText);

        private static readonly string s_waterHexText = "Water/Ice Hexagon";

    }
}
