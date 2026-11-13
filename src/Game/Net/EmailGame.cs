// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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

    public int LastDay { get; set; }

    // Diplomacy.pas:2465
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref Name, 20);
        stream.Int32(ref UniqueLocalId, 21);
        stream.Int32(ref UniqueId, 22);
        stream.String(ref EmailAddress, 23);
        stream.Int32(ref SecurityId, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        UniqueLocalId = persister.Int32("UniqueLocalId", value: UniqueLocalId);
        UniqueId = persister.Int32("UniqueId", value: UniqueId);
        EmailAddress = persister.String("EmailAddress", value: EmailAddress);
        SecurityId = persister.Int32("SecurityId", value: SecurityId);
    }

    // Engine.pas:524

}
