#nullable disable

using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

public sealed class NodeChildren(CustomNode node) : IChildCollection
{
    public int Count => node.GetCount();

    public IPersistable Get(int index) => node.GetChild(index);

    public bool IsStored(int index)
    {
        EngineObject child = node.GetChild(index);
        return child is not null && child.Owner == node
            && !child.GetControlStyle().Contains(EngineServices.Shared)
            && child.GetControlStyle().Contains(EngineServices.Stored);
    }

    public int Slot(int index) => index;

    public bool KeepIndex => node.GetControlStyle().Contains(EngineServices.KeepIndex);

    // IChildCollection takes IPersistable. A child of this node is an engine object, and the cast stays.
    public void Attach(IPersistable child) => ((EngineObject)child).SetOwnerWithoutNotify(node);

    public void Place(int slot, IPersistable child)
    {
        EngineObject objectValue = (EngineObject)child;
        objectValue.SetOwnerWithoutNotify(null);
        if (KeepIndex)
        {
            objectValue.SetArrayOwner(node, slot);
        }
        else
        {
            objectValue.SetOwner(node);
        }
    }
}
