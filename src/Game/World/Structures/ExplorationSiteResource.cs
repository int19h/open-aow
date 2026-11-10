// Translated from ExploreS.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Units;

namespace Aow.Game.World.Structures;

// FDefenderCollections, the available defender army compositions
public partial class ExplorationSiteResource : StructureResource
{
    public ExplorationSiteResource(ECustomNode owner) : base(owner)
    {
        FDefenderCollections = new UnitIndexListCollection();
        FTerrainBound = false;
    }

    // ExploreS.pas:373
    public new const int ClassID = 132034;

    public UnitIndexListCollection FDefenderCollections;

    // ExploreS.pas:364
    public override void Dispose()
    {
        FDefenderCollections?.Dispose();
        base.Dispose();
    }

    // ExploreS.pas:376
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(25, FDefenderCollections.LoadFromStream, FDefenderCollections.SaveToStream);
    }

    // ExploreS.pas:357

    // ExploreS.pas:382
    public override int ResourceUserClassID() => 132033;

    // ExploreS.pas:61

    // ExploreS.pas:370
    public override string ResourceEditName() => "";

}
