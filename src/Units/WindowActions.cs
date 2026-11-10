using Aow.Game.Combat;
using Aow.Game.Engine.Buffers;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public static class WindowActions
{
    internal static void RestoreWindowLayout(object Controller)
    {
        global::Aow.Rtl.Rect R;
        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(6);
        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(0);
        var playerInfo = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(2);
        var unitWindow = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin;
        if (R.Right != 0)
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Left;
            alignment.AlignHeight = AOWAlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            unitWindow.WinWidth = R.Right;
            unitWindow.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = unitWindow.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Center;
            alignment.AlignHeight = AOWAlignHeight.Center;

            unitWindow.WinWidth = unitWindow.MinWidth;
            unitWindow.WinHeight = unitWindow.MinHeight;
        }

        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(3);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(4);
        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(5);
        var realm = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg;
        if (R.Right != 0)
        {
            var alignment = realm.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Left;
            alignment.AlignHeight = AOWAlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            realm.WinWidth = R.Right;
            realm.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = realm.Alignment;
            alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AOWAlignWidth.Both;
            alignment.AlignHeight = AOWAlignHeight.Bottom;

            realm.WinWidth = realm.MinWidth;
            realm.WinHeight = realm.MinHeight;
        }

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(13);
        var magic = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg;
        if (R.Right != 0)
        {
            var alignment = magic.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Left;
            alignment.AlignHeight = AOWAlignHeight.Top;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            magic.WinWidth = R.Right;
            magic.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = magic.Alignment;
            alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.WinWidth;
            alignment.AlignWidth = AOWAlignWidth.Both;
            alignment.AlignHeight = AOWAlignHeight.Bottom;

            magic.WinWidth = magic.MinWidth;
            magic.WinHeight = magic.MinHeight;
        }

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(1);
        var controls = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(7);
        var info = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(8);
        var chat = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin;
        if (R.Right != 0)
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Left;
            alignment.BottomOffset = R.Top;
            alignment.LeftOffset = R.Left;

            chat.WinWidth = R.Right;
            chat.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = chat.Alignment;
            alignment.AlignWidth = AOWAlignWidth.Both;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;

            chat.WinWidth = chat.MinWidth;
            chat.WinHeight = chat.MinHeight;
        }

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(12);
        var tcScanner = Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(10);
        var tcUnit = Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin;
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

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(11);
        if (R.Right != 0)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect = R;
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight = AOWAlignHeight.Top;
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth = AOWAlignWidth.Left;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight = AOWAlignHeight.Both;
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth = AOWAlignWidth.Both;
            var rect = global::Aow.Rtl.RtlUnit.Rect(0, 0, tcScanner.MinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight);
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect = rect;
        }

        R = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetRectangle(9);
        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (R.Right != 0)
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = AOWAlignHeight.Top;
            alignment.AlignWidth = AOWAlignWidth.Left;
            alignment.TopOffset = R.Top;
            alignment.LeftOffset = R.Left;

            map.WinWidth = R.Right;
            map.WinHeight = R.Bottom;
        }
        else
        {
            var alignment = map.Alignment;
            alignment.AlignHeight = AOWAlignHeight.Both;
            alignment.AlignWidth = AOWAlignWidth.Both;
            alignment.TopOffset = 0;
            alignment.LeftOffset = 0;
            alignment.RightOffset = scanner.MinWidth;
            alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
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
