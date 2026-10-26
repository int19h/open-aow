namespace Aow.Rtl.Tests;

public sealed class FileAttributeTests
{
    [Fact]
    public void AttributesAndEmptyDirectoryRemovalFollowTheHostFileSystem()
    {
        using var directory = new ProbeDirectory();
        File.WriteAllText("attribute-file", "bytes");
        Assert.Equal(0, Rtl.FileSetAttr("attribute-file", Rtl.faReadOnly));
        Assert.Equal(Rtl.faReadOnly, Rtl.FileGetAttr("attribute-file") & Rtl.faReadOnly);
        Assert.Equal(0, Rtl.FileSetAttr("attribute-file", 0));
        Assert.Equal(0, Rtl.FileGetAttr("attribute-file") & Rtl.faReadOnly);
        Assert.Equal(-1, Rtl.FileGetAttr("missing"));
        Assert.Equal(-1, Rtl.FileSetAttr("missing", 0));
        Directory.CreateDirectory("empty");
        Assert.True(Rtl.RemoveDir("empty"));
        Assert.False(Rtl.RemoveDir("missing"));
        Directory.CreateDirectory("occupied");
        File.WriteAllText(Path.Combine("occupied", "file"), "bytes");
        Assert.False(Rtl.RemoveDir("occupied"));
    }
}
