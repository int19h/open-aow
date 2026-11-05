// Translated from AoWRealm.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.aowIntGfxMod.aowIntGfxMod;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWUnitWindow.AoWUnitWindow;
using static Aow.Units.AoWCityScreen.AoWCityScreen;
using static Aow.Units.AoWProdScreen.AoWProdScreen;
using Aow.Units.PrdPlace;
using Aow.Units.City;
using static Aow.Units.City.City;
using static Aow.Units.aowHelpWin.aowHelpWin;
using static Aow.Units.AoWUnitBanner.AoWUnitBanner;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.Farm.Farm;
using static Aow.Units.Mine.Mine;
using static Aow.Units.BldGuild.BldGuild;

namespace Aow.Units.AoWRealm
{
    public partial class TRealmWin : TDataModule
    {
        public TRealmWin(TComponent owner) : base(owner)
        { }

        public TAOWWindow Dlg;
        public TAOWLabel OWTitle;
        public TAOWImage OWTitleIc;
        public TAOWTabPanel HeaderTab;
        public TAOWPanel T1Pnl;
        public TAOWPanel T2Pnl;
        public TAOWPanel T2LBPnl;
        public TAOWPanel T3Pnl;
        public TAOWPanel T3LBPnl;
        public TAOWButton Minimize;
        public TAOWPanel T4Pnl;
        public TAOWPanel T4LBPnl;
        public TAOWListBox T2NameLB;
        public TAOWListBox T2RaceLB;
        public TAOWListBox T2IncomeLB;
        public TAOWListBox T2LevelLB;
        public TAOWListBox T2WallLB;
        public TAOWListBox T2SizeLB;
        public TAOWListBox T2EncLB;
        public TAOWButton T2SName;
        public TAOWButton T2SRace;
        public TAOWButton T2SIn;
        public TAOWButton T2SWall;
        public TAOWButton T2SLvl;
        public TAOWButton T2SSize;
        public TAOWButton T2SEnc;
        public TAOWListBox T2ActLB;
        public TAOWButton T2SAct;
        public TAOWListBox T2RelLB;
        public TAOWButton T2SRel;
        public TAOWListBox T2MorLB;
        public TAOWButton T2SMor;
        public TAOWVScrollBar CitySB;
        public TAOWListBox T3Name;
        public TAOWButton T3SName;
        public TAOWListBox T3ActLB;
        public TAOWButton T3Activity;
        public TAOWVScrollBar PlaceSB;
        public TAoWFrame UnitFr;
        public TAoWGauge UnitGg;
        public TAOWListBox Unit1LB;
        public TAOWListBox Unit2LB;
        public TAOWListBox Unit3LB;
        public TAOWListBox Unit4LB;
        public TAOWListBox Unit5LB;
        public TAOWListBox Unit6LB;
        public TAOWListBox Unit7LB;
        public TAOWListBox Unit8LB;
        public TAOWVScrollBar PartySb;
        public TAOWPanel HeroPnl;
        public TAOWPanel T5MainPnl;
        public TAOWListBox T5Name;
        public TAOWListBox T5Health;
        public TAOWListBox T5Level;
        public TAOWButton T5SName;
        public TAOWButton T5SHealth;
        public TAOWButton T5SLevel;
        public TAOWPanel T5HeroInf;
        public TAOWPanel T5LBFr;
        public TAOWButton HInfoBtn;
        public TAOWVScrollBar HeroSB;
        public TAOWPanel TotalPnl;
        public TAOWPanel IncomePnl;
        public TAOWMemo IncomeMem;
        public TAOWPanel IncomeBtm;
        public TAOWVScrollBar IncomeSB;
        public TAOWLabel IncomeLbl;
        public TAOWLabel Income;
        public TAOWPanel UpkeepPnl;
        public TAOWMemo UpkeepMem;
        public TAOWPanel UpkeepBtm;
        public TAOWVScrollBar UpkeepSB;
        public TAOWLabel UpkeepLbl;
        public TAOWLabel Upkeep;
        public TAOWLabel Totallbl;
        public TAOWLabel NetInc;
        public TAOWPanel TotalBtm;
        public TAOWLabel TreasuryLbl;
        public TAOWLabel Treasury;
        public TAOWPanel TreasuryPnl;
        public TAOWMemo IncomeValue;
        public TAOWMemo UpkeepValue;
        public TAOWImage GoldAnim;
        public TAOWPanel PicPnl;
        public TAOWButton Help;
        public TIvTranslator Translator;
        // AoWRealm.pas:238
        public void UpdateIncome()
        {
            int Value = default;
            var call__1 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            Treasury.Text = IntToStr(call__1.FGems);
            var call__2 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__3 = call__2.GetUpkeep();
            var call__4 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            NetInc.Text = IntToStr(call__4.FIncome - call__3);
            var call__5 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            Income.Text = IntToStr(call__5.FIncome);
            var call__6 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__7 = call__6.GetUpkeep();
            Upkeep.Text = IntToStr(call__7);
            IncomeMem.Items.Clear();
            IncomeValue.Items.Clear();
            UpkeepValue.Items.Clear();
            var call__8 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            Value = call__8.FBaseIncome;
            if (Value != 0)
            {
                var call__9 = TranslateRStr(ExternalIncomeRStr);
                IncomeMem.Items.Add(call__9);
                IncomeValue.Items.Add(IntToStr(Value));
            }

            var call__10 = CityIncome(AoWHSMap.FSeatedPlayer);
            Value = call__10;
            if (Value != 0)
            {
                var call__11 = TranslateRStr(CitiesRStr);
                IncomeMem.Items.Add(call__11);
                IncomeValue.Items.Add(IntToStr(Value));
            }

            var call__12 = FarmIncome(AoWHSMap.FSeatedPlayer);
            Value = call__12;
            if (Value != 0)
            {
                var call__13 = TranslateRStr(FarmsRStr);
                IncomeMem.Items.Add(call__13);
                IncomeValue.Items.Add(IntToStr(Value));
            }

            var call__14 = MineIncome(AoWHSMap.FSeatedPlayer);
            Value = call__14;
            if (Value != 0)
            {
                var call__15 = TranslateRStr(MinesRStr);
                IncomeMem.Items.Add(call__15);
                IncomeValue.Items.Add(IntToStr(Value));
            }

            var call__16 = BuildersGuildIncome(AoWHSMap.FSeatedPlayer);
            Value = call__16;
            if (Value != 0)
            {
                var call__17 = TranslateRStr(BuildersGuildsRStr);
                IncomeMem.Items.Add(call__17);
                IncomeValue.Items.Add(IntToStr(Value));
            }

            var call__18 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            UpkeepValue.Items.Add(IntToStr(call__18.FArmyUpkeep));
        }

