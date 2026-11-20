#nullable disable
// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// _children
public partial class OwningNode : CustomNode
{
    // Engine.pas:2861
    public new const int ClassId = 256;

    // Engine.pas:2807
    public OwningNode(CustomNode owner) : this(null, owner) { }

    private protected OwningNode(ConstructionContext context, CustomNode owner) : base(context, owner) => _children = new EngineObjectList();

    // The child slots. Every write of a child goes through AddChild, SetChild, InsertChild or the
    // members below, so the child check in CheckChild sees each child that enters.
    private readonly EngineObjectList _children;

    // Engine.pas:2833
    public override void Clear()
    {
        int i = Count - 1;
        EngineObject c;
        for (; i >= 0; i -= 1)
        {
            c = _children.List[i];
            if (c is not null && c.Owner == this)
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

        if (Count != 0)
        {
            i = Count - 1;
            for (; i >= 0; i -= 1)
            {
                c = _children.List[i];
                if (c is not null)
                {
                    if (!(c.Owner == this && c.GetControlStyle().Contains(EngineServices.Shared)))
                    {
                        _ = RemoveChild(c);
                    }
                }
            }

            i = _children.Count - 1;
            while (i >= 0 && _children.List[i] is null)
            {
                _children.Delete(i);
                i -= 1;
            }
        }
    }

    // Engine.pas:2866
    public override EngineObject GetChild(int index) => _children.List[index];

    // Engine.pas:2871
    public override void SetChild(int index, EngineObject child)
    {
        CheckChild(child);
        while (index >= Count)
        {
            _ = _children.Add(null);
        }

        if (_children.List[index] is not null && _children.List[index].Owner == this)
        {
            (_children.List[index] ?? throw new NullReferenceException()).Dispose();
        }

        _children.List[index] = child;
    }

    // Engine.pas:2879
    public override int GetCount() => _children.Count;

    // Engine.pas:2819
    public override void DestroyAllChildren()
    {
        Clear();
        if (_children.Count != 0)
        {
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                EngineObject c = _children.List[i];
                if (c is not null && c.Owner == this)
                {
                    c.Dispose();
                }
            }
        }
    }

