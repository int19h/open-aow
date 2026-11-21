#nullable disable warnings
// Translated from AoWCampaignTransferDlg.pas.
using System.Globalization;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// hero scroll. unit scroll. item scroll. left selection kind/index. right selection kind/index
public sealed partial class CampaignTransferDialog(UIContext ui) : UIModule(ui)
{
    public CheckBox AvailableHero1CheckBox { get; private set; }
    public CheckBox AvailableHero2CheckBox { get; private set; }
    public CheckBox AvailableHero3CheckBox { get; private set; }
    public CheckBox AvailableHero4CheckBox { get; private set; }
    public CheckBox AvailableItem1CheckBox { get; private set; }
    public Panel AvailableItemPanel { get; private set; }
    public CheckBox AvailableItem2CheckBox { get; private set; }
    public CheckBox AvailableItem3CheckBox { get; private set; }
    public CheckBox AvailableItem4CheckBox { get; private set; }
    public CheckBox AvailableUnit1CheckBox { get; private set; }
    public Panel AvailableUnitPanel { get; private set; }
    public CheckBox AvailableUnit2CheckBox { get; private set; }
    public CheckBox AvailableUnit3CheckBox { get; private set; }
    public CheckBox AvailableUnit4CheckBox { get; private set; }
    public BookWindow Book { get; private set; }
    public Panel TransferPanel { get; private set; }
    public Label TransferPointsLabel { get; private set; }
    public Label AvailableGoldLabel { get; private set; }
    public Label AvailableManaLabel { get; private set; }
    public Panel AvailableHeroPanel { get; private set; }
    public CheckBox TransferHero1CheckBox { get; private set; }
    public CheckBox TransferHero2CheckBox { get; private set; }
    public CheckBox TransferHero3CheckBox { get; private set; }
    public CheckBox TransferHero4CheckBox { get; private set; }
    public CheckBox TransferItem1CheckBox { get; private set; }
    public Panel TransferItemPanel { get; private set; }
    public CheckBox TransferItem2CheckBox { get; private set; }
    public CheckBox TransferItem3CheckBox { get; private set; }
    public CheckBox TransferItem4CheckBox { get; private set; }
    public CheckBox TransferUnit1CheckBox { get; private set; }
    public Panel TransferUnitPanel { get; private set; }
    public CheckBox TransferUnit2CheckBox { get; private set; }
    public CheckBox TransferUnit3CheckBox { get; private set; }
    public CheckBox TransferUnit4CheckBox { get; private set; }
    public Panel TransferHeroPanel { get; private set; }
    public HorizontalScrollBar GoldSlider { get; private set; }
    public HorizontalScrollBar ManaSlider { get; private set; }
    public Button AvailableHeroPreviousButton { get; private set; }
    public Button AvailableItemPreviousButton { get; private set; }
    public Button AvailableUnitPreviousButton { get; private set; }
    public Button AvailableHeroNextButton { get; private set; }
    public Button AvailableItemNextButton { get; private set; }
    public Button AvailableUnitNextButton { get; private set; }
    public Button TransferHeroPreviousButton { get; private set; }
    public Button TransferItemPreviousButton { get; private set; }
    public Button TransferUnitPreviousButton { get; private set; }
    public Button TransferHeroNextButton { get; private set; }
    public Button TransferItemNextButton { get; private set; }
    public Button TransferUnitNextButton { get; private set; }
    public Label TransferGoldLabel { get; private set; }
    public Label TransferManaLabel { get; private set; }

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
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        TransferPointsLabel.Text = $"{s.TransferPointsLeft.ToString(CultureInfo.InvariantCulture)}/{s.TransferPointsMax.ToString(CultureInfo.InvariantCulture)}";
        TransferManaLabel.Text = s.SelectedTransferMana.ToString(CultureInfo.InvariantCulture);
        TransferGoldLabel.Text = s.SelectedTransferGold.ToString(CultureInfo.InvariantCulture);
        AvailableManaLabel.Text = (s.TransferManaMax - s.SelectedTransferMana).ToString(CultureInfo.InvariantCulture);
        AvailableGoldLabel.Text = (s.TransferGoldMax - s.SelectedTransferGold).ToString(CultureInfo.InvariantCulture);
        ManaSlider.Position = s.SelectedTransferMana;
        GoldSlider.Position = s.SelectedTransferGold;
        AvailableHeroPreviousButton.Visible = _leftHero != 0;
        int count = s.Heroes.Count;
        AvailableHeroNextButton.Visible = count > _leftHero + 4;
        AvailableItemPreviousButton.Visible = _leftItem != 0;
        int fItemsCount = s.Items.Count;
        AvailableItemNextButton.Visible = fItemsCount > _leftItem + 4;
        AvailableUnitPreviousButton.Visible = _leftUnit != 0;
        int fUnitsCount = s.Units.Count;
        AvailableUnitNextButton.Visible = fUnitsCount > _leftUnit + 4;
        TransferHeroPreviousButton.Visible = _rightHero != 0;
        TransferHeroNextButton.Visible = _rightHero + 4 < s.SelectedHeroes.Count;
        TransferItemPreviousButton.Visible = _rightItem != 0;
        TransferItemNextButton.Visible = _rightItem + 4 < s.SelectedItems.Count;
        TransferUnitPreviousButton.Visible = _rightUnit != 0;
        TransferUnitNextButton.Visible = _rightUnit + 4 < s.SelectedUnits.Count;
        Book.IsDrawn = false;
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
        _ = AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        WorldMapCampaign? campaign = UI.WorldMap.Campaign;
        campaign.State += 1;
        UI.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:181
    public void CancelClick()
    {
        Book.Visible = false;
        UI.Engine.Campaign.ResetNextScenario();
        _ = AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        UI.WorldMap.Campaign.State = 1;
        UI.WorldMap.UpdateCampaignState();
    }

