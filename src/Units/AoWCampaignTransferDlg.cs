// Translated from AoWCampaignTransferDlg.pas.
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
        public TCampaignTransferDlg(TComponent owner) : base(owner)
        { }

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

        // AoWCampaignTransferDlg.pas:173
        public void BookStartClick(Aow.Rtl.TObject Sender)
        {
            TransBookWin.Visible = false;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__4 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__4.FState += 1;
            Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:181
        // { draft }
        public void CancelClick()
        {
            TransBookWin.Visible = false;
            Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.ResetNextScenario();
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__5 = Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__5.FState = 1;
            Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:190
        // { draft }
        public void GoldSliderChange(Aow.Rtl.TObject Sender)
        {
            var aoWCampaignTransferSettings = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            aoWCampaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:197
        public void ManaSliderChange(Aow.Rtl.TObject Sender)
        {
            var aoWCampaignTransferSettings = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            aoWCampaignTransferSettings.SetSelectedTransferMana(ManaSlider.Position);

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
            var call__8 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FHeroes.Count;
            if (call__8 > (FLeftHero + 4))
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
            var call__9 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FItems.Count;
            if (call__9 > (FLeftItem + 4))
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
            var call__10 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings.FUnits.Count;
            if (call__10 > (FLeftUnit + 4))
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
            var call__11 = S.FHeroes.GetHero(FLeftHero + Sender.Tag);
            Obj = call__11;
            if (Obj != null)
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__13 = Aow.Rtl.Rtl.Rect((LHeroPnl.WinLeft + checkBox.WinLeft) + 1, (LHeroPnl.WinTop + checkBox.WinTop) + 2, ((LHeroPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 2, ((LHeroPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                R = call__13;

                R.Left += 1;
                Obj.ShowFace(R, SurfDesc);
                R.Left -= 1;
                var call__14 = S.FSelectedHeroes.IndexOf(FLeftHero + Sender.Tag);
                if (call__14 != -1)
                {
                    var call__15 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__15.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                }

                if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                {
                    var call__16 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__16.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                }

                var call__17 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__17.Show(R.Left + 10, R.Top + 55, SurfDesc);
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__18 = Obj.GetCampaignTransferPoints();
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__18), SurfDesc);
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
                var call__19 = S.FSelectedHeroes[FRightHero + Sender.Tag];
                var call__20 = S.FHeroes.GetHero(call__19);
                Obj = call__20;
                if (Obj != null)
                {
                    var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__22 = Aow.Rtl.Rtl.Rect((RHeroPnl.WinLeft + checkBox.WinLeft) + 1, (RHeroPnl.WinTop + checkBox.WinTop) + 2, ((RHeroPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 2, ((RHeroPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                    R = call__22;

                    R.Left += 1;
                    Obj.ShowFace(R, SurfDesc);
                    R.Left -= 1;
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__23 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__23.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__24 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__24.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__25 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__25), SurfDesc);
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
            var call__26 = S.FUnits.Count;
            if ((FLeftUnit + Sender.Tag) < call__26)
            {
                var call__27 = S.FUnits.GetUnit(FLeftUnit + Sender.Tag);
                Obj = call__27;
                if (Obj != null)
                {
                    var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__29 = Aow.Rtl.Rtl.Rect((LUnitPnl.WinLeft + checkBox.WinLeft) + 1, (LUnitPnl.WinTop + checkBox.WinTop) + 2, ((LUnitPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 2, ((LUnitPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                    R = call__29;

                    var call__30 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__30.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                    Info.Animation = Aow.Units.AoWE.TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    var call__31 = S.FSelectedUnits.IndexOf(FLeftUnit + Sender.Tag);
                    if (call__31 != -1)
                    {
                        var call__32 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                        call__32.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                    }

                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__33 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__33.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__34 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__34.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__35 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__35), SurfDesc);
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
                var call__36 = S.FSelectedUnits[FRightUnit + Sender.Tag];
                var call__37 = S.FUnits.GetUnit(call__36);
                Obj = call__37;
                if (Obj != null)
                {
                    var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__39 = Aow.Rtl.Rtl.Rect((RUnitPnl.WinLeft + checkBox.WinLeft) + 1, (RUnitPnl.WinTop + checkBox.WinTop) + 2, ((RUnitPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 2, ((RUnitPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                    R = call__39;

                    var call__40 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__40.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
                    Info.Animation = Aow.Units.AoWE.TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__41 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__41.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__42 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__42.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__43 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__43), SurfDesc);
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
            var call__44 = S.FItems.GetItem(FLeftItem + Sender.Tag);
            Obj = call__44;
            if (Obj != null)
            {
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                var call__46 = Aow.Rtl.Rtl.Rect((LItemPnl.WinLeft + checkBox.WinLeft) + 2, (LItemPnl.WinTop + checkBox.WinTop) + 2, ((LItemPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 4, ((LItemPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                R = call__46;

                var call__47 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                call__47.Show(R.Left, R.Top, SurfDesc);
                Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                var call__48 = S.FSelectedItems.IndexOf(FLeftItem + Sender.Tag);
                if (call__48 != -1)
                {
                    var call__49 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__49.ShowBlended(R.Left, R.Top, SurfDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[65]);
                }

                if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                {
                    var call__50 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__50.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                }

                var call__51 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__51.Show(R.Left + 10, R.Top + 55, SurfDesc);
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__52 = Obj.GetCampaignTransferPoints();
                Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__52), SurfDesc);
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
                var call__53 = S.FSelectedItems[FRightItem + Sender.Tag];
                var call__54 = S.FItems.GetItem(call__53);
                Obj = call__54;
                if (Obj != null)
                {
                    var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                    var call__56 = Aow.Rtl.Rtl.Rect((RItemPnl.WinLeft + checkBox.WinLeft) + 2, (RItemPnl.WinTop + checkBox.WinTop) + 2, ((RItemPnl.WinLeft + checkBox.WinLeft) + checkBox.WinWidth) - 4, ((RItemPnl.WinTop + checkBox.WinTop) + checkBox.WinHeight) - 2);
                    R = call__56;

                    var call__57 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__57.Show(R.Left, R.Top, SurfDesc);
                    Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                    if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).FLit)
                    {
                        var call__58 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__58.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[50]);
                    }

                    var call__59 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__59.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__60 = Obj.GetCampaignTransferPoints();
                    Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, Aow.Rtl.Rtl.IntToStr(call__60), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:470
        public void LHeroesSelect(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                ClearLeftSelection();
                var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
                checkBox.Checked = true;
                FLeftKind = 1;
                FLeftSelection = checkBox.Tag + FLeftHero;
            }
        }

        // AoWCampaignTransferDlg.pas:484
        public void LItemsSelect(Aow.Rtl.TObject Sender)
        {
            ClearLeftSelection();
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FLeftKind = 2;
            FLeftSelection = checkBox.Tag + FLeftItem;
        }

        // AoWCampaignTransferDlg.pas:495
        public void LUnitSelect(Aow.Rtl.TObject Sender)
        {
            ClearLeftSelection();
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FLeftKind = 3;
            FLeftSelection = checkBox.Tag + FLeftUnit;
        }

        // AoWCampaignTransferDlg.pas:506
        public void RHeroesSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 1;
            FRightSelection = checkBox.Tag + FRightHero;
        }

        // AoWCampaignTransferDlg.pas:517
        public void RItemsSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 2;
            FRightSelection = checkBox.Tag + FRightItem;
        }

        // AoWCampaignTransferDlg.pas:528
        public void RUnitsSelect(Aow.Rtl.TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 3;
            FRightSelection = checkBox.Tag + FRightUnit;
        }

        // AoWCampaignTransferDlg.pas:539
        public void AddBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            switch (FLeftKind)
            {
                case 1:
                    var call__67 = S.AddHero(FLeftSelection);
                    if (call__67)
                    {
                        LHeroPnl.FDrawn = false;
                        RHeroPnl.FDrawn = false;
                        UpdateControls();
                        ClearLeftSelection();
                    }

                    break;
                case 2:
                    var call__68 = S.AddItem(FLeftSelection);
                    if (call__68)
                    {
                        LItemPnl.FDrawn = false;
                        RItemPnl.FDrawn = false;
                        UpdateControls();
                        ClearLeftSelection();
                    }

                    break;
                case 3:
                    var call__69 = S.AddUnit(FLeftSelection);
                    if (call__69)
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
                    var call__70 = S.FSelectedHeroes[FRightSelection];
                    var call__71 = S.DeleteHero(call__70);
                    if (call__71)
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
                    var call__72 = S.FSelectedItems[FRightSelection];
                    var call__73 = S.DeleteItem(call__72);
                    if (call__73)
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
                    var call__74 = S.FSelectedUnits[FRightSelection];
                    var call__75 = S.DeleteUnit(call__74);
                    if (call__75)
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
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            bool bool__77 = !checkBox.Checked;
            if (bool__77)
            {
                var call__78 = S.FHeroes.Count;
                bool__77 = (call__78 > (checkBox.Tag + FLeftHero));
            }

            bool bool__79 = bool__77;
            if (bool__79)
            {
                var call__80 = S.FSelectedHeroes.IndexOf(checkBox.Tag + FLeftHero);
                bool__79 = (call__80 == -1);
            }

            checkBox.FCaptureInput = bool__79;
        }

        // AoWCampaignTransferDlg.pas:617
        public void LItemsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FLeftKind == 2) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FLeftItem) == FLeftSelection);
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            bool bool__82 = !checkBox.Checked;
            if (bool__82)
            {
                var call__83 = S.FItems.Count;
                bool__82 = (call__83 > (checkBox.Tag + FLeftItem));
            }

            bool bool__84 = bool__82;
            if (bool__84)
            {
                var call__85 = S.FSelectedItems.IndexOf(checkBox.Tag + FLeftItem);
                bool__84 = (call__85 == -1);
            }

            checkBox.FCaptureInput = bool__84;
        }

        // AoWCampaignTransferDlg.pas:629
        public void LUnitsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FLeftKind == 3) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FLeftUnit) == FLeftSelection);
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            bool bool__87 = !checkBox.Checked;
            if (bool__87)
            {
                var call__88 = S.FUnits.Count;
                bool__87 = (call__88 > (checkBox.Tag + FLeftUnit));
            }

            bool bool__89 = bool__87;
            if (bool__89)
            {
                var call__90 = S.FSelectedUnits.IndexOf(checkBox.Tag + FLeftUnit);
                bool__89 = (call__90 == -1);
            }

            checkBox.FCaptureInput = bool__89;
        }

        // AoWCampaignTransferDlg.pas:641
        public void RHeroesUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 1) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightHero) == FRightSelection);
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && ((checkBox.Tag + FRightHero) < S.FSelectedHeroes.Count);
        }

        // AoWCampaignTransferDlg.pas:652
        public void RItemsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 2) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightItem) == FRightSelection);
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && ((checkBox.Tag + FRightItem) < S.FSelectedItems.Count);
        }

        // AoWCampaignTransferDlg.pas:663
        public void RUnitsUpdate(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TAoWCampaignTransferSettings S = default;
            S = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FTransferSettings;
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = (FRightKind == 3) && ((((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag + FRightUnit) == FRightSelection);
            var checkBox = (Aow.Units.AoWCheckBox.TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && ((checkBox.Tag + FRightUnit) < S.FSelectedUnits.Count);
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
