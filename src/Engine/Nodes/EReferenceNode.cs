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
            GetChild(I).Release();
        }

        ClearChildren();
    }

    // Engine.pas:8157
    public override void ReadWriteChildren(StorageStream stream)
    {
        if (stream.StorageMode().Contains(StorageModeElement.Write))
        {
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if (GetChild(I).Owner == null)
                {
                    GetChild(I).SetOwnerWithoutNotify(this);
                }
            }

            base.ReadWriteChildren(stream);
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                if (GetChild(I).Owner == this)
                {
                    GetChild(I).SetOwnerWithoutNotify(null);
                }
            }
        }
        else
        {
            base.ReadWriteChildren(stream);
            if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                for (int I = 0, last = Count - 1; I <= last; I++)
                {
                    GetChild(I).Release();
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
        int result = DropChild(child);
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

        AppendChild(child);
        if (child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        return Count - 1;
    }

    // Engine.pas:631

}
