// Translated from ILTer.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Terrain;

public class FixedImageLibraryTerrainMapObjectResource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
{

    // ILTer.pas:339
    public new const int ClassId = 131_465;

    // ILTer.pas:344
    public override int ResourceUserClassId() => FixedImageLibraryTerrainMapObject.ClassId;

    // ILTer.pas:334
    public override string ResourceEditName() => "Fixed ImageLibrary Terrain";

}
