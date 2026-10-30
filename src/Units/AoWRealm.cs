// Translated from AoWRealm.pas.
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
using Aow.Units.aowIntGfxMod;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWUnitWindow;
using static Aow.Units.AoWUnitWindow.AoWUnitWindow;
using Aow.Units.AoWCityScreen;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using Aow.Units.AoWProdScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.AoWUnitBanner;
using static Aow.Units.AoWUnitBanner.AoWUnitBanner;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
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
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.Farm;
using static Aow.Units.Farm.Farm;
using Aow.Units.Mine;
using static Aow.Units.Mine.Mine;
using Aow.Units.BldGuild;
using static Aow.Units.BldGuild.BldGuild;

namespace Aow.Units.AoWRealm
{
    // { TRealmWin (instance size 412, VMT 004484F0) }
    public partial class TRealmWin : Aow.Rtl.TDataModule
    {
        public TRealmWin(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel OWTitle;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage OWTitleIc;
        // { +004C }
        public Aow.Units.AoWTabPanel.TAOWTabPanel HeaderTab;
        // { +0050 }
        public Aow.Units.AoWPanel.TAOWPanel T1Pnl;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel T2Pnl;
        // { +0058 }
        public Aow.Units.AoWPanel.TAOWPanel T2LBPnl;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel T3Pnl;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel T3LBPnl;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton Minimize;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel T4Pnl;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel T4LBPnl;
        // { +0070 }
        public Aow.Units.AoWListBox.TAOWListBox T2NameLB;
        // { +0074 }
        public Aow.Units.AoWListBox.TAOWListBox T2RaceLB;
        // { +0078 }
        public Aow.Units.AoWListBox.TAOWListBox T2IncomeLB;
        // { +007C }
        public Aow.Units.AoWListBox.TAOWListBox T2LevelLB;
        // { +0080 }
        public Aow.Units.AoWListBox.TAOWListBox T2WallLB;
        // { +0084 }
        public Aow.Units.AoWListBox.TAOWListBox T2SizeLB;
        // { +0088 }
        public Aow.Units.AoWListBox.TAOWListBox T2EncLB;
        // { +008C }
        public Aow.Units.AoWButton.TAOWButton T2SName;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton T2SRace;
        // { +0094 }
        public Aow.Units.AoWButton.TAOWButton T2SIn;
        // { +0098 }
        public Aow.Units.AoWButton.TAOWButton T2SWall;
        // { +009C }
        public Aow.Units.AoWButton.TAOWButton T2SLvl;
        // { +00A0 }
        public Aow.Units.AoWButton.TAOWButton T2SSize;
        // { +00A4 }
        public Aow.Units.AoWButton.TAOWButton T2SEnc;
        // { +00A8 }
        public Aow.Units.AoWListBox.TAOWListBox T2ActLB;
        // { +00AC }
        public Aow.Units.AoWButton.TAOWButton T2SAct;
        // { +00B0 }
        public Aow.Units.AoWListBox.TAOWListBox T2RelLB;
        // { +00B4 }
        public Aow.Units.AoWButton.TAOWButton T2SRel;
        // { +00B8 }
        public Aow.Units.AoWListBox.TAOWListBox T2MorLB;
        // { +00BC }
        public Aow.Units.AoWButton.TAOWButton T2SMor;
        // { +00C0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar CitySB;
        // { +00C4 }
        public Aow.Units.AoWListBox.TAOWListBox T3Name;
        // { +00C8 }
        public Aow.Units.AoWButton.TAOWButton T3SName;
        // { +00CC }
        public Aow.Units.AoWListBox.TAOWListBox T3ActLB;
        // { +00D0 }
        public Aow.Units.AoWButton.TAOWButton T3Activity;
        // { +00D4 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlaceSB;
        // { +00D8 }
        public Aow.Units.AoWFrame.TAoWFrame UnitFr;
        // { +00DC }
        public Aow.Units.AOWGauge.TAoWGauge UnitGg;
        // { +00E0 }
        public Aow.Units.AoWListBox.TAOWListBox Unit1LB;
        // { +00E4 }
        public Aow.Units.AoWListBox.TAOWListBox Unit2LB;
        // { +00E8 }
        public Aow.Units.AoWListBox.TAOWListBox Unit3LB;
        // { +00EC }
        public Aow.Units.AoWListBox.TAOWListBox Unit4LB;
        // { +00F0 }
        public Aow.Units.AoWListBox.TAOWListBox Unit5LB;
        // { +00F4 }
        public Aow.Units.AoWListBox.TAOWListBox Unit6LB;
        // { +00F8 }
        public Aow.Units.AoWListBox.TAOWListBox Unit7LB;
        // { +00FC }
        public Aow.Units.AoWListBox.TAOWListBox Unit8LB;
        // { +0100 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PartySb;
        // { +0104 }
        public Aow.Units.AoWPanel.TAOWPanel HeroPnl;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel T5MainPnl;
        // { +010C }
        public Aow.Units.AoWListBox.TAOWListBox T5Name;
        // { +0110 }
        public Aow.Units.AoWListBox.TAOWListBox T5Health;
        // { +0114 }
        public Aow.Units.AoWListBox.TAOWListBox T5Level;
        // { +0118 }
        public Aow.Units.AoWButton.TAOWButton T5SName;
        // { +011C }
        public Aow.Units.AoWButton.TAOWButton T5SHealth;
        // { +0120 }
        public Aow.Units.AoWButton.TAOWButton T5SLevel;
        // { +0124 }
        public Aow.Units.AoWPanel.TAOWPanel T5HeroInf;
        // { +0128 }
        public Aow.Units.AoWPanel.TAOWPanel T5LBFr;
        // { +012C }
        public Aow.Units.AoWButton.TAOWButton HInfoBtn;
        // { +0130 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar HeroSB;
        // { +0134 }
        public Aow.Units.AoWPanel.TAOWPanel TotalPnl;
        // { +0138 }
        public Aow.Units.AoWPanel.TAOWPanel IncomePnl;
        // { +013C }
        public Aow.Units.AoWMemo.TAOWMemo IncomeMem;
        // { +0140 }
        public Aow.Units.AoWPanel.TAOWPanel IncomeBtm;
        // { +0144 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IncomeSB;
        // { +0148 }
        public Aow.Units.AoWLabel.TAOWLabel IncomeLbl;
        // { +014C }
        public Aow.Units.AoWLabel.TAOWLabel Income;
        // { +0150 }
        public Aow.Units.AoWPanel.TAOWPanel UpkeepPnl;
        // { +0154 }
        public Aow.Units.AoWMemo.TAOWMemo UpkeepMem;
        // { +0158 }
        public Aow.Units.AoWPanel.TAOWPanel UpkeepBtm;
        // { +015C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar UpkeepSB;
        // { +0160 }
        public Aow.Units.AoWLabel.TAOWLabel UpkeepLbl;
        // { +0164 }
        public Aow.Units.AoWLabel.TAOWLabel Upkeep;
        // { +0168 }
        public Aow.Units.AoWLabel.TAOWLabel Totallbl;
        // { +016C }
        public Aow.Units.AoWLabel.TAOWLabel NetInc;
        // { +0170 }
        public Aow.Units.AoWPanel.TAOWPanel TotalBtm;
        // { +0174 }
        public Aow.Units.AoWLabel.TAOWLabel TreasuryLbl;
        // { +0178 }
        public Aow.Units.AoWLabel.TAOWLabel Treasury;
        // { +017C }
        public Aow.Units.AoWPanel.TAOWPanel TreasuryPnl;
        // { +0180 }
        public Aow.Units.AoWMemo.TAOWMemo IncomeValue;
        // { +0184 }
        public Aow.Units.AoWMemo.TAOWMemo UpkeepValue;
        // { +0188 }
        public Aow.Units.AoWImage.TAOWImage GoldAnim;
        // { +018C }
        public Aow.Units.AoWPanel.TAOWPanel PicPnl;
        // { +0190 }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +0194 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWRealm.pas:238
        // { +0198 }
        public void UpdateIncome()
        {
            int Value = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Treasury.Text = Aow.Rtl.Rtl.IntToStr(call__1.FGems);
            var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__3 = call__2.GetUpkeep();
            var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            NetInc.Text = Aow.Rtl.Rtl.IntToStr(call__4.FIncome - call__3);
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Income.Text = Aow.Rtl.Rtl.IntToStr(call__5.FIncome);
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__7 = call__6.GetUpkeep();
            Upkeep.Text = Aow.Rtl.Rtl.IntToStr(call__7);
            IncomeMem.Items.Clear();
            IncomeValue.Items.Clear();
            UpkeepValue.Items.Clear();
            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Value = call__8.FBaseIncome;
            if (Value != 0)
            {
                var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExternalIncomeRStr);
                IncomeMem.Items.Add(call__9);
                IncomeValue.Items.Add(Aow.Rtl.Rtl.IntToStr(Value));
            }

            var call__10 = Aow.Units.City.City.CityIncome(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Value = call__10;
            if (Value != 0)
            {
                var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CitiesRStr);
                IncomeMem.Items.Add(call__11);
                IncomeValue.Items.Add(Aow.Rtl.Rtl.IntToStr(Value));
            }

            var call__12 = Aow.Units.Farm.Farm.FarmIncome(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Value = call__12;
            if (Value != 0)
            {
                var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FarmsRStr);
                IncomeMem.Items.Add(call__13);
                IncomeValue.Items.Add(Aow.Rtl.Rtl.IntToStr(Value));
            }

            var call__14 = Aow.Units.Mine.Mine.MineIncome(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Value = call__14;
            if (Value != 0)
            {
                var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MinesRStr);
                IncomeMem.Items.Add(call__15);
                IncomeValue.Items.Add(Aow.Rtl.Rtl.IntToStr(Value));
            }

            var call__16 = Aow.Units.BldGuild.BldGuild.BuildersGuildIncome(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Value = call__16;
            if (Value != 0)
            {
                var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BuildersGuildsRStr);
                IncomeMem.Items.Add(call__17);
                IncomeValue.Items.Add(Aow.Rtl.Rtl.IntToStr(Value));
            }

            var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            UpkeepValue.Items.Add(Aow.Rtl.Rtl.IntToStr(call__18.FArmyUpkeep));
        }

