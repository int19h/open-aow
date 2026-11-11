// Translated from AoWRealm.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class RealmWindow : DataModule
{
    public RealmWindow(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label OWTitle;
    public Image OWTitleIc;
    public TabPanel HeaderTab;
    public Panel T1Pnl;
    public Panel T2Pnl;
    public Panel T2LBPnl;
    public Panel T3Pnl;
    public Panel T3LBPnl;
    public Button Minimize;
    public Panel T4Pnl;
    public Panel T4LBPnl;
    public ListBox T2NameLB;
    public ListBox T2RaceLB;
    public ListBox T2IncomeLB;
    public ListBox T2LevelLB;
    public ListBox T2WallLB;
    public ListBox T2SizeLB;
    public ListBox T2EncLB;
    public Button T2SName;
    public Button T2SRace;
    public Button T2SIn;
    public Button T2SWall;
    public Button T2SLvl;
    public Button T2SSize;
    public Button T2SEnc;
    public ListBox T2ActLB;
    public Button T2SAct;
    public ListBox T2RelLB;
    public Button T2SRel;
    public ListBox T2MorLB;
    public Button T2SMor;
    public VerticalScrollBar CitySB;
    public ListBox T3Name;
    public Button T3SName;
    public ListBox T3ActLB;
    public Button T3Activity;
    public VerticalScrollBar PlaceSB;
    public Frame UnitFr;
    public Gauge UnitGg;
    public ListBox Unit1LB;
    public ListBox Unit2LB;
    public ListBox Unit3LB;
    public ListBox Unit4LB;
    public ListBox Unit5LB;
    public ListBox Unit6LB;
    public ListBox Unit7LB;
    public ListBox Unit8LB;
    private ListBox[] partyLists;
    public VerticalScrollBar PartySb;
    public Panel HeroPnl;
    public Panel T5MainPnl;
    public ListBox T5Name;
    public ListBox T5Health;
    public ListBox T5Level;
    public Button T5SName;
    public Button T5SHealth;
    public Button T5SLevel;
    public Panel T5HeroInf;
    public Panel T5LBFr;
    public Button HInfoBtn;
    public VerticalScrollBar HeroSB;
    public Panel TotalPnl;
    public Panel IncomePnl;
    public Memo IncomeMem;
    public Panel IncomeBtm;
    public VerticalScrollBar IncomeSB;
    public Label IncomeLbl;
    public Label Income;
    public Panel UpkeepPnl;
    public Memo UpkeepMem;
    public Panel UpkeepBtm;
    public VerticalScrollBar UpkeepSB;
    public Label UpkeepLbl;
    public Label Upkeep;
    public Label Totallbl;
    public Label NetInc;
    public Panel TotalBtm;
    public Label TreasuryLbl;
    public Label Treasury;
    public Panel TreasuryPnl;
    public Memo IncomeValue;
    public Memo UpkeepValue;
    public Image GoldAnim;
    public Panel PicPnl;
    public Button Help;
    public IvTranslator Translator;

    // AoWRealm.pas:238
    public void UpdateIncome()
    {
        int Value;
        var playerGems = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        Treasury.Text = IntToStr(playerGems.FGems);
        var upkeep = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetUpkeep();
        var playerIncome = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        NetInc.Text = IntToStr(playerIncome.FIncome - upkeep);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        Income.Text = IntToStr(player.FIncome);
        var upkeep2 = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetUpkeep();
        Upkeep.Text = IntToStr(upkeep2);
        IncomeMem.Items.Clear();
        IncomeValue.Items.Clear();
        UpkeepValue.Items.Clear();
        Value = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FBaseIncome;
        if (Value != 0)
        {
            var externalIncome = AbilityRules.TranslateRStr(ExternalIncomeRStr);
            IncomeMem.Items.Add(externalIncome);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = CityIncome(AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var cities = AbilityRules.TranslateRStr(CitiesRStr);
            IncomeMem.Items.Add(cities);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = FarmIncome(AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var farms = AbilityRules.TranslateRStr(FarmsRStr);
            IncomeMem.Items.Add(farms);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = MineIncome(AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var mines = AbilityRules.TranslateRStr(MinesRStr);
            IncomeMem.Items.Add(mines);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        Value = BuildersGuildIncome(AoWHSMap.Current.FSeatedPlayer);
        if (Value != 0)
        {
            var buildersGuilds = AbilityRules.TranslateRStr(BuildersGuildsRStr);
            IncomeMem.Items.Add(buildersGuilds);
            IncomeValue.Items.Add(IntToStr(Value));
        }

        var playerArmyUpkeep = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
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
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Structure = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(I);
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

                switch ((int)AoWHSMap.Current.FRaces.GetRace(C.FRace).GetPlayerRelation(AoWHSMap.Current.FSeatedPlayer) & 127)
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
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1; I <= last; I++)
        {
            Place = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(I);
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
        Troop U;
        T5Name.Items.Clear();
        T5Health.Items.Clear();
        T5Level.Items.Clear();
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                U = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J);
                if (U is Hero)
                {
                    var army = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I);
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
        if (AoWHSMap.Current.FSeatedPlayer == -1)
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
    public void DrawArmyUnit(int Column, in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
    {
        Troop U;
        Rect OldClip;
        if (Column < AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetCount())
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

            U = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(DrawItem).GetUnit(Column);
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
    public void T2WallLBDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
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
    public void Unit1LBDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc, int Slot)
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
    public void Unit1LBChange(ListBox ListBox, int Slot)
    {
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetCount() > 0)
        {
            ListBox.PopupMenu = DialogHosts.UnitBanner.BannerPopup;
            var unit = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetUnit(Slot);
            DialogHosts.General.FSelectedUnit = unit;
            var armyHS = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(ListBox.Selected).GetArmyHS();
            AoWHSMap.Current.Select(armyHS, true);
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
    public void T2MorLBDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
    {
        int Icon;
        string text = T2MorLB.Items[DrawItem];
        Icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var lbIcon = DialogHosts.IntGfxMod.LBIcons[Icon + 9];
        DialogHosts.IntGfxMod.LBIcons[Icon + 9].ShowClipped(DrawRect.Left + (DrawRect.Right - DrawRect.Left) / 2 - lbIcon.Width / 2, DrawRect.Top, SurfDesc);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(ListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        HexSprite Sprite;
        if (Button == MouseButton.Right && Sender.Selected != -1)
        {
            Sprite = (HexSprite)T2NameLB.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:469
    public void CityListChange(ListBox Sender)
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
    public void CityDoubleClick(ListBox Sender)
    {
        if (Sender.Selected != -1)
        {
            var city = T2NameLB.Items.Objects[Sender.Selected];
            DialogHosts.CityScreen.ShowCity((City)city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(ListBox Sender)
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
                AoWHSMap.Current.Select((HexSprite)hexagonSprite, true);
            }
        }
    }

    // AoWRealm.pas:501
    public void PlacesListMouseDown(ListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        HexSprite Sprite;
        if (Button == MouseButton.Right && Sender.Selected != -1)
        {
            Sprite = (HexSprite)T3Name.Items.Objects[Sender.Selected];
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
        }
    }

    // AoWRealm.pas:513
    public void HeroListDoubleClick(ListBox Sender)
    {
        var listBox = Sender;
        if (listBox.Selected != -1)
        {
            var armyHS = ((Army)T5Name.Items.Objects[listBox.Selected]).GetArmyHS();
            AoWHSMap.Current.Select(armyHS, true);
        }
    }

    // AoWRealm.pas:522
    public void HeroListMouseDown(ListBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        ListBox List;
        HexSprite Sprite;
        List = Sender;
        if (List.Selected != -1)
        {
            Sprite = ((Army)T5Name.Items.Objects[List.Selected]).GetArmyHS();
            var lhx = Sprite.GetLhx();
            var yhx = Sprite.GetYhx();
            var xhx = Sprite.GetXhx();
            AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            var spriteLhx = Sprite.GetLhx();
            var spriteYhx = Sprite.GetYhx();
            var spriteXhx = Sprite.GetXhx();
            AoWHSMap.Current.PlayCenterAnimation(spriteXhx, spriteYhx, spriteLhx);
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
    public void T2ActLBDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
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
    public void T2RelLBDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
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
        this.Dlg.Alignment.AlignWidth = AlignWidth.Left;
        this.Dlg.Alignment.AlignHeight = AlignHeight.Top;
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
    public void T3NameChange(ListBox Sender)
    {
        ListBox List;
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
    public void HeroListChange(ListBox Sender)
    {
        ListBox List;
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
    public void T5HeroInfDraw(LockedSurface SurfDesc)
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
    public void Unit1LBDoubleClick(ListBox Sender, int Slot)
    {
        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetCount() > 0)
        {
            var unit = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(Sender.Selected).GetUnit(Slot);
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
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
                AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(unitGFXResourceIndex).AddRef();
            }
        }
    }

    // AoWRealm.pas:926
    public void T4PnlHide()
    {
        for (int I = 0, last = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGFXResourceIndex = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FArmies.GetArmy(I).GetUnit(J).GetUnitGFXResourceIndex();
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
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
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
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
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
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
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
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
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
