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
    public ItemHexSprite FItemBag;
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static readonly Catalog<StructureView, Action<InfoWindow, Structure>> StructurePanels = new("structure info panel");
    public static readonly Catalog<StructureView, Action<InfoWindow, Structure>> StructureButtons = new("structure info button");

    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        this.InfoWin.Alignment.RightOffset = this.InfoWin.FWinManager.WinWidth - this.InfoWin.WinLeft - this.InfoWin.WinWidth;
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
        DialogHosts.TBWindow.StructButton.Visible = false;
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
        if (FItemBag != null)
        {
            var itemHS = FItemBag;
            var yhx = FItemBag.Yhx;
            var xhx = itemHS.Xhx;
            AoWHSMap.Current.CenterViewTo(xhx, yhx, FItemBag.Lhx);

            var fItemBagYhx = FItemBag.Yhx;
            var fItemBagXhx = FItemBag.Xhx;
            AoWHSMap.Current.PlayCenterAnimation(fItemBagXhx, fItemBagYhx, FItemBag.Lhx);
        }
        else
        {
            var yhx = DialogHosts.General.FStructure.Yhx;
            var xhx = DialogHosts.General.FStructure.Xhx;
            AoWHSMap.Current.CenterViewTo(xhx, yhx, DialogHosts.General.FStructure.Lhx);
            var fStructureYhx = DialogHosts.General.FStructure.Yhx;
            var fStructureXhx = DialogHosts.General.FStructure.Xhx;
            AoWHSMap.Current.PlayCenterAnimation(fStructureXhx, fStructureYhx, DialogHosts.General.FStructure.Lhx);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.FStructure != null)
        {
            if (DialogHosts.General.FStructure.CanRaze(AoWHSMap.Current.FSeatedPlayer, ref Reason))
            {
                DialogHosts.General.FStructure.Raze(AoWHSMap.Current.FSeatedPlayer);
            }
            else
            {
                AoWHSMap.Current.ShowWarning(Reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        ArmyHexSprite Army;
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.FStructure != null)
        {
            var structure = DialogHosts.General.FStructure;
            if (StructureButtons.TryGet(structure.InfoButton(AoWHSMap.Current.FSeatedPlayer), out var open))
            {
                open(this, structure);
            }
        }
        else
        {
            if (FItemBag != null)
            {
                Army = ((AoWMapField)FItemBag.Owner).GetArmyHS();
                if (Army != null && Army.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    if (Army.FArmy.Hero(255, RosterTables.AllHeroTypes) != null)
                    {
                        var hero = Army.FArmy.Hero(255, RosterTables.AllHeroTypes);
                        DialogHosts.General.FSelectedUnit = hero;
                        DialogHosts.UnitWindow.ShowUnit();
                        DialogHosts.UnitWindow.UnitTab.Selected = 2;
                    }
                }
            }
        }
    }

    // AoWInfoWin.pas:226
    public void ItembagLBDrawItem(in Rect DrawRect, int DrawItem, in LockedSurface Surface)
    {
        if (FItemBag != null)
        {
            // { Original uses the scroll offset for every row. }
            if (FItemBag.FItems.Count > ItembagLB.ListOffset)
            {
                FItemBag.FItems.GetItem(ItembagLB.ListOffset).Show(DrawRect.Left + 10, DrawRect.Top + 10, Surface);
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
        ESMemo.FDrawn = false;
        InfoWin.FDrawn = false;
    }

    internal static void SetStructureHeader(InfoWindow Dialog, Structure Structure)
    {
        var name = Structure.GetName();
        Dialog.StructNameLbl.Text = name;
        if (Structure.GetBottomBorderImage() != null)
        {
            var topBorderImage = Structure.GetTopBorderImage();
            Dialog.StructLine.ImageTop = topBorderImage;
            var middleBorderImage = Structure.GetMiddleBorderImage();
            Dialog.StructLine.ImageMiddle = middleBorderImage;
            var bottomBorderImage = Structure.GetBottomBorderImage();
            Dialog.StructLine.ImageBottom = bottomBorderImage;
        }

        Dialog.StructLine.Visible = true;
    }

    internal static void ShowMineInfo(InfoWindow Dialog, Mine Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 31;
        var income = Structure.GetIncome();
        Dialog.IncomeAmountLbl.Text = IntToStr(income);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowPowerNodeInfo(InfoWindow Dialog, PowerNode Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionIncome(InfoWindow Dialog, ProductionPlace Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowFarmInfo(InfoWindow Dialog, Farm Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.Items.Add("");
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.IncomeXRStr), [IntToStr(Structure.FIncome)]);
        Dialog.SBmemo.Items.Add(format);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowSignInfo(InfoWindow Dialog, Sign Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        DialogHosts.General.FSign = Structure;
        Dialog.SignPnl.Visible = true;
        Dialog.SignText.Items.Assign(Structure.FText);
    }

    internal static void ShowTeleportInfo(InfoWindow Dialog, Teleport Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = DialogHosts.General.FStructure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.FSized = false;
        var canTeleport = Structure.CanTeleport(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canTeleport;
        var teleport = AbilityRules.TranslateRStr(Teleport.Resource.TeleportRStr);
        Dialog.MainBtn.Text = teleport;
        Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
    }

    internal static void ShowTowerInfo(InfoWindow Dialog, WizardsTower Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowArenaInfo(InfoWindow Dialog, Arena Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowCaveInfo(InfoWindow Dialog, Cave Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        switch (Structure.FEntrance)
        {
            case Cave.Entrance.Up:
                var goUp = AbilityRules.TranslateRStr(GameText.GoUpRStr);
                Dialog.MainBtn.Text = goUp;
                break;
            case Cave.Entrance.Down:
                var goDown = AbilityRules.TranslateRStr(GameText.GoDownRStr);
                Dialog.MainBtn.Text = goDown;
                break;
        }
    }

    internal static void ShowWatchTowerInfo(InfoWindow Dialog, Tower Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowExplorationInfo(InfoWindow Dialog, ExplorationSite Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.ExplorationSitePnl.Visible = true;
        var description = DialogHosts.General.FStructure.GetDescription();
        Dialog.ESMemo.Items.Assign(description);
        var status = AbilityRules.TranslateRStr(CityScreen.StatusRStr);
        Dialog.ESLbl.Text = status + ":";
        if (Structure.GetExplored())
        {
            var explored = AbilityRules.TranslateRStr(GameText.ExploredRStr);
            Dialog.ESStatusLbl.Text = explored;
        }
        else
        {
            var unexplored = AbilityRules.TranslateRStr(GameText.UnexploredRStr);
            Dialog.ESStatusLbl.Text = unexplored;
        }

        var canSearch = ((ExplorationSite)DialogHosts.General.FStructure).CanSearch(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canSearch;
        var search = AbilityRules.TranslateRStr(GameText.SearchRStr);
        Dialog.MainBtn.Text = search;
    }

    internal static void ShowAltarInfo(InfoWindow Dialog, Altar Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.AltarPnl.Visible = true;
        if (Structure.GetTargeting())
        {
            var cancel = AbilityRules.TranslateRStr(GameText.CancelRStr);
            Dialog.MainBtn.Text = cancel;
        }
        else
        {
            var target = AbilityRules.TranslateRStr(GameText.TargetRStr);
            Dialog.MainBtn.Text = target;
        }

        Dialog.MainBtn.Visible = true;
        var rechargeDays = Structure.GetRechargeDays();
        Dialog.Bar.Max = rechargeDays;
        Dialog.Bar.Position = Structure.FRechargeDays;
        var structureRechargeDays = Structure.GetRechargeDays();
        Dialog.ChargePcntLbl.Text = IntToStr(Round((double)(Structure.FRechargeDays * 100) / structureRechargeDays)) + "%";
        var rechargeDays2 = Structure.GetRechargeDays();
        Dialog.DayLbl.Text = IntToStr(rechargeDays2 - Structure.FRechargeDays);
        var targetMana = Structure.TargetMana();
        Dialog.GemLbl.Text = IntToStr(targetMana);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowWarCityInfo(InfoWindow Dialog, City Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        var statusWarmode = AbilityRules.TranslateRStr(GameText.StatusWarmodeRStr);
        Dialog.SBmemo.Items.Add(statusWarmode);
    }

    internal static void ShowCityInfo(InfoWindow Dialog, City Structure)
    {
        string Reason = "";
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        var enter = AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        DialogHosts.General.FCity = Structure;
        Dialog.CityPanel.Visible = true;
        Dialog.ProductionWarningLbl.Visible = false;
        Dialog.ActiveCityActionMemo.Items.Clear();
        var name = DialogHosts.General.FCity.GetName();
        Dialog.StructNameLbl.Text = name;
        var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[DialogHosts.General.FCity.FRace]);
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.RaceXRStr), [raceNames]);
        Dialog.CityRaceLbl.Text = format;
        Dialog.CityLevelLbl.Text = IntToStr(Structure.FUpgradeLevel);
        Dialog.CityIncomeLbl.Text = IntToStr(Structure.FIncome);
        var civilStatus = Structure.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateRStr(CityScreen.TCivilStatusRStr[(int)civilStatus]);
        var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.StatusXRStr), [civilStatusText]);
        Dialog.CivilStatusLbl.Text = aoWEFormat;
        if (Structure.FProduction.FQueue.GetCount() != 0)
        {
            Structure.FProduction.FQueue.GetItem(0).Order.ActivityLines(Structure.FProduction, Dialog.ActiveCityActionMemo.Items);

            if (Structure.FProduction.FStatus == ProductionControl.Status.Producing)
            {
                if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                    Dialog.ActiveCityActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    var unlimited = AbilityRules.TranslateRStr(MapEvents.UnlimitedRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (Structure.FProduction.FStatus == ProductionControl.Status.Waiting)
                {
                    var queue = Structure.FProduction.FQueue.GetItem(0);
                    Structure.FProduction.CanExecuteProduction(queue, ref Reason);
                    Dialog.ProductionWarningLbl.Text = Reason;
                    Dialog.ProductionWarningLbl.Visible = true;
                }
            }
        }
        else
        {
            Dialog.ActiveCityActionMemo.Items.Add("");
            var producingMerchandise = AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
            Dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = AoWHSMap.Current.FRaces.GetRace(Structure.FRace).GetPlayerRelation(Structure.FPlayer);
        Dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionInfo(InfoWindow Dialog, ProductionPlace Structure)
    {
        string Reason = "";
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        var enter = AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        DialogHosts.General.FProductionPlace = (ProductionPlace)DialogHosts.General.FStructure;
        Dialog.ProductionPlacePnl.Visible = true;
        DialogHosts.General.FProductionControl = DialogHosts.General.FProductionPlace.FProductionControl;
        Dialog.CurrentActivityWarningLbl.Visible = false;
        Dialog.ActiveActionMemo.Items.Clear();
        Dialog.IncomeLbl.Visible = true;
        Dialog.IncomeTypeIcon.Visible = true;
        Dialog.IncomeValueLbl.Visible = true;
        if (Structure.GetMagicSphere() != MagicSphere.None)
        {
            Dialog.IncomeTypeIcon.CurrentImage = 33;
            var power = Structure.GetPower();
            Dialog.IncomeValueLbl.Text = IntToStr(power);
        }
        else
        {
            if (Structure.GetBaseIncome() != 0)
            {
                Dialog.IncomeTypeIcon.CurrentImage = 31;
                var income = Structure.GetIncome();
                Dialog.IncomeValueLbl.Text = IntToStr(income);
            }
            else
            {
                Dialog.IncomeLbl.Visible = false;
                Dialog.IncomeTypeIcon.Visible = false;
                Dialog.IncomeValueLbl.Visible = false;
            }
        }

        Dialog.ProductionPlacePnl.Visible = true;
        var name = Structure.GetName();
        Dialog.StructNameLbl.Text = name;
        if (Structure.FProductionControl.FQueue.GetCount() != 0)
        {
            Structure.FProductionControl.FQueue.GetItem(0).Order.ActivityLines(Structure.FProductionControl, Dialog.ActiveActionMemo.Items);

            if (Structure.FProductionControl.FStatus == ProductionControl.Status.Producing)
            {
                if (Structure.FProductionControl.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                    Dialog.ActiveActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    var unlimited = AbilityRules.TranslateRStr(MapEvents.UnlimitedRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (Structure.FProductionControl.FStatus == ProductionControl.Status.Waiting)
                {
                    var queue = Structure.FProductionControl.FQueue.GetItem(0);
                    Structure.FProductionControl.CanExecuteProduction(queue, ref Reason);
                    Dialog.CurrentActivityWarningLbl.Text = Reason;
                    Dialog.CurrentActivityWarningLbl.Visible = true;
                }
            }
        }
        else
        {
            if (Structure.GetMagicSphere() == MagicSphere.None)
            {
                Dialog.ActiveActionMemo.Items.Add("");
                if (Structure.GetBaseIncome() != 0)
                {
                    var producingMerchandise = AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
                    Dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
                    Dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = AbilityRules.TranslateRStr(GameText.GeneratingExtraPowerRStr);
                Dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowBuildingInfo(InfoWindow Dialog, Structure Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.ConstructionPnl.Visible = true;
        var fmtDaysLeftStr = DurationText.FmtDaysLeftStr(Structure.FBuildingDays);
        Dialog.ConstructionDaysLeftLbl.Text = fmtDaysLeftStr;
        var description = Structure.GetDescription();
        Dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(InfoWindow Dialog, Structure Structure)
    {
        string Text;
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        if (Structure is City)
        {
            var name = Structure.GetName();
            Text = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.CityOfXRStr), [name]);
        }
        else
        {
            Text = Structure.GetName();
        }

        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.TheXliesInRuinsRStr), [Text]);
        Dialog.SBmemo.Items.Text = format;
        Dialog.SBmemo.Items.Insert(0, "");
        Dialog.SBmemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(InfoWindow Dialog, PlayerStructure Structure)
    {
        int RaceIndex;
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.NotYoursPnl.Visible = true;
        if (Structure is City && Structure.FPlayer == 0)
        {
            Dialog.PopulationRaceLbl.Visible = true;
            var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[((City)Structure).FRace]);
            Dialog.PopulationRaceLbl.Text = raceNamesPlural;
            Dialog.PopulationLbl.Visible = true;
            Dialog.NotYoursMemo.Items.Clear();
        }
        else
        {
            var description = Structure.GetDescription();
            Dialog.NotYoursMemo.Items.Assign(description);
        }

        if (Structure.FPlayer != -1)
        {
            RaceIndex = AoWHSMap.Current.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[RaceIndex]);
            Dialog.OwnerRaceLbl.Text = raceNamesPlural;
            Dialog.OwnerRaceLbl.Visible = true;
            Dialog.LShield.CurrentImage = RaceIndex + 26;
            Dialog.LShield.Visible = true;
        }
        else
        {
            Dialog.OwnerRaceLbl.Visible = false;
            Dialog.LShield.Visible = false;
        }
    }

    internal static void ShowGenericInfo(InfoWindow Dialog, Structure Structure)
    {
        MapEvents.ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
    }

}
