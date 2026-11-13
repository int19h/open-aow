// Translated from AoWE.pas.
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
        _priority = 1;
    }

    public MovePath Path { get; }
    public int PathMovePoints { get; set; }
    public object MoveObject { get; set; }
    public int MovePoints { get; set; }
    public int Moves { get; set; }

    private readonly int _priority;

    public int Selection { get; set; }
    public int MovePosition { get; set; }
    public bool Valid { get; set; }

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
            result = MapCoordinate.None;
        }

        return result;
    }

    // Structur.PAS:4034
    public int GetMovePrio() => (Path.Count - MovePosition) * _priority;

    // intf_P1_04.inc:197

}
