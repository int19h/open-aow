using Aow.Rtl;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Aow.Game")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Aow.Rtl.Tests")]

namespace Aow.Units.ImageLib;

public static class LibraryImageCreation
{
    public static void Register(Type type, TLibraryImage.Meta meta, Func<TCustomImageLibrary, TLibraryImage> create, int instanceSize)
    {
        ArgumentNullException.ThrowIfNull(create);
        if (!typeof(TLibraryImage).IsAssignableFrom(type))
            throw new ArgumentException("An image creation entry represents a library image.", nameof(type));
        ArgumentNullException.ThrowIfNull(meta);
        if (meta.Type is not null)
            throw new InvalidOperationException("A metaclass is registered once per type.");
        meta.CompletedCreation = create;
        MetaRegistry.Register(type, meta, instanceSize);
    }
}
