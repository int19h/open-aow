using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Game.Audio;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
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
    public static void Register(GameEngine AEngine)
    {
        GameTables.AoWEngine.RegisterEClasses([EClass.Of<AoWCombatMap>(), EClass.Of<AoWCombatMapLevel>(), EClass.Of<UnitPositionHS>(), EClass.Of<TacticalBattle>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatILTerrain>(), EClass.Of<CombatILTerrainResource>(), EClass.Of<CombatFixedILTerrain>(), EClass.Of<CombatFixedILTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatHexagon.Resource>(), EClass.Of<CombatHexagon>(), EClass.Of<WaterCombatHexagon>(), EClass.Of<WaterCombatHexagon.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatRoad.Resource>(), EClass.Of<CombatRoad>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatTerrain>(), EClass.Of<CombatTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<CityWall>(), EClass.Of<CityWall.Resource>(), EClass.Of<CityWallDoor>(), EClass.Of<CityWallDoor.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<Workshop.Resource>(), EClass.Of<Workshop>()]);
        (AEngine).RegisterEClasses([EClass.Of<Townhall.Resource>(), EClass.Of<Townhall>()]);
        (AEngine).RegisterEClasses([EClass.Of<House.Resource>(), EClass.Of<House>()]);
        (AEngine).RegisterEClasses([EClass.Of<FleeLocation.Resource>(), EClass.Of<FleeLocation>(), EClass.Of<MovementHS>()]);
        (AEngine).RegisterEClasses([EClass.Of<SingleCombatHexagon.Resource>(), EClass.Of<SingleCombatHexagon>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatObstacle.Resource>(), EClass.Of<CombatObstacle>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatMoveTE>(), EClass.Of<CMeleeMoveTE>(), EClass.Of<CAbTouchMoveTE>(), EClass.Of<CAbRangedTE>(), EClass.Of<CEndTurnTE>(), EClass.Of<CEndCombatTE>(), EClass.Of<TerminateCombatTE>()]);
        (AEngine).RegisterEClasses([EClass.Of<IsometricCombatHexagon>(), EClass.Of<IsometricCombatHexagon.Resource>()]);
        (AEngine).RegisterEClasses([EClass.Of<MainStructure.Resource>(), EClass.Of<MainStructure>()]);
        (AEngine).RegisterEClasses([EClass.Of<CenterViewLocation.Resource>(), EClass.Of<CenterViewLocation>()]);
        (AEngine).RegisterEClasses([EClass.Of<CombatFire.Resource>(), EClass.Of<CombatFire>()]);
    }
}
