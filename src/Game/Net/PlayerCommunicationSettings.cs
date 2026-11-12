// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Net;

// _connectionMode (stored with id 5), _systemId (id 6). One per player
public partial class PlayerCommunicationSettings : EngineObject
{
    internal ConnectionMode _connectionMode;
    internal int _systemId;

    // NetworkE.pas:452
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Bytes(Bytes.Of(ref _connectionMode, 1), 1, 5);
        stream.Int32(ref _systemId, 6);
    }

    // NetworkE.pas:446
    public PlayerCommunicationSettings(ECustomNode owner) : base(owner)
    {
        _connectionMode = ConnectionMode.Local;
    }

    public ConnectionMode ConnectionMode { get => _connectionMode; set => _connectionMode = value; }
    public int SystemId { get => _systemId; set => _systemId = value; }

}
