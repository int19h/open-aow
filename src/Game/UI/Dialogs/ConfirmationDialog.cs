using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// The DWin form (AOWDialogWin.pas): a Yes/No confirmation and the debug message window. The
// confirmation runs the caller's callback on Yes and knows nothing of what it confirms. The debug
// window is its own DebugWindow over the form's components.
public partial class ConfirmationDialog : UIModule
{
    public ConfirmationDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Label TitleLabel;
    public Label MessageLabel;
    public Panel OKPanel;
    public Panel YesNoPanel;
    // The original stored an action code that yesBtnClick switched on.
    private Action _onConfirmed;

    public bool SkipCombatChoice { get; }

    public DebugWindow Debug { get; private set; }

    // AOWDialogWin.pas:121
    public void Confirm(string title, string text, Action onConfirmed)
    {
        _onConfirmed = onConfirmed;
        TitleLabel.Text = title;
        MessageLabel.Text = text;
        YesNoPanel.Visible = true;
        OKPanel.Visible = false;
        Window.Visible = true;
    }

    // AOWDialogWin.pas:60
    public void DButtonClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AOWDialogWin.pas:67
    public void DebugDOkBtnClick() => Debug.Close();

    // AOWDialogWin.pas:73
    public void yesBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _onConfirmed?.Invoke();
        Window.Visible = false;
    }

    // AOWDialogWin.pas:104
    public void DialogKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                DButtonClick();
                break;
            case 13:
                if (YesNoPanel.Visible)
                {
                    yesBtnClick();
                }
                else
                {
                    DButtonClick();
                }

                break;
        }
    }
}
