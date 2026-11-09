// Translated from E2WindowLayout.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AOWControlWin;
using Aow.Units.AoWInfoWin;
using Aow.Units.AoWPInfoWin;
using Aow.Units.AoWRealm;
using Aow.Units.AoWSWindow;
using Aow.Units.AoWTCUnit;
using Aow.Units.AoWUnitWindow;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AOWMWindow.AOWMWindowUnit;

namespace Aow.Units.E2WindowLayout;

public static partial class E2WindowLayoutUnit
{
    // E2WindowLayout.pas:13
    // The original window placement loader
    public static void RestoreWindowLayout(object Controller)
    {
        TRect R;
        R = AoWEngine.FRegistry.GetRectangle(6);
        var scanner = AoWSWindowUnit.SWindow.ScannerWindow;
        if (R.Right != 0)
        {
            scanner.Alignment.TopOffset = R.Top;
            scanner.Alignment.RightOffset = R.Left;
            scanner.WinWidth = R.Right;
            scanner.WinHeight = R.Bottom;
        }
        else
        {
            scanner.Alignment.TopOffset = 0;
            scanner.Alignment.RightOffset = 0;
            scanner.WinWidth = scanner.MinWidth;
            scanner.WinHeight = scanner.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(0);
        var playerInfo = AoWPInfoWinUnit.PInfoW.PInfoWin;
        if (R.Right != 0)
        {
            playerInfo.Alignment.BottomOffset = R.Top;
            playerInfo.Alignment.RightOffset = R.Left;
            playerInfo.WinWidth = R.Right;
            playerInfo.WinHeight = R.Bottom;
        }
        else
        {
            playerInfo.Alignment.BottomOffset = 0;
            playerInfo.Alignment.RightOffset = 0;
            playerInfo.WinWidth = playerInfo.MinWidth;
            playerInfo.WinHeight = playerInfo.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(2);
        var unitWindow = AoWUnitWindowUnit.UnitWindow.UnitWin;
        if (R.Right != 0)
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awLeft;
            alignment.AlignHeight = TAOWAlignHeight.ahTop;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            unitWindow.WinWidth = R.Right;
            unitWindow.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awCenter;
            alignment.AlignHeight = TAOWAlignHeight.ahCenter;

            unitWindow.WinWidth = unitWindow.MinWidth;
            unitWindow.WinHeight = unitWindow.MinHeight;
        }

        AoWEngine.FRegistry.GetRectangle(3);
        AoWEngine.FRegistry.GetRectangle(4);
        R = AoWEngine.FRegistry.GetRectangle(5);
        var realm = AoWRealmUnit.RealmWin.Dlg;
        if (R.Right != 0)
        {
            var alignment = realm.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awLeft;
            alignment.AlignHeight = TAOWAlignHeight.ahTop;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            realm.WinWidth = R.Right;
            realm.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = realm.Alignment;
            alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = TAOWAlignWidth.awBoth;
            alignment.AlignHeight = TAOWAlignHeight.ahBottom;

            realm.WinWidth = realm.MinWidth;
            realm.WinHeight = realm.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(13);
        var magic = MagicWin.Dlg;
        if (R.Right != 0)
        {
            var alignment = magic.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awLeft;
            alignment.AlignHeight = TAOWAlignHeight.ahTop;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            magic.WinWidth = R.Right;
            magic.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = magic.Alignment;
            alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = TAOWAlignWidth.awBoth;
            alignment.AlignHeight = TAOWAlignHeight.ahBottom;

            magic.WinWidth = magic.MinWidth;
            magic.WinHeight = magic.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(1);
        var controls = AOWControlWinUnit.ControlWin.ControlWindow;
        if (R.Right != 0)
        {
            controls.Alignment.TopOffset = R.Top;
            controls.Alignment.RightOffset = R.Left;
            controls.WinWidth = R.Right;
            controls.WinHeight = R.Bottom;
        }
        else
        {
            controls.Alignment.TopOffset = scanner.WinHeight;
            controls.Alignment.RightOffset = 0;
            controls.WinWidth = controls.MinWidth;
            controls.WinHeight = controls.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(7);
        var info = AoWInfoWinUnit.IWindow.InfoWin;
        if (R.Right != 0)
        {
            info.Alignment.TopOffset = R.Top;
            info.Alignment.RightOffset = R.Left;
            info.WinWidth = R.Right;
            info.WinHeight = R.Bottom;
        }
        else
        {
            info.Alignment.TopOffset = scanner.WinHeight;
            info.Alignment.RightOffset = 0;
            info.WinWidth = info.MinWidth;
            info.WinHeight = info.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(8);
        var chat = ChatWindow.ChatWin;
        if (R.Right != 0)
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awLeft;
            alignment.BottomOffset = R.Top;
            alignment.LeftOffset = R.Left;

            chat.WinWidth = R.Right;
            chat.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = TAOWAlignWidth.awBoth;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;

            chat.WinWidth = chat.MinWidth;
            chat.WinHeight = chat.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(12);
        var tcScanner = TCScanner.TCScannerWin;
        if (R.Right != 0)
        {
            tcScanner.Alignment.TopOffset = R.Top;
            tcScanner.Alignment.RightOffset = R.Left;
            tcScanner.WinWidth = R.Right;
            tcScanner.WinHeight = R.Bottom;
        }
        else
        {
            tcScanner.Alignment.TopOffset = 0;
            tcScanner.Alignment.RightOffset = 0;
            tcScanner.WinWidth = tcScanner.MinWidth;
            tcScanner.WinHeight = tcScanner.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(10);
        var tcUnit = AoWTCUnitUnit.TCUnit.TCUnitWin;
        if (R.Right != 0)
        {
            tcUnit.Alignment.BottomOffset = R.Top;
            tcUnit.Alignment.RightOffset = R.Left;
            tcUnit.WinWidth = R.Right;
            tcUnit.WinHeight = R.Bottom;
        }
        else
        {
            tcUnit.Alignment.BottomOffset = 0;
            tcUnit.Alignment.RightOffset = 0;
            tcUnit.WinWidth = tcUnit.MinWidth;
            tcUnit.WinHeight = tcUnit.MinHeight;
        }

        R = AoWEngine.FRegistry.GetRectangle(11);
        if (R.Right != 0)
        {
            MWindow.FMapRect = R;
            MWindow.FMapAlignHeight = TAOWAlignHeight.ahTop;
            MWindow.FMapAlignWidth = TAOWAlignWidth.awLeft;
        }
        else
        {
            MWindow.FMapAlignHeight = TAOWAlignHeight.ahBoth;
            MWindow.FMapAlignWidth = TAOWAlignWidth.awBoth;
            var rect = Rect(0, 0, tcScanner.MinWidth, TBWindow.TaskWindow.WinHeight);
            MWindow.FMapRect = rect;
        }

        R = AoWEngine.FRegistry.GetRectangle(9);
        var map = MWindow.MapWindow;
        if (R.Right != 0)
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = TAOWAlignHeight.ahTop;
            alignment.AlignWidth = TAOWAlignWidth.awLeft;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            map.WinWidth = R.Right;
            map.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            alignment.AlignWidth = TAOWAlignWidth.awBoth;
            alignment.TopOffset = 0;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
        }
    }
}
