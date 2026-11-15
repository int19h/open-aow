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
        OwningNode root = new(null!) { Holder = EngineFixture.Engine };
        OwningNode first = new(root);
        OwningNode second = new(first);
        OwningNode shared = new(null!) { Holder = EngineFixture.Engine };
        root.AddChild(shared);
        try
        {
            IOwner owner = root;
            Assert.Equal(new object[] { first, shared }, owner.Children.Cast<object>());
            Assert.Same(root, ((IOwned)first).Owner);
            Assert.Null(((IOwned)shared).Owner);
            Assert.Equal(new IOwned[] { first }, owner.Children.OfType<IOwned>().Where(child => child.Owner == owner));
            Assert.Equal("OwningNodeOwningNodeOwningNode", second.EngineObjectPath);
            List<IOwner> owners = new();
            for (IOwner? current = ((IOwned)second).Owner; current != null; current = (current as IOwned)?.Owner)
            {
                owners.Add(current);
            }

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
        AIBudgetList list = new(null!) { Holder = EngineFixture.Engine };
        AIBudget first = new(list);
        AIBudget second = new(null!) { Holder = EngineFixture.Engine };
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
        AIBudgetList list = new(null!) { Holder = EngineFixture.Engine };
        OwningNode stranger = new(null!) { Holder = EngineFixture.Engine };
        try
        {
            ArgumentException error = Assert.Throws<ArgumentException>(() => list.AddChild(stranger));
            Assert.Contains("AIBudgetList does not take a child of type OwningNode", error.Message, StringComparison.Ordinal);
            Assert.Throws<ArgumentException>(() => list.SetChild(0, stranger));
            Assert.Equal(0, list.Count);
            Assert.Throws<ArgumentException>(() => new EventLogList(null!) { Holder = EngineFixture.Engine }.AddChild(stranger));
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
        EngineFixture.FieldOwner field = new(new EngineFixture.Owner());
        OwningNode stranger = new(null!) { Holder = EngineFixture.Engine };
        SingleHexSprite sprite = new(field);
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
            MapLevel level = new(null!) { Holder = EngineFixture.Engine };
            MapField mapField = new(level);
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
        EngineObjectList list = new();
        Disposable?[] items = new[] { new Disposable(), null, new Disposable(), new Disposable() };
        foreach (Disposable? item in items)
        {
            list.Add(item);
        }

        Assert.Equal(list.Capacity, list.Count);
        list.ClearList();
        Assert.Equal(0, list.Count);
        Assert.All(items.OfType<Disposable>(), item => Assert.Equal(1, item.Disposals));
    }
}
