#nullable disable warnings
// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// editable email address list. setup control
public sealed partial class EmailGameSettingsDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Edit LocalEmailAddressEdit { get; private set; }
    public Edit EmailAddressEdit1 { get; private set; }
    public Panel PlayerPanel { get; private set; }
    public Label PlayerLabel1 { get; private set; }
    public VerticalScrollBar PlayerScrollBar { get; private set; }
    public Label PlayerLabel2 { get; private set; }
    public Label PlayerLabel3 { get; private set; }
    public Edit EmailAddressEdit2 { get; private set; }
    public Edit EmailAddressEdit3 { get; private set; }
    public Label PlayerLabel4 { get; private set; }
    public Edit EmailAddressEdit4 { get; private set; }
    public Edit EmailGameNameEdit { get; private set; }
    public Label PlayerLabel5 { get; private set; }
    public Edit EmailAddressEdit5 { get; private set; }
    public Label PlayerLabel6 { get; private set; }
    public Edit EmailAddressEdit6 { get; private set; }

    public DelphiStringList EmailAddresses { get; private set; }

    private SetupControl? _setupControl;

    // AoWEmailGameSettingsDlg.pas:85
    public void EmailGameSettingsDialogCreate() => EmailAddresses = new DelphiStringList();

    // AoWEmailGameSettingsDlg.pas:90
    public void EmailGameSettingsDialogDestroy() => EmailAddresses?.Dispose();

    // AoWEmailGameSettingsDlg.pas:95
    public void PlayerSBChange() => UpdatePlayers();

    // AoWEmailGameSettingsDlg.pas:100
    public void EmailAddressEdit1Change(Edit sender, int slot)
    {
        int i = slot + PlayerScrollBar.Position;
        if (i < EmailAddresses.Count)
        {
            EmailAddresses[i] = sender.Text;
        }
    }

    // AoWEmailGameSettingsDlg.pas:108
    public void OKBtnClick()
    {
        if (SettingsComplete())
        {
            if (_setupControl is not null)
            {
                for (int i = 0, last = EmailAddresses.Count - 1; i <= last; i++)
                {
                    object? objectValue = EmailAddresses.Objects[i];
                    int index = _setupControl.Settings.IndexOfRace((byte)ObjectToInt(objectValue));
                    string emailAddress = EmailAddresses[i];
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
            _ = UI.MessageDialog.MessageMemo.Items.Add("");
            string pleaseFillInAllSettings = UI.Engine.TranslateResourceString(s_pleaseFillInAllSettingsText);
            _ = UI.MessageDialog.MessageMemo.Items.Add(pleaseFillInAllSettings);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    // AoWEmailGameSettingsDlg.pas:136
    public void CancelButClick() => Window.Visible = false;

    // AoWEmailGameSettingsDlg.pas:141
    public void SettingsBtnClick() => EmailSettingsDialog.ShowEmailSettings(UI.EmailSettingsDialog);

    // AoWEmailGameSettingsDlg.pas:146
    public void LocalEmailAddressEditUpdate()
    {
        if (UI.Engine.Registry.GetLocalEmailAddress() != LocalEmailAddressEdit.Text)
        {
            string localEmailAddress = UI.Engine.Registry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = localEmailAddress;
        }
    }

    // AoWEmailGameSettingsDlg.pas:152
    public void Player6PnlDrawBack(Panel sender, LockedSurface lockedSurface, int slot)
    {
        Panel panel = sender;
        int i = slot + PlayerScrollBar.Position;
        if (i < EmailAddresses.Count)
        {
            object? objectValue = EmailAddresses.Objects[i];
            RaceResource? r = GamePack.Current.RaceResources.GetRaceResource(ObjectToInt(objectValue));
            _ = r.AddRef();
            r.ImageLibrary[38].Show(panel.AbsoluteLeft, panel.AbsoluteTop, lockedSurface);
            _ = r.Release();
        }
    }

    // AoWEmailGameSettingsDlg.pas:167
    public void DlgKeyDown(in ushort key, ShiftStates shift)
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
            object? objectValue = EmailAddresses.Objects[index];
            nameLabel.Text = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[ObjectToInt(objectValue)]);
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
        int count = EmailAddresses.Count;
        PlayerScrollBar.Max = count - 1;
        PlayerScrollBar.NumberVisible = 6;
        PlayerPanel.IsDrawn = false;
        UpdatePlayer(PlayerScrollBar.Position, nameLabel: PlayerLabel1, emailEdit: EmailAddressEdit1);
        UpdatePlayer(PlayerScrollBar.Position + 1, nameLabel: PlayerLabel2, emailEdit: EmailAddressEdit2);
        UpdatePlayer(PlayerScrollBar.Position + 2, nameLabel: PlayerLabel3, emailEdit: EmailAddressEdit3);
        UpdatePlayer(PlayerScrollBar.Position + 3, nameLabel: PlayerLabel4, emailEdit: EmailAddressEdit4);
        UpdatePlayer(PlayerScrollBar.Position + 4, nameLabel: PlayerLabel5, emailEdit: EmailAddressEdit5);
        UpdatePlayer(PlayerScrollBar.Position + 5, nameLabel: PlayerLabel6, emailEdit: EmailAddressEdit6);
    }

    // AoWEmailGameSettingsDlg.pas:199
    public bool SettingsComplete()
    {
        int i = EmailAddresses.Count;
        bool result = !IsEmpty(LocalEmailAddressEdit.Text) && !IsEmpty(EmailGameNameEdit.Text);
        while (result && i != 0)
        {
            i -= 1;
            result = !IsEmpty(EmailAddresses[i]);
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
        string localEmailAddress = UI.Engine.Registry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
    }

    // AoWEmailGameSettingsDlg.pas:220
    public void Execute(SetupControl control)
    {
        ResetSettings();
        _setupControl = control;
        SetupControl c = _setupControl;
        for (int i = 0, last = c.Settings.Count - 1; i <= last; i++)
        {
            if (c.Settings.GetSettings(i).Behavior.PlayerType == 0)
            {
                PlayerSetupSettings? setting = c.Settings.GetSettings(i);
                string localEmailAddress = UI.Engine.Registry.GetLocalEmailAddress();
                _ = EmailAddresses.AddObject(localEmailAddress, item: IntToObject(setting.Race));
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
            _ = UI.MessageDialog.MessageMemo.Items.Add("");
            string youNeed2HumanPlayersToPlayAnEmailGame = UI.Engine.TranslateResourceString(s_youNeed2HumanPlayersToPlayEmailGameText);
            _ = UI.MessageDialog.MessageMemo.Items.Add(youNeed2HumanPlayersToPlayAnEmailGame);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
        }
    }

    private static readonly string s_pleaseFillInAllSettingsText = "Please fill in all settings.";
    private static readonly string s_youNeed2HumanPlayersToPlayEmailGameText = "You need 2 human players to play an email game.";
}
