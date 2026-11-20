using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Formats.Tests;

/// <summary>
/// Rule-based G1 comparison, $layout refusal, ILB normalization and schema-declared skipped ids.
/// </summary>
public sealed class OriginalComparisonTests : FormatsTest
{
    private static FormatSchema Schema => FormatSchema.Current;

    [Fact]
    public void LayoutReservedMemberIsRefusedEverywhere()
    {
        // On a root object.
        JsonObject rootDoc = Envelope("hero-list", profile: "hero-list", root: new JsonObject().Add("$layout", new JsonObject()));
        ConversionException rootError = Assert.Throws<ConversionException>(() => GameFiles.Export(rootDoc));
        Assert.Equal(Codes.ReservedMember, rootError.Diagnostic.Code);
        Assert.Equal("root.$layout", rootError.Diagnostic.Path);

        // On $container.
        JsonObject container = new JsonObject()
            .Add("profile", new JsonString("hero-list"))
            .Add("compression", new JsonString("none"))
            .Add("$layout", new JsonObject());
        JsonObject containerDoc = new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
            .Add("$kind", new JsonString("hero-list"))
            .Add("$container", container)
            .Add("root", new JsonObject());
        ConversionException containerError = Assert.Throws<ConversionException>(() => GameFiles.Export(containerDoc));
        Assert.Equal(Codes.ReservedMember, containerError.Diagnostic.Code);
        Assert.Equal("$container.$layout", containerError.Diagnostic.Path);

        // On a nested campaign map.
        JsonObject nestedMap = new JsonObject()
            .Add("$container", new JsonObject().Add("profile", new JsonString("world-hsm")).Add("compression", new JsonString("none")))
            .Add("$layout", new JsonObject())
            .Add("root", new JsonObject().Add("$class", new JsonString("StrategicMap")));
        JsonObject campaignDoc = Envelope("campaign-save", profile: "campaign", root: new JsonObject()
            .Add("$class", new JsonString("Campaign"))
            .Add("Map", nestedMap));
        ConversionException mapError = Assert.Throws<ConversionException>(() => GameFiles.Export(campaignDoc));
        Assert.Equal(Codes.ReservedMember, mapError.Diagnostic.Code);
        Assert.Equal("root.Map.$layout", mapError.Diagnostic.Path);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void StrategicMapSkipsDeclaredId57AndRefusesOtherUndeclaredIds(int skippedDataLength)
    {
        SchemaClass mapClass = Schema.Get("StrategicMap");
        Assert.True(mapClass.IsSkippedId(57));
        Assert.False(mapClass.IsSkippedId(99));
        Assert.Null(mapClass.PropertyById(57));
        Assert.Null(mapClass.PropertyByName("UnknownProperty57"));

        // Build a StrategicMap image with Day (id 19, 4 bytes), skipped id 57, and DefendersSeed (id 28, 4 bytes).
        byte[] skippedBytes = new byte[skippedDataLength];
        for (int i = 0; i < skippedDataLength; i++)
        {
            skippedBytes[i] = (byte)(0x41 + i);
        }

        ByteBuffer body = new();
        body.Byte(3); // 3 short entries
        body.Byte(19);
        body.Byte(0);
        body.Byte(57);
        body.Byte(4);
        body.Byte(28);
        body.Byte((byte)(4 + skippedDataLength));
        body.Int32(7);
        body.Bytes(skippedBytes);
        body.Int32(99);
        byte[] rootImage = body.ToArray();

        // Wrap in raw HSM (world-hsm) for full converter + G1 round trip.
        ByteBuffer hsm = new();
        hsm.Int32(16);
        hsm.Bytes(HsmProfile.Magic);
        hsm.Int32(0);
        hsm.Int32(mapClass.Id!.Value);
        hsm.Bytes([1, 0, 0]);
        hsm.Bytes(rootImage);
        byte[] file = hsm.ToArray();

        FileProfile profile = FileProfile.Find("world-hsm")!;
        ImportResult imported = GameFiles.Import(file, profile, kind: "map");
        JsonObject root = (JsonObject)imported.Document["root"]!;
        Assert.Equal(7, ((JsonInteger)root["Day"]!).Value);
        Assert.Equal(99, ((JsonInteger)root["DefendersSeed"]!).Value);
        Assert.Null(root.Find("UnknownProperty57"));

        // Tree reader skips id 57 too, keeping Day and DefendersSeed intact.
        ObjectNode tree = OriginalBackend.Read(rootImage, classValue: mapClass, Schema, engine: "game");
        Assert.Equal(7, ((IntegerNode)tree.Find("Day")!).Value);
        Assert.Equal(99, ((IntegerNode)tree.Find("DefendersSeed")!).Value);
        Assert.Null(tree.Find("UnknownProperty57"));

        // Export omits id 57, and G1 matches under rule skipped-property.
        ExportResult exported = GameFiles.Export(imported.Document);
        OriginalComparisonResult comparison = GameFiles.CompareG1(file, exported: exported.Bytes, profile: profile, document: imported.Document, kind: "map");
        Assert.Equal(OriginalMatchKind.Masked, comparison.Kind);
        Assert.Equal(2 + skippedDataLength, comparison.MaskedBytesByRule[OriginalComparison.RuleSkippedProperty]);

        // JSON carrying UnknownProperty57 is refused as an unknown member.
        _ = root.Add("UnknownProperty57", new JsonBoolean(true));
        ConversionException jsonError = Assert.Throws<ConversionException>(() => GameFiles.Export(imported.Document));
        Assert.Equal(Codes.UnknownMember, jsonError.Diagnostic.Code);
        Assert.Equal("root.UnknownProperty57", jsonError.Diagnostic.Path);

        // Undeclared id 99 is still refused on both converter and tree reader.
        ByteBuffer bad = new();
        bad.Byte(1);
        bad.Byte(99);
        bad.Byte(0);
        byte[] badRoot = bad.ToArray();
        ByteBuffer badHsm = new();
        badHsm.Int32(16);
        badHsm.Bytes(HsmProfile.Magic);
        badHsm.Int32(0);
        badHsm.Int32(mapClass.Id!.Value);
        badHsm.Bytes([1, 0, 0]);
        badHsm.Bytes(badRoot);
        ConversionException convError = Assert.Throws<ConversionException>(() => GameFiles.Import(badHsm.ToArray(), profile, kind: "map"));
        Assert.Equal(Codes.UnknownProperty, convError.Diagnostic.Code);
        ConversionException treeError = Assert.Throws<ConversionException>(() => OriginalBackend.Read(badRoot, classValue: mapClass, Schema, engine: "game"));
        Assert.Equal(Codes.UnknownProperty, treeError.Diagnostic.Code);
    }

    [Fact]
    public void IlbNormalizationClearsHeaderPaddingAndSpriteDwordAndComparisonFailsOutsideThem()
    {
        byte[] ilb = BuildIlbWithSprite16(headerPad: [0x16, 0x50, 0x55], reservedDword: 0x5520B020, colorKey: 0x00001234, out int reservedOffset);

        // Wrap ILB inside a MapObjectResource in a written-object profile.
        byte[] resourceImage = WrapMapObjectResourceIlb(ilb);
        WrittenObjectProfile profile = new("pfs", kinds: ["pfs"], rootClass: "MapObjectResource", engine: "game");
        ImportResult imported = GameFiles.Import(resourceImage, profile, kind: "pfs");
        JsonObject root = (JsonObject)imported.Document["root"]!;
        JsonObject ilbJson = (JsonObject)root["ImageLibrary"]!;
        byte[] decodedIlb = System.Convert.FromBase64String(((JsonString)ilbJson["$ilb"]!).Value);
        Assert.Equal([0, 0, 0], decodedIlb[5..8]);
        Assert.Equal(0u, BinaryPrimitives.ReadUInt32LittleEndian(decodedIlb.AsSpan(reservedOffset)));
        Assert.Equal(0x00001234u, BinaryPrimitives.ReadUInt32LittleEndian(decodedIlb.AsSpan(reservedOffset - 4)));

        // Export matches original outside the two declared ILB rules.
        byte[] exported = profile.Export(
            container: (JsonObject)imported.Document["$container"]!,
            root: root,
            context: new EncodeContext(Schema, profile.Engine, profile: profile.Name, file: null, limits: FormatLimits.Default));
        OriginalComparisonResult comparison = GameFiles.CompareG1(resourceImage, exported: exported, profile: profile, document: imported.Document, kind: "pfs");
        Assert.Equal(OriginalMatchKind.Masked, comparison.Kind);
        Assert.Equal(3, comparison.MaskedBytesByRule[OriginalComparison.RuleIlbHeaderPadding]);
        Assert.Equal(4, comparison.MaskedBytesByRule[OriginalComparison.RuleIlbSpriteDword]);

        // Corrupting ColorKey (4 bytes before Reserved) in exported must fail G1.
        byte[] corrupted = (byte[])exported.Clone();
        int ilbStartInExport = exported.AsSpan().IndexOf("\u0004ILB\0"u8);
        corrupted[ilbStartInExport + reservedOffset - 4] ^= 0xFF;
        OriginalComparisonResult mismatch = GameFiles.CompareG1(resourceImage, exported: corrupted, profile: profile, document: imported.Document, kind: "pfs");
        Assert.Equal(OriginalMatchKind.Mismatch, mismatch.Kind);
        Assert.Equal(ilbStartInExport + reservedOffset - 4, mismatch.FirstDifferenceOffset);
    }

    [Fact]
    public void HssCrcRuleOnlyFiresWhenCoveredBytesChanged()
    {
        FileProfile profile = FileProfile.Find("scenario-set")!;
        byte[] clean = BuildMinimalHss(name: "Test", tailNonZero: false);
        ImportResult imported = GameFiles.Import(clean, profile, kind: "scenario-set");
        byte[] exported = GameFiles.Export(imported.Document).Bytes;
        Assert.Equal(OriginalMatchKind.Exact, GameFiles.CompareG1(clean, exported: exported, profile: profile, document: imported.Document, kind: "scenario-set").Kind);

        // Corrupting only the CRC byte in exported fails G1 because no covered byte changed.
        byte[] badCrc = (byte[])exported.Clone();
        badCrc[^1] ^= 0x01;
        OriginalComparisonResult mismatch = GameFiles.CompareG1(clean, exported: badCrc, profile: profile, document: imported.Document, kind: "scenario-set");
        Assert.Equal(OriginalMatchKind.Mismatch, mismatch.Kind);
        Assert.Equal(clean.Length - 1, mismatch.FirstDifferenceOffset);

        // Nonzero header tail masks hss-header-tail (38 bytes for 4-char name) and file-crc (4 bytes).
        byte[] dirty = BuildMinimalHss(name: "Test", tailNonZero: true);
        ImportResult dirtyImported = GameFiles.Import(dirty, profile, kind: "scenario-set");
        byte[] dirtyExported = GameFiles.Export(dirtyImported.Document).Bytes;
        OriginalComparisonResult masked = GameFiles.CompareG1(dirty, exported: dirtyExported, profile: profile, document: dirtyImported.Document, kind: "scenario-set");
        Assert.Equal(OriginalMatchKind.Masked, masked.Kind);
        Assert.Equal(52 - (10 + 4), masked.MaskedBytesByRule[OriginalComparison.RuleHssHeaderTail]);
        Assert.Equal(4, masked.MaskedBytesByRule[OriginalComparison.RuleFileCrc]);
    }

    [Fact]
    public void TableEntriesRuleMasksFormAndOrderButRefusesDataChanges()
    {
        // HeroLibrary with LibraryName (id 20, offset 0) and NextLibraryId (id 21, offset 4) stored as long entries.
        ByteBuffer orig = new();
        orig.Bytes([1, 0, 0]); // outer table
        orig.Byte(0x80); // 0 short, long marker set
        orig.Int32(2); // 2 long entries
        orig.Int32(20);
        orig.Int32(0);
        orig.Int32(21);
        orig.Int32(4);
        orig.Byte(3);
        orig.Bytes("abc"u8);
        orig.Int32(42);
        byte[] file = orig.ToArray();

        FileProfile profile = FileProfile.Find("hero-list")!;
        ImportResult imported = GameFiles.Import(file, profile, kind: "hero-list");
        byte[] exported = GameFiles.Export(imported.Document).Bytes;
        OriginalComparisonResult masked = GameFiles.CompareG1(file, exported: exported, profile: profile, document: imported.Document, kind: "hero-list");
        Assert.Equal(OriginalMatchKind.Masked, masked.Kind);
        Assert.True(masked.MaskedBytesByRule[OriginalComparison.RuleTableEntries] > 0);

        // Changing a data byte in exported must fail G1.
        byte[] corrupted = (byte[])exported.Clone();
        corrupted[^1] ^= 0x01;
        OriginalComparisonResult mismatch = GameFiles.CompareG1(file, exported: corrupted, profile: profile, document: imported.Document, kind: "hero-list");
        Assert.Equal(OriginalMatchKind.Mismatch, mismatch.Kind);
    }

    private static JsonObject Envelope(string kind, string profile, JsonObject root) => new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(OwnFormat.CurrentVersion))
        .Add("$kind", new JsonString(kind))
        .Add("$container", new JsonObject().Add("profile", new JsonString(profile)).Add("compression", new JsonString("none")))
        .Add("root", root);

