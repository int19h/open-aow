// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

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
        PlayDefaultEffects(FLocation.X, FLocation.Y, FLocation.L);
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
        Level = AoWHSMap.Current.MapContainer.MapLevels[FLocation.L];
        HNWalkData.InitHNWalk(ref Walk, FLocation.X, FLocation.Y, 0);
        while (Walk.Radius <= 1)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                MapF = (AoWMapField)Level.Field[Walk.X, Walk.Y];
                if (((GlobalTargetSpell)(object)AoWHSSet.Current.FSpellControl.GetSpell(18)).ValidTargetMapF((MapField)(object)MapF) && MapF.GetArmyHS() == null)
                {
                    AoWHSMap.Current.ClearTerrain(Walk.X, Walk.Y, FLocation.L, 0, -1);
                    if (MapF.TerrainType == (int)AoWTerrainType.Earth)
                    {
                        AoWHSMap.Current.ChangeTerrain(null, Walk.X, Walk.Y, FLocation.L, 0, (int)AoWTerrainType.Dirt);
                    }
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }
    }

    // NetworkE.pas:230

}
