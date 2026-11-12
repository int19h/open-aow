// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Layout;

public static class ImageLibraryMethods
{
    public static ImageLibrary CheckImageLibrary(this ImageLibrary library, int index, int count, Component sender)
    {
        ImageLibrary result = library;
        if (index + count > library.Count)
        {
            result = null;
            MessageBox(0, library.Name + " must have " + IntToStr(index + count) + " Images", "Error - " + sender.Name, 0);
        }
        else
        {
            for (int I = index; I <= index + count - 1; I++)
            {
                if (!library.ImageExist(I))
                {
                    MessageBox(0, library.Name + " Image # " + IntToStr(I) + " is invalid", "Error - " + sender.Name, 0);
                    result = null;
                }
            }
        }

        return result;
    }

    public static void DrawTiled(this ImageLibrary library, int index, in SizedRectangle area, ref LockedSurface lockedSurface)
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

        for (int J = 0, last = area.Height / library[index].Height; J <= last; J++)
        {
            for (int I = 0, lastI = area.Width / library[index].Width; I <= lastI; I++)
            {
                var item = library[index];
                var item2 = library[index];
                library[index].ShowClipped(area.Left + item2.Width * I, area.Top + item.Height * J, lockedSurface);
            }
        }

        lockedSurface.ClipRectangle = oldClip;
    }
}
