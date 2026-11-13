// Translated from ExploreS.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// FDefenderCollections, the available defender army compositions
public partial class ExplorationSiteResource : StructureResource
{
    public ExplorationSiteResource(ECustomNode owner) : base(owner)
    {
        DefenderCollections = new UnitIndexListCollection();
        TerrainBound = false;
    }

    // ExploreS.pas:373
    public new const int ClassId = 132034;

    public UnitIndexListCollection DefenderCollections { get; }

    // ExploreS.pas:364
    public override void Dispose()
    {
        DefenderCollections?.Dispose();
        base.Dispose();
    }

    // ExploreS.pas:376
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Payload(25, DefenderCollections.LoadFromStream, DefenderCollections.SaveToStream);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UnitIndexListCollection(persister, name: "DefenderCollections", collection: DefenderCollections);
    }

    // ExploreS.pas:357

    // ExploreS.pas:382
    public override int ResourceUserClassId() => 132033;

    // ExploreS.pas:61

    // ExploreS.pas:370
    public override string ResourceEditName() => "";

}
