using System.Text;
using Aow.Engine;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;

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
        foreach (NameRow row in NameTable.Rows)
        {
            Assert.False(string.IsNullOrEmpty(row.Delphi), row.Current);
            Assert.False(string.IsNullOrEmpty(row.Current), row.Delphi);
            if (row.Kind == "type")
            {
                types++;
            }
            else
            {
                members++;
            }
        }
        Assert.True(types > 0);
        Assert.True(members > 0);
        Assert.Contains(NameTable.Rows, row => row.Kind == "type" && row.Delphi == "TPersistent"
            && row.Current == NameTable.QualifiedName(typeof(Persistent)));

        Dictionary<string, int> counts = EngineClass.All.GroupBy(engineClass => engineClass.StorageKey).ToDictionary(group => group.Key, group => group.Count());
        foreach (EngineClass? engineClass in EngineClass.All)
        {
            Assert.Equal(NameTable.Stored(engineClass.Type), engineClass.StorageKey);
            Assert.Equal(engineClass.StorageKey, NameTable.ResolveStored(NameTable.Delphi(engineClass.Type)));
            if (counts[engineClass.StorageKey] == 1)
            {
                Assert.Equal(engineClass.Type, NameTable.Find(engineClass.StorageKey));
            }
            else
            {
                _ = Assert.Throws<InvalidOperationException>(() => NameTable.Find(engineClass.StorageKey));
            }
        }

        foreach (SchemaClass classValue in FormatSchema.Current.Classes)
        {
            Assert.Equal(classValue.Name, NameTable.ResolveStored(classValue.Name));
            if (classValue.Parent is not null)
            {
                Assert.Equal(classValue.Parent.Name, NameTable.ResolveStored(classValue.Parent.Name));
            }

            if (classValue.FieldClassName is not null)
            {
                Assert.Equal(classValue.FieldClassName, NameTable.ResolveStored(classValue.FieldClassName));
            }

            foreach (SchemaProperty property in classValue.Own)
            {
                Check(property.Codec, property.Name);
            }
        }

        Assert.Equal(nameof(Unlisted), NameTable.Delphi(typeof(Unlisted)));
        Assert.Equal("NameTableTests.Unlisted", NameTable.Stored(typeof(Unlisted)));
    }

    [Fact]
    public void AnOldDocumentNameLoads()
    {
        string table = "kind,delphi,current\n"
            + "type,OldBox," + NameTable.QualifiedName(typeof(OldNameBox)) + "\n"
            + "type,TSampleRank," + NameTable.QualifiedName(typeof(SampleRank)) + "\n"
            + "member,TSampleRank.oldGold," + NameTable.QualifiedName(typeof(SampleRank)) + ".Gold\n";
        FormatSchema schema = FormatSchema.Load(Encoding.UTF8.GetBytes(RenamedSchema), "renamed.json");
        SchemaClass holder = schema.Get("Holder");
        using (NameTable.Install(table))
        {
            Assert.Equal("OldNameBox", schema.Find("OldBox")!.Name);
            Assert.Equal("OldNameBox", schema.Find("OldNameBox")!.Name);
            Assert.True(schema.Get("OldNameBox").InheritsFrom("OldNameBox"));
            Assert.Equal("OldBox", NameTable.Delphi(typeof(OldNameBox)));
            Assert.Equal(nameof(OldNameBox), NameTable.Stored(typeof(OldNameBox)));
            Assert.Equal("oldGold", NameTable.DelphiMember(typeof(SampleRank), "Gold"));
            Assert.Equal("Gold", NameTable.StoredMember(typeof(SampleRank), "oldGold"));
            ObjectNode loaded = DocumentBackend.Read(new JsonObject()
                .Add("Rank", new JsonString("oldGold"))
                .Add("Child", new JsonObject().Add("$class", new JsonString("OldBox"))), classValue: holder, schema: schema, engine: "game");
            Assert.Equal(1, ((EnumNode)loaded.Find("Rank")!).Value);
            Assert.Equal("OldBox", ((ObjectNode)loaded.Find("Child")!).ClassName);
            ObjectNode again = DocumentBackend.Read(new JsonObject()
                .Add("Rank", new JsonString("Gold"))
                .Add("Child", new JsonObject().Add("$class", new JsonString("OldNameBox"))), classValue: holder, schema: schema, engine: "game");
            Assert.Equal(1, ((EnumNode)again.Find("Rank")!).Value);
            Assert.Equal(nameof(OldNameBox), ((ObjectNode)again.Find("Child")!).ClassName);

            ObjectNode live = new(null, null);
            live.Add("Rank", new EnumNode(NameTable.Stored(typeof(SampleRank)), 1, false, 1));
            live.Add("Child", new ObjectNode(NameTable.Stored(typeof(OldNameBox)), null));
            JsonObject written = DocumentBackend.Write(live, holder, schema, "game");
            Assert.Equal("Gold", ((JsonString)written["Rank"]!).Value);
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
                RecordTypeInfo? info = RecordCatalog.Find(record.TypeName);
                Assert.NotNull(info);
                foreach (RecordFieldInfo field in info.Fields)
                {
                    if (field.TypeName is not string element)
                    {
                        continue;
                    }

                    Assert.Equal(element, NameTable.ResolveStored(element));
                    if (field.ColorType is { IsEnum: true } color)
                    {
                        RoundTripEnum(element, field.Name, color);
                    }
                }
                break;
            case FixedObjectCodec objectValue:
                Assert.Equal(objectValue.ClassName, NameTable.ResolveStored(objectValue.ClassName));
                break;
            case CreatedObjectCodec created when created.Family is string family:
                Assert.Equal(family, NameTable.ResolveStored(family));
                break;
            default:
                break;
        }
    }

    private static void RoundTripEnum(string typeName, string path, Type? color = null)
    {
        Assert.Equal(typeName, NameTable.ResolveStored(typeName));
        EnumTypeInfo info = EnumCatalog.Require(typeName, null, path);
        color ??= info.Color;
        foreach (string member in info.ByName.Keys)
        {
            Assert.True(info.TryNumber(member, out long number));
            Assert.Equal(info.ByName[member], number);
            Assert.Equal(member, NameTable.StoredMember(color, member));
            string delphi = NameTable.DelphiMember(color, member);
            Assert.Equal(member, NameTable.StoredMember(color, delphi));
            Assert.True(info.TryNumber(delphi, out long fromDelphi));
            Assert.Equal(number, fromDelphi);
        }
    }

    [Fact]
    public void ADocumentFromBeforeTheRenameLoads()
    {
        FormatSchema schema = FormatSchema.Current;
        SchemaClass bridge = schema.Get("TBridge");
        Assert.Equal(NameTable.ResolveStored("TBridge"), bridge.Name);
        ObjectNode loadedBridge = DocumentBackend.Read(new JsonObject(), bridge, schema, "game");
        Assert.Empty(loadedBridge.Members);

        SchemaClass group = schema.Get("TAIGroup");
        ObjectNode loaded = DocumentBackend.Read(new JsonObject().Add("TargetType", new JsonString("gtsIdle")), classValue: group, schema: schema, engine: "game");
        EnumNode status = Assert.IsType<EnumNode>(loaded.Find("TargetType"));
        ObjectNode current = DocumentBackend.Read(new JsonObject().Add("TargetType", new JsonString("Idle")), group, schema, "game");
        EnumNode again = Assert.IsType<EnumNode>(current.Find("TargetType"));
        Assert.Equal(again.Value, status.Value);
        Assert.Equal(NameTable.ResolveStored("TBridge"), schema.Find("TBridge")!.Name);
    }

    [Fact]
    public void AnOldEngineBattleOrMovedEnumNameLoads()
    {
        FormatSchema schema = FormatSchema.Current;
        Assert.Equal("GameEngine", schema.Find("TAoWEngine")!.Name);
        Assert.Equal("GameEngine", schema.Find("TEngine")!.Name);
        Assert.Equal("GameEngine", schema.Find("THSEngine")!.Name);
        Assert.Equal("Battle", schema.Find("TCombat")!.Name);
        Assert.Equal("FastBattle", schema.Find("TFastCombat")!.Name);
        Assert.Equal("TacticalBattle", schema.Find("TTacticalCombat")!.Name);
        ObjectNode engine = DocumentBackend.Read(new JsonObject(), schema.Find("TAoWEngine")!, schema, "game");
        Assert.Empty(engine.Members);
        ObjectNode battle = DocumentBackend.Read(new JsonObject(), schema.Find("TCombat")!, schema, "game");
        Assert.Empty(battle.Members);

        SchemaClass hero = schema.Get("HeroResource");
        ObjectNode old = DocumentBackend.Read(new JsonObject().Add("Alignment", new JsonString("alGood")), hero, schema, "game");
        ObjectNode current = DocumentBackend.Read(new JsonObject().Add("Alignment", new JsonString("Good")), hero, schema, "game");
        Assert.Equal(((EnumNode)current.Find("Alignment")!).Value, ((EnumNode)old.Find("Alignment")!).Value);
    }

    [Fact]
    public void AnOwnDocumentFromBeforeThisRenameLoads()
    {
        FormatSchema schema = FormatSchema.Current;
        Assert.Equal("EngineObject", schema.Find("TEObject")!.Name);
        Assert.Equal("EngineObject", schema.Find("EObject")!.Name);
        Assert.Equal("Campaign", schema.Find("TAoWCampaign")!.Name);
        Assert.Equal("Campaign", schema.Find("AoWCampaign")!.Name);
        ObjectNode engineObject = DocumentBackend.Read(new JsonObject(), schema.Find("TEObject")!, schema, "game");
        Assert.Empty(engineObject.Members);
        ObjectNode campaign = DocumentBackend.Read(new JsonObject(), schema.Find("AoWCampaign")!, schema, "game");
        Assert.Empty(campaign.Members);
    }

    private const string RenamedSchema = """
        {"version": 2, "classes": [
          {"name": "OldNameBox", "id": 1, "engines": ["game"], "properties": []},
          {"name": "Holder", "id": 2, "engines": ["game"], "properties": [
            {"name": "Rank", "id": 1, "codec": "enum", "enum": "SampleRank", "width": 1, "signed": false},
            {"name": "Child", "id": 2, "codec": "created", "family": "OldNameBox"}
          ]}
        ]}
        """;

    private sealed class Unlisted;

    private sealed class OldNameBox;
}
