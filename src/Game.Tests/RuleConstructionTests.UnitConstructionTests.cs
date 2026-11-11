
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

public sealed partial class RuleConstructionTests
{
    public static IEnumerable<object[]> UnitClasses => new[]
    {
        "Troop", "Unit", "Hero", "Leader", "HypotheticalUnit", "WallCombatant"    }.Select(name => new object[] { name });

    private sealed class UnitOwner(bool reject) : EngineFixture.Owner
    {
        internal readonly List<string> Events = [];

        public override int AddChild(EngineObject child)
        {
            var unit = (Troop)child;
            Events.Add($"insert:{unit.FRefCount}:{unit.FUnitID}:{unit.FGroupID}");
            if (reject) return -1;
            base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    private static Troop CreateUnit(Type type, ECustomNode owner, int route)
    {
        var meta = EngineClass.Of(type);
        if (route == 1) return (Troop)meta.Create(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using GameEngine engine = new GameEngine(null!);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEClasses([EngineClass.Of(type)]);
            return (Troop)engine.CreateEObject(meta.ClassID, owner);
        }
        if (route == 3)
            return (Troop)Invoke(() => meta.GetType().GetMethod("Create", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        var constructor = type.GetConstructor([typeof(ECustomNode)])!;
        return (Troop)Invoke(() => constructor.Invoke([owner]))!;
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void UnitCreationMatchesTheBaseCommitThroughEveryFacade(string name)
    {
        Type type = TypeNamed(name);
        var previous = AoWHSMap.Current;
        AoWHSMap.Current = null!;
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
                    value.Dispose();
                    Assert.Empty(owner.Members);
                }
            }
            ConstructionSnapshot.Check("UnitConstruction", name, string.Join("\n", states));
        }
        finally { AoWHSMap.Current = previous; }
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void BinarySavedClassIdLoadsTheExactUnitWithOneReference(string name)
    {
        var previousMap = AoWHSMap.Current;
        var previousSet = AoWHSSet.Current;
        AoWHSMap.Current = null!;
        var set = EngineFixture.Configure(AoWHSSet.Create(null!), value => { value.FUnitResources = new UnitResourceList(null!); value.FHeroResources = new HeroResourceList(null!); value.FAbilityControl = new AbilityControl(null!); });
        AoWHSSet.Current = set;
        Type type = TypeNamed(name);
        var meta = EngineClass.Of(type);
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        EngineObject source = CreateUnit(type, null!, 0);
        EngineObject result = null!;
        var owner = new UnitOwner(false);
        using var memory = new MemoryStream();
        try
        {
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEClasses([EngineClass.Of(type)]);
            if (source is Unit unit) unit.FUnitResource = new UnitResource(set.FUnitResources);
            if (source is Hero hero) hero.FExperience = 1;
            if (source is HypotheticalUnit adjustable) adjustable.SetAbCount(0);
            ((Troop)source).FUnitID = 317;
            ((Troop)source).FGroupID = 29;
            var writer = new EWriteStorageStream(memory, engine, false);
            writer.rwCreateEObject(ref source, null!, 7);
            writer.Dispose();
            memory.Position = 0;
            using (var reader = new EReadStorageStream(memory, engine, false)) { reader.rwCreateEObject(ref result, owner, 7); }
            var loaded = Assert.IsAssignableFrom<Troop>(result);
            Assert.IsType(type, loaded);
            Assert.Equal(1, loaded.FRefCount);
            Assert.Equal(317, loaded.FUnitID);
            Assert.Equal(29, loaded.FGroupID);
            Assert.Same(owner, loaded.Owner);
            Assert.Same(loaded, Assert.Single(owner.Members));
            Assert.Equal(new[] { $"insert:{(loaded is Hero ? 3 : 2)}:317:29" }, owner.Events);
        }
        finally
        {
            result?.Dispose();
            source.Dispose();
            engine.Dispose();
            set.FUnitResources.Dispose();
            set.FHeroResources.Dispose();
            set.FAbilityControl.Dispose();
            AoWHSMap.Current = previousMap;
            AoWHSSet.Current = previousSet;
        }
        Assert.Empty(owner.Members);
    }

    private static UnitOwner? RefcountOwner;

    private sealed class ObservedUnit(ECustomNode owner) : Troop(owner)
    {
        public override int AddRef() { int result = base.AddRef(); RefcountOwner!.Events.Add("add:" + result); return result; }

        public override int Release() { int result = base.Release(); RefcountOwner!.Events.Add("release:" + result); return result; }
    }

    private sealed class ObservedHero(ECustomNode owner) : Hero(owner)
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
        var previous = AoWHSMap.Current;
        AoWHSMap.Current = null!;
        var owner = new UnitOwner(reject);
        RefcountOwner = owner;
        try
        {
            Troop value = hero ? new ObservedHero(owner) : new ObservedUnit(owner);
            Assert.Equal(hero ? new[] { "add:2", "add:3", "insert:3:0:0", "release:2", "release:1" }
                : new[] { "add:2", "insert:2:0:0", "release:1" }, owner.Events);
            Assert.Equal(1, value.FRefCount);
            Assert.Same(reject ? null : owner, value.Owner);
            value.Dispose();
            Assert.Empty(owner.Members);
        }
        finally { RefcountOwner = null; AoWHSMap.Current = previous; }
    }
}
