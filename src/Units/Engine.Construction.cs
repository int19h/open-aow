[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Aow.Rtl.Tests")]

namespace Aow.Units.Engine;

// The registered metaclass holds creation beside its represented type and class metadata.
public static class EObjectCreation
{
    public static void Register(Type type, TEObject.Meta meta, Func<TECustomNode, TEObject> create, int instanceSize)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(create);
        if (!typeof(TEObject).IsAssignableFrom(type))
            throw new ArgumentException("An engine creation entry represents a TEObject descendant.", nameof(type));
        if (meta.Type is not null)
            throw new InvalidOperationException("A metaclass is registered once per type.");
        meta.CompletedCreation = create;
        Aow.Rtl.MetaRegistry.Register(type, meta, instanceSize);
    }
}
