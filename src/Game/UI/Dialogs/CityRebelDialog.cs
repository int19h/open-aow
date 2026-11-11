// Translated from AoWCityRebelDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// completion callback. city-rebellion event
public partial class CityRebelDialog : DataModule
{
    public CityRebelDialog(Component owner) : base(owner)
    { }

    public Window RebelWin;
    public Label Title;
    public Button Close;
    public Image TitleIcon;
    public Frame BCeltL;
    public Button CenterBut;
    public Button CloseBut;
    public Image RebelBG;
    public Label CityNameLabel;
    public Label AOWLabel2;
    public Label AOWLabel3;
    public Frame AoWFrame1;
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

    internal static void ShowRebellion(CityRebelDialog Dialog, CityRebellionEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FOnClose = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.CityNameLabel.Text = Dialog.FEvent.FCityName;
        Dialog.RebelWin.Visible = true;
    }

}
