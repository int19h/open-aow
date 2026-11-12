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
        StructureIds = new Int32List();
    }

    public new int _phase;
    public Int32List StructureIds;

    // Structur.PAS:3324
    public override void Dispose()
    {
        StructureIds?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3330
    public override bool Activate()
    {
        bool result;
        base.Activate();
        if (Control.Player.PlayerIndex > 0)
        {
            for (int I = 0, last = AoWHSMap.Current.StructureControl.GetCount() - 1; I <= last; I++)
            {
                var structure = AoWHSMap.Current.StructureControl.GetStructure(I);
                StructureIds.Add(structure.StructureId);
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
        while (!waiting && StructureIds.Count != 0 && AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0 && !Control.Player.GetBusy())
        {
            var item = StructureIds[StructureIds.Count - 1];
            structure = AoWHSMap.Current.StructureControl.FindId(item);
            if (structure != null)
            {
                waiting = structure.ExecuteAI(Control.Player.PlayerIndex, _phase);
            }

            if (!waiting)
            {
                StructureIds.Delete(StructureIds.Count - 1);
            }
        }

        return StructureIds.Count != 0;
    }

    // intf_P1_12.inc:8

}
