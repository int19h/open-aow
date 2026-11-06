// Translated from AoWInfoWin.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AOWGauge;
using Aow.Units.AoWArnoLine;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.Mine;
using Aow.Units.PowerNode;
using Aow.Units.Farm;
using Aow.Units.Tower;
using Aow.Units.Sign;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWCityScreen.AoWCityScreenUnit;
using static Aow.Units.AoWProdScreen.AoWProdScreenUnit;
using Aow.Units.City;
using static Aow.Units.City.CityUnit;
using Aow.Units.PrdPlace;
using Aow.Units.Cave;
using Aow.Units.Teleport;
using Aow.Units.ExploreS;
using Aow.Units.WizardsTower;
using Aow.Units.Arena;
using Aow.Units.Altar;
using static Aow.Units.aowHelpWin.aowHelpWinUnit;
using static Aow.Units.E2CampaignSupport.E2CampaignSupportUnit;
using Aow.Units.AoWUnitWindow;
using Aow.Units.E3Globals;
using Aow.Units.aowIntGfxMod;

namespace Aow.Units.AoWInfoWin;

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
    // AoWInfoWin.pas:137
    public void InfoWinResize()
    {
        this.InfoWin.Alignment.RightOffset = this.InfoWin.FWinManager.WinWidth - this.InfoWin.WinLeft - this.InfoWin.WinWidth;
        this.InfoWin.Alignment.TopOffset = this.InfoWin.WinTop;
    }

    // AoWInfoWin.pas:146
    public void MinClick()
    {
        General.PlayButtonSound(0);
        InfoWin.Visible = false;
    }

    // AoWInfoWin.pas:152
    public void CloseClick(TObject Sender)
    {
        if (Sender != null)
        {
            General.PlayButtonSound(0);
        }

        InfoWin.Visible = false;
        TBWindow.StructButton.Visible = false;
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
        General.PlayButtonSound(0);
        if (FItemBag != null)
        {
            var itemHS = FItemBag;
            var yhx = FItemBag.Yhx;
            var xhx = itemHS.Xhx;
            AoWHSMap.CenterViewTo(xhx, yhx, FItemBag.Lhx);

            var fItemBagYhx = FItemBag.Yhx;
            var fItemBagXhx = FItemBag.Xhx;
            AoWHSMap.PlayCenterAnimation(fItemBagXhx, fItemBagYhx, FItemBag.Lhx);
        }
        else
        {
            var yhx = General.FStructure.Yhx;
            var xhx = General.FStructure.Xhx;
            AoWHSMap.CenterViewTo(xhx, yhx, General.FStructure.Lhx);
            var fStructureYhx = General.FStructure.Yhx;
            var fStructureXhx = General.FStructure.Xhx;
            AoWHSMap.PlayCenterAnimation(fStructureXhx, fStructureYhx, General.FStructure.Lhx);
        }
    }

    // AoWInfoWin.pas:180
    public void RazeBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (General.FStructure != null)
        {
            if (General.FStructure.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason))
            {
                General.FStructure.Raze(AoWHSMap.FSeatedPlayer);
            }
            else
            {
                AoWHSMap.ShowWarning(Reason);
            }
        }
    }

    // AoWInfoWin.pas:190
    public void MainBtnClick()
    {
        TArmyHS Army;
        string Reason = "";
        General.PlayButtonSound(0);
        if (General.FStructure != null)
        {
            if (General.FStructure is TCity)
            {
                CityScreen.ShowCity((TCity)General.FStructure);
            }
            else
            {
                if (General.FStructure is TProductionPlace)
                {
                    ProductionScreen.ShowPlace((TProductionPlace)General.FStructure);
                }
                else
                {
                    if (General.FStructure is TCave)
                    {
                        ((TCave)General.FStructure).Enter(AoWHSMap.FSeatedPlayer);
                    }
                    else
                    {
                        if (General.FStructure is TTeleport)
                        {
                            ((TTeleport)General.FStructure).Teleport(AoWHSMap.FSeatedPlayer);
                        }
                        else
                        {
                            if (General.FStructure is TExplorationSite)
                            {
                                ((TExplorationSite)General.FStructure).Search();
                            }
                            else
                            {
                                if (General.FStructure is TWizardsTower)
                                {
                                    ((TWizardsTower)General.FStructure).Enter();
                                }
                                else
                                {
                                    if (General.FStructure is TArena)
                                    {
                                        ((TArena)General.FStructure).Enter();
                                    }
                                    else
                                    {
                                        if (General.FStructure is TAltar)
                                        {
                                            if (!((TAltar)General.FStructure).GetTargeting())
                                            {
                                                if (((TAltar)General.FStructure).CanTarget(ref Reason))
                                                {
                                                    ((TAltar)General.FStructure).Target();
                                                }
                                                else
                                                {
                                                    AoWHSMap.ShowWarning(Reason);
                                                }
                                            }
                                            else
                                            {
                                                ((TAltar)General.FStructure).CancelTargeting();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (FItemBag != null)
            {
                Army = ((TAoWMapField)FItemBag.Owner).GetArmyHS();
                if (Army != null && Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    if (Army.FArmy.Hero(255, AllHeroTypes) != null)
                    {
                        var hero = Army.FArmy.Hero(255, AllHeroTypes);
                        General.FSelectedUnit = hero;
                        AoWUnitWindowUnit.UnitWindow.ShowUnit();
                        AoWUnitWindowUnit.UnitWindow.UnitTab.Selected = 2;
                    }
                }
            }
        }
    }

    // AoWInfoWin.pas:226
    public void ItembagLBDrawItem(TObject Sender, in TRect DrawRect, int DrawItem, in TSurfaceDesc Surface)
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
        ShowHelp(504);
    }

    // AoWInfoWin.pas:236
    public void ESSBChange()
    {
        ESMemo.FDrawn = false;
        InfoWin.FDrawn = false;
    }
}

public static partial class AoWInfoWinUnit
{
    public static TIWindow IWindow;
    // AoWInfoWin.pas:616
    public static void ShowStructureInfo(TIWindow Dialog, TStructure Structure)
    {
        if (Structure.GetBuilding())
        {
            ShowBuildingInfo(Dialog, Structure);
        }
        else
        {
            if (Structure.GetRazed())
            {
                ShowRazedInfo(Dialog, Structure);
            }
            else
            {
                if (Structure is TPlayerStructure)
                {
                    if (((TPlayerStructure)Structure).FPlayer == AoWHSMap.FSeatedPlayer || AoWEngine.FDebug)
                    {
                        if (Structure is TCity)
                        {
                            if (((TCity)Structure).FMode.Contains(TCityModeElement.cmWarmode))
                            {
                                ShowWarCityInfo(Dialog, (TCity)Structure);
                            }
                            else
                            {
                                ShowCityInfo(Dialog, (TCity)Structure);
                            }
                        }
                        else
                        {
                            if (Structure is TProductionPlace)
                            {
                                if (((TProductionPlace)Structure).GetMagicSphere() == TMagicSphere.msNone)
                                {
                                    ShowProductionInfo(Dialog, (TProductionPlace)Structure);
                                }
                                else
                                {
                                    ShowProductionIncome(Dialog, (TProductionPlace)Structure);
                                }
                            }
                            else
                            {
                                if (Structure is TMine)
                                {
                                    ShowMineInfo(Dialog, (TMine)Structure);
                                }
                                else
                                {
                                    if (Structure is TPowerNode)
                                    {
                                        ShowPowerNodeInfo(Dialog, (TPowerNode)Structure);
                                    }
                                    else
                                    {
                                        if (Structure is TFarm)
                                        {
                                            ShowFarmInfo(Dialog, (TFarm)Structure);
                                        }
                                        else
                                        {
                                            if (Structure is TTower)
                                            {
                                                ShowWatchTowerInfo(Dialog, (TTower)Structure);
                                            }
                                            else
                                            {
                                                if (Structure is TAltar)
                                                {
                                                    ShowAltarInfo(Dialog, (TAltar)Structure);
                                                }
                                                else
                                                {
                                                    ShowGenericInfo(Dialog, Structure);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Structure is TCity && ((TCity)Structure).FMode.Contains(TCityModeElement.cmWarmode))
                        {
                            ShowWarCityInfo(Dialog, (TCity)Structure);
                        }
                        else
                        {
                            ShowOtherPlayerInfo(Dialog, (TPlayerStructure)Structure);
                        }
                    }
                }
                else
                {
                    if (Structure is TSign)
                    {
                        ShowSignInfo(Dialog, (TSign)Structure);
                    }
                    else
                    {
                        if (Structure is TCave)
                        {
                            ShowCaveInfo(Dialog, (TCave)Structure);
                        }
                        else
                        {
                            if (Structure is TTeleport)
                            {
                                ShowTeleportInfo(Dialog, (TTeleport)Structure);
                            }
                            else
                            {
                                if (Structure is TExplorationSite)
                                {
                                    ShowExplorationInfo(Dialog, (TExplorationSite)Structure);
                                }
                                else
                                {
                                    if (Structure is TWizardsTower)
                                    {
                                        ShowTowerInfo(Dialog, (TWizardsTower)Structure);
                                    }
                                    else
                                    {
                                        if (Structure is TArena)
                                        {
                                            ShowArenaInfo(Dialog, (TArena)Structure);
                                        }
                                        else
                                        {
                                            ShowGenericInfo(Dialog, Structure);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (!TBWindow.StructButton.Visible)
        {
            IWindow.InfoWin.Visible = true;
        }

        TBWindow.StructButton.Visible = true;
    }

    // AoWInfoWin.pas:648
    public static void UpdateStructureInfo(TIWindow Dialog, TStructure Structure)
    {
        if (Dialog.InfoWin.Visible)
        {
            if (Structure == null)
            {
                Structure = General.FStructure;
                if (Structure != null)
                {
                    if (!Structure.Selected)
                    {
                        Structure = null;
                    }
                }
            }

            if (Structure != null)
            {
                if (Structure.Selected)
                {
                    ShowStructureInfo(Dialog, Structure);
                }
            }
        }
    }

    // AoWInfoWin.pas:661
    public static void ShowItemSelection(TIWindow Dialog, TItemHS Items)
    {
        TArmyHS Army;
        ResetStructureInfo(Dialog);
        var itemBag = TranslateRStr(E3GlobalsUnit.ItemBagRStr);
        Dialog.Title.Text = itemBag;
        Dialog.FItemBag = Items;
        Dialog.ItemPnl.Visible = true;
        Dialog.InfoWin.Visible = true;
        Dialog.StructNameLbl.Text = "";
        var miscArt = aowIntGfxModUnit.IntGfxMod.MiscArt[35];
        Dialog.StructLine.ImageTop = miscArt;
        var miscArt2 = aowIntGfxModUnit.IntGfxMod.MiscArt[36];
        Dialog.StructLine.ImageMiddle = miscArt2;
        var miscArt3 = aowIntGfxModUnit.IntGfxMod.MiscArt[37];
        Dialog.StructLine.ImageBottom = miscArt3;
        Army = ((TAoWMapField)Items.Owner).GetArmyHS();
        if (Army != null && Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
        {
            Dialog.ItemBagMemo.Visible = false;
            var hero = Army.FArmy.Hero(255, AllHeroTypes);
            Dialog.MainBtn.Visible = hero != null;
            var pickup = TranslateRStr(PickupRStr);
            Dialog.MainBtn.Text = pickup;
            Dialog.ItembagLB.Visible = true;
            Dialog.ItembagLB.Items.Clear();
            Dialog.ItembagSB.Visible = true;
            for (int I = 0, last = Items.FItems.Count - 1; I <= last; I++)
            {
                Dialog.ItembagLB.Items.Add("");
            }

            Dialog.ItemsInBagLbl.Visible = true;
        }
        else
        {
            Dialog.ItemBagMemo.Visible = true;
            Dialog.ItembagLB.Visible = false;
            Dialog.ItemsInBagLbl.Visible = false;
            Dialog.ItembagSB.Visible = false;
            Dialog.MainBtn.Visible = false;
        }
    }

    // AoWInfoWin.pas:692
    public static void UpdateItemSelection(TIWindow Dialog, TItemHS Items)
    {
        if (Dialog.InfoWin.Visible && Items == Dialog.FItemBag)
        {
            ShowItemSelection(Dialog, Dialog.FItemBag);
        }
    }

    // AoWInfoWin.pas:242
    public static void ResetStructureInfo(TIWindow Dialog)
    {
        Dialog.StructLine.Visible = true;
        Dialog.StructLine.FSized = false;
        Dialog.InfoWin.FAligned = false;
        Dialog.CityPanel.Visible = false;
        Dialog.CityBorderPnl.Visible = false;
        Dialog.ProductionPlacePnl.Visible = false;
        Dialog.SignPnl.Visible = false;
        Dialog.NotYoursPnl.Visible = false;
        Dialog.SingleButPnl.Visible = false;
        Dialog.MainBtn.Visible = true;
        Dialog.RazeBtn.Visible = true;
        Dialog.ExplorationSitePnl.Visible = false;
        Dialog.AltarPnl.Visible = false;
        Dialog.IncomePnl.Visible = false;
        Dialog.ItemPnl.Visible = false;
        Dialog.ConstructionPnl.Visible = false;
        Dialog.ProdOrb.Visible = false;
        Dialog.ProdOrbTime.Visible = false;
        Dialog.ProdOrbTurnsLbl.Visible = false;
        Dialog.RazePnl.Visible = false;
        Dialog.MainBtn.Visible = false;
        Dialog.SBmemo.Items.Clear();
        Dialog.PopulationRaceLbl.Visible = false;
        Dialog.PopulationLbl.Visible = false;
        var structure = TranslateRStr(StructureRStr);
        Dialog.Title.Text = structure;
    }

    // AoWInfoWin.pas:263
    public static void SetStructureHeader(TIWindow Dialog, TStructure Structure)
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

    // AoWInfoWin.pas:278
    public static void ShowMineInfo(TIWindow Dialog, TMine Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 31;
        var income = Structure.GetIncome();
        Dialog.IncomeAmountLbl.Text = IntToStr(income);
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:291
    public static void ShowPowerNodeInfo(TIWindow Dialog, TPowerNode Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:304
    public static void ShowProductionIncome(TIWindow Dialog, TProductionPlace Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.IncomePnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.IncomeMemo.Items.Assign(description);
        Dialog.IncomeTypeImage.CurrentImage = 33;
        var power = Structure.GetPower();
        Dialog.IncomeAmountLbl.Text = IntToStr(power);
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:317
    public static void ShowFarmInfo(TIWindow Dialog, TFarm Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.Items.Add("");
        var format = AoWEUnit.Format(TranslateRStr(IncomeXRStr), [IntToStr(Structure.FIncome)]);
        Dialog.SBmemo.Items.Add(format);
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:329
    public static void ShowSignInfo(TIWindow Dialog, TSign Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        General.FSign = Structure;
        Dialog.SignPnl.Visible = true;
        Dialog.SignText.Items.Assign(Structure.FText);
    }

    // AoWInfoWin.pas:337
    public static void ShowTeleportInfo(TIWindow Dialog, TTeleport Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = General.FStructure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.SBmemo.FSized = false;
        var canTeleport = Structure.CanTeleport(AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canTeleport;
        var teleport = TranslateRStr(TeleportRStr);
        Dialog.MainBtn.Text = teleport;
        Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
    }

    // AoWInfoWin.pas:351
    public static void ShowTowerInfo(TIWindow Dialog, TWizardsTower Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = TranslateRStr(EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    // AoWInfoWin.pas:362
    public static void ShowArenaInfo(TIWindow Dialog, TArena Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        var enter = TranslateRStr(EnterRStr);
        Dialog.MainBtn.Text = enter;
    }

    // AoWInfoWin.pas:373
    public static void ShowCaveInfo(TIWindow Dialog, TCave Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        var canEnter = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canEnter;
        switch (Structure.FEntrance)
        {
            case TCaveEntrance.ceUp:
                var goUp = TranslateRStr(GoUpRStr);
                Dialog.MainBtn.Text = goUp;
                break;
            case TCaveEntrance.ceDown:
                var goDown = TranslateRStr(GoDownRStr);
                Dialog.MainBtn.Text = goDown;
                break;
        }
    }

    // AoWInfoWin.pas:387
    public static void ShowWatchTowerInfo(TIWindow Dialog, TTower Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:395
    public static void ShowExplorationInfo(TIWindow Dialog, TExplorationSite Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.ExplorationSitePnl.Visible = true;
        var description = General.FStructure.GetDescription();
        Dialog.ESMemo.Items.Assign(description);
        var status = TranslateRStr(StatusRStr);
        Dialog.ESLbl.Text = status + ":";
        if (Structure.GetExplored())
        {
            var explored = TranslateRStr(ExploredRStr);
            Dialog.ESStatusLbl.Text = explored;
        }
        else
        {
            var unexplored = TranslateRStr(UnexploredRStr);
            Dialog.ESStatusLbl.Text = unexplored;
        }

        var canSearch = ((TExplorationSite)General.FStructure).CanSearch(AoWHSMap.FSeatedPlayer);
        Dialog.MainBtn.Visible = canSearch;
        var search = TranslateRStr(SearchRStr);
        Dialog.MainBtn.Text = search;
    }

    // AoWInfoWin.pas:410
    public static void ShowAltarInfo(TIWindow Dialog, TAltar Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.AltarPnl.Visible = true;
        if (Structure.GetTargeting())
        {
            var cancel = TranslateRStr(CancelRStr);
            Dialog.MainBtn.Text = cancel;
        }
        else
        {
            var target = TranslateRStr(TargetRStr);
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

    // AoWInfoWin.pas:427
    public static void ShowWarCityInfo(TIWindow Dialog, TCity Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        Dialog.SBmemo.Items.Add("");
        var statusWarmode = TranslateRStr(StatusWarmodeRStr);
        Dialog.SBmemo.Items.Add(statusWarmode);
    }

    // AoWInfoWin.pas:439
    public static void ShowCityInfo(TIWindow Dialog, TCity Structure)
    {
        TUnit UnitObject;
        string Reason = "";
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        var enter = TranslateRStr(EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        General.FCity = Structure;
        Dialog.CityPanel.Visible = true;
        Dialog.ProductionWarningLbl.Visible = false;
        Dialog.ActiveCityActionMemo.Items.Clear();
        var name = General.FCity.GetName();
        Dialog.StructNameLbl.Text = name;
        var raceNames = TranslateRStr(RaceNamesRStr[General.FCity.FRace]);
        var format = AoWEUnit.Format(TranslateRStr(RaceXRStr), [raceNames]);
        Dialog.CityRaceLbl.Text = format;
        Dialog.CityLevelLbl.Text = IntToStr(Structure.FUpgradeLevel);
        Dialog.CityIncomeLbl.Text = IntToStr(Structure.FIncome);
        var civilStatus = Structure.GetCivilStatus();
        var civilStatusText = TranslateRStr(TCivilStatusRStr[(int)civilStatus]);
        var aoWEFormat = AoWEUnit.Format(TranslateRStr(StatusXRStr), [civilStatusText]);
        Dialog.CivilStatusLbl.Text = aoWEFormat;
        if (Structure.FProduction.FQueue.GetCount() != 0)
        {
            switch (Structure.FProduction.FQueue.GetItem(0).FProductionID)
            {
                case 2:
                    var queueParameter = Structure.FProduction.FQueue.GetItem(0);
                    UnitObject = Structure.FProduction.CreateUnitProduction(queueParameter.FParameter);
                    var installing = TranslateRStr(InstallingRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(installing);
                    var unitObjectName = UnitObject.GetName();
                    Dialog.ActiveCityActionMemo.Items.Add(unitObjectName);
                    UnitObject.Release();
                    break;
                case 1:
                    var queue = Structure.FProduction.FQueue.GetItem(0);
                    UnitObject = Structure.FProduction.CreateUnitProduction(queue.FParameter);
                    Dialog.ActiveCityActionMemo.Items.Clear();
                    var producing = TranslateRStr(ProducingRStr);
                    Dialog.ActiveCityActionMemo.Items.Add(producing);
                    var name2 = UnitObject.GetName();
                    Dialog.ActiveCityActionMemo.Items.Add(name2);
                    UnitObject.Release();
                    break;
                default:
                    var nameProgressive = Structure.FProduction.FQueue.GetItem(0).GetNameProgressive();
                    Dialog.ActiveCityActionMemo.Items.Text = nameProgressive;
                    break;
            }

            if (Structure.FProduction.FStatus == TProductionControlStatus.pcsProducing)
            {
                if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                    Dialog.ActiveCityActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    var unlimited = TranslateRStr(UnlimitedRStr);
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
            var producingMerchandise = TranslateRStr(ProducingMerchandiseRStr);
            Dialog.ActiveCityActionMemo.Items.Add(producingMerchandise);
        }

        var playerRelation = AoWHSMap.FRaces.GetRace(Structure.FRace).GetPlayerRelation(Structure.FPlayer);
        Dialog.RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:496
    public static void ShowProductionInfo(TIWindow Dialog, TProductionPlace Structure)
    {
        TUnit UnitObject;
        string Reason = "";
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        var enter = TranslateRStr(EnterRStr);
        Dialog.MainBtn.Text = enter;
        Dialog.MainBtn.Visible = true;
        General.FProductionPlace = (TProductionPlace)General.FStructure;
        Dialog.ProductionPlacePnl.Visible = true;
        General.FProductionControl = General.FProductionPlace.FProductionControl;
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
            switch (Structure.FProductionControl.FQueue.GetItem(0).FProductionID)
            {
                case 2:
                    var queueParameter = Structure.FProductionControl.FQueue.GetItem(0);
                    UnitObject = Structure.FProductionControl.CreateUnitProduction(queueParameter.FParameter);
                    var installing = TranslateRStr(InstallingRStr);
                    Dialog.ActiveActionMemo.Items.Add(installing);
                    var unitObjectName = UnitObject.GetName();
                    Dialog.ActiveActionMemo.Items.Add(unitObjectName);
                    UnitObject.Release();
                    break;
                case 1:
                    var queue = Structure.FProductionControl.FQueue.GetItem(0);
                    UnitObject = Structure.FProductionControl.CreateUnitProduction(queue.FParameter);
                    Dialog.ActiveActionMemo.Items.Clear();
                    var producing = TranslateRStr(ProducingRStr);
                    Dialog.ActiveActionMemo.Items.Add(producing);
                    var name2 = UnitObject.GetName();
                    Dialog.ActiveActionMemo.Items.Add(name2);
                    UnitObject.Release();
                    break;
                default:
                    var nameProgressive = Structure.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
                    Dialog.ActiveActionMemo.Items.Text = nameProgressive;
                    break;
            }

            if (Structure.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
            {
                if (Structure.FProductionControl.FSettings.FTurnsLeft != -1)
                {
                    var fmtTurnsLeftStr = FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                    Dialog.ActiveActionMemo.Items.Add(fmtTurnsLeftStr);
                }
                else
                {
                    // { Original writes the city memo in this branch. }
                    var unlimited = TranslateRStr(UnlimitedRStr);
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
                    var producingMerchandise = TranslateRStr(ProducingMerchandiseRStr);
                    Dialog.ActiveActionMemo.Items.Add(producingMerchandise);
                }
                else
                {
                    var none = TranslateRStr(NoneRStr);
                    Dialog.ActiveActionMemo.Items.Add(none);
                }
            }
            else
            {
                var generatingExtraPower = TranslateRStr(GeneratingExtraPowerRStr);
                Dialog.ActiveActionMemo.Items.Add(generatingExtraPower);
            }
        }

        Dialog.RazePnl.Visible = true;
    }

    // AoWInfoWin.pas:558
    public static void ShowBuildingInfo(TIWindow Dialog, TStructure Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.ConstructionPnl.Visible = true;
        var fmtDaysLeftStr = FmtDaysLeftStr(Structure.FBuildingDays);
        Dialog.ConstructionDaysLeftLbl.Text = fmtDaysLeftStr;
        var description = Structure.GetDescription();
        Dialog.ConstructionMemo.Items.Assign(description);
    }

    // AoWInfoWin.pas:566
    public static void ShowRazedInfo(TIWindow Dialog, TStructure Structure)
    {
        string Text;
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        if (Structure is TCity)
        {
            var name = Structure.GetName();
            Text = AoWEUnit.Format(TranslateRStr(CityOfXRStr), [name]);
        }
        else
        {
            Text = Structure.GetName();
        }

        var format = AoWEUnit.Format(TranslateRStr(TheXliesInRuinsRStr), [Text]);
        Dialog.SBmemo.Items.Text = format;
        Dialog.SBmemo.Items.Insert(0, "");
        Dialog.SBmemo.Items.Insert(0, "");
    }

    // AoWInfoWin.pas:581
    public static void ShowOtherPlayerInfo(TIWindow Dialog, TPlayerStructure Structure)
    {
        int RaceIndex;
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.NotYoursPnl.Visible = true;
        if (Structure is TCity && Structure.FPlayer == 0)
        {
            Dialog.PopulationRaceLbl.Visible = true;
            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[((TCity)Structure).FRace]);
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
            RaceIndex = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer).FPlayerRace;
            if (RaceIndex == -1)
            {
                RaceIndex = 12;
            }

            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[RaceIndex]);
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

    // AoWInfoWin.pas:609
    public static void ShowGenericInfo(TIWindow Dialog, TStructure Structure)
    {
        ResetStructureInfo(Dialog);
        SetStructureHeader(Dialog, Structure);
        Dialog.SingleButPnl.Visible = true;
        var description = Structure.GetDescription();
        Dialog.SBmemo.Items.Assign(description);
    }
}
