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
public partial class CampaignTransferDialog : UIModule
{
    public CampaignTransferDialog(UIContext ui) : base(ui)
    { }

    public CheckBox AvailableHero1CheckBox;
    public CheckBox AvailableHero2CheckBox;
    public CheckBox AvailableHero3CheckBox;
    public CheckBox AvailableHero4CheckBox;
    public CheckBox AvailableItem1CheckBox;
    public Panel AvailableItemPanel;
    public CheckBox AvailableItem2CheckBox;
    public CheckBox AvailableItem3CheckBox;
    public CheckBox AvailableItem4CheckBox;
    public CheckBox AvailableUnit1CheckBox;
    public Panel AvailableUnitPanel;
    public CheckBox AvailableUnit2CheckBox;
    public CheckBox AvailableUnit3CheckBox;
    public CheckBox AvailableUnit4CheckBox;
    public BookWindow Book;
    public Panel TransferPanel;
    public Label TransferPointsLabel;
    public Label AvailableGoldLabel;
    public Label AvailableManaLabel;
    public Panel AvailableHeroPanel;
    public CheckBox TransferHero1CheckBox;
    public CheckBox TransferHero2CheckBox;
    public CheckBox TransferHero3CheckBox;
    public CheckBox TransferHero4CheckBox;
    public CheckBox TransferItem1CheckBox;
    public Panel TransferItemPanel;
    public CheckBox TransferItem2CheckBox;
    public CheckBox TransferItem3CheckBox;
    public CheckBox TransferItem4CheckBox;
    public CheckBox TransferUnit1CheckBox;
    public Panel TransferUnitPanel;
    public CheckBox TransferUnit2CheckBox;
    public CheckBox TransferUnit3CheckBox;
    public CheckBox TransferUnit4CheckBox;
    public Panel TransferHeroPanel;
    public HorizontalScrollBar GoldSlider;
    public HorizontalScrollBar ManaSlider;
    public Button AvailableHeroPreviousButton;
    public Button AvailableItemPreviousButton;
    public Button AvailableUnitPreviousButton;
    public Button AvailableHeroNextButton;
    public Button AvailableItemNextButton;
    public Button AvailableUnitNextButton;
    public Button TransferHeroPreviousButton;
    public Button TransferItemPreviousButton;
    public Button TransferUnitPreviousButton;
    public Button TransferHeroNextButton;
    public Button TransferItemNextButton;
    public Button TransferUnitNextButton;
    public Label TransferGoldLabel;
    public Label TransferManaLabel;
    private int _leftHero;
    private int _rightHero;
    private int _leftUnit;
    private int _rightUnit;
    private int _leftItem;
    private int _rightItem;
    private byte _leftKind;
    private int _leftSelection;
    private byte _rightKind;
    private int _rightSelection;

