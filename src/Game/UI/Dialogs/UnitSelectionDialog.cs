// Translated from AoWUnitSelDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// unit selector. retained event log
public partial class UnitSelectionDialog : DataModule
{
    public UnitSelectionDialog(Component owner) : base(owner)
    { }

    public Button SelectBtn;
    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Button CancelBtn;
    public Window Dlg;
    public Panel UnitPnl;
    public Panel SourcePnl;
    public Label SourceNameLbl;
    public CheckBox U1;
    public CheckBox U2;
    public CheckBox U3;
    public CheckBox U4;
    public CheckBox U5;
    public CheckBox U6;
    public CheckBox U7;
    public CheckBox U8;
    public Image DecoL;
    public Image DecoR;
    public Image SourceFace;
    public Frame AoWFrame1;
    public Panel SelectionInfoPnl;
    public Label SelectionInfoLbl;
    public Label SelectionLbl;
    public IvTranslator Translator;
    public UnitSelector FEvent;
    public UnitSelectionActivation FEventLog;

    // AoWUnitSelDlg.pas:66
    public void DrawUnit(int Index, int X, int Y, ref LockedSurface SD)
    {
        Rect SaveRect;
        UnitShowInfo Info;
        LibraryImage Img;
        Units.Alignment Alignment;
        if (Index < FEvent.FUnits.GetCount())
        {
            SaveRect = SD.ClipRect;
            var rect = new Rect(X + 5, Y + 4, X + 46, Y + 56);
            SD.ClipRect = rect;
            Alignment = FEvent.FUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                Img = DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X + 25 - Img.Width / 2, Y + 30 - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FEvent.FUnits.GetUnit(Index).ShowEx(X - 8, Y - 5, SD, Info);
            if ((((int)(((uint)FEvent.FSelectable) >> Index)) & 1) == 0)
            {
                Img = DialogHosts.IntGfxMod.UnitBGs[7];
                Img.ShowClipped(X + 25 - Img.Width / 2, Y + 30 - Img.Height / 2, SD);
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
        SelectionInfoLbl.Text = "";
    }

    // AoWUnitSelDlg.pas:186
    public void Execute(UnitSelectionActivation Request)
    {
        Troop U;
        ResetSelection();
        FEventLog = Request;
        FEventLog.AddRef();
        FEvent = Request.FUnitSelector;
        FEvent.AddRef();
        U = AoWHSMap.Current.FUnitControl.FindUnit(FEvent.FSourceUnitID);
        if (U != null)
        {
            SourcePnl.Visible = true;
            var name = U.GetName();
            SourceNameLbl.Text = name;
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
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWUnitSelDlg.pas:107
    public void SelectBtnClick()
    {
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (FEvent.CanSelect(ref Reason))
        {
            FEvent.Select();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowWarning(Reason);
        }
    }

    // AoWUnitSelDlg.pas:120
    public void U1Change(CheckBox Sender, int Slot)
    {
        string Reason = "";
        if (Sender.Checked)
        {
            if (!FEvent.FCanceled)
            {
                FEvent.SetSelection((byte)(1 << Slot));
            }
            else
            {
                FEvent.SetSelection((byte)(FEvent.FSelection | (1 << Slot)));
            }
        }
        else
        {
            FEvent.SetSelection((byte)(FEvent.FSelection & ~(1 << Slot)));
        }

        U1.Checked = (FEvent.FSelection & 1) != 0;
        U2.Checked = (FEvent.FSelection & 2) != 0;
        U3.Checked = (FEvent.FSelection & 4) != 0;
        U4.Checked = (FEvent.FSelection & 8) != 0;
        U5.Checked = (FEvent.FSelection & 16) != 0;
        U6.Checked = (FEvent.FSelection & 32) != 0;
        U7.Checked = (FEvent.FSelection & 64) != 0;
        U8.Checked = (FEvent.FSelection & 128) != 0;
        if ((FEvent.FSelectable & (1 << Slot)) != 0)
        {
            if (Sender.Checked)
            {
                DialogHosts.General.PlayButtonSound(1);
            }
            else
            {
                DialogHosts.General.PlayButtonSound(2);
            }
        }

        if (FEvent.FSelection != 0)
        {
            if (FEvent.CanSelect(ref Reason))
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
            SelectionInfoLbl.Text = "";
        }
    }

    // AoWUnitSelDlg.pas:146
    public void U1PreDraw(CheckBox Sender, LockedSurface SD, int Slot)
    {
        DrawUnit(Slot, UnitPnl.WinLeft + Sender.WinLeft, UnitPnl.WinTop + Sender.WinTop, ref SD);
    }

    // AoWUnitSelDlg.pas:151
    public void SourceFacePreDraw(LockedSurface SD)
    {
        Rect R = default;
        Troop U;
        if (FEvent != null)
        {
            U = AoWHSMap.Current.FUnitControl.FindUnit(FEvent.FSourceUnitID);
            if (U != null)
            {
                R.Left = this.SourceFace.WinLeft + SourcePnl.WinLeft;
                R.Right = this.SourceFace.WinWidth + R.Left;
                R.Top = this.SourceFace.WinTop + SourcePnl.WinTop;
                R.Bottom = this.SourceFace.WinHeight + R.Top;

                if (U.GetFace() != null)
                {
                    U.GetFace().ShowStretched(R, SD);
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
