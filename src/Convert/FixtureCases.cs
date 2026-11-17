using System.Buffers.Binary;
using System.IO.Compression;
using Aow.Formats;
using Aow.Formats.Json;
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
        {
            yield return new("containers", $"CFS level {level}", () =>
                RoundTrip(Cfs.Wrap(Payload(Game(Map)), level), "world-hsm", "map",
                    d => Object(d["$container"])["level"] is JsonInteger l && l.Value == level ? null : "level lost"));
        }

        yield return new("containers", "raw HSM without the CFS prefix", () =>
            RoundTrip(Payload(Game(Map)), "world-hsm", "map",
                d => Object(d["$container"])["compression"] is JsonString { Value: "none" } ? null : "raw HSM became CFS"));
        yield return new("containers", "raw HSM and CFS level 0 stay distinct", () =>
        {
            return GameFiles.Write(Import(Payload(Game(Map)), "world-hsm", "map")).AsSpan().SequenceEqual(GameFiles.Write(Import(Cfs.Wrap(Payload(Game(Map)), 0), "world-hsm", "map"))) ? Fail("the documents are equal") : Pass("distinct containers");
        });
        yield return new("containers", "HSM header extension", () =>
        {
            byte[] payload = Payload(Game(Map));
            byte[] extended = Concat(I32(20), payload[4..16], [1, 2, 3, 4], payload[16..]);
            return RoundTrip(Cfs.Wrap(extended, 2), "world-hsm", "map");
        });
        yield return new("containers", "HSS header extension", () =>
        {
            byte[] file = Game(ScenarioSet);
            byte[] body = Concat(I32(56), file[4..52], [9, 8, 7, 6], file[52..^4]);
            return RoundTrip(Concat(body, I32((int)Crc32.Compute(body))), "scenario-set", "scenario-set");
        });
        yield return new("containers", "HSS checksum failure", () =>
        {
            byte[] file = Game(ScenarioSet);
            file[^1] ^= 0x5A;
            return Expect(Codes.Checksum, () => Import(file, "scenario-set", "scenario-set"), Located);
        });
        yield return new("containers", "a CFS level above 3", () =>
        {
            byte[] file = Game(Map);
            file[4] = 9;
            return Expect(Codes.Compression, () => Import(file, "world-hsm", "map"), Located);
        });

        // Truncation at codec boundaries.
        foreach ((string? name, Func<int, int>? cut) in new (string, Func<int, int>)[]
        {
            ("inside the HSM header", n => 10), ("after the header", n => 16), ("inside the outer table", n => 17),
            ("inside the root table", n => 20), ("a third into the payload", n => n / 3), ("one byte short", n => n - 1),
        })
        {
            yield return new("truncation", name, () =>
            {
                byte[] payload = Payload(Game(Map));
                return ExpectAny(() => Import(payload[..cut(payload.Length)], "world-hsm", "map"));
            });
        }

        yield return new("truncation", "a hero list cut inside a string", () =>
        {
            byte[] file = Game(HeroList);
            return ExpectAny(() => Import(file[..12], "hero-list", "hero-list"));
        });

        // Unknown class, property and members.
        yield return new("unknown", "unknown stored class id", () =>
        {
            // A hero list whose only child names class id 0x7ffffff0.
            byte[] image = HeroListImage([1, 1, 0], Concat([1, 0, 0], I32(0x7FFFFFF0), [0]));
            return Expect(Codes.UnknownClass, () => Import(image, "hero-list", "hero-list"), Located);
        });
        yield return new("unknown", "unknown property id", () =>
            Expect(Codes.UnknownProperty, () => Import(HeroListImage([1, 99, 0], I32(5)), "hero-list", "hero-list"), Located));
        yield return new("unknown", "unknown JSON member", () =>
        {
            JsonObject d = Import(Game(HeroList), "hero-list", "hero-list");
            _ = Root(d).Add("Bogus", new JsonInteger(1));
            return Expect(Codes.UnknownMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "unknown $class in JSON", () =>
        {
            JsonObject d = MapDocument();
            Set(Root(d), "$class", new JsonString("TNoSuchMap"));
            return Expect(Codes.UnknownClass, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "reserved $mods in version 1", () =>
        {
            JsonObject d = Import(Game(HeroList), "hero-list", "hero-list");
            d.Members.Insert(4, new JsonMember("$mods", new JsonArray()));
            return Expect(Codes.ReservedMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("unknown", "unknown reserved member", () =>
        {
            JsonObject d = Import(Game(HeroList), "hero-list", "hero-list");
            Root(d).Members.Insert(0, new JsonMember("$colour", new JsonInteger(1)));
            return Expect(Codes.ReservedMember, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });

        // Presence: present defaults, absent values, stored nulls.
        yield return new("presence", "a present default stays present", () =>
        {
            JsonObject d = MapDocument();
            Set(Root(d), "Day", new JsonInteger(0));
            return OwnRoundTrip(d, (n, _) => Root(n)["Day"] is JsonInteger { Value: 0 } ? null : "the present zero was dropped");
        });
        yield return new("presence", "explicit false normalizes to absence", () =>
        {
            JsonObject d = MapDocument();
            Set(Root(d), "FogEnabled", new JsonBoolean(false));
            JsonObject n = Import(GameFiles.Export(d).Bytes, "world-hsm", "map");
            return Root(n).Find("FogEnabled") is null ? Pass("absent after normalization") : Fail("false survived");
        });
        yield return new("presence", "a stored null object", () =>
        {
            JsonObject d = Import(Game(Campaign), "campaign", "campaign");
            Set(Object(Root(d)["TransferSettings"]), name: "Leader", value: new JsonNull());
            return OwnRoundTrip(d, (n, _) => Object(Root(n)["TransferSettings"])["Leader"] is JsonNull ? null : "the stored null was lost");
        });
        yield return new("presence", "an empty fixed object stays present", () =>
            RoundTrip(Game(HeroList), "hero-list", "hero-list"));

        // Strings.
        yield return new("strings", "long prefix for a short string", () =>
        {
            return RoundTrip(HeroListImage([1, 20, 0], Concat([255], I32(3), "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list", inspect: d => d.ToString() is not null && GameFiles.Write(d).AsSpan().IndexOf("value-prefixes"u8) >= 0 ? null : "no value-prefixes part");
        });
        yield return new("strings", "all 256 code page bytes", () =>
        {
            byte[] bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
            return RoundTrip(HeroListImage([1, 20, 0], Concat([255], I32(256), bytes)), profile: "hero-list", kind: "hero-list", inspect: d => Root(d)["LibraryName"] is JsonString s && s.Value.Length == 256 && s.Value[0x81] == '\u0081' && s.Value[0x80] == '\u20ac'
                    ? null : "the code page mapping is not the frozen table");
        });
        yield return new("strings", "an unmapped character is refused with its location", () =>
        {
            JsonObject d = Import(Game(HeroList), "hero-list", "hero-list");
            Set(Root(d), "LibraryName", new JsonString("\u4e00"));
            return Expect(Codes.Text, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        foreach ((string? name, Func<JsonValue>? value) in new (string, Func<JsonValue>)[]
        {
            ("empty block", () => new JsonArray()),
            ("one empty line", () => new JsonArray().Add(new JsonString(""))),
            ("NUL in the block", () => new JsonObject().Add("$text", new JsonString("a\0b\r\n"))),
            ("LF line ends", () => new JsonObject().Add("$text", new JsonString("a\nb\n"))),
            ("CR line ends", () => new JsonObject().Add("$text", new JsonString("a\rb\r"))),
            ("no final line end", () => new JsonObject().Add("$text", new JsonString("a\r\nb"))),
        })
        {
            yield return new("strings", "rwStrings " + name, () =>
            {
                JsonObject d = MapDocument();
                Set(Root(d), "Objectives", value());
                return OwnRoundTrip(d);
            });
        }

        yield return new("strings", "rwStringsEx signed object values", () =>
        {
            JsonObject d = Import(Game(ScenarioSet), "scenario-set", "scenario-set");
            Set(Object(Root(d)["ResourceEditSettings"]), name: "Editors", value: new JsonObject()
                .Add("lines", new JsonArray().Add(new JsonString("a")).Add(new JsonString("b")))
                .Add("objects", new JsonArray().Add(new JsonInteger(-1)).Add(new JsonInteger(int.MinValue))));
            return OwnRoundTrip(d);
        });
        yield return new("strings", "rwStringsEx object count mismatch", () =>
        {
            JsonObject d = Import(Game(ScenarioSet), "scenario-set", "scenario-set");
            Set(Object(Root(d)["ResourceEditSettings"]), "Editors", new JsonObject()
                .Add("lines", new JsonArray().Add(new JsonString("a")))
                .Add("objects", new JsonArray().Add(new JsonInteger(1)).Add(new JsonInteger(2))));
            return Expect(Codes.Corrupt, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });

        // Floating point bits. From version 5 a single that a number carries exactly is that number,
        // and only negative zero, the infinities and NaN keep $bits.
        foreach ((string? bits, string? number) in new[] { ("00000080", null), ("0000807f", null), ("000080ff", null), ("0100c07f", null), ("ffffffff", null), ("0000803f", "1"), ("cdcccc3d", "0.1"), ("acc52737", "1e-5") })
        {
            yield return new("floats", "$bits " + bits, () =>
            {
                JsonObject d = MapDocument();
                JsonObject player = Object(Arr(Object(Root(d)["Players"])["Children"]).Items[1]);
                Set(Object(player["AIControl"]), "DefenseStrengthMultiplier", new JsonObject().Add("$bits", new JsonString(bits)));
                return OwnRoundTrip(d, (n, _) =>
                {
                    JsonValue? stored = Object(Object(Arr(Object(Root(n)["Players"])["Children"]).Items[1])["AIControl"])["DefenseStrengthMultiplier"];
                    string? text = stored switch
                    {
                        JsonInteger integer => integer.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        JsonNumber decimalText => decimalText.Text,
                        _ => null,
                    };
                    return number is null
                        ? stored is JsonObject objectValue && objectValue["$bits"] is JsonString { } s && s.Value == bits ? null : "the bits changed"
                        : text == number ? null : "the number is " + (text ?? stored?.KindName) + ", not " + number;
                });
            });
        }

        // Slots, families and dimensions.
        yield return new("identity", "deleting a child slot keeps the other slots", () =>
        {
            JsonObject d = MapDocument();
            JsonArray children = Arr(Object(Root(d)["Players"])["Children"]);
            int count = children.Items.Count;
            children.Items.RemoveAt(1);
            return OwnRoundTrip(d, (n, _) =>
            {
                List<long> slots = Arr(Object(Root(n)["Players"])["Children"]).Items.Select(c => ((JsonInteger)Object(c)["$slot"]!).Value).ToList();
                return count >= 3 && !slots.Contains(1) && slots.Contains(count - 1) ? null : "slots were renumbered: " + string.Join(",", slots);
            });
        });
        yield return new("identity", "duplicate child slot", () =>
        {
            JsonObject d = MapDocument();
            JsonArray children = Arr(Object(Root(d)["Players"])["Children"]);
            Set(Object(children.Items[1]), "$slot", new JsonInteger(0));
            return Expect(Codes.Slot, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "incompatible class family", () =>
        {
            JsonObject d = Import(Game(Campaign), "campaign", "campaign");
            Set(Object(Root(d)["TransferSettings"]), "Leader", new JsonObject().Add("$class", new JsonString("TCity")));
            return Expect(Codes.ClassFamily, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "a field row of the wrong length", () =>
        {
            JsonObject d = MapDocument();
            Arr(FirstFields(d).Items[3]).Items.RemoveAt(0);
            return Expect(Codes.Dimensions, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "a missing field row", () =>
        {
            JsonObject d = MapDocument();
            FirstFields(d).Items.RemoveAt(0);
            return Expect(Codes.Dimensions, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("identity", "editing one field rewrites one row line", () =>
        {
            JsonObject d = MapDocument();
            byte[] before = MapText();
            JsonArray row = Arr(FirstFields(d).Items[5]);
            Set(Object(row.Items[7]), "FieldFlags", new JsonInteger(7));
            byte[] after = GameFiles.Write(Import(GameFiles.Export(Parse(GameFiles.Write(d))).Bytes, "world-hsm", "map"));
            string[] a = System.Text.Encoding.UTF8.GetString(before).Split('\n');
            string[] b = System.Text.Encoding.UTF8.GetString(after).Split('\n');
            int changed = a.Length == b.Length ? a.Zip(b).Count(p => p.First != p.Second) : -1;
            return changed == 1 ? Pass("one line changed") : Fail($"{changed} lines changed");
        });

        // Layout residue: order, gaps, invalidation.
        yield return new("layout", "long table entries and a gap after a Boolean", () =>
        {
            return RoundTrip(HeroListImage(Concat([0x80], I32(1), I32(20), I32(2)), Concat([0xAA, 0xBB], [3], "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list", inspect: d => GameFiles.Write(d).AsSpan().IndexOf("\"gaps\""u8) >= 0 ? null : "no gap part");
        });
        yield return new("layout", "a noncanonical table order", () =>
        {
            return RoundTrip(HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list", inspect: d => GameFiles.Write(d).AsSpan().IndexOf("table-form-order"u8) >= 0 ? null : "no table part");
        });
        yield return new("layout", "an edit invalidates the layout and export rebuilds it", () =>
        {
            JsonObject d = Import(HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list");
            Set(Root(d), "LibraryName", new JsonString("longer name"));
            ExportResult exported = GameFiles.Export(Parse(GameFiles.Write(d)));
            JsonObject n = Import(exported.Bytes, "hero-list", "hero-list");
            return exported.Invalidated.Count == 1 && GameFiles.Write(n).AsSpan().IndexOf("$layout"u8) < 0
                ? Pass("invalidated and rebuilt canonically") : Fail($"{exported.Invalidated.Count} invalidations");
        });
        yield return new("layout", "--exact-layout refuses a stale layout", () =>
        {
            JsonObject d = Import(HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list");
            _ = Root(d).Remove("NextLibraryId");
            return Expect(Codes.Layout, () => GameFiles.Export(Parse(GameFiles.Write(d)), new ExportOptions { ExactLayout = true }), Located);
        });
        yield return new("layout", "a part that does not fit is an error, not a cleanup", () =>
        {
            JsonObject d = Import(HeroListImage([2, 21, 0, 20, 4], Concat(I32(7), [3], "abc"u8.ToArray())), profile: "hero-list", kind: "hero-list");
            JsonObject root = Root(d);
            JsonObject layout = Object(root["$layout"]);
            JsonArray entries = Arr(Object(Arr(layout["parts"]).Items[0])["entries"]);
            Arr(entries.Items[1]).Items[1] = new JsonInteger(2);
            return Expect(Codes.Layout, () => GameFiles.Export(Parse(GameFiles.Write(d))), Located);
        });
        yield return new("layout", "Tutorial property 57, a zero-size alias", () => RoundTrip(Game(Tutorial), "world-hsm", "map",
            d => Root(d)["UnknownProperty57"] is JsonBoolean { Value: true } ? null : "property 57 is missing"));

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
            Classification c = GameFiles.Classify(Gzip(GameFiles.Write(Import(Game(HeroList), "hero-list", "hero-list"))), ".ahl");
            return c.Format == DetectedFormat.GzipOwnJson ? Pass("gzip own JSON") : Fail(c.Format.ToString());
        });
        yield return new("gzip", "bounded gzip", () =>
        {
            byte[] bomb = Gzip(new byte[3_000_000]);
            return Expect(Codes.Limit, () => GameFiles.Read(bomb, null, new FormatLimits { DecompressedBytes = 1_000_000 }), Located);
        });
        yield return new("gzip", "a damaged own document never falls back to binary", () =>
        {
            Classification c = GameFiles.Classify("{\"$format\": \"OpenAoW\", \"$version\": 1,"u8.ToArray(), ".hsm");
            return c.Format == DetectedFormat.MalformedOwn ? Pass("malformed own input") : Fail(c.Format.ToString());
        });

        // JSON diagnostics.
        yield return new("diagnostics", "duplicate JSON member with both locations", () => Expect(Codes.Duplicate,
            () => GameFiles.Read("{\n  \"$format\": \"OpenAoW\",\n  \"$format\": \"OpenAoW\"\n}\n"u8.ToArray(), "dup.ahl"),
            d => d.Span.Line == 3 && d.Span.Column == 3 && d.Message.Contains("2:3", StringComparison.Ordinal) ? null : "wrong locations"));
        yield return new("diagnostics", "a valid-JSON semantic error points at the value", () =>
        {
            JsonObject d = Import(Game(HeroList), "hero-list", "hero-list");
            Set(Root(d), "NextLibraryId", new JsonString("seven"));
            byte[] text = GameFiles.Write(d);
            int line = System.Text.Encoding.UTF8.GetString(text).Split('\n').ToList().FindIndex(l => l.Contains("\"seven\"", StringComparison.Ordinal)) + 1;
            return Expect(Codes.Type, () => GameFiles.Export(Parse(text)),
                diag => diag.Span.Line == line && diag.Path == "root.NextLibraryId" ? null : $"expected line {line}");
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

        yield return new("diagnostics", "an older version fails and asks for a new conversion", () => Expect(Codes.Version,
            () => GameFiles.Read("{\"$format\": \"OpenAoW\", \"$version\": 5}"u8.ToArray()),
            d => d.Message.Contains("version 5", StringComparison.Ordinal)
                && d.Message.Contains("supported version " + OwnFormat.CurrentVersion, StringComparison.Ordinal)
                && d.Message.Contains("Convert the original file again", StringComparison.Ordinal)
                ? null : "the numbers or the advice are missing"));

        // Mapping anchors: document edits reach independently located bytes.
        yield return new("anchors", "seed, day, string, slot and coordinate edits reach their stored ids", Anchors);
        yield return new("legacy", "Valley01 sprite keeps its legacy shape", () =>
            RoundTrip(Game("Scenario/Campaign/Valley01.CSM"), "world-hsm", "campaign-map",
                d => GameFiles.Write(d).AsSpan().IndexOf("UnknownProperty12"u8) >= 0 ? null : "the legacy shape is missing"));

        // Case mappings of the production and diplomatic kinds.
        foreach (Case kindCase in KindCases())
        {
            yield return kindCase;
        }
    }

    private static JsonArray FirstFields(JsonObject document)
    {
        JsonObject root = Root(document);
        foreach (JsonValue child in Arr(root["Children"]).Items)
        {
            if (Object(child)["$class"] is JsonString { Value: "MapContainer" })
            {
                return Arr(Object(Arr(Object(child)["Children"]).Items[0])["Fields"]);
            }
        }

        throw new InvalidOperationException("No map container.");
    }

    private static Outcome ExpectAny(params Action[] actions)
    {
        foreach (Action action in actions)
        {
            try
            {
                action();
                return Fail("the conversion succeeded");
            }
            catch (ConversionException error)
            {
                if (Located(error.Diagnostic) is string problem)
                {
                    return Fail(problem);
                }
            }
        }
        return Pass("every input failed with a located diagnostic");
    }

    private static byte[] Gzip(byte[] data)
    {
        using MemoryStream output = new();
        using (GZipStream gz = new(output, CompressionLevel.Fastest, true))
        {
            gz.Write(data);
        }

        return output.ToArray();
    }

    private static Outcome NonemptyItemList()
    {
        JsonArray items = Arr(Object(Root(MapDocument())["ItemControl"])["Items"]);
        if (items.Items.Count == 0)
        {
            return Fail("the map has no items");
        }

        JsonArray children = new();
        for (int k = 0; k < items.Items.Count; k++)
        {
            JsonObject item = Copy(Envelope("hero-list", "hero-list", Object(items.Items[k])));
            JsonObject objectValue = Object(item["root"]);
            Set(objectValue, "$slot", new JsonInteger(k));
            _ = children.Add(objectValue);
        }
        return OwnRoundTrip(Envelope(root: new JsonObject().Add("Name", new JsonString("Fixture items")).Add("LastId", new JsonInteger(items.Items.Count)).Add("Children", children), kind: "item-list", profile: "item-list"));
    }

    private static JsonObject CampaignWithMap(out byte[] mapFile)
    {
        mapFile = Game(Map);
        JsonObject campaign = Import(Game(Campaign), "campaign", "campaign");
        JsonObject map = Import(mapFile, "world-hsm", "map");
        JsonObject nested = new JsonObject().Add("$container", map["$container"]!).Add("root", map["root"]!);
        JsonObject layout = new JsonObject()
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
        JsonObject campaign = CampaignWithMap(out byte[]? mapFile);
        ExportResult exported = GameFiles.Export(Parse(GameFiles.Write(campaign)));
        if (exported.Bytes.AsSpan().IndexOf(mapFile) < 0)
        {
            return Fail("the exact compressed map bytes are not in the export");
        }

        Outcome result = RoundTrip(exported.Bytes, "campaign", "campaign-save",
            d => GameFiles.Write(d).AsSpan().IndexOf("nested-compression"u8) >= 0 ? null : "no nested compression part");
        return result.Matched ? Pass("exact nested CFS bytes, " + result.Detail, result.OwnDocuments) : result;
    }

    private static Outcome CampaignSaveEdit()
    {
        JsonObject campaign = CampaignWithMap(out byte[]? mapFile);
        JsonObject map = Object(Root(campaign)["Map"]);
        Set(Object(map["root"]), "Day", new JsonInteger(42));
        ExportResult exported = GameFiles.Export(Parse(GameFiles.Write(campaign)));
        if (exported.Invalidated.Count == 0)
        {
            return Fail("the stale nested bytes were not reported");
        }

        if (exported.Bytes.AsSpan().IndexOf(mapFile) >= 0)
        {
            return Fail("the stale nested bytes were reused");
        }

        JsonValue? day = Object(Object(Root(Import(exported.Bytes, "campaign", "campaign-save"))["Map"])["root"])["Day"];
        return day is not JsonInteger { Value: 42 } ? Fail("the edit was lost") : RoundTrip(exported.Bytes, "campaign", "campaign-save");
    }

    /// <summary>
    /// Mapping anchors (own-formats.md 5.1): edits in the document reach the stored ids that an
    /// independent table walk in this method finds, not the codec under test.
    /// </summary>
    private static Outcome Anchors()
    {
        JsonObject d = MapDocument();
        JsonObject root = Root(d);
        Set(root, "DefendersSeed", new JsonInteger(0x1234567));
        Set(root, "Day", new JsonInteger(13));
        Set(root, "Author", new JsonString("Anchor"));
        byte[] payload = Payload(GameFiles.Export(Parse(GameFiles.Write(d))).Bytes);
        Dictionary<int, (int Start, int End)> top = Walk(payload, BinaryPrimitives.ReadInt32LittleEndian(payload));
        Dictionary<int, (int Start, int End)> props = Walk(payload, top[0].Start);
        int Int32(int id) => BinaryPrimitives.ReadInt32LittleEndian(payload.AsSpan(props[id].Start, 4));
        if (Int32(28) != 0x1234567)
        {
            return Fail("DefendersSeed did not reach id 28");
        }

        if (Int32(19) != 13)
        {
            return Fail("Day did not reach id 19");
        }

        if (props[7].End - props[7].Start != 7 || payload[props[7].Start] != 6)
        {
            return Fail("Author did not reach id 7");
        }

        Dictionary<int, (int Start, int End)> players = Walk(payload, props[21].Start);
        Dictionary<int, (int Start, int End)> children = Walk(payload, players[1].Start);
        return !children.ContainsKey(0) || !children.ContainsKey(1)
            ? Fail("player slots 0 and 1 are not child ids 0 and 1")
            : Pass("ids 28, 19, 7 and the child slots hold the edited values");
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
        List<(int Id, int Offset)> entries = [];
        for (int i = 0; i < shorts; i++, at += 2)
        {
            entries.Add((data[at], data[at + 1]));
        }

        for (int i = 0; i < longs; i++, at += 8)
        {
            entries.Add((BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at)), BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(at + 4))));
        }

        List<int> offsets = entries.Select(e => e.Offset).Distinct().Order().ToList();
        Dictionary<int, (int, int)> result = [];
        foreach ((int id, int offset) in entries)
        {
            int k = offsets.IndexOf(offset);
            int end = k + 1 < offsets.Count ? at + offsets[k + 1] : -1;
            result[id] = (at + offset, end);
        }
        return result;
    }
}
