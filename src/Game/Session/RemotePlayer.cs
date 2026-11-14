// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

namespace Aow.Game.Session;

public partial class RemotePlayer : EngineObject
{
    // AoWSetup.pas:266
    public new const int ClassId = 131682;

    public RemotePlayer(ECustomNode owner) : base(owner)
    { }

    public string Name { get; set; } = "";
    public int RemoteId { get; set; }
    public byte PreferredRace { get; set; }
    public MapValidationInfo MapValidation { get; set; }
    public int Version { get; set; }

    // AoWSetup.pas:269
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        RemoteId = persister.RawInt32("RemoteId", value: RemoteId);
        PreferredRace = persister.Byte("PreferredRace", value: PreferredRace);
        MapValidation = persister.Enum("MapValidation", value: MapValidation);
        Version = persister.Int32("Version", value: Version);
    }

}
