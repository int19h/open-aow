#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Structures;

public class ConstructionControlList(CustomNode owner) : OwningNode(owner), IChildList<ConstructionControl>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a ConstructionControl, so the cast never fails.
    public ConstructionControl this[int index] => GetChild<ConstructionControl>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is ConstructionControl;

    int IChildSink<ConstructionControl>.AddChild(ConstructionControl child) => AddChild(child);

    // AoWAb.PAS:5519
    public ConstructionControl GetConstructionControl(int index) => this[index];

    // AoWAb.PAS:5523
    public ConstructionControl FindId(int id)
    {
        ConstructionControl result = null;
        int i = GetCount();
        while (result is null && i != 0)
        {
            i -= 1;
            // { Read the list again after the virtual ID call. }
            if (this[i].Id() == id)
            {
                result = this[i];
            }
        }

        return result;
    }

    // The caller names the control registered at the id. Another class throws. A missing id returns
    // null, as FindId does.
    public T FindId<T>(int id) where T : ConstructionControl => (T)FindId(id);
}
