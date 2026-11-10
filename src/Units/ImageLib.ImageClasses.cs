using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

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
    public string ClassName => NameTable.Stored(Type);
    public int ClassID { get; }
    public byte ClassVersion { get; }
    public string Name { get; } = "";

    public TLibraryImage Create(TCustomImageLibrary owner) => create(owner);

    public static LibraryImageClass Register<T>() where T : TLibraryImage, ILibraryImageClass =>
        catalog.Add(typeof(T), new LibraryImageClass(typeof(T), T.ClassID, T.ClassVersion, T.Name, T.Create));

    public static LibraryImageClass Of<T>() where T : TLibraryImage, ILibraryImageClass => catalog[typeof(T)];
    public static LibraryImageClass Of(Type type) => catalog[type];

    public static IReadOnlyList<LibraryImageClass> All => catalog.All;

    public int GrabImages(in Aow.Game.Draw.TSurfaceDesc SD, TGrabInfo GrabInfo, TCustomImageLibrary ImageLibrary, TGrabProgressEvent OnProgress)
    {
        int Result = default;
        LibraryImageClass ImageClass;
        TLibraryImage Image = default;
        Aow.Game.Draw.TRectangleList Rects = default;
        void Progress(int Percent, string Msg)
        {
            if (OnProgress != null)
            {
                OnProgress(null, Percent, Msg);
            }
        }

        Progress(10, "Analyzing Image..");
        ImageClass = ImageLibUnit.FindLibraryImageClass(ClassID);
        if (GrabInfo.FindRectangles)
        {
            Rects = Aow.Game.Draw.GFXEUnit.GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
        }
        else
        {
            Rects = new Aow.Game.Draw.TRectangleList();
            Rects.Add(new Aow.Game.Draw.TRectangle(0, 0, SD.Width, SD.Height));
        }

        if (Rects != null && ImageClass != null)
        {
            for (int I = 0, last = Rects.Count - 1; I <= last; I++)
            {
                if (Rects.Count > 1)
                {
                    Progress(RtlUnit.Round((double)(I * 90) / (Rects.Count - 1)) + 10, "Grabbing Image " + RtlUnit.IntToStr(I + 1));
                }
                else
                {
                    Progress(100, "Grabbing Image");
                }

                Image = ImageClass.Create(null);
                GrabInfo.FileIndex = I;
                var rectangle = Rects.Rectangles[I];
                if (Image.GrabImage(SD, rectangle.Rect, GrabInfo))
                {
                    ImageLibrary.Add(Image);
                }
                else
                {
                    Image?.Dispose();
                }
            }

            Result = 0;
        }
        else
        {
            Result = Aow.Game.Draw.GFXEUnit.GFXE_ERROR;
        }

        Rects?.Dispose();
        return Result;
    }
}

public sealed class GrabInfoClass(string name, Func<TGrabInfo> create)
{
    public string Name { get; } = name;

    public TGrabInfo Create() => create();
}
