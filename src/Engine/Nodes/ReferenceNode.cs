#nullable disable warnings
// Translated from Engine.pas.
using Aow.Formats.Tree;

namespace Aow.Engine.Nodes;

// a TENode that holds references to objects owned elsewhere (AddRef on AddChild, Release on
// removal)
public class ReferenceNode(CustomNode? owner) : OwningNode(owner)
{

    // Engine.pas:8144
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            _ = GetChild(i).Release();
        }

        ClearChildren();
    }

    // Engine.pas:8157
    // The original write made the list the owner of each child without one, stored the children
    // it owned and cleared those owners again. The tree stores the same children through
    // ReferenceChildren and leaves the owners alone. The load releases the reference each loaded
    // child took when it was created.
    public override void PersistChildren(IPersister persister)
    {
        persister.Children("Children", children: new ReferenceChildren(this));
        if (persister.Loading)
        {
            for (int index = 0, last = Count - 1; index <= last; index++)
            {
                _ = GetChild(index).Release();
            }
        }
    }

    // Engine.pas:8152
    public override void DestroyAllChildren() => Clear();

    // Engine.pas:8177
    public override int RemoveChild(EngineObject? child)
    {
        int result = DropChild(child);
        if (result != -1)
        {
            _ = child.Release();
        }

        return result;
    }

    // Engine.pas:8183
    public override int AddChild(EngineObject? child)
    {
        CheckChild(child);
        if (child.Owner == this)
        {
            child.SetOwnerWithoutNotify(null);
        }

        _ = AppendChild(child);
        return child.AddRef() == -1 ? throw new NotSupportedException("EObject doesn't support ReferenceCounting") : Count - 1;
    }
}
