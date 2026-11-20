// Translated from BorderHx.pas and AoWHex.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The BorderHx unit of HSEPack.dpl: the border hexagon (TBorderHexagon, ClassID $20557) drawn at
//   level 11 over the map edge; it also adds itself to the hexagon above (HN 1) of its map field.
//   TBorderHexagonResource has ClassID $20558. }
public partial class BorderHex(CustomNode owner) : AbstractBorderHexagon(owner)
{

    // BorderHx.pas:59
    public new const int ClassId = 132439;

    // BorderHx.pas:39
    public override void SetOwner(CustomNode owner)
    {
        if (owner != Owner)
        {
            MapField oldField = Owner;
            if (oldField is not null && oldField.HexY != 0)
            {
                _ = oldField.Owner.GetField(oldField.HexX, Owner.HexY - 1).RemoveChild(this);
            }

            base.SetOwner(owner);
            MapField field = Owner;
            if (field is not null && field.HexY != 0)
            {
                _ = field.Owner.GetField(field.HexX, Owner.HexY - 1).AddHexSprite(this, 1);
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
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        ((MapObject)this).Resource.ImageLibrary[0].Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface);
    }

    // BorderHx.pas:72
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // BorderHx.pas:77
    public override byte GetLevel() => 11;

    // BorderHx.pas:82
    public override int GetShowPriority()
    {
        int pixelX = (Owner.HexX * 32) + 8;
        int pixelY = ((Owner.HexX & 1) * 16) + (Owner.HexY * 32);
        return ((pixelY + 32) << 13) + (11 << 26) + pixelX + 8;
    }

    // BorderHx.pas:95
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => info.HexNumber != 0;

    public new partial class Resource(CustomNode owner) : AbstractBorderHexagon.Resource(owner)
    {
        public new const int ClassId = 132440;

        // BorderHx.pas:112
        public override int ResourceUserClassId() => 132439;

        // BorderHx.pas:107
        public override string ResourceEditName() => "Border Hexagon";
    }

    // AoWHex.pas:261
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0 && StrategicMap.Current is not null && StrategicMap.Current.Day == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags & ~MapField.ExploredFlags;
        }
    }
}
