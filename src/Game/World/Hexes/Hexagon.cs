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

    public new const int ClassId = 131410;

    protected internal byte[] Transitions { get; } = new byte[3];

    protected internal byte TransitionSides { get; protected set; }

    // Hexagon.pas:60
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo info)
    {
        if (hexNumber == 0)
        {
            switch (info.HexNumber)
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
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int pixelX = default;
        int pixelY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        ((HexagonResource)_resource).Show(pixelX - pixelXSurface - 8, pixelY - pixelYSurface, lockedSurface, this);
    }

    // Hexagon.pas:74
    public override void UpdateTransitions()
    {
        byte side;
        for (int sideOrdinal = 0; sideOrdinal <= 2; sideOrdinal++)
        {
            side = (byte)sideOrdinal;
            UpdateTransition(side);
        }
    }

    // Hexagon.pas:70
    public virtual void UpdateTransition(byte side)
    {
    }

}
