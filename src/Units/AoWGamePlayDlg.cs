// Translated from AoWGamePlayDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWReg;
using Aow.Units.EventLog;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWGamePlayDlg
{
    // pending password
    public partial class TGamePlayDlg : TDataModule
    {
        public TGamePlayDlg(TComponent owner) : base(owner)
        { }

        public TAOWWindow GamePlayDlg;
        public TAOWButton CancelBtn;
        public TAoWFrame BCeltL;
        public TAOWLabel Title;
        public TAOWButton Close;
        public TAOWImage TCeltLR;
        public TAOWImage TCeltLL;
        public TAOWImage TitleIcon;
        public TAOWCheckBox AutoSaveCB;
        public TAOWLabel asLbl;
        public TAOWCheckBox SkipIntroCB;
        public TAOWLabel siLbl;
        public TAOWTabPanel Tab1;
        public TAOWPanel Pnl1;
        public TAOWPanel Pnl2;
        public TAOWLabel GamePasswordLbl;
        public TAOWEdit PasswordEdit;
        public TAOWComboBox CombatResolveModeCB;
        public TAOWVScrollBar CombatResolveModeSB;
        public TAOWCheckBox Classic1CB;
        public TAOWLabel PopupLbl1;
        public TAOWCheckBox Classic2CB;
        public TAOWLabel PopupLbl2;
        public TAOWCheckBox Classic3CB;
        public TAOWLabel PopupLbl3;
        public TAOWCheckBox Classic4CB;
        public TAOWCheckBox Classic5CB;
        public TAOWCheckBox Classic6CB;
        public TAOWLabel WindowPopupLbl;
        public TAOWLabel ClassicLbl;
        public TAOWLabel SimultaneousLbl;
        public TAOWCheckBox Sim1CB;
        public TAOWCheckBox Sim2CB;
        public TAOWCheckBox Sim3CB;
        public TAOWCheckBox Sim4CB;
        public TAOWCheckBox Sim5CB;
        public TAOWCheckBox Sim6CB;
        public TAOWLabel PopupLbl4;
        public TAOWLabel PopupLbl5;
        public TAOWLabel PopupLbl6;
        public TAOWCheckBox Classic7CB;
        public TAOWCheckBox Sim7CB;
        public TAOWLabel PopupLbl7;
        public TAOWButton ChangePasswordBtn;
        public TAOWComboBox MovementSpeedCB;
        public TAOWButton OKBtn;
        public TAOWCheckBox Classic8CB;
        public TAOWCheckBox Sim8CB;
        public TAOWLabel PopupLbl8;
        public TAOWLabel CombLbl;
        public TAOWPanel CombFr;
        public TAOWPanel CombPnl;
        public TAOWLabel Movelbl;
        public TAOWPanel MovFr;
        public TAOWPanel BtmPnl;
        public TAOWPanel MovPnl;
        public TAOWPanel AOWPanel1;
        public TAOWImage CeltLineL;
        public TIvTranslator Translator;
        public AnsiString FNewPassword;
        // AoWGamePlayDlg.pas:200
        public void PasswordEntered(TEventLog Log)
        {
            TPasswordEventLog Password = default;
            if (((TPasswordEventLog)Log).FPressedButton == TPressedEventLogButton.pebOk)
            {
                if (((TPasswordEventLog)Log).FPassword != (AnsiString)"")
                {
                    FNewPassword = ((TPasswordEventLog)Log).FPassword;
                    Password = new TPasswordEventLog(null);
                    var call__1 = TranslateRStr(PasswordConfirmationRStr);
                    Password.FTitle = call__1;
                    var call__2 = TranslateRStr(ReenterNewPasswordRStr);
                    Password.FPrompt = call__2;
                    AoWHSMap.TriggerExecuteEventLog(Password, true, PasswordConfirmed);
                    Password.Release();
                }
                else
                {
                    PasswordEdit.Text = (AnsiString)"";
                }
            }
        }

        // AoWGamePlayDlg.pas:217
        public void PasswordConfirmed(TEventLog Log)
        {
            if (((TPasswordEventLog)Log).FPassword == FNewPassword)
            {
                var call__3 = Encode(FNewPassword, 0);
                PasswordEdit.Text = call__3;
            }
            else
            {
                ResetMessageDialog(AoWMessageDlg.AoWMessageDlg.MessageDlg);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__4 = TranslateRStr(ConfirmationFailedRStr);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__4);
                AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }

        // AoWGamePlayDlg.pas:95
        public void GamePlayDlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    CancelClick(this);
                    break;
            }
        }

        // AoWGamePlayDlg.pas:100
        public void GamePlayDlgShow(TObject Sender)
        {
            TCombatResolveMode Mode = default;
            TSet<TWindowPopupEvent> Events = default;
            var call__5 = AoWEngine.FRegistry.GetAutoSave();
            AutoSaveCB.Checked = call__5;
            var call__6 = AoWEngine.FRegistry.GetSkipIntro();
            SkipIntroCB.Checked = call__6;
            var call__7 = AoWEngine.FRegistry.GetMovementSpeed();
            MovementSpeedCB.Selected = (int)call__7;
            CombatResolveModeCB.Items.Clear();
            {
                int first__11 = (int)(TCombatResolveMode)0;
                int end__8 = (int)(TCombatResolveMode)2;
                for (int ordinal__10 = first__11; ordinal__10 <= end__8; ordinal__10++)
                {
                    Mode = (TCombatResolveMode)ordinal__10;
                    var call__9 = TranslateRStr(TCombatResolveModeRStr[(int)Mode]);
                    CombatResolveModeCB.Items.AddObject(call__9, IntToObject((int)Mode));
                }
            }

            var call__12 = AoWEngine.FRegistry.GetCombatResolveMode();
            var call__13 = CombatResolveModeCB.Items.IndexOfObject(IntToObject((int)call__12));
            CombatResolveModeCB.Selected = call__13;
            var call__14 = AoWEngine.FRegistry.GetClassicWindowPopupEvents();
            Events = call__14;
            Classic1CB.Checked = Events.Contains(TWindowPopupEvent.wpCombat);
            Classic2CB.Checked = Events.Contains(TWindowPopupEvent.wpDiplomacy);
            Classic3CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellResearched);
            Classic4CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellReady);
            Classic5CB.Checked = Events.Contains(TWindowPopupEvent.wpRebellion);
            Classic6CB.Checked = Events.Contains(TWindowPopupEvent.wpPartyDamage);
            Classic7CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroUpgrade);
            Classic8CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroJoin);
            var call__15 = AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
            Events = call__15;
            Sim1CB.Checked = Events.Contains(TWindowPopupEvent.wpCombat);
            Sim2CB.Checked = Events.Contains(TWindowPopupEvent.wpDiplomacy);
            Sim3CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellResearched);
            Sim4CB.Checked = Events.Contains(TWindowPopupEvent.wpSpellReady);
            Sim5CB.Checked = Events.Contains(TWindowPopupEvent.wpRebellion);
            Sim6CB.Checked = Events.Contains(TWindowPopupEvent.wpPartyDamage);
            Sim7CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroUpgrade);
            Sim8CB.Checked = Events.Contains(TWindowPopupEvent.wpHeroJoin);
            var call__16 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            PasswordEdit.Text = call__16.FSettings;
        }

        // AoWGamePlayDlg.pas:133
        public void ChangePasswordBtnClick()
        {
            TPasswordEventLog Log = default;
            if (AoWHSMap.FPlayMode != TPlayMode.pmTurn || AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork || AoWHSMap.FCurrentPlayer == AoWHSMap.FSeatedPlayer)
            {
                Log = new TPasswordEventLog(null);
                var call__17 = TranslateRStr(EnterNewPasswordRStr);
                Log.FPrompt = call__17;
                AoWHSMap.TriggerExecuteEventLog(Log, true, PasswordEntered);
                Log.Release();
            }
        }

        // AoWGamePlayDlg.pas:147
        public void OKBtnClick(TObject Sender)
        {
            AnsiString Error = default;
            TSet<TWindowPopupEvent> Events = default;
            General.PlayButtonSound(0);
            var call__18 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            if (call__18.FSettings != PasswordEdit.Text)
            {
                if (AoWHSMap.FConnectionType != TPlayerConnectionType.pcNetwork)
                {
                    var call__19 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    call__19.FSettings = PasswordEdit.Text;
                }
                else
                {
                    var call__20 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__21 = call__20.ChangePlayerSettings(PasswordEdit.Text, ref Error);
                    if (!call__21)
                    {
                        ResetMessageDialog(AoWMessageDlg.AoWMessageDlg.MessageDlg);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                        var call__22 = TranslateRStr(CouldntChangePasswordRStr);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__22);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
                    }
                }
            }

            AoWEngine.FRegistry.SetAutoSave(AutoSaveCB.Checked);
            AoWEngine.FRegistry.SetSkipIntro(SkipIntroCB.Checked);
            AoWEngine.FRegistry.SetMovementSpeed((TMovementSpeed)MovementSpeedCB.Selected);
            var call__23 = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
            AoWEngine.FRegistry.SetCombatResolveMode((TCombatResolveMode)ObjectToInt(call__23));
            Events = TSet<TWindowPopupEvent>.Of();
            if (Classic1CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpCombat);
            }

            if (Classic2CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpDiplomacy);
            }

            if (Classic3CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellResearched);
            }

            if (Classic4CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellReady);
            }

            if (Classic5CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpRebellion);
            }

            if (Classic6CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpPartyDamage);
            }

            if (Classic7CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroUpgrade);
            }

            if (Classic8CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroJoin);
            }

            AoWEngine.FRegistry.SetClassicWindowPopupEvents(Events);
            Events = TSet<TWindowPopupEvent>.Of();
            if (Sim1CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpCombat);
            }

            if (Sim2CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpDiplomacy);
            }

            if (Sim3CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellResearched);
            }

            if (Sim4CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpSpellReady);
            }

            if (Sim5CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpRebellion);
            }

            if (Sim6CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpPartyDamage);
            }

            if (Sim7CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroUpgrade);
            }

            if (Sim8CB.Checked)
            {
                Events = Events + TSet<TWindowPopupEvent>.Of(TWindowPopupEvent.wpHeroJoin);
            }

            AoWEngine.FRegistry.SetSimultaneousWindowPopupEvents(Events);
            GamePlayDlg.Visible = false;
        }

        // AoWGamePlayDlg.pas:194
        public void CancelClick(TObject Sender)
        {
            General.PlayButtonSound(0);
            GamePlayDlg.Visible = false;
        }
    }

    public static partial class AoWGamePlayDlg
    {
    }
}
