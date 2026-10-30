using Aow.Rtl;

namespace Aow.Units.GFXE;

public interface IImageFileClass
{
    static abstract AnsiString GetName();
    static abstract AnsiString GetExtension();
    static abstract AnsiString GetDescription();
    static abstract int GetPixelFormatID(AnsiString fileName);
    static abstract TSurface CreateSurface(AnsiString fileName);
    static abstract int LoadToSurfaceDesc(AnsiString fileName, ref TSurfaceDesc surface, int x, int y);
}

public sealed class ImageFileClass
{
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
    public AnsiString GetName() => getName();
    public AnsiString GetExtension() => getExtension();
    public AnsiString GetDescription() => getDescription();
    public int GetPixelFormatID(AnsiString fileName) => getPixelFormatID(fileName);
    public TSurface CreateSurface(AnsiString fileName) => createSurface(fileName);
    public int LoadToSurfaceDesc(AnsiString fileName, ref TSurfaceDesc surface, int x, int y) => loadToSurfaceDesc(fileName, ref surface, x, y);
    public static ImageFileClass Of<T>() where T : TImageFile, IImageFileClass => Holder<T>.Value;

    private static class Holder<T> where T : TImageFile, IImageFileClass
    {
        public static readonly ImageFileClass Value = new(typeof(T), T.GetName, T.GetExtension,
            T.GetDescription, T.GetPixelFormatID, T.CreateSurface, T.LoadToSurfaceDesc);
    }
}
