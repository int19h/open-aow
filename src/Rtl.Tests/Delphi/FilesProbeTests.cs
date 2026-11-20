using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.RtlHost;

namespace Aow.Rtl.Tests;

public sealed class FilesProbeTests
{
    [ProbeFact("files")]
    public void FilesFixture()
    {
        using ProbeDirectory directory = new();
        ProbeLines p = new("files");
        _ = Directory.CreateDirectory("probe-files");
        File.SetAttributes("probe-files", fileAttributes: FileAttributes.Directory);
        static int Attributes(int n) => (n & 7) | ((n & 8) != 0 ? 32 : 0);
        void Make(string name, int attributes)
        {
            File.WriteAllBytes(Path.Combine("probe-files", name), bytes: [65, 0, 255]);
            File.SetAttributes(Path.Combine("probe-files", name), fileAttributes: (FileAttributes)attributes);
        }
        foreach (string name in new[] { "zeta.txt", "alpha.TXT", "Beta.txt", "a-2.txt", "a_1.TXT", ".dot", "NoExt" })
        {
            Make(name, attributes: 0);
        }

        for (int i = 15; i >= 0; i--)
        {
            Make($"f{i:X2}.dat", attributes: Attributes(i));
        }

        for (int i = 7; i >= 0; i--)
        {
            string path = Path.Combine("probe-files", $"d{i:X2}");
            _ = Directory.CreateDirectory(path);
            File.SetAttributes(path, fileAttributes: (FileAttributes)(faDirectory | Attributes(i)));
        }
        // The search lists entries without the Delphi result codes. The recording holds them, and with the
        // probe's files they follow from the count: 0 then 18 for a search with entries, 2 then 2 for one
        // whose pattern matches no name.
        void Enumerate(string tag, string pattern, int attribute)
        {
            IReadOnlyList<RtlFileEntry> entries = Files.FindEntries($"probe-files\\{pattern}", attributes: attribute);
            p.Fact($"{tag}_first_code", value: entries.Count == 0 ? 2 : 0);
            int n = 0;
            foreach (RtlFileEntry entry in entries)
            {
                p.Fact($"{tag}_name_{n}", value: entry.Name);
                p.Fact($"{tag}_attr_{n}", value: entry.Attribute);
                p.Fact($"{tag}_size_{n}", value: entry.Size);
                n++;
            }
            p.Fact($"{tag}_terminal_code", value: entries.Count == 0 ? 2 : 18);
            p.Fact($"{tag}_count", value: n);
        }
        for (int mask = 0; mask <= faAnyFile; mask++)
        {
            Enumerate($"mask_{mask:X2}", pattern: "*", attribute: mask);
        }

        Enumerate("all_dot_star", pattern: "*.*", attribute: faAnyFile);
        Enumerate("txt", pattern: "*.txt", attribute: faAnyFile);
        Enumerate("a_prefix", pattern: "A*", attribute: faAnyFile);
        Enumerate("no_extension", pattern: "*.", attribute: faAnyFile);
        Enumerate("missing", pattern: "absent*", attribute: faAnyFile);
        void PathFacts(string tag, string path)
        {
            p.Fact($"{tag}_input", value: path);
            p.Fact($"{tag}_name", value: ExtractFileName(path));
            p.Fact($"{tag}_path", value: ExtractFilePath(path));
            p.Fact($"{tag}_dir", value: ExtractFileDir(path));
            p.Fact($"{tag}_ext", value: ExtractFileExt(path));
            p.Fact($"{tag}_changed", value: ChangeFileExt(path, extension: ".new"));
            p.Fact($"{tag}_removed", value: ChangeFileExt(path, extension: ""));
        }
        foreach ((string? tag, string? path) in new[] { ("plain", "alpha.TXT"), ("nested", "sub\\alpha.tar.gz"), ("relative", ".\\sub\\..\\alpha.txt"), ("trailing", "sub\\"), ("empty", ""), ("dotfile", ".hidden"), ("noext", "NoExt"), ("trailing_dot", "name."), ("drive", "C:alpha.txt"), ("forward", "sub/name.ext"), ("dotted_dir", "sub.ext\\name") })
        {
            PathFacts(tag, path);
        }

        foreach ((string? tag, string? path) in new[] { ("empty", ""), ("dot", "."), ("parent", ".."), ("file", "probe-files\\alpha.TXT"), ("collapse", ".\\probe-files\\d00\\..\\Beta.txt"), ("back_to_base", "probe-files\\.."), ("nonexistent", "probe-files\\missing\\..\\new.txt"), ("forward", "probe-files/alpha.TXT") })
        {
            string full = ExpandFileName(path).ToString(), root = GetCurrentDir();
            string relative = full.Length == 0 ? "" : full == root ? "." : full == Path.GetDirectoryName(root) ? ".." : $".\\{Path.GetRelativePath(root, path: full).Replace('/', '\\')}";
            p.Fact($"expand_{tag}_input", value: path);
            p.Fact($"expand_{tag}_relative", value: relative);
        }
        p.End();
    }
}
