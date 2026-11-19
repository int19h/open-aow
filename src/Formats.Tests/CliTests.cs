using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>aowconv output rules and exit codes (own-formats.md 3.2).</summary>
public sealed class CliTests : FormatsTest, IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"aowconv-tests-{Guid.NewGuid():N}");

    public CliTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, true);

    private static string HeroList => Path.Combine(Checker.GameRoot, "Release", "High Level.ahl");

    private (int Code, string Output, string Errors) Run(params string[] arguments)
    {
        StringWriter output = new();
        StringWriter errors = new();
        int code = Program.Run(arguments, output, errors);
        return (code, output.ToString(), errors.ToString());
    }

    private string In(string name) => Path.Combine(_directory, name);

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
        (int code, _, string errors) = Run("to-own", HeroList, In("a.ahl"));
        Assert.Equal(Program.Error, code);
        Assert.Contains("--overwrite", errors);
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
        {
            Assert.Equal(Program.Error, Run("to-own", In("x.ahl"), In("X.AHL"), "--overwrite").Code);
        }

        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(In("x.ahl")));
    }

    [GameFact]
    public void AFailedConversionWritesNothing()
    {
        File.WriteAllBytes(In("bad.ahl"), [1, 0, 0, 1, 99, 0, 5, 0, 0, 0]);
        (int code, _, string errors) = Run("to-own", In("bad.ahl"), In("out.ahl"));
        Assert.Equal(Program.Error, code);
        Assert.Contains("E_UNKNOWN_PROPERTY", errors);
        Assert.False(File.Exists(In("out.ahl")));
        _ = Assert.Single(Directory.GetFiles(_directory));
    }

    [GameFact]
    public void NormalizeWritesASeparateOutput()
    {
        _ = Run("to-own", HeroList, In("a.ahl"));
        byte[] before = File.ReadAllBytes(In("a.ahl"));
        Assert.Equal(Program.Success, Run("normalize", In("a.ahl"), In("n.ahl")).Code);
        Assert.Equal(before, File.ReadAllBytes(In("a.ahl")));
        Assert.Equal(before, File.ReadAllBytes(In("n.ahl")));
        Assert.Equal(Program.Error, Run("normalize", In("a.ahl"), In("a.ahl"), "--overwrite").Code);
    }

    [GameFact]
    public void DirectoryTreesMayNotOverlap()
    {
        _ = Directory.CreateDirectory(In("in"));
        File.Copy(HeroList, Path.Combine(In("in"), "h.ahl"));
        Assert.Equal(Program.Error, Run("to-own", "--dir", In("in"), "--out", Path.Combine(In("in"), "out")).Code);
        (int code, string output, _) = Run("to-own", "--dir", In("in"), "--out", In("out"));
        Assert.Equal(Program.Success, code);
        Assert.Contains("1 converted, 0 failed, 0 skipped", output);
        // A derived output name takes the own extension on import and the original one on export.
        Assert.True(File.Exists(Path.Combine(In("out"), "h.oahl")));
        Assert.False(File.Exists(Path.Combine(In("out"), "h.ahl")));
        (int backCode, _, _) = Run("to-original", "--dir", In("out"), "--out", In("back"));
        Assert.Equal(Program.Success, backCode);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(Path.Combine(In("back"), "h.ahl")));
        _ = Assert.Single(Directory.GetFiles(In("back")));
    }

    // An own document named with an original extension exports to that original name.
    [GameFact]
    public void DirectoryExportOfAnOwnDocumentUnderAnOriginalNameKeepsTheName()
    {
        _ = Directory.CreateDirectory(In("in"));
        Assert.Equal(Program.Success, Run("to-own", HeroList, Path.Combine(In("in"), "h.ahl")).Code);
        Assert.Equal(Program.Success, Run("to-original", "--dir", In("in"), "--out", In("back")).Code);
        Assert.Equal(File.ReadAllBytes(HeroList), File.ReadAllBytes(Path.Combine(In("back"), "h.ahl")));
    }

    // Import skips own documents, and two own documents that export to one name are refused.
    [GameFact]
    public void DirectoryModesSkipOwnInputsAndRefuseCollidingExports()
    {
        _ = Directory.CreateDirectory(In("in"));
        Assert.Equal(Program.Success, Run("to-own", HeroList, Path.Combine(In("in"), "h.oahl")).Code);
        (_, string output, _) = Run("to-own", "--dir", In("in"), "--out", In("out"));
        Assert.Contains("0 converted, 0 failed, 1 skipped", output);
        Assert.Equal(Program.Success, Run("to-own", HeroList, Path.Combine(In("in"), "h.ahl")).Code);
        (int exportCode, _, string exportErrors) = Run("to-original", "--dir", In("in"), "--out", In("back"));
        Assert.NotEqual(Program.Success, exportCode);
        Assert.Contains("exports to the same name as", exportErrors);
        _ = Assert.Single(Directory.GetFiles(In("back")));
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
        _ = Run("to-own", HeroList, In("a.ahl"));
        (int code, string output, _) = Run("check", In("a.ahl"));
        Assert.Equal(Program.Success, code);
        Assert.Contains("Corpus: 0 original files", output);
        Assert.Contains("Own: all eligible documents G-own equal", output);
    }

    [GameFact]
    public void InfoReportsProfileKindAndLegacyRules()
    {
        (int code, string output, _) = Run("info", HeroList);
        Assert.Equal(Program.Success, code);
        Assert.Contains($"Profile: hero-list, kind hero-list, version {OwnFormat.CurrentVersion}", output);
        Assert.Contains("Legacy rules: none", output);
    }

    [Fact]
    public void UnknownCommandsAndMissingFilesAreErrors()
    {
        Assert.Equal(Program.Error, Run("frobnicate").Code);
        Assert.Equal(Program.Error, Run("info", In("missing.hsm")).Code);
    }
}
