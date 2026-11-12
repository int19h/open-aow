// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class RemotePlayer : EngineObject
{
    // AoWSetup.pas:266
    public new const int ClassId = 131682;

    public RemotePlayer(ECustomNode owner) : base(owner)
    { }

    public string Name = "";
    public int RemoteId;
    public byte PreferredRace;
    public MapValidationInfo MapValidation;
    public int Version;

    // AoWSetup.pas:269
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.String(ref Name, 20);
        stream.Bytes(Bytes.Of(ref RemoteId, 4), 4, 21);
        stream.Byte(ref PreferredRace, 22);
        stream.Bytes(Bytes.Of(ref MapValidation, 1), 1, 23);
        stream.Int32(ref Version, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", Name);
        RemoteId = persister.RawInt32("RemoteID", RemoteId);
        PreferredRace = persister.Byte("PreferredRace", PreferredRace);
        MapValidation = persister.Enum("MapValidation", MapValidation);
        Version = persister.Int32("Version", Version);
    }

}
