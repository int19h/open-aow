#nullable enable
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Engine.Storage;

/// <summary>
/// The translate stream's visit of one object whose class persists only through the tree
/// (issue 185 PE-3-i). <see cref="EngineObject.MainPersist"/> runs with this persister, which
/// neither loads nor saves: a string goes through <see cref="TranslateStorageStream.TranslateString"/>,
/// a string list (<see cref="PersistSupport.Lines"/>) through
/// <see cref="TranslateStorageStream.TranslateStrings"/>, an object, a created object and the
/// stored children go to the stream, which visits a child that still has <c>ReadWrite</c> through
/// <c>MainReadWrite</c>. Every other value comes back as it was. A tree method whose original
/// <c>ReadWrite</c> visits other members under the translate stream's mode (read, text only,
/// translate) asks <see cref="PersistSupport.Translating"/>.
/// </summary>
internal sealed class TranslatePersister(TranslateStorageStream stream, EngineObject node) : IPersister
{
    public bool Loading => false;

    public bool Preview => false;

    public IPersistableFactory Factory { get; } = new EnginePersistableFactory(stream.Engine ?? node.GetEngine());

    // The string list of a member, translated in place as the stream's Lines and LinesWithObjects
    // translate it.
    internal void Lines(StringCollection lines) => stream.TranslateStrings(ref lines);

    public bool Has(string name) => false;

    public int Int32(string name, int value, int fallback = 0) => value;

    public byte Byte(string name, byte value, byte fallback = 0) => value;

    public sbyte SByte(string name, sbyte value, sbyte fallback = 0) => value;

    public ushort UInt16(string name, ushort value, ushort fallback = 0) => value;

    public short Int16(string name, short value, short fallback = 0) => value;

    public uint UInt32(string name, uint value, uint fallback = 0) => value;

    public TEnum Enum<TEnum>(string name, TEnum value, TEnum fallback = default) where TEnum : struct, Enum => value;

    public Set<E> Set<E>(string name, Set<E> value) where E : unmanaged, Enum => value;

    public bool Boolean(string name, bool value) => value;

    public string String(string name, string value)
    {
        stream.TranslateString(ref value);
        return value;
    }

    // PersistSupport.Lines and LinesWithValues hand the stream the string collection itself.
    public void Lines(string name, IList<string> lines) =>
        throw new InvalidOperationException("The translate stream takes the string collection of " + name + " through PersistSupport.Lines.");

    public void LinesWithObjects(string name, IList<string> lines, IList<int> objects) =>
        throw new InvalidOperationException("The translate stream takes the string collection of " + name + " through PersistSupport.LinesWithValues.");

    public void Bytes(string name, Span<byte> data)
    {
    }

    public int RawInt32(string name, int value) => value;

    public byte RawByte(string name, byte value) => value;

    public sbyte RawSByte(string name, sbyte value) => value;

    public bool RawBool(string name, bool value) => value;

    public void BitArray(string name, int count, Span<byte> bits)
    {
    }

    public BitListImage? BitList(string name, BitListImage value) => value;

    public float Single(string name, float value) => value;

    public byte[]? Blob(string name, byte[] data) => data;

    public void SBytePairs(string name, IList<(sbyte First, sbyte Second)> pairs)
    {
    }

    public void ImageSequences(string name, IList<IReadOnlyList<SequenceLayer>?> entries)
    {
    }

    public void Object(string name, IPersistable objectValue) => stream.Object((EngineObject)objectValue, 0);

    public IPersistable? Created(string name, IPersistable? objectValue)
    {
        if (objectValue is not null) stream.Object((EngineObject)objectValue, 0);
        return objectValue;
    }

    public T? Created<T>(string name, T? objectValue) where T : class, IPersistable => (T?)Created(name, (IPersistable?)objectValue);

    public IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue) => Created(name, objectValue);

    public T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable => (T?)Created(name, (IPersistable?)objectValue);

    public void ObjectSlot(string name, int slot, IPersistable objectValue) => Object(name, objectValue);

    public void Skip(string name)
    {
    }

    public int Int32List(string name, IList<int> items, int defaultValue) => defaultValue;

    public byte ByteList(string name, IList<byte> items, byte defaultValue) => defaultValue;

    // The stream visits the children the node owns, as ReadWriteChildren hands the node to it. A
    // reference list's children without an owner are not visited, as the stream does not visit them.
    public void Children(string name, IChildCollection children) => stream.Children((ECustomNode)node, 1);

    public void Grid(string name, int width, int height, IReadOnlyList<IPersistable> fields, Action<int>? visited = null)
    {
        for (int index = 0; index < fields.Count; index++)
        {
            Object(name, fields[index]);
            visited?.Invoke(index);
        }
    }

    public void CreatedSequence(string name, IChildCollection items)
    {
        for (int index = 0; index < items.Count; index++)
            if (items.IsStored(index)) Created(name, items.Get(index));
    }

    public byte[] NestedDocument(string name, string profile, byte[] image) => image;

    public int Reference(string name, string domain, int key, int fallback = 0) => key;

    // A record and a list item are bytes to the stream, which visits no string inside them.
    public void Record(string name, Action<IPersister> body)
    {
    }

    public void List<T>(string name, IList<T> items, ItemTransfer<T> transfer)
    {
    }
}
