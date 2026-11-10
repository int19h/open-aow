// Translated from BorderHx.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The BorderHx unit of HSEPack.dpl: the border hexagon (TBorderHexagon, ClassID $20557) drawn at
//   level 11 over the map edge; it also adds itself to the hexagon above (HN 1) of its map field.
//   TBorderHexagonResource has ClassID $20558. }
public partial class TBorderHexagon : TAbstractBorderHexagon
{
    public TBorderHexagon(TECustomNode owner) : base(owner)
    { }

    // BorderHx.pas:59
    public new const int ClassID = 132439;

    // BorderHx.pas:39
    public override void SetOwner(TECustomNode AOwner)
    {
        TECustomNode OldField;
        if (AOwner != this.Owner)
        {
            OldField = Owner;
            if (OldField != null && OldField is TMapField && ((TMapField)OldField).Yhx != 0)
            {
                ((TMapLevel)OldField.Owner).GetField(((TMapField)OldField).Xhx, ((TMapField)Owner).Yhx - 1).RemoveChild(this);
            }

            base.SetOwner(AOwner);
            AOwner = Owner;
            if (AOwner != null && AOwner is TMapField && ((TMapField)AOwner).Yhx != 0)
            {
                ((TMapLevel)AOwner.Owner).GetField(((TMapField)AOwner).Xhx, ((TMapField)Owner).Yhx - 1).AddHS(this, 1);
            }
        }
        else
        {
            base.SetOwner(AOwner);
        }
    }

    // BorderHx.pas:64
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        Aow.Game.World.Map.HexGeometry.HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
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
        Xhp = ((TMapField)Owner).Xhx * 32 + 8;
        Yhp = (((TMapField)Owner).Xhx & 1) * 16 + ((TMapField)Owner).Yhx * 32;
        return ((Yhp + 32) << 13) + (11 << 26) + (Xhp + 8);
    }

    // BorderHx.pas:95
    public override bool CanChangeTerrain(in THSSenderInfo Info) => Info.HN != 0;

}

public partial class TBorderHexagonResource : TAbstractBorderHexagonResource
{
    public TBorderHexagonResource(TECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 132440;

    // BorderHx.pas:112
    public override int ResourceUserClassID() => 132439;

    // BorderHx.pas:107
    public override string ResourceEditName() => "Border Hexagon";

}
