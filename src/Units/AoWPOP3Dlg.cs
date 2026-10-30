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
    // { TPOP3Dlg (instance size 168, VMT 00429ECC) }
    public partial class TPOP3Dlg : Aow.Rtl.TDataModule
    {
        public TPOP3Dlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel SearchingEmailGameLbl;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel EmailGameFoundLbl;
        // { +0064 }
        public Aow.Units.AoWListBox.TAOWListBox FoundEmailGames;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel ExitPnl;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton Exit2Btn;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel StartExitPnl;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton StartBtn;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        // { +007C }
        public Aow.Units.AoWEdit.TAOWEdit LocalEmailAddressEdit;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel AttachmentDirectoryLbl;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel EditPnl;
        // { +008C }
        public Aow.Units.AoWPanel.TAOWPanel ModePnl;
        // { +0090 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar EmailGamesSB;
        // { +0094 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0098 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +009C }
        public bool FSearching;
        public Aow.Units.FixedArray_byte_3 FReservedA1;
        // { $A0 }
        public int FCheckDelay;
        // AoWPOP3Dlg.pas:64
        // { $A4 }
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
            /* TODO(T-shim) Platform or editor boundary Windows.CreateFile */
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
        // { draft }
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
        // { draft }
        public void ExitBtnClick()
        {
            Dlg.Visible = false;
        }

        // AoWPOP3Dlg.pas:152
        // { draft }
        public void FoundEmailGamesChange()
        {
            UpdateSelection();
        }

        // AoWPOP3Dlg.pas:157
        // { draft }
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
        // { draft }
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
