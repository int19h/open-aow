using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringPathTests
{
    [Fact]
    public void PathSplittingMatchesFilesProbeInputsAndDriveEdges()
    {
        foreach (string path in new[] {
            "alpha.TXT", "sub\\alpha.tar.gz", ".\\sub\\..\\alpha.txt", "sub\\", "", ".hidden",
            "NoExt", "name.", "C:alpha.txt", "sub/name.ext", "sub.ext\\name",
            "C:\\", "\\", "\\\\server\\share\\file.ext", "\\\\server\\share", "\\\\", "\\\\s",
            "C:\\a\\\u20ac.ext" })
        {
            var ansi = Cp1252.Decode(Cp1252.Encode(path));
            Assert.Equal(RtlUnit.ExtractFileName(ansi), RtlUnit.ExtractFileName(path));
            Assert.Equal(RtlUnit.ExtractFilePath(ansi), RtlUnit.ExtractFilePath(path));
            Assert.Equal(RtlUnit.ExtractFileDir(ansi), RtlUnit.ExtractFileDir(path));
            Assert.Equal(RtlUnit.ExtractFileExt(ansi), RtlUnit.ExtractFileExt(path));
            Assert.Equal(RtlUnit.ExtractFileDrive(ansi), RtlUnit.ExtractFileDrive(path));
            foreach (string extension in new[] { ".new", "", ".\u20ac" })
                Assert.Equal(RtlUnit.ChangeFileExt(ansi, extension), RtlUnit.ChangeFileExt(path, extension));
        }
    }

    [Fact]
    public void ExpandFileNameMatchesFilesProbeInputs()
    {
        using var mode = new StringMode(false);
        foreach (string path in new[] { "", ".", "..", "probe-files\\alpha.TXT", ".\\probe-files\\d00\\..\\Beta.txt",
            "probe-files\\..", "probe-files\\missing\\..\\new.txt", "probe-files/alpha.TXT" })
            Assert.Equal(RtlUnit.ExpandFileName(path), RtlUnit.ExpandFileName(path));
    }

    [Fact]
    public void PathsAcceptNullAndPreserveHostCharacters()
    {
        using var mode = new StringMode(false);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            RtlUnit.ExtractFileName, RtlUnit.ExtractFilePath, RtlUnit.ExtractFileDir, RtlUnit.ExtractFileExt,
            RtlUnit.ExtractFileDrive, RtlUnit.ExpandFileName }) Assert.Equal("", function(null));
        Assert.Equal("", RtlUnit.ChangeFileExt((string?)null, null));
        Assert.Equal("file", RtlUnit.ChangeFileExt("file.ext", (string?)null));
        Assert.Equal("\u2603.ext", RtlUnit.ExtractFileName("C:\\folder\\\u2603.ext"));
        Assert.Equal("C:\\folder\\\u2603.new", RtlUnit.ChangeFileExt("C:\\folder\\\u2603.ext", ".new"));
    }
}
