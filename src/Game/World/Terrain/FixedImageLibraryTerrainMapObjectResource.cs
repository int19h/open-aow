// Translated from ILTer.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Terrain;

public partial class FixedImageLibraryTerrainMapObjectResource : ImageLibraryTerrainMapObjectResource
{
    public FixedImageLibraryTerrainMapObjectResource(ECustomNode owner) : base(owner)
    { }

    // ILTer.pas:339
    public new const int ClassId = 131465;

    // ILTer.pas:344
    public override int ResourceUserClassId() => 131464;

    // ILTer.pas:334
    public override string ResourceEditName() => "Fixed ImageLibrary Terrain";

}