    private static byte[] BuildMinimalHss(string name, bool tailNonZero)
    {
        byte[] head = new byte[52];
        BinaryPrimitives.WriteInt32LittleEndian(head, value: 52);
        "\u0004HSS\0"u8.CopyTo(head.AsSpan(4));
        byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(name);
        head[9] = (byte)nameBytes.Length;
        nameBytes.CopyTo(head, index: 10);
        if (tailNonZero)
        {
            head[51] = 0xAB;
        }

        ByteBuffer body = new();
        body.Bytes(head);
        body.Bytes([1, 0, 0, 0]); // outer table + empty GamePack
        uint crc = Crc32.Compute(body.Span);
        body.Int32((int)crc);
        return body.ToArray();
    }

    private static byte[] WrapMapObjectResourceIlb(byte[] ilb)
    {
        // MapObjectResource ImageLibrary is property id 8.
        ByteBuffer body = new();
        body.Bytes([1, 0, 0]); // outer table
        body.Bytes([1, 8, 0]); // 1 short entry: id 8 at offset 0
        body.Bytes(ilb);
        return body.ToArray();
    }

    private static byte[] BuildIlbWithSprite16(byte[] headerPad, uint reservedDword, uint colorKey, out int reservedOffset)
    {
        ByteBuffer buf = new();
        buf.Bytes("\u0004ILB\0"u8);
        buf.Bytes(headerPad);
        buf.Int32(BitConverter.SingleToInt32Bits(4f));
        buf.Int32(24);
        int indexEndAt = buf.Length;
        buf.Int32(0); // IndexEnd placeholder
        int sizeAt = buf.Length;
        buf.Int32(0); // Size placeholder

        // 0 palettes.
        buf.Int32(0);
        // Image entry index 0, class 17 (TRLESprite16), version 3, empty name.
        buf.Int32(0);
        buf.Int32(17);
        buf.Byte(3); // version 3
        buf.Int32(0); // name length
        for (int i = 0; i < 5; i++)
        {
            buf.Int32(0); // common 5 ints
        }

        buf.Byte(0); // loadMode
        buf.Int32(0); // dataSize
        buf.Int32(0); // dataPosition (>= 0, not inline)
        buf.Int32(0); // v > 0 int 1
        buf.Int32(0); // v > 0 int 2
        buf.Int32(0); // ShowInfo bytes 0..3
        buf.Int32(0); // ShowInfo bytes 4..7
        buf.Int32(0); // pixel format (TImage16)
        buf.Int32(10); // SpriteWidth
        buf.Int32(10); // SpriteHeight
        buf.Int32(0); // SpriteX
        buf.Int32(0); // SpriteY
        buf.Int32((int)colorKey); // ColorKey
        reservedOffset = buf.Length;
        buf.Int32((int)reservedDword); // unused Reserved dword
        buf.Int32(0); // reference
        buf.Int32(-1); // terminator

        int total = buf.Length;
        buf.PatchInt32(indexEndAt, value: total);
        buf.PatchInt32(sizeAt, value: total);
        return buf.ToArray();
    }
}
