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
        Army => "army",
        UnitList => "units",
        HeroList => "heroes",
        null => "none",
    };

    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, EngineFixture.Engine, false))
        {
            writer.PersistObject(objectValue);
        }

        return memory.ToArray();
    }

    private static void Load(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream(image);
        using var reader = new StorageReader(memory, EngineFixture.Engine, false);
        reader.PersistObject(objectValue);
    }

    [Fact]
    public void TroopTakesAUnitListOrAHeroListOnly()
    {
        var army = new Army(null!) { Holder = EngineFixture.Engine };
        var heroes = new HeroList(null!) { Holder = EngineFixture.Engine };
        var stranger = new ENode(null!) { Holder = EngineFixture.Engine };
        var troop = new Troop(null!) { Holder = EngineFixture.Engine };
        var hero = new Hero(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => new Troop(stranger));
        Assert.Throws<ArgumentException>(() => troop.SetOwner(stranger));
        Assert.Throws<ArgumentException>(() => troop.SetArrayOwner(stranger, index: 0));
        Assert.Throws<ArgumentException>(() => troop.SetOwnerWithoutNotify(stranger));
        Assert.Null(troop.Owner);
        Assert.Equal("none", Kind(((IOwned<TroopOwner>)troop).Owner));

        troop.SetOwner(army);
        Assert.Same(army, troop.Owner);
        Assert.Same(army, troop.OwnerUnits.Holder);
        Assert.Same(army, troop.UnitHolder);
        Assert.Same(army, troop.ArmyOwner);
        Assert.Equal("army", Kind(((IOwned<TroopOwner>)troop).Owner));
        Assert.Same(army, ((IOwned<TroopOwner>)troop).Owner.Node);
        Assert.Same(troop, army.Units[0]);

        // A unit list that is not an army holds generated troops apart from an army.
        var units = new UnitList(null!) { Holder = EngineFixture.Engine };
        troop.SetOwner(units);
        Assert.Empty(army.Units);
        Assert.Same(units, troop.UnitHolder);
        Assert.Equal("units", Kind(((IOwned<TroopOwner>)troop).Owner));
        Assert.Throws<InvalidCastException>(() => troop.ArmyOwner);
        troop.SetOwner(army);

        // A hero list holds references: the hero joins it ownerless, and the list owns it only
        // while it saves or loads it.
        ((IOwnedSink<TroopOwner>)hero).SetOwner(heroes);
        Assert.Null(hero.Owner);
        Assert.Same(hero, heroes[0]);
        hero.SetOwnerWithoutNotify(heroes);
        Assert.Same(heroes, hero.Owner);
        Assert.Null(hero.UnitHolder);
        Assert.Null(hero.ArmyOwner);
        Assert.Equal("heroes", Kind(((IOwned<TroopOwner>)hero).Owner));
        Assert.Same(heroes, ((IOwned<TroopOwner>)hero).Owner.Node);

        stranger.Dispose();
    }

    [Fact]
    public void UnitListTakesTroopsOnlyAtEveryWrite()
    {
        var army = new Army(null!) { Holder = EngineFixture.Engine };
        var units = new UnitUserList(null!) { Holder = EngineFixture.Engine };
        var item = new Item(null!) { Holder = EngineFixture.Engine };
        var node = new ENode(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => army.AddChild(item));
        Assert.Throws<ArgumentException>(() => army.AddChild(node));
        Assert.Throws<ArgumentException>(() => army.SetChild(0, child: node));
        Assert.Throws<ArgumentException>(() => units.AddChild(item));
        Assert.Empty(army.Units);
        Assert.Equal(0, units.Count);

        var first = new Unit(null!) { Holder = EngineFixture.Engine, UnitId = 1 };
        var second = new Unit(null!) { Holder = EngineFixture.Engine, UnitId = 2 };
        ((IChildSink<Troop>)army).AddChild(first);
        army.SetChild(3, child: second);
        Assert.Equal(4, army.Units.Count);
        Assert.Same(first, army.Units[0]);
        Assert.Null(army.Units[1]);
        Assert.Null(army.Units[2]);
        Assert.Same(second, army.Units.GetUnit(3));
        IOwner<EngineObject> read = army;
        Assert.Same(second, read[3]);
        node.Dispose();
    }

    // Issue 158 part 4: an army is not a unit list. It holds its troops as its children and reads
    // them through Units, and the collection adds through the holder's own hooks.
    [Fact]
    public void AnArmyHoldsItsTroopsInUnitsAndAddsThroughItsOwnHooks()
    {
        var army = new Army(null!) { Holder = EngineFixture.Engine };
        var units = new UnitList(null!) { Holder = EngineFixture.Engine };
        var first = new Troop(null!) { Holder = EngineFixture.Engine, UnitId = 1 };
        var second = new Troop(null!) { Holder = EngineFixture.Engine, UnitId = 2 };
        Assert.False(typeof(UnitList).IsAssignableFrom(typeof(Army)));
        Assert.True(army.Units.AddUnit(first));
        Assert.True(units.Units.AddUnit(second));
        Assert.Same(army, first.Owner);
        Assert.Same(units, second.Owner);
        Assert.Same(army, army.Units.Holder);
        Assert.Equal(new Troop[] { first }, army.Units);
        Assert.Same(first, army.GetChild(0));
        Assert.Same(first, army.Units.FindId(1));
        Assert.Equal(0, army.Units.IndexOfId(1));
        Assert.Equal(-1, army.Units.IndexOfId(2));
        Assert.Null(army.Units.GetUnit(1));
        IOwner<Troop> typed = army;
        Assert.Same(first, typed[0]);
        Assert.Equal(1, typed.Count);
        Assert.True(army.DeleteUnit(first));
        Assert.Null(first.Owner);
        Assert.Empty(army.Units);
    }

    [Fact]
    public void ItemTakesAnItemListOnly()
    {
        var army = new Army(null!) { Holder = EngineFixture.Engine };
        var items = new ItemList(null!) { Holder = EngineFixture.Engine };
        var item = new Item(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => new Item(army));
        Assert.Throws<ArgumentException>(() => item.SetOwner(army));
        Assert.Throws<ArgumentException>(() => item.SetOwnerWithoutNotify(army));
        Assert.Null(item.Owner);
        Assert.Empty(army.Units);

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
        var prisoners = new PrisonersArmy(null!) { Holder = EngineFixture.Engine };
        var before = Image(prisoners);
        var item = new Item(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => item.SetOwner(prisoners));
        Assert.Empty(prisoners.Units);
        item.SetOwner(prisoners.Items);
        Assert.Same(prisoners.Items, item.Owner);
        Assert.Empty(prisoners.Units);
        Assert.Equal(1, prisoners.Items.Count);
        Assert.Equal(before, Image(prisoners));
        Assert.Null(prisoners.Items.Owner);
    }

    [Fact]
    public void ChildSlotsKeepTheirEmptySlotsAndReorderInPlace()
    {
        var list = new MoveTargetList(null!) { Holder = EngineFixture.Engine };
        var first = new MoveTarget(null!) { Holder = EngineFixture.Engine };
        var second = new MoveTarget(null!) { Holder = EngineFixture.Engine };
        var third = new MoveTarget(null!) { Holder = EngineFixture.Engine };
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
        Assert.Equal(-1, list.DropChild(new MoveTarget(null!) { Holder = EngineFixture.Engine }));
    }

    // The direct reorderings do not notify. Sort on a change-notifying list does.
    [Fact]
    public void ReorderingTheSlotsDoesNotNotifyAndSortDoes()
    {
        var items = new ItemList(null!) { Holder = EngineFixture.Engine };
        int changes = 0;
        var first = new Item(null!) { Holder = EngineFixture.Engine, ItemId = 1 };
        var second = new Item(null!) { Holder = EngineFixture.Engine, ItemId = 2 };
        first.SetOwner(items);
        second.SetOwner(items);
        items.OnChanged += _ => changes++;
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
        var stranger = new ENode(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => new MoveTarget(stranger));
        Assert.Throws<ArgumentException>(() => new TokenEvent(stranger));
        Assert.Throws<ArgumentException>(() => new Player(stranger));
        Assert.Throws<ArgumentException>(() => new EventLog(stranger));
        var events = new TokenEventReferenceList(null!) { Holder = EngineFixture.Engine };
        Assert.Throws<ArgumentException>(() => events.AddChild(new MoveTarget(null!) { Holder = EngineFixture.Engine }));
        Assert.Equal(EngineClass.Of<EReferenceNode>().ClassId, events.EngineClass.ClassId);
        var target = new MoveTarget(null!) { Holder = EngineFixture.Engine };
        ((IOwnedSink<MoveTargetList>)target).SetOwner(new MoveTargetList(null!) { Holder = EngineFixture.Engine });
        Assert.Null(target.Owner);
        stranger.Dispose();
    }

    // A reference list owns its ownerless children while it saves them. The typed owner of a token
    // event admits the typed list, and the event loads back into a list of the same type.
    [Fact]
    public void TokenEventsSaveAndLoadThroughTheirTypedList()
    {
        var events = new TokenEventList(null!) { Holder = EngineFixture.Engine };
        var empty = Image(events);
        var tokenEvent = new CheatTokenEvent(null!) { Holder = EngineFixture.Engine };
        events.Add(tokenEvent);
        var image = Image(events);
        Assert.True(image.Length > empty.Length);
        Assert.Null(tokenEvent.Owner);
        var loaded = new TokenEventList(null!) { Holder = EngineFixture.Engine };
        Load(loaded, image: image);
        Assert.Equal(1, loaded.Count);
        Assert.IsType<CheatTokenEvent>(loaded[0]);
        Assert.Null(loaded[0].Owner);
        Assert.Equal(image, Image(loaded));
    }
}
