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
    public Image AOWImage4;
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
    public int FSoundQuality;
    public SoundSettings FSettings;

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
        AudioPlayback.SetSFXVolume(SFXVolume.Position);
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
        PlayLB.FSized = false;
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

        MainPnl.FDrawn = false;
        PlayListPnl.FDrawn = false;
        MusicEnabledChk.Checked = FSettings.MusicEnabled;
        SFXEnabledChk.Checked = FSettings.SFXEnabled;
        DirectSoundEnabledChk.Checked = FSettings.UseDirectSound;
        SQCB1.Checked = FSoundQuality == 0;
        SQCB2.Checked = FSoundQuality == 1;
        SQCB3.Checked = FSoundQuality == 2;
        SQCB4.Checked = FSoundQuality == 3;
    }

    // AoWSoundDlg.pas:165
    public void PlayBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        PlayLB.FDrawn = false;
        AudioHost.SongPlayer.Play(PlayLB.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        var songName = AudioHost.SongPlayer.SongNames[AudioHost.SongPlayer.CurrentIndex];
        CurrentSongLbl.Text = songName;
        MainPnl.FDrawn = false;
        PlayLB.FDrawn = false;
    }

    // AoWSoundDlg.pas:179
    public void FindFiles()
    {
        string Directory;
        SearchRec Search = default;
        SongLB.Items.Clear();
        SongFiles.Clear();
        SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "\\Songs");
        Directory = GetCurrentDir();
        if (Copy(Directory, Directory.Length, 1) == "\\")
        {
            Directory = Copy(Directory, 1, Directory.Length - 1);
        }

        if (FindFirst(Directory + "\\*.*", 16, ref Search) == 0)
        {
            do
            {
                if (Search.Name != "." && Search.Name != "..")
                {
                    if (UpperCase(Copy(Search.Name, Search.Name.Length - 3, 4)) == ".S3M" || UpperCase(Copy(Search.Name, Search.Name.Length - 2, 3)) == ".IT")
                    {
                        SongFiles.Add("Songs\\" + Search.Name);
                        var songName = AudioPlayback.GetSongName("Songs\\" + Search.Name);
                        SongLB.Items.Add(songName);
                    }
                }
            }
            while (!(FindNext(ref Search) != 0));
        }

        FindClose(ref Search);
        StringSorting.SortStringsAscending(SongLB.Items, [SongFiles]);
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
        var songFile = SongFiles[SongLB.Selected];
        AudioHost.SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDlgWindowCreate()
    {
        SongFiles = new StringList();
    }

    // AoWSoundDlg.pas:223
    public void SoundDlgWindowDestroy()
    {
        SongFiles?.Dispose();
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
    public void SQCB1Change(CheckBox Sender)
    {
        if (SQCB1.Checked)
        {
            AudioPlayback.ReleaseSoundSystem();
            FSoundQuality = 0;
            CurrentSongLbl.Text = "";
            PlayLB.FDrawn = false;
            MainPnl.FDrawn = false;
        }

        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:251
    public void SQCB2Change(CheckBox Sender)
    {
        if (SQCB2.Checked)
        {
            FSoundQuality = 1;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                FSettings.SampleRate = 22050;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.Bits8);
                FSettings.SampleRate = 22050;
                AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:273
    public void SQCB3Change(CheckBox Sender)
    {
        if (SQCB3.Checked)
        {
            FSoundQuality = 2;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                FSettings.SampleRate = 44100;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16);
                FSettings.SampleRate = 44100;
                AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:295
    public void SQCB4Change(CheckBox Sender)
    {
        if (SQCB4.Checked)
        {
            FSoundQuality = 3;
            if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                FSettings.SampleRate = 44100;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.Bits16, SoundOutputModeElement.HighQuality);
                FSettings.SampleRate = 44100;
                AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:317
    public void PlayLBDrawItem(in Rect R, int Index, ref LockedSurface SurfDesc)
    {
        if (AudioHost.SongPlayer.CurrentIndex == Index)
        {
            DialogHosts.IntGfxMod.SoundWin[2].ShowClipped(R.Left, R.Top, SurfDesc);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(CheckBox Sender)
    {
        FSettings.MusicEnabled = MusicEnabledChk.Checked;
        SetMusicEnabled(FSettings.MusicEnabled);
        if (FSettings.MusicEnabled && FSoundQuality != 0)
        {
            AudioHost.SongPlayer.Play(0);
        }

        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:331
    public void SFXEnabledChkChange(CheckBox Sender)
    {
        FSettings.SFXEnabled = SFXEnabledChk.Checked;
        SetSFXEnabled(FSettings.SFXEnabled);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:339
    public void DirectSoundEnabledChkChange(CheckBox Sender)
    {
        FSettings.UseDirectSound = DirectSoundEnabledChk.Checked;
        if (SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
        {
            SetSoundSettings(FSettings);
        }

        if (Sender.Checked)
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
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static StringList SongFiles;

    private static Set<SoundSystemStatusElement> SoundSystemStatus()
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.FStatus;
        }
        return Set<SoundSystemStatusElement>.Of(Array.Empty<SoundSystemStatusElement>());
    }

    private static bool SetMusicEnabled(bool Value)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetMusicEnabled(Value);
        }
        return false;
    }

    private static bool SetSFXEnabled(bool Value)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetSFXEnabled(Value);
        }
        return false;
    }

    private static bool SetSoundSettings(in SoundSettings Settings)
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.SetSoundSettings(in Settings);
        }
        return false;
    }
}
