// Translated from AoWMsgReceiveDlg.pas.
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
// Translated from AoWMsgReceiveDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, retained event
public partial class TMsgReceiveDlg : TDataModule
{
    public TMsgReceiveDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow MsgReceiveWin;
    public TAOWMemo MessageMemo;
    public TAOWButton Close;
    public TAOWVScrollBar MessageSB;
    public TAOWImage LShield;
    public TAOWImage RShield;
    public TAOWMemo HeaderMemo;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TEventLog FEvent;

    // AoWMsgReceiveDlg.pas:54
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TEventLog Log;
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

    // AoWMsgReceiveDlg.pas:40
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        MsgReceiveWin.Visible = false;
    }

    // AoWMsgReceiveDlg.pas:47
    public void MsgReceiveWinHide()
    {
        FinishDialog();
    }

    internal static void ShowReceivedMessage(TMsgReceiveDlg Dialog, TEventLog Event, TExecuteDoneEvent Done)
    {
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.FDone = Done;
        var text = Event.GetText();
        Dialog.HeaderMemo.Items.Text = text;
        var fMessageText = ((TGameOverEventLog)Event).FMessage.Text;
        Dialog.MessageMemo.Items.Text = fMessageText;
        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Event.FPlayer);
        Dialog.LShield.CurrentImage = player.FPlayerRace + 13;
        Dialog.RShield.CurrentImage = Dialog.LShield.CurrentImage;
        Dialog.MsgReceiveWin.Visible = true;
    }

}
