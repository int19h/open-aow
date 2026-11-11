
using Aow.Game.Host;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public static class WindowActions
{
    internal static void RestoreWindowLayout(object Controller)
    {
        Rect R;
        R = GameTables.AoWEngine.FRegistry.GetRectangle(6);
        var scanner = DialogHosts.SWindow.MainWindow;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(0);
        var playerInfo = DialogHosts.PInfoW.PInfoWin;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(2);
        var unitWindow = DialogHosts.UnitWindow.UnitWin;
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

        GameTables.AoWEngine.FRegistry.GetRectangle(3);
        GameTables.AoWEngine.FRegistry.GetRectangle(4);
        R = GameTables.AoWEngine.FRegistry.GetRectangle(5);
        var realm = DialogHosts.RealmWin.Dlg;
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
            alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            realm.WinWidth = realm.MinWidth;
            realm.WinHeight = realm.MinHeight;
        }

        R = GameTables.AoWEngine.FRegistry.GetRectangle(13);
        var magic = DialogHosts.MagicWin.Dlg;
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
            alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AlignWidth.Both;
            alignment.AlignHeight = AlignHeight.Bottom;

            magic.WinWidth = magic.MinWidth;
            magic.WinHeight = magic.MinHeight;
        }

        R = GameTables.AoWEngine.FRegistry.GetRectangle(1);
        var controls = DialogHosts.ControlWin.MainWindow;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(7);
        var info = DialogHosts.IWindow.InfoWin;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(8);
        var chat = DialogHosts.ChatWindow.ChatWin;
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
            alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;

            chat.WinWidth = chat.MinWidth;
            chat.WinHeight = chat.MinHeight;
        }

        R = GameTables.AoWEngine.FRegistry.GetRectangle(12);
        var tcScanner = DialogHosts.TCScanner.TCScannerWin;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(10);
        var tcUnit = DialogHosts.TCUnit.TCUnitWin;
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

        R = GameTables.AoWEngine.FRegistry.GetRectangle(11);
        if (R.Right != 0)
        {
            DialogHosts.MWindow.FMapRect = R;
            DialogHosts.MWindow.FMapAlignHeight = AlignHeight.Top;
            DialogHosts.MWindow.FMapAlignWidth = AlignWidth.Left;
        }
        else
        {
            DialogHosts.MWindow.FMapAlignHeight = AlignHeight.Both;
            DialogHosts.MWindow.FMapAlignWidth = AlignWidth.Both;
            var rect = new Rect(0, 0, tcScanner.MinWidth, DialogHosts.TBWindow.TaskWindow.WinHeight);
            DialogHosts.MWindow.FMapRect = rect;
        }

        R = GameTables.AoWEngine.FRegistry.GetRectangle(9);
        var map = DialogHosts.MWindow.MainWindow;
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
            alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
        }
    }

    public static void OriginalAssert(string Message, string Filename, int LineNumber)
    {
        Assert(false, Message, Filename, LineNumber);
    }

    internal static void SetMainCursor(IGameHost Form, int Cursor)
    {
        Form.FCursor = (short)Math.Abs(Cursor);
    }

    internal static void ShowTitleScene(TitleScene scene, bool music) => scene.ShowScene(music);

}
