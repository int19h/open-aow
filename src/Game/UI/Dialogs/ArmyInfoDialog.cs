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
    private bool _unitInfoShown;
    private ExecuteDoneEvent _onClose;
    private ArmyInfoEventLog _event;

    // AoWArmyInfoDlg.pas:63
    public void DrawUnit(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        Rect saveRectangle;
        UnitShowInfo info;
        LibraryImage image;
        Units.Alignment alignment;
        int X;
        int Y;
        if (index < _event.ArmyView.GetCount())
        {
            saveRectangle = lockedSurface.ClipRectangle;
            lockedSurface.ClipRectangle = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            alignment = _event.ArmyView.GetUnit(index).GetAlignment();
            if ((int)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _event.ArmyView.GetUnit(index).Show(X - 33, Y - 35, lockedSurface, info);
            if ((((int)(((uint)_event.Selection) >> index)) & 1) == 0)
            {
                image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
        }
    }

    // AoWArmyInfoDlg.pas:92
    public void ClearEvent()
    {
        ArmyInfoEventLog eventValue;
        ExecuteDoneEvent notify;
        if (_unitInfoShown)
        {
            _unitInfoShown = false;
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWindow.MainWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }

        notify = _onClose;
        eventValue = _event;
        _onClose = null;
        _event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
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
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        var count = _event.ArmyView.GetCount();
        if (slot < count)
        {
            var troop = _event.ArmyView.GetUnit(slot);
            DialogHosts.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                _unitInfoShown = true;
                DialogHosts.UnitWindow.ShowUnit();
                DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(Frame sender, LockedSurface lockedSurface, int slot)
    {
        Rect R = default;
        R.Left = sender.WinLeft + UnitPnl.WinLeft + 1;
        R.Right = sender.WinWidth + R.Left;
        R.Top = sender.WinTop + UnitPnl.WinTop;
        R.Bottom = sender.WinHeight + R.Top;
        DrawUnit(slot, R, ref lockedSurface);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(LockedSurface lockedSurface)
    {
        Rect R = default;
        int I;
        R.Left = this.FaceFrame.WinLeft + Dlg.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Dlg.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        I = 0;
        while ((_event.Selection & (1 << I)) == 0 && I != _event.ArmyView.GetCount())
        {
            I += 1;
        }

        if (I < _event.ArmyView.GetCount())
        {
            _event.ArmyView.GetUnit(I).ShowFace(R, lockedSurface);
        }
    }

    // AoWArmyInfoDlg.pas:161
    public void DlgHide()
    {
        ClearEvent();
    }

    // AoWArmyInfoDlg.pas:166
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowArmyInfo(ArmyInfoDialog dialog, ArmyInfoEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog._onClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        var text = eventValue.Messages.Text;
        dialog.Message.Items.Text = text;
        dialog.Title.Text = eventValue.Title;
        dialog.HeaderLbl.Text = eventValue.Description;
        dialog.Dlg.Visible = true;
        dialog._unitInfoShown = false;
    }

}
