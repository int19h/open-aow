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
    public Label Label2;
    public Label Label3;
    public Frame Frame1;
    public IvTranslator Translator;
    public ExecuteDoneEvent OnClose;
    public CityRebellionEventLog Event;

    // AoWCityRebelDlg.pas:49
    public void ClearEvent()
    {
        CityRebellionEventLog eventValue;
        ExecuteDoneEvent notify;
        notify = OnClose;
        eventValue = Event;
        OnClose = null;
        Event = null;
        if (notify != null)
        {
            notify(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
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
        var cityRebellionEventLog = Event;
        AoWHSMap.Current.CenterViewTo(cityRebellionEventLog.HexX, cityRebellionEventLog.HexY, cityRebellionEventLog.HexLevel);
    }

    // AoWCityRebelDlg.pas:72
    public void RebelWinHide()
    {
        ClearEvent();
    }

    // AoWCityRebelDlg.pas:77
    public void RebelWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            RebelWin.Visible = false;
        }
    }

    internal static void ShowRebellion(CityRebelDialog dialog, CityRebellionEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog.Event = eventValue;
        dialog.Event.AddRef();
        dialog.CityNameLabel.Text = dialog.Event.CityName;
        dialog.RebelWin.Visible = true;
    }

}
