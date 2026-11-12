using System.Collections;
using System.Reflection;
using System.Runtime.ExceptionServices;

using System.Text.Json;
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

internal static class OrderingRecording
{
    internal static void Check(string name, SortedDictionary<string, string> actual)
    {
        Assert.Equal(ConstructionSnapshot.Keys(name), actual.Keys);
        foreach (var pair in actual) ConstructionSnapshot.Check(name, pair.Key, pair.Value);
    }

    internal static EngineObject Direct(Type type, ECustomNode? owner)
    {
        try
        {
            if (type.GetConstructor([typeof(ECustomNode)]) is { } ctor) return (EngineObject)ctor.Invoke([owner]);
            if (type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(ECustomNode)]) is { } factory)
                return (EngineObject)factory.Invoke(null, [owner])!;
            throw new InvalidOperationException(type.FullName + " has no public construction entry.");
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    internal static EngineObject Create(Type type, ECustomNode? owner, int route, GameEngine engine) => route switch
    {
        0 => Direct(type, owner),
        1 => EngineClass.Of(type).CreateInstance(owner!),
        2 => EngineClass.Of(type).CreateInstance(owner!),
        _ => engine.CreateEngineObject(EngineClass.Of(type).ClassId, owner!)
    };

    internal sealed class Fixture : IDisposable
    {
        private readonly int _seed = Runtime.RandSeed;

        internal AoWDictionary Dictionary => _dictionary;

        private readonly AoWDictionary _dictionary = EngineFixture.Engine.Dictionary;
        private readonly GamePack _set = GamePack.Current;
        private readonly GameEngine _engine = GameTables.GameEngine;
        private readonly StrategicMap _map = StrategicMap.Current;

        internal ImageLibraryManager Manager { get; } = new ImageLibraryManager(null!);
        internal GameEngine Engine { get; }

        internal Fixture(Type? type = null)
        {
            Runtime.RandSeed = 123456789;
            var value = EngineFixture.CreateGameSet(Manager);
            if (type == typeof(ArmyHexSprite)) value.UnitGraphicsResources = new UnitGFXResourceList(null!);
            GamePack.Current = value;
            GameTables.GameEngine = EngineFixture.Engine;
            StrategicMap.Current = null!;
            if (type != typeof(ArmyHexSprite)) GamePack.Current.UnitGraphicsResources = null!;
            Runtime.RandSeed = 123456789;
            if (EngineHost.Engines is null) EngineHost.Initialize();
            Engine = new GameEngine(null!);
            Engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
            foreach (var descriptor in EngineClass.All.Where(x => x.Type == type))
            {
                try { Engine.UnregisterEngineClasses([Engine.GetEngineClass(descriptor.ClassId)]); }
                catch (Exception error) when (PortedExceptions.Is(error)) { }
                Engine.RegisterEngineClasses([descriptor]);
            }
        }

        public void Dispose()
        {
            Engine.Dispose();
            EngineFixture.Engine.Dictionary = _dictionary;
            GamePack.Current?.UnitGraphicsResources?.Dispose();
            GamePack.Current = _set;
            GameTables.GameEngine = _engine;
            StrategicMap.Current = _map;
            Runtime.RandSeed = _seed;
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
            if (InlineArrays.Elements(item) is { } elements) return elements.Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?>(StringComparer.Ordinal) { ["$type"] = type.FullName };
            for (Type? current = type; current is not null && current != typeof(object) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(MemberNames.Of, StringComparer.Ordinal))
                {
                    if (field.Name == "_engineClass") continue; // the descriptor cache is not constructor state
                    string key = current.Name + "." + MemberNames.Of(field);
                    object? child = field.GetValue(item);
                    if (field.Name == "_owner") { fields[key] = child is null ? null : "$owner"; continue; }
                    fields[key] = State(child, path + "." + key);
                }
            return fields;
        }
        return JsonSerializer.Serialize(State(value, "$"));
    }
}
