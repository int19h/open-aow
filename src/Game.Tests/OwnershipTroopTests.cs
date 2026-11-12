using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Commands;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

// Issue 158 part 3: troops and items with typed owners, the unit and item lists with typed
// children, the prisoners' own item list, the typed child slots and the members that reorder
// them, and the typed owners of the children of the reference lists.
[Collection("Engine")]
[EngineState]
public sealed class OwnershipTroopTests
{
    private static string Kind(TroopOwner owner) => owner switch
    {
        UnitList => "units",
        HeroList => "heroes",
        null => "none",
    };

    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, EngineFixture.Engine, false))
        {
            objectValue.MainReadWrite(writer);
        }

        return memory.ToArray();
    }

    private static void Load(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream(image);
        using var reader = new StorageReader(memory, EngineFixture.Engine, false);
        objectValue.MainReadWrite(reader);
    }

    [Fact]
    public void TroopTakesAUnitListOrAHeroListOnly()
    {
        var army = new Army(null!);
        var heroes = new HeroList(null!);
        var stranger = new ENode(null!);
        var troop = new Troop(null!);
        var hero = new Hero(null!);
        Assert.Throws<ArgumentException>(() => new Troop(stranger));
        Assert.Throws<ArgumentException>(() => troop.SetOwner(stranger));
        Assert.Throws<ArgumentException>(() => troop.SetArrayOwner(stranger, index: 0));
        Assert.Throws<ArgumentException>(() => troop.SetOwnerWithoutNotify(stranger));
        Assert.Null(troop.Owner);
        Assert.Equal("none", Kind(((IOwned<TroopOwner>)troop).Owner));

        troop.SetOwner(army);
        Assert.Same(army, troop.Owner);
        Assert.Same(army, troop.UnitListOwner);
        Assert.Same(army, troop.ArmyOwner);
        Assert.Equal("units", Kind(((IOwned<TroopOwner>)troop).Owner));
        Assert.Same(troop, army[0]);

        // A hero list holds references: the hero joins it ownerless, and the list owns it only
        // while it saves or loads it.
        ((IOwnedSink<TroopOwner>)hero).SetOwner(heroes);
        Assert.Null(hero.Owner);
        Assert.Same(hero, heroes[0]);
        hero.SetOwnerWithoutNotify(heroes);
        Assert.Same(heroes, hero.Owner);
        Assert.Null(hero.UnitListOwner);
        Assert.Null(hero.ArmyOwner);
        Assert.Equal("heroes", Kind(((IOwned<TroopOwner>)hero).Owner));
        Assert.Same(heroes, ((IOwned<TroopOwner>)hero).Owner.Node);

        stranger.Dispose();
    }

    [Fact]
    public void UnitListTakesTroopsOnlyAtEveryWrite()
    {
        var army = new Army(null!);
        var units = new UnitUserList(null!);
        var item = new Item(null!);
        var node = new ENode(null!);
        Assert.Throws<ArgumentException>(() => army.AddChild(item));
        Assert.Throws<ArgumentException>(() => army.AddChild(node));
        Assert.Throws<ArgumentException>(() => army.SetChild(0, child: node));
        Assert.Throws<ArgumentException>(() => units.AddChild(item));
        Assert.Equal(0, army.Count);
        Assert.Equal(0, units.Count);

        var first = new Unit(null!) { UnitId = 1 };
        var second = new Unit(null!) { UnitId = 2 };
        ((IChildSink<Troop>)army).AddChild(first);
        army.SetUnit(3, troop: second);
        Assert.Equal(4, army.Count);
        Assert.Same(first, army[0]);
        Assert.Null(army[1]);
        Assert.Null(army[2]);
        Assert.Same(second, army.GetUnit(3));
        IOwner<EngineObject> read = army;
        Assert.Same(second, read[3]);
        node.Dispose();
    }

    [Fact]
    public void ItemTakesAnItemListOnly()
    {
        var army = new Army(null!);
        var items = new ItemList(null!);
        var item = new Item(null!);
        Assert.Throws<ArgumentException>(() => new Item(army));
        Assert.Throws<ArgumentException>(() => item.SetOwner(army));
        Assert.Throws<ArgumentException>(() => item.SetOwnerWithoutNotify(army));
        Assert.Null(item.Owner);
        Assert.Equal(0, army.Count);

        ((IOwnedSink<ItemList>)item).SetOwner(items);
        Assert.Same(items, item.Owner);
        Assert.Same(items, ((IOwned<ItemList>)item).Owner);
        Assert.Same(item, items[0]);
        item.SetOwner(null);
        Assert.Null(item.Owner);
        Assert.Equal(0, items.Count);
    }

    // A dungeon combat hands the items left on the battlefield to the prisoners. They lie in the
    // prisoners' own item list, the unit list keeps troops only, and the stored prisoners are the
    // same bytes with or without those items.
    [Fact]
    public void PrisonersKeepTheirItemsOffTheUnitListAndOutOfTheStoredBytes()
    {
        var prisoners = new PrisonersArmy(null!);
        var before = Image(prisoners);
        var item = new Item(null!);
        Assert.Throws<ArgumentException>(() => item.SetOwner(prisoners));
        Assert.Equal(0, prisoners.Count);
        item.SetOwner(prisoners.Items);
        Assert.Same(prisoners.Items, item.Owner);
        Assert.Equal(0, prisoners.Count);
        Assert.Equal(1, prisoners.Items.Count);
        Assert.Equal(before, Image(prisoners));
        Assert.Null(prisoners.Items.Owner);
    }

    [Fact]
    public void ChildSlotsKeepTheirEmptySlotsAndReorderInPlace()
    {
        var list = new MoveTargetList(null!);
        var first = new MoveTarget(null!);
        var second = new MoveTarget(null!);
        var third = new MoveTarget(null!);
        list.AddChild(first);
        list.SetChild(2, child: second);
        list.AddChild(third);
        Assert.Equal(4, list.Count);
        Assert.Equal(new object[] { first, null!, second, third }, ((IOwner)list).Children.Cast<object>());
        Assert.Null(list.GetChild(1));
        Assert.Null(list.GetChildInRange(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.GetChildInRange(4));
        Assert.Equal(2, list.IndexOfChild(second));

        list.ExchangeChildren(0, second: 3);
        Assert.Equal(new object[] { third, null!, second, first }, ((IOwner)list).Children.Cast<object>());
        list.MoveChild(3, target: 1);
        Assert.Equal(new object[] { third, first, null!, second }, ((IOwner)list).Children.Cast<object>());
        list.SortChildren((left, right) => (left == null ? 0 : left == first ? 1 : left == second ? 2 : 3)
            - (right == null ? 0 : right == first ? 1 : right == second ? 2 : 3));
        Assert.Equal(new object[] { null!, first, second, third }, ((IOwner)list).Children.Cast<object>());
        Assert.Equal(0, list.DropChild(null!));
        Assert.Equal(new object[] { first, second, third }, ((IOwner)list).Children.Cast<object>());
        Assert.Equal(-1, list.DropChild(new MoveTarget(null!)));
    }

    // The direct reorderings do not notify. Sort on a change-notifying list does.
    [Fact]
    public void ReorderingTheSlotsDoesNotNotifyAndSortDoes()
    {
        var items = new ItemList(null!);
        int changes = 0;
        var first = new Item(null!) { ItemId = 1 };
        var second = new Item(null!) { ItemId = 2 };
        first.SetOwner(items);
        second.SetOwner(items);
        items.OnChanged = _ => changes++;
        static int ById(EngineObject? left, EngineObject? right) => ((Item)right!).ItemId - ((Item)left!).ItemId;
        items.SortChildren(ById);
        items.ExchangeChildren(0, second: 1);
        items.MoveChild(0, target: 1);
        Assert.Equal(0, changes);
        Assert.Same(first, items[1]);
        items.Sort(ById);
        Assert.Equal(1, changes);
        Assert.Same(second, items[0]);
    }

    [Fact]
    public void ReferenceListChildrenTakeTheirListOnly()
    {
        var stranger = new ENode(null!);
        Assert.Throws<ArgumentException>(() => new MoveTarget(stranger));
        Assert.Throws<ArgumentException>(() => new TokenEvent(stranger));
        Assert.Throws<ArgumentException>(() => new Player(stranger));
        Assert.Throws<ArgumentException>(() => new EventLog(stranger));
        var events = new TokenEventReferenceList(null!);
        Assert.Throws<ArgumentException>(() => events.AddChild(new MoveTarget(null!)));
        Assert.Equal(EngineClass.Of<EReferenceNode>().ClassId, events.EngineClass.ClassId);
        var target = new MoveTarget(null!);
        ((IOwnedSink<MoveTargetList>)target).SetOwner(new MoveTargetList(null!));
        Assert.Null(target.Owner);
        stranger.Dispose();
    }

    // A reference list owns its ownerless children while it saves them. The typed owner of a token
    // event admits the typed list, and the event loads back into a list of the same type.
    [Fact]
    public void TokenEventsSaveAndLoadThroughTheirTypedList()
    {
        var events = new TokenEventList(null!);
        var empty = Image(events);
        var tokenEvent = new CheatTokenEvent(null!);
        events.Add(tokenEvent);
        var image = Image(events);
        Assert.True(image.Length > empty.Length);
        Assert.Null(tokenEvent.Owner);
        var loaded = new TokenEventList(null!);
        Load(loaded, image: image);
        Assert.Equal(1, loaded.Count);
        Assert.IsType<CheatTokenEvent>(loaded[0]);
        Assert.Null(loaded[0].Owner);
        Assert.Equal(image, Image(loaded));
    }
}
