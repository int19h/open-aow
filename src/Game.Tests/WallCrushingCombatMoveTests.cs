using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;

namespace Aow.Game.Tests;

// A city wall stands next to a walking unit with Wall Crushing selected. The selection control
// builds the path to the clicked wall hex, and the route step finds the wall as its target. The
// control armed CombatMoveModes.Climbing instead of WallCrushing, the wall hex failed destination
// validation, the path stayed empty and the click did nothing.
[Collection("Engine")]
[EngineState]
public sealed class WallCrushingCombatMoveTests
{
    private sealed class PlacedUnitHexSprite(CustomNode? owner) : TacticalBattleUnitHexSprite(owner)
    {
        public override sbyte HexX => 2;

        public override sbyte HexY => 2;

        public override sbyte HexLevel => 0;
    }

    private sealed class MapScope : IDisposable
    {
        private readonly IDisposable _previousScope = EngineFixture.UseMap(StrategicMap.Current);

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            EngineFixture.SetMap(Map);
        }

        public StrategicMap Map { get; }

        public void Dispose() => _previousScope.Dispose();
    }

    private static void SetWallHitPoints(CityWall wall)
    {
        MemberNames.Field(typeof(CityWall), name: "WallType")!.SetValue(wall, value: 2);
        wall.SetHitPoints(TacticalTables.WallMaxHitPoints[1]);
    }

    private static GamePack CreateCombatPack()
    {
        GamePack pack = EngineFixture.CreateGamePack(new ImageLibraryManager(null));
        pack.AbilityControl.RegisterAbility(new MovementAbility(null!) { Holder = EngineFixture.Engine, AbilityId = 0, AbilityName = "Walking", MoveTypes = MoveTypes.Walking });
        pack.AbilityControl.RegisterAbility(new SelfDestructAbility(null, holder: EngineFixture.Engine));
        pack.AbilityControl.RegisterAbility(new WallCrushingAbility(null, holder: EngineFixture.Engine));
        return pack;
    }

    private static void CrushWallThroughSelectionControl(CityWall wall, int ability)
    {
        using MapScope scope = new();
        using IDisposable packScope = EngineFixture.UsePack(CreateCombatPack());
        IDisposable previousCombatScope = EngineFixture.UseCombatMap(CombatMap.Current);
        CombatMap map = new(null, holder: EngineFixture.Engine);
        try
        {
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(5, 5);
            container.Map = map;
            map.MapContainer = container;
            container.Width = 5;
            container.Height = 5;
            container.LevelCount = 1;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    MapField field = container.MapLevels[0].Field[x, y];
                    bool border = x == 0 || y == 0 || x == 4 || y == 4;
                    field.SetTerrain(TerrainType.FromOriginal((sbyte)(border ? TerrainType.Border : TerrainType.Grass)));
                    field.OverlayType = null;
                }
            }

            const int wallX = 2;
            const int wallY = 1;
            CityWall.Resource resource = wall is CityWallDoor
                ? new CityWallDoor.Resource(null!) { Holder = EngineFixture.Engine }
                : new CityWall.Resource(null!) { Holder = EngineFixture.Engine };
            wall.SetResource(resource);
            MapField wallField = container.MapLevels[0].Field[wallX, wallY];
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexX")!.SetValue(wall, value: wallX);
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexY")!.SetValue(wall, value: wallY);
            wall.SetOwnerWithoutNotify(wallField);
            _ = wallField.AddHexSprite(wall, hexNumber: 0);
            wallField.UpdateMapField();
            SetWallHitPoints(wall);

            Troop troop = new(null) { Holder = EngineFixture.Engine };
            troop.SetAbilitySet(0, value: 1);
            troop.SetAbilitySet(ability, value: 1);
            TacticalBattle combat = new(null) { Holder = EngineFixture.Engine };
            TacticalBattlePlayer combatPlayer = new(null) { Holder = EngineFixture.Engine, Player = (sbyte)PlayerIdExtensions.ToOriginal(PlayerId.Player1) };
            combatPlayer.SetOwner(combat.CombatData.Players);
            TacticalBattleUnit unit = new(null) { Holder = EngineFixture.Engine, Troop = troop, MovePoints = 40, Combat = combat };
            unit.SetCombatPlayer(player: 0);
            PlacedUnitHexSprite sprite = new(null) { Holder = EngineFixture.Engine };
            sprite.Initialize(unit);
            sprite.SelectedAbility = ability;

            CombatUnitSelectionControl control = map.UnitSelection;
            control.Clear();
            _ = control.AddUnit(sprite);
            control.SetCurrentMove(0);
            control.CalculateMovePath(0, wallX, wallY, L: 0);

            CombatMoveControlSettings moveControlSettings = Assert.IsType<CombatMoveControlSettings>(control.MoveControlSettings);
            Assert.True(moveControlSettings.MoveFlags.HasFlag(CombatMoveModes.WallCrushing), userMessage: $"Ability {ability} did not arm WallCrushing.");
            MoveSettings settings = control.MoveSettingsList.GetMoveSettings(0);
            Assert.True(settings.Path.Count > 0, userMessage: $"Ability {ability} found no path to the wall hex.");
            Assert.Equal((sbyte)wallX, settings.Path[0].Location.X);
            Assert.Equal((sbyte)wallY, settings.Path[0].Location.Y);
            Assert.Equal((sbyte)0, settings.Path[0].Location.MapLevel);

            control.MoveSelectedRoute();
            MoveSettings routed = control.MoveSettingsList.GetMoveSettings(0);
            Assert.Equal(2, routed.Path.Count);
            Assert.Equal((sbyte)wallX, routed.Path[0].Location.X);
            Assert.Equal((sbyte)wallY, routed.Path[0].Location.Y);
            Assert.Equal((sbyte)0, routed.Path[0].Location.MapLevel);
        }
        finally
        {
            map.SetOwner(null);
            map.Dispose();
            previousCombatScope.Dispose();
        }
    }

    [Fact]
    public void WallCrushing117ReachesTheCityWall() => CrushWallThroughSelectionControl(new CityWall(null) { Holder = EngineFixture.Engine }, ability: 117);

    [Fact]
    public void WallCrushing117ReachesTheCityWallDoor() => CrushWallThroughSelectionControl(new CityWallDoor(null) { Holder = EngineFixture.Engine }, ability: 117);

    [Fact]
    public void Ability65ReachesTheCityWall() => CrushWallThroughSelectionControl(new CityWall(null) { Holder = EngineFixture.Engine }, ability: 65);
}
