// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// FSyncValue (stored with id), the sync value of the system that sent it. another value on
// execution triggers OnSyncError
public partial class SyncTokenEvent : TokenEvent
{
    public new const int ClassId = ClassIdSyncTokenEvent;

    public SyncTokenEvent(ECustomNode owner) : base(owner)
    { }

    internal int _syncValue;

    // NetworkE.pas:1738
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _syncValue, 20);
    }

    // NetworkE.pas:1744
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (tokenEvent.GetSyncValue() != _syncValue && tokenEvent._onSyncError != null)
        {
            tokenEvent._onSyncError(tokenEvent);
        }

        return result;
    }

    public int SyncValue { get => _syncValue; set => _syncValue = value; }

    private const short ClassIdSyncTokenEvent = 1365;

}
