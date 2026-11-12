using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Issue 158 part 2: the engine's and the sprite set's children as unions, the owners of the map's
// parts, the children of the sorted lists, the resource lists and the sound lists, and the child
// writes that now go through the checked path.
[Collection("Engine")]
public sealed class OwnershipUnionTests
{
    private sealed class FixedIncome() : IncomeSource(null!)
    {
        public override int Income() => 7;
    }

    private static string Kind(GameEngineChild child) => child switch
    {
        EControlStyleNode => "control",
        HSSet => "set",
        HSMap => "map",
        null => "empty",
    };

    private static string Kind(HSSetChild child) => child switch
    {
        EResourceList => "resources",
        MapObjectResourceTerrainControl => "terrain",
        ControlStyleObject => "control",
        null => "empty",
    };

    [Fact]
    public void EngineChildrenAreItsControlNodeItsMapsAndItsSet()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        var engine = new GameEngine(null!);
        var map = new HSMap(engine);
        var set = new HSSet(null!);
        try
        {
            IChildSink<GameEngineChild> sink = engine;
            Assert.Equal(2, sink.AddChild(set));
            Assert.Equal(["control", "map", "set"], Enumerable.Range(0, engine.Count).Select(index => Kind(engine[index])));
            Assert.Same(engine.ControlNode, engine[0].Node);
            Assert.Same(map, engine[1].Node);
            Assert.Same(set, engine[2].Node);
            IOwner<GameEngineChild> owner = engine;
            Assert.Equal(new object[] { engine.ControlNode, map, set }, Enumerable.Range(0, owner.Count).Select(index => owner[index].Value));
        }
        finally
        {
            engine.RemoveChild(set);
            set.Dispose();
            map.Dispose();
            engine.Dispose();
        }
    }

    [Fact]
    public void EngineRefusesAnotherChildAtEveryWrite()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        var engine = new GameEngine(null!);
        var stranger = new ENode(null!);
        var container = new MapContainer(null!);
        try
        {
            int count = engine.Count;
            var error = Assert.Throws<ArgumentException>(() => engine.AddChild(stranger));
            Assert.Contains("GameEngine does not take a child of type ENode", error.Message, StringComparison.Ordinal);
            Assert.Throws<ArgumentException>(() => engine.AddChild(container));
            Assert.Throws<ArgumentException>(() => engine.SetChild(count, stranger));
            Assert.Equal(count, engine.Count);
        }
        finally
        {
            container.Dispose();
            stranger.Dispose();
            engine.Dispose();
        }
    }

    [Fact]
    public void SetChildrenAreResourceListsTerrainControlsAndControlObjects()
    {
        var set = new HSSet(null!);
        var units = new UnitResourceList(null!);
        var stranger = new ENode(null!);
        var tokenEvent = new TokenEvent(null!);
        try
        {
            // The set makes its hexagon and road controls, its map object resources and its edit
            // settings in its constructor, in that order.
            Assert.Equal(["terrain", "terrain", "resources", "control"], Enumerable.Range(0, set.Count).Select(index => Kind(set[index])));
            Assert.IsType<HexControl>(set[0].Node);
            Assert.IsType<RoadControl>(set[1].Node);
            Assert.IsType<MapObjectResourceList>(set[2].Node);
            Assert.IsType<ResourceEditSettings>(set[3].Node);
            IChildSink<HSSetChild> sink = set;
            Assert.Equal(4, sink.AddChild(units));
            Assert.Equal("resources", Kind(set[4]));
            Assert.Same(units, set[4].Node);
            Assert.Throws<ArgumentException>(() => set.AddChild(stranger));
            Assert.Throws<ArgumentException>(() => set.AddChild(tokenEvent));
            Assert.Equal(5, set.Count);
        }
        finally
        {
            set.RemoveChild(units);
            units.Dispose();
            tokenEvent.Dispose();
            stranger.Dispose();
            set.Dispose();
        }
    }

    [Fact]
    public void MapPartsTakeTheirOwnTypeOfOwnerOnly()
    {
        var map = new HSMap(null!);
        var container = new MapContainer(map);
        var level = new MapLevel(container);
        var stranger = new ENode(null!);
        try
        {
            Assert.Same(map, container.Owner);
            Assert.Same(map, container.Map);
            Assert.Same(container, level.Owner);
            Assert.Same(level, container[0]);
            Assert.Same(level, container.GetMapLevel(0));
            Assert.Same(map, map.Selection.Owner);
            Assert.Same(map, map.DynamicFixedVisibleHexSprite.Owner);
            IOwned<MapContainer> owned = level;
            Assert.Same(container, owned.Owner);
            Assert.Throws<ArgumentException>(() => new MapLevel(stranger));
            Assert.Throws<ArgumentException>(() => new MapContainer(stranger));
            Assert.Throws<ArgumentException>(() => new VisibilityList(stranger));
            Assert.Throws<ArgumentException>(() => new HexSpriteSelection(stranger));
            Assert.Throws<ArgumentException>(() => level.SetOwner(stranger));
            Assert.Same(container, level.Owner);
            Assert.Throws<ArgumentException>(() => container.AddChild(stranger));
            Assert.Equal(1, container.Count);
            Assert.Equal(0, stranger.Count);
        }
        finally
        {
            stranger.Dispose();
            level.Dispose();
            container.Dispose();
            map.Dispose();
        }
    }

    [Fact]
    public void SortedListAndSelectionTakeSpritesOnly()
    {
        var map = new HSMap(null!);
        var stranger = new ENode(null!);
        var first = new HexSprite(null!);
        var second = new HexSprite(null!);
        var list = map.DynamicFixedVisibleHexSprite;
        try
        {
            var error = Assert.Throws<ArgumentException>(() => list.AddChild(stranger));
            Assert.Contains("VisibilityList does not take a child of type ENode", error.Message, StringComparison.Ordinal);
            Assert.Throws<ArgumentException>(() => list.AddSortChild(stranger, 3));
            Assert.Equal(0, list.Count);
            list.AddSortChild(first, 4);
            list.AddChild(second);
            Assert.Throws<ArgumentException>(() => list.SetChild(1, stranger));
            Assert.Same(first, list[0]);
            Assert.Same(second, list[1]);
            Assert.Equal(4, list.SortValue[0]);
            Assert.Throws<ArgumentException>(() => map.Selection.AddChild(stranger));
            Assert.Equal(0, map.Selection.Count);
        }
        finally
        {
            list.QuickClear();
            second.Dispose();
            first.Dispose();
            stranger.Dispose();
            map.Dispose();
        }
    }

    [Fact]
    public void ResourceAndSoundListsTakeTheirOwnKindOnly()
    {
        var units = new UnitResourceList(null!);
        var resources = new EResourceList(null!);
        var library = new SfxLibrary(null!);
        var sequence = new SfxSequence(null!);
        var pools = new PlayerStructureList(null!);
        var face = new FaceResource(null!);
        var sound = new Sfx(null!);
        var stranger = new ENode(null!);
        try
        {
            var error = Assert.Throws<ArgumentException>(() => units.AddChild(face));
            Assert.Contains("UnitResourceList does not take a child of type FaceResource", error.Message, StringComparison.Ordinal);
            Assert.Throws<ArgumentException>(() => units.AddChild(stranger));
            Assert.Equal(0, resources.AddChild(face));
            Assert.Same(face, resources[0]);
            Assert.Throws<ArgumentException>(() => resources.AddChild(stranger));
            Assert.Throws<ArgumentException>(() => library.AddChild(sound));
            Assert.Equal(0, library.AddChild(sequence));
            Assert.Same(sequence, library[0]);
            Assert.Equal(0, sequence.AddChild(sound));
            Assert.Same(sound, sequence[0]);
            Assert.Throws<ArgumentException>(() => sequence.AddChild(stranger));
            Assert.Throws<ArgumentException>(() => pools.AddChild(stranger));
            Assert.Equal(0, units.Count);
            Assert.Equal(0, pools.Count);
        }
        finally
        {
            resources.QuickClear();
            library.QuickClear();
            sequence.QuickClear();
            stranger.Dispose();
            sound.Dispose();
            face.Dispose();
            pools.Dispose();
            sequence.Dispose();
            library.Dispose();
            resources.Dispose();
            units.Dispose();
        }
    }

    [Fact]
    public void RegisteredIncomeSourceCountsOnce()
    {
        var player = new Player(null!);
        var source = new FixedIncome();
        player.RegisterIncomeSource(source);
        player.RegisterIncomeSource(source);
        Assert.Equal(7, player.Income);
        player.UnregisterIncomeSource(source);
        Assert.Equal(0, player.Income);
        source.Dispose();
    }

    [Fact]
    public void InsertedTokenEventKeepsItsPlaceAndTakesAReference()
    {
        var events = new TokenEventList(null!);
        var first = new TokenEvent(null!);
        var second = new TokenEvent(null!);
        var inserted = new TokenEvent(null!);
        events.Add(first);
        events.Add(second);
        events.Insert(1, inserted);
        Assert.Equal(new[] { first, inserted, second }, Enumerable.Range(0, events.Count).Select(index => events[index]));
        Assert.Equal(2, inserted.RefCount);
        var budgets = new AIBudgetList(null!);
        var stranger = new ENode(null!);
        Assert.Throws<ArgumentException>(() => budgets.InsertChild(0, stranger));
        Assert.Equal(0, budgets.Count);
        stranger.Dispose();
        budgets.Dispose();
    }
}
