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
        private readonly ImageLibraryManager _manager = new(null);

        internal Fixture() => GamePack.Current = EngineFixture.CreateGameSet(_manager);

        public void Dispose()
        {
            GamePack.Current = _previousSet;
            _manager.Dispose();
        }
    }

    private sealed class Attacker(CustomNode owner) : FastBattleUnit(owner);

    private sealed class RecordingWall(CustomNode owner) : CombatWall(owner)
    {
        internal List<(int Attack, int Damage, DamageTypes Types)> Rolls { get; } = [];

        public override int ExecuteDamageRole(int attack, int damage, DamageTypes damageTypes)
        {
            Rolls.Add((attack, damage, damageTypes));
            return base.ExecuteDamageRole(attack, damage, damageTypes);
        }
    }

    private static (Battle Combat, Attacker Source, RecordingWall Wall) Build()
    {
        Battle combat = new(null) { Holder = EngineFixture.Engine };
        combat.Status |= CombatStatuses.LogCombatActions;
        OwningNode objects = combat.CombatData.CombatObjects;
        Attacker source = new(objects) { Combat = combat, Id = 1 };
        RecordingWall wall = new(objects) { Combat = combat };
        wall.Setup(2, wallType: 2, combatPlayer: 1);
        return (combat, source, wall);
    }

    [Fact]
    public void EnabledWallTakesTheSixSixWallRoll()
    {
        using Fixture fixture = new();
        (Battle? combat, Attacker? source, RecordingWall? wall) = Build();
        WallCrushingAbility ability = new(null, holder: EngineFixture.Engine);
        FastBattleCommand command = new(117) { Target = wall };
        Assert.Equal(10, wall.HitPoints);
        int seed = DelphiRandom.RandSeed;
        try
        {
            // Seed 16 rolls 10. Attack 6 against wall defense 0 deals (5 * (10 + 2) + 10) / 20 + 1 = 4.
            DelphiRandom.RandSeed = 16;
            Assert.True(ability.FastBattleExecuteCombatCommand(source, command));
        }
        finally
        {
            DelphiRandom.RandSeed = seed;
        }

        (int attack, int damage, DamageTypes types) = Assert.Single(wall.Rolls);
        Assert.Equal(6, attack);
        Assert.Equal(6, damage);
        Assert.Equal(DamageTypes.Wall, types);
        Assert.Equal(1, combat.Logbook.Actions.Count);
        DamageCombatAction action = Assert.IsType<DamageCombatAction>(combat.Logbook.Actions.GetCombatActions(0));
        Assert.Equal(1, action.SourceId);
        Assert.Equal(2, action.TargetId);
        Assert.Equal(4, action.Damage);
        Assert.Equal(4, action.DamageDone);
        Assert.Equal(DamageTypes.Wall, action.DamageTypes);
        Assert.Equal(DamageTypes.None, action.Effects);
        Assert.Equal(6, wall.HitPoints);
        Assert.True(wall.Status.Contains(CombatObjectStatuses.Damaged));
    }

    [Fact]
    public void DestroyedWallGetsNoAction()
    {
        using Fixture fixture = new();
        (Battle? combat, Attacker? source, RecordingWall? wall) = Build();
        wall.Status |= CombatObjectStatuses.Destroyed;
        WallCrushingAbility ability = new(null, holder: EngineFixture.Engine);
        Assert.True(ability.FastBattleExecuteCombatCommand(command: new FastBattleCommand(117) { Target = wall }, source: source));
        Assert.Empty(wall.Rolls);
        Assert.Equal(0, combat.Logbook.Actions.Count);
        Assert.Equal(10, wall.HitPoints);
    }
}
