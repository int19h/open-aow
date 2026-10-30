// Translated from AoWInfoWin.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWArnoLine;
using static Aow.Units.AoWArnoLine.AoWArnoLine;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.Mine;
using static Aow.Units.Mine.Mine;
using Aow.Units.PowerNode;
using static Aow.Units.PowerNode.PowerNode;
using Aow.Units.Farm;
using static Aow.Units.Farm.Farm;
using Aow.Units.Tower;
using static Aow.Units.Tower.Tower;
using Aow.Units.Sign;
using static Aow.Units.Sign.Sign;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWCityScreen;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using Aow.Units.AoWProdScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.Cave;
using static Aow.Units.Cave.Cave;
using Aow.Units.Teleport;
using static Aow.Units.Teleport.Teleport;
using Aow.Units.ExploreS;
using static Aow.Units.ExploreS.ExploreS;
using Aow.Units.WizardsTower;
using static Aow.Units.WizardsTower.WizardsTower;
using Aow.Units.Arena;
using static Aow.Units.Arena.Arena;
using Aow.Units.Altar;
using static Aow.Units.Altar.Altar;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWInfoWin
{
    // { TIWindow: size $1B4; $1B0 item bag. VMT 00441930. }
    public partial class TIWindow : Aow.Rtl.TDataModule
    {
        public TIWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow InfoWin;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton Min;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage SIcon;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton MainBtn;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame CeltFrame;
        // { +005C }
        public Aow.Units.AoWArnoLine.TAoWArnoLine StructLine;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel StructNameLbl;
        // { +0064 }
        public Aow.Units.AoWPanel.TAOWPanel ProductionPlacePnl;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel NamePnl;
        // { +006C }
        public Aow.Units.AoWMemo.TAOWMemo ActiveActionMemo;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel SignPnl;
        // { +0074 }
        public Aow.Units.AoWMemo.TAOWMemo SignText;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel SignLbl;
        // { +007C }
        public Aow.Units.AoWFrame.TAoWFrame SignTLine;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage SignTDecoL;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage SignTDecoR;
        // { +0088 }
        public Aow.Units.AoWFrame.TAoWFrame SignBLine;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage SignBDecoL;
        // { +0090 }
        public Aow.Units.AoWImage.TAOWImage SingBDemoR;
        // { +0094 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SignTextSB;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel CityPanel;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel CityRaceLbl;
        // { +00A0 }
        public Aow.Units.AoWPanel.TAOWPanel CityBorderPnl;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage ProdOrb;
        // { +00A8 }
        public Aow.Units.AoWPanel.TAOWPanel SingleButPnl;
        // { +00AC }
        public Aow.Units.AoWMemo.TAOWMemo SBmemo;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel AltarPnl;
        // { +00B4 }
        public Aow.Units.AOWGauge.TAoWGauge Bar;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel ChargeLbl;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel GemLbl;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel ChargePcntLbl;
        // { +00C4 }
        public Aow.Units.AoWImage.TAOWImage T2GemI;
        // { +00C8 }
        public Aow.Units.AoWImage.TAOWImage T2TurnI;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel DayLbl;
        // { +00D0 }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +00D4 }
        public Aow.Units.AoWPanel.TAOWPanel NotYoursPnl;
        // { +00D8 }
        public Aow.Units.AoWMemo.TAOWMemo NotYoursMemo;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel Ownerlbl;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel OwnerRaceLbl;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel ProdOrbTurnsLbl;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel orLbl;
        // { +00EC }
        public Aow.Units.AoWPanel.TAOWPanel AltarInfoPnl;
        // { +00F0 }
        public Aow.Units.AoWImage.TAOWImage ProdOrbTime;
        // { +00F4 }
        public Aow.Units.AoWButton.TAOWButton Center;
        // { +00F8 }
        public Aow.Units.AoWPanel.TAOWPanel ConstructionPnl;
        // { +00FC }
        public Aow.Units.AoWMemo.TAOWMemo ConstructionMemo;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel UnderConstructionLbl;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel ConstructionDaysLeftLbl;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel RazePnl;
        // { +010C }
        public Aow.Units.AoWImage.TAOWImage RazeBg;
        // { +0110 }
        public Aow.Units.AoWButton.TAOWButton RazeBtn;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel CivilStatusLbl;
        // { +0118 }
        public Aow.Units.AoWImage.TAOWImage RaceRelationIcon;
        // { +011C }
        public Aow.Units.AoWLabel.TAOWLabel CityIncomeLbl;
        // { +0120 }
        public Aow.Units.AoWImage.TAOWImage GoldIcn;
        // { +0124 }
        public Aow.Units.AoWImage.TAOWImage UgradeI;
        // { +0128 }
        public Aow.Units.AoWLabel.TAOWLabel CityLevelLbl;
        // { +012C }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl;
        // { +0130 }
        public Aow.Units.AoWPanel.TAOWPanel ProductionPlaceInfoPnl;
        // { +0134 }
        public Aow.Units.AoWImage.TAOWImage IncomeTypeIcon;
        // { +0138 }
        public Aow.Units.AoWLabel.TAOWLabel IncomeValueLbl;
        // { +013C }
        public Aow.Units.AoWMemo.TAOWMemo ActiveCityActionMemo;
        // { +0140 }
        public Aow.Units.AoWLabel.TAOWLabel CostToFireLbl;
        // { +0144 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentActivityLbl;
        // { +0148 }
        public Aow.Units.AoWLabel.TAOWLabel IncomeLbl;
        // { +014C }
        public Aow.Units.AoWLabel.TAOWLabel ProductionWarningLbl;
        // { +0150 }
        public Aow.Units.AoWLabel.TAOWLabel CurrentActivityWarningLbl;
        // { +0154 }
        public Aow.Units.AoWPanel.TAOWPanel IncomePnl;
        // { +0158 }
        public Aow.Units.AoWPanel.TAOWPanel IncomInfoPnl;
        // { +015C }
        public Aow.Units.AoWImage.TAOWImage IncomeTypeImage;
        // { +0160 }
        public Aow.Units.AoWLabel.TAOWLabel IncomeAmountLbl;
        // { +0164 }
        public Aow.Units.AoWLabel.TAOWLabel IncomeTextLbl;
        // { +0168 }
        public Aow.Units.AoWMemo.TAOWMemo IncomeMemo;
        // { +016C }
        public Aow.Units.AoWPanel.TAOWPanel ItemPnl;
        // { +0170 }
        public Aow.Units.AoWLabel.TAOWLabel ItemsInBagLbl;
        // { +0174 }
        public Aow.Units.AoWListBox.TAOWListBox ItembagLB;
        // { +0178 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar ItembagSB;
        // { +017C }
        public Aow.Units.AoWMemo.TAOWMemo ItemBagMemo;
        // { +0180 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SBSB;
        // { +0184 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar NotYoursMemoSB;
        // { +0188 }
        public Aow.Units.AoWPanel.TAOWPanel ExplorationSitePnl;
        // { +018C }
        public Aow.Units.AoWMemo.TAOWMemo ESMemo;
        // { +0190 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar ESSB;
        // { +0194 }
        public Aow.Units.AoWLabel.TAOWLabel ESLbl;
        // { +0198 }
        public Aow.Units.AoWLabel.TAOWLabel ESStatusLbl;
        // { +019C }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +01A0 }
        public Aow.Units.AoWLabel.TAOWLabel PopulationRaceLbl;
        // { +01A4 }
        public Aow.Units.AoWLabel.TAOWLabel PopulationLbl;
        // { +01A8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +01AC }
        public Aow.Units.AoWE.TItemHS FItemBag;
        // AoWInfoWin.pas:137
        // { $1B0; 0044252F }
        public void InfoWinResize()
        {
            this.InfoWin.Alignment.RightOffset = (this.InfoWin.FWinManager.WinWidth - this.InfoWin.WinLeft) - this.InfoWin.WinWidth;
            this.InfoWin.Alignment.TopOffset = this.InfoWin.WinTop;
        }

        // AoWInfoWin.pas:146
        public void MinClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            InfoWin.Visible = false;
        }

        // AoWInfoWin.pas:152
        public void CloseClick(Aow.Rtl.TObject Sender)
        {
            if (Sender != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            }

            InfoWin.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StructButton.Visible = false;
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
            // { Tried direct item-bag coordinate paths; no exact match. }
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FItemBag != null)
            {
                var itemHS = FItemBag;
                var call__3 = FItemBag.Yhx;
                var call__4 = itemHS.Xhx;
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__4, call__3, FItemBag.Lhx);

                var call__5 = FItemBag.Yhx;
                var call__6 = FItemBag.Xhx;
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)call__6), unchecked((sbyte)call__5), unchecked((sbyte)FItemBag.Lhx));
            }
            else
            {
                var call__7 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Yhx;
                var call__8 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Xhx;
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__8, call__7, Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Lhx);
                var call__9 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Yhx;
                var call__10 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Xhx;
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)call__10), unchecked((sbyte)call__9), unchecked((sbyte)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Lhx));
            }
        }

        // AoWInfoWin.pas:180
        public void RazeBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure != null)
            {
                var call__11 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.CanRaze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                if (call__11)
                {
                    Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.Raze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                }
                else
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                }
            }
        }

        // AoWInfoWin.pas:190
        public void MainBtnClick()
        {
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure != null)
            {
                if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.City.TCity)
                {
                    Aow.Units.AoWCityScreen.AoWCityScreen.CityScreen.ShowCity((Aow.Units.City.TCity)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure);
                }
                else
                {
                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.PrdPlace.TProductionPlace)
                    {
                        Aow.Units.AoWProdScreen.AoWProdScreen.ProductionScreen.ShowPlace((Aow.Units.PrdPlace.TProductionPlace)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure);
                    }
                    else
                    {
                        if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.Cave.TCave)
                        {
                            ((Aow.Units.Cave.TCave)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Enter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        }
                        else
                        {
                            if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.Teleport.TTeleport)
                            {
                                ((Aow.Units.Teleport.TTeleport)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Teleport(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                            }
                            else
                            {
                                if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.ExploreS.TExplorationSite)
                                {
                                    ((Aow.Units.ExploreS.TExplorationSite)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Search();
                                }
                                else
                                {
                                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.WizardsTower.TWizardsTower)
                                    {
                                        ((Aow.Units.WizardsTower.TWizardsTower)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Enter();
                                    }
                                    else
                                    {
                                        if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.Arena.TArena)
                                        {
                                            ((Aow.Units.Arena.TArena)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Enter();
                                        }
                                        else
                                        {
                                            if (Aow.Units.AoWWinMan.AoWWinMan.General.FStructure is Aow.Units.Altar.TAltar)
                                            {
                                                var call__12 = ((Aow.Units.Altar.TAltar)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).GetTargeting();
                                                if (!call__12)
                                                {
                                                    var call__13 = ((Aow.Units.Altar.TAltar)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).CanTarget(ref Reason);
                                                    if (call__13)
                                                    {
                                                        ((Aow.Units.Altar.TAltar)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).Target();
                                                    }
                                                    else
                                                    {
                                                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                                                    }
                                                }
                                                else
                                                {
                                                    ((Aow.Units.Altar.TAltar)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).CancelTargeting();
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
                    var call__14 = ((Aow.Units.AoWE.TAoWMapField)FItemBag.Owner).GetArmyHS();
                    Army = call__14;
                    if ((Army != null) && (Army.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
                    {
                        var call__15 = Army.FArmy.Hero(255, Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                        if (call__15 != null)
                        {
                            var call__16 = Army.FArmy.Hero(255, Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                            Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__16;
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                            Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 2;
                        }
                    }
                }
            }
        }

        // AoWInfoWin.pas:226
        public void ItembagLBDrawItem(Aow.Rtl.TObject Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, in Aow.Units.GFXE.TSurfaceDesc Surface)
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
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(504);
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
        public static Aow.Units.AoWInfoWin.TIWindow IWindow;
        // AoWInfoWin.pas:616
        public static void ShowStructureInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
        {
            if (Structure.GetBuilding())
            {
                Aow.Units.AoWInfoWin.AoWInfoWin.ShowBuildingInfo(Dialog, Structure);
            }
            else
            {
                if (Structure.GetRazed())
                {
                    Aow.Units.AoWInfoWin.AoWInfoWin.ShowRazedInfo(Dialog, Structure);
                }
                else
                {
                    if (Structure is Aow.Units.AoWE.TPlayerStructure)
                    {
                        if ((((Aow.Units.AoWE.TPlayerStructure)Structure).FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) || Aow.Units.AoWE.AoWE.AoWEngine.FDebug)
                        {
                            if (Structure is Aow.Units.City.TCity)
                            {
                                if (((Aow.Units.City.TCity)Structure).FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
                                {
                                    Aow.Units.AoWInfoWin.AoWInfoWin.ShowWarCityInfo(Dialog, (Aow.Units.City.TCity)Structure);
                                }
                                else
                                {
                                    Aow.Units.AoWInfoWin.AoWInfoWin.ShowCityInfo(Dialog, (Aow.Units.City.TCity)Structure);
                                }
                            }
                            else
                            {
                                if (Structure is Aow.Units.PrdPlace.TProductionPlace)
                                {
                                    if (((Aow.Units.PrdPlace.TProductionPlace)Structure).GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                                    {
                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowProductionInfo(Dialog, (Aow.Units.PrdPlace.TProductionPlace)Structure);
                                    }
                                    else
                                    {
                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowProductionIncome(Dialog, (Aow.Units.PrdPlace.TProductionPlace)Structure);
                                    }
                                }
                                else
                                {
                                    if (Structure is Aow.Units.Mine.TMine)
                                    {
                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowMineInfo(Dialog, (Aow.Units.Mine.TMine)Structure);
                                    }
                                    else
                                    {
                                        if (Structure is Aow.Units.PowerNode.TPowerNode)
                                        {
                                            Aow.Units.AoWInfoWin.AoWInfoWin.ShowPowerNodeInfo(Dialog, (Aow.Units.PowerNode.TPowerNode)Structure);
                                        }
                                        else
                                        {
                                            if (Structure is Aow.Units.Farm.TFarm)
                                            {
                                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowFarmInfo(Dialog, (Aow.Units.Farm.TFarm)Structure);
                                            }
                                            else
                                            {
                                                if (Structure is Aow.Units.Tower.TTower)
                                                {
                                                    Aow.Units.AoWInfoWin.AoWInfoWin.ShowWatchTowerInfo(Dialog, (Aow.Units.Tower.TTower)Structure);
                                                }
                                                else
                                                {
                                                    if (Structure is Aow.Units.Altar.TAltar)
                                                    {
                                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowAltarInfo(Dialog, (Aow.Units.Altar.TAltar)Structure);
                                                    }
                                                    else
                                                    {
                                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowGenericInfo(Dialog, Structure);
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
                            if ((Structure is Aow.Units.City.TCity) && ((Aow.Units.City.TCity)Structure).FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
                            {
                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowWarCityInfo(Dialog, (Aow.Units.City.TCity)Structure);
                            }
                            else
                            {
                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowOtherPlayerInfo(Dialog, (Aow.Units.AoWE.TPlayerStructure)Structure);
                            }
                        }
                    }
                    else
                    {
                        if (Structure is Aow.Units.Sign.TSign)
                        {
                            Aow.Units.AoWInfoWin.AoWInfoWin.ShowSignInfo(Dialog, (Aow.Units.Sign.TSign)Structure);
                        }
                        else
                        {
                            if (Structure is Aow.Units.Cave.TCave)
                            {
                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowCaveInfo(Dialog, (Aow.Units.Cave.TCave)Structure);
                            }
                            else
                            {
                                if (Structure is Aow.Units.Teleport.TTeleport)
                                {
                                    Aow.Units.AoWInfoWin.AoWInfoWin.ShowTeleportInfo(Dialog, (Aow.Units.Teleport.TTeleport)Structure);
                                }
                                else
                                {
                                    if (Structure is Aow.Units.ExploreS.TExplorationSite)
                                    {
                                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowExplorationInfo(Dialog, (Aow.Units.ExploreS.TExplorationSite)Structure);
                                    }
                                    else
                                    {
                                        if (Structure is Aow.Units.WizardsTower.TWizardsTower)
                                        {
                                            Aow.Units.AoWInfoWin.AoWInfoWin.ShowTowerInfo(Dialog, (Aow.Units.WizardsTower.TWizardsTower)Structure);
                                        }
                                        else
                                        {
                                            if (Structure is Aow.Units.Arena.TArena)
                                            {
                                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowArenaInfo(Dialog, (Aow.Units.Arena.TArena)Structure);
                                            }
                                            else
                                            {
                                                Aow.Units.AoWInfoWin.AoWInfoWin.ShowGenericInfo(Dialog, Structure);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StructButton.Visible)
            {
                Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible = true;
            }

            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.StructButton.Visible = true;
        }

        // AoWInfoWin.pas:648
        public static void UpdateStructureInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
        {
            if (Dialog.InfoWin.Visible)
            {
                if (Structure == null)
                {
                    Structure = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure;
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
                        Aow.Units.AoWInfoWin.AoWInfoWin.ShowStructureInfo(Dialog, Structure);
                    }
                }
            }
        }

        // AoWInfoWin.pas:661
        public static void ShowItemSelection(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TItemHS Items)
        {
            int I = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E3Globals.E3Globals.ItemBagRStr);
            Dialog.Title.Text = call__23;
            Dialog.FItemBag = Items;
            Dialog.ItemPnl.Visible = true;
            Dialog.InfoWin.Visible = true;
            Dialog.StructNameLbl.Text = (AnsiString)"";
            var call__24 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[35];
            Dialog.StructLine.ImageTop = call__24;
            var call__25 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[36];
            Dialog.StructLine.ImageMiddle = call__25;
            var call__26 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt[37];
            Dialog.StructLine.ImageBottom = call__26;
            var call__27 = ((Aow.Units.AoWE.TAoWMapField)Items.Owner).GetArmyHS();
            Army = call__27;
            if ((Army != null) && (Army.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
            {
                Dialog.ItemBagMemo.Visible = false;
                var call__28 = Army.FArmy.Hero(255, Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                Dialog.MainBtn.Visible = call__28 != null;
                var call__29 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PickupRStr);
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
        public static void UpdateItemSelection(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TItemHS Items)
        {
            if (Dialog.InfoWin.Visible && (Items == Dialog.FItemBag))
            {
                Aow.Units.AoWInfoWin.AoWInfoWin.ShowItemSelection(Dialog, Dialog.FItemBag);
            }
        }

        // AoWInfoWin.pas:242
        public static void ResetStructureInfo(Aow.Units.AoWInfoWin.TIWindow Dialog)
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
            var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StructureRStr);
            Dialog.Title.Text = call__32;
        }

        // AoWInfoWin.pas:263
        public static void SetStructureHeader(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
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
        public static void ShowMineInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Mine.TMine Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__39 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__39);
            Dialog.IncomeTypeImage.CurrentImage = 31;
            var call__40 = Structure.GetIncome();
            Dialog.IncomeAmountLbl.Text = Aow.Rtl.Rtl.IntToStr(call__40);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:291
        public static void ShowPowerNodeInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.PowerNode.TPowerNode Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__42 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__42);
            Dialog.IncomeTypeImage.CurrentImage = 33;
            var call__43 = Structure.GetPower();
            Dialog.IncomeAmountLbl.Text = Aow.Rtl.Rtl.IntToStr(call__43);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:304
        public static void ShowProductionIncome(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.PrdPlace.TProductionPlace Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.IncomePnl.Visible = true;
            var call__45 = Structure.GetDescription();
            Dialog.IncomeMemo.Items.Assign(call__45);
            Dialog.IncomeTypeImage.CurrentImage = 33;
            var call__46 = Structure.GetPower();
            Dialog.IncomeAmountLbl.Text = Aow.Rtl.Rtl.IntToStr(call__46);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:317
        public static void ShowFarmInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Farm.TFarm Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__48 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__48);
            Dialog.SBmemo.Items.Add((AnsiString)"");
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IncomeXRStr);
            var call__50 = Aow.Units.AoWE.AoWE.Format(call__49, [Aow.Rtl.Rtl.IntToStr(Structure.FIncome)]);
            Dialog.SBmemo.Items.Add(call__50);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:329
        public static void ShowSignInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Sign.TSign Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Aow.Units.AoWWinMan.AoWWinMan.General.FSign = Structure;
            Dialog.SignPnl.Visible = true;
            Dialog.SignText.Items.Assign(Structure.FText);
        }

        // AoWInfoWin.pas:337
        public static void ShowTeleportInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Teleport.TTeleport Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__52 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__52);
            Dialog.SBmemo.FSized = false;
            var call__53 = Structure.CanTeleport(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Dialog.MainBtn.Visible = call__53;
            var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TeleportRStr);
            Dialog.MainBtn.Text = call__54;
            Dialog.RazePnl.Visible = Dialog.MainBtn.Visible;
        }

        // AoWInfoWin.pas:351
        public static void ShowTowerInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.WizardsTower.TWizardsTower Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__56 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__56);
            var call__57 = Structure.CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Dialog.MainBtn.Visible = call__57;
            var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterRStr);
            Dialog.MainBtn.Text = call__58;
        }

        // AoWInfoWin.pas:362
        public static void ShowArenaInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Arena.TArena Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__60 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__60);
            var call__61 = Structure.CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Dialog.MainBtn.Visible = call__61;
            var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterRStr);
            Dialog.MainBtn.Text = call__62;
        }

        // AoWInfoWin.pas:373
        public static void ShowCaveInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Cave.TCave Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__64 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__64);
            var call__65 = Structure.CanEnter(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Dialog.MainBtn.Visible = call__65;
            switch (Structure.FEntrance)
            {
                case Aow.Units.Cave.TCaveEntrance.ceUp:
                    var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoUpRStr);
                    Dialog.MainBtn.Text = call__66;
                    break;
                case Aow.Units.Cave.TCaveEntrance.ceDown:
                    var call__67 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoDownRStr);
                    Dialog.MainBtn.Text = call__67;
                    break;
            }
        }

        // AoWInfoWin.pas:387
        public static void ShowWatchTowerInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Tower.TTower Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__68 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__68);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:395
        public static void ShowExplorationInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.ExploreS.TExplorationSite Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.ExplorationSitePnl.Visible = true;
            var call__70 = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure.GetDescription();
            Dialog.ESMemo.Items.Assign(call__70);
            var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StatusRStr);
            Dialog.ESLbl.Text = call__71 + AnsiString.FromBytes(58);
            if (Structure.GetExplored())
            {
                var call__72 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExploredRStr);
                Dialog.ESStatusLbl.Text = call__72;
            }
            else
            {
                var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnexploredRStr);
                Dialog.ESStatusLbl.Text = call__73;
            }

            var call__74 = ((Aow.Units.ExploreS.TExplorationSite)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure).CanSearch(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Dialog.MainBtn.Visible = call__74;
            var call__75 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SearchRStr);
            Dialog.MainBtn.Text = call__75;
        }

        // AoWInfoWin.pas:410
        public static void ShowAltarInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.Altar.TAltar Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.AltarPnl.Visible = true;
            if (Structure.GetTargeting())
            {
                var call__77 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CancelRStr);
                Dialog.MainBtn.Text = call__77;
            }
            else
            {
                var call__78 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TargetRStr);
                Dialog.MainBtn.Text = call__78;
            }

            Dialog.MainBtn.Visible = true;
            var call__79 = Structure.GetRechargeDays();
            Dialog.Bar.Max = call__79;
            Dialog.Bar.Position = Structure.FRechargeDays;
            var call__80 = Structure.GetRechargeDays();
            Dialog.ChargePcntLbl.Text = Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Round(((double)(Structure.FRechargeDays * 100)) / call__80)) + AnsiString.FromBytes(37);
            var call__81 = Structure.GetRechargeDays();
            Dialog.DayLbl.Text = Aow.Rtl.Rtl.IntToStr(call__81 - Structure.FRechargeDays);
            var call__82 = Structure.TargetMana();
            Dialog.GemLbl.Text = Aow.Rtl.Rtl.IntToStr(call__82);
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:427
        public static void ShowWarCityInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.City.TCity Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            Dialog.SBmemo.Items.Add((AnsiString)"");
            var call__84 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StatusWarmodeRStr);
            Dialog.SBmemo.Items.Add(call__84);
        }

        // AoWInfoWin.pas:439
        public static void ShowCityInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.City.TCity Structure)
        {
            Aow.Units.AoWE.TUnit UnitObject = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            var call__86 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterRStr);
            Dialog.MainBtn.Text = call__86;
            Dialog.MainBtn.Visible = true;
            Aow.Units.AoWWinMan.AoWWinMan.General.FCity = Structure;
            Dialog.CityPanel.Visible = true;
            Dialog.ProductionWarningLbl.Visible = false;
            Dialog.ActiveCityActionMemo.Items.Clear();
            var call__87 = Aow.Units.AoWWinMan.AoWWinMan.General.FCity.GetName();
            Dialog.StructNameLbl.Text = call__87;
            var call__88 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesRStr[Aow.Units.AoWWinMan.AoWWinMan.General.FCity.FRace]);
            var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceXRStr);
            var call__90 = Aow.Units.AoWE.AoWE.Format(call__89, [call__88]);
            Dialog.CityRaceLbl.Text = call__90;
            Dialog.CityLevelLbl.Text = Aow.Rtl.Rtl.IntToStr(Structure.FUpgradeLevel);
            Dialog.CityIncomeLbl.Text = Aow.Rtl.Rtl.IntToStr(Structure.FIncome);
            var call__91 = Structure.GetCivilStatus();
            var call__92 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.City.City.TCivilStatusRStr[(int)call__91]);
            var call__93 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StatusXRStr);
            var call__94 = Aow.Units.AoWE.AoWE.Format(call__93, [call__92]);
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
                        var call__99 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallingRStr);
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
                        var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingRStr);
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

                if (Structure.FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing)
                {
                    if (Structure.FProduction.FSettings.FTurnsLeft != -1)
                    {
                        var call__107 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(Structure.FProduction.FSettings.FTurnsLeft);
                        Dialog.ActiveCityActionMemo.Items.Add(call__107);
                    }
                    else
                    {
                        var call__108 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnlimitedRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(Aow.Rtl.Rtl.LowerCase(call__108));
                    }
                }
                else
                {
                    if (Structure.FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsWaiting)
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
                var call__110 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                Dialog.ActiveCityActionMemo.Items.Add(call__110);
            }

            var call__111 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Structure.FRace);
            var call__112 = call__111.GetPlayerRelation(unchecked((sbyte)Structure.FPlayer));
            Dialog.RaceRelationIcon.CurrentImage = ((int)call__112) + 11;
            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:496
        public static void ShowProductionInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.PrdPlace.TProductionPlace Structure)
        {
            Aow.Units.AoWE.TUnit UnitObject = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            var call__114 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterRStr);
            Dialog.MainBtn.Text = call__114;
            Dialog.MainBtn.Visible = true;
            Aow.Units.AoWWinMan.AoWWinMan.General.FProductionPlace = (Aow.Units.PrdPlace.TProductionPlace)Aow.Units.AoWWinMan.AoWWinMan.General.FStructure;
            Dialog.ProductionPlacePnl.Visible = true;
            Aow.Units.AoWWinMan.AoWWinMan.General.FProductionControl = Aow.Units.AoWWinMan.AoWWinMan.General.FProductionPlace.FProductionControl;
            Dialog.CurrentActivityWarningLbl.Visible = false;
            Dialog.ActiveActionMemo.Items.Clear();
            Dialog.IncomeLbl.Visible = true;
            Dialog.IncomeTypeIcon.Visible = true;
            Dialog.IncomeValueLbl.Visible = true;
            if (Structure.GetMagicSphere() != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                Dialog.IncomeTypeIcon.CurrentImage = 33;
                var call__115 = Structure.GetPower();
                Dialog.IncomeValueLbl.Text = Aow.Rtl.Rtl.IntToStr(call__115);
            }
            else
            {
                if (Structure.GetBaseIncome() != 0)
                {
                    Dialog.IncomeTypeIcon.CurrentImage = 31;
                    var call__116 = Structure.GetIncome();
                    Dialog.IncomeValueLbl.Text = Aow.Rtl.Rtl.IntToStr(call__116);
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
                        var call__122 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallingRStr);
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
                        var call__126 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingRStr);
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

                if (Structure.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing)
                {
                    if (Structure.FProductionControl.FSettings.FTurnsLeft != -1)
                    {
                        var call__130 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(Structure.FProductionControl.FSettings.FTurnsLeft);
                        Dialog.ActiveActionMemo.Items.Add(call__130);
                    }
                    else
                    {
                        // { Original writes the city memo in this branch. }
                        var call__131 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnlimitedRStr);
                        Dialog.ActiveCityActionMemo.Items.Add(Aow.Rtl.Rtl.LowerCase(call__131));
                    }
                }
                else
                {
                    if (Structure.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsWaiting)
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
                if (Structure.GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone)
                {
                    Dialog.ActiveActionMemo.Items.Add((AnsiString)"");
                    if (Structure.GetBaseIncome() != 0)
                    {
                        var call__133 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__133);
                    }
                    else
                    {
                        var call__134 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                        Dialog.ActiveActionMemo.Items.Add(call__134);
                    }
                }
                else
                {
                    var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GeneratingExtraPowerRStr);
                    Dialog.ActiveActionMemo.Items.Add(call__135);
                }
            }

            Dialog.RazePnl.Visible = true;
        }

        // AoWInfoWin.pas:558
        public static void ShowBuildingInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.ConstructionPnl.Visible = true;
            var call__136 = Aow.Units.AoWE.AoWE.FmtDaysLeftStr(Structure.FBuildingDays);
            Dialog.ConstructionDaysLeftLbl.Text = call__136;
            var call__137 = Structure.GetDescription();
            Dialog.ConstructionMemo.Items.Assign(call__137);
        }

        // AoWInfoWin.pas:566
        public static void ShowRazedInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Rtl.AnsiString Text = default;
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            if (Structure is Aow.Units.City.TCity)
            {
                var call__139 = Structure.GetName();
                var call__140 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityOfXRStr);
                var call__141 = Aow.Units.AoWE.AoWE.Format(call__140, [call__139]);
                Text = call__141;
            }
            else
            {
                Text = Structure.GetName();
            }

            var call__142 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TheXliesInRuinsRStr);
            var call__143 = Aow.Units.AoWE.AoWE.Format(call__142, [Text]);
            Dialog.SBmemo.Items.Text = call__143;
            Dialog.SBmemo.Items.Insert(0, (AnsiString)"");
            Dialog.SBmemo.Items.Insert(0, (AnsiString)"");
        }

        // AoWInfoWin.pas:581
        public static void ShowOtherPlayerInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TPlayerStructure Structure)
        {
            int RaceIndex = default;
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.NotYoursPnl.Visible = true;
            if ((Structure is Aow.Units.City.TCity) && (Structure.FPlayer == 0))
            {
                Dialog.PopulationRaceLbl.Visible = true;
                var call__145 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[((Aow.Units.City.TCity)Structure).FRace]);
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
                var call__147 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Structure.FPlayer);
                RaceIndex = call__147.FPlayerRace;
                if (RaceIndex == -1)
                {
                    RaceIndex = 12;
                }

                var call__148 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[RaceIndex]);
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
        public static void ShowGenericInfo(Aow.Units.AoWInfoWin.TIWindow Dialog, Aow.Units.AoWE.TStructure Structure)
        {
            Aow.Units.AoWInfoWin.AoWInfoWin.ResetStructureInfo(Dialog);
            Aow.Units.AoWInfoWin.AoWInfoWin.SetStructureHeader(Dialog, Structure);
            Dialog.SingleButPnl.Visible = true;
            var call__149 = Structure.GetDescription();
            Dialog.SBmemo.Items.Assign(call__149);
        }
    }
}
