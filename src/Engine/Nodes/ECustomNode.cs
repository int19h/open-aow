// Translated from Engine.pas.
using System.Collections;

using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// children through an abstract list interface. Message ids carry flags: $10000000 deliver once
// per Seq, $20000000 owned children only, $40000000 do not forward to children
public partial class ECustomNode : EngineObject, IOwner
{

    // Engine.pas:2430
    public ECustomNode(ECustomNode owner) : this(null, owner) { }

    private protected ECustomNode(ConstructionContext context, ECustomNode owner) : base(context, owner)
    { }

    // Engine.pas:2666
    public override void MainReadWrite(StorageStream stream)
    {
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            if ((stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
            {
                Clear();
            }
        }

        ReadWrite(stream);
        ReadWriteChildren(stream);
    }

    public override void MainPersist(IPersister persister)
    {
        if (persister.Loading)
        {
            Clear();
        }

        PersistFields(persister);
        PersistChildren(persister);
    }

    // Engine.pas:2626
    public override void Clear()
    {
        EngineObject C;
        if (Count != 0)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null && C.Owner == this)
                {
                    if (!C.GetControlStyle().Contains(EngineServices.Shared))
                    {
                        C.Dispose();
                    }
                    else
                    {
                        C.Clear();
                    }
                }
            }

            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.Shared)))
                    {
                        RemoveChild(C);
                    }
                }
            }
        }
    }

    // Engine.pas:2648
    public override void Assign(EngineObject source, GameEngine engine)
    {
        Clear();
        base.Assign(source, engine);
    }

    // Engine.pas:2720
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (SendToChild(ref message))
        {
            SendMessage(ref message, sender);
        }
    }

    // Engine.pas:593
    public virtual EngineObject GetChild(int index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:594
    public virtual void SetChild(int index, EngineObject child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:595
    public virtual int GetCount()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:2661
    public virtual void ReadWriteChildren(StorageStream stream)
    {
        stream.Children(this, 1);
    }

    public virtual void PersistChildren(IPersister persister)
    {
        persister.Children("Children", children: new NodeChildren(this));
    }

    // Engine.pas:2715
    public virtual bool SendToChild(ref EngineMessage message) => (message.MessageId & 1073741824) == 0;

    // Engine.pas:2608
    public virtual void DestroyAllChildren()
    {
        EngineObject C;
        for (int I = Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C != null && C.Owner == this)
            {
                C.Dispose();
            }
        }

        if (Count != 0)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null)
                {
                    RemoveChild(C);
                }
            }
        }
    }

    // Engine.pas:2654
    public virtual void QuickClear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            RemoveChild(Children[I]);
        }
    }

    // Engine.pas:2674
    public virtual void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        EngineObject C;
        if ((message.MessageId & 268435456) != 0)
        {
            if ((message.MessageId & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMessage() != message.Sequence)
                    {
                        C.SetLastMessage(message.Sequence);
                        C.MessageProc(ref message, this);
                    }
                }
            }
            else
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMessage() != message.Sequence && C.Owner == this)
                    {
                        C.SetLastMessage(message.Sequence);
                        C.MessageProc(ref message, this);
                    }
                }
            }
        }
        else
        {
            if ((message.MessageId & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null)
                    {
                        C.MessageProc(ref message, this);
                    }
                }
            }
            else
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.Owner == this)
                    {
                        C.MessageProc(ref message, this);
                    }
                }
            }
        }
    }

    // Engine.pas:601
    public virtual int RemoveChild(EngineObject child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:602
    public virtual int AddChild(EngineObject child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:2786
    // { The original tests Result before the child-count call. }
    public virtual int DestroyChildren(int classId)
    {
        EngineObject C;
        int N;
        N = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C != null && C.EngineClass.ClassId == classId)
            {
                C.Dispose();
                N += 1;
            }
        }

        return N;
    }

    // Engine.pas:2768
    public virtual EngineObject FindChildInTree(int classId)
    {
        int I;
        EngineObject result = FindChild(classId);
        if (result == null)
        {
            I = 0;
            while (result == null && I < Count)
            {
                if (Children[I] is ECustomNode)
                {
                    result = ((ECustomNode)Children[I]).FindChildInTree(classId);
                }

                I += 1;
            }
        }

        return result;
    }

    // Engine.pas:2726
    public virtual EngineObject FindChild(int classId)
    {
        EngineObject result;
        int I;
        bool found;
        EngineObject C;
        I = 0;
        found = false;
        C = null;
        while (!found && I < Count)
        {
            C = Children[I];
            found = C != null && C.EngineClass.ClassId == classId;
            I += 1;
        }

        if (found)
        {
            result = C;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:2744
    public virtual EngineObject FindOwnedChild(int classId)
    {
        EngineObject result;
        int I;
        bool found;
        EngineObject C;
        I = 0;
        found = false;
        C = null;
        while (!found && I < Count)
        {
            C = Children[I];
            found = C != null && C.Owner == this && classId == C.EngineClass.ClassId;
            I += 1;
        }

        if (found)
        {
            result = C;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:2762
    public virtual int FindChildIndex(EngineObject child)
    {
        int result = Count - 1;
        while (result >= 0 && child != Children[result])
        {
            result -= 1;
        }

        return result;
    }

    // Engine.pas:2602
    public override void Dispose()
    {
        DestroyAllChildren();
        base.Dispose();
    }

    public int Count { get => GetCount(); }

    public sealed class ChildrenAccessor(ECustomNode target)
    {
        public EngineObject this[int index] { get => target.GetChild(index); set => target.SetChild(index, value); }
    }

    public ChildrenAccessor Children => new ChildrenAccessor(this);

    // The children in stored order, empty slots included, read through GetChild.
    IEnumerable IOwner.Children
    {
        get
        {
            for (int index = 0; index < Count; index++)
            {
                yield return GetChild(index);
            }
        }
    }

    /// <summary>
    /// Whether this node takes the child. A class with typed children (IOwner of its child type)
    /// overrides it, and every write of a child into its list checks it here, so the typed child
    /// read needs no check of its own.
    /// </summary>
    protected virtual bool AcceptsChild(EngineObject child) => true;

    protected void CheckChild(EngineObject child)
    {
        if (child != null && !AcceptsChild(child))
        {
            throw new ArgumentException(GetType().Name + " does not take a child of type " + child.GetType().Name + ".", nameof(child));
        }
    }
}
