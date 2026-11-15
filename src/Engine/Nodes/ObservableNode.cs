#nullable disable
// Translated from Engine.pas.
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// Engine.TEChangeNotifyNode (16 bytes): +8 FChildren, FUpdateCount. Changed on child changes
public partial class ObservableNode : CustomNode
{
    // Engine.pas:7246
    public new const int ClassId = 265;

    public ObservableNode(CustomNode owner) : base(owner)
    {
        _children = new EngineObjectList();
        _updateCount = 0;
    }

    // The child slots. Every write of a child goes through AddChild, SetChild or the members
    // below, so the child check in CheckChild sees each child that enters.
    private EngineObjectList _children;
    private int _updateCount;

    // Engine.pas:7212
    public override void Dispose()
    {
        BeginUpdate();
        base.Dispose();
        _children?.Dispose();
        _children = null;
    }

    // Engine.pas:7220
    public override void Clear()
    {
        int i;
        EngineObject c;
        i = Count - 1;
        for (; i >= 0; i -= 1)
        {
            c = _children.List[i];
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

        i = Count - 1;
        for (; i >= 0; i -= 1)
        {
            c = _children.List[i];
            if (c != null)
            {
                if (!(c.Owner == this && c.GetControlStyle().Contains(EngineServices.Shared)))
                {
                    RemoveChild(c);
                }
            }
        }

        i = _children.Count - 1;
        while (i >= 0 && _children.List[i] == null)
        {
            _children.Delete(i);
            i -= 1;
        }

        if (_updateCount == 0)
        {
            Emit(ObservableChange.Reset());
        }
    }

    // Engine.pas:7251
    public override EngineObject GetChild(int index) => _children.List[index];

    // Engine.pas:7256
    public override void SetChild(int index, EngineObject child)
    {
        CheckChild(child);
        EngineObject previous = index >= 0 && index < Count ? _children.List[index] : null;
        while (index >= Count)
        {
            _children.Add(null);
        }

        _children[index] = child;
        if (_updateCount == 0)
        {
            Emit(ObservableChange.Replaced(index, item: child, previous: previous));
        }
    }

    // Engine.pas:7263
    public override void Assign(EngineObject source, GameEngine engine)
    {
        using var batch = BeginUpdate();
        base.Assign(source, engine);
    }

    // Engine.pas:7270
    public void LockChanged()
    {
        _updateCount += 1;
    }

    // Engine.pas:7275
    public void UnlockChanged()
    {
        _updateCount -= 1;
    }

    // Engine.pas:7280
    // Subclasses react in OnChange. Emit runs that reaction and then raises CollectionChanged,
    // so the event follows every call of Changed even when the reaction does not call the base method.
    public event ObservableChangeHandler CollectionChanged;

    public void Changed() => Emit(ObservableChange.Reset());

    protected virtual void OnChange()
    {
    }

    protected void RaiseChange(ObservableChange change) => Emit(change);

    private void Emit(ObservableChange change)
    {
        OnChange();
        CollectionChanged?.Invoke(this, change);
    }

    // A batch disposes by calling EndUpdate. A default batch does nothing, so a conditional
    // update is `using var batch = loading ? BeginUpdate() : default`. Nested batches dispose
    // from the inside out and the count reaches zero once.
    public readonly struct UpdateBatch : IDisposable
    {
        private readonly ObservableNode _node;

        internal UpdateBatch(ObservableNode node) => _node = node;

        public void Dispose()
        {
            // A default batch leaves the field null. The units project compiles this file with nullable off.
            if (_node is not null)
            {
                _node.EndUpdate();
            }
        }
    }

    // Engine.pas:7284
    public UpdateBatch BeginUpdate()
    {
        _updateCount += 1;
        return new UpdateBatch(this);
    }

    // Engine.pas:7957
    // { Original source line layout for EndUpdate.
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    //
    // }
    public void EndUpdate()
    {
        Assert(_updateCount > 0, "Invalid UpdateCount Usage", "Engine.pas", 7959);
        _updateCount -= 1;
        if (_updateCount == 0)
        {
            Emit(ObservableChange.Reset());
        }
    }

    // Engine.pas:7964
    public void Sort(ListSortCompare<EngineObject> compare)
    {
        using var batch = BeginUpdate();
        _children.Sort(compare);
    }

    // Keeps the order of equal items. Reflecting pool ordering is the only caller.
    public void SortStable(ListSortCompare<EngineObject> compare)
    {
        using var batch = BeginUpdate();
        _children.SortStable(compare);
    }

    // Engine.pas:7971
    public override void MainPersist(IPersister persister)
    {
        using var batch = persister.Loading ? BeginUpdate() : default;
        base.MainPersist(persister);
    }

    // Engine.pas:7978
    public override int GetCount() => _children.Count;

    // Engine.pas:7983
    public override void SendMessage(ref EngineMessage message, EngineObject sender)
    {
        var children = _children;
        if (GetControlStyle().Contains(EngineServices.KeepIndex))
        {
            if ((message.MessageId & 268435456) != 0)
            {
                if ((message.MessageId & 536870912) == 0)
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] != null)
                        {
                            var node = children.List[i];
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
                        if (children.List[i] != null)
                        {
                            var node = children[i];
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
                        if (children.List[i] != null)
                        {
                            var node = children.List[i];
                            node.MessageProc(ref message, this);
                        }
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        if (children.List[i] != null)
                        {
                            var node = children[i];
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
                        var node = children.List[i];
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
                        var node = children[i];
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
                        var node = children.List[i];
                        node.MessageProc(ref message, this);
                    }
                }
                else
                {
                    for (int i = children.Count - 1; i >= 0; i--)
                    {
                        var node = children[i];
                        if (node.Owner == this)
                        {
                            node.MessageProc(ref message, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:8080
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
            result = _children.Remove(child);
        }

        if (_updateCount == 0)
        {
            Emit(ObservableChange.Removed(result, item: child));
        }

        return result;
    }

    // Engine.pas:8092
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        _children.Add(child);
        if (_updateCount == 0)
        {
            Emit(ObservableChange.Added(_children.Count - 1, item: child));
        }

        return _children.Count - 1;
    }

    // Engine.pas:8099
    public override EngineObject FindChild(int classId)
    {
        EngineObject result;
        int i;
        bool found;
        EngineObject c;
        i = 0;
        found = false;
        c = null;
        while (!found && i < _children.Count)
        {
            c = _children.List[i];
            found = c != null && classId == c.EngineClass.ClassId;
            i += 1;
        }

        if (found)
        {
            result = c;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Engine.pas:8117
    public override int FindChildIndex(EngineObject child)
    {
        int result = Count - 1;
        while (result >= 0 && child != _children.List[result])
        {
            result -= 1;
        }

        return result;
    }

    public int UpdateCount { get => _updateCount; }

    // The child in slot index, with the range check of TList.Get. GetChild reads the slot without it.
    public EngineObject GetChildInRange(int index) => _children[index];

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
    public void PackChildren() => _children.Pack();

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
            _children.Add(null);
        }

        _children.List[index] = child;
    }

    // Engine.pas:680

}
