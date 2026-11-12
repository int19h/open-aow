using System.Buffers.Binary;
using System.IO.Compression;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;

namespace Aow.Convert;

public static partial class Fixtures
{
    public static IEnumerable<Case> All()
    {
        // Hero and item lists, empty and nonempty.
        yield return new("lists", "empty hero list", () =>
            RoundTrip([1, 0, 0, 0], "hero-list", "hero-list"));
        yield return new("lists", "nonempty hero list", () => RoundTrip(Game(HeroList), "hero-list", "hero-list"));
        yield return new("lists", "empty item list", () => RoundTrip([1, 0, 0, 0], "item-list", "item-list"));
        yield return new("lists", "nonempty item list", NonemptyItemList);

        // Campaign saves with an exact nested CFS map.
        yield return new("campaign", "nonempty campaign save keeps the nested compressed bytes", CampaignSave);
        yield return new("campaign", "editing the nested map recompresses it and rebuilds its size", CampaignSaveEdit);
        yield return new("campaign", "absent map stays absent", () => RoundTrip(Game(Campaign), "campaign", "campaign",
            d => Root(d).Find("Map") is null ? null : "an absent map gained a member"));

        // CFS levels and raw HSM.
        foreach (int level in new[] { 0, 1, 2, 3 })
            yield return new("containers", $"CFS level {level}", () =>
                RoundTrip(Cfs.Wrap(Payload(Game(Map)), level), "world-hsm", "map",
                    d => Object(d["$container"])["level"] is JsonInteger l && l.Value == level ? null : "level lost"));
        yield return new("containers", "raw HSM without the CFS prefix", () =>
            RoundTrip(Payload(Game(Map)), "world-hsm", "map",
                d => Object(d["$container"])["compression"] is JsonString { Value: "none" } ? null : "raw HSM became CFS"));
        yield return new("containers", "raw HSM and CFS level 0 stay distinct", () =>
        {
            var raw = Import(Payload(Game(Map)), "world-hsm", "map");
            var level0 = Import(Cfs.Wrap(Payload(Game(Map)), 0), "world-hsm", "map");
            return GameFiles.Write(raw).AsSpan().SequenceEqual(GameFiles.Write(level0)) ? Fail("the documents are equal") : Pass("distinct containers");
        });
        yield return new("containers", "HSM header extension", () =>
        {
            var payload = Payload(Game(Map));
            var extended = Concat(I32(20), payload[4..16], [1, 2, 3, 4], payload[16..]);
            return RoundTrip(Cfs.Wrap(extended, 2), "world-hsm", "map");
        });
        yield return new("containers", "HSS header extension", () =>
        {
            var file = Game(ScenarioSet);
            var body = Concat(I32(56), file[4..52], [9, 8, 7, 6], file[52..^4]);
            return RoundTrip(Concat(body, I32((int)Crc32.Compute(body))), "scenario-set", "scenario-set");
        });
        yield return new("containers", "HSS checksum failure", () =>
        {
            var file = Game(ScenarioSet);
            file[^1] ^= 0x5A;
            return Expect(Codes.Checksum, () => Import(file, "scenario-set", "scenario-set"), Located);
        });
        yield return new("containers", "a CFS level above 3", () =>
        {
            var file = Game(Map);
            file[4] = 9;
            return Expect(Codes.Compression, () => Import(file, "world-hsm", "map"), Located);
        });

        // Truncation at codec boundaries.
        foreach (var (name, cut) in new (string, Func<int, int>)[]
        {
            ("inside the HSM header", n => 10), ("after the header", n => 16), ("inside the outer table", n => 17),
            ("inside the root table", n => 20), ("a third into the payload", n => n / 3), ("one byte short", n => n - 1),
        })
            yield return new("truncation", name, () =>
            {
                var payload = Payload(Game(Map));
                return ExpectAny(() => Import(payload[..cut(payload.Length)], "world-hsm", "map"));
            });
        yield return new("truncation", "a hero list cut inside a string", () =>
        {
            var file = Game(HeroList);
            return ExpectAny(() => Import(file[..12], "hero-list", "hero-list"));
        });

        // Unknown class, property and members.
        yield return new("unknown", "unknown stored class id", () =>
        {
            // A hero list whose only child names class id 0x7ffffff0.
            var image = HeroListImage([1, 1, 0], Concat([1, 0, 0], I32(0x7FFFFFF0), [0]));
            return Expect(Codes.UnknownClass, () => Import(image, "hero-list", "hero-list"), Located);
        });
        yield return new("unknown", "unknown property id", () =>
            Expect(Codes.UnknownProperty, () => Import(HeroListImage([1, 99, 0], I32(5)), "hero-list", "hero-list"), Located));
        yield return new("unknown", "unknown JSON member", () =>
        {
            var d = Import(Game(HeroList), "hero-list", "hero-list");
            Root(d).Add("Bogus", new JsonInteger(1));
            return Expect(Codes.UnknownMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "unknown $class in JSON", () =>
        {
            var d = MapDocument();
            Set(Root(d), "$class", new JsonString("TNoSuchMap"));
            return Expect(Codes.UnknownClass, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "reserved $mods in version 1", () =>
        {
            var d = Import(Game(HeroList), "hero-list", "hero-list");
            d.Members.Insert(4, new JsonMember("$mods", new JsonArray()));
            return Expect(Codes.ReservedMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "unknown reserved member", () =>
        {
            var d = Import(Game(HeroList), "hero-list", "hero-list");
            Root(d).Members.Insert(0, new JsonMember("$colour", new JsonInteger(1)));
            return Expect(Codes.ReservedMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });

        // Presence: present defaults, absent values, stored nulls.
        yield return new("presence", "a present default stays present", () =>
        {
            var d = MapDocument();
            Set(Root(d), "Day", new JsonInteger(0));
            return OwnRoundTrip(d, (n, _) => Root(n)["Day"] is JsonInteger { Value: 0 } ? null : "the present zero was dropped");
        });
        yield return new("presence", "explicit false normalizes to absence", () =>
        {
            var d = MapDocument();
            Set(Root(d), "FogEnabled", new JsonBoolean(false));
            var n = Import(GameFiles.Export(d).Bytes, "world-hsm", "map");
            return Root(n).Find("FogEnabled") is null ? Pass("absent after normalization") : Fail("false survived");
        });
        yield return new("presence", "a stored null object", () =>
        {
            var d = Import(Game(Campaign), "campaign", "campaign");
            var transfer = Object(Root(d)["TransferSettings"]);
            Set(transfer, "Leader", new JsonNull());
            return OwnRoundTrip(d, (n, _) => Object(Root(n)["TransferSettings"])["Leader"] is JsonNull ? null : "the stored null was lost");
        });
        yield return new("presence", "an empty fixed object stays present", () =>
            RoundTrip(Game(HeroList), "hero-list", "hero-list"));

        // Strings.
        yield return new("strings", "long prefix for a short string", () =>
        {
            var image = HeroListImage([1, 20, 0], Concat([255], I32(3), "abc"u8.ToArray()));
            return RoundTrip(image, "hero-list", "hero-list",
                d => d.ToString() is not null && GameFiles.Write(d).AsSpan().IndexOf("value-prefixes"u8) >= 0 ? null : "no value-prefixes part");
        });
        yield return new("strings", "all 256 code page bytes", () =>
        {
            var bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            var image = HeroListImage([1, 20, 0], Concat([255], I32(256), bytes));
            return RoundTrip(image, "hero-list", "hero-list",
                d => Root(d)["LibraryName"] is JsonString s && s.Value.Length == 256 && s.Value[0x81] == '\u0081' && s.Value[0x80] == '\u20ac'
                    ? null : "the code page mapping is not the frozen table");
        });
        yield return new("strings", "an unmapped character is refused with its location", () =>
        {
            var d = Import(Game(HeroList), "hero-list", "hero-list");
            Set(Root(d), "LibraryName", new JsonString("\u4e00"));
            return Expect(Codes.Text, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        foreach (var (name, value) in new (string, Func<JsonValue>)[]
        {
            ("empty block", () => new JsonArray()),
            ("one empty line", () => new JsonArray().Add(new JsonString(""))),
            ("NUL in the block", () => new JsonObject().Add("$text", new JsonString("a\0b\r\n"))),
            ("LF line ends", () => new JsonObject().Add("$text", new JsonString("a\nb\n"))),
            ("CR line ends", () => new JsonObject().Add("$text", new JsonString("a\rb\r"))),
            ("no final line end", () => new JsonObject().Add("$text", new JsonString("a\r\nb"))),
        })
            yield return new("strings", "rwStrings " + name, () =>
            {
                var d = MapDocument();
                Set(Root(d), "Objectives", value());
                return OwnRoundTrip(d);
            });
        yield return new("strings", "rwStringsEx signed object values", () =>
        {
            var d = Import(Game(ScenarioSet), "scenario-set", "scenario-set");
            var settings = Object(Root(d)["ResourceEditSettings"]);
            Set(settings, "Editors", new JsonObject()
                .Add("lines", new JsonArray().Add(new JsonString("a")).Add(new JsonString("b")))
                .Add("objects", new JsonArray().Add(new JsonInteger(-1)).Add(new JsonInteger(int.MinValue))));
            return OwnRoundTrip(d);
        });
        yield return new("strings", "rwStringsEx object count mismatch", () =>
        {
            var d = Import(Game(ScenarioSet), "scenario-set", "scenario-set");
            Set(Object(Root(d)["ResourceEditSettings"]), "Editors", new JsonObject()
                .Add("lines", new JsonArray().Add(new JsonString("a")))
                .Add("objects", new JsonArray().Add(new JsonInteger(1)).Add(new JsonInteger(2))));
            return Expect(Codes.Corrupt, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });

        // Floating point bits. From version 5 a single that a number carries exactly is that number,
        // and only negative zero, the infinities and NaN keep $bits.
        foreach (var (bits, number) in new[] { ("00000080", null), ("0000807f", null), ("000080ff", null), ("0100c07f", null), ("ffffffff", null), ("0000803f", "1"), ("cdcccc3d", "0.1"), ("acc52737", "1e-5") })
            yield return new("floats", "$bits " + bits, () =>
            {
                var d = MapDocument();
                var player = Object(Arr(Object(Root(d)["Players"])["Children"]).Items[1]);
                Set(Object(player["AIControl"]), "DefenseStrengthMultiplier", new JsonObject().Add("$bits", new JsonString(bits)));
                return OwnRoundTrip(d, (n, _) =>
                {
                    var stored = Object(Object(Arr(Object(Root(n)["Players"])["Children"]).Items[1])["AIControl"])["DefenseStrengthMultiplier"];
                    string? text = stored switch
                    {
                        JsonInteger integer => integer.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        JsonNumber decimalText => decimalText.Text,
                        _ => null,
                    };
                    if (number is null)
                        return stored is JsonObject objectValue && objectValue["$bits"] is JsonString { } s && s.Value == bits ? null : "the bits changed";
                    return text == number ? null : "the number is " + (text ?? stored?.KindName) + ", not " + number;
                });
            });

        // Slots, families and dimensions.
        yield return new("identity", "deleting a child slot keeps the other slots", () =>
        {
            var d = MapDocument();
            var children = Arr(Object(Root(d)["Players"])["Children"]);
            int count = children.Items.Count;
            children.Items.RemoveAt(1);
            return OwnRoundTrip(d, (n, _) =>
            {
                var slots = Arr(Object(Root(n)["Players"])["Children"]).Items.Select(c => ((JsonInteger)Object(c)["$slot"]!).Value).ToList();
                return count >= 3 && !slots.Contains(1) && slots.Contains(count - 1) ? null : "slots were renumbered: " + string.Join(",", slots);
            });
        });
        yield return new("identity", "duplicate child slot", () =>
        {
            var d = MapDocument();
            var children = Arr(Object(Root(d)["Players"])["Children"]);
            Set(Object(children.Items[1]), "$slot", new JsonInteger(0));
            return Expect(Codes.Slot, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "incompatible class family", () =>
        {
            var d = Import(Game(Campaign), "campaign", "campaign");
            Set(Object(Root(d)["TransferSettings"]), "Leader", new JsonObject().Add("$class", new JsonString("TCity")));
            return Expect(Codes.ClassFamily, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "a field row of the wrong length", () =>
        {
            var d = MapDocument();
            Arr(FirstFields(d).Items[3]).Items.RemoveAt(0);
            return Expect(Codes.Dimensions, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "a missing field row", () =>
        {
            var d = MapDocument();
            FirstFields(d).Items.RemoveAt(0);
            return Expect(Codes.Dimensions, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "editing one field rewrites one row line", () =>
        {
            var d = MapDocument();
            var before = MapText();
            var row = Arr(FirstFields(d).Items[5]);
            Set(Object(row.Items[7]), "FieldFlags", new JsonInteger(7));
            var after = GameFiles.Write(Import(GameFiles.Export(Parse(GameFiles.Write(d))).Bytes, "world-hsm", "map"));
            var a = System.Text.Encoding.UTF8.GetString(before).Split('\n');
            var b = System.Text.Encoding.UTF8.GetString(after).Split('\n');
            int changed = a.Length == b.Length ? a.Zip(b).Count(p => p.First != p.Second) : -1;
            return changed == 1 ? Pass("one line changed") : Fail($"{changed} lines changed");
        });

        // Layout residue: order, gaps, invalidation.
        yield return new("layout", "long table entries and a gap after a Boolean", () =>
        {
            // THeroLibrary: LibraryName (20) as a long entry, a Boolean-free gap at the end of data
            // is impossible, so the gap sits before the first value.
            var image = HeroListImage(Concat([0x80], I32(1), I32(20), I32(2)), Concat([0xAA, 0xBB], [3], "abc"u8.ToArray()));
            return RoundTrip(image, "hero-list", "hero-list",
                d => GameFiles.Write(d).AsSpan().IndexOf("\"gaps\""u8) >= 0 ? null : "no gap part");
        });
        yield return new("layout", "a noncanonical table order", () =>
        {
            // NextLibraryID (21) is stored before LibraryName (20) in the table and the data.
            var image = HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray()));
            return RoundTrip(image, "hero-list", "hero-list",
                d => GameFiles.Write(d).AsSpan().IndexOf("table-form-order"u8) >= 0 ? null : "no table part");
        });
        yield return new("layout", "an edit invalidates the layout and export rebuilds it", () =>
        {
            var image = HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray()));
            var d = Import(image, "hero-list", "hero-list");
            Set(Root(d), "LibraryName", new JsonString("longer name"));
            var exported = GameFiles.Export(Parse(GameFiles.Write(d)));
            var n = Import(exported.Bytes, "hero-list", "hero-list");
            return exported.Invalidated.Count == 1 && GameFiles.Write(n).AsSpan().IndexOf("$layout"u8) < 0
                ? Pass("invalidated and rebuilt canonically") : Fail($"{exported.Invalidated.Count} invalidations");
        });
        yield return new("layout", "--exact-layout refuses a stale layout", () =>
        {
            var image = HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray()));
            var d = Import(image, "hero-list", "hero-list");
            Root(d).Remove("NextLibraryID");
            return Expect(Codes.Layout, () => GameFiles.Export(Parse(GameFiles.Write(d)), new ExportOptions { ExactLayout = true }), Located);
        });
        yield return new("layout", "a part that does not fit is an error, not a cleanup", () =>
        {
            var image = HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray()));
            var d = Import(image, "hero-list", "hero-list");
            var root = Root(d);
            var layout = Object(root["$layout"]);
            var entries = Arr(Object(Arr(layout["parts"]).Items[0])["entries"]);
            Arr(entries.Items[1]).Items[1] = new JsonInteger(2);
            return Expect(Codes.Layout, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("layout", "Tutorial property 57, a zero-size alias", () => RoundTrip(Game(Tutorial), "world-hsm", "map",
            d => Root(d)["LegacyProperty57"] is JsonBoolean { Value: true } ? null : "property 57 is missing"));

        // Limits and gzip.
        yield return new("limits", "depth limit", () => Expect(Codes.Limit,
            () => GameFiles.Import(Game(Map), FindProfile("world-hsm"), "map", null, new FormatLimits { Depth = 4 }), Located));
        yield return new("limits", "node limit", () => Expect(Codes.Limit,
            () => GameFiles.Import(Game(Map), FindProfile("world-hsm"), "map", null, new FormatLimits { Nodes = 1000 }), Located));
        yield return new("limits", "collection limit", () => Expect(Codes.Limit,
            () => GameFiles.Import(Game(Map), FindProfile("world-hsm"), "map", null, new FormatLimits { CollectionItems = 100 }), Located));
        yield return new("limits", "string limit", () => Expect(Codes.Limit,
            () => GameFiles.Import(Game(Map), FindProfile("world-hsm"), "map", null, new FormatLimits { StringBytes = 10 }), Located));
        yield return new("limits", "decompressed size limit", () => Expect(Codes.Compression,
            () => GameFiles.Import(Game(Map), FindProfile("world-hsm"), "map", null, new FormatLimits { DecompressedBytes = 50_000 }), Located));
        yield return new("limits", "JSON depth limit", () => Expect(Codes.Limit,
            () => GameFiles.Read(GameFiles.Write(MapDocument()), null, new FormatLimits { Depth = 5 }), Located));
        yield return new("gzip", "a gzip own document reads as own JSON", () =>
        {
            var text = GameFiles.Write(Import(Game(HeroList), "hero-list", "hero-list"));
            var c = GameFiles.Classify(Gzip(text), ".ahl");
            return c.Format == DetectedFormat.GzipOwnJson ? Pass("gzip own JSON") : Fail(c.Format.ToString());
        });
        yield return new("gzip", "bounded gzip", () =>
        {
            var bomb = Gzip(new byte[3_000_000]);
            return Expect(Codes.Limit, () => GameFiles.Read(bomb, null, new FormatLimits { DecompressedBytes = 1_000_000 }), Located);
        });
        yield return new("gzip", "a damaged own document never falls back to binary", () =>
        {
            var c = GameFiles.Classify("{\"$format\": \"OpenAoW\", \"$version\": 1,"u8.ToArray(), ".hsm");
            return c.Format == DetectedFormat.MalformedOwn ? Pass("malformed own input") : Fail(c.Format.ToString());
        });

        // JSON diagnostics.
        yield return new("diagnostics", "duplicate JSON member with both locations", () => Expect(Codes.Duplicate,
            () => GameFiles.Read("{\n  \"$format\": \"OpenAoW\",\n  \"$format\": \"OpenAoW\"\n}\n"u8.ToArray(), "dup.ahl"),
            d => d.Span.Line == 3 && d.Span.Column == 3 && d.Message.Contains("2:3", StringComparison.Ordinal) ? null : "wrong locations"));
        yield return new("diagnostics", "a valid-JSON semantic error points at the value", () =>
        {
            var d = Import(Game(HeroList), "hero-list", "hero-list");
            Set(Root(d), "NextLibraryID", new JsonString("seven"));
            var text = GameFiles.Write(d);
            int line = System.Text.Encoding.UTF8.GetString(text).Split('\n').ToList().FindIndex(l => l.Contains("\"seven\"", StringComparison.Ordinal)) + 1;
            return Expect(Codes.Type, () => GameFiles.Export(Parse(text)),
                diag => diag.Span.Line == line && diag.Path == "root.NextLibraryID" ? null : $"expected line {line}");
        });
        yield return new("diagnostics", "a binary error gives the offset and the profile", () =>
            Expect(Codes.UnknownProperty, () => GameFiles.Import(HeroListImage([1, 99, 0], I32(5)), FindProfile("hero-list"), "hero-list", "bad.ahl"),
                d => d.Offset is not null && d.Profile == "hero-list" && !d.Span.IsKnown ? null : "not a binary location"));
        yield return new("diagnostics", "trailing commas and comments fail", () => ExpectAny(
            () => GameFiles.Read("{\"$format\": \"OpenAoW\", }"u8.ToArray()),
            () => GameFiles.Read("{\"$format\": \"OpenAoW\" // c\n}"u8.ToArray())));
        yield return new("diagnostics", "a newer version fails with both numbers", () => Expect(Codes.Version,
            () => GameFiles.Read("{\"$format\": \"OpenAoW\", \"$version\": 7}"u8.ToArray()),
            d => d.Message.Contains("version 7", StringComparison.Ordinal)
                && d.Message.Contains("supported version " + OwnFormat.CurrentVersion, StringComparison.Ordinal)
                ? null : "the numbers are missing"));

        // Migration chains.
        yield return new("migrations", "a historical chain migrates through every step", MigrationChainCase);

        // Mapping anchors: document edits reach independently located bytes.
        yield return new("anchors", "seed, day, string, slot and coordinate edits reach their stored ids", Anchors);
        yield return new("legacy", "Valley01 sprite keeps its legacy shape", () =>
            RoundTrip(Game("Scenario/Campaign/Valley01.CSM"), "world-hsm", "campaign-map",
                d => GameFiles.Write(d).AsSpan().IndexOf("LegacyProperty12"u8) >= 0 ? null : "the legacy shape is missing"));

        // Case mappings of the production and diplomatic kinds.
        foreach (var kindCase in KindCases()) yield return kindCase;
    }

    private static JsonArray FirstFields(JsonObject document)
    {
        var root = Root(document);
        foreach (var child in Arr(root["Children"]).Items)
            if (Object(child)["$class"] is JsonString { Value: "MapContainer" })
                return Arr(Object(Arr(Object(child)["Children"]).Items[0])["Fields"]);
        throw new InvalidOperationException("No map container.");
    }

    private static Outcome ExpectAny(params Action[] actions)
    {
        foreach (var action in actions)
        {
            try
            {
                action();
                return Fail("the conversion succeeded");
            }
            catch (ConversionException error)
            {
                if (Located(error.Diagnostic) is string problem) return Fail(problem);
            }
        }
        return Pass("every input failed with a located diagnostic");
    }

    private static byte[] Gzip(byte[] data)
    {
        using var output = new MemoryStream();
        using (var gz = new GZipStream(output, CompressionLevel.Fastest, true)) gz.Write(data);
        return output.ToArray();
    }

    private static Outcome NonemptyItemList()
    {
        // Items of an oracle save, copied into a library as its children.
        var save = MapDocument();
        var items = Arr(Object(Root(save)["ItemControl"])["Items"]);
        if (items.Items.Count == 0) return Fail("the map has no items");
        var children = new JsonArray();
        for (int k = 0; k < items.Items.Count; k++)
        {
            var item = Copy(Envelope("hero-list", "hero-list", Object(items.Items[k])));
            var objectValue = Object(item["root"]);
            Set(objectValue, "$slot", new JsonInteger(k));
            children.Add(objectValue);
        }
        var library = new JsonObject().Add("Name", new JsonString("Fixture items")).Add("LastID", new JsonInteger(items.Items.Count)).Add("Children", children);
        return OwnRoundTrip(Envelope("item-list", "item-list", library));
    }

    private static JsonObject CampaignWithMap(out byte[] mapFile)
    {
        mapFile = Game(Map);
        var campaign = Import(Game(Campaign), "campaign", "campaign");
        var map = Import(mapFile, "world-hsm", "map");
        var nested = new JsonObject().Add("$container", map["$container"]!).Add("root", map["root"]!);
        var layout = new JsonObject()
            .Add("codec", new JsonString(LayoutResidue.Codec))
            .Add("fingerprint", new JsonString(LayoutResidue.Fingerprint(nested, "world-hsm")))
            .Add("parts", new JsonArray().Add(new JsonObject()
                .Add("category", new JsonString(LayoutResidue.NestedCompression))
                .Add("bytes", new JsonString(System.Convert.ToHexStringLower(mapFile)))));
        nested.Members.Insert(1, new JsonMember("$layout", layout));
        Root(campaign).Members.Insert(1, new JsonMember("Map", nested));
        Set(campaign, "$kind", new JsonString("campaign-save"));
        return campaign;
    }

    private static Outcome CampaignSave()
    {
        var campaign = CampaignWithMap(out var mapFile);
        var exported = GameFiles.Export(Parse(GameFiles.Write(campaign)));
        if (exported.Bytes.AsSpan().IndexOf(mapFile) < 0) return Fail("the exact compressed map bytes are not in the export");
        var result = RoundTrip(exported.Bytes, "campaign", "campaign-save",
            d => GameFiles.Write(d).AsSpan().IndexOf("nested-compression"u8) >= 0 ? null : "no nested compression part");
        return result.Matched ? Pass("exact nested CFS bytes, " + result.Detail, result.OwnDocuments) : result;
    }

    private static Outcome CampaignSaveEdit()
    {
        var campaign = CampaignWithMap(out var mapFile);
        var map = Object(Root(campaign)["Map"]);
        Set(Object(map["root"]), "Day", new JsonInteger(42));
        var exported = GameFiles.Export(Parse(GameFiles.Write(campaign)));
        if (exported.Invalidated.Count == 0) return Fail("the stale nested bytes were not reported");
        if (exported.Bytes.AsSpan().IndexOf(mapFile) >= 0) return Fail("the stale nested bytes were reused");
        var again = Import(exported.Bytes, "campaign", "campaign-save");
        var day = Object(Object(Root(again)["Map"])["root"])["Day"];
        if (day is not JsonInteger { Value: 42 }) return Fail("the edit was lost");
        return RoundTrip(exported.Bytes, "campaign", "campaign-save");
    }

    /// <summary>A test chain 1 to 3: a rename, then a new member with an explicit default.</summary>
    internal static MigrationChain TestChain() => new(3,
    [
        new Migration(1, "test-1-rename-LibraryTitle", d =>
        {
            var root = Object(d["root"]);
            var m = root.Find("LibraryTitle");
            if (m is not null)
            {
                int at = root.Members.IndexOf(m);
                root.Members[at] = new JsonMember("LibraryName", m.Value) { NameSpan = m.NameSpan };
            }
            return d;
        }),
        new Migration(2, "test-2-drop-Obsolete", d =>
        {
            Object(d["root"]).Remove("Obsolete");
            return d;
        }),
    ]);

    private static Outcome MigrationChainCase()
    {
        var historical = "{\n  \"$format\": \"OpenAoW\",\n  \"$version\": 1,\n  \"$kind\": \"hero-list\",\n  \"$container\": {\n    \"profile\": \"hero-list\",\n    \"compression\": \"none\"\n  },\n  \"root\": {\n    \"LibraryTitle\": \"Old\",\n    \"Obsolete\": 5\n  }\n}\n"u8.ToArray();
        var chain = TestChain();
        var read = GameFiles.Read(historical, "old.ahl", null, chain);
        if (read.Migrations.Count != 2 || read.DeclaredVersion != 1) return Fail("the chain did not run both steps");
        var root = Object(read.Document["root"]);
        if (root["LibraryName"] is not JsonString { Value: "Old" } || root.Find("Obsolete") is not null) return Fail("the transforms did not apply");
        if (read.Document["$version"] is not JsonInteger { Value: 3, Origin: "test-2-drop-Obsolete" }) return Fail("the version provenance is missing");
        return Pass("migrated 1 to 3 through " + string.Join(", ", read.Migrations));
    }

    /// <summary>
    /// Mapping anchors (own-formats.md 5.1): edits in the document reach the stored ids that an
    /// independent table walk in this method finds, not the codec under test.
    /// </summary>
    private static Outcome Anchors()
    {
        var d = MapDocument();
        var root = Root(d);
        Set(root, "DefendersSeed", new JsonInteger(0x1234567));
        Set(root, "Day", new JsonInteger(13));
        Set(root, "Author", new JsonString("Anchor"));
        var payload = Payload(GameFiles.Export(Parse(GameFiles.Write(d))).Bytes);
        var top = Walk(payload, BinaryPrimitives.ReadInt32LittleEndian(payload));
        var props = Walk(payload, top[0].Start);
        int Int32(int id) => BinaryPrimitives.ReadInt32LittleEndian(payload.AsSpan(props[id].Start, 4));
        if (Int32(28) != 0x1234567) return Fail("DefendersSeed did not reach id 28");
        if (Int32(19) != 13) return Fail("Day did not reach id 19");
        if (props[7].End - props[7].Start != 7 || payload[props[7].Start] != 6) return Fail("Author did not reach id 7");
        var players = Walk(payload, props[21].Start);
        var children = Walk(payload, players[1].Start);
        if (!children.ContainsKey(0) || !children.ContainsKey(1)) return Fail("player slots 0 and 1 are not child ids 0 and 1");
        return Pass("ids 28, 19, 7 and the child slots hold the edited values");
    }

    /// <summary>An independent table walk: id to the byte range of its value.</summary>
    private static Dictionary<int, (int Start, int End)> Walk(byte[] data, int at)
    {
        int shorts = data[at++], longs = 0;
        if ((shorts & 0x80) != 0)
        {
            shorts &= 0x7F;
            longs = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at));
            at += 4;
        }
        var entries = new List<(int Id, int Offset)>();
        for (int i = 0; i < shorts; i++, at += 2) entries.Add((data[at], data[at + 1]));
        for (int i = 0; i < longs; i++, at += 8)
            entries.Add((BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at)), BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at + 4))));
        var offsets = entries.Select(e => e.Offset).Distinct().Order().ToList();
        var result = new Dictionary<int, (int, int)>();
        foreach (var (id, offset) in entries)
        {
            int k = offsets.IndexOf(offset);
            int end = k + 1 < offsets.Count ? at + offsets[k + 1] : -1;
            result[id] = (at + offset, end);
        }
        return result;
    }
}
