// Translated from Engine.pas.
using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FChildren
public partial class ENode : ECustomNode
{
    // Engine.pas:2861
    public new const int ClassID = 256;

    // Engine.pas:2807
    public ENode(ECustomNode owner) : this(null, owner) { }

    private protected ENode(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        FChildren = new EObjectList();
    }

    public EObjectList FChildren;

    // Engine.pas:2833
    public override void Clear()
    {
        int I;
        EObject C;
        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (EObject)FChildren.List[I];
            if (C != null && C.Owner == this)
            {
                if (!C.GetControlStyle().Contains(EngineServices.csShared))
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
                C = (EObject)FChildren.List[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.csShared)))
                    {
                        RemoveChild(C);
                    }
                }
            }

            I = FChildren.Count - 1;
            while (I >= 0 && FChildren.List[I] == null)
            {
                FChildren.Delete(I);
                I -= 1;
            }
        }
    }

    // Engine.pas:2866
    public override EObject GetChild(int Index) => (EObject)FChildren.List[Index];

    // Engine.pas:2871
    public override void SetChild(int Index, EObject Child)
    {
        while (Index >= Count)
        {
            FChildren.Add(null);
        }

        if (FChildren.List[Index] != null && ((EObject)FChildren.List[Index]).Owner == this)
        {
            ((FChildren.List[Index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        FChildren.List[Index] = Child;
    }

    // Engine.pas:2879
    public override int GetCount() => FChildren.Count;

    // Engine.pas:2819
    public override void DestroyAllChildren()
    {
        EObject C;
        Clear();
        if (FChildren.Count != 0)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (EObject)FChildren.List[I];
                if (C != null && C.Owner == this)
                {
                    C.Dispose();
                }
            }
        }
    }

    // Engine.pas:2884
    public override void SendMsg(ref EMsg Msg, EObject Sender)
    {
        var children = FChildren;
        if (GetControlStyle().Contains(EngineServices.csKeepIndex))
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EObject)children.List[I];
                            if (node.GetLastMsg() != Msg.Seq)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
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
                            var node = (EObject)children[I];
                            if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                            {
                                node.SetLastMsg(Msg.Seq);
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EObject)children.List[I];
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        if (children.List[I] != null)
                        {
                            var node = (EObject)children[I];
                            if (node.Owner == this)
                            {
                                node.MsgProc(ref Msg, this);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if ((Msg.MsgID & 268435456) != 0)
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EObject)children.List[I];
                        if (node.GetLastMsg() != Msg.Seq)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EObject)children[I];
                        if (node.GetLastMsg() != Msg.Seq && node.Owner == this)
                        {
                            node.SetLastMsg(Msg.Seq);
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
            else
            {
                if ((Msg.MsgID & 536870912) == 0)
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EObject)children.List[I];
                        node.MsgProc(ref Msg, this);
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EObject)children[I];
                        if (node.Owner == this)
                        {
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:2981
    public override int RemoveChild(EObject Child)
    {
        int Result;
        if (GetControlStyle().Contains(EngineServices.csKeepIndex))
        {
            Result = FChildren.IndexOf(Child);
            if (Result != -1)
            {
                FChildren.List[Result] = null;
            }
        }
        else
        {
            Result = FChildren.Remove(Child);
        }

        return Result;
    }

    // Engine.pas:2992
    public override int AddChild(EObject Child)
    {
        FChildren.Add(Child);
        return FChildren.Count - 1;
    }

    // Engine.pas:2998
    public override EObject FindChild(int AClassID)
    {
        EObject Result;
        int I;
        bool Found;
        EObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < FChildren.Count)
        {
            C = (EObject)FChildren.List[I];
            Found = C != null && AClassID == C.EClass.ClassID;
            I += 1;
        }

        if (Found)
        {
            Result = C;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Engine.pas:3016
    public override int FindChildIndex(EObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != (EObject)FChildren.List[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    // Engine.pas:2813
    public override void Dispose()
    {
        base.Dispose();
        FChildren?.Dispose();
    }

    // Engine.pas:3022
    public void DestroyEObjects(int AClassID)
    {
        EObject C;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (EObject)FChildren.List[I];
            if (C != null && AClassID == C.EClass.ClassID && C.Owner == this)
            {
                C.Dispose();
            }
        }
    }

    // Engine.pas:631
}
