
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

    // A troop takes a unit list or a hero list as its owner (issue 158). This unit list records
    // each membership change and keeps its members apart from its child slots.
    private sealed class UnitOwner(bool reject) : UnitList(null!)
    {
        internal List<string> Events { get; } = [];

        internal List<EngineObject> Members { get; } = [];

        public override int AddChild(EngineObject child)
        {
            var troop = (Troop)child;
            Events.Add($"insert:{troop.RefCount}:{troop.UnitId}:{troop.GroupId}");
            if (reject) return -1;
            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => Members.Remove(child) ? 0 : -1;
    }

    private static Troop CreateUnit(Type type, ECustomNode owner, int route)
    {
        var meta = EngineClass.Of(type);
        if (route == 1) return (Troop)meta.CreateInstance(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using GameEngine engine = new GameEngine(null!);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            return (Troop)engine.CreateEngineObject(meta.ClassId, owner);
        }
        if (route == 3)
            return (Troop)Invoke(() => meta.GetType().GetMethod("CreateInstance", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        var constructor = type.GetConstructor([typeof(ECustomNode)])!;
        return (Troop)Invoke(() => constructor.Invoke([owner]))!;
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void UnitCreationMatchesTheBaseCommitThroughEveryFacade(string name)
    {
        Type type = TypeNamed(name);
        var previous = StrategicMap.Current;
        StrategicMap.Current = null!;
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
                    Assert.Equal(1, value.RefCount);
                    Assert.Equal(-1, value.UnitId);
                    Assert.Equal(0, value.GroupId);
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
        finally { StrategicMap.Current = previous; }
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void BinarySavedClassIdLoadsTheExactUnitWithOneReference(string name)
    {
        var previousMap = StrategicMap.Current;
        var previousSet = GamePack.Current;
        StrategicMap.Current = null!;
        var set = EngineFixture.Configure(GamePack.Create(null!), value => { value.UnitResources = new UnitResourceList(null!); value.HeroResources = new HeroResourceList(null!); value.AbilityControl = new AbilityControl(null!); });
        GamePack.Current = set;
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
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            if (source is Unit unit) unit.UnitResource = new UnitResource(set.UnitResources);
            if (source is Hero hero) hero.Experience = 1;
            if (source is HypotheticalUnit adjustable) adjustable.SetAbilityCount(0);
            ((Troop)source).UnitId = 317;
            ((Troop)source).GroupId = 29;
            var writer = new StorageWriter(memory, engine, false);
            writer.Created(ref source, null!, 7);
            writer.Dispose();
            memory.Position = 0;
            using (var reader = new StorageReader(memory, engine, false)) { reader.Created(ref result, owner, 7); }
            var loaded = Assert.IsAssignableFrom<Troop>(result);
            Assert.IsType(type, loaded);
            Assert.Equal(1, loaded.RefCount);
            Assert.Equal(317, loaded.UnitId);
            Assert.Equal(29, loaded.GroupId);
            Assert.Same(owner, loaded.Owner);
            Assert.Same(loaded, Assert.Single(owner.Members));
            Assert.Equal(new[] { $"insert:{(loaded is Hero ? 3 : 2)}:317:29" }, owner.Events);
        }
        finally
        {
            result?.Dispose();
            source.Dispose();
            engine.Dispose();
            set.UnitResources.Dispose();
            set.HeroResources.Dispose();
            set.AbilityControl.Dispose();
            StrategicMap.Current = previousMap;
            GamePack.Current = previousSet;
        }
        Assert.Empty(owner.Members);
    }

    private static UnitOwner? s_refcountOwner;

    private sealed class ObservedUnit(ECustomNode owner) : Troop(owner)
    {
        public override int AddRef() { int result = base.AddRef(); s_refcountOwner!.Events.Add("add:" + result); return result; }

        public override int Release() { int result = base.Release(); s_refcountOwner!.Events.Add("release:" + result); return result; }
    }

    private sealed class ObservedHero(ECustomNode owner) : Hero(owner)
    {
        public override int AddRef() { int result = base.AddRef(); s_refcountOwner!.Events.Add("add:" + result); return result; }

        public override int Release() { int result = base.Release(); s_refcountOwner!.Events.Add("release:" + result); return result; }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnitReferenceCountsKeepThePreBaseSequence(bool hero, bool reject)
    {
        var previous = StrategicMap.Current;
        StrategicMap.Current = null!;
        var owner = new UnitOwner(reject);
        s_refcountOwner = owner;
        try
        {
            Troop value = hero ? new ObservedHero(owner) : new ObservedUnit(owner);
            Assert.Equal(hero ? new[] { "add:2", "add:3", "insert:3:0:0", "release:2", "release:1" }
                : new[] { "add:2", "insert:2:0:0", "release:1" }, owner.Events);
            Assert.Equal(1, value.RefCount);
            Assert.Same(reject ? null : owner, value.Owner);
            value.Dispose();
            Assert.Empty(owner.Members);
        }
        finally { s_refcountOwner = null; StrategicMap.Current = previous; }
    }
}
