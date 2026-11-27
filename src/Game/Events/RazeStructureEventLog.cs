#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// structure
public class RazeStructureEventLog(CustomNode? owner, EngineObject holder) : MessageEventLog(owner, holder)
{
    public Structure Structure { get; init; }

    // Structur.PAS:3086
    // (Structure)
    public override bool Yes()
    {
        _ = Structure.RazeWithoutConfirmation(GetEngine().StrategicMap.SeatedPlayer);
        return true;
    }
}
