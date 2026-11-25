#nullable disable warnings
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Engine;

/// <summary>The original width of a stored map coordinate.</summary>
public enum CoordinateWidth
{
    /// <summary>An unsigned byte, the two's complement byte of an sbyte.</summary>
    Byte,

    /// <summary>A signed 32-bit integer in the range -128 to 127.</summary>
    Int32,
}

public enum IdWidth
{
    UnsignedByte,
    Byte,
    SByte,
    Int32,
    UInt16,
}

public static class PersistSupport
{
    /// <summary>The engine of a tree load, which <c>ReadWrite</c> reads as <c>Stream.Engine</c>.</summary>
    public static GameEngine Engine(this IPersister persister)
        => (persister.Factory as IEnginePersistableFactory)?.Engine
            ?? throw new InvalidOperationException("The load's factory names no engine.");

    /// <summary>The progress handler of a tree load, which <c>ReadWrite</c> reads as <c>Stream.ProgressHandler</c>. Null when the load has none.</summary>
    public static ProgressEvent? Progress(this IPersister persister) => (persister.Factory as IEnginePersistableFactory)?.Progress;

    /// <summary>Reports load progress the way <c>Stream.ShowProgress</c> does: to the load's handler, when it has one.</summary>
    public static void ShowProgress(this IPersister persister, EngineObject sender, string message, int percent)
        => persister.Progress()?.Invoke(sender, Message: message, Progression: percent);

    /// <summary>
    /// A catalog record. The value moves through its I/O image, which maps it without loss and
    /// which the record catalog describes under the image's bound record name.
    /// </summary>
    public static TValue Record<TImage, TValue>(this IPersister persister, string name, TValue value) where TImage : unmanaged, IRecordImage<TImage, TValue>
        => RecordValues.Transfer(persister, name, TImage.From(value)).ToValue();

    /// <summary>A signed byte stored as an unsigned one (Delphi <c>rwByte</c> on a shortint), with its unsigned fallback.</summary>
    public static int UnsignedByte(this IPersister persister, string name, int value, int fallback = 0)
        => persister.Byte(name, unchecked((byte)value), fallback: fallback.TruncateToByte()).TruncateToSByte();

    /// <summary>
    /// A required id. Loading the type's exact stored none value throws <see cref="InvalidOperationException"/>
    /// instead of producing a present sentinel id. A width with no distinct stored none has no refusal.
    /// </summary>
    public static T Id<T>(this IPersister persister, string name, T value, IdWidth width, int fallback = 0) where T : struct, IOriginalId<T>
    {
        int stored = Transfer(persister, name, value: value.OriginalValue, width, fallback: fallback);
        int? storedNone = StoredNone<T>(width);
        return persister.Loading && value is IOptionalOriginalValue && stored == storedNone
            ? throw new InvalidOperationException($"{name} holds the none value {stored} for required {typeof(T).Name}.")
            : persister.Loading ? T.FromInt32(stored) : value;
    }

    public static T? OptionalId<T>(this IPersister persister, string name, T? value, IdWidth width, int? fallback = null) where T : struct, IOptionalOriginalId<T>
    {
        int storedNone = StoredNone<T>(width) ?? throw new InvalidOperationException($"{typeof(T).Name} none value {T.NoneValue} has no distinct stored form at {width}.");
        int stored = Transfer(persister, name, value: value?.OriginalValue ?? T.NoneValue, width, fallback: fallback ?? T.NoneValue);
        return !persister.Loading ? value : stored == storedNone ? null : T.FromInt32(stored);
    }

    public static PlayerId Player(this IPersister persister, string name, PlayerId value, IdWidth width, int fallback = 0)
        => persister.Id(name, value: value, width, fallback: fallback);

    public static PlayerId? OptionalPlayer(this IPersister persister, string name, PlayerId? value, IdWidth width, int? fallback = null)
        => persister.OptionalId(name, value: value, width, fallback: fallback);

