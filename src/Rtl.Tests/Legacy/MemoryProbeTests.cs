using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class MemoryProbeTests
{
    private struct Item
    {
        public int A;
        public int B;
    }

    [ProbeFact("integer-list-slots")]
    public void IntegerListSlots()
    {
        var probe = new ProbeLines("integer-list-slots");
        var list = new DelphiList();
        var stored = new object();
        list.Count = 3;
        for (int i = 0; i < 3; i++) list[i] = null;
        probe.Fact("nil", ObjectToInt((object?)list[0]));
        list[0] = IntToObject(17);
        list[1] = IntToObject(-9);
        list[2] = IntToObject(17);
        int total = 0;
        for (int i = 0; i < list.Count; i++) total += ObjectToInt((object?)list[i]);
        probe.Fact("sum", total);
        probe.Fact("same_box", Ord(ReferenceEquals(list[0], list[2])));
        probe.Fact("find_box", list.IndexOf(IntToObject(17)));
        list.Insert(1, IntToObject(int.MinValue));
        list.Add(IntToObject(int.MaxValue));
        probe.Fact("minimum", ObjectToInt((object?)list[1]));
        probe.Fact("maximum", ObjectToInt((object?)list[4]));
        if (ObjectToInt((object?)list[0]) < 30) list[0] = IntToObject(30);
        probe.Fact("maximum_score", ObjectToInt((object?)list[0]));
        list[0] = IntToObject(0);
        probe.Fact("zero_is_nil", Ord(list[0] == null));
        list[0] = stored;
        probe.Fact("object_identity", Ord(ObjectToInt((object?)list[0]) == ObjectToInt(stored)));
        probe.Fact("object_recovered", Ord(ReferenceEquals(list[0], stored)));
        probe.End();
    }

    [ProbeFact("record-move")]
    public void RecordMove()
    {
        var probe = new ProbeLines("record-move");
        var items = Ptr<Item>.Alloc(5);
        for (int i = 0; i < 5; i++)
        {
            items[i].A = i + 1;
            items[i].B = (i + 1) * 10;
        }
        int count = 3;
        Ptr<Item>.Copy(items + 1, items, count);
        for (int i = 0; i < 5; i++) probe.Fact("left", i + " " + items[i].A + " " + items[i].B);
        Ptr<Item>.Copy(items, items + 1, count);
        for (int i = 0; i < 5; i++) probe.Fact("right", i + " " + items[i].A + " " + items[i].B);
        count = 0;
        Ptr<Item>.Copy(items + 4, items, count);
        probe.Fact("zero", items.Value.A + " " + items.Value.B);
        probe.End();
    }

    [ProbeFact("reference-fill")]
    public void ReferenceFill()
    {
        var probe = new ProbeLines("reference-fill");
        var items = Ptr<object?>.Alloc(3);
        var item = new object();
        items[0] = item;
        items[1] = item;
        items[2] = item;
        Ptr<object?>.Fill(items + 1, 2, null);
        probe.Fact("first", Ord(items[0] == item));
        probe.Fact("second", Ord(items[1] == null));
        probe.Fact("third", Ord(items[2] == null));
        Ptr<object?>.Fill(items, 3, null);
        probe.Fact("root", Ord(items[0] == null && items[1] == null && items[2] == null));
        probe.End();
    }
}
