// Translated from AoWCampaignTransferDlg.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// hero scroll. unit scroll. item scroll. left selection kind/index. right selection kind/index
public partial class CampaignTransferDialog : DataModule
{
    public CampaignTransferDialog(Component owner) : base(owner)
    { }

    public CheckBox LHero1;
    public CheckBox LHero2;
    public CheckBox LHero3;
    public CheckBox LHero4;
    public CheckBox LItem1;
    public Panel LItemPnl;
    public CheckBox LItem2;
    public CheckBox LItem3;
    public CheckBox LItem4;
    public CheckBox LUnit1;
    public Panel LUnitPnl;
    public CheckBox LUnit2;
    public CheckBox LUnit3;
    public CheckBox LUnit4;
    public Label Available;
    public BookWindow TransBookWin;
    public Button BookStart;
    public Button Cancel;
    public Panel PnlL;
    public Label Title;
    public Panel PnlR;
    public Label TransferPointsLbl;
    public Button AddBtn;
    public Label LGoldLbl;
    public Label GoldLeftLbl;
    public Label LManaLbl;
    public Label ManaLeftLbl;
    public Label Next;
    public Frame LSep1;
    public Image LHeroImgL;
    public Panel LHeroPnl;
    public Image LItemImgL;
    public Image LUnitImgL;
    public Image LHeroImgR;
    public Image LItemImgR;
    public Image LUnitImgR;
    public CheckBox RHero1;
    public CheckBox RHero2;
    public CheckBox RHero3;
    public CheckBox RHero4;
    public CheckBox RItem1;
    public Panel RItemPnl;
    public CheckBox RItem2;
    public CheckBox RItem3;
    public CheckBox RItem4;
    public CheckBox RUnit1;
    public Panel RUnitPnl;
    public CheckBox RUnit2;
    public CheckBox RUnit3;
    public CheckBox RUnit4;
    public Image RHeroImgL;
    public Panel RHeroPnl;
    public Image RItemImgL;
    public Image RUnitImgL;
    public Image RHeroImgR;
    public Image RItemImgR;
    public Image RUnitImgR;
    public Frame RSep1;
    public Button RemoveBtn;
    public Frame LSep2;
    public Frame RSep2;
    public HorizontalScrollBar GoldSlider;
    public HorizontalScrollBar ManaSlider;
    public Button LHeroLB;
    public Button AutoBtn;
    public Button LItemLB;
    public Button LUnitLB;
    public Button LHeroRB;
    public Button LItemRB;
    public Button LUnitRB;
    public Button RHeroLB;
    public Button RItemLB;
    public Button RUnitLB;
    public Button RHeroRB;
    public Button RItemRB;
    public Button RUnitRB;
    public Image CostImg;
    public Panel CostPnl;
    public Label CostLbl;
    public Image LGoldImg;
    public Image LManaImg;
    public Label TGoldLbl;
    public Label TransferGoldLbl;
    public Label TManaLbl;
    public Label TransferManaLbl;
    public Image RGoldImg;
    public Image RManaImg;
    public Image ResBackG;
    public IvTranslator Translator;
    public int LeftHero;
    public int RightHero;
    public int LeftUnit;
    public int RightUnit;
    public int LeftItem;
    public int RightItem;
    public byte LeftKind;
    public int LeftSelection;
    public byte RightKind;
    public int RightSelection;

