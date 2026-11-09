// Translated from AoWSoundDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using Aow.Units.Sound;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.aowIntGfxMod.aowIntGfxModUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.Engine.EngineUnit;
using static Aow.Units.Sound.SoundUnit;

namespace Aow.Units.AoWSoundDlg;

// sound quality, sound settings (rate, music, DirectSound, window, SFX, voices)
public partial class TSoundDlg : TDataModule
{
    public TSoundDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWButton CloseBut;
    public TAOWImage TCeltLL;
    public TAoWFrame BCeltL;
    public TAOWLabel Title;
    public TAOWListBox SongLB;
    public TAOWLabel SongLbl;
    public TAOWButton Close;
    public TAOWPanel PlayListPnl;
    public TAOWTabPanel SoundTab;
    public TAOWListBox PlayLB;
    public TAOWButton AddBtn;
    public TAOWButton RemoveBtn;
    public TAOWButton PlayBtn;
    public TAOWLabel PlayLbl;
    public TAOWLabel CurrentSongLbl;
    public TAOWImage TCeltLR;
    public TAOWLabel MusicVolLbl;
    public TAOWLabel SFXVolLbl;
    public TAOWPanel MainPnl;
    public TAOWLabel SoundLbl;
    public TAOWPanel SettingsPnl;
    public TAOWLabel SFXLbl;
    public TAOWPanel SQPnl;
    public TAOWCheckBox SQCB1;
    public TAOWLabel SQLBL1;
    public TAOWCheckBox SQCB2;
    public TAOWLabel SQLBL2;
    public TAOWCheckBox SQCB3;
    public TAOWLabel SQLBL3;
    public TAOWCheckBox SQCB4;
    public TAOWLabel SQLBL4;
    public TAOWPanel SFXPnl;
    public TAOWCheckBox MusicEnabledChk;
    public TAOWLabel SFXLBL2;
    public TAOWCheckBox SFXEnabledChk;
    public TAOWLabel SFXLBL3;
    public TAoWFrame TopLine;
    public TAOWImage AOWImage4;
    public TAOWImage HarpL;
    public TAOWImage HarpR;
    public TAOWHScrollBar MusicVolume;
    public TAOWHScrollBar SFXVolume;
    public TAOWImage TitleIcon;
    public TAOWVScrollBar SongSB;
    public TAOWVScrollBar PlaySB;
    public TAOWCheckBox DirectSoundEnabledChk;
    public TAOWLabel UseDirectSoundLbl;
    public TAOWPanel FolderPnl;
    public TAOWPanel PlayPnl;
    public TIvTranslator Translator;
    public int FSoundQuality;
    public TSoundSettings FSettings;

    // AoWSoundDlg.pas:108
    public void CloseClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWSoundDlg.pas:114
    public void MusicVolumeChange()
    {
        SetMusicVolume(MusicVolume.Position);
    }

    // AoWSoundDlg.pas:119
    public void SFXVolumeChange()
    {
        SetSFXVolume(SFXVolume.Position);
    }

    // AoWSoundDlg.pas:124
    public void RemoveBtnClick()
    {
        General.PlayButtonSound(0);
        SongPlayer.Songs.Delete(PlayLB.Selected);
        if (SongPlayer.CurrentIndex == -1)
        {
            var count = SongPlayer.Songs.Count;
            if (PlayLB.Selected < count)
            {
                SongPlayer.Play(PlayLB.Selected);
            }
            else
            {
                if (SongPlayer.Songs.Count != 0)
                {
                    var songsCount = SongPlayer.Songs.Count;
                    SongPlayer.Play(songsCount - 1);
                }
            }
        }

        SoundSetup();
    }

    // AoWSoundDlg.pas:134
    public void SoundSetup()
    {
        PlayLB.Items.Clear();
        for (int I = 0, last = SongPlayer.SongNames.Count - 1; I <= last; I++)
        {
            var songName = SongPlayer.SongNames[I];
            PlayLB.Items.Add(songName);
        }

        PlayLB.Selected = SongPlayer.CurrentIndex;
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
            var songName = SongPlayer.SongNames[PlayLB.Selected];
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
        General.PlayButtonSound(0);
        PlayLB.FDrawn = false;
        SongPlayer.Play(PlayLB.Selected);
    }

    // AoWSoundDlg.pas:172
    public void SongChange()
    {
        var songName = SongPlayer.SongNames[SongPlayer.CurrentIndex];
        CurrentSongLbl.Text = songName;
        MainPnl.FDrawn = false;
        PlayLB.FDrawn = false;
    }

