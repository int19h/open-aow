using static Aow.Rtl.RtlUnit;

namespace Aow.Rtl.Tests;

public sealed class MemoryProbeTests
{
    private struct Item
    {
        public int First { get; set; }
        public int Second { get; set; }
    }

    [ProbeFact("integer-list-slots")]
    public void IntegerListSlots()
    {
        ProbeLines probe = new("integer-list-slots");
        DelphiList<object> list = new();
        object stored = new();
        list.Count = 3;
        for (int i = 0; i < 3; i++)
        {
            list[i] = null;
        }

        probe.Fact("nil", ObjectToInt(list[0]));
        list[0] = IntToObject(17);
        list[1] = IntToObject(-9);
        list[2] = IntToObject(17);
        int total = 0;
        for (int i = 0; i < list.Count; i++)
        {
            total += ObjectToInt(list[i]);
        }

        probe.Fact("sum", total);
        probe.Fact("same_box", Ord(ReferenceEquals(list[0], list[2])));
        probe.Fact("find_box", list.IndexOf(IntToObject(17)));
        list.Insert(1, IntToObject(int.MinValue));
        list.Add(IntToObject(int.MaxValue));
        probe.Fact("minimum", ObjectToInt(list[1]));
        probe.Fact("maximum", ObjectToInt(list[4]));
        if (ObjectToInt(list[0]) < 30)
        {
            list[0] = IntToObject(30);
        }

        probe.Fact("maximum_score", ObjectToInt(list[0]));
        list[0] = IntToObject(0);
        probe.Fact("zero_is_nil", Ord(list[0] == null));
        list[0] = stored;
        probe.Fact("object_identity", Ord(ObjectToInt(list[0]) == ObjectToInt(stored)));
        probe.Fact("object_recovered", Ord(ReferenceEquals(list[0], stored)));
        probe.End();
    }

    [ProbeFact("record-move")]
    public void RecordMove()
    {
        ProbeLines probe = new("record-move");
        Ptr<Item> items = Ptr<Item>.Alloc(5);
        for (int i = 0; i < 5; i++)
        {
            items[i].First = i + 1;
            items[i].Second = (i + 1) * 10;
        }
        int count = 3;
        Ptr<Item>.Copy(items + 1, items, count);
        for (int i = 0; i < 5; i++)
        {
            probe.Fact("left", i + " " + items[i].First + " " + items[i].Second);
        }

        Ptr<Item>.Copy(items, items + 1, count);
        for (int i = 0; i < 5; i++)
        {
            probe.Fact("right", i + " " + items[i].First + " " + items[i].Second);
        }

        count = 0;
        Ptr<Item>.Copy(items + 4, items, count);
        probe.Fact("zero", items.Value.First + " " + items.Value.Second);
        probe.End();
    }

    [ProbeFact("reference-fill")]
    public void ReferenceFill()
    {
        ProbeLines probe = new("reference-fill");
        Ptr<object?> items = Ptr<object?>.Alloc(3);
        object item = new();
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
