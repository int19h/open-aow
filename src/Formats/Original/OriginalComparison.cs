using System.Buffers.Binary;
using Aow.Formats.Json;
using Aow.Formats.Schema;

namespace Aow.Formats.Original;

/// <summary>How an exported original compares with the source original under check G1.</summary>
public enum OriginalMatchKind
{
    Exact,
    Masked,
    Document,
    Mismatch,
}

/// <summary>Result of comparing an original file with its exported round trip under G1.</summary>
public sealed class OriginalComparisonResult
{
    public required OriginalMatchKind Kind { get; init; }
    public long? FirstDifferenceOffset { get; init; }
    public string? Detail { get; init; }
    public IReadOnlyDictionary<string, long> MaskedBytesByRule { get; init; } = new Dictionary<string, long>(StringComparer.Ordinal);
}

/// <summary>
/// Per-format rule-based comparison for check G1 (own-formats.md 2.5 and 3.3).
/// Compares the original and exported bytes structurally, masking only declared don't-care
/// regions by rule: HSM header reserved bytes and header extensions, HSS header name tail,
/// padding, header extensions and file CRC over changed bytes, ILB header bytes 5 to 7,
/// 16-bit RLE sprite reserved dwords, schema-declared skipped properties, and property tables
/// whose entries carry the same values in a different order or form. When bytes differ due to
/// a non-canonical layout encoding beyond in-place rules (out-of-order property data, gaps,
/// long string prefix below 255, non-canonical run-list or sequence encoding, or nested
/// compression), falls back to canonical document equality. Any other difference fails.
/// </summary>
internal static class OriginalComparison
{
    public const string RuleHsmHeaderReserved = "hsm-header-reserved";
    public const string RuleHsmHeaderExtension = "hsm-header-extension";
    public const string RuleHssHeaderTail = "hss-header-tail";
    public const string RuleHssHeaderExtension = "hss-header-extension";
    public const string RuleFileCrc = "file-crc";
    public const string RuleIlbHeaderPadding = "ilb-header-padding";
    public const string RuleIlbSpriteDword = "ilb-sprite-dword";
    public const string RuleSkippedProperty = "skipped-property";
    public const string RuleTableEntries = "table-entries";

    public static OriginalComparisonResult CompareFile(
        byte[] originalFile,
        byte[] exportedFile,
        FileProfile profile,
        JsonObject originalDocument,
        string kind,
        FormatSchema schema,
        FormatLimits limits)
    {
        byte[] original = profile.Comparable(originalFile, limits);
        byte[] exported = profile.Comparable(exportedFile, limits);
        return CompareComparable(
            original,
            exported: exported,
            profile: profile,
            originalDocument: originalDocument,
            reimportExported: () => GameFiles.Import(exportedFile, profile: profile, kind: kind, fileName: null, limits: limits).Document,
            schema: schema,
            limits: limits);
    }

    public static OriginalComparisonResult CompareImage(
        byte[] originalFile,
        byte[] exportedImage,
        FileProfile profile,
        JsonObject container,
        JsonObject? originalDocument,
        string? kind,
        FormatSchema schema,
        FormatLimits limits)
    {
        byte[] original = profile.Comparable(originalFile, limits);
        byte[] exported = profile.ComparableImage(container, exportedImage);
        return CompareComparable(
            original,
            exported: exported,
            profile: profile,
            originalDocument: originalDocument,
            reimportExported: originalDocument is null || kind is null
                ? null
                : () => ReimportEngineImage(exportedImage, profile: profile, container: container, kind: kind, schema: schema, limits: limits),
            schema: schema,
            limits: limits);
    }

    private static JsonObject ReimportEngineImage(
        byte[] exportedImage,
        FileProfile profile,
        JsonObject container,
        string kind,
        FormatSchema schema,
        FormatLimits limits)
    {
        ImportSession session = new()
        {
            Schema = schema,
            Limits = limits,
            LegacyRules = [with(StringComparer.Ordinal)],
        };
        if (profile is HsmProfile hsm)
        {
            bool inflated = container["compression"] is JsonString { Value: "cfs" } && container["level"] is JsonInteger { Value: > 0 };
            JsonObject containerCopy = CopyContainer(container);
            (JsonObject importedContainer, JsonObject root) = hsm.ImportEnginePayload(exportedImage, container: containerCopy, inflated: inflated, session: session);
            return GameFiles.NewDocument(kind, container: importedContainer, root: root);
        }

        (JsonObject otherContainer, JsonObject otherRoot) = profile.Import(exportedImage, session);
        return GameFiles.NewDocument(kind, container: otherContainer, root: otherRoot);
    }

    private static JsonObject CopyContainer(JsonObject container)
    {
        JsonObject copy = new();
        foreach (JsonMember member in container.Members)
        {
            _ = copy.Add(member.Name, member.Value);
        }

        return copy;
    }

