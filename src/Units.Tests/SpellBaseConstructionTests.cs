using Aow.Units.AoWE;
using Aow.Units.Engine;

namespace Aow.Units.Tests;

public sealed partial class SpellConstructionTests
{
    private interface IOwnerProbe { int OwnerCalls { get; } }

    private sealed class ProbeTAbility : TAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTAdjustableAbility : TAdjustableAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTAdjustableAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTCombatAction : TCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTCombatAction(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDamageCA : TDamageCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDamageCA(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDispelMagicCA : TDispelMagicCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDispelMagicCA(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDurationAbility : TDurationAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTDurationAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTEnhancementAbility : TEnhancementAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTEnhancementAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTGlobalEnchantment : TGlobalEnchantment, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTGlobalEnchantment(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTMultiLevelAbility : TMultiLevelAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTMultiLevelAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSingleTargetCA : TSingleTargetCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSingleTargetCA(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpell : TSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSpell(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpellCaster : TSpellCaster, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTSpellCaster(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitEnchantmentAbility : TUnitEnchantmentAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTUnitEnchantmentAbility(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitSpell : TUnitSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }

        public ProbeTUnitSpell(TECustomNode owner) : base(owner) { }

        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    [Theory]
    [InlineData(typeof(Aow.Units.HolyG.THolyGroundSpell))]
    [InlineData(typeof(Aow.Units.UnHolyG.TUnholyGroundSpell))]
    [InlineData(typeof(Aow.Units.Mountain.TRaiseTerrain))]
    [InlineData(typeof(Aow.Units.PoisonP.TPoisonPlantSpell))]
    public void ConvertedGlobalTargetDescendantsUseCompletedConstruction(Type type)
    {
        using var fixture = new Fixture();
        var owner = new SpellOwner();
        var raw = (TSpell)EClass.Of(type).Create(owner);
        Assert.IsType(type, raw);
        Assert.NotNull(raw.FSpellText);
        try
        {
            Assert.Same(raw, Assert.Single(owner.Members));
            Assert.Equal(TSpellType.stGlobal, raw.FSpellType);
            Assert.Equal(1, raw.FResearchCost);
            Assert.Same(fixture.Manager, raw.FCastImages.FILManager);
        }
        finally { raw.Dispose(); }
        Assert.Empty(owner.Members);
    }
}
