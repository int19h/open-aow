// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class ConstructionControlList : ENode
{
    public ConstructionControlList(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:5519
    public ConstructionControl GetConstructionControl(int index) => (ConstructionControl)_children.List[index];

    // AoWAb.PAS:5523
    public ConstructionControl FindId(int id)
    {
        int I;
        ConstructionControl result = null;
        I = GetCount();
        while (result == null && I != 0)
        {
            I -= 1;
            // { Read the list again after the virtual ID call. }
            if (((ConstructionControl)_children.List[I]).Id() == id)
            {
                result = (ConstructionControl)_children.List[I];
            }
        }

        return result;
    }

    // Engine.pas:631

}
