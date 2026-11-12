// Translated from ILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

namespace Aow.Game.World.Terrain;

public partial class ImageLibraryTerrainMapObjectResource : MultiHexMapObjectResource
{
    public ImageLibraryTerrainMapObjectResource(ECustomNode owner) : base(owner)
    { }

    // ILTer.pas:269
    public new const int ClassId = 131463;

    // ILTer.pas:279
    public override void SetCellImage(LibraryImage value)
    {
    }

    // ILTer.pas:274
    public override int ResourceUserClassId() => 131462;

    // ILTer.pas:264
    public override string ResourceEditName() => "ImageLibrary Terrain";

}
