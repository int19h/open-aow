// Translated from AoWPartyJoin.pas.
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

namespace Aow.Units.AoWPartyJoin
{
    // done callback, join event, details flag, selection mask, unit list, face unit
    public partial class TPartyJoinDlg : Aow.Rtl.TDataModule
    {
        public TPartyJoinDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        public Aow.Units.AoWFrame.TAoWFrame U1;
        public Aow.Units.AoWFrame.TAoWFrame U2;
        public Aow.Units.AoWFrame.TAoWFrame U4;
        public Aow.Units.AoWFrame.TAoWFrame U5;
        public Aow.Units.AoWFrame.TAoWFrame U3;
        public Aow.Units.AoWFrame.TAoWFrame U6;
        public Aow.Units.AoWFrame.TAoWFrame U7;
        public Aow.Units.AoWFrame.TAoWFrame U8;
        public Aow.Units.AoWButton.TAOWButton MiddleBtn;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWButton.TAOWButton RightBtn;
        public Aow.Units.AoWButton.TAOWButton LeftBtn;
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWFrame.TAoWFrame UnitLine1;
        public Aow.Units.AoWFrame.TAoWFrame UnitLine2;
        public Aow.Units.AoWMemo.TAOWMemo JoinMemo;
        public Aow.Units.AoWLabel.TAOWLabel Header;
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        public Aow.Units.AoWLabel.TAOWLabel DetailsLbl;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.AoWE.TJoinEventLog FJoinEvent;
        public bool FDetailsOpen;
        public byte FSelection;
        public Aow.Units.AoWE.TUnitUserList FUnits;
        public Aow.Units.AoWE.TAbstractUnit FFaceUnit;
        // AoWPartyJoin.pas:73
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

        // AoWPartyJoin.pas:79
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

        // AoWPartyJoin.pas:99
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
                    var image = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[unchecked((int)Alignment)];
                    image.ShowClipped(X - (image.Width / 2), Y - (image.Height / 2), Surface);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__6 = FUnits.GetUnit(Index);
                call__6.ShowEx(X - 33, Y - 35, Surface, Info);
                if ((((int)(((uint)FSelection) >> Index)) & 1) == 0)
                {
                    var image2 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    image2.ShowClipped(X - (image2.Width / 2), Y - (image2.Height / 2), Surface);
                }

                Surface.ClipRect = OldClip;
            }
        }

        // AoWPartyJoin.pas:127
        public void PartyJoinDlgCreate()
        {
            var call__8 = new Aow.Units.AoWE.TUnitUserList(null);
            FUnits = call__8;
        }

        // AoWPartyJoin.pas:132
        public void PartyJoinDlgDestroy()
        {
            FUnits.Free();
        }

        // AoWPartyJoin.pas:137
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

        // AoWPartyJoin.pas:149
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

        // AoWPartyJoin.pas:161
        public void RightBtnClick()
        {
            if (ValidateJoin())
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
                Dlg.Visible = false;
            }
        }

        // AoWPartyJoin.pas:170
        public void CloseClick()
        {
            if (ValidateJoin())
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
                Dlg.Visible = false;
            }
        }

        // AoWPartyJoin.pas:179
        public void FaceFramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            if (ValidateJoin())
            {
                R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
                R.Right = FaceFrame.WinWidth + R.Left;
                R.Top = FaceFrame.WinTop + Dlg.WinTop;
                R.Bottom = FaceFrame.WinHeight + R.Top;
                if (FFaceUnit != null)
                {
                    var call__11 = FFaceUnit.GetFace();
                    if (call__11 != null)
                    {
                        var call__12 = FFaceUnit.GetFace();
                        call__12.ShowStretched(R, Surface);
                    }
                }
            }
        }

        // AoWPartyJoin.pas:194
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            if (ValidateJoin())
            {
                R.Left = (Sender.WinLeft + UnitPnl.WinLeft) + 1;
                R.Right = Sender.WinWidth + R.Left;
                R.Top = Sender.WinTop + UnitPnl.WinTop;
                R.Bottom = Sender.WinHeight + R.Top;

                DrawUnit(Sender.Tag, R, ref Surface);
            }
        }

        // AoWPartyJoin.pas:211
        public void U1MouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
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

        // AoWPartyJoin.pas:226
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWPartyJoin.pas:231
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWPartyJoin
    {
    }
}
