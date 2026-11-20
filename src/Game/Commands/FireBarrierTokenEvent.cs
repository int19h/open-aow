// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Commands;

public partial class FireBarrierTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{
    public new const int ClassId = 135940;

    // GlobalTargetSpells.pas:873
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        CreateFireBarrier();
        Spell spell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:842
    public void CreateFireBarrier()
    {
        RandSeed = StrategicMap.Current.Random(16777215);
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[Location.MapLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, Location.X, Location.Y, 0);
        while (walk.Radius <= 1)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField mapField = level.Field[walk.X, walk.Y];
                if (((GlobalTargetSpell)(object)GamePack.Current.SpellControl.GetSpell(11)).ValidTargetMapField(mapField) && mapField.GetArmyHexSprite() is null)
                {
                    Fire fire = GamePack.Current.FireControl.PlaceFire(walk.X, walk.Y, Location.MapLevel, FireType.Large);
                    if (fire is not null)
                    {
                        fire.SetupExplosion(Random(35));
                        fire.TurnsLeft = 3;
                        fire.Player = GetSource().Player;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // NetworkE.pas:230

}
