using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Combat.Actions;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed partial class SpellConstructionTests
{
    private sealed class SpellOwner : EngineFixture.Owner
    {

        internal List<EngineObject> Removed { get; } = [];
        internal List<string> Observations { get; } = [];

        public override int AddChild(EngineObject child)
        {
            Assert.True(EngineFixture.Forwards(child.Owner, this));
            Observations.Add(Snapshot(child));
            base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Removed.Add(child);
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly GamePack _previousSet = GamePack.Current;
        private readonly GameEngine _previousEngine = GameTables.GameEngine;

        internal ImageLibraryManager Manager { get; } = new ImageLibraryManager(null!);

        internal Fixture()
        {
            var set = EngineFixture.CreateGameSet(Manager);
            GamePack.Current = set;
            GameTables.GameEngine = null!;
        }

        public void Dispose()
        {
            GamePack.Current = _previousSet;
            GameTables.GameEngine = _previousEngine;
            Manager.Dispose();
        }
    }

    private static object? State(object? value, int depth = 0)
    {
        if (value is null) return null;
        Type type = value.GetType();
        if (value is string text) return text;
        if (type.IsEnum) return Convert.ToInt64(value);
        if (type.IsPrimitive || value is string || value is decimal) return value;
        if (value is ImageLibraryManager) return "$image-manager";
        if (value is Array array) return array.Cast<object?>().Select(x => State(x, depth + 1)).ToArray();
        if (InlineArrays.Elements(value) is { } elements) return elements.Select(x => State(x, depth + 1)).ToArray();
        if (depth > 8) throw new InvalidOperationException("Unexpected constructor-state cycle: " + type);
        var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
        for (Type? current = type; current is not null && current != typeof(object); current = current.BaseType)
            foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (field.Name is "_engineClass" or "_actionClass" or "_groupClass") continue; // the descriptor caches are not constructor state
                object? fieldValue = field.GetValue(value);
                fields[current.Name + "." + MemberNames.Of(field)] = field.Name == "_owner"
                    ? fieldValue is null ? null : "$owner"
                    : State(fieldValue, depth + 1);
            }
        return fields;
    }

    private static string Snapshot(EngineObject value) => JsonSerializer.Serialize(State(value));

    private static string SuccessSnapshot(EngineObject value)
    {
        var state = (SortedDictionary<string, object?>)State(value)!;
        state["EngineObject._owner"] = "$owner";
        return JsonSerializer.Serialize(state);
    }

    private static GameEngine RegisteredEngine(Type type)
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        try { engine.RegisterEngineClasses([EngineClass.Of(type)]); }
        catch { engine.Dispose(); throw; }
        return engine;
    }

    private static EngineObject Create(Type type, ECustomNode owner, int route, GameEngine? registered = null)
    {
        // A combat action takes a combat action list as its owner (issue 158). The recording owner
        // then stands behind a list that forwards to it.
        if (owner is EngineFixture.Owner recorder) owner = EngineFixture.OwnerFor(type, recorder);
        var meta = EngineClass.Of(type);
        if (route == 0)
        {
            try
            {
                if (type.GetConstructor([typeof(ECustomNode)]) is { } constructor)
                    return (EngineObject)constructor.Invoke([owner]);
                return (EngineObject)type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                    [typeof(ECustomNode)])!.Invoke(null, [owner])!;
            }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (route == 1) return meta.CreateInstance(owner);
        if (route == 3)
        {
            try { return (EngineObject)meta.GetType().GetMethod("CreateInstance", [typeof(ECustomNode)])!.Invoke(meta, [owner])!; }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (registered is not null) return registered.CreateEngineObject(meta.ClassId, owner);
        GameEngine engine = RegisteredEngine(type);
        try
        {
            return engine.CreateEngineObject(meta.ClassId, owner);
        }
        finally { engine.Dispose(); }
    }

    public static IEnumerable<object[]> Classes => ConstructionSnapshot.Keys("SpellConstruction").Select(name => new object[] { name });

    public static IEnumerable<object[]> FormerlyGuardedClasses => Classes;

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorsAndAllRegistryRoutesMatchTheLegacyState(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;
        using var fixture = new Fixture();
        var owner = new SpellOwner();
        var allocated = new List<EngineObject>();
        try
        {
            foreach (bool owned in new[] { false, true })
                for (int route = 0; route < 4; route++)
                {
                    ECustomNode argument = owned ? owner : null!;
                    EngineObject value = Create(type, argument, route);
                    allocated.Add(value);
                    Assert.IsType(type, value);
                    Assert.True(owned ? EngineFixture.Forwards(value.Owner, owner) : value.Owner is null);
                    Assert.Same(EngineClass.Of(type), value.EngineClass);
                    ConstructionSnapshot.Check("SpellConstruction", name, SuccessSnapshot(value), "State");
                    if (owned) ConstructionSnapshot.Check("SpellConstruction", name, owner.Observations[^1], "Observation");
                    if (value is Spell spell)
                    {
                        Assert.Same(fixture.Manager, spell.CastImages.ImageLibraryManager);
                        Assert.Equal(0, spell.SpellText.Count);
                        Assert.Equal(0, spell.CastSounds.Count);
                    }
                    if (value is Ability ability)
                    {
                        Assert.Same(fixture.Manager, ability.ImageSequences.ImageLibraryManager);
                        Assert.Equal(0, ability.Description.Count);
                        Assert.Equal(0, ability.SfxLibrary.Count);
                    }
                }
            Assert.Equal(4, owner.Members.Count);
        }
        finally { foreach (var value in allocated) value.Dispose(); }
        Assert.Empty(owner.Members);
        Assert.Equal(4, owner.Removed.Count);
    }

    [Theory]
    [MemberData(nameof(FormerlyGuardedClasses))]
    public void PlainSpellAndAbilityConstructorsPublishCompletedState(string name)
    {
        Type type = typeof(Spell).Assembly.GetType(name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = new SpellOwner();
            var value = Create(type, owner, route);
            Assert.IsType(type, value);
            Assert.True(EngineFixture.Forwards(value.Owner, owner));
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Empty(owner.Removed);
            if (value is Spell spell) Assert.Same(fixture.Manager, spell.CastImages.ImageLibraryManager);
            if (value is Ability ability) Assert.Same(fixture.Manager, ability.ImageSequences.ImageLibraryManager);
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Same(value, Assert.Single(owner.Removed));
        }
    }

    [Fact]
    public void InheritOnlyExclusiveActionHasItsOwnExactCreationRoutes()
    {
        Type type = typeof(ExclusiveCombatSpellCombatAction);
        var owner = new SpellOwner();
        for (int route = 0; route < 4; route++)
        {
            EngineObject value = Create(type, owner, route);
            Assert.IsType(type, value);
            Assert.Equal(1, ((CombatAction)value).RefCount);
            Assert.True(EngineFixture.Forwards(value.Owner, owner));
            value.Dispose();
        }
        Assert.Empty(owner.Members);
    }

    [Fact]
    public void NewCombatRoundKeepsItsExactLegacyType()
    {
        for (int route = 0; route < 4; route++)
        {
            var value = (NewCombatRoundCombatAction)Create(typeof(NewCombatRoundCombatAction), null!, route);
            Assert.IsType<NewCombatRoundCombatAction>(value);
            Assert.Equal(1, value.RefCount);
            value.Dispose();
        }
    }

    [Fact]
    public void SummonConstructionInitializesItsSpellState()
    {
        var raw = new SummonSpell(null!);
        Assert.NotNull(raw.SpellText);
        Assert.NotNull(raw.UnitIndices);
        Assert.Equal(0, raw.GetDispelResistance(null!));
        raw.Dispose();
    }
}