    private static OriginalComparisonResult CompareComparable(
        byte[] original,
        byte[] exported,
        FileProfile profile,
        JsonObject? originalDocument,
        Func<JsonObject>? reimportExported,
        FormatSchema schema,
        FormatLimits limits)
    {
        int common = original.AsSpan().CommonPrefixLength(exported);
        if (common == original.Length && common == exported.Length)
        {
            return new OriginalComparisonResult { Kind = OriginalMatchKind.Exact };
        }

        State state = new(original, exported: exported, schema: schema, limits: limits);
        try
        {
            CompareProfile(state, profile: profile, originalStart: 0, originalEnd: original.Length, exportedStart: 0, exportedEnd: exported.Length, hasCfsPrefix: profile is HsmProfile && Cfs.HasMagic(original));
        }
        catch (ConversionException)
        {
            return new OriginalComparisonResult
            {
                Kind = OriginalMatchKind.Mismatch,
                FirstDifferenceOffset = common,
                Detail = "The exported bytes do not parse as the profile's image.",
            };
        }

        if (state.MismatchOffset is long mismatch)
        {
            return new OriginalComparisonResult
            {
                Kind = OriginalMatchKind.Mismatch,
                FirstDifferenceOffset = mismatch,
                Detail = state.MismatchDetail,
            };
        }

        if (state.NeedsDocumentFallback)
        {
            if (originalDocument is null || reimportExported is null)
            {
                return new OriginalComparisonResult
                {
                    Kind = OriginalMatchKind.Mismatch,
                    FirstDifferenceOffset = common,
                    Detail = "Non-canonical layout differs and no document is available for fallback comparison.",
                };
            }

            try
            {
                JsonObject reimported = reimportExported();
                if (GameFiles.Write(reimported).AsSpan().SequenceEqual(GameFiles.Write(originalDocument)))
                {
                    return new OriginalComparisonResult
                    {
                        Kind = OriginalMatchKind.Document,
                        Detail = state.FallbackReason,
                    };
                }
            }
            catch (ConversionException)
            {
                // Fall through to mismatch below.
            }

            return new OriginalComparisonResult
            {
                Kind = OriginalMatchKind.Mismatch,
                FirstDifferenceOffset = common,
                Detail = "Re-imported document differs from original document.",
            };
        }

        return state.MaskedBytesByRule.Count == 0
            ? new OriginalComparisonResult
            {
                Kind = OriginalMatchKind.Mismatch,
                FirstDifferenceOffset = common,
                Detail = "Bytes differ without a matching rule.",
            }
            : new OriginalComparisonResult
            {
                Kind = OriginalMatchKind.Masked,
                MaskedBytesByRule = state.MaskedBytesByRule,
            };
    }

    private static void CompareProfile(
        State state,
        FileProfile profile,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd,
        bool hasCfsPrefix)
    {
        if (profile is HsmProfile hsm)
        {
            CompareHsm(state, hsm: hsm, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, hasCfsPrefix: hasCfsPrefix);
            return;
        }

        if (profile is ScenarioSetProfile)
        {
            CompareHss(state, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
            return;
        }

        if (profile is StoredObjectProfile stored)
        {
            if (!CompareTop(state, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, out int origRoot, out int expRoot))
            {
                return;
            }

            if (origRoot + 4 > originalEnd || expRoot + 4 > exportedEnd)
            {
                state.Fail(origRoot, "Root class id is truncated.");
                return;
            }

            int origClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(origRoot));
            int expClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(expRoot));
            if (origClassId != expClassId)
            {
                state.Fail(origRoot + state.Original.AsSpan(origRoot, 4).CommonPrefixLength(state.Exported.AsSpan(expRoot, 4)), "Root class id differs.");
                return;
            }

            SchemaClass? classValue = state.Schema.Resolve(stored.Engine, origClassId);
            if (classValue is null)
            {
                state.Fail(origRoot, "Unknown root class id.");
                return;
            }

            CompareObject(state, engine: stored.Engine, classValue: classValue, originalStart: origRoot + 4, originalEnd: originalEnd, exportedStart: expRoot + 4, exportedEnd: exportedEnd);
            return;
        }

        if (profile is WrittenObjectProfile written)
        {
            if (!CompareTop(state, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, out int origRoot, out int expRoot))
            {
                return;
            }

            CompareObject(state, engine: written.Engine, classValue: state.Schema.Get(written.RootFamily), originalStart: origRoot, originalEnd: originalEnd, exportedStart: expRoot, exportedEnd: exportedEnd);
            return;
        }

