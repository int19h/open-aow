// Translated from AoWSelFactionWin.pas.
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
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.E1Resources;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWSelFactionWin
{
    // { TSelFactionWin (instance size 144, VMT 00418940) }
    public partial class TSelFactionWin : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWPanel.TAOWPanel FactionPnl;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel TopLbl;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel LightPnl;
        // { +004C }
        public Aow.Units.AoWMemo.TAOWMemo LText;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton LStart;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton LBack;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage LSphere;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel DarkPnl;
        // { +0060 }
        public Aow.Units.AoWButton.TAOWButton DStart;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton DBack;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel LName;
        // { +006C }
        public Aow.Units.AoWMemo.TAOWMemo DText;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel DName;
        // { +0074 }
        public Aow.Units.AoWImage.TAOWImage DSphere;
        // { +0078 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +007C }
        public int FFade;
        // { $80 }
        public int FFadeImage;
        // { $84 }
        public int FFadeDirection;
        // { $88 }
        public Aow.Units.FixedArray_byte_4 F_SelFactionWin_8C;
        // AoWSelFactionWin.pas:63
        public void CloseSelection()
        {
            Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
            Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, true);
            FactionPnl.Visible = false;
            FFade = 0;
        }

        // AoWSelFactionWin.pas:72
        public void LStartClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
            FactionPnl.Visible = false;
            FFade = 0;
            if (Sender == LStart)
            {
                Aow.Units.E2GameFlow.E2GameFlow.StartCampaignJoin(Aow.Units.E2Globals.E2Globals.CampaignJoinDlg, 0);
            }
            else
            {
                Aow.Units.E2GameFlow.E2GameFlow.StartCampaignJoin(Aow.Units.E2Globals.E2Globals.CampaignJoinDlg, 1);
            }
        }

        // AoWSelFactionWin.pas:82
        public void LightPnlMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!DName.Visible && !LName.Visible)
            {
                DName.Visible = true;
                DText.Visible = true;
                DBack.Visible = true;
                DStart.Visible = true;
                DBack.CaptureInput = true;
                DStart.CaptureInput = true;
                TopLbl.Visible = false;
                FFade = 100;
                FactionPnl.ILIndexTexture = 2;
                FFadeImage = 0;
                FFadeDirection = 1;
            }
        }

        // AoWSelFactionWin.pas:100
        public void DarkPnlMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!LName.Visible && !DName.Visible)
            {
                LName.Visible = true;
                LText.Visible = true;
                LBack.Visible = true;
                LStart.Visible = true;
                LBack.CaptureInput = true;
                LStart.CaptureInput = true;
                TopLbl.Visible = false;
                FFade = 100;
                FactionPnl.ILIndexTexture = 1;
                FFadeImage = 0;
                FFadeDirection = 1;
            }
        }

        // AoWSelFactionWin.pas:118
        public void LBackClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            LBack.CaptureInput = false;
            LStart.CaptureInput = false;
            FFade = 100;
            FFadeImage = 0;
            FFadeDirection = 0;
        }

        // AoWSelFactionWin.pas:128
        public void DBackClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            DBack.CaptureInput = false;
            DStart.CaptureInput = false;
            FFade = 100;
            FFadeImage = 0;
            FFadeDirection = 0;
        }

        // AoWSelFactionWin.pas:138
        public void FactionPnlMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (((!LightPnl.FHandled && !DName.Visible) && !DarkPnl.FHandled) && !LName.Visible)
            {
                DSphere.Visible = false;
                LSphere.Visible = false;
                var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ChooseFactionRStr);
                TopLbl.Text = call__1;
            }
        }

        // AoWSelFactionWin.pas:148
        public void LightPnlMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!LName.Visible && !DSphere.Visible)
            {
                DSphere.Visible = true;
                LSphere.Visible = false;
                var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.CultOfStormsRStr);
                TopLbl.Text = call__2;
            }
        }

        // AoWSelFactionWin.pas:158
        public void DarkPnlMouseMove(Aow.Rtl.TObject Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!DName.Visible && !LSphere.Visible)
            {
                DSphere.Visible = false;
                LSphere.Visible = true;
                var call__3 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.KeepersRStr);
                TopLbl.Text = call__3;
            }
        }

        // AoWSelFactionWin.pas:168
        public void AbortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            CloseSelection();
        }

        // AoWSelFactionWin.pas:174
        public void FactionPnlUpdate()
        {
            Aow.Rtl.TRect R = default;
            if (Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFade >= 0)
            {
                Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFade -= 6;
                if (FFade < 6)
                {
                    if (LStart.Visible && !LStart.CaptureInput)
                    {
                        LName.Visible = false;
                        LText.Visible = false;
                        LBack.Visible = false;
                        LStart.Visible = false;
                        LSphere.Visible = false;
                        TopLbl.Visible = true;
                        var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ChooseFactionRStr);
                        TopLbl.Text = call__4;
                        FactionPnl.ILIndexTexture = 0;
                    }
                    else
                    {
                        if (DStart.Visible && !DStart.CaptureInput)
                        {
                            DName.Visible = false;
                            DText.Visible = false;
                            DBack.Visible = false;
                            DStart.Visible = false;
                            DSphere.Visible = false;
                            TopLbl.Visible = true;
                            var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ChooseFactionRStr);
                            TopLbl.Text = call__5;
                            FactionPnl.ILIndexTexture = 0;
                        }
                    }
                }

                var call__7 = Aow.Rtl.Rtl.Rect(this.FactionPnl.WinLeft, this.FactionPnl.WinTop, this.FactionPnl.WinLeft + this.FactionPnl.WinWidth, FactionPnl.WinTop + this.FactionPnl.WinHeight);
                R = call__7;

                Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.AddBltRect(ref R, null);
            }
        }

        // AoWSelFactionWin.pas:210
        public void FactionPnlKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    CloseSelection();
                    break;
            }
        }
    }

    public static partial class AoWSelFactionWin
    {
        public static Aow.Units.AoWSelFactionWin.TSelFactionWin SelFactionWin;
        // AoWSelFactionWin.pas:215
        public static void ShowFactionSelection(Aow.Units.AoWSelFactionWin.TSelFactionWin Dialog)
        {
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.Visible = true;
            Dialog.TopLbl.Visible = true;
            var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ChooseFactionRStr);
            Dialog.TopLbl.Text = call__8;
            Dialog.LName.Visible = false;
            Dialog.LText.Visible = false;
            Dialog.LBack.Visible = false;
            Dialog.LStart.Visible = false;
            Dialog.LSphere.Visible = false;
            Dialog.DName.Visible = false;
            Dialog.DText.Visible = false;
            Dialog.DBack.Visible = false;
            Dialog.DStart.Visible = false;
            Dialog.DSphere.Visible = false;
            Dialog.FFade = -1;
            Dialog.FactionPnl.ILIndexTexture = 0;
            Dialog.F_SelFactionWin_8C[0] = 0;
        }
    }
}
