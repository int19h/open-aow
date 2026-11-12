// Translated from BloodShrine.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.World.Structures;

public partial class BloodShrine : Structure
{
    public BloodShrine(ECustomNode owner) : base(owner)
    {
    }

    // BloodShrine.pas:65
    public new const int ClassId = 131376;

    // BloodShrine.pas:62

    // BloodShrine.pas:103
    public override void ArmyPlaced(ArmyHexSprite placedArmy)
    {
        base.ArmyPlaced(placedArmy);
        ExecuteBloodlust(placedArmy);
    }

    // BloodShrine.pas:68
    public void ShowBlessingAnimation(Animation animation, int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(_hexX, _hexY, ref pixelX, ref pixelY);
        pixelX -= pixelXSurface;
        pixelY -= pixelYSurface;
        _resource.ImageSequenceList.Show(20, pixelX, pixelY, animation.FrameCounter, lockedSurface);
    }

    // BloodShrine.pas:77
    public void ExecuteBloodlust(ArmyHexSprite armyHexSprite)
    {
        Troop troop = default;
        Rect R = default;
        BloodShrineAnimation animation = default;
        for (int I = 0, last = armyHexSprite.Army.Count - 1; I <= last; I++)
        {
            troop = armyHexSprite.Army.GetUnit(I);
            GamePack.Current.AbilityControl.GetAbility(168).Expand(troop);
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
            animation = new BloodShrineAnimation(null);
            R = new Rect(-16, -8, 32, 40);
            animation.SetDimensions(R);
            animation.AnimationLevel = 27;
            animation.OnShow = ShowBlessingAnimation;
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
            TerrainBound = true;
            ConnectRoads = false;
        }

        // BloodShrine.pas:116
        public new const int ClassId = 131377;

        // BloodShrine.pas:109

        // BloodShrine.pas:122
        public override int ResourceUserClassId() => 131376;

        // BloodShrine.pas:36

        // BloodShrine.pas:119
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_bloodShrineText);

        private static readonly string s_bloodShrineText = "Blood Shrine";

    }

    // BloodShrine.pas:26

}
