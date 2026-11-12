// Translated from AoWCityRebelDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// completion callback. city-rebellion event
public partial class CityRebelDialog : UIModule
{
    public CityRebelDialog(UIContext ui) : base(ui)
    { }

    public Window RebelWindow;
    public Label CityNameLabel;
    private event ExecuteDoneEvent OnClose;
    private CityRebellionEventLog _event;

    // AoWCityRebelDlg.pas:49
    public void ClearEvent()
    {
        CityRebellionEventLog eventValue;
        ExecuteDoneEvent notify;
        notify = OnClose;
        eventValue = _event;
        OnClose = null;
        _event = null;
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
        UI.General.PlayButtonSound(0);
        RebelWindow.Visible = false;
    }

    // AoWCityRebelDlg.pas:66
    public void CenterButClick()
    {
        UI.General.PlayButtonSound(0);
        var cityRebellionEventLog = _event;
        StrategicMap.Current.CenterViewTo(cityRebellionEventLog.HexX, cityRebellionEventLog.HexY, cityRebellionEventLog.HexLevel);
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
            RebelWindow.Visible = false;
        }
    }

    internal static void ShowRebellion(CityRebelDialog dialog, CityRebellionEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        dialog.CityNameLabel.Text = dialog._event.CityName;
        dialog.RebelWindow.Visible = true;
    }

}
