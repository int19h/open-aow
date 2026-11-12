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
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class PartyJoinDialog : DataModule
{
    public PartyJoinDialog(Component owner) : base(owner)
    { }

    public Panel UnitPnl;
    public Button MiddleBtn;
    public Label Title;
    public Button RightBtn;
    public Button LeftBtn;
    public Window Dlg;
    public Memo JoinMemo;
    public Image FaceFrame;
    private ExecuteDoneEvent _done;
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
            Dlg.Visible = false;
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
            DialogHosts.UnitWindow.UnitWin.Priority = DialogHosts.ControlWindow.MainWindow.Priority;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }

        done = _done;
        eventValue = _joinEvent;
        _done = null;
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
            if ((int)alignment < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _units.GetUnit(index).Show(X - 33, Y - 35, surface, info);
            if ((((int)(((uint)_selection) >> index)) & 1) == 0)
            {
                var image = DialogHosts.UIGraphics.UnitBGs[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWPartyJoin.pas:127
    public void PartyJoinDlgCreate()
    {
        _units = new UnitUserList(null);
    }

    // AoWPartyJoin.pas:132
    public void PartyJoinDlgDestroy()
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
                AoWHSMap.Current.ShowWarning(reason);
            }
            else
            {
                Dlg.Visible = false;
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
                AoWHSMap.Current.ShowWarning(reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }
    }

    // AoWPartyJoin.pas:161
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:170
    public void CloseClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWPartyJoin.pas:179
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
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
            R.Left = sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = sender.WinWidth + R.Left;
            R.Top = sender.WinTop + UnitPnl.WinTop;
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
                    DialogHosts.UnitWindow.UnitWin.Priority = Dlg.Priority + 1;
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
            Dlg.Visible = false;
        }
    }

    internal static void ResetPartyJoin(PartyJoinDialog dialog)
    {
        dialog.LeftBtn.Visible = true;
        dialog.MiddleBtn.Visible = true;
        dialog.RightBtn.Visible = true;
        var partyJoin = AbilityRules.TranslateResourceString(ScenarioText.PartyJoinText);
        dialog.Title.Text = partyJoin;
    }

    internal static void UpdatePartyJoin(PartyJoinDialog dialog)
    {
        if (dialog.ValidateJoin())
        {
            dialog._units.Clear();
            dialog._faceUnit = AoWHSMap.Current.UnitControl.FindUnit(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, ref dialog._selection);
            var text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowPartyJoin(PartyJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Dlg.Visible = true;
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog._done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftBtn.Text = accept;
        var attack = AbilityRules.TranslateResourceString(GameText.AttackText);
        dialog.MiddleBtn.Text = attack;
        var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
        dialog.RightBtn.Text = cancel;
        UpdatePartyJoin(dialog);
        AoWHSMap.Current.TutorialControl.ShowMessage(70, true, true);
    }

    internal static void ShowPrisonersJoin(PartyJoinDialog dialog, PrisonersJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetPartyJoin(dialog);
        dialog.Dlg.Visible = true;
        var prisonerLiberation = AbilityRules.TranslateResourceString(Dungeon.PrisonerLiberationText);
        dialog.Title.Text = prisonerLiberation;
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog._done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftBtn.Text = accept;
        dialog.MiddleBtn.Visible = false;
        var reject = AbilityRules.TranslateResourceString(GameText.RejectText);
        dialog.RightBtn.Text = reject;
        UpdatePartyJoin(dialog);
    }

}
