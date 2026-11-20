#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// Path, PathMovePoints, MoveObject, MovePoints, Priority, Selection, MovePosition, Valid
public class MoveSettings(CustomNode owner) : EngineObject(owner)
{

    public MovePath Path { get; } = new MovePath();
    public int PathMovePoints { get; set; }
    public object MoveObject { get; init; }
    public int MovePoints { get; set; }
    public int Moves { get; set; }

    private readonly int _priority = 1;

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
    public MapCoordinate MoveLocation => Path.Count > MovePosition && MovePosition >= 0 ? Path.List[MovePosition].Location : MapCoordinate.None;

    // Structur.PAS:4034
    public int GetMovePrio() => (Path.Count - MovePosition) * _priority;
}
