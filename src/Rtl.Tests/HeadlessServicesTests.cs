using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HeadlessServicesTests
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static int RetainObject() => Rtl.ObjectToInt(new TObject());

    [Fact]
    public void IntegerReferenceSlotsRetainObjectsUntilFree()
    {
        int id = RetainObject();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        var value = Rtl.ReferenceFromId<TObject>(id);
        Assert.NotNull(value);
        value.Free();
        Assert.Throws<EAccessViolation>(() => Rtl.ReferenceFromId<TObject>(id));
    }

    [Theory]
    [InlineData("C:\\file", "C:")]
    [InlineData("\\\\server\\share\\file", "\\\\server\\share")]
    [InlineData("relative\\file", "")]
    public void DriveExtractionUsesDelphiSeparators(string path, string drive) => Assert.Equal((AnsiString)drive, Rtl.ExtractFileDrive(path));

    [Fact]
    public void DirectoryChangesStayInsideTheFileService()
    {
        using var directory = new ProbeDirectory();
        var files = new SystemIoFileSystem();
        string processDirectory = Directory.GetCurrentDirectory();
        Directory.CreateDirectory("child");
        Assert.True(files.SetCurrentDirectory("child"));
        Assert.Equal(Path.Combine(directory.Root, "child"), files.CurrentDirectory);
        Assert.Equal(processDirectory, Directory.GetCurrentDirectory());
        Assert.Equal(Path.Combine(directory.Root, "child", "data"), files.ExpandPath("data"));
        Assert.False(files.SetCurrentDirectory("absent"));
        Assert.Equal(Path.Combine(directory.Root, "child"), files.CurrentDirectory);
        Assert.True(files.SetCurrentDirectory(".."));
        Assert.Equal(directory.Root, files.CurrentDirectory);
    }

    [Fact]
    public void RegistryDefaultsAreEmptyAndWritesRemainInMemory()
    {
        var registry = new TRegistry();
        Assert.True(registry.OpenKey("General", true));
        Assert.False(registry.ValueExists("Language"));
        registry.WriteInteger("Language", 1);
        registry.WriteBool("Enabled", true);
        registry.WriteString("Name", "English");
        registry.CloseKey();
        registry.OpenKey("general", false);
        Assert.Equal(1, registry.ReadInteger("language"));
        Assert.True(registry.ReadBool("enabled"));
        Assert.Equal((AnsiString)"English", registry.ReadString("name"));
        registry.OpenKey("Other", false);
        Assert.False(registry.ValueExists("Language"));
        var separate = new TRegistry();
        separate.OpenKey("General", false);
        Assert.False(separate.ValueExists("Language"));
    }
}
