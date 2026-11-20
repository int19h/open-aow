#nullable disable
// Translated from AoWHeroJoinDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.UI.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public sealed partial class HeroJoinDialog(UIContext ui) : UIModule(ui)
{
    public Panel UnitPanel { get; private set; }
    public Button MiddleButton { get; private set; }
    public Button RightButton { get; private set; }
    public Button LeftButton { get; private set; }
    public Window Window { get; private set; }
    public Label HeroNameLabel { get; private set; }
    public Label HeroLevelLabel { get; private set; }
    public Image FaceFrame { get; private set; }
    public Memo JoinMemo { get; private set; }

    private event ExecuteDoneEvent Done;
    private JoinEventLog _joinEvent;
    private bool _detailsOpen;
    private byte _selection;
    private UnitUserList _units;
    private Hero _faceUnit;

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
        _units.Clear();
        if (_detailsOpen)
        {
            _detailsOpen = false;
            UI.UnitWindow.MainWindow.Priority = UI.ControlWindow.MainWindow.Priority;
            UI.UnitWindow.MainWindow.Visible = false;
        }

        ExecuteDoneEvent done = Done;
        EventLog eventValue = _joinEvent;
        Done = null;
        _joinEvent = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWHeroJoinDlg.pas:94
    public void DrawUnit(int index, in Rect R, ref LockedSurface surface)
    {
        UnitShowInfo info;
        if (index < _units.GetCount())
        {
            Rect oldClip = surface.ClipRectangle;
            surface.ClipRectangle = R;
            int x = (R.Right + R.Left) / 2;
            int y = (R.Top + R.Bottom) / 2;
            Units.Alignment alignment = _units.GetUnit(index).GetAlignment();
            if ((int)alignment < UI.Graphics.UnitBackgrounds.Count)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[(int)alignment];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface: surface);
            }

            info = default;
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.HitPoints, UnitOverlayModesElement.Rank);
            _ = _units.GetUnit(index).Show(x - 33, y - 35, lockedSurface: surface, info);
            if (((_selection >>> index) & 1) == 0)
            {
                LibraryImage image = UI.Graphics.UnitBackgrounds[7];
                image.ShowClipped(x - (image.Width / 2), y - (image.Height / 2), lockedSurface: surface);
            }

            surface.ClipRectangle = oldClip;
        }
    }

    // AoWHeroJoinDlg.pas:122
    public void HeroJoinDialogCreate() => _units = new UnitUserList(null) { Holder = UI.Engine };

    // AoWHeroJoinDlg.pas:127
    public void HeroJoinDialogDestroy() => _units?.Dispose();

    // AoWHeroJoinDlg.pas:132
    public void LeftBtnClick()
    {
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            string reason = "";
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
        if (ValidateJoin())
        {
            UI.General.PlayButtonSound(0);
            string reason = "";
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
        Rect r = default;
        if (ValidateJoin())
        {
            r.Left = FaceFrame.WinLeft + Window.WinLeft;
            r.Right = FaceFrame.WinWidth + r.Left;
            r.Top = FaceFrame.WinTop + Window.WinTop;
            r.Bottom = FaceFrame.WinHeight + r.Top;
            _faceUnit.GetFace()?.ShowStretched(r, lockedSurface: surface);
        }
    }

    // AoWHeroJoinDlg.pas:179
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

    // AoWHeroJoinDlg.pas:196
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

    // AoWHeroJoinDlg.pas:211
    public void DlgHide() => FinishDialog();

    // AoWHeroJoinDlg.pas:216
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
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
            dialog._faceUnit = StrategicMap.Current.UnitControl.FindUnit<Hero>(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, out dialog._selection);
            sbyte level = dialog._faceUnit.GetLevel();
            string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.LevelFormat), arguments: [IntToStr(level)]);
            dialog.HeroLevelLabel.Text = format;
            dialog.HeroNameLabel.Text = dialog._faceUnit.HeroName;
            string text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowHeroJoin(HeroJoinDialog dialog, HeroJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        _ = dialog._joinEvent.AddRef();
        dialog.Done = done;
        string accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        string cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
        dialog.RightButton.Text = cancel;
        UpdateHeroJoin(dialog);
        dialog.Window.Visible = true;
    }

    internal static void ShowHeroNegotiation(HeroJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        _ = dialog._joinEvent.AddRef();
        dialog.Done = done;
        string accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        string attack = dialog.UI.Engine.TranslateResourceString(GameText.AttackText);
        dialog.MiddleButton.Text = attack;
        string cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
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
        _ = dialog._joinEvent.AddRef();
        dialog.Done = done;
        string accept = dialog.UI.Engine.TranslateResourceString(GameText.AcceptText);
        dialog.LeftButton.Text = accept;
        dialog.MiddleButton.Visible = false;
        string reject = dialog.UI.Engine.TranslateResourceString(GameText.RejectText);
        dialog.RightButton.Text = reject;
        UpdateHeroJoin(dialog);
        dialog.Window.Visible = true;
    }
}
