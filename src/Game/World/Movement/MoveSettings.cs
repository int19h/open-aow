// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// FPath, FPathMovePoints, FMoveObject, FMovePoints, FPriority, FSelection, FMovePos, FValid
public partial class MoveSettings : EObject
{
    // Structur.PAS:4011
    // path, path cost, object, available points, priority, selection, position, valid
    public MoveSettings(ECustomNode owner) : base(owner)
    {
        FPath = new Movepath();
        FPriority = 1;
    }

    public Movepath FPath;
    public int FPathMovePoints;
    public object FMoveObject;
    public int FMovePoints;
    public int FMoves;
    public int FPriority;
    public int FSelection;
    public int FMovePos;
    public bool FValid;

    // Structur.PAS:4018
    public override void Dispose()
    {
        FPath?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:4024
    public XYL GetMoveLoc()
    {
        XYL Result = default;
        if (FPath.Count > FMovePos && FMovePos >= 0)
        {
            Result = FPath.List[FMovePos];
            Result.Flag = 0;
        }
        else
        {
            Unsafe.As<XYL, int>(ref Result) = 16777215;
        }

        return Result;
    }

    // Structur.PAS:4034
    public int GetMovePrio() => (FPath.Count - FMovePos) * FPriority;

    // intf_P1_04.inc:197

}
