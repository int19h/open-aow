// Translated from AoWUnitSelDlg.pas.
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
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWUnitSelDlg
{
    // { TUnitSelectionDlg (instance size $B0, VMT 004449D4).
//     $A8 unit selector; $AC retained event log. }
    public partial class TUnitSelectionDlg : Aow.Rtl.TDataModule
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
        public Aow.Units.AoWPanel.TAOWPanel SourcePnl;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel SourceNameLbl;
        // { +0064 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U1;
        // { +0068 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U2;
        // { +006C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U3;
        // { +0070 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U4;
        // { +0074 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U5;
        // { +0078 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U6;
        // { +007C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U7;
        // { +0080 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox U8;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage DecoL;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage DecoR;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage SourceFace;
        // { +0090 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel SelectionInfoPnl;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel SelectionInfoLbl;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel SelectionLbl;
        // { +00A0 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00A4 }
        public Aow.Units.AoWE.TUnitSelector FEvent;
        // { $A8 }
        public Aow.Units.AoWE.TUnitSelectorAAI FEventLog;
        // AoWUnitSelDlg.pas:66
        // { draft }
        // { $AC }
        public void DrawUnit(int Index, int X, int Y, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect SaveRect = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Units.ImageLib.TLibraryImage Img = default;
            Aow.Units.AoWE.TAlignment Alignment = default;
            var call__1 = FEvent.FUnits.GetCount();
            if (Index < call__1)
            {
                SaveRect = SD.ClipRect;
                var call__2 = Aow.Rtl.Rtl.Rect(X + 5, Y + 4, X + 46, Y + 56);
                SD.ClipRect = call__2;
                var call__3 = FEvent.FUnits.GetUnit(Index);
                var call__4 = call__3.GetAlignment();
                Alignment = call__4;
                if (((int)Alignment) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    // { D3 masks the enum index after the original full-byte guard. }
                    var call__5 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)Alignment];
                    Img = call__5;
                    Img.ShowClipped((X + 25) - (Img.Width / 2), (Y + 30) - (Img.Height / 2), SD);
                }

                Info = default;
                Info.Direction = 3;
                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoHitPoints, Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                var call__6 = FEvent.FUnits.GetUnit(Index);
                call__6.ShowEx(X - 8, Y - 5, SD, Info);
                if ((((int)(((uint)FEvent.FSelectable) >> Index)) & 1) == 0)
                {
                    var call__7 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[7];
                    Img = call__7;
                    Img.ShowClipped((X + 25) - (Img.Width / 2), (Y + 30) - (Img.Height / 2), SD);
                }

                SD.ClipRect = SaveRect;
            }
        }

        // AoWUnitSelDlg.pas:177
        public void ResetSelection()
        {
            if (FEvent != null)
            {
                FEvent.Release();
                FEvent = null;
            }

            if (FEventLog != null)
            {
                FEventLog.Release();
                FEventLog = null;
            }

            U1.Checked = false;
            U2.Checked = false;
            U3.Checked = false;
            U4.Checked = false;
            U5.Checked = false;
            U6.Checked = false;
            U7.Checked = false;
            U8.Checked = false;
            SelectionInfoLbl.Text = (AnsiString)"";
        }

        // AoWUnitSelDlg.pas:186
        public void Execute(Aow.Units.AoWE.TUnitSelectorAAI Request)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            ResetSelection();
            FEventLog = Request;
            FEventLog.AddRef();
            FEvent = Request.FUnitSelector;
            FEvent.AddRef();
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FEvent.FSourceUnitID);
            U = call__8;
            if (U != null)
            {
                SourcePnl.Visible = true;
                var call__9 = U.GetName();
                SourceNameLbl.Text = call__9;
            }
            else
            {
                SourcePnl.Visible = false;
            }

            SelectionLbl.Text = FEvent.FCaption;
            Dlg.Visible = true;
        }

        // AoWUnitSelDlg.pas:95
        public void ClearEvent()
        {
            if (FEvent != null)
            {
                FEvent.Release();
                FEvent = null;
            }

            if (FEventLog != null)
            {
                FEventLog.Release();
                FEventLog = null;
            }
        }

        // AoWUnitSelDlg.pas:101
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWUnitSelDlg.pas:107
        // { draft }
        public void SelectBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__10 = FEvent.CanSelect(ref Reason);
            if (call__10)
            {
                FEvent.Select();
                Dlg.Visible = false;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
            }
        }

        // AoWUnitSelDlg.pas:120
        // { draft }
        public void U1Change(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                if (!FEvent.FCanceled)
                {
                    FEvent.SetSelection(unchecked((byte)(1 << ((Aow.Rtl.TComponent)Sender).Tag)));
                }
                else
                {
                    FEvent.SetSelection(unchecked((byte)(FEvent.FSelection | (1 << ((Aow.Rtl.TComponent)Sender).Tag))));
                }
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
            if ((FEvent.FSelectable & (1 << ((Aow.Rtl.TComponent)Sender).Tag)) != 0)
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
                var call__11 = FEvent.CanSelect(ref Reason);
                if (call__11)
                {
                    SelectionInfoLbl.TextColor = 0;
                }
                else
                {
                    SelectionInfoLbl.TextColor = 149;
                }
            }
            else
            {
                SelectionInfoLbl.Text = (AnsiString)"";
            }
        }

        // AoWUnitSelDlg.pas:146
        public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            var image = (Aow.Units.AoWImage.TAOWImage)Sender;
            DrawUnit(Sender.Tag, UnitPnl.WinLeft + image.WinLeft, UnitPnl.WinTop + image.WinTop, ref SD);
        }

        // AoWUnitSelDlg.pas:151
        public void SourceFacePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            if (FEvent != null)
            {
                var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FEvent.FSourceUnitID);
                U = call__13;
                if (U != null)
                {
                    R.Left = this.SourceFace.WinLeft + SourcePnl.WinLeft;
                    R.Right = this.SourceFace.WinWidth + R.Left;
                    R.Top = this.SourceFace.WinTop + SourcePnl.WinTop;
                    R.Bottom = this.SourceFace.WinHeight + R.Top;

                    if (U.GetFace() != null)
                    {
                        var call__15 = U.GetFace();
                        call__15.ShowStretched(R, SD);
                    }
                }
            }
        }

        // AoWUnitSelDlg.pas:171
        public void DlgHide()
        {
            ClearEvent();
        }
    }

    public static partial class AoWUnitSelDlg
    {
    }
}
