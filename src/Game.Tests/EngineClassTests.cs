using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// The class-id snapshot of brief 9.4-1: every engine class the product registers, with its saved
// class id, instance size and parent, taken on the base commit 6409c5c through the metaclass
// table. The table of EngineClass descriptors and every engine's registered set match it.
public sealed class EngineClassTests
{
    private static readonly JsonDocument s_baseline = JsonDocument.Parse(typeof(EngineClassTests).Assembly
        .GetManifestResourceStream("Aow.Game.Tests.EClassBaseline.json")!);

    private static IEnumerable<EngineClass> ProductClasses => EngineClass.All.Where(c => c.Type.Assembly == typeof(EngineObject).Assembly);

    [Fact]
    public void TableMatchesTheBaseCommitSnapshot()
    {
        var expected = s_baseline.RootElement.EnumerateObject().ToDictionary(e => e.Name, e => e.Value);
        var table = ProductClasses.ToDictionary(c => c.Type.FullName!);
        Assert.Equal(expected.Keys.Order(StringComparer.Ordinal), table.Keys.Order(StringComparer.Ordinal));
        foreach (var (name, entry) in expected)
        {
            var engineClass = table[name];
            Assert.Equal(entry.GetProperty("id").GetInt32(), engineClass.ClassId);
            Assert.Equal(entry.GetProperty("size").GetInt32(), engineClass.InstanceSize);
            var parent = entry.GetProperty("parent");
            if (parent.ValueKind == JsonValueKind.Null) Assert.Null(engineClass.Parent);
            else Assert.Equal(parent.GetString(), engineClass.Parent!.Type.FullName);
            Assert.Same(engineClass, EngineClass.Of(engineClass.Type));
            Assert.Equal(NameTable.Stored(engineClass.Type), engineClass.StorageKey);
            Assert.Equal(engineClass.StorageKey, NameTable.ResolveStored(NameTable.Delphi(engineClass.Type)));
        }
    }

    [Fact]
    public void RegistrationFollowsInheritance()
    {
        var seen = new HashSet<EngineClass>();
        foreach (var engineClass in EngineClass.All)
        {
            Assert.True(engineClass.Parent is null || seen.Contains(engineClass.Parent), engineClass.StorageKey);
            Assert.True(seen.Add(engineClass));
            Assert.True(engineClass.InheritsFrom(typeof(EngineObject)));
        }
        Assert.Same(EngineClass.Of<AoWHSMap>(), EngineClass.Of(typeof(AoWHSMap)));
        Assert.Equal(131089, EngineClass.Of<AoWHSMap>().ClassId);
        Assert.Equal(AoWHSMap.ClassId, EngineClass.Of<AoWHSMap>().ClassId);
        // An inherit-only class carries the ancestor's id with its own type.
        Assert.Equal(EngineObject.ClassId, EngineClass.Of<AIBudgetManager>().ClassId);
        Assert.Equal(typeof(AIBudgetManager), EngineClass.Of<AIBudgetManager>().Type);
    }

