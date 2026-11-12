// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// editable email address list. setup control
public partial class EmailGameSettingsDialog : UIModule
{
    public EmailGameSettingsDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Edit LocalEmailAddressEdit;
    public Edit EmailAddressEdit1;
    public Panel PlayerPanel;
    public Label PlayerLabel1;
    public VerticalScrollBar PlayerScrollBar;
    public Label PlayerLabel2;
    public Label PlayerLabel3;
    public Edit EmailAddressEdit2;
    public Edit EmailAddressEdit3;
    public Label PlayerLabel4;
    public Edit EmailAddressEdit4;
    public Edit EmailGameNameEdit;
    public Label PlayerLabel5;
    public Edit EmailAddressEdit5;
    public Label PlayerLabel6;
    public Edit EmailAddressEdit6;

    public StringList EmailAddresses { get; private set; }

    private SetupControl _setupControl;

    // AoWEmailGameSettingsDlg.pas:85
    public void EmailGameSettingsDialogCreate()
    {
        EmailAddresses = new StringList();
    }

    // AoWEmailGameSettingsDlg.pas:90
    public void EmailGameSettingsDialogDestroy()
    {
        EmailAddresses?.Dispose();
    }

    // AoWEmailGameSettingsDlg.pas:95
    public void PlayerSBChange()
    {
        UpdatePlayers();
    }

    // AoWEmailGameSettingsDlg.pas:100
    public void EmailAddressEdit1Change(Edit sender, int slot)
    {
        int I;
        I = slot + PlayerScrollBar.Position;
        if (I < EmailAddresses.Count)
        {
            EmailAddresses[I] = sender.Text;
        }
    }

    // AoWEmailGameSettingsDlg.pas:108
    public void OKBtnClick()
    {
        int index;
        if (SettingsComplete())
        {
            if (_setupControl != null)
            {
                for (int I = 0, last = EmailAddresses.Count - 1; I <= last; I++)
                {
                    var objectValue = EmailAddresses.Objects[I];
                    index = _setupControl.Settings.IndexOfRace((byte)ObjectToInt(objectValue));
                    var emailAddress = EmailAddresses[I];
                    _setupControl.Settings.GetSettings(index).EmailAddress = emailAddress;
                }

                _setupControl.Settings.EmailGameName = EmailGameNameEdit.Text;
                GameHost.DeferredSetupAction = 1;
                Window.Visible = false;
            }
        }
        else
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            UI.MessageDialog.MessageMemo.Items.Add("");
            var pleaseFillInAllSettings = AbilityRules.TranslateResourceString(s_pleaseFillInAllSettingsText);
            UI.MessageDialog.MessageMemo.Items.Add(pleaseFillInAllSettings);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    // AoWEmailGameSettingsDlg.pas:136
    public void CancelButClick()
    {
        Window.Visible = false;
    }

    // AoWEmailGameSettingsDlg.pas:141
    public void SettingsBtnClick()
    {
        EmailSettingsDialog.ShowEmailSettings(UI.EmailSettingsDialog);
    }

    // AoWEmailGameSettingsDlg.pas:146
    public void LocalEmailAddressEditUpdate()
    {
        if (GameTables.GameEngine.Registry.GetLocalEmailAddress() != LocalEmailAddressEdit.Text)
        {
            var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = localEmailAddress;
        }
    }

    // AoWEmailGameSettingsDlg.pas:152
    public void Player6PnlDrawBack(Panel sender, LockedSurface lockedSurface, int slot)
    {
        RaceResource R;
        int I;
        var panel = sender;
        I = slot + PlayerScrollBar.Position;
        if (I < EmailAddresses.Count)
        {
            var objectValue = EmailAddresses.Objects[I];
            R = GamePack.Current.RaceResources.GetRaceResource(ObjectToInt(objectValue));
            R.AddRef();
            R.ImageLibrary[38].Show(panel.WinLeft, panel.WinTop, lockedSurface);
            R.Release();
        }
    }

    // AoWEmailGameSettingsDlg.pas:167
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    // AoWEmailGameSettingsDlg.pas:173
    public void UpdatePlayer(int index, Label nameLabel, Edit emailEdit)
    {
        if (index < EmailAddresses.Count)
        {
            emailEdit.CaptureInput = true;
            emailEdit.Text = EmailAddresses[index];
            var objectValue = EmailAddresses.Objects[index];
            nameLabel.Text = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[ObjectToInt(objectValue)]);
        }
        else
        {
            emailEdit.CaptureInput = false;
            emailEdit.Text = "";
            nameLabel.Text = "";
        }
    }

    // AoWEmailGameSettingsDlg.pas:187
    public void UpdatePlayers()
    {
        PlayerScrollBar.Min = 0;
        var count = EmailAddresses.Count;
        PlayerScrollBar.Max = count - 1;
        PlayerScrollBar.NumberVisible = 6;
        PlayerPanel.Drawn = false;
        UpdatePlayer(PlayerScrollBar.Position, PlayerLabel1, EmailAddressEdit1);
        UpdatePlayer(PlayerScrollBar.Position + 1, PlayerLabel2, EmailAddressEdit2);
        UpdatePlayer(PlayerScrollBar.Position + 2, PlayerLabel3, EmailAddressEdit3);
        UpdatePlayer(PlayerScrollBar.Position + 3, PlayerLabel4, EmailAddressEdit4);
        UpdatePlayer(PlayerScrollBar.Position + 4, PlayerLabel5, EmailAddressEdit5);
        UpdatePlayer(PlayerScrollBar.Position + 5, PlayerLabel6, EmailAddressEdit6);
    }

    // AoWEmailGameSettingsDlg.pas:199
    public bool SettingsComplete()
    {
        int I;
        I = EmailAddresses.Count;
        bool result = !IsEmpty(LocalEmailAddressEdit.Text) && !IsEmpty(EmailGameNameEdit.Text);
        while (result && I != 0)
        {
            I -= 1;
            result = !IsEmpty(EmailAddresses[I]);
        }

        return result;
    }

    // AoWEmailGameSettingsDlg.pas:211
    public void ResetSettings()
    {
        _setupControl = null;
        EmailAddresses.Clear();
        PlayerScrollBar.Visible = true;
        PlayerScrollBar.Position = 0;
        var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
    }

    // AoWEmailGameSettingsDlg.pas:220
    public void Execute(SetupControl control)
    {
        SetupControl C;
        ResetSettings();
        _setupControl = control;
        C = _setupControl;
        for (int I = 0, last = C.Settings.Count - 1; I <= last; I++)
        {
            if (C.Settings.GetSettings(I).Behavior.PlayerType == 0)
            {
                var setting = C.Settings.GetSettings(I);
                var localEmailAddress = GameTables.GameEngine.Registry.GetLocalEmailAddress();
                EmailAddresses.AddObject(localEmailAddress, IntToObject(setting.Race));
            }
        }

        if (EmailAddresses.Count > 1)
        {
            UpdatePlayers();
            Window.Visible = true;
        }
        else
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            UI.MessageDialog.MessageMemo.Items.Add("");
            var youNeed2HumanPlayersToPlayAnEmailGame = AbilityRules.TranslateResourceString(s_youNeed2HumanPlayersToPlayEmailGameText);
            UI.MessageDialog.MessageMemo.Items.Add(youNeed2HumanPlayersToPlayAnEmailGame);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    private static readonly string s_pleaseFillInAllSettingsText = "Please fill in all settings.";
    private static readonly string s_youNeed2HumanPlayersToPlayEmailGameText = "You need 2 human players to play an email game.";
}
