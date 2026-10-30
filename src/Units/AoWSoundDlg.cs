// Translated from AoWSoundDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;

namespace Aow.Units.AoWSoundDlg
{
    // { TSoundDlg (size $12C, VMT 00420E44): $110 sound quality, $114 sound settings ($118 rate, $11C music, $11D DirectSound, $120 window, $124 SFX, $128 voices). }
    public partial class TSoundDlg : Aow.Rtl.TDataModule
    {
        public TSoundDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton CloseBut;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage TCeltLL;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWListBox.TAOWListBox SongLB;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel SongLbl;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel PlayListPnl;
        // { +0064 }
        public Aow.Units.AoWTabPanel.TAOWTabPanel SoundTab;
        // { +0068 }
        public Aow.Units.AoWListBox.TAOWListBox PlayLB;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton AddBtn;
        // { +0070 }
        public Aow.Units.AoWButton.TAOWButton RemoveBtn;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton PlayBtn;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel PlayLbl;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel CurrentSongLbl;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage TCeltLR;
        // { +0084 }
        public Aow.Units.AoWLabel.TAOWLabel MusicVolLbl;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel SFXVolLbl;
        // { +008C }
        public Aow.Units.AoWPanel.TAOWPanel MainPnl;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel SoundLbl;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel SettingsPnl;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel SFXLbl;
        // { +009C }
        public Aow.Units.AoWPanel.TAOWPanel SQPnl;
        // { +00A0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SQCB1;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel SQLBL1;
        // { +00A8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SQCB2;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel SQLBL2;
        // { +00B0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SQCB3;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel SQLBL3;
        // { +00B8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SQCB4;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel SQLBL4;
        // { +00C0 }
        public Aow.Units.AoWPanel.TAOWPanel SFXPnl;
        // { +00C4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox MusicEnabledChk;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel SFXLBL2;
        // { +00CC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SFXEnabledChk;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel SFXLBL3;
        // { +00D4 }
        public Aow.Units.AoWFrame.TAoWFrame TopLine;
        // { +00D8 }
        public Aow.Units.AoWImage.TAOWImage AOWImage4;
        // { +00DC }
        public Aow.Units.AoWImage.TAOWImage HarpL;
        // { +00E0 }
        public Aow.Units.AoWImage.TAOWImage HarpR;
        // { +00E4 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar MusicVolume;
        // { +00E8 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar SFXVolume;
        // { +00EC }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +00F0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SongSB;
        // { +00F4 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlaySB;
        // { +00F8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox DirectSoundEnabledChk;
        // { +00FC }
        public Aow.Units.AoWLabel.TAOWLabel UseDirectSoundLbl;
        // { +0100 }
        public Aow.Units.AoWPanel.TAOWPanel FolderPnl;
        // { +0104 }
        public Aow.Units.AoWPanel.TAOWPanel PlayPnl;
        // { +0108 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +010C }
        public int FSoundQuality;
        // { $110 }
        public Aow.Units.Sound.TSoundSettings FSettings;
        // AoWSoundDlg.pas:108
        // { $114..$12B }
        // { conflict: FSoundSettings: TSoundSettings; }
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWSoundDlg.pas:114
        public void MusicVolumeChange()
        {
            Aow.Units.Sound.Sound.SetMusicVolume(MusicVolume.Position);
        }

        // AoWSoundDlg.pas:119
        public void SFXVolumeChange()
        {
            Aow.Units.Sound.Sound.SetSFXVolume(SFXVolume.Position);
        }

        // AoWSoundDlg.pas:124
        public void RemoveBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.Sound.Sound.SongPlayer.Songs.Delete(PlayLB.Selected);
            if (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex == -1)
            {
                var call__1 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                if (PlayLB.Selected < call__1)
                {
                    Aow.Units.Sound.Sound.SongPlayer.Play(PlayLB.Selected);
                }
                else
                {
                    var call__2 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                    if (call__2 != 0)
                    {
                        var call__3 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                        Aow.Units.Sound.Sound.SongPlayer.Play(call__3 - 1);
                    }
                }
            }

            SoundSetup();
        }

