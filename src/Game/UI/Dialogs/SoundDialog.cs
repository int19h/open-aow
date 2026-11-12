// Translated from AoWSoundDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// sound quality, sound settings (rate, music, DirectSound, window, Sfx, voices)
public partial class SoundDialog : DataModule
{
    public SoundDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Button CloseBut;
    public Image TCeltLL;
    public Frame BCeltL;
    public Label Title;
    public ListBox SongLB;
    public Label SongLbl;
    public Button Close;
    public Panel PlayListPnl;
    public TabPanel SoundTab;
    public ListBox PlayLB;
    public Button AddBtn;
    public Button RemoveBtn;
    public Button PlayBtn;
    public Label PlayLbl;
    public Label CurrentSongLbl;
    public Image TCeltLR;
    public Label MusicVolLbl;
    public Label SFXVolLbl;
    public Panel MainPnl;
    public Label SoundLbl;
    public Panel SettingsPnl;
    public Label SFXLbl;
    public Panel SQPnl;
    public CheckBox SQCB1;
    public Label SQLBL1;
    public CheckBox SQCB2;
    public Label SQLBL2;
    public CheckBox SQCB3;
    public Label SQLBL3;
    public CheckBox SQCB4;
    public Label SQLBL4;
    public Panel SFXPnl;
    public CheckBox MusicEnabledChk;
    public Label SFXLBL2;
    public CheckBox SFXEnabledChk;
    public Label SFXLBL3;
    public Frame TopLine;
    public Image Image4;
    public Image HarpL;
    public Image HarpR;
    public HorizontalScrollBar MusicVolume;
    public HorizontalScrollBar SFXVolume;
    public Image TitleIcon;
    public VerticalScrollBar SongSB;
    public VerticalScrollBar PlaySB;
    public CheckBox DirectSoundEnabledChk;
    public Label UseDirectSoundLbl;
    public Panel FolderPnl;
    public Panel PlayPnl;
    public IvTranslator Translator;

    public int SoundQuality { get; set; }

    public SoundSettings Settings;

    // AoWSoundDlg.pas:108
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWSoundDlg.pas:114
    public void MusicVolumeChange()
    {
        AudioPlayback.SetMusicVolume(MusicVolume.Position);
    }

    // AoWSoundDlg.pas:119
    public void SFXVolumeChange()
    {
        AudioPlayback.SetSfxVolume(SFXVolume.Position);
    }

