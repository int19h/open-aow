using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.Engine;

// The engine side of the persistence tree. Formats does not name these types.
public sealed class EnginePersistableFactory : IPersistableFactory
{
    public static EnginePersistableFactory Instance { get; } = new();

    private readonly Dictionary<string, EClass> byName;

    public EnginePersistableFactory()
    {
        byName = new Dictionary<string, EClass>();
        foreach (var eclass in EClass.All)
            if (!byName.TryAdd(eclass.ClassName, eclass))
                throw new InvalidOperationException("Two engine classes are named " + eclass.ClassName + ".");
    }

    public string ClassName(IPersistable obj) => TObject.ClassNameOf(obj.GetType());

    public IPersistable Create(string className) =>
        byName.TryGetValue(className, out var eclass) ? eclass.Create(null) : throw new InvalidOperationException("The engine has no class " + className + ".");
}

public sealed class NodeChildren(TECustomNode node) : IChildCollection
{
    public int Count => node.GetCount();

    public IPersistable Get(int index) => node.GetChild(index);

    public bool IsStored(int index)
    {
        var child = node.GetChild(index);
        return child != null && child.Owner == node
            && !child.GetControlStyle().Contains(EngineUnit.csShared)
            && child.GetControlStyle().Contains(EngineUnit.csStored);
    }

    public int Slot(int index) => index;

    public bool KeepIndex => node.GetControlStyle().Contains(EngineUnit.csKeepIndex);

    public void Attach(IPersistable child) => ((TEObject)child).FOwner = node;

    public void Place(int slot, IPersistable child)
    {
        var obj = (TEObject)child;
        obj.FOwner = null;
        if (KeepIndex) obj.SetArrayOwner(node, slot);
        else obj.SetOwner(node);
    }
}

public static class PersistSupport
{
    public static void UnsignedByte(IPersister persister, string name, ref sbyte value)
    {
        byte stored = unchecked((byte)value);
        persister.Byte(name, ref stored);
        value = unchecked((sbyte)stored);
    }

    public static void Set<E>(IPersister persister, string name, ref TSet<E> value) where E : unmanaged
    {
        ulong image = value.Image;
        Span<byte> bytes = stackalloc byte[1];
        bytes[0] = (byte)image;
        persister.Bytes(name, bytes);
        if (!persister.Loading) return;
        image = (image & ~0xFFul) | bytes[0];
        value = TSet<E>.FromMask(image);
    }

    public static void ByteImage<T>(IPersister persister, string name, ref T value) where T : unmanaged
    {
        Span<byte> bytes = stackalloc byte[1];
        bytes[0] = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[0];
        persister.Bytes(name, bytes);
        if (persister.Loading) MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref value, 1))[0] = bytes[0];
    }

    public static void Lines(IPersister persister, string name, TStrings lines)
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

    public static void LinesEx(IPersister persister, string name, TStrings lines)
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
