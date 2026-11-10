// Translated from TCFCDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from TCFCDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Draw.GFXEUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Game.World.Map.HSEngineUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// combat request, completion event
public partial class TTCFCDialog : TDataModule
{
    public TTCFCDialog(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel DTitle;
    public TAOWLabel DMessage;
    public TAOWButton FightBtn;
    public TAOWButton CancelBtn;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWLabel Tactical;
    public TAOWLabel Automatic;
    public TAOWCheckBox TCchk;
    public TAOWCheckBox FCchk;
    public TAOWPanel MapViewPnl;
    public TaowUserDrawBox MapView;
    public TAOWImage InfoSep;
    public TIvTranslator Translator;
    public TCombatTypeRequestEventLog FRequest;
    public TNotifyEvent<object> FOnDone;

    // TCFCDlg.pas:119
    public void Finish()
    {
        TNotifyEvent<object> Done;
        TCombatTypeRequestEventLog Request;
        General.Manager.OverlayPriority = 0;
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
        General.PlayButtonSound(0);
        FRequest.Select(0);
        Dlg.Visible = false;
    }

    // TCFCDlg.pas:64
    public void FightBtnClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // TCFCDlg.pas:70
    public void FCchkChange(TAOWCheckBox Sender)
    {
        var checkBox = Sender;
        if (checkBox.Checked)
        {
            FRequest.Select((TCombatType)1);
        }

        if (checkBox.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // TCFCDlg.pas:79
    public void TCchkChange(TAOWCheckBox Sender)
    {
        var checkBox = Sender;
        if (checkBox.Checked)
        {
            FRequest.Select((TCombatType)2);
        }

        if (checkBox.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // TCFCDlg.pas:88
    public void MapViewDraw(TaowUserDrawBox Sender, TSurfaceDesc SD)
    {
        int X = default;
        int Y = default;
        TSurfaceDesc ViewSD = default;
        TRect R;
        int W;
        int H;
        R = Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HXtoHP(FRequest.FX, FRequest.FY, ref X, ref Y);
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

        CreateSDfromSD(SD, R, ref ViewSD);
        AoWHSMap.SetViewPort(X, Y, FRequest.FL, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.UpdateStaticScene();
        AoWHSMap.UpdateDynamicScene();
        AoWHSMap.DrawStaticScene(ref ViewSD);
        AoWHSMap.DrawDynamicScene(ref ViewSD);
    }

    // TCFCDlg.pas:113
    public void MapViewUpdate()
    {
        MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Frame & 1) != 0;
    }

    // TCFCDlg.pas:116
    public void DlgHide()
    {
        Finish();
    }

    // TCFCDlg.pas:133
    public void TCchkDoubleClick(TAOWCheckBox Sender)
    {
        if (Sender.Checked)
        {
            FRequest.Select((TCombatType)2);
        }

        Dlg.Visible = false;
    }

    // TCFCDlg.pas:139
    public void FCchkDoubleClick(TAOWCheckBox Sender)
    {
        if (Sender.Checked)
        {
            FRequest.Select((TCombatType)1);
        }

        Dlg.Visible = false;
    }
}

public static partial class TCFCDlgUnit
{
}
