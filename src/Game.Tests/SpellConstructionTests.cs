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

    private sealed class ProbeAbility(CustomNode owner) : Ability(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeAdjustableAbility(CustomNode owner) : AdjustableAbility(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeCombatAction(CustomNode owner) : CombatAction(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDamageCombatAction(CustomNode owner) : DamageCombatAction(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDispelMagicCombatAction(CustomNode owner) : DispelMagicCombatAction(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeDurationAbility(CustomNode owner) : DurationAbility(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeEnhancementAbility(CustomNode owner) : EnhancementAbility(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeGlobalEnchantment(CustomNode owner) : GlobalEnchantment(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeMultiLevelAbility(CustomNode owner) : MultiLevelAbility(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSingleTargetCombatAction(CustomNode owner) : SingleTargetCombatAction(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpell(CustomNode owner) : Spell(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeSpellCaster(CustomNode owner) : SpellCaster(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitEnchantmentAbility(CustomNode owner) : UnitEnchantmentAbility(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public override void SetOwner(CustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeUnitSpell(CustomNode owner) : UnitSpell(owner), IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

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
