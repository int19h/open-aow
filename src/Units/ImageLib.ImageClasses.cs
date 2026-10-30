using Aow.Rtl;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Aow.Game")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Aow.Rtl.Tests")]

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
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, LibraryImageClass> entries = new();
    private readonly Func<TCustomImageLibrary, TLibraryImage> create;

    private LibraryImageClass(Type type, int classID, byte classVersion, AnsiString name, Func<TCustomImageLibrary, TLibraryImage> create)
    {
        Type = type;
        ClassID = classID;
        ClassVersion = classVersion;
        Name = name;
        this.create = create;
        if (!entries.TryAdd(type, this))
            throw new InvalidOperationException($"Image class {type} is already registered.");
    }

    public Type Type { get; }
    public string ClassName => TObject.ClassNameOf(Type);
    public int ClassID { get; }
    public byte ClassVersion { get; }
    public AnsiString Name { get; }
    public TLibraryImage Create(TCustomImageLibrary owner) => create(owner);
    public static LibraryImageClass Of<T>() where T : TLibraryImage, ILibraryImageClass => Holder<T>.Value;
    public static LibraryImageClass Of(Type type) => entries.TryGetValue(type, out var value) ? value
        : throw new InvalidOperationException($"Image class {type} is not registered.");

    private static class Holder<T> where T : TLibraryImage, ILibraryImageClass
    {
        public static readonly LibraryImageClass Value = new(typeof(T), T.ClassID, T.ClassVersion, T.Name, T.Create);
    }

    public int GrabImages(in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TGrabInfo GrabInfo, Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary, Aow.Units.ImageLib.TGrabProgressEvent OnProgress)
    {
        int Result = default;
        LibraryImageClass ImageClass = default;
        Aow.Units.ImageLib.TLibraryImage Image = default;
        Aow.Rtl.AnsiString Msg = default;
        Aow.Units.GFXE.TRectangleList Rects = default;
        int I = default;
        void Progress(int Percent, Aow.Rtl.AnsiString Msg)
        {
            if (OnProgress != null)
            {
                OnProgress(null, Percent, Msg);
            }
        }

        Progress(10, (AnsiString)"Analyzing Image..");
        var call__9 = ClassID;
        var call__10 = Aow.Units.ImageLib.ImageLib.FindLibraryImageClass(call__9);
        ImageClass = call__10;
        if (GrabInfo.FindRectangles)
        {
            var call__11 = Aow.Units.GFXE.GFXE.GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
            Rects = call__11;
        }
        else
        {
            Rects = new Aow.Units.GFXE.TRectangleList();
            var call__12 = new Aow.Units.GFXE.TRectangle(0, 0, SD.Width, SD.Height);
            Rects.Add(call__12);
        }

        if ((Rects != null) && (ImageClass != null))
        {
            {
                I = 0;
                int end__13 = Rects.Count - 1;
                for (; I <= end__13; I += 1)
                {
                    Msg = (AnsiString)"Grabbing Image";
                    if (Rects.Count > 1)
                    {
                        Progress(Aow.Rtl.Rtl.Round(((double)(I * 90)) / (Rects.Count - 1)) + 10, ((AnsiString)"Grabbing Image ") + Aow.Rtl.Rtl.IntToStr(I + 1));
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
            Result = Aow.Units.GFXE.GFXE.GFXE_ERROR;
        }

        Rects.Free();
        return Result;
    }
}

public interface IGrabInfoClass
{
    static abstract TGrabInfo Create();
}

public sealed class GrabInfoClass
{
    private readonly Func<TGrabInfo> create;
    private GrabInfoClass(Type type, Func<TGrabInfo> create) { Type = type; this.create = create; }
    public Type Type { get; }
    public TGrabInfo Create() => create();
    public static GrabInfoClass Of<T>() where T : TGrabInfo, IGrabInfoClass => Holder<T>.Value;
    private static class Holder<T> where T : TGrabInfo, IGrabInfoClass
    {
        public static readonly GrabInfoClass Value = new(typeof(T), T.Create);
    }
}
