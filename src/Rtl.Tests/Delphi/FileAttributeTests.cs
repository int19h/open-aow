namespace Aow.Rtl.Tests;

public sealed class FileAttributeTests
{
    [Fact]
    public void AttributesAndEmptyDirectoryRemovalFollowTheHostFileSystem()
    {
        using ProbeDirectory directory = new();
        File.WriteAllText("attribute-file", "bytes");
        Assert.Equal(0, RtlUnit.FileSetAttr("attribute-file", RtlUnit.faReadOnly));
        Assert.Equal(RtlUnit.faReadOnly, RtlUnit.FileGetAttr("attribute-file") & RtlUnit.faReadOnly);
        Assert.Equal(0, RtlUnit.FileSetAttr("attribute-file", 0));
        Assert.Equal(0, RtlUnit.FileGetAttr("attribute-file") & RtlUnit.faReadOnly);
        Assert.Equal(-1, RtlUnit.FileGetAttr("missing"));
        Assert.Equal(-1, RtlUnit.FileSetAttr("missing", 0));
        _ = Directory.CreateDirectory("empty");
        Assert.True(RtlUnit.RemoveDir("empty"));
        Assert.False(RtlUnit.RemoveDir("missing"));
        _ = Directory.CreateDirectory("occupied");
        File.WriteAllText(Path.Combine("occupied", "file"), "bytes");
        Assert.False(RtlUnit.RemoveDir("occupied"));
    }
}
