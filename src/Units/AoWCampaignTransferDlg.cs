// Translated from AoWCampaignTransferDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.Sound.SoundUnit;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AoWBookWindow;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWorldMap;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWExeForms;

namespace Aow.Units.AoWCampaignTransferDlg;

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
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        TransferPointsLbl.Text = IntToStr(S.FTransferPointsLeft) + "/" + IntToStr(S.FTransferPointsMax);
        TransferManaLbl.Text = IntToStr(S.FSelectedTransferMana);
        TransferGoldLbl.Text = IntToStr(S.FSelectedTransferGold);
        ManaLeftLbl.Text = IntToStr(S.FTransferManaMax - S.FSelectedTransferMana);
        GoldLeftLbl.Text = IntToStr(S.FTransferGoldMax - S.FSelectedTransferGold);
        ManaSlider.Position = S.FSelectedTransferMana;
        GoldSlider.Position = S.FSelectedTransferGold;
        LHeroLB.Visible = FLeftHero != 0;
        var count = S.FHeroes.Count;
        LHeroRB.Visible = count > FLeftHero + 4;
        LItemLB.Visible = FLeftItem != 0;
        var fItemsCount = S.FItems.Count;
        LItemRB.Visible = fItemsCount > FLeftItem + 4;
        LUnitLB.Visible = FLeftUnit != 0;
        var fUnitsCount = S.FUnits.Count;
        LUnitRB.Visible = fUnitsCount > FLeftUnit + 4;
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
        PlaySample("Sfx\\Int\\BookClse.wav");
        var campaign = AoWWorldMapUnit.WorldMap.GetCampaign();
        campaign.FState += 1;
        AoWWorldMapUnit.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:181
    public void CancelClick()
    {
        TransBookWin.Visible = false;
        AoWEngine.FCampaign.ResetNextScenario();
        PlaySample("Sfx\\Int\\BookClse.wav");
        AoWWorldMapUnit.WorldMap.GetCampaign().FState = 1;
        AoWWorldMapUnit.WorldMap.UpdateCampaignState();
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
        if (AoWEngine.FCampaign.FTransferSettings.FHeroes.Count > FLeftHero + 4)
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
        if (AoWEngine.FCampaign.FTransferSettings.FItems.Count > FLeftItem + 4)
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
        if (AoWEngine.FCampaign.FTransferSettings.FUnits.Count > FLeftUnit + 4)
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
        THero Obj;
        TAoWCampaignTransferSettings S;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        Obj = S.FHeroes.GetHero(FLeftHero + Sender.Tag);
        if (Obj != null)
        {
            var checkBox = (TAOWCheckBox)Sender;
            R = Rect(LHeroPnl.WinLeft + checkBox.WinLeft + 1, LHeroPnl.WinTop + checkBox.WinTop + 2, LHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            R.Left += 1;
            Obj.ShowFace(R, SurfDesc);
            R.Left -= 1;
            if (S.FSelectedHeroes.IndexOf(FLeftHero + Sender.Tag) != -1)
            {
                aowIntGfxModUnit.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
            }

            if (((TAOWCheckBox)Sender).FLit)
            {
                aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
            }

            aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
            AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
            var campaignTransferPoints = Obj.GetCampaignTransferPoints();
            AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
        }
    }

    // AoWCampaignTransferDlg.pas:316
    public void RHeroesDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        THero Obj;
        TAoWCampaignTransferSettings S;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        if (FRightHero + Sender.Tag < S.FSelectedHeroes.Count)
        {
            var selectedHeroe = S.FSelectedHeroes[FRightHero + Sender.Tag];
            Obj = S.FHeroes.GetHero(selectedHeroe);
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                R = Rect(RHeroPnl.WinLeft + checkBox.WinLeft + 1, RHeroPnl.WinTop + checkBox.WinTop + 2, RHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                R.Left += 1;
                Obj.ShowFace(R, SurfDesc);
                R.Left -= 1;
                if (((TAOWCheckBox)Sender).FLit)
                {
                    aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:345
    public void LUnitsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TAbstractUnit Obj;
        TAoWCampaignTransferSettings S;
        TP2UnitShowInfo Info = default;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        var count = S.FUnits.Count;
        if (FLeftUnit + Sender.Tag < count)
        {
            Obj = S.FUnits.GetUnit(FLeftUnit + Sender.Tag);
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                R = Rect(LUnitPnl.WinLeft + checkBox.WinLeft + 1, LUnitPnl.WinTop + checkBox.WinTop + 2, LUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left + 1, R.Top, SurfDesc);
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
                Info.Animation = TUnitAnimation.uaStandStill;
                Info.Frame = 0;
                Info.Looped = false;
                Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                if (S.FSelectedUnits.IndexOf(FLeftUnit + Sender.Tag) != -1)
                {
                    aowIntGfxModUnit.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
                }

                if (((TAOWCheckBox)Sender).FLit)
                {
                    aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:381
    public void RUnitsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TAbstractUnit Obj;
        TAoWCampaignTransferSettings S;
        TP2UnitShowInfo Info = default;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        if (FRightUnit + Sender.Tag < S.FSelectedUnits.Count)
        {
            var selectedUnit = S.FSelectedUnits[FRightUnit + Sender.Tag];
            Obj = S.FUnits.GetUnit(selectedUnit);
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                R = Rect(RUnitPnl.WinLeft + checkBox.WinLeft + 1, RUnitPnl.WinTop + checkBox.WinTop + 2, RUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left + 1, R.Top, SurfDesc);
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
                Info.Animation = TUnitAnimation.uaStandStill;
                Info.Frame = 0;
                Info.Looped = false;
                Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                if (((TAOWCheckBox)Sender).FLit)
                {
                    aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:415
    public void LItemsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TItem Obj;
        TAoWCampaignTransferSettings S;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        Obj = S.FItems.GetItem(FLeftItem + Sender.Tag);
        if (Obj != null)
        {
            var checkBox = (TAOWCheckBox)Sender;
            R = Rect(LItemPnl.WinLeft + checkBox.WinLeft + 2, LItemPnl.WinTop + checkBox.WinTop + 2, LItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, LItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left, R.Top, SurfDesc);
            Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
            if (S.FSelectedItems.IndexOf(FLeftItem + Sender.Tag) != -1)
            {
                aowIntGfxModUnit.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, AlphaBlendTable.LookupTables[65]);
            }

            if (((TAOWCheckBox)Sender).FLit)
            {
                aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
            }

            aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
            AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
            var campaignTransferPoints = Obj.GetCampaignTransferPoints();
            AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
        }
    }

    // AoWCampaignTransferDlg.pas:442
    public void RItemsDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TItem Obj;
        TAoWCampaignTransferSettings S;
        TRect R;
        S = AoWEngine.FCampaign.FTransferSettings;
        if (FRightItem + Sender.Tag < S.FSelectedItems.Count)
        {
            var selectedItem = S.FSelectedItems[FRightItem + Sender.Tag];
            Obj = S.FItems.GetItem(selectedItem);
            if (Obj != null)
            {
                var checkBox = (TAOWCheckBox)Sender;
                R = Rect(RItemPnl.WinLeft + checkBox.WinLeft + 2, RItemPnl.WinTop + checkBox.WinTop + 2, RItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, RItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left, R.Top, SurfDesc);
                Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                if (((TAOWCheckBox)Sender).FLit)
                {
                    aowIntGfxModUnit.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, IntensityBlendTable.LookupTables[50]);
                }

                aowIntGfxModUnit.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                AoWExeFormsUnit.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                AoWExeFormsUnit.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
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
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        switch (FLeftKind)
        {
            case 1:
                if (S.AddHero(FLeftSelection))
                {
                    LHeroPnl.FDrawn = false;
                    RHeroPnl.FDrawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 2:
                if (S.AddItem(FLeftSelection))
                {
                    LItemPnl.FDrawn = false;
                    RItemPnl.FDrawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 3:
                if (S.AddUnit(FLeftSelection))
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
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        switch (FRightKind)
        {
            case 1:
                if (S.DeleteHero(S.FSelectedHeroes[FRightSelection]))
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
                if (S.DeleteItem(S.FSelectedItems[FRightSelection]))
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
                if (S.DeleteUnit(S.FSelectedUnits[FRightSelection]))
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
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        ((TAOWCheckBox)Sender).Checked = FLeftKind == 1 && ((TAOWCheckBox)Sender).Tag + FLeftHero == FLeftSelection;
        var checkBox = (TAOWCheckBox)Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FHeroes.Count > checkBox.Tag + FLeftHero && S.FSelectedHeroes.IndexOf(checkBox.Tag + FLeftHero) == -1;
    }

    // AoWCampaignTransferDlg.pas:617
    public void LItemsUpdate(TObject Sender)
    {
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        ((TAOWCheckBox)Sender).Checked = FLeftKind == 2 && ((TAOWCheckBox)Sender).Tag + FLeftItem == FLeftSelection;
        var checkBox = (TAOWCheckBox)Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FItems.Count > checkBox.Tag + FLeftItem && S.FSelectedItems.IndexOf(checkBox.Tag + FLeftItem) == -1;
    }

    // AoWCampaignTransferDlg.pas:629
    public void LUnitsUpdate(TObject Sender)
    {
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        ((TAOWCheckBox)Sender).Checked = FLeftKind == 3 && ((TAOWCheckBox)Sender).Tag + FLeftUnit == FLeftSelection;
        var checkBox = (TAOWCheckBox)Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FUnits.Count > checkBox.Tag + FLeftUnit && S.FSelectedUnits.IndexOf(checkBox.Tag + FLeftUnit) == -1;
    }

    // AoWCampaignTransferDlg.pas:641
    public void RHeroesUpdate(TObject Sender)
    {
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        ((TAOWCheckBox)Sender).Checked = FRightKind == 1 && ((TAOWCheckBox)Sender).Tag + FRightHero == FRightSelection;
        var checkBox = (TAOWCheckBox)Sender;
        checkBox.FCaptureInput = !checkBox.Checked && checkBox.Tag + FRightHero < S.FSelectedHeroes.Count;
    }

    // AoWCampaignTransferDlg.pas:652
    public void RItemsUpdate(TObject Sender)
    {
        TAoWCampaignTransferSettings S;
        S = AoWEngine.FCampaign.FTransferSettings;
        ((TAOWCheckBox)Sender).Checked = FRightKind == 2 && ((TAOWCheckBox)Sender).Tag + FRightItem == FRightSelection;
        var checkBox = (TAOWCheckBox)Sender;
        checkBox.FCaptureInput = !checkBox.Checked && checkBox.Tag + FRightItem < S.FSelectedItems.Count;
    }

    // AoWCampaignTransferDlg.pas:663
    public void RUnitsUpdate(TObject Sender)
    {
        TAoWCampaignTransferSettings S;
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

    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        var campaign = AoWEngine.FCampaign;
        ResetSelection();
        campaign.FTransferSettings.SetupSelection(FileBoundary.ReadName(AoWEngine.StartupDirectory + "Scenario\\Campaign\\" +
            campaign.FScenarios.GetScenario(campaign.FNextScenarioID).FFileName));
        if (campaign.FTransferSettings.FTransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.FTransferSettings.FTransferGoldMax;
            ManaSlider.Max = campaign.FTransferSettings.FTransferManaMax;
            UpdateControls();
            TransBookWin.Visible = true;
        }
        else BookStartClick(this);
    }
}

public static partial class AoWCampaignTransferDlgUnit
{
}
