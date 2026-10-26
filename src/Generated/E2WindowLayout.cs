// Generated from E2WindowLayout.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
            {
                var w__2 = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
                if (R.Right != 0)
                {
                    w__2.Alignment.TopOffset = R.Top;
                    w__2.Alignment.RightOffset = R.Left;
                    w__2.WinWidth = R.Right;
                    w__2.WinHeight = R.Bottom;
                }
                else
                {
                    w__2.Alignment.TopOffset = 0;
                    w__2.Alignment.RightOffset = 0;
                    w__2.WinWidth = w__2.MinWidth;
                    w__2.WinHeight = w__2.MinHeight;
                }
            }

            var call__3 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(0);
            R = call__3;
            {
                var w__4 = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
                if (R.Right != 0)
                {
                    w__4.Alignment.BottomOffset = R.Top;
                    w__4.Alignment.RightOffset = R.Left;
                    w__4.WinWidth = R.Right;
                    w__4.WinHeight = R.Bottom;
                }
                else
                {
                    w__4.Alignment.BottomOffset = 0;
                    w__4.Alignment.RightOffset = 0;
                    w__4.WinWidth = w__4.MinWidth;
                    w__4.WinHeight = w__4.MinHeight;
                }
            }

            var call__5 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(2);
            R = call__5;
            {
                var w__6 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
                if (R.Right != 0)
                {
                    {
                        var w__7 = w__6.Alignment;
                        w__7.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                        w__7.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                        w__7.TopOffset = R.Top;
                        w__7.LeftOffset = R.Left;
                    }

                    w__6.WinWidth = R.Right;
                    w__6.WinHeight = R.Bottom;
                }
                else
                {
                    {
                        var w__8 = w__6.Alignment;
                        w__8.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                        w__8.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;
                    }

                    w__6.WinWidth = w__6.MinWidth;
                    w__6.WinHeight = w__6.MinHeight;
                }
            }

            var call__9 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(3);
            R = call__9;
            var call__10 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(4);
            R = call__10;
            var call__11 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(5);
            R = call__11;
            {
                var w__12 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
                if (R.Right != 0)
                {
                    {
                        var w__13 = w__12.Alignment;
                        w__13.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                        w__13.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                        w__13.TopOffset = R.Top;
                        w__13.LeftOffset = R.Left;
                    }

                    w__12.WinWidth = R.Right;
                    w__12.WinHeight = R.Bottom;
                }
                else
                {
                    {
                        var w__14 = w__12.Alignment;
                        w__14.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    }

                    {
                        var w__15 = w__12.Alignment;
                        w__15.LeftOffset = 0;
                        w__15.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                        w__15.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                        w__15.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                    }

                    w__12.WinWidth = w__12.MinWidth;
                    w__12.WinHeight = w__12.MinHeight;
                }
            }

            var call__16 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(13);
            R = call__16;
            {
                var w__17 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
                if (R.Right != 0)
                {
                    {
                        var w__18 = w__17.Alignment;
                        w__18.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                        w__18.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                        w__18.TopOffset = R.Top;
                        w__18.LeftOffset = R.Left;
                    }

                    w__17.WinWidth = R.Right;
                    w__17.WinHeight = R.Bottom;
                }
                else
                {
                    {
                        var w__19 = w__17.Alignment;
                        w__19.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    }

                    {
                        var w__20 = w__17.Alignment;
                        w__20.LeftOffset = 0;
                        w__20.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                        w__20.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                        w__20.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                    }

                    w__17.WinWidth = w__17.MinWidth;
                    w__17.WinHeight = w__17.MinHeight;
                }
            }

            var call__21 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(1);
            R = call__21;
            {
                var w__22 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
                if (R.Right != 0)
                {
                    w__22.Alignment.TopOffset = R.Top;
                    w__22.Alignment.RightOffset = R.Left;
                    w__22.WinWidth = R.Right;
                    w__22.WinHeight = R.Bottom;
                }
                else
                {
                    w__22.Alignment.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                    w__22.Alignment.RightOffset = 0;
                    w__22.WinWidth = w__22.MinWidth;
                    w__22.WinHeight = w__22.MinHeight;
                }
            }

            var call__23 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(7);
            R = call__23;
            {
                var w__24 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
                if (R.Right != 0)
                {
                    w__24.Alignment.TopOffset = R.Top;
                    w__24.Alignment.RightOffset = R.Left;
                    w__24.WinWidth = R.Right;
                    w__24.WinHeight = R.Bottom;
                }
                else
                {
                    w__24.Alignment.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                    w__24.Alignment.RightOffset = 0;
                    w__24.WinWidth = w__24.MinWidth;
                    w__24.WinHeight = w__24.MinHeight;
                }
            }

            var call__25 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(8);
            R = call__25;
            {
                var w__26 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
                if (R.Right != 0)
                {
                    {
                        var w__27 = w__26.Alignment;
                        w__27.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                        w__27.BottomOffset = R.Top;
                        w__27.LeftOffset = R.Left;
                    }

                    w__26.WinWidth = R.Right;
                    w__26.WinHeight = R.Bottom;
                }
                else
                {
                    {
                        var w__28 = w__26.Alignment;
                        w__28.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                        w__28.LeftOffset = 0;
                        w__28.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.MinWidth;
                        w__28.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    }

                    w__26.WinWidth = w__26.MinWidth;
                    w__26.WinHeight = w__26.MinHeight;
                }
            }

            var call__29 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(12);
            R = call__29;
            {
                var w__30 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
                if (R.Right != 0)
                {
                    w__30.Alignment.TopOffset = R.Top;
                    w__30.Alignment.RightOffset = R.Left;
                    w__30.WinWidth = R.Right;
                    w__30.WinHeight = R.Bottom;
                }
                else
                {
                    w__30.Alignment.TopOffset = 0;
                    w__30.Alignment.RightOffset = 0;
                    w__30.WinWidth = w__30.MinWidth;
                    w__30.WinHeight = w__30.MinHeight;
                }
            }

            var call__31 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetRectangle(10);
            R = call__31;
            {
                var w__32 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
                if (R.Right != 0)
                {
                    w__32.Alignment.BottomOffset = R.Top;
                    w__32.Alignment.RightOffset = R.Left;
                    w__32.WinWidth = R.Right;
                    w__32.WinHeight = R.Bottom;
                }
                else
                {
                    w__32.Alignment.BottomOffset = 0;
                    w__32.Alignment.RightOffset = 0;
                    w__32.WinWidth = w__32.MinWidth;
                    w__32.WinHeight = w__32.MinHeight;
                }
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
            {
                var w__36 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                if (R.Right != 0)
                {
                    {
                        var w__37 = w__36.Alignment;
                        w__37.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
                        w__37.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
                        w__37.TopOffset = R.Top;
                        w__37.LeftOffset = R.Left;
                    }

                    w__36.WinWidth = R.Right;
                    w__36.WinHeight = R.Bottom;
                }
                else
                {
                    {
                        var w__38 = w__36.Alignment;
                        w__38.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
                        w__38.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                        w__38.TopOffset = 0;
                        w__38.LeftOffset = 0;
                        w__38.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.MinWidth;
                        w__38.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    }
                }
            }
        }
    }
}
