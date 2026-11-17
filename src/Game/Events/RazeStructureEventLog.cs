#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// structure
public partial class RazeStructureEventLog(CustomNode owner, EngineObject holder) : MessageEventLog(owner, holder)
{
    public Structure Structure { get; init; }

    // Structur.PAS:3086
    // (Structure)
    public override bool Yes()
    {
        _ = Structure.RazeWithoutConfirmation(StrategicMap.Current.SeatedPlayer);
        return true;
    }

}
