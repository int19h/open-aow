using static Aow.Rtl.Rtl;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class FilesProbeTests
{
    [ProbeFact("files")]
    public void FilesFixture()
    {
        using var directory = new ProbeDirectory();
        var p = new ProbeLines("files");
        Directory.CreateDirectory("probe-files"); File.SetAttributes("probe-files", FileAttributes.Directory);
        static int Attrs(int n) => (n & 7) | ((n & 8) != 0 ? 32 : 0);
        void Make(string name, int attrs)
        {
            File.WriteAllBytes(Path.Combine("probe-files", name), [65, 0, 255]); File.SetAttributes(Path.Combine("probe-files", name), (FileAttributes)attrs);
        }
        foreach (string name in new[] { "zeta.txt", "alpha.TXT", "Beta.txt", "a-2.txt", "a_1.TXT", ".dot", "NoExt" }) Make(name, 0);
        for (int i = 15; i >= 0; i--) Make($"f{i:X2}.dat", Attrs(i));
        for (int i = 7; i >= 0; i--) { string path = Path.Combine("probe-files", $"d{i:X2}"); Directory.CreateDirectory(path); File.SetAttributes(path, (FileAttributes)(faDirectory | Attrs(i))); }
        void Enumerate(string tag, string pattern, int attr)
        {
            TSearchRec search = default;
            int code = FindFirst("probe-files\\" + (AnsiString)pattern, attr, ref search); p.Fact(tag + "_first_code", code); int n = 0;
            if (code == 0)
            {
                try
                {
                    do { p.Fact($"{tag}_name_{n}", search.Name); p.Fact($"{tag}_attr_{n}", search.Attr); p.Fact($"{tag}_size_{n}", search.Size); n++; code = FindNext(ref search); } while (code == 0);
                }
                finally { FindClose(ref search); }
            }
            p.Fact(tag + "_terminal_code", code); p.Fact(tag + "_count", n);
        }
        for (int mask = 0; mask <= faAnyFile; mask++) Enumerate($"mask_{mask:X2}", "*", mask);
        Enumerate("all_dot_star", "*.*", faAnyFile); Enumerate("txt", "*.txt", faAnyFile); Enumerate("a_prefix", "A*", faAnyFile); Enumerate("no_extension", "*.", faAnyFile); Enumerate("missing", "absent*", faAnyFile);
        void PathFacts(string tag, AnsiString path)
        {
            p.Fact(tag + "_input", path); p.Fact(tag + "_name", ExtractFileName(path));
            p.Fact(tag + "_path", ExtractFilePath(path)); p.Fact(tag + "_dir", ExtractFileDir(path));
            p.Fact(tag + "_ext", ExtractFileExt(path)); p.Fact(tag + "_changed", ChangeFileExt(path, ".new")); p.Fact(tag + "_removed", ChangeFileExt(path, ""));
        }
        foreach (var (tag, path) in new[] { ("plain", "alpha.TXT"), ("nested", "sub\\alpha.tar.gz"), ("relative", ".\\sub\\..\\alpha.txt"), ("trailing", "sub\\"), ("empty", ""), ("dotfile", ".hidden"), ("noext", "NoExt"), ("trailing_dot", "name."), ("drive", "C:alpha.txt"), ("forward", "sub/name.ext"), ("dotted_dir", "sub.ext\\name") })
            PathFacts(tag, path);
        foreach (var (tag, path) in new[] { ("empty", ""), ("dot", "."), ("parent", ".."), ("file", "probe-files\\alpha.TXT"), ("collapse", ".\\probe-files\\d00\\..\\Beta.txt"), ("back_to_base", "probe-files\\.."), ("nonexistent", "probe-files\\missing\\..\\new.txt"), ("forward", "probe-files/alpha.TXT") })
        {
            string full = ExpandFileName(path).ToString(), root = GetCurrentDir().ToString();
            string relative = full.Length == 0 ? "" : full == root ? "." : full == Path.GetDirectoryName(root) ? ".." : ".\\" + Path.GetRelativePath(root, full).Replace('/', '\\');
            p.Fact("expand_" + tag + "_input", path); p.Fact("expand_" + tag + "_relative", relative);
        }
        p.End();
    }
}
