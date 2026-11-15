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

        public ProbeAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeAdjustableAbility : AdjustableAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeAdjustableAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeCombatAction : CombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeCombatAction(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDamageCombatAction : DamageCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDamageCombatAction(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDispelMagicCombatAction : DispelMagicCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDispelMagicCombatAction(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDurationAbility : DurationAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeDurationAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeEnhancementAbility : EnhancementAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeEnhancementAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeGlobalEnchantment : GlobalEnchantment, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeGlobalEnchantment(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeMultiLevelAbility : MultiLevelAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeMultiLevelAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSingleTargetCombatAction : SingleTargetCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSingleTargetCombatAction(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpell : Spell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSpell(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpellCaster : SpellCaster, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeSpellCaster(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitEnchantmentAbility : UnitEnchantmentAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeUnitEnchantmentAbility(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitSpell : UnitSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeUnitSpell(CustomNode owner) : base(owner) { }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    [Theory]
    [InlineData(typeof(HolyGroundSpell))]
    [InlineData(typeof(UnholyGroundSpell))]
    [InlineData(typeof(RaiseTerrain))]
    [InlineData(typeof(PoisonPlantSpell))]
    public void ConvertedGlobalTargetDescendantsUseCompletedConstruction(Type type)
    {
        using Fixture fixture = new();
        SpellOwner owner = new();
        Spell raw = (Spell)EngineClass.Of(type).CreateInstance(owner, holder: EngineFixture.Engine);
        Assert.IsType(type, raw);
        Assert.NotNull(raw.SpellText);
        try
        {
            Assert.Same(raw, Assert.Single(owner.Members));
            Assert.Equal(SpellType.Global, raw.SpellType);
            Assert.Equal(1, raw.ResearchCost);
            Assert.Same(fixture.Manager, raw.CastImages.ImageLibraryManager);
        }
        finally { raw.Dispose(); }
        Assert.Empty(owner.Members);
    }
}
