// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// FSyncValue (stored with id), the sync value of the system that sent it. another value on
// execution triggers OnSyncError
public class SyncTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = ClassIdSyncTokenEvent;

    // NetworkE.pas:1738
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SyncValue = persister.Int32("SyncValue", value: SyncValue);
    }

    // NetworkE.pas:1744
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result = base.Execute(tokenEvent);
        if (tokenEvent.GetSyncValue() != SyncValue)
        {
            tokenEvent.RaiseSyncError(tokenEvent);
        }

        return result;
    }

    public int SyncValue { get; set; }

    private const short ClassIdSyncTokenEvent = 1365;

}
