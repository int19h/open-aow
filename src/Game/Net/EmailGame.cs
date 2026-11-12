// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// { Interface declaration run owned by P5. }
// name. unique local ID. unique ID. security ID. email address. last day (not serialized by the
// original)
public partial class EmailGame : EngineObject
{
    // Diplomacy.pas:2462
    public new const int ClassId = 136960;

    public EmailGame(ECustomNode owner) : base(owner)
    { }

    public string Name = "";
    public int UniqueLocalId;
    public int UniqueId;
    public int SecurityId;
    public string EmailAddress = "";
    public int LastDay;

    // Diplomacy.pas:2465
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 20);
        stream.rwInteger(ref UniqueLocalId, 21);
        stream.rwInteger(ref UniqueId, 22);
        stream.rwString(ref EmailAddress, 23);
        stream.rwInteger(ref SecurityId, 24);
    }

    // Engine.pas:524

}
