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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class RealmWin : DataModule
{
    public RealmWin(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel OWTitle;
    public AOWImage OWTitleIc;
    public AOWTabPanel HeaderTab;
    public AOWPanel T1Pnl;
    public AOWPanel T2Pnl;
    public AOWPanel T2LBPnl;
    public AOWPanel T3Pnl;
    public AOWPanel T3LBPnl;
    public AOWButton Minimize;
    public AOWPanel T4Pnl;
    public AOWPanel T4LBPnl;
    public AOWListBox T2NameLB;
    public AOWListBox T2RaceLB;
    public AOWListBox T2IncomeLB;
    public AOWListBox T2LevelLB;
    public AOWListBox T2WallLB;
    public AOWListBox T2SizeLB;
    public AOWListBox T2EncLB;
    public AOWButton T2SName;
    public AOWButton T2SRace;
    public AOWButton T2SIn;
    public AOWButton T2SWall;
    public AOWButton T2SLvl;
    public AOWButton T2SSize;
    public AOWButton T2SEnc;
    public AOWListBox T2ActLB;
    public AOWButton T2SAct;
    public AOWListBox T2RelLB;
    public AOWButton T2SRel;
    public AOWListBox T2MorLB;
    public AOWButton T2SMor;
    public AOWVScrollBar CitySB;
    public AOWListBox T3Name;
    public AOWButton T3SName;
    public AOWListBox T3ActLB;
    public AOWButton T3Activity;
    public AOWVScrollBar PlaceSB;
    public AoWFrame UnitFr;
    public AoWGauge UnitGg;
    public AOWListBox Unit1LB;
    public AOWListBox Unit2LB;
    public AOWListBox Unit3LB;
    public AOWListBox Unit4LB;
    public AOWListBox Unit5LB;
    public AOWListBox Unit6LB;
    public AOWListBox Unit7LB;
    public AOWListBox Unit8LB;
    private AOWListBox[] partyLists;
    public AOWVScrollBar PartySb;
    public AOWPanel HeroPnl;
    public AOWPanel T5MainPnl;
    public AOWListBox T5Name;
    public AOWListBox T5Health;
    public AOWListBox T5Level;
    public AOWButton T5SName;
    public AOWButton T5SHealth;
    public AOWButton T5SLevel;
    public AOWPanel T5HeroInf;
    public AOWPanel T5LBFr;
    public AOWButton HInfoBtn;
    public AOWVScrollBar HeroSB;
    public AOWPanel TotalPnl;
    public AOWPanel IncomePnl;
    public AOWMemo IncomeMem;
    public AOWPanel IncomeBtm;
    public AOWVScrollBar IncomeSB;
    public AOWLabel IncomeLbl;
    public AOWLabel Income;
    public AOWPanel UpkeepPnl;
    public AOWMemo UpkeepMem;
    public AOWPanel UpkeepBtm;
    public AOWVScrollBar UpkeepSB;
    public AOWLabel UpkeepLbl;
    public AOWLabel Upkeep;
    public AOWLabel Totallbl;
    public AOWLabel NetInc;
    public AOWPanel TotalBtm;
    public AOWLabel TreasuryLbl;
    public AOWLabel Treasury;
    public AOWPanel TreasuryPnl;
    public AOWMemo IncomeValue;
    public AOWMemo UpkeepValue;
    public AOWImage GoldAnim;
    public AOWPanel PicPnl;
    public AOWButton Help;
    public IvTranslator Translator;

    // AoWRealm.pas:238
    public void UpdateIncome()
    {
        int Value;
        var playerGems = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
        Treasury.Text = IntToStr(playerGems.FGems);
        var upkeep = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetUpkeep();
        var playerIncome = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
        NetInc.Text = IntToStr(playerIncome.FIncome - upkeep);
        var player = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
        Income.Text = IntToStr(player.FIncome);
        var upkeep2 = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).GetUpkeep();
        Upkeep.Text = IntToStr(upkeep2);
        IncomeMem.Items.Clear();
        IncomeValue.Items.Clear();
        UpkeepValue.Items.Clear();
        Value = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FBaseIncome;
        if (Value != 0)
        {
            var externalIncome = AbilityRules.TranslateRStr(ExternalIncomeRStr);
            IncomeMem.Items.Add(externalIncome);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = CityIncome(World.Map.AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var cities = AbilityRules.TranslateRStr(CitiesRStr);
            IncomeMem.Items.Add(cities);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = FarmIncome(World.Map.AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var farms = AbilityRules.TranslateRStr(FarmsRStr);
            IncomeMem.Items.Add(farms);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = MineIncome(World.Map.AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var mines = AbilityRules.TranslateRStr(MinesRStr);
            IncomeMem.Items.Add(mines);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = BuildersGuildIncome(World.Map.AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var buildersGuilds = AbilityRules.TranslateRStr(BuildersGuildsRStr);
            IncomeMem.Items.Add(buildersGuilds);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        var playerArmyUpkeep = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer);
        UpkeepValue.Items.Add(IntToStr(playerArmyUpkeep.FArmyUpkeep));
    }

    // AoWRealm.pas:178
    public void UpdateCities()
    {
        PlayerStructure Structure;
        City C;
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
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Structure = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(I);
            if (Structure is City)
            {
                C = (City)Structure;
                var name = C.GetName();
                T2NameLB.Items.AddObject(name, C);
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[C.FRace]);
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
                    case City.CivilStatus.Unrest:
                        T2MorLB.Items.Add("1");
                        break;
                    case City.CivilStatus.Stable:
                        T2MorLB.Items.Add("2");
                        break;
                    case City.CivilStatus.Content:
                        T2MorLB.Items.Add("3");
                        break;
                    case City.CivilStatus.Cheerful:
                        T2MorLB.Items.Add("4");
                        break;
                    default:
                        T2MorLB.Items.Add("-");
                        break;
                }

                switch ((int)World.Map.AoWHSMap.Current.FRaces.GetRace(C.FRace).GetPlayerRelation(World.Map.AoWHSMap.Current.FSeatedPlayer) & 127)
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
        PlayerStructure Place;
        T3Name.Items.Clear();
        T3ActLB.Items.Clear();
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Place = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(I);
            if (!(Place is City))
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
        AbstractUnit U;
        T5Name.Items.Clear();
        T5Health.Items.Clear();
        T5Level.Items.Clear();
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                U = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J);
                if (U is Hero)
                {
                    var army = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I);
                    var name = U.GetName();
                    T5Name.Items.AddObject(name, army);
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    T5Health.Items.AddObject(IntToStr(hitPoints) + "/" + IntToStr(hits), U);
                    var level = ((Hero)U).GetLevel();
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
        if (World.Map.AoWHSMap.Current.FSeatedPlayer == -1)
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
    public void DrawArmyUnit(int Column, in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        AbstractUnit U;
        Rect OldClip;
        if (Column < World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetCount())
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

            U = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetUnit(Column);
            if ((int)U.GetAlignment() < DialogHosts.IntGfxMod.UnitBGs.Count)
            {
                var alignment = U.GetAlignment();
                var unitBG = DialogHosts.IntGfxMod.UnitBGs[(int)alignment];
                unitBG.ShowClipped(DrawRect.Left - (unitBG.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (unitBG.Height - (DrawRect.Bottom - DrawRect.Top - 10)) / 2, SurfDesc);
            }

            var previewImage = U.GetPreviewImage();
            var uPreviewImage = U.GetPreviewImage();
            U.ShowEx(DrawRect.Left - (uPreviewImage.Width - (DrawRect.Right - DrawRect.Left)) / 2, DrawRect.Top - (previewImage.Height - (Unit1LB.ItemHeight - 10)) / 2, SurfDesc, DialogHosts.General.FUnitShowInfo);
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
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWRealm.pas:388
    public void T2WallLBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        string text = T2WallLB.Items[DrawItem];
        switch (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]))
        {
            case 49:
                var lbIcon = DialogHosts.IntGfxMod.LBIcons[7];
                DialogHosts.IntGfxMod.LBIcons[7].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
                break;
            case 50:
                var lbIconWidth = DialogHosts.IntGfxMod.LBIcons[8];
                DialogHosts.IntGfxMod.LBIcons[8].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIconWidth.Width / 2, DrawRect.Top, SurfDesc);
                break;
        }
    }

    // AoWRealm.pas:398
    public void Unit1LBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc, int Slot)
    {
        DrawArmyUnit(Slot, DrawRect, DrawItem, ref SurfDesc);
    }

    // AoWRealm.pas:403
    public void HeaderTabChange()
    {
        DialogHosts.General.PlayButtonSound(0);
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
    public void Unit1LBChange(AOWListBox ListBox, int Slot)
    {
        if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetCount() > 0)
        {
            ListBox.PopupMenu = DialogHosts.UnitBanner.BannerPopup;
            var unit = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetUnit(Slot);
            DialogHosts.General.FSelectedUnit = unit;
            var armyHS = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetArmyHS();
            World.Map.AoWHSMap.Current.Select(armyHS, true);
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
    public void T2MorLBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        int Icon;
        string text = T2MorLB.Items[DrawItem];
        Icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var lbIcon = DialogHosts.IntGfxMod.LBIcons[Icon + 9];
        DialogHosts.IntGfxMod.LBIcons[Icon + 9].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(AOWListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        HexagonSprite Sprite;
        if (Button == MouseButton.Right && Sender.Selected != -1)
        {
            Sprite = (HexagonSprite)T2NameLB.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:469
    public void CityListChange(AOWListBox Sender)
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
    public void CityDoubleClick(AOWListBox Sender)
    {
        if (Sender.Selected != -1)
        {
            var city = T2NameLB.Items.Objects[Sender.Selected];
            DialogHosts.CityScreen.ShowCity((City)city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(AOWListBox Sender)
    {
        if (Sender.Selected != -1)
        {
            if (T3Name.Items.Objects[Sender.Selected] is ProductionPlace)
            {
                var productionPlace = T3Name.Items.Objects[Sender.Selected];
                DialogHosts.ProductionScreen.ShowPlace((ProductionPlace)productionPlace);
            }
            else
            {
                var hexagonSprite = T3Name.Items.Objects[Sender.Selected];
                World.Map.AoWHSMap.Current.Select((HexagonSprite)hexagonSprite, true);
            }
        }
    }

    // AoWRealm.pas:501
    public void PlacesListMouseDown(AOWListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        HexagonSprite Sprite;
        if (Button == MouseButton.Right && Sender.Selected != -1)
        {
            Sprite = (HexagonSprite)T3Name.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:513
    public void HeroListDoubleClick(AOWListBox Sender)
    {
        var listBox = Sender;
        if (listBox.Selected != -1)
        {
            var armyHS = ((Army)T5Name.Items.Objects[listBox.Selected]).GetArmyHS();
            World.Map.AoWHSMap.Current.Select(armyHS, true);
        }
    }

    // AoWRealm.pas:522
    public void HeroListMouseDown(AOWListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AOWListBox List;
        HexagonSprite Sprite;
        List = Sender;
        if (List.Selected != -1)
        {
            Sprite = ((Army)T5Name.Items.Objects[List.Selected]).GetArmyHS();
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            World.Map.AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:536
    public void HeroInfoClick()
    {
        int Index;
        DialogHosts.General.PlayButtonSound(0);
        Index = T5Name.Selected;
        if (Index != -1)
        {
            // { evaluate the index before the receiver chain }
            var hero = T5Health.Items.Objects[Index << 0];
            DialogHosts.General.FSelectedUnit = (Hero)hero;
            DialogHosts.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:549
    public void T2LevelLBDoubleClick()
    {
        DialogHosts.RealmWin.UpdateOverview();
        DialogHosts.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:555
    public void T2WallLBDoubleClick()
    {
        DialogHosts.RealmWin.UpdateOverview();
        DialogHosts.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:561
    public void T2RaceLBDoubleClick()
    {
        DialogHosts.RealmWin.UpdateOverview();
        DialogHosts.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:567
    public void T2ActLBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        byte Icon;
        string text = T2ActLB.Items[DrawItem];
        Icon = IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]);
        if (Icon != 45)
        {
            var lbIcon = DialogHosts.IntGfxMod.LBIcons[Icon - 23];
            DialogHosts.IntGfxMod.LBIcons[Icon - 23].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
        }
    }

    // AoWRealm.pas:577
    public void T2ActLBDoubleClick()
    {
        DialogHosts.RealmWin.UpdateOverview();
        DialogHosts.RealmWin.Dlg.Visible = true;
    }

    // AoWRealm.pas:583
    public void T2RelLBDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SurfDesc)
    {
        int Icon;
        string text = T2RelLB.Items[DrawItem];
        Icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var lbIcon = DialogHosts.IntGfxMod.LBIcons[Icon + 14];
        DialogHosts.IntGfxMod.LBIcons[Icon + 14].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
    }

    // AoWRealm.pas:592
    public void OverWindowResize()
    {
        this.Dlg.Alignment.AlignWidth = AOWAlignWidth.Left;
        this.Dlg.Alignment.AlignHeight = AOWAlignHeight.Top;
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
    public void T3NameChange(AOWListBox Sender)
    {
        AOWListBox List;
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
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:649
    public void T2SRaceClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:668
    public void T2SInClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 2);
    }

    // AoWRealm.pas:687
    public void T2SLvlClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:706
    public void T2SWallClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:725
    public void T2SSizeClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:744
    public void T2SRelClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:763
    public void T2SMorClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:782
    public void T2SActClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:801
    public void T2SEncClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
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
        DialogHosts.General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:820
    public void T3SNameClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T3Name);
        Lists.Add(T3ActLB);
        DialogHosts.General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:831
    public void HeroListChange(AOWListBox Sender)
    {
        AOWListBox List;
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
    public void T5HeroInfDraw(SurfaceDesc SurfDesc)
    {
        int Index;
        Hero Hero;
        Rect R;
        Index = T5Name.Selected;
        if (Index > -1)
        {
            Hero = (Hero)T5Health.Items.Objects[Index << 0];
            R = new Rect(T5HeroInf.WinLeft + 1, T5HeroInf.WinTop + 1, T5HeroInf.WinLeft + T5HeroInf.WinWidth - 1, T5HeroInf.WinTop + T5HeroInf.WinHeight - 1);
            if (Hero.GetFace() != null)
            {
                Hero.ShowFace(R, SurfDesc);
            }
        }
    }

    // AoWRealm.pas:866
    public void Unit1LBDoubleClick(AOWListBox Sender, int Slot)
    {
        if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetCount() > 0)
        {
            var unit = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetUnit(Slot);
            DialogHosts.General.FSelectedUnit = unit;
            DialogHosts.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:875
    public void T5SNameClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T5Name);
        Lists.Add(T5Health);
        Lists.Add(T5Level);
        DialogHosts.General.SortLists(ref Lists, 0);
    }

    // AoWRealm.pas:887
    public void T5SHealthClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T5Health);
        Lists.Add(T5Name);
        Lists.Add(T5Level);
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:899
    public void T5SLevelClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T5Level);
        Lists.Add(T5Name);
        Lists.Add(T5Health);
        DialogHosts.General.SortLists(ref Lists, 2);
    }

    // AoWRealm.pas:911
    public void GotoButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWRealm.pas:916
    public void T4PnlShow()
    {
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex).AddRef();
            }
        }
    }

    // AoWRealm.pas:926
    public void T4PnlHide()
    {
        for (int I = 0, last = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(World.Map.AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex).Release();
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
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T3ActLB);
        Lists.Add(T3Name);
        DialogHosts.General.SortLists(ref Lists, 1);
    }

    // AoWRealm.pas:962
    public void UpkeepSBChange()
    {
        UpkeepValue.ListOffset = UpkeepSB.Position;
    }

    // AoWRealm.pas:967
    public void DlgShow()
    {
        DialogHosts.General.RegisterDialog(Dlg);
    }

    // AoWRealm.pas:972
    public void DlgHide()
    {
        DialogHosts.General.UnregisterDialog(Dlg);
    }

    // AoWRealm.pas:977
    public void HelpClick()
    {
        HelpWindow.ShowHelp(505);
    }

    private static readonly string BuildersGuildsRStr = "Builder Guilds";
    private static readonly string CitiesRStr = "Cities";
    private static readonly string ExternalIncomeRStr = "External Income";
    private static readonly string FarmsRStr = "Farms";
    private static readonly string MinesRStr = "Mines";

    private static int BuildersGuildIncome(sbyte Player)
    {
        int num;
        if (World.Map.AoWHSMap.Current != null)
        {
            Player players = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
            num = 0;
            int i = 0;
            for (int num2 = players.FStructures.Count - 1; i <= num2; i++)
            {
                PlayerStructure structure = players.FStructures.GetStructure(i);
                if (structure.EClass.ClassID == 133422 && structure.FPlayer == Player)
                {
                    num += structure.GetIncome();
                }
            }
        }
        else
        {
            num = -1;
        }
        return num;
    }

    private static int CityIncome(sbyte Player)
    {
        int num;
        if (World.Map.AoWHSMap.Current != null)
        {
            Player players = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
            num = 0;
            int i = 0;
            for (int num2 = players.FStructures.Count - 1; i <= num2; i++)
            {
                PlayerStructure structure = players.FStructures.GetStructure(i);
                if (structure.EClass.ClassID == 131336 && structure.FPlayer == Player)
                {
                    num += ((PlayerCropStructure)structure).FIncome;
                }
            }
        }
        else
        {
            num = -1;
        }
        return num;
    }

    private static int FarmIncome(sbyte Player)
    {
        int num;
        if (World.Map.AoWHSMap.Current != null)
        {
            Player players = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
            num = 0;
            int i = 0;
            for (int num2 = players.FStructures.Count - 1; i <= num2; i++)
            {
                PlayerStructure structure = players.FStructures.GetStructure(i);
                if (structure.EClass.ClassID == 131950 && structure.FPlayer == Player)
                {
                    num += ((PlayerCropStructure)structure).FIncome;
                }
            }
        }
        else
        {
            num = -1;
        }
        return num;
    }

    private static int MineIncome(sbyte Player)
    {
        int num;
        if (World.Map.AoWHSMap.Current != null)
        {
            Player players = World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
            num = 0;
            int i = 0;
            for (int num2 = players.FStructures.Count - 1; i <= num2; i++)
            {
                PlayerStructure structure = players.FStructures.GetStructure(i);
                if (structure.EClass.ClassID == 131962 && (sbyte)(byte)structure.FPlayer == Player)
                {
                    num += structure.GetIncome();
                }
            }
        }
        else
        {
            num = -1;
        }
        return num;
    }
}
