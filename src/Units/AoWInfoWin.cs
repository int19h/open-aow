// Translated from AoWInfoWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
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
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.PrdPlace;
using Aow.Units.Cave;
using Aow.Units.Teleport;
using Aow.Units.ExploreS;
using Aow.Units.WizardsTower;
using Aow.Units.Arena;
using Aow.Units.Altar;
using static Aow.Units.aowHelpWin.aowHelpWin;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWInfoWin
{
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
                var call__3 = FItemBag.Yhx;
                var call__4 = itemHS.Xhx;
                AoWHSMap.CenterViewTo(call__4, call__3, FItemBag.Lhx);

                var call__5 = FItemBag.Yhx;
                var call__6 = FItemBag.Xhx;
                AoWHSMap.PlayCenterAnimation(call__6, call__5, FItemBag.Lhx);
            }
            else
            {
                var call__7 = General.FStructure.Yhx;
                var call__8 = General.FStructure.Xhx;
                AoWHSMap.CenterViewTo(call__8, call__7, General.FStructure.Lhx);
                var call__9 = General.FStructure.Yhx;
                var call__10 = General.FStructure.Xhx;
                AoWHSMap.PlayCenterAnimation(call__10, call__9, General.FStructure.Lhx);
            }
        }

        // AoWInfoWin.pas:180
        public void RazeBtnClick()
        {
            AnsiString Reason = default;
            General.PlayButtonSound(0);
            if (General.FStructure != null)
            {
                var call__11 = General.FStructure.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason);
                if (call__11)
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
            TArmyHS Army = default;
            AnsiString Reason = default;
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
                                                var call__12 = ((TAltar)General.FStructure).GetTargeting();
                                                if (!call__12)
                                                {
                                                    var call__13 = ((TAltar)General.FStructure).CanTarget(ref Reason);
                                                    if (call__13)
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
                    var call__14 = ((TAoWMapField)FItemBag.Owner).GetArmyHS();
                    Army = call__14;
                    if (Army != null && Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
                    {
                        var call__15 = Army.FArmy.Hero(255, AllHeroTypes);
                        if (call__15 != null)
                        {
                            var call__16 = Army.FArmy.Hero(255, AllHeroTypes);
                            General.FSelectedUnit = call__16;
                            AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                            AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 2;
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
                var call__17 = FItemBag.FItems.Count;
                if (call__17 > ItembagLB.ListOffset)
                {
                    var call__18 = FItemBag.FItems.GetItem(ItembagLB.ListOffset);
                    call__18.Show(DrawRect.Left + 10, DrawRect.Top + 10, Surface);
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

    public static partial class AoWInfoWin
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
                        var call__19 = Structure.Selected;
                        var call__20 = !call__19;
                        if (call__20)
                        {
                            Structure = null;
                        }
                    }
                }

                if (Structure != null)
                {
                    var call__21 = Structure.Selected;
                    if (call__21)
                    {
                        ShowStructureInfo(Dialog, Structure);
                    }
                }
            }
        }

        // AoWInfoWin.pas:661
        public static void ShowItemSelection(TIWindow Dialog, TItemHS Items)
        {
            int I = default;
            TArmyHS Army = default;
            ResetStructureInfo(Dialog);
            var call__23 = TranslateRStr(E3Globals.E3Globals.ItemBagRStr);
            Dialog.Title.Text = call__23;
            Dialog.FItemBag = Items;
            Dialog.ItemPnl.Visible = true;
            Dialog.InfoWin.Visible = true;
            Dialog.StructNameLbl.Text = (AnsiString)"";
            var call__24 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[35];
            Dialog.StructLine.ImageTop = call__24;
            var call__25 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[36];
            Dialog.StructLine.ImageMiddle = call__25;
            var call__26 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[37];
            Dialog.StructLine.ImageBottom = call__26;
            var call__27 = ((TAoWMapField)Items.Owner).GetArmyHS();
            Army = call__27;
            if (Army != null && Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
            {
                Dialog.ItemBagMemo.Visible = false;
                var call__28 = Army.FArmy.Hero(255, AllHeroTypes);
                Dialog.MainBtn.Visible = call__28 != null;
                var call__29 = TranslateRStr(PickupRStr);
                Dialog.MainBtn.Text = call__29;
                Dialog.ItembagLB.Visible = true;
                Dialog.ItembagLB.Items.Clear();
                Dialog.ItembagSB.Visible = true;
                {
                    I = 0;
                    int end__30 = Items.FItems.Count - 1;
                    for (; I <= end__30; I += 1)
                    {
                        Dialog.ItembagLB.Items.Add((AnsiString)"");
                    }
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
            var call__32 = TranslateRStr(StructureRStr);
            Dialog.Title.Text = call__32;
        }

        // AoWInfoWin.pas:263
        public static void SetStructureHeader(TIWindow Dialog, TStructure Structure)
        {
            var call__34 = Structure.GetName();
            Dialog.StructNameLbl.Text = call__34;
            if (Structure.GetBottomBorderImage() != null)
            {
                var call__35 = Structure.GetTopBorderImage();
                Dialog.StructLine.ImageTop = call__35;
                var call__36 = Structure.GetMiddleBorderImage();
                Dialog.StructLine.ImageMiddle = call__36;
                var call__37 = Structure.GetBottomBorderImage();
                Dialog.StructLine.ImageBottom = call__37;
            }

            Dialog.StructLine.Visible = true;
        }

        // AoWInfoWin.pas:278
        public static void ShowMineInfo(TIWindow Dialog, TMine Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__39 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__39);
            Dialog.IncomeTypeImage.CurrentImage = 31;
            var call__40 = Structure.GetIncome();
            Dialog.IncomeAmountLbl.Text = IntToStr(call__40);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:291
        public static void ShowPowerNodeInfo(TIWindow Dialog, TPowerNode Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__42 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__42);
            Dialog.IncomeTypeImage.CurrentImage = 33;
            var call__43 = Structure.GetPower();
            Dialog.IncomeAmountLbl.Text = IntToStr(call__43);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:304
        public static void ShowProductionIncome(TIWindow Dialog, TProductionPlace Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__45 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__45);
            Dialog.IncomeTypeImage.CurrentImage = 33;
            var call__46 = Structure.GetPower();
            Dialog.IncomeAmountLbl.Text = IntToStr(call__46);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:317
        public static void ShowFarmInfo(TIWindow Dialog, TFarm Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__48 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__48);
            Dialog.SBmemo.Items.Add((AnsiString)"");
            var call__49 = TranslateRStr(IncomeXRStr);
            var call__50 = AoWE.AoWE.Format(call__49, [IntToStr(Structure.FIncome)]);
            Dialog.SBmemo.Items.Add(call__50);
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
            var call__52 = General.FStructure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__52);
            Dialog.SBmemo.FSized = false;
            var call__53 = Structure.CanTeleport(AoWHSMap.FSeatedPlayer);
            Dialog.MainBtn.Visible = call__53;
            var call__54 = TranslateRStr(TeleportRStr);
            Dialog.MainBtn.Text = call__54;
            Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
        }

        // AoWInfoWin.pas:351
        public static void ShowTowerInfo(TIWindow Dialog, TWizardsTower Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__56 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__56);
            var call__57 = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
            Dialog.MainBtn.Visible = call__57;
            var call__58 = TranslateRStr(EnterRStr);
            Dialog.MainBtn.Text = call__58;
        }

        // AoWInfoWin.pas:362
        public static void ShowArenaInfo(TIWindow Dialog, TArena Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__60 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__60);
            var call__61 = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
            Dialog.MainBtn.Visible = call__61;
            var call__62 = TranslateRStr(EnterRStr);
            Dialog.MainBtn.Text = call__62;
        }

        // AoWInfoWin.pas:373
        public static void ShowCaveInfo(TIWindow Dialog, TCave Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__64 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__64);
            var call__65 = Structure.CanEnter(AoWHSMap.FSeatedPlayer);
            Dialog.MainBtn.Visible = call__65;
            switch (Structure.FEntrance)
            {
                case TCaveEntrance.ceUp:
                    var call__66 = TranslateRStr(GoUpRStr);
                    Dialog.MainBtn.Text = call__66;
                    break;
                case TCaveEntrance.ceDown:
                    var call__67 = TranslateRStr(GoDownRStr);
                    Dialog.MainBtn.Text = call__67;
                    break;
            }
        }

        // AoWInfoWin.pas:387
        public static void ShowWatchTowerInfo(TIWindow Dialog, TTower Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__68 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__68);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:395
        public static void ShowExplorationInfo(TIWindow Dialog, TExplorationSite Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.ExplorationSitePnl.Visible = true;
            var call__70 = General.FStructure.GetDescription();
            Dialog.ESMemo.Items.Assign(call__70);
            var call__71 = TranslateRStr(StatusRStr);
            Dialog.ESLbl.Text = call__71 + AnsiString.FromBytes(58);
            if (Structure.GetExplored())
            {
                var call__72 = TranslateRStr(ExploredRStr);
                Dialog.ESStatusLbl.Text = call__72;
            }
            else
            {
                var call__73 = TranslateRStr(UnexploredRStr);
                Dialog.ESStatusLbl.Text = call__73;
            }

            var call__74 = ((TExplorationSite)General.FStructure).CanSearch(AoWHSMap.FSeatedPlayer);
            Dialog.MainBtn.Visible = call__74;
            var call__75 = TranslateRStr(SearchRStr);
            Dialog.MainBtn.Text = call__75;
        }

        // AoWInfoWin.pas:410
        public static void ShowAltarInfo(TIWindow Dialog, TAltar Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.AltarPnl.Visible = true;
            if (Structure.GetTargeting())
            {
                var call__77 = TranslateRStr(CancelRStr);
                Dialog.MainBtn.Text = call__77;
            }
            else
            {
                var call__78 = TranslateRStr(TargetRStr);
                Dialog.MainBtn.Text = call__78;
            }

            Dialog.MainBtn.Visible = true;
            var call__79 = Structure.GetRechargeDays();
            Dialog.Bar.Max = call__79;
            Dialog.Bar.Position = Structure.FRechargeDays;
            var call__80 = Structure.GetRechargeDays();
            Dialog.ChargePcntLbl.Text = IntToStr(Round((double)(Structure.FRechargeDays * 100) / call__80)) + AnsiString.FromBytes(37);
            var call__81 = Structure.GetRechargeDays();
            Dialog.DayLbl.Text = IntToStr(call__81 - Structure.FRechargeDays);
            var call__82 = Structure.TargetMana();
            Dialog.GemLbl.Text = IntToStr(call__82);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:427
        public static void ShowWarCityInfo(TIWindow Dialog, TCity Structure)
        {
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            var call__84 = TranslateRStr(StatusWarmodeRStr);
            Dialog.SBmemo.Items.Add(call__84);
        }

        // AoWInfoWin.pas:439
        public static void ShowCityInfo(TIWindow Dialog, TCity Structure)
        {
            TUnit UnitObject = default;
            AnsiString Reason = default;
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            var call__86 = TranslateRStr(EnterRStr);
            Dialog.MainBtn.Text = call__86;
            Dialog.MainBtn.Visible = true;
            General.FCity = Structure;
            Dialog.CityPanel.Visible = true;
            Dialog.ProductionWarningLbl.Visible = false;
            Dialog.ActiveCityActionMemo.Items.Clear();
            var call__87 = General.FCity.GetName();
            Dialog.StructNameLbl.Text = call__87;
            var call__88 = TranslateRStr(RaceNamesRStr[General.FCity.FRace]);
            var call__89 = TranslateRStr(RaceXRStr);
            var call__90 = AoWE.AoWE.Format(call__89, [call__88]);
            Dialog.CityRaceLbl.Text = call__90;
            Dialog.CityLevelLbl.Text = IntToStr(Structure.FUpgradeLevel);
            Dialog.CityIncomeLbl.Text = IntToStr(Structure.FIncome);
            var call__91 = Structure.GetCivilStatus();
            var call__92 = TranslateRStr(TCivilStatusRStr[(int)call__91]);
            var call__93 = TranslateRStr(StatusXRStr);
            var call__94 = AoWE.AoWE.Format(call__93, [call__92]);
            Dialog.CivilStatusLbl.Text = call__94;
            var call__95 = Structure.FProduction.FQueue.GetCount();
            if (call__95 != 0)
            {
                var call__96 = Structure.FProduction.FQueue.GetItem(0);
                switch (call__96.FProductionID)
                {
                    case 2:
                        var call__97 = Structure.FProduction.FQueue.GetItem(0);
                        var call__98 = Structure.FProduction.CreateUnitProduction(call__97.FParameter);
                        UnitObject = call__98;
                        var call__99 = TranslateRStr(InstallingRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(call__99);
                        var call__100 = UnitObject.GetName();
                        Dialog.ActiveCityActionMemo.Items.Add(call__100);
                        UnitObject.Release();
                        break;
                    case 1:
                        var call__101 = Structure.FProduction.FQueue.GetItem(0);
                        var call__102 = Structure.FProduction.CreateUnitProduction(call__101.FParameter);
                        UnitObject = call__102;
                        Dialog.ActiveCityActionMemo.Items.Clear();
                        var call__103 = TranslateRStr(ProducingRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(call__103);
                        var call__104 = UnitObject.GetName();
                        Dialog.ActiveCityActionMemo.Items.Add(call__104);
                        UnitObject.Release();
                        break;
                    default:
                        var call__105 = Structure.FProduction.FQueue.GetItem(0);
                        var call__106 = call__105.GetNameProgressive();
                        Dialog.ActiveCityActionMemo.Items.Text = call__106;
                        break;
                }

                if (Structure.FProduction.FStatus == TProductionControlStatus.pcsProducing)
                {
                    if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                    {
                        var call__107 = FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                        Dialog.ActiveCityActionMemo.Items.Add(call__107);
                    }
                    else
                    {
                        var call__108 = TranslateRStr(UnlimitedRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(LowerCase(call__108));
                    }
                }
                else
                {
                    if (Structure.FProduction.FStatus == TProductionControlStatus.pcsWaiting)
                    {
                        var call__109 = Structure.FProduction.FQueue.GetItem(0);
                        Structure.FProduction.CanExecuteProduction(call__109, ref Reason);
                        Dialog.ProductionWarningLbl.Text = Reason;
                        Dialog.ProductionWarningLbl.Visible = true;
                    }
                }
            }
            else
            {
                Dialog.ActiveCityActionMemo.Items.Add((AnsiString)"");
                var call__110 = TranslateRStr(ProducingMerchandiseRStr);
                Dialog.ActiveCityActionMemo.Items.Add(call__110);
            }

            var call__111 = AoWHSMap.FRaces.GetRace(Structure.FRace);
            var call__112 = call__111.GetPlayerRelation(Structure.FPlayer);
            Dialog.RaceRelationIcon.CurrentImage = (int)call__112 + 11;
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:496
        public static void ShowProductionInfo(TIWindow Dialog, TProductionPlace Structure)
        {
            TUnit UnitObject = default;
            AnsiString Reason = default;
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            var call__114 = TranslateRStr(EnterRStr);
            Dialog.MainBtn.Text = call__114;
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
                var call__115 = Structure.GetPower();
                Dialog.IncomeValueLbl.Text = IntToStr(call__115);
            }
            else
            {
                if (Structure.GetBaseIncome() != 0)
                {
                    Dialog.IncomeTypeIcon.CurrentImage = 31;
                    var call__116 = Structure.GetIncome();
                    Dialog.IncomeValueLbl.Text = IntToStr(call__116);
                }
                else
                {
                    Dialog.IncomeLbl.Visible = false;
                    Dialog.IncomeTypeIcon.Visible = false;
                    Dialog.IncomeValueLbl.Visible = false;
                }
            }

            Dialog.ProductionPlacePnl.Visible = true;
            var call__117 = Structure.GetName();
            Dialog.StructNameLbl.Text = call__117;
            var call__118 = Structure.FProductionControl.FQueue.GetCount();
            if (call__118 != 0)
            {
                var call__119 = Structure.FProductionControl.FQueue.GetItem(0);
                switch (call__119.FProductionID)
                {
                    case 2:
                        var call__120 = Structure.FProductionControl.FQueue.GetItem(0);
                        var call__121 = Structure.FProductionControl.CreateUnitProduction(call__120.FParameter);
                        UnitObject = call__121;
                        var call__122 = TranslateRStr(InstallingRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__122);
                        var call__123 = UnitObject.GetName();
                        Dialog.ActiveActionMemo.Items.Add(call__123);
                        UnitObject.Release();
                        break;
                    case 1:
                        var call__124 = Structure.FProductionControl.FQueue.GetItem(0);
                        var call__125 = Structure.FProductionControl.CreateUnitProduction(call__124.FParameter);
                        UnitObject = call__125;
                        Dialog.ActiveActionMemo.Items.Clear();
                        var call__126 = TranslateRStr(ProducingRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__126);
                        var call__127 = UnitObject.GetName();
                        Dialog.ActiveActionMemo.Items.Add(call__127);
                        UnitObject.Release();
                        break;
                    default:
                        var call__128 = Structure.FProductionControl.FQueue.GetItem(0);
                        var call__129 = call__128.GetNameProgressive();
                        Dialog.ActiveActionMemo.Items.Text = call__129;
                        break;
                }

                if (Structure.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
                {
                    if (Structure.FProductionControl.FSettings.FTurnsLeft != -1)
                    {
                        var call__130 = FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                        Dialog.ActiveActionMemo.Items.Add(call__130);
                    }
                    else
                    {
                        // { Original writes the city memo in this branch. }
                        var call__131 = TranslateRStr(UnlimitedRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(LowerCase(call__131));
                    }
                }
                else
                {
                    if (Structure.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
                    {
                        var call__132 = Structure.FProductionControl.FQueue.GetItem(0);
                        Structure.FProductionControl.CanExecuteProduction(call__132, ref Reason);
                        Dialog.CurrentActivityWarningLbl.Text = Reason;
                        Dialog.CurrentActivityWarningLbl.Visible = true;
                    }
                }
            }
            else
            {
                if (Structure.GetMagicSphere() == TMagicSphere.msNone)
                {
                    Dialog.ActiveActionMemo.Items.Add((AnsiString)"");
                    if (Structure.GetBaseIncome() != 0)
                    {
                        var call__133 = TranslateRStr(ProducingMerchandiseRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__133);
                    }
                    else
                    {
                        var call__134 = TranslateRStr(NoneRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__134);
                    }
                }
                else
                {
                    var call__135 = TranslateRStr(GeneratingExtraPowerRStr);
                    Dialog.ActiveActionMemo.Items.Add(call__135);
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
            var call__136 = FmtDaysLeftStr(Structure.FBuildingDays);
            Dialog.ConstructionDaysLeftLbl.Text = call__136;
            var call__137 = Structure.GetDescription();
            Dialog.ConstructionMemo.Items.Assign(call__137);
        }

        // AoWInfoWin.pas:566
        public static void ShowRazedInfo(TIWindow Dialog, TStructure Structure)
        {
            AnsiString Text = default;
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            if (Structure is TCity)
            {
                var call__139 = Structure.GetName();
                var call__140 = TranslateRStr(CityOfXRStr);
                var call__141 = AoWE.AoWE.Format(call__140, [call__139]);
                Text = call__141;
            }
            else
            {
                Text = Structure.GetName();
            }

            var call__142 = TranslateRStr(TheXliesInRuinsRStr);
            var call__143 = AoWE.AoWE.Format(call__142, [Text]);
            Dialog.SBmemo.Items.Text = call__143;
            Dialog.SBmemo.Items.Insert(0, (AnsiString)"");
            Dialog.SBmemo.Items.Insert(0, (AnsiString)"");
        }

        // AoWInfoWin.pas:581
        public static void ShowOtherPlayerInfo(TIWindow Dialog, TPlayerStructure Structure)
        {
            int RaceIndex = default;
            ResetStructureInfo(Dialog);
            SetStructureHeader(Dialog, Structure);
            Dialog.NotYoursPnl.Visible = true;
            if (Structure is TCity && Structure.FPlayer == 0)
            {
                Dialog.PopulationRaceLbl.Visible = true;
                var call__145 = TranslateRStr(RaceNamesPluralRStr[((TCity)Structure).FRace]);
                Dialog.PopulationRaceLbl.Text = call__145;
                Dialog.PopulationLbl.Visible = true;
                Dialog.NotYoursMemo.Items.Clear();
            }
            else
            {
                var call__146 = Structure.GetDescription();
                Dialog.NotYoursMemo.Items.Assign(call__146);
            }

            if (Structure.FPlayer != -1)
            {
                var call__147 = AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                RaceIndex = call__147.FPlayerRace;
                if (RaceIndex == -1)
                {
                    RaceIndex = 12;
                }

                var call__148 = TranslateRStr(RaceNamesPluralRStr[RaceIndex]);
                Dialog.OwnerRaceLbl.Text = call__148;
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
            var call__149 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__149);
        }
    }
}
