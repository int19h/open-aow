using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Engine;

// The engine's class descriptor: one per TEObject descendant, registered in inheritance order by
// ClassRegistry from a module initializer. It carries the saved class id and the creation
// delegate. The base class and the inheritance tests come from Type. Descriptors are not TObjects
// and registration allocates no instance
public sealed class EngineClass
{
    private static readonly Catalog<Type, EngineClass> s_catalog = new("engine class");
    private static readonly List<EngineClass> s_aliases = [];
    private readonly Func<ECustomNode, EngineObject> _create;

    private EngineClass(Type type, int classId, Func<ECustomNode, EngineObject> create)
    {
        Type = type;
        ClassId = classId;
        _create = create;
    }

    public Type Type { get; }
    public int ClassId { get; }
    public string StorageKey => NameTable.Stored(Type);

    public EngineObject CreateInstance(ECustomNode owner) => _create(owner);

    public override string ToString() => StorageKey;

    // Every registered descriptor in registration order. Aliases are the folded engine ids.
    public static IReadOnlyList<EngineClass> All => s_catalog.All;

    // Class ids 17 and 65537 construct GameEngine. The type catalog keeps id 131073.
    internal static IReadOnlyList<EngineClass> Aliases => s_aliases;

    public static EngineClass Of<T>() where T : EngineObject => Of(typeof(T));

    public static EngineClass Of(Type type) => s_catalog[type];

    // The base class registers first, so a walk up Type.BaseType finds a descriptor at every step.
    public static EngineClass Register<T>(int classId, Func<ECustomNode, T> create)
        where T : EngineObject
    {
        var type = typeof(T);
        ArgumentNullException.ThrowIfNull(create);
        if (type != typeof(EngineObject)) Of(type.BaseType);
        return s_catalog.Add(type, new EngineClass(type, classId, create));
    }

    // A second class id for a type that the catalog already names. The folded engine keeps
    // 131073 on the type and records 17 and 65537 here. Each alias still constructs that type.
    internal static EngineClass Alias<T>(int classId, Func<ECustomNode, T> create)
        where T : EngineObject
    {
        var type = typeof(T);
        ArgumentNullException.ThrowIfNull(create);
        if (type != typeof(EngineObject)) Of(type.BaseType);
        var alias = new EngineClass(type, classId, create);
        s_aliases.Add(alias);
        return alias;
    }
}
