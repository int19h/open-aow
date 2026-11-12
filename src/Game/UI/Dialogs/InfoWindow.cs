// Translated from AoWInfoWin.pas.
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// item bag
public partial class InfoWindow : UIModule
{
    public InfoWindow(UIContext ui) : base(ui)
    { }

    public Window MainWindow;
    public Label TitleLabel;
    public Button MainButton;
    public ArnoLine StructureLine;
    public Label StructureNameLabel;
    public Panel ProductionPlacePanel;
    public Memo ActiveActionMemo;
    public Panel SignPanel;
    public Memo SignTextMemo;
    public Panel CityPanel;
    public Label CityRaceLabel;
    public Panel CityBorderPanel;
    public Image ProductionOrb;
    public Panel SingleButtonPanel;
    public Memo SingleButtonMemo;
    public Panel AltarPanel;
    public Gauge AltarChargeBar;
    public Label GemLabel;
    public Label ChargePercentLabel;
    public Label DayLabel;
    public Image OwnerShield;
    public Panel NotYoursPanel;
    public Memo NotYoursMemo;
    public Label OwnerRaceLabel;
    public Label ProductionOrbTurnsLabel;
    public Image ProductionOrbTimeImage;
    public Panel ConstructionPanel;
    public Memo ConstructionMemo;
    public Label ConstructionDaysLeftLabel;
    public Panel RazePanel;
    public Button RazeButton;
    public Label CivilStatusLabel;
    public Image RaceRelationIcon;
    public Label CityIncomeLabel;
    public Label CityLevelLabel;
    public Image IncomeTypeIcon;
    public Label IncomeValueLabel;
    public Memo ActiveCityActionMemo;
    public Label IncomeLabel;
    public Label ProductionWarningLabel;
    public Label CurrentActivityWarningLabel;
    public Panel IncomePanel;
    public Image IncomeTypeImage;
    public Label IncomeAmountLabel;
    public Memo IncomeMemo;
    public Panel ItemPanel;
    public Label ItemsInBagLabel;
    public ListBox ItemBagList;
    public VerticalScrollBar ItemBagScrollBar;
    public Memo ItemBagMemo;
    public Panel ExplorationSitePanel;
    public Memo ExplorationSiteMemo;
    public Label ExplorationSiteLabel;
    public Label ExplorationSiteStatusLabel;
    public Label PopulationRaceLabel;
    public Label PopulationLabel;

