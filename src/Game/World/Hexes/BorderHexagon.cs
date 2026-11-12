// Translated from BorderHx.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The BorderHx unit of HSEPack.dpl: the border hexagon (TBorderHexagon, ClassID $20557) drawn at
//   level 11 over the map edge; it also adds itself to the hexagon above (HN 1) of its map field.
//   TBorderHexagonResource has ClassID $20558. }
public partial class BorderHexagon : AbstractBorderHexagon
{
    public BorderHexagon(ECustomNode owner) : base(owner)
    { }

    // BorderHx.pas:59
    public new const int ClassId = 132439;

    // BorderHx.pas:39
    public override void SetOwner(ECustomNode owner)
    {
        ECustomNode oldField;
        if (owner != this.Owner)
        {
            oldField = Owner;
            if (oldField != null && oldField is MapField && ((MapField)oldField).HexY != 0)
            {
                ((MapLevel)oldField.Owner).GetField(((MapField)oldField).HexX, ((MapField)Owner).HexY - 1).RemoveChild(this);
            }

            base.SetOwner(owner);
            owner = Owner;
            if (owner != null && owner is MapField && ((MapField)owner).HexY != 0)
            {
                ((MapLevel)owner.Owner).GetField(((MapField)owner).HexX, ((MapField)Owner).HexY - 1).AddHexSprite(this, 1);
            }
        }
        else
        {
            base.SetOwner(owner);
        }
    }

    // BorderHx.pas:64
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(((MapField)Owner).HexX, ((MapField)Owner).HexY, ref pixelX, ref pixelY);
        _resource.ImageLibrary[0].Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface);
    }

    // BorderHx.pas:72
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // BorderHx.pas:77
    public override byte GetLevel() => 11;

    // BorderHx.pas:82
    public override int GetShowPriority()
    {
        int pixelX;
        int pixelY;
        pixelX = ((MapField)Owner).HexX * 32 + 8;
        pixelY = (((MapField)Owner).HexX & 1) * 16 + ((MapField)Owner).HexY * 32;
        return ((pixelY + 32) << 13) + (11 << 26) + (pixelX + 8);
    }

    // BorderHx.pas:95
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => info.HexNumber != 0;

    public new partial class Resource : AbstractBorderHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassId = 132440;

        // BorderHx.pas:112
        public override int ResourceUserClassId() => 132439;

        // BorderHx.pas:107
        public override string ResourceEditName() => "Border Hexagon";

    }

}
