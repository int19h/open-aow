using Aow.Units.AoWE;
using Aow.Units.Engine;

namespace Aow.Rtl.Tests;

public sealed partial class SpellConstructionTests
{
    private interface IOwnerProbe { int OwnerCalls { get; } }

    private sealed class ProbeTAbility : TAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTAbility() { }
        public ProbeTAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTAdjustableAbility : TAdjustableAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTAdjustableAbility() { }
        public ProbeTAdjustableAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTCombatAction : TCombatAction, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTCombatAction() { }
        public ProbeTCombatAction(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDamageCA : TDamageCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTDamageCA() { }
        public ProbeTDamageCA(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDispelMagicCA : TDispelMagicCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTDispelMagicCA() { }
        public ProbeTDispelMagicCA(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTDurationAbility : TDurationAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTDurationAbility() { }
        public ProbeTDurationAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTEnhancementAbility : TEnhancementAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTEnhancementAbility() { }
        public ProbeTEnhancementAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTGlobalEnchantment : TGlobalEnchantment, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTGlobalEnchantment() { }
        public ProbeTGlobalEnchantment(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTMultiLevelAbility : TMultiLevelAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTMultiLevelAbility() { }
        public ProbeTMultiLevelAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSingleTargetCA : TSingleTargetCA, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTSingleTargetCA() { }
        public ProbeTSingleTargetCA(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpell : TSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTSpell() { }
        public ProbeTSpell(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTSpellCaster : TSpellCaster, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTSpellCaster() { }
        public ProbeTSpellCaster(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitEnchantmentAbility : TUnitEnchantmentAbility, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTUnitEnchantmentAbility() { }
        public ProbeTUnitEnchantmentAbility(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    private sealed class ProbeTUnitSpell : TUnitSpell, IOwnerProbe
    {
        public int OwnerCalls { get; private set; }
        public ProbeTUnitSpell() { }
        public ProbeTUnitSpell(TECustomNode owner) : base(owner) { }
        public override void SetOwner(TECustomNode owner) { OwnerCalls++; base.SetOwner(owner); }
    }

    [Theory]
    [InlineData(typeof(ProbeTAbility), 4)]
    [InlineData(typeof(ProbeTAdjustableAbility), 4)]
    [InlineData(typeof(ProbeTCombatAction), 0)]
    [InlineData(typeof(ProbeTDamageCA), 0)]
    [InlineData(typeof(ProbeTDispelMagicCA), 1)]
    [InlineData(typeof(ProbeTDurationAbility), 4)]
    [InlineData(typeof(ProbeTEnhancementAbility), 4)]
    [InlineData(typeof(ProbeTGlobalEnchantment), 0)]
    [InlineData(typeof(ProbeTMultiLevelAbility), 5)]
    [InlineData(typeof(ProbeTSingleTargetCA), 0)]
    [InlineData(typeof(ProbeTSpell), 4)]
    [InlineData(typeof(ProbeTSpellCaster), 0)]
    [InlineData(typeof(ProbeTUnitEnchantmentAbility), 4)]
    [InlineData(typeof(ProbeTUnitSpell), 4)]
    public void SemanticAndLegacyBaseChainsRunEachBodyOnce(Type type, int children)
    {
        using var fixture = new Fixture();
        var owner = new Owner();
        int before = new TObject().InstanceId;
        var semantic = (TEObject)Activator.CreateInstance(type, owner)!;
        Assert.Equal(children + 2, new TObject().InstanceId - before);
        before = new TObject().InstanceId;
        var legacy = (TEObject)Activator.CreateInstance(type)!;
        Assert.Equal(0, ((IOwnerProbe)legacy).OwnerCalls);
        legacy.Create__init(owner);
        Assert.Equal(children + 2, new TObject().InstanceId - before);
        try
        {
            Assert.Equal(1, ((IOwnerProbe)semantic).OwnerCalls);
            Assert.Equal(1, ((IOwnerProbe)legacy).OwnerCalls);
            Assert.Equal(Snapshot(legacy), Snapshot(semantic));
            Assert.Equal(owner.Observations[0], owner.Observations[1]);
            Assert.Equal(2, owner.Members.Count);
        }
        finally { semantic.Destroy(); legacy.Destroy(); }
        Assert.Empty(owner.Members);
    }

    [Theory]
    [InlineData(typeof(Aow.Units.HolyG.THolyGroundSpell))]
    [InlineData(typeof(Aow.Units.UnHolyG.TUnholyGroundSpell))]
    [InlineData(typeof(Aow.Units.Mountain.TRaiseTerrain))]
    [InlineData(typeof(Aow.Units.PoisonP.TPoisonPlantSpell))]
    public void ConvertedGlobalTargetDescendantsUseCompletedConstruction(Type type)
    {
        using var fixture = new Fixture();
        var owner = new Owner();
        int before = new TObject().InstanceId;
        var raw = (TSpell)EClass.Of(type).Create(owner);
        Assert.IsType(type, raw);
        Assert.NotNull(raw.FSpellText);
        Assert.Equal(6, new TObject().InstanceId - before);
        try
        {
            Assert.Same(raw, Assert.Single(owner.Members));
            Assert.Equal(TSpellType.stGlobal, raw.FSpellType);
            Assert.Equal(1, raw.FResearchCost);
            Assert.Same(fixture.Manager, raw.FCastImages.FILManager);
        }
        finally { raw.Destroy(); }
        Assert.Empty(owner.Members);
    }
}