    private static int Transfer(IPersister persister, string name, int value, IdWidth width, int fallback) => width switch
    {
        IdWidth.UnsignedByte => persister.UnsignedByte(name, value, fallback: fallback),
        IdWidth.Byte => persister.Byte(name, unchecked((byte)value), fallback: fallback.TruncateToByte()),
        IdWidth.SByte => persister.SByte(name, value, fallback),
        IdWidth.Int32 => persister.Int32(name, value, fallback),
        IdWidth.UInt16 => persister.UInt16(name, unchecked((ushort)value), fallback: unchecked((ushort)fallback)),
        _ => throw new ArgumentOutOfRangeException(nameof(width)),
    };

    private static int? StoredNone<T>(IdWidth width) where T : struct, IOriginalId<T>
    {
        return width switch
        {
            IdWidth.UnsignedByte or IdWidth.Byte or IdWidth.SByte when T.FromOriginal(unchecked((byte)T.NoneValue)) is not null => null,
            IdWidth.UnsignedByte or IdWidth.SByte => unchecked((sbyte)T.NoneValue),
            IdWidth.Byte => unchecked((byte)T.NoneValue),
            IdWidth.Int32 => T.NoneValue,
            // The census stores ability absence as 65535. A present 65535 collides and loads as null
            // through OptionalId. Required Id refuses that stored value.
            IdWidth.UInt16 when typeof(T) == typeof(AbilityId) && T.NoneValue == -1 => 65_535,
            IdWidth.UInt16 when T.FromOriginal(unchecked((ushort)T.NoneValue)) is not null => null,
            IdWidth.UInt16 when T.NoneValue is >= -1 and <= 65_535 => unchecked((ushort)T.NoneValue),
            _ => null,
        };
    }

