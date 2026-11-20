#nullable disable
// Translated from AoWCityRebelDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// completion callback. city-rebellion event
public sealed partial class CityRebelDialog(UIContext ui) : UIModule(ui)
{
    public Window RebelWindow { get; private set; }
    public Label CityNameLabel { get; private set; }

    private event ExecuteDoneEvent OnClose;
    private CityRebellionEventLog _event;

    // AoWCityRebelDlg.pas:49
    public void ClearEvent()
    {
        ExecuteDoneEvent notify = OnClose;
        CityRebellionEventLog eventValue = _event;
        OnClose = null;
        _event = null;
        notify?.Invoke(eventValue);

        _ = (eventValue?.Release());
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
        CityRebellionEventLog cityRebellionEventLog = _event;
        StrategicMap.Current.CenterViewTo(cityRebellionEventLog.Location.X, cityRebellionEventLog.Location.Y, cityRebellionEventLog.Location.MapLevel);
    }

    // AoWCityRebelDlg.pas:72
    public void RebelWinHide() => ClearEvent();

    // AoWCityRebelDlg.pas:77
    public void RebelWinKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key is 27 or 32)
        {
            RebelWindow.Visible = false;
        }
    }

    internal static void ShowRebellion(CityRebelDialog dialog, CityRebellionEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.OnClose = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog.CityNameLabel.Text = dialog._event.CityName;
        dialog.RebelWindow.Visible = true;
    }
}
