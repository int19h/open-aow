
using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

public sealed class NodeChildren(ECustomNode node) : IChildCollection
{
    public int Count => node.GetCount();

    public IPersistable Get(int index) => node.GetChild(index);

    public bool IsStored(int index)
    {
        var child = node.GetChild(index);
        return child != null && child.Owner == node
            && !child.GetControlStyle().Contains(EngineServices.csShared)
            && child.GetControlStyle().Contains(EngineServices.csStored);
    }

    public int Slot(int index) => index;

    public bool KeepIndex => node.GetControlStyle().Contains(EngineServices.csKeepIndex);

    public void Attach(IPersistable child) => ((EObject)child).FOwner = node;

    public void Place(int slot, IPersistable child)
    {
        var obj = (EObject)child;
        obj.FOwner = null;
        if (KeepIndex) obj.SetArrayOwner(node, slot);
        else obj.SetOwner(node);
    }
}
