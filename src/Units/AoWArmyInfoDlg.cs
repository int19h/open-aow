// Translated from AoWArmyInfoDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AOWControlWin;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWUnitWindow;
using Aow.Units.AoWWindow;
using Aow.Units.EventLog;
using Aow.Units.GFXE;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWArmyInfoDlg;

// unit-info window open. completion callback. army-info event
public partial class TArmyInfoDlg : TDataModule
{
    public TArmyInfoDlg(TComponent owner) : base(owner)
    { }

    public TAOWPanel UnitPnl;
    public TAoWFrame U1;
    public TAoWFrame U2;
    public TAoWFrame U4;
    public TAoWFrame U5;
    public TAoWFrame U3;
    public TAoWFrame U6;
    public TAoWFrame U7;
    public TAoWFrame U8;
    public TAOWButton CloseBtn;
    public TAOWImage TitleIcon;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWWindow Dlg;
    public TAoWFrame UnitLine1;
    public TAoWFrame UnitLine2;
    public TAOWMemo Message;
    public TAOWImage FaceFrame;
    public TAOWLabel DetailsLbl;
    public TAOWLabel HeaderLbl;
    public TIvTranslator Translator;
    public bool FUnitInfoShown;
    public TExecuteDoneEvent FOnClose;
    public TArmyInfoEventLog FEvent;

    // AoWArmyInfoDlg.pas:63
    public void DrawUnit(int Index, in TRect R, ref TSurfaceDesc SD)
    {
        TRect SaveRect;
        TP2UnitShowInfo Info;
        ImageLib.TLibraryImage Img;
        TAlignment Alignment;
        int X;
        int Y;
        if (Index < FEvent.FArmyView.GetCount())
        {
            SaveRect = SD.ClipRect;
            SD.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FEvent.FArmyView.GetUnit(Index).GetAlignment();
            if ((int)Alignment < aowIntGfxModUnit.IntGfxMod.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                Img = aowIntGfxModUnit.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FEvent.FArmyView.GetUnit(Index).ShowEx(X - 33, Y - 35, SD, Info);
            if ((((int)(((uint)FEvent.FSelection) >> Index)) & 1) == 0)
            {
                Img = aowIntGfxModUnit.IntGfxMod.UnitBGs[7];
                Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
            }

            SD.ClipRect = SaveRect;
        }
    }

    // AoWArmyInfoDlg.pas:92
    public void ClearEvent()
    {
        TArmyInfoEventLog Event;
        TExecuteDoneEvent Notify;
        if (FUnitInfoShown)
        {
            FUnitInfoShown = false;
            AoWUnitWindowUnit.UnitWindow.UnitWin.Priority = AOWControlWinUnit.ControlWin.ControlWindow.Priority;
            AoWUnitWindowUnit.UnitWindow.UnitWin.Visible = false;
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
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:115
    public void CloseClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:121
    public void U1MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        var count = FEvent.FArmyView.GetCount();
        if (((TComponent)Sender).Tag < count)
        {
            var unit = FEvent.FArmyView.GetUnit(((TComponent)Sender).Tag);
            General.FSelectedUnit = unit;
            if (Button == TMouseButton.mbLeft)
            {
                FUnitInfoShown = true;
                AoWUnitWindowUnit.UnitWindow.ShowUnit();
                AoWUnitWindowUnit.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        TRect R = default;
        var image = (TAOWImage)Sender;
        R.Left = image.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = image.WinWidth + R.Left;
        R.Top = image.WinTop + UnitPnl.WinTop;
        R.Bottom = image.WinHeight + R.Top;
        DrawUnit(Sender.Tag, R, ref SD);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        TRect R = default;
        int I;
        R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        I = 0;
        while ((FEvent.FSelection & (1 << I)) == 0 && I != FEvent.FArmyView.GetCount())
        {
            I += 1;
        }

        if (I < FEvent.FArmyView.GetCount())
        {
            FEvent.FArmyView.GetUnit(I).ShowFace(R, SD);
        }
    }

    // AoWArmyInfoDlg.pas:161
    public void DlgHide()
    {
        ClearEvent();
    }

    // AoWArmyInfoDlg.pas:166
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWArmyInfoDlgUnit
{
}
