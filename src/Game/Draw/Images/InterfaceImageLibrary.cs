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
    public void DrawFrame(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (R.Left + R.Right > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = R.Left + R.Right;
        }

        if (R.Top + R.Bottom > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = R.Top + R.Bottom;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(R.Left, R.Top, lockedSurface);
        var libraryImage = this[index + 5];
        this[index + 5].ShowClipped(R.Left, R.Top + R.Bottom - libraryImage.Height, lockedSurface);
        var item = this[index + 4];
        if (R.Left + R.Right - item.Width < oldClip.Right)
        {
            var item3 = this[index + 4];
            lockedSurface.ClipRectangle.Right = R.Left + R.Right - item3.Width;
        }

        for (int I = 0, last = (R.Right - this[index].Width - this[index + 2].Width) / this[index + 1].Width; I <= last; I++)
        {
            var item3 = this[index];
            var item4 = this[index + 1];
            this[index + 1].ShowClipped(R.Left + item3.Width + item4.Width * I, R.Top, lockedSurface);
            var item5 = this[index + 5];
            var item6 = this[index + 6];
            var item7 = this[index + 6];
            this[index + 6].ShowClipped(R.Left + item5.Width + item6.Width * I, R.Top + R.Bottom - item7.Height, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item8 = this[index + 6];
        if (R.Top + R.Bottom - item8.Height < oldClip.Bottom)
        {
            var item3 = this[index + 6];
            lockedSurface.ClipRectangle.Bottom = R.Top + R.Bottom - item3.Height;
        }

        for (int I = 0, last = (R.Bottom - this[index].Height - this[index + 5].Height) / this[index + 3].Height; I <= last; I++)
        {
            var item3 = this[index];
            var item4 = this[index + 3];
            this[index + 3].ShowClipped(R.Left, R.Top + item3.Height + item4.Height * I, lockedSurface);
            var item5 = this[index + 2];
            var item6 = this[index + 4];
            var item7 = this[index + 4];
            this[index + 4].ShowClipped(R.Left + R.Right - item7.Width, R.Top + item5.Height + item6.Height * I, lockedSurface);
        }

        var rectangle3 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle3;
        var item9 = this[index + 2];
        this[index + 2].ShowClipped(R.Left + R.Right - item9.Width, R.Top, lockedSurface);
        var item10 = this[index + 7];
        var item11 = this[index + 7];
        this[index + 7].ShowClipped(R.Left + R.Right - item11.Width, R.Top + R.Bottom - item10.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:105
    // { DrawILT tiles image Index over R }
    public void DrawTiled(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (R.Left + R.Right > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = R.Left + R.Right;
        }

        if (R.Top + R.Bottom > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = R.Top + R.Bottom;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        if (R.Left + R.Right < oldClip.Right)
        {
            lockedSurface.ClipRectangle.Right = R.Left + R.Right;
        }

        if (R.Top + R.Bottom < oldClip.Bottom)
        {
            lockedSurface.ClipRectangle.Bottom = R.Top + R.Bottom;
        }

        for (int J = 0, last = R.Bottom / this[index].Height; J <= last; J++)
        {
            for (int I = 0, lastI = R.Right / this[index].Width; I <= lastI; I++)
            {
                var libraryImage = this[index];
                var item = this[index];
                this[index].ShowClipped(R.Left + item.Width * I, R.Top + libraryImage.Height * J, lockedSurface);
            }
        }

        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:133
    // { DrawILI draws image Index at the top left corner of R, clipped to R }
    public void DrawImage(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (R.Left + R.Right > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = R.Left + R.Right;
        }

        if (R.Top + R.Bottom > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = R.Top + R.Bottom;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(R.Left, R.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:154
    // { DrawILLh draws a horizontal line of 3 images: Index the left end, +1 the middle (repeated),
    //   +2 the right end }
    public void DrawHorizontalStrip(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (R.Left + R.Right > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = R.Left + R.Right;
        }

        if (R.Top + R.Bottom > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = R.Top + R.Bottom;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(R.Left, R.Top, lockedSurface);
        var libraryImage = this[index + 2];
        if (R.Left + R.Right - libraryImage.Width < oldClip.Right)
        {
            var item = this[index + 2];
            lockedSurface.ClipRectangle.Right = R.Left + R.Right - item.Width;
        }

        for (int I = 0, last = (R.Right - this[index].Width - this[index + 2].Width) / this[index + 1].Width; I <= last; I++)
        {
            var item2 = this[index];
            var item3 = this[index + 1];
            this[index + 1].ShowClipped(R.Left + item2.Width + item3.Width * I, R.Top, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item4 = this[index + 2];
        this[index + 2].ShowClipped(R.Left + R.Right - item4.Width, R.Top, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }

    // AOWInterfaceImgLib.pas:184
    // { DrawILLv draws a vertical line of 3 images: Index the top end, +1 the middle (repeated), +2
    //   the bottom end }
    public void DrawVerticalStrip(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        int clipRight;
        int clipBottom;
        Rect oldClip;
        oldClip = lockedSurface.ClipRectangle;
        if (R.Left + R.Right > lockedSurface.ClipRectangle.Right)
        {
            clipRight = lockedSurface.ClipRectangle.Right;
        }
        else
        {
            clipRight = R.Left + R.Right;
        }

        if (R.Top + R.Bottom > lockedSurface.ClipRectangle.Bottom)
        {
            clipBottom = lockedSurface.ClipRectangle.Bottom;
        }
        else
        {
            clipBottom = R.Top + R.Bottom;
        }

        var rectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle;
        this[index].ShowClipped(R.Left, R.Top, lockedSurface);
        var libraryImage = this[index + 2];
        if (R.Top + R.Bottom - libraryImage.Height < oldClip.Bottom)
        {
            var item = this[index + 2];
            lockedSurface.ClipRectangle.Bottom = R.Top + R.Bottom - item.Height;
        }

        for (int I = 0, last = (R.Bottom - this[index].Height - this[index + 2].Height) / this[index + 1].Height; I <= last; I++)
        {
            var item2 = this[index];
            var item3 = this[index + 1];
            this[index + 1].ShowClipped(R.Left, R.Top + item2.Height + item3.Height * I, lockedSurface);
        }

        var rectangle2 = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, clipRight, clipBottom);
        lockedSurface.ClipRectangle = rectangle2;
        var item4 = this[index + 2];
        this[index + 2].ShowClipped(R.Left, R.Top + R.Bottom - item4.Height, lockedSurface);
        lockedSurface.ClipRectangle = oldClip;
    }
}
