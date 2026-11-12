
namespace Aow.Game.Draw.Images;

public interface ILibraryImageClass
{
    static abstract int ClassId { get; }
    static abstract byte ClassVersion { get; }
    static abstract string Name { get; }

    static abstract LibraryImage CreateInstance(CustomImageLibrary owner);
}
