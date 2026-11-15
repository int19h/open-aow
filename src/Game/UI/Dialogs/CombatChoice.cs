#nullable disable
// Translated from TCFCDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// combat request, completion event
public partial class CombatChoice : UIModule
{
    public CombatChoice(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Button FightButton { get; private set; }
    public Button CancelButton { get; private set; }
    public CheckBox TacticalCombatCheckBox { get; private set; }
    public UserDrawBox MapView { get; private set; }

    private CombatTypeRequestEventLog _request;
    private event NotifyEvent<object> OnDone;

    // TCFCDlg.pas:119
    public void Finish()
    {
        NotifyEvent<object> done;
        CombatTypeRequestEventLog request;
        UI.General.Manager.OverlayPriority = 0;
        done = OnDone;
        request = _request;
        OnDone = null;
        _request = null;
        if (done != null)
        {
            done(request);
        }

        if (request != null)
        {
            request.Release();
        }
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
        var checkBox = sender;
        if (checkBox.Checked)
        {
            _request.Select((CombatType)1);
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
        var checkBox = sender;
        if (checkBox.Checked)
        {
            _request.Select((CombatType)2);
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
        int x = default;
        int y = default;
        LockedSurface viewSurfaceDescription = default;
        Rect r;
        int w;
        int h;
        r = new Rect(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
        w = r.Right - r.Left;
        h = r.Bottom - r.Top;
        HexGeometry.HexToPixel(_request.Location.X, _request.Location.Y, out x, out y);
        x = x + 16 - (int)(((uint)w) >> 1);
        y = y + 16 - (int)(((uint)h) >> 1);
        if (x < 16)
        {
            x = 16;
        }

        if (y < 16)
        {
            y = 16;
        }

        LockedSurface.CreateFrom(lockedSurface, r, out viewSurfaceDescription);
        StrategicMap.Current.SetViewPort(x, y, _request.Location.MapLevel, r.Right - r.Left, r.Bottom - r.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(ref viewSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref viewSurfaceDescription);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate() => MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;

    // TCFCDlg.pas:116
    public void DlgHide() => Finish();

    // TCFCDlg.pas:133
    public void TCchkDoubleClick(CheckBox sender)
    {
        if (sender.Checked)
        {
            _request.Select((CombatType)2);
        }

        Window.Visible = false;
    }

    // TCFCDlg.pas:139
    public void FCchkDoubleClick(CheckBox sender)
    {
        if (sender.Checked)
        {
            _request.Select((CombatType)1);
        }

        Window.Visible = false;
    }

    internal static void ShowCombatType(CombatChoice dialog, CombatTypeRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        if (dialog.UI.ConfirmationDialog.SkipCombatChoice)
        {
            if (dialog.TacticalCombatCheckBox.Checked)
            {
                eventValue.Select((CombatType)2);
            }
            else
            {
                eventValue.Select((CombatType)1);
            }

            done(eventValue);
        }
        else
        {
            dialog._request = eventValue;
            dialog._request.AddRef();
            dialog.OnDone = RebindDelegate<NotifyEvent<object>>(done);
            dialog.UI.General.Manager.OverlayPriority = 49;
            if (dialog.TacticalCombatCheckBox.Checked)
            {
                eventValue.Select((CombatType)2);
            }
            else
            {
                eventValue.Select((CombatType)1);
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
            StrategicMap.Current.TutorialControl.ShowMessage(61, true, true);
        }
    }

}
