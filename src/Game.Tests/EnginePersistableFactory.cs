using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Rtl;

namespace Aow.Game.Tests;

// Test harness for the persistence tree. Formats does not name these types.
public sealed class EnginePersistableFactory : IEnginePersistableFactory
{
    private readonly GameEngine _engine;
    private readonly Dictionary<string, EngineClass> _byName;

    public EnginePersistableFactory(GameEngine engine)
    {
        this._engine = engine ?? throw new InvalidOperationException("The engine is not running.");
        _byName = new Dictionary<string, EngineClass>();
        foreach (var engineClass in EngineClass.All)
            if (!_byName.TryAdd(engineClass.StorageKey, engineClass))
                throw new InvalidOperationException("Two engine classes are named " + engineClass.StorageKey + ".");
    }

    public GameEngine Engine => _engine;

    public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

    public void Release(IPersistable objectValue) => (objectValue as IDisposable)?.Dispose();

    public IPersistable Create(string className)
    {
        if (!_byName.TryGetValue(className, out var engineClass))
        {
            var resolved = NameTable.ResolveStored(className);
            if (resolved is null || !_byName.TryGetValue(resolved, out engineClass))
                throw new InvalidOperationException("The engine has no class " + className + ".");
        }
        if (!_engine.ClassTable.TryGetValue(engineClass.ClassId, out EngineClass? registered) || registered is null || registered.StorageKey != engineClass.StorageKey)
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + engineClass.ClassId.ToString(CultureInfo.InvariantCulture) + ")");
        return registered.CreateInstance(null);
    }
}
