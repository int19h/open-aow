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
public partial class CombatChoice : DataModule
{
    public CombatChoice(Component owner) : base(owner)
    { }

    public Window Window;
    public Button FightButton;
    public Button CancelButton;
    public CheckBox TacticalCombatCheckBox;
    public UserDrawBox MapView;
    private CombatTypeRequestEventLog _request;
    private event NotifyEvent<object> OnDone;

    // TCFCDlg.pas:119
    public void Finish()
    {
        NotifyEvent<object> done;
        CombatTypeRequestEventLog request;
        DialogHosts.General.Manager.OverlayPriority = 0;
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
        DialogHosts.General.PlayButtonSound(0);
        _request.Select(0);
        Window.Visible = false;
    }

    // TCFCDlg.pas:64
    public void FightBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // TCFCDlg.pas:88
    public void MapViewDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        int X = default;
        int Y = default;
        LockedSurface viewSurfaceDescription = default;
        Rect R;
        int W;
        int H;
        R = new Rect(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HexGeometry.HexToPixel(_request.X, _request.Y, ref X, ref Y);
        X = X + 16 - (int)(((uint)W) >> 1);
        Y = Y + 16 - (int)(((uint)H) >> 1);
        if (X < 16)
        {
            X = 16;
        }

        if (Y < 16)
        {
            Y = 16;
        }

        LockedSurface.CreateFrom(lockedSurface, R, ref viewSurfaceDescription);
        StrategicMap.Current.SetViewPort(X, Y, _request.MapLevel, R.Right - R.Left, R.Bottom - R.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(ref viewSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref viewSurfaceDescription);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate()
    {
        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // TCFCDlg.pas:116
    public void DlgHide()
    {
        Finish();
    }

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
        if (DialogHosts.ConfirmationDialog.SkipCombatChoice)
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
            DialogHosts.General.Manager.OverlayPriority = 49;
            if (dialog.TacticalCombatCheckBox.Checked)
            {
                eventValue.Select((CombatType)2);
            }
            else
            {
                eventValue.Select((CombatType)1);
            }

            if (StrategicMap.Current.Players.GetPlayers(dialog._request.Player).PlayerType == PlayerType.Human)
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
