#nullable disable
// Translated from Engine.pas.
using System.Collections;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// children through an abstract list interface. Message ids carry flags: $10000000 deliver once
// per Seq, $20000000 owned children only, $40000000 do not forward to children
public abstract partial class CustomNode : EngineObject, IOwner
{

    // Engine.pas:2430
    public CustomNode(CustomNode owner) : this(null, owner) { }

    private protected CustomNode(ConstructionContext context, CustomNode owner) : base(context, owner)
    { }

    // Engine.pas:2666
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
        EngineObject c;
        if (Count != 0)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                c = Children[i];
                if (c != null && c.Owner == this)
                {
                    if (!c.GetControlStyle().Contains(EngineServices.Shared))
                    {
                        c.Dispose();
                    }
                    else
                    {
                        c.Clear();
                    }
                }
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                c = Children[i];
                if (c != null)
                {
                    if (!(c.Owner == this && c.GetControlStyle().Contains(EngineServices.Shared)))
                    {
                        RemoveChild(c);
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
    public abstract EngineObject GetChild(int index);

    // Engine.pas:594
    public abstract void SetChild(int index, EngineObject child);

    // Engine.pas:595
    public abstract int GetCount();

    // Engine.pas:2661
    public virtual void PersistChildren(IPersister persister) => persister.Children("Children", children: new NodeChildren(this));

    // Engine.pas:2715
    public virtual bool SendToChild(ref EngineMessage message) => (message.MessageId & 1073741824) == 0;

    // Engine.pas:2608
    public virtual void DestroyAllChildren()
    {
        EngineObject c;
        for (int i = Count - 1; i >= 0; i--)
        {
            c = Children[i];
            if (c != null && c.Owner == this)
            {
                c.Dispose();
            }
        }

        if (Count != 0)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                c = Children[i];
                if (c != null)
                {
                    RemoveChild(c);
                }
            }
        }
    }

    // Engine.pas:2654
    public virtual void QuickClear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            RemoveChild(Children[i]);
        }
    }

    // Engine.pas:2674
    public virtual void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        EngineObject c;
        if ((message.MessageId & 268435456) != 0)
        {
            if ((message.MessageId & 536870912) == 0)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    c = Children[i];
                    if (c != null && c.GetLastMessage() != message.Sequence)
                    {
                        c.SetLastMessage(message.Sequence);
                        c.MessageProc(ref message, this);
                    }
                }
            }
            else
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    c = Children[i];
                    if (c != null && c.GetLastMessage() != message.Sequence && c.Owner == this)
                    {
                        c.SetLastMessage(message.Sequence);
                        c.MessageProc(ref message, this);
                    }
                }
            }
        }
        else
        {
            if ((message.MessageId & 536870912) == 0)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    c = Children[i];
                    c?.MessageProc(ref message, this);
                }
            }
            else
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    c = Children[i];
                    if (c != null && c.Owner == this)
                    {
                        c.MessageProc(ref message, this);
                    }
                }
            }
        }
    }

    // Engine.pas:601
    public abstract int RemoveChild(EngineObject child);

    // Engine.pas:602
    public abstract int AddChild(EngineObject child);

    // Engine.pas:2786
    // { The original tests Result before the child-count call. }
    public virtual int DestroyChildren(int classId)
    {
        EngineObject c;
        int n;
        n = 0;
        for (int i = Count - 1; i >= 0; i--)
        {
            c = Children[i];
            if (c != null && c.EngineClass.ClassId == classId)
            {
                c.Dispose();
                n += 1;
            }
        }

        return n;
    }

    // Engine.pas:2768
    public virtual EngineObject FindChildInTree(int classId)
    {
        int i;
        EngineObject result = FindChild(classId);
        if (result == null)
        {
            i = 0;
            while (result == null && i < Count)
            {
                if (Children[i] is CustomNode)
                {
                    result = ((CustomNode)Children[i]).FindChildInTree(classId);
                }

                i += 1;
            }
        }

        return result;
    }

    // Engine.pas:2726
    public virtual EngineObject FindChild(int classId)
    {
        EngineObject result;
        int i;
        bool found;
        EngineObject c;
        i = 0;
        found = false;
        c = null;
        while (!found && i < Count)
        {
            c = Children[i];
            found = c != null && c.EngineClass.ClassId == classId;
            i += 1;
        }

        result = found ? c : null;

        return result;
    }

    // Engine.pas:2744
    public virtual EngineObject FindOwnedChild(int classId)
    {
        EngineObject result;
        int i;
        bool found;
        EngineObject c;
        i = 0;
        found = false;
        c = null;
        while (!found && i < Count)
        {
            c = Children[i];
            found = c != null && c.Owner == this && classId == c.EngineClass.ClassId;
            i += 1;
        }

        result = found ? c : null;

        return result;
    }

    // The class id selects the child. A child of another class throws, the same way the call site did.
    public T FindChild<T>(int classId) where T : EngineObject => (T)FindChild(classId);

    // The class id selects the child. A child of another class throws, the same way the call site did.
    public T FindOwnedChild<T>(int classId) where T : EngineObject => (T)FindOwnedChild(classId);

    // The index selects the child. A child of another class throws, the same way the call site did.
    public T GetChild<T>(int index) where T : EngineObject => (T)GetChild(index);

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

    public int Count => GetCount();

    public sealed class ChildrenAccessor(CustomNode target)
    {
        public EngineObject this[int index] { get => target.GetChild(index); set => target.SetChild(index, value); }
    }

    public ChildrenAccessor Children => new(this);

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
