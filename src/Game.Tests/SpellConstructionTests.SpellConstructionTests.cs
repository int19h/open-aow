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
            _ = base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Removed.Add(child);
            int index = Members.IndexOf(child);
            _ = base.RemoveChild(child);
            return index;
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly GamePack _previousSet = GamePack.Current;

        internal ImageLibraryManager Manager { get; } = new ImageLibraryManager(null);

        internal Fixture()
        {
            GamePack set = EngineFixture.CreateGameSet(Manager);
            GamePack.Current = set;
        }

        public void Dispose()
        {
            GamePack.Current = _previousSet;
            Manager.Dispose();
        }
    }

    private static object? State(object? value, int depth = 0)
    {
        if (value is null)
        {
            return null;
        }

        Type type = value.GetType();
        if (value is string text)
        {
            return text;
        }

        if (type.IsEnum)
        {
            return Convert.ToInt64(value);
        }

        if (type.IsPrimitive || value is string || value is decimal)
        {
            return value;
        }

        if (value is ImageLibraryManager)
        {
            return "$image-manager";
        }

        if (value is Array array)
        {
            return array.Cast<object?>().Select(x => State(x, depth + 1)).ToArray();
        }

        if (InlineArrays.Elements(value) is { } elements)
        {
            return elements.Select(x => State(x, depth + 1)).ToArray();
        }

        if (depth > 8)
        {
            throw new InvalidOperationException($"Unexpected constructor-state cycle: {type}");
        }

        SortedDictionary<string, object?> fields = new(StringComparer.Ordinal) { ["$type"] = type.FullName };
        for (Type? current = type; current is not null && current != typeof(object); current = current.BaseType)
        {
            foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (MemberNames.Of(field) is "_engineClass" or "EngineClass" or "_actionClass" or "ActionClass" or "_groupClass" or "GroupClass" or "Holder" or "Creator")
                {
                    continue; // the descriptor caches and the link to the engine are not constructor state
                }

                object? fieldValue = field.GetValue(value);
                fields[$"{current.Name}.{MemberNames.Of(field)}"] = MemberNames.Of(field) is "_owner" or "Owner"
                    ? fieldValue is null ? null : "$owner"
                    : State(fieldValue, depth + 1);
            }
        }

        return fields;
    }

    private static string Snapshot(EngineObject value) => JsonSerializer.Serialize(State(value));

    private static string SuccessSnapshot(EngineObject value)
    {
        SortedDictionary<string, object?> state = (SortedDictionary<string, object?>)State(value)!;
        state["EngineObject._owner"] = "$owner";
        return JsonSerializer.Serialize(state);
    }

    private static GameEngine RegisteredEngine(Type type)
    {
        GameEngine engine = new(null);
        try { engine.RegisterEngineClasses([EngineClass.Of(type)]); }
        catch { engine.Dispose(); throw; }
        return engine;
    }

    private static EngineObject Create(Type type, CustomNode owner, int route, GameEngine? registered = null)
    {
        // A combat action takes a combat action list as its owner (issue 158). The recording owner
        // then stands behind a list that forwards to it.
        if (owner is EngineFixture.Owner recorder)
        {
            owner = EngineFixture.OwnerFor(type, recorder);
        }

        EngineClass meta = EngineClass.Of(type);
        if (route == 0)
        {
            try
            {
                return EngineClassCreation.OwnerConstructor(type) is { } constructor
                    ? (EngineObject)constructor.Invoke(EngineClassCreation.OwnerArguments(constructor, owner))
                    : (EngineObject)type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                    [typeof(CustomNode)])!.Invoke(null, [owner])!;
            }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (route == 1)
        {
            return meta.CreateInstance(owner, holder: EngineFixture.Engine);
        }

        if (route == 3)
        {
            try { return (EngineObject)meta.GetType().GetMethod("CreateInstance", [typeof(CustomNode), typeof(EngineObject)])!.Invoke(meta, [owner, EngineFixture.Engine])!; }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (registered is not null)
        {
            return registered.CreateEngineObject(meta.ClassId, owner);
        }

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
        using Fixture fixture = new();
        SpellOwner owner = new();
        List<EngineObject> allocated = [];
        try
        {
            foreach (bool owned in new[] { false, true })
            {
                for (int route = 0; route < 4; route++)
                {
                    CustomNode argument = owned ? owner : null!;
                    EngineObject value = Create(type, argument, route);
                    allocated.Add(value);
                    Assert.IsType(type, value);
                    Assert.True(owned ? EngineFixture.Forwards(value.Owner, owner) : value.Owner is null);
                    Assert.Same(EngineClass.Of(type), value.EngineClass);
                    ConstructionSnapshot.Check("SpellConstruction", name, SuccessSnapshot(value), "State");
                    if (owned)
                    {
                        ConstructionSnapshot.Check("SpellConstruction", name, owner.Observations[^1], "Observation");
                    }

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
            }

            Assert.Equal(4, owner.Members.Count);
        }
        finally
        {
            foreach (EngineObject value in allocated)
            {
                value.Dispose();
            }
        }
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
            using Fixture fixture = new();
            SpellOwner owner = new();
            EngineObject value = Create(type, owner, route);
            Assert.IsType(type, value);
            Assert.True(EngineFixture.Forwards(value.Owner, owner));
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Empty(owner.Removed);
            if (value is Spell spell)
            {
                Assert.Same(fixture.Manager, spell.CastImages.ImageLibraryManager);
            }

            if (value is Ability ability)
            {
                Assert.Same(fixture.Manager, ability.ImageSequences.ImageLibraryManager);
            }

            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Same(value, Assert.Single(owner.Removed));
        }
    }

    [Fact]
    public void InheritOnlyExclusiveActionHasItsOwnExactCreationRoutes()
    {
        Type type = typeof(ExclusiveCombatSpellCombatAction);
        SpellOwner owner = new();
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
            NewCombatRoundCombatAction value = (NewCombatRoundCombatAction)Create(typeof(NewCombatRoundCombatAction), null!, route);
            _ = Assert.IsType<NewCombatRoundCombatAction>(value);
            Assert.Equal(1, value.RefCount);
            value.Dispose();
        }
    }

    [Fact]
    public void SummonConstructionInitializesItsSpellState()
    {
        SummonSpell raw = new(null) { Holder = EngineFixture.Engine };
        Assert.NotNull(raw.SpellText);
        Assert.NotNull(raw.UnitIndices);
        Assert.Equal(0, raw.GetDispelResistance(null));
        raw.Dispose();
    }
}
