// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// FSyncValue (stored with id), the sync value of the system that sent it. another value on
// execution triggers OnSyncError
public partial class SyncTokenEvent : TokenEvent
{
    public new const int ClassId = ClassIdSyncTokenEvent;

    public SyncTokenEvent(ECustomNode owner) : base(owner)
    { }

    private int _syncValue;

    // NetworkE.pas:1738
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _syncValue = persister.Int32("SyncValue", value: _syncValue);
    }

    // NetworkE.pas:1744
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (tokenEvent.GetSyncValue() != _syncValue)
        {
            tokenEvent.RaiseSyncError(tokenEvent);
        }

        return result;
    }

    public int SyncValue { get => _syncValue; set => _syncValue = value; }

    private const short ClassIdSyncTokenEvent = 1365;

}
