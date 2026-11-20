namespace Aow.Rtl.Tests;

public sealed class FileAttributeTests
{
    [Fact]
    public void AttributesAndEmptyDirectoryRemovalFollowTheHostFileSystem()
    {
        using ProbeDirectory directory = new();
        File.WriteAllText("attribute-file", contents: "bytes");
        Assert.Equal(0, DelphiFiles.FileSetAttr("attribute-file", attributes: DelphiFiles.faReadOnly));
        Assert.Equal(DelphiFiles.faReadOnly, DelphiFiles.FileGetAttr("attribute-file") & DelphiFiles.faReadOnly);
        Assert.Equal(0, DelphiFiles.FileSetAttr("attribute-file", attributes: 0));
        Assert.Equal(0, DelphiFiles.FileGetAttr("attribute-file") & DelphiFiles.faReadOnly);
        Assert.Equal(-1, DelphiFiles.FileGetAttr("missing"));
        Assert.Equal(-1, DelphiFiles.FileSetAttr("missing", attributes: 0));
        _ = Directory.CreateDirectory("empty");
        Assert.True(DelphiFiles.RemoveDir("empty"));
        Assert.False(DelphiFiles.RemoveDir("missing"));
        _ = Directory.CreateDirectory("occupied");
        File.WriteAllText(Path.Combine("occupied", "file"), contents: "bytes");
        Assert.False(DelphiFiles.RemoveDir("occupied"));
    }
}
