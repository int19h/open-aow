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

internal static class OrderingRecording
{
    internal static string Hash(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    internal static void Check(string name, SortedDictionary<string, string> actual)
    {
        string? target = Environment.GetEnvironmentVariable("AOW_ORDERING_RECORD");
        if (target is not null)
            File.WriteAllText(Path.Combine(target, name + "Baseline.json"), JsonSerializer.Serialize(actual, new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n", StringComparison.Ordinal) + "\n", new UTF8Encoding(false));
        using var expected = JsonDocument.Parse(typeof(OrderingRecording).Assembly.GetManifestResourceStream("Aow.Rtl.Tests." + name + "Baseline.json")!);
        Assert.Equal(expected.RootElement.EnumerateObject().Count(), actual.Count);
        foreach (var pair in actual) Assert.True(expected.RootElement.GetProperty(pair.Key).GetString() == pair.Value, pair.Key + "\n" + pair.Value);
    }
    internal static TEObject Direct(Type type, TECustomNode? owner)
    {
        try
        {
            if (type.GetConstructor([typeof(TECustomNode)]) is { } ctor) return (TEObject)ctor.Invoke([owner]);
            if (type.GetMethod("Create", BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly, [typeof(TECustomNode)]) is { } factory)
                return (TEObject)factory.Invoke(null, [owner])!;
            throw new InvalidOperationException(type.FullName + " has no public construction entry.");
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }
    internal static TEObject Create(Type type, TECustomNode? owner, int route, TEngine engine) => route switch
    {
        0 => Direct(type, owner),
        1 => EClass.Of(type).Create(owner!),
        2 => ((TEObject.Meta)MetaRegistry.Of(type)).Create(owner!),
        _ => engine.CreateEObject(EClass.Of(type).ClassID, owner!)
    };
    internal sealed class Fixture : IDisposable
    {
        private readonly int seed = Rtl.RandSeed;
        internal TAoWDictionary Dictionary => dictionary;
        private readonly TAoWDictionary dictionary = EngineFixture.Engine.FDictionary;
        private readonly TAoWHSSet set = Game.AoWHSSet;
        private readonly TAoWEngine engine = Game.AoWEngine;
        private readonly TAoWHSMap map = Game.AoWHSMap;
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal readonly TEngine Engine;
        internal Fixture(Type? type = null)
        {
            Rtl.RandSeed = 123456789;
            var value = TAoWHSSet.Create(null!);
            if (type == typeof(TArmyHS)) value.FUnitGFXResources = new TUnitGFXResourceList(null!);
            typeof(THSSet).GetField("FImageLibraryManager", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(value, Manager);
            Game.AoWHSSet = value;
            Game.AoWEngine = EngineFixture.Engine;
            Game.AoWHSMap = null!;
            if (type != typeof(TArmyHS)) Game.AoWHSSet.FUnitGFXResources = null!;
            Rtl.RandSeed = 123456789;
            if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
            Engine = new TEngine(null!);
            Engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
            foreach (var descriptor in EClass.All.Where(x => x.Type == type))
            {
                try { Engine.UnregisterEClasses([Engine.GetEClass(descriptor.ClassID)]); }
                catch (Aow.Rtl.Exception) { }
                Engine.RegisterEClasses([descriptor]);
            }
        }
        public void Dispose()
        {
            Engine.Destroy();
            EngineFixture.Engine.FDictionary = dictionary;
            Game.AoWHSSet?.FUnitGFXResources?.Destroy();
            Game.AoWHSSet = set;
            Game.AoWEngine = engine;
            Game.AoWHSMap = map;
            Rtl.RandSeed = seed;
            Manager.Destroy();
        }
    }
    internal static string Snapshot(object value, int origin)
    {
        var visited = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        object? State(object? item, string path)
        {
            if (item is null) return null;
            Type type = item.GetType();
            if (item is AnsiString text) return text.ToString();
            if (type.IsEnum) return Convert.ToInt64(item);
            if (type.IsPrimitive || item is string || item is decimal) return item;
            if (item is Type represented) return represented.FullName;
            if (item is TObject.Meta meta) return meta.Type.FullName;
            if (item is Delegate handler) return handler.GetInvocationList().Select(x => x.Method.DeclaringType!.FullName + "." + x.Method.Name).ToArray();
            if (!type.IsValueType)
            {
                if (visited.TryGetValue(item, out string? found)) return "$ref:" + found;
                visited.Add(item, path);
            }
            if (item is Array array) return array.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            if (item is IList list) return list.Cast<object?>().Select((x, i) => State(x, path + "[" + i + "]")).ToArray();
            var fields = new SortedDictionary<string, object?> { ["$type"] = type.FullName };
            if (item is TObject obj) fields["$offset"] = obj.InstanceId - origin;
            for (Type? current = type; current is not null && current != typeof(TObject) && current != typeof(object); current = current.BaseType)
                foreach (FieldInfo field in current.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).OrderBy(x => x.Name))
                {
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
