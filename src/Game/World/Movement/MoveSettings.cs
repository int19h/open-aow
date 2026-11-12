// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// Path, PathMovePoints, MoveObject, MovePoints, Priority, Selection, MovePosition, Valid
public partial class MoveSettings : EngineObject
{
    // Structur.PAS:4011
    // path, path cost, object, available points, priority, selection, position, valid
    public MoveSettings(ECustomNode owner) : base(owner)
    {
        Path = new MovePath();
        Priority = 1;
    }

    public MovePath Path;
    public int PathMovePoints;
    public object MoveObject;
    public int MovePoints;
    public int Moves;
    public int Priority;
    public int Selection;
    public int MovePosition;
    public bool Valid;

    // Structur.PAS:4018
    public override void Dispose()
    {
        Path?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:4024
    public MapCoordinate GetMoveLocation()
    {
        MapCoordinate result = default;
        if (Path.Count > MovePosition && MovePosition >= 0)
        {
            result = Path.List[MovePosition];
            result.Tag = 0;
        }
        else
        {
            Unsafe.As<MapCoordinate, int>(ref result) = 16777215;
        }

        return result;
    }

    // Structur.PAS:4034
    public int GetMovePrio() => (Path.Count - MovePosition) * Priority;

    // intf_P1_04.inc:197

}
