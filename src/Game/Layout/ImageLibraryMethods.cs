// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.Layout;

public static class ImageLibraryMethods
{
    public static ImageLibrary CheckImageLibrary(this ImageLibrary library, int index, int count, Component sender)
    {
        ImageLibrary result = library;
        if (index + count > library.Count)
        {
            result = null;
            _ = MessageBox(0, $"{library.Name} must have {IntToStr(index + count)} Images", $"Error - {sender.Name}", 0);
        }
        else
        {
            for (int i = index; i <= index + count - 1; i++)
            {
                if (!library.ImageExist(i))
                {
                    _ = MessageBox(0, $"{library.Name} Image # {IntToStr(i)} is invalid", $"Error - {sender.Name}", 0);
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
        clipRight = area.Left + area.Width > lockedSurface.ClipRectangle.Right ? lockedSurface.ClipRectangle.Right : area.Left + area.Width;

        clipBottom = area.Top + area.Height > lockedSurface.ClipRectangle.Bottom ? lockedSurface.ClipRectangle.Bottom : area.Top + area.Height;

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

        for (int j = 0, last = area.Height / library[index].Height; j <= last; j++)
        {
            for (int i = 0, lastI = area.Width / library[index].Width; i <= lastI; i++)
            {
                LibraryImage item = library[index];
                LibraryImage item2 = library[index];
                library[index].ShowClipped(area.Left + (item2.Width * i), area.Top + (item.Height * j), lockedSurface);
            }
        }

        lockedSurface.ClipRectangle = oldClip;
    }
}
