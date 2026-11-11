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
        FStructureIDs = new Int32List();
    }

    public new int FPhase;
    public Int32List FStructureIDs;

    // Structur.PAS:3324
    public override void Dispose()
    {
        FStructureIDs?.Dispose();
        base.Dispose();
    }

    // Structur.PAS:3330
    public override bool Activate()
    {
        bool Result;
        base.Activate();
        if (FControl.FPlayer.FPlayerIndex > 0)
        {
            for (int I = 0, last = AoWHSMap.Current.FStructureControl.GetCount() - 1; I <= last; I++)
            {
                var structure = AoWHSMap.Current.FStructureControl.GetStructure(I);
                FStructureIDs.Add(structure.FStructureID);
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:3344
    public override bool Process()
    {
        bool Waiting;
        Structure Structure;
        Waiting = false;
        while (!Waiting && FStructureIDs.Count != 0 && AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0 && !FControl.FPlayer.GetBusy())
        {
            var item = FStructureIDs[FStructureIDs.Count - 1];
            Structure = AoWHSMap.Current.FStructureControl.FindID(item);
            if (Structure != null)
            {
                Waiting = Structure.ExecuteAI(FControl.FPlayer.FPlayerIndex, FPhase);
            }

            if (!Waiting)
            {
                FStructureIDs.Delete(FStructureIDs.Count - 1);
            }
        }

        return FStructureIDs.Count != 0;
    }

    // intf_P1_12.inc:8

}
