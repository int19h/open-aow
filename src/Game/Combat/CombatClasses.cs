
using Aow.Engine;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.World;
using Aow.Game.World.Hexes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Combat;

public static class CombatClasses
{
    public static void Register(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<CombatMap>(), EngineClass.Of<CombatMapLevel>(), EngineClass.Of<UnitPositionHexSprite>(), EngineClass.Of<TacticalBattle>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatImageLibraryTerrain>(), EngineClass.Of<CombatImageLibraryTerrainResource>(), EngineClass.Of<CombatFixedImageLibraryTerrain>(), EngineClass.Of<CombatFixedImageLibraryTerrain.Resource>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatHex.Resource>(), EngineClass.Of<CombatHex>(), EngineClass.Of<WaterCombatHex>(), EngineClass.Of<WaterCombatHex.Resource>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatRoad.Resource>(), EngineClass.Of<CombatRoad>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatTerrain>(), EngineClass.Of<CombatTerrain.Resource>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CityWall>(), EngineClass.Of<CityWall.Resource>(), EngineClass.Of<CityWallDoor>(), EngineClass.Of<CityWallDoor.Resource>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<House.Resource>(), EngineClass.Of<House>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<FleeLocation.Resource>(), EngineClass.Of<FleeLocation>(), EngineClass.Of<MovementHexSprite>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<SingleCombatHex.Resource>(), EngineClass.Of<SingleCombatHex>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatObstacle.Resource>(), EngineClass.Of<CombatObstacle>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatMoveTokenEvent>(), EngineClass.Of<TacticalBattleMeleeMoveTokenEvent>(), EngineClass.Of<CombatAbilityTouchMoveTokenEvent>(), EngineClass.Of<CombatAbilityRangedTokenEvent>(), EngineClass.Of<TacticalBattleEndTurnTokenEvent>(), EngineClass.Of<TacticalBattleEndCombatTokenEvent>(), EngineClass.Of<TerminateCombatTokenEvent>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<IsometricCombatHex>(), EngineClass.Of<IsometricCombatHex.Resource>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<MainStructure.Resource>(), EngineClass.Of<MainStructure>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CenterViewLocation.Resource>(), EngineClass.Of<CenterViewLocation>()]);
        (engine).RegisterEngineClasses([EngineClass.Of<CombatFire>()]);
    }
}
