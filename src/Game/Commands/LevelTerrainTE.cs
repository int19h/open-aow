// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Commands;

public partial class LevelTerrainTE : GlobalTargetSpellTE
{
    public LevelTerrainTE(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:940
    public new const int ClassID = 135945;

    // GlobalTargetSpells.pas:968
    public override bool Process(AbstractUnit source, int value)
    {
        base.Process(source, value);
        PlayDefaultEffects(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
        LevelTerrain();
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, spell.FBaseManaCost);
        return false;
    }

    // GlobalTargetSpells.pas:943
    public void LevelTerrain()
    {
        HNWalkData Walk = default;
        MapLevel Level;
        AoWMapField MapF;
        Level = AoWHSMap.Current.MapContainer.MapLevels[Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer];
        HNWalkData.InitHNWalk(ref Walk, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                MapF = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                if (((GlobalTargetSpell)(object)AoWHSSet.Current.FSpellControl.GetSpell(18)).ValidTargetMapF((MapField)(object)MapF) && MapF.GetArmyHS() == null)
                {
                    AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 0, -1);
                    if (MapF.TerrainType == (int)AoWTerrainType.Earth)
                    {
                        AoWHSMap.Current.ChangeTerrain(null, Walk.X, Walk.Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer, 0, (int)AoWTerrainType.Dirt);
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
