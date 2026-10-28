// Translated from AoWCityJoinDlg.pas.
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
using Aow.Units.AoWArnoLine;
using static Aow.Units.AoWArnoLine.AoWArnoLine;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWCityJoinDlg
{
    // { TCityJoinDlg (size $90, VMT 004371B4): $80 race, $84 completion event, $8C retained negotiation event. }
    public partial class TCityJoinDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel ApproachLbl;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame CeltFrame;
        // { +0050 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0054 }
        public Aow.Units.AoWMemo.TAOWMemo Memo;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton AttackBtn;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0068 }
        public Aow.Units.AoWButton.TAOWButton AcceptBtn;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel WhatLbl;
        // { +0070 }
        public Aow.Units.AoWArnoLine.TAoWArnoLine CityPicLine;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        // { +0078 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +007C }
        public sbyte FRace;
        // { $80 }
        public Aow.Units.FixedArray_byte_3 FReserved81;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $84 }
        public Aow.Units.AoWE.TNegotiateRequestEventLog FEvent;
        // AoWCityJoinDlg.pas:55
        // { $8C }
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.AoWE.TNegotiateRequestEventLog Log = default;
            Done = FDone;
            Log = FEvent;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Log);
            }

            if (Log != null)
            {
                Log.Release();
            }
        }

        // AoWCityJoinDlg.pas:69
        // { 004373A8 }
        public void AcceptBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__1 = FEvent.Accept(ref Reason);
            if (!call__1)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }

        // AoWCityJoinDlg.pas:78
        // { draft }
        public void AttackBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__2 = FEvent.Reject(ref Reason);
            if (!call__2)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }

        // AoWCityJoinDlg.pas:87
        // { draft }
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWCityJoinDlg.pas:93
        // { draft }
        public void DlgDrawBack(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.ImageLib.TLibraryImage Face = default;
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
            var call__4 = call__3.GetFace();
            Face = call__4;
            {
                var w__5 = Face;
                if (Face != null)
                {
                    w__5.ShowClipped(CeltFrame.WinLeft + 10, CeltFrame.WinTop + 28, Surface);
                }
            }
        }

        // AoWCityJoinDlg.pas:102
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWCityJoinDlg.pas:107
        // { draft }
        public void CloseBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWCityJoinDlg.pas:113
        // { draft }
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWCityJoinDlg
    {
    }
}
