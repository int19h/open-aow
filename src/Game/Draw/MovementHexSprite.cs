// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

namespace Aow.Game.Draw;

public partial class MovementHexSprite : SingleHexSprite
{
    public MovementHexSprite(ECustomNode owner) : base(owner)
    { }

    // FleeLoc.pas:172
    public new const int ClassId = 2228490;

    public bool Enabled;

    // FleeLoc.pas:175
    public override int GetShowPriority() => 32;

    // FleeLoc.pas:187
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
    }

    // FleeLoc.pas:178
    public override void CanMoveOn(in HexSpriteMoveOnInfo info, ref HexSpriteMoveOnResult moveResult)
    {
        if (info.HexNumber == 0 && !Enabled)
        {
            moveResult.Code = -1;
            moveResult.Done = true;
        }
    }

    // Engine.pas:524

}
