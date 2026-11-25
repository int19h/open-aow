#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
        private readonly bool _reject = false;

        public override int AddChild(EngineObject child)
        {
            _ = base.AddChild(child);
            _observations.Add(Snapshot(child));
            return _reject ? throw new InvalidOperationException("owner rejects after insertion") : Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child) => base.RemoveChild(child) < 0 ? -1 : 0;
    }

    private sealed class Fixture : IDisposable
    {
        private readonly IDisposable _previousSetScope = EngineFixture.UsePack(GamePack.Current);
        private readonly IDisposable _previousMapScope = EngineFixture.UseMap(StrategicMap.Current);
        private readonly ImageLibraryManager _manager = new(null);

        internal StrategicMap Map { get; }

        internal Fixture()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            GamePack set = EngineFixture.CreateGamePack(_manager);
            EngineFixture.SetPack(set);
            Map.Status = HexSpriteMapStatuses.Destroying;
            Map.Races = new RaceList(null) { Holder = EngineFixture.Engine };
            Map.UnitControl = new UnitControl(null) { Holder = EngineFixture.Engine };
            Map.HeroControl = new HeroControl(null) { Holder = EngineFixture.Engine };
            EngineFixture.SetMap(Map);
        }

        public void Dispose()
        {
            _previousSetScope.Dispose();
            _previousMapScope.Dispose();
            _manager.Dispose();
            Map.Races.Dispose();
            Map.UnitControl.Dispose();
            Map.HeroControl.Dispose();
        }
    }

    private static string Snapshot(object value)
    {
        Dictionary<object, string> visited = [with(ReferenceEqualityComparer.Instance)];
        object? State(object? item, string path, Type? declaredType = null)
        {
            if (OriginalValueSnapshot.TryRender(item, declaredType: declaredType, rendered: out object? originalValue))
            {
                return originalValue;
            }

            if (item is null)
            {
                return null;
            }

            if (item is GameEngine)
            {
                return "$engine";
            }

            Type type = item.GetType();
            if (item is string text)
            {
                return text;
            }

            if (type.IsEnum)
            {
                return Convert.ToInt64(item);
            }

            if (type.IsPrimitive || item is string || item is decimal)
            {
                return item;
            }

            if (item is Type represented)
            {
                return represented.FullName;
            }

            if (item is Delegate handler)
            {
                return HandlerNames.Of(handler);
            }

            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found))
                {
                    return $"$ref:{found}";
                }

                visited.Add(item, path);
            }
            if (item is Array array)
            {
                return array.Cast<object?>().Select((x, i) => State(x, path: $"{path}[{i}]", declaredType: type.GetElementType())).ToArray();
            }

            if (InlineArrays.Elements(item) is { } elements)
            {
                return elements.Select((x, i) => State(x, path: $"{path}[{i}]", declaredType: InlineArrays.ElementType(item))).ToArray();
            }

            if (item is IList list)
            {
                return list.Cast<object?>().Select((x, i) => State(x, path: $"{path}[{i}]", declaredType: type.IsGenericType ? type.GetGenericArguments()[0] : null)).ToArray();
            }

            SortedDictionary<string, object?> fields = new(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
            {
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, comparer: StringComparer.Ordinal))
                {
                    if (MemberNames.Of(field) is "_engineClass" or "EngineClass" or "_actionClass" or "ActionClass" or "_groupClass" or "GroupClass" or "Holder" or "_rememberedEngine" or "Creator")
                    {
                        continue; // the descriptor caches and the link to the engine are not constructor state
                    }
                    // CF-2 stores priority as a delegate. Keep its value in the base recording's key.
                    if (item is AIPlayerActionClass actionClass && field.Name == "_priority")
                    {
                        fields[$"{current.Name}.<Priority>k__BackingField"] = actionClass.Priority;
                        continue;
                    }
                    string key = $"{current.Name}.{MemberNames.Of(field)}";
                    object? child = field.GetValue(item);
                    if (MemberNames.Of(field) is "_owner" or "Owner")
                    {
                        fields[key] = child is null ? null : "$owner";
                        continue;
                    }
                    fields[key] = State(child, path: $"{path}.{key}", declaredType: field.FieldType);
                }
            }

            return fields;
        }
        return JsonSerializer.Serialize(State(value, path: "$"));
    }

    private static void AssertPrivateArrayChildren(object value)
    {
        if (value is AbilityControl control)
        {
            for (int i = 0; i < 5; i++)
            {
                Assert.Empty(control.AbilitiesByType[i]);
            }
        }

        if (value is UnitResource resource)
        {
            for (int i = 0; i < 3; i++)
            {
                Assert.NotNull(resource.Abilities[i]);
                if (i > 0)
                {
                    Assert.NotSame(resource.Abilities[0], resource.Abilities[i]);
                }
            }
        }

        if (value is UnitGraphicsResource graphics)
        {
            for (int i = 0; i < 5; i++)
            {
                Assert.NotNull(graphics.SfxLibraries[i]);
            }
        }

        if (value is CombatPredictor predictor)
        {
            Assert.NotNull(predictor.Sides[0]);
            Assert.NotNull(predictor.Sides[1]);
            Assert.NotSame(predictor.Sides[0], predictor.Sides[1]);
        }
        if (value is SpellCastingAbility casting)
        {
            for (int i = 1; i <= 5; i++)
            {
                Assert.Equal(20, casting.LevelCosts[i]);
            }
        }

        if (value is LeadershipAbility leadership)
        {
            Assert.Equal(10, leadership.LevelCosts[1]);
        }
    }

    private static object Create(Type type, CustomNode owner, int route, Fixture fixture)
    {
        if (route == 1)
        {
            return EngineClass.Of(type).CreateInstance(owner, holder: EngineFixture.Engine);
        }

        if (route == 2)
        {
            using GameEngine engine = new(null!);
            EngineClass meta = EngineClass.Of(type);
            engine.Options &= ~GameEngine.VerifyClasses;
            try
            {
                engine.UnregisterEngineClasses([engine.GetEngineClass(meta.ClassId)]);
            }
            catch (Exception error) when (PortedExceptions.Is(error)) { }
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
            return engine.CreateEngineObject(meta.ClassId, owner);
        }
        if (route == 3)
        {
            EngineClass meta = EngineClass.Of(type);
            return Invoke(() => meta.GetType().GetMethod("CreateInstance", types: [typeof(CustomNode), typeof(EngineObject)])!.Invoke(meta, parameters: [owner, EngineFixture.Engine]))!;
        }
        MethodInfo? factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        ConstructorInfo? constructor = typeof(EngineObject).IsAssignableFrom(type) ? EngineClassCreation.OwnerConstructor(type) : type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
        Assert.True(factory is not null || constructor is not null, userMessage: $"{type.Name} needs its own completed creation signature.");
        ParameterInfo[] parameters = factory?.GetParameters() ?? constructor!.GetParameters();
        object?[] arguments = [.. parameters.Select(p => p.ParameterType == typeof(CustomNode) ? owner
            : p.ParameterType == typeof(int) ? (object)37
            : p.ParameterType == typeof(GameMap) ? fixture.Map
            : p.ParameterType == typeof(EngineObject) ? EngineFixture.Engine
            : p.ParameterType == typeof(FastBattle) ? new FastBattle(null!) { Holder = EngineFixture.Engine }
            : p.ParameterType == typeof(CombatSide) ? new CombatSide(null!) { Holder = EngineFixture.Engine }
            : null)];
        try
        {
            return Invoke(() => factory is not null ? factory.Invoke(null, parameters: arguments) : constructor!.Invoke(arguments))!;
        }
        finally
        {
            foreach (object? argument in arguments)
            {
                if (argument is FastBattle or CombatSide)
                {
                    ((argument ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                }
            }
        }
    }

    private static object? Invoke(Func<object?> action)
    {
        try
        {
            return action();
        }
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
        Type[] matches = [.. typeof(Spell).Assembly.GetTypes().Where(type => type.Name == name)];
        if (matches.Length == 1)
        {
            return matches[0];
        }

        Type[] top = [.. matches.Where(type => type.DeclaringType is null)];
        return top.Length == 1
            ? top[0]
            : throw new InvalidOperationException(string.Create(CultureInfo.InvariantCulture, handler: $"{name} matches {matches.Length} types."));
    }

    public static IEnumerable<object[]> FormerlyGuardedClasses => new[] { "Ability", "AdjustableAbility", "ArmyHexSprite", "BoltsAbility", "BreathAbility", "CharmAbility", "CharmedAbility", "CommandAbility", "CommandedAbility", "ConstructAbility", "CosmagicSurgeryAbility", "DispelMagicAbility", "DominateAbility", "DominatedAbility", "DurationAbility", "EnhancementAbility", "EntangleAbility", "FlameThrowingAbility", "HealingAbility", "InvokeDeathAbility", "LeadershipAbility", "MultiLevelAbility", "PossessAbility", "PossessedAbility", "RangedAttackAbility", "RoundAttackAbility", "SeduceAbility", "SeducedAbility", "SelfDestructAbility", "Spell", "SpellCastingAbility", "StrikeAbility", "StrikeEnhancementAbility", "TouchAbility", "TransportAbility", "TurnUndeadAbility", "UnitEnchantmentAbility", "UnitSpell", "WallCrushingAbility", "WebAbility" }.Select(name => new object[] { name });

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorAndRegistryRoutesMatchTheBaseCommit(string name)
    {
        Type type = TypeNamed(name);
        using Fixture fixture = new();
        bool engineObject = typeof(EngineObject).IsAssignableFrom(type);
        for (int route = 0; route < (engineObject ? 4 : 1); route++)
        {
            RuleOwner owner = new();
            object? value = null;
            string state;
            List<string> states = [];
            try
            {
                value = Create(type, owner: EngineFixture.OwnerFor(type, recorder: owner), route, fixture);
                Assert.IsType(type, @object: value);
                AssertPrivateArrayChildren(value);
                state = $"success:{Snapshot(value)}";
            }
            catch (Exception error)
            {
                state = $"failure:{error.GetType().FullName}:{error.Message}";
            }
            finally
            {
                try
                {
                    // Route 0 leaves the player on an owner that reaches no engine. Route 2 leaves
                    // it on an engine whose map slot is empty. Disposal reads the fixture map.
                    if (value is Player player)
                    {
                        GameEngine? reached = player.GetEngine();
                        if (reached is null)
                        {
                            player.Holder = EngineFixture.Engine;
                        }
                        else
                        {
                            reached.StrategicMap ??= EngineFixture.Engine.StrategicMap;
                        }
                    }

                    (value as IDisposable)?.Dispose();
                }
                catch (Exception error)
                {
                    states.Add($"destroy:{error.GetType().FullName}:{error.Message}");
                }
            }
            states.Add(state);
            ConstructionSnapshot.Check("AoWERuleConstruction", key: name, actual: string.Join("\n", values: states));
            Assert.StartsWith("success:", state);
        }
    }

    [Theory]
    [MemberData(nameof(EngineClasses))]
    public void NullOwnerRoutesKeepTheExactTypeAndIdenticalState(string name)
    {
        Type type = TypeNamed(name);
        using Fixture fixture = new();
        string? expected = null;
        for (int route = 0; route < 4; route++)
        {
            EngineObject value = (EngineObject)Create(type, owner: null!, route, fixture);
            Assert.IsType(type, @object: value);
            Assert.Null(value.Owner);
            Assert.Same(EngineClass.Of(type), value.EngineClass);
            string state = Snapshot(value);
            // A null owner has no chain. The fixture holder is the engine whose map is current, so
            // disposal reads that map. The snapshot above is the pre-disposal state.
            value.Holder = EngineFixture.Engine;
            try
            {
                value.Dispose();
            }
            catch (Exception error)
            {
                state += $"\n{error.GetType().FullName}:{error.Message}";
            }
            expected ??= state;
            Assert.Equal(expected, state);
        }
    }

    private sealed class ObservedAISettings(CustomNode owner) : AIMoveControlSettings(owner)
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
        public ObservedSelector() : base(EngineFixture.Engine) { }

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
        using Fixture fixture = new();
        ObservedAISettings settings = new(null!) { Holder = EngineFixture.Engine };
        Assert.True(settings.SawInitializedFields);
        Assert.Equal(175, settings.MovePointsMax);
        Assert.Equal(-1, settings.Destination.X);
        settings.Dispose();
        using ObservedSelector selector = new();
        Assert.True(selector.SawPreAssignmentFields);
        Assert.NotNull(selector.MoveControlSettings);
        Assert.NotNull(selector.MoveSettingsList);
        Assert.Equal(-1, selector.CurrentMove);
    }

    [Fact]
    public void TacticalSelectorConstructorKeepsVirtualSettingsOrder()
    {
        using Fixture fixture = new();
        CombatUnitSelectionControl value = new(EngineFixture.Engine);
        try
        {
            _ = Assert.IsType<CombatMoveControlSettings>(value.MoveControlSettings);
            Assert.NotNull(value.MoveControlSettings.Path);
            Assert.NotNull(value.MoveControlSettings.SubMoves);
            Assert.NotNull(value.MoveControlSettings.Units);
            Assert.Equal(0, value.MoveSettingsList.Count);
            Assert.Equal(-1, value.CurrentMove);
        }
        finally
        {
            value.Dispose();
        }
    }

    [Theory]
    [MemberData(nameof(FormerlyGuardedClasses))]
    public void PlainConstructorsPublishCompletedState(string name)
    {
        Type type = TypeNamed(name);
        for (int route = 0; route < 4; route++)
        {
            using Fixture fixture = new();
            RuleOwner owner = new();
            CustomNode given = EngineFixture.OwnerFor(type, recorder: owner);
            EngineObject value = Assert.IsAssignableFrom<EngineObject>(Create(type, owner: given, route, fixture));
            Assert.IsType(type, @object: value);
            Assert.Same(given, value.Owner);
            Assert.Same(value, Assert.Single(owner.Members));
            Assert.Equal(0, owner.Removals);
            if (value is ArmyHexSprite army)
            {
                Assert.NotNull(army.Army);
            }

            value.SetOwner(null);
            // Detached from the recorder, the object keeps the fixture engine that holds the current map.
            value.Holder = EngineFixture.Engine;
            value.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(1, owner.Removals);
        }
    }
}
