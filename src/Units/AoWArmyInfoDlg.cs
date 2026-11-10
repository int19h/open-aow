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
public partial class ArmyInfoDlg : DataModule
{
    public ArmyInfoDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWPanel UnitPnl;
    public AoWFrame U1;
    public AoWFrame U2;
    public AoWFrame U4;
    public AoWFrame U5;
    public AoWFrame U3;
    public AoWFrame U6;
    public AoWFrame U7;
    public AoWFrame U8;
    public AOWButton CloseBtn;
    public AOWImage TitleIcon;
    public AOWButton Close;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWWindow Dlg;
    public AoWFrame UnitLine1;
    public AoWFrame UnitLine2;
    public AOWMemo Message;
    public AOWImage FaceFrame;
    public AOWLabel DetailsLbl;
    public AOWLabel HeaderLbl;
    public IvTranslator Translator;
    public bool FUnitInfoShown;
    public ExecuteDoneEvent FOnClose;
    public ArmyInfoEventLog FEvent;

    // AoWArmyInfoDlg.pas:63
    public void DrawUnit(int Index, in global::Aow.Rtl.Rect R, ref SurfaceDesc SD)
    {
        global::Aow.Rtl.Rect SaveRect;
        P2UnitShowInfo Info;
        Aow.Game.Draw.Images.LibraryImage Img;
        Alignment Alignment;
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
            Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
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
        ArmyInfoEventLog Event;
        ExecuteDoneEvent Notify;
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
    public void U1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        var count = FEvent.FArmyView.GetCount();
        if (Slot < count)
        {
            var unit = FEvent.FArmyView.GetUnit(Slot);
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
            if (Button == MouseButton.Left)
            {
                FUnitInfoShown = true;
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(AoWFrame Sender, SurfaceDesc SD, int Slot)
    {
        global::Aow.Rtl.Rect R = default;
        R.Left = Sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = Sender.WinWidth + R.Left;
        R.Top = Sender.WinTop + UnitPnl.WinTop;
        R.Bottom = Sender.WinHeight + R.Top;
        DrawUnit(Slot, R, ref SD);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(SurfaceDesc SD)
    {
        global::Aow.Rtl.Rect R = default;
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

    internal static void ShowArmyInfo(ArmyInfoDlg Dialog, ArmyInfoEventLog Event, ExecuteDoneEvent Done)
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
