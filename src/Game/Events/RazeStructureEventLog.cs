// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// structure
public partial class RazeStructureEventLog : MessageEventLog
{
    public RazeStructureEventLog(ECustomNode owner) : base(owner)
    { }

    public Structure Structure;

    // Structur.PAS:3086
    // (Structure)
    public override bool Yes()
    {
        Structure.RazeWithoutConfirmation(AoWHSMap.Current.SeatedPlayer);
        return true;
    }

}
