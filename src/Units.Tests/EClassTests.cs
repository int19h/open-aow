using System.Text.Json;
using Aow.Game.Ai;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

// The class-id snapshot of brief 9.4-1: every engine class the product registers, with its saved
// class id, instance size and parent, taken on the base commit 6409c5c through the metaclass
// table. The table of EClass descriptors and every engine's registered set match it.
public sealed class EClassTests
{
    private static readonly JsonDocument Baseline = JsonDocument.Parse(typeof(EClassTests).Assembly
        .GetManifestResourceStream("Aow.Units.Tests.EClassBaseline.json")!);

    private static IEnumerable<EClass> ProductClasses => EClass.All.Where(c => c.Type.Assembly == typeof(EObject).Assembly);

    [Fact]
    public void TableMatchesTheBaseCommitSnapshot()
    {
        var expected = Baseline.RootElement.EnumerateObject().ToDictionary(e => e.Name, e => e.Value);
        var table = ProductClasses.ToDictionary(c => c.Type.FullName!);
        Assert.Equal(expected.Keys.Order(StringComparer.Ordinal), table.Keys.Order(StringComparer.Ordinal));
        foreach (var (name, entry) in expected)
        {
            var eclass = table[name];
            Assert.Equal(entry.GetProperty("id").GetInt32(), eclass.ClassID);
            Assert.Equal(entry.GetProperty("size").GetInt32(), eclass.InstanceSize);
            var parent = entry.GetProperty("parent");
            if (parent.ValueKind == JsonValueKind.Null) Assert.Null(eclass.Parent);
            else Assert.Equal(parent.GetString(), eclass.Parent!.Type.FullName);
            Assert.Same(eclass, EClass.Of(eclass.Type));
            Assert.Equal(NameTable.Stored(eclass.Type), eclass.ClassName);
            Assert.Equal(eclass.ClassName, NameTable.ResolveStored(NameTable.Delphi(eclass.Type)));
        }
    }

    [Fact]
    public void RegistrationFollowsInheritance()
    {
        var seen = new HashSet<EClass>();
        foreach (var eclass in EClass.All)
        {
            Assert.True(eclass.Parent is null || seen.Contains(eclass.Parent), eclass.ClassName);
            Assert.True(seen.Add(eclass));
            Assert.True(eclass.InheritsFrom(typeof(EObject)));
        }
        Assert.Same(EClass.Of<AoWHSMap>(), EClass.Of(typeof(AoWHSMap)));
        Assert.Equal(131089, EClass.Of<AoWHSMap>().ClassID);
        Assert.Equal(AoWHSMap.ClassID, EClass.Of<AoWHSMap>().ClassID);
        // An inherit-only class carries the ancestor's id with its own type.
        Assert.Equal(EObject.ClassID, EClass.Of<AIBudgetManager>().ClassID);
        Assert.Equal(typeof(AIBudgetManager), EClass.Of<AIBudgetManager>().Type);
    }

    [Fact]
    public void EngineRegisteredSetsMatchTheSnapshot()
    {
        if (Aow.Game.Engine.Nodes.EngineHost.Engines is null) Aow.Game.Engine.Nodes.EngineHost.Initialize();
        using var engine = new Engine(null!);
        var engines = new List<Engine> { engine };
        foreach (var other in Aow.Game.Engine.Nodes.EngineHost.Engines!.List.OfType<Engine>())
            if (!ReferenceEquals(other, engine)) engines.Add(other);
        Assert.Contains(EClass.Of<EControlStyleObject>(), ProductRegistered(engine));
        foreach (var registered in engines)
        {
            var classes = ProductRegistered(registered);
            Assert.NotEmpty(classes);
            foreach (var eclass in classes)
            {
                var entry = Baseline.RootElement.GetProperty(eclass.Type.FullName!);
                Assert.Equal(entry.GetProperty("id").GetInt32(), eclass.ClassID);
                Assert.Same(eclass, registered.GetEClass(eclass.ClassID));
                Assert.Same(eclass, EClass.Of(eclass.Type));
            }
        }
    }

    private static List<EClass> ProductRegistered(Engine engine)
    {
        var result = new List<EClass>();
        for (int i = 0; i < engine.EClasses.Count; i++)
        {
            var eclass = engine.EClasses.EClasses[i];
            if (eclass.Type.Assembly == typeof(EObject).Assembly) result.Add(eclass);
        }
        return result;
    }

    [Fact]
    public void EveryDescriptorWalksToTEObjectThroughParent()
    {
        var root = EClass.Of<EObject>();
        Assert.Null(root.Parent);
        foreach (var eclass in EClass.All)
        {
            var c = eclass;
            int steps = 0;
            while (c.Type != typeof(EObject))
            {
                Assert.NotNull(c.Parent);
                Assert.Equal(c.Type.BaseType, c.Parent!.Type);
                c = c.Parent;
                Assert.True(++steps < 64, eclass.ClassName);
            }
            Assert.Same(root, c);
        }
    }

    [Fact]
    public void InstancesReachTheirDescriptorThroughTheCachedProperty()
    {
        var created = EClass.Of<EResourceEditSettings>().Create(null!);
        try
        {
            Assert.Same(EClass.Of<EResourceEditSettings>(), created.EClass);
            Assert.Same(created.EClass, created.EClass);
            Assert.Equal(EResourceEditSettings.ClassID, created.EClass.ClassID);
            Assert.Equal(1026, created.EClass.ClassID);
        }
        finally { created?.Dispose(); }
    }

    [Fact]
    public void CreationThroughTheDescriptorKeepsTheExactTypeAndThrowsWhenUnregistered()
    {
        var created = EClass.Of<EControlStyleObject>().Create(null!);
        try { Assert.IsType<EControlStyleObject>(created); }
        finally { created?.Dispose(); }
        var split = EClass.Of<EResourceList>().Create(null!);
        try { Assert.IsType<EResourceList>(split); }
        finally { split?.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EClass.Of(typeof(Unregistered)));
        Assert.Throws<InvalidOperationException>(() => EClass.Register<EObject>(80, static owner => new EObject(null!), 8));
    }

    [Fact]
    public void TypedClassConstantsAreReadyBeforeAnyEngine()
    {
        // The static class references of inc/Combat.pas evaluate during type initialization, after
        // the module initializer filled the registry.
        Assert.Equal(typeof(CommandAbilityData), Aow.Game.Magic.Abilities.CommandAbility.P4CommandDataClass.Type);
        Assert.Equal(EClass.Of<CommandAbilityData>().ClassID, Aow.Game.Magic.Abilities.CommandAbility.P4CommandDataClass.ClassID);
    }

    private sealed class Unregistered : EObject
    {
        public Unregistered() : base(null!) { }

    }

    private sealed class Registered(ECustomNode owner) : EObject(owner);

    [Fact]
    public void GenericRegistrationUsesTheTypedFactoryAndPreservesTheEarlierSnapshot()
    {
        var previous = EClass.All;
        var descriptor = EClass.Register<Registered>(EObject.ClassID, static owner => new Registered(owner), 8);
        Assert.Same(descriptor, EClass.Of<Registered>());
        Assert.Same(descriptor, EClass.Of(typeof(Registered)));
        Assert.Same(EClass.Of<EObject>(), descriptor.Parent);
        Assert.Equal(previous.Count + 1, EClass.All.Count);
        Assert.Same(descriptor, EClass.All[^1]);
        Assert.DoesNotContain(descriptor, previous);
        var instance = descriptor.Create(null!);
        try { Assert.IsType<Registered>(instance); }
        finally { instance.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EClass.Register<Registered>(0, static owner => new Registered(owner), 8));
    }
}
