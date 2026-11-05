// Translated from AOWInterfaceImgLib.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;

namespace Aow.Units.AOWInterfaceImgLib
{
    // { The AOWInterfaceImgLib unit of aowInt.dpl: TInterfaceIL, the image library that holds the
//   pieces the controls are drawn from (frames, textures, lines, icons). Its Draw* methods draw a
//   frame, a texture tile, a single image or a horizontal or vertical line of images into a
//   rectangle given as (Left, Top, Width, Height) in a TRect, clipped to the surface's clip rectangle,
//   which they narrow while drawing and restore. }
    // no fields or virtual methods of its own
    public partial class TInterfaceIL : TImageLibrary
    {
        public TInterfaceIL(TComponent owner) : base(owner)
        { }

        // AOWInterfaceImgLib.pas:34
        // { CheckIL returns Self when the library holds the Count images from Index on, else nil after a
//   message box that names the library and the component (Sender) that needs them }
        public TInterfaceIL CheckIL(int Index, int Count, TComponent Sender)
        {
            TInterfaceIL Result = this;
            if (Index + Count > this.Count)
            {
                Result = null;
                MessageBox(0, Name + " must have " + IntToStr(Index + Count) + " Images", "Error - " + Sender.Name, 0);
            }
            else
            {
                for (int I = Index; I <= Index + Count - 1; I++)
                {
                    if (!ImageExist(I))
                    {
                        MessageBox(0, Name + " Image # " + IntToStr(I) + " is invalid", "Error - " + Sender.Name, 0);
                        Result = null;
                    }
                }
            }

            return Result;
        }

        // AOWInterfaceImgLib.pas:58
        // { DrawILF draws a frame of 8 images: Index the top left corner, +1 the top edge, +2 the top right
//   corner, +3 the left edge, +4 the right edge, +5 the bottom left corner, +6 the bottom edge, +7
//   the bottom right corner }
        public void DrawILF(int Index, in TRect R, ref TSurfaceDesc SurfDesc)
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
            this[Index].ShowClipped(R.Left, R.Top, SurfDesc);
            var item = this[Index + 5];
            this[Index + 5].ShowClipped(R.Left, R.Top + R.Bottom - item.Height, SurfDesc);
            var item2 = this[Index + 4];
            if (R.Left + R.Right - item2.Width < OldClip.Right)
            {
                var item3 = this[Index + 4];
                SurfDesc.ClipRect.Right = R.Left + R.Right - item3.Width;
            }

            for (int I = 0, last = (R.Right - this[Index].Width - this[Index + 2].Width) / this[Index + 1].Width; I <= last; I++)
            {
                var item3 = this[Index];
                var item4 = this[Index + 1];
                this[Index + 1].ShowClipped(R.Left + item3.Width + item4.Width * I, R.Top, SurfDesc);
                var item5 = this[Index + 5];
                var item6 = this[Index + 6];
                var item7 = this[Index + 6];
                this[Index + 6].ShowClipped(R.Left + item5.Width + item6.Width * I, R.Top + R.Bottom - item7.Height, SurfDesc);
            }

            var rect2 = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = rect2;
            var item8 = this[Index + 6];
            if (R.Top + R.Bottom - item8.Height < OldClip.Bottom)
            {
                var item3 = this[Index + 6];
                SurfDesc.ClipRect.Bottom = R.Top + R.Bottom - item3.Height;
            }

            for (int I = 0, last = (R.Bottom - this[Index].Height - this[Index + 5].Height) / this[Index + 3].Height; I <= last; I++)
            {
                var item3 = this[Index];
                var item4 = this[Index + 3];
                this[Index + 3].ShowClipped(R.Left, R.Top + item3.Height + item4.Height * I, SurfDesc);
                var item5 = this[Index + 2];
                var item6 = this[Index + 4];
                var item7 = this[Index + 4];
                this[Index + 4].ShowClipped(R.Left + R.Right - item7.Width, R.Top + item5.Height + item6.Height * I, SurfDesc);
            }

            var rect3 = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = rect3;
            var item9 = this[Index + 2];
            this[Index + 2].ShowClipped(R.Left + R.Right - item9.Width, R.Top, SurfDesc);
            var item10 = this[Index + 7];
            var item11 = this[Index + 7];
            this[Index + 7].ShowClipped(R.Left + R.Right - item11.Width, R.Top + R.Bottom - item10.Height, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:105
        // { DrawILT tiles image Index over R }
        public void DrawILT(int Index, in TRect R, ref TSurfaceDesc SurfDesc)
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

            for (int J = 0, last = R.Bottom / this[Index].Height; J <= last; J++)
            {
                for (int I = 0, lastI = R.Right / this[Index].Width; I <= lastI; I++)
                {
                    var item = this[Index];
                    var item2 = this[Index];
                    this[Index].ShowClipped(R.Left + item2.Width * I, R.Top + item.Height * J, SurfDesc);
                }
            }

            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:133
        // { DrawILI draws image Index at the top left corner of R, clipped to R }
        public void DrawILI(int Index, in TRect R, ref TSurfaceDesc SurfDesc)
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
            this[Index].ShowClipped(R.Left, R.Top, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:154
        // { DrawILLh draws a horizontal line of 3 images: Index the left end, +1 the middle (repeated),
//   +2 the right end }
        public void DrawILLh(int Index, in TRect R, ref TSurfaceDesc SurfDesc)
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
            this[Index].ShowClipped(R.Left, R.Top, SurfDesc);
            var item = this[Index + 2];
            if (R.Left + R.Right - item.Width < OldClip.Right)
            {
                var item2 = this[Index + 2];
                SurfDesc.ClipRect.Right = R.Left + R.Right - item2.Width;
            }

            for (int I = 0, last = (R.Right - this[Index].Width - this[Index + 2].Width) / this[Index + 1].Width; I <= last; I++)
            {
                var item2 = this[Index];
                var item3 = this[Index + 1];
                this[Index + 1].ShowClipped(R.Left + item2.Width + item3.Width * I, R.Top, SurfDesc);
            }

            var rect2 = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = rect2;
            var item4 = this[Index + 2];
            this[Index + 2].ShowClipped(R.Left + R.Right - item4.Width, R.Top, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:184
        // { DrawILLv draws a vertical line of 3 images: Index the top end, +1 the middle (repeated), +2
//   the bottom end }
        public void DrawILLv(int Index, in TRect R, ref TSurfaceDesc SurfDesc)
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
            this[Index].ShowClipped(R.Left, R.Top, SurfDesc);
            var item = this[Index + 2];
            if (R.Top + R.Bottom - item.Height < OldClip.Bottom)
            {
                var item2 = this[Index + 2];
                SurfDesc.ClipRect.Bottom = R.Top + R.Bottom - item2.Height;
            }

            for (int I = 0, last = (R.Bottom - this[Index].Height - this[Index + 2].Height) / this[Index + 1].Height; I <= last; I++)
            {
                var item2 = this[Index];
                var item3 = this[Index + 1];
                this[Index + 1].ShowClipped(R.Left, R.Top + item2.Height + item3.Height * I, SurfDesc);
            }

            var rect2 = Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = rect2;
            var item4 = this[Index + 2];
            this[Index + 2].ShowClipped(R.Left, R.Top + R.Bottom - item4.Height, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }
    }

    public static partial class AOWInterfaceImgLib
    {
        // AOWInterfaceImgLib.pas:212
        public static void Register()
        {
            RegisterComponents("ImageLibrary", [typeof(TInterfaceIL)]);
        }
    }
}
