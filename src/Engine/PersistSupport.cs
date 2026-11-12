
using Aow.Engine.Collections;
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Game.Units;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

public static class PersistSupport
{
    /// <summary>The engine of a tree load, which <c>ReadWrite</c> reads as <c>Stream.Engine</c>.</summary>
    public static GameEngine Engine(IPersister persister) =>
        (persister.Factory as IEnginePersistableFactory)?.Engine
            ?? throw new InvalidOperationException("The load's factory names no engine.");

    public static T Record<T>(IPersister persister, string name, T value) where T : unmanaged =>
        RecordValues.Transfer(persister, name, value);

    public static sbyte UnsignedByte(IPersister persister, string name, sbyte value) =>
        unchecked((sbyte)persister.Byte(name, unchecked((byte)value)));

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