    /// <summary>
    /// A map coordinate stored as X, Y and MapLevel. Byte width is an unsigned byte.
    /// Int32 width is a signed 32-bit integer, and a loaded value outside -128 to 127 is refused.
    /// </summary>
    public static MapCoordinate Coordinate(this IPersister persister, string name, MapCoordinate value, CoordinateWidth width)
    {
        MapCoordinate loaded = value;
        persister.Record(name, body =>
        {
            if (width == CoordinateWidth.Byte)
            {
                int x = body.UnsignedByte("X", value.X);
                int y = body.UnsignedByte("Y", value.Y);
                int level = body.UnsignedByte("MapLevel", value.MapLevel);
                if (body.Loading)
                {
                    loaded = new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)level);
                }
            }
            else
            {
                int x = body.Int32("X", value.X);
                int y = body.Int32("Y", value.Y);
                int level = body.Int32("MapLevel", value.MapLevel);
                if (body.Loading)
                {
                    loaded = new MapCoordinate((sbyte)InRange(name, component: "X", stored: x), (sbyte)InRange(name, component: "Y", stored: y), (sbyte)InRange(name, component: "MapLevel", stored: level));
                }
            }
        });
        return persister.Loading ? loaded : value;
    }

    private static int InRange(string name, string component, int stored)
    {
        return stored is < -128 or > 127
            ? throw new InvalidOperationException(string.Create(System.Globalization.CultureInfo.InvariantCulture, handler: $"{name}.{component} is {stored}, outside -128 to 127."))
            : stored.TruncateToSByte();
    }

    public static void Int32Array(this IPersister persister, string name, Span<int> values)
    {
        List<int> list = [.. values];

        persister.List(name, items: list, static (slot, item) => slot.Int32(item));
        if (!persister.Loading)
        {
            return;
        }

        if (list.Count > values.Length)
        {
            throw new InvalidOperationException($"{name} holds at most {values.Length} values.");
        }

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = i < list.Count ? list[i] : 0;
        }
    }

    public static void UInt16Array(this IPersister persister, string name, Span<ushort> values)
    {
        List<ushort> list = [.. values];

        persister.List(name, items: list, static (slot, item) => slot.UInt16(item));
        if (!persister.Loading)
        {
            return;
        }

        if (list.Count > values.Length)
        {
            throw new InvalidOperationException($"{name} holds at most {values.Length} values.");
        }

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = i < list.Count ? list[i] : (ushort)0;
        }
    }

    /// <summary>
    /// A TIntegerList stream (Delphi <c>rwStreamProc</c> with <c>LoadFromStream</c> and
    /// <c>SaveToStream</c>). The default and the items are stored, an empty list included. A
    /// present member replaces the list and its default, as <c>LoadFromStream</c> does. An absent
    /// member leaves the list as it was.
    /// </summary>
    public static void Int32List(this IPersister persister, string name, Int32List list)
    {
        bool present = persister.Has(name);
        List<int> items = [with(list.Count)];
        items.AddRange(list);
        int stored = persister.Int32List(name, items, defaultValue: list.Default);
        if (!persister.Loading || !present)
        {
            return;
        }

        LoadPayload(() =>
        {
            list.Clear();
            list.Default = stored;
            list.SetCapacity(items.Count);
            foreach (int item in items)
            {
                _ = list.Add(item);
            }
        });
    }

    /// <summary>A TByteList stream, with the presence of <see cref="Int32List"/>.</summary>
    public static void ByteList(this IPersister persister, string name, ByteList list)
    {
        bool present = persister.Has(name);
        List<byte> items = [with(list.Count)];
        items.AddRange(list);
        int stored = persister.ByteList(name, items, defaultValue: list.Default);
        if (!persister.Loading || !present)
        {
            return;
        }

        LoadPayload(() =>
        {
            list.Clear();
            list.Default = (byte)stored;
            list.SetCapacity(items.Count);
            foreach (int item in items)
            {
                _ = list.Add(item);
            }
        });
    }

    /// <summary>
    /// A TUnitIndexList stream. The image is the TIntegerList image, and the constructor's default
    /// is -1. A stored default that differs from it is kept.
    /// </summary>
    public static void UnitIndexList(this IPersister persister, string name, UnitIndexList list)
        => persister.Int32List(name, list);

    /// <summary>
    /// A TUnitIndexListCollection stream: the flat TUnitIndexList of its groups. The document
    /// cuts it into groups of the schema's size.
    /// </summary>
    public static void UnitIndexListCollection(this IPersister persister, string name, UnitIndexListCollection collection)
        => persister.Int32List(name, list: collection.Units);

    /// <summary>
    /// A TBitList stream. The save writes what <c>SaveToStream</c> writes. A present member loads as
    /// <c>LoadFromStream</c> does, and an absent member leaves the list as it was. A load returns
    /// the stored image, for <see cref="BitListOverRead"/>, and null when the member is absent.
    /// </summary>
    public static BitListImage? BitList(this IPersister persister, string name, BitList? list)
    {
        if (persister.BitList(name, value: list.SaveImage()) is not { } stored || !persister.Loading)
        {
            return null;
        }

        LoadPayload(() => list.LoadImage(stored));
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
    public static void BitListOverRead(this IPersister persister, BitList? list, BitListImage stored, EngineObject owner, Action<IPersister> following)
    {
        int extra = ((list.Count + 7) >> 3) - stored.Bits.Length;
        if (extra <= 0)
        {
            return;
        }

        ObjectNode node = TreeWriter.Write(new MemberGroup(following), factory: persister.Factory);
        FormatSchema schema = FormatSchema.Current;
        byte[] data = OriginalBackend.WriteData(node, classValue: schema.Get(NameTable.Stored(owner.GetType())), schema: schema, engine: "game");
        Span<byte> bits = list.List.Span;
        for (int i = 0; i < extra && i < data.Length; i++)
        {
            bits[stored.Bits.Length + i] = data[i];
        }
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
    /// <c>LoadFromStream</c> draws it. An absent member leaves the list as it was. The translate
    /// stream visits no payload (the original stream procedure does nothing there), so the
    /// list is not reduced either.
    /// </summary>
    public static void ImageSequences(this IPersister persister, string name, ImageSequenceList? list)
    {
        if (persister.Translating())
        {
            return;
        }

        if (!persister.Loading)
        {
            persister.ImageSequences(name, entries: list.SaveLayers());
            return;
        }
        if (!persister.Has(name))
        {
            return;
        }

        List<IReadOnlyList<SequenceLayer>?> entries = [];
        persister.ImageSequences(name, entries: entries);
        LoadPayload(() => list.LoadLayers(entries));
    }

    /// <summary>
    /// A TTerrainList stream. The save packs the trailing pairs of none first, as
    /// <c>SaveToStream</c> does. A present member replaces the pairs, an absent member leaves them.
    /// The translate stream visits no payload, so it packs nothing.
    /// </summary>
    public static void TerrainList(this IPersister persister, string name, TerrainList list)
    {
        if (persister.Translating())
        {
            return;
        }

        List<(sbyte First, sbyte Second)> pairs = [];
        if (!persister.Loading)
        {
            list.Pack();
            for (int i = 0; i < list.Count; i++)
            {
                pairs.Add((list.GetTerrain(i), list.GetOverlay(i)));
            }

            persister.SBytePairs(name, pairs: pairs);
            return;
        }
        if (!persister.Has(name))
        {
            return;
        }

        persister.SBytePairs(name, pairs: pairs);
        LoadPayload(() =>
        {
            list.Count = pairs.Count;
            for (int i = 0; i < pairs.Count; i++)
            {
                list.SetTerrain(i, value: pairs[i].First);
                list.SetOverlay(i, value: pairs[i].Second);
            }
        });
    }

    /// <summary>
    /// An ILB library stream. The save writes the library as <c>SaveToStream</c> writes it, and a
    /// present member loads through <c>LoadFromStream</c>. An absent member leaves the library.
    /// The translate stream visits no payload.
    /// </summary>
    public static void ImageLibrary(this IPersister persister, string name, CustomImageLibrary library)
    {
        if (persister.Translating())
        {
            return;
        }

        if (!persister.Loading)
        {
            using MemoryStream output = new();
            library.SaveToStream(output);
            _ = persister.Blob(name, data: output.ToArray());
            return;
        }
        if (persister.Blob(name, data: []) is not { } stored)
        {
            return;
        }

        using MemoryStream input = new(stored, writable: false);
        LoadPayload(() => library.LoadFromStream(input));
    }

    /// <summary>
    /// The load of a stream callback's payload. The original stream procedure runs the load
    /// procedure inside an exception block with an empty handler (Engine.pas 10157), so a payload
    /// that fails to load keeps what its load did and the object's load goes on.
    /// </summary>
    private static void LoadPayload(Action load)
    {
        try
        {
            load();
        }
        catch (Exception error)
        {
            // rwStreamProc ignores the failure.
            HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
        }
    }

    /// <summary>
    /// True when the translate stream visits the object (<see cref="TranslatePersister"/>). The
    /// original <c>ReadWrite</c> ran under that stream's mode (read, text only, translate), and a
    /// tree method asks this where that mode visited other members than a save does.
    /// </summary>
    public static bool Translating(this IPersister persister) => persister is TranslatePersister;

    /// <summary>
    /// Sets the error handler of the translate stream that visits the object, as the original
    /// <c>ReadWrite</c> of a map in the editor sets it on its stream. Other persisters take the
    /// handler their caller passes to the load.
    /// </summary>
    public static void SetTranslateErrorHandler(this IPersister persister, TranslateVisit.TranslateErrorHandler handler)
    {
        if (persister is TranslatePersister translate)
        {
            translate.Stream.SetErrorHandler(handler);
        }
    }

    public static void Lines(this IPersister persister, string name, DelphiStringCollection? lines)
    {
        if (persister is TranslatePersister translate)
        {
            translate.Lines(lines);
            return;
        }

        List<string> copy = [with(lines.Count)];
        copy.AddRange(lines);
        persister.Lines(name, lines: copy);
        if (!persister.Loading)
        {
            return;
        }

        lines.BeginUpdate();
        try
        {
            lines.Clear();
            foreach (string line in copy)
            {
                _ = lines.Add(line);
            }
        }
        finally
        {
            lines.EndUpdate();
        }
    }

    public static void LinesWithValues(this IPersister persister, string name, DelphiStringCollection lines)
    {
        if (persister is TranslatePersister translate)
        {
            translate.Lines(lines);
            return;
        }

        List<string> copy = [with(lines.Count)];
        List<int> objects = [with(lines.Count)];
        for (int i = 0; i < lines.Count; i++)
        {
            copy.Add(lines[i]);
            objects.Add(ObjectToInt(lines.Objects[i]));
        }
        persister.LinesWithObjects(name, lines: copy, objects);
        if (!persister.Loading)
        {
            return;
        }

        lines.BeginUpdate();
        try
        {
            lines.Clear();
            for (int i = 0; i < copy.Count; i++)
            {
                _ = lines.AddObject(copy[i], item: IntToObject(objects[i]));
            }
        }
        finally
        {
            lines.EndUpdate();
        }
    }
}
