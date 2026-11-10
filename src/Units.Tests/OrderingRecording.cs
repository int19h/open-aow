using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

internal static class OrderingRecording
{
    internal static void Check(string name, SortedDictionary<string, string> actual)
    {
        Assert.Equal(ConstructionSnapshot.Keys(name), actual.Keys);
        foreach (var pair in actual) ConstructionSnapshot.Check(name, pair.Key, pair.Value);
    }

    internal static EObject Direct(Type type, ECustomNode? owner)
    {
        try
        {
            if (type.GetConstructor([typeof(ECustomNode)]) is { } ctor) return (EObject)ctor.Invoke([owner]);
            if (type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(ECustomNode)]) is { } factory)
                return (EObject)factory.Invoke(null, [owner])!;
            throw new InvalidOperationException(type.FullName + " has no public construction entry.");
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    internal static EObject Create(Type type, ECustomNode? owner, int route, Engine engine) => route switch
    {
        0 => Direct(type, owner),
        1 => EClass.Of(type).Create(owner!),
        2 => EClass.Of(type).Create(owner!),
        _ => engine.CreateEObject(EClass.Of(type).ClassID, owner!)
    };

    internal sealed class Fixture : IDisposable
    {
        private readonly int seed = Runtime.RandSeed;

        internal AoWDictionary Dictionary => dictionary;

        private readonly AoWDictionary dictionary = EngineFixture.Engine.FDictionary;
        private readonly AoWHSSet set = AoWHSSet.Current;
        private readonly AoWEngine engine = Game.Magic.Abilities.GameTables.AoWEngine;
        private readonly AoWHSMap map = AoWHSMap.Current;
        internal readonly ImageLibraryManager Manager = new ImageLibraryManager(null!);
        internal readonly Engine Engine;

        internal Fixture(Type? type = null)
        {
            Runtime.RandSeed = 123456789;
            var value = EngineFixture.CreateGameSet(Manager);
            if (type == typeof(ArmyHS)) value.FUnitGFXResources = new UnitGFXResourceList(null!);
            AoWHSSet.Current = value;
            Game.Magic.Abilities.GameTables.AoWEngine = EngineFixture.Engine;
            AoWHSMap.Current = null!;
            if (type != typeof(ArmyHS)) AoWHSSet.Current.FUnitGFXResources = null!;
            Runtime.RandSeed = 123456789;
            if (EngineHost.Engines is null) EngineHost.Initialize();
            Engine = new Engine(null!);
            Engine.Options -= Set<EngineModeElement>.Of(Engine.eoVerifyClasses);
            foreach (var descriptor in EClass.All.Where(x => x.Type == type))
            {
                try { Engine.UnregisterEClasses([Engine.GetEClass(descriptor.ClassID)]); }
                catch (System.Exception error) when (PortedExceptions.Is(error)) { }
                Engine.RegisterEClasses([descriptor]);
            }
        }

        public void Dispose()
        {
            Engine.Dispose();
            EngineFixture.Engine.FDictionary = dictionary;
            AoWHSSet.Current?.FUnitGFXResources?.Dispose();
            AoWHSSet.Current = set;
            Game.Magic.Abilities.GameTables.AoWEngine = engine;
            AoWHSMap.Current = map;
            Runtime.RandSeed = seed;
            Manager.Dispose();
        }
    }

    internal static string Snapshot(object value)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
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
                    if (field.Name == "captureOrdinal") continue;
                    if (field.Name == "eclass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + field.Name;
                    object? child = field.GetValue(item);
                    if (field.Name == "FOwner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }
}
