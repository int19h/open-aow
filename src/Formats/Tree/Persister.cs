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

    /// <summary>Frees an object the load created and then dropped. The default leaves it to the caller.</summary>
    void Release(IPersistable obj) { }
}

/// <summary>
/// The load's error handler. <paramref name="continueLoad"/> starts true.
/// The handler sets it false to stop the load. A null handler continues.
/// </summary>
public delegate void LoadErrorHandler(string message, ref bool continueLoad);

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

    void Short(ref short value);

    void UInt(ref uint value);

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
/// element, including zeros. <see cref="RawInt"/>, <see cref="RawByte"/>, <see cref="RawSByte"/>
/// and <see cref="RawBool"/> store rwData values, zero and false included.
/// <see cref="BitArray"/> stores the bits whenever the byte length is nonzero.
/// <see cref="Object"/>, <see cref="Created"/> and
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

    void Short(string name, ref short value, short fallback = 0);

    void UInt(string name, ref uint value, uint fallback = 0);

    void Enum<TEnum>(string name, ref TEnum value, TEnum fallback = default) where TEnum : struct, Enum;

    void Set<E>(string name, ref Set<E> value) where E : unmanaged, Enum;

    void Bool(string name, ref bool value);

    void String(string name, ref string value);

    void Lines(string name, IList<string> lines);

    void LinesWithObjects(string name, IList<string> lines, IList<int> objects);

    void Bytes(string name, Span<byte> data);

    /// <summary>An rwData integer. The value is stored even when it is zero. An absent member on load leaves the caller's value.</summary>
    void RawInt(string name, ref int value);

    /// <summary>An rwData byte. The value is stored even when it is zero. An absent member on load leaves the caller's value.</summary>
    void RawByte(string name, ref byte value);

    /// <summary>An rwData signed byte. The value is stored even when it is zero. An absent member on load leaves the caller's value.</summary>
    void RawSByte(string name, ref sbyte value);

    /// <summary>
    /// An rwData bool. The byte is stored even when false. 0 and 1 load as false and true.
    /// Any other byte stays that byte, so a stored 02 writes back as 02.
    /// An absent member on load leaves the caller's value.
    /// </summary>
    void RawBool(string name, ref bool value);

    /// <summary>
    /// A counted bit array stored by rwData. The length is <c>(count + 7) &gt;&gt; 3</c>.
    /// Length zero is omitted. Bits past the count in the last byte are residue.
    /// An absent member on load leaves the caller's bytes.
    /// </summary>
    void BitArray(string name, int count, Span<byte> bits);

    void Object(string name, IPersistable obj);

    void Created(string name, ref IPersistable? obj);

    void Created<T>(string name, ref T? obj) where T : class, IPersistable;

    void Children(string name, IChildCollection children);

    void Reference(string name, string domain, ref int key, int fallback = 0);

    void Record(string name, Action<IPersister> body);

    void List<T>(string name, IList<T> items, ItemTransfer<T> transfer);
}
