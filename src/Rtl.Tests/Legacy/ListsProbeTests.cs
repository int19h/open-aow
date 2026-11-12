using static Aow.Rtl.RtlUnit;

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
        var p = new ProbeLines("lists");
        var l = new DelphiList();
        Item[] items = Enumerable.Range(0, 257).Select(i => new Item(i, i % 3)).ToArray();
        void State(string tag)
        {
            p.Fact(tag + "_count", l.Count); p.Fact(tag + "_capacity", l.Capacity);
            for (int i = 0; i < l.Count; i++) p.Fact($"{tag}_item_{i}", l[i] is Item item ? item.Id : "nil");
        }
        p.Fact("growth_initial_capacity", l.Capacity);
        for (int i = 0; i < 140; i++) { p.Fact($"growth_add_index_{i}", l.Add(items[i])); p.Fact($"growth_capacity_{i}", l.Capacity); }
        l.Clear(); State("clear"); for (int i = 0; i < 4; i++) l.Add(items[i]); State("filled_four");
        l.Insert(0, items[4]); State("insert_start"); l.Insert(3, items[5]); State("insert_middle"); l.Insert(l.Count, items[6]); State("insert_end");
        p.Fact("indexof_present", l.IndexOf(items[5])); p.Fact("indexof_absent", l.IndexOf(items[99])); p.Fact("indexof_nil_absent", l.IndexOf(null));
        l.Exchange(0, l.Count - 1); State("exchange"); l.Delete(2); State("delete_middle"); l.Delete(0); State("delete_first"); l.Delete(l.Count - 1); State("delete_last");
        l.Add(null); l.Insert(1, null); l.Add(items[1]); State("nil_entries"); p.Fact("indexof_nil", l.IndexOf(null)); l.Pack(); State("pack");
        p.Fact("remove_present", l.Remove(items[1])); State("remove"); p.Fact("remove_absent", l.Remove(items[99])); l.Move(0, l.Count - 1); State("move");
        p.Fact("first_identity", ((Item)l.First()!).Id); p.Fact("last_identity", ((Item)l.Last()!).Id);
        l.Capacity = l.Count; State("capacity_exact"); l.Expand(); State("expand_full"); l.Expand(); State("expand_spare");
        p.Fact("delete_outside", p.Thrown(() => l.Delete(l.Count))); p.Fact("insert_outside", p.Thrown(() => l.Insert(-1, null)));
        foreach (int capacity in new[] { 8, 64 }) { l.Clear(); l.Capacity = capacity; for (int i = 0; i < capacity; i++) l.Add(items[i]); l.Expand(); State($"expand_capacity{capacity}"); }
        for (int pattern = 0; pattern <= 2; pattern++) foreach (int n in new[] { 0, 1, 2, 3, 4, 5, 7, 8, 9, 16, 17, 32, 64, 65, 100, 257 })
        {
            l.Clear(); for (int i = 0; i < n; i++) { items[i].Key = pattern switch { 0 => 0, 1 => i % 3, _ => (n - i) % 5 }; l.Add(items[i]); }
            l.Sort((a, b) => ((Item)a!).Key - ((Item)b!).Key);
            for (int i = 0; i < n; i++) { p.Fact($"sort_{pattern}_{n}_id_{i}", ((Item)l[i]!).Id); p.Fact($"sort_{pattern}_{n}_key_{i}", ((Item)l[i]!).Key); }
            p.Fact($"sort_{pattern}_{n}_count", l.Count);
        }
        var sl = new StringList();
        void StringState(string tag)
        {
            p.Fact(tag + "_count", sl.Count);
            for (int i = 0; i < sl.Count; i++) { p.Fact($"{tag}_text_{i}", "[" + sl[i] + "]"); p.Fact($"{tag}_object_{i}", (sl.Objects[i] as ObjectId)?.Id ?? 0); }
        }
        void Add(string tag, string s, int id)
        {
            try { p.Fact(tag + "_index", sl.AddObject(s, new ObjectId(id))); } catch (System.Exception e) { p.Fact(tag + "_error", p.DelphiName(e)); }
            StringState(tag);
        }
        void Lookup(string tag, string s)
        {
            int at = -99; bool hit = sl.Find(s, ref at); p.Fact(tag + "_found", Ord(hit)); p.Fact(tag + "_insertion_index", at); p.Fact(tag + "_indexof", sl.IndexOf(s));
        }
        p.Fact("default_sorted", Ord(sl.Sorted)); p.Fact("default_duplicates", (int)sl.Duplicates);
        for (int d = 0; d < 3; d++)
        {
            sl.Sorted = false; sl.Clear(); sl.Duplicates = (Duplicates)d;
            Add($"unsorted_{d}_z", "zeta", 1); Add($"unsorted_{d}_b", "Beta", 2); Add($"unsorted_{d}_a", "alpha", 3); Add($"unsorted_{d}_duplicate", "ALPHA", 4);
            Lookup($"unsorted_{d}_lookup", "Alpha"); Lookup($"unsorted_{d}_missing", "delta"); sl.Sorted = true; StringState($"enabled_sorted_{d}"); Lookup($"enabled_{d}_lookup", "alpha");
            sl.Clear(); Add($"sorted_{d}_z", "zeta", 1); Add($"sorted_{d}_b", "Beta", 2); Add($"sorted_{d}_a", "alpha", 3); Add($"sorted_{d}_duplicate", "ALPHA", 4); Add($"sorted_{d}_same", "alpha", 5); Add($"sorted_{d}_empty", "", 6); Add($"sorted_{d}_prefix", "alp", 7);
            Lookup($"sorted_{d}_lookup", "Alpha"); Lookup($"sorted_{d}_before", "!"); Lookup($"sorted_{d}_between", "delta"); Lookup($"sorted_{d}_after", "zzzz"); Lookup($"sorted_{d}_empty_lookup", "");
            p.Fact($"sorted_{d}_insert_error", p.Thrown(() => sl.Insert(0, "illegal")));
        }
        sl.Clear(); int index = -99; bool found = sl.Find("absent", ref index); p.Fact("empty_find", Ord(found)); p.Fact("empty_find_index", index); p.Fact("empty_indexof", sl.IndexOf("absent")); p.End();
    }
}
