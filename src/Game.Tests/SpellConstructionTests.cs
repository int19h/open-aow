using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

namespace Aow.Game.Tests;

public sealed partial class SpellConstructionTests
{
    private interface IOwnerProbe { int OwnerCalls { get; } }

    private sealed class ProbeTAbility : Ability, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTAdjustableAbility : AdjustableAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTAdjustableAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTCombatAction : CombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTCombatAction(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDamageCA : DamageCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDamageCA(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDispelMagicCA : DispelMagicCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDispelMagicCA(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDurationAbility : DurationAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDurationAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTEnhancementAbility : EnhancementAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTEnhancementAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTGlobalEnchantment : GlobalEnchantment, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTGlobalEnchantment(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTMultiLevelAbility : MultiLevelAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTMultiLevelAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSingleTargetCA : SingleTargetCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSingleTargetCA(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpell : Spell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSpell(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpellCaster : SpellCaster, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSpellCaster(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitEnchantmentAbility : UnitEnchantmentAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTUnitEnchantmentAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitSpell : UnitSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTUnitSpell(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    [Theory]
    [InlineData(typeof(HolyGroundSpell))]
    [InlineData(typeof(UnholyGroundSpell))]
    [InlineData(typeof(RaiseTerrain))]
    [InlineData(typeof(PoisonPlantSpell))]
    public void ConvertedGlobalTargetDescendantsUseCompletedConstruction(Type type)
    {
        using var fixture = new Fixture();
        var owner = new SpellOwner();
        var raw = (Spell)EClass.Of(type).Create(owner);
        Assert.IsType(type, raw);
        Assert.NotNull(raw.FSpellText);
        try
        {
            Assert.Same(raw, Assert.Single(owner.Members));
            Assert.Equal(SpellType.Global, raw.FSpellType);
            Assert.Equal(1, raw.FResearchCost);
            Assert.Same(fixture.Manager, raw.FCastImages.FILManager);
        }
        finally { raw.Dispose(); }
        Assert.Empty(owner.Members);
    }
}
