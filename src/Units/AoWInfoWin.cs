// Translated from AoWInfoWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWInfoWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// item bag
public partial class IWindow : DataModule
{
    public IWindow(Component owner) : base(owner)
    { }

    public AOWWindow InfoWin;
    public AOWButton Min;
    public AOWButton Close;
    public AOWLabel Title;
    public AOWImage SIcon;
    public AOWButton MainBtn;
    public AoWFrame CeltFrame;
    public AoWArnoLine StructLine;
    public AOWLabel StructNameLbl;
    public AOWPanel ProductionPlacePnl;
    public AOWPanel NamePnl;
    public AOWMemo ActiveActionMemo;
    public AOWPanel SignPnl;
    public AOWMemo SignText;
    public AOWLabel SignLbl;
    public AoWFrame SignTLine;
    public AOWImage SignTDecoL;
    public AOWImage SignTDecoR;
    public AoWFrame SignBLine;
    public AOWImage SignBDecoL;
    public AOWImage SingBDemoR;
    public AOWVScrollBar SignTextSB;
    public AOWPanel CityPanel;
    public AOWLabel CityRaceLbl;
    public AOWPanel CityBorderPnl;
    public AOWImage ProdOrb;
    public AOWPanel SingleButPnl;
    public AOWMemo SBmemo;
    public AOWPanel AltarPnl;
    public AoWGauge Bar;
    public AOWLabel ChargeLbl;
    public AOWLabel GemLbl;
    public AOWLabel ChargePcntLbl;
    public AOWImage T2GemI;
    public AOWImage T2TurnI;
    public AOWLabel DayLbl;
    public AOWImage LShield;
    public AOWPanel NotYoursPnl;
    public AOWMemo NotYoursMemo;
    public AOWLabel Ownerlbl;
    public AOWLabel OwnerRaceLbl;
    public AOWLabel ProdOrbTurnsLbl;
    public AOWLabel orLbl;
    public AOWPanel AltarInfoPnl;
    public AOWImage ProdOrbTime;
    public AOWButton Center;
    public AOWPanel ConstructionPnl;
    public AOWMemo ConstructionMemo;
    public AOWLabel UnderConstructionLbl;
    public AOWLabel ConstructionDaysLeftLbl;
    public AOWPanel RazePnl;
    public AOWImage RazeBg;
    public AOWButton RazeBtn;
    public AOWLabel CivilStatusLbl;
    public AOWImage RaceRelationIcon;
    public AOWLabel CityIncomeLbl;
    public AOWImage GoldIcn;
    public AOWImage UgradeI;
    public AOWLabel CityLevelLbl;
    public AOWPanel InfoPnl;
    public AOWPanel ProductionPlaceInfoPnl;
    public AOWImage IncomeTypeIcon;
    public AOWLabel IncomeValueLbl;
    public AOWMemo ActiveCityActionMemo;
    public AOWLabel CostToFireLbl;
    public AOWLabel CurrentActivityLbl;
    public AOWLabel IncomeLbl;
    public AOWLabel ProductionWarningLbl;
    public AOWLabel CurrentActivityWarningLbl;
    public AOWPanel IncomePnl;
    public AOWPanel IncomInfoPnl;
    public AOWImage IncomeTypeImage;
    public AOWLabel IncomeAmountLbl;
    public AOWLabel IncomeTextLbl;
    public AOWMemo IncomeMemo;
    public AOWPanel ItemPnl;
    public AOWLabel ItemsInBagLbl;
    public AOWListBox ItembagLB;
    public AOWVScrollBar ItembagSB;
    public AOWMemo ItemBagMemo;
    public AOWVScrollBar SBSB;
    public AOWVScrollBar NotYoursMemoSB;
    public AOWPanel ExplorationSitePnl;
    public AOWMemo ESMemo;
    public AOWVScrollBar ESSB;
    public AOWLabel ESLbl;
    public AOWLabel ESStatusLbl;
    public AOWButton Help;
    public AOWLabel PopulationRaceLbl;
    public AOWLabel PopulationLbl;
    public IvTranslator Translator;
    public ItemHS FItemBag;
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static readonly Catalog<StructureView, Action<IWindow, Structure>> StructurePanels = new("structure info panel");
    public static readonly Catalog<StructureView, Action<IWindow, Structure>> StructureButtons = new("structure info button");

    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        this.InfoWin.Alignment.RightOffset = this.InfoWin.FWinManager.WinWidth - this.InfoWin.WinLeft - this.InfoWin.WinWidth;
        this.InfoWin.Alignment.TopOffset = this.InfoWin.WinTop;
    }

    // AoWInfoWin.pas:146
    public void MinClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        InfoWin.Visible = false;
    }

    // AoWInfoWin.pas:152
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FItemBag != null)
        {
            var itemHS = FItemBag;
            var yhx = FItemBag.Yhx;
            var xhx = itemHS.Xhx;
            Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(xhx, yhx, FItemBag.Lhx);

            var fItemBagYhx = FItemBag.Yhx;
            var fItemBagXhx = FItemBag.Xhx;
            Aow.Game.World.Map.AoWHSMap.Current.PlayCenterAnimation(fItemBagXhx, fItemBagYhx, FItemBag.Lhx);
        }
        else
        {
            var yhx = DialogHosts.General.FStructure.Yhx;
            var xhx = DialogHosts.General.FStructure.Xhx;
            Aow.Game.World.Map.AoWHSMap.Current.CenterViewTo(xhx, yhx, DialogHosts.General.FStructure.Lhx);
            var fStructureYhx = DialogHosts.General.FStructure.Yhx;
            var fStructureXhx = DialogHosts.General.FStructure.Xhx;
            Aow.Game.World.Map.AoWHSMap.Current.PlayCenterAnimation(fStructureXhx, fStructureYhx, DialogHosts.General.FStructure.Lhx);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.FStructure != null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.CanRaze(AoWHSMap.Current.FSeatedPlayer, ref Reason))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Raze(AoWHSMap.Current.FSeatedPlayer);
            }
            else
            {
                Aow.Game.World.Map.AoWHSMap.Current.ShowWarning(Reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        ArmyHS Army;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                        DialogHosts.UnitWindow.UnitTab.Selected = 2;
                    }
                }
            }
        }
    }

    // AoWInfoWin.pas:226
    public void ItembagLBDrawItem(in Rect DrawRect, int DrawItem, in SurfaceDesc Surface)
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
        Aow.Game.Ui.Dialogs.HelpWindow.ShowHelp(504);
    }

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ESMemo.FDrawn = false;
        InfoWin.FDrawn = false;
    }

    internal static void SetStructureHeader(IWindow Dialog, Structure Structure)
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

    internal static void ShowMineInfo(IWindow Dialog, Mine Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 31;
        var income = Structure.GetIncome();
        Dialog.IncomeAmountLbl.Text = IntToStr(income);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowPowerNodeInfo(IWindow Dialog, PowerNode Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionIncome(IWindow Dialog, ProductionPlace Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowFarmInfo(IWindow Dialog, Farm Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.Items.Add("");
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.IncomeXRStr), [IntToStr(Structure.FIncome)]);
        Dialog.SBmemo.Items.Add(format);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowSignInfo(IWindow Dialog, Sign Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        DialogHosts.General.FSign = Structure;
        Dialog.SignPnl.Visible = true;
        Dialog.SignText.Items.Assign(Structure.FText);
    }

    internal static void ShowTeleportInfo(IWindow Dialog, Teleport Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.FSized = false;
        var canTeleport = Structure.CanTeleport(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canTeleport;
        var teleport = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Teleport.Resource.TeleportRStr);
        Dialog.MainBtn.Text = teleport;
        Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
    }

    internal static void ShowTowerInfo(IWindow Dialog, WizardsTower Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowArenaInfo(IWindow Dialog, Arena Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowCaveInfo(IWindow Dialog, Cave Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        switch (Structure.FEntrance)
        {
            case Cave.Entrance.Up:
                var goUp = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.GoUpRStr);
                Dialog.MainBtn.Text = goUp;
                break;
            case Cave.Entrance.Down:
                var goDown = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.GoDownRStr);
                Dialog.MainBtn.Text = goDown;
                break;
        }
    }

    internal static void ShowWatchTowerInfo(IWindow Dialog, Tower Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowExplorationInfo(IWindow Dialog, ExplorationSite Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.ExplorationSitePnl.Visible = true;
        var description = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.GetDescription();
        Dialog.ESMemo.Items.Assign(description);
        var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityScreen.StatusRStr);
        Dialog.ESLbl.Text = status + ":";
        if (Structure.GetExplored())
        {
            var explored = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ExploredRStr);
            Dialog.ESStatusLbl.Text = explored;
        }
        else
        {
            var unexplored = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.UnexploredRStr);
            Dialog.ESStatusLbl.Text = unexplored;
        }

        var canSearch = ((ExplorationSite)DialogHosts.General.FStructure).CanSearch(AoWHSMap.Current.FSeatedPlayer);
        Dialog.MainBtn.Visible = canSearch;
        var search = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.SearchRStr);
        Dialog.MainBtn.Text = search;
    }

    internal static void ShowAltarInfo(IWindow Dialog, Altar Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.AltarPnl.Visible = true;
        if (Structure.GetTargeting())
        {
            var cancel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.CancelRStr);
            Dialog.MainBtn.Text = cancel;
        }
        else
        {
            var target = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TargetRStr);
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

    internal static void ShowWarCityInfo(IWindow Dialog, City Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        var statusWarmode = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.StatusWarmodeRStr);
        Dialog.SBmemo.Items.Add(statusWarmode);
    }

    internal static void ShowCityInfo(IWindow Dialog, City Structure)
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        DialogHosts.General.FCity = Structure;
        Dialog.CityPanel.Visible = true;
        Dialog.ProductionWarningLbl.Visible = false;
        Dialog.ActiveCityActionMemo.Items.Clear();
        var name = Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.GetName();
        Dialog.StructNameLbl.Text = name;
        var raceNames = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[DialogHosts.General.FCity.FRace]);
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.RaceXRStr), [raceNames]);
        Dialog.CityRaceLbl.Text = format;
        Dialog.CityLevelLbl.Text = IntToStr(Structure.FUpgradeLevel);
        Dialog.CityIncomeLbl.Text = IntToStr(Structure.FIncome);
        var civilStatus = Structure.GetCivilStatus();
        var civilStatusText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(CityScreen.TCivilStatusRStr[(int)civilStatus]);
        var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.StatusXRStr), [civilStatusText]);
        Dialog.CivilStatusLbl.Text = aoWEFormat;
        if (Structure.FProduction.FQueue.GetCount() != 0)
        {
            Structure.FProduction.FQueue.GetItem(0).Order.ActivityLines(Structure.FProduction, Dialog.ActiveCityActionMemo.Items);

            if (Structure.FProduction.FStatus == ProductionControl.Status.Producing)
            {
                if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                    Dialog.ActiveCityActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    var unlimited = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MapEvents.UnlimitedRStr);
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
            var producingMerchandise = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
            Dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Structure.FRace).GetPlayerRelation(Structure.FPlayer);
        Dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionInfo(IWindow Dialog, ProductionPlace Structure)
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.EnterRStr);
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
                    var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                    Dialog.ActiveActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    var unlimited = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MapEvents.UnlimitedRStr);
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
                    var producingMerchandise = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
                    Dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.NoneRStr);
                    Dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.GeneratingExtraPowerRStr);
                Dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowBuildingInfo(IWindow Dialog, Structure Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.ConstructionPnl.Visible = true;
        var fmtDaysLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtDaysLeftStr(Structure.FBuildingDays);
        Dialog.ConstructionDaysLeftLbl.Text = fmtDaysLeftStr;
        var description = Structure.GetDescription();
        Dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(IWindow Dialog, Structure Structure)
    {
        string Text;
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        if (Structure is City)
        {
            var name = Structure.GetName();
            Text = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.CityOfXRStr), [name]);
        }
        else
        {
            Text = Structure.GetName();
        }

        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TheXliesInRuinsRStr), [Text]);
        Dialog.SBmemo.Items.Text = format;
        Dialog.SBmemo.Items.Insert(0, "");
        Dialog.SBmemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(IWindow Dialog, PlayerStructure Structure)
    {
        int RaceIndex;
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.NotYoursPnl.Visible = true;
        if (Structure is City && Structure.FPlayer == 0)
        {
            Dialog.PopulationRaceLbl.Visible = true;
            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[((City)Structure).FRace]);
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
            RaceIndex = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[RaceIndex]);
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

    internal static void ShowGenericInfo(IWindow Dialog, Structure Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.IWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
    }

}
