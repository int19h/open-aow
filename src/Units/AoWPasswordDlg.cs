// Translated from AoWPasswordDlg.pas.
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
// Translated from AoWPasswordDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class PasswordDlg : DataModule
{
    public PasswordDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWPanel OKCancelPnl;
    public AOWButton OKBtn;
    public AOWButton CancelBtn;
    public AOWLabel PasswordLbl;
    public AOWEdit PasswordEdit;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public PasswordEventLog FPasswordEvent;

    // AoWPasswordDlg.pas:43
    public void Finish()
    {
        ExecuteDoneEvent Done;
        PasswordEventLog Event;
        Dlg.Visible = false;
        Done = FDone;
        Event = FPasswordEvent;
        FDone = null;
        FPasswordEvent = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWPasswordDlg.pas:42
    public void Reset()
    {
    }

    // AoWPasswordDlg.pas:55
    public void DlgHide()
    {
        Finish();
    }

    // AoWPasswordDlg.pas:58
    public void OKBtnClick()
    {
        FPasswordEvent.FPassword = PasswordEdit.Text;
        FPasswordEvent.FPressedButton = PressedEventLogButton.Ok;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:65
    public void CancelBtnClick()
    {
        FPasswordEvent.FPassword = "";
        FPasswordEvent.FPressedButton = PressedEventLogButton.Cancel;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:72
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            CancelBtnClick();
        }
    }

    internal static void ShowPasswordEvent(PasswordDlg Dialog, PasswordEventLog Event, ExecuteDoneEvent Done)
    {
        PasswordEventLog Log;
        Dialog.Reset();
        Dialog.FDone = Done;
        Dialog.FPasswordEvent = Event;
        Dialog.FPasswordEvent.AddRef();
        Log = Dialog.FPasswordEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        Dialog.PasswordLbl.Text = Log.FPrompt;
        Dialog.PasswordEdit.Text = Log.FPassword;
        Dialog.PasswordEdit.FRedraw = true;
        Dialog.Dlg.Visible = true;
    }

}
