#nullable disable warnings

using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
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
            Troop troop = (Troop)child;
            Events.Add($"insert:{troop.RefCount}:{troop.UnitId}:{troop.GroupId}");
            if (reject)
            {
                return -1;
            }

            Members.Add(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => Members.Remove(child) ? 0 : -1;
    }

    private static Troop CreateUnit(Type type, CustomNode owner, int route)
    {
        EngineClass meta = EngineClass.Of(type);
        if (route == 1)
        {
            return (Troop)meta.CreateInstance(owner, holder: EngineFixture.Engine);
        }

        if (route == 2)
        {
            using GameEngine engine = new(null);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            return (Troop)engine.CreateEngineObject(meta.ClassId, owner);
        }
        if (route == 3)
        {
            return (Troop)Invoke(() => meta.GetType().GetMethod("CreateInstance", [typeof(CustomNode), typeof(EngineObject)]).Invoke(meta, [owner, EngineFixture.Engine]));
        }

        ConstructorInfo constructor = EngineClassCreation.OwnerConstructor(type);
        return (Troop)Invoke(() => constructor.Invoke(EngineClassCreation.OwnerArguments(constructor, owner)));
    }

    [Theory]
    [MemberData(nameof(UnitClasses))]
    public void UnitCreationMatchesTheBaseCommitThroughEveryFacade(string name)
    {
        Type type = TypeNamed(name);
        StrategicMap previous = StrategicMap.Current;
        StrategicMap.Current = null;
        try
        {
            List<string> states = [];
            for (int ownerMode = 0; ownerMode < 3; ownerMode++)
            {
                string? expected = null;
                for (int route = 0; route < 4; route++)
                {
                    UnitOwner owner = new(ownerMode == 2);
                    Troop value = CreateUnit(type, ownerMode == 0 ? null : owner, route);
                    Assert.IsType(type, value);
                    Assert.Equal(1, value.RefCount);
                    Assert.Equal(-1, value.UnitId);
                    Assert.Equal(0, value.GroupId);
                    Assert.Same(ownerMode == 1 ? owner : null, value.Owner);
                    string state = Snapshot(value) + "\n" + string.Join("\n", owner.Events);
                    expected ??= state;
                    Assert.Equal(expected, state);
                    if (route == 0)
                    {
                        states.Add(state);
                    }

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
        StrategicMap previousMap = StrategicMap.Current;
        GamePack previousSet = GamePack.Current;
        StrategicMap.Current = null;
        GamePack set = EngineFixture.Configure(GamePack.Create(null, holder: EngineFixture.Engine), value => { value.UnitResources = new UnitResourceList(null) { Holder = EngineFixture.Engine }; value.HeroResources = new HeroResourceList(null) { Holder = EngineFixture.Engine }; value.AbilityControl = new AbilityControl(null) { Holder = EngineFixture.Engine }; });
        GamePack.Current = set;
        Type type = TypeNamed(name);
        EngineClass meta = EngineClass.Of(type);
        GameEngine engine = new(null);
        EngineObject source = CreateUnit(type, null, 0);
        EngineObject result = null;
        using MemoryStream memory = new();
        try
        {
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            if (source is Unit unit)
            {
                unit.UnitResource = new UnitResource(set.UnitResources);
            }

            if (source is Hero hero)
            {
                hero.Experience = 1;
            }

            if (source is HypotheticalUnit adjustable)
            {
                adjustable.SetAbilityCount(0);
            }

            ((Troop)source).UnitId = 317;
            ((Troop)source).GroupId = 29;
            engine.StoreEngineObject(memory, objectValue: source, progress: null);
            memory.Position = 0;
            result = engine.RetrieveEngineObject(memory, progress: null);
            Troop loaded = Assert.IsAssignableFrom<Troop>(result);
            Assert.IsType(type, loaded);
            Assert.Equal(1, loaded.RefCount);
            Assert.Equal(317, loaded.UnitId);
            Assert.Equal(29, loaded.GroupId);
            Assert.Null(loaded.Owner);
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
    }

    private static UnitOwner? s_refcountOwner;

    private sealed class ObservedUnit(CustomNode owner) : Troop(owner)
    {
        public override int AddRef() { int result = base.AddRef(); s_refcountOwner.Events.Add("add:" + result); return result; }

        public override int Release() { int result = base.Release(); s_refcountOwner.Events.Add("release:" + result); return result; }
    }

    private sealed class ObservedHero(CustomNode owner) : Hero(owner)
    {
        public override int AddRef() { int result = base.AddRef(); s_refcountOwner.Events.Add("add:" + result); return result; }

        public override int Release() { int result = base.Release(); s_refcountOwner.Events.Add("release:" + result); return result; }
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void UnitReferenceCountsKeepThePreBaseSequence(bool hero, bool reject)
    {
        StrategicMap previous = StrategicMap.Current;
        StrategicMap.Current = null;
        UnitOwner owner = new(reject);
        s_refcountOwner = owner;
        try
        {
            Troop value = hero ? new ObservedHero(owner) : new ObservedUnit(owner);
            Assert.Equal(hero ? new[] { "add:2", "add:3", "insert:3:0:0", "release:2", "release:1" }
                : ["add:2", "insert:2:0:0", "release:1"], owner.Events);
            Assert.Equal(1, value.RefCount);
            Assert.Same(reject ? null : owner, value.Owner);
            value.Dispose();
            Assert.Empty(owner.Members);
        }
        finally { s_refcountOwner = null; StrategicMap.Current = previous; }
    }
}
