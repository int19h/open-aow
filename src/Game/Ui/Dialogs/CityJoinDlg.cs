// Translated from AoWCityJoinDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;

using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

// race, completion event, retained negotiation event
public partial class CityJoinDlg : DataModule
{
    public CityJoinDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel ApproachLbl;
    public AOWImage TCeltLineL;
    public AoWFrame CeltFrame;
    public AoWFrame BCeltL;
    public AOWMemo Memo;
    public AOWButton CancelBtn;
    public AOWButton AttackBtn;
    public AOWImage TitleIcon;
    public AOWLabel Title;
    public AOWButton AcceptBtn;
    public AOWLabel WhatLbl;
    public AoWArnoLine CityPicLine;
    public AOWButton CloseBtn;
    public IvTranslator Translator;
    public sbyte FRace;
    public ExecuteDoneEvent FDone;
    public NegotiateRequestEventLog FEvent;

    // AoWCityJoinDlg.pas:55
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        NegotiateRequestEventLog Log;
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
        DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.Accept(ref Reason))
        {
            AoWHSMap.Current.ShowWarning(Reason);
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
        DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.Reject(ref Reason))
        {
            AoWHSMap.Current.ShowWarning(Reason);
        }
        else
        {
            Dlg.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:87
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCityJoinDlg.pas:93
    public void DlgDrawBack(SurfaceDesc Surface)
    {
        LibraryImage Face;
        Face = AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).GetFace();
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
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCityJoinDlg.pas:113
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowCityNegotiation(CityJoinDlg Dialog, NegotiateRequestEventLog Event, ExecuteDoneEvent Done)
    {
        NegotiateRequestEventLog Log;
        City Town;
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        var text = Log.FJoinMessage.Text;
        Dialog.Memo.Items.Text = text;
        Dialog.ApproachLbl.Text = Log.FText;
        Town = (City)AoWHSMap.Current.MapContainer.GetField(Log.FX, Log.FY, Log.FL).FindNoneTransparentHS(typeof(City));
        Dialog.FRace = Town.FRace;
        var topCityActionImage = Town.GetTopCityActionImage(null);
        Dialog.CityPicLine.ImageTop = topCityActionImage;
        var middleCityActionImage = Town.GetMiddleCityActionImage(null);
        Dialog.CityPicLine.ImageMiddle = middleCityActionImage;
        var bottomCityActionImage = Town.GetBottomCityActionImage(null);
        Dialog.CityPicLine.ImageBottom = bottomCityActionImage;
        Dialog.Dlg.Visible = true;
        AoWHSMap.Current.FTutorialControl.ShowMessage(62, true, true);
    }

}
