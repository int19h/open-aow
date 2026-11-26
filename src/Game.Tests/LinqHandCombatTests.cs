using Aow.Game.Combat;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

namespace Aow.Game.Tests;

// Issue 317 package 6. Each fact pins a converted query on duplicates, an empty slot, and an empty list.
public sealed class LinqHandCombatTests
{
    [Fact]
    public void PredictorUnitSearchKeepsTheLastMatchAndANullSlot()
    {
        CombatPredictorUnitList empty = [];
        Assert.Equal(-1, empty.IndexOfCpuId(3));
        Assert.Equal(-1, empty.IndexOfUnit(null));

        CombatPredictorUnit first = new() { CpuId = 3 };
        CombatPredictorUnit last = new() { CpuId = 3 };
        CombatPredictorUnitList units = [];
        _ = units.Add(first);
        _ = units.Add(null);
        _ = units.Add(last);
        Assert.Equal(2, units.IndexOfCpuId(3));
        Assert.Equal(2, units.IndexOfUnit(null));
        _ = Assert.Throws<NullReferenceException>(() => units.IndexOfCpuId(1));

        CombatPredictorUnitList hole = [];
        _ = hole.Add(null);
        _ = Assert.Throws<NullReferenceException>(() => hole.IndexOfCpuId(3));
        _ = Assert.Throws<NullReferenceException>(() => hole.IndexOfUnit(null));
    }
}

[Collection("Engine")]
[EngineState]
public sealed class LinqHandCombatNodeTests
{
    [Fact]
    public void CombatSideCountsAndIndexKeepTheLastMatchAndANullSlot()
    {
        CombatSide empty = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, empty.IndexOfId(1));
        Assert.Equal(0, empty.GetDestroyedObjects());
        Assert.Equal(0, empty.GetUndestroyedConquerObjects());

        CombatObject firstDestroyed = new(null)
        {
            Holder = EngineFixture.Engine,
            Id = 7,
            Status = CombatObjectStatuses.Destroyed,
        };
        CombatObject conquer = new(null)
        {
            Holder = EngineFixture.Engine,
            Id = 7,
            Status = CombatObjectStatuses.None,
            ControlMode = CombatObjectControlModes.ConquerObject,
        };
        CombatObject lastDestroyed = new(null)
        {
            Holder = EngineFixture.Engine,
            Id = 7,
            Status = CombatObjectStatuses.Destroyed,
        };
        CombatSide side = new(null) { Holder = EngineFixture.Engine };
        _ = side.AddChild(firstDestroyed);
        _ = side.AddChild(conquer);
        _ = side.AddChild(lastDestroyed);
        Assert.Equal(2, side.IndexOfId(7));
        Assert.Equal(2, side.GetDestroyedObjects());
        Assert.Equal(1, side.GetUndestroyedConquerObjects());

