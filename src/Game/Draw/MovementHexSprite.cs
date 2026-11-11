// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

namespace Aow.Game.Draw;

public partial class MovementHexSprite : SingleHexSprite
{
    public MovementHexSprite(ECustomNode owner) : base(owner)
    { }

    // FleeLoc.pas:172
    public new const int ClassID = 2228490;

    public bool FEnabled;

    // FleeLoc.pas:175
    public override int GetShowPriority() => 32;

    // FleeLoc.pas:187
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
    }

    // FleeLoc.pas:178
    public override void CanMoveOn(in HexSpriteMoveOnInfo Info, ref HexSpriteMoveOnResult MoveResult)
    {
        if (Info.HN == 0 && !FEnabled)
        {
            MoveResult.Code = -1;
            MoveResult.Done = true;
        }
    }

    // Engine.pas:524

}
