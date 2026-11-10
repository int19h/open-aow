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
public sealed class WallCrushingFastCombatTests
{
    // TAbility's constructor reads the game set's image library manager.
    private sealed class Fixture : IDisposable
    {
        private readonly AoWHSSet previousSet = AoWHSSet.Current;
        private readonly GameEngine previousEngine = GameTables.AoWEngine;
        private readonly ImageLibraryManager manager = new ImageLibraryManager(null!);

        internal Fixture()
        {
            AoWHSSet.Current = EngineFixture.CreateGameSet(manager);
            GameTables.AoWEngine = null!;
        }

        public void Dispose()
        {
            AoWHSSet.Current = previousSet;
            GameTables.AoWEngine = previousEngine;
            manager.Dispose();
        }
    }

    private sealed class Attacker(ECustomNode owner) : CombatObject(owner);

    private sealed class RecordingWall(ECustomNode owner) : CombatWall(owner)
    {
        internal readonly List<(int Attack, int Damage, Set<DamageType> Types)> Rolls = [];

        public override int ExecuteDamageRole(int AAttack, int Damage, Set<DamageType> DamageTypes)
        {
            Rolls.Add((AAttack, Damage, DamageTypes));
            return base.ExecuteDamageRole(AAttack, Damage, DamageTypes);
        }
    }

    private static (Battle Combat, Attacker Source, RecordingWall Wall) Build()
    {
        var combat = new Battle(null!);
        combat.FStatus = combat.FStatus + Set<CombatStatusElement>.Of(CombatStatusElement.LogCombatActions);
        var objects = combat.FCombatData.FCombatObjects;
        var source = new Attacker(objects) { FCombat = combat, FID = 1 };
        var wall = new RecordingWall(objects) { FCombat = combat };
        wall.Setup(2, 2, 1);
        return (combat, source, wall);
    }

    [Fact]
    public void EnabledWallTakesTheSixSixWallRoll()
    {
        using var fixture = new Fixture();
        var (combat, source, wall) = Build();
        var ability = new WallCrushingAbility(null!);
        var command = new FastCombatCommand(117) { FTarget = wall };
        Assert.Equal(10, wall.GetHitPoints());
        int seed = RtlUnit.RandSeed;
        try
        {
            // Seed 16 rolls 10. Attack 6 against wall defense 0 deals (5 * (10 + 2) + 10) / 20 + 1 = 4.
            RtlUnit.RandSeed = 16;
            Assert.True(ability.fcExecuteCombatCommand(source, command));
        }
        finally
        {
            RtlUnit.RandSeed = seed;
        }

        var roll = Assert.Single(wall.Rolls);
        Assert.Equal(6, roll.Attack);
        Assert.Equal(6, roll.Damage);
        Assert.Equal(Set<DamageType>.Of(DamageType.Wall), roll.Types);
        Assert.Equal(1, combat.FLogbook.FActions.Count);
        var action = Assert.IsType<DamageCA>(combat.FLogbook.FActions.GetCombatActions(0));
        Assert.Equal(1, action.FSourceID);
        Assert.Equal(2, action.FTargetID);
        Assert.Equal(4, action.FDamage);
        Assert.Equal(4, action.FDamageDone);
        Assert.Equal(Set<DamageType>.Of(DamageType.Wall), action.FDamageTypes.Value);
        Assert.Equal(Set<DamageType>.Of(), action.FEffects.Value);
        Assert.Equal(6, wall.GetHitPoints());
        Assert.True(wall.FStatus.Contains(CombatObjectStatusElement.Damaged));
    }

    [Fact]
    public void DestroyedWallGetsNoAction()
    {
        using var fixture = new Fixture();
        var (combat, source, wall) = Build();
        wall.FStatus = wall.FStatus + Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        var ability = new WallCrushingAbility(null!);
        var command = new FastCombatCommand(117) { FTarget = wall };
        Assert.True(ability.fcExecuteCombatCommand(source, command));
        Assert.Empty(wall.Rolls);
        Assert.Equal(0, combat.FLogbook.FActions.Count);
        Assert.Equal(10, wall.GetHitPoints());
    }
}
