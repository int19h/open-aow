// Translated from AoWArmyInfoDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// unit-info window open. completion callback. army-info event
public partial class ArmyInfoDialog : DataModule
{
    public ArmyInfoDialog(Component owner) : base(owner)
    { }

    public Panel UnitPnl;
    public Frame U1;
    public Frame U2;
    public Frame U4;
    public Frame U5;
    public Frame U3;
    public Frame U6;
    public Frame U7;
    public Frame U8;
    public Button CloseBtn;
    public Image TitleIcon;
    public Button Close;
    public Label Title;
    public Frame BCeltL;
    public Window Dlg;
    public Frame UnitLine1;
    public Frame UnitLine2;
    public Memo Message;
    public Image FaceFrame;
    public Label DetailsLbl;
    public Label HeaderLbl;
    public IvTranslator Translator;
    public bool FUnitInfoShown;
    public ExecuteDoneEvent FOnClose;
    public ArmyInfoEventLog FEvent;

    // AoWArmyInfoDlg.pas:63
    public void DrawUnit(int Index, in Rect R, ref LockedSurface SD)
    {
        Rect SaveRect;
        UnitShowInfo Info;
        LibraryImage Img;
        Units.Alignment Alignment;
        int X;
        int Y;
        if (Index < FEvent.FArmyView.GetCount())
        {
            SaveRect = SD.ClipRect;
            SD.ClipRect = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            Alignment = FEvent.FArmyView.GetUnit(Index).GetAlignment();
            if ((int)Alignment < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                Img = DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            FEvent.FArmyView.GetUnit(Index).ShowEx(X - 33, Y - 35, SD, Info);
            if ((((int)(((uint)FEvent.FSelection) >> Index)) & 1) == 0)
            {
                Img = DialogHosts.IntGfxMod.UnitBGs[7];
                Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
            }

            SD.ClipRect = SaveRect;
        }
    }

    // AoWArmyInfoDlg.pas:92
    public void ClearEvent()
    {
        ArmyInfoEventLog Event;
        ExecuteDoneEvent Notify;
        if (FUnitInfoShown)
        {
            FUnitInfoShown = false;
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWin.MainWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
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
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:115
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:121
    public void U1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        var count = FEvent.FArmyView.GetCount();
        if (Slot < count)
        {
            var unit = FEvent.FArmyView.GetUnit(Slot);
            DialogHosts.General.FSelectedUnit = unit;
            if (Button == MouseButton.Left)
            {
                FUnitInfoShown = true;
                DialogHosts.UnitWindow.ShowUnit();
                DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(Frame Sender, LockedSurface SD, int Slot)
    {
        Rect R = default;
        R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = Sender.WinWidth + R.Left;
        R.Top = Sender.WinTop + UnitPnl.WinTop;
        R.Bottom = Sender.WinHeight + R.Top;
        DrawUnit(Slot, R, ref SD);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(LockedSurface SD)
    {
        Rect R = default;
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
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowArmyInfo(ArmyInfoDialog Dialog, ArmyInfoEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        var text = Event.FMessages.Text;
        Dialog.Message.Items.Text = text;
        Dialog.Title.Text = Event.FTitle;
        Dialog.HeaderLbl.Text = Event.FDescription;
        Dialog.Dlg.Visible = true;
        Dialog.FUnitInfoShown = false;
    }

}
