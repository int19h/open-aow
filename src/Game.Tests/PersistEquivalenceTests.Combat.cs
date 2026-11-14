using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Tests;

// PE-3-e: the classes of Game/Combat and Game/Combat/Actions whose instances the installed corpus
// does not prove, and both sides of each branch that depends on the load.
public sealed partial class PersistEquivalenceTests
{
    // The original ReadWrite of CombatObject clears the activated flag on a load. The write stores
    // it, and both loads clear it.
    [Fact]
    public void ALoadedCombatObjectIsNotActivatedOnBothPaths()
    {
        var classValue = s_schema.Get("CombatWall");
        using var source = new CombatWall(null!) { Holder = EngineFixture.Engine, Id = 4, Position = 2 };
        source.Status = Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated, CombatObjectStatusElement.Destroyed);
        var image = Image(source);
        var tree = TreeWriter.Write(source, factory: Factory);
        source.Status = Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed);
        EqualJson(PilotImage.Decode(image, className: "CombatWall"), actual: DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: "game"), label: "CombatWall write");
        EqualBytes(image, actual: OriginalBackend.Write(tree, classValue: classValue, schema: s_schema, engine: "game"), label: "CombatWall original");
        using var viaReadWrite = new CombatWall(null!) { Holder = EngineFixture.Engine };
        using var viaPersist = new CombatWall(null!) { Holder = EngineFixture.Engine };
        Load(viaReadWrite, image);
        TreeReader.Read(OriginalBackend.Read(image, classValue: classValue, schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory);
        Assert.Equal(Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed), viaReadWrite.Status);
        Assert.Equal(viaReadWrite.Status, viaPersist.Status);
        EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "CombatWall read");
    }

    // The original ReadWrite of CombatUnit hands a loaded troop to SetUnit, which takes its own
    // reference, and releases the one the load made. Both loads end with the troop held once.
    [Fact]
    public void ACombatUnitTakesItsStoredTroopOnBothLoads()
    {
        var classValue = s_schema.Get("CombatUnit");
        var hero = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Fighter", Player = 2 };
        var source = new CombatUnit(null!) { Holder = EngineFixture.Engine, Id = 3, Position = 1, Disabled = true };
        source.SetUnit(hero);
        hero.Release();
        var image = Image(source);
        Check("CombatUnit", objectValue: source);
        using var viaReadWrite = new CombatUnit(null!) { Holder = EngineFixture.Engine };
        using var viaPersist = new CombatUnit(null!) { Holder = EngineFixture.Engine };
        Load(viaReadWrite, image);
        TreeReader.Read(OriginalBackend.Read(image, classValue: classValue, schema: s_schema, engine: "game"), objectValue: viaPersist, factory: Factory);
        var stream = Assert.IsType<Hero>(viaReadWrite.Troop);
        var persisted = Assert.IsType<Hero>(viaPersist.Troop);
        Assert.Equal("Fighter", persisted.HeroName);
        Assert.Equal(References(stream), References(persisted));
        EqualBytes(Image(viaReadWrite), actual: Image(viaPersist), label: "CombatUnit read");
    }

    // The translate stream visits this family and the engine bases through their tree methods
    // (issue 185 PE-3-i part 4), under the conditions of the original ReadWrite: a combat unit
    // visits its troop in place and clears its activated flag, because the mode holds read, combat
    // data and an action visit their headers, the resource edit settings leave out their editors
    // and the plug-in control leaves out its plug-ins.
    [Fact]
    public void TheTranslateStreamVisitsTheCombatStringsTheOriginalReadWriteVisited()
    {
        var dictionary = new Dictionary<string, string>
        {
            ["Archer"] = "Bogenschütze",
            ["Ambush"] = "Hinterhalt",
            ["Siege"] = "Belagerung",
            ["Painter"] = "Maler",
            ["Abilities"] = "Fähigkeiten",
        };
        var hero = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Archer", Player = 2 };
        using var unit = new CombatUnit(null!) { Holder = EngineFixture.Engine, Id = 3 };
        unit.SetUnit(hero);
        hero.Release();
        unit.Status = Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Activated, CombatObjectStatusElement.Destroyed);
        using (var stream = new DictionaryStream(dictionary))
        {
            stream.Object(unit, 0);
            Assert.Contains("Archer", stream.Visited);
            Assert.Equal(["CombatUnit", "Hero"], stream.Objects.Take(2));
        }
        Assert.Same(hero, unit.Troop);
        Assert.Equal("Bogenschütze", hero.HeroName);
        Assert.Equal(Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed), unit.Status);

        using var data = new CombatData(null!) { Holder = EngineFixture.Engine, Header = "Siege", Round = 4 };
        var action = new UpdateHeaderCombatAction(null!) { Holder = EngineFixture.Engine, Header = "Ambush" };
        using (var stream = new DictionaryStream(dictionary))
        {
            stream.Object(data, 0);
            stream.Object(action, 0);
            Assert.Equal(["Siege", "Ambush"], stream.Visited);
        }
        Assert.Equal(("Belagerung", 4, "Hinterhalt"), (data.Header, data.Round, action.Header));
        action.Release();

        using var settings = new ResourceEditSettings(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(1, settings.GetEditId("Painter"));
        using var plugins = new PluginControl(null!) { Holder = EngineFixture.Engine };
        plugins.UsedPlugins.Add("Abilities");
        using (var stream = new DictionaryStream(dictionary))
        {
            stream.Object(settings, 0);
            stream.Object(plugins, 0);
            Assert.Empty(stream.Visited);
        }
        Assert.Equal("Painter", settings.Editors[0]);
        Assert.Equal("Abilities", plugins.UsedPlugins[0]);
    }

    // No token event or packet stores an object of this family: a battle's logbook reaches the
    // other players as a combat event log in their logbooks, which the saves store. A combat
    // logbook can still go through the storage streams as the root of an image, as a packet's
    // object does, and the streams store it through the tree and the original backend
    // (persistence-tree.md 14). The test sends one with its data, a combat unit with its hero, a
    // player and an action, and compares what arrives with what was sent.
    [Fact]
    public void ACombatLogbookTravelsThroughTheStorageStreams()
    {
        var enlisted = EnlistCombatClasses();
        try
        {
            using var logbook = new CombatLogbook(null!) { Holder = EngineFixture.Engine, PlayerEliminated = true };
            var data = logbook.InitialData;
            data.Header = "Siege";
            data.Round = 3;
            data.X = 7;
            data.TurnOrder.Add(1);
            _ = new CombatPlayer(data.Players) { Holder = EngineFixture.Engine, Player = 2, IndependentRelation = 1 };
            var hero = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Archer", Player = 2 };
            var unit = new CombatUnit(data.CombatObjects) { Holder = EngineFixture.Engine, Id = 3, Position = 1 };
            unit.SetUnit(hero);
            hero.Release();
            var action = new UpdateHeaderCombatAction(null!) { Holder = EngineFixture.Engine, Header = "Ambush" };
            logbook.Actions.AddChild(action);
            action.Release();
            using var memory = new MemoryStream();
            EngineFixture.Engine.StoreEngineObject(memory, objectValue: logbook, progress: null);
            var sent = memory.ToArray();
            memory.Position = 0;
            var received = Assert.IsType<CombatLogbook>(EngineFixture.Engine.RetrieveEngineObject(memory, progress: null));
            try
            {
                Assert.True(received.PlayerEliminated);
                var receivedData = received.InitialData;
                Assert.Equal(("Siege", 3, 7, 1), (receivedData.Header, receivedData.Round, receivedData.X, receivedData.TurnOrder.Count));
                Assert.Equal((sbyte)2, receivedData.Players[0].Player);
                var receivedUnit = Assert.IsType<CombatUnit>(receivedData.CombatObjects.GetChild(0));
                Assert.Equal("Archer", Assert.IsType<Hero>(receivedUnit.Troop).HeroName);
                Assert.Equal("Ambush", Assert.IsType<UpdateHeaderCombatAction>(received.Actions[0]).Header);
                Assert.True(logbook.Compare(received, engine: EngineFixture.Engine));
                using var again = new MemoryStream();
                EngineFixture.Engine.StoreEngineObject(again, objectValue: received, progress: null);
                EqualBytes(sent, actual: again.ToArray(), label: "combat logbook resent");
            }
            finally
            {
                received.Release();
            }
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    private static int References(Troop troop)
    {
        troop.AddRef();
        return troop.Release();
    }

    // Every class of the family on the tree, blank: the tree write matches the converter's
    // document and the stream image, and a load of that image ends alike on both paths, with the
    // same state or the same error.
    [Theory]
    [MemberData(nameof(CombatClasses))]
    public void ACombatClassWritesAndLoadsAlikeOnBothPaths(string className)
    {
        var enlisted = EnlistCombatClasses();
        try
        {
            WriteAndLoadAlike(className);
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    public static TheoryData<string> CombatClasses()
    {
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var engineClass in EngineClass.All)
        {
            var type = engineClass.Type;
            if (type.Assembly != typeof(CombatUnit).Assembly || type.IsAbstract) continue;
            if (type.Namespace is not ("Aow.Game.Combat" or "Aow.Game.Combat.Actions")) continue;
            if (EngineObject.TreePersistenceRefusal(type) is not null) continue;
            if (FormatSchema.Current.Find(engineClass.StorageKey) is null) continue;
            names.Add(engineClass.StorageKey);
        }
        var data = new TheoryData<string>();
        foreach (var name in names) data.Add(name);
        return data;
    }

    // The game enlists the combat spell actions with the combat spells, and the fixture engine
    // holds only the classes of the engine's start. Each class id is enlisted once.
    private static EngineClass[] EnlistCombatClasses()
    {
        var table = EngineFixture.Engine.ClassTable;
        var added = new Dictionary<int, EngineClass>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(CombatUnit).Assembly) continue;
            if (engineClass.Type.Namespace is not ("Aow.Game.Combat" or "Aow.Game.Combat.Actions")) continue;
            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId)) continue;
            added[engineClass.ClassId] = engineClass;
        }
        var classes = added.Values.ToArray();
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }
}
