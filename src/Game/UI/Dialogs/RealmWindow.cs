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

    public Window Window;
    public TabPanel HeaderTabs;
    public Panel CityPanel;
    public Panel CityListPanel;
    public Panel PartyPanel;
    public Panel PartyListPanel;
    public ListBox CityNameList;
    public ListBox CityRaceList;
    public ListBox CityIncomeList;
    public ListBox CityLevelList;
    public ListBox CityWallList;
    public ListBox CitySizeList;
    public ListBox CityEnchantmentList;
    public ListBox CityActionList;
    public ListBox CityRelationList;
    public ListBox CityMoraleList;
    public VerticalScrollBar CityScrollBar;
    public ListBox PlaceNameList;
    public ListBox PlaceActionList;
    public VerticalScrollBar PlaceScrollBar;
    public Gauge UnitGauge;
    public ListBox Unit1List;
    public ListBox Unit2List;
    public ListBox Unit3List;
    public ListBox Unit4List;
    public ListBox Unit5List;
    public ListBox Unit6List;
    public ListBox Unit7List;
    public ListBox Unit8List;
    private ListBox[] _partyLists;
    public VerticalScrollBar PartyScrollBar;
    public ListBox HeroNameList;
    public ListBox HeroHealthList;
    public ListBox HeroLevelList;
    public Panel HeroHeroInfoPanel;
    public VerticalScrollBar HeroScrollBar;
    public Memo IncomeMemo;
    public VerticalScrollBar IncomeScrollBar;
    public Label IncomeLabel;
    public VerticalScrollBar UpkeepScrollBar;
    public Label UpkeepLabel;
    public Label NetIncomeLabel;
    public Label TreasuryLabel;
    public Memo IncomeValueMemo;
    public Memo UpkeepValueMemo;

    // AoWRealm.pas:238
    public void UpdateIncome()
    {
        int value;
        var playerGems = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        TreasuryLabel.Text = IntToStr(playerGems.Gems);
        var upkeep = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetUpkeep();
        var playerIncome = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        NetIncomeLabel.Text = IntToStr(playerIncome.Income - upkeep);
        var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        IncomeLabel.Text = IntToStr(player.Income);
        var upkeep2 = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetUpkeep();
        UpkeepLabel.Text = IntToStr(upkeep2);
        IncomeMemo.Items.Clear();
        IncomeValueMemo.Items.Clear();
        UpkeepValueMemo.Items.Clear();
        value = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).BaseIncome;
        if (value != 0)
        {
            var externalIncome = AbilityRules.TranslateResourceString(s_externalIncomeText);
            IncomeMemo.Items.Add(externalIncome);
            IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = CityIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var cities = AbilityRules.TranslateResourceString(s_citiesText);
            IncomeMemo.Items.Add(cities);
            IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = FarmIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var farms = AbilityRules.TranslateResourceString(s_farmsText);
            IncomeMemo.Items.Add(farms);
            IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = MineIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var mines = AbilityRules.TranslateResourceString(s_minesText);
            IncomeMemo.Items.Add(mines);
            IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = BuildersGuildIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var buildersGuilds = AbilityRules.TranslateResourceString(s_buildersGuildsText);
            IncomeMemo.Items.Add(buildersGuilds);
            IncomeValueMemo.Items.Add(IntToStr(value));
        }

        var playerArmyUpkeep = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        UpkeepValueMemo.Items.Add(IntToStr(playerArmyUpkeep.ArmyUpkeep));
    }

    // AoWRealm.pas:178
    public void UpdateCities()
    {
        PlayerStructure structure;
        City C;
        CityNameList.Items.Clear();
        CityRaceList.Items.Clear();
        CityIncomeList.Items.Clear();
        CityLevelList.Items.Clear();
        CityWallList.Items.Clear();
        CitySizeList.Items.Clear();
        CityMoraleList.Items.Clear();
        CityRelationList.Items.Clear();
        CityActionList.Items.Clear();
        CityEnchantmentList.Items.Clear();
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1; I <= last; I++)
        {
            structure = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(I);
            if (structure is City)
            {
                C = (City)structure;
                var name = C.GetName();
                CityNameList.Items.AddObject(name, C);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[C.Race]);
                CityRaceList.Items.Add(raceNamesPlural);
                CityIncomeList.Items.Add(IntToStr(C.Income));
                CityLevelList.Items.Add(IntToStr(C.UpgradeLevel));
                switch (C.WallType)
                {
                    case 1:
                        CityWallList.Items.Add("1");
                        break;
                    case 2:
                        CityWallList.Items.Add("2");
                        break;
                    default:
                        CityWallList.Items.Add("-");
                        break;
                }

                switch (C.GetCivilStatus())
                {
                    case City.CivilStatus.Unrest:
                        CityMoraleList.Items.Add("1");
                        break;
                    case City.CivilStatus.Stable:
                        CityMoraleList.Items.Add("2");
                        break;
                    case City.CivilStatus.Content:
                        CityMoraleList.Items.Add("3");
                        break;
                    case City.CivilStatus.Cheerful:
                        CityMoraleList.Items.Add("4");
                        break;
                    default:
                        CityMoraleList.Items.Add("-");
                        break;
                }

                switch ((int)AoWHSMap.Current.Races.GetRace(C.Race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer) & 127)
                {
                    case 0:
                        CityRelationList.Items.Add("0");
                        break;
                    case 1:
                        CityRelationList.Items.Add("1");
                        break;
                    case 2:
                        CityRelationList.Items.Add("2");
                        break;
                    case 3:
                        CityRelationList.Items.Add("3");
                        break;
                    case 4:
                        CityRelationList.Items.Add("4");
                        break;
                    default:
                        CityRelationList.Items.Add("-");
                        break;
                }

                var activeOrder = C.GetActiveOrder();
                CityActionList.Items.Add(activeOrder != null ? activeOrder.RealmActivity(C) : "-");

                CityEnchantmentList.Items.Add("-");
                var size = C.GetSize();
                CitySizeList.Items.Add(IntToStr(size));
            }
        }
    }

    // AoWRealm.pas:309
    public void UpdatePlaces()
    {
        PlayerStructure place;
        PlaceNameList.Items.Clear();
        PlaceActionList.Items.Clear();
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1; I <= last; I++)
        {
            place = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(I);
            if (!(place is City))
            {
                var editName = place.EditName();
                PlaceNameList.Items.AddObject(editName, place);
                var currentActivityText = place.GetCurrentActivityText();
                PlaceActionList.Items.Add(currentActivityText);
            }
        }
    }

    // AoWRealm.pas:283
    public void UpdateHeroes()
    {
        Troop U;
        HeroNameList.Items.Clear();
        HeroHealthList.Items.Clear();
        HeroLevelList.Items.Clear();
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                U = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetUnit(J);
                if (U is Hero)
                {
                    var army = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I);
                    var name = U.GetName();
                    HeroNameList.Items.AddObject(name, army);
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    HeroHealthList.Items.AddObject(IntToStr(hitPoints) + "/" + IntToStr(hits), U);
                    var level = ((Hero)U).GetLevel();
                    HeroLevelList.Items.Add(IntToStr(level));
                }
            }
        }

        if (HeroNameList.Items.Count > 0 && HeroNameList.Selected == -1)
        {
            HeroNameList.Selected = 0;
            HeroListChange(HeroNameList);
        }
    }

    // AoWRealm.pas:327
    public void UpdateOther()
    {
    }

    // AoWRealm.pas:331
    public void UpdateOverview()
    {
        if (AoWHSMap.Current.SeatedPlayer == -1)
        {
            Window.Visible = false;
        }
        else
        {
            switch (HeaderTabs.Selected)
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
    public void DrawArmyUnit(int column, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        Troop U;
        Rect oldClip;
        if (column < AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(drawItem).GetCount())
        {
            oldClip = lockedSurface.ClipRectangle;
            lockedSurface.ClipRectangle.Left = drawRectangle.Left + 2;
            lockedSurface.ClipRectangle.Top = drawRectangle.Top + 1;
            lockedSurface.ClipRectangle.Right = drawRectangle.Right - 2;
            lockedSurface.ClipRectangle.Bottom = drawRectangle.Top + Unit1List.ItemHeight - 6;
            if (drawRectangle.Bottom - drawRectangle.Top < Unit1List.ItemHeight - 6)
            {
                lockedSurface.ClipRectangle.Bottom = drawRectangle.Bottom;
            }

            U = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(drawItem).GetUnit(column);
            if ((int)U.GetAlignment() < DialogHosts.UIGraphics.UnitBackgrounds.Count)
            {
                var alignment = U.GetAlignment();
                var unitBackground = DialogHosts.UIGraphics.UnitBackgrounds[(int)alignment];
                unitBackground.ShowClipped(drawRectangle.Left - (unitBackground.Width - (drawRectangle.Right - drawRectangle.Left)) / 2, drawRectangle.Top - (unitBackground.Height - (drawRectangle.Bottom - drawRectangle.Top - 10)) / 2, lockedSurface);
            }

            var previewImage = U.GetPreviewImage();
            var uPreviewImage = U.GetPreviewImage();
            U.Show(drawRectangle.Left - (uPreviewImage.Width - (drawRectangle.Right - drawRectangle.Left)) / 2, drawRectangle.Top - (previewImage.Height - (Unit1List.ItemHeight - 10)) / 2, lockedSurface, DialogHosts.General.UnitShowInfo);
            var gauge = UnitGauge;
            gauge.Visible = true;
            gauge.Max = U.GetHits();
            gauge.Position = U.GetHitPoints();
            gauge.WinLeft = drawRectangle.Left - PartyListPanel.WinLeft + 5;
            gauge.WinWidth = drawRectangle.Right - drawRectangle.Left - 10;
            gauge.WinTop = drawRectangle.Top + Unit1List.ItemHeight - PartyListPanel.WinTop - 9;
            // { Original sizes and draws the gauge directly. }
            gauge.SetSize();
            gauge.Draw(lockedSurface);
            gauge.Visible = false;

            lockedSurface.ClipRectangle = oldClip;
        }
    }

    // AoWRealm.pas:382
    public void OWMinIconClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWRealm.pas:388
    public void T2WallLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        string text = CityWallList.Items[drawItem];
        switch (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]))
        {
            case 49:
                var listBoxIcon = DialogHosts.UIGraphics.ListIcons[7];
                DialogHosts.UIGraphics.ListIcons[7].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
                break;
            case 50:
                var listBoxIconWidth = DialogHosts.UIGraphics.ListIcons[8];
                DialogHosts.UIGraphics.ListIcons[8].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIconWidth.Width / 2, drawRectangle.Top, lockedSurface);
                break;
        }
    }

    // AoWRealm.pas:398
    public void Unit1LBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface, int slot)
    {
        DrawArmyUnit(slot, drawRectangle, drawItem, ref lockedSurface);
    }

    // AoWRealm.pas:403
    public void HeaderTabChange()
    {
        DialogHosts.General.PlayButtonSound(0);
        UpdateOverview();
    }

    // AoWRealm.pas:409
    public void PartyScrollBarChange()
    {
        Unit1List.ListOffset = PartyScrollBar.Position;
        Unit2List.ListOffset = PartyScrollBar.Position;
        Unit3List.ListOffset = PartyScrollBar.Position;
        Unit4List.ListOffset = PartyScrollBar.Position;
        Unit5List.ListOffset = PartyScrollBar.Position;
        Unit6List.ListOffset = PartyScrollBar.Position;
        Unit7List.ListOffset = PartyScrollBar.Position;
        Unit8List.ListOffset = PartyScrollBar.Position;
    }

    // AoWRealm.pas:421
    public void Unit1LBChange(ListBox listBox, int slot)
    {
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetCount() > 0)
        {
            listBox.PopupMenu = DialogHosts.UnitBanner.BannerMenu;
            var troop = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetUnit(slot);
            DialogHosts.General.SelectedUnit = troop;
            var armyHexSprite = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetArmyHexSprite();
            AoWHSMap.Current.Select(armyHexSprite, true);
            Window.Priority = Window.Priority;
        }
        else
        {
            listBox.PopupMenu = null;
            listBox.Selected = -1;
        }

        _partyLists ??= [Unit1List, Unit2List, Unit3List, Unit4List, Unit5List, Unit6List, Unit7List, Unit8List];
        for (int index = 0; index < _partyLists.Length; index++)
        {
            if (index != slot)
            {
                _partyLists[index].Selected = -1;
            }
        }
    }

    // AoWRealm.pas:448
    public void T2MorLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        int icon;
        string text = CityMoraleList.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var listBoxIcon = DialogHosts.UIGraphics.ListIcons[icon + 9];
        DialogHosts.UIGraphics.ListIcons[icon + 9].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        HexSprite sprite;
        if (button == MouseButton.Right && sender.Selected != -1)
        {
            sprite = (HexSprite)CityNameList.Items.Objects[sender.Selected];
            var hexLevel = sprite.GetHexLevel();
            var hexY = sprite.GetHexY();
            var hexX = sprite.GetHexX();
            AoWHSMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            var spriteHexLevel = sprite.GetHexLevel();
            var spriteHexY = sprite.GetHexY();
            var spriteHexX = sprite.GetHexX();
            AoWHSMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:469
    public void CityListChange(ListBox sender)
    {
        var listBox = sender;
        CityNameList.Selected = listBox.Selected;
        CityRaceList.Selected = listBox.Selected;
        CityIncomeList.Selected = listBox.Selected;
        CityLevelList.Selected = listBox.Selected;
        CityWallList.Selected = listBox.Selected;
        CitySizeList.Selected = listBox.Selected;
        CityRelationList.Selected = listBox.Selected;
        CityMoraleList.Selected = listBox.Selected;
        CityActionList.Selected = listBox.Selected;
        CityEnchantmentList.Selected = listBox.Selected;
    }

    // AoWRealm.pas:486
    public void CityDoubleClick(ListBox sender)
    {
        if (sender.Selected != -1)
        {
            var city = CityNameList.Items.Objects[sender.Selected];
            DialogHosts.CityScreen.ShowCity((City)city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(ListBox sender)
    {
        if (sender.Selected != -1)
        {
            if (PlaceNameList.Items.Objects[sender.Selected] is ProductionPlace)
            {
                var productionPlace = PlaceNameList.Items.Objects[sender.Selected];
                DialogHosts.ProductionScreen.ShowPlace((ProductionPlace)productionPlace);
            }
            else
            {
                var hexSprite = PlaceNameList.Items.Objects[sender.Selected];
                AoWHSMap.Current.Select((HexSprite)hexSprite, true);
            }
        }
    }

    // AoWRealm.pas:501
    public void PlacesListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        HexSprite sprite;
        if (button == MouseButton.Right && sender.Selected != -1)
        {
            sprite = (HexSprite)PlaceNameList.Items.Objects[sender.Selected];
            var hexLevel = sprite.GetHexLevel();
            var hexY = sprite.GetHexY();
            var hexX = sprite.GetHexX();
            AoWHSMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            var spriteHexLevel = sprite.GetHexLevel();
            var spriteHexY = sprite.GetHexY();
            var spriteHexX = sprite.GetHexX();
            AoWHSMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:513
    public void HeroListDoubleClick(ListBox sender)
    {
        var listBox = sender;
        if (listBox.Selected != -1)
        {
            var armyHexSprite = ((Army)HeroNameList.Items.Objects[listBox.Selected]).GetArmyHexSprite();
            AoWHSMap.Current.Select(armyHexSprite, true);
        }
    }

    // AoWRealm.pas:522
    public void HeroListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        ListBox list;
        HexSprite sprite;
        list = sender;
        if (list.Selected != -1)
        {
            sprite = ((Army)HeroNameList.Items.Objects[list.Selected]).GetArmyHexSprite();
            var hexLevel = sprite.GetHexLevel();
            var hexY = sprite.GetHexY();
            var hexX = sprite.GetHexX();
            AoWHSMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            var spriteHexLevel = sprite.GetHexLevel();
            var spriteHexY = sprite.GetHexY();
            var spriteHexX = sprite.GetHexX();
            AoWHSMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:536
    public void HeroInfoClick()
    {
        int index;
        DialogHosts.General.PlayButtonSound(0);
        index = HeroNameList.Selected;
        if (index != -1)
        {
            // { evaluate the index before the receiver chain }
            var hero = HeroHealthList.Items.Objects[index << 0];
            DialogHosts.General.SelectedUnit = (Hero)hero;
            DialogHosts.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:549
    public void T2LevelLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:555
    public void T2WallLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:561
    public void T2RaceLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:567
    public void T2ActLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        byte icon;
        string text = CityActionList.Items[drawItem];
        icon = IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]);
        if (icon != 45)
        {
            var listBoxIcon = DialogHosts.UIGraphics.ListIcons[icon - 23];
            DialogHosts.UIGraphics.ListIcons[icon - 23].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
        }
    }

    // AoWRealm.pas:577
    public void T2ActLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:583
    public void T2RelLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        int icon;
        string text = CityRelationList.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var listBoxIcon = DialogHosts.UIGraphics.ListIcons[icon + 14];
        DialogHosts.UIGraphics.ListIcons[icon + 14].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
    }

    // AoWRealm.pas:592
    public void OverWindowResize()
    {
        this.Window.Alignment.AlignWidth = AlignWidth.Left;
        this.Window.Alignment.AlignHeight = AlignHeight.Top;
        this.Window.Alignment.LeftOffset = this.Window.WinLeft;
        this.Window.Alignment.TopOffset = this.Window.WinTop;
    }

    // AoWRealm.pas:603
    public void CityScrollBarChange()
    {
        CityNameList.ListOffset = CityScrollBar.Position;
        CityRaceList.ListOffset = CityScrollBar.Position;
        CityIncomeList.ListOffset = CityScrollBar.Position;
        CityLevelList.ListOffset = CityScrollBar.Position;
        CityWallList.ListOffset = CityScrollBar.Position;
        CitySizeList.ListOffset = CityScrollBar.Position;
        CityRelationList.ListOffset = CityScrollBar.Position;
        CityMoraleList.ListOffset = CityScrollBar.Position;
        CityActionList.ListOffset = CityScrollBar.Position;
    }

    // AoWRealm.pas:616
    public void T3NameChange(ListBox sender)
    {
        ListBox list;
        list = sender;
        PlaceNameList.Selected = list.Selected;
        PlaceActionList.Selected = list.Selected;
    }

    // AoWRealm.pas:625
    public void PlaceScrollBarChange()
    {
        PlaceNameList.ListOffset = PlaceScrollBar.Position;
    }

    // AoWRealm.pas:630
    public void T2SNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:649
    public void T2SRaceClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityRaceList);
        lists.Add(CityNameList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:668
    public void T2SInClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityIncomeList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 2);
    }

    // AoWRealm.pas:687
    public void T2SLvlClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityLevelList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:706
    public void T2SWallClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityWallList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:725
    public void T2SSizeClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CitySizeList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:744
    public void T2SRelClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityRelationList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:763
    public void T2SMorClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityMoraleList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityActionList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:782
    public void T2SActClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityActionList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityEnchantmentList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:801
    public void T2SEncClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(CityEnchantmentList);
        lists.Add(CityNameList);
        lists.Add(CityRaceList);
        lists.Add(CityIncomeList);
        lists.Add(CityLevelList);
        lists.Add(CityWallList);
        lists.Add(CitySizeList);
        lists.Add(CityRelationList);
        lists.Add(CityMoraleList);
        lists.Add(CityActionList);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:820
    public void T3SNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(PlaceNameList);
        lists.Add(PlaceActionList);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:831
    public void HeroListChange(ListBox sender)
    {
        ListBox list;
        list = sender;
        HeroNameList.Selected = list.Selected;
        HeroHealthList.Selected = list.Selected;
        HeroLevelList.Selected = list.Selected;
    }

    // AoWRealm.pas:841
    public void HeroScrollBarChange()
    {
        // { Original bug: the hero scrollbar moves T3Name instead of T5Name. }
        PlaceNameList.ListOffset = HeroScrollBar.Position;
        HeroHealthList.ListOffset = HeroScrollBar.Position;
        HeroLevelList.ListOffset = HeroScrollBar.Position;
    }

    // AoWRealm.pas:849
    public void T5HeroInfDraw(LockedSurface lockedSurface)
    {
        int index;
        Hero hero;
        Rect R;
        index = HeroNameList.Selected;
        if (index > -1)
        {
            hero = (Hero)HeroHealthList.Items.Objects[index << 0];
            R = new Rect(HeroHeroInfoPanel.WinLeft + 1, HeroHeroInfoPanel.WinTop + 1, HeroHeroInfoPanel.WinLeft + HeroHeroInfoPanel.WinWidth - 1, HeroHeroInfoPanel.WinTop + HeroHeroInfoPanel.WinHeight - 1);
            if (hero.GetFace() != null)
            {
                hero.ShowFace(R, lockedSurface);
            }
        }
    }

    // AoWRealm.pas:866
    public void Unit1LBDoubleClick(ListBox sender, int slot)
    {
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(sender.Selected).GetCount() > 0)
        {
            var troop = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(sender.Selected).GetUnit(slot);
            DialogHosts.General.SelectedUnit = troop;
            DialogHosts.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:875
    public void T5SNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(HeroNameList);
        lists.Add(HeroHealthList);
        lists.Add(HeroLevelList);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:887
    public void T5SHealthClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(HeroHealthList);
        lists.Add(HeroNameList);
        lists.Add(HeroLevelList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:899
    public void T5SLevelClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(HeroLevelList);
        lists.Add(HeroNameList);
        lists.Add(HeroHealthList);
        DialogHosts.General.SortLists(ref lists, 2);
    }

    // AoWRealm.pas:911
    public void GotoButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWRealm.pas:916
    public void T4PnlShow()
    {
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGraphicsResourceIndex = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetUnit(J).GetUnitGraphicsResourceIndex();
                AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex).AddRef();
            }
        }
    }

    // AoWRealm.pas:926
    public void T4PnlHide()
    {
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                var unitGraphicsResourceIndex = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetUnit(J).GetUnitGraphicsResourceIndex();
                AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex).Release();
            }
        }
    }

    // AoWRealm.pas:936
    public void OverWindowShow()
    {
        if (PartyPanel.Visible)
        {
            T4PnlShow();
        }
    }

    // AoWRealm.pas:941
    public void OverWindowHide()
    {
        if (PartyPanel.Visible)
        {
            T4PnlHide();
        }
    }

    // AoWRealm.pas:946
    public void IncomeSBChange()
    {
        IncomeValueMemo.ListOffset = IncomeScrollBar.Position;
    }

    // AoWRealm.pas:951
    public void T3ActivityClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(PlaceActionList);
        lists.Add(PlaceNameList);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:962
    public void UpkeepSBChange()
    {
        UpkeepValueMemo.ListOffset = UpkeepScrollBar.Position;
    }

    // AoWRealm.pas:967
    public void DlgShow()
    {
        DialogHosts.General.RegisterDialog(Window);
    }

    // AoWRealm.pas:972
    public void DlgHide()
    {
        DialogHosts.General.UnregisterDialog(Window);
    }

    // AoWRealm.pas:977
    public void HelpClick()
    {
        HelpWindow.ShowHelp(505);
    }

    private static readonly string s_buildersGuildsText = "Builder Guilds";
    private static readonly string s_citiesText = "Cities";
    private static readonly string s_externalIncomeText = "External Income";
    private static readonly string s_farmsText = "Farms";
    private static readonly string s_minesText = "Mines";

    private static int BuildersGuildIncome(sbyte player)
    {
        int number;
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 133422 && structure.Player == player)
                {
                    number += structure.GetIncome();
                }
            }
        }
        else
        {
            number = -1;
        }
        return number;
    }

    private static int CityIncome(sbyte player)
    {
        int number;
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 131336 && structure.Player == player)
                {
                    number += ((PlayerCropStructure)structure).Income;
                }
            }
        }
        else
        {
            number = -1;
        }
        return number;
    }

    private static int FarmIncome(sbyte player)
    {
        int number;
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 131950 && structure.Player == player)
                {
                    number += ((PlayerCropStructure)structure).Income;
                }
            }
        }
        else
        {
            number = -1;
        }
        return number;
    }

    private static int MineIncome(sbyte player)
    {
        int number;
        if (AoWHSMap.Current != null)
        {
            Player players = AoWHSMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 131962 && (sbyte)(byte)structure.Player == player)
                {
                    number += structure.GetIncome();
                }
            }
        }
        else
        {
            number = -1;
        }
        return number;
    }
}
