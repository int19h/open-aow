// Translated from AoWEmailGameSettingsDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWSetup;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Platform;

namespace Aow.Units.AoWEmailGameSettingsDlg
{
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
            var call__1 = new TStringList();
            FEmailAddresses = call__1;
        }

        // AoWEmailGameSettingsDlg.pas:90
        public void EmailGameSettingsDlgDestroy()
        {
            FEmailAddresses.Free();
        }

        // AoWEmailGameSettingsDlg.pas:95
        public void PlayerSBChange()
        {
            UpdatePlayers();
        }

        // AoWEmailGameSettingsDlg.pas:100
        public void EmailAddressEdit1Change(TObject Sender)
        {
            int I = default;
            I = ((TComponent)Sender).Tag + PlayerSB.Position;
            var call__2 = FEmailAddresses.Count;
            if (I < call__2)
            {
                FEmailAddresses[I] = ((TAOWEdit)Sender).Text;
            }
        }

        // AoWEmailGameSettingsDlg.pas:108
        public void OKBtnClick(TObject Sender)
        {
            int I = default;
            int Index = default;
            if (SettingsComplete())
            {
                if (FSetupControl != null)
                {
                    {
                        I = 0;
                        int end__3 = FEmailAddresses.Count - 1;
                        for (; I <= end__3; I += 1)
                        {
                            var call__4 = FEmailAddresses.Objects[I];
                            var call__5 = FSetupControl.FSettings.IndexOfRace((byte)ObjectToInt(call__4));
                            Index = call__5;
                            var call__6 = FEmailAddresses[I];
                            var call__7 = FSetupControl.FSettings.GetSettings(Index);
                            call__7.FEmailAddress = call__6;
                        }
                    }

                    FSetupControl.FSettings.FEmailGameName = EmailGameNameEdit.Text;
                    GameHost.DeferredSetupAction = 1;
                    Dlg.Visible = false;
                }
            }
            else
            {
                ResetMessageDialog(AoWMessageDlg.AoWMessageDlg.MessageDlg);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__8 = TranslateRStr(PleaseFillInAllSettingsRStr);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__8);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
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
            var call__9 = AoWEngine.FRegistry.GetLocalEmailAddress();
            if (call__9 != LocalEmailAddressEdit.Text)
            {
                var call__10 = AoWEngine.FRegistry.GetLocalEmailAddress();
                LocalEmailAddressEdit.Text = call__10;
            }
        }

        // AoWEmailGameSettingsDlg.pas:152
        public void Player6PnlDrawBack(TAoWComponent Sender, TSurfaceDesc SD)
        {
            TRaceResource R = default;
            int I = default;
            var panel = (TAOWPanel)Sender;
            I = panel.Tag + PlayerSB.Position;
            var call__12 = FEmailAddresses.Count;
            if (I < call__12)
            {
                var call__13 = FEmailAddresses.Objects[I];
                var call__14 = AoWHSSet.FRaceResources.GetRaceResource(ObjectToInt(call__13));
                R = call__14;
                R.AddRef();
                var call__15 = R.FImageLibrary[38];
                call__15.Show(panel.WinLeft, panel.WinTop, SD);
                R.Release();
            }
        }

        // AoWEmailGameSettingsDlg.pas:167
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }

        // AoWEmailGameSettingsDlg.pas:173
        public void UpdatePlayer(int Index, TAOWLabel NameLabel, TAOWEdit EmailEdit)
        {
            var call__16 = FEmailAddresses.Count;
            if (Index < call__16)
            {
                EmailEdit.CaptureInput = true;
                var call__17 = FEmailAddresses[Index];
                EmailEdit.Text = call__17;
                var call__18 = FEmailAddresses.Objects[Index];
                var call__19 = TranslateRStr(RaceNamesPluralRStr[ObjectToInt(call__18)]);
                NameLabel.Text = call__19;
            }
            else
            {
                EmailEdit.CaptureInput = false;
                EmailEdit.Text = (AnsiString)"";
                NameLabel.Text = (AnsiString)"";
            }
        }

        // AoWEmailGameSettingsDlg.pas:187
        public void UpdatePlayers()
        {
            PlayerSB.Min = 0;
            var call__20 = FEmailAddresses.Count;
            PlayerSB.Max = call__20 - 1;
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
            bool Result = default;
            int I = default;
            var call__21 = FEmailAddresses.Count;
            I = call__21;
            Result = LocalEmailAddressEdit.Text != (AnsiString)"" && EmailGameNameEdit.Text != (AnsiString)"";
            while (Result && I != 0)
            {
                I -= 1;
                var call__22 = FEmailAddresses[I];
                Result = call__22 != (AnsiString)"";
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
            var call__23 = AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = call__23;
        }

        // AoWEmailGameSettingsDlg.pas:220
        public void Execute(TSetupControl Control)
        {
            int I = default;
            TSetupControl C = default;
            ResetSettings();
            FSetupControl = Control;
            C = FSetupControl;
            {
                I = 0;
                int end__24 = C.FSettings.Count - 1;
                for (; I <= end__24; I += 1)
                {
                    var call__25 = C.FSettings.GetSettings(I);
                    if (call__25.FBehavior.PlayerType == 0)
                    {
                        var call__26 = C.FSettings.GetSettings(I);
                        var call__27 = AoWEngine.FRegistry.GetLocalEmailAddress();
                        FEmailAddresses.AddObject(call__27, IntToObject(call__26.FRace));
                    }
                }
            }

            var call__28 = FEmailAddresses.Count;
            if (call__28 > 1)
            {
                UpdatePlayers();
                Dlg.Visible = true;
            }
            else
            {
                ResetMessageDialog(AoWMessageDlg.AoWMessageDlg.MessageDlg);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__29 = TranslateRStr(YouNeed2HumanPlayersToPlayAnEmailGameRStr);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__29);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }
    }

    public static partial class AoWEmailGameSettingsDlg
    {
    }
}
