// Generated from AoWCampaignTransferDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWBookWindow;
using static Aow.Units.AoWBookWindow.AoWBookWindow;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWCampaignTransferDlg
{
    // { TCampaignTransferDlg (instance size $1D0, VMT 0040C058).
//     $1A8/$1AC hero scroll; $1B0/$1B4 unit scroll; $1B8/$1BC item scroll;
//     $1C0/$1C4 left selection kind/index; $1C8/$1CC right selection kind/index. }
    public partial class TCampaignTransferDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWCheckBox.TAOWCheckBox LHero1;
        // { +0044 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LHero2;
        // { +0048 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LHero3;
        // { +004C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LHero4;
        // { +0050 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LItem1;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel LItemPnl;
        // { +0058 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LItem2;
        // { +005C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LItem3;
        // { +0060 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LItem4;
        // { +0064 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LUnit1;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel LUnitPnl;
        // { +006C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LUnit2;
        // { +0070 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LUnit3;
        // { +0074 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LUnit4;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel Available;
        // { +007C }
        public Aow.Units.AoWBookWindow.TAoWBookWindow TransBookWin;
        // { +0080 }
        public Aow.Units.AoWButton.TAOWButton BookStart;
        // { +0084 }
        public Aow.Units.AoWButton.TAOWButton Cancel;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel PnlL;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel PnlR;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel TransferPointsLbl;
        // { +0098 }
        public Aow.Units.AoWButton.TAOWButton AddBtn;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel LGoldLbl;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel GoldLeftLbl;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel LManaLbl;
        // { +00A8 }
        public Aow.Units.AoWLabel.TAOWLabel ManaLeftLbl;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel Next;
        // { +00B0 }
        public Aow.Units.AoWFrame.TAoWFrame LSep1;
        // { +00B4 }
        public Aow.Units.AoWImage.TAOWImage LHeroImgL;
        // { +00B8 }
        public Aow.Units.AoWPanel.TAOWPanel LHeroPnl;
        // { +00BC }
        public Aow.Units.AoWImage.TAOWImage LItemImgL;
        // { +00C0 }
        public Aow.Units.AoWImage.TAOWImage LUnitImgL;
        // { +00C4 }
        public Aow.Units.AoWImage.TAOWImage LHeroImgR;
        // { +00C8 }
        public Aow.Units.AoWImage.TAOWImage LItemImgR;
        // { +00CC }
        public Aow.Units.AoWImage.TAOWImage LUnitImgR;
        // { +00D0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RHero1;
        // { +00D4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RHero2;
        // { +00D8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RHero3;
        // { +00DC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RHero4;
        // { +00E0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RItem1;
        // { +00E4 }
        public Aow.Units.AoWPanel.TAOWPanel RItemPnl;
        // { +00E8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RItem2;
        // { +00EC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RItem3;
        // { +00F0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RItem4;
        // { +00F4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RUnit1;
        // { +00F8 }
        public Aow.Units.AoWPanel.TAOWPanel RUnitPnl;
        // { +00FC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RUnit2;
        // { +0100 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RUnit3;
        // { +0104 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox RUnit4;
        // { +0108 }
        public Aow.Units.AoWImage.TAOWImage RHeroImgL;
        // { +010C }
        public Aow.Units.AoWPanel.TAOWPanel RHeroPnl;
        // { +0110 }
        public Aow.Units.AoWImage.TAOWImage RItemImgL;
        // { +0114 }
        public Aow.Units.AoWImage.TAOWImage RUnitImgL;
        // { +0118 }
        public Aow.Units.AoWImage.TAOWImage RHeroImgR;
        // { +011C }
        public Aow.Units.AoWImage.TAOWImage RItemImgR;
        // { +0120 }
        public Aow.Units.AoWImage.TAOWImage RUnitImgR;
        // { +0124 }
        public Aow.Units.AoWFrame.TAoWFrame RSep1;
        // { +0128 }
        public Aow.Units.AoWButton.TAOWButton RemoveBtn;
        // { +012C }
        public Aow.Units.AoWFrame.TAoWFrame LSep2;
        // { +0130 }
        public Aow.Units.AoWFrame.TAoWFrame RSep2;
        // { +0134 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar GoldSlider;
        // { +0138 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar ManaSlider;
        // { +013C }
        public Aow.Units.AoWButton.TAOWButton LHeroLB;
        // { +0140 }
        public Aow.Units.AoWButton.TAOWButton AutoBtn;
        // { +0144 }
        public Aow.Units.AoWButton.TAOWButton LItemLB;
        // { +0148 }
        public Aow.Units.AoWButton.TAOWButton LUnitLB;
        // { +014C }
        public Aow.Units.AoWButton.TAOWButton LHeroRB;
        // { +0150 }
        public Aow.Units.AoWButton.TAOWButton LItemRB;
        // { +0154 }
        public Aow.Units.AoWButton.TAOWButton LUnitRB;
        // { +0158 }
        public Aow.Units.AoWButton.TAOWButton RHeroLB;
        // { +015C }
        public Aow.Units.AoWButton.TAOWButton RItemLB;
        // { +0160 }
        public Aow.Units.AoWButton.TAOWButton RUnitLB;
        // { +0164 }
        public Aow.Units.AoWButton.TAOWButton RHeroRB;
        // { +0168 }
        public Aow.Units.AoWButton.TAOWButton RItemRB;
        // { +016C }
        public Aow.Units.AoWButton.TAOWButton RUnitRB;
        // { +0170 }
        public Aow.Units.AoWImage.TAOWImage CostImg;
        // { +0174 }
        public Aow.Units.AoWPanel.TAOWPanel CostPnl;
        // { +0178 }
        public Aow.Units.AoWLabel.TAOWLabel CostLbl;
        // { +017C }
        public Aow.Units.AoWImage.TAOWImage LGoldImg;
        // { +0180 }
        public Aow.Units.AoWImage.TAOWImage LManaImg;
        // { +0184 }
        public Aow.Units.AoWLabel.TAOWLabel TGoldLbl;
        // { +0188 }
        public Aow.Units.AoWLabel.TAOWLabel TransferGoldLbl;
        // { +018C }
        public Aow.Units.AoWLabel.TAOWLabel TManaLbl;
        // { +0190 }
        public Aow.Units.AoWLabel.TAOWLabel TransferManaLbl;
        // { +0194 }
        public Aow.Units.AoWImage.TAOWImage RGoldImg;
        // { +0198 }
        public Aow.Units.AoWImage.TAOWImage RManaImg;
        // { +019C }
        public Aow.Units.AoWImage.TAOWImage ResBackG;
        // { +01A0 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +01A4 }
        public int FLeftHero;
        // { $1A8 }
        public int FRightHero;
        // { $1AC }
        public int FLeftUnit;
        // { $1B0 }
        public int FRightUnit;
        // { $1B4 }
        public int FLeftItem;
        // { $1B8 }
        public int FRightItem;
        // { $1BC }
        public byte FLeftKind;
        // { $1C0 }
        public Aow.Units.FixedArray_byte_3 F_CampaignTransferDlg_1C1;
        public int FLeftSelection;
        // { $1C4 }
        public byte FRightKind;
        // { $1C8 }
        public Aow.Units.FixedArray_byte_3 F_CampaignTransferDlg_1C9;
        public int FRightSelection;
        // AoWCampaignTransferDlg.pas:683
        // { $1CC }
        public void UpdateControls()
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            TransferPointsLbl.Text = (Aow.Rtl.Rtl.IntToStr(S.FTransferPointsLeft) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(S.FTransferPointsMax);
            TransferManaLbl.Text = Aow.Rtl.Rtl.IntToStr(S.FSelectedTransferMana);
            TransferGoldLbl.Text = Aow.Rtl.Rtl.IntToStr(S.FSelectedTransferGold);
            ManaLeftLbl.Text = Aow.Rtl.Rtl.IntToStr(S.FTransferManaMax - S.FSelectedTransferMana);
            GoldLeftLbl.Text = Aow.Rtl.Rtl.IntToStr(S.FTransferGoldMax - S.FSelectedTransferGold);
            ManaSlider.Position = S.FSelectedTransferMana;
            GoldSlider.Position = S.FSelectedTransferGold;
            LHeroLB.Visible = FLeftHero != 0;
            var call__1 = S.FHeroes.Count;
            LHeroRB.Visible = call__1 > (FLeftHero + 4);
            LItemLB.Visible = FLeftItem != 0;
            var call__2 = S.FItems.Count;
            LItemRB.Visible = call__2 > (FLeftItem + 4);
            LUnitLB.Visible = FLeftUnit != 0;
            var call__3 = S.FUnits.Count;
            LUnitRB.Visible = call__3 > (FLeftUnit + 4);
            RHeroLB.Visible = FRightHero != 0;
            RHeroRB.Visible = (FRightHero + 4) < S.FSelectedHeroes.Count;
            RItemLB.Visible = FRightItem != 0;
            RItemRB.Visible = (FRightItem + 4) < S.FSelectedItems.Count;
            RUnitLB.Visible = FRightUnit != 0;
            RUnitRB.Visible = (FRightUnit + 4) < S.FSelectedUnits.Count;
            TransBookWin.FDrawn = false;
        }

        // AoWCampaignTransferDlg.pas:710
        public void ResetSelection()
        {
            FLeftHero = 0;
            FRightHero = 0;
            FLeftUnit = 0;
            FRightUnit = 0;
            FLeftItem = 0;
            FRightItem = 0;
            ClearLeftSelection();
            ClearRightSelection();
        }

        // AoWCampaignTransferDlg.pas:722
        public void ClearLeftSelection()
        {
            LHero1.Checked = false;
            LHero2.Checked = false;
            LHero3.Checked = false;
            LHero4.Checked = false;
            LItem1.Checked = false;
            LItem2.Checked = false;
            LItem3.Checked = false;
            LItem4.Checked = false;
            LUnit1.Checked = false;
            LUnit2.Checked = false;
            LUnit3.Checked = false;
            LUnit4.Checked = false;
            FLeftKind = 0;
            FLeftSelection = -1;
        }

        // AoWCampaignTransferDlg.pas:740
        public void ClearRightSelection()
        {
            RHero1.Checked = false;
            RHero2.Checked = false;
            RHero3.Checked = false;
            RHero4.Checked = false;
            RItem1.Checked = false;
            RItem2.Checked = false;
            RItem3.Checked = false;
            RItem4.Checked = false;
            RUnit1.Checked = false;
            RUnit2.Checked = false;
            RUnit3.Checked = false;
            RUnit4.Checked = false;
            FRightKind = 0;
            FRightSelection = -1;
        }

        // AoWCampaignTransferDlg.pas:758
        public void Execute()
        {
            Aow.Units.AoWE.TAoWCampaign C = default;
            C = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign;
            ResetSelection();
            var call__4 = C.FScenarios.GetScenario(C.FNextScenarioID);
            C.FTransferSettings.SetupSelection((Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"\\Maps\\")) + call__4.FFileName);
            if (C.FTransferSettings.FTransferPointsMax > 0)
            {
                GoldSlider.Max = C.FTransferSettings.FTransferGoldMax;
                ManaSlider.Max = C.FTransferSettings.FTransferManaMax;
                UpdateControls();
                TransBookWin.Visible = true;
            }
            else
            {
                BookStartClick(this);
            }
        }

        // AoWCampaignTransferDlg.pas:173
        public void BookStartClick(Aow.Rtl.TObject Sender)
        {
            TransBookWin.Visible = false;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__5 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__5.FState += 1;
            Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:181
        // { draft }
        public void CancelClick()
        {
            TransBookWin.Visible = false;
            Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.ResetNextScenario();
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__6 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__6.FState = 1;
            Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:190
        // { draft }
        public void GoldSliderChange(Aow.Rtl.TObject Sender)
        {
            {
                var w__7 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
                w__7.SetSelectedTransferGold(GoldSlider.Position);
            }

            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:197
        public void ManaSliderChange(Aow.Rtl.TObject Sender)
        {
            {
                var w__8 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
                w__8.SetSelectedTransferMana(ManaSlider.Position);
            }

            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:204
        public void LHeroLBClick(Aow.Rtl.TObject Sender)
        {
            if (FLeftHero != 0)
            {
                FLeftHero -= 1;
            }

            LHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:211
        public void LHeroRBClick(Aow.Rtl.TObject Sender)
        {
            var call__9 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FHeroes.Count;
            if (call__9 > (FLeftHero + 4))
            {
                FLeftHero += 1;
            }

            LHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:218
        public void LItemLBClick(Aow.Rtl.TObject Sender)
        {
            if (FLeftItem != 0)
            {
                FLeftItem -= 1;
            }

            LItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:225
        public void LItemRBClick(Aow.Rtl.TObject Sender)
        {
            var call__10 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FItems.Count;
            if (call__10 > (FLeftItem + 4))
            {
                FLeftItem += 1;
            }

            LItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:232
        public void LUnitLBClick(Aow.Rtl.TObject Sender)
        {
            if (FLeftUnit != 0)
            {
                FLeftUnit -= 1;
            }

            LUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:239
        public void LUnitRBClick(Aow.Rtl.TObject Sender)
        {
            var call__11 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FUnits.Count;
            if (call__11 > (FLeftUnit + 4))
            {
                FLeftUnit += 1;
            }

            LUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:246
        public void RHeroLBClick(Aow.Rtl.TObject Sender)
        {
            if (FRightHero != 0)
            {
                FRightHero -= 1;
            }

            RHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:253
        public void RHeroRBClick(Aow.Rtl.TObject Sender)
        {
            if ((FRightHero + 4) < Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FSelectedHeroes.Count)
            {
                FRightHero += 1;
            }

            RHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:260
        public void RItemLBClick(Aow.Rtl.TObject Sender)
        {
            if (FRightItem != 0)
            {
                FRightItem -= 1;
            }

            RItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:267
        public void RItemRBClick(Aow.Rtl.TObject Sender)
        {
            if ((FRightItem + 4) < Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FSelectedItems.Count)
            {
                FRightItem += 1;
            }

            RItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:274
        public void RUnitLBClick(Aow.Rtl.TObject Sender)
        {
            if (FRightUnit != 0)
            {
                FRightUnit -= 1;
            }

            RUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:281
        public void RUnitRBClick(Aow.Rtl.TObject Sender)
        {
            if ((FRightUnit + 4) < Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FSelectedUnits.Count)
            {
                FRightUnit += 1;
            }

            RUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:288
        public void LHeroesDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.THero Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            var call__12 = S.FHeroes.GetHero(FLeftHero + Sender.Tag);
            Obj = call__12;
            if (Obj != null)
            {
                {
                    var w__13 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__14 = Aow.Rtl.Rtl.Rect((LHeroPnl.WinLeft + w__13.WinLeft) + 1, (LHeroPnl.WinTop + w__13.WinTop) + 2, ((LHeroPnl.WinLeft + w__13.WinLeft) + w__13.WinWidth) - 2, ((LHeroPnl.WinTop + w__13.WinTop) + w__13.WinHeight) - 2);
                    R = call__14;
                }

                R.Left += 1;
                Obj.ShowFace(R, SurfDesc);
                R.Left -= 1;
                var call__15 = S.FSelectedHeroes.IndexOf(FLeftHero + Sender.Tag);
                if (call__15 != -1)
                {
                    var call__16 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__16.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                }

                if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                {
                    var call__17 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__17.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                }

                var call__18 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__18.Show(R.Left + 10, R.Top + 55, SurfDesc);
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__19 = Obj.GetCampaignTransferPoints();
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__19), SurfDesc);
            }
        }

        // AoWCampaignTransferDlg.pas:316
        public void RHeroesDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.THero Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            if ((FRightHero + Sender.Tag) < S.FSelectedHeroes.Count)
            {
                var call__20 = S.FSelectedHeroes[FRightHero + Sender.Tag];
                var call__21 = S.FHeroes.GetHero(call__20);
                Obj = call__21;
                if (Obj != null)
                {
                    {
                        var w__22 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__23 = Aow.Rtl.Rtl.Rect((RHeroPnl.WinLeft + w__22.WinLeft) + 1, (RHeroPnl.WinTop + w__22.WinTop) + 2, ((RHeroPnl.WinLeft + w__22.WinLeft) + w__22.WinWidth) - 2, ((RHeroPnl.WinTop + w__22.WinTop) + w__22.WinHeight) - 2);
                        R = call__23;
                    }

                    R.Left += 1;
                    Obj.ShowFace(R, SurfDesc);
                    R.Left -= 1;
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__24 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__24.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__25 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__25.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__26 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__26), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:345
        public void LUnitsDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TAbstractUnit Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            var call__27 = S.FUnits.Count;
            if ((FLeftUnit + Sender.Tag) < call__27)
            {
                var call__28 = S.FUnits.GetUnit(FLeftUnit + Sender.Tag);
                Obj = call__28;
                if (Obj != null)
                {
                    {
                        var w__29 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__30 = Aow.Rtl.Rtl.Rect((LUnitPnl.WinLeft + w__29.WinLeft) + 1, (LUnitPnl.WinTop + w__29.WinTop) + 2, ((LUnitPnl.WinLeft + w__29.WinLeft) + w__29.WinWidth) - 2, ((LUnitPnl.WinTop + w__29.WinTop) + w__29.WinHeight) - 2);
                        R = call__30;
                    }

                    var call__31 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__31.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                    Info.Animation = Aow.Units.AoWE.TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    var call__32 = S.FSelectedUnits.IndexOf(FLeftUnit + Sender.Tag);
                    if (call__32 != -1)
                    {
                        var call__33 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                        call__33.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                    }

                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__34 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__34.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__35 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__35.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__36 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__36), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:381
        public void RUnitsDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TAbstractUnit Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            if ((FRightUnit + Sender.Tag) < S.FSelectedUnits.Count)
            {
                var call__37 = S.FSelectedUnits[FRightUnit + Sender.Tag];
                var call__38 = S.FUnits.GetUnit(call__37);
                Obj = call__38;
                if (Obj != null)
                {
                    {
                        var w__39 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__40 = Aow.Rtl.Rtl.Rect((RUnitPnl.WinLeft + w__39.WinLeft) + 1, (RUnitPnl.WinTop + w__39.WinTop) + 2, ((RUnitPnl.WinLeft + w__39.WinLeft) + w__39.WinWidth) - 2, ((RUnitPnl.WinTop + w__39.WinTop) + w__39.WinHeight) - 2);
                        R = call__40;
                    }

                    var call__41 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__41.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                    Info.Animation = Aow.Units.AoWE.TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__42 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__42.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__43 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__43.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__44 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__44), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:415
        public void LItemsDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TItem Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            var call__45 = S.FItems.GetItem(FLeftItem + Sender.Tag);
            Obj = call__45;
            if (Obj != null)
            {
                {
                    var w__46 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__47 = Aow.Rtl.Rtl.Rect((LItemPnl.WinLeft + w__46.WinLeft) + 2, (LItemPnl.WinTop + w__46.WinTop) + 2, ((LItemPnl.WinLeft + w__46.WinLeft) + w__46.WinWidth) - 4, ((LItemPnl.WinTop + w__46.WinTop) + w__46.WinHeight) - 2);
                    R = call__47;
                }

                var call__48 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                call__48.Show(R.Left, R.Top, SurfDesc);
                Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                var call__49 = S.FSelectedItems.IndexOf(FLeftItem + Sender.Tag);
                if (call__49 != -1)
                {
                    var call__50 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__50.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                }

                if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                {
                    var call__51 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__51.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                }

                var call__52 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__52.Show(R.Left + 10, R.Top + 55, SurfDesc);
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__53 = Obj.GetCampaignTransferPoints();
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__53), SurfDesc);
            }
        }

        // AoWCampaignTransferDlg.pas:442
        public void RItemsDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TItem Obj = default;
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            Aow.Rtl.TRect R = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            if ((FRightItem + Sender.Tag) < S.FSelectedItems.Count)
            {
                var call__54 = S.FSelectedItems[FRightItem + Sender.Tag];
                var call__55 = S.FItems.GetItem(call__54);
                Obj = call__55;
                if (Obj != null)
                {
                    {
                        var w__56 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                        var call__57 = Aow.Rtl.Rtl.Rect((RItemPnl.WinLeft + w__56.WinLeft) + 2, (RItemPnl.WinTop + w__56.WinTop) + 2, ((RItemPnl.WinLeft + w__56.WinLeft) + w__56.WinWidth) - 4, ((RItemPnl.WinTop + w__56.WinTop) + w__56.WinHeight) - 2);
                        R = call__57;
                    }

                    var call__58 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__58.Show(R.Left, R.Top, SurfDesc);
                    Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__59 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__59.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__60 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__60.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__61 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__61), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:470
        public void LHeroesSelect(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                ClearLeftSelection();
                {
                    var w__62 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    w__62.Checked = true;
                    FLeftKind = 1;
                    FLeftSelection = w__62.Tag + FLeftHero;
                }
            }
        }

        // AoWCampaignTransferDlg.pas:484
        public void LItemsSelect(Aow.Rtl.TObject Sender)
        {
            ClearLeftSelection();
            {
                var w__63 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__63.Checked = true;
                FLeftKind = 2;
                FLeftSelection = w__63.Tag + FLeftItem;
            }
        }

        // AoWCampaignTransferDlg.pas:495
        public void LUnitSelect(Aow.Rtl.TObject Sender)
        {
            ClearLeftSelection();
            {
                var w__64 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__64.Checked = true;
                FLeftKind = 3;
                FLeftSelection = w__64.Tag + FLeftUnit;
            }
        }

        // AoWCampaignTransferDlg.pas:506
        public void RHeroesSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            {
                var w__65 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__65.Checked = true;
                FRightKind = 1;
                FRightSelection = w__65.Tag + FRightHero;
            }
        }

        // AoWCampaignTransferDlg.pas:517
        public void RItemsSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            {
                var w__66 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__66.Checked = true;
                FRightKind = 2;
                FRightSelection = w__66.Tag + FRightItem;
            }
        }

        // AoWCampaignTransferDlg.pas:528
        public void RUnitsSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            {
                var w__67 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__67.Checked = true;
                FRightKind = 3;
                FRightSelection = w__67.Tag + FRightUnit;
            }
        }

        // AoWCampaignTransferDlg.pas:539
        public void AddBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            switch (FLeftKind)
            {
                case 1:
                    var call__68 = S.AddHero(FLeftSelection);
                    if (call__68)
                    {
                        LHeroPnl.FDrawn = false;
                        RHeroPnl.FDrawn = false;
                        UpdateControls();
                        ClearLeftSelection();
                    }

                    break;
                case 2:
                    var call__69 = S.AddItem(FLeftSelection);
                    if (call__69)
                    {
                        LItemPnl.FDrawn = false;
                        RItemPnl.FDrawn = false;
                        UpdateControls();
                        ClearLeftSelection();
                    }

                    break;
                case 3:
                    var call__70 = S.AddUnit(FLeftSelection);
                    if (call__70)
                    {
                        LUnitPnl.FDrawn = false;
                        RUnitPnl.FDrawn = false;
                        UpdateControls();
                        ClearLeftSelection();
                    }

                    break;
            }
        }

        // AoWCampaignTransferDlg.pas:569
        public void RemoveBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            switch (FRightKind)
            {
                case 1:
                    var call__71 = S.FSelectedHeroes[FRightSelection];
                    var call__72 = S.DeleteHero(call__71);
                    if (call__72)
                    {
                        LHeroPnl.FDrawn = false;
                        RHeroPnl.FDrawn = false;
                        PnlR.FDrawn = false;
                        UpdateControls();
                        ClearRightSelection();
                        TransBookWin.FDrawn = false;
                    }

                    break;
                case 2:
                    var call__73 = S.FSelectedItems[FRightSelection];
                    var call__74 = S.DeleteItem(call__73);
                    if (call__74)
                    {
                        LItemPnl.FDrawn = false;
                        RItemPnl.FDrawn = false;
                        PnlR.FDrawn = false;
                        UpdateControls();
                        ClearRightSelection();
                        TransBookWin.FDrawn = false;
                    }

                    break;
                case 3:
                    var call__75 = S.FSelectedUnits[FRightSelection];
                    var call__76 = S.DeleteUnit(call__75);
                    if (call__76)
                    {
                        LUnitPnl.FDrawn = false;
                        RUnitPnl.FDrawn = false;
                        PnlR.FDrawn = false;
                        UpdateControls();
                        ClearRightSelection();
                        TransBookWin.FDrawn = false;
                    }

                    break;
            }
        }

        // AoWCampaignTransferDlg.pas:605
        public void LHeroesUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FLeftKind == 1) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FLeftHero) == FLeftSelection);
            {
                var w__77 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                bool bool__78 = !w__77.Checked;
                if (bool__78)
                {
                    var call__79 = S.FHeroes.Count;
                    bool__78 = (call__79 > (w__77.Tag + FLeftHero));
                }

                bool bool__80 = bool__78;
                if (bool__80)
                {
                    var call__81 = S.FSelectedHeroes.IndexOf(w__77.Tag + FLeftHero);
                    bool__80 = (call__81 == -1);
                }

                w__77.FCaptureInput = bool__80;
            }
        }

        // AoWCampaignTransferDlg.pas:617
        public void LItemsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FLeftKind == 2) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FLeftItem) == FLeftSelection);
            {
                var w__82 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                bool bool__83 = !w__82.Checked;
                if (bool__83)
                {
                    var call__84 = S.FItems.Count;
                    bool__83 = (call__84 > (w__82.Tag + FLeftItem));
                }

                bool bool__85 = bool__83;
                if (bool__85)
                {
                    var call__86 = S.FSelectedItems.IndexOf(w__82.Tag + FLeftItem);
                    bool__85 = (call__86 == -1);
                }

                w__82.FCaptureInput = bool__85;
            }
        }

        // AoWCampaignTransferDlg.pas:629
        public void LUnitsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FLeftKind == 3) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FLeftUnit) == FLeftSelection);
            {
                var w__87 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                bool bool__88 = !w__87.Checked;
                if (bool__88)
                {
                    var call__89 = S.FUnits.Count;
                    bool__88 = (call__89 > (w__87.Tag + FLeftUnit));
                }

                bool bool__90 = bool__88;
                if (bool__90)
                {
                    var call__91 = S.FSelectedUnits.IndexOf(w__87.Tag + FLeftUnit);
                    bool__90 = (call__91 == -1);
                }

                w__87.FCaptureInput = bool__90;
            }
        }

        // AoWCampaignTransferDlg.pas:641
        public void RHeroesUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 1) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightHero) == FRightSelection);
            {
                var w__92 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__92.FCaptureInput = !w__92.Checked && ((w__92.Tag + FRightHero) < S.FSelectedHeroes.Count);
            }
        }

        // AoWCampaignTransferDlg.pas:652
        public void RItemsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 2) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightItem) == FRightSelection);
            {
                var w__93 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__93.FCaptureInput = !w__93.Checked && ((w__93.Tag + FRightItem) < S.FSelectedItems.Count);
            }
        }

        // AoWCampaignTransferDlg.pas:663
        public void RUnitsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 3) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightUnit) == FRightSelection);
            {
                var w__94 = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                w__94.FCaptureInput = !w__94.Checked && ((w__94.Tag + FRightUnit) < S.FSelectedUnits.Count);
            }
        }

        // AoWCampaignTransferDlg.pas:674
        public void AutoBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.AutoSelect();
            UpdateControls();
            TransBookWin.FDrawn = false;
        }
    }

    public static partial class AoWCampaignTransferDlg
    {
    }
}