    // AoWSoundDlg.pas:124
    public void RemoveBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        AudioHost.SongPlayer.Songs.Delete(PlayLB.Selected);
        if (AudioHost.SongPlayer.CurrentIndex == -1)
        {
            var count = AudioHost.SongPlayer.Songs.Count;
            if (PlayLB.Selected < count)
            {
                AudioHost.SongPlayer.Play(PlayLB.Selected);
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
        PlayLB.Items.Clear();
        for (int I = 0, last = AudioHost.SongPlayer.SongNames.Count - 1; I <= last; I++)
        {
            var songName = AudioHost.SongPlayer.SongNames[I];
            PlayLB.Items.Add(songName);
        }

        PlayLB.Selected = AudioHost.SongPlayer.CurrentIndex;
        PlayLB.Sized = false;
        if (PlayLB.Selected == -1)
        {
            RemoveBtn.Visible = false;
            PlayBtn.Visible = false;
            CurrentSongLbl.Text = "";
        }
        else
        {
            RemoveBtn.Visible = true;
            PlayBtn.Visible = true;
            var songName = AudioHost.SongPlayer.SongNames[PlayLB.Selected];
            CurrentSongLbl.Text = songName;
        }

        MainPnl.Drawn = false;
        PlayListPnl.Drawn = false;
        MusicEnabledChk.Checked = Settings.MusicEnabled;
        SFXEnabledChk.Checked = Settings.SfxEnabled;
        DirectSoundEnabledChk.Checked = Settings.UseDirectSound;
        SQCB1.Checked = SoundQuality == 0;
        SQCB2.Checked = SoundQuality == 1;
        SQCB3.Checked = SoundQuality == 2;
        SQCB4.Checked = SoundQuality == 3;
    }

    // AoWSoundDlg.pas:165
    public void PlayBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        PlayLB.Drawn = false;
        AudioHost.SongPlayer.Play(PlayLB.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        var songName = AudioHost.SongPlayer.SongNames[AudioHost.SongPlayer.CurrentIndex];
        CurrentSongLbl.Text = songName;
        MainPnl.Drawn = false;
        PlayLB.Drawn = false;
    }

    // AoWSoundDlg.pas:179
    public void FindFiles()
    {
        string directory;
        SearchRec search = default;
        SongLB.Items.Clear();
        s_songFiles.Clear();
        SetCurrentDir(GameTables.GameEngine.StartupDirectory + "\\Songs");
        directory = GetCurrentDir();
        if (Copy(directory, directory.Length, 1) == "\\")
        {
            directory = Copy(directory, 1, directory.Length - 1);
        }

        if (FindFirst(directory + "\\*.*", 16, ref search) == 0)
        {
            do
            {
                if (search.Name != "." && search.Name != "..")
                {
                    if (UpperCase(Copy(search.Name, search.Name.Length - 3, 4)) == ".S3M" || UpperCase(Copy(search.Name, search.Name.Length - 2, 3)) == ".IT")
                    {
                        s_songFiles.Add("Songs\\" + search.Name);
                        var songName = AudioPlayback.GetSongName("Songs\\" + search.Name);
                        SongLB.Items.Add(songName);
                    }
                }
            }
            while (!(FindNext(ref search) != 0));
        }

        FindClose(ref search);
        StringSorting.SortStringsAscending(SongLB.Items, [s_songFiles]);
        SongLB.Selected = -1;
        SongLB.ListOffset = 0;
        SongSB.Position = 0;
    }

    // AoWSoundDlg.pas:206
    public void SongLBChange()
    {
        if (SongLB.Selected > -1)
        {
            AddBtn.Visible = true;
        }
    }

    // AoWSoundDlg.pas:211
    public void AddBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var songFile = s_songFiles[SongLB.Selected];
        AudioHost.SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDlgWindowCreate()
    {
        s_songFiles = new StringList();
    }

    // AoWSoundDlg.pas:223
    public void SoundDlgWindowDestroy()
    {
        s_songFiles?.Dispose();
    }

    // AoWSoundDlg.pas:228
    public void PlayLBChange()
    {
        if (PlayLB.Selected > -1)
        {
            RemoveBtn.Visible = true;
            PlayBtn.Visible = true;
        }
    }

    // AoWSoundDlg.pas:237
    public void SQCB1Change(CheckBox sender)
    {
        if (SQCB1.Checked)
        {
            AudioPlayback.ReleaseSoundSystem();
            SoundQuality = 0;
            CurrentSongLbl.Text = "";
            PlayLB.Drawn = false;
            MainPnl.Drawn = false;
        }

        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:251
    public void SQCB2Change(CheckBox sender)
    {
        if (SQCB2.Checked)
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:273
    public void SQCB3Change(CheckBox sender)
    {
        if (SQCB3.Checked)
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:295
    public void SQCB4Change(CheckBox sender)
    {
        if (SQCB4.Checked)
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:317
    public void PlayLBDrawItem(in Rect R, int index, ref LockedSurface lockedSurface)
    {
        if (AudioHost.SongPlayer.CurrentIndex == index)
        {
            DialogHosts.UIGraphics.SoundWin[2].ShowClipped(R.Left, R.Top, lockedSurface);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(CheckBox sender)
    {
        Settings.MusicEnabled = MusicEnabledChk.Checked;
        SetMusicEnabled(Settings.MusicEnabled);
        if (Settings.MusicEnabled && SoundQuality != 0)
        {
            AudioHost.SongPlayer.Play(0);
        }

        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:331
    public void SFXEnabledChkChange(CheckBox sender)
    {
        Settings.SfxEnabled = SFXEnabledChk.Checked;
        SetSfxEnabled(Settings.SfxEnabled);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:339
    public void DirectSoundEnabledChkChange(CheckBox sender)
    {
        Settings.UseDirectSound = DirectSoundEnabledChk.Checked;
        if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
        {
            SetSoundSettings(Settings);
        }

        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:347
    public void SoundTabChange()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWSoundDlg.pas:352
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
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
