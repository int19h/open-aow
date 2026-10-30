using Aow.Rtl;

namespace Aow.Units.GFXE;

public interface IImageFileClass
{
    static abstract AnsiString Name { get; }
    static abstract AnsiString Extension { get; }
    static abstract AnsiString Description { get; }
    static abstract int GetPixelFormatID(AnsiString fileName);
    static abstract TSurface CreateSurface(AnsiString fileName);
    static abstract int LoadToSurfaceDesc(AnsiString fileName, ref TSurfaceDesc surface, int x, int y);
}

public sealed class ImageFileClass
{
    private static readonly Catalog<Type, ImageFileClass> catalog = new("image file class");
    private delegate int LoadImage(AnsiString fileName, ref TSurfaceDesc surface, int x, int y);
    private readonly Func<AnsiString> getName;
    private readonly Func<AnsiString> getExtension;
    private readonly Func<AnsiString> getDescription;
    private readonly Func<AnsiString, int> getPixelFormatID;
    private readonly Func<AnsiString, TSurface> createSurface;
    private readonly LoadImage loadToSurfaceDesc;

    private ImageFileClass(Type type, Func<AnsiString> getName, Func<AnsiString> getExtension,
        Func<AnsiString> getDescription, Func<AnsiString, int> getPixelFormatID,
        Func<AnsiString, TSurface> createSurface, LoadImage loadToSurfaceDesc)
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
    public string ClassName => TObject.ClassNameOf(Type);
    public AnsiString Name => getName();
    public AnsiString Extension => getExtension();
    public AnsiString Description => getDescription();
    public int GetPixelFormatID(AnsiString fileName) => getPixelFormatID(fileName);
    public TSurface CreateSurface(AnsiString fileName) => createSurface(fileName);
    public int LoadToSurfaceDesc(AnsiString fileName, ref TSurfaceDesc surface, int x, int y) => loadToSurfaceDesc(fileName, ref surface, x, y);
    public static ImageFileClass Register<T>() where T : TImageFile, IImageFileClass =>
        catalog.Add(typeof(T), new ImageFileClass(typeof(T), static () => T.Name, static () => T.Extension,
            static () => T.Description, T.GetPixelFormatID, T.CreateSurface, T.LoadToSurfaceDesc));
    public static ImageFileClass Of<T>() where T : TImageFile, IImageFileClass => catalog[typeof(T)];
    public static ImageFileClass Of(Type type) => catalog[type];
    public static IReadOnlyList<ImageFileClass> All => catalog.All;
}
