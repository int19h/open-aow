// Translated from AoWCampaignTransferDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWCampaignTransferDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// hero scroll. unit scroll. item scroll. left selection kind/index. right selection kind/index
public partial class CampaignTransferDlg : DataModule
{
    public CampaignTransferDlg(Component owner) : base(owner)
    { }

    public AOWCheckBox LHero1;
    public AOWCheckBox LHero2;
    public AOWCheckBox LHero3;
    public AOWCheckBox LHero4;
    public AOWCheckBox LItem1;
    public AOWPanel LItemPnl;
    public AOWCheckBox LItem2;
    public AOWCheckBox LItem3;
    public AOWCheckBox LItem4;
    public AOWCheckBox LUnit1;
    public AOWPanel LUnitPnl;
    public AOWCheckBox LUnit2;
    public AOWCheckBox LUnit3;
    public AOWCheckBox LUnit4;
    public AOWLabel Available;
    public AoWBookWindow TransBookWin;
    public AOWButton BookStart;
    public AOWButton Cancel;
    public AOWPanel PnlL;
    public AOWLabel Title;
    public AOWPanel PnlR;
    public AOWLabel TransferPointsLbl;
    public AOWButton AddBtn;
    public AOWLabel LGoldLbl;
    public AOWLabel GoldLeftLbl;
    public AOWLabel LManaLbl;
    public AOWLabel ManaLeftLbl;
    public AOWLabel Next;
    public AoWFrame LSep1;
    public AOWImage LHeroImgL;
    public AOWPanel LHeroPnl;
    public AOWImage LItemImgL;
    public AOWImage LUnitImgL;
    public AOWImage LHeroImgR;
    public AOWImage LItemImgR;
    public AOWImage LUnitImgR;
    public AOWCheckBox RHero1;
    public AOWCheckBox RHero2;
    public AOWCheckBox RHero3;
    public AOWCheckBox RHero4;
    public AOWCheckBox RItem1;
    public AOWPanel RItemPnl;
    public AOWCheckBox RItem2;
    public AOWCheckBox RItem3;
    public AOWCheckBox RItem4;
    public AOWCheckBox RUnit1;
    public AOWPanel RUnitPnl;
    public AOWCheckBox RUnit2;
    public AOWCheckBox RUnit3;
    public AOWCheckBox RUnit4;
    public AOWImage RHeroImgL;
    public AOWPanel RHeroPnl;
    public AOWImage RItemImgL;
    public AOWImage RUnitImgL;
    public AOWImage RHeroImgR;
    public AOWImage RItemImgR;
    public AOWImage RUnitImgR;
    public AoWFrame RSep1;
    public AOWButton RemoveBtn;
    public AoWFrame LSep2;
    public AoWFrame RSep2;
    public AOWHScrollBar GoldSlider;
    public AOWHScrollBar ManaSlider;
    public AOWButton LHeroLB;
    public AOWButton AutoBtn;
    public AOWButton LItemLB;
    public AOWButton LUnitLB;
    public AOWButton LHeroRB;
    public AOWButton LItemRB;
    public AOWButton LUnitRB;
    public AOWButton RHeroLB;
    public AOWButton RItemLB;
    public AOWButton RUnitLB;
    public AOWButton RHeroRB;
    public AOWButton RItemRB;
    public AOWButton RUnitRB;
    public AOWImage CostImg;
    public AOWPanel CostPnl;
    public AOWLabel CostLbl;
    public AOWImage LGoldImg;
    public AOWImage LManaImg;
    public AOWLabel TGoldLbl;
    public AOWLabel TransferGoldLbl;
    public AOWLabel TManaLbl;
    public AOWLabel TransferManaLbl;
    public AOWImage RGoldImg;
    public AOWImage RManaImg;
    public AOWImage ResBackG;
    public IvTranslator Translator;
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
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
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
    public void BookStartClick()
    {
        TransBookWin.Visible = false;
        Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        var campaign = Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign();
        campaign.FState += 1;
        Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:181
    public void CancelClick()
    {
        TransBookWin.Visible = false;
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.ResetNextScenario();
        Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.GetCampaign().FState = 1;
        Aow.Game.Ui.Dialogs.DialogHosts.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:190
    public void GoldSliderChange()
    {
        var aoWCampaignTransferSettings = GameTables.AoWEngine.FCampaign.FTransferSettings;
        aoWCampaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:197
    public void ManaSliderChange()
    {
        var aoWCampaignTransferSettings = GameTables.AoWEngine.FCampaign.FTransferSettings;
        aoWCampaignTransferSettings.SetSelectedTransferMana(ManaSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:204
    public void LHeroLBClick()
    {
        if (FLeftHero != 0)
        {
            FLeftHero -= 1;
        }

        LHeroPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:211
    public void LHeroRBClick()
    {
        if (GameTables.AoWEngine.FCampaign.FTransferSettings.FHeroes.Count > FLeftHero + 4)
        {
            FLeftHero += 1;
        }

        LHeroPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:218
    public void LItemLBClick()
    {
        if (FLeftItem != 0)
        {
            FLeftItem -= 1;
        }

        LItemPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:225
    public void LItemRBClick()
    {
        if (GameTables.AoWEngine.FCampaign.FTransferSettings.FItems.Count > FLeftItem + 4)
        {
            FLeftItem += 1;
        }

        LItemPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:232
    public void LUnitLBClick()
    {
        if (FLeftUnit != 0)
        {
            FLeftUnit -= 1;
        }

        LUnitPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:239
    public void LUnitRBClick()
    {
        if (GameTables.AoWEngine.FCampaign.FTransferSettings.FUnits.Count > FLeftUnit + 4)
        {
            FLeftUnit += 1;
        }

        LUnitPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:246
    public void RHeroLBClick()
    {
        if (FRightHero != 0)
        {
            FRightHero -= 1;
        }

        RHeroPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:253
    public void RHeroRBClick()
    {
        if (FRightHero + 4 < GameTables.AoWEngine.FCampaign.FTransferSettings.FSelectedHeroes.Count)
        {
            FRightHero += 1;
        }

        RHeroPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:260
    public void RItemLBClick()
    {
        if (FRightItem != 0)
        {
            FRightItem -= 1;
        }

        RItemPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:267
    public void RItemRBClick()
    {
        if (FRightItem + 4 < GameTables.AoWEngine.FCampaign.FTransferSettings.FSelectedItems.Count)
        {
            FRightItem += 1;
        }

        RItemPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:274
    public void RUnitLBClick()
    {
        if (FRightUnit != 0)
        {
            FRightUnit -= 1;
        }

        RUnitPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:281
    public void RUnitRBClick()
    {
        if (FRightUnit + 4 < GameTables.AoWEngine.FCampaign.FTransferSettings.FSelectedUnits.Count)
        {
            FRightUnit += 1;
        }

        RUnitPnl.FDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:288
    public void LHeroesDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        Hero Obj;
        AoWCampaignTransferSettings S;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Obj = S.FHeroes.GetHero(FLeftHero + Slot);
        if (Obj != null)
        {
            var checkBox = Sender;
            R = new Rect(LHeroPnl.WinLeft + checkBox.WinLeft + 1, LHeroPnl.WinTop + checkBox.WinTop + 2, LHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            R.Left += 1;
            Obj.ShowFace(R, SurfDesc);
            R.Left -= 1;
            if (S.FSelectedHeroes.IndexOf(FLeftHero + Slot) != -1)
            {
                DialogHosts.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, GfxState.AlphaBlendTable.LookupTables[65]);
            }

            if (Sender.FLit)
            {
                DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
            }

            DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
            DialogHosts.FontModule.Age8.Color = 16777215;
            var campaignTransferPoints = Obj.GetCampaignTransferPoints();
            Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
        }
    }

    // AoWCampaignTransferDlg.pas:316
    public void RHeroesDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        Hero Obj;
        AoWCampaignTransferSettings S;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        if (FRightHero + Slot < S.FSelectedHeroes.Count)
        {
            var selectedHeroe = S.FSelectedHeroes[FRightHero + Slot];
            Obj = S.FHeroes.GetHero(selectedHeroe);
            if (Obj != null)
            {
                var checkBox = Sender;
                R = new Rect(RHeroPnl.WinLeft + checkBox.WinLeft + 1, RHeroPnl.WinTop + checkBox.WinTop + 2, RHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                R.Left += 1;
                Obj.ShowFace(R, SurfDesc);
                R.Left -= 1;
                if (Sender.FLit)
                {
                    DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                DialogHosts.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:345
    public void LUnitsDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        AbstractUnit Obj;
        AoWCampaignTransferSettings S;
        P2UnitShowInfo Info = default;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        var count = S.FUnits.Count;
        if (FLeftUnit + Slot < count)
        {
            Obj = S.FUnits.GetUnit(FLeftUnit + Slot);
            if (Obj != null)
            {
                var checkBox = Sender;
                R = new Rect(LUnitPnl.WinLeft + checkBox.WinLeft + 1, LUnitPnl.WinTop + checkBox.WinTop + 2, LUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.IntGfxMod.CityWin[4].Show(R.Left + 1, R.Top, SurfDesc);
                Info.Direction = 3;
                Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                Info.Animation = UnitAnimation.StandStill;
                Info.Frame = 0;
                Info.Looped = false;
                Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                if (S.FSelectedUnits.IndexOf(FLeftUnit + Slot) != -1)
                {
                    DialogHosts.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, GfxState.AlphaBlendTable.LookupTables[65]);
                }

                if (Sender.FLit)
                {
                    DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                DialogHosts.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:381
    public void RUnitsDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        AbstractUnit Obj;
        AoWCampaignTransferSettings S;
        P2UnitShowInfo Info = default;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        if (FRightUnit + Slot < S.FSelectedUnits.Count)
        {
            var selectedUnit = S.FSelectedUnits[FRightUnit + Slot];
            Obj = S.FUnits.GetUnit(selectedUnit);
            if (Obj != null)
            {
                var checkBox = Sender;
                R = new Rect(RUnitPnl.WinLeft + checkBox.WinLeft + 1, RUnitPnl.WinTop + checkBox.WinTop + 2, RUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.IntGfxMod.CityWin[4].Show(R.Left + 1, R.Top, SurfDesc);
                Info.Direction = 3;
                Info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                Info.Animation = UnitAnimation.StandStill;
                Info.Frame = 0;
                Info.Looped = false;
                Obj.ShowEx(R.Left - 8, R.Top, SurfDesc, Info);
                if (Sender.FLit)
                {
                    DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                DialogHosts.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:415
    public void LItemsDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        Item Obj;
        AoWCampaignTransferSettings S;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Obj = S.FItems.GetItem(FLeftItem + Slot);
        if (Obj != null)
        {
            var checkBox = Sender;
            R = new Rect(LItemPnl.WinLeft + checkBox.WinLeft + 2, LItemPnl.WinTop + checkBox.WinTop + 2, LItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, LItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            DialogHosts.IntGfxMod.CityWin[4].Show(R.Left, R.Top, SurfDesc);
            Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
            if (S.FSelectedItems.IndexOf(FLeftItem + Slot) != -1)
            {
                DialogHosts.IntGfxMod.CityWin[5].ShowBlended(R.Left, R.Top, SurfDesc, GfxState.AlphaBlendTable.LookupTables[65]);
            }

            if (Sender.FLit)
            {
                DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
            }

            DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
            DialogHosts.FontModule.Age8.Color = 16777215;
            var campaignTransferPoints = Obj.GetCampaignTransferPoints();
            Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
        }
    }

    // AoWCampaignTransferDlg.pas:442
    public void RItemsDraw(AOWCheckBox Sender, SurfaceDesc SurfDesc, int Slot)
    {
        Item Obj;
        AoWCampaignTransferSettings S;
        Rect R;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        if (FRightItem + Slot < S.FSelectedItems.Count)
        {
            var selectedItem = S.FSelectedItems[FRightItem + Slot];
            Obj = S.FItems.GetItem(selectedItem);
            if (Obj != null)
            {
                var checkBox = Sender;
                R = new Rect(RItemPnl.WinLeft + checkBox.WinLeft + 2, RItemPnl.WinTop + checkBox.WinTop + 2, RItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, RItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.IntGfxMod.CityWin[4].Show(R.Left, R.Top, SurfDesc);
                Obj.Show(R.Left + 5, R.Top + 10, SurfDesc);
                if (Sender.FLit)
                {
                    DialogHosts.IntGfxMod.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, SurfDesc, GfxState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.IntGfxMod.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, SurfDesc);
                DialogHosts.FontModule.Age8.Color = 16777215;
                var campaignTransferPoints = Obj.GetCampaignTransferPoints();
                Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), SurfDesc);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:470
    public void LHeroesSelect(AOWCheckBox Sender, int Slot)
    {
        if (Sender.Checked)
        {
            ClearLeftSelection();
            var checkBox = Sender;
            checkBox.Checked = true;
            FLeftKind = 1;
            FLeftSelection = Slot + FLeftHero;
        }
    }

    // AoWCampaignTransferDlg.pas:484
    public void LItemsSelect(AOWCheckBox Sender, int Slot)
    {
        ClearLeftSelection();
        var checkBox = Sender;
        checkBox.Checked = true;
        FLeftKind = 2;
        FLeftSelection = Slot + FLeftItem;
    }

    // AoWCampaignTransferDlg.pas:495
    public void LUnitSelect(AOWCheckBox Sender, int Slot)
    {
        ClearLeftSelection();
        var checkBox = Sender;
        checkBox.Checked = true;
        FLeftKind = 3;
        FLeftSelection = Slot + FLeftUnit;
    }

    // AoWCampaignTransferDlg.pas:506
    public void RHeroesSelect(AOWCheckBox Sender, int Slot)
    {
        ClearRightSelection();
        var checkBox = Sender;
        checkBox.Checked = true;
        FRightKind = 1;
        FRightSelection = Slot + FRightHero;
    }

    // AoWCampaignTransferDlg.pas:517
    public void RItemsSelect(AOWCheckBox Sender, int Slot)
    {
        ClearRightSelection();
        var checkBox = Sender;
        checkBox.Checked = true;
        FRightKind = 2;
        FRightSelection = Slot + FRightItem;
    }

    // AoWCampaignTransferDlg.pas:528
    public void RUnitsSelect(AOWCheckBox Sender, int Slot)
    {
        ClearRightSelection();
        var checkBox = Sender;
        checkBox.Checked = true;
        FRightKind = 3;
        FRightSelection = Slot + FRightUnit;
    }

    // AoWCampaignTransferDlg.pas:539
    public void AddBtnClick()
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
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
    public void RemoveBtnClick()
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
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
    public void LHeroesUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FLeftKind == 1 && Slot + FLeftHero == FLeftSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FHeroes.Count > Slot + FLeftHero && S.FSelectedHeroes.IndexOf(Slot + FLeftHero) == -1;
    }

    // AoWCampaignTransferDlg.pas:617
    public void LItemsUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FLeftKind == 2 && Slot + FLeftItem == FLeftSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FItems.Count > Slot + FLeftItem && S.FSelectedItems.IndexOf(Slot + FLeftItem) == -1;
    }

    // AoWCampaignTransferDlg.pas:629
    public void LUnitsUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FLeftKind == 3 && Slot + FLeftUnit == FLeftSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && S.FUnits.Count > Slot + FLeftUnit && S.FSelectedUnits.IndexOf(Slot + FLeftUnit) == -1;
    }

    // AoWCampaignTransferDlg.pas:641
    public void RHeroesUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FRightKind == 1 && Slot + FRightHero == FRightSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && Slot + FRightHero < S.FSelectedHeroes.Count;
    }

    // AoWCampaignTransferDlg.pas:652
    public void RItemsUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FRightKind == 2 && Slot + FRightItem == FRightSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && Slot + FRightItem < S.FSelectedItems.Count;
    }

    // AoWCampaignTransferDlg.pas:663
    public void RUnitsUpdate(AOWCheckBox Sender, int Slot)
    {
        AoWCampaignTransferSettings S;
        S = GameTables.AoWEngine.FCampaign.FTransferSettings;
        Sender.Checked = FRightKind == 3 && Slot + FRightUnit == FRightSelection;
        var checkBox = Sender;
        checkBox.FCaptureInput = !checkBox.Checked && Slot + FRightUnit < S.FSelectedUnits.Count;
    }

    // AoWCampaignTransferDlg.pas:674
    public void AutoBtnClick()
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FTransferSettings.AutoSelect();
        UpdateControls();
        TransBookWin.FDrawn = false;
    }

    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        var campaign = GameTables.AoWEngine.FCampaign;
        ResetSelection();
        campaign.FTransferSettings.SetupSelection(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" +
            campaign.FScenarios.GetScenario(campaign.FNextScenarioID).FFileName));
        if (campaign.FTransferSettings.FTransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.FTransferSettings.FTransferGoldMax;
            ManaSlider.Max = campaign.FTransferSettings.FTransferManaMax;
            UpdateControls();
            TransBookWin.Visible = true;
        }
        else BookStartClick();
    }
}