    public ItemHexSprite ItemBag { get; set; }
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static Catalog<StructureView, Action<InfoWindow, Structure>> StructurePanels { get; } = new("structure info panel");
    public static Catalog<StructureView, Action<InfoWindow, Structure>> StructureButtons { get; } = new("structure info button");

    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow._windowManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
    }

    // AoWInfoWin.pas:146
    public void MinClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWInfoWin.pas:152
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
        UI.TaskBar.StructureButton.Visible = false;
    }

    // AoWInfoWin.pas:159
    public void ProdOrb2Draw()
    {
    }

    // AoWInfoWin.pas:162
    public void ProdOrb1Draw()
    {
    }

    // AoWInfoWin.pas:165
    public void CenterClick()
    {
        UI.General.PlayButtonSound(0);
        if (ItemBag != null)
        {
            var itemHexSprite = ItemBag;
            var hexY = ItemBag.HexY;
            var hexX = itemHexSprite.HexX;
            StrategicMap.Current.CenterViewTo(hexX, hexY, ItemBag.HexLevel);

            var fItemBagHexY = ItemBag.HexY;
            var fItemBagHexX = ItemBag.HexX;
            StrategicMap.Current.PlayCenterAnimation(fItemBagHexX, fItemBagHexY, ItemBag.HexLevel);
        }
        else
        {
            var hexY = UI.General.Structure.HexY;
            var hexX = UI.General.Structure.HexX;
            StrategicMap.Current.CenterViewTo(hexX, hexY, UI.General.Structure.HexLevel);
            var fStructureHexY = UI.General.Structure.HexY;
            var fStructureHexX = UI.General.Structure.HexX;
            StrategicMap.Current.PlayCenterAnimation(fStructureHexX, fStructureHexY, UI.General.Structure.HexLevel);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string reason = "";
        UI.General.PlayButtonSound(0);
        if (UI.General.Structure != null)
        {
            if (UI.General.Structure.CanRaze(StrategicMap.Current.SeatedPlayer, ref reason))
            {
                UI.General.Structure.Raze(StrategicMap.Current.SeatedPlayer);
            }
            else
            {
                StrategicMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        ArmyHexSprite army;
        UI.General.PlayButtonSound(0);
        if (UI.General.Structure != null)
        {
            var structure = UI.General.Structure;
            if (StructureButtons.TryGet(structure.InfoButton(StrategicMap.Current.SeatedPlayer), out var open))
            {
                open(this, structure);
            }
        }
        else
        {
            if (ItemBag != null)
            {
                army = ItemBag.Owner.GetArmyHexSprite();
                if (army != null && army.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    if (army.Army.Hero(255, RosterTables.AllHeroTypes) != null)
                    {
                        var hero = army.Army.Hero(255, RosterTables.AllHeroTypes);
                        UI.General.SelectedUnit = hero;
                        UI.UnitWindow.ShowUnit();
                        UI.UnitWindow.UnitTabs.Selected = 2;
                    }
                }
            }
        }
    }

    // AoWInfoWin.pas:226
    public void ItembagLBDrawItem(in Rect drawRectangle, int drawItem, in LockedSurface surface)
    {
        if (ItemBag != null)
        {
            // { Original uses the scroll offset for every row. }
            if (ItemBag.Items.Count > ItemBagList.ListOffset)
            {
                ItemBag.Items.GetItem(ItemBagList.ListOffset).Show(drawRectangle.Left + 10, drawRectangle.Top + 10, surface);
            }
        }
    }

    // AoWInfoWin.pas:233
    public void HelpClick()
    {
        UI.HelpWindow.ShowHelp(504);
    }

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ExplorationSiteMemo.Drawn = false;
        MainWindow.Drawn = false;
    }

    internal static void SetStructureHeader(InfoWindow dialog, Structure structure)
    {
        var name = structure.GetName();
        dialog.StructureNameLabel.Text = name;
        if (structure.GetBottomBorderImage() != null)
        {
            var topBorderImage = structure.GetTopBorderImage();
            dialog.StructureLine.ImageTop = topBorderImage;
            var middleBorderImage = structure.GetMiddleBorderImage();
            dialog.StructureLine.ImageMiddle = middleBorderImage;
            var bottomBorderImage = structure.GetBottomBorderImage();
            dialog.StructureLine.ImageBottom = bottomBorderImage;
        }

        dialog.StructureLine.Visible = true;
    }

    internal static void ShowMineInfo(InfoWindow dialog, Mine structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 31;
        var income = structure.GetIncome();
        dialog.IncomeAmountLabel.Text = IntToStr(income);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowPowerNodeInfo(InfoWindow dialog, PowerNode structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        var power = structure.GetPower();
        dialog.IncomeAmountLabel.Text = IntToStr(power);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowProductionIncome(InfoWindow dialog, ProductionPlace structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        var power = structure.GetPower();
        dialog.IncomeAmountLabel.Text = IntToStr(power);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowFarmInfo(InfoWindow dialog, Farm structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        dialog.SingleButtonMemo.Items.Add("");
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.IncomeFormat), [IntToStr(structure.Income)]);
        dialog.SingleButtonMemo.Items.Add(format);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowSignInfo(InfoWindow dialog, Sign structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.UI.General.Sign = structure;
        dialog.SignPanel.Visible = true;
        dialog.SignTextMemo.Items.Assign(structure.Text);
    }

    internal static void ShowTeleportInfo(InfoWindow dialog, Teleport structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = dialog.UI.General.Structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        dialog.SingleButtonMemo.Sized = false;
        var canTeleport = structure.CanTeleport(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canTeleport;
        var teleport = AbilityRules.TranslateResourceString(Teleport.Resource.TeleportText);
        dialog.MainButton.Text = teleport;
        dialog.RazePanel.Visible = dialog.MainButton.Visible;
    }

    internal static void ShowTowerInfo(InfoWindow dialog, WizardsTower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        var canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        var enter = AbilityRules.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
    }

    internal static void ShowArenaInfo(InfoWindow dialog, Arena structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        var canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        var enter = AbilityRules.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
    }

    internal static void ShowCaveInfo(InfoWindow dialog, Cave structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        var canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        switch (structure.Entrance)
        {
            case CaveEntrance.Up:
                var goUp = AbilityRules.TranslateResourceString(GameText.GoUpText);
                dialog.MainButton.Text = goUp;
                break;
            case CaveEntrance.Down:
                var goDown = AbilityRules.TranslateResourceString(GameText.GoDownText);
                dialog.MainButton.Text = goDown;
                break;
        }
    }

    internal static void ShowWatchTowerInfo(InfoWindow dialog, Tower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowExplorationInfo(InfoWindow dialog, ExplorationSite structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ExplorationSitePanel.Visible = true;
        var description = dialog.UI.General.Structure.GetDescription();
        dialog.ExplorationSiteMemo.Items.Assign(description);
        var status = AbilityRules.TranslateResourceString(CityScreen.StatusText);
        dialog.ExplorationSiteLabel.Text = status + ":";
        if (structure.GetExplored())
        {
            var explored = AbilityRules.TranslateResourceString(GameText.ExploredText);
            dialog.ExplorationSiteStatusLabel.Text = explored;
        }
        else
        {
            var unexplored = AbilityRules.TranslateResourceString(GameText.UnexploredText);
            dialog.ExplorationSiteStatusLabel.Text = unexplored;
        }

        var canSearch = ((ExplorationSite)dialog.UI.General.Structure).CanSearch(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canSearch;
        var search = AbilityRules.TranslateResourceString(GameText.SearchText);
        dialog.MainButton.Text = search;
    }

    internal static void ShowAltarInfo(InfoWindow dialog, Altar structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.AltarPanel.Visible = true;
        if (structure.GetTargeting())
        {
            var cancel = AbilityRules.TranslateResourceString(GameText.CancelText);
            dialog.MainButton.Text = cancel;
        }
        else
        {
            var target = AbilityRules.TranslateResourceString(GameText.TargetText);
            dialog.MainButton.Text = target;
        }

        dialog.MainButton.Visible = true;
        var rechargeDays = structure.GetRechargeDays();
        dialog.AltarChargeBar.Max = rechargeDays;
        dialog.AltarChargeBar.Position = structure.RechargeDays;
        var structureRechargeDays = structure.GetRechargeDays();
        dialog.ChargePercentLabel.Text = IntToStr(Round((double)(structure.RechargeDays * 100) / structureRechargeDays)) + "%";
        var rechargeDays2 = structure.GetRechargeDays();
        dialog.DayLabel.Text = IntToStr(rechargeDays2 - structure.RechargeDays);
        var targetMana = structure.TargetMana();
        dialog.GemLabel.Text = IntToStr(targetMana);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowWarCityInfo(InfoWindow dialog, City structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        dialog.SingleButtonMemo.Items.Add("");
        dialog.SingleButtonMemo.Items.Add("");
        dialog.SingleButtonMemo.Items.Add("");
        dialog.SingleButtonMemo.Items.Add("");
        dialog.SingleButtonMemo.Items.Add("");
        dialog.SingleButtonMemo.Items.Add("");
        var statusWarMode = AbilityRules.TranslateResourceString(GameText.StatusWarModeText);
        dialog.SingleButtonMemo.Items.Add(statusWarMode);
    }

    internal static void ShowCityInfo(InfoWindow dialog, City structure)
    {
        string reason = "";
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        var enter = AbilityRules.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
        dialog.MainButton.Visible = true;
        dialog.UI.General.City = structure;
        dialog.CityPanel.Visible = true;
        dialog.ProductionWarningLabel.Visible = false;
        dialog.ActiveCityActionMemo.Items.Clear();
        var name = dialog.UI.General.City.GetName();
        dialog.StructureNameLabel.Text = name;
        var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[dialog.UI.General.City.Race]);
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.RaceFormat), [raceNames]);
        dialog.CityRaceLabel.Text = format;
        dialog.CityLevelLabel.Text = IntToStr(structure.UpgradeLevel);
        dialog.CityIncomeLabel.Text = IntToStr(structure.Income);
        var civilStatus = structure.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateResourceString(CityScreen.CivilStatusTexts[(int)civilStatus]);
        var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.StatusFormat), [civilStatusText]);
        dialog.CivilStatusLabel.Text = formattedText;
        if (structure.Production.Queue.GetCount() != 0)
        {
            structure.Production.Queue.GetItem(0).Order.ActivityLines(structure.Production, dialog.ActiveCityActionMemo.Items);

            if (structure.Production.Status == ProductionControlStatus.Producing)
            {
                if (structure.Production.Settings.TurnsLeft != -1)
                {
                    var formatTurnsLeftString = DurationText.FormatTurnsLeftString(structure.Production.Settings.TurnsLeft);
                    dialog.ActiveCityActionMemo.Items.Add(formatTurnsLeftString);
                }
                else
                {
                    var unlimited = AbilityRules.TranslateResourceString(MapEvents.UnlimitedText);
                    dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.Production.Status == ProductionControlStatus.Waiting)
                {
                    var queue = structure.Production.Queue.GetItem(0);
                    structure.Production.CanExecuteProduction(queue, ref reason);
                    dialog.ProductionWarningLabel.Text = reason;
                    dialog.ProductionWarningLabel.Visible = true;
                }
            }
        }
        else
        {
            dialog.ActiveCityActionMemo.Items.Add("");
            var producingMerchandise = AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText);
            dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = StrategicMap.Current.Races.GetRace(structure.Race).GetPlayerRelation(structure.Player);
        dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowProductionInfo(InfoWindow dialog, ProductionPlace structure)
    {
        string reason = "";
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        var enter = AbilityRules.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
        dialog.MainButton.Visible = true;
        dialog.UI.General.ProductionPlace = (ProductionPlace)dialog.UI.General.Structure;
        dialog.ProductionPlacePanel.Visible = true;
        dialog.UI.General.ProductionControl = dialog.UI.General.ProductionPlace.ProductionControl;
        dialog.CurrentActivityWarningLabel.Visible = false;
        dialog.ActiveActionMemo.Items.Clear();
        dialog.IncomeLabel.Visible = true;
        dialog.IncomeTypeIcon.Visible = true;
        dialog.IncomeValueLabel.Visible = true;
        if (structure.GetMagicSphere() != MagicSphere.None)
        {
            dialog.IncomeTypeIcon.CurrentImage = 33;
            var power = structure.GetPower();
            dialog.IncomeValueLabel.Text = IntToStr(power);
        }
        else
        {
            if (structure.GetBaseIncome() != 0)
            {
                dialog.IncomeTypeIcon.CurrentImage = 31;
                var income = structure.GetIncome();
                dialog.IncomeValueLabel.Text = IntToStr(income);
            }
            else
            {
                dialog.IncomeLabel.Visible = false;
                dialog.IncomeTypeIcon.Visible = false;
                dialog.IncomeValueLabel.Visible = false;
            }
        }

        dialog.ProductionPlacePanel.Visible = true;
        var name = structure.GetName();
        dialog.StructureNameLabel.Text = name;
        if (structure.ProductionControl.Queue.GetCount() != 0)
        {
            structure.ProductionControl.Queue.GetItem(0).Order.ActivityLines(structure.ProductionControl, dialog.ActiveActionMemo.Items);

            if (structure.ProductionControl.Status == ProductionControlStatus.Producing)
            {
                if (structure.ProductionControl.Settings.TurnsLeft != -1)
                {
                    var formatTurnsLeftString = DurationText.FormatTurnsLeftString(structure.ProductionControl.Settings.TurnsLeft);
                    dialog.ActiveActionMemo.Items.Add(formatTurnsLeftString);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    var unlimited = AbilityRules.TranslateResourceString(MapEvents.UnlimitedText);
                    dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    var queue = structure.ProductionControl.Queue.GetItem(0);
                    structure.ProductionControl.CanExecuteProduction(queue, ref reason);
                    dialog.CurrentActivityWarningLabel.Text = reason;
                    dialog.CurrentActivityWarningLabel.Visible = true;
                }
            }
        }
        else
        {
            if (structure.GetMagicSphere() == MagicSphere.None)
            {
                dialog.ActiveActionMemo.Items.Add("");
                if (structure.GetBaseIncome() != 0)
                {
                    var producingMerchandise = AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText);
                    dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
                    dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = AbilityRules.TranslateResourceString(GameText.GeneratingExtraPowerText);
                dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        dialog.RazePanel.Visible = true;
    }

    internal static void ShowBuildingInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ConstructionPanel.Visible = true;
        var formatDaysLeftString = DurationText.FormatDaysLeftString(structure.BuildingDays);
        dialog.ConstructionDaysLeftLabel.Text = formatDaysLeftString;
        var description = structure.GetDescription();
        dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(InfoWindow dialog, Structure structure)
    {
        string text;
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        if (structure is City)
        {
            var name = structure.GetName();
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.CityFormat), [name]);
        }
        else
        {
            text = structure.GetName();
        }

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LiesInRuinsFormat), [text]);
        dialog.SingleButtonMemo.Items.Text = format;
        dialog.SingleButtonMemo.Items.Insert(0, "");
        dialog.SingleButtonMemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(InfoWindow dialog, PlayerStructure structure)
    {
        int raceIndex;
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.NotYoursPanel.Visible = true;
        if (structure is City && structure.Player == 0)
        {
            dialog.PopulationRaceLabel.Visible = true;
            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[((City)structure).Race]);
            dialog.PopulationRaceLabel.Text = raceNamesPlural;
            dialog.PopulationLabel.Visible = true;
            dialog.NotYoursMemo.Items.Clear();
        }
        else
        {
            var description = structure.GetDescription();
            dialog.NotYoursMemo.Items.Assign(description);
        }

        if (structure.Player != -1)
        {
            raceIndex = StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;
            if (raceIndex == -1)
            {
                raceIndex = 12;
            }

            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[raceIndex]);
            dialog.OwnerRaceLabel.Text = raceNamesPlural;
            dialog.OwnerRaceLabel.Visible = true;
            dialog.OwnerShield.CurrentImage = raceIndex + 26;
            dialog.OwnerShield.Visible = true;
        }
        else
        {
            dialog.OwnerRaceLabel.Visible = false;
            dialog.OwnerShield.Visible = false;
        }
    }

    internal static void ShowGenericInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        var description = structure.GetDescription();
        dialog.SingleButtonMemo.Items.Assign(description);
    }

}
