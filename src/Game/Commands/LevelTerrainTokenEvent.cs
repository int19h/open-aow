// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

public partial class LevelTerrainTokenEvent : GlobalTargetSpellTokenEvent
{
    public LevelTerrainTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:940
    public new const int ClassId = 135945;

    // GlobalTargetSpells.pas:968
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        PlayDefaultEffects(Location.X, Location.Y, Location.MapLevel);
        LevelTerrain();
        var spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:943
    public void LevelTerrain()
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        MapField mapField;
        level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, Location.X, Location.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                mapField = level.Field[walk.X, walk.Y];
                if (((GlobalTargetSpell)(object)GamePack.Current.SpellControl.GetSpell(18)).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() == null)
                {
                    StrategicMap.Current.ClearTerrain(walk.X, walk.Y, Location.MapLevel, 0, -1);
                    if (mapField.TerrainType == (int)TerrainType.Earth)
                    {
                        StrategicMap.Current.ChangeTerrain(null, walk.X, walk.Y, Location.MapLevel, 0, (int)TerrainType.Dirt);
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // NetworkE.pas:230

}
