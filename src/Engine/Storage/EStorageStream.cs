// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Storage;

// the base of every storage stream. The rw* methods do nothing here. the subclasses read,
// write, compare or translate
public partial class EStorageStream : IDisposable
{

    internal GameEngine FEngine;
    internal Stream FStream;
    internal bool FErrorHandled;
    internal PropertyTable FPropertyTable;
    internal StorageStreamError FOnError;
    internal ProgressEvent FOnProgress;

    // Engine.pas:1184
    public virtual Set<StorageModeElement> StorageMode()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:9502
    public virtual Stream OpenPropertyStream(int ID) => null;

    // Engine.pas:1186
    public virtual void rwCreateEObject(ref EngineObject Obj, ECustomNode AOwner, int ID)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1187
    public virtual void rwEObject(EngineObject Obj, int ID)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1188
    public virtual void rwEObjectStream(EngineObject Obj, int ID)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:1189
    public virtual void rwChildren(ECustomNode Node, int ID)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:9559
    public virtual void rwStreamProc(int ID, StreamProc LoadProc, StreamProc SaveProc)
    {
    }

    // Engine.pas:9507
    public virtual void rwData(Span<byte> Buffer, int Size, int ID)
    {
    }

    // Engine.pas:9511
    public virtual void rwString(ref string S, int ID)
    {
    }

    // Engine.pas:9515
    public virtual void rwStrings(StringCollection Strings, int ID)
    {
    }

    // Engine.pas:9519
    public virtual void rwStringsEx(StringCollection Strings, int ID)
    {
    }

    // Engine.pas:9523
    public virtual void rwInteger(ref int I, int ID)
    {
    }

    // Engine.pas:9527
    public virtual void rwByte(ref byte B, int ID)
    {
    }

    // Engine.pas:9535
    public virtual void rwWord(ref ushort W, int ID)
    {
    }

    // Engine.pas:9539
    public virtual void rwBool(ref bool B, int ID)
    {
    }

    // Engine.pas:9531
    public virtual void rwShortint(ref sbyte S, int ID)
    {
    }

    // Engine.pas:9543
    public virtual void rwIntegerEx(ref int I, int Default, int ID)
    {
    }

    // Engine.pas:9547
    public virtual void rwByteEx(ref byte B, byte Default, int ID)
    {
    }

    // Engine.pas:9551
    public virtual void rwWordEx(ref ushort W, ushort Default, int ID)
    {
    }

    // Engine.pas:9555
    public virtual void rwShortintEx(ref sbyte S, sbyte Default, int ID)
    {
    }

    // Engine.pas:9489
    public EStorageStream(Stream AStream, GameEngine AEngine)
    {
        FEngine = AEngine;
        FStream = AStream;
        FErrorHandled = false;
    }

    // Engine.pas:9497
    public void ShowProgress(EngineObject Sender, string Msg, int Percent)
    {
        if (FOnProgress != null)
        {
            FOnProgress(Sender, Msg, Percent);
        }
    }

    public GameEngine Engine { get => FEngine; }
    public Stream Stream { get => FStream; }
    public PropertyTable PropertyTable { get => FPropertyTable; }
    public StorageStreamError OnError { get => FOnError; set => FOnError = value; }
    public ProgressEvent OnProgress { get => FOnProgress; set => FOnProgress = value; }

    // Delphi var parameters alias a descendant reference through a base-class cast.
    public void rwCreateEObject<T>(ref T value, ECustomNode owner, int id) where T : EngineObject
    {
        EngineObject result = value;
        try { rwCreateEObject(ref result, owner, id); }
        finally { value = (T)result; }
    }

    public virtual void Dispose()
    {
    }

    public delegate void StorageStreamError(EStorageStream StorageStream, string Message, ref bool Handled);
}
