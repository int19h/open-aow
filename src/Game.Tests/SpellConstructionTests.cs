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

    private sealed class ProbeAbility : Ability, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeAdjustableAbility : AdjustableAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeAdjustableAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeCombatAction : CombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeCombatAction(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDamageCombatAction : DamageCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDamageCombatAction(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDispelMagicCombatAction : DispelMagicCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDispelMagicCombatAction(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDurationAbility : DurationAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDurationAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeEnhancementAbility : EnhancementAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeEnhancementAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeGlobalEnchantment : GlobalEnchantment, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeGlobalEnchantment(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeMultiLevelAbility : MultiLevelAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeMultiLevelAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSingleTargetCombatAction : SingleTargetCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSingleTargetCombatAction(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpell : Spell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSpell(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpellCaster : SpellCaster, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSpellCaster(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitEnchantmentAbility : UnitEnchantmentAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeUnitEnchantmentAbility(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitSpell : UnitSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeUnitSpell(ECustomNode owner) : base(owner) { }

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
        var raw = (Spell)EngineClass.Of(type).CreateInstance(owner);
        Assert.IsType(type, raw);
        Assert.NotNull(raw.SpellText);
        try
        {
            Assert.Same(raw, Assert.Single(owner.Members));
            Assert.Equal(SpellType.Global, raw.SpellType);
            Assert.Equal(1, raw.ResearchCost);
            Assert.Same(fixture.Manager, raw.CastImages._imageLibraryManager);
        }
        finally { raw.Dispose(); }
        Assert.Empty(owner.Members);
    }
}
