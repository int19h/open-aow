// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class FreezeWaterTokenEvent : GlobalTargetSpellTokenEvent
{
    public FreezeWaterTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135939;

    // GlobalTargetSpells.pas:679
    public override bool Process(Troop source, int value)
    {
        TwinkleAnimation twinkle = default;
        base.Process(source, value);
        StrategicMap.Current.ValidateTerrainObservation(source.Player, (sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2);
        if (StrategicMap.Current.TerrainVisible((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            twinkle = new TwinkleAnimation(null) { Holder = CreationHolder() };
            twinkle.Setup(GamePack.Current.SpellControl.GetSpell(10).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand), 100, 64, 50, 30);
            StrategicMap.Current.PlaceHex(twinkle, Location.X, Location.Y, Location.MapLevel);
        }

        // Use the map's seated player.
        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.WatchingTerrain((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            GamePack.Current.SpellControl.GetSpell(10).CastSounds.Play(0);
        }

        StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, Location.MapLevel, 1, ChangeTerrain, TerrainChanged);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:641
    public void ChangeTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        FrozenWaterHexSprite ice;
        if (mapField.GetArmyHexSprite() == null)
        {
            if (terrainType == (int)World.Map.TerrainType.Water)
            {
                terrainType = (int)World.Map.TerrainType.Ice;
            }
            else
            {
                if (terrainType == (int)World.Map.TerrainType.UndergroundWater)
                {
                    terrainType = (int)World.Map.TerrainType.UndergroundIce;
                }
                else
                {
                    if (Set<byte>.WithBounds(0, 255).With((int)World.Map.TerrainType.Ice).With((int)World.Map.TerrainType.UndergroundIce).Contains((byte)terrainType))
                    {
                        ice = (FrozenWaterHexSprite)mapField.FindOwnedHexSprite(typeof(FrozenWaterHexSprite));
                        if (ice != null)
                        {
                            ice.DaysRemaining = (sbyte)((StrategicMap.Current.Random(11) - 5) / 5 + 3);
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
        FrozenWaterHexSprite ice;
        if (Set<byte>.WithBounds(0, 255).With((int)TerrainType.Ice).With((int)TerrainType.UndergroundIce).Contains((byte)mapField.TerrainType) && !Set<byte>.WithBounds(0, 255).With((int)TerrainType.Ice).With((int)TerrainType.UndergroundIce).Contains((byte)originalTerrainType))
        {
            ice = (FrozenWaterHexSprite)mapField.FindOwnedHexSprite(typeof(FrozenWaterHexSprite));
            if (ice == null)
            {
                ice = new FrozenWaterHexSprite(null) { Holder = CreationHolder() };
                StrategicMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, mapField.HexLevel);
            }

            ice.DaysRemaining = (sbyte)((StrategicMap.Current.Random(11) - 5) / 5 + 3);
            ice.CasterPlayer = (sbyte)EventPlayerIndex;
        }
    }

    // NetworkE.pas:230

}
