// Translated from AoWArmyInfoDlg.pas.
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
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.WizardsTower;
using static Aow.Units.WizardsTower.WizardsTower;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
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
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWArmyInfoDlg
{
    // { TArmyInfoDlg (instance size $AC, VMT 004475A0).
//     $9C unit-info window open; $A0 completion callback; $A8 army-info event. }
    public partial class TArmyInfoDlg : Aow.Rtl.TDataModule
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
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        // { +0068 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0074 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0078 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +007C }
        public Aow.Units.AoWFrame.TAoWFrame UnitLine1;
        // { +0080 }
        public Aow.Units.AoWFrame.TAoWFrame UnitLine2;
        // { +0084 }
        public Aow.Units.AoWMemo.TAOWMemo Message;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel DetailsLbl;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        // { +0094 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0098 }
        public bool FUnitInfoShown;
        // { $9C }
        public Aow.Units.FixedArray_byte_3 F_ArmyInfoDlg_9D;
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        // { $A0 }
        public Aow.Units.AoWE.TArmyInfoEventLog FEvent;
        // AoWArmyInfoDlg.pas:63
        // { $A8 }
        public void DrawUnit(int Index, in Aow.Rtl.TRect R, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect SaveRect = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Units.ImageLib.TLibraryImage Img = default;
            Aow.Units.AoWE.TAlignment Alignment = default;
            int X = default;
            int Y = default;
            var call__1 = FEvent.FArmyView.GetCount();
            if (Index < call__1)
            {
                SaveRect = SD.ClipRect;
                SD.ClipRect = R;
                X = (R.Right + R.Left) / 2;
                Y = (R.Top + R.Bottom) / 2;
                var call__2 = FEvent.FArmyView.GetUnit(Index);
                var call__3 = call__2.GetAlignment();
                Alignment = call__3;
                if (((int)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    // { D3 masks the enum index after the original full-byte guard. }
                    var call__4 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)Alignment];
                    Img = call__4;
                    Img.ShowClipped(X - (Img.Width / 2), Y - (Img.Height / 2), SD);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__5 = FEvent.FArmyView.GetUnit(Index);
                call__5.ShowEx(X - 33, Y - 35, SD, Info);
                if ((((int)(((uint)FEvent.FSelection) >> Index)) & 1) == 0)
                {
                    var call__6 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    Img = call__6;
                    Img.ShowClipped(X - (Img.Width / 2), Y - (Img.Height / 2), SD);
                }

                SD.ClipRect = SaveRect;
            }
        }

        // AoWArmyInfoDlg.pas:92
        public void ClearEvent()
        {
            Aow.Units.AoWE.TArmyInfoEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Notify = default;
            if (FUnitInfoShown)
            {
                FUnitInfoShown = false;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Priority;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
            }

            Notify = FOnClose;
            Event = FEvent;
            FOnClose = null;
            FEvent = null;
            if (Notify != null)
            {
                Notify(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWArmyInfoDlg.pas:109
        public void CloseBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWArmyInfoDlg.pas:115
        // { draft }
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWArmyInfoDlg.pas:121
        // { draft }
        public void U1MouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            ;
            var call__7 = FEvent.FArmyView.GetCount();
            if (((Aow.Rtl.TComponent)Sender).Tag < call__7)
            {
                var call__8 = FEvent.FArmyView.GetUnit(((Aow.Rtl.TComponent)Sender).Tag);
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__8;
                if (Button == Aow.Rtl.TMouseButton.mbLeft)
                {
                    FUnitInfoShown = true;
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                    Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
                }
            }
        }

        // AoWArmyInfoDlg.pas:134
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            {
                var w__9 = (Aow.Units.AoWImage.TAOWImage)Sender;
                R.Left = (w__9.WinLeft + UnitPnl.WinLeft) + 1;
                R.Right = w__9.WinWidth + R.Left;
                R.Top = w__9.WinTop + UnitPnl.WinTop;
                R.Bottom = w__9.WinHeight + R.Top;
                DrawUnit(Sender.Tag, R, ref SD);
            }
        }

        // AoWArmyInfoDlg.pas:146
        public void FaceFramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            int I = default;
            ;
            {
                var w__10 = FaceFrame;
                R.Left = w__10.WinLeft + Dlg.WinLeft;
                R.Right = w__10.WinWidth + R.Left;
                R.Top = w__10.WinTop + Dlg.WinTop;
                R.Bottom = w__10.WinHeight + R.Top;
            }

            I = 0;
            while (true)
            {
                bool bool__11 = ((FEvent.FSelection & (1 << I)) == 0);
                if (bool__11)
                {
                    var call__12 = FEvent.FArmyView.GetCount();
                    bool__11 = (I != call__12);
                }

                if (!bool__11)
                    break;
                {
                    I += 1;
                }
            }

            var call__13 = FEvent.FArmyView.GetCount();
            if (I < call__13)
            {
                var call__14 = FEvent.FArmyView.GetUnit(I);
                call__14.ShowFace(R, SD);
            }
        }

        // AoWArmyInfoDlg.pas:161
        public void DlgHide()
        {
            ClearEvent();
        }

        // AoWArmyInfoDlg.pas:166
        // { draft }
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWArmyInfoDlg
    {
    }
}
