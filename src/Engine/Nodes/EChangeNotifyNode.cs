// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// Engine.TEChangeNotifyNode (16 bytes): +8 FChildren, FUpdateCount. Changed on child changes
public partial class EChangeNotifyNode : ECustomNode
{
    // Engine.pas:7246
    public new const int ClassId = 265;

    public EChangeNotifyNode(ECustomNode owner) : base(owner)
    {
        _children = new EngineObjectList();
        _updateCount = 0;
    }

    public EngineObjectList _children;
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
        int I;
        EngineObject C;
        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (EngineObject)_children.List[I];
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

        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (EngineObject)_children.List[I];
            if (C != null)
            {
                if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.Shared)))
                {
                    RemoveChild(C);
                }
            }
        }

        I = _children.Count - 1;
        while (I >= 0 && _children.List[I] == null)
        {
            _children.Delete(I);
            I -= 1;
        }

        if (_updateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7251
    public override EngineObject GetChild(int index) => (EngineObject)_children.List[index];

    // Engine.pas:7256
    public override void SetChild(int index, EngineObject child)
    {
        while (index >= Count)
        {
            _children.Add(null);
        }

        _children[index] = child;
        if (_updateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7263
    public override void Assign(EngineObject source, GameEngine engine)
    {
        BeginUpdate();
        base.Assign(source, engine);
        EndUpdate();
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
    public virtual void Changed()
    {
    }

    // Engine.pas:7284
    public void BeginUpdate()
    {
        _updateCount += 1;
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
            Changed();
        }
    }

    // Engine.pas:7964
    public void Sort(ListSortCompare compare)
    {
        BeginUpdate();
        _children.Sort(compare);
        EndUpdate();
    }

    // Keeps the order of equal items. Reflecting pool ordering is the only caller.
    public void SortStable(ListSortCompare compare)
    {
        BeginUpdate();
        _children.SortStable(compare);
        EndUpdate();
    }

    // Engine.pas:7971
    public override void MainReadWrite(EStorageStream stream)
    {
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            BeginUpdate();
        }

        base.MainReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            EndUpdate();
        }
    }

    public override void MainPersist(IPersister persister)
    {
        if (persister.Loading)
        {
            BeginUpdate();
        }

        base.MainPersist(persister);
        if (persister.Loading)
        {
            EndUpdate();
        }
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
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EngineObject)children.List[I];
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
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EngineObject)children[I];
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
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EngineObject)children.List[I];
                            node.MessageProc(ref message, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EngineObject)children[I];
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
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EngineObject)children.List[I];
                        if (node.GetLastMessage() != message.Sequence)
                        {
                            node.SetLastMessage(message.Sequence);
                            node.MessageProc(ref message, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EngineObject)children[I];
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
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EngineObject)children.List[I];
                        node.MessageProc(ref message, this);
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EngineObject)children[I];
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
            Changed();
        }

        return result;
    }

    // Engine.pas:8092
    public override int AddChild(EngineObject child)
    {
        _children.Add(child);
        if (_updateCount == 0)
        {
            Changed();
        }

        return _children.Count - 1;
    }

    // Engine.pas:8099
    public override EngineObject FindChild(int classId)
    {
        EngineObject result;
        int I;
        bool found;
        EngineObject C;
        I = 0;
        found = false;
        C = null;
        while (!found && I < _children.Count)
        {
            C = (EngineObject)_children.List[I];
            found = C != null && classId == C.EngineClass.ClassId;
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

    // Engine.pas:680

}
