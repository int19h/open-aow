// Generated from AoWArenaDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.Arena;
using static Aow.Units.Arena.Arena;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
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
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWArenaDlg
{
    // { TArenaDlg (instance size $B4, VMT 0044BDA8).
//     $A8 arena event; $AC completion callback. }
    public partial class TArenaDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWButton.TAOWButton SelectBtn;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0054 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0058 }
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel MapViewPnl;
        // { +0060 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U1;
        // { +0064 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U2;
        // { +0068 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U3;
        // { +006C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U4;
        // { +0070 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U5;
        // { +0074 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U6;
        // { +0078 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U7;
        // { +007C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U8;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage DecoL;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage DecoR;
        // { +0088 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +008C }
        public Aow.Units.AoWPanel.TAOWPanel SelectionInfoPnl;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel SelectionInfoLbl;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel SelectionLbl;
        // { +0098 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox MapView;
        // { +009C }
        public Aow.Units.AoWImage.TAOWImage InfoSep;
        // { +00A0 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00A4 }
        public Aow.Units.Arena.TEnterArenaEventLog FEvent;
        // { $A8 }
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        // AoWArenaDlg.pas:104
        // { $AC }
        public void DrawUnit(int Index, int X, int Y, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect SaveRect = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Units.ImageLib.TLibraryImage Img = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Units.AoWE.TAlignment Alignment = default;
            var call__1 = FEvent.GetCount();
            if (Index < call__1)
            {
                var call__2 = FEvent.GetUnit(Index);
                U = call__2;
                SaveRect = SD.ClipRect;
                var call__3 = Aow.Rtl.Rtl.Rect(X + 5, Y + 4, X + 46, Y + 56);
                SD.ClipRect = call__3;
                Alignment = U.GetAlignment();
                if (unchecked((byte)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    var call__4 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[unchecked((int)Alignment)];
                    Img = call__4;
                    Img.ShowClipped((X + 25) - (Img.Width / 2), (Y + 30) - (Img.Height / 2), SD);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                U.ShowEx(X - 8, Y - 5, SD, Info);
                if ((((int)(((uint)FEvent.FTrainable) >> Index)) & 1) == 0)
                {
                    var call__5 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    Img = call__5;
                    Img.ShowClipped((X + 25) - (Img.Width / 2), (Y + 30) - (Img.Height / 2), SD);
                }

                SD.ClipRect = SaveRect;
                if ((((int)(((uint)FEvent.FTrainable) >> Index)) & 1) != 0)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 0;
                    var call__6 = FEvent.FArena.UnitTrainCost(U);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(X + 25, Y + 62, Aow.Rtl.Rtl.IntToStr(call__6), SD);
                }
            }
        }

        // AoWArenaDlg.pas:140
        public void ClearEvent()
        {
            Aow.Units.Arena.TEnterArenaEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Notify = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
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

        // AoWArenaDlg.pas:152
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWArenaDlg.pas:158
        // { draft }
        public void SelectBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__7 = FEvent.Train(ref Reason);
            if (call__7)
            {
                Dlg.Visible = false;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
            }
        }

        // AoWArenaDlg.pas:170
        // { draft }
        public void U1Change(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                FEvent.SetSelection(unchecked((byte)(FEvent.FSelection | (1 << ((Aow.Rtl.TComponent)Sender).Tag))));
            }
            else
            {
                FEvent.SetSelection(unchecked((byte)(FEvent.FSelection & ~(1 << ((Aow.Rtl.TComponent)Sender).Tag))));
            }

            U1.Checked = (FEvent.FSelection & 1) != 0;
            U2.Checked = (FEvent.FSelection & 2) != 0;
            U3.Checked = (FEvent.FSelection & 4) != 0;
            U4.Checked = (FEvent.FSelection & 8) != 0;
            U5.Checked = (FEvent.FSelection & 16) != 0;
            U6.Checked = (FEvent.FSelection & 32) != 0;
            U7.Checked = (FEvent.FSelection & 64) != 0;
            U8.Checked = (FEvent.FSelection & 128) != 0;
            if ((FEvent.FTrainable & (1 << ((Aow.Rtl.TComponent)Sender).Tag)) != 0)
            {
                if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
                }
                else
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
                }
            }

            if (FEvent.FSelection != 0)
            {
                var call__8 = FEvent.CanTrain(ref Reason);
                if (call__8)
                {
                    SelectionInfoLbl.TextColor = 0;
                }
                else
                {
                    SelectionInfoLbl.TextColor = 149;
                }

                var call__9 = FEvent.GetSelectionInfo();
                SelectionInfoLbl.Text = call__9;
            }
            else
            {
                SelectionInfoLbl.Text = (AnsiString)"";
            }
        }

        // AoWArenaDlg.pas:196
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            {
                var w__10 = (Aow.Units.AoWImage.TAOWImage)Sender;
                DrawUnit(Sender.Tag, UnitPnl.WinLeft + w__10.WinLeft, UnitPnl.WinTop + w__10.WinTop, ref SD);
            }
        }

        // AoWArenaDlg.pas:201
        public void DlgHide()
        {
            ClearEvent();
        }

        // AoWArenaDlg.pas:206
        // { draft }
        public void MapViewDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            Aow.Units.GFXE.TSurfaceDesc MapSD = default;
            Aow.Rtl.TRect R = default;
            int W = default;
            int H = default;
            var call__11 = Aow.Rtl.Rtl.Rect(Dlg.WinLeft + ((Aow.Units.AoWImage.TAOWImage)Sender).WinLeft, Dlg.WinTop + ((Aow.Units.AoWImage.TAOWImage)Sender).WinTop, (Dlg.WinLeft + ((Aow.Units.AoWImage.TAOWImage)Sender).WinLeft) + ((Aow.Units.AoWImage.TAOWImage)Sender).WinWidth, (Dlg.WinTop + ((Aow.Units.AoWImage.TAOWImage)Sender).WinTop) + ((Aow.Units.AoWImage.TAOWImage)Sender).WinHeight);
            R = call__11;
            W = R.Right - R.Left;
            H = R.Bottom - R.Top;
            var call__12 = FEvent.FArena.GetYhx();
            var call__13 = FEvent.FArena.GetXhx();
            Aow.Units.HSEngine.HSEngine.HXtoHP(call__13, call__12, ref X, ref Y);
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

            Aow.Units.GFXE.GFXE.CreateSDfromSD(SD, R, ref MapSD);
            var call__14 = FEvent.FArena.GetLhx();
            Aow.Units.AoWE.AoWE.AoWHSMap.SetViewPort(X, Y, call__14, R.Right - R.Left, R.Bottom - R.Top);
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateStaticScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateDynamicScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawStaticScene(ref MapSD);
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawDynamicScene(ref MapSD);
        }

        // AoWArenaDlg.pas:224
        public void MapViewUpdate()
        {
            MapView.FDrawn = MapView.FDrawn && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 1) != 0);
        }

        // AoWArenaDlg.pas:229
        // { draft }
        public void DlgDrawBack()
        {
        }

        // AoWArenaDlg.pas:232
        // { draft }
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWArenaDlg
    {
        // AoWArenaDlg.pas:90
        public static void ShowArena(Aow.Units.AoWArenaDlg.TArenaDlg Dialog, Aow.Units.Arena.TEnterArenaEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            {
                var w__15 = Dialog;
                Aow.Rtl.Rtl.Assert(w__15.FEvent == null, (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoW\\AoWArenaDlg.pas", 94);
                Aow.Units.AoWArenaDlg.AoWArenaDlg.ResetArena(Dialog);
                w__15.FOnClose = Done;
                w__15.FEvent = Event;
                w__15.FEvent.AddRef();
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
                w__15.Dlg.Visible = true;
            }
        }

        // AoWArenaDlg.pas:238
        public static void ResetArena(Aow.Units.AoWArenaDlg.TArenaDlg Dialog)
        {
            {
                var w__16 = Dialog;
                w__16.U1.Checked = false;
                w__16.U2.Checked = false;
                w__16.U3.Checked = false;
                w__16.U4.Checked = false;
                w__16.U5.Checked = false;
                w__16.U6.Checked = false;
                w__16.U7.Checked = false;
                w__16.U8.Checked = false;
                w__16.SelectionInfoLbl.Text = (AnsiString)"";
            }
        }
    }
}
