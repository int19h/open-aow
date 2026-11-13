using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Game.Draw.Images;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

public static class PersistSupport
{
    /// <summary>The engine of a tree load, which <c>ReadWrite</c> reads as <c>Stream.Engine</c>.</summary>
    public static GameEngine Engine(IPersister persister) =>
        (persister.Factory as IEnginePersistableFactory)?.Engine
            ?? throw new InvalidOperationException("The load's factory names no engine.");

    /// <summary>The progress handler of a tree load, which <c>ReadWrite</c> reads as <c>Stream.ProgressHandler</c>. Null when the load has none.</summary>
    public static ProgressEvent Progress(IPersister persister) => (persister.Factory as IEnginePersistableFactory)?.Progress;

    /// <summary>Reports load progress the way <c>Stream.ShowProgress</c> does: to the load's handler, when it has one.</summary>
    public static void ShowProgress(IPersister persister, EngineObject sender, string message, int percent) =>
        Progress(persister)?.Invoke(sender, Message: message, Progression: percent);

    /// <summary>
    /// A catalog record. The value moves through its I/O image, which maps it without loss and
    /// which the record catalog describes under the image's bound record name.
    /// </summary>
    public static TValue Record<TImage, TValue>(IPersister persister, string name, TValue value) where TImage : unmanaged, IRecordImage<TImage, TValue> =>
        RecordValues.Transfer(persister, name, TImage.From(value)).ToValue();

    /// <summary>A signed byte stored as an unsigned one (Delphi <c>rwByte</c> on a shortint), with its unsigned fallback.</summary>
    public static sbyte UnsignedByte(IPersister persister, string name, sbyte value, byte fallback = 0) =>
        unchecked((sbyte)persister.Byte(name, unchecked((byte)value), fallback));

    public static void Int32Array(IPersister persister, string name, Span<int> values)
    {
        var list = new List<int>(values.Length);
        for (int i = 0; i < values.Length; i++) list.Add(values[i]);
        persister.List(name, list, static (IItemSlot slot, int item) => slot.Int32(item));
        if (!persister.Loading) return;
        if (list.Count > values.Length)
            throw new InvalidOperationException(name + " holds at most " + values.Length + " values.");
        for (int i = 0; i < values.Length; i++) values[i] = i < list.Count ? list[i] : 0;
    }

    public static void UInt16Array(IPersister persister, string name, Span<ushort> values)
    {
        var list = new List<ushort>(values.Length);
        for (int i = 0; i < values.Length; i++) list.Add(values[i]);
        persister.List(name, list, static (IItemSlot slot, ushort item) => slot.UInt16(item));
        if (!persister.Loading) return;
        if (list.Count > values.Length)
            throw new InvalidOperationException(name + " holds at most " + values.Length + " values.");
        for (int i = 0; i < values.Length; i++) values[i] = i < list.Count ? list[i] : (ushort)0;
    }

    /// <summary>
    /// A TIntegerList stream (Delphi <c>rwStreamProc</c> with <c>LoadFromStream</c> and
    /// <c>SaveToStream</c>). The default and the items are stored, an empty list included. A
    /// present member replaces the list and its default, as <c>LoadFromStream</c> does. An absent
    /// member leaves the list as it was.
    /// </summary>
    public static void Int32List(IPersister persister, string name, Int32List list)
    {
        bool present = persister.Has(name);
        var items = new List<int>(list.Count);
        for (int i = 0; i < list.Count; i++) items.Add(list[i]);
        int stored = persister.Int32List(name, items, list.Default);
        if (!persister.Loading || !present) return;
        list.Clear();
        list.Default = stored;
        list.SetCapacity(items.Count);
        foreach (int item in items) list.Add(item);
    }

    /// <summary>A TByteList stream, with the presence of <see cref="Int32List"/>.</summary>
    public static void ByteList(IPersister persister, string name, ByteList list)
    {
        bool present = persister.Has(name);
        var items = new List<byte>(list.Count);
        for (int i = 0; i < list.Count; i++) items.Add(list[i]);
        byte stored = persister.ByteList(name, items, list.Default);
        if (!persister.Loading || !present) return;
        list.Clear();
        list.Default = stored;
        list.SetCapacity(items.Count);
        foreach (byte item in items) list.Add(item);
    }

    /// <summary>
    /// A TUnitIndexList stream. The image is the TIntegerList image, and the constructor's default
    /// is -1. A stored default that differs from it is kept.
    /// </summary>
    public static void UnitIndexList(IPersister persister, string name, UnitIndexList list) =>
        Int32List(persister, name, list);

    /// <summary>
    /// A TUnitIndexListCollection stream: the flat TUnitIndexList of its groups. The document
    /// cuts it into groups of the schema's size.
    /// </summary>
    public static void UnitIndexListCollection(IPersister persister, string name, UnitIndexListCollection collection) =>
        Int32List(persister, name, collection.Units);

    /// <summary>
    /// A TBitList stream. The save writes what <c>SaveToStream</c> writes. A present member loads as
    /// <c>LoadFromStream</c> does, and an absent member leaves the list as it was. A load returns
    /// the stored image, for <see cref="BitListOverRead"/>, and null when the member is absent.
    /// </summary>
    public static BitListImage BitList(IPersister persister, string name, BitList list)
    {
        if (persister.BitList(name, value: list.SaveImage()) is not { } stored || !persister.Loading) return null;
        list.LoadImage(stored);
        return stored;
    }

