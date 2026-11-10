// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;
using static Aow.Game.Ui.Dialogs.E3GlobalsUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// editable email address list. setup control
public partial class TEmailGameSettingsDlg : TDataModule
{
    public TEmailGameSettingsDlg(TComponent owner) : base(owner)
    { }

    public TAOWButton OKBtn;
    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAOWButton CancelBut;
    public TAOWWindow Dlg;
    public TAOWLabel LocalEmailAdressLbl;
    public TAOWImage DecoL;
    public TAOWImage DecoR;
    public TAoWFrame AoWFrame1;
    public TAOWEdit LocalEmailAddressEdit;
    public TAOWLabel PlayerEMailAddressesLbl;
    public TAOWEdit EmailAddressEdit1;
    public TAOWPanel PlayerPnl;
    public TAOWLabel PlayerLbl1;
    public TAOWVScrollBar PlayerSB;
    public TAOWLabel PlayerLbl2;
    public TAOWLabel PlayerLbl3;
    public TAOWEdit EmailAddressEdit2;
    public TAOWEdit EmailAddressEdit3;
    public TAOWLabel PlayerLbl4;
    public TAOWEdit EmailAddressEdit4;
    public TAOWLabel EmailGameNameLbl;
    public TAOWEdit EmailGameNameEdit;
    public TAOWButton SettingsBtn;
    public TAOWPanel Player1Pnl;
    public TAOWPanel PowePnl;
    public TAOWPanel Player2Pnl;
    public TAOWPanel Player3Pnl;
    public TAOWPanel Player4Pnl;
    public TAOWLabel PlayerLbl5;
    public TAOWEdit EmailAddressEdit5;
    public TAOWLabel PlayerLbl6;
    public TAOWEdit EmailAddressEdit6;
    public TAOWPanel Player5Pnl;
    public TAOWPanel Player6Pnl;
    public TAOWPanel NamePnl;
    public TAOWPanel LocalPnl;
    public TAOWButton CloseBtn;
    public TIvTranslator Translator;
    public TStringList FEmailAddresses;
    public TSetupControl FSetupControl;

    // AoWEmailGameSettingsDlg.pas:85
    public void EmailGameSettingsDlgCreate()
    {
        FEmailAddresses = new TStringList();
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
    public void EmailAddressEdit1Change(TAOWEdit Sender, int Slot)
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
            ResetMessageDialog(AoWMessageDlgUnit.MessageDlg);
            AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add("");
            var pleaseFillInAllSettings = TranslateRStr(PleaseFillInAllSettingsRStr);
            AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add(pleaseFillInAllSettings);
            AoWMessageDlgUnit.MessageDlg.OKPnl.Visible = true;
            AoWMessageDlgUnit.MessageDlg.Dlg.Visible = true;
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
        ShowEmailSettings(EmailSettingsDlg);
    }

    // AoWEmailGameSettingsDlg.pas:146
    public void LocalEmailAddressEditUpdate()
    {
        if (AoWEngine.FRegistry.GetLocalEmailAddress() != LocalEmailAddressEdit.Text)
        {
            var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = localEmailAddress;
        }
    }

    // AoWEmailGameSettingsDlg.pas:152
    public void Player6PnlDrawBack(TAOWPanel Sender, TSurfaceDesc SD, int Slot)
    {
        TRaceResource R;
        int I;
        var panel = Sender;
        I = Slot + PlayerSB.Position;
        if (I < FEmailAddresses.Count)
        {
            var objectValue = FEmailAddresses.Objects[I];
            R = AoWHSSet.FRaceResources.GetRaceResource(ObjectToInt(objectValue));
            R.AddRef();
            R.FImageLibrary[38].Show(panel.WinLeft, panel.WinTop, SD);
            R.Release();
        }
    }

    // AoWEmailGameSettingsDlg.pas:167
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWEmailGameSettingsDlg.pas:173
    public void UpdatePlayer(int Index, TAOWLabel NameLabel, TAOWEdit EmailEdit)
    {
        if (Index < FEmailAddresses.Count)
        {
            EmailEdit.CaptureInput = true;
            EmailEdit.Text = FEmailAddresses[Index];
            var objectValue = FEmailAddresses.Objects[Index];
            NameLabel.Text = TranslateRStr(RaceNamesPluralRStr[ObjectToInt(objectValue)]);
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
        var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
        LocalEmailAddressEdit.Text = localEmailAddress;
    }

    // AoWEmailGameSettingsDlg.pas:220
    public void Execute(TSetupControl Control)
    {
        TSetupControl C;
        ResetSettings();
        FSetupControl = Control;
        C = FSetupControl;
        for (int I = 0, last = C.FSettings.Count - 1; I <= last; I++)
        {
            if (C.FSettings.GetSettings(I).FBehavior.PlayerType == 0)
            {
                var setting = C.FSettings.GetSettings(I);
                var localEmailAddress = AoWEngine.FRegistry.GetLocalEmailAddress();
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
            ResetMessageDialog(AoWMessageDlgUnit.MessageDlg);
            AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add("");
            var youNeed2HumanPlayersToPlayAnEmailGame = TranslateRStr(YouNeed2HumanPlayersToPlayAnEmailGameRStr);
            AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add(youNeed2HumanPlayersToPlayAnEmailGame);
            AoWMessageDlgUnit.MessageDlg.OKPnl.Visible = true;
            AoWMessageDlgUnit.MessageDlg.Dlg.Visible = true;
        }
    }
}

public static partial class AoWEmailGameSettingsDlgUnit
{
}
