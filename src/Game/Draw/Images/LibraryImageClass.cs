
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public sealed class LibraryImageClass
{
    private static readonly Catalog<Type, LibraryImageClass> s_catalog = new("library image class");
    private readonly Func<CustomImageLibrary, LibraryImage> _create;

    private LibraryImageClass(Type type, int classId, byte classVersion, string name, Func<CustomImageLibrary, LibraryImage> create)
    {
        Type = type;
        ClassId = classId;
        ClassVersion = classVersion;
        Name = name;
        this._create = create;
    }

    public Type Type { get; }
    public string StorageKey => NameTable.Stored(Type);
    public int ClassId { get; }
    public byte ClassVersion { get; }
    public string Name { get; } = "";

    public LibraryImage CreateInstance(CustomImageLibrary owner) => _create(owner);

    public static LibraryImageClass Register<T>() where T : LibraryImage, ILibraryImageClass =>
        s_catalog.Add(typeof(T), new LibraryImageClass(typeof(T), T.ClassId, T.ClassVersion, T.Name, T.CreateInstance));

    public static LibraryImageClass Of<T>() where T : LibraryImage, ILibraryImageClass => s_catalog[typeof(T)];
    public static LibraryImageClass Of(Type type) => s_catalog[type];

    public static IReadOnlyList<LibraryImageClass> All => s_catalog.All;

    public int GrabImages(in LockedSurface lockedSurface, GrabInfo grabInfo, CustomImageLibrary imageLibrary, GrabProgressEvent onProgress)
    {
        int result = default;
        LibraryImageClass imageClass;
        LibraryImage image = default;
        RectangleList rects = default;
        void Progress(int percent, string message)
        {
            if (onProgress != null)
            {
                onProgress(null, percent, message);
            }
        }

        Progress(10, "Analyzing Image..");
        imageClass = LibraryCatalog.FindLibraryImageClass(ClassId);
        if (grabInfo.FindRectangles)
        {
            rects = GetSurfaceRectangles(lockedSurface, grabInfo.TransparentColor, null);
        }
        else
        {
            rects = new RectangleList();
            rects.Add(new Rectangle(0, 0, lockedSurface.Width, lockedSurface.Height));
        }

        if (rects != null && imageClass != null)
        {
            for (int I = 0, last = rects.Count - 1; I <= last; I++)
            {
                if (rects.Count > 1)
                {
                    Progress(RtlUnit.Round((double)(I * 90) / (rects.Count - 1)) + 10, "Grabbing Image " + RtlUnit.IntToStr(I + 1));
                }
                else
                {
                    Progress(100, "Grabbing Image");
                }

                image = imageClass.CreateInstance(null);
                grabInfo.FileIndex = I;
                var rectangle = rects.Rectangles[I];
                if (image.GrabImage(lockedSurface, rectangle.Bounds, grabInfo))
                {
                    imageLibrary.Add(image);
                }
                else
                {
                    image?.Dispose();
                }
            }

            result = 0;
        }
        else
        {
            result = PixelFormats.GFXE_ERROR;
        }

        rects?.Dispose();
        return result;
    }

    internal static RectangleList GetSurfaceRectangles(LockedSurface lockedSurface, int transparentColor, ProgressEvent onProgress)
    {
        RectangleList rectangleList = new RectangleList();
        int i = 0;
        for (int number = lockedSurface.Height - 1; i <= number; i++)
        {
            ushort number2 = (ushort)i;
            int j = 0;
            for (int number3 = lockedSurface.Width - 1; j <= number3; j++)
            {
                ushort x = (ushort)j;
                if (GetPixel(x, number2) == transparentColor)
                {
                    Rectangle rectangle = FindRectangle(x, number2);
                    if (rectangle != null)
                    {
                        rectangleList.Add(rectangle);
                    }
                }
            }
            if (onProgress != null && (number2 % 20 == 0 || number2 == lockedSurface.Height - 1))
            {
                onProgress(null, RtlUnit.Round((double)(number2 * 100) / (double)(lockedSurface.Height - 1)), 0);
            }
        }
        if (rectangleList.Count == 0)
        {
            if (rectangleList != null)
            {
                rectangleList.Dispose();
            }
            return null;
        }
        return rectangleList;
        Rectangle FindRectangle(int X, int Y)
        {
            Rectangle result = null;
            int k = 1;
            int l = 1;
            for (; GetPixel(X + k, Y) == transparentColor && GetPixel(X + k, Y + 1) != transparentColor && X + k < lockedSurface.Width - 1; k++)
            {
            }
            if (GetPixel(X + k, Y) == transparentColor && GetPixel(X + k, Y + 1) == transparentColor && X + k < lockedSurface.Width - 1 && k > 1)
            {
                for (; GetPixel(X, Y + l) == transparentColor && GetPixel(X + k, Y + l) == transparentColor && GetPixel(X + 1, Y + l) != transparentColor && GetPixel(X + k - 1, Y + l) != transparentColor && Y + l < lockedSurface.Height; l++)
                {
                }
                if (GetPixel(X, Y + l) == transparentColor && GetPixel(X + 1, Y + l) == transparentColor && GetPixel(X + k, Y + l) == transparentColor && GetPixel(X + k - 1, Y + l) == transparentColor && Y + l < lockedSurface.Height && l > 1)
                {
                    bool flag = true;
                    int number4 = 1;
                    while (flag && number4 != k)
                    {
                        flag = GetPixel(X + number4, Y + l) == transparentColor;
                        number4++;
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
            SurfaceView value;
            switch (lockedSurface.BitCount)
            {
                case 8:
                    value = lockedSurface.View();
                    return value.Byte(X + lockedSurface.Pitch * Y);
                case 16:
                    value = lockedSurface.View();
                    return value.UInt16(X + X + lockedSurface.Pitch * Y);
                case 24:
                    {
                        if (X + 1 < lockedSurface.Width)
                        {
                            value = lockedSurface.View();
                            return (int)(value.Dword(X + X + X + lockedSurface.Pitch * Y) & 0xFFFFFF);
                        }
                        int number4 = X + X + X + lockedSurface.Pitch * Y;
                        value = lockedSurface.View();
                        ushort number5 = value.UInt16(number4);
                        value = lockedSurface.View();
                        return number5 + (value.Byte(number4 + 2) << 16);
                    }
                case 32:
                    value = lockedSurface.View();
                    return (int)value.Dword((X << 2) + lockedSurface.Pitch * Y);
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }
        }
    }

    public delegate void GrabProgressEvent(object Sender, int Percent, string Msg);
}
