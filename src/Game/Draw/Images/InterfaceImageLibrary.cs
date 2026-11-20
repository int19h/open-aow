#nullable disable
// Translated from AOWInterfaceImgLib.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Draw.Images;

// { The AOWInterfaceImgLib unit of aowInt.dpl: TInterfaceIL, the image library that holds the
//   pieces the controls are drawn from (frames, textures, lines, icons). Its Draw* methods draw a
//   frame, a texture tile, a single image or a horizontal or vertical line of images into a
//   SizedRectangle, clipped to the surface's clip rectangle, which they narrow while drawing and
//   restore. }
// no fields or virtual methods of its own
public partial class InterfaceImageLibrary(Component owner) : ImageLibrary(owner)
{

    // AOWInterfaceImgLib.pas:34
    // { CheckIL returns Self when the library holds the Count images from Index on, else nil after a
    //   message box that names the library and the component (Sender) that needs them }
    public InterfaceImageLibrary CheckImageLibrary(int index, int count, Component sender)
    {
        InterfaceImageLibrary result = this;
        if (index + count > Count)
        {
            result = null;
            _ = MessageBox(0, text: $"{Name} must have {IntToStr(index + count)} Images", caption: $"Error - {sender.Name}", flags: 0);
        }
        else
        {
            for (int i = index; i <= index + count - 1; i++)
            {
                if (!ImageExist(i))
                {
                    _ = MessageBox(0, text: $"{Name} Image # {IntToStr(i)} is invalid", caption: $"Error - {sender.Name}", flags: 0);
                    result = null;
                }
            }
        }

        return result;
    }

