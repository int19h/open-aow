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
public partial class ArmyInfoDialog : UIModule
{
    public ArmyInfoDialog(UIContext ui) : base(ui)
    { }

    public Panel UnitPanel;
    public Label TitleLabel;
    public Window Window;
    public Memo MessageMemo;
    public Image FaceFrame;
    public Label HeaderLabel;
    private bool _unitInfoShown;
    private event ExecuteDoneEvent OnClose;
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
        if (index < _event.ArmyView.Units.Count)
        {
            saveRectangle = lockedSurface.ClipRectangle;
            lockedSurface.ClipRectangle = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            alignment = _event.ArmyView.Units.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _event.ArmyView.Units.GetUnit(index).Show(X - 33, Y - 35, lockedSurface, info);
            if ((((int)(((uint)_event.Selection) >> index)) & 1) == 0)
            {
                image = UI.Graphics.UnitBackgrounds[7];
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
            UI.UnitWindow.MainWindow.Priority = UI.ControlWindow.MainWindow.Priority;
            UI.UnitWindow.MainWindow.Visible = false;
        }

        notify = OnClose;
        eventValue = _event;
        OnClose = null;
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
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWArmyInfoDlg.pas:115
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWArmyInfoDlg.pas:121
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        var count = _event.ArmyView.Units.Count;
        if (slot < count)
        {
            var troop = _event.ArmyView.Units.GetUnit(slot);
            UI.General.SelectedUnit = troop;
            if (button == MouseButton.Left)
            {
                _unitInfoShown = true;
                UI.UnitWindow.ShowUnit();
                UI.UnitWindow.MainWindow.Priority = Window.Priority + 1;
            }
        }
    }

    // AoWArmyInfoDlg.pas:134
    public void U1PreDraw(Frame sender, LockedSurface lockedSurface, int slot)
    {
        Rect R = default;
        R.Left = sender.WinLeft + UnitPanel.WinLeft + 1;
        R.Right = sender.WinWidth + R.Left;
        R.Top = sender.WinTop + UnitPanel.WinTop;
        R.Bottom = sender.WinHeight + R.Top;
        DrawUnit(slot, R, ref lockedSurface);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(LockedSurface lockedSurface)
    {
        Rect R = default;
        int I;
        R.Left = this.FaceFrame.WinLeft + Window.WinLeft;
        R.Right = this.FaceFrame.WinWidth + R.Left;
        R.Top = this.FaceFrame.WinTop + Window.WinTop;
        R.Bottom = this.FaceFrame.WinHeight + R.Top;

        I = 0;
        while ((_event.Selection & (1 << I)) == 0 && I != _event.ArmyView.Units.Count)
        {
            I += 1;
        }

        if (I < _event.ArmyView.Units.Count)
        {
            _event.ArmyView.Units.GetUnit(I).ShowFace(R, lockedSurface);
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
            Window.Visible = false;
        }
    }

    internal static void ShowArmyInfo(ArmyInfoDialog dialog, ArmyInfoEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        var text = eventValue.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        dialog.TitleLabel.Text = eventValue.Title;
        dialog.HeaderLabel.Text = eventValue.Description;
        dialog.Window.Visible = true;
        dialog._unitInfoShown = false;
    }

}
