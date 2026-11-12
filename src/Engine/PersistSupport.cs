
using Aow.Formats.Tree;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Engine;

public static class PersistSupport
{
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