        state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Unsupported profile.");
    }

    private static void CompareHsm(
        State state,
        HsmProfile hsm,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd,
        bool hasCfsPrefix)
    {
        if (hasCfsPrefix)
        {
            if (originalEnd - originalStart < 5 || exportedEnd - exportedStart < 5)
            {
                state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "CFS prefix is truncated.");
                return;
            }

            ReadOnlySpan<byte> origPrefix = state.Original.AsSpan(originalStart, 5);
            ReadOnlySpan<byte> expPrefix = state.Exported.AsSpan(exportedStart, 5);
            if (!origPrefix.SequenceEqual(expPrefix))
            {
                state.Fail(originalStart + origPrefix.CommonPrefixLength(expPrefix), "CFS prefix differs.");
                return;
            }

            originalStart += 5;
            exportedStart += 5;
        }
        else if (exportedEnd - exportedStart >= 5 && Cfs.HasMagic(state.Exported.AsSpan(exportedStart)))
        {
            state.Fail(originalStart, "CFS prefix differs.");
            return;
        }

        if (originalEnd - originalStart < 16 || exportedEnd - exportedStart < 16)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "HSM header is truncated.");
            return;
        }

        int origSize = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(originalStart));
        int expSize = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(exportedStart));
        if (origSize < 16 || originalStart + origSize > originalEnd || expSize != 16 || exportedStart + expSize > exportedEnd)
        {
            state.Fail(originalStart, "HSM header size differs.");
            return;
        }

        if (!state.Original.AsSpan(originalStart + 4, 4).SequenceEqual(state.Exported.AsSpan(exportedStart + 4, 4)))
        {
            state.Fail(originalStart + 4 + state.Original.AsSpan(originalStart + 4, 4).CommonPrefixLength(state.Exported.AsSpan(exportedStart + 4, 4)), "HSM magic differs.");
            return;
        }

        ReadOnlySpan<byte> origReserved = state.Original.AsSpan(originalStart + 8, 4);
        ReadOnlySpan<byte> expReserved = state.Exported.AsSpan(exportedStart + 8, 4);
        if (!origReserved.SequenceEqual(expReserved))
        {
            if (expReserved.ContainsAnyExcept((byte)0))
            {
                state.Fail(originalStart + 8 + origReserved.CommonPrefixLength(expReserved), "Exported HSM reserved field is not zero.");
                return;
            }

            state.Mask(RuleHsmHeaderReserved, 4);
        }

        int origClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(originalStart + 12));
        int expClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(exportedStart + 12));
        if (origClassId != expClassId)
        {
            state.Fail(originalStart + 12 + state.Original.AsSpan(originalStart + 12, 4).CommonPrefixLength(state.Exported.AsSpan(exportedStart + 12, 4)), "HSM root class id differs.");
            return;
        }

        if (origSize > 16)
        {
            state.Mask(RuleHsmHeaderExtension, origSize - 16);
        }

        SchemaClass? classValue = state.Schema.Resolve(hsm.Engine, origClassId);
        if (classValue is null)
        {
            state.Fail(originalStart + 12, "Unknown HSM root class id.");
            return;
        }

        if (!CompareTop(state, originalStart: originalStart + origSize, originalEnd: originalEnd, exportedStart: exportedStart + expSize, exportedEnd: exportedEnd, out int origRoot, out int expRoot))
        {
            return;
        }

        CompareObject(state, engine: hsm.Engine, classValue: classValue, originalStart: origRoot, originalEnd: originalEnd, exportedStart: expRoot, exportedEnd: exportedEnd);
    }

    private static void CompareHss(State state, int originalStart, int originalEnd, int exportedStart, int exportedEnd)
    {
        const int HeaderSize = 52;
        if (originalEnd - originalStart < HeaderSize + 4 || exportedEnd - exportedStart < HeaderSize + 4)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "HSS header is truncated.");
            return;
        }

        int origSize = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(originalStart));
        int expSize = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(exportedStart));
        if (origSize < HeaderSize || originalStart + origSize > originalEnd - 4 || expSize != HeaderSize || exportedStart + expSize > exportedEnd - 4)
        {
            state.Fail(originalStart, "HSS header size differs.");
            return;
        }

        // Identifier (4..8) and name length (9).
        ReadOnlySpan<byte> origId = state.Original.AsSpan(originalStart + 4, 6);
        ReadOnlySpan<byte> expId = state.Exported.AsSpan(exportedStart + 4, 6);
        if (!origId.SequenceEqual(expId))
        {
            state.Fail(originalStart + 4 + origId.CommonPrefixLength(expId), "HSS identifier or name length differs.");
            return;
        }

        int nameLength = state.Original[originalStart + 9];
        if (nameLength > 40)
        {
            state.Fail(originalStart + 9, "HSS name length exceeds 40.");
            return;
        }

        ReadOnlySpan<byte> origName = state.Original.AsSpan(originalStart + 10, nameLength);
        ReadOnlySpan<byte> expName = state.Exported.AsSpan(exportedStart + 10, nameLength);
        if (!origName.SequenceEqual(expName))
        {
            state.Fail(originalStart + 10 + origName.CommonPrefixLength(expName), "HSS header name differs.");
            return;
        }

        int tailOffset = 10 + nameLength;
        int tailLength = HeaderSize - tailOffset;
        ReadOnlySpan<byte> origTail = state.Original.AsSpan(originalStart + tailOffset, tailLength);
        ReadOnlySpan<byte> expTail = state.Exported.AsSpan(exportedStart + tailOffset, tailLength);
        if (!origTail.SequenceEqual(expTail))
        {
            if (expTail.ContainsAnyExcept((byte)0))
            {
                state.Fail(originalStart + tailOffset + origTail.CommonPrefixLength(expTail), "Exported HSS header tail is not zero.");
                return;
            }

            state.Mask(RuleHssHeaderTail, tailLength);
        }

        if (origSize > HeaderSize)
        {
            state.Mask(RuleHssHeaderExtension, origSize - HeaderSize);
        }

        int origBodyEnd = originalEnd - 4;
        int expBodyEnd = exportedEnd - 4;
        if (!CompareTop(state, originalStart: originalStart + origSize, originalEnd: origBodyEnd, exportedStart: exportedStart + expSize, exportedEnd: expBodyEnd, out int origRoot, out int expRoot))
        {
            return;
        }

        CompareObject(state, engine: "game", classValue: state.Schema.Get("GamePack"), originalStart: origRoot, originalEnd: origBodyEnd, exportedStart: expRoot, exportedEnd: expBodyEnd);
        if (state.Stopped)
        {
            return;
        }

        ReadOnlySpan<byte> origCrcBytes = state.Original.AsSpan(origBodyEnd, 4);
        ReadOnlySpan<byte> expCrcBytes = state.Exported.AsSpan(expBodyEnd, 4);
        if (!origCrcBytes.SequenceEqual(expCrcBytes))
        {
            uint origCrc = BinaryPrimitives.ReadUInt32LittleEndian(origCrcBytes);
            uint expCrc = BinaryPrimitives.ReadUInt32LittleEndian(expCrcBytes);
            uint computedOrig = Crc32.Compute(state.Original.AsSpan(originalStart, origBodyEnd - originalStart));
            uint computedExp = Crc32.Compute(state.Exported.AsSpan(exportedStart, expBodyEnd - exportedStart));
            if (state.MaskedBytesByRule.Count == 0 || origCrc != computedOrig || expCrc != computedExp)
            {
                state.Fail(origBodyEnd + origCrcBytes.CommonPrefixLength(expCrcBytes), "HSS checksum differs without a declared change in its covered bytes.");
                return;
            }

            state.Mask(RuleFileCrc, 4);
        }
    }

    private static bool CompareTop(
        State state,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd,
        out int origRoot,
        out int expRoot)
    {
        origRoot = 0;
        expRoot = 0;
        PropertyTable origTable = PropertyTable.Read(state.OriginalContext("game"), start: originalStart, end: originalEnd, path: DocumentNodePath.Envelope);
        PropertyTable expTable = PropertyTable.Read(state.ExportedContext("game"), start: exportedStart, end: exportedEnd, path: DocumentNodePath.Envelope);
        if (origTable.Entries.Count != 1 || origTable.Entries[0].Id != 0 || expTable.Entries.Count != 1 || expTable.Entries[0].Id != 0)
        {
            state.Fail(originalStart, "Outer table does not hold only id 0.");
            return false;
        }

        TableEntry origEntry = origTable.Entries[0];
        TableEntry expEntry = expTable.Entries[0];
        if (origEntry.Offset != 0)
        {
            state.Fallback("outer-gap");
            return false;
        }

        if (expEntry.Offset != 0 || expEntry.Long || expTable.EmptyLongMarker)
        {
            state.Fail(originalStart, "Exported outer table is not canonical.");
            return false;
        }

        if (origEntry.Long || origTable.EmptyLongMarker)
        {
            int origLen = origTable.DataBase - originalStart;
            int expLen = expTable.DataBase - exportedStart;
            state.Mask(RuleTableEntries, Math.Max(origLen, expLen));
        }

        origRoot = origTable.DataBase + origEntry.Offset;
        expRoot = expTable.DataBase + expEntry.Offset;
        return true;
    }

    private sealed record EntrySlice(TableEntry Entry, SchemaProperty? Property, int Start, int End);

    private static void CompareObject(
        State state,
        string engine,
        SchemaClass classValue,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        if (state.Original.AsSpan(originalStart, originalEnd - originalStart)
            .SequenceEqual(state.Exported.AsSpan(exportedStart, exportedEnd - exportedStart)))
        {
            return;
        }

        PropertyTable origTable = PropertyTable.Read(state.OriginalContext(engine), start: originalStart, end: originalEnd, path: DocumentNodePath.Root);
        PropertyTable expTable = PropertyTable.Read(state.ExportedContext(engine), start: exportedStart, end: exportedEnd, path: DocumentNodePath.Root);

        int origBase = origTable.DataBase;
        int origLen = originalEnd - origBase;
        int[] origBoundaries = [.. origTable.Entries.Select(e => e.Offset).Append(origLen).Distinct().Order()];

        List<EntrySlice> origSlots = [with(origTable.Entries.Count)];
        List<EntrySlice> origSkipped = [];
        HashSet<int> origValuedOffsets = [];
        foreach (TableEntry entry in origTable.Entries)
        {
            if (classValue.IsSkippedId(entry.Id))
            {
                origSkipped.Add(new EntrySlice(entry, Property: null, Start: origBase + entry.Offset, End: origBase + entry.Offset));
                continue;
            }

            SchemaProperty? prop = classValue.PropertyById(entry.Id);
            if (prop is null)
            {
                state.Fail(origBase + entry.Offset, $"Class {classValue.Name} has no property {entry.Id}.");
                return;
            }

            int slotStart = origBase + entry.Offset;
            int slotEnd = slotStart;
            if (!prop.Codec.ZeroSize)
            {
                int next = Array.BinarySearch(origBoundaries, entry.Offset);
                slotEnd = origBase + (next + 1 < origBoundaries.Length ? origBoundaries[next + 1] : origLen);
                _ = origValuedOffsets.Add(entry.Offset);
            }

            origSlots.Add(new EntrySlice(entry, Property: prop, Start: slotStart, End: slotEnd));
        }

        // Compute spans of skipped entries: if no valued property starts at the skipped offset,
        // its data runs to the next boundary.
        long skippedDataBytes = 0;
        for (int i = 0; i < origSkipped.Count; i++)
        {
            EntrySlice skipped = origSkipped[i];
            if (!origValuedOffsets.Contains(skipped.Entry.Offset))
            {
                int next = Array.BinarySearch(origBoundaries, skipped.Entry.Offset);
                int skippedEnd = origBase + (next + 1 < origBoundaries.Length ? origBoundaries[next + 1] : origLen);
                origSkipped[i] = skipped with { End = skippedEnd };
                skippedDataBytes += skippedEnd - skipped.Start;
            }
        }

        // Check for unclaimed gap bytes in original data.
        int cursor = origBase;
        foreach (EntrySlice slice in origSlots.Concat(origSkipped).Where(s => s.End > s.Start).OrderBy(s => s.Start))
        {
            if (slice.Start > cursor)
            {
                state.Fallback("gaps");
                return;
            }

            cursor = Math.Max(cursor, slice.End);
        }

        if (cursor < originalEnd)
        {
            state.Fallback("gaps");
            return;
        }

        int expBase = expTable.DataBase;
        int expLen = exportedEnd - expBase;
        int[] expBoundaries = [.. expTable.Entries.Select(e => e.Offset).Append(expLen).Distinct().Order()];
        List<EntrySlice> expSlots = [with(expTable.Entries.Count)];
        foreach (TableEntry entry in expTable.Entries)
        {
            SchemaProperty? prop = classValue.PropertyById(entry.Id);
            if (prop is null)
            {
                state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: $"Exported class {classValue.Name} has unknown property {entry.Id}.");
                return;
            }

            int slotStart = expBase + entry.Offset;
            int slotEnd = slotStart;
            if (!prop.Codec.ZeroSize)
            {
                int next = Array.BinarySearch(expBoundaries, entry.Offset);
                slotEnd = expBase + (next + 1 < expBoundaries.Length ? expBoundaries[next + 1] : expLen);
            }

            expSlots.Add(new EntrySlice(entry, Property: prop, Start: slotStart, End: slotEnd));
        }

        if (origSlots.Count != expSlots.Count)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: $"Property count differs on {classValue.Name}.");
            return;
        }

        List<EntrySlice> origOrdered = [.. origSlots.OrderBy(s => s.Entry.Offset).ThenBy(s => s.End > s.Start ? 1 : 0).ThenBy(s => s.Entry.Index)];
        List<EntrySlice> expOrdered = [.. expSlots.OrderBy(s => s.Entry.Offset).ThenBy(s => s.End > s.Start ? 1 : 0).ThenBy(s => s.Entry.Index)];

        // Check whether properties appear in the same data order.
        bool sameDataOrder = true;
        for (int i = 0; i < origOrdered.Count; i++)
        {
            EntrySlice o = origOrdered[i];
            EntrySlice e = expOrdered[i];
            bool sameSlot = ReferenceEquals(o.Property, e.Property)
                && (o.Property is { IsDynamic: true, SlotBase: null } || o.Entry.Id == e.Entry.Id);
            if (!sameSlot)
            {
                sameDataOrder = false;
                break;
            }
        }

        if (!sameDataOrder)
        {
            // Verify the two objects carry the same set of properties before allowing document fallback.
            if (SamePropertyMultiset(origOrdered, expOrdered))
            {
                state.Fallback("data-order");
                return;
            }

            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: $"Property set differs on {classValue.Name}.");
            return;
        }

        // Compare each property value in order.
        for (int i = 0; i < origOrdered.Count; i++)
        {
            EntrySlice o = origOrdered[i];
            EntrySlice e = expOrdered[i];
            SchemaProperty prop = o.Property!;
            CompareValue(
                state,
                engine: engine,
                ownerClass: classValue,
                property: prop,
                entryId: o.Entry.Id,
                originalStart: o.Start,
                originalEnd: o.End,
                exportedStart: e.Start,
                exportedEnd: e.End);
            if (state.Stopped)
            {
                return;
            }
        }

        // Record table-level rules (skipped-property and table-entries).
        if (origSkipped.Count != 0)
        {
            long entryBytes = 0;
            foreach (EntrySlice skipped in origSkipped)
            {
                entryBytes += skipped.Entry.Long ? 8 : 2;
            }

            bool origHasLong = origTable.EmptyLongMarker || origTable.Entries.Any(e => e.Long);
            bool expHasLong = expTable.EmptyLongMarker || expTable.Entries.Any(e => e.Long);
            if (origHasLong && !expHasLong)
            {
                entryBytes += 4;
            }

            state.Mask(RuleSkippedProperty, entryBytes + skippedDataBytes);
        }

        // Check whether the remaining non-skipped table entries differ in form, order, empty-long-marker, or sequential ids.
        List<TableEntry> origNonSkippedTable = [.. origTable.Entries.Where(e => !classValue.IsSkippedId(e.Id))];
        long tableRuleBytes = CountTableRuleDifference(origNonSkippedTable, origTable.EmptyLongMarker, expTable.Entries, expTable.EmptyLongMarker, origOrdered, expOrdered);
        if (tableRuleBytes > 0)
        {
            state.Mask(RuleTableEntries, tableRuleBytes);
        }
    }

    private static bool SamePropertyMultiset(List<EntrySlice> left, List<EntrySlice> right)
    {
        Dictionary<(SchemaProperty Property, int Key), int> counts = [];
        foreach (EntrySlice item in left)
        {
            (SchemaProperty Property, int Key) key = (item.Property!, item.Property is { IsDynamic: true, SlotBase: null } ? -1 : item.Entry.Id);
            counts[key] = counts.GetValueOrDefault(key) + 1;
        }

        foreach (EntrySlice item in right)
        {
            (SchemaProperty Property, int Key) key = (item.Property!, item.Property is { IsDynamic: true, SlotBase: null } ? -1 : item.Entry.Id);
            if (!counts.TryGetValue(key, out int count) || count == 0)
            {
                return false;
            }

            counts[key] = count - 1;
        }

        return true;
    }

    private static long CountTableRuleDifference(
        List<TableEntry> origTableEntries,
        bool origEmptyLong,
        IReadOnlyList<TableEntry> expTableEntries,
        bool expEmptyLong,
        List<EntrySlice> origOrdered,
        List<EntrySlice> expOrdered)
    {
        // Map each original entry index to its paired exported entry index (matching by position in data order).
        Dictionary<int, TableEntry> paired = [with(origOrdered.Count)];
        for (int i = 0; i < origOrdered.Count; i++)
        {
            paired[origOrdered[i].Entry.Index] = expOrdered[i].Entry;
        }

        long bytes = 0;
        if (origEmptyLong != expEmptyLong)
        {
            bytes += 4;
        }

        for (int i = 0; i < origTableEntries.Count; i++)
        {
            TableEntry o = origTableEntries[i];
            TableEntry expected = expTableEntries[i];
            TableEntry pairedExp = paired[o.Index];

            // Position in the table differs, or short/long form differs (beyond offset growth forcing long),
            // or a sequential dynamic id was renumbered.
            bool samePosition = pairedExp.Index == expected.Index;
            bool formDiffers = o.Long != pairedExp.Long && !(o.Long && !PropertyTable.IsShort(o.Id, o.Offset));
            bool idDiffers = o.Id != pairedExp.Id;
            if (!samePosition || formDiffers)
            {
                bytes += o.Long ? 8 : 2;
            }
            else if (idDiffers)
            {
                bytes += o.Long ? 4 : 1;
            }
        }

        return bytes;
    }

    private static void CompareValue(
        State state,
        string engine,
        SchemaClass ownerClass,
        SchemaProperty property,
        int entryId,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        ReadOnlySpan<byte> origSpan = state.Original.AsSpan(originalStart, originalEnd - originalStart);
        ReadOnlySpan<byte> expSpan = state.Exported.AsSpan(exportedStart, exportedEnd - exportedStart);
        if (origSpan.SequenceEqual(expSpan))
        {
            return;
        }

        switch (property.Codec)
        {
            case ImageLibraryCodec:
                CompareIlb(state, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case FixedObjectCodec fixedObject:
                CompareObject(state, engine: engine, classValue: state.Schema.Get(fixedObject.ClassName), originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case CreatedObjectCodec:
                CompareCreated(state, engine: engine, property: property, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case ChildrenCodec:
                CompareChildren(state, engine: engine, property: property, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case MapFieldsCodec:
                CompareMapFields(state, engine: engine, ownerClass: ownerClass, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case CampaignMapCodec:
                CompareCampaignMapEntry(state, property: property, entryId: entryId, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case StringCodec:
                CompareString(state, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            case RunListCodec:
            case GroupedListCodec:
            case ImageSequencesCodec:
                CompareValueEncoding(state, engine: engine, property: property, originalStart: originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd);
                return;

            default:
                state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: $"Property {ownerClass.Name}.{property.Name} differs.");
                return;
        }
    }

    private static void CompareIlb(State state, int originalStart, int originalEnd, int exportedStart, int exportedEnd)
    {
        ReadOnlySpan<byte> orig = state.Original.AsSpan(originalStart, originalEnd - originalStart);
        ReadOnlySpan<byte> exp = state.Exported.AsSpan(exportedStart, exportedEnd - exportedStart);
        if (orig.Length != exp.Length || ImageLibraryCodec.Problem(orig) is not null || ImageLibraryCodec.Problem(exp) is not null)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "ILB library differs.");
            return;
        }

        List<int> spriteDwords = [];
        _ = ImageLibraryCodec.TryFindSpriteReservedOffsets(orig, spriteDwords);
        spriteDwords.Sort();

        // Walk byte ranges between allowed masks: header bytes 5..7, then each sprite reserved dword.
        int cursor = 0;
        if (!CheckEqualSegment(state, originalStart: originalStart, exportedStart: exportedStart, from: cursor, to: 5))
        {
            return;
        }

        ReadOnlySpan<byte> origPad = orig.Slice(5, 3);
        ReadOnlySpan<byte> expPad = exp.Slice(5, 3);
        if (!origPad.SequenceEqual(expPad))
        {
            if (expPad.ContainsAnyExcept((byte)0))
            {
                state.Fail(originalStart + 5 + origPad.CommonPrefixLength(expPad), "Exported ILB header padding is not zero.");
                return;
            }

            state.Mask(RuleIlbHeaderPadding, 3);
        }

        cursor = 8;
        foreach (int offset in spriteDwords)
        {
            if (offset < cursor || offset + 4 > orig.Length)
            {
                continue;
            }

            if (!CheckEqualSegment(state, originalStart: originalStart, exportedStart: exportedStart, from: cursor, to: offset))
            {
                return;
            }

            ReadOnlySpan<byte> origDword = orig.Slice(offset, 4);
            ReadOnlySpan<byte> expDword = exp.Slice(offset, 4);
            if (!origDword.SequenceEqual(expDword))
            {
                if (expDword.ContainsAnyExcept((byte)0))
                {
                    state.Fail(originalStart + offset + origDword.CommonPrefixLength(expDword), "Exported ILB sprite reserved dword is not zero.");
                    return;
                }

                state.Mask(RuleIlbSpriteDword, 4);
            }

            cursor = offset + 4;
        }

        _ = CheckEqualSegment(state, originalStart: originalStart, exportedStart: exportedStart, from: cursor, to: orig.Length);
    }

    private static bool CheckEqualSegment(State state, int originalStart, int exportedStart, int from, int to)
    {
        if (to <= from)
        {
            return true;
        }

        ReadOnlySpan<byte> origSeg = state.Original.AsSpan(originalStart + from, to - from);
        ReadOnlySpan<byte> expSeg = state.Exported.AsSpan(exportedStart + from, to - from);
        if (origSeg.SequenceEqual(expSeg))
        {
            return true;
        }

        state.Fail(originalStart + from + origSeg.CommonPrefixLength(expSeg), "ILB library differs outside declared don't-care regions.");
        return false;
    }

    private static void CompareCreated(
        State state,
        string engine,
        SchemaProperty property,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        if (originalEnd - originalStart < 4 || exportedEnd - exportedStart < 4)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Created object class id is truncated.");
            return;
        }

        int origClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(originalStart));
        int expClassId = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(exportedStart));
        if (origClassId != expClassId)
        {
            state.Fail(originalStart + state.Original.AsSpan(originalStart, 4).CommonPrefixLength(state.Exported.AsSpan(exportedStart, 4)), "Created object class id differs.");
            return;
        }

        if (origClassId == 0)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Nil object differs.");
            return;
        }

        SchemaClass? classValue = state.Schema.Resolve(engine, origClassId);
        if (classValue is null)
        {
            state.Fail(originalStart, "Unknown created object class id.");
            return;
        }

        (string Rule, SchemaClass Shape)? legacy = state.Schema.LegacyShape(property.Owner.Name + "." + property.Name, classValue.Name);
        CompareObject(state, engine: engine, classValue: legacy?.Shape ?? classValue, originalStart: originalStart + 4, originalEnd: originalEnd, exportedStart: exportedStart + 4, exportedEnd: exportedEnd);
    }

    private static void CompareChildren(
        State state,
        string engine,
        SchemaProperty property,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        PropertyTable origTable = PropertyTable.Read(state.OriginalContext(engine), start: originalStart, end: originalEnd, path: DocumentNodePath.Root);
        PropertyTable expTable = PropertyTable.Read(state.ExportedContext(engine), start: exportedStart, end: exportedEnd, path: DocumentNodePath.Root);
        if (origTable.Entries.Count != expTable.Entries.Count)
        {
            state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Children count differs.");
            return;
        }

        int origBase = origTable.DataBase;
        int origLen = originalEnd - origBase;
        int[] origBounds = [.. origTable.Entries.Select(e => e.Offset).Append(origLen).Distinct().Order()];
        List<EntrySlice> origOrdered = [with(origTable.Entries.Count)];
        int cursor = 0;
        foreach (TableEntry entry in origTable.Entries.OrderBy(e => e.Offset).ThenBy(e => e.Index))
        {
            if (entry.Offset > cursor)
            {
                state.Fallback("children-gaps");
                return;
            }

            int next = origBounds[Array.BinarySearch(origBounds, entry.Offset) + 1];
            origOrdered.Add(new EntrySlice(entry, Property: property, Start: origBase + entry.Offset, End: origBase + next));
            cursor = next;
        }

        if (cursor < origLen)
        {
            state.Fallback("children-gaps");
            return;
        }

        int expBase = expTable.DataBase;
        int expLen = exportedEnd - expBase;
        int[] expBounds = [.. expTable.Entries.Select(e => e.Offset).Append(expLen).Distinct().Order()];
        List<EntrySlice> expOrdered = [with(expTable.Entries.Count)];
        foreach (TableEntry entry in expTable.Entries.OrderBy(e => e.Offset).ThenBy(e => e.Index))
        {
            int next = expBounds[Array.BinarySearch(expBounds, entry.Offset) + 1];
            expOrdered.Add(new EntrySlice(entry, Property: property, Start: expBase + entry.Offset, End: expBase + next));
        }

        for (int i = 0; i < origOrdered.Count; i++)
        {
            if (origOrdered[i].Entry.Id != expOrdered[i].Entry.Id)
            {
                state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Children order differs.");
                return;
            }

            CompareCreated(
                state,
                engine: engine,
                property: property,
                originalStart: origOrdered[i].Start,
                originalEnd: origOrdered[i].End,
                exportedStart: expOrdered[i].Start,
                exportedEnd: expOrdered[i].End);
            if (state.Stopped)
            {
                return;
            }
        }

        long tableRuleBytes = CountTableRuleDifference([.. origTable.Entries], origTable.EmptyLongMarker, expTable.Entries, expTable.EmptyLongMarker, origOrdered, expOrdered);
        if (tableRuleBytes > 0)
        {
            state.Mask(RuleTableEntries, tableRuleBytes);
        }
    }

    private static void CompareMapFields(
        State state,
        string engine,
        SchemaClass ownerClass,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        SchemaClass fieldClass = state.Schema.Get(ownerClass.FieldClass ?? "MapField");
        int origAt = originalStart;
        int expAt = exportedStart;
        while (origAt < originalEnd && expAt < exportedEnd)
        {
            if (origAt + 4 > originalEnd || expAt + 4 > exportedEnd)
            {
                state.FailLeaf(origAt, originalEnd: originalEnd, exportedStart: expAt, exportedEnd: exportedEnd, detail: "Map field length is truncated.");
                return;
            }

            int origLength = BinaryPrimitives.ReadInt32LittleEndian(state.Original.AsSpan(origAt));
            int expLength = BinaryPrimitives.ReadInt32LittleEndian(state.Exported.AsSpan(expAt));
            if (origLength <= 0 || origAt + 4L + origLength > originalEnd || expLength <= 0 || expAt + 4L + expLength > exportedEnd)
            {
                state.Fail(origAt, "Map field length is invalid.");
                return;
            }

            CompareObject(
                state,
                engine: engine,
                classValue: fieldClass,
                originalStart: origAt + 4,
                originalEnd: origAt + 4 + origLength,
                exportedStart: expAt + 4,
                exportedEnd: expAt + 4 + expLength);
            if (state.Stopped)
            {
                return;
            }

            origAt += 4 + origLength;
            expAt += 4 + expLength;
        }

        if (origAt != originalEnd || expAt != exportedEnd)
        {
            state.FailLeaf(origAt, originalEnd: originalEnd, exportedStart: expAt, exportedEnd: exportedEnd, detail: "Map field count differs.");
        }
    }

    private static void CompareCampaignMapEntry(
        State state,
        SchemaProperty property,
        int entryId,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        // Size (id 5) or compressed bytes (id 6): deflate output can differ in length and bytes
        // even for identical inflated payloads, so verify via document fallback.
        if (entryId == property.Id && originalEnd - originalStart == 4 && exportedEnd - exportedStart == 4)
        {
            state.Fallback("nested-compression");
            return;
        }

        if (entryId == property.DataId
            && originalEnd - originalStart >= 5
            && exportedEnd - exportedStart >= 5
            && Cfs.HasMagic(state.Original.AsSpan(originalStart))
            && state.Original.AsSpan(originalStart, 5).SequenceEqual(state.Exported.AsSpan(exportedStart, 5))
            && state.Original[originalStart + 4] != 0)
        {
            state.Fallback("nested-compression");
            return;
        }

        state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "Campaign map entry differs.");
    }

    private static void CompareString(State state, int originalStart, int originalEnd, int exportedStart, int exportedEnd)
    {
        // A long 5-byte prefix (0xFF + 4-byte length) for a string shorter than 255 bytes.
        ReadOnlySpan<byte> orig = state.Original.AsSpan(originalStart, originalEnd - originalStart);
        ReadOnlySpan<byte> exp = state.Exported.AsSpan(exportedStart, exportedEnd - exportedStart);
        if (orig.Length >= 5 && orig[0] == 255 && exp.Length >= 1 && exp[0] < 255)
        {
            int origLen = BinaryPrimitives.ReadInt32LittleEndian(orig[1..]);
            if (origLen == exp[0] && orig.Length == 5 + origLen && exp.Length == 1 + origLen && orig[5..].SequenceEqual(exp[1..]))
            {
                state.Fallback("long-string-prefix");
                return;
            }
        }

        state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: "String value differs.");
    }

    private static void CompareValueEncoding(
        State state,
        string engine,
        SchemaProperty property,
        int originalStart,
        int originalEnd,
        int exportedStart,
        int exportedEnd)
    {
        try
        {
            ValueSite origSite = new() { Property = property, Id = property.Id ?? 0, Path = DocumentNodePath.Root, Owner = new JsonObject() };
            JsonValue origDoc = property.Codec.Decode(state.OriginalContext(engine), start: originalStart, end: originalEnd, site: origSite);
            ValueSite expSite = new() { Property = property, Id = property.Id ?? 0, Path = DocumentNodePath.Root, Owner = new JsonObject() };
            JsonValue expDoc = property.Codec.Decode(state.ExportedContext(engine), start: exportedStart, end: exportedEnd, site: expSite);
            if (JsonWriter.ToUtf8(origDoc).AsSpan().SequenceEqual(JsonWriter.ToUtf8(expDoc)))
            {
                state.Fallback("value-encoding");
                return;
            }
        }
        catch (ConversionException)
        {
            // Fall through to mismatch below.
        }

        state.FailLeaf(originalStart, originalEnd: originalEnd, exportedStart: exportedStart, exportedEnd: exportedEnd, detail: $"Property {property.Owner.Name}.{property.Name} differs.");
    }

    private sealed class State(byte[] original, byte[] exported, FormatSchema schema, FormatLimits limits)
    {
        public byte[] Original { get; } = original;
        public byte[] Exported { get; } = exported;
        public FormatSchema Schema { get; } = schema;
        public FormatLimits Limits { get; } = limits;
        public Dictionary<string, long> MaskedBytesByRule { get; } = [with(StringComparer.Ordinal)];
        public long? MismatchOffset { get; private set; }
        public string? MismatchDetail { get; private set; }
        public bool NeedsDocumentFallback { get; private set; }
        public string? FallbackReason { get; private set; }

        public bool Stopped => MismatchOffset is not null || NeedsDocumentFallback;

        public DecodeContext OriginalContext(string engine) => new(Original, Schema, engine, "g1", file: null, limits: Limits);

        public DecodeContext ExportedContext(string engine) => new(Exported, Schema, engine, "g1", file: null, limits: Limits);

        public void Mask(string rule, long bytes)
        {
            if (bytes > 0)
            {
                MaskedBytesByRule[rule] = MaskedBytesByRule.GetValueOrDefault(rule) + bytes;
            }
        }

        public void Fallback(string reason)
        {
            NeedsDocumentFallback = true;
            FallbackReason ??= reason;
        }

        public void Fail(long offset, string detail)
        {
            MismatchOffset = offset;
            MismatchDetail = detail;
        }

        public void FailLeaf(int originalStart, int originalEnd, int exportedStart, int exportedEnd, string detail)
        {
            int common = Original.AsSpan(originalStart, originalEnd - originalStart)
                .CommonPrefixLength(Exported.AsSpan(exportedStart, exportedEnd - exportedStart));
            Fail(originalStart + common, detail);
        }
    }
}