        CombatObject holeMatch = new(null)
        {
            Holder = EngineFixture.Engine,
            Id = 7,
            Status = CombatObjectStatuses.None,
            ControlMode = CombatObjectControlModes.ConquerObject,
        };
        CombatSide hole = new(null) { Holder = EngineFixture.Engine };
        _ = hole.AddChild(new CombatObject(null) { Holder = EngineFixture.Engine, Id = 7, Status = CombatObjectStatuses.Destroyed });
        _ = hole.AddChild(null);
        _ = hole.AddChild(holeMatch);
        Assert.Equal(2, hole.IndexOfId(7));
        _ = Assert.Throws<NullReferenceException>(() => hole.IndexOfId(1));
        _ = Assert.Throws<NullReferenceException>(() => hole.GetDestroyedObjects());
        _ = Assert.Throws<NullReferenceException>(() => hole.GetUndestroyedConquerObjects());
    }

    [Fact]
    public void EnchantmentIndexKeepsTheLastMatchAndANullSlot()
    {
        EnchantmentList empty = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, empty.IndexOfId(4));

        Enchantment first = new(null!) { Holder = EngineFixture.Engine, EnchantmentId = 4 };
        Enchantment last = new(null!) { Holder = EngineFixture.Engine, EnchantmentId = 4 };
        EnchantmentList list = new(null) { Holder = EngineFixture.Engine };
        _ = list.AddChild(first);
        _ = list.AddChild(null);
        _ = list.AddChild(last);
        Assert.Equal(2, list.IndexOfId(4));
        _ = Assert.Throws<NullReferenceException>(() => list.IndexOfId(9));
    }

    [Fact]
    public void TargetStrengthAveragesDuplicateDamageAndAZeroColumn()
    {
        CombatObject host = new(null) { Holder = EngineFixture.Engine };
        host.TargetsChanged();
        Assert.Equal(0, host.GetTargetStrength());

        CombatObject target = new(null) { Holder = EngineFixture.Engine };
        _ = host.Targets.AddABCD(target, B: 0, C: 0, D: 4);
        _ = host.Targets.AddABCD(target, B: 0, C: 0, D: 0);
        _ = host.Targets.AddABCD(target, B: 0, C: 0, D: 4);
        host.TargetsChanged();
        Assert.Equal(2, host.GetTargetStrength());
    }

    [Fact]
    public void TargetStrengthOverflowThrows()
    {
        CombatObject host = new(null) { Holder = EngineFixture.Engine };
        CombatObject target = new(null) { Holder = EngineFixture.Engine };
        _ = host.Targets.AddABCD(target, B: 0, C: 0, D: int.MaxValue);
        _ = host.Targets.AddABCD(target, B: 0, C: 0, D: int.MaxValue);
        host.TargetsChanged();
        _ = Assert.Throws<OverflowException>(() => host.GetTargetStrength());
    }

    [Fact]
    public void SideLeaderInvolvedSearchesBackwardOverDuplicates()
    {
        CombatData empty = new(null) { Holder = EngineFixture.Engine };
        Assert.False(empty.GetSideLeaderInvolved(1));

        CombatData data = new(null) { Holder = EngineFixture.Engine };
        _ = data.Players.AddChild(null);
        _ = data.Players.AddChild(new CombatPlayer(null!) { Holder = EngineFixture.Engine, Player = 1, SideIndex = 1 });
        _ = data.InvolvedLeaders.Add(0);
        _ = data.InvolvedLeaders.Add(1);
        _ = data.InvolvedLeaders.Add(1);
        Assert.True(data.GetSideLeaderInvolved(1));
        _ = Assert.Throws<NullReferenceException>(() => data.GetSideLeaderInvolved(0));
    }

    [Fact]
    public void LeaderDeleteSphereRemovesTheLastDuplicate()
    {
        Leader leader = new(null) { Holder = EngineFixture.Engine };
        Assert.True(leader.AddSphere(MagicSphere.Fire));
        Assert.True(leader.AddSphere(MagicSphere.Life));
        Assert.True(leader.AddSphere(MagicSphere.Fire));
        Assert.True(leader.DeleteSphere(MagicSphere.Fire));
        Assert.Equal(2, leader.SphereCount);
        Assert.Equal(MagicSphere.Fire, leader.GetSpheres(0));
        Assert.Equal(MagicSphere.Life, leader.GetSpheres(1));
        Assert.False(leader.DeleteSphere(MagicSphere.Death));
    }

    [Fact]
    public void EnchantmentUpkeepSumsTheNamedPlayer()
    {
        GlobalMagicControl empty = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(0, empty.GetEnchantmentUpkeep(PlayerId.Player1));

        GlobalMagicControl magic = new(null) { Holder = EngineFixture.Engine };
        _ = magic.Enchantments.AddChild(new GlobalEnchantment(null) { Holder = EngineFixture.Engine, PlayerIndex = PlayerId.Player1, EnchantmentManaUpkeep = 3 });
        _ = magic.Enchantments.AddChild(new GlobalEnchantment(null) { Holder = EngineFixture.Engine, PlayerIndex = PlayerId.Player2, EnchantmentManaUpkeep = 7 });
        _ = magic.Enchantments.AddChild(new GlobalEnchantment(null) { Holder = EngineFixture.Engine, PlayerIndex = PlayerId.Player1, EnchantmentManaUpkeep = 5 });
        Assert.Equal(8, magic.GetEnchantmentUpkeep(PlayerId.Player1));
        Assert.Equal(7, magic.GetEnchantmentUpkeep(PlayerId.Player2));
        Assert.Equal(0, magic.GetEnchantmentUpkeep(PlayerId.Player3));
    }
}
