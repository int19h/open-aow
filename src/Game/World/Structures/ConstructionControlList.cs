// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class ConstructionControlList : ENode
{
    public ConstructionControlList(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:5519
    public ConstructionControl GetConstructionControl(int Index) => (ConstructionControl)FChildren.List[Index];

    // AoWAb.PAS:5523
    public ConstructionControl FindID(int ID)
    {
        int I;
        ConstructionControl Result = null;
        I = GetCount();
        while (Result == null && I != 0)
        {
            I -= 1;
            // { Read the list again after the virtual ID call. }
            if (((ConstructionControl)FChildren.List[I]).ID() == ID)
            {
                Result = (ConstructionControl)FChildren.List[I];
            }
        }

        return Result;
    }

    // Engine.pas:631

}