        // AoWSoundDlg.pas:134
        public void SoundSetup()
        {
            int I = default;
            PlayLB.Items.Clear();
            {
                I = 0;
                int end__4 = Aow.Units.Sound.Sound.SongPlayer.SongNames.Count - 1;
                for (; I <= end__4; I += 1)
                {
                    var call__5 = Aow.Units.Sound.Sound.SongPlayer.SongNames[I];
                    PlayLB.Items.Add(call__5);
                }
            }

            PlayLB.Selected = Aow.Units.Sound.Sound.SongPlayer.CurrentIndex;
            PlayLB.FSized = false;
            if (PlayLB.Selected == -1)
            {
                RemoveBtn.Visible = false;
                PlayBtn.Visible = false;
                CurrentSongLbl.Text = (AnsiString)"";
            }
            else
            {
                RemoveBtn.Visible = true;
                PlayBtn.Visible = true;
                var call__6 = Aow.Units.Sound.Sound.SongPlayer.SongNames[PlayLB.Selected];
                CurrentSongLbl.Text = call__6;
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            PlayLB.FDrawn = false;
            Aow.Units.Sound.Sound.SongPlayer.Play(PlayLB.Selected);
        }

        // AoWSoundDlg.pas:172
        public void SongChange(Aow.Rtl.TObject Sender)
        {
            var call__7 = Aow.Units.Sound.Sound.SongPlayer.SongNames[Aow.Units.Sound.Sound.SongPlayer.CurrentIndex];
            CurrentSongLbl.Text = call__7;
            MainPnl.FDrawn = false;
            PlayLB.FDrawn = false;
        }

        // AoWSoundDlg.pas:179
        public void FindFiles()
        {
            Aow.Rtl.AnsiString Directory = default;
            Aow.Rtl.TSearchRec Search = default;
            SongLB.Items.Clear();
            Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles.Clear();
            Aow.Rtl.Rtl.SetCurrentDir(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"\\Songs"));
            Directory = Aow.Rtl.Rtl.GetCurrentDir();
            if (Aow.Rtl.Rtl.Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
            {
                Directory = Aow.Rtl.Rtl.Copy(Directory, 1, Directory.Length - 1);
            }

            if (Aow.Rtl.Rtl.FindFirst(Directory + ((AnsiString)"\\*.*"), 16, ref Search) == 0)
            {
                do
                {
                    if ((Search.Name != AnsiString.FromBytes(46)) && (Search.Name != ((AnsiString)"..")))
                    {
                        if ((Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.Copy(Search.Name, Search.Name.Length - 3, 4)) == ((AnsiString)".S3M")) || (Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.Copy(Search.Name, Search.Name.Length - 2, 3)) == ((AnsiString)".IT")))
                        {
                            Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles.Add(((AnsiString)"Songs\\") + Search.Name);
                            var call__8 = Aow.Units.Sound.Sound.GetSongName(((AnsiString)"Songs\\") + Search.Name);
                            SongLB.Items.Add(call__8);
                        }
                    }
                }
                while (!(Aow.Rtl.Rtl.FindNext(ref Search) != 0));
            }

