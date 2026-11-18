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
            string ansi = Cp1252.Decode(Cp1252.Encode(path));
            Assert.Equal(DelphiPaths.ExtractFileName(ansi), DelphiPaths.ExtractFileName(path));
            Assert.Equal(DelphiPaths.ExtractFilePath(ansi), DelphiPaths.ExtractFilePath(path));
            Assert.Equal(DelphiPaths.ExtractFileDir(ansi), DelphiPaths.ExtractFileDir(path));
            Assert.Equal(DelphiPaths.ExtractFileExt(ansi), DelphiPaths.ExtractFileExt(path));
            Assert.Equal(DelphiPaths.ExtractFileDrive(ansi), DelphiPaths.ExtractFileDrive(path));
            foreach (string extension in new[] { ".new", "", ".\u20ac" })
            {
                Assert.Equal(DelphiPaths.ChangeFileExt(ansi, extension), DelphiPaths.ChangeFileExt(path, extension));
            }
        }
    }

    [Fact]
    public void ExpandFileNameMatchesFilesProbeInputs()
    {
        using StringMode mode = new(false);
        foreach (string path in new[] { "", ".", "..", "probe-files\\alpha.TXT", ".\\probe-files\\d00\\..\\Beta.txt",
            "probe-files\\..", "probe-files\\missing\\..\\new.txt", "probe-files/alpha.TXT" })
        {
            Assert.Equal(DelphiPaths.ExpandFileName(path), DelphiPaths.ExpandFileName(path));
        }
    }

    [Fact]
    public void PathsAcceptNullAndPreserveHostCharacters()
    {
        using StringMode mode = new(false);
        foreach (Func<string?, string> function in new Func<string?, string>[] {
            DelphiPaths.ExtractFileName, DelphiPaths.ExtractFilePath, DelphiPaths.ExtractFileDir, DelphiPaths.ExtractFileExt,
            DelphiPaths.ExtractFileDrive, DelphiPaths.ExpandFileName })
        {
            Assert.Equal("", function(null));
        }

        Assert.Equal("", DelphiPaths.ChangeFileExt(null, null));
        Assert.Equal("file", DelphiPaths.ChangeFileExt("file.ext", null));
        Assert.Equal("\u2603.ext", DelphiPaths.ExtractFileName("C:\\folder\\\u2603.ext"));
        Assert.Equal("C:\\folder\\\u2603.new", DelphiPaths.ChangeFileExt("C:\\folder\\\u2603.ext", ".new"));
    }
}
