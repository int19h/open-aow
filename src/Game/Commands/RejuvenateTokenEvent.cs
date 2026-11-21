#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class RejuvenateTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1036
    public new const int ClassId = 135_952;

    // GlobalTargetSpells.pas:1044
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        _ = StrategicMap.Current.ValidateTerrainObservation(source.Player, (sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2);
        if (StrategicMap.Current.TerrainVisible((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2))
        {
            TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
            twinkle.Setup(GamePack.Current.SpellControl[41].CastImages.ImageSequences[0], mode: TwinkleModes.Rotate, count: 100, radius: 64, duration: 50, startDelay: 30);
            _ = StrategicMap.Current.PlaceHex(twinkle, Location.X, Location.Y, L: Location.MapLevel);
        }

        // Use the map's seated player.
        if (StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.WatchingTerrain((sbyte)(byte)Location.X, (sbyte)(byte)Location.Y, L: (sbyte)(byte)Location.MapLevel, radius: 2))
        {
            _ = GamePack.Current.SpellControl[41].CastSounds.Play(0);
        }

        _ = StrategicMap.Current.ChangeTerrain(null, Location.X, Location.Y, L: Location.MapLevel, radius: 2, changeTerrainProc: RejuvenateTerrain, terrainChangedProc: null);
        Spell? spell = GamePack.Current.SpellControl[EventSpellId];
        GamePack.Current.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
        // The original event result is zero.
        return false;
    }

    // GlobalTargetSpells.pas:1039
    public void RejuvenateTerrain(MapField mapField, ref sbyte terrainType, in sbyte overlayType)
    {
        if (terrainType is >= (sbyte)TerrainType.Desert and <= (sbyte)TerrainType.Wasteland)
        {
            terrainType = (int)TerrainType.Grass;
        }
    }
}
