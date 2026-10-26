// Generated from TCFCDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.TCFCDlg
{
    // { TTCFCDialog: size $98; $8C combat request, $90 completion event. VMT 00439834. }
    public partial class TTCFCDialog : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel DTitle;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel DMessage;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton FightBtn;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel Tactical;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel Automatic;
        // { +0070 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox TCchk;
        // { +0074 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox FCchk;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel MapViewPnl;
        // { +007C }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox MapView;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage InfoSep;
        // { +0084 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0088 }
        public Aow.Units.AoWE.TCombatTypeRequestEventLog FRequest;
        // { $8C }
        public Aow.Rtl.TNotifyEvent FOnDone;
        // TCFCDlg.pas:119
        // { $90 }
        public void Finish()
        {
            Aow.Rtl.TNotifyEvent Done = default;
            Aow.Units.AoWE.TCombatTypeRequestEventLog Request = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Done = FOnDone;
            Request = FRequest;
            FOnDone = null;
            FRequest = null;
            if (Done != null)
            {
                Done(Request);
            }

            if (Request != null)
            {
                Request.Release();
            }
        }

        // TCFCDlg.pas:57
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FRequest.Select((Aow.Units.AoWE.TCombatType)0);
            Dlg.Visible = false;
        }

        // TCFCDlg.pas:64
        public void FightBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // TCFCDlg.pas:70
        public void FCchkChange(Aow.Rtl.TObject Sender)
        {
            {
                var w__1 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                if (w__1.Checked)
                {
                    FRequest.Select((Aow.Units.AoWE.TCombatType)1);
                }

                if (w__1.Checked)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
                }
                else
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
                }
            }
        }

        // TCFCDlg.pas:79
        public void TCchkChange(Aow.Rtl.TObject Sender)
        {
            {
                var w__2 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                if (w__2.Checked)
                {
                    FRequest.Select((Aow.Units.AoWE.TCombatType)2);
                }

                if (w__2.Checked)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
                }
                else
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
                }
            }
        }

        // TCFCDlg.pas:88
        public void MapViewDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            Aow.Units.GFXE.TSurfaceDesc ViewSD = default;
            Aow.Rtl.TRect R = default;
            int W = default;
            int H = default;
            var call__3 = Aow.Rtl.Rtl.Rect(Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
            R = call__3;
            W = R.Right - R.Left;
            H = R.Bottom - R.Top;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FRequest.FX, FRequest.FY, ref X, ref Y);
            X = (X + 16) - ((int)(((uint)W) >> 1));
            Y = (Y + 16) - ((int)(((uint)H) >> 1));
            if (X < 16)
            {
                X = 16;
            }

            if (Y < 16)
            {
                Y = 16;
            }

            Aow.Units.GFXE.GFXE.CreateSDfromSD(SD, R, ref ViewSD);
            Aow.Units.AoWE.AoWE.AoWHSMap.SetViewPort(X, Y, FRequest.FL, R.Right - R.Left, R.Bottom - R.Top);
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateStaticScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateDynamicScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawStaticScene(ref ViewSD);
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawDynamicScene(ref ViewSD);
        }

        // TCFCDlg.pas:113
        public void MapViewUpdate()
        {
            MapView.FDrawn = MapView.FDrawn && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 1) != 0);
        }

        // TCFCDlg.pas:116
        public void DlgHide()
        {
            Finish();
        }

        // TCFCDlg.pas:133
        public void TCchkDoubleClick(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                FRequest.Select((Aow.Units.AoWE.TCombatType)2);
            }

            Dlg.Visible = false;
        }

        // TCFCDlg.pas:139
        public void FCchkDoubleClick(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                FRequest.Select((Aow.Units.AoWE.TCombatType)1);
            }

            Dlg.Visible = false;
        }
    }

    public static partial class TCFCDlg
    {
    }
}
