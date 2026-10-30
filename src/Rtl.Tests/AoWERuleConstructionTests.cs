using System.Collections;
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

[Collection("Engine")]
[EngineState]
public sealed partial class AoWERuleConstructionTests
{
    private sealed class Owner : TECustomNode
    {
        public Owner() : base(null!) { }

        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Observations = [];
        internal int Removals;
        internal bool Reject = false;
        public override int AddChild(TEObject child)
        {
            Members.Add(child);
            Observations.Add(Snapshot(child));
            if (Reject) throw new InvalidOperationException("owner rejects after insertion");
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child) { Removals++; return Members.Remove(child) ? 0 : -1; }
    }

    private sealed class Fixture : IDisposable
    {
        private readonly TAoWHSSet previousSet = Game.AoWHSSet;
        private readonly TAoWEngine previousEngine = Game.AoWEngine;
        private readonly TAoWHSMap previousMap = Game.AoWHSMap;
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal readonly TAoWHSMap Map;
        internal Fixture()
        {
            Game.AoWEngine = EngineFixture.Engine;
            Map = TAoWHSMap.Create(null!);
            var set = TAoWHSSet.Create(null!);
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(set, Manager);
            Game.AoWHSSet = set;
            Game.AoWEngine = null!;
            Map.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
            Map.FRaces = new TRaceList(null!);
            Map.FUnitControl = new TUnitControl(null!);
            Map.FHeroControl = new THeroControl(null!);
            Game.AoWHSMap = Map;
        }
        public void Dispose()
        {
            Game.AoWHSSet = previousSet;
            Game.AoWEngine = previousEngine;
            Game.AoWHSMap = previousMap;
            Manager.Destroy();
            Map.FRaces.Destroy();
            Map.FUnitControl.Destroy();
            Map.FHeroControl.Destroy();
        }
    }

