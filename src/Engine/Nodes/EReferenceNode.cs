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
            ((EObject)FChildren.List[I]).Release();
        }

        FChildren.Clear();
    }

    // Engine.pas:8157
    public override void ReadWriteChildren(EStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(StorageModeElement.Write))
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if (((EObject)FChildren.List[I]).FOwner == null)
                {
                    ((EObject)FChildren.List[I]).FOwner = this;
                }
            }

            base.ReadWriteChildren(Stream);
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                if (((EObject)FChildren.List[I]).FOwner == this)
                {
                    ((EObject)FChildren.List[I]).FOwner = null;
                }
            }
        }
        else
        {
            base.ReadWriteChildren(Stream);
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
            {
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    ((EObject)FChildren.List[I]).Release();
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
    public override int RemoveChild(EObject Child)
    {
        int Result = FChildren.Remove(Child);
        if (Result != -1)
        {
            Child.Release();
        }

        return Result;
    }

    // Engine.pas:8183
    public override int AddChild(EObject Child)
    {
        if (Child.FOwner == this)
        {
            Child.FOwner = null;
        }

        FChildren.Add(Child);
        if (Child.AddRef() == -1)
        {
            throw new NotSupportedException("EObject doesn't support ReferenceCounting");
        }

        return FChildren.Count - 1;
    }

    // Engine.pas:631

}
