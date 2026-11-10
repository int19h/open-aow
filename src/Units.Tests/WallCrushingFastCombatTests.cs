using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;

namespace Aow.Units.Tests;

// TWallCrushingAbility.fcExecuteCombatCommand (AoWAb.PAS:2081) against a city wall, headless.
[Collection("Engine")]
[EngineState]
public sealed class WallCrushingFastCombatTests
{
    // TAbility's constructor reads the game set's image library manager.
    private sealed class Fixture : IDisposable
    {
        private readonly TAoWHSSet previousSet = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet;
        private readonly TAoWEngine previousEngine = Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine;
        private readonly TImageLibraryManager manager = new TImageLibraryManager(null!);

        internal Fixture()
        {
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet = EngineFixture.CreateGameSet(manager);
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine = null!;
        }

        public void Dispose()
        {
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet = previousSet;
            Aow.Game.Magic.Abilities.AoWEUnit.AoWEngine = previousEngine;
            manager.Dispose();
        }
    }

    private sealed class Attacker(TECustomNode owner) : TCombatObject(owner);

    private sealed class RecordingWall(TECustomNode owner) : TCombatWall(owner)
    {
        internal readonly List<(int Attack, int Damage, TSet<TDamageType> Types)> Rolls = [];

        public override int ExecuteDamageRole(int AAttack, int Damage, TSet<TDamageType> DamageTypes)
        {
            Rolls.Add((AAttack, Damage, DamageTypes));
            return base.ExecuteDamageRole(AAttack, Damage, DamageTypes);
        }
    }

    private static (TCombat Combat, Attacker Source, RecordingWall Wall) Build()
    {
        var combat = new TCombat(null!);
        combat.FStatus = combat.FStatus + TSet<TCombatStatusElement>.Of(TCombatStatusElement.cLogCombatActions);
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
        var ability = new TWallCrushingAbility(null!);
        var command = new TFastCombatCommand(117) { FTarget = wall };
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
        Assert.Equal(TSet<TDamageType>.Of(TDamageType.dtWall), roll.Types);
        Assert.Equal(1, combat.FLogbook.FActions.Count);
        var action = Assert.IsType<TDamageCA>(combat.FLogbook.FActions.GetCombatActions(0));
        Assert.Equal(1, action.FSourceID);
        Assert.Equal(2, action.FTargetID);
        Assert.Equal(4, action.FDamage);
        Assert.Equal(4, action.FDamageDone);
        Assert.Equal(TSet<TDamageType>.Of(TDamageType.dtWall), action.FDamageTypes.Value);
        Assert.Equal(TSet<TDamageType>.Of(), action.FEffects.Value);
        Assert.Equal(6, wall.GetHitPoints());
        Assert.True(wall.FStatus.Contains(TCombatObjectStatusElement.csDamaged));
    }

    [Fact]
    public void DestroyedWallGetsNoAction()
    {
        using var fixture = new Fixture();
        var (combat, source, wall) = Build();
        wall.FStatus = wall.FStatus + TSet<TCombatObjectStatusElement>.Of(TCombatObjectStatusElement.csDestroyed);
        var ability = new TWallCrushingAbility(null!);
        var command = new TFastCombatCommand(117) { FTarget = wall };
        Assert.True(ability.fcExecuteCombatCommand(source, command));
        Assert.Empty(wall.Rolls);
        Assert.Equal(0, combat.FLogbook.FActions.Count);
        Assert.Equal(10, wall.GetHitPoints());
    }
}
