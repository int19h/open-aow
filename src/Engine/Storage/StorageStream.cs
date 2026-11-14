// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Schema;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Storage;

// the base of every storage stream. The value methods do nothing here. the subclasses read,
// write or translate
public partial class StorageStream : IDisposable
{

    private protected GameEngine _engine;
    private protected Stream _stream;
    private protected bool _errorHandled;
    private protected PropertyTable _propertyTable;
    public event StorageStreamError OnError;

    protected void RaiseError(StorageStream storageStream, string message, ref bool handled)
    {
        if (OnError != null)
        {
            OnError(storageStream, message, ref handled);
        }
    }

    protected bool HasErrorHandlers => OnError != null;

    internal void SetErrorHandler(StorageStreamError handler) => OnError = handler;

    public event ProgressEvent OnProgress;

    // A map read through this stream reads its hex sprite set through a stream of its own, and
    // that stream reports progress to the handler of this one (GameMap.ReadWrite).
    internal ProgressEvent ProgressHandler => OnProgress;

    // Engine.pas:1184
    public virtual Set<StorageModeElement> StorageMode()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:9502
    public virtual Stream OpenProperty(int id) => null;

    // Engine.pas:1186
    public virtual void Created(ref EngineObject objectValue, ECustomNode owner, int id)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1187
    public virtual void Object(EngineObject objectValue, int id)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1188
    public virtual void SizedObject(EngineObject objectValue, int id)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1189
    public virtual void Children(ECustomNode node, int id)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:9559
    public virtual void Payload(int id, StreamProc load, StreamProc save)
    {
    }

    // Engine.pas:9507
    public virtual void Bytes(Span<byte> buffer, int size, int id)
    {
    }

    // Engine.pas:9511
    public virtual void String(ref string value, int id)
    {
    }

    // Engine.pas:9515
    public virtual void Lines(StringCollection lines, int id)
    {
    }

    // Engine.pas:9519
    public virtual void LinesWithObjects(StringCollection lines, int id)
    {
    }

    // Engine.pas:9523
    public virtual void Int32(ref int value, int id)
    {
    }

    // Engine.pas:9527
    public virtual void Byte(ref byte value, int id)
    {
    }

    // Engine.pas:9535
    public virtual void UInt16(ref ushort value, int id)
    {
    }

    // Engine.pas:9539
    public virtual void Boolean(ref bool value, int id)
    {
    }

    // Engine.pas:9531
    public virtual void SByte(ref sbyte value, int id)
    {
    }

    // Engine.pas:9543
    public virtual void Int32(ref int value, int fallback, int id)
    {
    }

    // Engine.pas:9547
    public virtual void Byte(ref byte value, byte fallback, int id)
    {
    }

    // Engine.pas:9551
    public virtual void UInt16(ref ushort value, ushort fallback, int id)
    {
    }

    // Engine.pas:9555
    public virtual void SByte(ref sbyte value, sbyte fallback, int id)
    {
    }

    // Engine.pas:9489
    public StorageStream(Stream stream, GameEngine engine)
    {
        _engine = engine;
        _stream = stream;
        _errorHandled = false;
    }

    // Engine.pas:9497
    public void ShowProgress(EngineObject sender, string message, int percent)
    {
        if (OnProgress != null)
        {
            OnProgress(sender, message, percent);
        }
    }

    public GameEngine Engine { get => _engine; }
    public Stream Stream { get => _stream; }
    public PropertyTable PropertyTable { get => _propertyTable; }

    // Delphi var parameters alias a descendant reference through a base-class cast.
    public void Created<T>(ref T value, ECustomNode owner, int id) where T : EngineObject
    {
        EngineObject result = value;
        try { Created(ref result, owner, id); }
        finally { value = (T)result; }
    }

    public virtual void Dispose()
    {
    }

    // The stored image of one object: MainReadWrite, or the tree for a class whose stream
    // methods are retired (EngineObject.StreamRetired). The writer and the reader call it where
    // the original calls MainReadWrite, with the object's own property table in place.
    internal void PersistObject(EngineObject objectValue)
    {
        if (EngineObject.StreamRetired(objectValue.GetType()))
            PersistTree(objectValue);
        else
            objectValue.MainReadWrite(this);
    }

    private protected virtual void PersistTree(EngineObject objectValue) => objectValue.MainReadWrite(this);

    // The engine table that resolves the class ids inside the image of a class: the strategic
    // table when it enlists the class, otherwise the first table that does.
    private protected static string ClassTable(SchemaClass classValue) =>
        classValue.Engines.Contains("game") || classValue.Engines.Count == 0 ? "game" : classValue.Engines[0];

    public delegate void StorageStreamError(StorageStream stream, string message, ref bool handled);
}
