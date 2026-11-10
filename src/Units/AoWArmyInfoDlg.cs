// Translated from AoWArmyInfoDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWArmyInfoDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
        Aow.Game.Draw.Images.TLibraryImage Img;
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
            if ((int)Alignment < Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                Img = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[(int)Alignment];
                Img.ShowClipped(X - Img.Width / 2, Y - Img.Height / 2, SD);
            }

            Info = default;
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoHitPoints, TUnitOverlayModesElement.uoRank);
            FEvent.FArmyView.GetUnit(Index).ShowEx(X - 33, Y - 35, SD, Info);
            if ((((int)(((uint)FEvent.FSelection) >> Index)) & 1) == 0)
            {
                Img = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitBGs[7];
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
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Priority;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = false;
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:115
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWArmyInfoDlg.pas:121
    public void U1MouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y, int Slot)
    {
        var count = FEvent.FArmyView.GetCount();
        if (Slot < count)
        {
            var unit = FEvent.FArmyView.GetUnit(Slot);
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
            if (Button == TMouseButton.mbLeft)
            {
                FUnitInfoShown = true;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(TAoWFrame Sender, TSurfaceDesc SD, int Slot)
    {
        TRect R = default;
        R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = Sender.WinWidth + R.Left;
        R.Top = Sender.WinTop + UnitPnl.WinTop;
        R.Bottom = Sender.WinHeight + R.Top;
        DrawUnit(Slot, R, ref SD);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(TSurfaceDesc SD)
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowArmyInfo(TArmyInfoDlg Dialog, TArmyInfoEventLog Event, TExecuteDoneEvent Done)
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
