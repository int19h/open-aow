// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// editable email address list. setup control
public partial class EmailGameSettingsDlg : DataModule
{
    public EmailGameSettingsDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWButton OKBtn;
    public AOWImage TitleIcon;
    public AOWLabel Title;
    public AOWButton CancelBut;
    public AOWWindow Dlg;
    public AOWLabel LocalEmailAdressLbl;
    public AOWImage DecoL;
    public AOWImage DecoR;
    public AoWFrame AoWFrame1;
    public AOWEdit LocalEmailAddressEdit;
    public AOWLabel PlayerEMailAddressesLbl;
    public AOWEdit EmailAddressEdit1;
    public AOWPanel PlayerPnl;
    public AOWLabel PlayerLbl1;
    public AOWVScrollBar PlayerSB;
    public AOWLabel PlayerLbl2;
    public AOWLabel PlayerLbl3;
    public AOWEdit EmailAddressEdit2;
    public AOWEdit EmailAddressEdit3;
    public AOWLabel PlayerLbl4;
    public AOWEdit EmailAddressEdit4;
    public AOWLabel EmailGameNameLbl;
    public AOWEdit EmailGameNameEdit;
    public AOWButton SettingsBtn;
    public AOWPanel Player1Pnl;
    public AOWPanel PowePnl;
    public AOWPanel Player2Pnl;
    public AOWPanel Player3Pnl;
    public AOWPanel Player4Pnl;
    public AOWLabel PlayerLbl5;
    public AOWEdit EmailAddressEdit5;
    public AOWLabel PlayerLbl6;
    public AOWEdit EmailAddressEdit6;
    public AOWPanel Player5Pnl;
    public AOWPanel Player6Pnl;
    public AOWPanel NamePnl;
    public AOWPanel LocalPnl;
    public AOWButton CloseBtn;
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
    public void EmailAddressEdit1Change(AOWEdit Sender, int Slot)
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
            Aow.Game.Ui.Dialogs.MessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var pleaseFillInAllSettings = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.EmailGameSettingsDlg.PleaseFillInAllSettingsRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(pleaseFillInAllSettings);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
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
        Aow.Game.Ui.Dialogs.EmailSettingsDlg.ShowEmailSettings(Aow.Game.Ui.Dialogs.DialogHosts.EmailSettingsDlg);
    }

    // AoWEmailGameSettingsDlg.pas:146
    public void LocalEmailAddressEditUpdate()
    {
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress() != LocalEmailAddressEdit.Text)
        {
            var localEmailAddress = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = localEmailAddress;
        }
    }

    // AoWEmailGameSettingsDlg.pas:152
    public void Player6PnlDrawBack(AOWPanel Sender, SurfaceDesc SD, int Slot)
    {
        RaceResource R;
        int I;
        var panel = Sender;
        I = Slot + PlayerSB.Position;
        if (I < FEmailAddresses.Count)
        {
            var objectValue = FEmailAddresses.Objects[I];
            R = Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(ObjectToInt(objectValue));
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
    public void UpdatePlayer(int Index, AOWLabel NameLabel, AOWEdit EmailEdit)
    {
        if (Index < FEmailAddresses.Count)
        {
            EmailEdit.CaptureInput = true;
            EmailEdit.Text = FEmailAddresses[Index];
            var objectValue = FEmailAddresses.Objects[Index];
            NameLabel.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[ObjectToInt(objectValue)]);
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
        var localEmailAddress = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
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
                var localEmailAddress = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress();
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
            Aow.Game.Ui.Dialogs.MessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var youNeed2HumanPlayersToPlayAnEmailGame = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.EmailGameSettingsDlg.YouNeed2HumanPlayersToPlayAnEmailGameRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(youNeed2HumanPlayersToPlayAnEmailGame);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
        }
    }

    private static readonly string PleaseFillInAllSettingsRStr = "Please fill in all settings.";
    private static readonly string YouNeed2HumanPlayersToPlayAnEmailGameRStr = "You need 2 human players to play an email game.";
}
