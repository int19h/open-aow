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
public partial class TIWindow : TDataModule
{
    public TIWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow InfoWin;
    public TAOWButton Min;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAOWImage SIcon;
    public TAOWButton MainBtn;
    public TAoWFrame CeltFrame;
    public TAoWArnoLine StructLine;
    public TAOWLabel StructNameLbl;
    public TAOWPanel ProductionPlacePnl;
    public TAOWPanel NamePnl;
    public TAOWMemo ActiveActionMemo;
    public TAOWPanel SignPnl;
    public TAOWMemo SignText;
    public TAOWLabel SignLbl;
    public TAoWFrame SignTLine;
    public TAOWImage SignTDecoL;
    public TAOWImage SignTDecoR;
    public TAoWFrame SignBLine;
    public TAOWImage SignBDecoL;
    public TAOWImage SingBDemoR;
    public TAOWVScrollBar SignTextSB;
    public TAOWPanel CityPanel;
    public TAOWLabel CityRaceLbl;
    public TAOWPanel CityBorderPnl;
    public TAOWImage ProdOrb;
    public TAOWPanel SingleButPnl;
    public TAOWMemo SBmemo;
    public TAOWPanel AltarPnl;
    public TAoWGauge Bar;
    public TAOWLabel ChargeLbl;
    public TAOWLabel GemLbl;
    public TAOWLabel ChargePcntLbl;
    public TAOWImage T2GemI;
    public TAOWImage T2TurnI;
    public TAOWLabel DayLbl;
    public TAOWImage LShield;
    public TAOWPanel NotYoursPnl;
    public TAOWMemo NotYoursMemo;
    public TAOWLabel Ownerlbl;
    public TAOWLabel OwnerRaceLbl;
    public TAOWLabel ProdOrbTurnsLbl;
    public TAOWLabel orLbl;
    public TAOWPanel AltarInfoPnl;
    public TAOWImage ProdOrbTime;
    public TAOWButton Center;
    public TAOWPanel ConstructionPnl;
    public TAOWMemo ConstructionMemo;
    public TAOWLabel UnderConstructionLbl;
    public TAOWLabel ConstructionDaysLeftLbl;
    public TAOWPanel RazePnl;
    public TAOWImage RazeBg;
    public TAOWButton RazeBtn;
    public TAOWLabel CivilStatusLbl;
    public TAOWImage RaceRelationIcon;
    public TAOWLabel CityIncomeLbl;
    public TAOWImage GoldIcn;
    public TAOWImage UgradeI;
    public TAOWLabel CityLevelLbl;
    public TAOWPanel InfoPnl;
    public TAOWPanel ProductionPlaceInfoPnl;
    public TAOWImage IncomeTypeIcon;
    public TAOWLabel IncomeValueLbl;
    public TAOWMemo ActiveCityActionMemo;
    public TAOWLabel CostToFireLbl;
    public TAOWLabel CurrentActivityLbl;
    public TAOWLabel IncomeLbl;
    public TAOWLabel ProductionWarningLbl;
    public TAOWLabel CurrentActivityWarningLbl;
    public TAOWPanel IncomePnl;
    public TAOWPanel IncomInfoPnl;
    public TAOWImage IncomeTypeImage;
    public TAOWLabel IncomeAmountLbl;
    public TAOWLabel IncomeTextLbl;
    public TAOWMemo IncomeMemo;
    public TAOWPanel ItemPnl;
    public TAOWLabel ItemsInBagLbl;
    public TAOWListBox ItembagLB;
    public TAOWVScrollBar ItembagSB;
    public TAOWMemo ItemBagMemo;
    public TAOWVScrollBar SBSB;
    public TAOWVScrollBar NotYoursMemoSB;
    public TAOWPanel ExplorationSitePnl;
    public TAOWMemo ESMemo;
    public TAOWVScrollBar ESSB;
    public TAOWLabel ESLbl;
    public TAOWLabel ESStatusLbl;
    public TAOWButton Help;
    public TAOWLabel PopulationRaceLbl;
    public TAOWLabel PopulationLbl;
    public TIvTranslator Translator;
    public TItemHS FItemBag;
    // Renderers of the structure views (Structure.Views.cs): the panel of each InfoPanel key and the
    // screen of each InfoButton key. ClassRegistry adds the original renderers.
    public static readonly Catalog<StructureView, Action<TIWindow, TStructure>> StructurePanels = new("structure info panel");
    public static readonly Catalog<StructureView, Action<TIWindow, TStructure>> StructureButtons = new("structure info button");

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
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StructButton.Visible = false;
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
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(xhx, yhx, FItemBag.Lhx);

