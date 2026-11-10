using System.Runtime.CompilerServices;
using Aow.Game.Audio;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

public static class CombatClasses
{
    public static void Register(global::Aow.Game.Engine.Engine AEngine)
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.RegisterEClasses([EClass.Of<AoWCombatMap>(), EClass.Of<AoWCombatMapLevel>(), EClass.Of<UnitPositionHS>(), EClass.Of<TacticalCombat>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatILTerrain>(), EClass.Of<CombatILTerrainResource>(), EClass.Of<CombatFixedILTerrain>(), EClass.Of<Aow.Game.World.Terrain.CombatFixedILTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Hexes.CombatHexagon.Resource>(), EClass.Of<CombatHexagon>(), EClass.Of<WaterCombatHexagon>(), EClass.Of<Aow.Game.World.Hexes.WaterCombatHexagon.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Hexes.CombatRoad.Resource>(), EClass.Of<CombatRoad>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatTerrain>(), EClass.Of<Aow.Game.World.Hexes.CombatTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<CityWall>(), EClass.Of<Aow.Game.World.Terrain.CityWall.Resource>(), EClass.Of<CityWallDoor>(), EClass.Of<Aow.Game.World.Terrain.CityWallDoor.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Terrain.Workshop.Resource>(), EClass.Of<Workshop>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Terrain.Townhall.Resource>(), EClass.Of<Townhall>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Terrain.House.Resource>(), EClass.Of<House>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.FleeLocation.Resource>(), EClass.Of<FleeLocation>(), EClass.Of<MovementHS>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Hexes.SingleCombatHexagon.Resource>(), EClass.Of<SingleCombatHexagon>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.CombatObstacle.Resource>(), EClass.Of<CombatObstacle>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatMoveTE>(), EClass.Of<CMeleeMoveTE>(), EClass.Of<CAbTouchMoveTE>(), EClass.Of<CAbRangedTE>(), EClass.Of<CEndTurnTE>(), EClass.Of<CEndCombatTE>(), EClass.Of<TerminateCombatTE>()]);
        (AEngine).RegisterEClasses([EClass.Of<IsometricCombatHexagon>(), EClass.Of<Aow.Game.World.Hexes.IsometricCombatHexagon.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.Terrain.MainStructure.Resource>(), EClass.Of<MainStructure>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.World.CenterViewLocation.Resource>(), EClass.Of<CenterViewLocation>()]);
        (AEngine).RegisterEClasses([EClass.Of<Aow.Game.Draw.CombatFire.Resource>(), EClass.Of<CombatFire>()]);
    }
}