            Aow.Rtl.Rtl.FindClose(ref Search);
            Aow.Units.Engine.Engine.SortStringsAscending(SongLB.Items, [Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles]);
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
        public void AddBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__9 = Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles[SongLB.Selected];
            Aow.Units.Sound.Sound.SongPlayer.Songs.Add(call__9);
            SoundSetup();
        }

        // AoWSoundDlg.pas:218
        public void SoundDlgWindowCreate()
        {
            var call__10 = new Aow.Rtl.TStringList();
            Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles = call__10;
        }

        // AoWSoundDlg.pas:223
        public void SoundDlgWindowDestroy()
        {
            Aow.Units.AoWSoundDlg.AoWSoundDlg.SongFiles.Free();
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
        public void SQCB1Change(Aow.Rtl.TObject Sender)
        {
            if (SQCB1.Checked)
            {
                Aow.Units.Sound.Sound.ReleaseSoundSystem();
                FSoundQuality = 0;
                CurrentSongLbl.Text = (AnsiString)"";
                PlayLB.FDrawn = false;
                MainPnl.FDrawn = false;
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:251
        public void SQCB2Change(Aow.Rtl.TObject Sender)
        {
            if (SQCB2.Checked)
            {
                FSoundQuality = 1;
                if (Aow.Units.Sound.Sound.SoundSystemStatus().Contains(Aow.Units.Sound.TSoundSystemStatusElement.ssActivated))
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omMono, Aow.Units.Sound.TSoundOutputModeElement.om8bit);
                    FSettings.SampleRate = 22050;
                    Aow.Units.Sound.Sound.SetSoundSettings(FSettings);
                }
                else
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omMono, Aow.Units.Sound.TSoundOutputModeElement.om8bit);
                    FSettings.SampleRate = 22050;
                    Aow.Units.Sound.Sound.InitializeSoundSystem((AnsiString)"Galaxy", FSettings);
                }
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:273
        public void SQCB3Change(Aow.Rtl.TObject Sender)
        {
            if (SQCB3.Checked)
            {
                FSoundQuality = 2;
                if (Aow.Units.Sound.Sound.SoundSystemStatus().Contains(Aow.Units.Sound.TSoundSystemStatusElement.ssActivated))
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit);
                    FSettings.SampleRate = 44100;
                    Aow.Units.Sound.Sound.SetSoundSettings(FSettings);
                }
                else
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit);
                    FSettings.SampleRate = 44100;
                    Aow.Units.Sound.Sound.InitializeSoundSystem((AnsiString)"Galaxy", FSettings);
                }
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:295
        public void SQCB4Change(Aow.Rtl.TObject Sender)
        {
            if (SQCB4.Checked)
            {
                FSoundQuality = 3;
                if (Aow.Units.Sound.Sound.SoundSystemStatus().Contains(Aow.Units.Sound.TSoundSystemStatusElement.ssActivated))
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit, Aow.Units.Sound.TSoundOutputModeElement.omHighQuality);
                    FSettings.SampleRate = 44100;
                    Aow.Units.Sound.Sound.SetSoundSettings(FSettings);
                }
                else
                {
                    FSettings.OutputMode = TSet<Aow.Units.Sound.TSoundOutputModeElement>.Of(Aow.Units.Sound.TSoundOutputModeElement.omStereo, Aow.Units.Sound.TSoundOutputModeElement.om16bit, Aow.Units.Sound.TSoundOutputModeElement.omHighQuality);
                    FSettings.SampleRate = 44100;
                    Aow.Units.Sound.Sound.InitializeSoundSystem((AnsiString)"Galaxy", FSettings);
                }
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:317
        public void PlayLBDrawItem(Aow.Rtl.TObject Sender, in Aow.Rtl.TRect R, int Index, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            if (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex == Index)
            {
                var call__11 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.SoundWin[2];
                call__11.ShowClipped(R.Left, R.Top, SurfDesc);
            }
        }

        // AoWSoundDlg.pas:322
        public void MusicEnabledChkChange(Aow.Rtl.TObject Sender)
        {
            FSettings.MusicEnabled = MusicEnabledChk.Checked;
            Aow.Units.Sound.Sound.SetMusicEnabled(FSettings.MusicEnabled);
            if (FSettings.MusicEnabled && (FSoundQuality != 0))
            {
                Aow.Units.Sound.Sound.SongPlayer.Play(0);
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:331
        public void SFXEnabledChkChange(Aow.Rtl.TObject Sender)
        {
            FSettings.SFXEnabled = SFXEnabledChk.Checked;
            Aow.Units.Sound.Sound.SetSFXEnabled(FSettings.SFXEnabled);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:339
        public void DirectSoundEnabledChkChange(Aow.Rtl.TObject Sender)
        {
            FSettings.UseDirectSound = DirectSoundEnabledChk.Checked;
            if (Aow.Units.Sound.Sound.SoundSystemStatus().Contains(Aow.Units.Sound.TSoundSystemStatusElement.ssActivated))
            {
                Aow.Units.Sound.Sound.SetSoundSettings(FSettings);
            }

            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSoundDlg.pas:347
        public void SoundTabChange()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWSoundDlg.pas:352
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWSoundDlg
    {
        public static Aow.Rtl.TStringList SongFiles;
    }
}
