// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// editable email address list. setup control
public partial class EmailGameSettingsDialog : DataModule
{
    public EmailGameSettingsDialog(Component owner) : base(owner)
    { }

    public Button OKBtn;
    public Image TitleIcon;
    public Label Title;
    public Button CancelBut;
    public Window Dlg;
    public Label LocalEmailAdressLbl;
    public Image DecoL;
    public Image DecoR;
    public Frame AoWFrame1;
    public Edit LocalEmailAddressEdit;
    public Label PlayerEMailAddressesLbl;
    public Edit EmailAddressEdit1;
    public Panel PlayerPnl;
    public Label PlayerLbl1;
    public VerticalScrollBar PlayerSB;
    public Label PlayerLbl2;
    public Label PlayerLbl3;
    public Edit EmailAddressEdit2;
    public Edit EmailAddressEdit3;
    public Label PlayerLbl4;
    public Edit EmailAddressEdit4;
    public Label EmailGameNameLbl;
    public Edit EmailGameNameEdit;
    public Button SettingsBtn;
    public Panel Player1Pnl;
    public Panel PowePnl;
    public Panel Player2Pnl;
    public Panel Player3Pnl;
    public Panel Player4Pnl;
    public Label PlayerLbl5;
    public Edit EmailAddressEdit5;
    public Label PlayerLbl6;
    public Edit EmailAddressEdit6;
    public Panel Player5Pnl;
    public Panel Player6Pnl;
    public Panel NamePnl;
    public Panel LocalPnl;
    public Button CloseBtn;
    public IvTranslator Translator;
    public StringList FEmailAddresses;
    public SetupControl FSetupControl;

    // AoWEmailGameSettingsDlg.pas:85
    public void EmailGameSettingsDlgCreate()
    {
        FEmailAddresses = new StringList();
    }

    // AoWEmailGameSettingsDlg.pas:90
    public void EmailGameSettingsDlgDestroy()
    {
        FEmailAddresses?.Dispose();
    }

    // AoWEmailGameSettingsDlg.pas:95
    public void PlayerSBChange()
    {
        UpdatePlayers();
    }

    // AoWEmailGameSettingsDlg.pas:100
    public void EmailAddressEdit1Change(Edit Sender, int Slot)
    {
        int I;
        I = Slot + PlayerSB.Position;
        if (I < FEmailAddresses.Count)
        {
            FEmailAddresses[I] = Sender.Text;
        }
    }

    // AoWEmailGameSettingsDlg.pas:108
    public void OKBtnClick()
    {
        int Index;
        if (SettingsComplete())
        {
            if (FSetupControl != null)
            {
                for (int I = 0, last = FEmailAddresses.Count - 1; I <= last; I++)
                {
                    var objectValue = FEmailAddresses.Objects[I];
                    Index = FSetupControl.FSettings.IndexOfRace((byte)ObjectToInt(objectValue));
                    var emailAddress = FEmailAddresses[I];
                    FSetupControl.FSettings.GetSettings(Index).FEmailAddress = emailAddress;
                }

                FSetupControl.FSettings.FEmailGameName = EmailGameNameEdit.Text;
                GameHost.DeferredSetupAction = 1;
                Dlg.Visible = false;
            }
        }
        else
        {
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDlg);
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var pleaseFillInAllSettings = AbilityRules.TranslateRStr(PleaseFillInAllSettingsRStr);
            DialogHosts.MessageDlg.MessageMemo.Items.Add(pleaseFillInAllSettings);
            DialogHosts.MessageDlg.OKPnl.Visible = true;
            DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    // AoWEmailGameSettingsDlg.pas:136
    public void CancelButClick()
    {
        Dlg.Visible = false;
    }

    // AoWEmailGameSettingsDlg.pas:141
    public void SettingsBtnClick()
    {
        EmailSettingsDialog.ShowEmailSettings(DialogHosts.EmailSettingsDlg);
    }

    // AoWEmailGameSettingsDlg.pas:146
    public void LocalEmailAddressEditUpdate()
    {
        if (GameTables.AoWEngine.FRegistry.GetLocalEmailAddress() != LocalEmailAddressEdit.Text)
        {
            var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = localEmailAddress;
        }
    }

    // AoWEmailGameSettingsDlg.pas:152
    public void Player6PnlDrawBack(Panel Sender, LockedSurface SD, int Slot)
    {
        RaceResource R;
        int I;
        var panel = Sender;
        I = Slot + PlayerSB.Position;
        if (I < FEmailAddresses.Count)
        {
            var objectValue = FEmailAddresses.Objects[I];
            R = AoWHSSet.Current.FRaceResources.GetRaceResource(ObjectToInt(objectValue));
            R.AddRef();
            R.FImageLibrary[38].Show(panel.WinLeft, panel.WinTop, SD);
            R.Release();
        }
    }

    // AoWEmailGameSettingsDlg.pas:167
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWEmailGameSettingsDlg.pas:173
    public void UpdatePlayer(int Index, Label NameLabel, Edit EmailEdit)
    {
        if (Index < FEmailAddresses.Count)
        {
            EmailEdit.CaptureInput = true;
            EmailEdit.Text = FEmailAddresses[Index];
            var objectValue = FEmailAddresses.Objects[Index];
            NameLabel.Text = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[ObjectToInt(objectValue)]);
        }
        else
        {
            EmailEdit.CaptureInput = false;
            EmailEdit.Text = "";
            NameLabel.Text = "";
        }
    }

