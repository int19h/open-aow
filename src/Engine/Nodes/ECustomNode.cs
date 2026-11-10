// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// children through an abstract list interface. Message ids carry flags: $10000000 deliver once
// per Seq, $20000000 owned children only, $40000000 do not forward to children
public partial class ECustomNode : EObject
{

    // Engine.pas:2430
    public ECustomNode(ECustomNode owner) : this(null, owner) { }

    private protected ECustomNode(ConstructionContext context, ECustomNode owner) : base(context, owner)
    { }

    // Engine.pas:2666
    public override void MainReadWrite(EStorageStream Stream)
    {
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
            {
                Clear();
            }
        }

        ReadWrite(Stream);
        ReadWriteChildren(Stream);
    }

    public override void MainPersist(IPersister persister)
    {
        if (persister.Loading)
        {
            Clear();
        }

        PersistFields(persister);
        persister.Children("Children", new NodeChildren(this));
    }

    // Engine.pas:2626
    public override void Clear()
    {
        EObject C;
        if (Count != 0)
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
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

            for (int I = Count - 1; I >= 0; I--)
            {
                C = Children[I];
                if (C != null)
                {
                    if (!(C.Owner == this && C.GetControlStyle().Contains(EngineServices.csShared)))
                    {
                        RemoveChild(C);
                    }
                }
            }
        }
    }

    // Engine.pas:2648
    public override void Assign(EObject Source, GameEngine AEngine)
    {
        Clear();
        base.Assign(Source, AEngine);
    }

    // Engine.pas:2720
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (SendToChild(ref Msg))
        {
            SendMsg(ref Msg, Sender);
        }
    }

    // Engine.pas:593
    public virtual EObject GetChild(int Index)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:594
    public virtual void SetChild(int Index, EObject Child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:595
    public virtual int GetCount()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:2661
    public virtual void ReadWriteChildren(EStorageStream Stream)
    {
        Stream.rwChildren(this, 1);
    }

    // Engine.pas:2715
    public virtual bool SendToChild(ref EMsg Msg) => (Msg.MsgID & 1073741824) == 0;

    // Engine.pas:2608
    public virtual void DestroyAllChildren()
    {
        EObject C;
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
    public virtual void SendMsg(ref EMsg Msg, EObject Sender)
    {
        EObject C;
        if ((Msg.MsgID & 268435456) != 0)
        {
            if ((Msg.MsgID & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMsg() != Msg.Seq)
                    {
                        C.SetLastMsg(Msg.Seq);
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
            else
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null && C.GetLastMsg() != Msg.Seq && C.Owner == this)
                    {
                        C.SetLastMsg(Msg.Seq);
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
        }
        else
        {
            if ((Msg.MsgID & 536870912) == 0)
            {
                for (int I = Count - 1; I >= 0; I--)
                {
                    C = Children[I];
                    if (C != null)
                    {
                        C.MsgProc(ref Msg, this);
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
                        C.MsgProc(ref Msg, this);
                    }
                }
            }
        }
    }

    // Engine.pas:601
    public virtual int RemoveChild(EObject Child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:602
    public virtual int AddChild(EObject Child)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Engine.pas:2786
    // { The original tests Result before the child-count call. }
    public virtual int DestroyChildren(int AClassID)
    {
        EObject C;
        int N;
        N = 0;
        for (int I = Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C != null && C.EClass.ClassID == AClassID)
            {
                C.Dispose();
                N += 1;
            }
        }

        return N;
    }

    // Engine.pas:2768
    public virtual EObject FindChildInTree(int AClassID)
    {
        int I;
        EObject Result = FindChild(AClassID);
        if (Result == null)
        {
            I = 0;
            while (Result == null && I < Count)
            {
                if (Children[I] is ECustomNode)
                {
                    Result = ((ECustomNode)Children[I]).FindChildInTree(AClassID);
                }

                I += 1;
            }
        }

        return Result;
    }

    // Engine.pas:2726
    public virtual EObject FindChild(int AClassID)
    {
        EObject Result;
        int I;
        bool Found;
        EObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < Count)
        {
            C = Children[I];
            Found = C != null && C.EClass.ClassID == AClassID;
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

    // Engine.pas:2744
    public virtual EObject FindOwnedChild(int AClassID)
    {
        EObject Result;
        int I;
        bool Found;
        EObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < Count)
        {
            C = Children[I];
            Found = C != null && C.Owner == this && AClassID == C.EClass.ClassID;
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

    // Engine.pas:2762
    public virtual int FindChildIndex(EObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != Children[Result])
        {
            Result -= 1;
        }

        return Result;
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
        public EObject this[int Index] { get => target.GetChild(Index); set => target.SetChild(Index, value); }
    }

    public ChildrenAccessor Children => new ChildrenAccessor(this);
}
