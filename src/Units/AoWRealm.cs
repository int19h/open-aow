// Translated from AoWRealm.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWRealm.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWCityScreenUnit;
using static Aow.Game.Ui.Dialogs.aowHelpWinUnit;
using static Aow.Game.Ui.Dialogs.aowIntGfxModUnit;
using static Aow.Game.Ui.Dialogs.AoWProdScreenUnit;
using static Aow.Game.Ui.Dialogs.AoWUnitBannerUnit;
using static Aow.Game.Ui.Dialogs.AoWUnitWindowUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Game.World.Structures.BldGuildUnit;
using static Aow.Game.World.Structures.CityUnit;
using static Aow.Game.World.Structures.FarmUnit;
using static Aow.Game.World.Structures.MineUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
    private TAOWListBox[] partyLists;
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
        int Value;
        var playerGems = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
        Treasury.Text = IntToStr(playerGems.FGems);
        var upkeep = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetUpkeep();
        var playerIncome = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
        NetInc.Text = IntToStr(playerIncome.FIncome - upkeep);
        var player = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
        Income.Text = IntToStr(player.FIncome);
        var upkeep2 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).GetUpkeep();
        Upkeep.Text = IntToStr(upkeep2);
        IncomeMem.Items.Clear();
        IncomeValue.Items.Clear();
        UpkeepValue.Items.Clear();
        Value = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FBaseIncome;
        if (Value != 0)
        {
            var externalIncome = TranslateRStr(ExternalIncomeRStr);
            IncomeMem.Items.Add(externalIncome);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = CityIncome(AoWHSMap.FSeatedPlayer);
        if (Value != 0)
        {
            var cities = TranslateRStr(CitiesRStr);
            IncomeMem.Items.Add(cities);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = FarmIncome(AoWHSMap.FSeatedPlayer);
        if (Value != 0)
        {
            var farms = TranslateRStr(FarmsRStr);
            IncomeMem.Items.Add(farms);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = MineIncome(AoWHSMap.FSeatedPlayer);
        if (Value != 0)
        {
            var mines = TranslateRStr(MinesRStr);
            IncomeMem.Items.Add(mines);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = BuildersGuildIncome(AoWHSMap.FSeatedPlayer);
        if (Value != 0)
        {
            var buildersGuilds = TranslateRStr(BuildersGuildsRStr);
            IncomeMem.Items.Add(buildersGuilds);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        var playerArmyUpkeep = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
        UpkeepValue.Items.Add(IntToStr(playerArmyUpkeep.FArmyUpkeep));
    }

    // AoWRealm.pas:178
    public void UpdateCities()
    {
        TPlayerStructure Structure;
        TCity C;
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
        for (int I = 0, last = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Structure = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(I);
            if (Structure is TCity)
            {
                C = (TCity)Structure;
                var name = C.GetName();
                T2NameLB.Items.AddObject(name, C);
                var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[C.FRace]);
                T2RaceLB.Items.Add(raceNamesPlural);
                T2IncomeLB.Items.Add(IntToStr(C.FIncome));
                T2LevelLB.Items.Add(IntToStr(C.FUpgradeLevel));
                switch (C.FWallType)
                {
                    case 1:
                        T2WallLB.Items.Add("1");
                        break;
                    case 2:
                        T2WallLB.Items.Add("2");
                        break;
                    default:
                        T2WallLB.Items.Add("-");
                        break;
                }

                switch (C.GetCivilStatus())
                {
                    case TCivilStatus.csUnrest:
                        T2MorLB.Items.Add("1");
                        break;
                    case TCivilStatus.csStable:
                        T2MorLB.Items.Add("2");
                        break;
                    case TCivilStatus.csContent:
                        T2MorLB.Items.Add("3");
                        break;
                    case TCivilStatus.csCheerful:
                        T2MorLB.Items.Add("4");
                        break;
                    default:
                        T2MorLB.Items.Add("-");
                        break;
                }

                switch ((int)AoWHSMap.FRaces.GetRace(C.FRace).GetPlayerRelation(AoWHSMap.FSeatedPlayer) & 127)
                {
                    case 0:
                        T2RelLB.Items.Add("0");
                        break;
                    case 1:
                        T2RelLB.Items.Add("1");
                        break;
                    case 2:
                        T2RelLB.Items.Add("2");
                        break;
                    case 3:
                        T2RelLB.Items.Add("3");
                        break;
                    case 4:
                        T2RelLB.Items.Add("4");
                        break;
                    default:
                        T2RelLB.Items.Add("-");
                        break;
                }

                var activeOrder = C.GetActiveOrder();
                T2ActLB.Items.Add(activeOrder != null ? activeOrder.RealmActivity(C) : "-");

                T2EncLB.Items.Add("-");
                var size = C.GetSize();
                T2SizeLB.Items.Add(IntToStr(size));
            }
        }
    }

    // AoWRealm.pas:309
    public void UpdatePlaces()
    {
        TPlayerStructure Place;
        T3Name.Items.Clear();
        T3ActLB.Items.Clear();
        for (int I = 0, last = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Place = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(I);
            if (!(Place is TCity))
            {
                var editName = Place.EditName();
                T3Name.Items.AddObject(editName, Place);
                var currentActivityText = Place.GetCurrentActivityText();
                T3ActLB.Items.Add(currentActivityText);
            }
        }
    }

    // AoWRealm.pas:283
    public void UpdateHeroes()
    {
        TAbstractUnit U;
        T5Name.Items.Clear();
        T5Health.Items.Clear();
        T5Level.Items.Clear();
        for (int I = 0, last = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                U = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J);
                if (U is THero)
                {
                    var army = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I);
                    var name = U.GetName();
                    T5Name.Items.AddObject(name, army);
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    T5Health.Items.AddObject(IntToStr(hitPoints) + "/" + IntToStr(hits), U);
                    var level = ((THero)U).GetLevel();
                    T5Level.Items.Add(IntToStr(level));
                }
            }
        }

        if (T5Name.Items.Count > 0 && T5Name.Selected == -1)
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
        TAbstractUnit U;
        TRect OldClip;
        if (Column < AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetCount())
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

            U = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetUnit(Column);
            if ((int)U.GetAlignment() < IntGfxMod.UnitBGs.Count)
            {
                var alignment = U.GetAlignment();
                var unitBG = IntGfxMod.UnitBGs[(int)alignment];
                unitBG.ShowClipped(DrawRect.Left - (unitBG.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (unitBG.Height - (DrawRect.Bottom - DrawRect.Top - 10)) / 2, SurfDesc);
            }

            var previewImage = U.GetPreviewImage();
            var uPreviewImage = U.GetPreviewImage();
            U.ShowEx(DrawRect.Left - (uPreviewImage.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (previewImage.Height - (Unit1LB.ItemHeight - 10)) / 2, SurfDesc, General.FUnitShowInfo);
            var gauge = UnitGg;
            gauge.Visible = true;
            gauge.Max = U.GetHits();
            gauge.Position = U.GetHitPoints();
            gauge.WinLeft = DrawRect.Left - T4LBPnl.WinLeft + 5;
            gauge.WinWidth = DrawRect.Right - DrawRect.Left - 10;
            gauge.WinTop = DrawRect.Top + Unit1LB.ItemHeight - T4LBPnl.WinTop - 9;
            // { Original sizes and draws the gauge directly. }
            gauge.SetSize();
            gauge.Draw(SurfDesc);
            gauge.Visible = false;

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
    public void T2WallLBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        string text = T2WallLB.Items[DrawItem];
        switch (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]))
        {
            case 49:
                var lbIcon = IntGfxMod.LBIcons[7];
                IntGfxMod.LBIcons[7].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
                break;
            case 50:
                var lbIconWidth = IntGfxMod.LBIcons[8];
                IntGfxMod.LBIcons[8].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIconWidth.Width / 2, DrawRect.Top, SurfDesc);
                break;
        }
    }

    // AoWRealm.pas:398
    public void Unit1LBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc, int Slot)
    {
        DrawArmyUnit(Slot, DrawRect, DrawItem, ref SurfDesc);
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
    public void Unit1LBChange(TAOWListBox ListBox, int Slot)
    {
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetCount() > 0)
        {
            ListBox.PopupMenu = UnitBanner.BannerPopup;
            var unit = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetUnit(Slot);
            General.FSelectedUnit = unit;
            var armyHS = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetArmyHS();
            AoWHSMap.Select(armyHS, true);
            Dlg.Priority = Dlg.Priority;
        }
        else
        {
            ListBox.PopupMenu = null;
            ListBox.Selected = -1;
        }

        partyLists ??= [Unit1LB, Unit2LB, Unit3LB, Unit4LB, Unit5LB, Unit6LB, Unit7LB, Unit8LB];
        for (int index = 0; index < partyLists.Length; index++)
        {
            if (index != Slot)
            {
                partyLists[index].Selected = -1;
            }
        }
    }

    // AoWRealm.pas:448
    public void T2MorLBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        int Icon;
        string text = T2MorLB.Items[DrawItem];
        Icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var lbIcon = IntGfxMod.LBIcons[Icon + 9];
        IntGfxMod.LBIcons[Icon + 9].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(TAOWListBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        THexagonSprite Sprite;
        if (Button == TMouseButton.mbRight && Sender.Selected != -1)
        {
            Sprite = (THexagonSprite)T2NameLB.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:469
    public void CityListChange(TAOWListBox Sender)
    {
        var listBox = Sender;
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
    public void CityDoubleClick(TAOWListBox Sender)
    {
        if (Sender.Selected != -1)
        {
            var city = T2NameLB.Items.Objects[Sender.Selected];
            CityScreen.ShowCity((TCity)city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(TAOWListBox Sender)
    {
        if (Sender.Selected != -1)
        {
            if (T3Name.Items.Objects[Sender.Selected] is TProductionPlace)
            {
                var productionPlace = T3Name.Items.Objects[Sender.Selected];
                ProductionScreen.ShowPlace((TProductionPlace)productionPlace);
            }
            else
            {
                var hexagonSprite = T3Name.Items.Objects[Sender.Selected];
                AoWHSMap.Select((THexagonSprite)hexagonSprite, true);
            }
        }
    }

    // AoWRealm.pas:501
    public void PlacesListMouseDown(TAOWListBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        THexagonSprite Sprite;
        if (Button == TMouseButton.mbRight && Sender.Selected != -1)
        {
            Sprite = (THexagonSprite)T3Name.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:513
    public void HeroListDoubleClick(TAOWListBox Sender)
    {
        var listBox = Sender;
        if (listBox.Selected != -1)
        {
            var armyHS = ((TArmy)T5Name.Items.Objects[listBox.Selected]).GetArmyHS();
            AoWHSMap.Select(armyHS, true);
        }
    }

    // AoWRealm.pas:522
    public void HeroListMouseDown(TAOWListBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAOWListBox List;
        THexagonSprite Sprite;
        List = Sender;
        if (List.Selected != -1)
        {
            Sprite = ((TArmy)T5Name.Items.Objects[List.Selected]).GetArmyHS();
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:536
    public void HeroInfoClick()
    {
        int Index;
        General.PlayButtonSound(0);
        Index = T5Name.Selected;
        if (Index != -1)
        {
            // { evaluate the index before the receiver chain }
            var hero = T5Health.Items.Objects[Index << 0];
            General.FSelectedUnit = (THero)hero;
            UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:549
    public void T2LevelLBDoubleClick()
    {
        AoWRealmUnit.RealmWin.UpdateOverview();
        AoWRealmUnit.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:555
    public void T2WallLBDoubleClick()
    {
        AoWRealmUnit.RealmWin.UpdateOverview();
        AoWRealmUnit.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:561
    public void T2RaceLBDoubleClick()
    {
        AoWRealmUnit.RealmWin.UpdateOverview();
        AoWRealmUnit.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:567
    public void T2ActLBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        byte Icon;
        string text = T2ActLB.Items[DrawItem];
        Icon = IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]);
        if (Icon != 45)
        {
            var lbIcon = IntGfxMod.LBIcons[Icon - 23];
            IntGfxMod.LBIcons[Icon - 23].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
        }
    }

    // AoWRealm.pas:577
    public void T2ActLBDoubleClick()
    {
        AoWRealmUnit.RealmWin.UpdateOverview();
        AoWRealmUnit.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:583
    public void T2RelLBDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        int Icon;
        string text = T2RelLB.Items[DrawItem];
        Icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var lbIcon = IntGfxMod.LBIcons[Icon + 14];
        IntGfxMod.LBIcons[Icon + 14].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
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
    public void T3NameChange(TAOWListBox Sender)
    {
        TAOWListBox List;
        List = Sender;
        T3Name.Selected = List.Selected;
        T3ActLB.Selected = List.Selected;
    }

    // AoWRealm.pas:625
    public void T3SBChange()
    {
        T3Name.ListOffset = PlaceSB.Position;
    }

    // AoWRealm.pas:630
    public void T2SNameClick(int Button)
    {
        TList Lists;
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
    public void T2SRaceClick(int Button)
    {
        TList Lists;
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
    public void T2SInClick(int Button)
    {
        TList Lists;
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
    public void T2SLvlClick(int Button)
    {
        TList Lists;
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
    public void T2SWallClick(int Button)
    {
        TList Lists;
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
    public void T2SSizeClick(int Button)
    {
        TList Lists;
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
    public void T2SRelClick(int Button)
    {
        TList Lists;
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
    public void T2SMorClick(int Button)
    {
        TList Lists;
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
    public void T2SActClick(int Button)
    {
        TList Lists;
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
    public void T2SEncClick(int Button)
    {
        TList Lists;
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
    public void T3SNameClick(int Button)
    {
        TList Lists;
        General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T3Name);
        Lists.Add(T3ActLB);
        General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:831
    public void HeroListChange(TAOWListBox Sender)
    {
        TAOWListBox List;
        List = Sender;
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
    public void T5HeroInfDraw(TSurfaceDesc SurfDesc)
    {
        int Index;
        THero Hero;
        TRect R;
        Index = T5Name.Selected;
        if (Index > -1)
        {
            Hero = (THero)T5Health.Items.Objects[Index << 0];
            R = Rect(T5HeroInf.WinLeft + 1, T5HeroInf.WinTop + 1, T5HeroInf.WinLeft + T5HeroInf.WinWidth - 1, T5HeroInf.WinTop + T5HeroInf.WinHeight - 1);
            if (Hero.GetFace() != null)
            {
                Hero.ShowFace(R, SurfDesc);
            }
        }
    }

    // AoWRealm.pas:866
    public void Unit1LBDoubleClick(TAOWListBox Sender, int Slot)
    {
        if (AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetCount() > 0)
        {
            var unit = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetUnit(Slot);
            General.FSelectedUnit = unit;
            UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:875
    public void T5SNameClick(int Button)
    {
        TList Lists;
        General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T5Name);
        Lists.Add(T5Health);
        Lists.Add(T5Level);
        General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:887
    public void T5SHealthClick(int Button)
    {
        TList Lists;
        General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T5Health);
        Lists.Add(T5Name);
        Lists.Add(T5Level);
        General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:899
    public void T5SLevelClick(int Button)
    {
        TList Lists;
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
    public void T4PnlShow()
    {
        for (int I = 0, last = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
                AoWHSSet.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex).AddRef();
            }
        }
    }

    // AoWRealm.pas:926
    public void T4PnlHide()
    {
        for (int I = 0, last = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
                AoWHSSet.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex).Release();
            }
        }
    }

    // AoWRealm.pas:936
    public void OverWindowShow()
    {
        if (T4Pnl.Visible)
        {
            T4PnlShow();
        }
    }

    // AoWRealm.pas:941
    public void OverWindowHide()
    {
        if (T4Pnl.Visible)
        {
            T4PnlHide();
        }
    }

    // AoWRealm.pas:946
    public void IncomeSBChange()
    {
        IncomeValue.ListOffset = IncomeSB.Position;
    }

    // AoWRealm.pas:951
    public void T3ActivityClick(int Button)
    {
        TList Lists;
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

public static partial class AoWRealmUnit
{
    public static TRealmWin RealmWin;
}
