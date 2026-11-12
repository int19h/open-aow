// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// a TENode that holds references to objects owned elsewhere (AddRef on AddChild, Release on
// removal)
public partial class EReferenceNode : ENode
{

    public EReferenceNode(ECustomNode owner) : base(owner)
    { }

    // Engine.pas:8144
    public override void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((EngineObject)_children.List[I]).Release();
        }

        _children.Clear();
    }

    // Engine.pas:8157
    public override void ReadWriteChildren(EStorageStream stream)
    {
        if (stream.StorageMode().Contains(StorageModeElement.Write))
        {
            for (int I = 0, last = _children.Count - 1; I <= last; I++)
            {
                if (((EngineObject)_children.List[I]).Owner == null)
                {
                    ((EngineObject)_children.List[I]).SetOwnerWithoutNotify(this);
                }
            }

            base.ReadWriteChildren(stream);
            for (int I = 0, last = _children.Count - 1; I <= last; I++)
            {
                if (((EngineObject)_children.List[I]).Owner == this)
                {
                    ((EngineObject)_children.List[I]).SetOwnerWithoutNotify(null);
                }
            }
        }
        else
        {
            base.ReadWriteChildren(stream);
            if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                for (int I = 0, last = _children.Count - 1; I <= last; I++)
                {
                    ((EngineObject)_children.List[I]).Release();
                }
            }
        }
    }

    // Engine.pas:8152
    public override void DestroyAllChildren()
    {
        Clear();
    }

    // Engine.pas:8177
    public override int RemoveChild(EngineObject child)
    {
        int result = _children.Remove(child);
        if (result != -1)
        {
            child.Release();
        }

        return result;
    }

    // Engine.pas:8183
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        if (child.Owner == this)
        {
            child.SetOwnerWithoutNotify(null);
        }

        _children.Add(child);
        if (child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        return _children.Count - 1;
    }

    // Engine.pas:631

}
