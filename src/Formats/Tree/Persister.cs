using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// One stored object. <see cref="Persist"/> is the stored image, including children of a node.
/// The virtual field method of an engine object is the body of <c>ReadWrite</c>.
/// </summary>
public interface IPersistable
{
    void Persist(IPersister persister);
}

/// <summary>
/// Creates an object from its schema class name and reports that name. The engine registry
/// supplies the implementation. Formats does not name an engine type.
/// </summary>
public interface IPersistableFactory
{
    string ClassName(IPersistable obj);

    IPersistable Create(string className);
}

/// <summary>
/// The children of one node, in stored order. A stored child has its owner, is not shared and
/// is marked stored. The slot is the child's index. <see cref="Place"/> puts a loaded child
/// back, at that index when <see cref="KeepIndex"/> is set and otherwise at the end.
/// </summary>
public interface IChildCollection
{
    int Count { get; }

    IPersistable? Get(int index);

    bool IsStored(int index);

    int Slot(int index);

    bool KeepIndex { get; }

    /// <summary>Sets the owner for the load of <paramref name="child"/> before <see cref="Place"/>.</summary>
    void Attach(IPersistable child);

    void Place(int slot, IPersistable child);
}

/// <summary>One value of a variable-length list. The call stores exactly one value.</summary>
public interface IItemSlot
{
    void Int(ref int value);

    void Byte(ref byte value);

    void SByte(ref sbyte value);

    void UShort(ref ushort value);

    void Bool(ref bool value);

    void String(ref string value);

    void Record(Action<IPersister> body);

    void Object(ref IPersistable? value);
}

/// <summary>Transfers one list item in either direction.</summary>
public delegate void ItemTransfer<T>(IItemSlot slot, ref T value);

/// <summary>
/// One symmetric serializer. A call names a member by its schema name and runs for both
/// directions. On load, an absent member takes the fallback. On save, a value equal to the
/// fallback is omitted. <see cref="Bool"/> is present only when true. An empty string or an
/// empty list is omitted. <see cref="Bytes"/> of length zero is omitted, and an absent value
/// leaves the caller's span unchanged. <see cref="Enum{TEnum}"/> and <see cref="Set{E}"/> are
/// always stored, including zero, because the original writes every positive-size image. Their
/// fallback applies only when the member is absent on load. A fixed integer array stores every
/// element, including zeros. <see cref="Object"/>, <see cref="Created"/> and
/// <see cref="Children"/> are always stored. <see cref="Record"/> is stored whenever the
/// serializer calls it.
/// </summary>
public interface IPersister
{
    bool Loading { get; }

    IPersistableFactory Factory { get; }

    /// <summary>True on load when the member is present. False on save.</summary>
    bool Has(string name);

    void Int(string name, ref int value, int fallback = 0);

    void Byte(string name, ref byte value, byte fallback = 0);

    void SByte(string name, ref sbyte value, sbyte fallback = 0);

    void UShort(string name, ref ushort value, ushort fallback = 0);

    void Enum<TEnum>(string name, ref TEnum value, TEnum fallback = default) where TEnum : struct, Enum;

    void Set<E>(string name, ref TSet<E> value) where E : unmanaged, Enum;

    void Bool(string name, ref bool value);

    void String(string name, ref string value);

    void Lines(string name, IList<string> lines);

    void LinesWithObjects(string name, IList<string> lines, IList<int> objects);

    void Bytes(string name, Span<byte> data);

    void Object(string name, IPersistable obj);

    void Created(string name, ref IPersistable? obj);

    void Created<T>(string name, ref T? obj) where T : class, IPersistable;

    void Children(string name, IChildCollection children);

    void Reference(string name, string domain, ref int key, int fallback = 0);

    void Record(string name, Action<IPersister> body);

    void List<T>(string name, IList<T> items, ItemTransfer<T> transfer);
}
