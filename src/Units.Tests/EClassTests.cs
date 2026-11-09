using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

// The class-id snapshot of brief 9.4-1: every engine class the product registers, with its saved
// class id, instance size and parent, taken on the base commit 6409c5c through the metaclass
// table. The table of EClass descriptors and every engine's registered set match it.
public sealed class EClassTests
{
    private static readonly JsonDocument Baseline = JsonDocument.Parse(typeof(EClassTests).Assembly
        .GetManifestResourceStream("Aow.Units.Tests.EClassBaseline.json")!);

    private static IEnumerable<EClass> ProductClasses => EClass.All.Where(c => c.Type.Assembly == typeof(TEObject).Assembly);

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
            Assert.Equal(ClassNames.ClassNameOf(eclass.Type), eclass.ClassName);
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
            Assert.True(eclass.InheritsFrom(typeof(TEObject)));
        }
        Assert.Same(EClass.Of<TAoWHSMap>(), EClass.Of(typeof(TAoWHSMap)));
        Assert.Equal(131089, EClass.Of<TAoWHSMap>().ClassID);
        Assert.Equal(TAoWHSMap.ClassID, EClass.Of<TAoWHSMap>().ClassID);
        // An inherit-only class carries the ancestor's id with its own type.
        Assert.Equal(TEObject.ClassID, EClass.Of<TAIBudgetManager>().ClassID);
        Assert.Equal(typeof(TAIBudgetManager), EClass.Of<TAIBudgetManager>().Type);
    }

    [Fact]
    public void EngineRegisteredSetsMatchTheSnapshot()
    {
        if (EngineUnit.Engines is null) EngineUnit.Initialize();
        using var engine = new TEngine(null!);
        var engines = new List<TEngine> { engine };
        foreach (var other in EngineUnit.Engines!.List.OfType<TEngine>())
            if (!ReferenceEquals(other, engine)) engines.Add(other);
        Assert.Contains(EClass.Of<TEControlStyleObject>(), ProductRegistered(engine));
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

    private static List<EClass> ProductRegistered(TEngine engine)
    {
        var result = new List<EClass>();
        for (int i = 0; i < engine.EClasses.Count; i++)
        {
            var eclass = engine.EClasses.EClasses[i];
            if (eclass.Type.Assembly == typeof(TEObject).Assembly) result.Add(eclass);
        }
        return result;
    }

    [Fact]
    public void EveryDescriptorWalksToTEObjectThroughParent()
    {
        var root = EClass.Of<TEObject>();
        Assert.Null(root.Parent);
        foreach (var eclass in EClass.All)
        {
            var c = eclass;
            int steps = 0;
            while (c.Type != typeof(TEObject))
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
        var created = EClass.Of<TEResourceEditSettings>().Create(null!);
        try
        {
            Assert.Same(EClass.Of<TEResourceEditSettings>(), created.EClass);
            Assert.Same(created.EClass, created.EClass);
            Assert.Equal(TEResourceEditSettings.ClassID, created.EClass.ClassID);
            Assert.Equal(1026, created.EClass.ClassID);
        }
        finally { created?.Dispose(); }
    }

    [Fact]
    public void CreationThroughTheDescriptorKeepsTheExactTypeAndThrowsWhenUnregistered()
    {
        var created = EClass.Of<TEControlStyleObject>().Create(null!);
        try { Assert.IsType<TEControlStyleObject>(created); }
        finally { created?.Dispose(); }
        var split = EClass.Of<TEResourceList>().Create(null!);
        try { Assert.IsType<TEResourceList>(split); }
        finally { split?.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EClass.Of(typeof(Unregistered)));
        Assert.Throws<InvalidOperationException>(() => EClass.Register<TEObject>(80, static owner => new TEObject(null!), 8));
    }

    [Fact]
    public void TypedClassConstantsAreReadyBeforeAnyEngine()
    {
        // The static class references of inc/Combat.pas evaluate during type initialization, after
        // the module initializer filled the registry.
        Assert.Equal(typeof(TCommandAbilityData), Game.P4CommandDataClass.Type);
        Assert.Equal(EClass.Of<TCommandAbilityData>().ClassID, Game.P4CommandDataClass.ClassID);
    }

    private sealed class Unregistered : TEObject
    {
        public Unregistered() : base(null!) { }

    }

    private sealed class Registered(TECustomNode owner) : TEObject(owner);

    [Fact]
    public void GenericRegistrationUsesTheTypedFactoryAndPreservesTheEarlierSnapshot()
    {
        var previous = EClass.All;
        var descriptor = EClass.Register<Registered>(TEObject.ClassID, static owner => new Registered(owner), 8);
        Assert.Same(descriptor, EClass.Of<Registered>());
        Assert.Same(descriptor, EClass.Of(typeof(Registered)));
        Assert.Same(EClass.Of<TEObject>(), descriptor.Parent);
        Assert.Equal(previous.Count + 1, EClass.All.Count);
        Assert.Same(descriptor, EClass.All[^1]);
        Assert.DoesNotContain(descriptor, previous);
        var instance = descriptor.Create(null!);
        try { Assert.IsType<Registered>(instance); }
        finally { instance.Dispose(); }
        Assert.Throws<InvalidOperationException>(() => EClass.Register<Registered>(0, static owner => new Registered(owner), 8));
    }
}
