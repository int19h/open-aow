using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>aowconv output rules and exit codes (own-formats.md 3.2).</summary>
public sealed class CliTests : FormatsTest, IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "aowconv-tests-" + Guid.NewGuid().ToString("N"));

    public CliTests() => Directory.CreateDirectory(directory);

    public void Dispose() => Directory.Delete(directory, true);

    private static string HeroList => Path.Combine(Checker.GameRoot, "Release", "High Level.ahl");

    private (int Code, string Output, string Errors) Run(params string[] args)
    {
        var output = new StringWriter();
        var errors = new StringWriter();
        int code = Program.Run(args, output, errors);
        return (code, output.ToString(), errors.ToString());
    }

    private string In(string name) => Path.Combine(directory, name);

    [GameFact]
    public void ToOwnThenToOriginalReproducesTheFile()
    {
        Assert.Equal(Program.Success, Run("to-own", HeroList, In("a.ahl")).Code);
        Assert.Equal(Program.Success, Run("to-original", In("a.ahl"), In("b.ahl")).Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(In("b.ahl")));
        Assert.Equal((byte)'{', File.ReadAllBytes(In("a.ahl"))[0]);
    }

    [GameFact]
    public void AnExistingDestinationNeedsOverwrite()
    {
        File.WriteAllText(In("a.ahl"), "keep");
        var refused = Run("to-own", HeroList, In("a.ahl"));
        Assert.Equal(Program.Error, refused.Code);
        Assert.Contains("--overwrite", refused.Errors);
        Assert.Equal("keep", File.ReadAllText(In("a.ahl")));
        Assert.Equal(Program.Success, Run("to-own", HeroList, In("a.ahl"), "--overwrite").Code);
        Assert.NotEqual("keep", File.ReadAllText(In("a.ahl")));
    }

    [GameFact]
    public void TheInputIsNeverTheOutput()
    {
        File.Copy(HeroList, In("x.ahl"));
        Assert.Equal(Program.Error, Run("to-own", In("x.ahl"), In("x.ahl"), "--overwrite").Code);
        if (OperatingSystem.IsWindows())
            Assert.Equal(Program.Error, Run("to-own", In("x.ahl"), In("X.AHL"), "--overwrite").Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(In("x.ahl")));
    }

    [GameFact]
    public void AFailedConversionWritesNothing()
    {
        File.WriteAllBytes(In("bad.ahl"), [1, 0, 0, 1, 99, 0, 5, 0, 0, 0]);
        var result = Run("to-own", In("bad.ahl"), In("out.ahl"));
        Assert.Equal(Program.Error, result.Code);
        Assert.Contains("E_UNKNOWN_PROPERTY", result.Errors);
        Assert.False(File.Exists(In("out.ahl")));
        Assert.Single(Directory.GetFiles(directory));
    }

    [GameFact]
    public void NormalizeWritesASeparateOutput()
    {
        Run("to-own", HeroList, In("a.ahl"));
        var before = File.ReadAllBytes(In("a.ahl"));
        Assert.Equal(Program.Success, Run("normalize", In("a.ahl"), In("n.ahl")).Code);
        Assert.Equal(before, File.ReadAllBytes(In("a.ahl")));
        Assert.Equal(before, File.ReadAllBytes(In("n.ahl")));
        Assert.Equal(Program.Error, Run("normalize", In("a.ahl"), In("a.ahl"), "--overwrite").Code);
    }

    [Fact]
    public void ExactLayoutConflictsWithLossyExport()
    {
        var result = Run("to-original", "a", "b", "--exact-layout", "--allow-lossy");
        Assert.Equal(Program.Error, result.Code);
        Assert.Contains("conflicts", result.Errors);
    }

    [GameFact]
    public void DirectoryTreesMayNotOverlap()
    {
        Directory.CreateDirectory(In("in"));
        File.Copy(HeroList, Path.Combine(In("in"), "h.ahl"));
        Assert.Equal(Program.Error, Run("to-own", "--dir", In("in"), "--out", Path.Combine(In("in"), "out")).Code);
        var result = Run("to-own", "--dir", In("in"), "--out", In("out"));
        Assert.Equal(Program.Success, result.Code);
        Assert.Contains("1 converted, 0 failed, 0 skipped", result.Output);
        // A derived output name takes the own extension on import and the original one on export.
        Assert.True(File.Exists(Path.Combine(In("out"), "h.oahl")));
        Assert.False(File.Exists(Path.Combine(In("out"), "h.ahl")));
        var back = Run("to-original", "--dir", In("out"), "--out", In("back"));
        Assert.Equal(Program.Success, back.Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(Path.Combine(In("back"), "h.ahl")));
        Assert.Single(Directory.GetFiles(In("back")));
    }

    // An own document named with an original extension exports to that original name.
    [GameFact]
    public void DirectoryExportOfAnOwnDocumentUnderAnOriginalNameKeepsTheName()
    {
        Directory.CreateDirectory(In("in"));
        Assert.Equal(Program.Success, Run("to-own", HeroList, Path.Combine(In("in"), "h.ahl")).Code);
        Assert.Equal(Program.Success, Run("to-original", "--dir", In("in"), "--out", In("back")).Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(Path.Combine(In("back"), "h.ahl")));
    }

    // The explicit output path of the single-file forms is kept as given.
    [GameFact]
    public void AnExplicitOutputPathIsKept()
    {
        Assert.Equal(Program.Success, Run("to-own", HeroList, In("a.json")).Code);
        Assert.True(File.Exists(In("a.json")));
        Assert.Equal(Program.Success, Run("to-original", In("a.json"), In("b.bin")).Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(In("b.bin")));
    }

    [GameFact]
    public void CheckReportsOwnDocuments()
    {
        Run("to-own", HeroList, In("a.ahl"));
        var result = Run("check", In("a.ahl"));
        Assert.Equal(Program.Success, result.Code);
        Assert.Contains("Corpus: 0 original files", result.Output);
        Assert.Contains("Own: all eligible documents G-own equal", result.Output);
    }

    [GameFact]
    public void InfoReportsProfileKindAndResidue()
    {
        var result = Run("info", HeroList);
        Assert.Equal(Program.Success, result.Code);
        Assert.Contains("Profile: hero-list, kind hero-list, version 1", result.Output);
        Assert.Contains("Residue: 0 layout parts", result.Output);
    }

    [Fact]
    public void UnknownCommandsAndMissingFilesAreErrors()
    {
        Assert.Equal(Program.Error, Run("frobnicate").Code);
        Assert.Equal(Program.Error, Run("info", In("missing.hsm")).Code);
    }
}
