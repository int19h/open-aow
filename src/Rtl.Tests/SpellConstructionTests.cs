using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Rtl.Tests;

public sealed partial class SpellConstructionTests
{
    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<TEObject> Removed = [];
        internal readonly List<string> Observations = [];
        public override int AddChild(TEObject child)
        {
            Assert.Same(this, child.Owner);
            Observations.Add(Snapshot(child));
            Members.Add(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Removed.Add(child);
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly TAoWHSSet previousSet = Game.AoWHSSet;
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal Fixture()
        {
            var set = new TAoWHSSet();
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(set, Manager);
            Game.AoWHSSet = set;
            Game.AoWEngine = null!;
        }
        public void Dispose()
        {
            Game.AoWHSSet = previousSet;
            Game.AoWEngine = previousEngine;
            Manager.Destroy();
        }
    }

    private static object? State(object? value, int depth = 0)
    {
        if (value is null) return null;
        Type type = value.GetType();
        if (value is AnsiString text) return text.ToString();
        if (type.IsEnum) return Convert.ToInt64(value);
        if (type.IsPrimitive || value is string || value is decimal) return value;
        if (value is TImageLibraryManager) return "$image-manager";
        if (value is Array array) return array.Cast<object?>().Select(x => State(x, depth + 1)).ToArray();
        if (depth > 8) throw new InvalidOperationException("Unexpected constructor-state cycle: " + type);
        var fields = new SortedDictionary<string, object?> { ["$type"] = type.FullName };
        for (Type? current = type; current is not null && current != typeof(TObject); current = current.BaseType)
            foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (field.Name is "eclass" or "actionClass" or "groupClass") continue; // the descriptor caches are not constructor state
                object? fieldValue = field.GetValue(value);
                fields[current.Name + "." + field.Name] = field.Name == "FOwner"
                    ? fieldValue is null ? null : "$owner"
                    : State(fieldValue, depth + 1);
            }
        return fields;
    }

    private static string Snapshot(TEObject value) => JsonSerializer.Serialize(State(value));

    private static string SuccessSnapshot(TEObject value)
    {
        var state = (SortedDictionary<string, object?>)State(value)!;
        state["TEObject.FOwner"] = "$owner";
        return JsonSerializer.Serialize(state);
    }

