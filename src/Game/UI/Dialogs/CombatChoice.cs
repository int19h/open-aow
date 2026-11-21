#nullable disable warnings
// Translated from TCFCDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelegateRoutines;

namespace Aow.Game.UI.Dialogs;

// combat request, completion event
public sealed partial class CombatChoice(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Button FightButton { get; private set; }
    public Button CancelButton { get; private set; }
    public CheckBox TacticalCombatCheckBox { get; private set; }
    public UserDrawBox MapView { get; private set; }

    private CombatTypeRequestEventLog? _request;
    private NotifyEvent<object>? _done;

    // TCFCDlg.pas:119
    public void Finish()
    {
        UI.General.Manager.OverlayPriority = 0;
        NotifyEvent<object>? done = _done;
        CombatTypeRequestEventLog? request = _request;
        _done = null;
        _request = null;
        done?.Invoke(request);

        _ = (request?.Release());
    }

    // TCFCDlg.pas:57
    public void CancelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _request.Select(0);
        Window.Visible = false;
    }

    // TCFCDlg.pas:64
    public void FightBtnClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // TCFCDlg.pas:70
    public void FCchkChange(CheckBox sender)
    {
        CheckBox checkBox = sender;
        if (checkBox.Checked)
        {
            _request.Select(CombatType.Fast);
        }

        if (checkBox.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // TCFCDlg.pas:79
    public void TCchkChange(CheckBox sender)
    {
        CheckBox checkBox = sender;
        if (checkBox.Checked)
        {
            _request.Select(CombatType.Tactical);
        }

        if (checkBox.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // TCFCDlg.pas:88
    public void MapViewDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        Rect r = new(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
        int w = r.Right - r.Left;
        int h = r.Bottom - r.Top;
        HexGeometry.HexToPixel(_request.Location.X, _request.Location.Y, out int x, out int y);
        x = x + 16 - (w >>> 1);
        y = y + 16 - (h >>> 1);
        if (x < 16)
        {
            x = 16;
        }

        if (y < 16)
        {
            y = 16;
        }

        LockedSurface viewSurfaceDescription = LockedSurface.CreateFrom(lockedSurface, r);
        _ = StrategicMap.Current.SetViewPort(x, y, lHitPoints: _request.Location.MapLevel, r.Right - r.Left, r.Bottom - r.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(in viewSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref viewSurfaceDescription);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate() => MapView.IsDrawn = MapView.IsDrawn && (StrategicMap.Current.Frame & 1) != 0;

    // TCFCDlg.pas:116
    public void DlgHide() => Finish();

    // TCFCDlg.pas:133
    public void TCchkDoubleClick(CheckBox sender)
    {
        if (sender.Checked)
        {
            _request.Select(CombatType.Tactical);
        }

        Window.Visible = false;
    }

    // TCFCDlg.pas:139
    public void FCchkDoubleClick(CheckBox sender)
    {
        if (sender.Checked)
        {
            _request.Select(CombatType.Fast);
        }

        Window.Visible = false;
    }

    internal static void ShowCombatType(CombatChoice dialog, CombatTypeRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        if (dialog.UI.ConfirmationDialog.SkipCombatChoice)
        {
            if (dialog.TacticalCombatCheckBox.Checked)
            {
                eventValue.Select(CombatType.Tactical);
            }
            else
            {
                eventValue.Select(CombatType.Fast);
            }

            done(eventValue);
        }
        else
        {
            dialog._request = eventValue;
            _ = dialog._request.AddRef();
            dialog._done = RebindDelegate<NotifyEvent<object>>(done);
            dialog.UI.General.Manager.OverlayPriority = 49;
            if (dialog.TacticalCombatCheckBox.Checked)
            {
                eventValue.Select(CombatType.Tactical);
            }
            else
            {
                eventValue.Select(CombatType.Fast);
            }

            if (StrategicMap.Current.Players.GetPlayers(dialog._request.AttackerPlayer).PlayerType == PlayerType.Human)
            {
                dialog.CancelButton.Visible = true;
                dialog.FightButton.Alignment.AlignWidth = AlignWidth.Left;
            }
            else
            {
                dialog.CancelButton.Visible = false;
                dialog.FightButton.Alignment.AlignWidth = AlignWidth.Center;
            }

            dialog.Window.ReAlign();
            dialog.Window.Visible = true;
            StrategicMap.Current.TutorialControl.ShowMessage(61, immediate: true, onceOnly: true);
        }
    }
}
