#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Commands;

public class FreezeWaterTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{
    public new const int ClassId = 135_939;

    // GlobalTargetSpells.pas:679
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 2);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.TerrainVisible(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
        {
            TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
            twinkle.Setup(GetEngine().GamePack.SpellControl[10].CastImages.ImageSequences[0], mode: TwinkleModes.Expand, count: 100, radius: 64, duration: 50, startDelay: 30);
            _ = strategicMap.PlaceHex(twinkle, Location.X, Location.Y, L: Location.MapLevel);
        }

        // Use the map's seated player.
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (engineStrategicMap.SeatedPlayer == source.Player || engineStrategicMap.WatchingTerrain(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
        {
            _ = GetEngine().GamePack.SpellControl[10].CastSounds.Play(0);
        }

        _ = GetEngine().StrategicMap.ChangeTerrain(null, Location.X, Location.Y, L: Location.MapLevel, radius: 1, changeTerrainProc: ChangeTerrain, terrainChangedProc: TerrainChanged);
        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
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
                            ice.DaysRemaining = (((GetEngine().StrategicMap.Random(11) - 5) / 5) + 3).TruncateToSByte();
                            ice.CasterPlayer = PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(EventPlayerIndex));
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
                _ = GetEngine().StrategicMap.PlaceHex(ice, mapField.HexX, mapField.HexY, L: mapField.HexLevel);
            }

            ice.DaysRemaining = (((GetEngine().StrategicMap.Random(11) - 5) / 5) + 3).TruncateToSByte();
            ice.CasterPlayer = PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(EventPlayerIndex));
        }
    }
}
