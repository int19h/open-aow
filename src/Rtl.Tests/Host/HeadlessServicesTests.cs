using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HeadlessServicesTests
{
    [Theory]
    [InlineData("C:\\file", "C:")]
    [InlineData("\\\\server\\share\\file", "\\\\server\\share")]
    [InlineData("relative\\file", "")]
    public void DriveExtractionUsesDelphiSeparators(string path, string drive) => Assert.Equal(drive, DelphiPaths.ExtractFileDrive(path));

    [Fact]
    public void DirectoryChangesStayInsideTheFileService()
    {
        using ProbeDirectory directory = new();
        PhysicalFileSystem files = new();
        string processDirectory = Directory.GetCurrentDirectory();
        _ = Directory.CreateDirectory("child");
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
        DelphiRegistry registry = new();
        Assert.True(registry.OpenKey("General", create: true));
        Assert.False(registry.ValueExists("Language"));
        registry.WriteInteger("Language", 1);
        registry.WriteBool("Enabled", value: true);
        registry.WriteString("Name", "English");
        registry.CloseKey();
        _ = registry.OpenKey("general", create: false);
        Assert.Equal(1, registry.ReadInteger("language"));
        Assert.True(registry.ReadBool("enabled"));
        Assert.Equal("English", registry.ReadString("name"));
        _ = registry.OpenKey("Other", create: false);
        Assert.False(registry.ValueExists("Language"));
        DelphiRegistry separate = new();
        _ = separate.OpenKey("General", create: false);
        Assert.False(separate.ValueExists("Language"));
    }

    [Fact]
    public void MissingStringValueReadsAsEmpty()
    {
        DelphiRegistry registry = new();
        _ = registry.OpenKey("General", create: false);
        Assert.False(registry.ValueExists("Absent Name"));
        Assert.Equal("", registry.ReadString("Absent Name"));
    }

    [Fact]
    public void BinarySettingsRetainTheirStoredSizeAndLeaveTheUnreadTail()
    {
        DelphiRegistry registry = new();
        _ = registry.OpenKey("General", create: true);
        registry.WriteBinaryData("Movement Speed", buffer: [2, 7, 9], size: 2);
        byte[] result = [0xff, 0xff, 0xff, 0xff];
        Assert.Equal(2, registry.ReadBinaryData("Movement Speed", buffer: result, size: result.Length));
        Assert.Equal(new byte[] { 2, 7, 0xff, 0xff }, result);
    }
}
