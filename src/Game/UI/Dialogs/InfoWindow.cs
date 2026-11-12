// Translated from AoWInfoWin.pas.
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// item bag
public partial class InfoWindow : DataModule
{
    public InfoWindow(Component owner) : base(owner)
    { }

    public Window InfoWin;
    public Button Min;
    public Button Close;
    public Label Title;
    public Image SIcon;
    public Button MainBtn;
    public Frame CeltFrame;
    public ArnoLine StructLine;
    public Label StructNameLbl;
    public Panel ProductionPlacePnl;
    public Panel NamePnl;
    public Memo ActiveActionMemo;
    public Panel SignPnl;
    public Memo SignText;
    public Label SignLbl;
    public Frame SignTLine;
    public Image SignTDecoL;
    public Image SignTDecoR;
    public Frame SignBLine;
    public Image SignBDecoL;
    public Image SingBDemoR;
    public VerticalScrollBar SignTextSB;
    public Panel CityPanel;
    public Label CityRaceLbl;
    public Panel CityBorderPnl;
    public Image ProdOrb;
    public Panel SingleButPnl;
    public Memo SBmemo;
    public Panel AltarPnl;
    public Gauge Bar;
    public Label ChargeLbl;
    public Label GemLbl;
    public Label ChargePcntLbl;
    public Image T2GemI;
    public Image T2TurnI;
    public Label DayLbl;
    public Image LShield;
    public Panel NotYoursPnl;
    public Memo NotYoursMemo;
    public Label Ownerlbl;
    public Label OwnerRaceLbl;
    public Label ProdOrbTurnsLbl;
    public Label orLbl;
    public Panel AltarInfoPnl;
    public Image ProdOrbTime;
    public Button Center;
    public Panel ConstructionPnl;
    public Memo ConstructionMemo;
    public Label UnderConstructionLbl;
    public Label ConstructionDaysLeftLbl;
    public Panel RazePnl;
    public Image RazeBg;
    public Button RazeBtn;
    public Label CivilStatusLbl;
    public Image RaceRelationIcon;
    public Label CityIncomeLbl;
    public Image GoldIcn;
    public Image UgradeI;
    public Label CityLevelLbl;
    public Panel InfoPnl;
    public Panel ProductionPlaceInfoPnl;
    public Image IncomeTypeIcon;
    public Label IncomeValueLbl;
    public Memo ActiveCityActionMemo;
    public Label CostToFireLbl;
    public Label CurrentActivityLbl;
    public Label IncomeLbl;
    public Label ProductionWarningLbl;
    public Label CurrentActivityWarningLbl;
    public Panel IncomePnl;
    public Panel IncomInfoPnl;
    public Image IncomeTypeImage;
    public Label IncomeAmountLbl;
    public Label IncomeTextLbl;
    public Memo IncomeMemo;
    public Panel ItemPnl;
    public Label ItemsInBagLbl;
    public ListBox ItembagLB;
    public VerticalScrollBar ItembagSB;
    public Memo ItemBagMemo;
    public VerticalScrollBar SBSB;
    public VerticalScrollBar NotYoursMemoSB;
    public Panel ExplorationSitePnl;
    public Memo ESMemo;
    public VerticalScrollBar ESSB;
    public Label ESLbl;
    public Label ESStatusLbl;
    public Button Help;
    public Label PopulationRaceLbl;
    public Label PopulationLbl;
    public IvTranslator Translator;
    public ItemHexSprite ItemBag;
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static readonly Catalog<StructureView, Action<InfoWindow, Structure>> StructurePanels = new("structure info panel");
    public static readonly Catalog<StructureView, Action<InfoWindow, Structure>> StructureButtons = new("structure info button");

    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        this.InfoWin.Alignment.RightOffset = this.InfoWin._windowManager.WinWidth - this.InfoWin.WinLeft - this.InfoWin.WinWidth;
        this.InfoWin.Alignment.TopOffset = this.InfoWin.WinTop;
    }

    // AoWInfoWin.pas:146
    public void MinClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        InfoWin.Visible = false;
    }

    // AoWInfoWin.pas:152
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        InfoWin.Visible = false;
        DialogHosts.TaskBar.StructButton.Visible = false;
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
        DialogHosts.General.PlayButtonSound(0);
        if (ItemBag != null)
        {
            var itemHexSprite = ItemBag;
            var hexY = ItemBag.HexY;
            var hexX = itemHexSprite.HexX;
            AoWHSMap.Current.CenterViewTo(hexX, hexY, ItemBag.HexLevel);

            var fItemBagHexY = ItemBag.HexY;
            var fItemBagHexX = ItemBag.HexX;
            AoWHSMap.Current.PlayCenterAnimation(fItemBagHexX, fItemBagHexY, ItemBag.HexLevel);
        }
        else
        {
            var hexY = DialogHosts.General.Structure.HexY;
            var hexX = DialogHosts.General.Structure.HexX;
            AoWHSMap.Current.CenterViewTo(hexX, hexY, DialogHosts.General.Structure.HexLevel);
            var fStructureHexY = DialogHosts.General.Structure.HexY;
            var fStructureHexX = DialogHosts.General.Structure.HexX;
            AoWHSMap.Current.PlayCenterAnimation(fStructureHexX, fStructureHexY, DialogHosts.General.Structure.HexLevel);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.Structure != null)
        {
            if (DialogHosts.General.Structure.CanRaze(AoWHSMap.Current.SeatedPlayer, ref reason))
            {
                DialogHosts.General.Structure.Raze(AoWHSMap.Current.SeatedPlayer);
            }
            else
            {
                AoWHSMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        ArmyHexSprite army;
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.Structure != null)
        {
            var structure = DialogHosts.General.Structure;
            if (StructureButtons.TryGet(structure.InfoButton(AoWHSMap.Current.SeatedPlayer), out var open))
            {
                open(this, structure);
            }
        }
        else
        {
            if (ItemBag != null)
            {
                army = ((AoWMapField)ItemBag.Owner).GetArmyHexSprite();
                if (army != null && army.Army.Player == AoWHSMap.Current.SeatedPlayer)
                {
                    if (army.Army.Hero(255, RosterTables.AllHeroTypes) != null)
                    {
                        var hero = army.Army.Hero(255, RosterTables.AllHeroTypes);
                        DialogHosts.General.SelectedUnit = hero;
                        DialogHosts.UnitWindow.ShowUnit();
                        DialogHosts.UnitWindow.UnitTab.Selected = 2;
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
            if (ItemBag.Items.Count > ItembagLB.ListOffset)
            {
                ItemBag.Items.GetItem(ItembagLB.ListOffset).Show(drawRectangle.Left + 10, drawRectangle.Top + 10, surface);
            }
        }
    }

    // AoWInfoWin.pas:233
    public void HelpClick()
    {
        HelpWindow.ShowHelp(504);
    }

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ESMemo.Drawn = false;
        InfoWin.Drawn = false;
    }

    internal static void SetStructureHeader(InfoWindow dialog, Structure structure)
    {
        var name = structure.GetName();
        dialog.StructNameLbl.Text = name;
        if (structure.GetBottomBorderImage() != null)
        {
            var topBorderImage = structure.GetTopBorderImage();
            dialog.StructLine.ImageTop = topBorderImage;
            var middleBorderImage = structure.GetMiddleBorderImage();
            dialog.StructLine.ImageMiddle = middleBorderImage;
            var bottomBorderImage = structure.GetBottomBorderImage();
            dialog.StructLine.ImageBottom = bottomBorderImage;
        }

        dialog.StructLine.Visible = true;
    }

    internal static void ShowMineInfo(InfoWindow dialog, Mine structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePnl.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 31;
        var income = structure.GetIncome();
        dialog.IncomeAmountLbl.Text = IntToStr(income);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowPowerNodeInfo(InfoWindow dialog, PowerNode structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePnl.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        var power = structure.GetPower();
        dialog.IncomeAmountLbl.Text = IntToStr(power);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionIncome(InfoWindow dialog, ProductionPlace structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.IncomePnl.Visible = true;
        var description = structure.GetDescription();
        dialog.IncomeMemo.Items.Assign(description);
        dialog.IncomeTypeImage.CurrentImage = 33;
        var power = structure.GetPower();
        dialog.IncomeAmountLbl.Text = IntToStr(power);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowFarmInfo(InfoWindow dialog, Farm structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        dialog.SBmemo.Items.Add("");
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_incomeFormat), [IntToStr(structure.Income)]);
        dialog.SBmemo.Items.Add(format);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowSignInfo(InfoWindow dialog, Sign structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        DialogHosts.General.Sign = structure;
        dialog.SignPnl.Visible = true;
        dialog.SignText.Items.Assign(structure.Text);
    }

    internal static void ShowTeleportInfo(InfoWindow dialog, Teleport structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = DialogHosts.General.Structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        dialog.SBmemo.Sized = false;
        var canTeleport = structure.CanTeleport(AoWHSMap.Current.SeatedPlayer);
        dialog.MainBtn.Visible = canTeleport;
        var teleport = AbilityRules.TranslateResourceString(Teleport.Resource.s_teleportText);
        dialog.MainBtn.Text = teleport;
        dialog.RazePnl.Visible = dialog.MainBtn.Visible;
    }

    internal static void ShowTowerInfo(InfoWindow dialog, WizardsTower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        var canEnter = structure.CanEnter(AoWHSMap.Current.SeatedPlayer);
        dialog.MainBtn.Visible = canEnter;
        var enter = AbilityRules.TranslateResourceString(GameText.s_enterText);
        dialog.MainBtn.Text = enter;
    }

    internal static void ShowArenaInfo(InfoWindow dialog, Arena structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        var canEnter = structure.CanEnter(AoWHSMap.Current.SeatedPlayer);
        dialog.MainBtn.Visible = canEnter;
        var enter = AbilityRules.TranslateResourceString(GameText.s_enterText);
        dialog.MainBtn.Text = enter;
    }

    internal static void ShowCaveInfo(InfoWindow dialog, Cave structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        var canEnter = structure.CanEnter(AoWHSMap.Current.SeatedPlayer);
        dialog.MainBtn.Visible = canEnter;
        switch (structure.Entrance)
        {
            case CaveEntrance.Up:
                var goUp = AbilityRules.TranslateResourceString(GameText.s_goUpText);
                dialog.MainBtn.Text = goUp;
                break;
            case CaveEntrance.Down:
                var goDown = AbilityRules.TranslateResourceString(GameText.s_goDownText);
                dialog.MainBtn.Text = goDown;
                break;
        }
    }

    internal static void ShowWatchTowerInfo(InfoWindow dialog, Tower structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowExplorationInfo(InfoWindow dialog, ExplorationSite structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ExplorationSitePnl.Visible = true;
        var description = DialogHosts.General.Structure.GetDescription();
        dialog.ESMemo.Items.Assign(description);
        var status = AbilityRules.TranslateResourceString(CityScreen.s_statusText);
        dialog.ESLbl.Text = status + ":";
        if (structure.GetExplored())
        {
            var explored = AbilityRules.TranslateResourceString(GameText.s_exploredText);
            dialog.ESStatusLbl.Text = explored;
        }
        else
        {
            var unexplored = AbilityRules.TranslateResourceString(GameText.s_unexploredText);
            dialog.ESStatusLbl.Text = unexplored;
        }

        var canSearch = ((ExplorationSite)DialogHosts.General.Structure).CanSearch(AoWHSMap.Current.SeatedPlayer);
        dialog.MainBtn.Visible = canSearch;
        var search = AbilityRules.TranslateResourceString(GameText.s_searchText);
        dialog.MainBtn.Text = search;
    }

    internal static void ShowAltarInfo(InfoWindow dialog, Altar structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.AltarPnl.Visible = true;
        if (structure.GetTargeting())
        {
            var cancel = AbilityRules.TranslateResourceString(GameText.s_cancelText);
            dialog.MainBtn.Text = cancel;
        }
        else
        {
            var target = AbilityRules.TranslateResourceString(GameText.s_targetText);
            dialog.MainBtn.Text = target;
        }

        dialog.MainBtn.Visible = true;
        var rechargeDays = structure.GetRechargeDays();
        dialog.Bar.Max = rechargeDays;
        dialog.Bar.Position = structure.RechargeDays;
        var structureRechargeDays = structure.GetRechargeDays();
        dialog.ChargePcntLbl.Text = IntToStr(Round((double)(structure.RechargeDays * 100) / structureRechargeDays)) + "%";
        var rechargeDays2 = structure.GetRechargeDays();
        dialog.DayLbl.Text = IntToStr(rechargeDays2 - structure.RechargeDays);
        var targetMana = structure.TargetMana();
        dialog.GemLbl.Text = IntToStr(targetMana);
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowWarCityInfo(InfoWindow dialog, City structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        dialog.SBmemo.Items.Add("");
        dialog.SBmemo.Items.Add("");
        dialog.SBmemo.Items.Add("");
        dialog.SBmemo.Items.Add("");
        dialog.SBmemo.Items.Add("");
        dialog.SBmemo.Items.Add("");
        var statusWarMode = AbilityRules.TranslateResourceString(GameText.s_statusWarModeText);
        dialog.SBmemo.Items.Add(statusWarMode);
    }

    internal static void ShowCityInfo(InfoWindow dialog, City structure)
    {
        string reason = "";
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        var enter = AbilityRules.TranslateResourceString(GameText.s_enterText);
        dialog.MainBtn.Text = enter;
        dialog.MainBtn.Visible = true;
        DialogHosts.General.City = structure;
        dialog.CityPanel.Visible = true;
        dialog.ProductionWarningLbl.Visible = false;
        dialog.ActiveCityActionMemo.Items.Clear();
        var name = DialogHosts.General.City.GetName();
        dialog.StructNameLbl.Text = name;
        var raceNames = AbilityRules.TranslateResourceString(GameTables.s_raceNames[DialogHosts.General.City.Race]);
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_raceFormat), [raceNames]);
        dialog.CityRaceLbl.Text = format;
        dialog.CityLevelLbl.Text = IntToStr(structure.UpgradeLevel);
        dialog.CityIncomeLbl.Text = IntToStr(structure.Income);
        var civilStatus = structure.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateResourceString(CityScreen.s_civilStatusTexts[(int)civilStatus]);
        var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_statusFormat), [civilStatusText]);
        dialog.CivilStatusLbl.Text = formattedText;
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
                    var unlimited = AbilityRules.TranslateResourceString(MapEvents.s_unlimitedText);
                    dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.Production.Status == ProductionControlStatus.Waiting)
                {
                    var queue = structure.Production.Queue.GetItem(0);
                    structure.Production.CanExecuteProduction(queue, ref reason);
                    dialog.ProductionWarningLbl.Text = reason;
                    dialog.ProductionWarningLbl.Visible = true;
                }
            }
        }
        else
        {
            dialog.ActiveCityActionMemo.Items.Add("");
            var producingMerchandise = AbilityRules.TranslateResourceString(GameText.s_producingMerchandiseText);
            dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = AoWHSMap.Current.Races.GetRace(structure.Race).GetPlayerRelation(structure.Player);
        dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionInfo(InfoWindow dialog, ProductionPlace structure)
    {
        string reason = "";
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        var enter = AbilityRules.TranslateResourceString(GameText.s_enterText);
        dialog.MainBtn.Text = enter;
        dialog.MainBtn.Visible = true;
        DialogHosts.General.ProductionPlace = (ProductionPlace)DialogHosts.General.Structure;
        dialog.ProductionPlacePnl.Visible = true;
        DialogHosts.General.ProductionControl = DialogHosts.General.ProductionPlace.ProductionControl;
        dialog.CurrentActivityWarningLbl.Visible = false;
        dialog.ActiveActionMemo.Items.Clear();
        dialog.IncomeLbl.Visible = true;
        dialog.IncomeTypeIcon.Visible = true;
        dialog.IncomeValueLbl.Visible = true;
        if (structure.GetMagicSphere() != MagicSphere.None)
        {
            dialog.IncomeTypeIcon.CurrentImage = 33;
            var power = structure.GetPower();
            dialog.IncomeValueLbl.Text = IntToStr(power);
        }
        else
        {
            if (structure.GetBaseIncome() != 0)
            {
                dialog.IncomeTypeIcon.CurrentImage = 31;
                var income = structure.GetIncome();
                dialog.IncomeValueLbl.Text = IntToStr(income);
            }
            else
            {
                dialog.IncomeLbl.Visible = false;
                dialog.IncomeTypeIcon.Visible = false;
                dialog.IncomeValueLbl.Visible = false;
            }
        }

        dialog.ProductionPlacePnl.Visible = true;
        var name = structure.GetName();
        dialog.StructNameLbl.Text = name;
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
                    var unlimited = AbilityRules.TranslateResourceString(MapEvents.s_unlimitedText);
                    dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (structure.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    var queue = structure.ProductionControl.Queue.GetItem(0);
                    structure.ProductionControl.CanExecuteProduction(queue, ref reason);
                    dialog.CurrentActivityWarningLbl.Text = reason;
                    dialog.CurrentActivityWarningLbl.Visible = true;
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
                    var producingMerchandise = AbilityRules.TranslateResourceString(GameText.s_producingMerchandiseText);
                    dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = AbilityRules.TranslateResourceString(GameTables.s_noneText);
                    dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = AbilityRules.TranslateResourceString(GameText.s_generatingExtraPowerText);
                dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        dialog.RazePnl.Visible = true;
    }

    internal static void ShowBuildingInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.ConstructionPnl.Visible = true;
        var formatDaysLeftString = DurationText.FormatDaysLeftString(structure.BuildingDays);
        dialog.ConstructionDaysLeftLbl.Text = formatDaysLeftString;
        var description = structure.GetDescription();
        dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(InfoWindow dialog, Structure structure)
    {
        string text;
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        if (structure is City)
        {
            var name = structure.GetName();
            text = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_cityFormat), [name]);
        }
        else
        {
            text = structure.GetName();
        }

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_liesInRuinsFormat), [text]);
        dialog.SBmemo.Items.Text = format;
        dialog.SBmemo.Items.Insert(0, "");
        dialog.SBmemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(InfoWindow dialog, PlayerStructure structure)
    {
        int raceIndex;
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.NotYoursPnl.Visible = true;
        if (structure is City && structure.Player == 0)
        {
            dialog.PopulationRaceLbl.Visible = true;
            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[((City)structure).Race]);
            dialog.PopulationRaceLbl.Text = raceNamesPlural;
            dialog.PopulationLbl.Visible = true;
            dialog.NotYoursMemo.Items.Clear();
        }
        else
        {
            var description = structure.GetDescription();
            dialog.NotYoursMemo.Items.Assign(description);
        }

        if (structure.Player != -1)
        {
            raceIndex = AoWHSMap.Current.Players.GetPlayers(structure.Player).PlayerRace;
            if (raceIndex == -1)
            {
                raceIndex = 12;
            }

            var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[raceIndex]);
            dialog.OwnerRaceLbl.Text = raceNamesPlural;
            dialog.OwnerRaceLbl.Visible = true;
            dialog.LShield.CurrentImage = raceIndex + 26;
            dialog.LShield.Visible = true;
        }
        else
        {
            dialog.OwnerRaceLbl.Visible = false;
            dialog.LShield.Visible = false;
        }
    }

    internal static void ShowGenericInfo(InfoWindow dialog, Structure structure)
    {
        MapEvents.ResetStructureInfo(dialog);
        SetStructureHeader(dialog, structure);
        dialog.SingleButPnl.Visible = true;
        var description = structure.GetDescription();
        dialog.SBmemo.Items.Assign(description);
    }

}
