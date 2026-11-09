using System.Text;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Formats.Tests;

/// <summary>
/// Own-format names round-trip through the name table, and a document that still uses an old name loads.
/// </summary>
public sealed class NameTableTests : FormatsTest
{
    [Fact]
    public void OwnFormatNamesRoundTrip()
    {
        int types = 0;
        int members = 0;
        foreach (var row in NameTable.Rows)
        {
            bool identity = row.Current == row.Delphi || row.Current.EndsWith("." + row.Delphi, StringComparison.Ordinal);
            Assert.True(identity, row.Kind + " " + row.Delphi + " " + row.Current);
            if (row.Kind == "type") types++;
            else members++;
        }
        Assert.True(types > 0);
        Assert.True(members > 0);
        Assert.Contains(NameTable.Rows, row => row.Kind == "type" && row.Delphi == nameof(TPersistent)
            && row.Current == NameTable.QualifiedName(typeof(TPersistent)));

        var counts = EClass.All.GroupBy(eclass => eclass.ClassName).ToDictionary(group => group.Key, group => group.Count());
        foreach (var eclass in EClass.All)
        {
            Assert.Equal(NameTable.Stored(eclass.Type), eclass.ClassName);
            Assert.Equal(NameTable.Delphi(eclass.Type), eclass.ClassName);
            if (counts[eclass.ClassName] == 1) Assert.Equal(eclass.Type, NameTable.Find(eclass.ClassName));
            else Assert.Throws<InvalidOperationException>(() => NameTable.Find(eclass.ClassName));
        }

        foreach (var cls in FormatSchema.Current.Classes)
        {
            Assert.Equal(cls.Name, NameTable.ResolveStored(cls.Name));
            if (cls.Parent is not null) Assert.Equal(cls.Parent.Name, NameTable.ResolveStored(cls.Parent.Name));
            if (cls.FieldClassName is not null) Assert.Equal(cls.FieldClassName, NameTable.ResolveStored(cls.FieldClassName));
            foreach (var property in cls.Own) Check(property.Codec, property.Name);
        }

        Assert.Equal(nameof(Unlisted), NameTable.Delphi(typeof(Unlisted)));
        Assert.Equal(nameof(Unlisted), NameTable.Stored(typeof(Unlisted)));
    }

    [Fact]
    public void AnOldDocumentNameLoads()
    {
        string table = "kind\tdelphi\tcurrent\n"
            + "type\tOldBox\t" + NameTable.QualifiedName(typeof(OldNameBox)) + "\n"
            + "member\tTSampleRank.oldGold\t" + NameTable.QualifiedName(typeof(TSampleRank)) + ".srGold\n";
        var schema = FormatSchema.Load(Encoding.UTF8.GetBytes(RenamedSchema), "renamed.json");
        var holder = schema.Get("Holder");
        using (NameTable.Install(table))
        {
            Assert.Equal("OldNameBox", schema.Find("OldBox")!.Name);
            Assert.Equal("OldNameBox", schema.Find("OldNameBox")!.Name);
            Assert.True(schema.Get("OldNameBox").InheritsFrom("OldNameBox"));
            Assert.Equal("OldBox", NameTable.Delphi(typeof(OldNameBox)));
            Assert.Equal(nameof(OldNameBox), NameTable.Stored(typeof(OldNameBox)));
            Assert.Equal("oldGold", NameTable.DelphiMember(typeof(TSampleRank), "srGold"));
            Assert.Equal("srGold", NameTable.StoredMember(typeof(TSampleRank), "oldGold"));

            var old = new JsonObject()
                .Add("Rank", new JsonString("oldGold"))
                .Add("Child", new JsonObject().Add("$class", new JsonString("OldBox")));
            var loaded = DocumentBackend.Read(old, holder, schema, "game");
            Assert.Equal(1, ((EnumNode)loaded.Find("Rank")!).Value);
            Assert.Equal("OldBox", ((ObjectNode)loaded.Find("Child")!).ClassName);

            var current = new JsonObject()
                .Add("Rank", new JsonString("srGold"))
                .Add("Child", new JsonObject().Add("$class", new JsonString("OldNameBox")));
            var again = DocumentBackend.Read(current, holder, schema, "game");
            Assert.Equal(1, ((EnumNode)again.Find("Rank")!).Value);
            Assert.Equal(nameof(OldNameBox), ((ObjectNode)again.Find("Child")!).ClassName);

            var live = new ObjectNode(null, null);
            live.Add("Rank", new EnumNode(NameTable.Stored(typeof(TSampleRank)), 1, false, 1));
            live.Add("Child", new ObjectNode(NameTable.Stored(typeof(OldNameBox)), null));
            var written = DocumentBackend.Write(live, holder, schema, "game");
            Assert.Equal("srGold", ((JsonString)written["Rank"]!).Value);
            Assert.Equal(nameof(OldNameBox), ((JsonString)((JsonObject)written["Child"]!)["$class"]!).Value);
        }
    }

    private static void Check(PropertyCodec codec, string path)
    {
        switch (codec)
        {
            case EnumCodec enumerated:
                RoundTripEnum(enumerated.TypeName, path);
                break;
            case SetCodec set:
                RoundTripEnum(set.TypeName, path);
                break;
            case CatalogRecordCodec record:
                Assert.Equal(record.TypeName, NameTable.ResolveStored(record.TypeName));
                var info = RecordCatalog.Find(record.TypeName);
                Assert.NotNull(info);
                foreach (var field in info.Fields)
                {
                    if (field.TypeName is not string element) continue;
                    Assert.Equal(element, NameTable.ResolveStored(element));
                    if (field.ClrType is { IsEnum: true } clr) RoundTripEnum(element, field.Name, clr);
                }
                break;
            case FixedObjectCodec obj:
                Assert.Equal(obj.ClassName, NameTable.ResolveStored(obj.ClassName));
                break;
            case CreatedObjectCodec created when created.Family is string family:
                Assert.Equal(family, NameTable.ResolveStored(family));
                break;
        }
    }

    private static void RoundTripEnum(string typeName, string path, Type? clr = null)
    {
        Assert.Equal(typeName, NameTable.ResolveStored(typeName));
        var info = EnumCatalog.Require(typeName, null, path);
        clr ??= info.Clr;
        foreach (var member in info.ByName.Keys)
        {
            Assert.True(info.TryNumber(member, out long number));
            Assert.Equal(info.ByName[member], number);
            Assert.Equal(member, NameTable.StoredMember(clr, member));
            Assert.Equal(member, NameTable.DelphiMember(clr, member));
        }
    }

    private const string RenamedSchema = """
        {"version": 2, "classes": [
          {"name": "OldNameBox", "id": 1, "engines": ["game"], "properties": []},
          {"name": "Holder", "id": 2, "engines": ["game"], "properties": [
            {"name": "Rank", "id": 1, "codec": "enum", "enum": "TSampleRank", "width": 1, "signed": false},
            {"name": "Child", "id": 2, "codec": "created", "family": "OldNameBox"}
          ]}
        ]}
        """;

    private sealed class Unlisted;

    private sealed class OldNameBox;
}
