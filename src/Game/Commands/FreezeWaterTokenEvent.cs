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
        AoWHSMap.Current.ValidateTerrainObservation(source.Player, (sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2);
        if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            twinkle = new TwinkleAnimation(null);
            twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(10).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand), 100, 64, 50, 30);
            AoWHSMap.Current.PlaceHex(twinkle, Location.X, Location.Y, Location.MapLevel);
        }

        // Use the map's seated player.
        if (AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            AoWHSSet.Current.SpellControl.GetSpell(10).CastSounds.Play(0);
        }

        AoWHSMap.Current.ChangeTerrain(null, Location.X, Location.Y, Location.MapLevel, 1, ChangeTerrain, TerrainChanged);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:641
    public void ChangeTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        FrozenWaterHexSprite ice;
        if (((AoWMapField)mapField).GetArmyHexSprite() == null)
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
                            ice.DaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
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
                ice = new FrozenWaterHexSprite(null);
                AoWHSMap.Current.PlaceHex(ice, mapField.HexX, mapField.HexY, mapField.HexLevel);
            }

            ice.DaysRemaining = (sbyte)((AoWHSMap.Current.Random(11) - 5) / 5 + 3);
            ice.CasterPlayer = (sbyte)EventPlayerIndex;
        }
    }

    // NetworkE.pas:230

}
