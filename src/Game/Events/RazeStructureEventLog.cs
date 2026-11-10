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

    public Structure FStructure;

    // Structur.PAS:3086
    // (FStructure)
    public override bool Yes()
    {
        FStructure.RazeEx(AoWHSMap.Current.FSeatedPlayer);
        return true;
    }

}
