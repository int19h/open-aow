// Translated from AoWHeroJoinDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, join event, details flag, selection mask, unit list, face unit
public partial class HeroJoinDialog : DataModule
{
    public HeroJoinDialog(Component owner) : base(owner)
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
    public Button MiddleBtn;
    public Frame BCeltL;
    public Button RightBtn;
    public Button LeftBtn;
    public Window Dlg;
    public Label HeroName;
    public Label HeroLevel;
    public Image FaceFrame;
    public Label DetailsLbl;
    public Memo JoinMemo;
    public IvTranslator Translator;
    private ExecuteDoneEvent _done;
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
            Dlg.Visible = false;
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

    // AoWHeroJoinDlg.pas:122
    public void HeroJoinDlgCreate()
    {
        _units = new UnitUserList(null);
    }

    // AoWHeroJoinDlg.pas:127
    public void HeroJoinDlgDestroy()
    {
        _units?.Dispose();
    }

    // AoWHeroJoinDlg.pas:132
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

    // AoWHeroJoinDlg.pas:144
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

    // AoWHeroJoinDlg.pas:156
    public void RightBtnClick()
    {
        if (ValidateJoin())
        {
            DialogHosts.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }
    }

    // AoWHeroJoinDlg.pas:165
    public void FaceFramePreDraw(LockedSurface surface)
    {
        Rect R = default;
        if (ValidateJoin())
        {
            R.Left = FaceFrame.WinLeft + Dlg.WinLeft;
            R.Right = FaceFrame.WinWidth + R.Left;
            R.Top = FaceFrame.WinTop + Dlg.WinTop;
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
            R.Left = sender.WinLeft + UnitPnl.WinLeft + 1;
            R.Right = sender.WinWidth + R.Left;
            R.Top = sender.WinTop + UnitPnl.WinTop;
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
            Dlg.Visible = false;
        }
    }

    internal static void UpdateHeroJoin(HeroJoinDialog dialog)
    {
        if (dialog.ValidateJoin())
        {
            dialog._units.Clear();
            dialog._faceUnit = AoWHSMap.Current.UnitControl.FindUnit(dialog._joinEvent.FirstUnitId);
            dialog._joinEvent.ListJoinUnits(dialog._units, ref dialog._selection);
            var level = ((Hero)dialog._faceUnit).GetLevel();
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LevelFormat), [IntToStr(level)]);
            dialog.HeroLevel.Text = format;
            dialog.HeroName.Text = ((Hero)dialog._faceUnit).HeroName;
            var text = dialog._joinEvent.JoinMessage.Text;
            dialog.JoinMemo.Items.Text = text;
        }
    }

    internal static void ShowHeroJoin(HeroJoinDialog dialog, HeroJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog._done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftBtn.Text = accept;
        dialog.MiddleBtn.Visible = false;
        var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
        dialog.RightBtn.Text = cancel;
        UpdateHeroJoin(dialog);
        dialog.Dlg.Visible = true;
    }

    internal static void ShowHeroNegotiation(HeroJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog._done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftBtn.Text = accept;
        var attack = AbilityRules.TranslateResourceString(GameText.AttackText);
        dialog.MiddleBtn.Text = attack;
        var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
        dialog.RightBtn.Text = cancel;
        UpdateHeroJoin(dialog);
        dialog.Dlg.Visible = true;
    }

    internal static void ResetHeroJoin(HeroJoinDialog dialog)
    {
        dialog.LeftBtn.Visible = true;
        dialog.MiddleBtn.Visible = true;
        dialog.RightBtn.Visible = true;
    }

    internal static void ShowPrisonerHero(HeroJoinDialog dialog, PrisonersJoinEventLog eventValue, ExecuteDoneEvent done)
    {
        ResetHeroJoin(dialog);
        dialog._joinEvent = eventValue;
        dialog._joinEvent.AddRef();
        dialog._done = done;
        var accept = AbilityRules.TranslateResourceString(GameText.AcceptText);
        dialog.LeftBtn.Text = accept;
        dialog.MiddleBtn.Visible = false;
        var reject = AbilityRules.TranslateResourceString(GameText.RejectText);
        dialog.RightBtn.Text = reject;
        UpdateHeroJoin(dialog);
        dialog.Dlg.Visible = true;
    }

}
