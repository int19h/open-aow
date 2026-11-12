// Translated from AoWCityJoinDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// race, completion event, retained negotiation event
public partial class CityJoinDialog : DataModule
{
    public CityJoinDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label ApproachLabel;
    public Frame CelticFrame;
    public Memo Memo;
    public ArnoLine CityPictureLine;
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
            StrategicMap.Current.ShowWarning(reason);
        }
        else
        {
            Window.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:78
    public void AttackBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (!_event.Reject(ref reason))
        {
            StrategicMap.Current.ShowWarning(reason);
        }
        else
        {
            Window.Visible = false;
        }
    }

    // AoWCityJoinDlg.pas:87
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWCityJoinDlg.pas:93
    public void DlgDrawBack(LockedSurface surface)
    {
        LibraryImage face;
        face = GamePack.Current.RaceResources.GetRaceResource(_race).GetFace();
        if (face != null)
        {
            face.ShowClipped(CelticFrame.WinLeft + 10, CelticFrame.WinTop + 28, surface);
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
        Window.Visible = false;
    }

    // AoWCityJoinDlg.pas:113
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
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
        dialog.ApproachLabel.Text = log.Text;
        town = (City)StrategicMap.Current.MapContainer.GetField(log.X, log.Y, log.MapLevel).FindNoneTransparentHexSprite(typeof(City));
        dialog._race = town.Race;
        var topCityActionImage = town.GetTopCityActionImage(null);
        dialog.CityPictureLine.ImageTop = topCityActionImage;
        var middleCityActionImage = town.GetMiddleCityActionImage(null);
        dialog.CityPictureLine.ImageMiddle = middleCityActionImage;
        var bottomCityActionImage = town.GetBottomCityActionImage(null);
        dialog.CityPictureLine.ImageBottom = bottomCityActionImage;
        dialog.Window.Visible = true;
        StrategicMap.Current.TutorialControl.ShowMessage(62, true, true);
    }

}
