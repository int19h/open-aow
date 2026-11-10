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
    public static void Register(TEngine AEngine)
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.RegisterEClasses([EClass.Of<TAoWCombatMap>(), EClass.Of<TAoWCombatMapLevel>(), EClass.Of<TUnitPositionHS>(), EClass.Of<TTacticalCombat>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatILTerrain>(), EClass.Of<TCombatILTerrainResource>(), EClass.Of<TCombatFixedILTerrain>(), EClass.Of<TCombatFixedILTerrainResource>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatHexagonResource>(), EClass.Of<TCombatHexagon>(), EClass.Of<TWaterCombatHexagon>(), EClass.Of<TWaterCombatHexagonResource>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatRoadResource>(), EClass.Of<TCombatRoad>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatTerrain>(), EClass.Of<TCombatTerrainResource>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCityWall>(), EClass.Of<TCityWallResource>(), EClass.Of<TCityWallDoor>(), EClass.Of<TCityWallDoorResource>()]);
        (AEngine).RegisterEClasses([EClass.Of<TWorkshopResource>(), EClass.Of<TWorkshop>()]);
        (AEngine).RegisterEClasses([EClass.Of<TTownhallResource>(), EClass.Of<TTownhall>()]);
        (AEngine).RegisterEClasses([EClass.Of<THouseResource>(), EClass.Of<THouse>()]);
        (AEngine).RegisterEClasses([EClass.Of<TFleeLocationResource>(), EClass.Of<TFleeLocation>(), EClass.Of<TMovementHS>()]);
        (AEngine).RegisterEClasses([EClass.Of<TSingleCombatHexagonResource>(), EClass.Of<TSingleCombatHexagon>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatObstacleResource>(), EClass.Of<TCombatObstacle>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatMoveTE>(), EClass.Of<TCMeleeMoveTE>(), EClass.Of<TCAbTouchMoveTE>(), EClass.Of<TCAbRangedTE>(), EClass.Of<TCEndTurnTE>(), EClass.Of<TCEndCombatTE>(), EClass.Of<TTerminateCombatTE>()]);
        (AEngine).RegisterEClasses([EClass.Of<TIsometricCombatHexagon>(), EClass.Of<TIsometricCombatHexagonResource>()]);
        (AEngine).RegisterEClasses([EClass.Of<TMainStructureResource>(), EClass.Of<TMainStructure>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCenterViewLocationResource>(), EClass.Of<TCenterViewLocation>()]);
        (AEngine).RegisterEClasses([EClass.Of<TCombatFireResource>(), EClass.Of<TCombatFire>()]);
    }
}
