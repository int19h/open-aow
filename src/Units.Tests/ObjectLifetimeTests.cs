using System.Runtime.CompilerServices;
using Aow.Units.AoWE;
using Aow.Units.aowFX;
using Aow.Units.Engine;
using Aow.Units.GlobalEnchantmentSpells;

namespace Aow.Units.Tests;

// The units once stored object references as integer ids, and the id table kept every mapped object
// alive until Free (review-9x.md finding 22). The lists and phases now hold or read the object
// itself, so an object its owner releases becomes garbage with no Free.
[Collection("Engine")]
[EngineState]
public sealed class ObjectLifetimeTests
{
    private static void Collect()
    {
        for (int i = 0; i < 3; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    // One predicted round: the attacker lists the target in FTargets, copies it to its round
    // targets and hits it. The owner frees the attacker and nothing frees the target.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Attacker, WeakReference Target) PredictRound()
    {
        var attacker = new TCombatPredictorUnit { FEnabled = 1, FTargets = new TQuadItemList<TCombatPredictorUnit>() };
        var target = new TCombatPredictorUnit { FEnabled = 1, FHitPoints = 10, FCPUID = 7 };
        attacker.FTargets.AddABCD(target, 4, target.FCPUID, 0);
        attacker.SetupRoundTargets(0);
        attacker.SortRoundTargets();
        Assert.Same(target, attacker.FRoundTargets[0].A);
        Assert.Equal(4, target.FIncomingCombatValue);
        Assert.True(attacker.ExecuteRound());
        Assert.Equal(6, target.FHitPoints);
        var result = (new WeakReference(attacker), new WeakReference(target));
        attacker?.Dispose();
        return result;
    }

    [Fact]
    public void PredictorUnitsAreCollectibleOnceTheirOwnerReleasesThem()
    {
        var (attacker, target) = PredictRound();
        Collect();
        Assert.False(attacker.IsAlive);
        Assert.False(target.IsAlive);
    }

    // AddTarget records each side in the other's list. The owner frees the attacker and lets the
    // target go without Free.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference Attacker, WeakReference Target) Target()
    {
        using var owner = new TENode(null!);
        var attacker = new TCombatObject(owner);
        var target = new TCombatObject(owner);
        attacker.AddTarget(target, 5, 0, 2);
        Assert.Equal(0, attacker.FTargets.IndexOfItemA(target));
        Assert.Equal(0, target.FTargetedBy.IndexOfItemA(attacker));
        Assert.Equal(5, target.FIncomingCombatValue);
        attacker.RemoveTarget(target);
        Assert.Equal(-1, target.FTargetedBy.IndexOfItemA(attacker));
        attacker.AddTarget(target, 3, 0, 1);
        var result = (new WeakReference(attacker), new WeakReference(target));
        attacker?.Dispose();
        target.SetOwner(null!);
        Assert.Equal(0, owner.Count);
        return result;
    }

    [Fact]
    public void CombatObjectsAreCollectibleOnceTheirOwnerReleasesThem()
    {
        var (attacker, target) = Target();
        Collect();
        Assert.False(attacker.IsAlive);
        Assert.False(target.IsAlive);
    }

    // The particle reads its instance number as the brightness phase while it draws. The Earth
    // Mastery spell has no cast images under the test host, so the draw stops after the phase and
    // blits nothing. The particle goes with no Free.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference ShowParticle()
    {
        var particle = new TWorldFXHS(null!) { FEffectType = TworldFXType.fxEMParticle, FFrame = 0 };
        particle.FPath.Add(new TXYZ { X = 1, Y = 2, Z = 3 });
        Runtime.Dialogs = null;
        particle.Show(0, 0, default);
        return new WeakReference(particle);
    }

    [Fact]
    public void DrawnEffectsAreCollectibleOnceDropped()
    {
        var control = AoWEUnit.AoWHSSet.FSpellControl;
        var spell = new TEarthMastery(null!);
        WeakReference particle;
        control.RegisterSpell(spell);
        try
        {
            Assert.Null(control.GetSpell(64).FCastImages.GetImageSequence(0));
            particle = ShowParticle();
        }
        finally
        {
            control.UnregisterSpell(spell);
            spell?.Dispose();
        }
        Collect();
        Assert.False(particle.IsAlive);
    }
}
