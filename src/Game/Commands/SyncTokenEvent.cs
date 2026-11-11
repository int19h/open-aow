// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// FSyncValue (stored with id), the sync value of the system that sent it. another value on
// execution triggers OnSyncError
public partial class SyncTokenEvent : TokenEvent
{
    public new const int ClassID = CID_SyncTE;

    public SyncTokenEvent(ECustomNode owner) : base(owner)
    { }

    internal int FSyncValue;

    // NetworkE.pas:1738
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSyncValue, 20);
    }

    // NetworkE.pas:1744
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result = base.Execute(TE);
        if (TE.GetSyncValue() != FSyncValue && TE.FOnSyncError != null)
        {
            TE.FOnSyncError(TE);
        }

        return Result;
    }

    public int SyncValue { get => FSyncValue; set => FSyncValue = value; }

    private const short CID_SyncTE = 1365;

}
