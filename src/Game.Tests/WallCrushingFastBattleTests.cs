using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// TWallCrushingAbility.fcExecuteCombatCommand (AoWAb.PAS:2081) against a city wall, headless.
[Collection("Engine")]
[EngineState]
public sealed class WallCrushingFastBattleTests
{
    // TAbility's constructor reads the game set's image library manager.
    private sealed class Fixture : IDisposable
    {
        private readonly GamePack _previousSet = GamePack.Current;
        private readonly GameEngine _previousEngine = GameTables.GameEngine;
        private readonly ImageLibraryManager _manager = new ImageLibraryManager(null!);

        internal Fixture()
        {
            GamePack.Current = EngineFixture.CreateGameSet(_manager);
            GameTables.GameEngine = null!;
        }

        public void Dispose()
        {
            GamePack.Current = _previousSet;
            GameTables.GameEngine = _previousEngine;
            _manager.Dispose();
        }
    }

    private sealed class Attacker(ECustomNode owner) : CombatObject(owner);

    private sealed class RecordingWall(ECustomNode owner) : CombatWall(owner)
    {
        internal List<(int Attack, int Damage, Set<DamageType> Types)> Rolls { get; } = [];

        public override int ExecuteDamageRole(int attack, int damage, Set<DamageType> damageTypes)
        {
            Rolls.Add((attack, damage, damageTypes));
            return base.ExecuteDamageRole(attack, damage, damageTypes);
        }
    }

    private static (Battle Combat, Attacker Source, RecordingWall Wall) Build()
    {
        var combat = new Battle(null!);
        combat.Status = combat.Status + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        var objects = combat.CombatData.CombatObjects;
        var source = new Attacker(objects) { Combat = combat, Id = 1 };
        var wall = new RecordingWall(objects) { Combat = combat };
        wall.Setup(2, 2, 1);
        return (combat, source, wall);
    }

    [Fact]
    public void EnabledWallTakesTheSixSixWallRoll()
    {
        using var fixture = new Fixture();
        var (combat, source, wall) = Build();
        var ability = new WallCrushingAbility(null!);
        var command = new FastBattleCommand(117) { Target = wall };
        Assert.Equal(10, wall.GetHitPoints());
        int seed = RtlUnit.RandSeed;
        try
        {
            // Seed 16 rolls 10. Attack 6 against wall defense 0 deals (5 * (10 + 2) + 10) / 20 + 1 = 4.
            RtlUnit.RandSeed = 16;
            Assert.True(ability.FastBattleExecuteCombatCommand(source, command));
        }
        finally
        {
            RtlUnit.RandSeed = seed;
        }

        var roll = Assert.Single(wall.Rolls);
        Assert.Equal(6, roll.Attack);
        Assert.Equal(6, roll.Damage);
        Assert.Equal(Set<DamageType>.Of(DamageType.Wall), roll.Types);
        Assert.Equal(1, combat.Logbook.Actions.Count);
        var action = Assert.IsType<DamageCombatAction>(combat.Logbook.Actions.GetCombatActions(0));
        Assert.Equal(1, action.SourceId);
        Assert.Equal(2, action.TargetId);
        Assert.Equal(4, action.Damage);
        Assert.Equal(4, action.DamageDone);
        Assert.Equal(Set<DamageType>.Of(DamageType.Wall), action.DamageTypes.Value);
        Assert.Equal(Set<DamageType>.Of(), action.Effects.Value);
        Assert.Equal(6, wall.GetHitPoints());
        Assert.True(wall.Status.Contains(CombatObjectStatusElement.Damaged));
    }

    [Fact]
    public void DestroyedWallGetsNoAction()
    {
        using var fixture = new Fixture();
        var (combat, source, wall) = Build();
        wall.Status = wall.Status + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        var ability = new WallCrushingAbility(null!);
        var command = new FastBattleCommand(117) { Target = wall };
        Assert.True(ability.FastBattleExecuteCombatCommand(source, command));
        Assert.Empty(wall.Rolls);
        Assert.Equal(0, combat.Logbook.Actions.Count);
        Assert.Equal(10, wall.GetHitPoints());
    }
}
