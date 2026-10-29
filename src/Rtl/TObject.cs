namespace Aow.Rtl;

public class TObject
{
    private static int nextId;
    internal static int AllocateId() => Interlocked.Increment(ref nextId);
    public int InstanceId { get; } = AllocateId();
    public Meta ClassType => MetaRegistry.Of(GetType());
    public static string ClassNameOf(Type type) => type.Name.TrimStart('@');
    public virtual void Destroy() => Rtl.ForgetReference(this);
    public virtual void FreeInstance() { }

    public class Meta
    {
        public Type Type { get; internal set; } = null!;
        public string ClassName => ClassNameOf(Type);
        public Meta ClassParent => MetaRegistry.Of(Type.BaseType!);
        public int InstanceSize { get; internal set; }
        internal Func<TObject>? Factory;
        public virtual TObject NewInstance() => Factory is { } factory ? factory()
            : throw new InvalidOperationException($"Class {Type} has no raw allocation route.");
        public bool InheritsFrom(Type type) => type.IsAssignableFrom(Type);
        public static Meta Of<T>() where T : TObject => MetaRegistry.Of(typeof(T));
    }
}

public static class MetaRegistry
{
    private static readonly Dictionary<Type, TObject.Meta> entries = [];

    public static void Register(Type type, TObject.Meta meta, Func<TObject> factory, int instanceSize)
    {
        ArgumentNullException.ThrowIfNull(factory);
        RegisterCore(type, meta, factory, instanceSize);
    }

    public static void Register(Type type, TObject.Meta meta, int instanceSize) =>
        RegisterCore(type, meta, null, instanceSize);

    private static void RegisterCore(Type type, TObject.Meta meta, Func<TObject>? factory, int instanceSize)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(meta);
        if (!typeof(TObject).IsAssignableFrom(type))
            throw new ArgumentException("A metaclass represents a TObject descendant.", nameof(type));
        ArgumentOutOfRangeException.ThrowIfNegative(instanceSize);
        lock (entries)
        {
            if (entries.ContainsKey(type) || entries.ContainsValue(meta))
                throw new InvalidOperationException("A metaclass is registered once per type.");
            meta.Type = type;
            meta.Factory = factory;
            meta.InstanceSize = instanceSize;
            entries.Add(type, meta);
        }
    }

    public static TObject.Meta Of(Type type)
    {
        lock (entries)
            return entries.TryGetValue(type, out var meta) ? meta
                : throw new InvalidOperationException($"Class {type} is outside the metaclass closure.");
    }
}

public static class TObjectExtensions
{
    public static void Free(this TObject? value)
    {
        if (value is null) return;
        try { value.Destroy(); }
        finally { Rtl.ForgetReference(value); }
    }
}
