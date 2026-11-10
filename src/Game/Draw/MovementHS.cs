// Translated from FleeLoc.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

namespace Aow.Game.Draw;

public partial class MovementHS : SingleHS
{
    public MovementHS(ECustomNode owner) : base(owner)
    { }

    // FleeLoc.pas:172
    public new const int ClassID = 2228490;

    public bool FEnabled;

    // FleeLoc.pas:175
    public override int GetShowPriority() => 32;

    // FleeLoc.pas:187
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
    }

    // FleeLoc.pas:178
    public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
    {
        if (Info.HN == 0 && !FEnabled)
        {
            MoveResult.Code = -1;
            MoveResult.Done = true;
        }
    }

    // Engine.pas:524

}