    // AoWCampaignTransferDlg.pas:190
    public void GoldSliderChange()
    {
        CampaignTransferSettings campaignTransferSettings = UI.Engine.Campaign.TransferSettings;
        campaignTransferSettings.SetSelectedTransferGold(GoldSlider.Position);

        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:197
    public void ManaSliderChange()
    {
        CampaignTransferSettings campaignTransferSettings = UI.Engine.Campaign.TransferSettings;
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

        AvailableHeroPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:211
    public void LHeroRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Heroes.Count > _leftHero + 4)
        {
            _leftHero += 1;
        }

        AvailableHeroPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:218
    public void LItemLBClick()
    {
        if (_leftItem != 0)
        {
            _leftItem -= 1;
        }

        AvailableItemPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:225
    public void LItemRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Items.Count > _leftItem + 4)
        {
            _leftItem += 1;
        }

        AvailableItemPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:232
    public void LUnitLBClick()
    {
        if (_leftUnit != 0)
        {
            _leftUnit -= 1;
        }

        AvailableUnitPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:239
    public void LUnitRBClick()
    {
        if (UI.Engine.Campaign.TransferSettings.Units.Count > _leftUnit + 4)
        {
            _leftUnit += 1;
        }

        AvailableUnitPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:246
    public void RHeroLBClick()
    {
        if (_rightHero != 0)
        {
            _rightHero -= 1;
        }

        TransferHeroPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:253
    public void RHeroRBClick()
    {
        if (_rightHero + 4 < UI.Engine.Campaign.TransferSettings.SelectedHeroes.Count)
        {
            _rightHero += 1;
        }

        TransferHeroPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:260
    public void RItemLBClick()
    {
        if (_rightItem != 0)
        {
            _rightItem -= 1;
        }

        TransferItemPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:267
    public void RItemRBClick()
    {
        if (_rightItem + 4 < UI.Engine.Campaign.TransferSettings.SelectedItems.Count)
        {
            _rightItem += 1;
        }

        TransferItemPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:274
    public void RUnitLBClick()
    {
        if (_rightUnit != 0)
        {
            _rightUnit -= 1;
        }

        TransferUnitPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:281
    public void RUnitRBClick()
    {
        if (_rightUnit + 4 < UI.Engine.Campaign.TransferSettings.SelectedUnits.Count)
        {
            _rightUnit += 1;
        }

        TransferUnitPanel.IsDrawn = false;
        UpdateControls();
    }

    // AoWCampaignTransferDlg.pas:288
    public void LHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Rect r;
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        Hero objectValue = s.Heroes.GetHero(_leftHero + slot);
        if (objectValue is not null)
        {
            CheckBox checkBox = sender;
            r = new Rect(AvailableHeroPanel.AbsoluteLeft + checkBox.WinLeft + 1, AvailableHeroPanel.AbsoluteTop + checkBox.WinTop + 2, AvailableHeroPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 2, AvailableHeroPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

            r.Left += 1;
            objectValue.ShowFace(r, lockedSurface);
            r.Left -= 1;
            if (s.SelectedHeroes.IndexOf(_leftHero + slot) != -1)
            {
                UI.Graphics.CityWindow[5].ShowBlended(r.Left, r.Top, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
            UI.Fonts.Age8.Color = 0xFF_FF_FF;
            int campaignTransferPoints = objectValue.GetCampaignTransferPoints();
            UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:316
    public void RHeroesDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Rect r;
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        if (_rightHero + slot < s.SelectedHeroes.Count)
        {
            int selectedHero = s.SelectedHeroes[_rightHero + slot];
            Hero objectValue = s.Heroes.GetHero(selectedHero);
            if (objectValue is not null)
            {
                CheckBox checkBox = sender;
                r = new Rect(TransferHeroPanel.AbsoluteLeft + checkBox.WinLeft + 1, TransferHeroPanel.AbsoluteTop + checkBox.WinTop + 2, TransferHeroPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 2, TransferHeroPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

                r.Left += 1;
                objectValue.ShowFace(r, lockedSurface);
                r.Left -= 1;
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 0xFF_FF_FF;
                int campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:345
    public void LUnitsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        UnitShowInfo info = default;
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        int count = s.Units.Count;
        if (_leftUnit + slot < count)
        {
            Troop? objectValue = s.Units.GetUnit(_leftUnit + slot);
            if (objectValue is not null)
            {
                CheckBox checkBox = sender;
                Rect r = new(AvailableUnitPanel.AbsoluteLeft + checkBox.WinLeft + 1, AvailableUnitPanel.AbsoluteTop + checkBox.WinTop + 2, AvailableUnitPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 2, AvailableUnitPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(r.Left + 1, r.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = UnitOverlayModes.Rank;
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                _ = objectValue.Show(r.Left - 8, r.Top, lockedSurface, info);
                if (s.SelectedUnits.IndexOf(_leftUnit + slot) != -1)
                {
                    UI.Graphics.CityWindow[5].ShowBlended(r.Left, r.Top, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[65]);
                }

                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 0xFF_FF_FF;
                int campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:381
    public void RUnitsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        UnitShowInfo info = default;
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        if (_rightUnit + slot < s.SelectedUnits.Count)
        {
            int selectedUnit = s.SelectedUnits[_rightUnit + slot];
            Troop? objectValue = s.Units.GetUnit(selectedUnit);
            if (objectValue is not null)
            {
                CheckBox checkBox = sender;
                Rect r = new(TransferUnitPanel.AbsoluteLeft + checkBox.WinLeft + 1, TransferUnitPanel.AbsoluteTop + checkBox.WinTop + 2, TransferUnitPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 2, TransferUnitPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(r.Left + 1, r.Top, lockedSurface);
                info.Direction = 3;
                info.Overlays = UnitOverlayModes.Rank;
                info.Animation = UnitAnimation.StandStill;
                info.Frame = 0;
                info.Looped = false;
                _ = objectValue.Show(r.Left - 8, r.Top, lockedSurface, info);
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 0xFF_FF_FF;
                int campaignTransferPoints = objectValue.GetCampaignTransferPoints();
                UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:415
    public void LItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        Item? objectValue = s.Items.GetItem(_leftItem + slot);
        if (objectValue is not null)
        {
            CheckBox checkBox = sender;
            Rect r = new(AvailableItemPanel.AbsoluteLeft + checkBox.WinLeft + 2, AvailableItemPanel.AbsoluteTop + checkBox.WinTop + 2, AvailableItemPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 4, AvailableItemPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

            UI.Graphics.CityWindow[4].Show(r.Left, r.Top, lockedSurface);
            objectValue.Show(r.Left + 5, r.Top + 10, surface: lockedSurface);
            if (s.SelectedItems.IndexOf(_leftItem + slot) != -1)
            {
                UI.Graphics.CityWindow[5].ShowBlended(r.Left, r.Top, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[65]);
            }

            if (sender.Lit)
            {
                UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
            }

            UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
            UI.Fonts.Age8.Color = 0xFF_FF_FF;
            int campaignTransferPoints = objectValue.CampaignTransferPoints;
            UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
        }
    }

    // AoWCampaignTransferDlg.pas:442
    public void RItemsDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        if (_rightItem + slot < s.SelectedItems.Count)
        {
            int selectedItem = s.SelectedItems[_rightItem + slot];
            Item? objectValue = s.Items.GetItem(selectedItem);
            if (objectValue is not null)
            {
                CheckBox checkBox = sender;
                Rect r = new(TransferItemPanel.AbsoluteLeft + checkBox.WinLeft + 2, TransferItemPanel.AbsoluteTop + checkBox.WinTop + 2, TransferItemPanel.AbsoluteLeft + checkBox.WinLeft + checkBox.WinWidth - 4, TransferItemPanel.AbsoluteTop + checkBox.WinTop + checkBox.WinHeight - 2);

                UI.Graphics.CityWindow[4].Show(r.Left, r.Top, lockedSurface);
                objectValue.Show(r.Left + 5, r.Top + 10, surface: lockedSurface);
                if (sender.Lit)
                {
                    UI.Graphics.CampaignTransfer[20].ShowBlended(r.Left - 1, r.Top - 2, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[50]);
                }

                UI.Graphics.CampaignTransfer[31].Show(r.Left + 10, r.Top + 55, lockedSurface);
                UI.Fonts.Age8.Color = 0xFF_FF_FF;
                int campaignTransferPoints = objectValue.CampaignTransferPoints;
                UI.Fonts.Age8.SetTextCentered(r.Left + 24, r.Top + 56, text: campaignTransferPoints.ToString(CultureInfo.InvariantCulture), lockedSurface);
            }
        }
    }

    // AoWCampaignTransferDlg.pas:470
    public void LHeroesSelect(CheckBox sender, int slot)
    {
        if (sender.Checked)
        {
            ClearLeftSelection();
            CheckBox checkBox = sender;
            checkBox.Checked = true;
            _leftKind = 1;
            _leftSelection = slot + _leftHero;
        }
    }

    // AoWCampaignTransferDlg.pas:484
    public void LItemsSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        CheckBox checkBox = sender;
        checkBox.Checked = true;
        _leftKind = 2;
        _leftSelection = slot + _leftItem;
    }

    // AoWCampaignTransferDlg.pas:495
    public void LUnitSelect(CheckBox sender, int slot)
    {
        ClearLeftSelection();
        CheckBox checkBox = sender;
        checkBox.Checked = true;
        _leftKind = 3;
        _leftSelection = slot + _leftUnit;
    }

    // AoWCampaignTransferDlg.pas:506
    public void RHeroesSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        CheckBox checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 1;
        _rightSelection = slot + _rightHero;
    }

    // AoWCampaignTransferDlg.pas:517
    public void RItemsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        CheckBox checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 2;
        _rightSelection = slot + _rightItem;
    }

    // AoWCampaignTransferDlg.pas:528
    public void RUnitsSelect(CheckBox sender, int slot)
    {
        ClearRightSelection();
        CheckBox checkBox = sender;
        checkBox.Checked = true;
        _rightKind = 3;
        _rightSelection = slot + _rightUnit;
    }

    // AoWCampaignTransferDlg.pas:539
    public void AddBtnClick()
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        switch (_leftKind)
        {
            case 1:
                if (s.AddHero(_leftSelection))
                {
                    AvailableHeroPanel.IsDrawn = false;
                    TransferHeroPanel.IsDrawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 2:
                if (s.AddItem(_leftSelection))
                {
                    AvailableItemPanel.IsDrawn = false;
                    TransferItemPanel.IsDrawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            case 3:
                if (s.AddUnit(_leftSelection))
                {
                    AvailableUnitPanel.IsDrawn = false;
                    TransferUnitPanel.IsDrawn = false;
                    UpdateControls();
                    ClearLeftSelection();
                }

                break;
            default:
                break;
        }
    }

    // AoWCampaignTransferDlg.pas:569
    public void RemoveBtnClick()
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        switch (_rightKind)
        {
            case 1:
                if (s.DeleteHero(s.SelectedHeroes[_rightSelection]))
                {
                    AvailableHeroPanel.IsDrawn = false;
                    TransferHeroPanel.IsDrawn = false;
                    TransferPanel.IsDrawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.IsDrawn = false;
                }

                break;
            case 2:
                if (s.DeleteItem(s.SelectedItems[_rightSelection]))
                {
                    AvailableItemPanel.IsDrawn = false;
                    TransferItemPanel.IsDrawn = false;
                    TransferPanel.IsDrawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.IsDrawn = false;
                }

                break;
            case 3:
                if (s.DeleteUnit(s.SelectedUnits[_rightSelection]))
                {
                    AvailableUnitPanel.IsDrawn = false;
                    TransferUnitPanel.IsDrawn = false;
                    TransferPanel.IsDrawn = false;
                    UpdateControls();
                    ClearRightSelection();
                    Book.IsDrawn = false;
                }

                break;
            default:
                break;
        }
    }

    // AoWCampaignTransferDlg.pas:605
    public void LHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 1 && slot + _leftHero == _leftSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && s.Heroes.Count > slot + _leftHero && s.SelectedHeroes.IndexOf(slot + _leftHero) == -1;
    }

    // AoWCampaignTransferDlg.pas:617
    public void LItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 2 && slot + _leftItem == _leftSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && s.Items.Count > slot + _leftItem && s.SelectedItems.IndexOf(slot + _leftItem) == -1;
    }

    // AoWCampaignTransferDlg.pas:629
    public void LUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _leftKind == 3 && slot + _leftUnit == _leftSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && s.Units.Count > slot + _leftUnit && s.SelectedUnits.IndexOf(slot + _leftUnit) == -1;
    }

    // AoWCampaignTransferDlg.pas:641
    public void RHeroesUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 1 && slot + _rightHero == _rightSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightHero < s.SelectedHeroes.Count;
    }

    // AoWCampaignTransferDlg.pas:652
    public void RItemsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 2 && slot + _rightItem == _rightSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightItem < s.SelectedItems.Count;
    }

    // AoWCampaignTransferDlg.pas:663
    public void RUnitsUpdate(CheckBox sender, int slot)
    {
        CampaignTransferSettings s = UI.Engine.Campaign.TransferSettings;
        sender.Checked = _rightKind == 3 && slot + _rightUnit == _rightSelection;
        CheckBox checkBox = sender;
        checkBox.CaptureInput = !checkBox.Checked && slot + _rightUnit < s.SelectedUnits.Count;
    }

    // AoWCampaignTransferDlg.pas:674
    public void AutoBtnClick()
    {
        UI.Engine.Campaign.TransferSettings.AutoSelect();
        UpdateControls();
        Book.IsDrawn = false;
    }

    // AoW.exe pushes "Scenario\Campaign\"
    public void Execute()
    {
        Campaign? campaign = UI.Engine.Campaign;
        ResetSelection();
        campaign.TransferSettings.SetupSelection(FileBoundary.ReadName($"{UI.Engine.StartupDirectory}Scenario\\Campaign\\{campaign.Scenarios.GetScenario(campaign.NextScenarioId).FileName}"));
        if (campaign.TransferSettings.TransferPointsMax > 0)
        {
            GoldSlider.Max = campaign.TransferSettings.TransferGoldMax;
            ManaSlider.Max = campaign.TransferSettings.TransferManaMax;
            UpdateControls();
            Book.Visible = true;
        }
        else
        {
            BookStartClick();
        }
    }
}