    // AoWSoundDlg.pas:179
    public void FindFiles()
    {
        string Directory;
        TSearchRec Search = default;
        SongLB.Items.Clear();
        AoWSoundDlgUnit.SongFiles.Clear();
        SetCurrentDir(AoWEngine.StartupDirectory + "\\Songs");
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
                        AoWSoundDlgUnit.SongFiles.Add("Songs\\" + Search.Name);
                        var songName = GetSongName("Songs\\" + Search.Name);
                        SongLB.Items.Add(songName);
                    }
                }
            }
            while (!(FindNext(ref Search) != 0));
        }

        FindClose(ref Search);
        SortStringsAscending(SongLB.Items, [AoWSoundDlgUnit.SongFiles]);
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
        General.PlayButtonSound(0);
        var songFile = AoWSoundDlgUnit.SongFiles[SongLB.Selected];
        SongPlayer.Songs.Add(songFile);
        SoundSetup();
    }

    // AoWSoundDlg.pas:218
    public void SoundDlgWindowCreate()
    {
        AoWSoundDlgUnit.SongFiles = new TStringList();
    }

    // AoWSoundDlg.pas:223
    public void SoundDlgWindowDestroy()
    {
        AoWSoundDlgUnit.SongFiles?.Dispose();
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
    public void SQCB1Change(TAOWCheckBox Sender)
    {
        if (SQCB1.Checked)
        {
            ReleaseSoundSystem();
            FSoundQuality = 0;
            CurrentSongLbl.Text = "";
            PlayLB.FDrawn = false;
            MainPnl.FDrawn = false;
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:251
    public void SQCB2Change(TAOWCheckBox Sender)
    {
        if (SQCB2.Checked)
        {
            FSoundQuality = 1;
            if (SoundSystemStatus().Contains(TSoundSystemStatusElement.ssActivated))
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omMono, TSoundOutputModeElement.om8bit);
                FSettings.SampleRate = 22050;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omMono, TSoundOutputModeElement.om8bit);
                FSettings.SampleRate = 22050;
                InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:273
    public void SQCB3Change(TAOWCheckBox Sender)
    {
        if (SQCB3.Checked)
        {
            FSoundQuality = 2;
            if (SoundSystemStatus().Contains(TSoundSystemStatusElement.ssActivated))
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omStereo, TSoundOutputModeElement.om16bit);
                FSettings.SampleRate = 44100;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omStereo, TSoundOutputModeElement.om16bit);
                FSettings.SampleRate = 44100;
                InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:295
    public void SQCB4Change(TAOWCheckBox Sender)
    {
        if (SQCB4.Checked)
        {
            FSoundQuality = 3;
            if (SoundSystemStatus().Contains(TSoundSystemStatusElement.ssActivated))
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omStereo, TSoundOutputModeElement.om16bit, TSoundOutputModeElement.omHighQuality);
                FSettings.SampleRate = 44100;
                SetSoundSettings(FSettings);
            }
            else
            {
                FSettings.OutputMode = TSet<TSoundOutputModeElement>.Of(TSoundOutputModeElement.omStereo, TSoundOutputModeElement.om16bit, TSoundOutputModeElement.omHighQuality);
                FSettings.SampleRate = 44100;
                InitializeSoundSystem("Galaxy", FSettings);
            }
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:317
    public void PlayLBDrawItem(in TRect R, int Index, ref TSurfaceDesc SurfDesc)
    {
        if (SongPlayer.CurrentIndex == Index)
        {
            IntGfxMod.SoundWin[2].ShowClipped(R.Left, R.Top, SurfDesc);
        }
    }

    // AoWSoundDlg.pas:322
    public void MusicEnabledChkChange(TAOWCheckBox Sender)
    {
        FSettings.MusicEnabled = MusicEnabledChk.Checked;
        SetMusicEnabled(FSettings.MusicEnabled);
        if (FSettings.MusicEnabled && FSoundQuality != 0)
        {
            SongPlayer.Play(0);
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:331
    public void SFXEnabledChkChange(TAOWCheckBox Sender)
    {
        FSettings.SFXEnabled = SFXEnabledChk.Checked;
        SetSFXEnabled(FSettings.SFXEnabled);
        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:339
    public void DirectSoundEnabledChkChange(TAOWCheckBox Sender)
    {
        FSettings.UseDirectSound = DirectSoundEnabledChk.Checked;
        if (SoundSystemStatus().Contains(TSoundSystemStatusElement.ssActivated))
        {
            SetSoundSettings(FSettings);
        }

        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSoundDlg.pas:347
    public void SoundTabChange()
    {
        General.PlayButtonSound(0);
    }

    // AoWSoundDlg.pas:352
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWSoundDlgUnit
{
    public static TStringList SongFiles;
}
