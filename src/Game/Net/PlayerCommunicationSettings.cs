// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Net;

// _connectionMode (stored with id 5), _systemId (id 6). One per player
public partial class PlayerCommunicationSettings : EngineObject
{
    private ConnectionMode _connectionMode;
    private int _systemId;

    // NetworkE.pas:452
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _connectionMode = persister.Enum("ConnectionMode", value: _connectionMode);
        _systemId = persister.Int32("SystemId", value: _systemId);
    }

    // NetworkE.pas:446
    public PlayerCommunicationSettings(CustomNode owner) : base(owner)
    {
        _connectionMode = ConnectionMode.Local;
    }

    public ConnectionMode ConnectionMode { get => _connectionMode; set => _connectionMode = value; }
    public int SystemId { get => _systemId; set => _systemId = value; }

}
