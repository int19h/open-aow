
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringsTests
{
    private sealed class TrackObject : IDisposable
    {
        internal bool Destroyed { get; private set; }

        public void Dispose() => Destroyed = true;
    }

    [Fact]
    public void ObjectsNamesValuesAndAssignmentPreserveSourceRules()
    {
        // CLASSES.PAS:2125..2163 and :2263..2354.
        var list = new StringList(); var objectValue = new TrackObject(); var other = new TrackObject();
        Assert.Equal(0, list.AddObject("First=one", objectValue)); list.InsertObject(1, "blank", other); list.Add("FIRST=two"); list.Add("=emptyname");
        Assert.Equal("First", list.Names[0]); Assert.Equal("", list.Names[1]); Assert.Equal(0, list.IndexOfName("first")); Assert.Equal(3, list.IndexOfName(""));
        Assert.Equal(1, list.IndexOfObject(other)); Assert.Equal(2, list.IndexOfObject(null)); Assert.Equal(-1, list.IndexOfObject(new object()));
        Assert.Equal("one", list.Values["FIRST"]); Assert.Equal("", list.Values["missing"]);
        list.Values["FIRST"] = "updated"; Assert.Equal("FIRST=updated", list.Strings[0]); Assert.Same(objectValue, list.Objects[0]);
        list.Values["added"] = "new"; Assert.Equal("new", list.Values["added"]); list.Values["added"] = ""; Assert.Equal(-1, list.IndexOfName("added")); list.Values["absent"] = "";
        list.Strings[1] = "changed"; Assert.Equal("changed", list[1]);
        list.Objects[1] = objectValue; Assert.Same(objectValue, list.Objects[1]);
        var copy = new StringList(); copy.Assign(list); Assert.Equal(list.Text, copy.Text); Assert.Same(objectValue, copy.Objects[0]);
        copy.AddStrings(list); Assert.Equal(list.Count * 2, copy.Count);
        list.Move(0, 2); Assert.Equal("FIRST=updated", list[2]); Assert.Same(objectValue, list.Objects[2]);
        list.Exchange(0, 2); Assert.Equal("FIRST=updated", list[0]); Assert.Same(objectValue, list.Objects[0]);
        list.Move(-1, -1);
        Assert.Throws<FormatException>(() => copy.Assign(new Persistent()));
        copy.Assign(copy); Assert.Equal(0, copy.Count);
        list.Dispose(); Assert.False(objectValue.Destroyed); Assert.False(other.Destroyed);
    }

    [Fact]
    public void StringListEventsBatchAndSortedMutationErrorsFollowSource()
    {
        // CLASSES.PAS:2575..2615, :2719..2733 and :2775..2788.
        var list = new StringList(); var events = new List<string>();
        list.OnChanging = s => { Assert.Same(list, s); events.Add("changing"); };
        list.OnChange = s => { Assert.Same(list, s); events.Add("changed"); };
        list.Clear(); Assert.Empty(events);
        list.BeginUpdate(); list.BeginUpdate(); list.Add("b"); list.Add("a"); list.EndUpdate(); Assert.Equal(new[] { "changing" }, events); list.EndUpdate(); Assert.Equal(new[] { "changing", "changed" }, events);
        events.Clear(); list.Sorted = true; Assert.Equal(new[] { "changing", "changed" }, events); Assert.Equal("a", list[0]);
        events.Clear(); list.Sort(); list.Sorted = true; Assert.Empty(events);
        Assert.Throws<InvalidOperationException>(() => list[0] = "z"); Assert.Throws<InvalidOperationException>(() => list.Insert(0, "z"));
        list.Objects[0] = new object(); list.Exchange(0, 1); Assert.Equal(4, events.Count);
        list.Sorted = false; events.Clear(); list[0] = "x"; list.Delete(1); Assert.Equal(4, events.Count);
        events.Clear(); list.Clear(); Assert.Equal(new[] { "changing", "changed" }, events); Assert.Equal(0, list.Capacity);
        list.Capacity = 8; list.Clear(); Assert.Equal(8, list.Capacity);
        list.Add("a"); Assert.Throws<ArgumentOutOfRangeException>(() => list.Capacity = 0);
        foreach (int i in new[] { -1, 1 })
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => list[i]); Assert.Throws<ArgumentOutOfRangeException>(() => list[i] = "x");
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Objects[i]); Assert.Throws<ArgumentOutOfRangeException>(() => list.Objects[i] = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Delete(i)); Assert.Throws<ArgumentOutOfRangeException>(() => list.Exchange(0, i));
        }
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, "x")); Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(2, "x"));
        list.Duplicates = Duplicates.Ignore; list.Sorted = true; list.AddObject("A", new object()); Assert.Equal(1, list.Count);
        events.Clear(); list.Dispose(); Assert.Empty(events); Assert.Null(list.OnChange); Assert.Null(list.OnChanging);
    }

    [Fact]
    public void CommaTextPortsTheDelphiParserIncludingEmptyAndTrailingFields()
    {
        // CLASSES.PAS:2239..2261 and :2454..2484, also checked with DCC32.
        var list = new StringList();
        foreach (var (input, expected, canonical) in new[]{
            ("",Array.Empty<string>(),""), ("a,,b,",new[]{"a","","b"},"a,,b"), (",,",new[]{"",""},","),
            ("\"a\"\"b\", \"\",c",new[]{"a\"b","","c"},"\"a\"\"b\",,c"),
            ("a b, c",new[]{"a","b","c"},"a,b,c"), (" \"a b\",\"x,y\" ",new[]{"a b","x,y"},"\"a b\",\"x,y\""),
            ("\"\"",new[]{""},"\"\""), ("\"unterminated",new[]{"unterminated"},"unterminated"), ("a\0b",new[]{"a"},"a") })
        {
            list.CommaText = input; Assert.Equal(expected, Enumerable.Range(0, list.Count).Select(i => list[i])); Assert.Equal(canonical, list.CommaText);
        }
        list.Clear(); list.Add("a\tb"); list.Add("c\0d"); Assert.Equal("\"a\tb\",c\0d", list.CommaText);
        // Both branches keep a char outside the table (strings.md 2.6).
        list.CommaText = "\"\u0141,x\",\u03a9"; Assert.Equal(new[] { "\u0141,x", "\u03a9" }, Enumerable.Range(0, list.Count).Select(i => list[i]));
    }

    // A minimal TStrings descendant exercises the base implementations separately from TStringList.
    private sealed class PlainStrings : StringCollection
    {
        private readonly List<string> _items = [];

        public override int Count => _items.Count;
        public override string this[int index] { get => _items[index]; set => _items[index] = value; }

        public override void Insert(int index, string value) => _items.Insert(index, value);
        public override void Delete(int index) => _items.RemoveAt(index);
        public override void Clear() => _items.Clear();
    }

    [Fact]
    public void BaseStringsUpdateAssignExchangeMoveAndStreamMethods()
    {
        // CLASSES.PAS:2119..2540.
        using var directory = new ProbeDirectory();
        var source = new PlainStrings(); var ignored = new object();
        source.AddObject("a", ignored); source.InsertObject(1, "b", ignored); Assert.Null(source.Objects[0]); source.Objects[0] = ignored;
        Assert.Equal(0, source.IndexOfObject(null)); Assert.Equal(-1, source.IndexOfObject(ignored));
        Assert.Equal(2, source.Capacity); source.Capacity = 100; Assert.Equal(2, source.Capacity);
        source.Exchange(0, 1); Assert.Equal("b", source[0]); source.Move(0, 1); Assert.Equal("a", source[0]); source.Move(-1, -1);
        Assert.Equal(0, source.IndexOf("A")); Assert.Equal(-1, source.IndexOf("c"));
        var copy = new PlainStrings(); copy.Assign(source); copy.AddStrings(source); Assert.Equal("a\r\nb\r\na\r\nb\r\n", copy.Text);
        using var memory = new MemoryStream(); memory.WriteBuffer(new byte[] { 0x78, 0x61, 0x0a, 0x62 }); memory.Position = 1;
        copy.LoadFromStream(memory); Assert.Equal("a\r\nb\r\n", copy.Text);
        memory.Clear(); copy.SaveToStream(memory); Assert.Equal(new byte[] { 97, 13, 10, 98, 13, 10 }, memory.ToArray());
        copy.SaveToFile("strings.txt"); copy.Clear(); copy.LoadFromFile("strings.txt"); Assert.Equal(source.Text, copy.Text);
        Assert.Equal("error 1", Assert.Throws<ArgumentException>(() => StringCollection.Error("error %d", 1)).Message);
        copy.BeginUpdate(); copy.EndUpdate(); copy.Dispose();
        var readOnly = new IndexedProperty<int, int>(i => i + 1); Assert.Equal(3, readOnly[2]); Assert.Throws<InvalidOperationException>(() => readOnly[2] = 4);
    }
}
