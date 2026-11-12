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
    private ListBox[] _partyLists;
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
        int value;
        var playerGems = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        Treasury.Text = IntToStr(playerGems.Gems);
        var upkeep = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetUpkeep();
        var playerIncome = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        NetInc.Text = IntToStr(playerIncome.Income - upkeep);
        var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        Income.Text = IntToStr(player.Income);
        var upkeep2 = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetUpkeep();
        Upkeep.Text = IntToStr(upkeep2);
        IncomeMem.Items.Clear();
        IncomeValue.Items.Clear();
        UpkeepValue.Items.Clear();
        value = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).BaseIncome;
        if (value != 0)
        {
            var externalIncome = AbilityRules.TranslateResourceString(s_externalIncomeText);
            IncomeMem.Items.Add(externalIncome);
            IncomeValue.Items.Add(IntToStr(value));
        }

        value = CityIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var cities = AbilityRules.TranslateResourceString(s_citiesText);
            IncomeMem.Items.Add(cities);
            IncomeValue.Items.Add(IntToStr(value));
        }

        value = FarmIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var farms = AbilityRules.TranslateResourceString(s_farmsText);
            IncomeMem.Items.Add(farms);
            IncomeValue.Items.Add(IntToStr(value));
        }

        value = MineIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var mines = AbilityRules.TranslateResourceString(s_minesText);
            IncomeMem.Items.Add(mines);
            IncomeValue.Items.Add(IntToStr(value));
        }

        value = BuildersGuildIncome(AoWHSMap.Current.SeatedPlayer);
        if (value != 0)
        {
            var buildersGuilds = AbilityRules.TranslateResourceString(s_buildersGuildsText);
            IncomeMem.Items.Add(buildersGuilds);
            IncomeValue.Items.Add(IntToStr(value));
        }

        var playerArmyUpkeep = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        UpkeepValue.Items.Add(IntToStr(playerArmyUpkeep.ArmyUpkeep));
    }

    // AoWRealm.pas:178
    public void UpdateCities()
    {
        PlayerStructure structure;
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
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1; I <= last; I++)
        {
            structure = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(I);
            if (structure is City)
            {
                C = (City)structure;
                var name = C.GetName();
                T2NameLB.Items.AddObject(name, C);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.s_raceNamesPluralTexts[C.Race]);
                T2RaceLB.Items.Add(raceNamesPlural);
                T2IncomeLB.Items.Add(IntToStr(C.Income));
                T2LevelLB.Items.Add(IntToStr(C.UpgradeLevel));
                switch (C.WallType)
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

                switch ((int)AoWHSMap.Current.Races.GetRace(C.Race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer) & 127)
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
        PlayerStructure place;
        T3Name.Items.Clear();
        T3ActLB.Items.Clear();
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1; I <= last; I++)
        {
            place = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(I);
            if (!(place is City))
            {
                var editName = place.EditName();
                T3Name.Items.AddObject(editName, place);
                var currentActivityText = place.GetCurrentActivityText();
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
        for (int I = 0, last = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetCount() - 1; I <= last; I++)
        {
            for (int J = 0, lastJ = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetCount() - 1; J <= lastJ; J++)
            {
                U = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I).GetUnit(J);
                if (U is Hero)
                {
                    var army = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(I);
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
        if (AoWHSMap.Current.SeatedPlayer == -1)
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
            lockedSurface.ClipRectangle.Bottom = drawRectangle.Top + Unit1LB.ItemHeight - 6;
            if (drawRectangle.Bottom - drawRectangle.Top < Unit1LB.ItemHeight - 6)
            {
                lockedSurface.ClipRectangle.Bottom = drawRectangle.Bottom;
            }

            U = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(drawItem).GetUnit(column);
            if ((int)U.GetAlignment() < DialogHosts.UIGraphics.UnitBGs.Count)
            {
                var alignment = U.GetAlignment();
                var unitBackground = DialogHosts.UIGraphics.UnitBGs[(int)alignment];
                unitBackground.ShowClipped(drawRectangle.Left - (unitBackground.Width - (drawRectangle.Right - drawRectangle.Left)) / 2, drawRectangle.Top - (unitBackground.Height - (drawRectangle.Bottom - drawRectangle.Top - 10)) / 2, lockedSurface);
            }

            var previewImage = U.GetPreviewImage();
            var uPreviewImage = U.GetPreviewImage();
            U.Show(drawRectangle.Left - (uPreviewImage.Width - (drawRectangle.Right - drawRectangle.Left)) / 2, drawRectangle.Top - (previewImage.Height - (Unit1LB.ItemHeight - 10)) / 2, lockedSurface, DialogHosts.General.UnitShowInfo);
            var gauge = UnitGg;
            gauge.Visible = true;
            gauge.Max = U.GetHits();
            gauge.Position = U.GetHitPoints();
            gauge.WinLeft = drawRectangle.Left - T4LBPnl.WinLeft + 5;
            gauge.WinWidth = drawRectangle.Right - drawRectangle.Left - 10;
            gauge.WinTop = drawRectangle.Top + Unit1LB.ItemHeight - T4LBPnl.WinTop - 9;
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
        Dlg.Visible = false;
    }

    // AoWRealm.pas:388
    public void T2WallLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        string text = T2WallLB.Items[drawItem];
        switch (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]))
        {
            case 49:
                var listBoxIcon = DialogHosts.UIGraphics.LBIcons[7];
                DialogHosts.UIGraphics.LBIcons[7].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
                break;
            case 50:
                var listBoxIconWidth = DialogHosts.UIGraphics.LBIcons[8];
                DialogHosts.UIGraphics.LBIcons[8].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIconWidth.Width / 2, drawRectangle.Top, lockedSurface);
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
    public void Unit1LBChange(ListBox listBox, int slot)
    {
        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetCount() > 0)
        {
            listBox.PopupMenu = DialogHosts.UnitBanner.BannerPopup;
            var troop = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetUnit(slot);
            DialogHosts.General.SelectedUnit = troop;
            var armyHexSprite = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Armies.GetArmy(listBox.Selected).GetArmyHexSprite();
            AoWHSMap.Current.Select(armyHexSprite, true);
            Dlg.Priority = Dlg.Priority;
        }
        else
        {
            listBox.PopupMenu = null;
            listBox.Selected = -1;
        }

        _partyLists ??= [Unit1LB, Unit2LB, Unit3LB, Unit4LB, Unit5LB, Unit6LB, Unit7LB, Unit8LB];
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
        string text = T2MorLB.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var listBoxIcon = DialogHosts.UIGraphics.LBIcons[icon + 9];
        DialogHosts.UIGraphics.LBIcons[icon + 9].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
    }

    // AoWRealm.pas:457
    public void CityListMouseDown(ListBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        HexSprite sprite;
        if (button == MouseButton.Right && sender.Selected != -1)
        {
            sprite = (HexSprite)T2NameLB.Items.Objects[sender.Selected];
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
    public void CityDoubleClick(ListBox sender)
    {
        if (sender.Selected != -1)
        {
            var city = T2NameLB.Items.Objects[sender.Selected];
            DialogHosts.CityScreen.ShowCity((City)city);
        }
    }

    // AoWRealm.pas:492
    public void PlaceListDoubleClick(ListBox sender)
    {
        if (sender.Selected != -1)
        {
            if (T3Name.Items.Objects[sender.Selected] is ProductionPlace)
            {
                var productionPlace = T3Name.Items.Objects[sender.Selected];
                DialogHosts.ProductionScreen.ShowPlace((ProductionPlace)productionPlace);
            }
            else
            {
                var hexSprite = T3Name.Items.Objects[sender.Selected];
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
            sprite = (HexSprite)T3Name.Items.Objects[sender.Selected];
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
            var armyHexSprite = ((Army)T5Name.Items.Objects[listBox.Selected]).GetArmyHexSprite();
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
            sprite = ((Army)T5Name.Items.Objects[list.Selected]).GetArmyHexSprite();
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
        index = T5Name.Selected;
        if (index != -1)
        {
            // { evaluate the index before the receiver chain }
            var hero = T5Health.Items.Objects[index << 0];
            DialogHosts.General.SelectedUnit = (Hero)hero;
            DialogHosts.UnitWindow.ShowUnit();
        }
    }

    // AoWRealm.pas:549
    public void T2LevelLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Dlg.Visible = true;
    }

    // AoWRealm.pas:555
    public void T2WallLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Dlg.Visible = true;
    }

    // AoWRealm.pas:561
    public void T2RaceLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Dlg.Visible = true;
    }

    // AoWRealm.pas:567
    public void T2ActLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        byte icon;
        string text = T2ActLB.Items[drawItem];
        icon = IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0]);
        if (icon != 45)
        {
            var listBoxIcon = DialogHosts.UIGraphics.LBIcons[icon - 23];
            DialogHosts.UIGraphics.LBIcons[icon - 23].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
        }
    }

    // AoWRealm.pas:577
    public void T2ActLBDoubleClick()
    {
        DialogHosts.RealmWindow.UpdateOverview();
        DialogHosts.RealmWindow.Dlg.Visible = true;
    }

    // AoWRealm.pas:583
    public void T2RelLBDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        int icon;
        string text = T2RelLB.Items[drawItem];
        icon = (IsEmpty(text) ? (byte)0 : Cp1252.Byte(text[0])) - 48;
        var listBoxIcon = DialogHosts.UIGraphics.LBIcons[icon + 14];
        DialogHosts.UIGraphics.LBIcons[icon + 14].ShowClipped(drawRectangle.Left + (drawRectangle.Right - drawRectangle.Left) / 2 - listBoxIcon.Width / 2, drawRectangle.Top, lockedSurface);
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
    public void T3NameChange(ListBox sender)
    {
        ListBox list;
        list = sender;
        T3Name.Selected = list.Selected;
        T3ActLB.Selected = list.Selected;
    }

    // AoWRealm.pas:625
    public void T3SBChange()
    {
        T3Name.ListOffset = PlaceSB.Position;
    }

    // AoWRealm.pas:630
    public void T2SNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:649
    public void T2SRaceClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2RaceLB);
        lists.Add(T2NameLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:668
    public void T2SInClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2IncomeLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 2);
    }

    // AoWRealm.pas:687
    public void T2SLvlClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2LevelLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:706
    public void T2SWallClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2WallLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:725
    public void T2SSizeClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2SizeLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:744
    public void T2SRelClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2RelLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:763
    public void T2SMorClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2MorLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2ActLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:782
    public void T2SActClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2ActLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2EncLB);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:801
    public void T2SEncClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T2EncLB);
        lists.Add(T2NameLB);
        lists.Add(T2RaceLB);
        lists.Add(T2IncomeLB);
        lists.Add(T2LevelLB);
        lists.Add(T2WallLB);
        lists.Add(T2SizeLB);
        lists.Add(T2RelLB);
        lists.Add(T2MorLB);
        lists.Add(T2ActLB);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:820
    public void T3SNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T3Name);
        lists.Add(T3ActLB);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:831
    public void HeroListChange(ListBox sender)
    {
        ListBox list;
        list = sender;
        T5Name.Selected = list.Selected;
        T5Health.Selected = list.Selected;
        T5Level.Selected = list.Selected;
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
    public void T5HeroInfDraw(LockedSurface lockedSurface)
    {
        int index;
        Hero hero;
        Rect R;
        index = T5Name.Selected;
        if (index > -1)
        {
            hero = (Hero)T5Health.Items.Objects[index << 0];
            R = new Rect(T5HeroInf.WinLeft + 1, T5HeroInf.WinTop + 1, T5HeroInf.WinLeft + T5HeroInf.WinWidth - 1, T5HeroInf.WinTop + T5HeroInf.WinHeight - 1);
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
        lists.Add(T5Name);
        lists.Add(T5Health);
        lists.Add(T5Level);
        DialogHosts.General.SortLists(ref lists, 0);
    }

    // AoWRealm.pas:887
    public void T5SHealthClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T5Health);
        lists.Add(T5Name);
        lists.Add(T5Level);
        DialogHosts.General.SortLists(ref lists, 1);
    }

    // AoWRealm.pas:899
    public void T5SLevelClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T5Level);
        lists.Add(T5Name);
        lists.Add(T5Health);
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
    public void T3ActivityClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T3ActLB);
        lists.Add(T3Name);
        DialogHosts.General.SortLists(ref lists, 1);
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
