// Translated from TCFCDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// combat request, completion event
public partial class CombatChoice : DataModule
{
    public CombatChoice(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label DTitle;
    public Label DMessage;
    public Button FightBtn;
    public Button CancelBtn;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image Image1;
    public Image Image2;
    public Label Tactical;
    public Label Automatic;
    public CheckBox TCchk;
    public CheckBox FCchk;
    public Panel MapViewPnl;
    public UserDrawBox MapView;
    public Image InfoSep;
    public IvTranslator Translator;
    private CombatTypeRequestEventLog _request;
    private NotifyEvent<object> _onDone;

    // TCFCDlg.pas:119
    public void Finish()
    {
        NotifyEvent<object> done;
        CombatTypeRequestEventLog request;
        DialogHosts.General.Manager.OverlayPriority = 0;
        done = _onDone;
        request = _request;
        _onDone = null;
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
        Dlg.Visible = false;
    }

    // TCFCDlg.pas:64
    public void FightBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
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
        R = new Rect(Dlg.WinLeft + sender.WinLeft, Dlg.WinTop + sender.WinTop, Dlg.WinLeft + sender.WinLeft + sender.WinWidth, Dlg.WinTop + sender.WinTop + sender.WinHeight);
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
        AoWHSMap.Current.SetViewPort(X, Y, _request.MapLevel, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.Current.UpdateStaticScene();
        AoWHSMap.Current.UpdateDynamicScene();
        AoWHSMap.Current.DrawStaticScene(ref viewSurfaceDescription);
        AoWHSMap.Current.DrawDynamicScene(ref viewSurfaceDescription);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate()
    {
        MapView.Drawn = MapView.Drawn && (AoWHSMap.Current.Frame & 1) != 0;
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

        Dlg.Visible = false;
    }

    // TCFCDlg.pas:139
    public void FCchkDoubleClick(CheckBox sender)
    {
        if (sender.Checked)
        {
            _request.Select((CombatType)1);
        }

        Dlg.Visible = false;
    }

    internal static void ShowCombatType(CombatChoice dialog, CombatTypeRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        if (DialogHosts.ConfirmationDialog.SkipCombatChoice)
        {
            if (dialog.TCchk.Checked)
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
            dialog._onDone = RebindDelegate<NotifyEvent<object>>(done);
            DialogHosts.General.Manager.OverlayPriority = 49;
            if (dialog.TCchk.Checked)
            {
                eventValue.Select((CombatType)2);
            }
            else
            {
                eventValue.Select((CombatType)1);
            }

            if (AoWHSMap.Current.Players.GetPlayers(dialog._request.Player).PlayerType == PlayerType.Human)
            {
                dialog.CancelBtn.Visible = true;
                dialog.FightBtn.Alignment.AlignWidth = AlignWidth.Left;
            }
            else
            {
                dialog.CancelBtn.Visible = false;
                dialog.FightBtn.Alignment.AlignWidth = AlignWidth.Center;
            }

            dialog.Dlg.ReAlign();
            dialog.Dlg.Visible = true;
            AoWHSMap.Current.TutorialControl.ShowMessage(61, true, true);
        }
    }

}
