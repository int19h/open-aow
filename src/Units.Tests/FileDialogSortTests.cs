using Aow.Platform;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFileDialog;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWReg;
using Aow.Units.AoWWorldMap;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

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
        WithDialog((dialog, settingsDir) =>
        {
            Assert.False(Game.AoWEngine.FRegistry.GetFileDialogSortByDate());
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "aaa.asg", "zzz.asg"], Names(dialog.FileLB));
            Assert.Equal(Names(dialog.FileLB), Names(dialog.SaveLB));
            Assert.DoesNotContain("note.txt", Names(dialog.FileLB));
            Assert.Equal(-1, dialog.FileLB.Selected);

            Game.AoWEngine.FRegistry.SetFileDialogSortByDate(true);
            dialog.FileLB.Selected = 0;
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileLB));
            Assert.Equal("", dialog.DateLB.Items[0]);
            Assert.Equal("", dialog.DateLB.Items[1]);
            Assert.NotEqual("", dialog.DateLB.Items[2]);
            Assert.Equal(-1, dialog.FileLB.Selected);

            Assert.True(Runtime.SetCurrentDir("..\\two"));
            dialog.FindFiles();
            Assert.Equal(["gamma", "yyy.asg", "bbb.asg"], Names(dialog.FileLB));

            Runtime.Settings = new JsonSettingsStore(settingsDir);
            Game.AoWEngine.FRegistry = new TAoWRegistry();
            Assert.True(Game.AoWEngine.FRegistry.GetFileDialogSortByDate());
            Assert.True(Runtime.SetCurrentDir("..\\one"));
            dialog.FindFiles();
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileLB));
        });
    }

    [Fact]
    public void SortKeepsTheSelectedNameAndNullDatesStayFirst()
    {
        var older = new TFileTime { FTime = 1 };
        var newer = new TFileTime { FTime = 2 };
        Assert.Equal(-1, AoWFileDialogUnit.CompareFileDates("beta", "aaa.asg", null, older));
        Assert.Equal(1, AoWFileDialogUnit.CompareFileDates("aaa.asg", "beta", older, null));
        Assert.True(AoWFileDialogUnit.CompareFileDates("beta", "alpha", null, null) > 0);
        Assert.Equal(1, AoWFileDialogUnit.CompareFileDates("aaa.asg", "zzz.asg", older, newer));

        WithDialog((dialog, _) =>
        {
            Assert.True(Runtime.SetCurrentDir("one"));
            dialog.FindFiles();
            dialog.FileLB.Selected = dialog.FileLB.Items.IndexOf("aaa.asg");
            dialog.SortByDate();
            Assert.Equal("aaa.asg", dialog.FileLB.Items[dialog.FileLB.Selected]);
            Assert.Equal(3, dialog.FileLB.Selected);
            Assert.Equal(dialog.FileLB.Selected, dialog.SaveLB.Selected);
            Assert.Equal(dialog.FileLB.Selected, dialog.DateLB.Selected);
            Assert.Equal(["alpha", "beta", "zzz.asg", "aaa.asg"], Names(dialog.FileLB));

            // Name order copies the file rows onto FileLB, then puts the directories back on SaveLB and DateLB only.
            // The selected name stays on FileLB. SaveLB, the list the open and save dialogs show, keeps the directories first.
            dialog.SortByName();
            Assert.Equal("aaa.asg", dialog.FileLB.Items[dialog.FileLB.Selected]);
            Assert.Equal(0, dialog.FileLB.Selected);
            Assert.Equal(["aaa.asg", "zzz.asg"], Names(dialog.FileLB));
            Assert.Equal(["alpha", "beta", "aaa.asg", "zzz.asg"], Names(dialog.SaveLB));
            Assert.Equal("", dialog.DateLB.Items[0]);
            Assert.Equal("", dialog.DateLB.Items[1]);
            Assert.Equal(dialog.FileLB.Selected, dialog.SaveLB.Selected);
        });
    }

    private static string[] Names(TAOWListBox list)
    {
        var names = new string[list.Items.Count];
        for (int i = 0; i < names.Length; i++) names[i] = list.Items[i];
        return names;
    }

    private static void WithDialog(Action<TFileDlgWindow, string> body)
    {
        string directory = Path.Combine(Path.GetTempPath(), "open-aow-sort-" + Guid.NewGuid().ToString("N"));
        string root = Path.Combine(directory, "root");
        string user = Path.Combine(directory, "user");
        string settingsDir = Path.Combine(directory, "settings");
        var previousDirectories = AoWFileDialogUnit.Directories;
        var previousFiles = AoWFileDialogUnit.Files;
        var previousDrives = AoWFileDialogUnit.Drives;
        var previousWindow = AoWFileDialogUnit.FileDlgWindow;
        TFileDlgWindow? dialog = null;
        try
        {
            Directory.CreateDirectory(root);
            Directory.CreateDirectory(settingsDir);
            WriteTree(user);
            Runtime.Files = new LayeredFileSystem(root, user);
            Runtime.Settings = new JsonSettingsStore(settingsDir);
            Game.AoWEngine.FRegistry = new TAoWRegistry();
            AoWFileDialogUnit.Directories = new TStringList();
            AoWFileDialogUnit.Files = new TStringList();
            AoWFileDialogUnit.Drives = new TStringList();
            dialog = Dialog();
            body(dialog, settingsDir);
        }
        finally
        {
            if (dialog != null)
            {
                dialog.FileDlgWindowDestroy();
                dialog.FileLB.Destroy();
                dialog.SaveLB.Destroy();
                dialog.DateLB.Destroy();
                dialog.PathEdit.Destroy();
                dialog.SFileName.Destroy();
                dialog.FileName.Destroy();
                dialog.SPlayers.Destroy();
                dialog.SSize.Destroy();
                dialog.STowns.Destroy();
                dialog.LPlayers.Destroy();
                dialog.LSize.Destroy();
                dialog.LTowns.Destroy();
                dialog.LTurns.Destroy();
                dialog.Destroy();
            }

            AoWFileDialogUnit.Directories = previousDirectories;
            AoWFileDialogUnit.Files = previousFiles;
            AoWFileDialogUnit.Drives = previousDrives;
            AoWFileDialogUnit.FileDlgWindow = previousWindow;
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

    private static TFileDlgWindow Dialog()
    {
        var dialog = new TFileDlgWindow(null);
        dialog.FMode = 0;
        dialog.FileLB = new TAOWListBox(null);
        dialog.SaveLB = new TAOWListBox(null);
        dialog.DateLB = new TAOWListBox(null);
        dialog.PathEdit = new TAOWEdit(null);
        dialog.SFileName = new TAOWEdit(null);
        dialog.FileName = new TAOWEdit(null);
        dialog.SPlayers = new TAOWLabel(null);
        dialog.SSize = new TAOWLabel(null);
        dialog.STowns = new TAOWLabel(null);
        dialog.LPlayers = new TAOWLabel(null);
        dialog.LSize = new TAOWLabel(null);
        dialog.LTowns = new TAOWLabel(null);
        dialog.LTurns = new TAOWLabel(null);
        return dialog;
    }
}
