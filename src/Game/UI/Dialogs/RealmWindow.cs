#nullable disable
// Translated from AoWRealm.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.UI.Dialogs;

public partial class RealmWindow(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public TabPanel HeaderTabs { get; private set; }
    public Panel CityPanel { get; private set; }
    public Panel CityListPanel { get; private set; }
    public Panel PartyPanel { get; private set; }
    public Panel PartyListPanel { get; private set; }
    public ListBox<City> CityNameList { get; private set; }
    public ListBox CityRaceList { get; private set; }
    public ListBox CityIncomeList { get; private set; }
    public ListBox CityLevelList { get; private set; }
    public ListBox CityWallList { get; private set; }
    public ListBox CitySizeList { get; private set; }
    public ListBox CityEnchantmentList { get; private set; }
    public ListBox CityActionList { get; private set; }
    public ListBox CityRelationList { get; private set; }
    public ListBox CityMoraleList { get; private set; }
    public VerticalScrollBar CityScrollBar { get; private set; }
    public ListBox<PlayerStructure> PlaceNameList { get; private set; }
    public ListBox PlaceActionList { get; private set; }
    public VerticalScrollBar PlaceScrollBar { get; private set; }
    public Gauge UnitGauge { get; private set; }
    public ListBox Unit1List { get; private set; }
    public ListBox Unit2List { get; private set; }
    public ListBox Unit3List { get; private set; }
    public ListBox Unit4List { get; private set; }
    public ListBox Unit5List { get; private set; }
    public ListBox Unit6List { get; private set; }
    public ListBox Unit7List { get; private set; }
    public ListBox Unit8List { get; private set; }

    private ListBox[] _partyLists;

    public VerticalScrollBar PartyScrollBar { get; private set; }
    public ListBox<Army> HeroNameList { get; private set; }
    public ListBox<Hero> HeroHealthList { get; private set; }
    public ListBox HeroLevelList { get; private set; }
    public Panel HeroHeroInfoPanel { get; private set; }
    public VerticalScrollBar HeroScrollBar { get; private set; }
    public Memo IncomeMemo { get; private set; }
    public VerticalScrollBar IncomeScrollBar { get; private set; }
    public Label IncomeLabel { get; private set; }
    public VerticalScrollBar UpkeepScrollBar { get; private set; }
    public Label UpkeepLabel { get; private set; }
    public Label NetIncomeLabel { get; private set; }
    public Label TreasuryLabel { get; private set; }
    public Memo IncomeValueMemo { get; private set; }
    public Memo UpkeepValueMemo { get; private set; }

    // AoWRealm.pas:238
    public void UpdateIncome()
    {
        int value;
        Player playerGems = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        TreasuryLabel.Text = IntToStr(playerGems.Gems);
        int upkeep = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetUpkeep();
        Player playerIncome = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        NetIncomeLabel.Text = IntToStr(playerIncome.Income - upkeep);
        Player player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        IncomeLabel.Text = IntToStr(player.Income);
        int upkeep2 = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetUpkeep();
        UpkeepLabel.Text = IntToStr(upkeep2);
        IncomeMemo.Items.Clear();
        IncomeValueMemo.Items.Clear();
        UpkeepValueMemo.Items.Clear();
        value = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).BaseIncome;
        if (value != 0)
        {
            string externalIncome = UI.Engine.TranslateResourceString(s_externalIncomeText);
            _ = IncomeMemo.Items.Add(externalIncome);
            _ = IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = CityIncome(StrategicMap.Current.SeatedPlayer);
        if (value != 0)
        {
            string cities = UI.Engine.TranslateResourceString(s_citiesText);
            _ = IncomeMemo.Items.Add(cities);
            _ = IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = FarmIncome(StrategicMap.Current.SeatedPlayer);
        if (value != 0)
        {
            string farms = UI.Engine.TranslateResourceString(s_farmsText);
            _ = IncomeMemo.Items.Add(farms);
            _ = IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = MineIncome(StrategicMap.Current.SeatedPlayer);
        if (value != 0)
        {
            string mines = UI.Engine.TranslateResourceString(s_minesText);
            _ = IncomeMemo.Items.Add(mines);
            _ = IncomeValueMemo.Items.Add(IntToStr(value));
        }

        value = BuildersGuildIncome(StrategicMap.Current.SeatedPlayer);
        if (value != 0)
        {
            string buildersGuilds = UI.Engine.TranslateResourceString(s_buildersGuildsText);
            _ = IncomeMemo.Items.Add(buildersGuilds);
            _ = IncomeValueMemo.Items.Add(IntToStr(value));
        }

        Player playerArmyUpkeep = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        _ = UpkeepValueMemo.Items.Add(IntToStr(playerArmyUpkeep.ArmyUpkeep));
    }

    // AoWRealm.pas:178
    public void UpdateCities()
    {
        PlayerStructure structure;
        City c;
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
        for (int i = 0, last = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1; i <= last; i++)
        {
            structure = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(i);
            if (structure is City found)
            {
                c = found;
                string name = c.GetName();
                _ = CityNameList.Items.AddObject(name, c);
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[c.Race]);
                _ = CityRaceList.Items.Add(raceNamesPlural);
                _ = CityIncomeList.Items.Add(IntToStr(c.Income));
                _ = CityLevelList.Items.Add(IntToStr(c.UpgradeLevel));
                _ = c.WallType switch
                {
                    1 => CityWallList.Items.Add("1"),
                    2 => CityWallList.Items.Add("2"),
                    _ => CityWallList.Items.Add("-"),
                };
                _ = c.GetCivilStatus() switch
                {
                    City.CivilStatus.Unrest => CityMoraleList.Items.Add("1"),
                    City.CivilStatus.Stable => CityMoraleList.Items.Add("2"),
                    City.CivilStatus.Content => CityMoraleList.Items.Add("3"),
                    City.CivilStatus.Cheerful => CityMoraleList.Items.Add("4"),
                    City.CivilStatus.Unruly or City.CivilStatus.Oppressed or City.CivilStatus.Slavery or _ => CityMoraleList.Items.Add("-"),
                };
                _ = ((int)StrategicMap.Current.Races.GetRace(c.Race).GetPlayerRelation(StrategicMap.Current.SeatedPlayer) & 127) switch
                {
                    0 => CityRelationList.Items.Add("0"),
                    1 => CityRelationList.Items.Add("1"),
                    2 => CityRelationList.Items.Add("2"),
                    3 => CityRelationList.Items.Add("3"),
                    4 => CityRelationList.Items.Add("4"),
                    _ => CityRelationList.Items.Add("-"),
                };
                ProductionOrder activeOrder = c.GetActiveOrder();
                _ = CityActionList.Items.Add(activeOrder != null ? activeOrder.RealmActivity(c) : "-");

                _ = CityEnchantmentList.Items.Add("-");
                int size = c.GetSize();
                _ = CitySizeList.Items.Add(IntToStr(size));
            }
        }
    }

    // AoWRealm.pas:309
    public void UpdatePlaces()
    {
        PlayerStructure place;
        PlaceNameList.Items.Clear();
        PlaceActionList.Items.Clear();
        for (int i = 0, last = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1; i <= last; i++)
        {
            place = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(i);
            if (place is not City)
            {
                string editName = place.EditName();
                _ = PlaceNameList.Items.AddObject(editName, place);
                string currentActivityText = place.GetCurrentActivityText();
                _ = PlaceActionList.Items.Add(currentActivityText);
            }
        }
    }

    // AoWRealm.pas:283
    public void UpdateHeroes()
    {
        Troop u;
        HeroNameList.Items.Clear();
        HeroHealthList.Items.Clear();
        HeroLevelList.Items.Clear();
        for (int i = 0, last = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetCount() - 1; i <= last; i++)
        {
            for (int j = 0, lastJ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.Count - 1; j <= lastJ; j++)
            {
                u = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.GetUnit(j);
                if (u is Hero hero)
                {
                    Army army = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i);
                    string name = u.GetName();
                    _ = HeroNameList.Items.AddObject(name, army);
                    sbyte hitPoints = u.GetHitPoints();
                    sbyte hits = u.GetHits();
                    _ = HeroHealthList.Items.AddObject(IntToStr(hitPoints) + "/" + IntToStr(hits), hero);
                    sbyte level = hero.GetLevel();
                    _ = HeroLevelList.Items.Add(IntToStr(level));
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
        if (StrategicMap.Current.SeatedPlayer == -1)
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
                default:
                    break;
            }
        }
    }

    // AoWRealm.pas:345
    public void DrawArmyUnit(int column, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        Troop u;
        Rect oldClip;
        if (column < StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(drawItem).Units.Count)
        {
            oldClip = lockedSurface.ClipRectangle;
            Rect clipRectangle = lockedSurface.ClipRectangle;
            clipRectangle.Left = drawRectangle.Left + 2;
            lockedSurface.ClipRectangle = clipRectangle;
            Rect clipRectangle2 = lockedSurface.ClipRectangle;
            clipRectangle2.Top = drawRectangle.Top + 1;
            lockedSurface.ClipRectangle = clipRectangle2;
            Rect clipRectangle3 = lockedSurface.ClipRectangle;
            clipRectangle3.Right = drawRectangle.Right - 2;
            lockedSurface.ClipRectangle = clipRectangle3;
            Rect clipRectangle4 = lockedSurface.ClipRectangle;
            clipRectangle4.Bottom = drawRectangle.Top + Unit1List.ItemHeight - 6;
            lockedSurface.ClipRectangle = clipRectangle4;
            if (drawRectangle.Bottom - drawRectangle.Top < Unit1List.ItemHeight - 6)
            {
                Rect clipRectangle5 = lockedSurface.ClipRectangle;
                clipRectangle5.Bottom = drawRectangle.Bottom;
                lockedSurface.ClipRectangle = clipRectangle5;
            }

            u = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(drawItem).Units.GetUnit(column);
            if ((int)u.GetAlignment() < UI.Graphics.UnitBackgrounds.Count)
            {
                Units.Alignment alignment = u.GetAlignment();
                LibraryImage unitBackground = UI.Graphics.UnitBackgrounds[(int)alignment];
                unitBackground.ShowClipped(drawRectangle.Left - ((unitBackground.Width - (drawRectangle.Right - drawRectangle.Left)) / 2), drawRectangle.Top - ((unitBackground.Height - (drawRectangle.Bottom - drawRectangle.Top - 10)) / 2), lockedSurface);
            }

            LibraryImage previewImage = u.GetPreviewImage();
            LibraryImage uPreviewImage = u.GetPreviewImage();
            _ = u.Show(drawRectangle.Left - ((uPreviewImage.Width - (drawRectangle.Right - drawRectangle.Left)) / 2), drawRectangle.Top - ((previewImage.Height - (Unit1List.ItemHeight - 10)) / 2), lockedSurface, UI.General.UnitShowInfo);
            Gauge gauge = UnitGauge;
            gauge.Visible = true;
            gauge.Max = u.GetHits();
            gauge.Position = u.GetHitPoints();
            gauge.WinLeft = drawRectangle.Left - PartyListPanel.AbsoluteLeft + 5;
            gauge.WinWidth = drawRectangle.Right - drawRectangle.Left - 10;
            gauge.WinTop = drawRectangle.Top + Unit1List.ItemHeight - PartyListPanel.AbsoluteTop - 9;
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
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWRealm.pas:388
    public void T2WallLBDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        string text = CityWallList.Items[drawItem];
        switch (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]))
        {
            case 49:
                LibraryImage listBoxIcon = UI.Graphics.ListIcons[7];
                UI.Graphics.ListIcons[7].ShowClipped(drawRectangle.Left + ((drawRectangle.Right - drawRectangle.Left) / 2) - (listBoxIcon.Width / 2), drawRectangle.Top, lockedSurface);
                break;
            case 50:
                LibraryImage listBoxIconWidth = UI.Graphics.ListIcons[8];
                UI.Graphics.ListIcons[8].ShowClipped(drawRectangle.Left + ((drawRectangle.Right - drawRectangle.Left) / 2) - (listBoxIconWidth.Width / 2), drawRectangle.Top, lockedSurface);
                break;
            default:
                break;
        }
    }

    // AoWRealm.pas:398
    public void Unit1LBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface, int slot) => DrawArmyUnit(slot, drawRectangle, drawItem, ref lockedSurface);

    // AoWRealm.pas:403
    public void HeaderTabChange()
    {
        UI.General.PlayButtonSound(0);
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
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).Units.Count > 0)
        {
            listBox.PopupMenu = UI.UnitBanner.BannerMenu;
            Troop troop = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).Units.GetUnit(slot);
            UI.General.SelectedUnit = troop;
            ArmyHexSprite armyHexSprite = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetArmyHexSprite();
            StrategicMap.Current.Select(armyHexSprite, true);
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
    public void T2MorLBDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        int icon;
        string text = CityMoraleList.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        LibraryImage listBoxIcon = UI.Graphics.ListIcons[icon + 9];
        UI.Graphics.ListIcons[icon + 9].ShowClipped(drawRectangle.Left + ((drawRectangle.Right - drawRectangle.Left) / 2) - (listBoxIcon.Width / 2), drawRectangle.Top, lockedSurface);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        HexSprite sprite;
        if (button == MouseButton.Right && sender.Selected != -1)
        {
            sprite = CityNameList.Items.Objects[sender.Selected];
            sbyte hexLevel = sprite.GetHexLevel();
            sbyte hexY = sprite.GetHexY();
            sbyte hexX = sprite.GetHexX();
            StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            sbyte spriteHexLevel = sprite.GetHexLevel();
            sbyte spriteHexY = sprite.GetHexY();
            sbyte spriteHexX = sprite.GetHexX();
            StrategicMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:469
    public void CityListChange(ListBox sender)
    {
        ListBox listBox = sender;
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
            City city = CityNameList.Items.Objects[sender.Selected];
            UI.CityScreen.ShowCity(city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(ListBox sender)
    {
        if (sender.Selected != -1)
        {
            if (PlaceNameList.Items.Objects[sender.Selected] is ProductionPlace productionPlace)
            {
                UI.ProductionScreen.ShowPlace(productionPlace);
            }
            else
            {
                PlayerStructure hexSprite = PlaceNameList.Items.Objects[sender.Selected];
                StrategicMap.Current.Select(hexSprite, true);
            }
        }
    }

    // AoWRealm.pas:501
    public void PlacesListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        HexSprite sprite;
        if (button == MouseButton.Right && sender.Selected != -1)
        {
            sprite = PlaceNameList.Items.Objects[sender.Selected];
            sbyte hexLevel = sprite.GetHexLevel();
            sbyte hexY = sprite.GetHexY();
            sbyte hexX = sprite.GetHexX();
            StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            sbyte spriteHexLevel = sprite.GetHexLevel();
            sbyte spriteHexY = sprite.GetHexY();
            sbyte spriteHexX = sprite.GetHexX();
            StrategicMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:513
    public void HeroListDoubleClick(ListBox sender)
    {
        ListBox listBox = sender;
        if (listBox.Selected != -1)
        {
            ArmyHexSprite armyHexSprite = HeroNameList.Items.Objects[listBox.Selected].GetArmyHexSprite();
            StrategicMap.Current.Select(armyHexSprite, true);
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
            sprite = HeroNameList.Items.Objects[list.Selected].GetArmyHexSprite();
            sbyte hexLevel = sprite.GetHexLevel();
            sbyte hexY = sprite.GetHexY();
            sbyte hexX = sprite.GetHexX();
            StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            sbyte spriteHexLevel = sprite.GetHexLevel();
            sbyte spriteHexY = sprite.GetHexY();
            sbyte spriteHexX = sprite.GetHexX();
            StrategicMap.Current.PlayCenterAnimation(spriteHexX, spriteHexY, spriteHexLevel);
        }
    }

    // AoWRealm.pas:536
    public void HeroInfoClick()
    {
        int index;
        UI.General.PlayButtonSound(0);
        index = HeroNameList.Selected;
        if (index != -1)
        {
            // { evaluate the index before the receiver chain }
            Hero hero = HeroHealthList.Items.Objects[index << 0];
            UI.General.SelectedUnit = hero;
            UI.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:549
    public void T2LevelLBDoubleClick()
    {
        UI.RealmWindow.UpdateOverview();
        UI.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:555
    public void T2WallLBDoubleClick()
    {
        UI.RealmWindow.UpdateOverview();
        UI.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:561
    public void T2RaceLBDoubleClick()
    {
        UI.RealmWindow.UpdateOverview();
        UI.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:567
    public void T2ActLBDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        byte icon;
        string text = CityActionList.Items[drawItem];
        icon = IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]);
        if (icon != 45)
        {
            LibraryImage listBoxIcon = UI.Graphics.ListIcons[icon - 23];
            UI.Graphics.ListIcons[icon - 23].ShowClipped(drawRectangle.Left + ((drawRectangle.Right - drawRectangle.Left) / 2) - (listBoxIcon.Width / 2), drawRectangle.Top, lockedSurface);
        }
    }

    // AoWRealm.pas:577
    public void T2ActLBDoubleClick()
    {
        UI.RealmWindow.UpdateOverview();
        UI.RealmWindow.Window.Visible = true;
    }

    // AoWRealm.pas:583
    public void T2RelLBDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        int icon;
        string text = CityRelationList.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        LibraryImage listBoxIcon = UI.Graphics.ListIcons[icon + 14];
        UI.Graphics.ListIcons[icon + 14].ShowClipped(drawRectangle.Left + ((drawRectangle.Right - drawRectangle.Left) / 2) - (listBoxIcon.Width / 2), drawRectangle.Top, lockedSurface);
    }

    // AoWRealm.pas:592
    public void OverWindowResize()
    {
        Window.Alignment.AlignWidth = AlignWidth.Left;
        Window.Alignment.AlignHeight = AlignHeight.Top;
        Window.Alignment.LeftOffset = Window.WinLeft;
        Window.Alignment.TopOffset = Window.WinTop;
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
    public void PlaceScrollBarChange() => PlaceNameList.ListOffset = PlaceScrollBar.Position;

    // AoWRealm.pas:630
    public void T2SNameClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 0);
    }

    // AoWRealm.pas:649
    public void T2SRaceClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 0);
    }

    // AoWRealm.pas:668
    public void T2SInClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 2);
    }

    // AoWRealm.pas:687
    public void T2SLvlClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:706
    public void T2SWallClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityWallList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:725
    public void T2SSizeClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:744
    public void T2SRelClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:763
    public void T2SMorClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:782
    public void T2SActClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityActionList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityEnchantmentList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:801
    public void T2SEncClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(CityEnchantmentList);
        _ = lists.Add(CityNameList);
        _ = lists.Add(CityRaceList);
        _ = lists.Add(CityIncomeList);
        _ = lists.Add(CityLevelList);
        _ = lists.Add(CityWallList);
        _ = lists.Add(CitySizeList);
        _ = lists.Add(CityRelationList);
        _ = lists.Add(CityMoraleList);
        _ = lists.Add(CityActionList);
        UI.General.SortLists(lists, 0);
    }

    // AoWRealm.pas:820
    public void T3SNameClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(PlaceNameList);
        _ = lists.Add(PlaceActionList);
        UI.General.SortLists(lists, 0);
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
        Rect r;
        index = HeroNameList.Selected;
        if (index > -1)
        {
            hero = HeroHealthList.Items.Objects[index << 0];
            r = new Rect(HeroHeroInfoPanel.AbsoluteLeft + 1, HeroHeroInfoPanel.AbsoluteTop + 1, HeroHeroInfoPanel.AbsoluteLeft + HeroHeroInfoPanel.WinWidth - 1, HeroHeroInfoPanel.AbsoluteTop + HeroHeroInfoPanel.WinHeight - 1);
            if (hero.GetFace() != null)
            {
                hero.ShowFace(r, lockedSurface);
            }
        }
    }

    // AoWRealm.pas:866
    public void Unit1LBDoubleClick(ListBox sender, int slot)
    {
        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(sender.Selected).Units.Count > 0)
        {
            Troop troop = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(sender.Selected).Units.GetUnit(slot);
            UI.General.SelectedUnit = troop;
            UI.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:875
    public void T5SNameClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(HeroNameList);
        _ = lists.Add(HeroHealthList);
        _ = lists.Add(HeroLevelList);
        UI.General.SortLists(lists, 0);
    }

    // AoWRealm.pas:887
    public void T5SHealthClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(HeroHealthList);
        _ = lists.Add(HeroNameList);
        _ = lists.Add(HeroLevelList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:899
    public void T5SLevelClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(HeroLevelList);
        _ = lists.Add(HeroNameList);
        _ = lists.Add(HeroHealthList);
        UI.General.SortLists(lists, 2);
    }

    // AoWRealm.pas:911
    public void GotoButClick() => UI.General.PlayButtonSound(0);

    // AoWRealm.pas:916
    public void T4PnlShow()
    {
        for (int i = 0, last = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetCount() - 1; i <= last; i++)
        {
            for (int j = 0, lastJ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.Count - 1; j <= lastJ; j++)
            {
                int unitGraphicsResourceIndex = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.GetUnit(j).GetUnitGraphicsResourceIndex();
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex).AddRef();
            }
        }
    }

    // AoWRealm.pas:926
    public void T4PnlHide()
    {
        for (int i = 0, last = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetCount() - 1; i <= last; i++)
        {
            for (int j = 0, lastJ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.Count - 1; j <= lastJ; j++)
            {
                int unitGraphicsResourceIndex = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Armies.GetArmy(i).Units.GetUnit(j).GetUnitGraphicsResourceIndex();
                _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(unitGraphicsResourceIndex).Release();
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
    public void IncomeSBChange() => IncomeValueMemo.ListOffset = IncomeScrollBar.Position;

    // AoWRealm.pas:951
    public void T3ActivityClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        _ = lists.Add(PlaceActionList);
        _ = lists.Add(PlaceNameList);
        UI.General.SortLists(lists, 1);
    }

    // AoWRealm.pas:962
    public void UpkeepSBChange() => UpkeepValueMemo.ListOffset = UpkeepScrollBar.Position;

    // AoWRealm.pas:967
    public void DlgShow() => UI.General.RegisterDialog(Window);

    // AoWRealm.pas:972
    public void DlgHide() => UI.General.UnregisterDialog(Window);

    // AoWRealm.pas:977
    public void HelpClick() => UI.HelpWindow.ShowHelp(505);

    private static readonly string s_buildersGuildsText = "Builder Guilds";
    private static readonly string s_citiesText = "Cities";
    private static readonly string s_externalIncomeText = "External Income";
    private static readonly string s_farmsText = "Farms";
    private static readonly string s_minesText = "Mines";

    private static int BuildersGuildIncome(sbyte player)
    {
        int number;
        if (StrategicMap.Current != null)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(player);
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
        if (StrategicMap.Current != null)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 131336 && structure.Player == player)
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

    private static int FarmIncome(sbyte player)
    {
        int number;
        if (StrategicMap.Current != null)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(player);
            number = 0;
            int i = 0;
            for (int number2 = players.Structures.Count - 1; i <= number2; i++)
            {
                PlayerStructure structure = players.Structures.GetStructure(i);
                if (structure.EngineClass.ClassId == 131950 && structure.Player == player)
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

    private static int MineIncome(sbyte player)
    {
        int number;
        if (StrategicMap.Current != null)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(player);
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
