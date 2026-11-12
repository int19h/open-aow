// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.AI;

// { Interface declaration run owned by P1. }
public partial class StructureAIPlayerAction : AIPlayerAction
{
    // Structur.PAS:3318
    // (phase, structure IDs)
    public StructureAIPlayerAction(ECustomNode owner) : base(owner)
    {
        _structureIds = new Int32List();
    }

    public int Phase { get; protected set; }

    private readonly Int32List _structureIds;

    // Structur.PAS:3324
    public override void Dispose()
    {
        _structureIds?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3330
    public override bool Activate()
    {
        bool result;
        base.Activate();
        if (Control.Player.PlayerIndex > 0)
        {
            for (int I = 0, last = StrategicMap.Current.StructureControl.GetCount() - 1; I <= last; I++)
            {
                var structure = StrategicMap.Current.StructureControl.GetStructure(I);
                _structureIds.Add(structure.StructureId);
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:3344
    public override bool Process()
    {
        bool waiting;
        Structure structure;
        waiting = false;
        while (!waiting && _structureIds.Count != 0 && StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && !Control.Player.GetBusy())
        {
            var item = _structureIds[_structureIds.Count - 1];
            structure = StrategicMap.Current.StructureControl.FindId(item);
            if (structure != null)
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

    // intf_P1_12.inc:8

}
