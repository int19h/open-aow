// Translated from AOWInterfaceImgLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// { The AOWInterfaceImgLib unit of aowInt.dpl: TInterfaceIL, the image library that holds the
//   pieces the controls are drawn from (frames, textures, lines, icons). Its Draw* methods draw a
//   frame, a texture tile, a single image or a horizontal or vertical line of images into a
//   rectangle given as (Left, Top, Width, Height) in a TRect, clipped to the surface's clip rectangle,
//   which they narrow while drawing and restore. }
// no fields or virtual methods of its own
public partial class InterfaceImageLibrary : ImageLibrary
{
    public InterfaceImageLibrary(Component owner) : base(owner)
    { }

    // AOWInterfaceImgLib.pas:34
    // { CheckIL returns Self when the library holds the Count images from Index on, else nil after a
    //   message box that names the library and the component (Sender) that needs them }
    public InterfaceImageLibrary CheckImageLibrary(int index, int count, Component sender)
    {
        InterfaceImageLibrary result = this;
        if (index + count > this.Count)
        {
            result = null;
            MessageBox(0, Name + " must have " + IntToStr(index + count) + " Images", "Error - " + sender.Name, 0);
        }
        else
        {
            for (int I = index; I <= index + count - 1; I++)
            {
                if (!ImageExist(I))
                {
                    MessageBox(0, Name + " Image # " + IntToStr(I) + " is invalid", "Error - " + sender.Name, 0);
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
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (area.Left + area.Width > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = area.Left + area.Width;
        }

        if (area.Top + area.Height > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = area.Top + area.Height;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        var libraryImage = this[index + 5];
        this[index + 5].ShowClipped(area.Left, area.Top + area.Height - libraryImage.Height, lockedSurface);
        var item = this[index + 4];
        if (area.Left + area.Width - item.Width < oldClip.Right)
        {
            var item3 = this[index + 4];
            lockedSurface.ClipRectangle.Right = area.Left + area.Width - item3.Width;
        }

        for (int I = 0, last = (area.Width - this[index].Width - this[index + 2].Width) / this[index + 1].Width; I <= last; I++)
        {
            var item3 = this[index];
            var item4 = this[index + 1];
            this[index + 1].ShowClipped(area.Left + item3.Width + item4.Width * I, area.Top, lockedSurface);
            var item5 = this[index + 5];
            var item6 = this[index + 6];
            var item7 = this[index + 6];
            this[index + 6].ShowClipped(area.Left + item5.Width + item6.Width * I, area.Top + area.Height - item7.Height, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item8 = this[index + 6];
        if (area.Top + area.Height - item8.Height < oldClip.Bottom)
        {
            var item3 = this[index + 6];
            lockedSurface.ClipRectangle.Bottom = area.Top + area.Height - item3.Height;
        }

        for (int I = 0, last = (area.Height - this[index].Height - this[index + 5].Height) / this[index + 3].Height; I <= last; I++)
        {
            var item3 = this[index];
            var item4 = this[index + 3];
            this[index + 3].ShowClipped(area.Left, area.Top + item3.Height + item4.Height * I, lockedSurface);
            var item5 = this[index + 2];
            var item6 = this[index + 4];
            var item7 = this[index + 4];
            this[index + 4].ShowClipped(area.Left + area.Width - item7.Width, area.Top + item5.Height + item6.Height * I, lockedSurface);
        }

        var rectangle3 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle3;
        var item9 = this[index + 2];
        this[index + 2].ShowClipped(area.Left + area.Width - item9.Width, area.Top, lockedSurface);
        var item10 = this[index + 7];
        var item11 = this[index + 7];
        this[index + 7].ShowClipped(area.Left + area.Width - item11.Width, area.Top + area.Height - item10.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:105
    // { DrawILT tiles image Index over R }
    public void DrawTiled(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (area.Left + area.Width > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = area.Left + area.Width;
        }

        if (area.Top + area.Height > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = area.Top + area.Height;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        if (area.Left + area.Width < oldClip.Right)
        {
            lockedSurface.ClipRectangle.Right = area.Left + area.Width;
        }

        if (area.Top + area.Height < oldClip.Bottom)
        {
            lockedSurface.ClipRectangle.Bottom = area.Top + area.Height;
        }

        for (int J = 0, last = area.Height / this[index].Height; J <= last; J++)
        {
            for (int I = 0, lastI = area.Width / this[index].Width; I <= lastI; I++)
            {
                var libraryImage = this[index];
                var item = this[index];
                this[index].ShowClipped(area.Left + item.Width * I, area.Top + libraryImage.Height * J, lockedSurface);
            }
        }

        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:133
    // { DrawILI draws image Index at the top left corner of R, clipped to R }
    public void DrawImage(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (area.Left + area.Width > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = area.Left + area.Width;
        }

        if (area.Top + area.Height > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = area.Top + area.Height;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:154
    // { DrawILLh draws a horizontal line of 3 images: Index the left end, +1 the middle (repeated),
    //   +2 the right end }
    public void DrawHorizontalStrip(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (area.Left + area.Width > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = area.Left + area.Width;
        }

        if (area.Top + area.Height > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = area.Top + area.Height;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        var libraryImage = this[index + 2];
        if (area.Left + area.Width - libraryImage.Width < oldClip.Right)
        {
            var item = this[index + 2];
            lockedSurface.ClipRectangle.Right = area.Left + area.Width - item.Width;
        }

        for (int I = 0, last = (area.Width - this[index].Width - this[index + 2].Width) / this[index + 1].Width; I <= last; I++)
        {
            var item2 = this[index];
            var item3 = this[index + 1];
            this[index + 1].ShowClipped(area.Left + item2.Width + item3.Width * I, area.Top, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item4 = this[index + 2];
        this[index + 2].ShowClipped(area.Left + area.Width - item4.Width, area.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:184
    // { DrawILLv draws a vertical line of 3 images: Index the top end, +1 the middle (repeated), +2
    //   the bottom end }
    public void DrawVerticalStrip(int index, in SizedRectangle area, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (area.Left + area.Width > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = area.Left + area.Width;
        }

        if (area.Top + area.Height > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = area.Top + area.Height;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(area.Left, area.Top, lockedSurface);
        var libraryImage = this[index + 2];
        if (area.Top + area.Height - libraryImage.Height < oldClip.Bottom)
        {
            var item = this[index + 2];
            lockedSurface.ClipRectangle.Bottom = area.Top + area.Height - item.Height;
        }

        for (int I = 0, last = (area.Height - this[index].Height - this[index + 2].Height) / this[index + 1].Height; I <= last; I++)
        {
            var item2 = this[index];
            var item3 = this[index + 1];
            this[index + 1].ShowClipped(area.Left, area.Top + item2.Height + item3.Height * I, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item4 = this[index + 2];
        this[index + 2].ShowClipped(area.Left, area.Top + area.Height - item4.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }
}
