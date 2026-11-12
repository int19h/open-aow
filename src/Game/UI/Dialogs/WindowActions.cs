
using Aow.Game.Host;

using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public static class WindowActions
{
    internal static void RestoreWindowLayout(General controller)
    {
        Rect R;
        R = controller.UI.Engine.Registry.GetRectangle(6);
        var scanner = controller.UI.ScannerWindow.MainWindow;
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

        R = controller.UI.Engine.Registry.GetRectangle(0);
        var playerInfo = controller.UI.PlayerInfoWindow.MainWindow;
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

        R = controller.UI.Engine.Registry.GetRectangle(2);
        var unitWindow = controller.UI.UnitWindow.MainWindow;
        if (R.Right != 0)
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            unitWindow.WinWidth = R.Right;
            unitWindow.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = AlignWidth.Center;
            alignment.AlignHeight = AlignHeight.Center;

            unitWindow.WinWidth = unitWindow.MinWidth;
            unitWindow.WinHeight = unitWindow.MinHeight;
        }

        controller.UI.Engine.Registry.GetRectangle(3);
        controller.UI.Engine.Registry.GetRectangle(4);
        R = controller.UI.Engine.Registry.GetRectangle(5);
        var realm = controller.UI.RealmWindow.Window;
        if (R.Right != 0)
        {
            var alignment = realm.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            realm.WinWidth = R.Right;
            realm.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = realm.Alignment;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            realm.WinWidth = realm.MinWidth;
            realm.WinHeight = realm.MinHeight;
        }

        R = controller.UI.Engine.Registry.GetRectangle(13);
        var magic = controller.UI.MagicWindow.Window;
        if (R.Right != 0)
        {
            var alignment = magic.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            magic.WinWidth = R.Right;
            magic.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = magic.Alignment;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            magic.WinWidth = magic.MinWidth;
            magic.WinHeight = magic.MinHeight;
        }

        R = controller.UI.Engine.Registry.GetRectangle(1);
        var controls = controller.UI.ControlWindow.MainWindow;
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

        R = controller.UI.Engine.Registry.GetRectangle(7);
        var info = controller.UI.InfoWindow.MainWindow;
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

        R = controller.UI.Engine.Registry.GetRectangle(8);
        var chat = controller.UI.ChatWindow.MainWindow;
        if (R.Right != 0)
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.BottomOffset = R.Top;
            alignment.LeftOffset = R.Left;

            chat.WinWidth = R.Right;
            chat.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;

            chat.WinWidth = chat.MinWidth;
            chat.WinHeight = chat.MinHeight;
        }

        R = controller.UI.Engine.Registry.GetRectangle(12);
        var tacticalBattleScanner = controller.UI.CombatScanner.CombatScannerWindow;
        if (R.Right != 0)
        {
            tacticalBattleScanner.Alignment.TopOffset = R.Top;
            tacticalBattleScanner.Alignment.RightOffset = R.Left;
            tacticalBattleScanner.WinWidth = R.Right;
            tacticalBattleScanner.WinHeight = R.Bottom;
        }
        else
        {
            tacticalBattleScanner.Alignment.TopOffset = 0;
            tacticalBattleScanner.Alignment.RightOffset = 0;
            tacticalBattleScanner.WinWidth = tacticalBattleScanner.MinWidth;
            tacticalBattleScanner.WinHeight = tacticalBattleScanner.MinHeight;
        }

        R = controller.UI.Engine.Registry.GetRectangle(10);
        var tacticalBattleUnit = controller.UI.CombatUnitInfo.MainWindow;
        if (R.Right != 0)
        {
            tacticalBattleUnit.Alignment.BottomOffset = R.Top;
            tacticalBattleUnit.Alignment.RightOffset = R.Left;
            tacticalBattleUnit.WinWidth = R.Right;
            tacticalBattleUnit.WinHeight = R.Bottom;
        }
        else
        {
            tacticalBattleUnit.Alignment.BottomOffset = 0;
            tacticalBattleUnit.Alignment.RightOffset = 0;
            tacticalBattleUnit.WinWidth = tacticalBattleUnit.MinWidth;
            tacticalBattleUnit.WinHeight = tacticalBattleUnit.MinHeight;
        }

        R = controller.UI.Engine.Registry.GetRectangle(11);
        if (R.Right != 0)
        {
            controller.UI.MapWindow.MapRectangle = R;
            controller.UI.MapWindow.MapAlignHeight = AlignHeight.Top;
            controller.UI.MapWindow.MapAlignWidth = AlignWidth.Left;
        }
        else
        {
            controller.UI.MapWindow.MapAlignHeight = AlignHeight.Both;
            controller.UI.MapWindow.MapAlignWidth = AlignWidth.Both;
            var rectangle = new Rect(0, 0, tacticalBattleScanner.MinWidth, controller.UI.TaskBar.TaskWindow.WinHeight);
            controller.UI.MapWindow.MapRectangle = rectangle;
        }

        R = controller.UI.Engine.Registry.GetRectangle(9);
        var map = controller.UI.MapWindow.MainWindow;
        if (R.Right != 0)
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = AlignHeight.Top;
            alignment.AlignWidth = AlignWidth.Left;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            map.WinWidth = R.Right;
            map.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = AlignHeight.Both;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.TopOffset = 0;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = controller.UI.TaskBar.TaskWindow.WinHeight;
        }
    }

    public static void OriginalAssert(string message, string fileName, int lineNumber)
    {
        Assert(false, message, fileName, lineNumber);
    }

    internal static void SetMainCursor(IGameHost form, int cursor)
    {
        form.FCursor = (short)Math.Abs(cursor);
    }

    internal static void ShowTitleScene(TitleScene scene, bool music) => scene.ShowScene(music);

}
