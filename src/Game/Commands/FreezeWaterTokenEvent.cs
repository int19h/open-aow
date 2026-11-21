#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class FreezeWaterTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{
    public new const int ClassId = 135_939;

    // GlobalTargetSpells.pas:679
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, (sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2);
        if (StrategicMap.Current.TerrainVisible((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2))
        {
            TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
            twinkle.Setup(GamePack.Current.SpellControl[10].CastImages.ImageSequences[0], mode: TwinkleModes.Expand, count: 100, radius: 64, duration: 50, startDelay: 30);
            _ = StrategicMap.Current.PlaceHex(twinkle, Location.X, Location.Y, L: Location.MapLevel);
        }

        // Use the map's seated player.
        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.WatchingTerrain((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2))
        {
            _ = GamePack.Current.SpellControl[10].CastSounds.Play(0);
        }

        _ = StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, L: Location.MapLevel, radius: 1, changeTerrainProc: ChangeTerrain, terrainChangedProc: TerrainChanged);
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:641
    public void ChangeTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (mapField.GetArmyHexSprite() is null)
        {
            if (terrainType == (int)TerrainType.Water)
            {
                terrainType = (int)TerrainType.Ice;
            }
            else
            {
                if (terrainType == (int)TerrainType.UndergroundWater)
                {
                    terrainType = (int)TerrainType.UndergroundIce;
                }
                else
                {
                    if (terrainType is (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundIce)
                    {
                        FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
                        if (ice is not null)
                        {
                            ice.DaysRemaining = (sbyte)(((StrategicMap.Current.Random(11) - 5) / 5) + 3);
                            ice.CasterPlayer = (sbyte)EventPlayerIndex;
                        }
                    }
                }
            }
        }
    }

    // GlobalTargetSpells.pas:661
    public void TerrainChanged(MapField mapField, sbyte originalTerrainType, sbyte originalOverlayType)
    {
        if (mapField.TerrainType is (sbyte)TerrainType.Ice or (sbyte)TerrainType.UndergroundIce && (byte)originalTerrainType is not (6 or 13))
        {
            FrozenWaterHexSprite ice = mapField.FindOwnedHexSprite<FrozenWaterHexSprite>();
            if (ice is null)
            {
                ice = new FrozenWaterHexSprite(null) { Holder = CreationHolder() };
                _ = StrategicMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, L: mapField.HexLevel);
            }

            ice.DaysRemaining = (sbyte)(((StrategicMap.Current.Random(11) - 5) / 5) + 3);
            ice.CasterPlayer = (sbyte)EventPlayerIndex;
        }
    }
}
