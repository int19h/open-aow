using Aow.Rtl;

namespace Aow.Game.Draw;

public sealed class ImageFileClass
{
    private static readonly Catalog<Type, ImageFileClass> s_catalog = new("image file class");

    private delegate GraphicsResult LoadImage(string fileName, ref readonly LockedSurface surface, int x, int y);

    private readonly Func<string> _getName;
    private readonly Func<string> _getExtension;
    private readonly Func<string> _getDescription;
    private readonly Func<string, PixelFormat> _getPixelFormatId;
    private readonly Func<string, Surface> _createSurface;
    private readonly LoadImage _loadToSurfaceDescription;

    private ImageFileClass(Type type, Func<string> getName, Func<string> getExtension,
        Func<string> getDescription, Func<string, PixelFormat> getPixelFormatId,
        Func<string, Surface> createSurface, LoadImage loadToSurfaceDescription)
    {
        Type = type;
        _getName = getName;
        _getExtension = getExtension;
        _getDescription = getDescription;
        _getPixelFormatId = getPixelFormatId;
        _createSurface = createSurface;
        _loadToSurfaceDescription = loadToSurfaceDescription;
    }

    public Type Type { get; }
    public string StorageKey => NameTable.Stored(Type);
    public string Name => _getName();
    public string Extension => _getExtension();
    public string Description => _getDescription();

    public PixelFormat GetPixelFormatId(string fileName) => _getPixelFormatId(fileName);
    public Surface CreateSurface(string fileName) => _createSurface(fileName);
    public GraphicsResult LoadToSurfaceDescription(string fileName, ref readonly LockedSurface surface, int x, int y) => _loadToSurfaceDescription(fileName, in surface, x, y);

    public static ImageFileClass Register<T>() where T : ImageFile, IImageFileClass =>
        s_catalog.Add(typeof(T), new ImageFileClass(typeof(T), static () => T.Name, static () => T.Extension,
            static () => T.Description, T.GetPixelFormatId, T.CreateSurface, T.LoadToSurfaceDescription));

    public static ImageFileClass Of<T>() where T : ImageFile, IImageFileClass => s_catalog[typeof(T)];
    public static ImageFileClass Of(Type type) => s_catalog[type];

    public static IReadOnlyList<ImageFileClass> All => s_catalog.All;
}
