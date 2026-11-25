#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.AI;

// { Interface declaration run owned by P1. }
public class StructureAIPlayerAction(CustomNode owner) : AIPlayerAction(owner)
{

    public int Phase { get; protected init; }

    private readonly Int32List _structureIds = [];

    // Structur.PAS:3324
    public override void Dispose() => base.Dispose();

    // Structur.PAS:3330
    public override bool Activate()
    {
        _ = base.Activate();
        if (PlayerIdExtensions.ToOriginal(Control.Player.PlayerIndex) > 0)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            for (int i = 0, last = strategicMap.StructureControl.GetCount() - 1; i <= last; i++)
            {
                Structure structure = strategicMap.StructureControl[i];
                _ = _structureIds.Add(structure.StructureId);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Structur.PAS:3344
    public override bool Process()
    {
        bool waiting = false;
        while (!waiting && _structureIds.Count != 0 && GetEngine().StrategicMap.AIExecutor.ProcessTimeLeft > 0 && !Control.Player.GetBusy())
        {
            int item = _structureIds[^1];
            Structure structure = GetEngine().StrategicMap.StructureControl.FindId(item);
            if (structure is not null)
            {
                waiting = structure.ExecuteAI(Control.Player.PlayerIndex, Phase);
            }

            if (!waiting)
            {
                _structureIds.Delete(_structureIds.Count - 1);
            }
        }

        return _structureIds.Count != 0;
    }
}
