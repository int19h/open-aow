// Translated from AoWCityJoinDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWCityJoinDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.Accept(ref Reason))
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (!FEvent.Reject(ref Reason))
        {
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
        }
        else
        {
            Dlg.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:87
    public void CancelBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWCityJoinDlg.pas:93
    public void DlgDrawBack(TSurfaceDesc Surface)
    {
        Aow.Game.Draw.Images.TLibraryImage Face;
        Face = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FRaceResources.GetRaceResource(FRace).GetFace();
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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

    internal static void ShowCityNegotiation(TCityJoinDlg Dialog, TNegotiateRequestEventLog Event, TExecuteDoneEvent Done)
    {
        TNegotiateRequestEventLog Log;
        TCity Town;
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        var text = Log.FJoinMessage.Text;
        Dialog.Memo.Items.Text = text;
        Dialog.ApproachLbl.Text = Log.FText;
        Town = (TCity)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetField(Log.FX, Log.FY, Log.FL).FindNoneTransparentHS(typeof(TCity));
        Dialog.FRace = Town.FRace;
        var topCityActionImage = Town.GetTopCityActionImage(null);
        Dialog.CityPicLine.ImageTop = topCityActionImage;
        var middleCityActionImage = Town.GetMiddleCityActionImage(null);
        Dialog.CityPicLine.ImageMiddle = middleCityActionImage;
        var bottomCityActionImage = Town.GetBottomCityActionImage(null);
        Dialog.CityPicLine.ImageBottom = bottomCityActionImage;
        Dialog.Dlg.Visible = true;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(62, true, true);
    }

}