    // AOWInterfaceImgLib.pas:58
    // { DrawILF draws a frame of 8 images: Index the top left corner, +1 the top edge, +2 the top right
    //   corner, +3 the left edge, +4 the right edge, +5 the bottom left corner, +6 the bottom edge, +7
    //   the bottom right corner }
    public void DrawFrame(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        Rect oldClip = lockedSurface.ClipRectangle;
        int clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        int clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

        Rect rectangle = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        LibraryImage libraryImage = this[index + 5];
        this[index + 5].ShowClipped(area.Left, area.Top + area.Height - libraryImage.Height, lockedSurface);
        LibraryImage item = this[index + 4];
        if (area.Left + area.Width - item.Width < oldClip.Right)
        {
            LibraryImage item3 = this[index + 4];
            Rect clipRectangle = lockedSurface.ClipRectangle;
            clipRectangle.Right = area.Left + area.Width - item3.Width;
            lockedSurface.ClipRectangle = clipRectangle;
        }

        for (int i = 0, last = (area.Width - this[index].Width - this[index + 2].Width) / this[index + 1].Width; i <= last; i++)
        {
            LibraryImage item3 = this[index];
            LibraryImage item4 = this[index + 1];
            this[index + 1].ShowClipped(area.Left + item3.Width + (item4.Width * i), area.Top, lockedSurface);
            LibraryImage item5 = this[index + 5];
            LibraryImage item6 = this[index + 6];
            LibraryImage item7 = this[index + 6];
            this[index + 6].ShowClipped(area.Left + item5.Width + (item6.Width * i), area.Top + area.Height - item7.Height, lockedSurface);
        }

        Rect rectangle2 = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        LibraryImage item8 = this[index + 6];
        if (area.Top + area.Height - item8.Height < oldClip.Bottom)
        {
            LibraryImage item3 = this[index + 6];
            Rect clipRectangle2 = lockedSurface.ClipRectangle;
            clipRectangle2.Bottom = area.Top + area.Height - item3.Height;
            lockedSurface.ClipRectangle = clipRectangle2;
        }

        for (int i = 0, last = (area.Height - this[index].Height - this[index + 5].Height) / this[index + 3].Height; i <= last; i++)
        {
            LibraryImage item3 = this[index];
            LibraryImage item4 = this[index + 3];
            this[index + 3].ShowClipped(area.Left, area.Top + item3.Height + (item4.Height * i), lockedSurface);
            LibraryImage item5 = this[index + 2];
            LibraryImage item6 = this[index + 4];
            LibraryImage item7 = this[index + 4];
            this[index + 4].ShowClipped(area.Left + area.Width - item7.Width, area.Top + item5.Height + (item6.Height * i), lockedSurface);
        }

        Rect rectangle3 = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle3;
        LibraryImage item9 = this[index + 2];
        this[index + 2].ShowClipped(area.Left + area.Width - item9.Width, area.Top, lockedSurface);
        LibraryImage item10 = this[index + 7];
        LibraryImage item11 = this[index + 7];
        this[index + 7].ShowClipped(area.Left + area.Width - item11.Width, area.Top + area.Height - item10.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:105
    // { DrawILT tiles image Index over R }
    public void DrawTiled(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        Rect oldClip = lockedSurface.ClipRectangle;
        int clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        int clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

        Rect rectangle = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        if (area.Left + area.Width < oldClip.Right)
        {
            Rect clipRectangle = lockedSurface.ClipRectangle;
            clipRectangle.Right = area.Left + area.Width;
            lockedSurface.ClipRectangle = clipRectangle;
        }

        if (area.Top + area.Height < oldClip.Bottom)
        {
            Rect clipRectangle2 = lockedSurface.ClipRectangle;
            clipRectangle2.Bottom = area.Top + area.Height;
            lockedSurface.ClipRectangle = clipRectangle2;
        }

        for (int j = 0, last = area.Height / this[index].Height; j <= last; j++)
        {
            for (int i = 0, lastI = area.Width / this[index].Width; i <= lastI; i++)
            {
                LibraryImage libraryImage = this[index];
                LibraryImage item = this[index];
                this[index].ShowClipped(area.Left + (item.Width * i), area.Top + (libraryImage.Height * j), lockedSurface);
            }
        }

        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:133
    // { DrawILI draws image Index at the top left corner of R, clipped to R }
    public void DrawImage(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        Rect oldClip = lockedSurface.ClipRectangle;
        int clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        int clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

        Rect rectangle = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:154
    // { DrawILLh draws a horizontal line of 3 images: Index the left end, +1 the middle (repeated),
    //   +2 the right end }
    public void DrawHorizontalStrip(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        Rect oldClip = lockedSurface.ClipRectangle;
        int clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        int clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

        Rect rectangle = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        LibraryImage libraryImage = this[index + 2];
        if (area.Left + area.Width - libraryImage.Width < oldClip.Right)
        {
            LibraryImage item = this[index + 2];
            Rect clipRectangle = lockedSurface.ClipRectangle;
            clipRectangle.Right = area.Left + area.Width - item.Width;
            lockedSurface.ClipRectangle = clipRectangle;
        }

        for (int i = 0, last = (area.Width - this[index].Width - this[index + 2].Width) / this[index + 1].Width; i <= last; i++)
        {
            LibraryImage item2 = this[index];
            LibraryImage item3 = this[index + 1];
            this[index + 1].ShowClipped(area.Left + item2.Width + (item3.Width * i), area.Top, lockedSurface);
        }

        Rect rectangle2 = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        LibraryImage item4 = this[index + 2];
        this[index + 2].ShowClipped(area.Left + area.Width - item4.Width, area.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:184
    // { DrawILLv draws a vertical line of 3 images: Index the top end, +1 the middle (repeated), +2
    //   the bottom end }
    public void DrawVerticalStrip(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        Rect oldClip = lockedSurface.ClipRectangle;
        int clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        int clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

        Rect rectangle = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        LibraryImage libraryImage = this[index + 2];
        if (area.Top + area.Height - libraryImage.Height < oldClip.Bottom)
        {
            LibraryImage item = this[index + 2];
            Rect clipRectangle = lockedSurface.ClipRectangle;
            clipRectangle.Bottom = area.Top + area.Height - item.Height;
            lockedSurface.ClipRectangle = clipRectangle;
        }

        for (int i = 0, last = (area.Height - this[index].Height - this[index + 2].Height) / this[index + 1].Height; i <= last; i++)
        {
            LibraryImage item2 = this[index];
            LibraryImage item3 = this[index + 1];
            this[index + 1].ShowClipped(area.Left, area.Top + item2.Height + (item3.Height * i), lockedSurface);
        }

        Rect rectangle2 = new(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        LibraryImage item4 = this[index + 2];
        this[index + 2].ShowClipped(area.Left, area.Top + area.Height - item4.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }
}
