// Translated from Spring.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class SpringOfLife : Structure
{
    public SpringOfLife(ECustomNode owner) : base(owner)
    {
    }

    // Spring.pas:73
    public new const int ClassId = 131328;

    // Spring.pas:70

    // Spring.pas:111
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        if (!Set<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)Owner.TerrainType))
        {
            ExecuteNaturesBlessing(placedArmy);
        }
    }

    // Spring.pas:76
    public void ShowBlessingAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, out pixelX, out pixelY);
        pixelX -= pixelXSurface;
        pixelY -= pixelYSurface;
        _resource.ImageSequenceList.Show(20, pixelX, pixelY, animation.FrameCounter, lockedSurface);
    }

    // Spring.pas:86
    public void ExecuteNaturesBlessing(ArmyHexSprite armyHexSprite)
    {
        SpringOfLifeAnimation animation = default;
        Rect R = default;
        // { Keep the parameter in its original stack slot. }
        for (int I = 0, last = armyHexSprite.Army.Units.Count - 1; I <= last; I++)
        {
            armyHexSprite.Army.Units.GetUnit(I).ExpandAbility(99);
        }

        bool condition = armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer;
        if (!condition)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            condition = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
        }

        if (condition)
        {
            AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
        }

        var hexLevel2 = GetHexLevel();
        var hexY2 = GetHexY();
        var hexX2 = GetHexX();
        if (StrategicMap.Current.Visible(hexX2, hexY2, hexLevel2, 0))
        {
            animation = new SpringOfLifeAnimation(null) { Holder = CreationHolder() };
            R = new Rect(-16, -8, 32, 40);
            animation.SetDimensions(R);
            animation.AnimationLevel = 27;
            animation.OnShow += ShowBlessingAnimation;
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            StrategicMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
        }
    }

    public new partial class Resource : StructureResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
            TerrainBound = false;
        }

        // Spring.pas:123
        public new const int ClassId = 131329;

        // Spring.pas:117

        // Spring.pas:129
        public override int ResourceUserClassId() => 131328;

        // Spring.pas:37

        // Spring.pas:126
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_springOfLifeText);

        private static readonly string s_springOfLifeText = "Spring of Life";

    }

    // Spring.pas:27

}
