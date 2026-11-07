// Translated from AoWCityRebelDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWWindow;
using Aow.Units.City;
using Aow.Units.EventLog;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;

namespace Aow.Units.AoWCityRebelDlg;

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
        General.PlayButtonSound(0);
        RebelWin.Visible = false;
    }

    // AoWCityRebelDlg.pas:66
    public void CenterButClick()
    {
        General.PlayButtonSound(0);
        var cityRebellionEventLog = FEvent;
        AoWHSMap.CenterViewTo(cityRebellionEventLog.FXhx, cityRebellionEventLog.FYhx, cityRebellionEventLog.FLhx);
    }

    // AoWCityRebelDlg.pas:72
    public void RebelWinHide()
    {
        ClearEvent();
    }

    // AoWCityRebelDlg.pas:77
    public void RebelWinKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            RebelWin.Visible = false;
        }
    }
}

public static partial class AoWCityRebelDlgUnit
{
}