    private static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            if (item is TAoWEngine) return "$engine";
            Type type = item.GetType();
            if (item is AnsiString text) return text.ToString();
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
            var fields = new SortedDictionary<string, object?> { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(TObject) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name))
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

    private static void AssertPrivateArrayChildren(TObject value)
    {
        if (value is TAbilityControl control)
            for (int i = 0; i < 5; i++) Assert.Equal(0, control.FAbilitiesByType[i].Count);
        if (value is TUnitResource resource)
            for (int i = 0; i < 3; i++)
            {
                Assert.NotNull(resource.FAbilities[i]);
                if (i > 0) Assert.NotSame(resource.FAbilities[0], resource.FAbilities[i]);
            }
        if (value is TUnitGFXResource graphics)
            for (int i = 0; i < 5; i++) Assert.NotNull(graphics.FSFXLibraries[i]);
        if (value is TCombatPredictor predictor)
        {
            Assert.NotNull(predictor.FSides[0]);
            Assert.NotNull(predictor.FSides[1]);
            Assert.NotSame(predictor.FSides[0], predictor.FSides[1]);
        }
        if (value is TSpellCastingAbility casting)
            for (int i = 1; i <= 5; i++) Assert.Equal(20, casting.FLevelCosts[i]);
        if (value is TLeadershipAbility leadership) Assert.Equal(10, leadership.FLevelCosts[1]);
    }

    private static string Hash(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    private static TObject Create(Type type, TECustomNode owner, int route, Fixture fixture)
    {
        if (route == 1) return EClass.Of(type).Create(owner);
        if (route == 2)
        {
            if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
            var engine = new TEngine(null!);
            try
            {
                var meta = EClass.Of(type);
                engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
                try { engine.UnregisterEClasses([engine.GetEClass(meta.ClassID)]); }
                catch (Aow.Rtl.Exception) { }
                engine.RegisterEClasses([EClass.Of(type)]);
                return engine.CreateEObject(meta.ClassID, owner);
            }
            finally { engine.Destroy(); }
        }
        if (route == 3)
        {
            var meta = Aow.Units.Engine.EClass.Of(type);
            return (TObject)Invoke(() => meta.GetType().GetMethod("Create", [typeof(TECustomNode)])!.Invoke(meta, [owner]))!;
        }
        var factory = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).SingleOrDefault(x => x.Name == "Create");
        var constructor = typeof(TEObject).IsAssignableFrom(type) ? type.GetConstructor([typeof(TECustomNode)]) : type.GetConstructors().OrderByDescending(x => x.GetParameters().Length).FirstOrDefault();
        Assert.True(factory is not null || constructor is not null, type.Name + " needs its own completed creation signature.");
        ParameterInfo[] parameters = factory?.GetParameters() ?? constructor!.GetParameters();
        object?[] args = parameters.Select(p => p.ParameterType == typeof(TECustomNode) ? owner
            : p.ParameterType == typeof(int) ? (object)37
            : p.ParameterType == typeof(THSMap) ? fixture.Map
            : p.ParameterType == typeof(TFastCombat) ? new TFastCombat(null!)
            : p.ParameterType == typeof(TCombatSide) ? new TCombatSide(null!)
            : null).ToArray();
        try { return (TObject)Invoke(() => factory is not null ? factory.Invoke(null, args) : constructor!.Invoke(args))!; }
        finally { foreach (object? arg in args) if (arg is TFastCombat or TCombatSide) ((TObject)arg).Destroy(); }
    }
    private static object? Invoke(Func<object?> action)
    {
        try { return action(); }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    private static readonly JsonDocument Baseline = JsonDocument.Parse(typeof(AoWERuleConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.AoWERuleConstructionBaseline.json")!);
    private static readonly JsonDocument FailureBaseline = JsonDocument.Parse(typeof(AoWERuleConstructionTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.AoWERuleFailureBaseline.json")!);
    public static IEnumerable<object[]> Classes => Names.Select(name => new object[] { name });
    public static IEnumerable<object[]> EngineClasses => Classes.Where(row => typeof(TEObject).IsAssignableFrom(typeof(TSpell).Assembly.GetType("Aow.Units.AoWE." + (string)row[0])!));
    public static IEnumerable<object[]> GuardedClasses => FailureBaseline.RootElement.EnumerateObject().Select(entry => new object[] { entry.Name });

    [Theory]
    [MemberData(nameof(Classes))]
    public void ConstructorAndRegistryRoutesMatchTheBaseCommit(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType("Aow.Units.AoWE." + name)!;
        using var fixture = new Fixture();
        bool engineObject = typeof(TEObject).IsAssignableFrom(type);
        for (int route = 0; route < (engineObject ? 4 : 1); route++)
        {
            var owner = new Owner();
            TObject? value = null;
            string state;
            var states = new List<string>();
            try
            {
                value = Create(type, owner, route, fixture);
                Assert.IsType(type, value);
                AssertPrivateArrayChildren(value);
                state = "success:" + Snapshot(value);
            }
            catch (System.Exception error) { state = "failure:" + error.GetType().FullName + ":" + error.Message; }
            finally
            {
                try { value?.Destroy(); }
                catch (System.Exception error) { states.Add("destroy:" + error.GetType().FullName + ":" + error.Message); }
            }
            states.Add(state);
            string actual = Hash(string.Join("\n", states));
            Assert.True(Baseline.RootElement.GetProperty(name).GetString() == actual, name + " route " + route + "\n" + string.Join("\n", states));
            Assert.StartsWith("success:", state);
        }
    }

    [Theory]
    [MemberData(nameof(EngineClasses))]
    public void NullOwnerRoutesKeepTheExactTypeAndIdenticalState(string name)
    {
        Type type = typeof(TSpell).Assembly.GetType("Aow.Units.AoWE." + name)!;
        using var fixture = new Fixture();
        string? expected = null;
        for (int route = 0; route < 4; route++)
        {
            var value = (TEObject)Create(type, null!, route, fixture);
            Assert.IsType(type, value);
            Assert.Null(value.Owner);
            Assert.Same(Aow.Units.Engine.EClass.Of(type), value.EClass);
            string state = Snapshot(value);
            try { value.Destroy(); }
            catch (System.Exception error) { state += "\n" + error.GetType().FullName + ":" + error.Message; }
            expected ??= state;
            Assert.Equal(expected, state);
        }
    }



    private sealed class ObservedAISettings(TECustomNode owner) : TAIMoveControlSettings(owner)
    {
        internal bool SawInitializedFields;
        public override void Clear()
        {
            SawInitializedFields = FTransportPath is not null && FRefCount == 1;
            base.Clear();
        }
    }

    private sealed class ObservedSelector : TMoveSelector
    {
        internal bool SawPreAssignmentFields;
        public override TMoveControlSettings CreateMCS()
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
        settings.Destroy();
        var selector = new ObservedSelector();
        Assert.True(selector.SawPreAssignmentFields);
        Assert.NotNull(selector.FMoveControlSettings);
        Assert.NotNull(selector.FMoveSettingsList);
        Assert.Equal(-1, selector.FCurrentMove);
        selector.Destroy();
    }

    [Fact]
    public void DictionaryContinuesToIgnoreItsComponentOwner()
    {
        var owner = new TComponent(null!);
        var dictionary = new TAoWDictionary(owner);
        Assert.Equal(0, owner.ComponentCount);
        dictionary.Destroy();
        owner.Destroy();
    }

    [Fact]
    public void TacticalSelectorConstructorKeepsVirtualSettingsOrder()
    {
        using var fixture = new Fixture();
        var value = new Aow.Units.AoWTC.TCombatUnitSelectionControl();
        try
        {
            Assert.IsType<Aow.Units.AoWTC.TCombatMoveControlSettings>(value.FMoveControlSettings);
            Assert.NotNull(value.FMoveControlSettings.FPath);
            Assert.NotNull(value.FMoveControlSettings.FSubMoves);
            Assert.NotNull(value.FMoveControlSettings.FUnits);
            Assert.Equal(0, value.FMoveSettingsList.FChildren.Count);
            Assert.Equal(-1, value.FCurrentMove);
        }
        finally { value.Destroy(); }
    }

    [Theory]
    [MemberData(nameof(GuardedClasses))]
    public void FailureAfterOwnerPublicationMatchesTheBaseCommit(string name)
    {
        Type type = typeof(TAbility).Assembly.GetType("Aow.Units.AoWE." + name)!;
        for (int route = 0; route < 4; route++)
        {
            using var fixture = new Fixture();
            Game.AoWHSSet = null!;
            var owner = new Owner { Reject = type == typeof(TArmyHS) };
            TObject? result = null;
            System.Exception? failure = null;
            try { result = Create(type, owner, route, fixture); }
            catch (System.Exception error) { failure = error; }
            Assert.Null(result);
            Assert.NotNull(failure);
            string state = JsonSerializer.Serialize(new
            {
                Error = failure.GetType().FullName,
                failure.Message,
                Members = owner.Members.Count,
                owner.Removals,
                owner.Observations
            });
            if (type == typeof(TArmyHS))
            {
                Assert.IsType<NullReferenceException>(failure);
                Assert.Single(owner.Members);
                Assert.Equal(0, owner.Removals);
            }
            else
            {
                Assert.Empty(owner.Members);
                Assert.Equal(1, owner.Removals);
            }
            Assert.True(FailureBaseline.RootElement.GetProperty(name).GetString() == Hash(state), name + " route " + route + " " + state);
        }
    }
}