    // Engine.pas:2884
    public override void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        EngineObjectList children = _children;
        if (GetControlStyle().Contains(EngineServices.KeepIndex))
        {
            if ((message.MessageId & 268435456) != 0)
            {
                if ((message.MessageId & 536870912) == 0)
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] is not null)
                        {
                            EngineObject node = children.List[i];
                            if (node.GetLastMessage() != message.Sequence)
                            {
                                node.SetLastMessage(message.Sequence);
                                node.MessageProc(ref message, this);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] is not null)
                        {
                            EngineObject node = children[i];
                            if (node.GetLastMessage() != message.Sequence && node.Owner == this)
                            {
                                node.SetLastMessage(message.Sequence);
                                node.MessageProc(ref message, this);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((message.MessageId & 536870912) == 0)
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] is not null)
                        {
                            EngineObject node = children.List[i];
                            node.MessageProc(ref message, this);
                        }
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] is not null)
                        {
                            EngineObject node = children[i];
                            if (node.Owner == this)
                            {
                                node.MessageProc(ref message, this);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if ((message.MessageId & 268435456) != 0)
            {
                if ((message.MessageId & 536870912) == 0)
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        EngineObject node = children.List[i];
                        if (node.GetLastMessage() != message.Sequence)
                        {
                            node.SetLastMessage(message.Sequence);
                            node.MessageProc(ref message, this);
                        }
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        EngineObject node = children[i];
                        if (node.GetLastMessage() != message.Sequence && node.Owner == this)
                        {
                            node.SetLastMessage(message.Sequence);
                            node.MessageProc(ref message, this);
                        }
                    }
                }
            }
            else
            {
                if ((message.MessageId & 536870912) == 0)
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        EngineObject node = children.List[i];
                        node.MessageProc(ref message, this);
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        EngineObject node = children[i];
                        if (node.Owner == this)
                        {
                            node.MessageProc(ref message, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:2981
    public override int RemoveChild(EngineObject child)
    {
        int result;
        if (GetControlStyle().Contains(EngineServices.KeepIndex))
        {
            result = _children.IndexOf(child);
            if (result != -1)
            {
                _children.List[result] = null;
            }
        }
        else
        {
            return _children.Remove(child);
        }

        return result;
    }

    // Engine.pas:2992
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        _ = _children.Add(child);
        return _children.Count - 1;
    }

    // Inserts the child at index after the check AddChild makes. TTokenEventList.Insert writes the
    // list directly, and TokenEventList inserts through this instead.
    public void InsertChild(int index, EngineObject child)
    {
        CheckChild(child);
        _children.Insert(index, child);
    }

    // Engine.pas:2998
    public override EngineObject FindChild(int classId)
    {
        int i = 0;
        bool found = false;
        EngineObject c = null;
        while (!found && i < _children.Count)
        {
            c = _children.List[i];
            found = c is not null && classId == c.EngineClass.ClassId;
            i += 1;
        }

        return found ? c : null;
    }

    // Engine.pas:3016
    public override int FindChildIndex(EngineObject child)
    {
        int result = Count - 1;
        while (result >= 0 && child != _children.List[result])
        {
            result -= 1;
        }

        return result;
    }

    // Engine.pas:2813
    public override void Dispose()
    {
        base.Dispose();
        _children?.Dispose();
    }

    // Engine.pas:3022
    public void DestroyEObjects(int classId)
    {
        for (int i = _children.Count - 1; i >= 0; i--)
        {
            EngineObject c = _children.List[i];
            if (c is not null && classId == c.EngineClass.ClassId && c.Owner == this)
            {
                c.Dispose();
            }
        }
    }

    // The child in slot index, with the range check of TList.Get. GetChild reads the slot without it.
    public EngineObject GetChildInRange(int index) => _children[index];

    // The index selects the child. A child of another class throws, the same way the call site did.
    public T GetChildInRange<T>(int index) where T : EngineObject => (T)GetChildInRange(index);

    // The first slot that holds the child, or -1. FindChildIndex searches from the last slot.
    public int IndexOfChild(EngineObject child) => _children.IndexOf(child);

    // The members below reorder or drop child slots as the original's direct writes of FChildren
    // do: no change notification, no reference count, no release.
    public void SortChildren(ListSortCompare<EngineObject> compare) => _children.Sort(compare);

    // Keeps the order of equal children.
    public void SortChildrenStable(ListSortCompare<EngineObject> compare) => _children.SortStable(compare);

    // The same sorts with a comparer of the child type a typed list admits. Each slot is a T or empty.
    public void SortChildren<T>(ListSortCompare<T> compare)
        where T : EngineObject => _children.Sort(compare);

    public void SortChildrenStable<T>(ListSortCompare<T> compare)
        where T : EngineObject => _children.SortStable(compare);

    public void MoveChild(int current, int target) => _children.Move(current, target: target);

    public void ExchangeChildren(int first, int second) => _children.Exchange(first, second: second);

    // Removes the first slot that holds the child and returns its index, or -1.
    public int DropChild(EngineObject child) => _children.Remove(child);

    protected void DeleteChildAt(int index) => _children.Delete(index);

    // Removes the empty slots.
    protected void PackChildren() => _children.Pack();

    // Drops every slot without disposing or releasing a child.
    public void ClearChildren() => _children.Clear();

    // Appends the child after the check AddChild makes, for an AddChild override that keeps its
    // own bookkeeping.
    protected int AppendChild(EngineObject child)
    {
        CheckChild(child);
        return _children.Add(child);
    }

    // Writes the child into slot index after the check, padding the slots before it with empty
    // ones. The previous child of the slot is not disposed or released.
    protected void PlaceChild(int index, EngineObject child)
    {
        CheckChild(child);
        while (index >= Count)
        {
            _ = _children.Add(null);
        }

        _children.List[index] = child;
    }

    // Engine.pas:631
}
