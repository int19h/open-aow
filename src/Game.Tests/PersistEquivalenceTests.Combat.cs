using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.Combat;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Tests;

// PE-3-e: the classes of Game/Combat and Game/Combat/Actions whose instances the installed corpus
// does not prove, and both sides of each branch that depends on the load.
public sealed partial class PersistEquivalenceTests
{
    // CombatObject.ReadWrite clears the activated flag on a load. The write stores it, and both
    // loads clear it.
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

    // CombatUnit.ReadWrite hands a loaded troop to SetUnit, which takes its own reference, and
    // releases the one the load made. Both loads end with the troop held once.
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
