// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Session;

public sealed class RemotePlayer(CustomNode owner) : EngineObject(owner)
{
    // AoWSetup.pas:266
    public new const int ClassId = 131_682;

    public string Name { get; set; } = "";
    public int RemoteId { get; set; }
    public int PreferredRace { get; set; }
    public MapValidationInfo MapValidation { get; set; }
    public int Version { get; set; }

    // AoWSetup.pas:269
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Name = persister.String("Name", value: Name);
        RemoteId = persister.RawInt32("RemoteId", value: RemoteId);
        PreferredRace = persister.Byte("PreferredRace", value: PreferredRace.TruncateToByte());
        MapValidation = persister.Enum("MapValidation", value: MapValidation);
        Version = persister.Int32("Version", value: Version);
    }
}
