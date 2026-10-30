// Translated from AoWEmailGameSettingsDlg.pas.
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
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
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
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWEmailGameSettingsDlg
{
    // editable email address list. setup control
    public partial class TEmailGameSettingsDlg : Aow.Rtl.TDataModule
    {
        public TEmailGameSettingsDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWButton.TAOWButton OKBtn;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWButton.TAOWButton CancelBut;
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel LocalEmailAdressLbl;
        public Aow.Units.AoWImage.TAOWImage DecoL;
        public Aow.Units.AoWImage.TAOWImage DecoR;
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        public Aow.Units.AoWEdit.TAOWEdit LocalEmailAddressEdit;
        public Aow.Units.AoWLabel.TAOWLabel PlayerEMailAddressesLbl;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit1;
        public Aow.Units.AoWPanel.TAOWPanel PlayerPnl;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl1;
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl2;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl3;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit2;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit3;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl4;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit4;
        public Aow.Units.AoWLabel.TAOWLabel EmailGameNameLbl;
        public Aow.Units.AoWEdit.TAOWEdit EmailGameNameEdit;
        public Aow.Units.AoWButton.TAOWButton SettingsBtn;
        public Aow.Units.AoWPanel.TAOWPanel Player1Pnl;
        public Aow.Units.AoWPanel.TAOWPanel PowePnl;
        public Aow.Units.AoWPanel.TAOWPanel Player2Pnl;
        public Aow.Units.AoWPanel.TAOWPanel Player3Pnl;
        public Aow.Units.AoWPanel.TAOWPanel Player4Pnl;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl5;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit5;
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl6;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit6;
        public Aow.Units.AoWPanel.TAOWPanel Player5Pnl;
        public Aow.Units.AoWPanel.TAOWPanel Player6Pnl;
        public Aow.Units.AoWPanel.TAOWPanel NamePnl;
        public Aow.Units.AoWPanel.TAOWPanel LocalPnl;
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Rtl.TStringList FEmailAddresses;
        public Aow.Units.AoWSetup.TSetupControl FSetupControl;
        // AoWEmailGameSettingsDlg.pas:85
        public void EmailGameSettingsDlgCreate()
        {
            var call__1 = new Aow.Rtl.TStringList();
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
        public void EmailAddressEdit1Change(Aow.Rtl.TObject Sender)
        {
            int I = default;
            I = ((Aow.Rtl.TComponent)Sender).Tag + PlayerSB.Position;
            var call__2 = FEmailAddresses.Count;
            if (I < call__2)
            {
                FEmailAddresses[I] = ((Aow.Units.AoWEdit.TAOWEdit)Sender).Text;
            }
        }

        // AoWEmailGameSettingsDlg.pas:108
        public void OKBtnClick(Aow.Rtl.TObject Sender)
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
                            var call__5 = FSetupControl.FSettings.IndexOfRace(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__4)));
                            Index = call__5;
                            var call__6 = FEmailAddresses[I];
                            var call__7 = FSetupControl.FSettings.GetSettings(Index);
                            call__7.FEmailAddress = call__6;
                        }
                    }

                    FSetupControl.FSettings.FEmailGameName = EmailGameNameEdit.Text;
                    Aow.Platform.GameHost.DeferredSetupAction = 1;
                    Dlg.Visible = false;
                }
            }
            else
            {
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PleaseFillInAllSettingsRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__8);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
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
            Aow.Units.E3Globals.E3Globals.ShowEmailSettings(Aow.Units.E3Globals.E3Globals.EmailSettingsDlg);
        }

        // AoWEmailGameSettingsDlg.pas:146
        public void LocalEmailAddressEditUpdate()
        {
            var call__9 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            if (call__9 != LocalEmailAddressEdit.Text)
            {
                var call__10 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
                LocalEmailAddressEdit.Text = call__10;
            }
        }

        // AoWEmailGameSettingsDlg.pas:152
        public void Player6PnlDrawBack(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TRaceResource R = default;
            int I = default;
            var panel = (Aow.Units.AoWPanel.TAOWPanel)Sender;
            I = panel.Tag + PlayerSB.Position;
            var call__12 = FEmailAddresses.Count;
            if (I < call__12)
            {
                var call__13 = FEmailAddresses.Objects[I];
                var call__14 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Rtl.Rtl.ObjectToInt(call__13));
                R = call__14;
                R.AddRef();
                var call__15 = R.FImageLibrary[38];
                call__15.Show(panel.WinLeft, panel.WinTop, SD);
                R.Release();
            }
        }

        // AoWEmailGameSettingsDlg.pas:167
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }

        // AoWEmailGameSettingsDlg.pas:173
        public void UpdatePlayer(int Index, Aow.Units.AoWLabel.TAOWLabel NameLabel, Aow.Units.AoWEdit.TAOWEdit EmailEdit)
        {
            var call__16 = FEmailAddresses.Count;
            if (Index < call__16)
            {
                EmailEdit.CaptureInput = true;
                var call__17 = FEmailAddresses[Index];
                EmailEdit.Text = call__17;
                var call__18 = FEmailAddresses.Objects[Index];
                var call__19 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Aow.Rtl.Rtl.ObjectToInt(call__18)]);
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
            Result = (LocalEmailAddressEdit.Text != ((AnsiString)"")) && (EmailGameNameEdit.Text != ((AnsiString)""));
            while (Result && (I != 0))
            {
                I -= 1;
                var call__22 = FEmailAddresses[I];
                Result = call__22 != ((AnsiString)"");
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
            var call__23 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = call__23;
        }

        // AoWEmailGameSettingsDlg.pas:220
        public void Execute(Aow.Units.AoWSetup.TSetupControl Control)
        {
            int I = default;
            Aow.Units.AoWSetup.TSetupControl C = default;
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
                        var call__27 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
                        FEmailAddresses.AddObject(call__27, Aow.Rtl.Rtl.IntToObject((int)call__26.FRace));
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
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouNeed2HumanPlayersToPlayAnEmailGameRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__29);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }
    }

    public static partial class AoWEmailGameSettingsDlg
    {
    }
}
