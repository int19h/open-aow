// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

namespace Aow.Game.World.Terrain;

public class ImageLibraryTerrainMapObjectResource(CustomNode owner) : MultiHexMapObjectResource(owner)
{

    // ILTer.pas:269
    public new const int ClassId = 131_463;

    // ILTer.pas:279
    public override void SetCellImage(LibraryImage value)
    {
    }

    // ILTer.pas:274
    public override int ResourceUserClassId() => ImageLibraryTerrainMapObject.ClassId;

    // ILTer.pas:264
    public override string ResourceEditName() => "ImageLibrary Terrain";

}
