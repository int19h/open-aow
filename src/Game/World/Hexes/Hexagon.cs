// Translated from Hexagon.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// { The Hexagon unit of HSEPack.dpl: the plain terrain hexagon (THexagon, ClassID $20152) drawn from
//   the first image of its resource's image library, with the transitions to the neighbour terrain
//   on three sides from HexTrans.ILB (THexagonResource, ClassID $20153). }
// FTransitions (the transition image of the sides 0.2), FTransitionSides (bits 0.2: the sides
// with a transition, bits 3.5: the shifted variants)
public partial class Hexagon : AbstractHexagon
{
    public Hexagon(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131410;

    protected internal ByteArray3 FTransitions;
    protected internal byte FTransitionSides;

    // Hexagon.pas:60
    public override void NeighbourTerrainChanged(int HN, in HexSpriteTerrainInfo Info)
    {
        if (HN == 0)
        {
            switch (Info.HN)
            {
                case 1:
                    UpdateTransition(0);
                    break;
                case 2:
                    UpdateTransition(1);
                    break;
                case 6:
                    UpdateTransition(2);
                    break;
            }
        }
    }

    // Hexagon.pas:52
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        ((HexagonResource)FResource).Show(Xhp - XhpSurf - 8, Yhp - YhpSurf, SurfaceDesc, this);
    }

    // Hexagon.pas:74
    public override void UpdateTransitions()
    {
        byte Side;
        for (int sideOrdinal = 0; sideOrdinal <= 2; sideOrdinal++)
        {
            Side = (byte)sideOrdinal;
            UpdateTransition(Side);
        }
    }

    // Hexagon.pas:70
    public virtual void UpdateTransition(byte Side)
    {
    }

}
