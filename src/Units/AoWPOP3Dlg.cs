// Translated from AoWPOP3Dlg.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;

namespace Aow.Units.AoWPOP3Dlg
{
    public partial class TPOP3Dlg : Aow.Rtl.TDataModule
    {
        public TPOP3Dlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        public Aow.Units.AoWLabel.TAOWLabel SearchingEmailGameLbl;
        public Aow.Units.AoWLabel.TAOWLabel EmailGameFoundLbl;
        public Aow.Units.AoWListBox.TAOWListBox FoundEmailGames;
        public Aow.Units.AoWPanel.TAOWPanel ExitPnl;
        public Aow.Units.AoWButton.TAOWButton Exit2Btn;
        public Aow.Units.AoWPanel.TAOWPanel StartExitPnl;
        public Aow.Units.AoWButton.TAOWButton StartBtn;
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        public Aow.Units.AoWEdit.TAOWEdit LocalEmailAddressEdit;
        public Aow.Units.AoWLabel.TAOWLabel AttachmentDirectoryLbl;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWPanel.TAOWPanel EditPnl;
        public Aow.Units.AoWPanel.TAOWPanel ModePnl;
        public Aow.Units.AoWVScroll.TAOWVScrollBar EmailGamesSB;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Rtl.TIvTranslator Translator;
        public bool FSearching;
        public int FCheckDelay;
        // AoWPOP3Dlg.pas:64
        public void ResetSearch()
        {
            FCheckDelay = 30;
            FSearching = false;
            FoundEmailGames.Items.Clear();
            ExitPnl.Visible = true;
            StartExitPnl.Visible = false;
        }

        // AoWPOP3Dlg.pas:73
        public void UpdateSelection()
        {
            var call__1 = FoundEmailGames.Items.Count;
            if ((call__1 != 0) && (FoundEmailGames.Selected != -1))
            {
                StartExitPnl.Visible = true;
                ExitPnl.Visible = false;
            }
            else
            {
                StartExitPnl.Visible = false;
                ExitPnl.Visible = true;
            }
        }

        // AoWPOP3Dlg.pas:87
        public int CheckAttachments()
        {
            /* TODO: Implement Windows.CreateFile in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AoWPOP3Dlg.pas:121
        public void ShowDialog()
        {
            ResetSearch();
            var call__2 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = call__2;
            var call__3 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetAttachmentDirectory();
            var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AttachmentDirectoryXRStr);
            var call__5 = Aow.Units.AoWE.AoWE.Format(call__4, [call__3]);
            AttachmentDirectoryLbl.Text = (AnsiString.FromBytes(40) + call__5) + AnsiString.FromBytes(41);
            Dlg.Visible = true;
        }

        // AoWPOP3Dlg.pas:130
        public void CancelBtnClick()
        {
            Dlg.Visible = false;
        }

        // AoWPOP3Dlg.pas:135
        public void DlgUpdate()
        {
            FCheckDelay -= 1;
            if (FCheckDelay < 0)
            {
                var call__6 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetCheckEmailFrequency();
                FCheckDelay = (call__6 * 30) * 60;
                if (FCheckDelay < 30)
                {
                    FCheckDelay = 30;
                }

                // { Original overrides the configured interval. }
                FCheckDelay = 150;
            }

            switch (FCheckDelay)
            {
                case 25:
                    CheckAttachments();
                    break;
            }
        }

        // AoWPOP3Dlg.pas:147
        public void ExitBtnClick()
        {
            Dlg.Visible = false;
        }

        // AoWPOP3Dlg.pas:152
        public void FoundEmailGamesChange()
        {
            UpdateSelection();
        }

        // AoWPOP3Dlg.pas:157
        public void StartBtnClick()
        {
            if (FoundEmailGames.Selected != -1)
            {
                var call__7 = FoundEmailGames.Items[FoundEmailGames.Selected];
                Aow.Units.AoWWinMan.AoWWinMan.LoadGame(Aow.Units.AoWWinMan.AoWWinMan.General, (Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"EmailIn\\")) + call__7);
                Aow.Units.AoWWinMan.AoWWinMan.StartGame(Aow.Units.AoWWinMan.AoWWinMan.General);
            }
        }

        // AoWPOP3Dlg.pas:166
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWPOP3Dlg
    {
    }
}
