// Translated from AoWPartyJoin.pas.
using Aow.Game.Draw;

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
public partial class PartyJoinDialog : DataModule
{
    public PartyJoinDialog(Component owner) : base(owner)
    { }

    public Panel UnitPanel;
    public Button MiddleButton;
    public Label TitleLabel;
    public Button RightButton;
    public Button LeftButton;
    public Window Window;
    public Memo JoinMemo;
    public Image FaceFrame;
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
            DialogHosts.UnitWindow.MainWindow.Priority = DialogHosts.ControlWindow.MainWindow.Priority;
            DialogHosts.UnitWindow.MainWindow.Visible = false;
        }

        done = Done;
        eventValue = _joinEvent;
        Done = null;
        _joinEvent = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWPartyJoin.pas:99
    public void DrawUnit(int index, in Rect R, ref LockedSurface surface)
    {
        Units.Alignment alignment;
        int X;
        int Y;
        Rect oldClip;
        UnitShowInfo info;
        if (index < _units.GetCount())
        {
            oldClip = surface.ClipRectangle;
            surface.ClipRectangle = R;
            X = (R.Right + R.Left) / 2;
            Y = (R.Top + R.Bottom) / 2;
            alignment = _units.GetUnit(index).GetAlignment();
            if ((int)alignment < DialogHosts.UIGraphics.UnitBackgrounds.Count)
            {
                var image = DialogHosts.UIGraphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _units.GetUnit(index).Show(X - 33, Y - 35, surface, info);
            if ((((int)(((uint)_selection) >> index)) & 1) == 0)
            {
                var image = DialogHosts.UIGraphics.UnitBackgrounds[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWPartyJoin.pas:127
    public void PartyJoinDialogCreate()
    {
        _units = new UnitUserList(null);
    }

    // AoWPartyJoin.pas:132
    public void PartyJoinDialogDestroy()
    {
        _units?.Dispose();
    }

    // AoWPartyJoin.pas:137
    public void LeftBtnClick()
    {
        string reason = "";
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
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
            DialogHosts.General.PlayButtonSound(0);
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
            DialogHosts.General.PlayButtonSound(0);
            Window.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Window.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Window.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Window.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (_faceUnit != null)
            {
                if (_faceUnit.GetFace() != null)
                {
                    _faceUnit.GetFace().ShowStretched(R, surface);
                }
            }
        }
    }

    // AoWPartyJoin.pas:194
    public void U1PreDraw(Frame sender, LockedSurface surface, int slot)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = sender.WinLeft + UnitPanel.WinLeft + 1;
            R.Right = sender.WinWidth + R.Left;
            R.Top = sender.WinTop + UnitPanel.WinTop;
            R.Bottom = sender.WinHeight + R.Top;

            DrawUnit(slot, R, ref surface);
        }
    }

    // AoWPartyJoin.pas:211
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        if (ValidateJoin())
        {
            var count = _units.GetCount();
            if (slot < count)
            {
                var troop = _units.GetUnit(slot);
                DialogHosts.General.SelectedUnit = troop;
                if (button == MouseButton.Left)
                {
                    _detailsOpen = true;
                    DialogHosts.UnitWindow.ShowUnit();
                    DialogHosts.UnitWindow.MainWindow.Priority = Window.Priority + 1;
                }
            }
        }
    }

    // AoWPartyJoin.pas:226
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWPartyJoin.pas:231
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
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
        var partyJoin = AbilityRules.TranslateResourceString(ScenarioText.PartyJoinText);
        dialog.TitleLabel.Text = partyJoin;
    }

    internal static void UpdatePartyJoin(PartyJoinDialog dialog)
    {
        if (dialog.ValidateJoin())
        {
            dialog._units.Clear();
            dialog._faceUnit = StrategicMap.Current.UnitControl.FindUnit(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, ref dialog._selection);
            var text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowPartyJoin(PartyJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Window.Visible = true;
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog.Done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        var attack = AbilityRules.TranslateResourceString(GameText.AttackText);
        dialog.MiddleButton.Text = attack;
        var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
        dialog.RightButton.Text = cancel;
        UpdatePartyJoin(dialog);
        StrategicMap.Current.TutorialControl.ShowMessage(70, true, true);
    }

    internal static void ShowPrisonersJoin(PartyJoinDialog dialog, PrisonersJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Window.Visible = true;
        var prisonerLiberation = AbilityRules.TranslateResourceString(Dungeon.PrisonerLiberationText);
        dialog.TitleLabel.Text = prisonerLiberation;
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog.Done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        var reject = AbilityRules.TranslateResourceString(GameText.RejectText);
        dialog.RightButton.Text = reject;
        UpdatePartyJoin(dialog);
    }

}