            var fItemBagYhx = FItemBag.Yhx;
            var fItemBagXhx = FItemBag.Xhx;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlayCenterAnimation(fItemBagXhx, fItemBagYhx, FItemBag.Lhx);
        }
        else
        {
            var yhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Yhx;
            var xhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Xhx;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.CenterViewTo(xhx, yhx, Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Lhx);
            var fStructureYhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Yhx;
            var fStructureXhx = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Xhx;
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlayCenterAnimation(fStructureXhx, fStructureYhx, Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Lhx);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure != null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.CanRaze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, ref Reason))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.Raze(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
            }
            else
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        TArmyHS Army;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure != null)
        {
            var structure = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure;
            if (StructureButtons.TryGet(structure.InfoButton(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer), out var open))
            {
                open(this, structure);
            }
        }
        else
        {
            if (FItemBag != null)
            {
                Army = ((TAoWMapField)FItemBag.Owner).GetArmyHS();
                if (Army != null && Army.FArmy.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
                {
                    if (Army.FArmy.Hero(255, Aow.Game.Magic.Abilities.RosterTables.AllHeroTypes) != null)
                    {
                        var hero = Army.FArmy.Hero(255, Aow.Game.Magic.Abilities.RosterTables.AllHeroTypes);
                        Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = hero;
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                        Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Selected = 2;
                    }
                }
            }
        }
    }

    // AoWInfoWin.pas:226
    public void ItembagLBDrawItem(in TRect DrawRect, int DrawItem, in TSurfaceDesc Surface)
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
        Aow.Game.Ui.Dialogs.THelpWindow.ShowHelp(504);
    }

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ESMemo.FDrawn = false;
        InfoWin.FDrawn = false;
    }

    internal static void SetStructureHeader(TIWindow Dialog, TStructure Structure)
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

    internal static void ShowMineInfo(TIWindow Dialog, TMine Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 31;
        var income = Structure.GetIncome();
        Dialog.IncomeAmountLbl.Text = IntToStr(income);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowPowerNodeInfo(TIWindow Dialog, TPowerNode Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionIncome(TIWindow Dialog, TProductionPlace Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowFarmInfo(TIWindow Dialog, TFarm Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.Items.Add("");
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.IncomeXRStr), [IntToStr(Structure.FIncome)]);
        Dialog.SBmemo.Items.Add(format);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowSignInfo(TIWindow Dialog, TSign Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Aow.Game.Ui.Dialogs.DialogHosts.General.FSign = Structure;
        Dialog.SignPnl.Visible = true;
        Dialog.SignText.Items.Assign(Structure.FText);
    }

    internal static void ShowTeleportInfo(TIWindow Dialog, TTeleport Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.FSized = false;
        var canTeleport = Structure.CanTeleport(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canTeleport;
        var teleport = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TTeleportResource.TeleportRStr);
        Dialog.MainBtn.Text = teleport;
        Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
    }

    internal static void ShowTowerInfo(TIWindow Dialog, TWizardsTower Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowArenaInfo(TIWindow Dialog, TArena Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    internal static void ShowCaveInfo(TIWindow Dialog, TCave Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        switch (Structure.FEntrance)
        {
            case TCaveEntrance.ceUp:
                var goUp = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoUpRStr);
                Dialog.MainBtn.Text = goUp;
                break;
            case TCaveEntrance.ceDown:
                var goDown = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GoDownRStr);
                Dialog.MainBtn.Text = goDown;
                break;
        }
    }

    internal static void ShowWatchTowerInfo(TIWindow Dialog, TTower Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowExplorationInfo(TIWindow Dialog, TExplorationSite Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.ExplorationSitePnl.Visible = true;
        var description = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure.GetDescription();
        Dialog.ESMemo.Items.Assign(description);
        var status = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TCityScreen.StatusRStr);
        Dialog.ESLbl.Text = status + ":";
        if (Structure.GetExplored())
        {
            var explored = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ExploredRStr);
            Dialog.ESStatusLbl.Text = explored;
        }
        else
        {
            var unexplored = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnexploredRStr);
            Dialog.ESStatusLbl.Text = unexplored;
        }

        var canSearch = ((TExplorationSite)Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure).CanSearch(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canSearch;
        var search = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SearchRStr);
        Dialog.MainBtn.Text = search;
    }

    internal static void ShowAltarInfo(TIWindow Dialog, TAltar Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.AltarPnl.Visible = true;
        if (Structure.GetTargeting())
        {
            var cancel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CancelRStr);
            Dialog.MainBtn.Text = cancel;
        }
        else
        {
            var target = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TargetRStr);
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

    internal static void ShowWarCityInfo(TIWindow Dialog, TCity Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        var statusWarmode = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StatusWarmodeRStr);
        Dialog.SBmemo.Items.Add(statusWarmode);
    }

    internal static void ShowCityInfo(TIWindow Dialog, TCity Structure)
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FCity = Structure;
        Dialog.CityPanel.Visible = true;
        Dialog.ProductionWarningLbl.Visible = false;
        Dialog.ActiveCityActionMemo.Items.Clear();
        var name = Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.GetName();
        Dialog.StructNameLbl.Text = name;
        var raceNames = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesRStr[Aow.Game.Ui.Dialogs.DialogHosts.General.FCity.FRace]);
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.RaceXRStr), [raceNames]);
        Dialog.CityRaceLbl.Text = format;
        Dialog.CityLevelLbl.Text = IntToStr(Structure.FUpgradeLevel);
        Dialog.CityIncomeLbl.Text = IntToStr(Structure.FIncome);
        var civilStatus = Structure.GetCivilStatus();
        var civilStatusText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TCityScreen.TCivilStatusRStr[(int)civilStatus]);
        var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.StatusXRStr), [civilStatusText]);
        Dialog.CivilStatusLbl.Text = aoWEFormat;
        if (Structure.FProduction.FQueue.GetCount() != 0)
        {
            Structure.FProduction.FQueue.GetItem(0).Order.ActivityLines(Structure.FProduction, Dialog.ActiveCityActionMemo.Items);

            if (Structure.FProduction.FStatus == TProductionControlStatus.pcsProducing)
            {
                if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                    Dialog.ActiveCityActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    var unlimited = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TMapEvents.UnlimitedRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (Structure.FProduction.FStatus == TProductionControlStatus.pcsWaiting)
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
            var producingMerchandise = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingMerchandiseRStr);
            Dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRaces.GetRace(Structure.FRace).GetPlayerRelation(Structure.FPlayer);
        Dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowProductionInfo(TIWindow Dialog, TProductionPlace Structure)
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        var enter = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionPlace = (TProductionPlace)Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure;
        Dialog.ProductionPlacePnl.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionControl = Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionPlace.FProductionControl;
        Dialog.CurrentActivityWarningLbl.Visible = false;
        Dialog.ActiveActionMemo.Items.Clear();
        Dialog.IncomeLbl.Visible = true;
        Dialog.IncomeTypeIcon.Visible = true;
        Dialog.IncomeValueLbl.Visible = true;
        if (Structure.GetMagicSphere() != TMagicSphere.msNone)
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

            if (Structure.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
            {
                if (Structure.FProductionControl.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                    Dialog.ActiveActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    var unlimited = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TMapEvents.UnlimitedRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(LowerCase(unlimited));
                }
            }
            else
            {
                if (Structure.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
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
            if (Structure.GetMagicSphere() == TMagicSphere.msNone)
            {
                Dialog.ActiveActionMemo.Items.Add("");
                if (Structure.GetBaseIncome() != 0)
                {
                    var producingMerchandise = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ProducingMerchandiseRStr);
                    Dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
                    Dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.GeneratingExtraPowerRStr);
                Dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        Dialog.RazePnl.Visible = true;
    }

    internal static void ShowBuildingInfo(TIWindow Dialog, TStructure Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.ConstructionPnl.Visible = true;
        var fmtDaysLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtDaysLeftStr(Structure.FBuildingDays);
        Dialog.ConstructionDaysLeftLbl.Text = fmtDaysLeftStr;
        var description = Structure.GetDescription();
        Dialog.ConstructionMemo.Items.Assign(description);
    }

    internal static void ShowRazedInfo(TIWindow Dialog, TStructure Structure)
    {
        string Text;
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        if (Structure is TCity)
        {
            var name = Structure.GetName();
            Text = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CityOfXRStr), [name]);
        }
        else
        {
            Text = Structure.GetName();
        }

        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TheXliesInRuinsRStr), [Text]);
        Dialog.SBmemo.Items.Text = format;
        Dialog.SBmemo.Items.Insert(0, "");
        Dialog.SBmemo.Items.Insert(0, "");
    }

    internal static void ShowOtherPlayerInfo(TIWindow Dialog, TPlayerStructure Structure)
    {
        int RaceIndex;
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.NotYoursPnl.Visible = true;
        if (Structure is TCity && Structure.FPlayer == 0)
        {
            Dialog.PopulationRaceLbl.Visible = true;
            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[((TCity)Structure).FRace]);
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
            RaceIndex = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[RaceIndex]);
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

    internal static void ShowGenericInfo(TIWindow Dialog, TStructure Structure)
    {
        Aow.Game.Ui.Dialogs.TMapEvents.ResetStructureInfo(Dialog);
        Aow.Game.Ui.Dialogs.TIWindow.SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
    }

}
