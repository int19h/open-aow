// Generated from AoWHeroJoinDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWHeroJoinDlg
{
    // { THeroJoinDlg: done callback $94, join event $9C, details flag $A0, selection mask, unit list $A4, face unit $A8.
//     (instance size 172, VMT 004382E8) }
    public partial class THeroJoinDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame U1;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame U2;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame U4;
        // { +0050 }
        public Aow.Units.AoWFrame.TAoWFrame U5;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame U3;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame U6;
        // { +005C }
        public Aow.Units.AoWFrame.TAoWFrame U7;
        // { +0060 }
        public Aow.Units.AoWFrame.TAoWFrame U8;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton MiddleBtn;
        // { +0068 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton RightBtn;
        // { +0070 }
        public Aow.Units.AoWButton.TAOWButton LeftBtn;
        // { +0074 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel HeroName;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel HeroLevel;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +0084 }
        public Aow.Units.AoWLabel.TAOWLabel DetailsLbl;
        // { +0088 }
        public Aow.Units.AoWMemo.TAOWMemo JoinMemo;
        // { +008C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0090 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $94 }
        public Aow.Units.AoWE.TJoinEventLog FJoinEvent;
        // { $9C }
        public bool FDetailsOpen;
        // { $A0 }
        public byte FSelection;
        public Aow.Units.FixedArray_byte_2 FReserved;
        // { $A1 }
        public Aow.Units.AoWE.TUnitUserList FUnits;
        // { $A4 }
        public Aow.Units.AoWE.TAbstractUnit FFaceUnit;
        // AoWHeroJoinDlg.pas:68
        // { draft }
        // { $A8 }
        public bool ValidateJoin()
        {
            bool Result = default;
            var call__1 = FJoinEvent.Validate();
            Result = call__1;
            if (!Result)
            {
                Dlg.Visible = false;
            }

            return Result;
        }

        // AoWHeroJoinDlg.pas:74
        public void FinishDialog()
        {
            Aow.Units.EventLog.TEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            FUnits.Clear();
            if (FDetailsOpen)
            {
                FDetailsOpen = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
            }

            Done = FDone;
            Event = FJoinEvent;
            FDone = null;
            FJoinEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWHeroJoinDlg.pas:94
        public void DrawUnit(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TAlignment Alignment = default;
            int X = default;
            int Y = default;
            Aow.Rtl.TRect OldClip = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            var call__2 = FUnits.GetCount();
            if (Index < call__2)
            {
                OldClip = Surface.ClipRect;
                Surface.ClipRect = R;
                X = (R.Right + R.Left) / 2;
                Y = (R.Top + R.Bottom) / 2;
                var call__3 = FUnits.GetUnit(Index);
                var call__4 = call__3.GetAlignment();
                Alignment = call__4;
                if (((int)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    {
                        var w__5 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[unchecked((int)Alignment)];
                        w__5.ShowClipped(X - (w__5.Width / 2), Y - (w__5.Height / 2), Surface);
                    }
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__6 = FUnits.GetUnit(Index);
                call__6.ShowEx(X - 33, Y - 35, Surface, Info);
                if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
                {
                    {
                        var w__7 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                        w__7.ShowClipped(X - (w__7.Width / 2), Y - (w__7.Height / 2), Surface);
                    }
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWHeroJoinDlg.pas:122
        public void HeroJoinDlgCreate()
        {
            var call__8 = Aow.Units.AoWE.TUnitUserList.Create(null);
            FUnits = call__8;
        }

        // AoWHeroJoinDlg.pas:127
        // { draft }
        public void HeroJoinDlgDestroy()
        {
            FUnits.Free();
        }

        // AoWHeroJoinDlg.pas:132
        // { draft }
        public void LeftBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            if (ValidateJoin())
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
                var call__9 = FJoinEvent.Accept(ref Reason);
                if (!call__9)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                }
                else
                {
                    Dlg.Visible = false;
                }
            }
        }

        // AoWHeroJoinDlg.pas:144
        // { draft }
        public void MiddleBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            if (ValidateJoin())
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
                var call__10 = FJoinEvent.Reject(ref Reason);
                if (!call__10)
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                }
                else
                {
                    Dlg.Visible = false;
                }
            }
        }

        // AoWHeroJoinDlg.pas:156
        // { draft }
        public void RightBtnClick()
        {
            if (ValidateJoin())
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
                Dlg.Visible = false;
            }
        }

        // AoWHeroJoinDlg.pas:165
        // { draft }
        public void FaceFramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            if (ValidateJoin())
            {
                R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
                R.Right = FaceFrame.WinWidth + R.Left;
                R.Top = FaceFrame.WinTop + Dlg.WinTop;
                R.Bottom = FaceFrame.WinHeight + R.Top;
                var call__11 = FFaceUnit.GetFace();
                if (call__11 != null)
                {
                    var call__12 = FFaceUnit.GetFace();
                    call__12.ShowStretched(R, Surface);
                }
            }
        }

        // AoWHeroJoinDlg.pas:179
        // { draft }
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            if (ValidateJoin())
            {
                {
                    var w__13 = Sender;
                    R.Left = (w__13.WinLeft + UnitPnl.WinLeft) + 1;
                    R.Right = w__13.WinWidth + R.Left;
                    R.Top = w__13.WinTop + UnitPnl.WinTop;
                    R.Bottom = w__13.WinHeight + R.Top;
                }

                DrawUnit(Sender.Tag, R, ref Surface);
            }
        }

        // AoWHeroJoinDlg.pas:196
        // { draft }
        public void U1MouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried with/local receiver; TObject Sender with component cast; TObject Sender identity; opaque Sender plus Self pointer identity; named opaque Sender local before validation; no exact match. }
            if (ValidateJoin())
            {
                var call__14 = FUnits.GetCount();
                if (Sender.Tag < call__14)
                {
                    var call__15 = FUnits.GetUnit(Sender.Tag);
                    Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__15;
                    if (Button == Aow.Rtl.TMouseButton.mbLeft)
                    {
                        FDetailsOpen = true;
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                        Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                    }
                }
            }
        }

        // AoWHeroJoinDlg.pas:211
        // { draft }
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWHeroJoinDlg.pas:216
        // { draft }
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWHeroJoinDlg
    {
    }
}
