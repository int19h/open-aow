
namespace Aow.Rtl.Tests;

public sealed class FileAttributeTests
{
    [Fact]
    public void AttributesAndEmptyDirectoryRemovalFollowTheHostFileSystem()
    {
        using ProbeDirectory directory = new();
        File.WriteAllText("attribute-file", contents: "bytes");
        Assert.Equal(0, RtlHost.Files.SetAttributes("attribute-file", attributes: DelphiFiles.faReadOnly));
        Assert.Equal(DelphiFiles.faReadOnly, RtlHost.Files.GetAttributes("attribute-file") & DelphiFiles.faReadOnly);
        Assert.Equal(0, RtlHost.Files.SetAttributes("attribute-file", attributes: 0));
        Assert.Equal(0, RtlHost.Files.GetAttributes("attribute-file") & DelphiFiles.faReadOnly);
        Assert.Equal(-1, RtlHost.Files.GetAttributes("missing"));
        Assert.Equal(-1, RtlHost.Files.SetAttributes("missing", attributes: 0));
        _ = Directory.CreateDirectory("empty");
        Assert.True(RtlHost.Files.RemoveDirectory("empty"));
        Assert.False(RtlHost.Files.RemoveDirectory("missing"));
        _ = Directory.CreateDirectory("occupied");
        File.WriteAllText(Path.Combine("occupied", "file"), contents: "bytes");
        Assert.False(RtlHost.Files.RemoveDirectory("occupied"));
    }
}
