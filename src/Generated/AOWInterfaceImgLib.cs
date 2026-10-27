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
            Aow.Units.AOWInterfaceImgLib.TInterfaceIL Result = default;
            int I = default;
            Result = this;
            if ((Index + Count) > this.Count)
            {
                Result = null;
                Aow.Rtl.Rtl.MessageBox((int)0, ((Name + ((AnsiString)" must have ")) + Aow.Rtl.Rtl.IntToStr(Index + Count)) + ((AnsiString)" Images"), ((AnsiString)"Error - ") + Sender.Name, (int)0);
            }
            else
            {
                {
                    I = Index;
                    int end__1 = (Index + Count) - 1;
                    for (; I <= end__1; I += 1)
                    {
                        if (!ImageExist(I))
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, ((Name + AnsiString.FromBytes(32, 73, 109, 97, 103, 101, 32, 35, 32)) + Aow.Rtl.Rtl.IntToStr(I)) + ((AnsiString)" is invalid"), ((AnsiString)"Error - ") + Sender.Name, (int)0);
                            Result = null;
                        }
                    }
                }
            }

            return Result;
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

            var call__2 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__2;
            var call__3 = this[Index];
            call__3.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__4 = this[Index + 5];
            var call__5 = this[Index + 5];
            call__5.ShowClipped(R.Left, (R.Top + R.Bottom) - call__4.Height, SurfDesc);
            var call__6 = this[Index + 4];
            if (((R.Left + R.Right) - call__6.Width) < OldClip.Right)
            {
                var call__7 = this[Index + 4];
                SurfDesc.ClipRect.Right = (R.Left + R.Right) - call__7.Width;
            }

            {
                I = 0;
                int end__8 = ((R.Right - this[Index].Width) - this[Index + 2].Width) / this[Index + 1].Width;
                for (; I <= end__8; I += 1)
                {
                    var call__9 = this[Index];
                    var call__10 = this[Index + 1];
                    var call__11 = this[Index + 1];
                    call__11.ShowClipped((R.Left + call__9.Width) + (call__10.Width * I), R.Top, SurfDesc);
                    var call__12 = this[Index + 5];
                    var call__13 = this[Index + 6];
                    var call__14 = this[Index + 6];
                    var call__15 = this[Index + 6];
                    call__15.ShowClipped((R.Left + call__12.Width) + (call__13.Width * I), (R.Top + R.Bottom) - call__14.Height, SurfDesc);
                }
            }

            var call__16 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__16;
            var call__17 = this[Index + 6];
            if (((R.Top + R.Bottom) - call__17.Height) < OldClip.Bottom)
            {
                var call__18 = this[Index + 6];
                SurfDesc.ClipRect.Bottom = (R.Top + R.Bottom) - call__18.Height;
            }

            {
                I = 0;
                int end__19 = ((R.Bottom - this[Index].Height) - this[Index + 5].Height) / this[Index + 3].Height;
                for (; I <= end__19; I += 1)
                {
                    var call__20 = this[Index];
                    var call__21 = this[Index + 3];
                    var call__22 = this[Index + 3];
                    call__22.ShowClipped(R.Left, (R.Top + call__20.Height) + (call__21.Height * I), SurfDesc);
                    var call__23 = this[Index + 2];
                    var call__24 = this[Index + 4];
                    var call__25 = this[Index + 4];
                    var call__26 = this[Index + 4];
                    call__26.ShowClipped((R.Left + R.Right) - call__25.Width, (R.Top + call__23.Height) + (call__24.Height * I), SurfDesc);
                }
            }

            var call__27 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__27;
            var call__28 = this[Index + 2];
            var call__29 = this[Index + 2];
            call__29.ShowClipped((R.Left + R.Right) - call__28.Width, R.Top, SurfDesc);
            var call__30 = this[Index + 7];
            var call__31 = this[Index + 7];
            var call__32 = this[Index + 7];
            call__32.ShowClipped((R.Left + R.Right) - call__31.Width, (R.Top + R.Bottom) - call__30.Height, SurfDesc);
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
                int end__34 = R.Bottom / this[Index].Height;
                for (; J <= end__34; J += 1)
                {
                    {
                        I = 0;
                        int end__35 = R.Right / this[Index].Width;
                        for (; I <= end__35; I += 1)
                        {
                            var call__36 = this[Index];
                            var call__37 = this[Index];
                            var call__38 = this[Index];
                            call__38.ShowClipped(R.Left + (call__37.Width * I), R.Top + (call__36.Height * J), SurfDesc);
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

            var call__39 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__39;
            var call__40 = this[Index];
            call__40.ShowClipped(R.Left, R.Top, SurfDesc);
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

            var call__41 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__41;
            var call__42 = this[Index];
            call__42.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__43 = this[Index + 2];
            if (((R.Left + R.Right) - call__43.Width) < OldClip.Right)
            {
                var call__44 = this[Index + 2];
                SurfDesc.ClipRect.Right = (R.Left + R.Right) - call__44.Width;
            }

            {
                I = 0;
                int end__45 = ((R.Right - this[Index].Width) - this[Index + 2].Width) / this[Index + 1].Width;
                for (; I <= end__45; I += 1)
                {
                    var call__46 = this[Index];
                    var call__47 = this[Index + 1];
                    var call__48 = this[Index + 1];
                    call__48.ShowClipped((R.Left + call__46.Width) + (call__47.Width * I), R.Top, SurfDesc);
                }
            }

            var call__49 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__49;
            var call__50 = this[Index + 2];
            var call__51 = this[Index + 2];
            call__51.ShowClipped((R.Left + R.Right) - call__50.Width, R.Top, SurfDesc);
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

            var call__52 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__52;
            var call__53 = this[Index];
            call__53.ShowClipped(R.Left, R.Top, SurfDesc);
            var call__54 = this[Index + 2];
            if (((R.Top + R.Bottom) - call__54.Height) < OldClip.Bottom)
            {
                var call__55 = this[Index + 2];
                SurfDesc.ClipRect.Bottom = (R.Top + R.Bottom) - call__55.Height;
            }

            {
                I = 0;
                int end__56 = ((R.Bottom - this[Index].Height) - this[Index + 2].Height) / this[Index + 1].Height;
                for (; I <= end__56; I += 1)
                {
                    var call__57 = this[Index];
                    var call__58 = this[Index + 1];
                    var call__59 = this[Index + 1];
                    call__59.ShowClipped(R.Left, (R.Top + call__57.Height) + (call__58.Height * I), SurfDesc);
                }
            }

            var call__60 = Aow.Rtl.Rtl.Rect(SurfDesc.ClipRect.Left, SurfDesc.ClipRect.Top, ClipRight, ClipBottom);
            SurfDesc.ClipRect = call__60;
            var call__61 = this[Index + 2];
            var call__62 = this[Index + 2];
            call__62.ShowClipped(R.Left, (R.Top + R.Bottom) - call__61.Height, SurfDesc);
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
