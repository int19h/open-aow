// Translated from AoWCityRebelDlg.pas.
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
// Translated from AoWCityRebelDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// completion callback. city-rebellion event
public partial class TCityRebelDlg : TDataModule
{
    public TCityRebelDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow RebelWin;
    public TAOWLabel Title;
    public TAOWButton Close;
    public TAOWImage TitleIcon;
    public TAoWFrame BCeltL;
    public TAOWButton CenterBut;
    public TAOWButton CloseBut;
    public TAOWImage RebelBG;
    public TAOWLabel CityNameLabel;
    public TAOWLabel AOWLabel2;
    public TAOWLabel AOWLabel3;
    public TAoWFrame AoWFrame1;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FOnClose;
    public TCityRebellionEventLog FEvent;

    // AoWCityRebelDlg.pas:49
    public void ClearEvent()
    {
        TCityRebellionEventLog Event;
        TExecuteDoneEvent Notify;
        Notify = FOnClose;
        Event = FEvent;
        FOnClose = null;
        FEvent = null;
        if (Notify != null)
        {
            Notify(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWCityRebelDlg.pas:60
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        RebelWin.Visible = false;
    }

    // AoWCityRebelDlg.pas:66
    public void CenterButClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var cityRebellionEventLog = FEvent;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(cityRebellionEventLog.FXhx, cityRebellionEventLog.FYhx, cityRebellionEventLog.FLhx);
    }

    // AoWCityRebelDlg.pas:72
    public void RebelWinHide()
    {
        ClearEvent();
    }

    // AoWCityRebelDlg.pas:77
    public void RebelWinKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            RebelWin.Visible = false;
        }
    }

    internal static void ShowRebellion(TCityRebelDlg Dialog, TCityRebellionEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.CityNameLabel.Text = Dialog.FEvent.FCityName;
        Dialog.RebelWin.Visible = true;
    }

}