    private static void AssertRecorded(string expected, string state)
    {
        string actual = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(state)));
        Assert.True(expected == actual, "Constructor state differs from the base-commit recording:\n" + state);
    }

    private static TEngine RegisteredEngine(Type type)
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine();
        engine.Create__init(null!);
        try { engine.RegisterEClasses([EClass.Of(type)]); }
        catch { engine.Destroy(); throw; }
        return engine;
    }

    private static TEObject Create(Type type, TECustomNode owner, int route, TEngine? registered = null)
    {
        var meta = (TEObject.Meta)MetaRegistry.Of(type);
        if (route == 0)
        {
            try
            {
                if (type.GetConstructor([typeof(TECustomNode)]) is { } constructor)
                    return (TEObject)constructor.Invoke([owner]);
                return (TEObject)type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly,
                    [typeof(TECustomNode)])!.Invoke(null, [owner])!;
            }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (route == 1) return meta.Create(owner);
        if (route == 3)
        {
            try { return (TEObject)meta.GetType().GetMethod("Create", [typeof(TECustomNode)])!.Invoke(meta, [owner])!; }
            catch (TargetInvocationException error) when (error.InnerException is not null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
                throw;
            }
        }
        if (registered is not null) return registered.CreateEObject(meta.ClassID(), owner);
        var engine = RegisteredEngine(type);
        try
        {
            return engine.CreateEObject(meta.ClassID(), owner);
        }
        finally { engine.Destroy(); }
    }

    private static string Failure(Type type, int route)
    {
        using var fixture = new Fixture();
        TEngine? engine = route == 2 ? RegisteredEngine(type) : null;
        if (typeof(TSpell).IsAssignableFrom(type) || typeof(TAbility).IsAssignableFrom(type)) Game.AoWHSSet = null!;
        else Game.AoWEngine = new TAoWEngine();
        var owner = new Owner();
        TEObject? result = null;
        System.Exception? failure = null;
        try { result = Create(type, owner, route, engine); }
        catch (System.Exception error) { failure = error; }
        finally { engine?.Destroy(); }
        Assert.Null(result);
        Assert.NotNull(failure);
        Assert.IsType<NullReferenceException>(failure);
        Assert.Empty(owner.Members);
        Assert.True(owner.Removed.Count == 1, $"Route {route}, observations {owner.Observations.Count}: {failure}");
        Assert.Null(Assert.Single(owner.Removed).Owner);
        return JsonSerializer.Serialize(new
        {
            Error = failure.GetType().FullName,
            failure.Message,
            Members = owner.Members.Count,
            Removed = owner.Removed.Count,
            Observations = owner.Observations,
            RemovedState = owner.Removed.Select(Snapshot).ToArray()
        });
    }

    private static readonly JsonDocument Baseline = JsonDocument.Parse(typeof(SpellConstructionTests).Assembly
        .GetManifestResourceStream("Aow.Rtl.Tests.SpellConstructionBaseline.json")!);

    public static IEnumerable<object[]> Classes => Baseline.RootElement.EnumerateObject().Select(entry => new object[] { entry.Name });

    public static IEnumerable<object[]> GuardedClasses => Baseline.RootElement.EnumerateObject()
        .Where(entry => entry.Value.GetProperty("Failure").ValueKind == JsonValueKind.String)
        .Select(entry => new object[] { entry.Name });

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorsAndAllRegistryRoutesMatchTheLegacyState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        JsonElement expected = Baseline.RootElement.GetProperty(name);
        using var fixture = new Fixture();
        var owner = new Owner();
        var allocated = new List<TEObject>();
        try
        {
            foreach (bool owned in new[] { false, true })
                for (int route = 0; route < 4; route++)
                {
                    TECustomNode argument = owned ? owner : null!;
                    TEObject value = Create(type, argument, route);
                    allocated.Add(value);
                    Assert.IsType(type, value);
                    Assert.Same(argument, value.Owner);
                    Assert.Same(MetaRegistry.Of(type), value.ClassType);
                    AssertRecorded(expected.GetProperty("State").GetString()!, SuccessSnapshot(value));
                    if (owned) AssertRecorded(expected.GetProperty("Observation").GetString()!, owner.Observations[^1]);
                    if (value is TSpell spell)
                    {
                        Assert.Same(fixture.Manager, spell.FCastImages.FILManager);
                        Assert.Equal(0, spell.FSpellText.Count);
                        Assert.Equal(0, spell.FCastSounds.Count);
                    }
                    if (value is TAbility ability)
                    {
                        Assert.Same(fixture.Manager, ability.FImageSequences.FILManager);
                        Assert.Equal(0, ability.FDescription.Count);
                        Assert.Equal(0, ability.FSFXLibrary.Count);
                    }
                }
            Assert.Equal(4, owner.Members.Count);
            Assert.Throws<InvalidOperationException>(() => MetaRegistry.Of(type).NewInstance());
            bool hasRawFixture = type == typeof(Aow.Units.SummonSpells.TSummonSpell);
            if (hasRawFixture) Assert.Single(type.GetConstructors());
            else Assert.DoesNotContain(type.GetConstructors(), constructor => constructor.GetParameters().Length == 0);
            if (expected.GetProperty("Failure").ValueKind == JsonValueKind.String)
                Assert.DoesNotContain(type.GetConstructors(), constructor => constructor.GetParameters().Length != 0);
        }
        finally { foreach (var value in allocated) value.Destroy(); }
        Assert.Empty(owner.Members);
        Assert.Equal(4, owner.Removed.Count);
    }

    [Theory]
    [MemberData(nameof(GuardedClasses))]
    public void FailureAfterPublicationMatchesTheLegacyDestructorAndOwnerState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        string expected = Baseline.RootElement.GetProperty(name).GetProperty("Failure").GetString()!;
        for (int route = 0; route < 4; route++) AssertRecorded(expected, Failure(type, route));
    }

    [Fact]
    public void InheritOnlyExclusiveActionHasItsOwnExactCreationRoutes()
    {
        Type type = typeof(Aow.Units.CombatSpells.TExclusiveCombatSpellCA);
        var owner = new Owner();
        for (int route = 0; route < 4; route++)
        {
            TEObject value = Create(type, owner, route);
            Assert.IsType(type, value);
            Assert.Equal(1, ((TCombatAction)value).FRefCount);
            Assert.Same(owner, value.Owner);
            value.Destroy();
        }
        Assert.Empty(owner.Members);
    }

    [Fact]
    public void NewCombatRoundKeepsItsExactLegacyType()
    {
        for (int route = 0; route < 4; route++)
        {
            var value = (TNewCombatRoundCA)Create(typeof(TNewCombatRoundCA), null!, route);
            Assert.IsType<TNewCombatRoundCA>(value);
            Assert.Equal(1, value.FRefCount);
            value.Destroy();
        }
    }

    [Fact]
    public void MovementProbeKeepsItsNamedRawSummonAllocation()
    {
        var raw = new Aow.Units.SummonSpells.TSummonSpell();
        Assert.Null(raw.FSpellText);
        Assert.Null(raw.FUnitIndices);
        Assert.Equal(0, raw.GetDispelResistance(null!));
        raw.Destroy();
    }
}
