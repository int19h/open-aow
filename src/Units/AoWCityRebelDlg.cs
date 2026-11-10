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
public partial class CityRebelDlg : DataModule
{
    public CityRebelDlg(Component owner) : base(owner)
    { }

    public AOWWindow RebelWin;
    public AOWLabel Title;
    public AOWButton Close;
    public AOWImage TitleIcon;
    public AoWFrame BCeltL;
    public AOWButton CenterBut;
    public AOWButton CloseBut;
    public AOWImage RebelBG;
    public AOWLabel CityNameLabel;
    public AOWLabel AOWLabel2;
    public AOWLabel AOWLabel3;
    public AoWFrame AoWFrame1;
    public IvTranslator Translator;
    public ExecuteDoneEvent FOnClose;
    public CityRebellionEventLog FEvent;

    // AoWCityRebelDlg.pas:49
    public void ClearEvent()
    {
        CityRebellionEventLog Event;
        ExecuteDoneEvent Notify;
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
        DialogHosts.General.PlayButtonSound(0);
        RebelWin.Visible = false;
    }

    // AoWCityRebelDlg.pas:66
    public void CenterButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var cityRebellionEventLog = FEvent;
        AoWHSMap.Current.CenterViewTo(cityRebellionEventLog.FXhx, cityRebellionEventLog.FYhx, cityRebellionEventLog.FLhx);
    }

    // AoWCityRebelDlg.pas:72
    public void RebelWinHide()
    {
        ClearEvent();
    }

    // AoWCityRebelDlg.pas:77
    public void RebelWinKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            RebelWin.Visible = false;
        }
    }

    internal static void ShowRebellion(CityRebelDlg Dialog, CityRebellionEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.CityNameLabel.Text = Dialog.FEvent.FCityName;
        Dialog.RebelWin.Visible = true;
    }

}
