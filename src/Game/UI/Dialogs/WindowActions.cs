
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.UI.Dialogs;

public static class WindowActions
{
    internal static void RestoreWindowLayout(General controller)
    {
        Rect r = controller.UI.Engine.Registry.GetRectangle(6);
        Window scanner = controller.UI.ScannerWindow.MainWindow;
        if (r.Right != 0)
        {
            scanner.Alignment.TopOffset = r.Top;
            scanner.Alignment.RightOffset = r.Left;
            scanner.WinWidth = r.Right;
            scanner.WinHeight = r.Bottom;
        }
        else
        {
            scanner.Alignment.TopOffset = 0;
            scanner.Alignment.RightOffset = 0;
            scanner.WinWidth = scanner.MinWidth;
            scanner.WinHeight = scanner.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(0);
        Window playerInfo = controller.UI.PlayerInfoWindow.MainWindow;
        if (r.Right != 0)
        {
            playerInfo.Alignment.BottomOffset = r.Top;
            playerInfo.Alignment.RightOffset = r.Left;
            playerInfo.WinWidth = r.Right;
            playerInfo.WinHeight = r.Bottom;
        }
        else
        {
            playerInfo.Alignment.BottomOffset = 0;
            playerInfo.Alignment.RightOffset = 0;
            playerInfo.WinWidth = playerInfo.MinWidth;
            playerInfo.WinHeight = playerInfo.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(2);
        Window unitWindow = controller.UI.UnitWindow.MainWindow;
        if (r.Right != 0)
        {
            Alignment alignment = unitWindow.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = r.Top;
            alignment.LeftOffset = r.Left;

            unitWindow.WinWidth = r.Right;
            unitWindow.WinHeight = r.Bottom;
        }
        else
        {
            Alignment alignment = unitWindow.Alignment;
            alignment.AlignWidth = AlignWidth.Center;
            alignment.AlignHeight = AlignHeight.Center;

            unitWindow.WinWidth = unitWindow.MinWidth;
            unitWindow.WinHeight = unitWindow.MinHeight;
        }

        _ = controller.UI.Engine.Registry.GetRectangle(3);
        _ = controller.UI.Engine.Registry.GetRectangle(4);
        r = controller.UI.Engine.Registry.GetRectangle(5);
        Window realm = controller.UI.RealmWindow.Window;
        if (r.Right != 0)
        {
            Alignment alignment = realm.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = r.Top;
            alignment.LeftOffset = r.Left;

            realm.WinWidth = r.Right;
            realm.WinHeight = r.Bottom;
        }
        else
        {
            Alignment alignment = realm.Alignment;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            realm.WinWidth = realm.MinWidth;
            realm.WinHeight = realm.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(13);
        Window magic = controller.UI.MagicWindow.Window;
        if (r.Right != 0)
        {
            Alignment alignment = magic.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = r.Top;
            alignment.LeftOffset = r.Left;

            magic.WinWidth = r.Right;
            magic.WinHeight = r.Bottom;
        }
        else
        {
            Alignment alignment = magic.Alignment;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            magic.WinWidth = magic.MinWidth;
            magic.WinHeight = magic.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(1);
        Window controls = controller.UI.ControlWindow.MainWindow;
        if (r.Right != 0)
        {
            controls.Alignment.TopOffset = r.Top;
            controls.Alignment.RightOffset = r.Left;
            controls.WinWidth = r.Right;
            controls.WinHeight = r.Bottom;
        }
        else
        {
            controls.Alignment.TopOffset = scanner.WinHeight;
            controls.Alignment.RightOffset = 0;
            controls.WinWidth = controls.MinWidth;
            controls.WinHeight = controls.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(7);
        Window info = controller.UI.InfoWindow.MainWindow;
        if (r.Right != 0)
        {
            info.Alignment.TopOffset = r.Top;
            info.Alignment.RightOffset = r.Left;
            info.WinWidth = r.Right;
            info.WinHeight = r.Bottom;
        }
        else
        {
            info.Alignment.TopOffset = scanner.WinHeight;
            info.Alignment.RightOffset = 0;
            info.WinWidth = info.MinWidth;
            info.WinHeight = info.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(8);
        Window chat = controller.UI.ChatWindow.MainWindow;
        if (r.Right != 0)
        {
            Alignment alignment = chat.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.BottomOffset = r.Top;
            alignment.LeftOffset = r.Left;

            chat.WinWidth = r.Right;
            chat.WinHeight = r.Bottom;
        }
        else
        {
            Alignment alignment = chat.Alignment;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;

            chat.WinWidth = chat.MinWidth;
            chat.WinHeight = chat.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(12);
        Window tacticalBattleScanner = controller.UI.CombatScanner.CombatScannerWindow;
        if (r.Right != 0)
        {
            tacticalBattleScanner.Alignment.TopOffset = r.Top;
            tacticalBattleScanner.Alignment.RightOffset = r.Left;
            tacticalBattleScanner.WinWidth = r.Right;
            tacticalBattleScanner.WinHeight = r.Bottom;
        }
        else
        {
            tacticalBattleScanner.Alignment.TopOffset = 0;
            tacticalBattleScanner.Alignment.RightOffset = 0;
            tacticalBattleScanner.WinWidth = tacticalBattleScanner.MinWidth;
            tacticalBattleScanner.WinHeight = tacticalBattleScanner.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(10);
        Window tacticalBattleUnit = controller.UI.CombatUnitInfo.MainWindow;
        if (r.Right != 0)
        {
            tacticalBattleUnit.Alignment.BottomOffset = r.Top;
            tacticalBattleUnit.Alignment.RightOffset = r.Left;
            tacticalBattleUnit.WinWidth = r.Right;
            tacticalBattleUnit.WinHeight = r.Bottom;
        }
        else
        {
            tacticalBattleUnit.Alignment.BottomOffset = 0;
            tacticalBattleUnit.Alignment.RightOffset = 0;
            tacticalBattleUnit.WinWidth = tacticalBattleUnit.MinWidth;
            tacticalBattleUnit.WinHeight = tacticalBattleUnit.MinHeight;
        }

        r = controller.UI.Engine.Registry.GetRectangle(11);
        if (r.Right != 0)
        {
            controller.UI.MapWindow.MapRectangle = r;
            controller.UI.MapWindow.MapAlignHeight = AlignHeight.Top;
            controller.UI.MapWindow.MapAlignWidth = AlignWidth.Left;
        }
        else
        {
            controller.UI.MapWindow.MapAlignHeight = AlignHeight.Both;
            controller.UI.MapWindow.MapAlignWidth = AlignWidth.Both;
            Rect rectangle = new(0, 0, tacticalBattleScanner.MinWidth, controller.UI.TaskBar.TaskWindow.WinHeight);
            controller.UI.MapWindow.MapRectangle = rectangle;
        }

        r = controller.UI.Engine.Registry.GetRectangle(9);
        Window map = controller.UI.MapWindow.MainWindow;
        if (r.Right != 0)
        {
            Alignment alignment = map.Alignment;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.TopOffset = r.Top;
            alignment.LeftOffset = r.Left;

            map.WinWidth = r.Right;
            map.WinHeight = r.Bottom;
        }
        else
        {
            Alignment alignment = map.Alignment;
            alignment.AlignHeight = AlignHeight.Both;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.TopOffset = 0;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
        }
    }

    public static void OriginalAssert(string message, string fileName, int lineNumber) => Assert(false, message, file: fileName, line: lineNumber);

}
