using static Aow.Rtl.IntegerObjects;

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
        DelphiList<object> list = [];
        object stored = new();
        list.Count = 3;
        for (int i = 0; i < 3; i++)
        {
            list[i] = null;
        }

        probe.Fact("nil", value: ObjectToInt(list[0]));
        list[0] = IntToObject(17);
        list[1] = IntToObject(-9);
        list[2] = IntToObject(17);
        int total = 0;
        for (int i = 0; i < list.Count; i++)
        {
            total += ObjectToInt(list[i]);
        }

        probe.Fact("sum", value: total);
        probe.Fact("same_box", value: ReferenceEquals(list[0], list[2]) ? 1 : 0);
        probe.Fact("find_box", value: list.IndexOf(IntToObject(17)));
        list.Insert(1, item: IntToObject(int.MinValue));
        _ = list.Add(IntToObject(int.MaxValue));
        probe.Fact("minimum", value: ObjectToInt(list[1]));
        probe.Fact("maximum", value: ObjectToInt(list[4]));
        if (ObjectToInt(list[0]) < 30)
        {
            list[0] = IntToObject(30);
        }

        probe.Fact("maximum_score", value: ObjectToInt(list[0]));
        list[0] = IntToObject(0);
        probe.Fact("zero_is_nil", value: list[0] is null ? 1 : 0);
        list[0] = stored;
        probe.Fact("object_identity", value: ObjectToInt(list[0]) == ObjectToInt(stored) ? 1 : 0);
        probe.Fact("object_recovered", value: ReferenceEquals(list[0], stored) ? 1 : 0);
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
        Ptr<Item>.Copy(items + 1, destination: items, count);
        for (int i = 0; i < 5; i++)
        {
            probe.Fact("left", value: $"{i} {items[i].First} {items[i].Second}");
        }

        Ptr<Item>.Copy(items, destination: items + 1, count);
        for (int i = 0; i < 5; i++)
        {
            probe.Fact("right", value: $"{i} {items[i].First} {items[i].Second}");
        }

        count = 0;
        Ptr<Item>.Copy(items + 4, destination: items, count);
        probe.Fact("zero", value: $"{items.Value.First} {items.Value.Second}");
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
        Ptr<object?>.Fill(items + 1, count: 2, value: null);
        probe.Fact("first", value: items[0] == item ? 1 : 0);
        probe.Fact("second", value: items[1] is null ? 1 : 0);
        probe.Fact("third", value: items[2] is null ? 1 : 0);
        Ptr<object?>.Fill(items, count: 3, value: null);
        probe.Fact("root", value: items[0] is null && items[1] is null && items[2] is null ? 1 : 0);
        probe.End();
    }
}
