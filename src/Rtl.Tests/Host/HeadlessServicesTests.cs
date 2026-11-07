using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HeadlessServicesTests
{
    [Theory]
    [InlineData("C:\\file", "C:")]
    [InlineData("\\\\server\\share\\file", "\\\\server\\share")]
    [InlineData("relative\\file", "")]
    public void DriveExtractionUsesDelphiSeparators(string path, string drive) => Assert.Equal(drive, RtlUnit.ExtractFileDrive(path));

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
        Assert.Equal("English", registry.ReadString("name"));
        registry.OpenKey("Other", false);
        Assert.False(registry.ValueExists("Language"));
        var separate = new TRegistry();
        separate.OpenKey("General", false);
        Assert.False(separate.ValueExists("Language"));
    }

    [Fact]
    public void MissingStringValueReadsAsEmpty()
    {
        var registry = new TRegistry();
        registry.OpenKey("General", false);
        Assert.False(registry.ValueExists("Absent Name"));
        Assert.Equal("", registry.ReadString("Absent Name"));
    }

    [Fact]
    public void BinarySettingsRetainTheirStoredSizeAndLeaveTheUnreadTail()
    {
        var registry = new TRegistry();
        registry.OpenKey("General", true);
        registry.WriteBinaryData("Movement Speed", new byte[] { 2, 7, 9 }, 2);
        byte[] result = [0xff, 0xff, 0xff, 0xff];
        Assert.Equal(2, registry.ReadBinaryData("Movement Speed", result, result.Length));
        Assert.Equal(new byte[] { 2, 7, 0xff, 0xff }, result);
    }
}
