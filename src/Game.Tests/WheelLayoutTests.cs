
using Aow.Game.Layout;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
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
    private readonly Host host;

    public WheelLayoutTests(Host host) => this.host = host;

    public sealed class Host : IDisposable
    {
        private readonly Action<string> previousMessage = Runtime.MessageHandler;
        private readonly IntGfxMod? previousGfx = DialogHosts.IntGfxMod;
        private readonly FontModule? previousFont = DialogHosts.FontModule;
        private readonly General? previousGeneral = DialogHosts.General;
        private readonly TitleWindow? previousTitle = DialogHosts.TitleWindow;
        private readonly SpellBook? previousSpell = DialogHosts.SpellBook;
        private readonly WorldMap? previousWorld = DialogHosts.WorldMap;
        private readonly FileDlgWindow? previousFiles = DialogHosts.FileDlgWindow;
        private readonly RealmWin? previousRealm = DialogHosts.RealmWin;
        private readonly UnitBanner? previousBanner = DialogHosts.UnitBanner;
        private readonly StringList? previousDirectories = FileDlgWindow.Directories;
        private readonly StringList? previousFileList = FileDlgWindow.Files;
        private readonly StringList? previousDrives = FileDlgWindow.Drives;

        internal SpellBook Spell = null!;
        internal WorldMap World = null!;
        internal FileDlgWindow Files = null!;
        internal RealmWin Realm = null!;
        private IntGfxMod gfx = null!;
        private FontModule font = null!;
        private General general = null!;
        private TitleWindow title = null!;
        private UnitBanner banner = null!;

        public Host()
        {
            Runtime.MessageHandler = _ => { };
            try
            {
                var session = new LayoutSession();
                gfx = Load(session, new IntGfxMod(null), module => DialogHosts.IntGfxMod = module);
                font = Load(session, new FontModule(null), module => DialogHosts.FontModule = module);
                general = Load(session, new General(null), module => DialogHosts.General = module);
                title = Load(session, new TitleWindow(null), module => DialogHosts.TitleWindow = module);
                banner = Load(session, new UnitBanner(null), module => DialogHosts.UnitBanner = module);
                Spell = Load(session, new SpellBook(null), module => DialogHosts.SpellBook = module);
                World = Load(session, new WorldMap(null), module => DialogHosts.WorldMap = module);
                Files = Load(session, new FileDlgWindow(null), module => DialogHosts.FileDlgWindow = module);
                Realm = Load(session, new RealmWin(null), module => DialogHosts.RealmWin = module);
            }
            catch
            {
                Dispose();
                throw;
            }
            finally
            {
                Runtime.MessageHandler = previousMessage;
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
                title?.Dispose();
                banner?.Dispose();
                general?.Dispose();
                font?.Dispose();
                gfx?.Dispose();
            }
            finally
            {
                DialogHosts.IntGfxMod = previousGfx;
                DialogHosts.FontModule = previousFont;
                DialogHosts.General = previousGeneral;
                DialogHosts.TitleWindow = previousTitle;
                DialogHosts.SpellBook = previousSpell;
                DialogHosts.WorldMap = previousWorld;
                DialogHosts.FileDlgWindow = previousFiles;
                DialogHosts.RealmWin = previousRealm;
                DialogHosts.UnitBanner = previousBanner;
                FileDlgWindow.Directories = previousDirectories;
                FileDlgWindow.Files = previousFileList;
                FileDlgWindow.Drives = previousDrives;
                Runtime.MessageHandler = previousMessage;
            }
        }
    }

    private void Show(AOWWindow window)
    {
        host.Spell.Dlg.FVisible = false;
        host.World.BookWin.FVisible = false;
        host.Files.FileDialog.FVisible = false;
        host.Realm.Dlg.FVisible = false;
        DialogHosts.TitleWindow.TitleWin.FVisible = false;
        window.FVisible = true;
    }

    [Fact]
    public void SpellBookTurnsOnePagePerEventAndStopsAtTheEnds()
    {
        Show(host.Spell.Dlg);
        var book = host.Spell;
        book.FMode = 4;
        book.FPage = 0;
        book.FPageKind = 0;
        book.PrevBtn.FVisible = true;
        book.NextBtn.FVisible = true;
        book.PrevBtn.FCaptureInput = true;
        book.NextBtn.FCaptureInput = true;
        book.Dlg.FAnimating = false;
        Assert.Same(book.PrevBtn, book.Dlg.PageBack);
        Assert.Same(book.NextBtn, book.Dlg.PageForward);
        int x = book.Dlg.FWinLeft + 8;
        int y = book.Dlg.FWinTop + 200;
        Set<ShiftState> shift = default;

        Assert.True(DialogHosts.General.Manager.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(1, book.FPage);
        Assert.True(DialogHosts.General.Manager.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.FPage);
        book.NextBtn.FVisible = false;
        Assert.False(book.Dlg.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.FPage);

        book.PrevBtn.FVisible = true;
        book.PrevBtn.FCaptureInput = true;
        Assert.True(book.Dlg.CheckMouseWheel(5, shift, x, y));
        Assert.Equal(1, book.FPage);
        book.PrevBtn.FVisible = false;
        Assert.False(book.Dlg.CheckMouseWheel(1, shift, x, y));
        Assert.Equal(1, book.FPage);
    }

    [Fact]
    public void WorldMapBookScrollsALongIntroAndTurnsThePageOverEmptySpace()
    {
        Show(host.World.BookWin);
        var map = host.World;
        map.FBookAction = 0;
        map.Back.FCaptureInput = true;
        map.Next.FCaptureInput = true;
        map.MemoL.FItems.Text = new string('a', 40) + " " + new string('b', 40);
        map.leftSB.FMax = 40;
        map.leftSB.FNumberVisible = 5;
        map.leftSB.FPosition = 0;
        map.leftSB.FVisible = true;
        Assert.Same(map.Back, map.BookWin.PageBack);
        Assert.Same(map.Next, map.BookWin.PageForward);
        int x = map.BookWin.FWinLeft + map.MemoL.FWinLeft + 20;
        int y = map.BookWin.FWinTop + map.MemoL.FWinTop + 40;
        Set<ShiftState> shift = default;

        Assert.True(map.BookWin.CheckMouseWheel(-3, shift, x, y));
        Assert.Equal(3, map.leftSB.FPosition);
        Assert.Equal(0, map.FBookAction);

        map.leftSB.FMax = 0;
        map.leftSB.FNumberVisible = 1;
        map.leftSB.FPosition = 0;
        map.leftSB.FVisible = false;
        Assert.True(map.BookWin.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(0, map.leftSB.FPosition);
        Assert.Equal(2, map.FBookAction);

        map.FBookAction = 0;
        map.Next.FCaptureInput = true;
        int emptyX = map.BookWin.FWinLeft + 300;
        int emptyY = map.BookWin.FWinTop + 20;
        Assert.True(map.BookWin.CheckMouseWheel(-1, shift, emptyX, emptyY));
        Assert.Equal(2, map.FBookAction);
    }

    [Fact]
    public void FileDialogSaveListAndDateListScrollTogether()
    {
        Show(host.Files.FileDialog);
        var dialog = host.Files;
        Assert.Same(dialog.SSBar, dialog.SaveLB.WheelScrollBar);
        Assert.Same(dialog.SSBar, dialog.DateLB.VScrollBar);
        dialog.SaveLB.Items.Clear();
        dialog.DateLB.Items.Clear();
        for (int i = 0; i < 8; i++)
        {
            dialog.SaveLB.Items.Add("save" + i);
            dialog.DateLB.Items.Add("date" + i);
        }

        dialog.SSBar.FPosition = 0;
        int x = dialog.FileDialog.FWinLeft + dialog.SavePnl.FWinLeft + dialog.SaveLBFr.FWinLeft + dialog.SaveLB.FWinLeft + 4;
        int y = dialog.FileDialog.FWinTop + dialog.SavePnl.FWinTop + dialog.SaveLBFr.FWinTop + dialog.SaveLB.FWinTop + 8;
        Set<ShiftState> shift = default;
        Assert.True(dialog.FileDialog.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(1, dialog.SSBar.FPosition);
        Assert.Equal(1, dialog.SaveLB.ListOffset);
        dialog.DateLB.Update();
        Assert.Equal(1, dialog.DateLB.ListOffset);

        int dateX = dialog.FileDialog.FWinLeft + dialog.SavePnl.FWinLeft + dialog.SaveLBFr.FWinLeft + dialog.DateLB.FWinLeft + 4;
        Assert.True(dialog.FileDialog.CheckMouseWheel(-1, shift, dateX, y));
        Assert.Equal(2, dialog.SSBar.FPosition);
        Assert.Equal(2, dialog.SaveLB.ListOffset);
        dialog.DateLB.Update();
        Assert.Equal(2, dialog.DateLB.ListOffset);
    }

    [Fact]
    public void RealmColumnListScrollsTheSharedBar()
    {
        Show(host.Realm.Dlg);
        var realm = host.Realm;
        realm.T2Pnl.FVisible = true;
        Assert.Same(realm.CitySB, realm.T2NameLB.WheelScrollBar);
        realm.T2NameLB.Items.Clear();
        for (int i = 0; i < 12; i++) realm.T2NameLB.Items.Add("city" + i);
        realm.CitySB.FPosition = 0;
        int x = realm.Dlg.FWinLeft + realm.T2Pnl.FWinLeft + realm.T2LBPnl.FWinLeft + realm.T2NameLB.FWinLeft + 4;
        int y = realm.Dlg.FWinTop + realm.T2Pnl.FWinTop + realm.T2LBPnl.FWinTop + realm.T2NameLB.FWinTop + 8;
        Assert.True(realm.Dlg.CheckMouseWheel(-1, default, x, y));
        Assert.Equal(1, realm.CitySB.FPosition);
        Assert.Equal(1, realm.T2NameLB.ListOffset);
    }

    [Fact]
    public void MapPickerKeepsNameOrderAndTheFilledMapName()
    {
        Show(host.Files.FileDialog);
        var dialog = host.Files;
        string directory = Path.Combine(Path.GetTempPath(), "open-aow-map-" + Guid.NewGuid().ToString("N"));
        string root = Path.Combine(directory, "root");
        string user = Path.Combine(directory, "user");
        string settingsDir = Path.Combine(directory, "settings");
        try
        {
            Directory.CreateDirectory(root);
            Directory.CreateDirectory(settingsDir);
            Directory.CreateDirectory(Path.Combine(user, "one"));
            string older = Path.Combine(user, "one", "aaa.asg");
            string newer = Path.Combine(user, "one", "zzz.asg");
            File.WriteAllBytes(older, [1]);
            File.WriteAllBytes(newer, [1]);
            File.SetLastWriteTimeUtc(older, new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            File.SetLastWriteTimeUtc(newer, new DateTime(2001, 6, 1, 0, 0, 0, DateTimeKind.Utc));
            Directory.CreateDirectory(Path.Combine(user, "one", "alpha"));
            Runtime.Files = new LayeredFileSystem(root, user);
            Runtime.Settings = new JsonSettingsStore(settingsDir);
            GameTables.AoWEngine.FRegistry = new AoWRegistry();
            GameTables.AoWEngine.FRegistry.SetFileDialogSortByDate(true);
            dialog.FMode = 2;
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

    private static string[] Names(AOWListBox list)
    {
        var names = new string[list.Items.Count];
        for (int i = 0; i < names.Length; i++) names[i] = list.Items[i];
        return names;
    }
}
