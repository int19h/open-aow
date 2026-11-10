
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public sealed class LibraryImageClass
{
    private static readonly Catalog<Type, LibraryImageClass> catalog = new("library image class");
    private readonly Func<CustomImageLibrary, LibraryImage> create;

    private LibraryImageClass(Type type, int classID, byte classVersion, string name, Func<CustomImageLibrary, LibraryImage> create)
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

    public LibraryImage Create(CustomImageLibrary owner) => create(owner);

    public static LibraryImageClass Register<T>() where T : LibraryImage, ILibraryImageClass =>
        catalog.Add(typeof(T), new LibraryImageClass(typeof(T), T.ClassID, T.ClassVersion, T.Name, T.Create));

    public static LibraryImageClass Of<T>() where T : LibraryImage, ILibraryImageClass => catalog[typeof(T)];
    public static LibraryImageClass Of(Type type) => catalog[type];

    public static IReadOnlyList<LibraryImageClass> All => catalog.All;

    public int GrabImages(in SurfaceDesc SD, GrabInfo GrabInfo, CustomImageLibrary ImageLibrary, GrabProgressEvent OnProgress)
    {
        int Result = default;
        LibraryImageClass ImageClass;
        LibraryImage Image = default;
        RectangleList Rects = default;
        void Progress(int Percent, string Msg)
        {
            if (OnProgress != null)
            {
                OnProgress(null, Percent, Msg);
            }
        }

        Progress(10, "Analyzing Image..");
        ImageClass = LibraryCatalog.FindLibraryImageClass(ClassID);
        if (GrabInfo.FindRectangles)
        {
            Rects = GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
        }
        else
        {
            Rects = new RectangleList();
            Rects.Add(new Rectangle(0, 0, SD.Width, SD.Height));
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
            Result = PixelFormats.GFXE_ERROR;
        }

        Rects?.Dispose();
        return Result;
    }

    internal static RectangleList GetSurfaceRectangles(SurfaceDesc SD, int TransparentColor, ProgressEvent OnProgress)
    {
        RectangleList tRectangleList = new RectangleList();
        int i = 0;
        for (int num = SD.Height - 1; i <= num; i++)
        {
            ushort num2 = (ushort)i;
            int j = 0;
            for (int num3 = SD.Width - 1; j <= num3; j++)
            {
                ushort x = (ushort)j;
                if (GetPixel(x, num2) == TransparentColor)
                {
                    Rectangle tRectangle = FindRectangle(x, num2);
                    if (tRectangle != null)
                    {
                        ((DelphiList)tRectangleList).Add((object)tRectangle);
                    }
                }
            }
            if (OnProgress != null && (num2 % 20 == 0 || num2 == SD.Height - 1))
            {
                OnProgress(null, RtlUnit.Round((double)(num2 * 100) / (double)(SD.Height - 1)), 0);
            }
        }
        if (((DelphiList)tRectangleList).Count == 0)
        {
            if (tRectangleList != null)
            {
                ((DelphiList)tRectangleList).Dispose();
            }
            return null;
        }
        return tRectangleList;
        Rectangle FindRectangle(int X, int Y)
        {
            Rectangle result = null;
            int k = 1;
            int l = 1;
            for (; GetPixel(X + k, Y) == TransparentColor && GetPixel(X + k, Y + 1) != TransparentColor && X + k < SD.Width - 1; k++)
            {
            }
            if (GetPixel(X + k, Y) == TransparentColor && GetPixel(X + k, Y + 1) == TransparentColor && X + k < SD.Width - 1 && k > 1)
            {
                for (; GetPixel(X, Y + l) == TransparentColor && GetPixel(X + k, Y + l) == TransparentColor && GetPixel(X + 1, Y + l) != TransparentColor && GetPixel(X + k - 1, Y + l) != TransparentColor && Y + l < SD.Height; l++)
                {
                }
                if (GetPixel(X, Y + l) == TransparentColor && GetPixel(X + 1, Y + l) == TransparentColor && GetPixel(X + k, Y + l) == TransparentColor && GetPixel(X + k - 1, Y + l) == TransparentColor && Y + l < SD.Height && l > 1)
                {
                    bool flag = true;
                    int num4 = 1;
                    while (flag && num4 != k)
                    {
                        flag = GetPixel(X + num4, Y + l) == TransparentColor;
                        num4++;
                    }
                    if (flag)
                    {
                        result = new Rectangle(X + 1, Y + 1, X + k, Y + l);
                    }
                }
            }
            return result;
        }
        int GetPixel(int X, int Y)
        {
            SurfaceView val;
            switch (SD.BitCount)
            {
                case 8:
                    val = SD.View();
                    return val.Byte(X + SD.Pitch * Y);
                case 16:
                    val = SD.View();
                    return val.Word(X + X + SD.Pitch * Y);
                case 24:
                    {
                        if (X + 1 < SD.Width)
                        {
                            val = SD.View();
                            return (int)(val.Dword(X + X + X + SD.Pitch * Y) & 0xFFFFFF);
                        }
                        int num4 = X + X + X + SD.Pitch * Y;
                        val = SD.View();
                        ushort num5 = val.Word(num4);
                        val = SD.View();
                        return num5 + (val.Byte(num4 + 2) << 16);
                    }
                case 32:
                    val = SD.View();
                    return (int)val.Dword((X << 2) + SD.Pitch * Y);
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }
        }
    }

    public delegate void GrabProgressEvent(object Sender, int Percent, string Msg);
}
