// Translated from AoWUnitSelDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
// Translated from AoWUnitSelDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// unit selector. retained event log
public partial class UnitSelectionDlg : DataModule
{
    public UnitSelectionDlg(Component owner) : base(owner)
    { }

    public AOWButton SelectBtn;
    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AOWButton CancelBtn;
    public AOWWindow Dlg;
    public AOWPanel UnitPnl;
    public AOWPanel SourcePnl;
    public AOWLabel SourceNameLbl;
    public AOWCheckBox U1;
    public AOWCheckBox U2;
    public AOWCheckBox U3;
    public AOWCheckBox U4;
    public AOWCheckBox U5;
    public AOWCheckBox U6;
    public AOWCheckBox U7;
    public AOWCheckBox U8;
    public AOWImage DecoL;
    public AOWImage DecoR;
    public AOWImage SourceFace;
    public AoWFrame AoWFrame1;
    public AOWPanel SelectionInfoPnl;
    public AOWLabel SelectionInfoLbl;
    public AOWLabel SelectionLbl;
    public IvTranslator Translator;
    public UnitSelector FEvent;
    public UnitSelectorAAI FEventLog;

    // AoWUnitSelDlg.pas:66
    public void DrawUnit(int Index, int X, int Y, ref SurfaceDesc SD)
    {
        Rect SaveRect;
        P2UnitShowInfo Info;
        LibraryImage Img;
        Alignment Alignment;
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
    public void Execute(UnitSelectorAAI Request)
    {
        AbstractUnit U;
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
    public void U1Change(AOWCheckBox Sender, int Slot)
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
    public void U1PreDraw(AOWCheckBox Sender, SurfaceDesc SD, int Slot)
    {
        DrawUnit(Slot, UnitPnl.WinLeft + Sender.WinLeft, UnitPnl.WinTop + Sender.WinTop, ref SD);
    }

    // AoWUnitSelDlg.pas:151
    public void SourceFacePreDraw(SurfaceDesc SD)
    {
        Rect R = default;
        AbstractUnit U;
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
