#nullable enable
using System.Globalization;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Rtl;

namespace Aow.Engine;

/// <summary>
/// The factory of a game load or save through the tree (persistence-tree.md 7). It creates an
/// object of a stored class name the way <c>TEReadStorageStream.rwCreateEObject</c> creates one of
/// a class id: the class the engine table holds at that id, with the engine as its holder. It
/// names the engine the load reads (<c>Stream.Engine</c> of <c>ReadWrite</c>) and the progress
/// handler the storage stream held (<c>OnProgress</c>).
/// </summary>
public sealed class EnginePersistableFactory : IEnginePersistableFactory
{
    // The classes by stored name, indexed at the first creation. A copy of an object that creates
    // nothing never builds it.
    private Dictionary<string, EngineClass>? _byName;

    public EnginePersistableFactory(GameEngine engine, ProgressEvent? progress = null)
    {
        Engine = engine ?? throw new InvalidOperationException("The engine is not running.");
        Progress = progress;
    }

    public GameEngine Engine { get; }

    public ProgressEvent? Progress { get; }

    public string ClassName(IPersistable objectValue) => NameTable.Stored(objectValue.GetType());

    public void Release(IPersistable objectValue) => (objectValue as IDisposable)?.Dispose();

    public IPersistable Create(string className)
    {
        var byName = _byName ??= Index();
        if (!byName.TryGetValue(className, out var engineClass))
        {
            var resolved = NameTable.ResolveStored(className);
            if (resolved is null || !byName.TryGetValue(resolved, out engineClass))
                throw new InvalidOperationException("The engine has no class " + className + ".");
        }
        // The stream stores the class id, and rwCreateEObject creates the class the engine table
        // holds at that id (TEngine.GetEClass). A class that keeps its parent's id, such as a
        // tactical battle unit, comes back as the class the table holds there.
        if (!Engine.ClassTable.TryGetValue(engineClass.ClassId, out EngineClass? registered) || registered is null)
            throw new KeyNotFoundException("Unregistered E-ClassID found (" + engineClass.ClassId.ToString(CultureInfo.InvariantCulture) + ")");
        return registered.CreateInstance(null!, holder: Engine);
    }

    private static Dictionary<string, EngineClass> Index()
    {
        var byName = new Dictionary<string, EngineClass>();
        foreach (var engineClass in EngineClass.All)
            if (!byName.TryAdd(engineClass.StorageKey, engineClass))
                throw new InvalidOperationException("Two engine classes are named " + engineClass.StorageKey + ".");
        return byName;
    }
}
