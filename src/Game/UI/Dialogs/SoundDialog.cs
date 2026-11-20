#nullable disable
// Translated from AoWSoundDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Dialogs;

// sound quality, sound settings (rate, music, DirectSound, window, Sfx, voices)
public partial class SoundDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public ListBox SongList { get; private set; }
    public Panel PlayListPanel { get; private set; }
    public ListBox PlayList { get; private set; }
    public Button AddButton { get; private set; }
    public Button RemoveButton { get; private set; }
    public Button PlayButton { get; private set; }
    public Label CurrentSongLabel { get; private set; }
    public Panel MainPanel { get; private set; }
    public CheckBox SoundQualityNoneCheckBox { get; private set; }
    public CheckBox SoundQualityLowCheckBox { get; private set; }
    public CheckBox SoundQualityMediumCheckBox { get; private set; }
    public CheckBox SoundQualityHighCheckBox { get; private set; }
    public CheckBox MusicEnabledCheckBox { get; private set; }
    public CheckBox EffectsEnabledCheckBox { get; private set; }
    public HorizontalScrollBar MusicVolumeSlider { get; private set; }
    public HorizontalScrollBar EffectsVolumeSlider { get; private set; }
    public VerticalScrollBar SongScrollBar { get; private set; }
    public CheckBox DirectSoundEnabledCheckBox { get; private set; }

    public int SoundQuality { get; set; }

    public SoundSettings Settings { get; internal set; }

    // AoWSoundDlg.pas:108
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWSoundDlg.pas:114
    public void MusicVolumeChange() => AudioPlayback.SetMusicVolume(MusicVolumeSlider.Position);

    // AoWSoundDlg.pas:119
    public void SFXVolumeChange() => AudioPlayback.SetSfxVolume(EffectsVolumeSlider.Position);

    // AoWSoundDlg.pas:124
    public void RemoveBtnClick()
    {
        UI.General.PlayButtonSound(0);
        AudioHost.SongPlayer.Songs.Delete(PlayList.Selected);
        if (AudioHost.SongPlayer.CurrentIndex == -1)
        {
            int count = AudioHost.SongPlayer.Songs.Count;
            if (PlayList.Selected < count)
            {
                _ = AudioHost.SongPlayer.Play(PlayList.Selected);
            }
            else
            {
                if (AudioHost.SongPlayer.Songs.Count != 0)
                {
                    int songsCount = AudioHost.SongPlayer.Songs.Count;
                    _ = AudioHost.SongPlayer.Play(songsCount - 1);
                }
            }
        }

        SoundSetup();
    }

    // AoWSoundDlg.pas:134
    public void SoundSetup()
    {
        PlayList.Items.Clear();
        for (int i = 0, last = AudioHost.SongPlayer.SongNames.Count - 1; i <= last; i++)
        {
            string songName = AudioHost.SongPlayer.SongNames[i];
            _ = PlayList.Items.Add(songName);
        }

        PlayList.Selected = AudioHost.SongPlayer.CurrentIndex;
        PlayList.Sized = false;
        if (PlayList.Selected == -1)
        {
            RemoveButton.Visible = false;
            PlayButton.Visible = false;
            CurrentSongLabel.Text = "";
        }
        else
        {
            RemoveButton.Visible = true;
            PlayButton.Visible = true;
            string songName = AudioHost.SongPlayer.SongNames[PlayList.Selected];
            CurrentSongLabel.Text = songName;
        }

        MainPanel.Drawn = false;
        PlayListPanel.Drawn = false;
        MusicEnabledCheckBox.Checked = Settings.MusicEnabled;
        EffectsEnabledCheckBox.Checked = Settings.SfxEnabled;
        DirectSoundEnabledCheckBox.Checked = Settings.UseDirectSound;
        SoundQualityNoneCheckBox.Checked = SoundQuality == 0;
        SoundQualityLowCheckBox.Checked = SoundQuality == 1;
        SoundQualityMediumCheckBox.Checked = SoundQuality == 2;
        SoundQualityHighCheckBox.Checked = SoundQuality == 3;
    }

    // AoWSoundDlg.pas:165
    public void PlayBtnClick()
    {
        UI.General.PlayButtonSound(0);
        PlayList.Drawn = false;
        _ = AudioHost.SongPlayer.Play(PlayList.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        string songName = AudioHost.SongPlayer.SongNames[AudioHost.SongPlayer.CurrentIndex];
        CurrentSongLabel.Text = songName;
        MainPanel.Drawn = false;
        PlayList.Drawn = false;
    }

    // AoWSoundDlg.pas:179
    public void FindFiles()
    {
        SongList.Items.Clear();
        s_songFiles.Clear();
        _ = SetCurrentDir($"{UI.Engine.StartupDirectory}\\Songs");
        string directory = GetCurrentDir();
        if (Copy(directory, index: directory.Length, count: 1) == "\\")
        {
            directory = Copy(directory, index: 1, count: directory.Length - 1);
        }

        foreach (RtlFileEntry entry in Files.FindEntries($"{directory}\\*.*", attributes: 16))
        {
            if (entry.Name is not "." and not "..")
            {
                if (UpperCase(Copy(entry.Name, index: entry.Name.Length - 3, count: 4)) == ".S3M" || UpperCase(Copy(entry.Name, index: entry.Name.Length - 2, count: 3)) == ".IT")
                {
                    _ = s_songFiles.Add($"Songs\\{entry.Name}");
                    string songName = AudioPlayback.GetSongName($"Songs\\{entry.Name}");
                    _ = SongList.Items.Add(songName);
                }
            }
        }

        StringSorting.SortStringsAscending(SongList.Items, linkedStrings: [s_songFiles]);
        SongList.Selected = -1;
        SongList.ListOffset = 0;
        SongScrollBar.Position = 0;
    }

    // AoWSoundDlg.pas:206
    public void SongLBChange()
    {
        if (SongList.Selected > -1)
        {
            AddButton.Visible = true;
        }
    }

    // AoWSoundDlg.pas:211
    public void AddBtnClick()
    {
        UI.General.PlayButtonSound(0);
        string songFile = s_songFiles[SongList.Selected];
        _ = AudioHost.SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDialogCreate() => s_songFiles = new DelphiStringList();

    // AoWSoundDlg.pas:223
    public void SoundDialogDestroy() => s_songFiles?.Dispose();

    // AoWSoundDlg.pas:228
    public void PlayLBChange()
    {
        if (PlayList.Selected > -1)
        {
            RemoveButton.Visible = true;
            PlayButton.Visible = true;
        }
    }

    // AoWSoundDlg.pas:237
    public void SQCB1Change(CheckBox sender)
    {
        if (SoundQualityNoneCheckBox.Checked)
        {
            AudioPlayback.ReleaseSoundSystem();
            SoundQuality = 0;
            CurrentSongLabel.Text = "";
            PlayList.Drawn = false;
            MainPanel.Drawn = false;
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:251
    public void SQCB2Change(CheckBox sender)
    {
        if (SoundQualityLowCheckBox.Checked)
        {
            SoundQuality = 1;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                SoundSettings settings = Settings;
                settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                Settings = settings;
                SoundSettings settings2 = Settings;
                settings2.SampleRate = 22_050;
                Settings = settings2;
                _ = SetSoundSettings(Settings);
            }
            else
            {
                SoundSettings settings3 = Settings;
                settings3.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                Settings = settings3;
                SoundSettings settings4 = Settings;
                settings4.SampleRate = 22_050;
                Settings = settings4;
                _ = AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
            }
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:273
    public void SQCB3Change(CheckBox sender)
    {
        if (SoundQualityMediumCheckBox.Checked)
        {
            SoundQuality = 2;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                SoundSettings settings = Settings;
                settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                Settings = settings;
                SoundSettings settings2 = Settings;
                settings2.SampleRate = 44_100;
                Settings = settings2;
                _ = SetSoundSettings(Settings);
            }
            else
            {
                SoundSettings settings3 = Settings;
                settings3.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                Settings = settings3;
                SoundSettings settings4 = Settings;
                settings4.SampleRate = 44_100;
                Settings = settings4;
                _ = AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
            }
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:295
    public void SQCB4Change(CheckBox sender)
    {
        if (SoundQualityHighCheckBox.Checked)
        {
            SoundQuality = 3;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                SoundSettings settings = Settings;
                settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                Settings = settings;
                SoundSettings settings2 = Settings;
                settings2.SampleRate = 44_100;
                Settings = settings2;
                _ = SetSoundSettings(Settings);
            }
            else
            {
                SoundSettings settings3 = Settings;
                settings3.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                Settings = settings3;
                SoundSettings settings4 = Settings;
                settings4.SampleRate = 44_100;
                Settings = settings4;
                _ = AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
            }
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:317
    public void PlayLBDrawItem(in Rect R, int index, ref readonly LockedSurface lockedSurface)
    {
        if (AudioHost.SongPlayer.CurrentIndex == index)
        {
            UI.Graphics.SoundWindow[2].ShowClipped(R.Left, R.Top, lockedSurface);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(CheckBox sender)
    {
        SoundSettings settings = Settings;
        settings.MusicEnabled = MusicEnabledCheckBox.Checked;
        Settings = settings;
        _ = SetMusicEnabled(Settings.MusicEnabled);
        if (Settings.MusicEnabled && SoundQuality != 0)
        {
            _ = AudioHost.SongPlayer.Play(0);
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:331
    public void SFXEnabledChkChange(CheckBox sender)
    {
        SoundSettings settings = Settings;
        settings.SfxEnabled = EffectsEnabledCheckBox.Checked;
        Settings = settings;
        _ = SetSfxEnabled(Settings.SfxEnabled);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:339
    public void DirectSoundEnabledChkChange(CheckBox sender)
    {
        SoundSettings settings = Settings;
        settings.UseDirectSound = DirectSoundEnabledCheckBox.Checked;
        Settings = settings;
        if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
        {
            _ = SetSoundSettings(Settings);
        }

        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:347
    public void SoundTabChange() => UI.General.PlayButtonSound(0);

    // AoWSoundDlg.pas:352
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static DelphiStringList s_songFiles;

    private static Set<SoundSystemStatusElement> SoundSystemStatus()
    {
        return AudioHost.SoundSystem is not null
            ? AudioHost.SoundSystem.Status
            : Set<SoundSystemStatusElement>.Of([]);
    }

    private static bool SetMusicEnabled(bool value) => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SetMusicEnabled(value);

    private static bool SetSfxEnabled(bool value) => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SetSfxEnabled(value);

    private static bool SetSoundSettings(in SoundSettings settings) => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SetSoundSettings(in settings);
}
