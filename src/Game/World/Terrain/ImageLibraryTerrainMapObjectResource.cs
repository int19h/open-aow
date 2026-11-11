// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

namespace Aow.Game.World.Terrain;

public partial class ImageLibraryTerrainMapObjectResource : MultiHexMapObjectResource
{
    public ImageLibraryTerrainMapObjectResource(ECustomNode owner) : base(owner)
    { }

    // ILTer.pas:269
    public new const int ClassID = 131463;

    // ILTer.pas:279
    public override void SetCellImage(LibraryImage Value)
    {
    }

    // ILTer.pas:274
    public override int ResourceUserClassID() => 131462;

    // ILTer.pas:264
    public override string ResourceEditName() => "ImageLibrary Terrain";

}
