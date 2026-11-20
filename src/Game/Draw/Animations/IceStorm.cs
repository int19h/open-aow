#nullable disable
// Translated from Storms.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

public partial class IceStorm(CustomNode owner) : BlastStorm(owner)
{

    // Storms.pas:363
    public new const int ClassId = 135738;

    // Storms.pas:390
    public override void ChangeStormTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        // { Original sends terrain 13 to the default branch before the ice refresh test. }
        if (terrainType == 0)
        {
            terrainType = 6;
        }
        else
        {
            if (terrainType != 6)
            {
                terrainType = 3;
            }
            else
            {
                if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)terrainType))
                {
                    FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
                    if (ice is not null)
                    {
                        ice.DaysRemaining = (sbyte)(3 + ((StrategicMap.Current.Random(11) - 5) / 5));
                        ice.CasterPlayer = Player;
                    }
                }
            }
        }
    }

    // Storms.pas:373
    public override void StormTerrainChanged(MapField mapField, sbyte originalTerrainType, sbyte originalOverlayType)
    {
        if (Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With(6).With(13).Contains((byte)originalTerrainType))
        {
            FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
            if (ice is null)
            {
                ice = new FrozenWaterHexSprite(null) { Holder = CreationHolder() };
                _ = StrategicMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, mapField.HexLevel);
            }

            ice.DaysRemaining = (sbyte)(3 + ((StrategicMap.Current.Random(11) - 5) / 5));
            ice.CasterPlayer = Player;
        }
    }

    // Storms.pas:409
    public override string StormFileName() => "EFFECTS\\STORM1.ILB";

    // Storms.pas:414
    public override StormType StormType() => Magic.Spells.StormType.Ice;

    // Animation.pas:43

    // Storms.pas:368
    public override string Name() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.IceStormText);

}
