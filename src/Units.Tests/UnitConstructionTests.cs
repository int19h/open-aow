using System.Reflection;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Units.Tests;

public sealed partial class AoWERuleConstructionTests
{
    public static IEnumerable<object[]> UnitClasses => new[]
    {
        "TAbstractUnit", "TUnit", "THero", "TLeader", "TAdjustableUnit", "TWallUnit", "THeroCastingViewP4"
    }.Select(name => new object[] { name });

    private sealed class UnitOwner(bool reject) : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];
        public override int AddChild(TEObject child)
        {
            var unit = (TAbstractUnit)child;
            Events.Add($"insert:{unit.FRefCount}:{unit.FUnitID}:{unit.FGroupID}");
            if (reject) return -1;
            base.AddChild(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    private static TAbstractUnit CreateUnit(Type type, TECustomNode owner, int route)
    {
        var meta = EClass.Of(type);
        if (route == 1) return (TAbstractUnit)meta.Create(owner);
        if (route == 2)
        {
            if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
            var engine = new TEngine(null!);
            try
            {
                engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
                try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
                catch (Aow.Rtl.Exception) { }
                engine.RegisterEClasses([EClass.Of(type)]);
                return (TAbstractUnit)engine.CreateEObject(meta.ClassID, owner);
            }
            finally { engine.Destroy(); }
        }
        if (route == 3)
            return (TAbstractUnit)Invoke(() => meta.GetType().GetMethod("Create", [typeof(TECustomNode)])!.Invoke(meta, [owner]))!;
        var constructor = type.GetConstructor([typeof(TECustomNode)])!;
        return (TAbstractUnit)Invoke(() => constructor.Invoke([owner]))!;
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void UnitCreationMatchesTheBaseCommitThroughEveryFacade(string name)
    {
        Type type = typeof(TAbstractUnit).Assembly.GetType("Aow.Units.AoWE." + name)!;
        var previous = Game.AoWHSMap;
        Game.AoWHSMap = null!;
        try
        {
            var states = new List<string>();
            for (int ownerMode = 0; ownerMode < 3; ownerMode++)
            {
                string? expected = null;
                for (int route = 0; route < 4; route++)
                {
                    var owner = new UnitOwner(ownerMode == 2);
                    var value = CreateUnit(type, ownerMode == 0 ? null! : owner, route);
                    Assert.IsType(type, value);
                    Assert.Equal(1, value.FRefCount);
                    Assert.Equal(-1, value.FUnitID);
                    Assert.Equal(0, value.FGroupID);
                    Assert.Same(ownerMode == 1 ? owner : null, value.Owner);
                    string state = Snapshot(value) + "\n" + string.Join("\n", owner.Events);
                    expected ??= state;
                    Assert.Equal(expected, state);
                    if (route == 0) states.Add(state);
                    value.Destroy();
                    Assert.Empty(owner.Members);
                }
            }
            ConstructionSnapshot.Check("UnitConstruction", name, string.Join("\n", states));
        }
        finally { Game.AoWHSMap = previous; }
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void BinarySavedClassIdLoadsTheExactUnitWithOneReference(string name)
    {
        var previousMap = Game.AoWHSMap;
        var previousSet = Game.AoWHSSet;
        Game.AoWHSMap = null!;
        var set = EngineFixture.Configure(TAoWHSSet.Create(null!), value => { value.FUnitResources = new TUnitResourceList(null!); value.FHeroResources = new THeroResourceList(null!); value.FAbilityControl = new TAbilityControl(null!); });
        Game.AoWHSSet = set;
        Type type = typeof(TAbstractUnit).Assembly.GetType("Aow.Units.AoWE." + name)!;
        var meta = EClass.Of(type);
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine(null!);
        TEObject source = CreateUnit(type, null!, 0);
        TEObject result = null!;
        var owner = new UnitOwner(false);
        using var memory = new TMemoryStream();
        try
        {
            engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (Aow.Rtl.Exception) { }
            engine.RegisterEClasses([EClass.Of(type)]);
            if (source is TUnit unit) unit.FUnitResource = new TUnitResource(set.FUnitResources);
            if (source is THero hero) hero.FExperience = 1;
            if (source is TAdjustableUnit adjustable) adjustable.SetAbCount(0);
            ((TAbstractUnit)source).FUnitID = 317;
            ((TAbstractUnit)source).FGroupID = 29;
            var writer = new TEWriteStorageStream(memory, engine, false);
            writer.rwCreateEObject(ref source, null!, 7);
            writer.Destroy();
            memory.Position = 0;
            var reader = new TEReadStorageStream(memory, engine, false);
            try { reader.rwCreateEObject(ref result, owner, 7); }
            finally { reader.Destroy(); }
            var loaded = Assert.IsAssignableFrom<TAbstractUnit>(result);
            Assert.IsType(type, loaded);
            Assert.Equal(1, loaded.FRefCount);
            Assert.Equal(317, loaded.FUnitID);
            Assert.Equal(29, loaded.FGroupID);
            Assert.Same(owner, loaded.Owner);
            Assert.Same(loaded, Assert.Single(owner.Members));
            Assert.Equal(new[] { $"insert:{(loaded is THero ? 3 : 2)}:317:29" }, owner.Events);
        }
        finally
        {
            result?.Destroy();
            source.Destroy();
            engine.Destroy();
            set.FUnitResources.Destroy();
            set.FHeroResources.Destroy();
            set.FAbilityControl.Destroy();
            Game.AoWHSMap = previousMap;
            Game.AoWHSSet = previousSet;
        }
        Assert.Empty(owner.Members);
    }

    private static UnitOwner? RefcountOwner;
    private sealed class ObservedUnit(TECustomNode owner) : TAbstractUnit(owner)
    {
        public override int AddRef() { int result = base.AddRef(); RefcountOwner!.Events.Add("add:" + result); return result; }
        public override int Release() { int result = base.Release(); RefcountOwner!.Events.Add("release:" + result); return result; }
    }
    private sealed class ObservedHero(TECustomNode owner) : THero(owner)
    {
        public override int AddRef() { int result = base.AddRef(); RefcountOwner!.Events.Add("add:" + result); return result; }
        public override int Release() { int result = base.Release(); RefcountOwner!.Events.Add("release:" + result); return result; }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnitReferenceCountsKeepThePreBaseSequence(bool hero, bool reject)
    {
        var previous = Game.AoWHSMap;
        Game.AoWHSMap = null!;
        var owner = new UnitOwner(reject);
        RefcountOwner = owner;
        try
        {
            TAbstractUnit value = hero ? new ObservedHero(owner) : new ObservedUnit(owner);
            Assert.Equal(hero ? new[] { "add:2", "add:3", "insert:3:0:0", "release:2", "release:1" }
                : new[] { "add:2", "insert:2:0:0", "release:1" }, owner.Events);
            Assert.Equal(1, value.FRefCount);
            Assert.Same(reject ? null : owner, value.Owner);
            value.Destroy();
            Assert.Empty(owner.Members);
        }
        finally { RefcountOwner = null; Game.AoWHSMap = previous; }
    }
}
