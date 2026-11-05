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
            Assert.Equal(Rtl.ExtractFileName(ansi), Rtl.ExtractFileName(path));
            Assert.Equal(Rtl.ExtractFilePath(ansi), Rtl.ExtractFilePath(path));
            Assert.Equal(Rtl.ExtractFileDir(ansi), Rtl.ExtractFileDir(path));
            Assert.Equal(Rtl.ExtractFileExt(ansi), Rtl.ExtractFileExt(path));
            Assert.Equal(Rtl.ExtractFileDrive(ansi), Rtl.ExtractFileDrive(path));
            foreach (string extension in new[] { ".new", "", ".\u20ac" })
                Assert.Equal(Rtl.ChangeFileExt(ansi, extension), Rtl.ChangeFileExt(path, extension));
        }
    }

    [Fact]
    public void ExpandFileNameMatchesFilesProbeInputs()
    {
        using var mode = new StringMode(false);
        foreach (string path in new[] { "", ".", "..", "probe-files\\alpha.TXT", ".\\probe-files\\d00\\..\\Beta.txt",
            "probe-files\\..", "probe-files\\missing\\..\\new.txt", "probe-files/alpha.TXT" })
            Assert.Equal(Rtl.ExpandFileName(path), Rtl.ExpandFileName(path));
    }

    [Fact]
    public void PathsAcceptNullAndPreserveHostCharacters()
    {
        using var mode = new StringMode(false);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            Rtl.ExtractFileName, Rtl.ExtractFilePath, Rtl.ExtractFileDir, Rtl.ExtractFileExt,
            Rtl.ExtractFileDrive, Rtl.ExpandFileName }) Assert.Equal("", function(null));
        Assert.Equal("", Rtl.ChangeFileExt((string?)null, null));
        Assert.Equal("file", Rtl.ChangeFileExt("file.ext", (string?)null));
        Assert.Equal("\u2603.ext", Rtl.ExtractFileName("C:\\folder\\\u2603.ext"));
        Assert.Equal("C:\\folder\\\u2603.new", Rtl.ChangeFileExt("C:\\folder\\\u2603.ext", ".new"));
    }
}
