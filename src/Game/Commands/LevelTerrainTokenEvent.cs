#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

public class LevelTerrainTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:940
    public new const int ClassId = 135945;

    // GlobalTargetSpells.pas:968
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        PlayDefaultEffects(Location.X, Location.Y, Location.MapLevel);
        LevelTerrain();
        Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:943
    public void LevelTerrain()
    {
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.Field[walk.X, walk.Y];
                if (((GlobalTargetSpell)(object)GamePack.Current.SpellControl.GetSpell(18)).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    StrategicMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                    if (mapField.TerrainType == (int)TerrainType.Earth)
                    {
                        _ = StrategicMap.Current.ChangeTerrain(null, walk.X, walk.Y, Location.MapLevel, 0, (int)TerrainType.Dirt);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