    // AoWCampaignTransferDlg.pas:683
    public void UpdateControls()
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        TransferPointsLabel.Text = IntToStr(S.TransferPointsLeft) + "/" + IntToStr(S.TransferPointsMax);
        TransferManaLabel.Text = IntToStr(S.SelectedTransferMana);
        TransferGoldLabel.Text = IntToStr(S.SelectedTransferGold);
        AvailableManaLabel.Text = IntToStr(S.TransferManaMax - S.SelectedTransferMana);
        AvailableGoldLabel.Text = IntToStr(S.TransferGoldMax - S.SelectedTransferGold);
        ManaSlider.Position = S.SelectedTransferMana;
        GoldSlider.Position = S.SelectedTransferGold;
        AvailableHeroPreviousButton.Visible = _leftHero != 0;
        var count = S.Heroes.Count;
        AvailableHeroNextButton.Visible = count > _leftHero + 4;
        AvailableItemPreviousButton.Visible = _leftItem != 0;
        var fItemsCount = S.Items.Count;
        AvailableItemNextButton.Visible = fItemsCount > _leftItem + 4;
        AvailableUnitPreviousButton.Visible = _leftUnit != 0;
        var fUnitsCount = S.Units.Count;
        AvailableUnitNextButton.Visible = fUnitsCount > _leftUnit + 4;
        TransferHeroPreviousButton.Visible = _rightHero != 0;
        TransferHeroNextButton.Visible = _rightHero + 4 < S.SelectedHeroes.Count;
        TransferItemPreviousButton.Visible = _rightItem != 0;
        TransferItemNextButton.Visible = _rightItem + 4 < S.SelectedItems.Count;
        TransferUnitPreviousButton.Visible = _rightUnit != 0;
        TransferUnitNextButton.Visible = _rightUnit + 4 < S.SelectedUnits.Count;
        Book.Drawn = false;
    }

    // AoWCampaignTransferDlg.pas:710
    public void ResetSelection()
    {
        _leftHero = 0;
        _rightHero = 0;
        _leftUnit = 0;
        _rightUnit = 0;
        _leftItem = 0;
        _rightItem = 0;
        ClearLeftSelection();
        ClearRightSelection();
    }

    // AoWCampaignTransferDlg.pas:722
    public void ClearLeftSelection()
    {
        AvailableHero1CheckBox.Checked = false;
        AvailableHero2CheckBox.Checked = false;
        AvailableHero3CheckBox.Checked = false;
        AvailableHero4CheckBox.Checked = false;
        AvailableItem1CheckBox.Checked = false;
        AvailableItem2CheckBox.Checked = false;
        AvailableItem3CheckBox.Checked = false;
        AvailableItem4CheckBox.Checked = false;
        AvailableUnit1CheckBox.Checked = false;
        AvailableUnit2CheckBox.Checked = false;
        AvailableUnit3CheckBox.Checked = false;
        AvailableUnit4CheckBox.Checked = false;
        _leftKind = 0;
        _leftSelection = -1;
    }

    // AoWCampaignTransferDlg.pas:740
    public void ClearRightSelection()
    {
        TransferHero1CheckBox.Checked = false;
        TransferHero2CheckBox.Checked = false;
        TransferHero3CheckBox.Checked = false;
        TransferHero4CheckBox.Checked = false;
        TransferItem1CheckBox.Checked = false;
        TransferItem2CheckBox.Checked = false;
        TransferItem3CheckBox.Checked = false;
        TransferItem4CheckBox.Checked = false;
        TransferUnit1CheckBox.Checked = false;
        TransferUnit2CheckBox.Checked = false;
        TransferUnit3CheckBox.Checked = false;
        TransferUnit4CheckBox.Checked = false;
        _rightKind = 0;
        _rightSelection = -1;
    }

    // AoWCampaignTransferDlg.pas:173
    public void BookStartClick()
    {
        Book.Visible = false;
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        var campaign = UI.WorldMap.GetCampaign();
        campaign.State += 1;
        UI.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:181
    public void CancelClick()
    {
        Book.Visible = false;
        UI.Engine.Campaign.ResetNextScenario();
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        UI.WorldMap.GetCampaign().State = 1;
        UI.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:190
    public void GoldSliderChange()
    {
        var campaignTransferSettings = UI.Engine.Campaign.TransferSettings;
        campaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:197
    public void ManaSliderChange()
    {
        var campaignTransferSettings = UI.Engine.Campaign.TransferSettings;
        campaignTransferSettings.SetSelectedTransferMana(ManaSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:204
    public void LHeroLBClick()
    {
        if (_leftHero != 0)
        {
            _leftHero -= 1;
        }

        AvailableHeroPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:211
    public void LHeroRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Heroes.Count > _leftHero + 4)
        {
            _leftHero += 1;
        }

        AvailableHeroPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:218
    public void LItemLBClick()
    {
        if (_leftItem != 0)
        {
            _leftItem -= 1;
        }

        AvailableItemPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:225
    public void LItemRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Items.Count > _leftItem + 4)
        {
            _leftItem += 1;
        }

        AvailableItemPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:232
    public void LUnitLBClick()
    {
        if (_leftUnit != 0)
        {
            _leftUnit -= 1;
        }

        AvailableUnitPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:239
    public void LUnitRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Units.Count > _leftUnit + 4)
        {
            _leftUnit += 1;
        }

        AvailableUnitPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:246
    public void RHeroLBClick()
    {
        if (_rightHero != 0)
        {
            _rightHero -= 1;
        }

        TransferHeroPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:253
    public void RHeroRBClick()
    {
        if (_rightHero + 4 < UI.Engine.Campaign.TransferSettings.SelectedHeroes.Count)
        {
            _rightHero += 1;
        }

        TransferHeroPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:260
    public void RItemLBClick()
    {
        if (_rightItem != 0)
        {
            _rightItem -= 1;
        }

        TransferItemPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:267
    public void RItemRBClick()
    {
        if (_rightItem + 4 < UI.Engine.Campaign.TransferSettings.SelectedItems.Count)
        {
            _rightItem += 1;
        }

        TransferItemPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:274
    public void RUnitLBClick()
    {
        if (_rightUnit != 0)
        {
            _rightUnit -= 1;
        }

        TransferUnitPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:281
    public void RUnitRBClick()
    {
        if (_rightUnit + 4 < UI.Engine.Campaign.TransferSettings.SelectedUnits.Count)
        {
            _rightUnit += 1;
        }

        TransferUnitPanel.Drawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:288
    public void LHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Hero objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = UI.Engine.Campaign.TransferSettings;
        objectValue = S.Heroes.GetHero(_leftHero + slot);
        if (objectValue != null)
        {
            var checkBox = sender;
            R = new Rect(AvailableHeroPanel.WinLeft + checkBox.WinLeft + 1, AvailableHeroPanel.WinTop + checkBox.WinTop + 2, AvailableHeroPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, AvailableHeroPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            R.Left += 1;
            objectValue.ShowFace(R, lockedSurface);
            R.Left -= 1;
            if (S.SelectedHeroes.IndexOf(_leftHero + slot) != -1)
            {
                UI.Graphics.CityWindow[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
            UI.Fonts.Age8.Color = 16777215;
            var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
            UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:316
    public void RHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Hero objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = UI.Engine.Campaign.TransferSettings;
        if (_rightHero + slot < S.SelectedHeroes.Count)
        {
            var selectedHero = S.SelectedHeroes[_rightHero + slot];
            objectValue = S.Heroes.GetHero(selectedHero);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(TransferHeroPanel.WinLeft + checkBox.WinLeft + 1, TransferHeroPanel.WinTop + checkBox.WinTop + 2, TransferHeroPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, TransferHeroPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                R.Left += 1;
                objectValue.ShowFace(R, lockedSurface);
                R.Left -= 1;
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
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
        S = UI.Engine.Campaign.TransferSettings;
        var count = S.Units.Count;
        if (_leftUnit + slot < count)
        {
            objectValue = S.Units.GetUnit(_leftUnit + slot);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(AvailableUnitPanel.WinLeft + checkBox.WinLeft + 1, AvailableUnitPanel.WinTop + checkBox.WinTop + 2, AvailableUnitPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, AvailableUnitPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(R.Left + 1, R.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                objectValue.Show(R.Left - 8, R.Top, lockedSurface, info);
                if (S.SelectedUnits.IndexOf(_leftUnit + slot) != -1)
                {
                    UI.Graphics.CityWindow[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
                }

                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
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
        S = UI.Engine.Campaign.TransferSettings;
        if (_rightUnit + slot < S.SelectedUnits.Count)
        {
            var selectedUnit = S.SelectedUnits[_rightUnit + slot];
            objectValue = S.Units.GetUnit(selectedUnit);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(TransferUnitPanel.WinLeft + checkBox.WinLeft + 1, TransferUnitPanel.WinTop + checkBox.WinTop + 2, TransferUnitPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 2, TransferUnitPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(R.Left + 1, R.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                objectValue.Show(R.Left - 8, R.Top, lockedSurface, info);
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:415
    public void LItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Item objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = UI.Engine.Campaign.TransferSettings;
        objectValue = S.Items.GetItem(_leftItem + slot);
        if (objectValue != null)
        {
            var checkBox = sender;
            R = new Rect(AvailableItemPanel.WinLeft + checkBox.WinLeft + 2, AvailableItemPanel.WinTop + checkBox.WinTop + 2, AvailableItemPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, AvailableItemPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

            UI.Graphics.CityWindow[4].Show(R.Left, R.Top, lockedSurface);
            objectValue.Show(R.Left + 5, R.Top + 10, lockedSurface);
            if (S.SelectedItems.IndexOf(_leftItem + slot) != -1)
            {
                UI.Graphics.CityWindow[5].ShowBlended(R.Left, R.Top, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
            UI.Fonts.Age8.Color = 16777215;
            var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
            UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:442
    public void RItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Item objectValue;
        CampaignTransferSettings S;
        Rect R;
        S = UI.Engine.Campaign.TransferSettings;
        if (_rightItem + slot < S.SelectedItems.Count)
        {
            var selectedItem = S.SelectedItems[_rightItem + slot];
            objectValue = S.Items.GetItem(selectedItem);
            if (objectValue != null)
            {
                var checkBox = sender;
                R = new Rect(TransferItemPanel.WinLeft + checkBox.WinLeft + 2, TransferItemPanel.WinTop + checkBox.WinTop + 2, TransferItemPanel.WinLeft + checkBox.WinLeft + checkBox.WinWidth - 4, TransferItemPanel.WinTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(R.Left, R.Top, lockedSurface);
                objectValue.Show(R.Left + 5, R.Top + 10, lockedSurface);
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(R.Left - 1, R.Top - 2, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(R.Left + 10, R.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 16777215;
                var campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(R.Left + 24, R.Top + 56, IntToStr(campaignTransferPoints), lockedSurface);
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
            _leftKind = 1;
            _leftSelection = slot + _leftHero;
        }
    }

    // AoWCampaignTransferDlg.pas:484
    public void LItemsSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        _leftKind = 2;
        _leftSelection = slot + _leftItem;
    }

    // AoWCampaignTransferDlg.pas:495
    public void LUnitSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        _leftKind = 3;
        _leftSelection = slot + _leftUnit;
    }

    // AoWCampaignTransferDlg.pas:506
    public void RHeroesSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 1;
        _rightSelection = slot + _rightHero;
    }

    // AoWCampaignTransferDlg.pas:517
    public void RItemsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 2;
        _rightSelection = slot + _rightItem;
    }

    // AoWCampaignTransferDlg.pas:528
    public void RUnitsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        var checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 3;
        _rightSelection = slot + _rightUnit;
    }

    // AoWCampaignTransferDlg.pas:539
    public void AddBtnClick()
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        switch (_leftKind)
        {
            case 1:
                if (S.AddHero(_leftSelection))
                {
                    AvailableHeroPanel.Drawn = false;
                    TransferHeroPanel.Drawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 2:
                if (S.AddItem(_leftSelection))
                {
                    AvailableItemPanel.Drawn = false;
                    TransferItemPanel.Drawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 3:
                if (S.AddUnit(_leftSelection))
                {
                    AvailableUnitPanel.Drawn = false;
                    TransferUnitPanel.Drawn = false;
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
        S = UI.Engine.Campaign.TransferSettings;
        switch (_rightKind)
        {
            case 1:
                if (S.DeleteHero(S.SelectedHeroes[_rightSelection]))
                {
                    AvailableHeroPanel.Drawn = false;
                    TransferHeroPanel.Drawn = false;
                    TransferPanel.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.Drawn = false;
                }

                break;
            case 2:
                if (S.DeleteItem(S.SelectedItems[_rightSelection]))
                {
                    AvailableItemPanel.Drawn = false;
                    TransferItemPanel.Drawn = false;
                    TransferPanel.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.Drawn = false;
                }

                break;
            case 3:
                if (S.DeleteUnit(S.SelectedUnits[_rightSelection]))
                {
                    AvailableUnitPanel.Drawn = false;
                    TransferUnitPanel.Drawn = false;
                    TransferPanel.Drawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.Drawn = false;
                }

                break;
        }
    }

    // AoWCampaignTransferDlg.pas:605
    public void LHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 1 && slot + _leftHero == _leftSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && S.Heroes.Count > slot + _leftHero && S.SelectedHeroes.IndexOf(slot + _leftHero) == -1;
    }

    // AoWCampaignTransferDlg.pas:617
    public void LItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 2 && slot + _leftItem == _leftSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && S.Items.Count > slot + _leftItem && S.SelectedItems.IndexOf(slot + _leftItem) == -1;
    }

    // AoWCampaignTransferDlg.pas:629
    public void LUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 3 && slot + _leftUnit == _leftSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && S.Units.Count > slot + _leftUnit && S.SelectedUnits.IndexOf(slot + _leftUnit) == -1;
    }

    // AoWCampaignTransferDlg.pas:641
    public void RHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 1 && slot + _rightHero == _rightSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightHero < S.SelectedHeroes.Count;
    }

    // AoWCampaignTransferDlg.pas:652
    public void RItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 2 && slot + _rightItem == _rightSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightItem < S.SelectedItems.Count;
    }

    // AoWCampaignTransferDlg.pas:663
    public void RUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings S;
        S = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 3 && slot + _rightUnit == _rightSelection;
        var checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightUnit < S.SelectedUnits.Count;
    }

    // AoWCampaignTransferDlg.pas:674
    public void AutoBtnClick()
    {
        UI.Engine.Campaign.TransferSettings.AutoSelect();
        UpdateControls();
        Book.Drawn = false;
    }

    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        var campaign = UI.Engine.Campaign;
        ResetSelection();
        campaign.TransferSettings.SetupSelection(FileBoundary.ReadName(UI.Engine.StartupDirectory + "Scenario\\Campaign\\" +
            campaign.Scenarios.GetScenario(campaign.NextScenarioId).FileName));
        if (campaign.TransferSettings.TransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.TransferSettings.TransferGoldMax;
            ManaSlider.Max = campaign.TransferSettings.TransferManaMax;
            UpdateControls();
            Book.Visible = true;
        }
        else BookStartClick();
    }
}
