#nullable disable warnings
// Translated from Spring.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class SpringOfLife(CustomNode owner) : Structure(owner)
{

    // Spring.pas:73
    public new const int ClassId = 131_328;

    // Spring.pas:111
    public override void ArmyPlaced(ArmyHexSprite? placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        if (Owner.TerrainType is not ((sbyte)Map.TerrainType.Snow or (sbyte)Map.TerrainType.Wasteland))
        {
            ExecuteNaturesBlessing(placedArmy);
        }
    }

    // Spring.pas:76
    public void ShowBlessingAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        pixelX -= pixelXSurface;
        pixelY -= pixelYSurface;
        _ = base.Resource.ImageSequenceList.Show(20, pixelX, pixelY, frame: animation.FrameCounter, lockedSurface);
    }

    // Spring.pas:86
    public void ExecuteNaturesBlessing(ArmyHexSprite? armyHexSprite)
    {
        // { Keep the parameter in its original stack slot. }
        for (int i = 0, last = armyHexSprite.Army.Units.Count - 1; i <= last; i++)
        {
            _ = armyHexSprite.Army.Units.GetUnit(i).ExpandAbility(99);
        }

        bool condition = armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer;
        if (!condition)
        {
            int hexLevel = HexLevel;
            int hexY = HexY;
            int hexX = HexX;
            condition = StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0);
        }

        if (condition)
        {
            _ = AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
        }

        int hexLevel2 = HexLevel;
        int hexY2 = HexY;
        int hexX2 = HexX;
        if (StrategicMap.Current.Visible(hexX2, hexY2, L: hexLevel2, radius: 0))
        {
            SpringOfLifeAnimation animation = new(null, show: ShowBlessingAnimation) { Holder = CreationHolder() };
            Rect r = new(-16, -8, 32, 40);
            animation.SetDimensions(r);
            animation.AnimationLevel = 27;
            int hexLevel = HexLevel;
            int hexY = HexY;
            int hexX = HexX;
            _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY, L: hexLevel);
        }
    }

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner) => TerrainBound = false;

        // Spring.pas:123
        public new const int ClassId = 131_329;

        // Spring.pas:129
        public override int ResourceUserClassId() => SpringOfLife.ClassId;

        // Spring.pas:126
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_springOfLifeText);

        private static readonly string s_springOfLifeText = "Spring of Life";

    }
}