    // AoWEmailGameSettingsDlg.pas:187
    public void UpdatePlayers()
    {
        PlayerSB.Min = 0;
        var count = FEmailAddresses.Count;
        PlayerSB.Max = count - 1;
        PlayerSB.NumberVisible = 6;
        PlayerPnl.FDrawn = false;
        UpdatePlayer(PlayerSB.Position, PlayerLbl1, EmailAddressEdit1);
        UpdatePlayer(PlayerSB.Position + 1, PlayerLbl2, EmailAddressEdit2);
        UpdatePlayer(PlayerSB.Position + 2, PlayerLbl3, EmailAddressEdit3);
        UpdatePlayer(PlayerSB.Position + 3, PlayerLbl4, EmailAddressEdit4);
        UpdatePlayer(PlayerSB.Position + 4, PlayerLbl5, EmailAddressEdit5);
        UpdatePlayer(PlayerSB.Position + 5, PlayerLbl6, EmailAddressEdit6);
    }

    // AoWEmailGameSettingsDlg.pas:199
    public bool SettingsComplete()
    {
        int I;
        I = FEmailAddresses.Count;
        bool Result = !IsEmpty(LocalEmailAddressEdit.Text) && !IsEmpty(EmailGameNameEdit.Text);
        while (Result && I != 0)
        {
            I -= 1;
            Result = !IsEmpty(FEmailAddresses[I]);
        }

        return Result;
    }

    // AoWEmailGameSettingsDlg.pas:211
    public void ResetSettings()
    {
        FSetupControl = null;
        FEmailAddresses.Clear();
        PlayerSB.Visible = true;
        PlayerSB.Position = 0;
        var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
    }

    // AoWEmailGameSettingsDlg.pas:220
    public void Execute(SetupControl Control)
    {
        SetupControl C;
        ResetSettings();
        FSetupControl = Control;
        C = FSetupControl;
        for (int I = 0, last = C.FSettings.Count - 1; I <= last; I++)
        {
            if (C.FSettings.GetSettings(I).FBehavior.PlayerType == 0)
            {
                var setting = C.FSettings.GetSettings(I);
                var localEmailAddress = GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
                FEmailAddresses.AddObject(localEmailAddress, IntToObject(setting.FRace));
            }
        }

        if (FEmailAddresses.Count > 1)
        {
            UpdatePlayers();
            Dlg.Visible = true;
        }
        else
        {
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDlg);
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var youNeed2HumanPlayersToPlayAnEmailGame = AbilityRules.TranslateRStr(YouNeed2HumanPlayersToPlayAnEmailGameRStr);
            DialogHosts.MessageDlg.MessageMemo.Items.Add(youNeed2HumanPlayersToPlayAnEmailGame);
            DialogHosts.MessageDlg.OKPnl.Visible = true;
            DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    private static readonly string PleaseFillInAllSettingsRStr = "Please fill in all settings.";
    private static readonly string YouNeed2HumanPlayersToPlayAnEmailGameRStr = "You need 2 human players to play an email game.";
}
