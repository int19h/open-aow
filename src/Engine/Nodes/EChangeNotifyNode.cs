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
    public new const int ClassID = 265;

    public EChangeNotifyNode(ECustomNode owner) : base(owner)
    {
        FChildren = new EngineObjectList();
        FUpdateCount = 0;
    }

    public EngineObjectList FChildren;
    internal int FUpdateCount;

    // Engine.pas:7212
    public override void Dispose()
    {
        BeginUpdate();
        base.Dispose();
        FChildren?.Dispose();
        FChildren = null;
    }

    // Engine.pas:7220
    public override void Clear()
    {
        int I;
        EngineObject C;
        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (EngineObject)FChildren.List[I];
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

        I = Count - 1;
        for (; I >= 0; I -= 1)
        {
            C = (EngineObject)FChildren.List[I];
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

        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7251
    public override EngineObject GetChild(int Index) => (EngineObject)FChildren.List[Index];

    // Engine.pas:7256
    public override void SetChild(int Index, EngineObject Child)
    {
        while (Index >= Count)
        {
            FChildren.Add(null);
        }

        FChildren[Index] = Child;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7263
    public override void Assign(EngineObject Source, GameEngine AEngine)
    {
        BeginUpdate();
        base.Assign(Source, AEngine);
        EndUpdate();
    }

    // Engine.pas:7270
    public void LockChanged()
    {
        FUpdateCount += 1;
    }

    // Engine.pas:7275
    public void UnlockChanged()
    {
        FUpdateCount -= 1;
    }

    // Engine.pas:7280
    public virtual void Changed()
    {
    }

    // Engine.pas:7284
    public void BeginUpdate()
    {
        FUpdateCount += 1;
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
        Assert(FUpdateCount > 0, "Invalid UpdateCount Usage", "Engine.pas", 7959);
        FUpdateCount -= 1;
        if (FUpdateCount == 0)
        {
            Changed();
        }
    }

    // Engine.pas:7964
    public void Sort(ListSortCompare Compare)
    {
        BeginUpdate();
        FChildren.Sort(Compare);
        EndUpdate();
    }

    // Keeps the order of equal items. Reflecting pool ordering is the only caller.
    public void SortStable(ListSortCompare Compare)
    {
        BeginUpdate();
        FChildren.SortStable(Compare);
        EndUpdate();
    }

    // Engine.pas:7971
    public override void MainReadWrite(EStorageStream Stream)
    {
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            BeginUpdate();
        }

        base.MainReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
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
    public override int GetCount() => FChildren.Count;

    // Engine.pas:7983
    public override void SendMsg(ref EngineMessage Msg, EngineObject Sender)
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
                            var node = (EngineObject)children.List[I];
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
                            var node = (EngineObject)children[I];
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
                            var node = (EngineObject)children.List[I];
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
                            var node = (EngineObject)children[I];
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
                        var node = (EngineObject)children.List[I];
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
                        var node = (EngineObject)children[I];
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
                        var node = (EngineObject)children.List[I];
                        node.MsgProc(ref Msg, this);
                    }
                }
                else
                {
                    for (int I = children.Count - 1; I >= 0; I--)
                    {
                        var node = (EngineObject)children[I];
                        if (node.Owner == this)
                        {
                            node.MsgProc(ref Msg, this);
                        }
                    }
                }
            }
        }
    }

    // Engine.pas:8080
    public override int RemoveChild(EngineObject Child)
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

        if (FUpdateCount == 0)
        {
            Changed();
        }

        return Result;
    }

    // Engine.pas:8092
    public override int AddChild(EngineObject Child)
    {
        FChildren.Add(Child);
        if (FUpdateCount == 0)
        {
            Changed();
        }

        return FChildren.Count - 1;
    }

    // Engine.pas:8099
    public override EngineObject FindChild(int AClassID)
    {
        EngineObject Result;
        int I;
        bool Found;
        EngineObject C;
        I = 0;
        Found = false;
        C = null;
        while (!Found && I < FChildren.Count)
        {
            C = (EngineObject)FChildren.List[I];
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

    // Engine.pas:8117
    public override int FindChildIndex(EngineObject Child)
    {
        int Result = Count - 1;
        while (Result >= 0 && Child != FChildren.List[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    public int UpdateCount { get => FUpdateCount; }

    // Engine.pas:680

}
