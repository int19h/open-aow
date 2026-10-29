// Translated from E2WindowLayout.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.E2WindowLayout
{
    public static partial class E2WindowLayout
    {
        // E2WindowLayout.pas:13
        // { The original window placement loader, 00455F58. }
        public static void RestoreWindowLayout(Aow.Rtl.TObject Controller)
        {
            Aow.Rtl.TRect R = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(6);
            R = call__1;
            var window = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
            if (R.Right != 0)
            {
                window.Alignment.TopOffset = R.Top;
                window.Alignment.RightOffset = R.Left;
                window.WinWidth = R.Right;
                window.WinHeight = R.Bottom;
            }
            else
            {
                window.Alignment.TopOffset = 0;
                window.Alignment.RightOffset = 0;
                window.WinWidth = window.MinWidth;
                window.WinHeight = window.MinHeight;
            }

            var call__3 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(0);
            R = call__3;
            var window2 = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
            if (R.Right != 0)
            {
                window2.Alignment.BottomOffset = R.Top;
                window2.Alignment.RightOffset = R.Left;
                window2.WinWidth = R.Right;
                window2.WinHeight = R.Bottom;
            }
            else
            {
                window2.Alignment.BottomOffset = 0;
                window2.Alignment.RightOffset = 0;
                window2.WinWidth = window2.MinWidth;
                window2.WinHeight = window2.MinHeight;
            }

            var call__5 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(2);
            R = call__5;
            var window3 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            if (R.Right != 0)
            {
                var alignment = window3.Alignment;
                alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                alignment.TopOffset = R.Top;
                alignment.LeftOffset = R.Left;

                window3.WinWidth = R.Right;
                window3.WinHeight = R.Bottom;
            }
            else
            {
                var alignment2 = window3.Alignment;
                alignment2.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                alignment2.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;

                window3.WinWidth = window3.MinWidth;
                window3.WinHeight = window3.MinHeight;
            }

            var call__9 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(3);
            R = call__9;
            var call__10 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(4);
            R = call__10;
            var call__11 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(5);
            R = call__11;
            var window4 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
            if (R.Right != 0)
            {
                var alignment3 = window4.Alignment;
                alignment3.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                alignment3.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                alignment3.TopOffset = R.Top;
                alignment3.LeftOffset = R.Left;

                window4.WinWidth = R.Right;
                window4.WinHeight = R.Bottom;
            }
            else
            {
                var alignment4 = window4.Alignment;
                alignment4.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;

                var alignment5 = window4.Alignment;
                alignment5.LeftOffset = 0;
                alignment5.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                alignment5.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                alignment5.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;

                window4.WinWidth = window4.MinWidth;
                window4.WinHeight = window4.MinHeight;
            }

            var call__16 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(13);
            R = call__16;
            var window5 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
            if (R.Right != 0)
            {
                var alignment6 = window5.Alignment;
                alignment6.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                alignment6.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                alignment6.TopOffset = R.Top;
                alignment6.LeftOffset = R.Left;

                window5.WinWidth = R.Right;
                window5.WinHeight = R.Bottom;
            }
            else
            {
                var alignment7 = window5.Alignment;
                alignment7.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;

                var alignment8 = window5.Alignment;
                alignment8.LeftOffset = 0;
                alignment8.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                alignment8.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                alignment8.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;

                window5.WinWidth = window5.MinWidth;
                window5.WinHeight = window5.MinHeight;
            }

            var call__21 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(1);
            R = call__21;
            var window6 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            if (R.Right != 0)
            {
                window6.Alignment.TopOffset = R.Top;
                window6.Alignment.RightOffset = R.Left;
                window6.WinWidth = R.Right;
                window6.WinHeight = R.Bottom;
            }
            else
            {
                window6.Alignment.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                window6.Alignment.RightOffset = 0;
                window6.WinWidth = window6.MinWidth;
                window6.WinHeight = window6.MinHeight;
            }

            var call__23 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(7);
            R = call__23;
            var window7 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            if (R.Right != 0)
            {
                window7.Alignment.TopOffset = R.Top;
                window7.Alignment.RightOffset = R.Left;
                window7.WinWidth = R.Right;
                window7.WinHeight = R.Bottom;
            }
            else
            {
                window7.Alignment.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                window7.Alignment.RightOffset = 0;
                window7.WinWidth = window7.MinWidth;
                window7.WinHeight = window7.MinHeight;
            }

            var call__25 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(8);
            R = call__25;
            var window8 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
            if (R.Right != 0)
            {
                var alignment9 = window8.Alignment;
                alignment9.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                alignment9.BottomOffset = R.Top;
                alignment9.LeftOffset = R.Left;

                window8.WinWidth = R.Right;
                window8.WinHeight = R.Bottom;
            }
            else
            {
                var alignment10 = window8.Alignment;
                alignment10.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                alignment10.LeftOffset = 0;
                alignment10.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.MinWidth;
                alignment10.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;

                window8.WinWidth = window8.MinWidth;
                window8.WinHeight = window8.MinHeight;
            }

            var call__29 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(12);
            R = call__29;
            var window9 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
            if (R.Right != 0)
            {
                window9.Alignment.TopOffset = R.Top;
                window9.Alignment.RightOffset = R.Left;
                window9.WinWidth = R.Right;
                window9.WinHeight = R.Bottom;
            }
            else
            {
                window9.Alignment.TopOffset = 0;
                window9.Alignment.RightOffset = 0;
                window9.WinWidth = window9.MinWidth;
                window9.WinHeight = window9.MinHeight;
            }

            var call__31 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(10);
            R = call__31;
            var window10 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            if (R.Right != 0)
            {
                window10.Alignment.BottomOffset = R.Top;
                window10.Alignment.RightOffset = R.Left;
                window10.WinWidth = R.Right;
                window10.WinHeight = R.Bottom;
            }
            else
            {
                window10.Alignment.BottomOffset = 0;
                window10.Alignment.RightOffset = 0;
                window10.WinWidth = window10.MinWidth;
                window10.WinHeight = window10.MinHeight;
            }

            var call__33 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(11);
            R = call__33;
            if (R.Right != 0)
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect = R;
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            }
            else
            {
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                var call__34 = Aow.Rtl.Rtl.Rect(0, 0, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.MinWidth, Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight);
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect = call__34;
            }

            var call__35 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(9);
            R = call__35;
            var window11 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
            if (R.Right != 0)
            {
                var alignment11 = window11.Alignment;
                alignment11.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                alignment11.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                alignment11.TopOffset = R.Top;
                alignment11.LeftOffset = R.Left;

                window11.WinWidth = R.Right;
                window11.WinHeight = R.Bottom;
            }
            else
            {
                var alignment12 = window11.Alignment;
                alignment12.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
                alignment12.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                alignment12.TopOffset = 0;
                alignment12.LeftOffset = 0;
                alignment12.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.MinWidth;
                alignment12.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
            }
        }
    }
}
