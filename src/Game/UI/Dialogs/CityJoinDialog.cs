// Translated from AoWCityJoinDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// race, completion event, retained negotiation event
public partial class CityJoinDialog : DataModule
{
    public CityJoinDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label ApproachLbl;
    public Image TCeltLineL;
    public Frame CeltFrame;
    public Frame BCeltL;
    public Memo Memo;
    public Button CancelBtn;
    public Button AttackBtn;
    public Image TitleIcon;
    public Label Title;
    public Button AcceptBtn;
    public Label WhatLbl;
    public ArnoLine CityPicLine;
    public Button CloseBtn;
    public IvTranslator Translator;
    private sbyte _race;
    private ExecuteDoneEvent _done;
    private NegotiateRequestEventLog _event;

    // AoWCityJoinDlg.pas:55
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        NegotiateRequestEventLog log;
        done = _done;
        log = _event;
        _done = null;
        _event = null;
        if (done != null)
        {
            done(log);
        }

        if (log != null)
        {
            log.Release();
        }
    }

    // AoWCityJoinDlg.pas:69
    public void AcceptBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!_event.Accept(ref reason))
        {
            AoWHSMap.Current.ShowWarning(reason);
        }
        else
        {
            Dlg.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:78
    public void AttackBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!_event.Reject(ref reason))
        {
            AoWHSMap.Current.ShowWarning(reason);
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
    public void DlgDrawBack(LockedSurface surface)
    {
        LibraryImage face;
        face = AoWHSSet.Current.RaceResources.GetRaceResource(_race).GetFace();
        if (face != null)
        {
            face.ShowClipped(CeltFrame.WinLeft + 10, CeltFrame.WinTop + 28, surface);
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
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowCityNegotiation(CityJoinDialog dialog, NegotiateRequestEventLog eventValue, ExecuteDoneEvent done)
    {
        NegotiateRequestEventLog log;
        City town;
        dialog._done = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        log = dialog._event;
        var text = log.JoinMessage.Text;
        dialog.Memo.Items.Text = text;
        dialog.ApproachLbl.Text = log.Text;
        town = (City)AoWHSMap.Current.MapContainer.GetField(log.X, log.Y, log.MapLevel).FindNoneTransparentHexSprite(typeof(City));
        dialog._race = town.Race;
        var topCityActionImage = town.GetTopCityActionImage(null);
        dialog.CityPicLine.ImageTop = topCityActionImage;
        var middleCityActionImage = town.GetMiddleCityActionImage(null);
        dialog.CityPicLine.ImageMiddle = middleCityActionImage;
        var bottomCityActionImage = town.GetBottomCityActionImage(null);
        dialog.CityPicLine.ImageBottom = bottomCityActionImage;
        dialog.Dlg.Visible = true;
        AoWHSMap.Current.TutorialControl.ShowMessage(62, true, true);
    }

}