    // AoWCampaignTransferDlg.pas:683
    public void UpdateControls()
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        TransferPointsLbl.Text = IntToStr(S.TransferPointsLeft) + "/" + IntToStr(S.TransferPointsMax);
        TransferManaLbl.Text = IntToStr(S.SelectedTransferMana);
        TransferGoldLbl.Text = IntToStr(S.SelectedTransferGold);
        ManaLeftLbl.Text = IntToStr(S.TransferManaMax - S.SelectedTransferMana);
        GoldLeftLbl.Text = IntToStr(S.TransferGoldMax - S.SelectedTransferGold);
        ManaSlider.Position = S.SelectedTransferMana;
        GoldSlider.Position = S.SelectedTransferGold;
        LHeroLB.Visible = LeftHero != 0;
        var count = S.Heroes.Count;
        LHeroRB.Visible = count > LeftHero + 4;
        LItemLB.Visible = LeftItem != 0;
        var fItemsCount = S.Items.Count;
        LItemRB.Visible = fItemsCount > LeftItem + 4;
        LUnitLB.Visible = LeftUnit != 0;
        var fUnitsCount = S.Units.Count;
        LUnitRB.Visible = fUnitsCount > LeftUnit + 4;
        RHeroLB.Visible = RightHero != 0;
        RHeroRB.Visible = RightHero + 4 < S.SelectedHeroes.Count;
        RItemLB.Visible = RightItem != 0;
        RItemRB.Visible = RightItem + 4 < S.SelectedItems.Count;
        RUnitLB.Visible = RightUnit != 0;
        RUnitRB.Visible = RightUnit + 4 < S.SelectedUnits.Count;
        TransBookWin.Drawn = false;
    }

    // AoWCampaignTransferDlg.pas:710
    public void ResetSelection()
    {
        LeftHero = 0;
        RightHero = 0;
        LeftUnit = 0;
        RightUnit = 0;
        LeftItem = 0;
        RightItem = 0;
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
        LeftKind = 0;
        LeftSelection = -1;
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
        RightKind = 0;
        RightSelection = -1;
    }

    // AoWCampaignTransferDlg.pas:173
    public void BookStartClick()
    {
        TransBookWin.Visible = false;
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        var campaign = DialogHosts.WorldMap.GetCampaign();
        campaign.State += 1;
        DialogHosts.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:181
    public void CancelClick()
    {
        TransBookWin.Visible = false;
        GameTables.GameEngine.Campaign.ResetNextScenario();
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        DialogHosts.WorldMap.GetCampaign().State = 1;
        DialogHosts.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:190
    public void GoldSliderChange()
    {
        var campaignTransferSettings = GameTables.GameEngine.Campaign.TransferSettings;
        campaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:197
    public void ManaSliderChange()
    {
        var campaignTransferSettings = GameTables.GameEngine.Campaign.TransferSettings;
        campaignTransferSettings.SetSelectedTransferMana(ManaSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:204
    public void LHeroLBClick()
    {
        if (LeftHero != 0)
        {
            LeftHero -= 1;
        }

        LHeroPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:211
    public void LHeroRBClick()
    {
        if (GameTables.GameEngine.Campaign.TransferSettings.Heroes.Count > LeftHero + 4)
        {
            LeftHero += 1;
        }

        LHeroPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:218
    public void LItemLBClick()
    {
        if (LeftItem != 0)
        {
            LeftItem -= 1;
        }

        LItemPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:225
    public void LItemRBClick()
    {
        if (GameTables.GameEngine.Campaign.TransferSettings.Items.Count > LeftItem + 4)
        {
            LeftItem += 1;
        }

        LItemPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:232
    public void LUnitLBClick()
    {
        if (LeftUnit != 0)
        {
            LeftUnit -= 1;
        }

        LUnitPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:239
    public void LUnitRBClick()
    {
        if (GameTables.GameEngine.Campaign.TransferSettings.Units.Count > LeftUnit + 4)
        {
            LeftUnit += 1;
        }

        LUnitPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:246
    public void RHeroLBClick()
    {
        if (RightHero != 0)
        {
            RightHero -= 1;
        }

        RHeroPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:253
    public void RHeroRBClick()
    {
        if (RightHero + 4 < GameTables.GameEngine.Campaign.TransferSettings.SelectedHeroes.Count)
        {
            RightHero += 1;
        }

        RHeroPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:260
    public void RItemLBClick()
    {
        if (RightItem != 0)
        {
            RightItem -= 1;
        }

        RItemPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:267
    public void RItemRBClick()
    {
        if (RightItem + 4 < GameTables.GameEngine.Campaign.TransferSettings.SelectedItems.Count)
        {
            RightItem += 1;
        }

        RItemPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:274
    public void RUnitLBClick()
    {
        if (RightUnit != 0)
        {
            RightUnit -= 1;
        }

        RUnitPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:281
    public void RUnitRBClick()
    {
        if (RightUnit + 4 < GameTables.GameEngine.Campaign.TransferSettings.SelectedUnits.Count)
        {
            RightUnit += 1;
        }

        RUnitPnl.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:288
    public void LHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Hero objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        objectValue = S.Heroes.GetHero(LeftHero + slot);
        if (objectValue != null)
        {
            var checkBox = sender;
            R = new Rect(LHeroPnl.WinLeft + checkBox.WinLeft + 1, LHeroPnl.WinTop + checkBox.WinTop + 2, LHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            R.Left += 1;
            objectValue.ShowFace(R, lockedSurface);
            R.Left -= 1;
            if (S.SelectedHeroes.IndexOf(LeftHero + slot) != -1)
            {
                DialogHosts.UIGraphics.CityWin[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
            DialogHosts.UIFonts.Age8.Color = 16777215;
            var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
            DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:316
    public void RHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Hero objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        if (RightHero + slot < S.SelectedHeroes.Count)
        {
            var selectedHero = S.SelectedHeroes[RightHero + slot];
            objectValue = S.Heroes.GetHero(selectedHero);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(RHeroPnl.WinLeft + checkBox.WinLeft + 1, RHeroPnl.WinTop + checkBox.WinTop + 2, RHeroPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RHeroPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                R.Left += 1;
                objectValue.ShowFace(R, lockedSurface);
                R.Left -= 1;
                if (sender.Lit)
                {
                    DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                DialogHosts.UIFonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:345
    public void LUnitsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Troop objectValue;
        CampaignTransferSettings S;
        UnitShowInfo info = default;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        var count = S.Units.Count;
        if (LeftUnit + slot < count)
        {
            objectValue = S.Units.GetUnit(LeftUnit + slot);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(LUnitPnl.WinLeft + checkBox.WinLeft + 1, LUnitPnl.WinTop + checkBox.WinTop + 2, LUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, LUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.UIGraphics.CityWin[4].Show(R.Left + 1, R.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                objectValue.Show(R.Left - 8, R.Top, lockedSurface, info);
                if (S.SelectedUnits.IndexOf(LeftUnit + slot) != -1)
                {
                    DialogHosts.UIGraphics.CityWin[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
                }

                if (sender.Lit)
                {
                    DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                DialogHosts.UIFonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:381
    public void RUnitsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Troop objectValue;
        CampaignTransferSettings S;
        UnitShowInfo info = default;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        if (RightUnit + slot < S.SelectedUnits.Count)
        {
            var selectedUnit = S.SelectedUnits[RightUnit + slot];
            objectValue = S.Units.GetUnit(selectedUnit);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(RUnitPnl.WinLeft + checkBox.WinLeft + 1, RUnitPnl.WinTop + checkBox.WinTop + 2, RUnitPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, RUnitPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.UIGraphics.CityWin[4].Show(R.Left + 1, R.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                objectValue.Show(R.Left - 8, R.Top, lockedSurface, info);
                if (sender.Lit)
                {
                    DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                DialogHosts.UIFonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:415
    public void LItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Item objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        objectValue = S.Items.GetItem(LeftItem + slot);
        if (objectValue != null)
        {
            var checkBox = sender;
            R = new Rect(LItemPnl.WinLeft + checkBox.WinLeft + 2, LItemPnl.WinTop + checkBox.WinTop + 2, LItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, LItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            DialogHosts.UIGraphics.CityWin[4].Show(R.Left, R.Top, lockedSurface);
            objectValue.Show(R.Left + 5, R.Top + 10, lockedSurface);
            if (S.SelectedItems.IndexOf(LeftItem + slot) != -1)
            {
                DialogHosts.UIGraphics.CityWin[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
            DialogHosts.UIFonts.Age8.Color = 16777215;
            var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
            DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:442
    public void RItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Item objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        if (RightItem + slot < S.SelectedItems.Count)
        {
            var selectedItem = S.SelectedItems[RightItem + slot];
            objectValue = S.Items.GetItem(selectedItem);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(RItemPnl.WinLeft + checkBox.WinLeft + 2, RItemPnl.WinTop + checkBox.WinTop + 2, RItemPnl.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, RItemPnl.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                DialogHosts.UIGraphics.CityWin[4].Show(R.Left, R.Top, lockedSurface);
                objectValue.Show(R.Left + 5, R.Top + 10, lockedSurface);
                if (sender.Lit)
                {
                    DialogHosts.UIGraphics.CampaignTrans[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                DialogHosts.UIGraphics.CampaignTrans[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                DialogHosts.UIFonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                DialogHosts.UIFonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:470
    public void LHeroesSelect(CheckBox sender, int slot)
    {
        if (sender.Checked)
        {
            ClearLeftSelection();
            var checkBox = sender;
            checkBox.Checked = true;
            LeftKind = 1;
            LeftSelection = slot + LeftHero;
        }
    }

    // AoWCampaignTransferDlg.pas:484
    public void LItemsSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        LeftKind = 2;
        LeftSelection = slot + LeftItem;
    }

    // AoWCampaignTransferDlg.pas:495
    public void LUnitSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        LeftKind = 3;
        LeftSelection = slot + LeftUnit;
    }

    // AoWCampaignTransferDlg.pas:506
    public void RHeroesSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        RightKind = 1;
        RightSelection = slot + RightHero;
    }

    // AoWCampaignTransferDlg.pas:517
    public void RItemsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        RightKind = 2;
        RightSelection = slot + RightItem;
    }

    // AoWCampaignTransferDlg.pas:528
    public void RUnitsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        RightKind = 3;
        RightSelection = slot + RightUnit;
    }

    // AoWCampaignTransferDlg.pas:539
    public void AddBtnClick()
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        switch (LeftKind)
        {
            case 1:
                if (S.AddHero(LeftSelection))
                {
                    LHeroPnl.Drawn = false;
                    RHeroPnl.Drawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 2:
                if (S.AddItem(LeftSelection))
                {
                    LItemPnl.Drawn = false;
                    RItemPnl.Drawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 3:
                if (S.AddUnit(LeftSelection))
                {
                    LUnitPnl.Drawn = false;
                    RUnitPnl.Drawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
        }
    }

    // AoWCampaignTransferDlg.pas:569
    public void RemoveBtnClick()
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        switch (RightKind)
        {
            case 1:
                if (S.DeleteHero(S.SelectedHeroes[RightSelection]))
                {
                    LHeroPnl.Drawn = false;
                    RHeroPnl.Drawn = false;
                    PnlR.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    TransBookWin.Drawn = false;
                }

                break;
            case 2:
                if (S.DeleteItem(S.SelectedItems[RightSelection]))
                {
                    LItemPnl.Drawn = false;
                    RItemPnl.Drawn = false;
                    PnlR.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    TransBookWin.Drawn = false;
                }

                break;
            case 3:
                if (S.DeleteUnit(S.SelectedUnits[RightSelection]))
                {
                    LUnitPnl.Drawn = false;
                    RUnitPnl.Drawn = false;
                    PnlR.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    TransBookWin.Drawn = false;
                }

                break;
        }
    }

    // AoWCampaignTransferDlg.pas:605
    public void LHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = LeftKind == 1 && slot + LeftHero == LeftSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && S.Heroes.Count > slot + LeftHero && S.SelectedHeroes.IndexOf(slot + LeftHero) == -1;
    }

    // AoWCampaignTransferDlg.pas:617
    public void LItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = LeftKind == 2 && slot + LeftItem == LeftSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && S.Items.Count > slot + LeftItem && S.SelectedItems.IndexOf(slot + LeftItem) == -1;
    }

    // AoWCampaignTransferDlg.pas:629
    public void LUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = LeftKind == 3 && slot + LeftUnit == LeftSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && S.Units.Count > slot + LeftUnit && S.SelectedUnits.IndexOf(slot + LeftUnit) == -1;
    }

    // AoWCampaignTransferDlg.pas:641
    public void RHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = RightKind == 1 && slot + RightHero == RightSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && slot + RightHero < S.SelectedHeroes.Count;
    }

    // AoWCampaignTransferDlg.pas:652
    public void RItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = RightKind == 2 && slot + RightItem == RightSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && slot + RightItem < S.SelectedItems.Count;
    }

    // AoWCampaignTransferDlg.pas:663
    public void RUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = GameTables.GameEngine.Campaign.TransferSettings;
        sender.Checked = RightKind == 3 && slot + RightUnit == RightSelection;
        var checkBox = sender;
        checkBox._captureInput = !checkBox.Checked && slot + RightUnit < S.SelectedUnits.Count;
    }

    // AoWCampaignTransferDlg.pas:674
    public void AutoBtnClick()
    {
        GameTables.GameEngine.Campaign.TransferSettings.AutoSelect();
        UpdateControls();
        TransBookWin.Drawn = false;
    }

    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        var campaign = GameTables.GameEngine.Campaign;
        ResetSelection();
        campaign.TransferSettings.SetupSelection(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + "Scenario\\Campaign\\" +
            campaign.Scenarios.GetScenario(campaign.NextScenarioId).FileName));
        if (campaign.TransferSettings.TransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.TransferSettings.TransferGoldMax;
            ManaSlider.Max = campaign.TransferSettings.TransferManaMax;
            UpdateControls();
            TransBookWin.Visible = true;
        }
        else BookStartClick();
    }
}
