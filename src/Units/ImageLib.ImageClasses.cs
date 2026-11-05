using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.ImageLib;

public interface ILibraryImageClass
{
    static abstract int ClassID { get; }
    static abstract byte ClassVersion { get; }
    static abstract string Name { get; }
    static abstract TLibraryImage Create(TCustomImageLibrary owner);
}

public sealed class LibraryImageClass
{
    private static readonly Catalog<Type, LibraryImageClass> catalog = new("library image class");
    private readonly Func<TCustomImageLibrary, TLibraryImage> create;

    private LibraryImageClass(Type type, int classID, byte classVersion, string name, Func<TCustomImageLibrary, TLibraryImage> create)
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
    public string Name { get; } = "";
    public TLibraryImage Create(TCustomImageLibrary owner) => create(owner);
    public static LibraryImageClass Register<T>() where T : TLibraryImage, ILibraryImageClass =>
        catalog.Add(typeof(T), new LibraryImageClass(typeof(T), T.ClassID, T.ClassVersion, T.Name, T.Create));
    public static LibraryImageClass Of<T>() where T : TLibraryImage, ILibraryImageClass => catalog[typeof(T)];
    public static LibraryImageClass Of(Type type) => catalog[type];
    public static IReadOnlyList<LibraryImageClass> All => catalog.All;

    public int GrabImages(in TSurfaceDesc SD, TGrabInfo GrabInfo, TCustomImageLibrary ImageLibrary, TGrabProgressEvent OnProgress)
    {
        int Result = default;
        LibraryImageClass ImageClass;
        TLibraryImage Image = default;
        TRectangleList Rects = default;
        void Progress(int Percent, string Msg)
        {
            if (OnProgress != null)
            {
                OnProgress(null, Percent, Msg);
            }
        }

        Progress(10, "Analyzing Image..");
        ImageClass = ImageLib.FindLibraryImageClass(ClassID);
        if (GrabInfo.FindRectangles)
        {
            Rects = GFXE.GFXE.GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
        }
        else
        {
            Rects = new TRectangleList();
            Rects.Add(new TRectangle(0, 0, SD.Width, SD.Height));
        }

        if (Rects != null && ImageClass != null)
        {
            for (int I = 0, last = Rects.Count - 1; I <= last; I++)
            {
                if (Rects.Count > 1)
                {
                    Progress(Rtl.Rtl.Round((double)(I * 90) / (Rects.Count - 1)) + 10, "Grabbing Image " + Rtl.Rtl.IntToStr(I + 1));
                }
                else
                {
                    Progress(100, "Grabbing Image");
                }

                Image = ImageClass.Create(null);
                GrabInfo.FileIndex = I;
                var item = Rects.Rectangles[I];
                if (Image.GrabImage(SD, item.Rect, GrabInfo))
                {
                    ImageLibrary.Add(Image);
                }
                else
                {
                    Image.Free();
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

public sealed class GrabInfoClass(string name, Func<TGrabInfo> create)
{
    public string Name { get; } = name;
    public TGrabInfo Create() => create();
}
