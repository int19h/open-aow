#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

// AnimationFrame, AnimationIndex, ShoreTerrain[1.6]
public partial class WaterHex : LowerIsometricHex
{
    public WaterHex(CustomNode owner) : base(owner)
    {
        _animationFrame = (-1 - Random(20)).TruncateToSByte();
        int random = Random(4);
        Sides = (Sides | (random << 6)).TruncateToByte();
    }

    // AoWHex.pas:275
    public new const int ClassId = 132_925;

    private int _animationFrame;
    private byte _animationIndex;

    public byte[] ShoreTerrain { get; } = new byte[6];

    // AoWHex.pas:48
    // AoWHex.pas:315. The base texture includes an owner-address phase.
    public override void Show(int xSurface, int ySurface, in LockedSurface surface)
    {
        MapField field = Owner;
        HexGeometry.HexToPixel(field.HexX, field.HexY, out int x, out int y);
        x -= xSurface + 8;
        y -= ySurface;
        CustomImageLibrary library = ((MapObject)this).Resource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        int phase = FrameFixtureInputs.WaterBasePhase(Owner);
        library[((phase + field.HexX + (field.HexY * 2) + (Sides >> 6)) & 0xF) + 140].Show(x, y, lockedSurface: surface);
        x -= 6;
        y -= 9;
        if ((Sides & 1) != 0)
        {
            library[36].Show(x, y, lockedSurface: surface);
        }

        if ((Sides & 0b0010) != 0)
        {
            library[37].Show(x, y, lockedSurface: surface);
        }

        if ((Sides & 0b0010_0000) != 0)
        {
            library[35].Show(x, y, lockedSurface: surface);
        }
    }

    public override void UpdateTransitions() => UpdateTransitionCore(typeof(LowerIsometricHex), water: true,
        (hexNumber, terrain) => ShoreTerrain[hexNumber - 1] = (byte)terrain);

    public new class Resource : LowerIsometricHex.Resource
    {
        public Resource(CustomNode owner) : base(owner) => DynamicLevel = 23;

        // AoWHex.pas:452
        public new const int ClassId = 132_926;

        // AoWHex.pas:458
        public override int ResourceUserClassId() => WaterHex.ClassId;

        // AoWHex.pas:455
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_waterHexText);

        private static readonly string s_waterHexText = "Water/Ice Hexagon";

    }
}
