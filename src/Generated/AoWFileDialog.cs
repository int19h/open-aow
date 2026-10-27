// Generated from AoWFileDialog.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWorldMap;
using static Aow.Units.AoWWorldMap.AoWWorldMap;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;

namespace Aow.Units.AoWFileDialog
{
    // { TFileDlgWindow (size $14C, VMT 0041EB60): $134 directory, $138 last filename, $13C save event, $140 done callback, $148 dialog mode. }
    public partial class TFileDlgWindow : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow FileDialog;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton CancelBut;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton Openbut;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWListBox.TAOWListBox FileLB;
        // { +0058 }
        public Aow.Units.AoWEdit.TAOWEdit FileName;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel ScenLbl;
        // { +0060 }
        public Aow.Units.AoWComboBox.TAOWComboBox DriveCmb;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel DriveL;
        // { +0068 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton UpDirBtn;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel PathL;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0078 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar DriveSB;
        // { +007C }
        public Aow.Units.AoWImage.TAOWImage CeltM;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage LPlayersI;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel LSavedInfo;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage LSizeI;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage LTownsI;
        // { +0090 }
        public Aow.Units.AoWImage.TAOWImage LTurnsI;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel LPlayers;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel LSize;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel LTowns;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel LTurns;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage CeltL;
        // { +00A8 }
        public Aow.Units.AoWImage.TAOWImage CeltR;
        // { +00AC }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +00B0 }
        public Aow.Units.AoWImage.TAOWImage TCelt;
        // { +00B4 }
        public Aow.Units.AoWImage.TAOWImage SPlayersI;
        // { +00B8 }
        public Aow.Units.AoWPanel.TAOWPanel NewInfo;
        // { +00BC }
        public Aow.Units.AoWImage.TAOWImage SSizeI;
        // { +00C0 }
        public Aow.Units.AoWImage.TAOWImage STownsI;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel SPlayers;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel SSize;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel STowns;
        // { +00D0 }
        public Aow.Units.AoWPanel.TAOWPanel NewPnl;
        // { +00D4 }
        public Aow.Units.AoWListBox.TAOWListBox SaveLB;
        // { +00D8 }
        public Aow.Units.AoWEdit.TAOWEdit SFileName;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel SavedInfo;
        // { +00E0 }
        public Aow.Units.AoWPanel.TAOWPanel SavePnl;
        // { +00E4 }
        public Aow.Units.AoWListBox.TAOWListBox DateLB;
        // { +00E8 }
        public Aow.Units.AoWButton.TAOWButton SaveBtn;
        // { +00EC }
        public Aow.Units.AoWButton.TAOWButton DateBtn;
        // { +00F0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar NSBar;
        // { +00F4 }
        public Aow.Units.AoWPanel.TAOWPanel DriveFr;
        // { +00F8 }
        public Aow.Units.AoWPanel.TAOWPanel TopPnl;
        // { +00FC }
        public Aow.Units.AoWPanel.TAOWPanel PathFr;
        // { +0100 }
        public Aow.Units.AoWEdit.TAOWEdit PathEdit;
        // { +0104 }
        public Aow.Units.AoWPanel.TAOWPanel NewLBFr;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel NewInfo1;
        // { +010C }
        public Aow.Units.AoWPanel.TAOWPanel NewInfo2;
        // { +0110 }
        public Aow.Units.AoWPanel.TAOWPanel NewInfo3;
        // { +0114 }
        public Aow.Units.AoWPanel.TAOWPanel SaveLBFr;
        // { +0118 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SSBar;
        // { +011C }
        public Aow.Units.AoWPanel.TAOWPanel SavedInfo1;
        // { +0120 }
        public Aow.Units.AoWPanel.TAOWPanel SavedInfo2;
        // { +0124 }
        public Aow.Units.AoWPanel.TAOWPanel SavedInfo3;
        // { +0128 }
        public Aow.Units.AoWPanel.TAOWPanel SavedInfo4;
        // { +012C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0130 }
        public Aow.Rtl.AnsiString FDirectory;
        // { $134 }
        public Aow.Rtl.AnsiString FLastFileName;
        // { $138 }
        public Aow.Units.EventLog.TSaveGameEventLog FSaveEvent;
        // { $13C }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $140 }
        // { $144 is the data pointer of FDone. }
        public int FMode;
        // AoWFileDialog.pas:175
        // { $148; 0 load, 1 save, 2 select map }
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TSaveGameEventLog Event = default;
            Done = FDone;
            Event = FSaveEvent;
            FDone = null;
            FSaveEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWFileDialog.pas:132
        public void ExitForLoad(Aow.Units.AoWE.TExitGameMode Mode, bool Success)
        {
            if (Success)
            {
                /* TODO(T-shim) Platform or editor boundary AoWUnit.DeferredSetupAction */
                throw new System.NotImplementedException();
            }
        }

