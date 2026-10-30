using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aow.Rtl.Tests")]

namespace Aow.Units.Engine;

// { The engine's class descriptor: one per TEObject descendant, registered in inheritance order by
//   the generated registry from a module initializer. It carries the saved class id, the native
//   instance size, the parent descriptor and the creation delegate. Descriptors are not TObjects
//   and registration allocates no instance. }
public sealed class EClass
{
    private static readonly Dictionary<Type, EClass> entries = [];
    private static readonly List<EClass> order = [];
    private readonly Func<TECustomNode, TEObject> create;

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
    public string ClassName => Aow.Rtl.TObject.ClassNameOf(Type);

    public TEObject Create(TECustomNode owner) => create(owner);
    public bool InheritsFrom(Type type) => type.IsAssignableFrom(Type);
    public override string ToString() => ClassName;

    // Every registered descriptor in registration order.
    public static IReadOnlyList<EClass> All
    {
        get { lock (entries) return order.ToArray(); }
    }

    public static EClass Of<T>() where T : TEObject => Of(typeof(T));

    public static EClass Of(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        lock (entries)
            return entries.TryGetValue(type, out var found) ? found
                : throw new InvalidOperationException($"Class {type} has no engine class descriptor.");
    }

    public static EClass Register(Type type, int classId, Func<TECustomNode, TEObject> create, int instanceSize)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(create);
        if (!typeof(TEObject).IsAssignableFrom(type))
            throw new ArgumentException("An engine class descriptor represents a TEObject descendant.", nameof(type));
        ArgumentOutOfRangeException.ThrowIfNegative(instanceSize);
        lock (entries)
        {
            if (entries.ContainsKey(type))
                throw new InvalidOperationException("An engine class is registered once per type.");
            var parent = type == typeof(TEObject) ? null : Of(type.BaseType);
            var result = new EClass(type, classId, parent, create, instanceSize);
            entries.Add(type, result);
            order.Add(result);
            return result;
        }
    }

    // A class whose constructor is still split allocates in its delegate and runs the initializer
    // here, with the guard the static factories keep: the initializer's failure destroys the instance.
    public static TEObject Initialize(TEObject instance, TECustomNode owner)
    {
        ArgumentNullException.ThrowIfNull(instance);
        try
        {
            instance.Create__init(owner);
        }
        catch
        {
            instance.Destroy();
            throw;
        }

        return instance;
    }
}
