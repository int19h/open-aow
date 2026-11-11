// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FTick, FCount, FTimerLists[1.10]. Children sit in one of ten lists and get TimerEvent every
// tick, every second tick. every tenth
public partial class ETimerNode : ECustomNode
{
    // Engine.pas:1836
    public new const int ClassID = 264;

    internal int FTick;
    internal int FCount;
    internal ListArray10 FTimerLists;

    // Engine.pas:1856
    internal int FindTimerList(EngineObject Child)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I < 10)
        {
            I += 1;
            // { the original indexes the lists with Result (-1 on the first round), not with I }
            Result = FTimerLists[Result - 1].IndexOf(Child);
        }

        if (Result != -1)
        {
            Result = I;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:1819
    public ETimerNode(ECustomNode owner) : base(owner)
    {
        FCount = 0;
        for (int I = 1; I <= 10; I++)
        {
            var list = new DelphiList();
            FTimerLists[I - 1] = list;
        }
    }

    // Engine.pas:1828
    public override void Dispose()
    {
        base.Dispose();
        for (int I = 1; I <= 10; I++)
        {
            FTimerLists[I - 1]?.Dispose();
        }
    }

    // Engine.pas:1841
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared, EngineServices.csReserved2);

    // Engine.pas:1846
    public override int GetCount() => FCount;

    // Engine.pas:1851
    public DelphiList GetTimerList(sbyte Timer) => FTimerLists[Timer - 1];

    // Engine.pas:1871
    public override int RemoveChild(EngineObject Child)
    {
        int Result;
        int L;
        L = FindTimerList(Child);
        if (L != -1)
        {
            Result = FTimerLists[L - 1].Remove(Child);
            if (Result != -1)
            {
                FCount -= 1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Engine.pas:1885
    public override int AddChild(EngineObject Child)
    {
        int Result = FTimerLists[1 - 1].Add(Child);
        FCount += 1;
        return Result;
    }

    // Engine.pas:1891
    public override EngineObject GetChild(int Index)
    {
        int I;
        int Total;
        DelphiList L;
        I = 1;
        L = FTimerLists[1 - 1];
        Total = L.Count;
        while (Index >= Total && I < 10)
        {
            I += 1;
            L = FTimerLists[I - 1];
            Total += L.Count;
        }

        L = FTimerLists[I - 1];
        Total -= L.Count;
        return (EngineObject)L.List[Index - Total];
    }

    // Engine.pas:1910
    public override void SetChild(int Index, EngineObject Child)
    {
        throw new NotSupportedException("Set Child not supported (TETimerNode)");
    }

    // Engine.pas:1915
    public int AddTimerChild(EngineObject Child, sbyte Timer)
    {
        int Result = FTimerLists[Timer - 1].Add(Child);
        FCount += 1;
        return Result;
    }

    // Engine.pas:1921
    public int RemoveTimerChild(EngineObject Child, sbyte Timer)
    {
        int Result = FTimerLists[Timer - 1].Remove(Child);
        if (Result != -1)
        {
            FCount -= 1;
        }

        return Result;
    }

    // Engine.pas:1927
    public void TriggerTimerListEvent(int Timer)
    {
        for (int I = 0, last = FTimerLists[Timer - 1].Count - 1; I <= last; I++)
        {
            ((EngineObject)FTimerLists[Timer - 1].List[I]).TimerEvent(FTick, this);
        }
    }

    // Engine.pas:1935
    public void TriggerTimerEvent()
    {
        if (FTick > 268435455)
        {
            FTick = FTick & 15;
        }

        FTick += 1;
        TriggerTimerListEvent(1);
        if ((FTick & 1) == 0)
        {
            TriggerTimerListEvent(2);
        }

        if (FTick % 3 == 0)
        {
            TriggerTimerListEvent(3);
        }

        if ((FTick & 3) == 2)
        {
            TriggerTimerListEvent(4);
        }

        if (FTick % 5 == 0)
        {
            TriggerTimerListEvent(5);
        }

        if (FTick % 6 == 0)
        {
            TriggerTimerListEvent(6);
        }

        if (FTick % 7 == 0)
        {
            TriggerTimerListEvent(7);
        }

        if ((FTick & 7) == 4)
        {
            TriggerTimerListEvent(8);
        }

        if (FTick % 9 == 0)
        {
            TriggerTimerListEvent(9);
        }

        if (FTick % 10 == 0)
        {
            TriggerTimerListEvent(10);
        }
    }

    public int Tick { get => FTick; }

}
