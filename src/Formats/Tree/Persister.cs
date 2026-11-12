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
    string ClassName(IPersistable objectValue);

    IPersistable Create(string className);

    /// <summary>Frees an object the load created and then dropped. The default leaves it to the caller.</summary>
    void Release(IPersistable objectValue) { }
}

/// <summary>
/// The load's error handler. The handler sets <paramref name="continueLoad"/> true to go on
/// or false to stop the load. A null handler continues.
/// </summary>
public delegate void LoadErrorHandler(string message, out bool continueLoad);

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

/// <summary>
/// One value of a variable-length list. The call stores exactly one value. A value call takes the
/// value to save and returns the value loaded, or the same value on save.
/// </summary>
public interface IItemSlot
{
    int Int32(int value);

    byte Byte(byte value);

    sbyte SByte(sbyte value);

    ushort UInt16(ushort value);

    short Int16(short value);

    uint UInt32(uint value);

    bool Boolean(bool value);

    string String(string value);

    void Record(Action<IPersister> body);

    IPersistable? Object(IPersistable? value);
}

/// <summary>Transfers one list item in either direction. It returns the item to keep.</summary>
public delegate T ItemTransfer<T>(IItemSlot slot, T value);

/// <summary>
/// One symmetric serializer. A call names a member by its schema name and runs for both
/// directions. A value call takes the value to save and returns the value loaded, or the same
/// value on save, so the caller assigns the result: <c>Gold = persister.Int32("Gold", Gold)</c>.
/// On load, an absent member takes the fallback. On save, a value equal to the
/// fallback is omitted. <see cref="Boolean"/> is present only when true. An empty string or an
/// empty list is omitted. <see cref="Bytes"/> of length zero is omitted, and an absent value
/// leaves the caller's span unchanged. <see cref="Enum{TEnum}"/> and <see cref="Set{E}"/> are
/// always stored, including zero, because the original writes every positive-size image. Their
/// fallback applies only when the member is absent on load. A fixed integer array stores every
/// element, including zeros. <see cref="RawInt32"/>, <see cref="RawByte"/>, <see cref="RawSByte"/>
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

    int Int32(string name, int value, int fallback = 0);

    byte Byte(string name, byte value, byte fallback = 0);

    sbyte SByte(string name, sbyte value, sbyte fallback = 0);

    ushort UInt16(string name, ushort value, ushort fallback = 0);

    short Int16(string name, short value, short fallback = 0);

    uint UInt32(string name, uint value, uint fallback = 0);

    TEnum Enum<TEnum>(string name, TEnum value, TEnum fallback = default) where TEnum : struct, Enum;

    Set<E> Set<E>(string name, Set<E> value) where E : unmanaged, Enum;

    bool Boolean(string name, bool value);

    string String(string name, string value);

    void Lines(string name, IList<string> lines);

    void LinesWithObjects(string name, IList<string> lines, IList<int> objects);

    void Bytes(string name, Span<byte> data);

    /// <summary>An rwData integer. The value is stored even when it is zero. An absent member on load returns the caller's value.</summary>
    int RawInt32(string name, int value);

    /// <summary>An rwData byte. The value is stored even when it is zero. An absent member on load returns the caller's value.</summary>
    byte RawByte(string name, byte value);

    /// <summary>An rwData signed byte. The value is stored even when it is zero. An absent member on load returns the caller's value.</summary>
    sbyte RawSByte(string name, sbyte value);

    /// <summary>
    /// An rwData bool. The byte is stored even when false. 0 and 1 load as false and true.
    /// Any other byte stays that byte, so a stored 02 writes back as 02.
    /// An absent member on load returns the caller's value.
    /// </summary>
    bool RawBool(string name, bool value);

    /// <summary>
    /// A counted bit array stored by rwData. The length is <c>(count + 7) &gt;&gt; 3</c>.
    /// Length zero is omitted. Bits past the count in the last byte are residue.
    /// An absent member on load leaves the caller's bytes.
    /// </summary>
    void BitArray(string name, int count, Span<byte> bits);

    /// <summary>
    /// A bit list (Delphi <c>TBitList</c>, stored by <c>rwStreamProc</c>). The member is always
    /// stored, an empty list included. On load a present member returns the stored image and an
    /// absent member returns null, so the caller leaves its list as it was.
    /// </summary>
    BitListImage? BitList(string name, BitListImage value);

    /// <summary>
    /// An rwData single (Delphi <c>Single</c>). The value is stored even when it is zero, with its
    /// exact bits. An absent member on load returns the caller's value.
    /// </summary>
    float Single(string name, float value);

    /// <summary>
    /// A whole payload that a callback reads and writes (Delphi <c>rwStreamProc</c> with an ILB
    /// library). The member is always stored, empty bytes included. On load a present member
    /// returns the stored bytes and an absent member returns null, so the caller leaves its object
    /// as it was.
    /// </summary>
    byte[]? Blob(string name, byte[] data);

    /// <summary>
    /// Pairs of signed bytes in order (Delphi <c>TTerrainList</c>). The member is always stored,
    /// an empty list included. On load a present member replaces the pairs and an absent member
    /// leaves them.
    /// </summary>
    void SBytePairs(string name, IList<(sbyte First, sbyte Second)> pairs);

    /// <summary>
    /// An image sequence list (Delphi <c>TImageSequenceList</c>): one entry per index, null for an
    /// index without a sequence, otherwise the layers of its chain. The member is always stored,
    /// an empty list included. On load a present member replaces the entries and an absent member
    /// leaves them. The caller draws the random image index of each frame-table layer as it builds
    /// the chains, in entry and layer order, where <c>LoadFromStream</c> draws it.
    /// </summary>
    void ImageSequences(string name, IList<IReadOnlyList<SequenceLayer>?> entries);

    void Object(string name, IPersistable objectValue);

    IPersistable? Created(string name, IPersistable? objectValue);

    T? Created<T>(string name, T? objectValue) where T : class, IPersistable;

    /// <summary>
    /// One created object of a keyed dynamic member, stored at <paramref name="slot"/>. On save the
    /// calls of one member add their slots in call order, a null object stores the stored null, and
    /// a slot stored twice is an error. On load the call returns the object stored at that slot,
    /// or null when the slot is absent or holds the stored null. A slot the load does not ask for
    /// is left unread, as <c>ReadWrite</c> leaves an id it does not ask for.
    /// </summary>
    IPersistable? CreatedSlot(string name, int slot, IPersistable? objectValue);

    T? CreatedSlot<T>(string name, int slot, T? objectValue) where T : class, IPersistable;

    /// <summary>
    /// One fixed object of a keyed dynamic member (Delphi <c>TAbilityControl</c> and
    /// <c>TSpellControl</c>), stored at <paramref name="slot"/> and persisted in place, as
    /// <see cref="Object"/> persists one. On save the calls of one member add their slots in call
    /// order, and a slot stored twice is an error. On load a present slot loads into the object
    /// with the <see cref="Object"/> error rule, and an absent slot leaves it. A slot the load does
    /// not ask for is left unread, as <c>ReadWrite</c> leaves an id it does not ask for.
    /// </summary>
    void ObjectSlot(string name, int slot, IPersistable objectValue);

    /// <summary>
    /// On load, consumes a member without reading it, the way <c>ReadWrite</c> leaves a stored
    /// property unread. A member already read stays as it is. On save it stores nothing.
    /// </summary>
    void Skip(string name);

    /// <summary>
    /// A list with a stored default of signed 32-bit values (Delphi <c>TIntegerList</c>). The
    /// member is always stored, an empty list included. On load a present member replaces the items
    /// and returns the stored default. An absent member leaves the items as they are and returns
    /// <paramref name="defaultValue"/>.
    /// </summary>
    int Int32List(string name, IList<int> items, int defaultValue);

    /// <summary>A list with a stored default of bytes (Delphi <c>TByteList</c>), with the presence of <see cref="Int32List"/>.</summary>
    byte ByteList(string name, IList<byte> items, byte defaultValue);

    void Children(string name, IChildCollection children);

    /// <summary>
    /// The fields of a grid (Delphi <c>TMapLevel</c> property 6): <paramref name="height"/> rows of
    /// <paramref name="width"/> fixed objects, in row order, each persisted in place as
    /// <see cref="Object"/> persists one. The member is always stored on save, an empty grid
    /// included. On load an absent member leaves the fields as they are, and a present one needs
    /// the same width and height. <paramref name="visited"/> runs after each field with its index,
    /// on save and on load, so the caller reports progress where <c>ReadWrite</c> does.
    /// </summary>
    void Grid(string name, int width, int height, IReadOnlyList<IPersistable> fields, Action<int>? visited = null);

    /// <summary>
    /// The created objects of a dynamic member without slots (Delphi <c>TMapField</c> sprites), in
    /// stored order. On save every stored item of <paramref name="items"/> is written in index order,
    /// and the member is absent when no item is stored. On load each stored object is created,
    /// attached, persisted and placed in stored order, through the rules of <see cref="Created"/>,
    /// and a stored null is skipped. The stored id carries no identity, so the slot is not stored.
    /// </summary>
    void CreatedSequence(string name, IChildCollection items);

    /// <summary>
    /// A whole original file of <paramref name="profile"/> held as bytes (Delphi
    /// <c>TAoWCampaign</c> ids 5 and 6). Empty bytes are omitted on save. On load an absent member
    /// returns empty bytes and a present one returns the stored file.
    /// </summary>
    byte[] NestedDocument(string name, string profile, byte[] image);

    int Reference(string name, string domain, int key, int fallback = 0);

    void Record(string name, Action<IPersister> body);

    void List<T>(string name, IList<T> items, ItemTransfer<T> transfer);
}
