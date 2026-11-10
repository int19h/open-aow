using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed partial class AoWERuleConstructionTests
{
    private sealed class RuleOwner : EngineFixture.Owner
    {

        internal readonly List<string> Observations = [];
        internal bool Reject = false;

        public override int AddChild(EObject child)
        {
            base.AddChild(child);
            Observations.Add(Snapshot(child));
            if (Reject) throw new InvalidOperationException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(EObject child) { return base.RemoveChild(child) < 0 ? -1 : 0; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly AoWHSSet previousSet = AoWHSSet.Current;
        private readonly GameEngine previousEngine = GameTables.AoWEngine;
        private readonly AoWHSMap previousMap = AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly AoWHSMap Map;

        internal Fixture()
        {
            GameTables.AoWEngine = EngineFixture.Engine;
            Map = new AoWHSMap(null!);
            var set = EngineFixture.CreateGameSet(Manager);
            AoWHSSet.Current = set;
            GameTables.AoWEngine = null!;
            Map.FStatus = Set<HSMapStatusElement>.Of(HSMapStatusElement.Destroying);
            Map.FRaces = new RaceList(null!);
            Map.FUnitControl = new UnitControl(null!);
            Map.FHeroControl = new HeroControl(null!);
            AoWHSMap.Current = Map;
        }

        public void Dispose()
        {
            AoWHSSet.Current = previousSet;
            GameTables.AoWEngine = previousEngine;
            AoWHSMap.Current = previousMap;
            Manager.Dispose();
            Map.FRaces.Dispose();
            Map.FUnitControl.Dispose();
            Map.FHeroControl.Dispose();
        }
    }

    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            if (item is GameEngine) return "$engine";
            Type type = item.GetType();
            if (item is string text) return text;
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name, StringComparer.Ordinal))
                {
                    if (field.Name is "eclass" or "actionClass" or "groupClass") continue; // the descriptor caches are not constructor state
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "priority")
                    {
                        fields[current.Name + ".<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "FOwner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static void AssertPrivateArrayChildren(object value)
    {
        if (value is AbilityControl control)
            for (int i = 0; i < 5; i++) Assert.Equal(0, control.FAbilitiesByType[i].Count);
        if (value is UnitResource resource)
            for (int i = 0; i < 3; i++)
            {
                Assert.NotNull(resource.FAbilities[i]);
                if (i > 0) Assert.NotSame(resource.FAbilities[0], resource.FAbilities[i]);
            }
        if (value is UnitGFXResource graphics)
            for (int i = 0; i < 5; i++) Assert.NotNull(graphics.FSFXLibraries[i]);
        if (value is CombatPredictor predictor)
        {
            Assert.NotNull(predictor.FSides[0]);
            Assert.NotNull(predictor.FSides[1]);
            Assert.NotSame(predictor.FSides[0], predictor.FSides[1]);
        }
        if (value is SpellCastingAbility casting)
            for (int i = 1; i <= 5; i++) Assert.Equal(20, casting.FLevelCosts[i]);
        if (value is LeadershipAbility leadership) Assert.Equal(10, leadership.FLevelCosts[1]);
    }

    private static object Create(Type type, ECustomNode owner, int route, Fixture fixture)
    {
        if (route == 1) return EClass.Of(type).Create(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using GameEngine engine = new GameEngine(null!);
            var meta = EClass.Of(type);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.eoVerifyClasses);
            try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEClasses([EClass.Of(type)]);
            return engine.CreateEObject(meta.ClassID, owner);
        }
        if (route == 3)
        {
            var meta = EClass.Of(type);
            return (object)Invoke(() => meta.GetType().GetMethod("Create", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        var constructor = typeof(EObject).IsAssignableFrom(type) ? type.GetConstructor([typeof(ECustomNode)]) : type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
        Assert.True(factory is not null || constructor is not null, type.Name + " needs its own completed creation signature.");
        ParameterInfo[] parameters = factory?.GetParameters() ?? constructor!.GetParameters();
        object?[] args = parameters.Select(p => p.ParameterType == typeof(ECustomNode) ? owner
            : p.ParameterType == typeof(int) ? (object)37
            : p.ParameterType == typeof(HSMap) ? fixture.Map
            : p.ParameterType == typeof(FastBattle) ? new FastBattle(null!)
            : p.ParameterType == typeof(CombatSide) ? new CombatSide(null!)
            : null).ToArray();
        try { return (object)Invoke(() => factory is not null ? factory.Invoke(null, args) : constructor!.Invoke(args))!; }
        finally { foreach (object? arg in args) if (arg is FastBattle or CombatSide) ((((object)arg) ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
    }

    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });
    public static IEnumerable<object[]> EngineClasses => Classes.Where(row => typeof(EObject).IsAssignableFrom(TypeNamed((string)row[0])));

    // The AoWE unit namespace is gone. The simple name is enough while it names one type.
    private static Type TypeNamed(string name)
    {
        var matches = typeof(Spell).Assembly.GetTypes().Where(type => type.Name == name).ToArray();
        if (matches.Length != 1)
            throw new InvalidOperationException(name + " matches " + matches.Length.ToString(CultureInfo.InvariantCulture) + " types.");
        return matches[0];
    }

    public static IEnumerable<object[]> FormerlyGuardedClasses => new[] { "Ability", "AdjustableAbility", "ArmyHS", "BoltsAbility", "BreathAbility", "CastingPointsReturnViewP4", "CharmAbility", "CharmedAbility", "CommandAbility", "CommandedAbility", "ConstructAbility", "CosmeticSurgeryAbility", "DispelMagicAbility", "DominateAbility", "DominatedAbility", "DurationAbility", "DurationReturnView", "EnhancementAbility", "EntangleAbility", "FlameThrowingAbility", "HealingAbility", "InvokeDeathAbility", "LeadershipAbility", "MultiLevelAbility", "PossessAbility", "PossessedAbility", "RangedAttackAbility", "RoundAttackAbility", "SeduceAbility", "SeducedAbility", "SelfDestructAbility", "Spell", "SpellCastingAbility", "StrikeAbility", "StrikeEnhancementAbility", "TouchAbility", "TransportAbility", "TurnUndeadAbility", "UnitEnchantmentAbility", "UnitSpell", "WallCrushingAbility", "WebAbility" }.Select(name => new object[] { name });

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorAndRegistryRoutesMatchTheBaseCommit(string name)
    {
        Type type = TypeNamed(name);
        using var fixture = new Fixture();
        bool engineObject = typeof(EObject).IsAssignableFrom(type);
        for (int route = 0; route < (engineObject ? 4 : 1); route++)
        {
            var owner = new RuleOwner();
            object? value = null;
            string state;
            var states = new List<string>();
            try
            {
                value = Create(type, owner, route, fixture);
                Assert.IsType(type, value);
                AssertPrivateArrayChildren(value);
                state = "success:" + Snapshot(value);
            }
            catch (Exception error) { state = "failure:" + error.GetType().FullName + ":" + error.Message; }
            finally
            {
                try { (value as IDisposable)?.Dispose(); }
                catch (Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add(state);
            ConstructionSnapshot.Check("AoWERuleConstruction", name, string.Join("\n", states));
            Assert.StartsWith("success:", state);
        }
    }

    [Theory]
    [MemberData(nameof(EngineClasses))]
    public void NullOwnerRoutesKeepTheExactTypeAndIdenticalState(string name)
    {
        Type type = TypeNamed(name);
        using var fixture = new Fixture();
        string? expected = null;
        for (int route = 0; route < 4; route++)
        {
            var value = (EObject)Create(type, null!, route, fixture);
            Assert.IsType(type, value);
            Assert.Null(value.Owner);
            Assert.Same(EClass.Of(type), value.EClass);
            string state = Snapshot(value);
            try { value.Dispose(); }
            catch (Exception error) { state += "\n" + error.GetType().FullName + ":" + error.Message; }
            expected ??= state;
            Assert.Equal(expected, state);
        }
    }

    private sealed class ObservedAISettings(ECustomNode owner) : AIMoveControlSettings(owner)
    {
        internal bool SawInitializedFields;

        public override void Clear()
        {
            SawInitializedFields = FTransportPath is not null && FRefCount == 1;
            base.Clear();
        }
    }

    private sealed class ObservedSelector : MoveSelector
    {
        internal bool SawPreAssignmentFields;

        public override MoveControlSettings CreateMCS()
        {
            SawPreAssignmentFields = FMoveControlSettings is null && FMoveSettingsList is null && FCurrentMove == 0 && FMap is null;
            return base.CreateMCS();
        }
    }

    [Fact]
    public void VirtualClearAndCreateMCSKeepTheirOriginalPositions()
    {
        using var fixture = new Fixture();
        var settings = new ObservedAISettings(null!);
        Assert.True(settings.SawInitializedFields);
        Assert.Equal(175, settings.FMovePointsMax);
        Assert.Equal(-1, settings.FDestinationX);
        settings.Dispose();
        using var selector = new ObservedSelector();
        Assert.True(selector.SawPreAssignmentFields);
        Assert.NotNull(selector.FMoveControlSettings);
        Assert.NotNull(selector.FMoveSettingsList);
        Assert.Equal(-1, selector.FCurrentMove);
    }

    [Fact]
    public void DictionaryContinuesToIgnoreItsComponentOwner()
    {
        using var owner = new Component(null!);
        using var dictionary = new AoWDictionary(owner);
        Assert.Equal(0, owner.ComponentCount);
    }

    [Fact]
    public void TacticalSelectorConstructorKeepsVirtualSettingsOrder()
    {
        using var fixture = new Fixture();
        var value = new CombatUnitSelectionControl();
        try
        {
            Assert.IsType<CombatMoveControlSettings>(value.FMoveControlSettings);
            Assert.NotNull(value.FMoveControlSettings.FPath);
            Assert.NotNull(value.FMoveControlSettings.FSubMoves);
            Assert.NotNull(value.FMoveControlSettings.FUnits);
            Assert.Equal(0, value.FMoveSettingsList.FChildren.Count);
            Assert.Equal(-1, value.FCurrentMove);
        }
        finally { value.Dispose(); }
    }

    [Theory]
    [MemberData(nameof(FormerlyGuardedClasses))]
    public void PlainConstructorsPublishCompletedState(string name)
    {
        Type type = TypeNamed(name);
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            var owner = new RuleOwner();
            var value = Assert.IsAssignableFrom<EObject>(Create(type, owner, route, fixture));
            Assert.IsType(type, value);
            Assert.Same(owner, value.Owner);
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Equal(0, owner.Removals);
            if (value is ArmyHS army) Assert.NotNull(army.FArmy);
            value.SetOwner(null!);
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
        }
    }
}
