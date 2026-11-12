// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FTick, FCount, FTimerLists[1.10]. Children sit in one of ten lists and get TimerEvent every
// tick, every second tick. every tenth
public partial class ETimerNode : ECustomNode
{
    // Engine.pas:1836
    public new const int ClassId = 264;

    private int _tick;
    private int _count;
    private ListArray10 _timerLists;

    // Engine.pas:1856
    internal int FindTimerList(EngineObject child)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I < 10)
        {
            I += 1;
            // { the original indexes the lists with Result (-1 on the first round), not with I }
            result = _timerLists[result - 1].IndexOf(child);
        }

        if (result != -1)
        {
            result = I;
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:1819
    public ETimerNode(ECustomNode owner) : base(owner)
    {
        _count = 0;
        for (int I = 1; I <= 10; I++)
        {
            var list = new DelphiList();
            _timerLists[I - 1] = list;
        }
    }

    // Engine.pas:1828
    public override void Dispose()
    {
        base.Dispose();
        for (int I = 1; I <= 10; I++)
        {
            _timerLists[I - 1]?.Dispose();
        }
    }

    // Engine.pas:1841
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared, EngineServices.Reserved2);

    // Engine.pas:1846
    public override int GetCount() => _count;

    // Engine.pas:1851
    public DelphiList GetTimerList(sbyte timer) => _timerLists[timer - 1];

    // Engine.pas:1871
    public override int RemoveChild(EngineObject child)
    {
        int result;
        int L;
        L = FindTimerList(child);
        if (L != -1)
        {
            result = _timerLists[L - 1].Remove(child);
            if (result != -1)
            {
                _count -= 1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:1885
    public override int AddChild(EngineObject child)
    {
        int result = _timerLists[1 - 1].Add(child);
        _count += 1;
        return result;
    }

    // Engine.pas:1891
    public override EngineObject GetChild(int index)
    {
        int I;
        int total;
        DelphiList L;
        I = 1;
        L = _timerLists[1 - 1];
        total = L.Count;
        while (index >= total && I < 10)
        {
            I += 1;
            L = _timerLists[I - 1];
            total += L.Count;
        }

        L = _timerLists[I - 1];
        total -= L.Count;
        return (EngineObject)L.List[index - total];
    }

    // Engine.pas:1910
    public override void SetChild(int index, EngineObject child)
    {
        throw new NotSupportedException("Set Child not supported (TETimerNode)");
    }

    // Engine.pas:1915
    public int AddTimerChild(EngineObject child, sbyte timer)
    {
        int result = _timerLists[timer - 1].Add(child);
        _count += 1;
        return result;
    }

    // Engine.pas:1921
    public int RemoveTimerChild(EngineObject child, sbyte timer)
    {
        int result = _timerLists[timer - 1].Remove(child);
        if (result != -1)
        {
            _count -= 1;
        }

        return result;
    }

    // Engine.pas:1927
    public void TriggerTimerListEvent(int timer)
    {
        for (int I = 0, last = _timerLists[timer - 1].Count - 1; I <= last; I++)
        {
            ((EngineObject)_timerLists[timer - 1].List[I]).TimerEvent(_tick, this);
        }
    }

    // Engine.pas:1935
    public void TriggerTimerEvent()
    {
        if (_tick > 268435455)
        {
            _tick = _tick & 15;
        }

        _tick += 1;
        TriggerTimerListEvent(1);
        if ((_tick & 1) == 0)
        {
            TriggerTimerListEvent(2);
        }

        if (_tick % 3 == 0)
        {
            TriggerTimerListEvent(3);
        }

        if ((_tick & 3) == 2)
        {
            TriggerTimerListEvent(4);
        }

        if (_tick % 5 == 0)
        {
            TriggerTimerListEvent(5);
        }

        if (_tick % 6 == 0)
        {
            TriggerTimerListEvent(6);
        }

        if (_tick % 7 == 0)
        {
            TriggerTimerListEvent(7);
        }

        if ((_tick & 7) == 4)
        {
            TriggerTimerListEvent(8);
        }

        if (_tick % 9 == 0)
        {
            TriggerTimerListEvent(9);
        }

        if (_tick % 10 == 0)
        {
            TriggerTimerListEvent(10);
        }
    }

    public int Tick { get => _tick; }

}