        // AoWRealm.pas:178
        // { 00448EB4 }
        public void UpdateCities()
        {
            int I = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Aow.Units.City.TCity C = default;
            T2NameLB.Items.Clear();
            T2RaceLB.Items.Clear();
            T2IncomeLB.Items.Clear();
            T2LevelLB.Items.Clear();
            T2WallLB.Items.Clear();
            T2SizeLB.Items.Clear();
            T2MorLB.Items.Clear();
            T2RelLB.Items.Clear();
            T2ActLB.Items.Clear();
            T2EncLB.Items.Clear();
            {
                I = 0;
                int end__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
                for (; I <= end__19; I += 1)
                {
                    var call__20 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__21 = call__20.FStructures.GetStructure(I);
                    Structure = call__21;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        C = (Aow.Units.City.TCity)Structure;
                        var call__22 = C.GetName();
                        T2NameLB.Items.AddObject(call__22, C);
                        var call__23 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[C.FRace]);
                        T2RaceLB.Items.Add(call__23);
                        T2IncomeLB.Items.Add(Aow.Rtl.Rtl.IntToStr(C.FIncome));
                        T2LevelLB.Items.Add(Aow.Rtl.Rtl.IntToStr(C.FUpgradeLevel));
                        switch (C.FWallType)
                        {
                            case 1:
                                T2WallLB.Items.Add(AnsiString.FromBytes(49));
                                break;
                            case 2:
                                T2WallLB.Items.Add(AnsiString.FromBytes(50));
                                break;
                            default:
                                T2WallLB.Items.Add(AnsiString.FromBytes(45));
                                break;
                        }

                        switch (C.GetCivilStatus())
                        {
                            case Aow.Units.City.TCivilStatus.csUnrest:
                                T2MorLB.Items.Add(AnsiString.FromBytes(49));
                                break;
                            case Aow.Units.City.TCivilStatus.csStable:
                                T2MorLB.Items.Add(AnsiString.FromBytes(50));
                                break;
                            case Aow.Units.City.TCivilStatus.csContent:
                                T2MorLB.Items.Add(AnsiString.FromBytes(51));
                                break;
                            case Aow.Units.City.TCivilStatus.csCheerful:
                                T2MorLB.Items.Add(AnsiString.FromBytes(52));
                                break;
                            default:
                                T2MorLB.Items.Add(AnsiString.FromBytes(45));
                                break;
                        }

                        var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(C.FRace);
                        var call__25 = call__24.GetPlayerRelation(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        switch (unchecked((int)call__25) & 127)
                        {
                            case 0:
                                T2RelLB.Items.Add(AnsiString.FromBytes(48));
                                break;
                            case 1:
                                T2RelLB.Items.Add(AnsiString.FromBytes(49));
                                break;
                            case 2:
                                T2RelLB.Items.Add(AnsiString.FromBytes(50));
                                break;
                            case 3:
                                T2RelLB.Items.Add(AnsiString.FromBytes(51));
                                break;
                            case 4:
                                T2RelLB.Items.Add(AnsiString.FromBytes(52));
                                break;
                            default:
                                T2RelLB.Items.Add(AnsiString.FromBytes(45));
                                break;
                        }

                        switch (unchecked((int)C.GetActiveAction()) & 127)
                        {
                            case 3:
                                if (C.FWallType == 1)
                                {
                                    T2ActLB.Items.Add(AnsiString.FromBytes(49));
                                }
                                else
                                {
                                    T2ActLB.Items.Add(AnsiString.FromBytes(48));
                                }

                                break;
                            case 2:
                                T2ActLB.Items.Add(AnsiString.FromBytes(50));
                                break;
                            case 5:
                                T2ActLB.Items.Add(AnsiString.FromBytes(51));
                                break;
                            case 4:
                                T2ActLB.Items.Add(AnsiString.FromBytes(52));
                                break;
                            case 7:
                            case 8:
                                T2ActLB.Items.Add(AnsiString.FromBytes(53));
                                break;
                            default:
                                T2ActLB.Items.Add(AnsiString.FromBytes(45));
                                break;
                        }

                        T2EncLB.Items.Add(AnsiString.FromBytes(45));
                        var call__26 = C.GetSize();
                        T2SizeLB.Items.Add(Aow.Rtl.Rtl.IntToStr(call__26));
                    }
                }
            }
        }

