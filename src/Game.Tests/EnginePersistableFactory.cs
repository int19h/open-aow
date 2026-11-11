using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Rtl;

namespace Aow.Game.Tests;

// Test harness for the persistence tree. Formats does not name these types.
public sealed class EnginePersistableFactory : IPersistableFactory
{
    private readonly GameEngine engine;
    private readonly Dictionary<string, EngineClass> byName;

    public EnginePersistableFactory(GameEngine engine)
    {
        this.engine = engine ?? throw new InvalidOperationException("The engine is not running.");
        byName = new Dictionary<string, EngineClass>();
        foreach (var eclass in EngineClass.All)
            if (!byName.TryAdd(eclass.ClassName, eclass))
                throw new InvalidOperationException("Two engine classes are named " + eclass.ClassName + ".");
    }

    public string ClassName(IPersistable obj) => NameTable.Stored(obj.GetType());

    public void Release(IPersistable obj) => (obj as IDisposable)?.Dispose();

    public IPersistable Create(string className)
    {
        if (!byName.TryGetValue(className, out var eclass))
        {
            var resolved = NameTable.ResolveStored(className);
            if (resolved is null || !byName.TryGetValue(resolved, out eclass))
                throw new InvalidOperationException("The engine has no class " + className + ".");
        }
        if (!engine.ClassTable.TryGetValue(eclass.ClassID, out EngineClass? registered) || registered is null || registered.ClassName != eclass.ClassName)
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + eclass.ClassID.ToString(CultureInfo.InvariantCulture) + ")");
        return registered.Create(null);
    }
}
