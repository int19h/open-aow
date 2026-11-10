// Translated from AoWCityJoinDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWCityJoinDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;

namespace Aow.Game.Ui.Dialogs;

// race, completion event, retained negotiation event
public partial class TCityJoinDlg : TDataModule
{
    public TCityJoinDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel ApproachLbl;
    public TAOWImage TCeltLineL;
    public TAoWFrame CeltFrame;
    public TAoWFrame BCeltL;
    public TAOWMemo Memo;
    public TAOWButton CancelBtn;
    public TAOWButton AttackBtn;
    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAOWButton AcceptBtn;
    public TAOWLabel WhatLbl;
    public TAoWArnoLine CityPicLine;
    public TAOWButton CloseBtn;
    public TIvTranslator Translator;
    public sbyte FRace;
    public TExecuteDoneEvent FDone;
    public TNegotiateRequestEventLog FEvent;

    // AoWCityJoinDlg.pas:55
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TNegotiateRequestEventLog Log;
        Done = FDone;
        Log = FEvent;
        FDone = null;
        FEvent = null;
        if (Done != null)
        {
            Done(Log);
        }

        if (Log != null)
        {
            Log.Release();
        }
    }

    // AoWCityJoinDlg.pas:69
    public void AcceptBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (!FEvent.Accept(ref Reason))
        {
            AoWHSMap.ShowWarning(Reason);
        }
        else
        {
            Dlg.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:78
    public void AttackBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (!FEvent.Reject(ref Reason))
        {
            AoWHSMap.ShowWarning(Reason);
        }
        else
        {
            Dlg.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:87
    public void CancelBtnClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCityJoinDlg.pas:93
    public void DlgDrawBack(TSurfaceDesc Surface)
    {
        Aow.Game.Draw.Images.TLibraryImage Face;
        Face = AoWHSSet.FRaceResources.GetRaceResource(FRace).GetFace();
        if (Face != null)
        {
            Face.ShowClipped(CeltFrame.WinLeft + 10, CeltFrame.WinTop + 28, Surface);
        }
    }

    // AoWCityJoinDlg.pas:102
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWCityJoinDlg.pas:107
    public void CloseBtnClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCityJoinDlg.pas:113
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWCityJoinDlgUnit
{
}