    /// <summary>
    /// The bytes <c>TBitList.LoadFromStream</c> reads past a stored bit list. <c>SetCapacity</c> puts
    /// the default into each new bit through <c>Put</c>, and <c>Put</c> past the count raises the
    /// count, so the count is the capacity, a multiple of 32, when <c>Stream.Read(FList^, (FCount +
    /// 7) shr 3)</c> reads the bits (Engine.pas 4743 to 4789). The read takes up to 3 bytes past the
    /// stored bits: the data the owner stores after the list. The original backend writes that
    /// data from the members <paramref name="following"/> persists, in the owner's stored class,
    /// and those bytes become the bits past the stored ones, as the stream load leaves them. A
    /// following datum shorter than the read leaves the rest of the bits at the default.
    /// </summary>
    public static void BitListOverRead(IPersister persister, BitList list, BitListImage stored, EngineObject owner, Action<IPersister> following)
    {
        int extra = ((list.Count + 7) >> 3) - stored.Bits.Length;
        if (extra <= 0) return;
        var node = TreeWriter.Write(new MemberGroup(following), factory: persister.Factory);
        var schema = FormatSchema.Current;
        var data = OriginalBackend.WriteData(node, classValue: schema.Get(NameTable.Stored(owner.GetType())), schema: schema, engine: "game");
        var bits = list.List.Span;
        for (int i = 0; i < extra && i < data.Length; i++) bits[stored.Bits.Length + i] = data[i];
    }

    // Some members of an owner, persisted as an object of their own.
    private sealed class MemberGroup(Action<IPersister> body) : IPersistable
    {
        public void Persist(IPersister persister) => body(persister);
    }

    /// <summary>
    /// A TImageSequenceList stream (Delphi <c>rwStreamProc</c> with <c>LoadFromStream</c> and
    /// <c>SaveToStream</c>). The save reduces the count first, as <c>SaveToStream</c> does. A
    /// present member replaces the sequences, and each frame-table layer draws its image index
    /// from <c>Random</c> in entry and layer order, at the point of the load where
    /// <c>LoadFromStream</c> draws it. An absent member leaves the list as it was.
    /// </summary>
    public static void ImageSequences(IPersister persister, string name, ImageSequenceList list)
    {
        if (!persister.Loading)
        {
            persister.ImageSequences(name, entries: list.SaveLayers());
            return;
        }
        if (!persister.Has(name)) return;
        var entries = new List<IReadOnlyList<SequenceLayer>>();
        persister.ImageSequences(name, entries: entries);
        list.LoadLayers(entries);
    }

    /// <summary>
    /// A TTerrainList stream. The save packs the trailing pairs of none first, as
    /// <c>SaveToStream</c> does. A present member replaces the pairs, an absent member leaves them.
    /// </summary>
    public static void TerrainList(IPersister persister, string name, TerrainList list)
    {
        var pairs = new List<(sbyte First, sbyte Second)>();
        if (!persister.Loading)
        {
            list.Pack();
            for (int i = 0; i < list.Count; i++) pairs.Add((list.GetTerrain(i), list.GetOverlay(i)));
            persister.SBytePairs(name, pairs: pairs);
            return;
        }
        if (!persister.Has(name)) return;
        persister.SBytePairs(name, pairs: pairs);
        list.Count = pairs.Count;
        for (int i = 0; i < pairs.Count; i++)
        {
            list.SetTerrain(i, pairs[i].First);
            list.SetOverlay(i, pairs[i].Second);
        }
    }

    /// <summary>
    /// An ILB library stream. The save writes the library as <c>SaveToStream</c> writes it, and a
    /// present member loads through <c>LoadFromStream</c>. An absent member leaves the library.
    /// </summary>
    public static void ImageLibrary(IPersister persister, string name, CustomImageLibrary library)
    {
        if (!persister.Loading)
        {
            using var output = new MemoryStream();
            library.SaveToStream(output);
            persister.Blob(name, data: output.ToArray());
            return;
        }
        if (persister.Blob(name, data: []) is not { } stored) return;
        using var input = new MemoryStream(stored, writable: false);
        library.LoadFromStream(input);
    }

    public static void Lines(IPersister persister, string name, StringCollection lines)
    {
        var copy = new List<string>(lines.Count);
        for (int i = 0; i < lines.Count; i++) copy.Add(lines[i]);
        persister.Lines(name, copy);
        if (!persister.Loading) return;
        lines.BeginUpdate();
        try
        {
            lines.Clear();
            foreach (var line in copy) lines.Add(line);
        }
        finally
        {
            lines.EndUpdate();
        }
    }

    public static void LinesWithValues(IPersister persister, string name, StringCollection lines)
    {
        var copy = new List<string>(lines.Count);
        var objects = new List<int>(lines.Count);
        for (int i = 0; i < lines.Count; i++)
        {
            copy.Add(lines[i]);
            objects.Add(ObjectToInt(lines.Objects[i]));
        }
        persister.LinesWithObjects(name, copy, objects);
        if (!persister.Loading) return;
        lines.BeginUpdate();
        try
        {
            lines.Clear();
            for (int i = 0; i < copy.Count; i++) lines.AddObject(copy[i], IntToObject(objects[i]));
        }
        finally
        {
            lines.EndUpdate();
        }
    }
}
