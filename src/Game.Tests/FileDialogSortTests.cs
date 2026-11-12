
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Dialogs;
using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// The open and save dialogs remember the last sort button. Name order is the default, directories
/// stay first in both orders, and the choice survives a new settings store over the same file.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class FileDialogSortTests
{
    [Fact]
    public void SortOrderSurvivesReopenDirectoryChangeAndANewSettingsStore()
    {
        WithDialog((dialog, settingsDirectory) =>
        {
            Assert.False(GameTables.GameEngine.Registry.GetFileDialogSortByDate());
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "aaa.asg", "zzz.asg"], Names(dialog.FileList));
            Assert.Equal(Names(dialog.FileList), Names(dialog.SaveList));
            Assert.DoesNotContain("note.txt", Names(dialog.FileList));
            Assert.Equal(-1, dialog.FileList.Selected);

            GameTables.GameEngine.Registry.SetFileDialogSortByDate(true);
            dialog.FileList.Selected = 0;
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileList));
            Assert.Equal("", dialog.DateList.Items[0]);
            Assert.Equal("", dialog.DateList.Items[1]);
            Assert.NotEqual("", dialog.DateList.Items[2]);
            Assert.Equal(-1, dialog.FileList.Selected);

            Assert.True(Runtime.SetCurrentDir("..\\two"));
            dialog.FindFiles();
            Assert.Equal(["gamma", "yyy.asg", "bbb.asg"], Names(dialog.FileList));

            Runtime.Settings = new JsonSettingsStore(settingsDirectory);
            GameTables.GameEngine.Registry = new GameSettings();
            Assert.True(GameTables.GameEngine.Registry.GetFileDialogSortByDate());
            Assert.True(Runtime.SetCurrentDir("..\\one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileList));
        });
    }

    [Fact]
    public void SortKeepsTheSelectedNameAndNullDatesStayFirst()
    {
        var older = new FileTime { Time = 1 };
        var newer = new FileTime { Time = 2 };
        Assert.Equal(-1, FileDialogWindow.CompareFileDates("beta", "aaa.asg", null, older));
        Assert.Equal(1, FileDialogWindow.CompareFileDates("aaa.asg", "beta", older, null));
        Assert.True(FileDialogWindow.CompareFileDates("beta", "alpha", null, null) > 0);
        Assert.Equal(1, FileDialogWindow.CompareFileDates("aaa.asg", "zzz.asg", older, newer));

        WithDialog((dialog, _) =>
        {
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            dialog.FileList.Selected = dialog.FileList.Items.IndexOf("aaa.asg");
            dialog.SortByDate();
            Assert.Equal("aaa.asg", dialog.FileList.Items[dialog.FileList.Selected]);
            Assert.Equal(3, dialog.FileList.Selected);
            Assert.Equal(dialog.FileList.Selected, dialog.SaveList.Selected);
            Assert.Equal(dialog.FileList.Selected, dialog.DateList.Selected);
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileList));

            // Name order copies the file rows onto FileLB, then puts the directories back on SaveLB and DateLB only.
            // The selected name stays on FileLB. SaveLB, the list the open and save dialogs show, keeps the directories first.
            dialog.SortByName();
            Assert.Equal("aaa.asg", dialog.FileList.Items[dialog.FileList.Selected]);
            Assert.Equal(0, dialog.FileList.Selected);
            Assert.Equal(["aaa.asg", "zzz.asg"], Names(dialog.FileList));
            Assert.Equal(["alpha", "beta", "aaa.asg", "zzz.asg"], Names(dialog.SaveList));
            Assert.Equal("", dialog.DateList.Items[0]);
            Assert.Equal("", dialog.DateList.Items[1]);
            Assert.Equal(dialog.FileList.Selected, dialog.SaveList.Selected);
        });
    }

    [Fact]
    public void MapPickerKeepsNameOrderAndTheFilledMapNameWhenDateIsRemembered()
    {
        WithDialog((dialog, _) =>
        {
            GameTables.GameEngine.Registry.SetFileDialogSortByDate(true);
            dialog.Mode = 2;
            dialog.FileNameEdit.Text = "valley.map";
            dialog.SaveFileNameEdit.Text = "stale.asg";
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "aaa.asg", "zzz.asg"], Names(dialog.FileList));
            Assert.Equal("valley.map", dialog.FileNameEdit.Text);
        });
    }

    private static string[] Names(ListBox list)
    {
        var names = new string[list.Items.Count];
        for (int i = 0; i < names.Length; i++) names[i] = list.Items[i];
        return names;
    }

    private static void WithDialog(Action<FileDialogWindow, string> body)
    {
        string directory = Path.Combine(Path.GetTempPath(), "open-aow-sort-" + Guid.NewGuid().ToString("N"));
        string root = Path.Combine(directory, "root");
        string user = Path.Combine(directory, "user");
        string settingsDirectory = Path.Combine(directory, "settings");
        var previousDirectories = FileDialogWindow.Directories;
        var previousFiles = FileDialogWindow.Files;
        var previousDrives = FileDialogWindow.Drives;
        FileDialogWindow? dialog = null;
        try
        {
            Directory.CreateDirectory(root);
            Directory.CreateDirectory(settingsDirectory);
            WriteTree(user);
            Runtime.Files = new LayeredFileSystem(root, user);
            Runtime.Settings = new JsonSettingsStore(settingsDirectory);
            GameTables.GameEngine.Registry = new GameSettings();
            FileDialogWindow.Directories = new StringList();
            FileDialogWindow.Files = new StringList<FileTime>();
            FileDialogWindow.Drives = new StringList();
            dialog = Dialog();
            body(dialog, settingsDirectory);
        }
        finally
        {
            if (dialog != null)
            {
                dialog.FileDialogWindowDestroy();
                dialog.FileList.Dispose();
                dialog.SaveList.Dispose();
                dialog.DateList.Dispose();
                dialog.PathEdit.Dispose();
                dialog.SaveFileNameEdit.Dispose();
                dialog.FileNameEdit.Dispose();
                dialog.ScenarioPlayersLabel.Dispose();
                dialog.ScenarioSizeLabel.Dispose();
                dialog.ScenarioTownsLabel.Dispose();
                dialog.SavedPlayersLabel.Dispose();
                dialog.SavedSizeLabel.Dispose();
                dialog.SavedTownsLabel.Dispose();
                dialog.SavedTurnsLabel.Dispose();
                dialog.Dispose();
            }

            FileDialogWindow.Directories = previousDirectories;
            FileDialogWindow.Files = previousFiles;
            FileDialogWindow.Drives = previousDrives;
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }
    }

    private static void WriteTree(string user)
    {
        Directory.CreateDirectory(Path.Combine(user, "one", "alpha"));
        Directory.CreateDirectory(Path.Combine(user, "one", "beta"));
        Directory.CreateDirectory(Path.Combine(user, "two", "gamma"));
        Write("one", "aaa.asg", new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        Write("one", "zzz.asg", new DateTime(2001, 6, 1, 0, 0, 0, DateTimeKind.Utc));
        Write("one", "note.txt", new DateTime(2002, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        Write("two", "bbb.asg", new DateTime(1999, 1, 1, 0, 0, 0, DateTimeKind.Utc));
        Write("two", "yyy.asg", new DateTime(2001, 6, 1, 0, 0, 0, DateTimeKind.Utc));
        return;

        void Write(string folder, string name, DateTime stamp)
        {
            string path = Path.Combine(user, folder, name);
            File.WriteAllBytes(path, [1]);
            File.SetLastWriteTimeUtc(path, stamp);
        }
    }

    private static FileDialogWindow Dialog()
    {
        var ui = new UIContext();
        var dialog = ui.Add(new FileDialogWindow(ui));
        dialog.Mode = 0;
        dialog.FileList = new ListBox<FileTime>(null);
        dialog.SaveList = new ListBox<FileTime>(null);
        dialog.DateList = new ListBox<FileTime>(null);
        dialog.PathEdit = new Edit(null);
        dialog.SaveFileNameEdit = new Edit(null);
        dialog.FileNameEdit = new Edit(null);
        dialog.ScenarioPlayersLabel = new Label(null);
        dialog.ScenarioSizeLabel = new Label(null);
        dialog.ScenarioTownsLabel = new Label(null);
        dialog.SavedPlayersLabel = new Label(null);
        dialog.SavedSizeLabel = new Label(null);
        dialog.SavedTownsLabel = new Label(null);
        dialog.SavedTurnsLabel = new Label(null);
        return dialog;
    }
}
