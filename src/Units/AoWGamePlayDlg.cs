// Translated from AoWGamePlayDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWGamePlayDlg
{
    // { TGamePlayDlg (size $134, VMT 0042200C): $130 pending password. }
    public partial class TGamePlayDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow GamePlayDlg;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage TCeltLR;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage TCeltLL;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0060 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox AutoSaveCB;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel asLbl;
        // { +0068 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox SkipIntroCB;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel siLbl;
        // { +0070 }
        public Aow.Units.AoWTabPanel.TAOWTabPanel Tab1;
        // { +0074 }
        public Aow.Units.AoWPanel.TAOWPanel Pnl1;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel Pnl2;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel GamePasswordLbl;
        // { +0080 }
        public Aow.Units.AoWEdit.TAOWEdit PasswordEdit;
        // { +0084 }
        public Aow.Units.AoWComboBox.TAOWComboBox CombatResolveModeCB;
        // { +0088 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar CombatResolveModeSB;
        // { +008C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic1CB;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl1;
        // { +0094 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic2CB;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl2;
        // { +009C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic3CB;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl3;
        // { +00A4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic4CB;
        // { +00A8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic5CB;
        // { +00AC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic6CB;
        // { +00B0 }
        public Aow.Units.AoWLabel.TAOWLabel WindowPopupLbl;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel ClassicLbl;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel SimultaneousLbl;
        // { +00BC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim1CB;
        // { +00C0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim2CB;
        // { +00C4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim3CB;
        // { +00C8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim4CB;
        // { +00CC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim5CB;
        // { +00D0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim6CB;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl4;
        // { +00D8 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl5;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl6;
        // { +00E0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic7CB;
        // { +00E4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim7CB;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl7;
        // { +00EC }
        public Aow.Units.AoWButton.TAOWButton ChangePasswordBtn;
        // { +00F0 }
        public Aow.Units.AoWComboBox.TAOWComboBox MovementSpeedCB;
        // { +00F4 }
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +00F8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Classic8CB;
        // { +00FC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Sim8CB;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel PopupLbl8;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel CombLbl;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel CombFr;
        // { +010C }
        public Aow.Units.AoWPanel.TAOWPanel CombPnl;
        // { +0110 }
        public Aow.Units.AoWLabel.TAOWLabel Movelbl;
        // { +0114 }
        public Aow.Units.AoWPanel.TAOWPanel MovFr;
        // { +0118 }
        public Aow.Units.AoWPanel.TAOWPanel BtmPnl;
        // { +011C }
        public Aow.Units.AoWPanel.TAOWPanel MovPnl;
        // { +0120 }
        public Aow.Units.AoWPanel.TAOWPanel AOWPanel1;
        // { +0124 }
        public Aow.Units.AoWImage.TAOWImage CeltLineL;
        // { +0128 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +012C }
        public Aow.Rtl.AnsiString FNewPassword;
        // AoWGamePlayDlg.pas:200
        // { $130 }
        public void PasswordEntered(Aow.Units.EventLog.TEventLog Log)
        {
            Aow.Units.EventLog.TPasswordEventLog Password = default;
            if (((Aow.Units.EventLog.TPasswordEventLog)Log).FPressedButton == Aow.Units.EventLog.TPressedEventLogButton.pebOk)
            {
                if (((Aow.Units.EventLog.TPasswordEventLog)Log).FPassword != ((AnsiString)""))
                {
                    FNewPassword = ((Aow.Units.EventLog.TPasswordEventLog)Log).FPassword;
                    Password = Aow.Units.EventLog.TPasswordEventLog.Create(null);
                    var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PasswordConfirmationRStr);
                    Password.FTitle = call__1;
                    var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ReenterNewPasswordRStr);
                    Password.FPrompt = call__2;
                    Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Password, true, PasswordConfirmed);
                    Password.Release();
                }
                else
                {
                    PasswordEdit.Text = (AnsiString)"";
                }
            }
        }

        // AoWGamePlayDlg.pas:217
        public void PasswordConfirmed(Aow.Units.EventLog.TEventLog Log)
        {
            if (((Aow.Units.EventLog.TPasswordEventLog)Log).FPassword == FNewPassword)
            {
                var call__3 = Aow.Units.AoWE.AoWE.Encode(FNewPassword, 0);
                PasswordEdit.Text = call__3;
            }
            else
            {
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__4 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConfirmationFailedRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__4);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }

        // AoWGamePlayDlg.pas:95
        public void GamePlayDlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    CancelClick(this);
                    break;
            }
        }

        // AoWGamePlayDlg.pas:100
        public void GamePlayDlgShow(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWReg.TCombatResolveMode Mode = default;
            TSet<Aow.Units.AoWReg.TWindowPopupEvent> Events = default;
            var call__5 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetAutoSave();
            AutoSaveCB.Checked = call__5;
            var call__6 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSkipIntro();
            SkipIntroCB.Checked = call__6;
            var call__7 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetMovementSpeed();
            MovementSpeedCB.Selected = (int)call__7;
            CombatResolveModeCB.Items.Clear();
            {
                int first__11 = (int)((Aow.Units.AoWReg.TCombatResolveMode)0);
                int end__8 = (int)((Aow.Units.AoWReg.TCombatResolveMode)2);
                for (int ordinal__10 = first__11; ordinal__10 <= end__8; ordinal__10++)
                {
                    Mode = (Aow.Units.AoWReg.TCombatResolveMode)ordinal__10;
                    var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TCombatResolveModeRStr[(int)Mode]);
                    CombatResolveModeCB.Items.AddObject(call__9, Aow.Rtl.Rtl.IntToObject((int)Mode));
                }
            }

            var call__12 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetCombatResolveMode();
            var call__13 = CombatResolveModeCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)call__12));
            CombatResolveModeCB.Selected = call__13;
            var call__14 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
            Events = call__14;
            Classic1CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpCombat);
            Classic2CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpDiplomacy);
            Classic3CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellResearched);
            Classic4CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellReady);
            Classic5CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
            Classic6CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpPartyDamage);
            Classic7CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroUpgrade);
            Classic8CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroJoin);
            var call__15 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
            Events = call__15;
            Sim1CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpCombat);
            Sim2CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpDiplomacy);
            Sim3CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellResearched);
            Sim4CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellReady);
            Sim5CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
            Sim6CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpPartyDamage);
            Sim7CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroUpgrade);
            Sim8CB.Checked = Events.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroJoin);
            var call__16 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            PasswordEdit.Text = call__16.FSettings;
        }

        // AoWGamePlayDlg.pas:133
        public void ChangePasswordBtnClick()
        {
            Aow.Units.EventLog.TPasswordEventLog Log = default;
            if (((Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode != Aow.Units.AoWE.TPlayMode.pmTurn) || (Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType != Aow.Units.AoWE.TPlayerConnectionType.pcNetwork)) || (Aow.Units.AoWE.AoWE.AoWHSMap.FCurrentPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
            {
                Log = Aow.Units.EventLog.TPasswordEventLog.Create(null);
                var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterNewPasswordRStr);
                Log.FPrompt = call__17;
                Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Log, true, PasswordEntered);
                Log.Release();
            }
        }

        // AoWGamePlayDlg.pas:147
        public void OKBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Error = default;
            TSet<Aow.Units.AoWReg.TWindowPopupEvent> Events = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (call__18.FSettings != PasswordEdit.Text)
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FConnectionType != Aow.Units.AoWE.TPlayerConnectionType.pcNetwork)
                {
                    var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    call__19.FSettings = PasswordEdit.Text;
                }
                else
                {
                    var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__21 = call__20.ChangePlayerSettings(PasswordEdit.Text, ref Error);
                    if (!call__21)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                        var call__22 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CouldntChangePasswordRStr);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__22);
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                        Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
                    }
                }
            }

            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetAutoSave(AutoSaveCB.Checked);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetSkipIntro(SkipIntroCB.Checked);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetMovementSpeed((Aow.Units.AoWReg.TMovementSpeed)MovementSpeedCB.Selected);
            var call__23 = CombatResolveModeCB.Items.Objects[CombatResolveModeCB.Selected];
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetCombatResolveMode((Aow.Units.AoWReg.TCombatResolveMode)Aow.Rtl.Rtl.ObjectToInt(call__23));
            Events = TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of();
            if (Classic1CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpCombat);
            }

            if (Classic2CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpDiplomacy);
            }

            if (Classic3CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellResearched);
            }

            if (Classic4CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellReady);
            }

            if (Classic5CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
            }

            if (Classic6CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpPartyDamage);
            }

            if (Classic7CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroUpgrade);
            }

            if (Classic8CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroJoin);
            }

            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetClassicWindowPopupEvents(Events);
            Events = TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of();
            if (Sim1CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpCombat);
            }

            if (Sim2CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpDiplomacy);
            }

            if (Sim3CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellResearched);
            }

            if (Sim4CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpSpellReady);
            }

            if (Sim5CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
            }

            if (Sim6CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpPartyDamage);
            }

            if (Sim7CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroUpgrade);
            }

            if (Sim8CB.Checked)
            {
                Events = Events + TSet<Aow.Units.AoWReg.TWindowPopupEvent>.Of(Aow.Units.AoWReg.TWindowPopupEvent.wpHeroJoin);
            }

            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetSimultaneousWindowPopupEvents(Events);
            GamePlayDlg.Visible = false;
        }

        // AoWGamePlayDlg.pas:194
        public void CancelClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            GamePlayDlg.Visible = false;
        }
    }

    public static partial class AoWGamePlayDlg
    {
    }
}
