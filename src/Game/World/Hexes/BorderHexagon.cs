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
    public new const int ClassID = 132439;

    // BorderHx.pas:39
    public override void SetOwner(ECustomNode AOwner)
    {
        ECustomNode OldField;
        if (AOwner != this.Owner)
        {
            OldField = Owner;
            if (OldField != null && OldField is MapField && ((MapField)OldField).Yhx != 0)
            {
                ((MapLevel)OldField.Owner).GetField(((MapField)OldField).Xhx, ((MapField)Owner).Yhx - 1).RemoveChild(this);
            }

            base.SetOwner(AOwner);
            AOwner = Owner;
            if (AOwner != null && AOwner is MapField && ((MapField)AOwner).Yhx != 0)
            {
                ((MapLevel)AOwner.Owner).GetField(((MapField)AOwner).Xhx, ((MapField)Owner).Yhx - 1).AddHS(this, 1);
            }
        }
        else
        {
            base.SetOwner(AOwner);
        }
    }

    // BorderHx.pas:64
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        FResource.ImageLibrary[0].Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc);
    }

    // BorderHx.pas:72
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // BorderHx.pas:77
    public override byte GetLevel() => 11;

    // BorderHx.pas:82
    public override int GetShowPriority()
    {
        int Xhp;
        int Yhp;
        Xhp = ((MapField)Owner).Xhx * 32 + 8;
        Yhp = (((MapField)Owner).Xhx & 1) * 16 + ((MapField)Owner).Yhx * 32;
        return ((Yhp + 32) << 13) + (11 << 26) + (Xhp + 8);
    }

    // BorderHx.pas:95
    public override bool CanChangeTerrain(in HexSpriteSenderInfo Info) => Info.HN != 0;

    public new partial class Resource : AbstractBorderHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 132440;

        // BorderHx.pas:112
        public override int ResourceUserClassID() => 132439;

        // BorderHx.pas:107
        public override string ResourceEditName() => "Border Hexagon";

    }

}
