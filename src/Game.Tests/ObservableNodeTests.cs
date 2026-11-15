using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Tests;

// Issue 158 part 10. CollectionChanged follows each Changed call, with the action and the
// index the call site knows. A batch folds into the one reset EndUpdate sends.
[Collection("Engine")]
[EngineState]
public sealed class ObservableNodeTests
{
    [Fact]
    public void AddsRemovesReplacementsAndDirectCallsMatchChanged()
    {
        var node = new RecordingNode();
        var first = new Leaf(1);
        var second = new Leaf(2);
        var third = new Leaf(3);
        node.AddChild(first);
        node.AddChild(second);
        node.SetChild(0, child: third);
        node.RemoveChild(second);
        node.RemoveChild(first);
        node.Changed();
        Assert.Equal(
            Calls(
                "Added:0:-1:1:-",
                "Added:1:-1:2:-",
                "Replaced:0:-1:3:1",
                "Removed:1:-1:2:-",
                "Removed:-1:-1:1:-",
                "Reset:-1:-1:-:-"),
            node.Trace);
        Assert.Equal([third], Children(node));

        node.Trace.Clear();
        var padded = new Leaf(7);
        node.SetChild(3, child: padded);
        Assert.Equal(Calls("Replaced:3:-1:7:-"), node.Trace);
        Assert.Equal(4, node.Count);
        Assert.Null(node.GetChild(1));
        Assert.Same(padded, node.GetChild(3));
    }

    [Fact]
    public void ClearSendsOneRemovePerChildAndThenAReset()
    {
        var node = new RecordingNode();
        node.AddChild(new Leaf(1));
        node.AddChild(new Leaf(2));
        node.Trace.Clear();
        node.Clear();
        Assert.Equal(
            Calls("Removed:1:-1:2:-", "Removed:0:-1:1:-", "Reset:-1:-1:-:-"),
            node.Trace);
        Assert.Equal(0, node.Count);

        node.Trace.Clear();
        node.Clear();
        Assert.Equal(Calls("Reset:-1:-1:-:-"), node.Trace);
    }

    [Fact]
    public void SortAndABatchFoldIntoTheResetEndUpdateSends()
    {
        var node = new RecordingNode();
        var low = new Leaf(1);
        var high = new Leaf(2);
        node.AddChild(high);
        node.AddChild(low);
        node.Trace.Clear();
        node.Sort((left, right) => Tag(left) - Tag(right));
        Assert.Equal(Calls("Reset:-1:-1:-:-"), node.Trace);
        Assert.Equal([low, high], Children(node));

        node.Trace.Clear();
        using (node.BeginUpdate())
        {
            node.AddChild(new Leaf(3));
            node.RemoveChild(low);
            node.SetChild(0, child: new Leaf(4));
            Assert.Empty(node.Trace);
        }

        Assert.Equal(Calls("Reset:-1:-1:-:-"), node.Trace);

        node.Trace.Clear();
        using (node.BeginUpdate())
        {
            using (node.BeginUpdate())
            {
                node.AddChild(new Leaf(5));
            }

            Assert.Empty(node.Trace);
        }

        Assert.Equal(Calls("Reset:-1:-1:-:-"), node.Trace);

        node.Trace.Clear();
        node.LockChanged();
        node.AddChild(new Leaf(6));
        node.UnlockChanged();
        Assert.Empty(node.Trace);

        node.Trace.Clear();
        using (node.BeginUpdate())
        {
            node.Sort((left, right) => Tag(left) - Tag(right));
            Assert.Empty(node.Trace);
        }

        Assert.Equal(Calls("Reset:-1:-1:-:-"), node.Trace);
    }

    [Fact]
    public void DirectReordersDoNotNotifyAndAMoveCarriesItsIndexes()
    {
        var node = new RecordingNode();
        var first = new Leaf(1);
        var second = new Leaf(2);
        node.AddChild(first);
        node.AddChild(second);
        node.Trace.Clear();
        node.MoveChild(0, target: 1);
        node.ExchangeChildren(0, second: 1);
        node.SortChildren((left, right) => Tag(left) - Tag(right));
        Assert.Empty(node.Trace);
        var moved = ObservableChange.Moved(0, newIndex: 1, item: first);
        Assert.Equal(ObservableChangeKind.Moved, moved.Kind);
        Assert.Equal(0, moved.Index);
        Assert.Equal(1, moved.NewIndex);
        Assert.Same(first, moved.Item);
        Assert.Null(moved.Previous);
    }

    [Fact]
    public void KeepIndexRemoveNotifiesTheSlotItClears()
    {
        var node = new KeepingNode();
        var child = new Leaf(1);
        node.AddChild(child);
        node.Trace.Clear();
        Assert.Equal(0, node.RemoveChild(child));
        Assert.Equal(Calls("Removed:0:-1:1:-"), node.Trace);
        Assert.Null(node.GetChild(0));
        Assert.Equal(1, node.Count);
    }

    [Fact]
    public void EventListRaisesItsMulticastBeforeTheCollectionEvent()
    {
        var list = new ObservableEventList(null!) { Holder = EngineFixture.Engine };
        var trace = new List<string>();
        list.Events += _ => trace.Add("events");
        list.CollectionChanged += (_, change) => trace.Add(change.Kind.ToString());
        list.AddChild(new Leaf(1));
        list.Changed();
        Assert.Equal(["events", "Added", "events", "Reset"], trace);
    }

    [Fact]
    public void ItemListOnChangedRunsBeforeTheCollectionEvent()
    {
        var items = new ItemList(null!) { Holder = EngineFixture.Engine };
        var trace = new List<string>();
        items.OnChanged += _ => trace.Add("on");
        items.CollectionChanged += (_, _) => trace.Add("collection");
        items.AddChild(new Item(null!) { Holder = EngineFixture.Engine });
        Assert.Equal(["on", "collection"], trace);
    }

    private static int Tag(EngineObject? item) => item is Leaf leaf ? leaf.Tag : 0;

    private static EngineObject[] Children(ObservableNode node) =>
        Enumerable.Range(0, node.Count).Select(node.GetChild).ToArray();

    private static string[] Calls(params string[] events)
    {
        var lines = new List<string>(events.Length * 2);
        foreach (string line in events)
        {
            lines.Add("changed");
            lines.Add(line);
        }

        return lines.ToArray();
    }

    private sealed class Leaf : EngineObject
    {
        public Leaf(int tag) : base(null!)
        {
            Holder = EngineFixture.Engine;
            Tag = tag;
        }

        public int Tag { get; }
    }

    private class RecordingNode : ObservableNode
    {
        public RecordingNode() : base(null!)
        {
            Holder = EngineFixture.Engine;
            CollectionChanged += (_, change) => Trace.Add(Format(change));
        }

        public List<string> Trace { get; } = [];

        protected override void OnChange() => Trace.Add("changed");

        private static string Format(ObservableChange change) =>
            change.Kind + ":" + change.Index + ":" + change.NewIndex + ":" + Label(change.Item) + ":" + Label(change.Previous);

        private static string Label(EngineObject item) => item is Leaf leaf ? leaf.Tag.ToString() : "-";
    }

    private sealed class KeepingNode : RecordingNode
    {
        public override Set<ControlStyleElement> GetControlStyle() =>
            base.GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.KeepIndex);
    }
}
