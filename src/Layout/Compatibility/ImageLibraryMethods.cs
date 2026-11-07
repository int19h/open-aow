// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Layout
{
    public static class ImageLibraryMethods
    {
        public static TImageLibrary CheckIL(this TImageLibrary library, int Index, int Count, TComponent Sender)
        {
            TImageLibrary Result = library;
            if (Index + Count > library.Count)
            {
                Result = null;
                MessageBox(0, library.Name + " must have " + IntToStr(Index + Count) + " Images", "Error - " + Sender.Name, 0);
            }
            else
            {
                for (int I = Index; I <= Index + Count - 1; I++)
                {
                    if (!library.ImageExist(I))
                    {
                        MessageBox(0, library.Name + " Image # " + IntToStr(I) + " is invalid", "Error - " + Sender.Name, 0);
                        Result = null;
                    }
                }
            }

            return Result;
        }

        public static void DrawILT(this TImageLibrary library, int Index, in TRect R, ref TSurfaceDesc SurfDesc)
        {
            int ClipRight;
            int ClipBottom;
            TRect OldClip;
            OldClip = SurfDesc.ClipRect;
            if (R.Left + R.Right > SurfDesc.ClipRect.Right)
            {
                ClipRight = SurfDesc.ClipRect.Right;
            }
            else
            {
                ClipRight = R.Left + R.Right;
            }

            if (R.Top + R.Bottom > SurfDesc.ClipRect.Bottom)
            {
                ClipBottom = SurfDesc.ClipRect.Bottom;
            }
            else
            {
                ClipBottom = R.Top + R.Bottom;
            }

            var rect = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = rect;
            if (R.Left + R.Right < OldClip.Right)
            {
                SurfDesc.ClipRect.Right = R.Left + R.Right;
            }

            if (R.Top + R.Bottom < OldClip.Bottom)
            {
                SurfDesc.ClipRect.Bottom = R.Top + R.Bottom;
            }

            for (int J = 0, last = R.Bottom / library[Index].Height; J <= last; J++)
            {
                for (int I = 0, lastI = R.Right / library[Index].Width; I <= lastI; I++)
                {
                    var item = library[Index];
                    var item2 = library[Index];
                    library[Index].ShowClipped(R.Left + item2.Width * I, R.Top + item.Height * J, SurfDesc);
                }
            }

            SurfDesc.ClipRect = OldClip;
        }
    }
}
