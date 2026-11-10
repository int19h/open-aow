using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Engine;

// The engine's class descriptor: one per TEObject descendant, registered in inheritance order by
// ClassRegistry from a module initializer. It carries the saved class id, the native instance size,
// the parent descriptor and the creation delegate. Descriptors are not TObjects and registration
// allocates no instance
public sealed class EClass
{
    private static readonly Catalog<Type, EClass> catalog = new("engine class");
    private static readonly List<EClass> aliases = [];
    private Func<ECustomNode, EObject> create;

    private EClass(Type type, int classId, EClass parent, Func<ECustomNode, EObject> create, int instanceSize)
    {
        Type = type;
        ClassID = classId;
        Parent = parent;
        InstanceSize = instanceSize;
        this.create = create;
    }

    public Type Type { get; }
    public int ClassID { get; }
    public int InstanceSize { get; }
    // The base class's descriptor, null at TEObject.
    public EClass Parent { get; }
    public string ClassName => NameTable.Stored(Type);

    public EObject Create(ECustomNode owner) => create(owner);

    // A test swaps the delegate and restores the previous one. Create has no per-call check.
    internal Func<ECustomNode, EObject> SubstituteCreate(Func<ECustomNode, EObject> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        var previous = create;
        create = next;
        return previous;
    }

    public bool InheritsFrom(Type type) => type.IsAssignableFrom(Type);
    public override string ToString() => ClassName;

    // Every registered descriptor in registration order. Aliases are the folded engine ids.
    public static IReadOnlyList<EClass> All => catalog.All;

    // Class ids 17 and 65537 construct GameEngine. The type catalog keeps id 131073.
    internal static IReadOnlyList<EClass> Aliases => aliases;

    public static EClass Of<T>() where T : EObject => Of(typeof(T));

    public static EClass Of(Type type) => catalog[type];

    public static EClass Register<T>(int classId, Func<ECustomNode, T> create, int instanceSize)
        where T : EObject
    {
        var type = typeof(T);
        ArgumentNullException.ThrowIfNull(create);
        ArgumentOutOfRangeException.ThrowIfNegative(instanceSize);
        var parent = type == typeof(EObject) ? null : Of(type.BaseType);
        return catalog.Add(type, new EClass(type, classId, parent, create, instanceSize));
    }

    // A second class id for a type that the catalog already names. The folded engine keeps
    // 131073 on the type and records 17 and 65537 here. Each alias still constructs that type.
    internal static EClass Alias<T>(int classId, Func<ECustomNode, T> create, int instanceSize)
        where T : EObject
    {
        var type = typeof(T);
        ArgumentNullException.ThrowIfNull(create);
        ArgumentOutOfRangeException.ThrowIfNegative(instanceSize);
        var parent = type == typeof(EObject) ? null : Of(type.BaseType);
        var alias = new EClass(type, classId, parent, create, instanceSize);
        aliases.Add(alias);
        return alias;
    }
}
