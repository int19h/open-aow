using Aow.Game.Engine.Nodes;
using Aow.Rtl;

namespace Aow.Game.Engine;

// The engine's class descriptor: one per TEObject descendant, registered in inheritance order by
// ClassRegistry from a module initializer. It carries the saved class id, the native instance size,
// the parent descriptor and the creation delegate. Descriptors are not TObjects and registration
// allocates no instance
public sealed class EClass
{
    private static readonly Catalog<Type, EClass> catalog = new("engine class");
    private Func<TECustomNode, TEObject> create;

    private EClass(Type type, int classId, EClass parent, Func<TECustomNode, TEObject> create, int instanceSize)
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

    public TEObject Create(TECustomNode owner) => create(owner);

    // A test swaps the delegate and restores the previous one. Create has no per-call check.
    internal Func<TECustomNode, TEObject> SubstituteCreate(Func<TECustomNode, TEObject> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        var previous = create;
        create = next;
        return previous;
    }

    public bool InheritsFrom(Type type) => type.IsAssignableFrom(Type);
    public override string ToString() => ClassName;

    // Every registered descriptor in registration order.
    public static IReadOnlyList<EClass> All => catalog.All;

    public static EClass Of<T>() where T : TEObject => Of(typeof(T));

    public static EClass Of(Type type) => catalog[type];

    public static EClass Register<T>(int classId, Func<TECustomNode, T> create, int instanceSize)
        where T : TEObject
    {
        var type = typeof(T);
        ArgumentNullException.ThrowIfNull(create);
        ArgumentOutOfRangeException.ThrowIfNegative(instanceSize);
        var parent = type == typeof(TEObject) ? null : Of(type.BaseType);
        return catalog.Add(type, new EClass(type, classId, parent, create, instanceSize));
    }
}
