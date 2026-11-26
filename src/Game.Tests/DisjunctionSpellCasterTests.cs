using Aow.Game.Magic.Spells;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 344, the port-only checks of the dispel path whose expected values come from the rules
/// (GlobalSpells.pas:806-846): the Disjunction caster starts at a chance of 50 and its dialog
/// clamps the chance to 1 through 75. The dispel's outcome in play is checked by the headless
/// dispel input and by the oracle's dispel scenario.
/// </summary>
public sealed class DisjunctionSpellCasterTests
{
    [Fact]
    public void ANewCasterStartsAtHalfChance()
    {
        DisjunctionSpellCaster caster = new(null);
        Assert.Equal(50, caster.DispelChance);
        Assert.Equal(75, caster.DispelChanceMax);
    }

    [Theory]
    [InlineData(-5, 1)]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(24, 24)]
    [InlineData(75, 75)]
    [InlineData(76, 75)]
    [InlineData(1_000, 75)]
    public void TheChanceIsClampedToTheDialogRange(int requested, int expected)
    {
        DisjunctionSpellCaster caster = new(null);
        caster.SetDispelChance(requested);
        Assert.Equal(expected, caster.DispelChance);
    }

    [Fact]
    public void TheEnchantmentIdIsStoredAsGiven()
    {
        DisjunctionSpellCaster caster = new(null);
        caster.SetEnchantmentId(7);
        Assert.Equal(7, caster.EnchantmentId);
    }
}
