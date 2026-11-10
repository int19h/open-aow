// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Ai;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// references, target data, location, move points, transporter ID, path
public partial class MoveTarget : EObject
{
    // AIGroup.pas:4215
    public MoveTarget(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FRefCount;
    public AITargetData FData;
    public XYL FLocation;
    public int FMovePoints;
    public int FTransporterID;
    public Movepath FMovepath;

    // AIGroup.pas:4221
    public override void Dispose()
    {
        if (FMovepath != null)
        {
            FMovepath?.Dispose();
        }

        base.Dispose();
    }

    // AIGroup.pas:4227
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:4233
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (Result == 0)
        {
            Dispose();
        }

        return Result;
    }

    // intf_P5_06.inc:204

}
