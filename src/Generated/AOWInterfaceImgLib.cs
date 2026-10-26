// Generated from AOWInterfaceImgLib.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.AOWInterfaceImgLib
{
    // { The AOWInterfaceImgLib unit of aowInt.dpl: TInterfaceIL, the image library that holds the
//   pieces the controls are drawn from (frames, textures, lines, icons). Its Draw* methods draw a
//   frame, a texture tile, a single image or a horizontal or vertical line of images into a
//   rectangle given as (Left, Top, Width, Height) in a TRect, clipped to the surface's clip rectangle,
//   which they narrow while drawing and restore. }
    // { AOWInterfaceImgLib.TInterfaceIL (instance size 80): no fields or virtual methods of its own }
    public partial class TInterfaceIL : Aow.Units.ImageLib.TImageLibrary
    {
        // AOWInterfaceImgLib.pas:34
        // { TInterfaceIL }
        // { CheckIL returns Self when the library holds the Count images from Index on, else nil after a
//   message box that names the library and the component (Sender) that needs them }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CheckIL(int Index, int Count, Aow.Rtl.TComponent Sender)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AOWInterfaceImgLib.pas:58
        // { DrawILF draws a frame of 8 images: Index the top left corner, +1 the top edge, +2 the top right
//   corner, +3 the left edge, +4 the right edge, +5 the bottom left corner, +6 the bottom edge, +7
//   the bottom right corner }
        public void DrawILF(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
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

            var call__1 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__1;
            var call__2 = this[Index];
            call__2.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__3 = this[Index + 5];
            var call__4 = this[Index + 5];
            call__4.ShowClipped(R.Left, (R.Top + R.Bottom) - call__3.Height, SurfDesc);
            var call__5 = this[Index + 4];
            if (((R.Left + R.Right) - call__5.Width) < OldClip.Right)
            {
                var call__6 = this[Index + 4];
                SurfDesc.ClipRect.Right = (R.Left + R.Right) - call__6.Width;
            }

            {
                I = 0;
                int end__7 = ((R.Right - this[Index].Width) - this[Index + 2].Width) / this[Index + 1].Width;
                for (; I <= end__7; I += 1)
                {
                    var call__8 = this[Index];
                    var call__9 = this[Index + 1];
                    var call__10 = this[Index + 1];
                    call__10.ShowClipped((R.Left + call__8.Width) + (call__9.Width * I), R.Top, SurfDesc);
                    var call__11 = this[Index + 5];
                    var call__12 = this[Index + 6];
                    var call__13 = this[Index + 6];
                    var call__14 = this[Index + 6];
                    call__14.ShowClipped((R.Left + call__11.Width) + (call__12.Width * I), (R.Top + R.Bottom) - call__13.Height, SurfDesc);
                }
            }

            var call__15 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__15;
            var call__16 = this[Index + 6];
            if (((R.Top + R.Bottom) - call__16.Height) < OldClip.Bottom)
            {
                var call__17 = this[Index + 6];
                SurfDesc.ClipRect.Bottom = (R.Top + R.Bottom) - call__17.Height;
            }

            {
                I = 0;
                int end__18 = ((R.Bottom - this[Index].Height) - this[Index + 5].Height) / this[Index + 3].Height;
                for (; I <= end__18; I += 1)
                {
                    var call__19 = this[Index];
                    var call__20 = this[Index + 3];
                    var call__21 = this[Index + 3];
                    call__21.ShowClipped(R.Left, (R.Top + call__19.Height) + (call__20.Height * I), SurfDesc);
                    var call__22 = this[Index + 2];
                    var call__23 = this[Index + 4];
                    var call__24 = this[Index + 4];
                    var call__25 = this[Index + 4];
                    call__25.ShowClipped((R.Left + R.Right) - call__24.Width, (R.Top + call__22.Height) + (call__23.Height * I), SurfDesc);
                }
            }

            var call__26 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__26;
            var call__27 = this[Index + 2];
            var call__28 = this[Index + 2];
            call__28.ShowClipped((R.Left + R.Right) - call__27.Width, R.Top, SurfDesc);
            var call__29 = this[Index + 7];
            var call__30 = this[Index + 7];
            var call__31 = this[Index + 7];
            call__31.ShowClipped((R.Left + R.Right) - call__30.Width, (R.Top + R.Bottom) - call__29.Height, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:105
        // { DrawILT tiles image Index over R }
        public void DrawILT(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
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

            var call__32 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__32;
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
                int end__33 = R.Bottom / this[Index].Height;
                for (; J <= end__33; J += 1)
                {
                    {
                        I = 0;
                        int end__34 = R.Right / this[Index].Width;
                        for (; I <= end__34; I += 1)
                        {
                            var call__35 = this[Index];
                            var call__36 = this[Index];
                            var call__37 = this[Index];
                            call__37.ShowClipped(R.Left + (call__36.Width * I), R.Top + (call__35.Height * J), SurfDesc);
                        }
                    }
                }
            }

            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:133
        // { DrawILI draws image Index at the top left corner of R, clipped to R }
        public void DrawILI(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
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

            var call__38 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__38;
            var call__39 = this[Index];
            call__39.ShowClipped(R.Left, R.Top, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:154
        // { DrawILLh draws a horizontal line of 3 images: Index the left end, +1 the middle (repeated),
//   +2 the right end }
        public void DrawILLh(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
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

            var call__40 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__40;
            var call__41 = this[Index];
            call__41.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__42 = this[Index + 2];
            if (((R.Left + R.Right) - call__42.Width) < OldClip.Right)
            {
                var call__43 = this[Index + 2];
                SurfDesc.ClipRect.Right = (R.Left + R.Right) - call__43.Width;
            }

            {
                I = 0;
                int end__44 = ((R.Right - this[Index].Width) - this[Index + 2].Width) / this[Index + 1].Width;
                for (; I <= end__44; I += 1)
                {
                    var call__45 = this[Index];
                    var call__46 = this[Index + 1];
                    var call__47 = this[Index + 1];
                    call__47.ShowClipped((R.Left + call__45.Width) + (call__46.Width * I), R.Top, SurfDesc);
                }
            }

            var call__48 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__48;
            var call__49 = this[Index + 2];
            var call__50 = this[Index + 2];
            call__50.ShowClipped((R.Left + R.Right) - call__49.Width, R.Top, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // AOWInterfaceImgLib.pas:184
        // { DrawILLv draws a vertical line of 3 images: Index the top end, +1 the middle (repeated), +2
//   the bottom end }
        public void DrawILLv(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
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

            var call__51 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__51;
            var call__52 = this[Index];
            call__52.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__53 = this[Index + 2];
            if (((R.Top + R.Bottom) - call__53.Height) < OldClip.Bottom)
            {
                var call__54 = this[Index + 2];
                SurfDesc.ClipRect.Bottom = (R.Top + R.Bottom) - call__54.Height;
            }

            {
                I = 0;
                int end__55 = ((R.Bottom - this[Index].Height) - this[Index + 2].Height) / this[Index + 1].Height;
                for (; I <= end__55; I += 1)
                {
                    var call__56 = this[Index];
                    var call__57 = this[Index + 1];
                    var call__58 = this[Index + 1];
                    call__58.ShowClipped(R.Left, (R.Top + call__56.Height) + (call__57.Height * I), SurfDesc);
                }
            }

            var call__59 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__59;
            var call__60 = this[Index + 2];
            var call__61 = this[Index + 2];
            call__61.ShowClipped(R.Left, (R.Top + R.Bottom) - call__60.Height, SurfDesc);
            SurfDesc.ClipRect = OldClip;
        }

        // ImageLib.pas:111
        public static new Aow.Units.AOWInterfaceImgLib.TInterfaceIL Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AOWInterfaceImgLib.TInterfaceIL o = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AOWInterfaceImgLib
    {
        // AOWInterfaceImgLib.pas:212
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"ImageLibrary", [typeof(Aow.Units.AOWInterfaceImgLib.TInterfaceIL)]);
        }
    }
}
