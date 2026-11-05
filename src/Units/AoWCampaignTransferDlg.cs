// Translated from AoWCampaignTransferDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWBookWindow;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;

namespace Aow.Units.AoWCampaignTransferDlg
{
    // hero scroll. unit scroll. item scroll. left selection kind/index. right selection kind/index
    public partial class TCampaignTransferDlg : TDataModule
    {
        public TCampaignTransferDlg(TComponent owner) : base(owner)
        { }

        public TAOWCheckBox LHero1;
        public TAOWCheckBox LHero2;
        public TAOWCheckBox LHero3;
        public TAOWCheckBox LHero4;
        public TAOWCheckBox LItem1;
        public TAOWPanel LItemPnl;
        public TAOWCheckBox LItem2;
        public TAOWCheckBox LItem3;
        public TAOWCheckBox LItem4;
        public TAOWCheckBox LUnit1;
        public TAOWPanel LUnitPnl;
        public TAOWCheckBox LUnit2;
        public TAOWCheckBox LUnit3;
        public TAOWCheckBox LUnit4;
        public TAOWLabel Available;
        public TAoWBookWindow TransBookWin;
        public TAOWButton BookStart;
        public TAOWButton Cancel;
        public TAOWPanel PnlL;
        public TAOWLabel Title;
        public TAOWPanel PnlR;
        public TAOWLabel TransferPointsLbl;
        public TAOWButton AddBtn;
        public TAOWLabel LGoldLbl;
        public TAOWLabel GoldLeftLbl;
        public TAOWLabel LManaLbl;
        public TAOWLabel ManaLeftLbl;
        public TAOWLabel Next;
        public TAoWFrame LSep1;
        public TAOWImage LHeroImgL;
        public TAOWPanel LHeroPnl;
        public TAOWImage LItemImgL;
        public TAOWImage LUnitImgL;
        public TAOWImage LHeroImgR;
        public TAOWImage LItemImgR;
        public TAOWImage LUnitImgR;
        public TAOWCheckBox RHero1;
        public TAOWCheckBox RHero2;
        public TAOWCheckBox RHero3;
        public TAOWCheckBox RHero4;
        public TAOWCheckBox RItem1;
        public TAOWPanel RItemPnl;
        public TAOWCheckBox RItem2;
        public TAOWCheckBox RItem3;
        public TAOWCheckBox RItem4;
        public TAOWCheckBox RUnit1;
        public TAOWPanel RUnitPnl;
        public TAOWCheckBox RUnit2;
        public TAOWCheckBox RUnit3;
        public TAOWCheckBox RUnit4;
        public TAOWImage RHeroImgL;
        public TAOWPanel RHeroPnl;
        public TAOWImage RItemImgL;
        public TAOWImage RUnitImgL;
        public TAOWImage RHeroImgR;
        public TAOWImage RItemImgR;
        public TAOWImage RUnitImgR;
        public TAoWFrame RSep1;
        public TAOWButton RemoveBtn;
        public TAoWFrame LSep2;
        public TAoWFrame RSep2;
        public TAOWHScrollBar GoldSlider;
        public TAOWHScrollBar ManaSlider;
        public TAOWButton LHeroLB;
        public TAOWButton AutoBtn;
        public TAOWButton LItemLB;
        public TAOWButton LUnitLB;
        public TAOWButton LHeroRB;
        public TAOWButton LItemRB;
        public TAOWButton LUnitRB;
        public TAOWButton RHeroLB;
        public TAOWButton RItemLB;
        public TAOWButton RUnitLB;
        public TAOWButton RHeroRB;
        public TAOWButton RItemRB;
        public TAOWButton RUnitRB;
        public TAOWImage CostImg;
        public TAOWPanel CostPnl;
        public TAOWLabel CostLbl;
        public TAOWImage LGoldImg;
        public TAOWImage LManaImg;
        public TAOWLabel TGoldLbl;
        public TAOWLabel TransferGoldLbl;
        public TAOWLabel TManaLbl;
        public TAOWLabel TransferManaLbl;
        public TAOWImage RGoldImg;
        public TAOWImage RManaImg;
        public TAOWImage ResBackG;
        public TIvTranslator Translator;
        public int FLeftHero;
        public int FRightHero;
        public int FLeftUnit;
        public int FRightUnit;
        public int FLeftItem;
        public int FRightItem;
        public byte FLeftKind;
        public int FLeftSelection;
        public byte FRightKind;
        public int FRightSelection;
        // AoWCampaignTransferDlg.pas:683
        public void UpdateControls()
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            TransferPointsLbl.Text = IntToStr(S.FTransferPointsLeft) + AnsiString.FromBytes(47) + IntToStr(S.FTransferPointsMax);
            TransferManaLbl.Text = IntToStr(S.FSelectedTransferMana);
            TransferGoldLbl.Text = IntToStr(S.FSelectedTransferGold);
            ManaLeftLbl.Text = IntToStr(S.FTransferManaMax - S.FSelectedTransferMana);
            GoldLeftLbl.Text = IntToStr(S.FTransferGoldMax - S.FSelectedTransferGold);
            ManaSlider.Position = S.FSelectedTransferMana;
            GoldSlider.Position = S.FSelectedTransferGold;
            LHeroLB.Visible = FLeftHero != 0;
            var call__1 = S.FHeroes.Count;
            LHeroRB.Visible = call__1 > FLeftHero + 4;
            LItemLB.Visible = FLeftItem != 0;
            var call__2 = S.FItems.Count;
            LItemRB.Visible = call__2 > FLeftItem + 4;
            LUnitLB.Visible = FLeftUnit != 0;
            var call__3 = S.FUnits.Count;
            LUnitRB.Visible = call__3 > FLeftUnit + 4;
            RHeroLB.Visible = FRightHero != 0;
            RHeroRB.Visible = FRightHero + 4 < S.FSelectedHeroes.Count;
            RItemLB.Visible = FRightItem != 0;
            RItemRB.Visible = FRightItem + 4 < S.FSelectedItems.Count;
            RUnitLB.Visible = FRightUnit != 0;
            RUnitRB.Visible = FRightUnit + 4 < S.FSelectedUnits.Count;
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
        public void BookStartClick(TObject Sender)
        {
            TransBookWin.Visible = false;
            PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__4 = AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__4.FState += 1;
            AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:181
        public void CancelClick()
        {
            TransBookWin.Visible = false;
            AoWEngine.FCampaign.ResetNextScenario();
            PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            var call__5 = AoWWorldMap.AoWWorldMap.WorldMap.GetCampaign();
            call__5.FState = 1;
            AoWWorldMap.AoWWorldMap.WorldMap.UpdateCampaignState();
        }

        // AoWCampaignTransferDlg.pas:190
        public void GoldSliderChange(TObject Sender)
        {
            var aoWCampaignTransferSettings = AoWEngine.FCampaign.FTransferSettings;
            aoWCampaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:197
        public void ManaSliderChange(TObject Sender)
        {
            var aoWCampaignTransferSettings = AoWEngine.FCampaign.FTransferSettings;
            aoWCampaignTransferSettings.SetSelectedTransferMana(ManaSlider.Position);

            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:204
        public void LHeroLBClick(TObject Sender)
        {
            if (FLeftHero != 0)
            {
                FLeftHero -= 1;
            }

            LHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:211
        public void LHeroRBClick(TObject Sender)
        {
            var call__8 = AoWEngine.FCampaign.FTransferSettings.FHeroes.Count;
            if (call__8 > FLeftHero + 4)
            {
                FLeftHero += 1;
            }

            LHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:218
        public void LItemLBClick(TObject Sender)
        {
            if (FLeftItem != 0)
            {
                FLeftItem -= 1;
            }

            LItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:225
        public void LItemRBClick(TObject Sender)
        {
            var call__9 = AoWEngine.FCampaign.FTransferSettings.FItems.Count;
            if (call__9 > FLeftItem + 4)
            {
                FLeftItem += 1;
            }

            LItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:232
        public void LUnitLBClick(TObject Sender)
        {
            if (FLeftUnit != 0)
            {
                FLeftUnit -= 1;
            }

            LUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:239
        public void LUnitRBClick(TObject Sender)
        {
            var call__10 = AoWEngine.FCampaign.FTransferSettings.FUnits.Count;
            if (call__10 > FLeftUnit + 4)
            {
                FLeftUnit += 1;
            }

            LUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:246
        public void RHeroLBClick(TObject Sender)
        {
            if (FRightHero != 0)
            {
                FRightHero -= 1;
            }

            RHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:253
        public void RHeroRBClick(TObject Sender)
        {
            if (FRightHero + 4 < AoWEngine.FCampaign.FTransferSettings.FSelectedHeroes.Count)
            {
                FRightHero += 1;
            }

            RHeroPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:260
        public void RItemLBClick(TObject Sender)
        {
            if (FRightItem != 0)
            {
                FRightItem -= 1;
            }

            RItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:267
        public void RItemRBClick(TObject Sender)
        {
            if (FRightItem + 4 < AoWEngine.FCampaign.FTransferSettings.FSelectedItems.Count)
            {
                FRightItem += 1;
            }

            RItemPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:274
        public void RUnitLBClick(TObject Sender)
        {
            if (FRightUnit != 0)
            {
                FRightUnit -= 1;
            }

            RUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:281
        public void RUnitRBClick(TObject Sender)
        {
            if (FRightUnit + 4 < AoWEngine.FCampaign.FTransferSettings.FSelectedUnits.Count)
            {
                FRightUnit += 1;
            }

            RUnitPnl.FDrawn = false;
            UpdateControls();
        }

        // AoWCampaignTransferDlg.pas:288
        public void LHeroesDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            THero Obj = default;
            TAoWCampaignTransferSettings S = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            var call__11 = S.FHeroes.GetHero(FLeftHero + Sender.Tag);
            Obj = call__11;
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__13 = Rect(LHeroPnl.WinLeft + checkBox.WinLeft + 1, LHeroPnl.WinTop + checkBox.WinTop + 2, LHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                R = call__13;

                R.Left += 1;
                Obj.ShowFace(R, SurfDesc);
                R.Left -= 1;
                var call__14 = S.FSelectedHeroes.IndexOf(FLeftHero + Sender.Tag);
                if (call__14 != -1)
                {
                    var call__15 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__15.ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
                }

                if (((TAOWCheckBox)Sender).FLit)
                {
                    var call__16 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__16.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                var call__17 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__17.Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__18 = Obj.GetCampaignTransferPoints();
                AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__18), SurfDesc);
            }
        }

        // AoWCampaignTransferDlg.pas:316
        public void RHeroesDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            THero Obj = default;
            TAoWCampaignTransferSettings S = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            if (FRightHero + Sender.Tag < S.FSelectedHeroes.Count)
            {
                var call__19 = S.FSelectedHeroes[FRightHero + Sender.Tag];
                var call__20 = S.FHeroes.GetHero(call__19);
                Obj = call__20;
                if (Obj != null)
                {
                    var checkBox = (TAOWCheckBox)Sender;
                    var call__22 = Rect(RHeroPnl.WinLeft + checkBox.WinLeft + 1, RHeroPnl.WinTop + checkBox.WinTop + 2, RHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                    R = call__22;

                    R.Left += 1;
                    Obj.ShowFace(R, SurfDesc);
                    R.Left -= 1;
                    if (((TAOWCheckBox)Sender).FLit)
                    {
                        var call__23 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__23.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                    }

                    var call__24 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__24.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__25 = Obj.GetCampaignTransferPoints();
                    AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__25), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:345
        public void LUnitsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TAbstractUnit Obj = default;
            TAoWCampaignTransferSettings S = default;
            TP2UnitShowInfo Info = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            var call__26 = S.FUnits.Count;
            if (FLeftUnit + Sender.Tag < call__26)
            {
                var call__27 = S.FUnits.GetUnit(FLeftUnit + Sender.Tag);
                Obj = call__27;
                if (Obj != null)
                {
                    var checkBox = (TAOWCheckBox)Sender;
                    var call__29 = Rect(LUnitPnl.WinLeft + checkBox.WinLeft + 1, LUnitPnl.WinTop + checkBox.WinTop + 2, LUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                    R = call__29;

                    var call__30 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__30.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
                    Info.Animation = TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    var call__31 = S.FSelectedUnits.IndexOf(FLeftUnit + Sender.Tag);
                    if (call__31 != -1)
                    {
                        var call__32 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                        call__32.ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
                    }

                    if (((TAOWCheckBox)Sender).FLit)
                    {
                        var call__33 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__33.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                    }

                    var call__34 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__34.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__35 = Obj.GetCampaignTransferPoints();
                    AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__35), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:381
        public void RUnitsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TAbstractUnit Obj = default;
            TAoWCampaignTransferSettings S = default;
            TP2UnitShowInfo Info = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            if (FRightUnit + Sender.Tag < S.FSelectedUnits.Count)
            {
                var call__36 = S.FSelectedUnits[FRightUnit + Sender.Tag];
                var call__37 = S.FUnits.GetUnit(call__36);
                Obj = call__37;
                if (Obj != null)
                {
                    var checkBox = (TAOWCheckBox)Sender;
                    var call__39 = Rect(RUnitPnl.WinLeft + checkBox.WinLeft + 1, RUnitPnl.WinTop + checkBox.WinTop + 2, RUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                    R = call__39;

                    var call__40 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__40.Show(R.Left + 1, R.Top, SurfDesc);
                    Info.Direction = 3;
                    Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
                    Info.Animation = TUnitAnimation.uaStandStill;
                    Info.Frame = 0;
                    Info.Looped = false;
                    Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                    if (((TAOWCheckBox)Sender).FLit)
                    {
                        var call__41 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__41.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                    }

                    var call__42 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__42.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__43 = Obj.GetCampaignTransferPoints();
                    AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__43), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:415
        public void LItemsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TItem Obj = default;
            TAoWCampaignTransferSettings S = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            var call__44 = S.FItems.GetItem(FLeftItem + Sender.Tag);
            Obj = call__44;
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__46 = Rect(LItemPnl.WinLeft + checkBox.WinLeft + 2, LItemPnl.WinTop + checkBox.WinTop + 2, LItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, LItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                R = call__46;

                var call__47 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                call__47.Show(R.Left, R.Top, SurfDesc);
                Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                var call__48 = S.FSelectedItems.IndexOf(FLeftItem + Sender.Tag);
                if (call__48 != -1)
                {
                    var call__49 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__49.ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
                }

                if (((TAOWCheckBox)Sender).FLit)
                {
                    var call__50 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                    call__50.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                var call__51 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                call__51.Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                var call__52 = Obj.GetCampaignTransferPoints();
                AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__52), SurfDesc);
            }
        }

        // AoWCampaignTransferDlg.pas:442
        public void RItemsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TItem Obj = default;
            TAoWCampaignTransferSettings S = default;
            TRect R = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            if (FRightItem + Sender.Tag < S.FSelectedItems.Count)
            {
                var call__53 = S.FSelectedItems[FRightItem + Sender.Tag];
                var call__54 = S.FItems.GetItem(call__53);
                Obj = call__54;
                if (Obj != null)
                {
                    var checkBox = (TAOWCheckBox)Sender;
                    var call__56 = Rect(RItemPnl.WinLeft + checkBox.WinLeft + 2, RItemPnl.WinTop + checkBox.WinTop + 2, RItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, RItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);
                    R = call__56;

                    var call__57 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__57.Show(R.Left, R.Top, SurfDesc);
                    Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                    if (((TAOWCheckBox)Sender).FLit)
                    {
                        var call__58 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[20];
                        call__58.ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                    }

                    var call__59 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans[31];
                    call__59.Show(R.Left + 10, R.Top + 55, SurfDesc);
                    AoWExeForms.AoWExeForms.FontModule.Age8.Color = 16777215;
                    var call__60 = Obj.GetCampaignTransferPoints();
                    AoWExeForms.AoWExeForms.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(call__60), SurfDesc);
                }
            }
        }

        // AoWCampaignTransferDlg.pas:470
        public void LHeroesSelect(TObject Sender)
        {
            if (((TAOWCheckBox)Sender).Checked)
            {
                ClearLeftSelection();
                var checkBox = (TAOWCheckBox)Sender;
                checkBox.Checked = true;
                FLeftKind = 1;
                FLeftSelection = checkBox.Tag + FLeftHero;
            }
        }

        // AoWCampaignTransferDlg.pas:484
        public void LItemsSelect(TObject Sender)
        {
            ClearLeftSelection();
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FLeftKind = 2;
            FLeftSelection = checkBox.Tag + FLeftItem;
        }

        // AoWCampaignTransferDlg.pas:495
        public void LUnitSelect(TObject Sender)
        {
            ClearLeftSelection();
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FLeftKind = 3;
            FLeftSelection = checkBox.Tag + FLeftUnit;
        }

        // AoWCampaignTransferDlg.pas:506
        public void RHeroesSelect(TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 1;
            FRightSelection = checkBox.Tag + FRightHero;
        }

        // AoWCampaignTransferDlg.pas:517
        public void RItemsSelect(TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 2;
            FRightSelection = checkBox.Tag + FRightItem;
        }

        // AoWCampaignTransferDlg.pas:528
        public void RUnitsSelect(TObject Sender)
        {
            ClearRightSelection();
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.Checked = true;
            FRightKind = 3;
            FRightSelection = checkBox.Tag + FRightUnit;
        }

        // AoWCampaignTransferDlg.pas:539
        public void AddBtnClick(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
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
        public void RemoveBtnClick(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
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
        public void LHeroesUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FLeftKind == 1 && ((TAOWCheckBox)Sender).Tag + FLeftHero == FLeftSelection;
            var checkBox = (TAOWCheckBox)Sender;
            bool bool__77 = !checkBox.Checked;
            if (bool__77)
            {
                var call__78 = S.FHeroes.Count;
                bool__77 = call__78 > checkBox.Tag + FLeftHero;
            }

            bool bool__79 = bool__77;
            if (bool__79)
            {
                var call__80 = S.FSelectedHeroes.IndexOf(checkBox.Tag + FLeftHero);
                bool__79 = call__80 == -1;
            }

            checkBox.FCaptureInput = bool__79;
        }

        // AoWCampaignTransferDlg.pas:617
        public void LItemsUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FLeftKind == 2 && ((TAOWCheckBox)Sender).Tag + FLeftItem == FLeftSelection;
            var checkBox = (TAOWCheckBox)Sender;
            bool bool__82 = !checkBox.Checked;
            if (bool__82)
            {
                var call__83 = S.FItems.Count;
                bool__82 = call__83 > checkBox.Tag + FLeftItem;
            }

            bool bool__84 = bool__82;
            if (bool__84)
            {
                var call__85 = S.FSelectedItems.IndexOf(checkBox.Tag + FLeftItem);
                bool__84 = call__85 == -1;
            }

            checkBox.FCaptureInput = bool__84;
        }

        // AoWCampaignTransferDlg.pas:629
        public void LUnitsUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FLeftKind == 3 && ((TAOWCheckBox)Sender).Tag + FLeftUnit == FLeftSelection;
            var checkBox = (TAOWCheckBox)Sender;
            bool bool__87 = !checkBox.Checked;
            if (bool__87)
            {
                var call__88 = S.FUnits.Count;
                bool__87 = call__88 > checkBox.Tag + FLeftUnit;
            }

            bool bool__89 = bool__87;
            if (bool__89)
            {
                var call__90 = S.FSelectedUnits.IndexOf(checkBox.Tag + FLeftUnit);
                bool__89 = call__90 == -1;
            }

            checkBox.FCaptureInput = bool__89;
        }

        // AoWCampaignTransferDlg.pas:641
        public void RHeroesUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FRightKind == 1 && ((TAOWCheckBox)Sender).Tag + FRightHero == FRightSelection;
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && checkBox.Tag + FRightHero < S.FSelectedHeroes.Count;
        }

        // AoWCampaignTransferDlg.pas:652
        public void RItemsUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FRightKind == 2 && ((TAOWCheckBox)Sender).Tag + FRightItem == FRightSelection;
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && checkBox.Tag + FRightItem < S.FSelectedItems.Count;
        }

        // AoWCampaignTransferDlg.pas:663
        public void RUnitsUpdate(TObject Sender)
        {
            TAoWCampaignTransferSettings S = default;
            S = AoWEngine.FCampaign.FTransferSettings;
            ((TAOWCheckBox)Sender).Checked = FRightKind == 3 && ((TAOWCheckBox)Sender).Tag + FRightUnit == FRightSelection;
            var checkBox = (TAOWCheckBox)Sender;
            checkBox.FCaptureInput = !checkBox.Checked && checkBox.Tag + FRightUnit < S.FSelectedUnits.Count;
        }

        // AoWCampaignTransferDlg.pas:674
        public void AutoBtnClick(TObject Sender)
        {
            AoWEngine.FCampaign.FTransferSettings.AutoSelect();
            UpdateControls();
            TransBookWin.FDrawn = false;
        }
    }

    public static partial class AoWCampaignTransferDlg
    {
    }
}
