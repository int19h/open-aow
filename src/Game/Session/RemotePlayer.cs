// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Rtl;

namespace Aow.Game.Session;

public partial class RemotePlayer : EObject
{
    // AoWSetup.pas:266
    public new const int ClassID = 131682;

    public RemotePlayer(ECustomNode owner) : base(owner)
    { }

    public string FName = "";
    public int FRemoteID;
    public byte FPreferredRace;
    public MapValidationInfo FMapValidation;
    public int FVersion;

    // AoWSetup.pas:269
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 20);
        Stream.rwData(Bytes.Of(ref FRemoteID, 4), 4, 21);
        Stream.rwByte(ref FPreferredRace, 22);
        Stream.rwData(Bytes.Of(ref FMapValidation, 1), 1, 23);
        Stream.rwInteger(ref FVersion, 24);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("Name", ref FName);
        persister.RawInt("RemoteID", ref FRemoteID);
        persister.Byte("PreferredRace", ref FPreferredRace);
        persister.Enum("MapValidation", ref FMapValidation);
        persister.Int("Version", ref FVersion);
    }

}
