// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Net;

// _connectionMode (stored with id 5), _systemId (id 6). One per player
public partial class PlayerCommunicationSettings(CustomNode owner) : EngineObject(owner)
{

    // NetworkE.pas:452
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ConnectionMode = persister.Enum("ConnectionMode", value: ConnectionMode);
        SystemId = persister.Int32("SystemId", value: SystemId);
    }

    public ConnectionMode ConnectionMode { get; set; }
    public int SystemId { get; set; }

}
