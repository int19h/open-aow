
using Aow.Engine;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World;
using Aow.Game.World.Hexes;

using Aow.Game.World.Terrain;

namespace Aow.Game.Combat;

public static class CombatClasses
{
    public static void Register(GameEngine AEngine)
    {
        GameTables.AoWEngine.RegisterEClasses([EngineClass.Of<CombatMap>(), EngineClass.Of<CombatMapLevel>(), EngineClass.Of<UnitPositionHexSprite>(), EngineClass.Of<TacticalBattle>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatImageLibraryTerrain>(), EngineClass.Of<CombatImageLibraryTerrainResource>(), EngineClass.Of<CombatFixedImageLibraryTerrain>(), EngineClass.Of<CombatFixedImageLibraryTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatHex.Resource>(), EngineClass.Of<CombatHex>(), EngineClass.Of<WaterCombatHex>(), EngineClass.Of<WaterCombatHex.Resource>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatRoad.Resource>(), EngineClass.Of<CombatRoad>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatTerrain>(), EngineClass.Of<CombatTerrain.Resource>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CityWall>(), EngineClass.Of<CityWall.Resource>(), EngineClass.Of<CityWallDoor>(), EngineClass.Of<CityWallDoor.Resource>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<House.Resource>(), EngineClass.Of<House>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<FleeLocation.Resource>(), EngineClass.Of<FleeLocation>(), EngineClass.Of<MovementHexSprite>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<SingleCombatHex.Resource>(), EngineClass.Of<SingleCombatHex>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatObstacle.Resource>(), EngineClass.Of<CombatObstacle>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatMoveTokenEvent>(), EngineClass.Of<TacticalBattleMeleeMoveTokenEvent>(), EngineClass.Of<CombatAbilityTouchMoveTokenEvent>(), EngineClass.Of<CombatAbilityRangedTokenEvent>(), EngineClass.Of<TacticalBattleEndTurnTokenEvent>(), EngineClass.Of<TacticalBattleEndCombatTokenEvent>(), EngineClass.Of<TerminateCombatTokenEvent>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<IsometricCombatHex>(), EngineClass.Of<IsometricCombatHex.Resource>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<MainStructure.Resource>(), EngineClass.Of<MainStructure>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CenterViewLocation.Resource>(), EngineClass.Of<CenterViewLocation>()]);
        (AEngine).RegisterEClasses([EngineClass.Of<CombatFire>()]);
    }
}
