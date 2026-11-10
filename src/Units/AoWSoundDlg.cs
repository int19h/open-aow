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
    public SoundDlg(global::Aow.Rtl.Component owner) : base(owner)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWSoundDlg.pas:114
    public void MusicVolumeChange()
    {
        Aow.Game.Audio.AudioPlayback.SetMusicVolume(MusicVolume.Position);
    }

    // AoWSoundDlg.pas:119
    public void SFXVolumeChange()
    {
        Aow.Game.Audio.AudioPlayback.SetSFXVolume(SFXVolume.Position);
    }

    // AoWSoundDlg.pas:124
    public void RemoveBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Delete(PlayLB.Selected);
        if (Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex == -1)
        {
            var count = Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count;
            if (PlayLB.Selected < count)
            {
                Aow.Game.Audio.AudioHost.SongPlayer.Play(PlayLB.Selected);
            }
            else
            {
                if (Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count != 0)
                {
                    var songsCount = Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count;
                    Aow.Game.Audio.AudioHost.SongPlayer.Play(songsCount - 1);
                }
            }
        }

        SoundSetup();
    }

    // AoWSoundDlg.pas:134
    public void SoundSetup()
    {
        PlayLB.Items.Clear();
        for (int I = 0, last = Aow.Game.Audio.AudioHost.SongPlayer.SongNames.Count - 1; I <= last; I++)
        {
            var songName = Aow.Game.Audio.AudioHost.SongPlayer.SongNames[I];
            PlayLB.Items.Add(songName);
        }

        PlayLB.Selected = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex;
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
            var songName = Aow.Game.Audio.AudioHost.SongPlayer.SongNames[PlayLB.Selected];
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        PlayLB.FDrawn = false;
        Aow.Game.Audio.AudioHost.SongPlayer.Play(PlayLB.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        var songName = Aow.Game.Audio.AudioHost.SongPlayer.SongNames[Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex];
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
        Aow.Game.Ui.Dialogs.SoundDlg.SongFiles.Clear();
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "\\Songs");
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
                        Aow.Game.Ui.Dialogs.SoundDlg.SongFiles.Add("Songs\\" + Search.Name);
                        var songName = Aow.Game.Audio.AudioPlayback.GetSongName("Songs\\" + Search.Name);
                        SongLB.Items.Add(songName);
                    }
                }
            }
            while (!(FindNext(ref Search) != 0));
        }

        FindClose(ref Search);
        Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(SongLB.Items, [Aow.Game.Ui.Dialogs.SoundDlg.SongFiles]);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var songFile = Aow.Game.Ui.Dialogs.SoundDlg.SongFiles[SongLB.Selected];
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDlgWindowCreate()
    {
        Aow.Game.Ui.Dialogs.SoundDlg.SongFiles = new StringList();
    }

    // AoWSoundDlg.pas:223
    public void SoundDlgWindowDestroy()
    {
        Aow.Game.Ui.Dialogs.SoundDlg.SongFiles?.Dispose();
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
            Aow.Game.Audio.AudioPlayback.ReleaseSoundSystem();
            FSoundQuality = 0;
            CurrentSongLbl.Text = "";
            PlayLB.FDrawn = false;
            MainPnl.FDrawn = false;
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:251
    public void SQCB2Change(AOWCheckBox Sender)
    {
        if (SQCB2.Checked)
        {
            FSoundQuality = 1;
            if (Aow.Game.Ui.Dialogs.SoundDlg.SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.om8bit);
                FSettings.SampleRate = 22050;
                Aow.Game.Ui.Dialogs.SoundDlg.SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Mono, SoundOutputModeElement.om8bit);
                FSettings.SampleRate = 22050;
                Aow.Game.Audio.AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:273
    public void SQCB3Change(AOWCheckBox Sender)
    {
        if (SQCB3.Checked)
        {
            FSoundQuality = 2;
            if (Aow.Game.Ui.Dialogs.SoundDlg.SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.om16bit);
                FSettings.SampleRate = 44100;
                Aow.Game.Ui.Dialogs.SoundDlg.SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.om16bit);
                FSettings.SampleRate = 44100;
                Aow.Game.Audio.AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:295
    public void SQCB4Change(AOWCheckBox Sender)
    {
        if (SQCB4.Checked)
        {
            FSoundQuality = 3;
            if (Aow.Game.Ui.Dialogs.SoundDlg.SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.om16bit, SoundOutputModeElement.HighQuality);
                FSettings.SampleRate = 44100;
                Aow.Game.Ui.Dialogs.SoundDlg.SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = Set<SoundOutputModeElement>.Of(SoundOutputModeElement.Stereo, SoundOutputModeElement.om16bit, SoundOutputModeElement.HighQuality);
                FSettings.SampleRate = 44100;
                Aow.Game.Audio.AudioPlayback.InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:317
    public void PlayLBDrawItem(in global::Aow.Rtl.Rect R, int Index, ref SurfaceDesc SurfDesc)
    {
        if (Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex == Index)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.SoundWin[2].ShowClipped(R.Left, R.Top, SurfDesc);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(AOWCheckBox Sender)
    {
        FSettings.MusicEnabled = MusicEnabledChk.Checked;
        Aow.Game.Ui.Dialogs.SoundDlg.SetMusicEnabled(FSettings.MusicEnabled);
        if (FSettings.MusicEnabled && FSoundQuality != 0)
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:331
    public void SFXEnabledChkChange(AOWCheckBox Sender)
    {
        FSettings.SFXEnabled = SFXEnabledChk.Checked;
        Aow.Game.Ui.Dialogs.SoundDlg.SetSFXEnabled(FSettings.SFXEnabled);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:339
    public void DirectSoundEnabledChkChange(AOWCheckBox Sender)
    {
        FSettings.UseDirectSound = DirectSoundEnabledChk.Checked;
        if (Aow.Game.Ui.Dialogs.SoundDlg.SoundSystemStatus().Contains(SoundSystemStatusElement.Activated))
        {
            Aow.Game.Ui.Dialogs.SoundDlg.SetSoundSettings(FSettings);
        }

        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:347
    public void SoundTabChange()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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

    private static Set<global::Aow.Game.Audio.SoundSystemStatusElement> SoundSystemStatus()
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.FStatus;
        }
        return Set<global::Aow.Game.Audio.SoundSystemStatusElement>.Of(global::System.Array.Empty<global::Aow.Game.Audio.SoundSystemStatusElement>());
    }

    private static bool SetMusicEnabled(bool Value)
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.SetMusicEnabled(Value);
        }
        return false;
    }

    private static bool SetSFXEnabled(bool Value)
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.SetSFXEnabled(Value);
        }
        return false;
    }

    private static bool SetSoundSettings(in global::Aow.Game.Audio.SoundSettings Settings)
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.SetSoundSettings(in Settings);
        }
        return false;
    }
}