        // AoWFileDialog.pas:137
        public void LoadSelectedGame(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.AoWE.TAoWHSMap Map = default;
            Aow.Units.AoWE.AoWE.AoWEngine.CloseCampaign();
            Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            if (Aow.Rtl.Rtl.LowerCase(Aow.Rtl.Rtl.ExtractFileExt(Name)) == ((AnsiString)".acg"))
            {
                if (!Aow.Units.AoWE.AoWE.AoWEngine.FDemo)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.LoadCampaign(Aow.Units.AoWWinMan.AoWWinMan.General, Name);
                    if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                    {
                        Aow.Units.AoWWinMan.AoWWinMan.StartGame(Aow.Units.AoWWinMan.AoWWinMan.General);
                    }
                }
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWEngine.CloseCampaign();
                var call__1 = Aow.Units.AoWE.AoWE.AoWEngine.CreatePreviewHSM(Name);
                Map = (Aow.Units.AoWE.TAoWHSMap)call__1;
                if (Map != null)
                {
                    try
                    {
                        if (Map.FConnectionType != Aow.Units.AoWE.TPlayerConnectionType.pcNetwork)
                        {
                            Aow.Units.AoWWinMan.AoWWinMan.LoadGame(Aow.Units.AoWWinMan.AoWWinMan.General, Name);
                            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                            {
                                Aow.Units.AoWWinMan.AoWWinMan.StartGame(Aow.Units.AoWWinMan.AoWWinMan.General);
                            }
                        }
                        else
                        {
                            Aow.Units.E2Globals.E2Globals.TitleScene.HideScene();
                            Aow.Units.AoWWinMan.AoWWinMan.General.HideWindows();
                            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.Visible = false;
                            var call__2 = Aow.Rtl.Rtl.ExtractRelativePath(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory, Name);
                            Aow.Units.AoWExeForms.AoWExeForms.MultiWindow.Execute(call__2);
                        }
                    }
                    finally
                    {
                        Map.Free();
                    }
                }
            }
        }

        // AoWFileDialog.pas:188
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FSaveEvent != null)
            {
                if (!FSaveEvent.FMode.Contains(Aow.Units.EventLog.TSaveGameEventLogModeElement.sgmDisableCancel))
                {
                    FileDialog.Visible = false;
                }
            }
            else
            {
                FileDialog.Visible = false;
            }
        }

        // AoWFileDialog.pas:198
        // { draft }
        public void FindDrives()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.GetLogicalDrives */
            throw new System.NotImplementedException();
        }

        // AoWFileDialog.pas:233
        public void FindFiles()
        {
            bool IncludeFile = default;
            Aow.Rtl.AnsiString Extension = default;
            Aow.Rtl.AnsiString Directory = default;
            int I = default;
            Aow.Rtl.TSearchRec Search = default;
            Aow.Units.AoWWorldMap.TFileTime Stamp = default;
            {
                I = 0;
                int end__3 = DateLB.Items.Count - 1;
                for (; I <= end__3; I += 1)
                {
                    var call__4 = DateLB.Items.Objects[I];
                    if (call__4 != null)
                    {
                        var call__5 = DateLB.Items.Objects[I];
                        call__5.Free();
                    }
                }
            }

            Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Clear();
            Aow.Units.AoWFileDialog.AoWFileDialog.Files.Clear();
            SPlayers.Text = (AnsiString)"";
            SSize.Text = (AnsiString)"";
            STowns.Text = (AnsiString)"";
            LPlayers.Text = (AnsiString)"";
            LSize.Text = (AnsiString)"";
            LTowns.Text = (AnsiString)"";
            LTurns.Text = (AnsiString)"";
            Directory = Aow.Rtl.Rtl.GetCurrentDir();
            if (Aow.Rtl.Rtl.Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
            {
                Directory = Aow.Rtl.Rtl.Copy(Directory, 1, Directory.Length - 1);
            }

            if (Aow.Rtl.Rtl.FindFirst(Directory + ((AnsiString)"\\**"), 16, ref Search) == 0)
            {
                do
                {
                    if ((Search.Name != AnsiString.FromBytes(46)) && (Search.Name != ((AnsiString)"..")))
                    {
                        if ((Search.Attr & 16) == 16)
                        {
                            Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Add(Search.Name);
                        }
                        else
                        {
                            IncludeFile = false;
                            var call__6 = Aow.Rtl.Rtl.ExtractFileExt(Search.Name);
                            Extension = Aow.Rtl.Rtl.LowerCase(call__6);
                            IncludeFile = IncludeFile || (((FMode == 1) && (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)) && (Extension == ((AnsiString)".acg")));
                            IncludeFile = IncludeFile || (((FMode == 1) && (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign == null)) && (Extension == ((AnsiString)".asg")));
                            IncludeFile = IncludeFile || ((FMode == 0) && ((Extension == ((AnsiString)".asg")) || (Extension == ((AnsiString)".acg"))));
                            // { Original accepts .asg in every mode, including map selection. }
                            IncludeFile = (IncludeFile || ((FMode == 2) && (Extension == ((AnsiString)".hsm")))) || (Extension == ((AnsiString)".asg"));
                            if (IncludeFile)
                            {
                                Stamp = Aow.Units.AoWWorldMap.TFileTime.Create();
                                var call__7 = Aow.Rtl.Rtl.FileDateToDateTime(Search.Time);
                                Stamp.FTime = call__7;
                                Aow.Units.AoWFileDialog.AoWFileDialog.Files.AddObject(Search.Name, Stamp);
                            }
                        }
                    }
                }
                while (!(Aow.Rtl.Rtl.FindNext(ref Search) != 0));
            }

            Aow.Rtl.Rtl.FindClose(ref Search);
            FileLB.Selected = -1;
            SaveLB.Selected = -1;
            DateLB.Selected = -1;
            FileLB.ListOffset = 0;
            SaveLB.ListOffset = 0;
            DateLB.ListOffset = 0;
            FileLB.Items.Clear();
            SaveLB.Items.Clear();
            DateLB.Items.Clear();
            Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Sort();
            Aow.Units.AoWFileDialog.AoWFileDialog.Files.Sort();
            FileLB.Items.AddStrings(Aow.Units.AoWFileDialog.AoWFileDialog.Directories);
            FileLB.Items.AddStrings(Aow.Units.AoWFileDialog.AoWFileDialog.Files);
            SaveLB.Items.AddStrings(FileLB.Items);
            PathEdit.Text = Directory + AnsiString.FromBytes(92);
            {
                I = 0;
                int end__8 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count - 1;
                for (; I <= end__8; I += 1)
                {
                    DateLB.Items.Add((AnsiString)"");
                }
            }

            {
                I = 0;
                int end__9 = Aow.Units.AoWFileDialog.AoWFileDialog.Files.Count - 1;
                for (; I <= end__9; I += 1)
                {
                    var call__10 = Aow.Units.AoWFileDialog.AoWFileDialog.Files.Objects[I];
                    var call__11 = Aow.Rtl.Rtl.DateTimeToStr(((Aow.Units.AoWWorldMap.TFileTime)call__10).FTime);
                    var call__12 = Aow.Units.AoWFileDialog.AoWFileDialog.Files.Objects[I];
                    DateLB.Items.AddObject(call__11, call__12);
                }
            }
        }

        // AoWFileDialog.pas:296
        public void FileDlgWindowCreate()
        {
            var call__13 = new Aow.Rtl.TStringList();
            Aow.Units.AoWFileDialog.AoWFileDialog.Drives = call__13;
            var call__14 = new Aow.Rtl.TStringList();
            Aow.Units.AoWFileDialog.AoWFileDialog.Directories = call__14;
            var call__15 = new Aow.Rtl.TStringList();
            Aow.Units.AoWFileDialog.AoWFileDialog.Files = call__15;
            FDirectory = Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Save\\");
            FLastFileName = (AnsiString)"";
        }

        // AoWFileDialog.pas:305
        public void FileDlgWindowDestroy()
        {
            int I = default;
            {
                I = 0;
                int end__16 = DateLB.Items.Count - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = DateLB.Items.Objects[I];
                    if (call__17 != null)
                    {
                        var call__18 = DateLB.Items.Objects[I];
                        call__18.Free();
                    }
                }
            }

            Aow.Units.AoWFileDialog.AoWFileDialog.Drives.Free();
            Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Free();
            Aow.Units.AoWFileDialog.AoWFileDialog.Files.Free();
        }

        // AoWFileDialog.pas:316
        public void OpenbutClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Path = default;
            Aow.Rtl.AnsiString Directory = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__19 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count;
            if ((FileLB.Selected < call__19) && (FileLB.Selected > -1))
            {
                Directory = Aow.Rtl.Rtl.GetCurrentDir();
                if (Aow.Rtl.Rtl.Copy(Directory, Directory.Length, 1) == AnsiString.FromBytes(92))
                {
                    Directory = Aow.Rtl.Rtl.Copy(Directory, 1, Directory.Length - 1);
                }

                var call__20 = FileLB.Items[FileLB.Selected];
                var call__21 = Aow.Rtl.Rtl.SetCurrentDir((Directory + AnsiString.FromBytes(92)) + call__20);
                if (call__21)
                {
                    FindFiles();
                }

                FileLB.Selected = -1;
            }
            else
            {
                Path = Aow.Rtl.Rtl.GetCurrentDir();
                if ((Path.Length > 0) && (Path[Path.Length] != 92))
                {
                    Path = Path + AnsiString.FromBytes(92);
                }

                switch (FMode)
                {
                    case 0:
                        bool bool__22 = (SFileName.Text != ((AnsiString)""));
                        if (bool__22)
                        {
                            var call__23 = Aow.Rtl.Rtl.FileExists(SFileName.Text);
                            bool__22 = call__23;
                        }

                        if (bool__22)
                        {
                            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                            {
                                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null)
                                {
                                    Aow.Units.AoWWinMan.AoWWinMan.General.ReturnToTitle();
                                }
                                else
                                {
                                    if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
                                    {
                                        if (Aow.Rtl.Rtl.CompareText(Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FFileName, FDirectory + SFileName.Text) == 0)
                                        {
                                            Aow.Units.AoWE.AoWE.AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmReload, ExitForLoad);
                                        }
                                        else
                                        {
                                            Aow.Units.AoWE.AoWE.AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmLoad, ExitForLoad);
                                        }
                                    }
                                    else
                                    {
                                        if (Aow.Rtl.Rtl.CompareText(Aow.Units.AoWE.AoWE.AoWHSMap.FileName, FDirectory + SFileName.Text) == 0)
                                        {
                                            Aow.Units.AoWE.AoWE.AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmReload, ExitForLoad);
                                        }
                                        else
                                        {
                                            Aow.Units.AoWE.AoWE.AoWHSMap.ExitGame(Aow.Units.AoWE.TExitGameMode.egmLoad, ExitForLoad);
                                        }
                                    }
                                }
                            }

                            if ((Aow.Units.AoWE.AoWE.AoWHSMap == null) || Aow.Units.AoWE.AoWE.AoWHSMap.FExitPending)
                            {
                                FileDialog.Visible = false;
                                LoadSelectedGame(Path + SFileName.Text);
                            }

                            FLastFileName = Path + SFileName.Text;
                            FDirectory = Path;
                        }

                        break;
                    case 1:
                        if (SFileName.Text != ((AnsiString)""))
                        {
                            FDirectory = Path;
                            if (Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(46), SFileName.Text) != 0)
                            {
                                SFileName.Text = Aow.Rtl.Rtl.Copy(SFileName.Text, 1, Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(46), SFileName.Text) - 1);
                            }

                            if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
                            {
                                FileDialog.Visible = false;
                                SFileName.Text = SFileName.Text + ((AnsiString)".acg");
                                Aow.Units.E1GameFlow.E1GameFlow.SaveCampaign(Aow.Units.AoWWinMan.AoWWinMan.General, SFileName.Text);
                            }
                            else
                            {
                                if (FSaveEvent != null)
                                {
                                    FSaveEvent.FFileName = SFileName.Text + ((AnsiString)".asg");
                                    FileDialog.Visible = false;
                                }
                                else
                                {
                                    FileDialog.Visible = false;
                                    SFileName.Text = SFileName.Text + ((AnsiString)".asg");
                                    Aow.Units.E1GameFlow.E1GameFlow.SaveGame(Aow.Units.AoWWinMan.AoWWinMan.General, SFileName.Text);
                                }
                            }

                            FLastFileName = SFileName.Text;
                            FDirectory = Path;
                        }

                        break;
                    case 2:
                        bool bool__24 = (FileName.Text != ((AnsiString)""));
                        if (bool__24)
                        {
                            var call__25 = Aow.Rtl.Rtl.FileExists(FileName.Text);
                            bool__24 = call__25;
                        }

                        if (bool__24)
                        {
                            Aow.Units.E1GameFlow.E1GameFlow.PickScenarioMap(Aow.Units.AoWExeForms.AoWExeForms.ScenSetup, FileName.Text);
                            FLastFileName = FileName.Text;
                        }

                        break;
                }
            }
        }

        // AoWFileDialog.pas:394
        // { draft }
        public void UpDirBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Rtl.Rtl.SetCurrentDir((AnsiString)".."))
            {
                FindFiles();
            }
        }

        // AoWFileDialog.pas:400
        public void DriveCmbChange()
        {
            var call__26 = DriveCmb.Items[DriveCmb.Selected];
            var call__27 = DriveCmb.Items[DriveCmb.Selected];
            var call__28 = Aow.Rtl.Rtl.SetCurrentDir(Aow.Rtl.Rtl.Copy(call__27, call__26.Length - 3, 3));
            if (call__28)
            {
                FindFiles();
            }
            else
            {
                FindDrives();
            }
        }

        // AoWFileDialog.pas:406
        public void FileLBChange(Aow.Units.AoWListBox.TAOWListBox Sender)
        {
            Aow.Units.AoWE.TAoWHSMap Map = default;
            Aow.Units.AoWListBox.TAOWListBox List = default;
            List = Sender;
            FileLB.Selected = List.Selected;
            SaveLB.Selected = List.Selected;
            DateLB.Selected = List.Selected;
            var call__29 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count;
            if ((call__29 - 1) < this.FileLB.Selected)
            {
                var call__30 = FileLB.Items[FileLB.Selected];
                SFileName.Text = call__30;
                var call__31 = Aow.Rtl.Rtl.ExtractFileExt(SFileName.Text);
                if (Aow.Rtl.Rtl.LowerCase(call__31) != ((AnsiString)".acg"))
                {
                    var call__32 = Aow.Units.AoWE.AoWE.AoWEngine.CreatePreviewHSM(SFileName.Text);
                    Map = (Aow.Units.AoWE.TAoWHSMap)call__32;
                    if (Map != null)
                    {
                        var call__33 = Map.FPlayers.GetCount();
                        LPlayers.Text = Aow.Rtl.Rtl.IntToStr(call__33 - 1);
                        switch (Map.MapContainer.Width)
                        {
                            case 48:
                                var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(AnsiString.FromBytes(Aow.Units.AoWE.AoWE.SmallMapAbrRStr));
                                LSize.Text = call__34;
                                break;
                            case 64:
                                var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(AnsiString.FromBytes(Aow.Units.AoWE.AoWE.MediumMapAbrRStr));
                                LSize.Text = call__35;
                                break;
                            case 96:
                                var call__36 = Aow.Units.AoWE.AoWE.TranslateRStr(AnsiString.FromBytes(Aow.Units.AoWE.AoWE.LargeMapAbrRStr));
                                LSize.Text = call__36;
                                break;
                            case 127:
                            case 128:
                                var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExtraLargeMapAbrRStr);
                                LSize.Text = call__37;
                                break;
                            default:
                                LSize.Text = AnsiString.FromBytes(63);
                                break;
                        }

                        LTowns.Text = Aow.Rtl.Rtl.IntToStr(Map.FStructureControl.FCityCount);
                        LTurns.Text = Aow.Rtl.Rtl.IntToStr(Map.FDay);
                        Map.Free();
                    }
                    else
                    {
                        LPlayers.Text = (AnsiString)"";
                        LSize.Text = (AnsiString)"";
                        LTowns.Text = (AnsiString)"";
                        LTurns.Text = (AnsiString)"";
                    }
                }
                else
                {
                    LPlayers.Text = (AnsiString)"";
                    LSize.Text = (AnsiString)"";
                    LTowns.Text = (AnsiString)"";
                    LTurns.Text = (AnsiString)"";
                }
            }
            else
            {
                LPlayers.Text = (AnsiString)"";
                LSize.Text = (AnsiString)"";
                LTowns.Text = (AnsiString)"";
                LTurns.Text = (AnsiString)"";
            }

            FileName.Text = SFileName.Text;
            SPlayers.Text = LPlayers.Text;
            SSize.Text = LSize.Text;
            STowns.Text = LTowns.Text;
        }

        // AoWFileDialog.pas:586
        public void FileLBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            var call__38 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count;
            if (call__38 > DrawItem)
            {
                var call__39 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[0];
                call__39.ShowClipped(DrawRect.Left + 2, DrawRect.Top + 1, SurfDesc);
            }
        }

        // AoWFileDialog.pas:462
        public void FileDialogShow()
        {
            switch (FMode)
            {
                case 0:
                    Aow.Rtl.Rtl.SetCurrentDir(FDirectory);
                    var call__40 = Aow.Rtl.Rtl.ExtractFileName(FLastFileName);
                    SFileName.Text = call__40;
                    FileName.Text = SFileName.Text;
                    var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectAFileToLoadRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Title.Text = call__41;
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.SFileName.Editable = false;
                    var call__42 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OpenRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Openbut.Text = call__42;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 1:
                    Aow.Rtl.Rtl.SetCurrentDir(FDirectory);
                    var call__43 = Aow.Rtl.Rtl.ExtractFileName(FLastFileName);
                    SFileName.Text = call__43;
                    if (Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(46), SFileName.Text) != 0)
                    {
                        SFileName.Text = Aow.Rtl.Rtl.Copy(SFileName.Text, 1, Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(46), SFileName.Text) - 1);
                    }

                    if (SFileName.Text != ((AnsiString)""))
                    {
                        if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
                        {
                            SFileName.Text = SFileName.Text + ((AnsiString)".acg");
                        }
                        else
                        {
                            SFileName.Text = SFileName.Text + ((AnsiString)".asg");
                        }
                    }

                    FileName.Text = SFileName.Text;
                    var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SaveGameAsRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Title.Text = call__44;
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.SFileName.Editable = true;
                    var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SaveRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Openbut.Text = call__45;
                    SavePnl.Visible = true;
                    NewPnl.Visible = false;
                    break;
                case 2:
                    var call__46 = Aow.Rtl.Rtl.ExtractFileExt(FLastFileName);
                    if (call__46 == ((AnsiString)".hsm"))
                    {
                        var call__47 = Aow.Rtl.Rtl.ExtractFileName(FLastFileName);
                        FileName.Text = call__47;
                    }
                    else
                    {
                        FileName.Text = (AnsiString)"";
                    }

                    Aow.Rtl.Rtl.SetCurrentDir(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Scenario\\Custom\\"));
                    var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectAMapRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Title.Text = call__48;
                    var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SelectRStr);
                    Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.Openbut.Text = call__49;
                    SavePnl.Visible = false;
                    NewPnl.Visible = true;
                    break;
            }

            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FindDrives();
        }

        // AoWFileDialog.pas:502
        public void mSBarChange(Aow.Units.AoWVScroll.TAOWVScrollBar Sender)
        {
            Aow.Units.AoWVScroll.TAOWVScrollBar Bar = default;
            Bar = Sender;
            FileLB.ListOffset = Bar.Position;
            SaveLB.ListOffset = Bar.Position;
            DateLB.ListOffset = Bar.Position;
        }

        // AoWFileDialog.pas:511
        public void SFileNameReturn()
        {
            OpenbutClick(this);
        }

        // AoWFileDialog.pas:516
        public void SFileNameChange(int A1)
        {
            FileLB.Selected = -1;
            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:522
        public void SaveBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Selected = default;
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                var call__50 = FileLB.Items[FileLB.Selected];
                Selected = call__50;
            }

            {
                I = 0;
                int end__51 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count - 1;
                for (; I <= end__51; I += 1)
                {
                    SaveLB.Items.Delete(0);
                    DateLB.Items.Delete(0);
                }
            }

            if ((FMode == 1) || (FMode == 0))
            {
                Aow.Units.Engine.Engine.SortStringsAscending(SaveLB.Items, [DateLB.Items, FileLB.Items]);
                FileLB.Items.Assign(SaveLB.Items);
            }
            else
            {
                if (FMode == 2)
                {
                    Aow.Units.Engine.Engine.SortStringsAscending(FileLB.Items, [DateLB.Items, SaveLB.Items]);
                }
            }

            {
                I = 0;
                int end__52 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories.Count - 1;
                for (; I <= end__52; I += 1)
                {
                    var call__53 = Aow.Units.AoWFileDialog.AoWFileDialog.Directories[I];
                    SaveLB.Items.Insert(I, call__53);
                    DateLB.Items.Insert(I, (AnsiString)"");
                }
            }

            if (Selected != ((AnsiString)""))
            {
                var call__54 = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = call__54;
            }

            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:549
        public void DateBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Selected = default;
            int I = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FileLB.Selected != -1)
            {
                var call__55 = FileLB.Items[FileLB.Selected];
                Selected = call__55;
            }

            if ((FMode == 1) || (FMode == 0))
            {
                {
                    I = 0;
                    int end__56 = SaveLB.Items.Count - 1;
                    for (; I <= end__56; I += 1)
                    {
                        var call__57 = DateLB.Items.Objects[I];
                        SaveLB.Items.Objects[I] = call__57;
                    }
                }

                Aow.Units.Engine.Engine.SortStringsEx(SaveLB.Items, Aow.Units.AoWFileDialog.AoWFileDialog.CompareFileDates, [DateLB.Items, FileLB.Items]);
            }
            else
            {
                if (FMode == 2)
                {
                    {
                        I = 0;
                        int end__58 = FileLB.Items.Count - 1;
                        for (; I <= end__58; I += 1)
                        {
                            var call__59 = DateLB.Items.Objects[I];
                            FileLB.Items.Objects[I] = call__59;
                        }
                    }

                    Aow.Units.Engine.Engine.SortStringsEx(FileLB.Items, Aow.Units.AoWFileDialog.AoWFileDialog.CompareFileDates, [DateLB.Items, SaveLB.Items]);
                }
            }

            if (Selected != ((AnsiString)""))
            {
                var call__60 = FileLB.Items.IndexOf(Selected);
                FileLB.Selected = call__60;
            }

            FileLBChange(FileLB);
        }

        // AoWFileDialog.pas:570
        public void SSBarChange()
        {
            SaveLB.ListOffset = SSBar.Position;
        }

        // AoWFileDialog.pas:575
        public void FileDialogHide()
        {
            FinishDialog();
        }

        // AoWFileDialog.pas:580
        public void FileDialogKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                FileDialog.Visible = false;
            }
        }
    }

    public static partial class AoWFileDialog
    {
        public static Aow.Units.AoWFileDialog.TFileDlgWindow FileDlgWindow;
        public static Aow.Rtl.TStringList Drives;
        public static Aow.Rtl.TStringList Directories;
        public static Aow.Rtl.TStringList Files;
        // AoWFileDialog.pas:121
        public static int CompareFileDates(Aow.Rtl.AnsiString S1, Aow.Rtl.AnsiString S2, Aow.Rtl.TObject O1, Aow.Rtl.TObject O2)
        {
            int Result = default;
            if ((O1 == null) && (O2 == null))
            {
                Result = Aow.Rtl.Rtl.CompareText(S1, S2);
            }
            else
            {
                if (O1 == null)
                {
                    Result = -1;
                }
                else
                {
                    if (O2 == null)
                    {
                        Result = 1;
                    }
                    else
                    {
                        if (((double)((Aow.Units.AoWWorldMap.TFileTime)O2).FTime) > ((Aow.Units.AoWWorldMap.TFileTime)O1).FTime)
                        {
                            Result = 1;
                        }
                        else
                        {
                            if (((double)((Aow.Units.AoWWorldMap.TFileTime)O2).FTime) < ((Aow.Units.AoWWorldMap.TFileTime)O1).FTime)
                            {
                                Result = -1;
                            }
                            else
                            {
                                Result = 0;
                            }
                        }
                    }
                }
            }

            return Result;
        }
    }
}
