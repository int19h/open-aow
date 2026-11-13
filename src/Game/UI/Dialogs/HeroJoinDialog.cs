// Translated from AoWHeroJoinDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class HeroJoinDialog : UIModule
{
    public HeroJoinDialog(UIContext ui) : base(ui)
    { }

    public Panel UnitPanel;
    public Button MiddleButton;
    public Button RightButton;
    public Button LeftButton;
    public Window Window;
    public Label HeroNameLabel;
    public Label HeroLevelLabel;
    public Image FaceFrame;
    public Memo JoinMemo;
    private event ExecuteDoneEvent Done;
    private JoinEventLog _joinEvent;
    private bool _detailsOpen;
    private byte _selection;
    private UnitUserList _units;
    private Troop _faceUnit;

    // AoWHeroJoinDlg.pas:68
    public bool ValidateJoin()
    {
        bool result = _joinEvent.Validate();
        if (!result)
        {
            Window.Visible = false;
        }

        return result;
    }

    // AoWHeroJoinDlg.pas:74
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
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWHeroJoinDlg.pas:94
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
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                var image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _units.GetUnit(index).Show(X - 33, Y - 35, surface, info);
            if ((((int)(((uint)_selection) >> index)) & 1) == 0)
            {
                var image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(X - image.Width / 2, Y - image.Height / 2, surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWHeroJoinDlg.pas:122
    public void HeroJoinDialogCreate()
    {
        _units = new UnitUserList(null) { Holder = UI.Engine };
    }

    // AoWHeroJoinDlg.pas:127
    public void HeroJoinDialogDestroy()
    {
        _units?.Dispose();
    }

    // AoWHeroJoinDlg.pas:132
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

    // AoWHeroJoinDlg.pas:144
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

    // AoWHeroJoinDlg.pas:156
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            Window.Visible = false;
        }
    }

    // AoWHeroJoinDlg.pas:165
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Window.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Window.WinTop;
            R.Bottom = FaceFrame.WinHeight + R.Top;
            if (_faceUnit.GetFace() != null)
            {
                _faceUnit.GetFace().ShowStretched(R, surface);
            }
        }
    }

    // AoWHeroJoinDlg.pas:179
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

    // AoWHeroJoinDlg.pas:196
    public void U1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        if (ValidateJoin())
        {
            var count = _units.GetCount();
            if (slot < count)
            {
                var troop = _units.GetUnit(slot);
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

    // AoWHeroJoinDlg.pas:211
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWHeroJoinDlg.pas:216
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void UpdateHeroJoin(HeroJoinDialog dialog)
    {
        if (dialog.ValidateJoin())
        {
            dialog._units.Clear();
            dialog._faceUnit = StrategicMap.Current.UnitControl.FindUnit(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, out dialog._selection);
            var level = ((Hero)dialog._faceUnit).GetLevel();
            var format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.LevelFormat), [IntToStr(level)]);
            dialog.HeroLevelLabel.Text = format;
            dialog.HeroNameLabel.Text = ((Hero)dialog._faceUnit).HeroName;
            var text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowHeroJoin(HeroJoinDialog dialog, HeroJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog.Done = done;
        var accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        var cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
        dialog.RightButton.Text = cancel;
        UpdateHeroJoin(dialog);
        dialog.Window.Visible = true;
    }

    internal static void ShowHeroNegotiation(HeroJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog.Done = done;
        var accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        var attack = dialog.UI.Engine.TranslateResourceString(GameText.AttackText);
        dialog.MiddleButton.Text = attack;
        var cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
        dialog.RightButton.Text = cancel;
        UpdateHeroJoin(dialog);
        dialog.Window.Visible = true;
    }

    internal static void ResetHeroJoin(HeroJoinDialog dialog)
    {
        dialog.LeftButton.Visible = true;
        dialog.MiddleButton.Visible = true;
        dialog.RightButton.Visible = true;
    }

    internal static void ShowPrisonerHero(HeroJoinDialog dialog, PrisonersJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog.Done = done;
        var accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        var reject = dialog.UI.Engine.TranslateResourceString(GameText.RejectText);
        dialog.RightButton.Text = reject;
        UpdateHeroJoin(dialog);
        dialog.Window.Visible = true;
    }

}
