// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Collections;

public partial class EventList : Persistent
{
    internal NotifyEvent<object>[] FList = [];
    internal int FCount;
    internal int FCapacity;
    internal NotifyEvent<object> FDefault;

    // Engine.pas:4929
    internal NotifyEvent<object> Get(int Index)
    {
        NotifyEvent<object> Result;
        if (Index < 0 || Index >= FCount)
        {
            Result = FDefault;
        }
        else
        {
            Result = FList[Index];
        }

        return Result;
    }

    // Engine.pas:4987
    internal void Put(int Index, NotifyEvent<object> Item)
    {
        if (Index < 0)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        FList[Index] = Item;
    }

    // Engine.pas:5011
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // Engine.pas:5022
    internal void SetCount(int NewCount)
    {
        int OldCount;
        if (NewCount < 0 || NewCount > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            OldCount = FCount;
            FCount = NewCount;
            for (int I = OldCount, last = FCount - 1; I <= last; I++)
            {
                Put(I, FDefault);
            }
        }
        else
        {
            FCount = NewCount;
        }
    }

    // Engine.pas:4937
    protected internal virtual void Grow()
    {
        int Delta;
        if (FCapacity > 64)
        {
            Delta = FCapacity / 4;
        }
        else
        {
            if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }
        }

        SetCapacity(FCapacity + Delta);
    }

    // Engine.pas:4854
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:4859
    public override void Assign(Persistent Source)
    {
        if (Source is EventList)
        {
            var eventList = (EventList)Source;
            this.FCount = eventList.FCount;
            this.SetCapacity(eventList.FCount);
            Array.Copy(eventList.FList, 0, this.FList, 0, this.FCount);
            this.FDefault = eventList.FDefault;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // Engine.pas:4873
    public int Add(NotifyEvent<object> Item)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result] = Item;
        FCount += 1;
        return Result;
    }

    // Engine.pas:4881
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4887
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            Array.Copy(FList, Index + 1, FList, Index, FCount - Index);
        }
    }

    // Engine.pas:4907
    public void Exchange(int Index1, int Index2)
    {
        NotifyEvent<object> Item;
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        Item = FList[Index1];
        FList[Index1] = FList[Index2];
        FList[Index2] = Item;
    }

    // Engine.pas:4918
    public EventList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4924
    public NotifyEvent<object> First() => Get(0);

    // Engine.pas:4947
    public int IndexOf(NotifyEvent<object> Item)
    {
        Delegate M;
        int Result = 0;
        M = Item;
        while (Result < FCount && (MethodTarget(FList[Result]) != MethodTarget(M) || MethodCode(FList[Result]) != MethodCode(M)))
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:4958
    public void Insert(int Index, NotifyEvent<object> Item)
    {
        if (Index < 0 || Index > FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (FCount == FCapacity)
        {
            Grow();
        }

        if (Index < FCount)
        {
            Array.Copy(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // Engine.pas:4969
    public NotifyEvent<object> Last() => Get(FCount - 1);

    // Engine.pas:4974
    public void Move(int CurIndex, int NewIndex)
    {
        NotifyEvent<object> Item;
        if (CurIndex != NewIndex)
        {
            if (NewIndex < 0 || NewIndex >= FCount)
            {
                Error("List index out of bounds (%d)", NewIndex);
            }

            Item = Get(CurIndex);
            Delete(CurIndex);
            Insert(NewIndex, Item);
        }
    }

    // Engine.pas:4994
    public int Remove(NotifyEvent<object> Item)
    {
        int Result = default;
        Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // Engine.pas:5000
    public void Pack()
    {
        Delegate M;
        M = FDefault;
        // { sic: the original deletes an item only when both halves differ from the default }
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (MethodTarget(FList[I]) != MethodTarget(M) && MethodCode(FList[I]) != MethodCode(M))
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:5039
    public bool Compare(EventList List) => List.FCount == FCount && FList.AsSpan(0, FCount).SequenceEqual(List.FList.AsSpan(0, FCount));

    // Engine.pas:5046
    public void TriggerNotifyEvents(object Sender)
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (FList[I] != null)
            {
                FList[I](Sender);
            }
        }
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }
    public NotifyEvent<object> Default { get => FDefault; set => FDefault = value; }

    public NotifyEvent<object> this[int Index] { get => Get(Index); set => Put(Index, value); }

    public NotifyEvent<object>[] List { get => FList; }

    // Engine.pas:4896
    protected internal static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

}
