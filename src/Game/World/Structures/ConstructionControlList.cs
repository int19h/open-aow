// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public partial class ConstructionControlList : ENode, IChildList<ConstructionControl>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a ConstructionControl, so the cast never fails.
    public ConstructionControl this[int index] => (ConstructionControl)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is ConstructionControl;

    int IChildSink<ConstructionControl>.AddChild(ConstructionControl child) => AddChild(child);

    public ConstructionControlList(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:5519
    public ConstructionControl GetConstructionControl(int index) => this[index];

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
            if (this[I].Id() == id)
            {
                result = this[I];
            }
        }

        return result;
    }

    // Engine.pas:631

}
