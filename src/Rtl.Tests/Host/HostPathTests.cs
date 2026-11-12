using Aow.Rtl.Host;
using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

// A host path keeps its chars after the strings pass, also those outside the code page 1252
// table (strings.md 2.5 and section 4, "Host paths outside the table").
public sealed class HostPathTests
{
    // U+0141, U+017A, U+03A9 and U+4E2D have no byte in the table. "U+0141.sav" and "U+03A9.sav" both encode to "?.sav".
    private static readonly string[] s_names = ["\u0141\u00f3d\u017a.sav", "\u0141.sav", "\u03a9.sav", "\u4e2d.sav", "plain.sav"];

    [Fact]
    public void NamesOutsideTheTableAreFoundOpenedCasedAndToldApart()
    {
        foreach (string name in s_names) Assert.True(name == "plain.sav" || !name.All(Cp1252.IsMapped), name);
        Assert.Equal(Cp1252.Encode("\u0141.sav"), Cp1252.Encode("\u03a9.sav"));
        using var directory = new ProbeDirectory();
        var saved = Files;
        try
        {
            Files = new PhysicalFileSystem();
            for (int i = 0; i < s_names.Length; i++) File.WriteAllBytes(Path.Combine(directory.Root, s_names[i]), [(byte)i]);

            var found = new List<string>();
            foreach (var entry in Files.FindEntries("*.sav", attributes: faAnyFile))
                found.Add(entry.Name);
            Assert.Equal(s_names.Order(StringComparer.Ordinal), found.Order(StringComparer.Ordinal));

            foreach (string name in found)
            {
                using var stream = OpenFile(name, fmOpenRead);
                byte[] content = new byte[1];
                Assert.Equal(1, stream.Read(content));
                Assert.Equal(Array.IndexOf(s_names, name), content[0]);
            }
        }
        finally { Files = saved; }

        Assert.Equal("\u0141\u00d3D\u017a.SAV", AnsiUpperCase("\u0141\u00f3d\u017a.sav"));
        Assert.Equal("\u0141\u00f3d\u017a.sav", AnsiLowerCase("\u0141\u00d3D\u017a.SAV"));
        Assert.Equal("\u03a9.SAV", AnsiUpperCase("\u03a9.sav"));
        Assert.Equal("\u4e2d.SAV", UpperCase("\u4e2d.sav"));
        for (int i = 0; i < s_names.Length; i++)
            for (int j = 0; j < s_names.Length; j++)
            {
                Assert.Equal(i == j, AnsiCompareText(s_names[i], s_names[j]) == 0);
                Assert.Equal(i == j, AnsiCompareStr(s_names[i], s_names[j]) == 0);
                Assert.Equal(i == j, CompareText(s_names[i], s_names[j]) == 0);
                Assert.Equal(Math.Sign(AnsiCompareText(s_names[i], s_names[j])), -Math.Sign(AnsiCompareText(s_names[j], s_names[i])));
            }

        var list = new StringList { Sorted = true, Duplicates = Duplicates.Ignore };
        foreach (string name in s_names.Concat(Enumerable.Reverse(s_names))) list.Add(name);
        Assert.Equal(s_names.Length, list.Count);
        foreach (string name in s_names) Assert.Equal(name, list[list.IndexOf(name)]);
    }
}
