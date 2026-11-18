#nullable disable
// Translated from AoWPartyJoin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class PartyJoinDialog(UIContext ui) : UIModule(ui)
{
    public Panel UnitPanel { get; private set; }
    public Button MiddleButton { get; private set; }
    public Label TitleLabel { get; private set; }
    public Button RightButton { get; private set; }
    public Button LeftButton { get; private set; }
    public Window Window { get; private set; }
    public Memo JoinMemo { get; private set; }
    public Image FaceFrame { get; private set; }

    private event ExecuteDoneEvent Done;
    private JoinEventLog _joinEvent;
    private bool _detailsOpen;
    private byte _selection;
    private UnitUserList _units;
    private Troop _faceUnit;

    // AoWPartyJoin.pas:73
    public bool ValidateJoin()
    {
        bool result = _joinEvent.Validate();
        if (!result)
        {
            Window.Visible = false;
        }

        return result;
    }

    // AoWPartyJoin.pas:79
    public void FinishDialog()
    {
        EventLog eventValue;
        ExecuteDoneEvent done;
        _units.Clear();
        if (_detailsOpen)
        {
            _detailsOpen = false;
            UI.UnitWindow.MainWindow.Priority = UI.ControlWindow.MainWindow.Priority;
            UI.UnitWindow.MainWindow.Visible = false;
        }

        done = Done;
        eventValue = _joinEvent;
        Done = null;
        _joinEvent = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWPartyJoin.pas:99
    public void DrawUnit(int index, in Rect R, ref LockedSurface surface)
    {
        Units.Alignment alignment;
        int x;
        int y;
        Rect oldClip;
        UnitShowInfo info;
        if (index < _units.GetCount())
        {
            oldClip = surface.ClipRectangle;
            surface.ClipRectangle = R;
            x = (R.Right + R.Left) / 2;
            y = (R.Top + R.Bottom) / 2;
            alignment = _units.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _ = _units.GetUnit(index).Show(x - 33, y - 35, surface, info);
            if ((((int)(((uint)_selection) >> index)) & 1) == 0)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWPartyJoin.pas:127
    public void PartyJoinDialogCreate() => _units = new UnitUserList(null) { Holder = UI.Engine };

    // AoWPartyJoin.pas:132
    public void PartyJoinDialogDestroy() => _units?.Dispose();

    // AoWPartyJoin.pas:137
    public void LeftBtnClick()
    {
        string reason = "";
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            if (!_joinEvent.Accept(ref reason))
            {
                StrategicMap.Current.ShowWarning(reason);
            }
            else
            {
                Window.Visible = false;
            }
        }
    }

    // AoWPartyJoin.pas:149
    public void MiddleBtnClick()
    {
        string reason = "";
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            if (!_joinEvent.Reject(ref reason))
            {
                StrategicMap.Current.ShowWarning(reason);
            }
            else
            {
                Window.Visible = false;
            }
        }
    }

    // AoWPartyJoin.pas:161
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            Window.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            Window.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect r = default;
        if (ValidateJoin())
        {
            r.Left = FaceFrame.WinLeft + Window.WinLeft;
            r.Right = FaceFrame.WinWidth + r.Left;
            r.Top = FaceFrame.WinTop + Window.WinTop;
            r.Bottom = FaceFrame.WinHeight + r.Top;
            _faceUnit?.GetFace()?.ShowStretched(r, surface);
        }
    }

    // AoWPartyJoin.pas:194
    public void U1PreDraw(Frame sender, LockedSurface surface, int slot)
    {
        Rect r = default;
        if (ValidateJoin())
        {
            r.Left = sender.WinLeft + UnitPanel.AbsoluteLeft + 1;
            r.Right = sender.WinWidth + r.Left;
            r.Top = sender.WinTop + UnitPanel.AbsoluteTop;
            r.Bottom = sender.WinHeight + r.Top;

            DrawUnit(slot, r, ref surface);
        }
    }

    // AoWPartyJoin.pas:211
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        if (ValidateJoin())
        {
            int count = _units.GetCount();
            if (slot < count)
            {
                Troop troop = _units.GetUnit(slot);
                UI.General.SelectedUnit = troop;
                if (button == MouseButton.Left)
                {
                    _detailsOpen = true;
                    UI.UnitWindow.ShowUnit();
                    UI.UnitWindow.MainWindow.Priority = Window.Priority + 1;
                }
            }
        }
    }

    // AoWPartyJoin.pas:226
    public void DlgHide() => FinishDialog();

    // AoWPartyJoin.pas:231
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ResetPartyJoin(PartyJoinDialog dialog)
    {
        dialog.LeftButton.Visible = true;
        dialog.MiddleButton.Visible = true;
        dialog.RightButton.Visible = true;
        string partyJoin = dialog.UI.Engine.TranslateResourceString(ScenarioText.PartyJoinText);
        dialog.TitleLabel.Text = partyJoin;
    }

    internal static void UpdatePartyJoin(PartyJoinDialog dialog)
    {
        if (dialog.ValidateJoin())
        {
            dialog._units.Clear();
            dialog._faceUnit = StrategicMap.Current.UnitControl.FindUnit(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, out dialog._selection);
            string text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowPartyJoin(PartyJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Window.Visible = true;
        dialog._joinEvent = eventValue;
        _ = dialog._joinEvent.AddRef();
        dialog.Done = done;
        string accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        string attack = dialog.UI.Engine.TranslateResourceString(GameText.AttackText);
        dialog.MiddleButton.Text = attack;
        string cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
        dialog.RightButton.Text = cancel;
        UpdatePartyJoin(dialog);
        StrategicMap.Current.TutorialControl.ShowMessage(70, true, true);
    }

    internal static void ShowPrisonersJoin(PartyJoinDialog dialog, PrisonersJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Window.Visible = true;
        string prisonerLiberation = dialog.UI.Engine.TranslateResourceString(Dungeon.PrisonerLiberationText);
        dialog.TitleLabel.Text = prisonerLiberation;
        dialog._joinEvent = eventValue;
        _ = dialog._joinEvent.AddRef();
        dialog.Done = done;
        string accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        string reject = dialog.UI.Engine.TranslateResourceString(GameText.RejectText);
        dialog.RightButton.Text = reject;
        UpdatePartyJoin(dialog);
    }

}