    [Fact]
    public void EngineRegisteredSetsMatchTheSnapshot()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        using GameEngine engine = new GameEngine(null!);
        var engines = new List<GameEngine> { engine };
        foreach (GameEngine other in EngineHost.Engines!)
            if (!ReferenceEquals(other, engine)) engines.Add(other);
        Assert.Contains(EngineClass.Of<ControlStyleObject>(), ProductRegistered(engine));
        foreach (GameEngine registered in engines)
        {
            var classes = ProductRegistered(registered);
            Assert.NotEmpty(classes);
            foreach (var engineClass in classes)
            {
                var entry = s_baseline.RootElement.GetProperty(engineClass.Type.FullName!);
                Assert.Equal(entry.GetProperty("id").GetInt32(), engineClass.ClassId);
                Assert.Same(engineClass, registered.GetEngineClass(engineClass.ClassId));
                Assert.Same(engineClass, EngineClass.Of(engineClass.Type));
            }
        }
    }

    private static List<EngineClass> ProductRegistered(GameEngine engine)
    {
        var result = new List<EngineClass>();
        for (int i = 0; i < engine.EngineClasses.Count; i++)
        {
            var engineClass = engine.EngineClasses[i];
            if (engineClass.Type.Assembly == typeof(EngineObject).Assembly) result.Add(engineClass);
        }
        return result;
    }

    [Fact]
    public void EveryDescriptorWalksToTokenEventObjectThroughParent()
    {
        var root = EngineClass.Of<EngineObject>();
        Assert.Null(root.Parent);
        foreach (var engineClass in EngineClass.All)
        {
            var c = engineClass;
            int steps = 0;
            while (c.Type != typeof(EngineObject))
            {
                Assert.NotNull(c.Parent);
                Assert.Equal(c.Type.BaseType, c.Parent!.Type);
                c = c.Parent;
                Assert.True(++steps < 64, engineClass.StorageKey);
            }
            Assert.Same(root, c);
        }
    }

    [Fact]
    public void InstancesReachTheirDescriptorThroughTheCachedProperty()
    {
        var created = EngineClass.Of<ResourceEditSettings>().CreateInstance(null!);
        try
        {
            Assert.Same(EngineClass.Of<ResourceEditSettings>(), created.EngineClass);
            Assert.Same(created.EngineClass, created.EngineClass);
            Assert.Equal(ResourceEditSettings.ClassId, created.EngineClass.ClassId);
            Assert.Equal(1026, created.EngineClass.ClassId);
        }
        finally { created?.Dispose(); }
    }

    [Fact]
    public void CreationThroughTheDescriptorKeepsTheExactTypeAndThrowsWhenUnregistered()
    {
        var created = EngineClass.Of<ControlStyleObject>().CreateInstance(null!);
        try { Assert.IsType<ControlStyleObject>(created); }
        finally { created?.Dispose(); }
        var split = EngineClass.Of<EResourceList>().CreateInstance(null!);
        try { Assert.IsType<EResourceList>(split); }
        finally { split?.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EngineClass.Of(typeof(Unregistered)));
        Assert.Throws<InvalidOperationException>(() => EngineClass.Register(80, static owner => new EngineObject(null!), 8));
    }

    [Fact]
    public void TypedClassConstantsAreReadyBeforeAnyEngine()
    {
        // The static class references of inc/Combat.pas evaluate during type initialization, after
        // the module initializer filled the registry.
        Assert.Equal(typeof(CommandAbilityData), CommandAbility.CommandDataClass.Type);
        Assert.Equal(EngineClass.Of<CommandAbilityData>().ClassId, CommandAbility.CommandDataClass.ClassId);
    }

    private sealed class Unregistered : EngineObject
    {
        public Unregistered() : base(null!) { }

    }

    private sealed class Registered(ECustomNode owner) : EngineObject(owner);

    [Fact]
    public void GenericRegistrationUsesTheTypedFactoryAndPreservesTheEarlierSnapshot()
    {
        var previous = EngineClass.All;
        var descriptor = EngineClass.Register(EngineObject.ClassId, static owner => new Registered(owner), 8);
        Assert.Same(descriptor, EngineClass.Of<Registered>());
        Assert.Same(descriptor, EngineClass.Of(typeof(Registered)));
        Assert.Same(EngineClass.Of<EngineObject>(), descriptor.Parent);
        Assert.Equal(previous.Count + 1, EngineClass.All.Count);
        Assert.Same(descriptor, EngineClass.All[^1]);
        Assert.DoesNotContain(descriptor, previous);
        var instance = descriptor.CreateInstance(null!);
        try { Assert.IsType<Registered>(instance); }
        finally { instance.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EngineClass.Register(0, static owner => new Registered(owner), 8));
    }
}
