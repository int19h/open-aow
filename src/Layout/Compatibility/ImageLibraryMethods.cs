// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Layout
{
    public static class ImageLibraryMethods
    {
        public static Aow.Units.ImageLib.TImageLibrary CheckIL(this Aow.Units.ImageLib.TImageLibrary library, int Index, int Count, Aow.Rtl.TComponent Sender)
        {
            Aow.Units.ImageLib.TImageLibrary Result = default;
            int I = default;
            Result = library;
            if ((Index + Count) > library.Count)
            {
                Result = null;
                Aow.Rtl.Rtl.MessageBox((int)0, ((library.Name + ((AnsiString)" must have ")) + Aow.Rtl.Rtl.IntToStr(Index + Count)) + ((AnsiString)" Images"), ((AnsiString)"Error - ") + Sender.Name, (int)0);
            }
            else
            {
                {
                    I = Index;
                    int end__1 = (Index + Count) - 1;
                    for (; I <= end__1; I += 1)
                    {
                        if (!library.ImageExist(I))
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, ((library.Name + AnsiString.FromBytes(32, 73, 109, 97, 103, 101, 32, 35, 32)) + Aow.Rtl.Rtl.IntToStr(I)) + ((AnsiString)" is invalid"), ((AnsiString)"Error - ") + Sender.Name, (int)0);
                            Result = null;
                        }
                    }
                }
            }

            return Result;
        }

        public static void DrawILT(this Aow.Units.ImageLib.TImageLibrary library, int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            int J = default;
            int ClipRight = default;
            int ClipBottom = default;
            Aow.Rtl.TRect OldClip = default;
            OldClip = SurfDesc.ClipRect;
            if ((R.Left + R.Right) > SurfDesc.ClipRect.Right)
            {
                ClipRight = SurfDesc.ClipRect.Right;
            }
            else
            {
                ClipRight = R.Left + R.Right;
            }

            if ((R.Top + R.Bottom) > SurfDesc.ClipRect.Bottom)
            {
                ClipBottom = SurfDesc.ClipRect.Bottom;
            }
            else
            {
                ClipBottom = R.Top + R.Bottom;
            }

            var call__33 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__33;
            if ((R.Left + R.Right) < OldClip.Right)
            {
                SurfDesc.ClipRect.Right = R.Left + R.Right;
            }

            if ((R.Top + R.Bottom) < OldClip.Bottom)
            {
                SurfDesc.ClipRect.Bottom = R.Top + R.Bottom;
            }

            {
                J = 0;
                int end__34 = R.Bottom / library[Index].Height;
                for (; J <= end__34; J += 1)
                {
                    {
                        I = 0;
                        int end__35 = R.Right / library[Index].Width;
                        for (; I <= end__35; I += 1)
                        {
                            var call__36 = library[Index];
                            var call__37 = library[Index];
                            var call__38 = library[Index];
                            call__38.ShowClipped(R.Left + (call__37.Width * I), R.Top + (call__36.Height * J), SurfDesc);
                        }
                    }
                }
            }

            SurfDesc.ClipRect = OldClip;
        }
    }
}
