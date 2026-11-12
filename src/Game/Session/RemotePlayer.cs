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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwString(ref Name, 20);
        stream.rwData(Bytes.Of(ref RemoteId, 4), 4, 21);
        stream.rwByte(ref PreferredRace, 22);
        stream.rwData(Bytes.Of(ref MapValidation, 1), 1, 23);
        stream.rwInteger(ref Version, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("Name", ref Name);
        persister.RawInt32("RemoteID", ref RemoteId);
        persister.Byte("PreferredRace", ref PreferredRace);
        persister.Enum("MapValidation", ref MapValidation);
        persister.Int32("Version", ref Version);
    }

}
