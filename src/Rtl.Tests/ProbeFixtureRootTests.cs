namespace Aow.Rtl.Tests;

public sealed class ProbeFixtureRootTests
{
    [Fact]
    public void PrivateCheckoutVariableSelectsTheFixtureRoot()
    {
        string? previous = Environment.GetEnvironmentVariable("AOW_PRIVATE");
        string checkout = Path.Combine(Path.GetTempPath(), "private-checkout");
        try
        {
            Environment.SetEnvironmentVariable("AOW_PRIVATE", checkout);
            Assert.Equal(Path.Combine(checkout, "tests", "probes", "expected"), ProbeFactAttribute.Root);
        }
        finally { Environment.SetEnvironmentVariable("AOW_PRIVATE", previous); }
    }

    [Fact]
    public void WithoutTheVariableThePrivateCheckoutIsTheProductSibling()
    {
        string parent = Path.Combine(Path.GetTempPath(), "checkouts");
        Assert.Equal(Path.Combine(parent, "open-aow-private", "tests", "probes", "expected"),
            ProbeFactAttribute.ResolveRoot(null, Path.Combine(parent, "open-aow")));
    }

    [Fact]
    public void MissingFixturesReportTheResolvedRootAndTheVariable()
    {
        string root = Path.Combine(Path.GetTempPath(), "absent-probes-" + Guid.NewGuid().ToString("N"));
        var fact = new ProbeFactAttribute("random", root);
        Assert.Contains(Path.Combine(root, "random.txt"), fact.Skip);
        Assert.Contains("Fixture root: " + root, fact.Skip);
        Assert.Contains("AOW_PRIVATE", fact.Skip);
    }
}
