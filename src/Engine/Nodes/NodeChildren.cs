
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
            && !child.GetControlStyle().Contains(EngineServices.Shared)
            && child.GetControlStyle().Contains(EngineServices.Stored);
    }

    public int Slot(int index) => index;

    public bool KeepIndex => node.GetControlStyle().Contains(EngineServices.KeepIndex);

    public void Attach(IPersistable child) => ((EngineObject)child).SetOwnerWithoutNotify(node);

    public void Place(int slot, IPersistable child)
    {
        var objectValue = (EngineObject)child;
        objectValue.SetOwnerWithoutNotify(null);
        if (KeepIndex) objectValue.SetArrayOwner(node, slot);
        else objectValue.SetOwner(node);
    }
}
