// Generated from AoWEmailGameSettingsDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { TEmailGameSettingsDlg (instance size $E8, VMT 0040EFAC).
//     $E0 editable email address list; $E4 setup control. }
    public partial class TEmailGameSettingsDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton CancelBut;
        // { +0050 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel LocalEmailAdressLbl;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage DecoL;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage DecoR;
        // { +0060 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0064 }
        public Aow.Units.AoWEdit.TAOWEdit LocalEmailAddressEdit;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerEMailAddressesLbl;
        // { +006C }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit1;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel PlayerPnl;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl1;
        // { +0078 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlayerSB;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl2;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl3;
        // { +0084 }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit2;
        // { +0088 }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit3;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl4;
        // { +0090 }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit4;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel EmailGameNameLbl;
        // { +0098 }
        public Aow.Units.AoWEdit.TAOWEdit EmailGameNameEdit;
        // { +009C }
        public Aow.Units.AoWButton.TAOWButton SettingsBtn;
        // { +00A0 }
        public Aow.Units.AoWPanel.TAOWPanel Player1Pnl;
        // { +00A4 }
        public Aow.Units.AoWPanel.TAOWPanel PowePnl;
        // { +00A8 }
        public Aow.Units.AoWPanel.TAOWPanel Player2Pnl;
        // { +00AC }
        public Aow.Units.AoWPanel.TAOWPanel Player3Pnl;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel Player4Pnl;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl5;
        // { +00B8 }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit5;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel PlayerLbl6;
        // { +00C0 }
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit6;
        // { +00C4 }
        public Aow.Units.AoWPanel.TAOWPanel Player5Pnl;
        // { +00C8 }
        public Aow.Units.AoWPanel.TAOWPanel Player6Pnl;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel NamePnl;
        // { +00D0 }
        public Aow.Units.AoWPanel.TAOWPanel LocalPnl;
        // { +00D4 }
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        // { +00D8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00DC }
        public Aow.Rtl.TStringList FEmailAddresses;
        // { $E0 }
        public Aow.Units.AoWSetup.TSetupControl FSetupControl;
        // AoWEmailGameSettingsDlg.pas:85
        // { $E4 }
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
            /* TODO(T-shim) Platform or editor boundary AoWUnit.DeferredSetupAction */
            throw new System.NotImplementedException();
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
            var call__3 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            if (call__3 != LocalEmailAddressEdit.Text)
            {
                var call__4 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
                LocalEmailAddressEdit.Text = call__4;
            }
        }

        // AoWEmailGameSettingsDlg.pas:152
        public void Player6PnlDrawBack(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TRaceResource R = default;
            int I = default;
            {
                var w__5 = (Aow.Units.AoWPanel.TAOWPanel)Sender;
                I = w__5.Tag + PlayerSB.Position;
                var call__6 = FEmailAddresses.Count;
                if (I < call__6)
                {
                    var call__7 = FEmailAddresses.Objects[I];
                    var call__8 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Rtl.Rtl.ObjectToInt(call__7));
                    R = call__8;
                    R.AddRef();
                    var call__9 = R.FImageLibrary[38];
                    call__9.Show(w__5.WinLeft, w__5.WinTop, SD);
                    R.Release();
                }
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
            var call__10 = FEmailAddresses.Count;
            if (Index < call__10)
            {
                EmailEdit.CaptureInput = true;
                var call__11 = FEmailAddresses[Index];
                EmailEdit.Text = call__11;
                var call__12 = FEmailAddresses.Objects[Index];
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Aow.Rtl.Rtl.ObjectToInt(call__12)]);
                NameLabel.Text = call__13;
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
            var call__14 = FEmailAddresses.Count;
            PlayerSB.Max = call__14 - 1;
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
            var call__15 = FEmailAddresses.Count;
            I = call__15;
            Result = (LocalEmailAddressEdit.Text != ((AnsiString)"")) && (EmailGameNameEdit.Text != ((AnsiString)""));
            while (Result && (I != 0))
            {
                I -= 1;
                var call__16 = FEmailAddresses[I];
                Result = call__16 != ((AnsiString)"");
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
            var call__17 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            LocalEmailAddressEdit.Text = call__17;
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
                int end__18 = C.FSettings.Count - 1;
                for (; I <= end__18; I += 1)
                {
                    var call__19 = C.FSettings.GetSettings(I);
                    if (call__19.FBehavior.PlayerType == 0)
                    {
                        var call__20 = C.FSettings.GetSettings(I);
                        var call__21 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
                        FEmailAddresses.AddObject(call__21, Aow.Rtl.Rtl.IntToObject((int)call__20.FRace));
                    }
                }
            }

            var call__22 = FEmailAddresses.Count;
            if (call__22 > 1)
            {
                UpdatePlayers();
                Dlg.Visible = true;
            }
            else
            {
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouNeed2HumanPlayersToPlayAnEmailGameRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__23);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }
    }

    public static partial class AoWEmailGameSettingsDlg
    {
    }
}
