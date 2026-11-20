using static Aow.Rtl.DelphiOrdinals;

namespace Aow.Rtl.Tests;

public sealed class ListsProbeTests
{
    private sealed class Item(int id, int key)
    {
        internal int Id { get; } = id;
        internal int Key { get; set; } = key;
    }

    private sealed class ObjectId(int id) { internal int Id { get; } = id; }

    [ProbeFact("lists")]
    public void ListsFixture()
    {
        ProbeLines p = new("lists");
        DelphiList<Item> l = new();
        Item[] items = [.. Enumerable.Range(0, count: 257).Select(i => new Item(i, key: i % 3))];
        void State(string tag)
        {
            p.Fact($"{tag}_count", value: l.Count);
            p.Fact($"{tag}_capacity", value: l.Capacity);
            for (int i = 0; i < l.Count; i++)
            {
                p.Fact($"{tag}_item_{i}", value: l[i] is Item item ? item.Id : "nil");
            }
        }
        p.Fact("growth_initial_capacity", value: l.Capacity);
        for (int i = 0; i < 140; i++)
        {
            p.Fact($"growth_add_index_{i}", value: l.Add(items[i]));
            p.Fact($"growth_capacity_{i}", value: l.Capacity);
        }
        l.Clear();
        State("clear");
        for (int i = 0; i < 4; i++)
        {
            _ = l.Add(items[i]);
        }

        State("filled_four");
        l.Insert(0, item: items[4]);
        State("insert_start");
        l.Insert(3, item: items[5]);
        State("insert_middle");
        l.Insert(l.Count, item: items[6]);
        State("insert_end");
        p.Fact("indexof_present", value: l.IndexOf(items[5]));
        p.Fact("indexof_absent", value: l.IndexOf(items[99]));
        p.Fact("indexof_nil_absent", value: l.IndexOf(null));
        l.Exchange(0, second: l.Count - 1);
        State("exchange");
        l.Delete(2);
        State("delete_middle");
        l.Delete(0);
        State("delete_first");
        l.Delete(l.Count - 1);
        State("delete_last");
        _ = l.Add(null);
        l.Insert(1, item: null);
        _ = l.Add(items[1]);
        State("nil_entries");
        p.Fact("indexof_nil", value: l.IndexOf(null));
        l.Pack();
        State("pack");
        p.Fact("remove_present", value: l.Remove(items[1]));
        State("remove");
        p.Fact("remove_absent", value: l.Remove(items[99]));
        l.Move(0, target: l.Count - 1);
        State("move");
        p.Fact("first_identity", value: l.First()!.Id);
        p.Fact("last_identity", value: l.Last()!.Id);
        l.Capacity = l.Count;
        State("capacity_exact");
        _ = l.Expand();
        State("expand_full");
        _ = l.Expand();
        State("expand_spare");
        p.Fact("delete_outside", value: p.Thrown(() => l.Delete(l.Count)));
        p.Fact("insert_outside", value: p.Thrown(() => l.Insert(-1, item: null)));
        foreach (int capacity in new[] { 8, 64 })
        {
            l.Clear();
            l.Capacity = capacity;
            for (int i = 0; i < capacity; i++)
            {
                _ = l.Add(items[i]);
            }

            _ = l.Expand();
            State($"expand_capacity{capacity}");
        }
        for (int pattern = 0; pattern <= 2; pattern++)
        {
            foreach (int n in new[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 16, 17, 32, 64, 65, 100, 257 })
            {
                l.Clear();
                for (int i = 0; i < n; i++)
                {
                    items[i].Key = pattern switch { 0 => 0, 1 => i % 3, _ => (n - i) % 5 };
                    _ = l.Add(items[i]);
                }
                l.Sort((a, b) => a!.Key - b!.Key);
                for (int i = 0; i < n; i++)
                {
                    p.Fact($"sort_{pattern}_{n}_id_{i}", value: l[i]!.Id);
                    p.Fact($"sort_{pattern}_{n}_key_{i}", value: l[i]!.Key);
                }
                p.Fact($"sort_{pattern}_{n}_count", value: l.Count);
            }
        }

        DelphiStringList sl = new();
        void StringState(string tag)
        {
            p.Fact($"{tag}_count", value: sl.Count);
            for (int i = 0; i < sl.Count; i++)
            {
                p.Fact($"{tag}_text_{i}", value: $"[{sl[i]}]");
                p.Fact($"{tag}_object_{i}", value: (sl.Objects[i] as ObjectId)?.Id ?? 0);
            }
        }
        void Add(string tag, string s, int id)
        {
            try
            {
                p.Fact($"{tag}_index", value: sl.AddObject(s, item: new ObjectId(id)));
            }
            catch (Exception e)
            {
                p.Fact($"{tag}_error", value: p.DelphiName(e));
            }
            StringState(tag);
        }
        void Lookup(string tag, string s)
        {
            p.Fact($"{tag}_found", value: Ord(sl.Find(s, index: out int at)));
            p.Fact($"{tag}_insertion_index", value: at);
            p.Fact($"{tag}_indexof", value: sl.IndexOf(s));
        }
        p.Fact("default_sorted", value: Ord(sl.Sorted));
        p.Fact("default_duplicates", value: (int)sl.Duplicates);
        for (int d = 0; d < 3; d++)
        {
            sl.Sorted = false;
            sl.Clear();
            sl.Duplicates = (Duplicates)d;
            Add($"unsorted_{d}_z", s: "zeta", id: 1);
            Add($"unsorted_{d}_b", s: "Beta", id: 2);
            Add($"unsorted_{d}_a", s: "alpha", id: 3);
            Add($"unsorted_{d}_duplicate", s: "ALPHA", id: 4);
            Lookup($"unsorted_{d}_lookup", s: "Alpha");
            Lookup($"unsorted_{d}_missing", s: "delta");
            sl.Sorted = true;
            StringState($"enabled_sorted_{d}");
            Lookup($"enabled_{d}_lookup", s: "alpha");
            sl.Clear();
            Add($"sorted_{d}_z", s: "zeta", id: 1);
            Add($"sorted_{d}_b", s: "Beta", id: 2);
            Add($"sorted_{d}_a", s: "alpha", id: 3);
            Add($"sorted_{d}_duplicate", s: "ALPHA", id: 4);
            Add($"sorted_{d}_same", s: "alpha", id: 5);
            Add($"sorted_{d}_empty", s: "", id: 6);
            Add($"sorted_{d}_prefix", s: "alp", id: 7);
            Lookup($"sorted_{d}_lookup", s: "Alpha");
            Lookup($"sorted_{d}_before", s: "!");
            Lookup($"sorted_{d}_between", s: "delta");
            Lookup($"sorted_{d}_after", s: "zzzz");
            Lookup($"sorted_{d}_empty_lookup", s: "");
            p.Fact($"sorted_{d}_insert_error", value: p.Thrown(() => sl.Insert(0, value: "illegal")));
        }
        sl.Clear();
        p.Fact("empty_find", value: Ord(sl.Find("absent", out int index)));
        p.Fact("empty_find_index", value: index);
        p.Fact("empty_indexof", value: sl.IndexOf("absent"));
        p.End();
    }
}
