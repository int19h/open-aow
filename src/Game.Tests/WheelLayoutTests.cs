
using Aow.Game.Layout;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Game.UI.Windows;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// These cases load the dialogs through the layout session the game uses and drive the wheel on
/// the loaded windows. Image libraries have no files in the test host, so a missing library shows
/// no message and leaves the control without pictures.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class WheelLayoutTests : IClassFixture<WheelLayoutTests.Host>
{
    private readonly Host _host;

    public WheelLayoutTests(Host host) => this._host = host;

    public sealed class Host : IDisposable
    {
        private readonly Action<string> _previousMessage = Runtime.MessageHandler;
        private readonly UIGraphics? _previousGraphics = DialogHosts.UIGraphics;
        private readonly UIFonts? _previousFont = DialogHosts.UIFonts;
        private readonly General? _previousGeneral = DialogHosts.General;
        private readonly TitleWindow? _previousTitle = DialogHosts.TitleWindow;
        private readonly SpellBook? _previousSpell = DialogHosts.SpellBook;
        private readonly WorldMap? _previousWorld = DialogHosts.WorldMap;
        private readonly FileDialogWindow? _previousFiles = DialogHosts.FileDialogWindow;
        private readonly RealmWindow? _previousRealm = DialogHosts.RealmWindow;
        private readonly UnitBanner? _previousBanner = DialogHosts.UnitBanner;
        private readonly StringList? _previousDirectories = FileDialogWindow.Directories;
        private readonly StringList? _previousFileList = FileDialogWindow.Files;
        private readonly StringList? _previousDrives = FileDialogWindow.Drives;

        internal SpellBook Spell { get; } = null!;
        internal WorldMap World { get; } = null!;
        internal FileDialogWindow Files { get; } = null!;
        internal RealmWindow Realm { get; } = null!;

        private UIGraphics _graphics = null!;
        private UIFonts _font = null!;
        private General _general = null!;
        private TitleWindow _title = null!;
        private UnitBanner _banner = null!;

        public Host()
        {
            Runtime.MessageHandler = _ => { };
            try
            {
                var session = new LayoutSession();
                _graphics = Load(session, new UIGraphics(null), module => DialogHosts.UIGraphics = module);
                _font = Load(session, new UIFonts(null), module => DialogHosts.UIFonts = module);
                _general = Load(session, new General(null), module => DialogHosts.General = module);
                _title = Load(session, new TitleWindow(null), module => DialogHosts.TitleWindow = module);
                _banner = Load(session, new UnitBanner(null), module => DialogHosts.UnitBanner = module);
                Spell = Load(session, new SpellBook(null), module => DialogHosts.SpellBook = module);
                World = Load(session, new WorldMap(null), module => DialogHosts.WorldMap = module);
                Files = Load(session, new FileDialogWindow(null), module => DialogHosts.FileDialogWindow = module);
                Realm = Load(session, new RealmWindow(null), module => DialogHosts.RealmWindow = module);
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                Runtime.MessageHandler = _previousMessage;
            }
        }

        // Load destroys a module whose layout throws, so the field is set only after Load returns.
        private static T Load<T>(LayoutSession session, T module, Action<T?> publish) where T : Component, ILayoutModule
        {
            session.Load(module, publish);
            return module;
        }

        public void Dispose()
        {
            try
            {
                Spell?.Dispose();
                World?.Dispose();
                Files?.Dispose();
                Realm?.Dispose();
                _title?.Dispose();
                _banner?.Dispose();
                _general?.Dispose();
                _font?.Dispose();
                _graphics?.Dispose();
            }
            finally
            {
                DialogHosts.UIGraphics = _previousGraphics;
                DialogHosts.UIFonts = _previousFont;
                DialogHosts.General = _previousGeneral;
                DialogHosts.TitleWindow = _previousTitle;
                DialogHosts.SpellBook = _previousSpell;
                DialogHosts.WorldMap = _previousWorld;
                DialogHosts.FileDialogWindow = _previousFiles;
                DialogHosts.RealmWindow = _previousRealm;
                DialogHosts.UnitBanner = _previousBanner;
                FileDialogWindow.Directories = _previousDirectories;
                FileDialogWindow.Files = _previousFileList;
                FileDialogWindow.Drives = _previousDrives;
                Runtime.MessageHandler = _previousMessage;
            }
        }
    }

    private void Show(Window window)
    {
        _host.Spell.Dlg.SetVisibleWithoutUpdate(false);
        _host.World.BookWin.SetVisibleWithoutUpdate(false);
        _host.Files.FileDialog.SetVisibleWithoutUpdate(false);
        _host.Realm.Dlg.SetVisibleWithoutUpdate(false);
        DialogHosts.TitleWindow.TitleWin.SetVisibleWithoutUpdate(false);
        window.SetVisibleWithoutUpdate(true);
    }

    [Fact]
    public void SpellBookTurnsOnePagePerEventAndStopsAtTheEnds()
    {
        Show(_host.Spell.Dlg);
        var book = _host.Spell;
        book.Mode = 4;
        book.Page = 0;
        book.PageKind = 0;
        book.PrevBtn.SetVisibleWithoutUpdate(true);
        book.NextBtn.SetVisibleWithoutUpdate(true);
        book.PrevBtn.CaptureInput = true;
        book.NextBtn.CaptureInput = true;
        book.Dlg.Animating = false;
        Assert.Same(book.PrevBtn, book.Dlg.PageBack);
        Assert.Same(book.NextBtn, book.Dlg.PageForward);
        int x = book.Dlg.WinLeft + 8;
        int y = book.Dlg.WinTop + 200;
        Set<ShiftState> shift = default;

        Assert.True(DialogHosts.General.Manager.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(1, book.Page);
        Assert.True(DialogHosts.General.Manager.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.Page);
        book.NextBtn.SetVisibleWithoutUpdate(false);
        Assert.False(book.Dlg.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.Page);

        book.PrevBtn.SetVisibleWithoutUpdate(true);
        book.PrevBtn.CaptureInput = true;
        Assert.True(book.Dlg.CheckMouseWheel(5, shift, x, y));
        Assert.Equal(1, book.Page);
        book.PrevBtn.SetVisibleWithoutUpdate(false);
        Assert.False(book.Dlg.CheckMouseWheel(1, shift, x, y));
        Assert.Equal(1, book.Page);
    }

    [Fact]
    public void WorldMapBookScrollsALongIntroAndTurnsThePageOverEmptySpace()
    {
        Show(_host.World.BookWin);
        var map = _host.World;
        map.BookAction = 0;
        map.Back.CaptureInput = true;
        map.Next.CaptureInput = true;
        map.MemoL.Items.Text = new string('a', 40) + " " + new string('b', 40);
        map.leftSB.SetMaxUnclamped(40);
        map.leftSB.SetNumberVisibleUnclamped(5);
        map.leftSB.SetPositionUnclamped(0);
        map.leftSB.SetVisibleWithoutUpdate(true);
        Assert.Same(map.Back, map.BookWin.PageBack);
        Assert.Same(map.Next, map.BookWin.PageForward);
        int x = map.BookWin.WinLeft + map.MemoL.WinLeft + 20;
        int y = map.BookWin.WinTop + map.MemoL.WinTop + 40;
        Set<ShiftState> shift = default;

        Assert.True(map.BookWin.CheckMouseWheel(-3, shift, x, y));
        Assert.Equal(3, map.leftSB.Position);
        Assert.Equal(0, map.BookAction);

        map.leftSB.SetMaxUnclamped(0);
        map.leftSB.SetNumberVisibleUnclamped(1);
        map.leftSB.SetPositionUnclamped(0);
        map.leftSB.SetVisibleWithoutUpdate(false);
        Assert.True(map.BookWin.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(0, map.leftSB.Position);
        Assert.Equal(2, map.BookAction);

        map.BookAction = 0;
        map.Next.CaptureInput = true;
        int emptyX = map.BookWin.WinLeft + 300;
        int emptyY = map.BookWin.WinTop + 20;
        Assert.True(map.BookWin.CheckMouseWheel(-1, shift, emptyX, emptyY));
        Assert.Equal(2, map.BookAction);
    }

    [Fact]
    public void FileDialogSaveListAndDateListScrollTogether()
    {
        Show(_host.Files.FileDialog);
        var dialog = _host.Files;
        Assert.Same(dialog.SSBar, dialog.SaveLB.WheelScrollBar);
        Assert.Same(dialog.SSBar, dialog.DateLB.VerticalScrollBar);
        dialog.SaveLB.Items.Clear();
        dialog.DateLB.Items.Clear();
        for (int i = 0; i < 8; i++)
        {
            dialog.SaveLB.Items.Add("save" + i);
            dialog.DateLB.Items.Add("date" + i);
        }

        dialog.SSBar.SetPositionUnclamped(0);
        int x = dialog.FileDialog.WinLeft + dialog.SavePnl.WinLeft + dialog.SaveLBFr.WinLeft + dialog.SaveLB.WinLeft + 4;
        int y = dialog.FileDialog.WinTop + dialog.SavePnl.WinTop + dialog.SaveLBFr.WinTop + dialog.SaveLB.WinTop + 8;
        Set<ShiftState> shift = default;
        Assert.True(dialog.FileDialog.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(1, dialog.SSBar.Position);
        Assert.Equal(1, dialog.SaveLB.ListOffset);
        dialog.DateLB.Update();
        Assert.Equal(1, dialog.DateLB.ListOffset);

        int dateX = dialog.FileDialog.WinLeft + dialog.SavePnl.WinLeft + dialog.SaveLBFr.WinLeft + dialog.DateLB.WinLeft + 4;
        Assert.True(dialog.FileDialog.CheckMouseWheel(-1, shift, dateX, y));
        Assert.Equal(2, dialog.SSBar.Position);
        Assert.Equal(2, dialog.SaveLB.ListOffset);
        dialog.DateLB.Update();
        Assert.Equal(2, dialog.DateLB.ListOffset);
    }

    [Fact]
    public void RealmColumnListScrollsTheSharedBar()
    {
        Show(_host.Realm.Dlg);
        var realm = _host.Realm;
        realm.T2Pnl.SetVisibleWithoutUpdate(true);
        Assert.Same(realm.CitySB, realm.T2NameLB.WheelScrollBar);
        realm.T2NameLB.Items.Clear();
        for (int i = 0; i < 12; i++) realm.T2NameLB.Items.Add("city" + i);
        realm.CitySB.SetPositionUnclamped(0);
        int x = realm.Dlg.WinLeft + realm.T2Pnl.WinLeft + realm.T2LBPnl.WinLeft + realm.T2NameLB.WinLeft + 4;
        int y = realm.Dlg.WinTop + realm.T2Pnl.WinTop + realm.T2LBPnl.WinTop + realm.T2NameLB.WinTop + 8;
        Assert.True(realm.Dlg.CheckMouseWheel(-1, default, x, y));
        Assert.Equal(1, realm.CitySB.Position);
        Assert.Equal(1, realm.T2NameLB.ListOffset);
    }

    [Fact]
    public void MapPickerKeepsNameOrderAndTheFilledMapName()
    {
        Show(_host.Files.FileDialog);
        var dialog = _host.Files;
        string directory = Path.Combine(Path.GetTempPath(), "open-aow-map-" + Guid.NewGuid().ToString("N"));
        string root = Path.Combine(directory, "root");
        string user = Path.Combine(directory, "user");
        string settingsDirectory = Path.Combine(directory, "settings");
        try
        {
            Directory.CreateDirectory(root);
            Directory.CreateDirectory(settingsDirectory);
            Directory.CreateDirectory(Path.Combine(user, "one"));
            string older = Path.Combine(user, "one", "aaa.asg");
            string newer = Path.Combine(user, "one", "zzz.asg");
            File.WriteAllBytes(older, [1]);
            File.WriteAllBytes(newer, [1]);
            File.SetLastWriteTimeUtc(older, new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            File.SetLastWriteTimeUtc(newer, new DateTime(2001, 6, 1, 0, 0, 0, DateTimeKind.Utc));
            Directory.CreateDirectory(Path.Combine(user, "one", "alpha"));
            Runtime.Files = new LayeredFileSystem(root, user);
            Runtime.Settings = new JsonSettingsStore(settingsDirectory);
            GameTables.GameEngine.Registry = new GameSettings();
            GameTables.GameEngine.Registry.SetFileDialogSortByDate(true);
            dialog.Mode = 2;
            dialog.FileName.Text = "valley.map";
            dialog.SFileName.Text = "stale.asg";
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "aaa.asg", "zzz.asg"], Names(dialog.FileLB));
            Assert.Equal("valley.map", dialog.FileName.Text);
        }
        finally
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }
    }

    private static string[] Names(ListBox list)
    {
        var names = new string[list.Items.Count];
        for (int i = 0; i < names.Length; i++) names[i] = list.Items[i];
        return names;
    }
}
