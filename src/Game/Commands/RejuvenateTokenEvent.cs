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
        _ = GetEngine().StrategicMap.ValidateTerrainObservation(source.Player, Location.X, Location.Y, L: Location.MapLevel, radius: 2);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.TerrainVisible(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
        {
            TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
            twinkle.Setup(GetEngine().GamePack.SpellControl[41].CastImages.ImageSequences[0], mode: TwinkleModes.Rotate, count: 100, radius: 64, duration: 50, startDelay: 30);
            _ = strategicMap.PlaceHex(twinkle, Location.X, Location.Y, L: Location.MapLevel);
        }

        // Use the map's seated player.
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (engineStrategicMap.SeatedPlayer == source.Player || engineStrategicMap.WatchingTerrain(Location.X, Location.Y, L: Location.MapLevel, radius: 2))
        {
            _ = GetEngine().GamePack.SpellControl[41].CastSounds.Play(0);
        }

        _ = GetEngine().StrategicMap.ChangeTerrain(null, Location.X, Location.Y, L: Location.MapLevel, radius: 2, changeTerrainProc: RejuvenateTerrain, terrainChangedProc: null);
        GamePack gamePack = GetEngine().GamePack;
        Spell? spell = gamePack.SpellControl[EventSpellId];
        gamePack.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
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