        // AoWRealm.pas:178
        public void UpdateCities()
        {
            int I = default;
            TPlayerStructure Structure = default;
            TCity C = default;
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
                int end__19 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
                for (; I <= end__19; I += 1)
                {
                    var call__20 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__21 = call__20.FStructures.GetStructure(I);
                    Structure = call__21;
                    if (Structure is TCity)
                    {
                        C = (TCity)Structure;
                        var call__22 = C.GetName();
                        T2NameLB.Items.AddObject(call__22, C);
                        var call__23 = TranslateRStr(RaceNamesPluralRStr[C.FRace]);
                        T2RaceLB.Items.Add(call__23);
                        T2IncomeLB.Items.Add(IntToStr(C.FIncome));
                        T2LevelLB.Items.Add(IntToStr(C.FUpgradeLevel));
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
                            case TCivilStatus.csUnrest:
                                T2MorLB.Items.Add(AnsiString.FromBytes(49));
                                break;
                            case TCivilStatus.csStable:
                                T2MorLB.Items.Add(AnsiString.FromBytes(50));
                                break;
                            case TCivilStatus.csContent:
                                T2MorLB.Items.Add(AnsiString.FromBytes(51));
                                break;
                            case TCivilStatus.csCheerful:
                                T2MorLB.Items.Add(AnsiString.FromBytes(52));
                                break;
                            default:
                                T2MorLB.Items.Add(AnsiString.FromBytes(45));
                                break;
                        }

                        var call__24 = AoWHSMap.FRaces.GetRace(C.FRace);
                        var call__25 = call__24.GetPlayerRelation(AoWHSMap.FSeatedPlayer);
                        switch ((int)call__25 & 127)
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

                        switch ((int)C.GetActiveAction() & 127)
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
                        T2SizeLB.Items.Add(IntToStr(call__26));
                    }
                }
            }
        }

        // AoWRealm.pas:309
        public void UpdatePlaces()
        {
            int I = default;
            TPlayerStructure Place = default;
            T3Name.Items.Clear();
            T3ActLB.Items.Clear();
            {
                I = 0;
                int end__27 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
                for (; I <= end__27; I += 1)
                {
                    var call__28 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__29 = call__28.FStructures.GetStructure(I);
                    Place = call__29;
                    if (!(Place is TCity))
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
        public void UpdateHeroes()
        {
            int I = default;
            int J = default;
            TAbstractUnit U = default;
            T5Name.Items.Clear();
            T5Health.Items.Clear();
            T5Level.Items.Clear();
            {
                I = 0;
                int end__32 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__32; I += 1)
                {
                    {
                        J = 0;
                        int end__33 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__33; J += 1)
                        {
                            var call__34 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                            var call__35 = call__34.FArmies.GetArmy(I);
                            var call__36 = call__35.GetUnit(J);
                            U = call__36;
                            if (U is THero)
                            {
                                var call__37 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                                var call__38 = call__37.FArmies.GetArmy(I);
                                var call__39 = U.GetName();
                                T5Name.Items.AddObject(call__39, call__38);
                                var call__40 = U.GetHitPoints();
                                var call__41 = U.GetHits();
                                T5Health.Items.AddObject(IntToStr(call__40) + AnsiString.FromBytes(47) + IntToStr(call__41), U);
                                var call__42 = ((THero)U).GetLevel();
                                T5Level.Items.Add(IntToStr(call__42));
                            }
                        }
                    }
                }
            }

            var call__43 = T5Name.Items.Count;
            if (call__43 > 0 && T5Name.Selected == -1)
            {
                T5Name.Selected = 0;
                HeroListChange(T5Name);
            }
        }

        // AoWRealm.pas:327
        public void UpdateOther()
        {
        }

        // AoWRealm.pas:331
        public void UpdateOverview()
        {
            if (AoWHSMap.FSeatedPlayer == -1)
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
        public void DrawArmyUnit(int Column, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            TAbstractUnit U = default;
            TRect OldClip = default;
            var call__44 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__45 = call__44.FArmies.GetArmy(DrawItem);
            var call__46 = call__45.GetCount();
            if (Column < call__46)
            {
                OldClip = SurfDesc.ClipRect;
                SurfDesc.ClipRect.Left = DrawRect.Left + 2;
                SurfDesc.ClipRect.Top = DrawRect.Top + 1;
                SurfDesc.ClipRect.Right = DrawRect.Right - 2;
                SurfDesc.ClipRect.Bottom = DrawRect.Top + Unit1LB.ItemHeight - 6;
                if (DrawRect.Bottom - DrawRect.Top < Unit1LB.ItemHeight - 6)
                {
                    SurfDesc.ClipRect.Bottom = DrawRect.Bottom;
                }

                var call__47 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__48 = call__47.FArmies.GetArmy(DrawItem);
                var call__49 = call__48.GetUnit(Column);
                U = call__49;
                var call__50 = U.GetAlignment();
                if ((int)call__50 < IntGfxMod.UnitBGs.Count)
                {
                    var call__51 = U.GetAlignment();
                    var call__52 = IntGfxMod.UnitBGs[(int)call__51];
                    var call__53 = U.GetAlignment();
                    var call__54 = IntGfxMod.UnitBGs[(int)call__53];
                    var call__55 = U.GetAlignment();
                    var call__56 = IntGfxMod.UnitBGs[(int)call__55];
                    call__56.ShowClipped(DrawRect.Left - (call__54.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (call__52.Height - (DrawRect.Bottom - DrawRect.Top - 10)) / 2, SurfDesc);
                }

                var call__57 = U.GetPreviewImage();
                var call__58 = U.GetPreviewImage();
                U.ShowEx(DrawRect.Left - (call__58.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (call__57.Height - (Unit1LB.ItemHeight - 10)) / 2, SurfDesc, General.FUnitShowInfo);
                var gauge2 = UnitGg;
                gauge2.Visible = true;
                var call__60 = U.GetHits();
                gauge2.Max = call__60;
                var call__61 = U.GetHitPoints();
                gauge2.Position = call__61;
                gauge2.WinLeft = DrawRect.Left - T4LBPnl.WinLeft + 5;
                gauge2.WinWidth = DrawRect.Right - DrawRect.Left - 10;
                gauge2.WinTop = DrawRect.Top + Unit1LB.ItemHeight - T4LBPnl.WinTop - 9;
                // { Original sizes and draws the gauge directly. }
                gauge2.SetSize();
                gauge2.Draw(SurfDesc);
                gauge2.Visible = false;

                SurfDesc.ClipRect = OldClip;
            }
        }

        // AoWRealm.pas:382
        public void OWMinIconClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWRealm.pas:388
        public void T2WallLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            var call__62 = T2WallLB.Items[DrawItem];
            ReadOnlySpan<byte> text = StringBytes(call__62);
            switch (text.IsEmpty ? (byte)0 : text[0])
            {
                case 49:
                    var call__63 = IntGfxMod.LBIcons[7];
                    var call__64 = IntGfxMod.LBIcons[7];
                    call__64.ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - call__63.Width / 2, DrawRect.Top, SurfDesc);
                    break;
                case 50:
                    var call__65 = IntGfxMod.LBIcons[8];
                    var call__66 = IntGfxMod.LBIcons[8];
                    call__66.ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - call__65.Width / 2, DrawRect.Top, SurfDesc);
                    break;
            }
        }

        // AoWRealm.pas:398
        public void Unit1LBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            DrawArmyUnit(Sender.Tag, DrawRect, DrawItem, ref SurfDesc);
        }

        // AoWRealm.pas:403
        public void HeaderTabChange()
        {
            General.PlayButtonSound(0);
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
        public void Unit1LBChange(TObject Sender)
        {
            var listBox = (TAOWListBox)Sender;
            var call__68 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__69 = call__68.FArmies.GetArmy(listBox.Selected);
            var call__70 = call__69.GetCount();
            if (call__70 > 0)
            {
                listBox.PopupMenu = UnitBanner.BannerPopup;
                var call__71 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__72 = call__71.FArmies.GetArmy(listBox.Selected);
                var call__73 = call__72.GetUnit(listBox.Tag);
                General.FSelectedUnit = call__73;
                var call__74 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__75 = call__74.FArmies.GetArmy(listBox.Selected);
                var call__76 = call__75.GetArmyHS();
                AoWHSMap.Select(call__76, true);
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
        public void T2MorLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            int Icon = default;
            var call__77 = T2MorLB.Items[DrawItem];
            ReadOnlySpan<byte> text = StringBytes(call__77);
            Icon = (text.IsEmpty ? (byte)0 : text[0]) - 48;
            var call__78 = IntGfxMod.LBIcons[Icon + 9];
            var call__79 = IntGfxMod.LBIcons[Icon + 9];
            call__79.ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - call__78.Width / 2, DrawRect.Top, SurfDesc);
        }

        // AoWRealm.pas:457
        public void CityListMouseDown(TAOWListBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            THexagonSprite Sprite = default;
            if (Button == TMouseButton.mbRight && Sender.Selected != -1)
            {
                var call__80 = T2NameLB.Items.Objects[Sender.Selected];
                Sprite = (THexagonSprite)call__80;
                var call__81 = Sprite.GetLhx();
                var call__82 = Sprite.GetYhx();
                var call__83 = Sprite.GetXhx();
                AoWHSMap.CenterViewTo(call__83, call__82, call__81);
                var call__84 = Sprite.GetLhx();
                var call__85 = Sprite.GetYhx();
                var call__86 = Sprite.GetXhx();
                AoWHSMap.PlayCenterAnimation(call__86, call__85, call__84);
            }
        }

        // AoWRealm.pas:469
        public void CityListChange(TObject Sender)
        {
            var listBox = (TAOWListBox)Sender;
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
        public void CityDoubleClick(TObject Sender)
        {
            if (((TAOWListBox)Sender).Selected != -1)
            {
                var call__88 = T2NameLB.Items.Objects[((TAOWListBox)Sender).Selected];
                CityScreen.ShowCity((TCity)call__88);
            }
        }

        // AoWRealm.pas:492
        public void PlaceListDoubleClick(TObject Sender)
        {
            if (((TAOWListBox)Sender).Selected != -1)
            {
                var call__89 = T3Name.Items.Objects[((TAOWListBox)Sender).Selected];
                if (call__89 is TProductionPlace)
                {
                    var call__90 = T3Name.Items.Objects[((TAOWListBox)Sender).Selected];
                    ProductionScreen.ShowPlace((TProductionPlace)call__90);
                }
                else
                {
                    var call__91 = T3Name.Items.Objects[((TAOWListBox)Sender).Selected];
                    AoWHSMap.Select((THexagonSprite)call__91, true);
                }
            }
        }

        // AoWRealm.pas:501
        public void PlacesListMouseDown(TAOWListBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            THexagonSprite Sprite = default;
            if (Button == TMouseButton.mbRight && Sender.Selected != -1)
            {
                var call__92 = T3Name.Items.Objects[Sender.Selected];
                Sprite = (THexagonSprite)call__92;
                var call__93 = Sprite.GetLhx();
                var call__94 = Sprite.GetYhx();
                var call__95 = Sprite.GetXhx();
                AoWHSMap.CenterViewTo(call__95, call__94, call__93);
                var call__96 = Sprite.GetLhx();
                var call__97 = Sprite.GetYhx();
                var call__98 = Sprite.GetXhx();
                AoWHSMap.PlayCenterAnimation(call__98, call__97, call__96);
            }
        }

        // AoWRealm.pas:513
        public void HeroListDoubleClick(TObject Sender)
        {
            var listBox = (TAOWListBox)Sender;
            if (listBox.Selected != -1)
            {
                var call__100 = T5Name.Items.Objects[listBox.Selected];
                var call__101 = ((TArmy)call__100).GetArmyHS();
                AoWHSMap.Select(call__101, true);
            }
        }

        // AoWRealm.pas:522
        public void HeroListMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAOWListBox List = default;
            THexagonSprite Sprite = default;
            List = (TAOWListBox)Sender;
            if (List.Selected != -1)
            {
                var call__102 = T5Name.Items.Objects[List.Selected];
                var call__103 = ((TArmy)call__102).GetArmyHS();
                Sprite = call__103;
                var call__104 = Sprite.GetLhx();
                var call__105 = Sprite.GetYhx();
                var call__106 = Sprite.GetXhx();
                AoWHSMap.CenterViewTo(call__106, call__105, call__104);
                var call__107 = Sprite.GetLhx();
                var call__108 = Sprite.GetYhx();
                var call__109 = Sprite.GetXhx();
                AoWHSMap.PlayCenterAnimation(call__109, call__108, call__107);
            }
        }

        // AoWRealm.pas:536
        public void HeroInfoClick()
        {
            int Index = default;
            General.PlayButtonSound(0);
            Index = T5Name.Selected;
            if (Index != -1)
            {
                // { evaluate the index before the receiver chain }
                var call__110 = T5Health.Items.Objects[Index << 0];
                General.FSelectedUnit = (THero)call__110;
                UnitWindow.ShowUnit();
            }
        }

        // AoWRealm.pas:549
        public void T2LevelLBDoubleClick()
        {
            AoWRealm.RealmWin.UpdateOverview();
            AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:555
        public void T2WallLBDoubleClick()
        {
            AoWRealm.RealmWin.UpdateOverview();
            AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:561
        public void T2RaceLBDoubleClick()
        {
            AoWRealm.RealmWin.UpdateOverview();
            AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:567
        public void T2ActLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            byte Icon = default;
            var call__111 = T2ActLB.Items[DrawItem];
            ReadOnlySpan<byte> text = StringBytes(call__111);
            Icon = text.IsEmpty ? (byte)0 : text[0];
            if (Icon != 45)
            {
                var call__112 = IntGfxMod.LBIcons[Icon - 23];
                var call__113 = IntGfxMod.LBIcons[Icon - 23];
                call__113.ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - call__112.Width / 2, DrawRect.Top, SurfDesc);
            }
        }

        // AoWRealm.pas:577
        public void T2ActLBDoubleClick()
        {
            AoWRealm.RealmWin.UpdateOverview();
            AoWRealm.RealmWin.Dlg.Visible = true;
        }

        // AoWRealm.pas:583
        public void T2RelLBDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            int Icon = default;
            var call__114 = T2RelLB.Items[DrawItem];
            ReadOnlySpan<byte> text = StringBytes(call__114);
            Icon = (text.IsEmpty ? (byte)0 : text[0]) - 48;
            var call__115 = IntGfxMod.LBIcons[Icon + 14];
            var call__116 = IntGfxMod.LBIcons[Icon + 14];
            call__116.ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - call__115.Width / 2, DrawRect.Top, SurfDesc);
        }

        // AoWRealm.pas:592
        public void OverWindowResize()
        {
            this.Dlg.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            this.Dlg.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
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
        public void T3NameChange(TObject Sender)
        {
            TAOWListBox List = default;
            List = (TAOWListBox)Sender;
            T3Name.Selected = List.Selected;
            T3ActLB.Selected = List.Selected;
        }

        // AoWRealm.pas:625
        public void T3SBChange()
        {
            T3Name.ListOffset = PlaceSB.Position;
        }

        // AoWRealm.pas:630
        public void T2SNameClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:649
        public void T2SRaceClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:668
        public void T2SInClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 2);
        }

        // AoWRealm.pas:687
        public void T2SLvlClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:706
        public void T2SWallClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:725
        public void T2SSizeClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:744
        public void T2SRelClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:763
        public void T2SMorClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:782
        public void T2SActClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:801
        public void T2SEncClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
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
            General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:820
        public void T3SNameClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T3Name);
            Lists.Add(T3ActLB);
            General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:831
        public void HeroListChange(TObject Sender)
        {
            TAOWListBox List = default;
            List = (TAOWListBox)Sender;
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
        public void T5HeroInfDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            int Index = default;
            THero Hero = default;
            TRect R = default;
            Index = T5Name.Selected;
            if (Index > -1)
            {
                var call__118 = T5Health.Items.Objects[Index << 0];
                Hero = (THero)call__118;
                var call__119 = Rect(T5HeroInf.WinLeft + 1, T5HeroInf.WinTop + 1, T5HeroInf.WinLeft + T5HeroInf.WinWidth - 1, T5HeroInf.WinTop + T5HeroInf.WinHeight - 1);
                R = call__119;
                if (Hero.GetFace() != null)
                {
                    Hero.ShowFace(R, SurfDesc);
                }
            }
        }

        // AoWRealm.pas:866
        public void Unit1LBDoubleClick(TObject Sender)
        {
            var call__120 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__121 = call__120.FArmies.GetArmy(((TAOWListBox)Sender).Selected);
            var call__122 = call__121.GetCount();
            if (call__122 > 0)
            {
                var call__123 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__124 = call__123.FArmies.GetArmy(((TAOWListBox)Sender).Selected);
                var call__125 = call__124.GetUnit(((TAOWListBox)Sender).Tag);
                General.FSelectedUnit = call__125;
                UnitWindow.ShowUnit();
            }
        }

        // AoWRealm.pas:875
        public void T5SNameClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T5Name);
            Lists.Add(T5Health);
            Lists.Add(T5Level);
            General.SortLists(ref Lists, 0);
        }

        // AoWRealm.pas:887
        public void T5SHealthClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T5Health);
            Lists.Add(T5Name);
            Lists.Add(T5Level);
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:899
        public void T5SLevelClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T5Level);
            Lists.Add(T5Name);
            Lists.Add(T5Health);
            General.SortLists(ref Lists, 2);
        }

        // AoWRealm.pas:911
        public void GotoButClick()
        {
            General.PlayButtonSound(0);
        }

        // AoWRealm.pas:916
        public void T4PnlShow(TObject Sender)
        {
            int I = default;
            int J = default;
            {
                I = 0;
                int end__126 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__126; I += 1)
                {
                    {
                        J = 0;
                        int end__127 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__127; J += 1)
                        {
                            var call__128 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                            var call__129 = call__128.FArmies.GetArmy(I);
                            var call__130 = call__129.GetUnit(J);
                            var call__131 = call__130.GetUnitGFXResourceIndex();
                            var call__132 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__131);
                            call__132.AddRef();
                        }
                    }
                }
            }
        }

        // AoWRealm.pas:926
        public void T4PnlHide(TObject Sender)
        {
            int I = default;
            int J = default;
            {
                I = 0;
                int end__133 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1;
                for (; I <= end__133; I += 1)
                {
                    {
                        J = 0;
                        int end__134 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1;
                        for (; J <= end__134; J += 1)
                        {
                            var call__135 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                            var call__136 = call__135.FArmies.GetArmy(I);
                            var call__137 = call__136.GetUnit(J);
                            var call__138 = call__137.GetUnitGFXResourceIndex();
                            var call__139 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__138);
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
        public void T3ActivityClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T3ActLB);
            Lists.Add(T3Name);
            General.SortLists(ref Lists, 1);
        }

        // AoWRealm.pas:962
        public void UpkeepSBChange()
        {
            UpkeepValue.ListOffset = UpkeepSB.Position;
        }

        // AoWRealm.pas:967
        public void DlgShow()
        {
            General.RegisterDialog(Dlg);
        }

        // AoWRealm.pas:972
        public void DlgHide()
        {
            General.UnregisterDialog(Dlg);
        }

        // AoWRealm.pas:977
        public void HelpClick()
        {
            ShowHelp(505);
        }
    }

    public static partial class AoWRealm
    {
        public static TRealmWin RealmWin;
    }
}
