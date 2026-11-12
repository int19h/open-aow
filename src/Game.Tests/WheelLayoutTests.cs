
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
        private readonly StringList? _previousDirectories = FileDialogWindow.Directories;
        private readonly StringList<FileTime>? _previousFileList = FileDialogWindow.Files;
        private readonly StringList? _previousDrives = FileDialogWindow.Drives;

        internal UIContext UI { get; } = new();
        internal SpellBook Spell { get; } = null!;
        internal WorldMap World { get; } = null!;
        internal FileDialogWindow Files { get; } = null!;
        internal RealmWindow Realm { get; } = null!;

        public Host()
        {
            Runtime.MessageHandler = _ => { };
            try
            {
                var session = new LayoutSession();
                UI.Load(session, module: new UIGraphics());
                UI.Load(session, module: new UIFonts());
                UI.Load(session, module: new General(UI));
                UI.Load(session, module: new TitleWindow(UI));
                UI.Load(session, module: new UnitBanner(UI));
                Spell = UI.Load(session, module: new SpellBook(UI));
                World = UI.Load(session, module: new WorldMap(UI));
                Files = UI.Load(session, module: new FileDialogWindow(UI));
                Realm = UI.Load(session, module: new RealmWindow(UI));
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

        public void Dispose()
        {
            try
            {
                UI.SpellBook?.Dispose();
                UI.WorldMap?.Dispose();
                UI.FileDialogWindow?.Dispose();
                UI.RealmWindow?.Dispose();
                UI.TitleWindow?.Dispose();
                UI.UnitBanner?.Dispose();
                UI.General?.Dispose();
                UI.Dispose();
            }
            finally
            {
                FileDialogWindow.Directories = _previousDirectories;
                FileDialogWindow.Files = _previousFileList;
                FileDialogWindow.Drives = _previousDrives;
                Runtime.MessageHandler = _previousMessage;
            }
        }
    }

    private void Show(Window window)
    {
        _host.Spell.Book.SetVisibleWithoutUpdate(false);
        _host.World.Book.SetVisibleWithoutUpdate(false);
        _host.Files.Window.SetVisibleWithoutUpdate(false);
        _host.Realm.Window.SetVisibleWithoutUpdate(false);
        _host.UI.TitleWindow.MainWindow.SetVisibleWithoutUpdate(false);
        window.SetVisibleWithoutUpdate(true);
    }

    [Fact]
    public void SpellBookTurnsOnePagePerEventAndStopsAtTheEnds()
    {
        Show(_host.Spell.Book);
        var book = _host.Spell;
        book.Mode = 4;
        book.Page = 0;
        book.PageKind = 0;
        book.PreviousButton.SetVisibleWithoutUpdate(true);
        book.NextButton.SetVisibleWithoutUpdate(true);
        book.PreviousButton.CaptureInput = true;
        book.NextButton.CaptureInput = true;
        book.Book.Animating = false;
        Assert.Same(book.PreviousButton, book.Book.PageBack);
        Assert.Same(book.NextButton, book.Book.PageForward);
        int x = book.Book.WinLeft + 8;
        int y = book.Book.WinTop + 200;
        Set<ShiftState> shift = default;

        Assert.True(_host.UI.General.Manager.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(1, book.Page);
        Assert.True(_host.UI.General.Manager.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.Page);
        book.NextButton.SetVisibleWithoutUpdate(false);
        Assert.False(book.Book.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(2, book.Page);

        book.PreviousButton.SetVisibleWithoutUpdate(true);
        book.PreviousButton.CaptureInput = true;
        Assert.True(book.Book.CheckMouseWheel(5, shift, x, y));
        Assert.Equal(1, book.Page);
        book.PreviousButton.SetVisibleWithoutUpdate(false);
        Assert.False(book.Book.CheckMouseWheel(1, shift, x, y));
        Assert.Equal(1, book.Page);
    }

    [Fact]
    public void WorldMapBookScrollsALongIntroAndTurnsThePageOverEmptySpace()
    {
        Show(_host.World.Book);
        var map = _host.World;
        map.BookAction = 0;
        map.BackButton.CaptureInput = true;
        map.NextButton.CaptureInput = true;
        map.LeftPageMemo.Items.Text = new string('a', 40) + " " + new string('b', 40);
        map.LeftPageScrollBar.SetMaxUnclamped(40);
        map.LeftPageScrollBar.SetNumberVisibleUnclamped(5);
        map.LeftPageScrollBar.SetPositionUnclamped(0);
        map.LeftPageScrollBar.SetVisibleWithoutUpdate(true);
        Assert.Same(map.BackButton, map.Book.PageBack);
        Assert.Same(map.NextButton, map.Book.PageForward);
        int x = map.Book.WinLeft + map.LeftPageMemo.WinLeft + 20;
        int y = map.Book.WinTop + map.LeftPageMemo.WinTop + 40;
        Set<ShiftState> shift = default;

        Assert.True(map.Book.CheckMouseWheel(-3, shift, x, y));
        Assert.Equal(3, map.LeftPageScrollBar.Position);
        Assert.Equal(0, map.BookAction);

        map.LeftPageScrollBar.SetMaxUnclamped(0);
        map.LeftPageScrollBar.SetNumberVisibleUnclamped(1);
        map.LeftPageScrollBar.SetPositionUnclamped(0);
        map.LeftPageScrollBar.SetVisibleWithoutUpdate(false);
        Assert.True(map.Book.CheckMouseWheel(-5, shift, x, y));
        Assert.Equal(0, map.LeftPageScrollBar.Position);
        Assert.Equal(2, map.BookAction);

        map.BookAction = 0;
        map.NextButton.CaptureInput = true;
        int emptyX = map.Book.WinLeft + 300;
        int emptyY = map.Book.WinTop + 20;
        Assert.True(map.Book.CheckMouseWheel(-1, shift, emptyX, emptyY));
        Assert.Equal(2, map.BookAction);
    }

    [Fact]
    public void FileDialogSaveListAndDateListScrollTogether()
    {
        Show(_host.Files.Window);
        var dialog = _host.Files;
        Assert.Same(dialog.SaveScrollBar, dialog.SaveList.WheelScrollBar);
        Assert.Same(dialog.SaveScrollBar, dialog.DateList.VerticalScrollBar);
        dialog.SaveList.Items.Clear();
        dialog.DateList.Items.Clear();
        for (int i = 0; i < 8; i++)
        {
            dialog.SaveList.Items.Add("save" + i);
            dialog.DateList.Items.Add("date" + i);
        }

        dialog.SaveScrollBar.SetPositionUnclamped(0);
        int x = dialog.Window.WinLeft + dialog.SavePanel.WinLeft + dialog.SaveListFrame.WinLeft + dialog.SaveList.WinLeft + 4;
        int y = dialog.Window.WinTop + dialog.SavePanel.WinTop + dialog.SaveListFrame.WinTop + dialog.SaveList.WinTop + 8;
        Set<ShiftState> shift = default;
        Assert.True(dialog.Window.CheckMouseWheel(-1, shift, x, y));
        Assert.Equal(1, dialog.SaveScrollBar.Position);
        Assert.Equal(1, dialog.SaveList.ListOffset);
        dialog.DateList.Update();
        Assert.Equal(1, dialog.DateList.ListOffset);

        int dateX = dialog.Window.WinLeft + dialog.SavePanel.WinLeft + dialog.SaveListFrame.WinLeft + dialog.DateList.WinLeft + 4;
        Assert.True(dialog.Window.CheckMouseWheel(-1, shift, dateX, y));
        Assert.Equal(2, dialog.SaveScrollBar.Position);
        Assert.Equal(2, dialog.SaveList.ListOffset);
        dialog.DateList.Update();
        Assert.Equal(2, dialog.DateList.ListOffset);
    }

    [Fact]
    public void RealmColumnListScrollsTheSharedBar()
    {
        Show(_host.Realm.Window);
        var realm = _host.Realm;
        realm.CityPanel.SetVisibleWithoutUpdate(true);
        Assert.Same(realm.CityScrollBar, realm.CityNameList.WheelScrollBar);
        realm.CityNameList.Items.Clear();
        for (int i = 0; i < 12; i++) realm.CityNameList.Items.Add("city" + i);
        realm.CityScrollBar.SetPositionUnclamped(0);
        int x = realm.Window.WinLeft + realm.CityPanel.WinLeft + realm.CityListPanel.WinLeft + realm.CityNameList.WinLeft + 4;
        int y = realm.Window.WinTop + realm.CityPanel.WinTop + realm.CityListPanel.WinTop + realm.CityNameList.WinTop + 8;
        Assert.True(realm.Window.CheckMouseWheel(-1, default, x, y));
        Assert.Equal(1, realm.CityScrollBar.Position);
        Assert.Equal(1, realm.CityNameList.ListOffset);
    }

    [Fact]
    public void MapPickerKeepsNameOrderAndTheFilledMapName()
    {
        Show(_host.Files.Window);
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
            dialog.FileNameEdit.Text = "valley.map";
            dialog.SaveFileNameEdit.Text = "stale.asg";
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "aaa.asg", "zzz.asg"], Names(dialog.FileList));
            Assert.Equal("valley.map", dialog.FileNameEdit.Text);
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
