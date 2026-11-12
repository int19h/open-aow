// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// _children
public partial class ENode : ECustomNode
{
    // Engine.pas:2861
    public new const int ClassId = 256;

    // Engine.pas:2807
    public ENode(ECustomNode owner) : this(null, owner) { }

    private protected ENode(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        _children = new EngineObjectList();
    }

    public EngineObjectList _children;

    // Engine.pas:2833
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

        if (Count != 0)
        {
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
        }
    }

    // Engine.pas:2866
    public override EngineObject GetChild(int index) => (EngineObject)_children.List[index];

    // Engine.pas:2871
    public override void SetChild(int index, EngineObject child)
    {
        CheckChild(child);
        while (index >= Count)
        {
            _children.Add(null);
        }

        if (_children.List[index] != null && ((EngineObject)_children.List[index]).Owner == this)
        {
            ((_children.List[index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        _children.List[index] = child;
    }

    // Engine.pas:2879
    public override int GetCount() => _children.Count;

    // Engine.pas:2819
    public override void DestroyAllChildren()
    {
        EngineObject C;
        Clear();
        if (_children.Count != 0)
        {
            for (int I = _children.Count - 1; I >= 0; I--)
            {
                C = (EngineObject)_children.List[I];
                if (C != null && C.Owner == this)
                {
                    C.Dispose();
                }
            }
        }
    }

    // Engine.pas:2884
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
            result = _children.Remove(child);
        }

        return result;
    }

    // Engine.pas:2992
    public override int AddChild(EngineObject child)
    {
        CheckChild(child);
        _children.Add(child);
        return _children.Count - 1;
    }

    // Engine.pas:2998
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

    // Engine.pas:3016
    public override int FindChildIndex(EngineObject child)
    {
        int result = Count - 1;
        while (result >= 0 && child != (EngineObject)_children.List[result])
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
        EngineObject C;
        for (int I = _children.Count - 1; I >= 0; I--)
        {
            C = (EngineObject)_children.List[I];
            if (C != null && classId == C.EngineClass.ClassId && C.Owner == this)
            {
                C.Dispose();
            }
        }
    }

    // Engine.pas:631
}
