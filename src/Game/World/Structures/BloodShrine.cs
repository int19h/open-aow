#nullable disable
// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public class BloodShrine(CustomNode owner) : Structure(owner)
{

    // BloodShrine.pas:65
    public new const int ClassId = 131376;

    // BloodShrine.pas:103
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        ExecuteBloodlust(placedArmy);
    }

    // BloodShrine.pas:68
    public void ShowBlessingAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        HexGeometry.HexToPixel(StoredHexX, StoredHexY, out int pixelX, out int pixelY);
        pixelX -= pixelXSurface;
        pixelY -= pixelYSurface;
        _ = base.Resource.ImageSequenceList.Show(20, pixelX, pixelY, frame: animation.FrameCounter, lockedSurface);
    }

    // BloodShrine.pas:77
    public void ExecuteBloodlust(ArmyHexSprite armyHexSprite)
    {
        for (int i = 0, last = armyHexSprite.Army.Units.Count - 1; i <= last; i++)
        {
            Troop troop = armyHexSprite.Army.Units.GetUnit(i);
            _ = GamePack.Current.AbilityControl[168].Expand(troop);
        }

        bool condition = armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer;
        if (!condition)
        {
            sbyte hexLevel = HexLevel;
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            condition = StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0);
        }

        if (condition)
        {
            _ = AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
        }

        sbyte hexLevel2 = HexLevel;
        sbyte hexY2 = HexY;
        sbyte hexX2 = HexX;
        if (StrategicMap.Current.Visible(hexX2, hexY2, L: hexLevel2, radius: 0))
        {
            BloodShrineAnimation animation = new(null) { Holder = CreationHolder() };
            Rect r = new(-16, -8, 32, 40);
            animation.SetDimensions(r);
            animation.AnimationLevel = 27;
            animation.OnShow += ShowBlessingAnimation;
            sbyte hexLevel = HexLevel;
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY, L: hexLevel);
        }
    }

    public new class Resource : StructureResource
    {
        public Resource(CustomNode owner) : base(owner)
        {
            TerrainBound = true;
            ConnectRoads = false;
        }

        // BloodShrine.pas:116
        public new const int ClassId = 131377;

        // BloodShrine.pas:122
        public override int ResourceUserClassId() => 131376;

        // BloodShrine.pas:119
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_bloodShrineText);

        private static readonly string s_bloodShrineText = "Blood Shrine";

    }
}
