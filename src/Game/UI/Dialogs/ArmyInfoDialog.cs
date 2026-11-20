#nullable disable
// Translated from AoWArmyInfoDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// unit-info window open. completion callback. army-info event
public partial class ArmyInfoDialog(UIContext ui) : UIModule(ui)
{
    public Panel UnitPanel { get; private set; }
    public Label TitleLabel { get; private set; }
    public Window Window { get; private set; }
    public Memo MessageMemo { get; private set; }
    public Image FaceFrame { get; private set; }
    public Label HeaderLabel { get; private set; }

    private bool _unitInfoShown;
    private event ExecuteDoneEvent OnClose;
    private ArmyInfoEventLog _event;

    // AoWArmyInfoDlg.pas:63
    public void DrawUnit(int index, in Rect R, ref LockedSurface lockedSurface)
    {
        UnitShowInfo info;
        if (index < _event.ArmyView.Units.Count)
        {
            Rect saveRectangle = lockedSurface.ClipRectangle;
            lockedSurface.ClipRectangle = R;
            int x = (R.Right + R.Left) / 2;
            int y = (R.Top + R.Bottom) / 2;
            Units.Alignment alignment = _event.ArmyView.Units.GetUnit(index).GetAlignment();
            LibraryImage image;
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                // { D3 masks the enum index after the original full-byte guard. }
                image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _ = _event.ArmyView.Units.GetUnit(index).Show(x - 33, y - 35, lockedSurface, info);
            if ((((int)(((uint)_event.Selection) >> index)) & 1) == 0)
            {
                image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface);
            }

            lockedSurface.ClipRectangle = saveRectangle;
        }
    }

    // AoWArmyInfoDlg.pas:92
    public void ClearEvent()
    {
        if (_unitInfoShown)
        {
            _unitInfoShown = false;
            UI.UnitWindow.MainWindow.Priority = UI.ControlWindow.MainWindow.Priority;
            UI.UnitWindow.MainWindow.Visible = false;
        }

        ExecuteDoneEvent notify = OnClose;
        ArmyInfoEventLog eventValue = _event;
        OnClose = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());
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
        int count = _event.ArmyView.Units.Count;
        if (slot < count)
        {
            Troop troop = _event.ArmyView.Units.GetUnit(slot);
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
        Rect r = default;
        r.Left = sender.WinLeft + UnitPanel.AbsoluteLeft + 1;
        r.Right = sender.WinWidth + r.Left;
        r.Top = sender.WinTop + UnitPanel.AbsoluteTop;
        r.Bottom = sender.WinHeight + r.Top;
        DrawUnit(slot, r, ref lockedSurface);
    }

    // AoWArmyInfoDlg.pas:146
    public void FaceFramePreDraw(LockedSurface lockedSurface)
    {
        Rect r = default;
        r.Left = FaceFrame.WinLeft + Window.WinLeft;
        r.Right = FaceFrame.WinWidth + r.Left;
        r.Top = FaceFrame.WinTop + Window.WinTop;
        r.Bottom = FaceFrame.WinHeight + r.Top;

        int i = 0;
        while ((_event.Selection & (1 << i)) == 0 && i != _event.ArmyView.Units.Count)
        {
            i += 1;
        }

        if (i < _event.ArmyView.Units.Count)
        {
            _event.ArmyView.Units.GetUnit(i).ShowFace(r, lockedSurface);
        }
    }

    // AoWArmyInfoDlg.pas:161
    public void DlgHide() => ClearEvent();

    // AoWArmyInfoDlg.pas:166
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key is 27 or 32)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowArmyInfo(ArmyInfoDialog dialog, ArmyInfoEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        string text = eventValue.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        dialog.TitleLabel.Text = eventValue.Title;
        dialog.HeaderLabel.Text = eventValue.Description;
        dialog.Window.Visible = true;
        dialog._unitInfoShown = false;
    }
}
