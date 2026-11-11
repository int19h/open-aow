// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class FireBarrierTokenEvent : GlobalTargetSpellTokenEvent
{
    public FireBarrierTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135940;

    // GlobalTargetSpells.pas:873
    public override bool Process(Troop source, int value)
    {
        base.Process(source, value);
        CreateFireBarrier();
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:842
    public void CreateFireBarrier()
    {
        HexNumberWalkData Walk = default;
        MapLevel Level;
        AoWMapField MapF;
        Fire Fire;
        RandSeed = AoWHSMap.Current.Random(16777215);
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
        HexNumberWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                MapF = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                if (((GlobalTargetSpell)(object)AoWHSSet.Current.FSpellControl.GetSpell(11)).ValidTargetMapF((MapField)(object)MapF) && MapF.GetArmyHS() == null)
                {
                    Fire = AoWHSSet.Current.FFireControl.PlaceFire(Walk.X, Walk.Y, FLocation.L, FireType.Large);
                    if (Fire != null)
                    {
                        Fire.SetupExplosion(Random(35));
                        Fire.FTurnsLeft = 3;
                        Fire.FPlayer = this.GetSource().FPlayer;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