        // AoWRealm.pas:309
        // { 00449230 }
        public void UpdatePlaces()
        {
            int I = default;
            Aow.Units.AoWE.TPlayerStructure Place = default;
            T3Name.Items.Clear();
            T3ActLB.Items.Clear();
            {
                I = 0;
                int end__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
                for (; I <= end__27; I += 1)
                {
                    var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    var call__29 = call__28.FStructures.GetStructure(I);
                    Place = call__29;
                    if (!(Place is Aow.Units.City.TCity))
                    {
                        var call__30 = Place.EditName();
                        T3Name.Items.AddObject(call__30, Place);
                        var call__31 = Place.GetCurrentActivityText();
                        T3ActLB.Items.Add(call__31);
                    }
                }
            }
        }

        // AoWRealm.pas:283
        // { 0044975C }
        public void UpdateHeroes()
        {
            int I = default;
            int J = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            T5Name.Items.Clear();
            T5Health.Items.Clear();
            T5Level.Items.Clear();
            {
                I = 0;
                int end__32 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__32; I += 1)
                {
                    {
                        J = 0;
                        int end__33 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__33; J += 1)
                        {
                            var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            var call__35 = call__34.FArmies.GetArmy(I);
                            var call__36 = call__35.GetUnit(J);
                            U = call__36;
                            if (U is Aow.Units.AoWE.THero)
                            {
                                var call__37 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                                var call__38 = call__37.FArmies.GetArmy(I);
                                var call__39 = U.GetName();
                                T5Name.Items.AddObject(call__39, call__38);
                                var call__40 = U.GetHitPoints();
                                var call__41 = U.GetHits();
                                T5Health.Items.AddObject((Aow.Rtl.Rtl.IntToStr(call__40) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__41), U);
                                var call__42 = ((Aow.Units.AoWE.THero)U).GetLevel();
                                T5Level.Items.Add(Aow.Rtl.Rtl.IntToStr(call__42));
                            }
                        }
                    }
                }
            }

            var call__43 = T5Name.Items.Count;
            if ((call__43 > 0) && (T5Name.Selected == -1))
            {
                T5Name.Selected = 0;
                HeroListChange(T5Name);
            }
        }

        // AoWRealm.pas:327
        // { 00449894 }
        public void UpdateOther()
        {
        }

        // AoWRealm.pas:331
        // { 00449B2C }
        public void UpdateOverview()
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == -1)
            {
                Dlg.Visible = false;
            }
            else
            {
                switch (HeaderTab.Selected)
                {
                    case 0:
                        UpdateIncome();
                        break;
                    case 1:
                        UpdateCities();
                        break;
                    case 2:
                        UpdatePlaces();
                        break;
                    case 3:
                        UpdateHeroes();
                        break;
                    case 4:
                        UpdateHeroes();
                        break;
                    case 5:
                        UpdateOther();
                        break;
                }
            }
        }

        // AoWRealm.pas:345
        // { 00449B30 }
        public void DrawArmyUnit(int Column, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TAbstractUnit U = default;
            Aow.Rtl.TRect OldClip = default;
            var call__44 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__45 = call__44.FArmies.GetArmy(DrawItem);
            var call__46 = call__45.GetCount();
            if (Column < call__46)
            {
                OldClip = SurfDesc.ClipRect;
                SurfDesc.ClipRect.Left = DrawRect.Left + 2;
                SurfDesc.ClipRect.Top = DrawRect.Top + 1;
                SurfDesc.ClipRect.Right = DrawRect.Right - 2;
                SurfDesc.ClipRect.Bottom = (DrawRect.Top + Unit1LB.ItemHeight) - 6;
                if ((DrawRect.Bottom - DrawRect.Top) < (Unit1LB.ItemHeight - 6))
                {
                    SurfDesc.ClipRect.Bottom = DrawRect.Bottom;
                }

                var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__48 = call__47.FArmies.GetArmy(DrawItem);
                var call__49 = call__48.GetUnit(Column);
                U = call__49;
                var call__50 = U.GetAlignment();
                if (unchecked((int)call__50) < Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs.Count)
                {
                    var call__51 = U.GetAlignment();
                    var call__52 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__51];
                    var call__53 = U.GetAlignment();
                    var call__54 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__53];
                    var call__55 = U.GetAlignment();
                    var call__56 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitBGs[(int)call__55];
                    call__56.ShowClipped(DrawRect.Left - ((call__54.Width - (DrawRect.Right - DrawRect.Left)) / 2), DrawRect.Top - ((call__52.Height - ((DrawRect.Bottom - DrawRect.Top) - 10)) / 2), SurfDesc);
                }

                var call__57 = U.GetPreviewImage();
                var call__58 = U.GetPreviewImage();
                U.ShowEx(DrawRect.Left - ((call__58.Width - (DrawRect.Right - DrawRect.Left)) / 2), DrawRect.Top - ((call__57.Height - (Unit1LB.ItemHeight - 10)) / 2), SurfDesc, Aow.Units.AoWWinMan.AoWWinMan.General.FUnitShowInfo);
                var gauge2 = UnitGg;
                gauge2.Visible = true;
                var call__60 = U.GetHits();
                gauge2.Max = call__60;
                var call__61 = U.GetHitPoints();
                gauge2.Position = call__61;
                gauge2.WinLeft = (DrawRect.Left - T4LBPnl.WinLeft) + 5;
                gauge2.WinWidth = (DrawRect.Right - DrawRect.Left) - 10;
                gauge2.WinTop = ((DrawRect.Top + Unit1LB.ItemHeight) - T4LBPnl.WinTop) - 9;
                // { Original sizes and draws the gauge directly. }
                gauge2.SetSize();
                gauge2.Draw(SurfDesc);
                gauge2.Visible = false;

                SurfDesc.ClipRect = OldClip;
            }
        }

        // AoWRealm.pas:382
        // { 0044A030 }
        public void OWMinIconClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWRealm.pas:388
        public void T2WallLBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            var call__62 = T2WallLB.Items[DrawItem];
            ReadOnlySpan<byte> text = Aow.Rtl.Rtl.StringBytes(call__62);
            switch ((text.IsEmpty ? (byte)0 : text[0]))
            {
                case 49:
                    var call__63 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[7];
                    var call__64 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[7];
                    call__64.ShowClipped((DrawRect.Left + ((DrawRect.Right - DrawRect.Left) / 2)) - (call__63.Width / 2), DrawRect.Top, SurfDesc);
                    break;
                case 50:
                    var call__65 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[8];
                    var call__66 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[8];
                    call__66.ShowClipped((DrawRect.Left + ((DrawRect.Right - DrawRect.Left) / 2)) - (call__65.Width / 2), DrawRect.Top, SurfDesc);
                    break;
            }
        }

        // AoWRealm.pas:398
        public void Unit1LBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            DrawArmyUnit(Sender.Tag, DrawRect, DrawItem, ref SurfDesc);
        }

        // AoWRealm.pas:403
        public void HeaderTabChange()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            UpdateOverview();
        }

        // AoWRealm.pas:409
        public void PartyVSBChange()
        {
            Unit1LB.ListOffset = PartySb.Position;
            Unit2LB.ListOffset = PartySb.Position;
            Unit3LB.ListOffset = PartySb.Position;
            Unit4LB.ListOffset = PartySb.Position;
            Unit5LB.ListOffset = PartySb.Position;
            Unit6LB.ListOffset = PartySb.Position;
            Unit7LB.ListOffset = PartySb.Position;
            Unit8LB.ListOffset = PartySb.Position;
        }

        // AoWRealm.pas:421
        public void Unit1LBChange(Aow.Rtl.TObject Sender)
        {
            // { Tried typed Sender without redundant with cast; typed Sender retaining cast plus explicit window with; no exact match. }
            var listBox = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            var call__68 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__69 = call__68.FArmies.GetArmy(listBox.Selected);
            var call__70 = call__69.GetCount();
            if (call__70 > 0)
            {
                listBox.PopupMenu = Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup;
                var call__71 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__72 = call__71.FArmies.GetArmy(listBox.Selected);
                var call__73 = call__72.GetUnit(listBox.Tag);
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__73;
                var call__74 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__75 = call__74.FArmies.GetArmy(listBox.Selected);
                var call__76 = call__75.GetArmyHS();
                Aow.Units.AoWE.AoWE.AoWHSMap.Select(call__76, true);
                Dlg.Priority = Dlg.Priority;
            }
            else
            {
                listBox.PopupMenu = null;
                listBox.Selected = -1;
            }

            if (Sender != Unit1LB)
            {
                Unit1LB.Selected = -1;
            }

            if (Sender != Unit2LB)
            {
                Unit2LB.Selected = -1;
            }

            if (Sender != Unit3LB)
            {
                Unit3LB.Selected = -1;
            }

            if (Sender != Unit4LB)
            {
                Unit4LB.Selected = -1;
            }

            if (Sender != Unit5LB)
            {
                Unit5LB.Selected = -1;
            }

            if (Sender != Unit6LB)
            {
                Unit6LB.Selected = -1;
            }

            if (Sender != Unit7LB)
            {
                Unit7LB.Selected = -1;
            }

            if (Sender != Unit8LB)
            {
                Unit8LB.Selected = -1;
            }
        }

        // AoWRealm.pas:448
        public void T2MorLBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int Icon = default;
            var call__77 = T2MorLB.Items[DrawItem];
            ReadOnlySpan<byte> text = Aow.Rtl.Rtl.StringBytes(call__77);
            Icon = ((int)(text.IsEmpty ? (byte)0 : text[0])) - ((int)48);
            var call__78 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[Icon + 9];
            var call__79 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[Icon + 9];
            call__79.ShowClipped((DrawRect.Left + ((DrawRect.Right - DrawRect.Left) / 2)) - (call__78.Width / 2), DrawRect.Top, SurfDesc);
        }

        // AoWRealm.pas:457
        public void CityListMouseDown(Aow.Units.AoWListBox.TAOWListBox Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.HSEngine.THexagonSprite Sprite = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (Sender.Selected != -1))
            {
                var call__80 = T2NameLB.Items.Objects[Sender.Selected];
                Sprite = (Aow.Units.HSEngine.THexagonSprite)call__80;
                var call__81 = Sprite.GetLhx();
                var call__82 = Sprite.GetYhx();
                var call__83 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__83, call__82, call__81);
                var call__84 = Sprite.GetLhx();
                var call__85 = Sprite.GetYhx();
                var call__86 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)call__86), unchecked((sbyte)call__85), unchecked((sbyte)call__84));
            }
        }

        // AoWRealm.pas:469
        public void CityListChange(Aow.Rtl.TObject Sender)
        {
            var listBox = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            T2NameLB.Selected = listBox.Selected;
            T2RaceLB.Selected = listBox.Selected;
            T2IncomeLB.Selected = listBox.Selected;
            T2LevelLB.Selected = listBox.Selected;
            T2WallLB.Selected = listBox.Selected;
            T2SizeLB.Selected = listBox.Selected;
            T2RelLB.Selected = listBox.Selected;
            T2MorLB.Selected = listBox.Selected;
            T2ActLB.Selected = listBox.Selected;
            T2EncLB.Selected = listBox.Selected;
        }

        // AoWRealm.pas:486
        public void CityDoubleClick(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected != -1)
            {
                var call__88 = T2NameLB.Items.Objects[((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected];
                Aow.Units.AoWCityScreen.AoWCityScreen.CityScreen.ShowCity((Aow.Units.City.TCity)call__88);
            }
        }

        // AoWRealm.pas:492
        public void PlaceListDoubleClick(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected != -1)
            {
                var call__89 = T3Name.Items.Objects[((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected];
                if (call__89 is Aow.Units.PrdPlace.TProductionPlace)
                {
                    var call__90 = T3Name.Items.Objects[((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected];
                    Aow.Units.AoWProdScreen.AoWProdScreen.ProductionScreen.ShowPlace((Aow.Units.PrdPlace.TProductionPlace)call__90);
                }
                else
                {
                    var call__91 = T3Name.Items.Objects[((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected];
                    Aow.Units.AoWE.AoWE.AoWHSMap.Select((Aow.Units.HSEngine.THexagonSprite)call__91, true);
                }
            }
        }

        // AoWRealm.pas:501
        public void PlacesListMouseDown(Aow.Units.AoWListBox.TAOWListBox Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.HSEngine.THexagonSprite Sprite = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (Sender.Selected != -1))
            {
                var call__92 = T3Name.Items.Objects[Sender.Selected];
                Sprite = (Aow.Units.HSEngine.THexagonSprite)call__92;
                var call__93 = Sprite.GetLhx();
                var call__94 = Sprite.GetYhx();
                var call__95 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__95, call__94, call__93);
                var call__96 = Sprite.GetLhx();
                var call__97 = Sprite.GetYhx();
                var call__98 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)call__98), unchecked((sbyte)call__97), unchecked((sbyte)call__96));
            }
        }

        // AoWRealm.pas:513
        public void HeroListDoubleClick(Aow.Rtl.TObject Sender)
        {
            // { Tried separate selection cast; no exact match. }
            var listBox = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            if (listBox.Selected != -1)
            {
                var call__100 = T5Name.Items.Objects[listBox.Selected];
                var call__101 = ((Aow.Units.AoWE.TArmy)call__100).GetArmyHS();
                Aow.Units.AoWE.AoWE.AoWHSMap.Select(call__101, true);
            }
        }

        // AoWRealm.pas:522
        public void HeroListMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWListBox.TAOWListBox List = default;
            Aow.Units.HSEngine.THexagonSprite Sprite = default;
            List = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            if (List.Selected != -1)
            {
                var call__102 = T5Name.Items.Objects[List.Selected];
                var call__103 = ((Aow.Units.AoWE.TArmy)call__102).GetArmyHS();
                Sprite = call__103;
                var call__104 = Sprite.GetLhx();
                var call__105 = Sprite.GetYhx();
                var call__106 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(call__106, call__105, call__104);
                var call__107 = Sprite.GetLhx();
                var call__108 = Sprite.GetYhx();
                var call__109 = Sprite.GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)call__109), unchecked((sbyte)call__108), unchecked((sbyte)call__107));
            }
        }

        // AoWRealm.pas:536
        public void HeroInfoClick()
        {
            int Index = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Index = T5Name.Selected;
            if (Index != -1)
            {
                // { evaluate the index before the receiver chain }
                var call__110 = T5Health.Items.Objects[Index << 0];
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = (Aow.Units.AoWE.THero)call__110;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
            }
        }

        // AoWRealm.pas:549
        public void T2LevelLBDoubleClick()
        {
            Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:555
        public void T2WallLBDoubleClick()
        {
            Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:561
        public void T2RaceLBDoubleClick()
        {
            Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:567
        public void T2ActLBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            byte Icon = default;
            var call__111 = T2ActLB.Items[DrawItem];
            ReadOnlySpan<byte> text = Aow.Rtl.Rtl.StringBytes(call__111);
            Icon = unchecked((byte)(text.IsEmpty ? (byte)0 : text[0]));
            if (Icon != 45)
            {
                var call__112 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[((int)Icon) - 23];
                var call__113 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[((int)Icon) - 23];
                call__113.ShowClipped((DrawRect.Left + ((DrawRect.Right - DrawRect.Left) / 2)) - (call__112.Width / 2), DrawRect.Top, SurfDesc);
            }
        }

        // AoWRealm.pas:577
        public void T2ActLBDoubleClick()
        {
            Aow.Units.AoWRealm.AoWRealm.RealmWin.UpdateOverview();
            Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:583
        public void T2RelLBDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int Icon = default;
            var call__114 = T2RelLB.Items[DrawItem];
            ReadOnlySpan<byte> text = Aow.Rtl.Rtl.StringBytes(call__114);
            Icon = ((int)(text.IsEmpty ? (byte)0 : text[0])) - ((int)48);
            var call__115 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[Icon + 14];
            var call__116 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons[Icon + 14];
            call__116.ShowClipped((DrawRect.Left + ((DrawRect.Right - DrawRect.Left) / 2)) - (call__115.Width / 2), DrawRect.Top, SurfDesc);
        }

        // AoWRealm.pas:592
        public void OverWindowResize()
        {
            this.Dlg.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            this.Dlg.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            this.Dlg.Alignment.LeftOffset = this.Dlg.WinLeft;
            this.Dlg.Alignment.TopOffset = this.Dlg.WinTop;
        }

        // AoWRealm.pas:603
        public void T2SBChange()
        {
            T2NameLB.ListOffset = CitySB.Position;
            T2RaceLB.ListOffset = CitySB.Position;
            T2IncomeLB.ListOffset = CitySB.Position;
            T2LevelLB.ListOffset = CitySB.Position;
            T2WallLB.ListOffset = CitySB.Position;
            T2SizeLB.ListOffset = CitySB.Position;
            T2RelLB.ListOffset = CitySB.Position;
            T2MorLB.ListOffset = CitySB.Position;
            T2ActLB.ListOffset = CitySB.Position;
        }

        // AoWRealm.pas:616
        public void T3NameChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWListBox.TAOWListBox List = default;
            List = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            T3Name.Selected = List.Selected;
            T3ActLB.Selected = List.Selected;
        }

        // AoWRealm.pas:625
        public void T3SBChange()
        {
            T3Name.ListOffset = PlaceSB.Position;
        }

        // AoWRealm.pas:630
        public void T2SNameClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:649
        public void T2SRaceClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2RaceLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:668
        public void T2SInClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2IncomeLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 2);
        }

        // AoWRealm.pas:687
        public void T2SLvlClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2LevelLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:706
        public void T2SWallClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2WallLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:725
        public void T2SSizeClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2SizeLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:744
        public void T2SRelClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2RelLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:763
        public void T2SMorClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2MorLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2ActLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:782
        public void T2SActClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2ActLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2EncLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:801
        public void T2SEncClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T2EncLB);
            Lists.Add(T2NameLB);
            Lists.Add(T2RaceLB);
            Lists.Add(T2IncomeLB);
            Lists.Add(T2LevelLB);
            Lists.Add(T2WallLB);
            Lists.Add(T2SizeLB);
            Lists.Add(T2RelLB);
            Lists.Add(T2MorLB);
            Lists.Add(T2ActLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:820
        public void T3SNameClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T3Name);
            Lists.Add(T3ActLB);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:831
        public void HeroListChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWListBox.TAOWListBox List = default;
            List = (Aow.Units.AoWListBox.TAOWListBox)Sender;
            T5Name.Selected = List.Selected;
            T5Health.Selected = List.Selected;
            T5Level.Selected = List.Selected;
        }

        // AoWRealm.pas:841
        public void HeroVSBChange()
        {
            // { Original bug: the hero scrollbar moves T3Name instead of T5Name. }
            T3Name.ListOffset = HeroSB.Position;
            T5Health.ListOffset = HeroSB.Position;
            T5Level.ListOffset = HeroSB.Position;
        }

        // AoWRealm.pas:849
        public void T5HeroInfDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int Index = default;
            Aow.Units.AoWE.THero Hero = default;
            Aow.Rtl.TRect R = default;
            Index = T5Name.Selected;
            if (Index > -1)
            {
                var call__118 = T5Health.Items.Objects[Index << 0];
                Hero = (Aow.Units.AoWE.THero)call__118;
                var call__119 = Aow.Rtl.Rtl.Rect(T5HeroInf.WinLeft + 1, T5HeroInf.WinTop + 1, (T5HeroInf.WinLeft + T5HeroInf.WinWidth) - 1, (T5HeroInf.WinTop + T5HeroInf.WinHeight) - 1);
                R = call__119;
                if (Hero.GetFace() != null)
                {
                    Hero.ShowFace(R, SurfDesc);
                }
            }
        }

        // AoWRealm.pas:866
        public void Unit1LBDoubleClick(Aow.Rtl.TObject Sender)
        {
            var call__120 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__121 = call__120.FArmies.GetArmy(((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected);
            var call__122 = call__121.GetCount();
            if (call__122 > 0)
            {
                var call__123 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__124 = call__123.FArmies.GetArmy(((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected);
                var call__125 = call__124.GetUnit(((Aow.Units.AoWListBox.TAOWListBox)Sender).Tag);
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = call__125;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
            }
        }

        // AoWRealm.pas:875
        public void T5SNameClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T5Name);
            Lists.Add(T5Health);
            Lists.Add(T5Level);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:887
        public void T5SHealthClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T5Health);
            Lists.Add(T5Name);
            Lists.Add(T5Level);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:899
        public void T5SLevelClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T5Level);
            Lists.Add(T5Name);
            Lists.Add(T5Health);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 2);
        }

        // AoWRealm.pas:911
        public void GotoButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWRealm.pas:916
        public void T4PnlShow(Aow.Rtl.TObject Sender)
        {
            int I = default;
            int J = default;
            {
                I = 0;
                int end__126 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__126; I += 1)
                {
                    {
                        J = 0;
                        int end__127 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__127; J += 1)
                        {
                            var call__128 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            var call__129 = call__128.FArmies.GetArmy(I);
                            var call__130 = call__129.GetUnit(J);
                            var call__131 = call__130.GetUnitGFXResourceIndex();
                            var call__132 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__131);
                            call__132.AddRef();
                        }
                    }
                }
            }
        }

        // AoWRealm.pas:926
        public void T4PnlHide(Aow.Rtl.TObject Sender)
        {
            int I = default;
            int J = default;
            {
                I = 0;
                int end__133 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__133; I += 1)
                {
                    {
                        J = 0;
                        int end__134 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__134; J += 1)
                        {
                            var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            var call__136 = call__135.FArmies.GetArmy(I);
                            var call__137 = call__136.GetUnit(J);
                            var call__138 = call__137.GetUnitGFXResourceIndex();
                            var call__139 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__138);
                            call__139.Release();
                        }
                    }
                }
            }
        }

        // AoWRealm.pas:936
        public void OverWindowShow()
        {
            if (T4Pnl.Visible)
            {
                T4PnlShow(this);
            }
        }

        // AoWRealm.pas:941
        public void OverWindowHide()
        {
            if (T4Pnl.Visible)
            {
                T4PnlHide(this);
            }
        }

        // AoWRealm.pas:946
        public void IncomeSBChange()
        {
            IncomeValue.ListOffset = IncomeSB.Position;
        }

        // AoWRealm.pas:951
        public void T3ActivityClick(Aow.Rtl.TObject Sender, int Button)
        {
            Aow.Rtl.TList Lists = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Lists = new Aow.Rtl.TList();
            Lists.Add(T3ActLB);
            Lists.Add(T3Name);
            Aow.Units.AoWWinMan.AoWWinMan.General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:962
        public void UpkeepSBChange()
        {
            UpkeepValue.ListOffset = UpkeepSB.Position;
        }

        // AoWRealm.pas:967
        public void DlgShow()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.RegisterDialog(Dlg);
        }

        // AoWRealm.pas:972
        public void DlgHide()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.UnregisterDialog(Dlg);
        }

        // AoWRealm.pas:977
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(505);
        }
    }

    public static partial class AoWRealm
    {
        public static Aow.Units.AoWRealm.TRealmWin RealmWin;
    }
}
