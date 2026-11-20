#nullable disable
// Translated from AoWInfoWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.UI.Dialogs;

// item bag
public partial class InfoWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public Label TitleLabel { get; private set; }
    public Button MainButton { get; private set; }
    public ArnoLine StructureLine { get; private set; }
    public Label StructureNameLabel { get; private set; }
    public Panel ProductionPlacePanel { get; private set; }
    public Memo ActiveActionMemo { get; private set; }
    public Panel SignPanel { get; private set; }
    public Memo SignTextMemo { get; private set; }
    public Panel CityPanel { get; private set; }
    public Label CityRaceLabel { get; private set; }
    public Panel CityBorderPanel { get; private set; }
    public Image ProductionOrb { get; private set; }
    public Panel SingleButtonPanel { get; private set; }
    public Memo SingleButtonMemo { get; private set; }
    public Panel AltarPanel { get; private set; }
    public Gauge AltarChargeBar { get; private set; }
    public Label GemLabel { get; private set; }
    public Label ChargePercentLabel { get; private set; }
    public Label DayLabel { get; private set; }
    public Image OwnerShield { get; private set; }
    public Panel NotYoursPanel { get; private set; }
    public Memo NotYoursMemo { get; private set; }
    public Label OwnerRaceLabel { get; private set; }
    public Label ProductionOrbTurnsLabel { get; private set; }
    public Image ProductionOrbTimeImage { get; private set; }
    public Panel ConstructionPanel { get; private set; }
    public Memo ConstructionMemo { get; private set; }
    public Label ConstructionDaysLeftLabel { get; private set; }
    public Panel RazePanel { get; private set; }
    public Button RazeButton { get; private set; }
    public Label CivilStatusLabel { get; private set; }
    public Image RaceRelationIcon { get; private set; }
    public Label CityIncomeLabel { get; private set; }
    public Label CityLevelLabel { get; private set; }
    public Image IncomeTypeIcon { get; private set; }
    public Label IncomeValueLabel { get; private set; }
    public Memo ActiveCityActionMemo { get; private set; }
    public Label IncomeLabel { get; private set; }
    public Label ProductionWarningLabel { get; private set; }
    public Label CurrentActivityWarningLabel { get; private set; }
    public Panel IncomePanel { get; private set; }
    public Image IncomeTypeImage { get; private set; }
    public Label IncomeAmountLabel { get; private set; }
    public Memo IncomeMemo { get; private set; }
    public Panel ItemPanel { get; private set; }
    public Label ItemsInBagLabel { get; private set; }
    public ListBox ItemBagList { get; private set; }
    public VerticalScrollBar ItemBagScrollBar { get; private set; }
    public Memo ItemBagMemo { get; private set; }
    public Panel ExplorationSitePanel { get; private set; }
    public Memo ExplorationSiteMemo { get; private set; }
    public Label ExplorationSiteLabel { get; private set; }
    public Label ExplorationSiteStatusLabel { get; private set; }
    public Label PopulationRaceLabel { get; private set; }
    public Label PopulationLabel { get; private set; }

    public ItemHexSprite ItemBag { get; set; }
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static Catalog<StructureView, Action<InfoWindow, Structure>> StructurePanels { get; } = new("structure info panel");
    public static Catalog<StructureView, Action<InfoWindow, Structure>> StructureButtons { get; } = new("structure info button");

    // The view key names the kind. Another kind throws, and null stays null.
    internal static void AddPanel<TStructure>(StructureView view, Action<InfoWindow, TStructure> fill)
        where TStructure : Structure
        => StructurePanels.Add(view, value: (dialog, structure) => fill(dialog, (TStructure)structure));

    // The view key names the kind. Another kind throws, and null stays null.
    internal static void AddButton<TStructure>(StructureView view, Action<InfoWindow, TStructure> open)
        where TStructure : Structure
        => StructureButtons.Add(view, value: (dialog, structure) => open(dialog, (TStructure)structure));

    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        MainWindow.Alignment.RightOffset = MainWindow.WindowManager.WinWidth - MainWindow.WinLeft - MainWindow.WinWidth;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
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
        if (ItemBag is not null)
        {
            ItemHexSprite itemHexSprite = ItemBag;
            sbyte hexY = ItemBag.HexY;
            sbyte hexX = itemHexSprite.HexX;
            StrategicMap.Current.CenterViewTo(hexX, hexY, ItemBag.HexLevel);

            sbyte fItemBagHexY = ItemBag.HexY;
            sbyte fItemBagHexX = ItemBag.HexX;
            StrategicMap.Current.PlayCenterAnimation(fItemBagHexX, fItemBagHexY, ItemBag.HexLevel);
        }
        else
        {
            sbyte hexY = UI.General.Structure.HexY;
            sbyte hexX = UI.General.Structure.HexX;
            StrategicMap.Current.CenterViewTo(hexX, hexY, UI.General.Structure.HexLevel);
            sbyte fStructureHexY = UI.General.Structure.HexY;
            sbyte fStructureHexX = UI.General.Structure.HexX;
            StrategicMap.Current.PlayCenterAnimation(fStructureHexX, fStructureHexY, UI.General.Structure.HexLevel);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.General.Structure is not null)
        {
            string reason = "";
            if (UI.General.Structure.CanRaze(StrategicMap.Current.SeatedPlayer, ref reason))
            {
                _ = UI.General.Structure.Raze(StrategicMap.Current.SeatedPlayer);
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
        UI.General.PlayButtonSound(0);
        if (UI.General.Structure is not null)
        {
            Structure structure = UI.General.Structure;
            if (StructureButtons.TryGet(structure.InfoButton(StrategicMap.Current.SeatedPlayer), out Action<InfoWindow, Structure> open))
            {
                open(this, structure);
            }
        }
        else
        {
            if (ItemBag is not null)
            {
                ArmyHexSprite army = ItemBag.Owner.GetArmyHexSprite();
                if (army is not null && army.Army.Player == StrategicMap.Current.SeatedPlayer)
                {
                    if (army.Army.Hero(255, RosterTables.AllHeroTypes) is not null)
                    {
                        Hero hero = army.Army.Hero(255, RosterTables.AllHeroTypes);
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
        if (ItemBag is not null)
        {
            // { Original uses the scroll offset for every row. }
            if (ItemBag.Items.Count > ItemBagList.ListOffset)
            {
                ItemBag.Items.GetItem(ItemBagList.ListOffset).Show(drawRectangle.Left + 10, drawRectangle.Top + 10, surface);
            }
        }
    }

    // AoWInfoWin.pas:233
    public void HelpClick() => UI.HelpWindow.ShowHelp(504);

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ExplorationSiteMemo.Drawn = false;
        MainWindow.Drawn = false;
    }

    internal static void SetStructureHeader(InfoWindow dialog, Structure structure)
    {
        string name = structure.GetName();
        dialog.StructureNameLabel.Text = name;
        if (structure.GetBottomBorderImage() is not null)
        {
            LibraryImage topBorderImage = structure.GetTopBorderImage();
            dialog.StructureLine.ImageTop = topBorderImage;
            LibraryImage middleBorderImage = structure.GetMiddleBorderImage();
            dialog.StructureLine.ImageMiddle = middleBorderImage;
            LibraryImage bottomBorderImage = structure.GetBottomBorderImage();
            dialog.StructureLine.ImageBottom = bottomBorderImage;
        }

        dialog.StructureLine.Visible = true;
    }

    internal static void ShowMineInfo(InfoWindow dialog, Mine structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 31;
        int income = structure.GetIncome();
        dialog.IncomeAmountLabel.Text = IntToStr(income);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowPowerNodeInfo(InfoWindow dialog, PowerNode structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        int power = structure.GetPower();
        dialog.IncomeAmountLabel.Text = IntToStr(power);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowProductionIncome(InfoWindow dialog, ProductionPlace structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        int power = structure.GetPower();
        dialog.IncomeAmountLabel.Text = IntToStr(power);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowFarmInfo(InfoWindow dialog, Farm structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        _ = dialog.SingleButtonMemo.Items.Add("");
        string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.IncomeFormat), [IntToStr(structure.Income)]);
        _ = dialog.SingleButtonMemo.Items.Add(format);
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
        DelphiStringCollection description = dialog.UI.General.Structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        dialog.SingleButtonMemo.Sized = false;
        bool canTeleport = structure.CanTeleport(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canTeleport;
        string teleport = dialog.UI.Engine.TranslateResourceString(Teleport.Resource.TeleportText);
        dialog.MainButton.Text = teleport;
        dialog.RazePanel.Visible = dialog.MainButton.Visible;
    }

    internal static void ShowTowerInfo(InfoWindow dialog, WizardsTower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        bool canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        string enter = dialog.UI.Engine.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
    }

    internal static void ShowArenaInfo(InfoWindow dialog, Arena structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        bool canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        string enter = dialog.UI.Engine.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
    }

    internal static void ShowCaveInfo(InfoWindow dialog, Cave structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        bool canEnter = structure.CanEnter(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canEnter;
        switch (structure.Entrance)
        {
            case CaveEntrance.Up:
                string goUp = dialog.UI.Engine.TranslateResourceString(GameText.GoUpText);
                dialog.MainButton.Text = goUp;
                break;
            case CaveEntrance.Down:
                string goDown = dialog.UI.Engine.TranslateResourceString(GameText.GoDownText);
                dialog.MainButton.Text = goDown;
                break;
            case CaveEntrance.Idle:
            default:
                break;
        }
    }

    internal static void ShowWatchTowerInfo(InfoWindow dialog, Tower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowExplorationInfo(InfoWindow dialog, ExplorationSite structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ExplorationSitePanel.Visible = true;
        DelphiStringCollection description = dialog.UI.General.Structure.Description;
        dialog.ExplorationSiteMemo.Items.Assign(description);
        string status = dialog.UI.Engine.TranslateResourceString(CityScreen.StatusText);
        dialog.ExplorationSiteLabel.Text = $"{status}:";
        if (structure.Explored)
        {
            string explored = dialog.UI.Engine.TranslateResourceString(GameText.ExploredText);
            dialog.ExplorationSiteStatusLabel.Text = explored;
        }
        else
        {
            string unexplored = dialog.UI.Engine.TranslateResourceString(GameText.UnexploredText);
            dialog.ExplorationSiteStatusLabel.Text = unexplored;
        }

        bool canSearch = structure.CanSearch(StrategicMap.Current.SeatedPlayer);
        dialog.MainButton.Visible = canSearch;
        string search = dialog.UI.Engine.TranslateResourceString(GameText.SearchText);
        dialog.MainButton.Text = search;
    }

    internal static void ShowAltarInfo(InfoWindow dialog, Altar structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.AltarPanel.Visible = true;
        if (structure.Targeting)
        {
            string cancel = dialog.UI.Engine.TranslateResourceString(GameText.CancelText);
            dialog.MainButton.Text = cancel;
        }
        else
        {
            string target = dialog.UI.Engine.TranslateResourceString(GameText.TargetText);
            dialog.MainButton.Text = target;
        }

        dialog.MainButton.Visible = true;
        byte rechargeDays = structure.GetRechargeDays();
        dialog.AltarChargeBar.Max = rechargeDays;
        dialog.AltarChargeBar.Position = structure.RechargeDays;
        byte structureRechargeDays = structure.GetRechargeDays();
        dialog.ChargePercentLabel.Text = $"{IntToStr(Round((double)(structure.RechargeDays * 100) / structureRechargeDays))}%";
        byte rechargeDays2 = structure.GetRechargeDays();
        dialog.DayLabel.Text = IntToStr(rechargeDays2 - structure.RechargeDays);
        int targetMana = structure.TargetMana();
        dialog.GemLabel.Text = IntToStr(targetMana);
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowWarCityInfo(InfoWindow dialog, City structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        _ = dialog.SingleButtonMemo.Items.Add("");
        _ = dialog.SingleButtonMemo.Items.Add("");
        _ = dialog.SingleButtonMemo.Items.Add("");
        _ = dialog.SingleButtonMemo.Items.Add("");
        _ = dialog.SingleButtonMemo.Items.Add("");
        _ = dialog.SingleButtonMemo.Items.Add("");
        string statusWarMode = dialog.UI.Engine.TranslateResourceString(GameText.StatusWarModeText);
        _ = dialog.SingleButtonMemo.Items.Add(statusWarMode);
    }

    internal static void ShowCityInfo(InfoWindow dialog, City structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        string enter = dialog.UI.Engine.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
        dialog.MainButton.Visible = true;
        dialog.UI.General.City = structure;
        dialog.CityPanel.Visible = true;
        dialog.ProductionWarningLabel.Visible = false;
        dialog.ActiveCityActionMemo.Items.Clear();
        string name = dialog.UI.General.City.GetName();
        dialog.StructureNameLabel.Text = name;
        string raceNames = dialog.UI.Engine.TranslateResourceString(GameTables.RaceNames[dialog.UI.General.City.Race]);
        string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.RaceFormat), [raceNames]);
        dialog.CityRaceLabel.Text = format;
        dialog.CityLevelLabel.Text = IntToStr(structure.UpgradeLevel);
        dialog.CityIncomeLabel.Text = IntToStr(structure.Income);
        City.CivilStatus civilStatus = structure.GetCivilStatus();
        string civilStatusText = dialog.UI.Engine.TranslateResourceString(CityScreen.CivilStatusTexts[(int)civilStatus]);
        string formattedText = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.StatusFormat), [civilStatusText]);
        dialog.CivilStatusLabel.Text = formattedText;
        if (structure.Production.Queue.GetCount() != 0)
        {
            structure.Production.Queue.GetItem(0).Order.ActivityLines(structure.Production, dialog.ActiveCityActionMemo.Items);

            if (structure.Production.Status == ProductionControlStatus.Producing)
            {
                if (structure.Production.Settings.TurnsLeft != -1)
                {
                    string formatTurnsLeftString = DurationText.FormatTurnsLeftString(dialog.UI.Engine, turns: structure.Production.Settings.TurnsLeft);
                    _ = dialog.ActiveCityActionMemo.Items.Add(formatTurnsLeftString);
                }
                else
                {
                    string unlimited = dialog.UI.Engine.TranslateResourceString(MapEvents.UnlimitedText);
                    _ = dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.Production.Status == ProductionControlStatus.Waiting)
                {
                    ProductionItem queue = structure.Production.Queue.GetItem(0);
                    string reason = "";
                    _ = structure.Production.CanExecuteProduction(queue, ref reason);
                    dialog.ProductionWarningLabel.Text = reason;
                    dialog.ProductionWarningLabel.Visible = true;
                }
            }
        }
        else
        {
            _ = dialog.ActiveCityActionMemo.Items.Add("");
            string producingMerchandise = dialog.UI.Engine.TranslateResourceString(GameText.ProducingMerchandiseText);
            _ = dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        Status playerRelation = StrategicMap.Current.Races.GetRace(structure.Race).GetPlayerRelation(structure.Player);
        dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        dialog.RazePanel.Visible = true;
    }

    internal static void ShowProductionInfo(InfoWindow dialog, ProductionPlace structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        string enter = dialog.UI.Engine.TranslateResourceString(GameText.EnterText);
        dialog.MainButton.Text = enter;
        dialog.MainButton.Visible = true;
        // The view key already named this production place. The selection stores that same object.
        dialog.UI.General.ProductionPlace = structure;
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
            int power = structure.GetPower();
            dialog.IncomeValueLabel.Text = IntToStr(power);
        }
        else
        {
            if (structure.BaseIncome != 0)
            {
                dialog.IncomeTypeIcon.CurrentImage = 31;
                int income = structure.GetIncome();
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
        string name = structure.GetName();
        dialog.StructureNameLabel.Text = name;
        if (structure.ProductionControl.Queue.GetCount() != 0)
        {
            structure.ProductionControl.Queue.GetItem(0).Order.ActivityLines(structure.ProductionControl, dialog.ActiveActionMemo.Items);

            if (structure.ProductionControl.Status == ProductionControlStatus.Producing)
            {
                if (structure.ProductionControl.Settings.TurnsLeft != -1)
                {
                    string formatTurnsLeftString = DurationText.FormatTurnsLeftString(dialog.UI.Engine, turns: structure.ProductionControl.Settings.TurnsLeft);
                    _ = dialog.ActiveActionMemo.Items.Add(formatTurnsLeftString);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    string unlimited = dialog.UI.Engine.TranslateResourceString(MapEvents.UnlimitedText);
                    _ = dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    ProductionItem queue = structure.ProductionControl.Queue.GetItem(0);
                    string reason = "";
                    _ = structure.ProductionControl.CanExecuteProduction(queue, ref reason);
                    dialog.CurrentActivityWarningLabel.Text = reason;
                    dialog.CurrentActivityWarningLabel.Visible = true;
                }
            }
        }
        else
        {
            if (structure.GetMagicSphere() == MagicSphere.None)
            {
                _ = dialog.ActiveActionMemo.Items.Add("");
                if (structure.BaseIncome != 0)
                {
                    string producingMerchandise = dialog.UI.Engine.TranslateResourceString(GameText.ProducingMerchandiseText);
                    _ = dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    string none = dialog.UI.Engine.TranslateResourceString(GameTables.NoneText);
                    _ = dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                string generatingExtraPower = dialog.UI.Engine.TranslateResourceString(GameText.GeneratingExtraPowerText);
                _ = dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        dialog.RazePanel.Visible = true;
    }

    internal static void ShowBuildingInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ConstructionPanel.Visible = true;
        string formatDaysLeftString = DurationText.FormatDaysLeftString(dialog.UI.Engine, days: structure.BuildingDays);
        dialog.ConstructionDaysLeftLabel.Text = formatDaysLeftString;
        DelphiStringCollection description = structure.Description;
        dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButtonPanel.Visible = true;
        string text;
        if (structure is City)
        {
            string name = structure.GetName();
            text = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.CityFormat), [name]);
        }
        else
        {
            text = structure.GetName();
        }

        string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.LiesInRuinsFormat), [text]);
        dialog.SingleButtonMemo.Items.Text = format;
        dialog.SingleButtonMemo.Items.Insert(0, "");
        dialog.SingleButtonMemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(InfoWindow dialog, PlayerStructure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.NotYoursPanel.Visible = true;
        if (structure is City city && city.Player == 0)
        {
            dialog.PopulationRaceLabel.Visible = true;
            string raceNamesPlural = dialog.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[city.Race]);
            dialog.PopulationRaceLabel.Text = raceNamesPlural;
            dialog.PopulationLabel.Visible = true;
            dialog.NotYoursMemo.Items.Clear();
        }
        else
        {
            DelphiStringCollection description = structure.Description;
            dialog.NotYoursMemo.Items.Assign(description);
        }

        if (structure.Player != -1)
        {
            int raceIndex = StrategicMap.Current.Players.GetPlayers(structure.Player).PlayerRace;
            if (raceIndex == -1)
            {
                raceIndex = 12;
            }

            string raceNamesPlural = dialog.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[raceIndex]);
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
        DelphiStringCollection description = structure.Description;
        dialog.SingleButtonMemo.Items.Assign(description);
    }
}
