#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Commands;

public class FireBarrierTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{
    public new const int ClassId = 135_940;

    // GlobalTargetSpells.pas:873
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreateFireBarrier();
        GamePack gamePack = GetEngine().GamePack;
        Spell spell = gamePack.SpellControl[EventSpellId];
        gamePack.SpellControl[EventSpellId].CastingDone(source, mana: spell.BaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:842
    public void CreateFireBarrier()
    {
        RandSeed = GetEngine().StrategicMap.Random(16777215);
        MapLevel level = GetEngine().StrategicMap.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, radius: 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.Field[walk.X, walk.Y];
                GamePack gamePack = GetEngine().GamePack;
                if (((GlobalTargetSpell)(object)gamePack.SpellControl[11]).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    Fire fire = gamePack.FireControl.PlaceFire(walk.X, walk.Y, L: Location.MapLevel, fireType: FireType.Large);
                    if (fire is not null)
                    {
                        fire.SetupExplosion(Random(35));
                        fire.TurnsLeft = 3;
                        fire.Player = Source.Player;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }
}
