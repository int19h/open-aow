// Translated from TCFCDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;

using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// combat request, completion event
public partial class CombatChoice : DataModule
{
    public CombatChoice(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel DTitle;
    public AOWLabel DMessage;
    public AOWButton FightBtn;
    public AOWButton CancelBtn;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWLabel Tactical;
    public AOWLabel Automatic;
    public AOWCheckBox TCchk;
    public AOWCheckBox FCchk;
    public AOWPanel MapViewPnl;
    public TaowUserDrawBox MapView;
    public AOWImage InfoSep;
    public IvTranslator Translator;
    public CombatTypeRequestEventLog FRequest;
    public NotifyEvent<object> FOnDone;

    // TCFCDlg.pas:119
    public void Finish()
    {
        NotifyEvent<object> Done;
        CombatTypeRequestEventLog Request;
        DialogHosts.General.Manager.OverlayPriority = 0;
        Done = FOnDone;
        Request = FRequest;
        FOnDone = null;
        FRequest = null;
        if (Done != null)
        {
            Done(Request);
        }

        if (Request != null)
        {
            Request.Release();
        }
    }

    // TCFCDlg.pas:57
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FRequest.Select(0);
        Dlg.Visible = false;
    }

    // TCFCDlg.pas:64
    public void FightBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // TCFCDlg.pas:70
    public void FCchkChange(AOWCheckBox Sender)
    {
        var checkBox = Sender;
        if (checkBox.Checked)
        {
            FRequest.Select((CombatType)1);
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
    public void TCchkChange(AOWCheckBox Sender)
    {
        var checkBox = Sender;
        if (checkBox.Checked)
        {
            FRequest.Select((CombatType)2);
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
    public void MapViewDraw(TaowUserDrawBox Sender, SurfaceDesc SD)
    {
        int X = default;
        int Y = default;
        SurfaceDesc ViewSD = default;
        Rect R;
        int W;
        int H;
        R = new Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        World.Map.HexGeometry.HXtoHP(FRequest.FX, FRequest.FY, ref X, ref Y);
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

        SurfaceDesc.CreateSDfromSD(SD, R, ref ViewSD);
        World.Map.AoWHSMap.Current.SetViewPort(X, Y, FRequest.FL, R.Right - R.Left, R.Bottom - R.Top);
        World.Map.AoWHSMap.Current.UpdateStaticScene();
        World.Map.AoWHSMap.Current.UpdateDynamicScene();
        World.Map.AoWHSMap.Current.DrawStaticScene(ref ViewSD);
        World.Map.AoWHSMap.Current.DrawDynamicScene(ref ViewSD);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate()
    {
        MapView.FDrawn = MapView.FDrawn && (World.Map.AoWHSMap.Current.Frame & 1) != 0;
    }

    // TCFCDlg.pas:116
    public void DlgHide()
    {
        Finish();
    }

    // TCFCDlg.pas:133
    public void TCchkDoubleClick(AOWCheckBox Sender)
    {
        if (Sender.Checked)
        {
            FRequest.Select((CombatType)2);
        }

        Dlg.Visible = false;
    }

    // TCFCDlg.pas:139
    public void FCchkDoubleClick(AOWCheckBox Sender)
    {
        if (Sender.Checked)
        {
            FRequest.Select((CombatType)1);
        }

        Dlg.Visible = false;
    }

    internal static void ShowCombatType(CombatChoice Dialog, CombatTypeRequestEventLog Event, ExecuteDoneEvent Done)
    {
        if (DialogHosts.DialogWin.FSkipCombatChoice)
        {
            if (Dialog.TCchk.Checked)
            {
                Event.Select((CombatType)2);
            }
            else
            {
                Event.Select((CombatType)1);
            }

            Done(Event);
        }
        else
        {
            Dialog.FRequest = Event;
            Dialog.FRequest.AddRef();
            Dialog.FOnDone = RebindDelegate<NotifyEvent<object>>(Done);
            DialogHosts.General.Manager.OverlayPriority = 49;
            if (Dialog.TCchk.Checked)
            {
                Event.Select((CombatType)2);
            }
            else
            {
                Event.Select((CombatType)1);
            }

            if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Dialog.FRequest.FPlayer).FPlayerType == PlayerType.Human)
            {
                Dialog.CancelBtn.Visible = true;
                Dialog.FightBtn.Alignment.AlignWidth = AOWAlignWidth.Left;
            }
            else
            {
                Dialog.CancelBtn.Visible = false;
                Dialog.FightBtn.Alignment.AlignWidth = AOWAlignWidth.Center;
            }

            Dialog.Dlg.ReAlign();
            Dialog.Dlg.Visible = true;
            World.Map.AoWHSMap.Current.FTutorialControl.ShowMessage(61, true, true);
        }
    }

}
