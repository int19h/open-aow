using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWE;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed partial class SpellConstructionTests
{
    private sealed class SpellOwner : EngineFixture.Owner
    {

        internal readonly List<TEObject> Removed = [];
        internal readonly List<string> Observations = [];
        public override int AddChild(TEObject child)
        {
            Assert.Same(this, child.Owner);
            Observations.Add(Snapshot(child));
            base.AddChild(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Removed.Add(child);
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly TAoWHSSet previousSet = Game.AoWHSSet;
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        internal readonly TImageLibraryManager Manager = new TImageLibraryManager(null!);
        internal Fixture()
        {
            var set = EngineFixture.CreateGameSet(Manager);
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


    private static TEngine RegisteredEngine(Type type)
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine(null!);
        try { engine.RegisterEClasses([EClass.Of(type)]); }
        catch { engine.Destroy(); throw; }
        return engine;
    }

    private static TEObject Create(Type type, TECustomNode owner, int route, TEngine? registered = null)
    {
        var meta = EClass.Of(type);
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
        if (registered is not null) return registered.CreateEObject(meta.ClassID, owner);
        var engine = RegisteredEngine(type);
        try
        {
            return engine.CreateEObject(meta.ClassID, owner);
        }
        finally { engine.Destroy(); }
    }



    public static IEnumerable<object[]> Classes => ConstructionSnapshot.Keys("SpellConstruction").Select(name => new object[] { name });

    public static IEnumerable<object[]> FormerlyGuardedClasses => Classes;

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorsAndAllRegistryRoutesMatchTheLegacyState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        using var fixture = new Fixture();
        var owner = new SpellOwner();
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
                    Assert.Same(Aow.Units.Engine.EClass.Of(type), value.EClass);
                    ConstructionSnapshot.Check("SpellConstruction", name, SuccessSnapshot(value), "State");
                    if (owned) ConstructionSnapshot.Check("SpellConstruction", name, owner.Observations[^1], "Observation");
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
        }
        finally { foreach (var value in allocated) value.Destroy(); }
        Assert.Empty(owner.Members);
        Assert.Equal(4, owner.Removed.Count);
    }

    [Theory]
    [MemberData(nameof(FormerlyGuardedClasses))]
    public void PlainSpellAndAbilityConstructorsPublishCompletedState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType(name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = new SpellOwner();
            var value = Create(type, owner, route);
            Assert.IsType(type, value);
            Assert.Same(owner, value.Owner);
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Empty(owner.Removed);
            if (value is TSpell spell) Assert.Same(fixture.Manager, spell.FCastImages.FILManager);
            if (value is TAbility ability) Assert.Same(fixture.Manager, ability.FImageSequences.FILManager);
            value.Destroy();
            Assert.Empty(owner.Members);
            Assert.Same(value, Assert.Single(owner.Removed));
        }
    }

    [Fact]
    public void InheritOnlyExclusiveActionHasItsOwnExactCreationRoutes()
    {
        Type type = typeof(Aow.Units.CombatSpells.TExclusiveCombatSpellCA);
        var owner = new SpellOwner();
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
    public void SummonConstructionInitializesItsSpellState()
    {
        var raw = new Aow.Units.SummonSpells.TSummonSpell(null!);
        Assert.NotNull(raw.FSpellText);
        Assert.NotNull(raw.FUnitIndices);
        Assert.Equal(0, raw.GetDispelResistance(null!));
        raw.Destroy();
    }
}
