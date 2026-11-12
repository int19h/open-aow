using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed partial class RuleConstructionTests
{
    private sealed class RuleOwner : EngineFixture.Owner
    {

        private readonly List<string> _observations = [];
        private bool _reject = false;

        public override int AddChild(EngineObject child)
        {
            base.AddChild(child);
            _observations.Add(Snapshot(child));
            if (_reject) throw new InvalidOperationException("owner rejects after insertion");
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) { return base.RemoveChild(child) < 0 ? -1 : 0; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly GamePack _previousSet = GamePack.Current;
        private readonly GameEngine _previousEngine = GameTables.GameEngine;
        private readonly StrategicMap _previousMap = StrategicMap.Current;
        private readonly ImageLibraryManager _manager = new ImageLibraryManager(null!);

        internal StrategicMap Map { get; }

        internal Fixture()
        {
            GameTables.GameEngine = EngineFixture.Engine;
            Map = new StrategicMap(null!);
            var set = EngineFixture.CreateGameSet(_manager);
            GamePack.Current = set;
            GameTables.GameEngine = null!;
            Map.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
            Map.Races = new RaceList(null!);
            Map.UnitControl = new UnitControl(null!);
            Map.HeroControl = new HeroControl(null!);
            StrategicMap.Current = Map;
        }

        public void Dispose()
        {
            GamePack.Current = _previousSet;
            GameTables.GameEngine = _previousEngine;
            StrategicMap.Current = _previousMap;
            _manager.Dispose();
            Map.Races.Dispose();
            Map.UnitControl.Dispose();
            Map.HeroControl.Dispose();
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
            if (InlineArrays.Elements(item) is { } elements) return elements.Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, StringComparer.Ordinal))
                {
                    if (field.Name is "_engineClass" or "_actionClass" or "_groupClass") continue; // the descriptor caches are not constructor state
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "_priority")
                    {
                        fields[current.Name + ".<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
                    string key = current.Name + "." + MemberNames.Of(field);
                    object? child = field.GetValue(item);
                    if (field.Name == "_owner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }

    private static void AssertPrivateArrayChildren(object value)
    {
        if (value is AbilityControl control)
            for (int i = 0; i < 5; i++) Assert.Equal(0, control.AbilitiesByType[i].Count);
        if (value is UnitResource resource)
            for (int i = 0; i < 3; i++)
            {
                Assert.NotNull(resource.Abilities[i]);
                if (i > 0) Assert.NotSame(resource.Abilities[0], resource.Abilities[i]);
            }
        if (value is UnitGraphicsResource graphics)
            for (int i = 0; i < 5; i++) Assert.NotNull(graphics.SfxLibraries[i]);
        if (value is CombatPredictor predictor)
        {
            Assert.NotNull(predictor.Sides[0]);
            Assert.NotNull(predictor.Sides[1]);
            Assert.NotSame(predictor.Sides[0], predictor.Sides[1]);
        }
        if (value is SpellCastingAbility casting)
            for (int i = 1; i <= 5; i++) Assert.Equal(20, casting.LevelCosts[i]);
        if (value is LeadershipAbility leadership) Assert.Equal(10, leadership.LevelCosts[1]);
    }

    private static object Create(Type type, ECustomNode owner, int route, Fixture fixture)
    {
        if (route == 1) return EngineClass.Of(type).CreateInstance(owner);
        if (route == 2)
        {
            if (EngineHost.Engines is null) EngineHost.Initialize();
            using GameEngine engine = new GameEngine(null!);
            var meta = EngineClass.Of(type);
            engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            try { engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]); }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            return engine.CreateEngineObject(meta.ClassId, owner);
        }
        if (route == 3)
        {
            var meta = EngineClass.Of(type);
            return (object)Invoke(() => meta.GetType().GetMethod("CreateInstance", [typeof(ECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        var constructor = typeof(EngineObject).IsAssignableFrom(type) ? type.GetConstructor([typeof(ECustomNode)]) : type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
        Assert.True(factory is not null || constructor is not null, type.Name + " needs its own completed creation signature.");
        ParameterInfo[] parameters = factory?.GetParameters() ?? constructor!.GetParameters();
        object?[] arguments = parameters.Select(p => p.ParameterType == typeof(ECustomNode) ? owner
            : p.ParameterType == typeof(int) ? (object)37
            : p.ParameterType == typeof(GameMap) ? fixture.Map
            : p.ParameterType == typeof(FastBattle) ? new FastBattle(null!)
            : p.ParameterType == typeof(CombatSide) ? new CombatSide(null!)
            : null).ToArray();
        try { return (object)Invoke(() => factory is not null ? factory.Invoke(null, arguments) : constructor!.Invoke(arguments))!; }
        finally { foreach (object? argument in arguments) if (argument is FastBattle or CombatSide) ((((object)argument) ?? throw new NullReferenceException()) as IDisposable)?.Dispose(); }
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

    public static IEnumerable<object[]> Classes => s_names.Select(name => new object[] { name });
    public static IEnumerable<object[]> EngineClasses => Classes.Where(row => typeof(EngineObject).IsAssignableFrom(TypeNamed((string)row[0])));

    // The AoWE unit namespace is gone. A simple name that one top-level type carries selects that type.
    // A nested type can reuse the simple name.
    private static Type TypeNamed(string name)
    {
        var matches = typeof(Spell).Assembly.GetTypes().Where(type => type.Name == name).ToArray();
        if (matches.Length == 1) return matches[0];
        var top = matches.Where(type => type.DeclaringType is null).ToArray();
        if (top.Length == 1) return top[0];
        throw new InvalidOperationException(name + " matches " + matches.Length.ToString(CultureInfo.InvariantCulture) + " types.");
    }

    public static IEnumerable<object[]> FormerlyGuardedClasses => new[] { "Ability", "AdjustableAbility", "ArmyHexSprite", "BoltsAbility", "BreathAbility", "CharmAbility", "CharmedAbility", "CommandAbility", "CommandedAbility", "ConstructAbility", "CosmeticSurgeryAbility", "DispelMagicAbility", "DominateAbility", "DominatedAbility", "DurationAbility", "EnhancementAbility", "EntangleAbility", "FlameThrowingAbility", "HealingAbility", "InvokeDeathAbility", "LeadershipAbility", "MultiLevelAbility", "PossessAbility", "PossessedAbility", "RangedAttackAbility", "RoundAttackAbility", "SeduceAbility", "SeducedAbility", "SelfDestructAbility", "Spell", "SpellCastingAbility", "StrikeAbility", "StrikeEnhancementAbility", "TouchAbility", "TransportAbility", "TurnUndeadAbility", "UnitEnchantmentAbility", "UnitSpell", "WallCrushingAbility", "WebAbility" }.Select(name => new object[] { name });

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorAndRegistryRoutesMatchTheBaseCommit(string name)
    {
        Type type = TypeNamed(name);
        using var fixture = new Fixture();
        bool engineObject = typeof(EngineObject).IsAssignableFrom(type);
        for (int route = 0; route < (engineObject ? 4 : 1); route++)
        {
            var owner = new RuleOwner();
            object? value = null;
            string state;
            var states = new List<string>();
            try
            {
                value = Create(type, EngineFixture.OwnerFor(type, owner), route, fixture);
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
            var value = (EngineObject)Create(type, null!, route, fixture);
            Assert.IsType(type, value);
            Assert.Null(value.Owner);
            Assert.Same(EngineClass.Of(type), value.EngineClass);
            string state = Snapshot(value);
            try { value.Dispose(); }
            catch (Exception error) { state += "\n" + error.GetType().FullName + ":" + error.Message; }
            expected ??= state;
            Assert.Equal(expected, state);
        }
    }

    private sealed class ObservedAISettings(ECustomNode owner) : AIMoveControlSettings(owner)
    {
        internal bool SawInitializedFields { get; private set; }

        public override void Clear()
        {
            SawInitializedFields = TransportPath is not null && RefCount == 1;
            base.Clear();
        }
    }

    private sealed class ObservedSelector : MoveSelector
    {
        internal bool SawPreAssignmentFields { get; private set; }

        public override MoveControlSettings CreateMoveControlSettings()
        {
            SawPreAssignmentFields = MoveControlSettings is null && MoveSettingsList is null && CurrentMove == 0 && Map is null;
            return base.CreateMoveControlSettings();
        }
    }

    [Fact]
    public void VirtualClearAndCreateMoveControlSettingsKeepTheirOriginalPositions()
    {
        using var fixture = new Fixture();
        var settings = new ObservedAISettings(null!);
        Assert.True(settings.SawInitializedFields);
        Assert.Equal(175, settings.MovePointsMax);
        Assert.Equal(-1, settings.DestinationX);
        settings.Dispose();
        using var selector = new ObservedSelector();
        Assert.True(selector.SawPreAssignmentFields);
        Assert.NotNull(selector.MoveControlSettings);
        Assert.NotNull(selector.MoveSettingsList);
        Assert.Equal(-1, selector.CurrentMove);
    }

    [Fact]
    public void TacticalSelectorConstructorKeepsVirtualSettingsOrder()
    {
        using var fixture = new Fixture();
        var value = new CombatUnitSelectionControl();
        try
        {
            Assert.IsType<CombatMoveControlSettings>(value.MoveControlSettings);
            Assert.NotNull(value.MoveControlSettings.Path);
            Assert.NotNull(value.MoveControlSettings.SubMoves);
            Assert.NotNull(value.MoveControlSettings.Units);
            Assert.Equal(0, value.MoveSettingsList.Count);
            Assert.Equal(-1, value.CurrentMove);
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
            var given = EngineFixture.OwnerFor(type, owner);
            var value = Assert.IsAssignableFrom<EngineObject>(Create(type, given, route, fixture));
            Assert.IsType(type, value);
            Assert.Same(given, value.Owner);
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Equal(0, owner.Removals);
            if (value is ArmyHexSprite army) Assert.NotNull(army.Army);
            value.SetOwner(null!);
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
        }
    }
}
