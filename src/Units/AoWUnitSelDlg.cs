// Translated from AoWUnitSelDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWUnitSelDlg;

// unit selector. retained event log
public partial class TUnitSelectionDlg : TDataModule
{
    public TUnitSelectionDlg(TComponent owner) : base(owner)
    { }

    public TAOWButton SelectBtn;
    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAOWButton CancelBtn;
    public TAOWWindow Dlg;
    public TAOWPanel UnitPnl;
    public TAOWPanel SourcePnl;
    public TAOWLabel SourceNameLbl;
    public TAOWCheckBox U1;
    public TAOWCheckBox U2;
    public TAOWCheckBox U3;
    public TAOWCheckBox U4;
    public TAOWCheckBox U5;
    public TAOWCheckBox U6;
    public TAOWCheckBox U7;
    public TAOWCheckBox U8;
    public TAOWImage DecoL;
    public TAOWImage DecoR;
    public TAOWImage SourceFace;
    public TAoWFrame AoWFrame1;
    public TAOWPanel SelectionInfoPnl;
    public TAOWLabel SelectionInfoLbl;
    public TAOWLabel SelectionLbl;
    public TIvTranslator Translator;
    public TUnitSelector FEvent;
    public TUnitSelectorAAI FEventLog;

    // AoWUnitSelDlg.pas:66
    public void DrawUnit(int Index, int X, int Y, ref TSurfaceDesc SD)
    {
        TRect SaveRect;
        TP2UnitShowInfo Info;
        ImageLib.TLibraryImage Img;
        TAlignment Alignment;
        if (Index < FEvent.FUnits.GetCount())
        {
            SaveRect = SD.ClipRect;
            var rect = Rect(X + 5, Y + 4, X + 46, Y + 56);
            SD.ClipRect = rect;
            Alignment = FEvent.FUnits.GetUnit(Index).GetAlignment();
            if ((int)Alignment < aowIntGfxModUnit.IntGfxMod.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                Img = aowIntGfxModUnit.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X + 25 - Img.Width / 2, Y + 30 - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FEvent.FUnits.GetUnit(Index).ShowEx(X - 8, Y - 5, SD, Info);
            if ((((int)(((uint)FEvent.FSelectable) >> Index)) & 1) == 0)
            {
                Img = aowIntGfxModUnit.IntGfxMod.UnitBGs[7];
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
    public void Execute(TUnitSelectorAAI Request)
    {
        TAbstractUnit U;
        ResetSelection();
        FEventLog = Request;
        FEventLog.AddRef();
        FEvent = Request.FUnitSelector;
        FEvent.AddRef();
        U = AoWHSMap.FUnitControl.FindUnit(FEvent.FSourceUnitID);
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
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWUnitSelDlg.pas:107
    public void SelectBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (FEvent.CanSelect(ref Reason))
        {
            FEvent.Select();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.ShowWarning(Reason);
        }
    }

    // AoWUnitSelDlg.pas:120
    public void U1Change(TObject Sender)
    {
        string Reason = "";
        if (((TAOWCheckBox)Sender).Checked)
        {
            if (!FEvent.FCanceled)
            {
                FEvent.SetSelection((byte)(1 << ((TComponent)Sender).Tag));
            }
            else
            {
                FEvent.SetSelection((byte)(FEvent.FSelection | (1 << ((TComponent)Sender).Tag)));
            }
        }
        else
        {
            FEvent.SetSelection((byte)(FEvent.FSelection & ~(1 << ((TComponent)Sender).Tag)));
        }

        U1.Checked = (FEvent.FSelection & 1) != 0;
        U2.Checked = (FEvent.FSelection & 2) != 0;
        U3.Checked = (FEvent.FSelection & 4) != 0;
        U4.Checked = (FEvent.FSelection & 8) != 0;
        U5.Checked = (FEvent.FSelection & 16) != 0;
        U6.Checked = (FEvent.FSelection & 32) != 0;
        U7.Checked = (FEvent.FSelection & 64) != 0;
        U8.Checked = (FEvent.FSelection & 128) != 0;
        if ((FEvent.FSelectable & (1 << ((TComponent)Sender).Tag)) != 0)
        {
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
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
    public void U1PreDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        DrawUnit(Sender.Tag, UnitPnl.WinLeft + Sender.WinLeft, UnitPnl.WinTop + Sender.WinTop, ref SD);
    }

    // AoWUnitSelDlg.pas:151
    public void SourceFacePreDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        TRect R = default;
        TAbstractUnit U;
        if (FEvent != null)
        {
            U = AoWHSMap.FUnitControl.FindUnit(FEvent.FSourceUnitID);
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

public static partial class AoWUnitSelDlgUnit
{
}
