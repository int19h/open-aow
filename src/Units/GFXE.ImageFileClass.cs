using Aow.Rtl;

namespace Aow.Units.GFXE;

public interface IImageFileClass
{
    static abstract string Name { get; }
    static abstract string Extension { get; }
    static abstract string Description { get; }

    static abstract int GetPixelFormatID(string fileName);
    static abstract TSurface CreateSurface(string fileName);
    static abstract int LoadToSurfaceDesc(string fileName, ref TSurfaceDesc surface, int x, int y);
}

public sealed class ImageFileClass
{
    private static readonly Catalog<Type, ImageFileClass> catalog = new("image file class");

    private delegate int LoadImage(string fileName, ref TSurfaceDesc surface, int x, int y);

    private readonly Func<string> getName;
    private readonly Func<string> getExtension;
    private readonly Func<string> getDescription;
    private readonly Func<string, int> getPixelFormatID;
    private readonly Func<string, TSurface> createSurface;
    private readonly LoadImage loadToSurfaceDesc;

    private ImageFileClass(Type type, Func<string> getName, Func<string> getExtension,
        Func<string> getDescription, Func<string, int> getPixelFormatID,
        Func<string, TSurface> createSurface, LoadImage loadToSurfaceDesc)
    {
        Type = type;
        this.getName = getName;
        this.getExtension = getExtension;
        this.getDescription = getDescription;
        this.getPixelFormatID = getPixelFormatID;
        this.createSurface = createSurface;
        this.loadToSurfaceDesc = loadToSurfaceDesc;
    }

    public Type Type { get; }
    public string ClassName => NameTable.Stored(Type);
    public string Name => getName();
    public string Extension => getExtension();
    public string Description => getDescription();

    public int GetPixelFormatID(string fileName) => getPixelFormatID(fileName);
    public TSurface CreateSurface(string fileName) => createSurface(fileName);
    public int LoadToSurfaceDesc(string fileName, ref TSurfaceDesc surface, int x, int y) => loadToSurfaceDesc(fileName, ref surface, x, y);

    public static ImageFileClass Register<T>() where T : TImageFile, IImageFileClass =>
        catalog.Add(typeof(T), new ImageFileClass(typeof(T), static () => T.Name, static () => T.Extension,
            static () => T.Description, T.GetPixelFormatID, T.CreateSurface, T.LoadToSurfaceDesc));

    public static ImageFileClass Of<T>() where T : TImageFile, IImageFileClass => catalog[typeof(T)];
    public static ImageFileClass Of(Type type) => catalog[type];

    public static IReadOnlyList<ImageFileClass> All => catalog.All;
}
