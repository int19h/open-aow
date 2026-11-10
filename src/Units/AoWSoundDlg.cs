// Translated from AoWSoundDlg.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSoundDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// sound quality, sound settings (rate, music, DirectSound, window, SFX, voices)
public partial class SoundDlg : DataModule
{
    public SoundDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWButton CloseBut;
    public AOWImage TCeltLL;
    public AoWFrame BCeltL;
    public AOWLabel Title;
    public AOWListBox SongLB;
    public AOWLabel SongLbl;
    public AOWButton Close;
    public AOWPanel PlayListPnl;
    public AOWTabPanel SoundTab;
    public AOWListBox PlayLB;
    public AOWButton AddBtn;
    public AOWButton RemoveBtn;
    public AOWButton PlayBtn;
    public AOWLabel PlayLbl;
    public AOWLabel CurrentSongLbl;
    public AOWImage TCeltLR;
    public AOWLabel MusicVolLbl;
    public AOWLabel SFXVolLbl;
    public AOWPanel MainPnl;
    public AOWLabel SoundLbl;
    public AOWPanel SettingsPnl;
    public AOWLabel SFXLbl;
    public AOWPanel SQPnl;
    public AOWCheckBox SQCB1;
    public AOWLabel SQLBL1;
    public AOWCheckBox SQCB2;
    public AOWLabel SQLBL2;
    public AOWCheckBox SQCB3;
    public AOWLabel SQLBL3;
    public AOWCheckBox SQCB4;
    public AOWLabel SQLBL4;
    public AOWPanel SFXPnl;
    public AOWCheckBox MusicEnabledChk;
    public AOWLabel SFXLBL2;
    public AOWCheckBox SFXEnabledChk;
    public AOWLabel SFXLBL3;
    public AoWFrame TopLine;
    public AOWImage AOWImage4;
    public AOWImage HarpL;
    public AOWImage HarpR;
    public AOWHScrollBar MusicVolume;
    public AOWHScrollBar SFXVolume;
    public AOWImage TitleIcon;
    public AOWVScrollBar SongSB;
    public AOWVScrollBar PlaySB;
    public AOWCheckBox DirectSoundEnabledChk;
    public AOWLabel UseDirectSoundLbl;
    public AOWPanel FolderPnl;
    public AOWPanel PlayPnl;
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
        SetCurrentDir(Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "\\Songs");
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
        Engine.Nodes.StringSorting.SortStringsAscending(SongLB.Items, [SongFiles]);
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
    public void SQCB1Change(AOWCheckBox Sender)
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
    public void SQCB2Change(AOWCheckBox Sender)
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
    public void SQCB3Change(AOWCheckBox Sender)
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
    public void SQCB4Change(AOWCheckBox Sender)
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
    public void PlayLBDrawItem(in Rect R, int Index, ref SurfaceDesc SurfDesc)
    {
        if (AudioHost.SongPlayer.CurrentIndex == Index)
        {
            DialogHosts.IntGfxMod.SoundWin[2].ShowClipped(R.Left, R.Top, SurfDesc);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(AOWCheckBox Sender)
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
    public void SFXEnabledChkChange(AOWCheckBox Sender)
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
    public void DirectSoundEnabledChkChange(AOWCheckBox Sender)
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
