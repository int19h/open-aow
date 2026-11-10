
namespace Aow.Game.Draw.Images;

public interface ILibraryImageClass
{
    static abstract int ClassID { get; }
    static abstract byte ClassVersion { get; }
    static abstract string Name { get; }

    static abstract LibraryImage Create(CustomImageLibrary owner);
}
