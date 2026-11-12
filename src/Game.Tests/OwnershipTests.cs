using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Issue 158: the ownership interfaces, the typed owner and child checks at the generic setters,
// and EngineObjectList.ClearList.
public sealed class OwnershipTests
{
    private sealed class Disposable() : EngineObject(null!)
    {
        internal int Disposals { get; private set; }

        public override void Dispose() => Disposals++;
    }

    [Fact]
    public void ChildrenAndOwnerWalkTheTreeThroughTheInterfaces()
    {
        var root = new ENode(null!);
        var first = new ENode(root);
        var second = new ENode(first);
        var shared = new ENode(null!);
        root.AddChild(shared);
        try
        {
            IOwner owner = root;
            Assert.Equal(new object[] { first, shared }, owner.Children.Cast<object>());
            Assert.Same(root, ((IOwned)first).Owner);
            Assert.Null(((IOwned)shared).Owner);
            Assert.Equal(new IOwned[] { first }, owner.Children.OfType<IOwned>().Where(child => child.Owner == owner));
            Assert.Equal("ENodeENodeENode", second.EngineObjectPath);
            var owners = new List<IOwner>();
            for (var current = ((IOwned)second).Owner; current != null; current = (current as IOwned)?.Owner) owners.Add(current);
            Assert.Equal(new IOwner[] { first, root }, owners);
        }
        finally
        {
            root.RemoveChild(shared);
            shared.Dispose();
            root.Dispose();
        }
    }

    [Fact]
    public void TypedChildListReadsTheChildrenInStoredOrder()
    {
        var list = new AIBudgetList(null!);
        var first = new AIBudget(list);
        var second = new AIBudget(null!);
        try
        {
            IChildSink<AIBudget> sink = list;
            Assert.Equal(1, sink.AddChild(second));
            IOwner<EngineObject> general = list;
            Assert.Equal(2, general.Count);
            Assert.Same(first, general[0]);
            Assert.Same(second, list[1]);
            Assert.Equal(new object[] { first, second }, ((IOwner)list).Children.Cast<object>());
        }
        finally
        {
            list.QuickClear();
            second.Dispose();
            list.Dispose();
        }
    }

    [Fact]
    public void TypedChildListRefusesAnotherChildAtTheGenericAdder()
    {
        var list = new AIBudgetList(null!);
        var stranger = new ENode(null!);
        try
        {
            var error = Assert.Throws<ArgumentException>(() => list.AddChild(stranger));
            Assert.Contains("AIBudgetList does not take a child of type ENode", error.Message, StringComparison.Ordinal);
            Assert.Throws<ArgumentException>(() => list.SetChild(0, stranger));
            Assert.Equal(0, list.Count);
            Assert.Throws<ArgumentException>(() => new EventLogList(null!).AddChild(stranger));
        }
        finally
        {
            stranger.Dispose();
            list.Dispose();
        }
    }

    [Fact]
    public void TypedOwnerRefusesAnotherOwnerAtEveryWrite()
    {
        var field = new EngineFixture.FieldOwner(new EngineFixture.Owner());
        var stranger = new ENode(null!);
        var sprite = new SingleHexSprite(field);
        try
        {
            IOwned<MapField> owned = sprite;
            Assert.Same(field, owned.Owner);
            Assert.Same(field, ((IOwned)sprite).Owner);
            Assert.Throws<ArgumentException>(() => new SingleHexSprite(stranger));
            Assert.Throws<ArgumentException>(() => sprite.SetOwner(stranger));
            Assert.Throws<ArgumentException>(() => sprite.SetArrayOwner(stranger, 0));
            Assert.Throws<ArgumentException>(() => sprite.SetOwnerWithoutNotify(stranger));
            Assert.Same(field, sprite.Owner);
            Assert.Throws<ArgumentException>(() => new MapField(stranger));
            var level = new MapLevel(null!);
            var mapField = new MapField(level);
            Assert.Same(level, ((IOwned<MapLevel>)mapField).Owner);
            ((IOwnedSink<MapLevel>)mapField).SetOwner(null!);
            Assert.Null(mapField.Owner);
        }
        finally
        {
            sprite.SetOwner(null!);
            stranger.Dispose();
        }
    }

    // The original tests List^[Count], one past the end. A full list read that slot out of range.
    [Fact]
    public void ClearListDisposesEachItemAndSkipsEmptySlots()
    {
        var list = new EngineObjectList();
        var items = new[] { new Disposable(), null, new Disposable(), new Disposable() };
        foreach (var item in items) list.Add(item);
        Assert.Equal(list.Capacity, list.Count);
        list.ClearList();
        Assert.Equal(0, list.Count);
        Assert.All(items.OfType<Disposable>(), item => Assert.Equal(1, item.Disposals));
    }
}
