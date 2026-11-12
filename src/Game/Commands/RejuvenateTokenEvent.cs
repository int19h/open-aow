// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

public partial class RejuvenateTokenEvent : GlobalTargetSpellTokenEvent
{
    public RejuvenateTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:1036
    public new const int ClassId = 135952;

    // GlobalTargetSpells.pas:1044
    public override bool Process(Troop source, int value)
    {
        TwinkleAnimation twinkle = default;
        base.Process(source, value);
        AoWHSMap.Current.ValidateTerrainObservation(source.Player, (sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2);
        if (AoWHSMap.Current.TerrainVisible((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            twinkle = new TwinkleAnimation(null);
            twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(41).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 100, 64, 50, 30);
            AoWHSMap.Current.PlaceHex(twinkle, Location.X, Location.Y, Location.MapLevel);
        }

        // Use the map's seated player.
        if (AoWHSMap.Current.SeatedPlayer == source.Player || AoWHSMap.Current.WatchingTerrain((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, (sbyte)(byte)Location.MapLevel, 2))
        {
            AoWHSSet.Current.SpellControl.GetSpell(41).CastSounds.Play(0);
        }

        AoWHSMap.Current.ChangeTerrain(null, Location.X, Location.Y, Location.MapLevel, 2, RejuvenateTerrain, null);
        var spell = AoWHSSet.Current.SpellControl.GetSpell(EventSpellId);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:1039
    public void RejuvenateTerrain(MapField mapField, ref sbyte terrainType, ref sbyte overlayType)
    {
        if (Set<byte>.WithBounds(0, 255).WithRange((int)World.Map.TerrainType.Desert, (int)World.Map.TerrainType.Wasteland).Contains((byte)terrainType))
        {
            terrainType = (int)World.Map.TerrainType.Grass;
        }
    }

    // NetworkE.pas:230

}
