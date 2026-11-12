// Translated from AoWSoundDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Host;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// sound quality, sound settings (rate, music, DirectSound, window, Sfx, voices)
public partial class SoundDialog : UIModule
{
    public SoundDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public ListBox SongList;
    public Panel PlayListPanel;
    public ListBox PlayList;
    public Button AddButton;
    public Button RemoveButton;
    public Button PlayButton;
    public Label CurrentSongLabel;
    public Panel MainPanel;
    public CheckBox SoundQualityNoneCheckBox;
    public CheckBox SoundQualityLowCheckBox;
    public CheckBox SoundQualityMediumCheckBox;
    public CheckBox SoundQualityHighCheckBox;
    public CheckBox MusicEnabledCheckBox;
    public CheckBox EffectsEnabledCheckBox;
    public HorizontalScrollBar MusicVolumeSlider;
    public HorizontalScrollBar EffectsVolumeSlider;
    public VerticalScrollBar SongScrollBar;
    public CheckBox DirectSoundEnabledCheckBox;

    public int SoundQuality { get; set; }

    public SoundSettings Settings;

    // AoWSoundDlg.pas:108
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWSoundDlg.pas:114
    public void MusicVolumeChange()
    {
        AudioPlayback.SetMusicVolume(MusicVolumeSlider.Position);
    }

    // AoWSoundDlg.pas:119
    public void SFXVolumeChange()
    {
        AudioPlayback.SetSfxVolume(EffectsVolumeSlider.Position);
    }

    // AoWSoundDlg.pas:124
    public void RemoveBtnClick()
    {
        UI.General.PlayButtonSound(0);
        AudioHost.SongPlayer.Songs.Delete(PlayList.Selected);
        if (AudioHost.SongPlayer.CurrentIndex == -1)
        {
            var count = AudioHost.SongPlayer.Songs.Count;
            if (PlayList.Selected < count)
            {
                AudioHost.SongPlayer.Play(PlayList.Selected);
            }
            else
            {
                if (AudioHost.SongPlayer.Songs.Count != 0)
                {
                    var songsCount = AudioHost.SongPlayer.Songs.Count;
                    AudioHost.SongPlayer.Play(songsCount - 1);
                }
            }
        }

        SoundSetup();
    }

    // AoWSoundDlg.pas:134
    public void SoundSetup()
    {
        PlayList.Items.Clear();
        for (int I = 0, last = AudioHost.SongPlayer.SongNames.Count - 1; I <= last; I++)
        {
            var songName = AudioHost.SongPlayer.SongNames[I];
            PlayList.Items.Add(songName);
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
            var songName = AudioHost.SongPlayer.SongNames[PlayList.Selected];
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
        AudioHost.SongPlayer.Play(PlayList.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        var songName = AudioHost.SongPlayer.SongNames[AudioHost.SongPlayer.CurrentIndex];
        CurrentSongLabel.Text = songName;
        MainPanel.Drawn = false;
        PlayList.Drawn = false;
    }

    // AoWSoundDlg.pas:179
    public void FindFiles()
    {
        string directory;
        SongList.Items.Clear();
        s_songFiles.Clear();
        SetCurrentDir(UI.Engine.StartupDirectory + "\\Songs");
        directory = GetCurrentDir();
        if (Copy(directory, directory.Length, 1) == "\\")
        {
            directory = Copy(directory, 1, directory.Length - 1);
        }

        foreach (var entry in Files.FindEntries(directory + "\\*.*", attributes: 16))
        {
            if (entry.Name != "." && entry.Name != "..")
            {
                if (UpperCase(Copy(entry.Name, entry.Name.Length - 3, 4)) == ".S3M" || UpperCase(Copy(entry.Name, entry.Name.Length - 2, 3)) == ".IT")
                {
                    s_songFiles.Add("Songs\\" + entry.Name);
                    var songName = AudioPlayback.GetSongName("Songs\\" + entry.Name);
                    SongList.Items.Add(songName);
                }
            }
        }

        StringSorting.SortStringsAscending(SongList.Items, [s_songFiles]);
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
        var songFile = s_songFiles[SongList.Selected];
        AudioHost.SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDialogCreate()
    {
        s_songFiles = new StringList();
    }

    // AoWSoundDlg.pas:223
    public void SoundDialogDestroy()
    {
        s_songFiles?.Dispose();
    }

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
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                Settings.SampleRate = 22050;
                SetSoundSettings(Settings);
            }
            else
            {
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                Settings.SampleRate = 22050;
                AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
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
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                Settings.SampleRate = 44100;
                SetSoundSettings(Settings);
            }
            else
            {
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                Settings.SampleRate = 44100;
                AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
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
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                Settings.SampleRate = 44100;
                SetSoundSettings(Settings);
            }
            else
            {
                Settings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                Settings.SampleRate = 44100;
                AudioPlayback.InitializeSoundSystem("Galaxy", Settings);
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
    public void PlayLBDrawItem(in Rect R, int index, ref LockedSurface lockedSurface)
    {
        if (AudioHost.SongPlayer.CurrentIndex == index)
        {
            UI.Graphics.SoundWindow[2].ShowClipped(R.Left, R.Top, lockedSurface);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(CheckBox sender)
    {
        Settings.MusicEnabled = MusicEnabledCheckBox.Checked;
        SetMusicEnabled(Settings.MusicEnabled);
        if (Settings.MusicEnabled && SoundQuality != 0)
        {
            AudioHost.SongPlayer.Play(0);
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
        Settings.SfxEnabled = EffectsEnabledCheckBox.Checked;
        SetSfxEnabled(Settings.SfxEnabled);
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
        Settings.UseDirectSound = DirectSoundEnabledCheckBox.Checked;
        if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
        {
            SetSoundSettings(Settings);
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
    public void SoundTabChange()
    {
        UI.General.PlayButtonSound(0);
    }

    // AoWSoundDlg.pas:352
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static StringList s_songFiles;

    private static Set<SoundSystemStatusElement> SoundSystemStatus()
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.Status;
        }
        return Set<SoundSystemStatusElement>.Of(Array.Empty<SoundSystemStatusElement>());
    }

    private static bool SetMusicEnabled(bool value)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetMusicEnabled(value);
        }
        return false;
    }

    private static bool SetSfxEnabled(bool value)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetSfxEnabled(value);
        }
        return false;
    }

    private static bool SetSoundSettings(in SoundSettings settings)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetSoundSettings(in settings);
        }
        return false;
    }
}
