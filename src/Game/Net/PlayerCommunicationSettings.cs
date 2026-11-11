// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Net;

// FConnectionMode (stored with id 5), FSystemID (id 6). One per player
public partial class PlayerCommunicationSettings : EngineObject
{
    internal ConnectionMode FConnectionMode;
    internal int FSystemID;

    // NetworkE.pas:452
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwData(Bytes.Of(ref FConnectionMode, 1), 1, 5);
        Stream.rwInteger(ref FSystemID, 6);
    }

    // NetworkE.pas:446
    public PlayerCommunicationSettings(ECustomNode owner) : base(owner)
    {
        FConnectionMode = ConnectionMode.Local;
    }

    public ConnectionMode ConnectionMode { get => FConnectionMode; set => FConnectionMode = value; }
    public int SystemID { get => FSystemID; set => FSystemID = value; }

}
