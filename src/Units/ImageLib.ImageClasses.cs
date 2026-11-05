using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public interface ILibraryImageClass
{
    static abstract int ClassID { get; }
    static abstract byte ClassVersion { get; }
    static abstract AnsiString Name { get; }
    static abstract TLibraryImage Create(TCustomImageLibrary owner);
}

public sealed class LibraryImageClass
{
    private static readonly Catalog<Type, LibraryImageClass> catalog = new("library image class");
    private readonly Func<TCustomImageLibrary, TLibraryImage> create;

    private LibraryImageClass(Type type, int classID, byte classVersion, AnsiString name, Func<TCustomImageLibrary, TLibraryImage> create)
    {
        Type = type;
        ClassID = classID;
        ClassVersion = classVersion;
        Name = name;
        this.create = create;
    }

    public Type Type { get; }
    public string ClassName => TObject.ClassNameOf(Type);
    public int ClassID { get; }
    public byte ClassVersion { get; }
    public AnsiString Name { get; }
    public TLibraryImage Create(TCustomImageLibrary owner) => create(owner);
    public static LibraryImageClass Register<T>() where T : TLibraryImage, ILibraryImageClass =>
        catalog.Add(typeof(T), new LibraryImageClass(typeof(T), T.ClassID, T.ClassVersion, T.Name, T.Create));
    public static LibraryImageClass Of<T>() where T : TLibraryImage, ILibraryImageClass => catalog[typeof(T)];
    public static LibraryImageClass Of(Type type) => catalog[type];
    public static IReadOnlyList<LibraryImageClass> All => catalog.All;

    public int GrabImages(in TSurfaceDesc SD, TGrabInfo GrabInfo, TCustomImageLibrary ImageLibrary, TGrabProgressEvent OnProgress)
    {
        int Result = default;
        LibraryImageClass ImageClass = default;
        TLibraryImage Image = default;
        AnsiString Msg = default;
        TRectangleList Rects = default;
        int I = default;
        void Progress(int Percent, AnsiString Msg)
        {
            if (OnProgress != null)
            {
                OnProgress(null, Percent, Msg);
            }
        }

        Progress(10, (AnsiString)"Analyzing Image..");
        var call__9 = ClassID;
        var call__10 = ImageLib.FindLibraryImageClass(call__9);
        ImageClass = call__10;
        if (GrabInfo.FindRectangles)
        {
            var call__11 = GFXE.GFXE.GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
            Rects = call__11;
        }
        else
        {
            Rects = new TRectangleList();
            var call__12 = new TRectangle(0, 0, SD.Width, SD.Height);
            Rects.Add(call__12);
        }

        if (Rects != null && ImageClass != null)
        {
            {
                I = 0;
                int end__13 = Rects.Count - 1;
                for (; I <= end__13; I += 1)
                {
                    Msg = (AnsiString)"Grabbing Image";
                    if (Rects.Count > 1)
                    {
                        Progress(Rtl.Rtl.Round((double)(I * 90) / (Rects.Count - 1)) + 10, (AnsiString)"Grabbing Image " + Rtl.Rtl.IntToStr(I + 1));
                    }
                    else
                    {
                        Progress(100, (AnsiString)"Grabbing Image");
                    }

                    Image = ImageClass.Create(null);
                    GrabInfo.FileIndex = I;
                    var call__14 = Rects.Rectangles[I];
                    var call__15 = Image.GrabImage(SD, call__14.Rect, GrabInfo);
                    if (call__15)
                    {
                        ImageLibrary.Add(Image);
                    }
                    else
                    {
                        Image.Free();
                    }
                }
            }

            Result = 0;
        }
        else
        {
            Result = GFXE.GFXE.GFXE_ERROR;
        }

        Rects.Free();
        return Result;
    }
}

public sealed class GrabInfoClass(AnsiString name, Func<TGrabInfo> create)
{
    public AnsiString Name { get; } = name;
    public TGrabInfo Create() => create();
}
