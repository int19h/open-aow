#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Session;

// leader. heroes. items. units. research percentage. spells. research spell ID. research points
// left. selected heroes/items/units. gold/mana. spheres. points max. gold max. mana max. points
// left
public partial class CampaignTransferSettings : EngineObject
{
    // Diplomacy.pas:1736
    public CampaignTransferSettings(CustomNode owner) : base(owner)
    {
        Heroes = new HeroList(null) { Holder = CreationHolder() };
        Items = new ItemList(null) { Holder = CreationHolder() };
        Units = new UnitList(null) { Holder = CreationHolder() };
        _spells = new Int32List();
        _researchPercentage = 50;
        SelectedHeroes = new Int32List();
        SelectedItems = new Int32List();
        SelectedUnits = new Int32List();
        SelectedSpheres = new ByteList();
    }

    public Leader Leader { get; private set; }

    public HeroList Heroes { get; }
    public ItemList Items { get; }
    public UnitList Units { get; }

    private int _researchPercentage;
    private readonly Int32List _spells;
    private int _researchSpellId;
    private int _researchPointsLeft;

    public Int32List SelectedHeroes { get; }
    public Int32List SelectedItems { get; }
    public Int32List SelectedUnits { get; }
    public int SelectedTransferGold { get; private set; }
    public int SelectedTransferMana { get; private set; }
    public ByteList SelectedSpheres { get; }
    public int TransferPointsMax { get; private set; }
    public int TransferGoldMax { get; private set; }
    public int TransferManaMax { get; private set; }
    public int TransferPointsLeft { get; private set; }

    // Diplomacy.pas:1750
    public override void Dispose()
    {
        Clear();
        Heroes?.Dispose();
        Items?.Dispose();
        Units?.Dispose();
        SelectedSpheres?.Dispose();
        _spells?.Dispose();
        // { Original bug: the three selected-index lists are never freed. }
        base.Dispose();
    }

    // Diplomacy.pas:1776
    public override void PersistFields(IPersister persister)
    {
        Leader leader = default;
        base.PersistFields(persister);
        persister.Object("Heroes", objectValue: Heroes);
        persister.Object("Items", objectValue: Items);
        persister.Object("Units", objectValue: Units);
        PersistSupport.Int32List(persister, name: "Spells", list: _spells);
        _researchSpellId = persister.Int32("ResearchSpellId", value: _researchSpellId);
        _researchPointsLeft = persister.Int32("ResearchPointsLeft", value: _researchPointsLeft);
        _researchPercentage = persister.Int32("ResearchPercentage", value: _researchPercentage);
        if (persister.Loading)
        {
            leader = null;
            leader = persister.Created("Leader", objectValue: leader);
            SetLeader(leader);
            if (leader != null)
            {
                leader.Release();
            }
        }
        else
        {
            Leader = persister.Created("Leader", objectValue: Leader);
        }
    }

    // Diplomacy.pas:1762
    public override void Clear()
    {
        base.Clear();
        if (Leader != null)
        {
            Leader.Release();
            Leader = null;
        }

        Heroes.Clear();
        Items.Clear();
        Units.Clear();
        _spells.Clear();
    }

    // Diplomacy.pas:1797
    public void SetLeader(Leader leader)
    {
        if (Leader != null)
        {
            Leader.Release();
        }

        Leader = leader;
        if (Leader != null)
        {
            Leader.AddRef();
        }
    }

    // Diplomacy.pas:1804
    public void GenerateTransferUnits(UnitUserList units)
    {
        int index;
        int maxUnits;
        Span<int> levelCounts = stackalloc int[4];
        DelphiList<Troop> candidates;
        Troop troop;
        Units.Clear();
        candidates = new DelphiList<Troop>();
        maxUnits = 10;
        levelCounts[1 - 1] = 8;
        levelCounts[2 - 1] = 4;
        levelCounts[3 - 1] = 1;
        if (GetEngine().Campaign.RaceHistory.Count > 2)
        {
            levelCounts[4 - 1] = 1;
        }
        else
        {
            levelCounts[4 - 1] = 0;
        }

        for (int level = 4; level >= 1; level--)
        {
            if (levelCounts[level - 1] != 0)
            {
                candidates.Clear();
                for (int i = 0, last = units.Count - 1; i <= last; i++)
                {
                    troop = units.GetUnit(i);
                    if (troop.GetUnitLevel() == level && Units.IndexOf(troop) == -1)
                    {
                        candidates.Add(troop);
                    }
                }

                // { Original bug: the per-level quota is tested but never decremented. }
                while (levelCounts[level - 1] != 0 && candidates.Count != 0 && Units.Count != maxUnits)
                {
                    index = Random(candidates.Count);
                    var abstractUnit = candidates[index];
                    Units.AddUnit(abstractUnit);
                    candidates.Delete(index);
                }
            }
        }

        candidates?.Dispose();
    }

    // Diplomacy.pas:1842
    public void Retrieve()
    {
        UnitUserList units;
        sbyte player;
        Troop troop;
        Troop newUnit;
        Hero hero;
        Item item;
        Item copyItem;
        Clear();
        if (StrategicMap.Current != null)
        {
            units = new UnitUserList(null) { Holder = CreationHolder() };
            player = (sbyte)StrategicMap.Current.Players.LocalPlayers[0];
            Leader = (Leader)StrategicMap.Current.Players.GetPlayers(player).Leader.Copy(GetEngine());
            Leader.RemoveEnchantments();
            Leader.RemoveHandicaps();
            Leader.ExecuteCancelCasting();
            for (int j = Leader.Inventory.Items.Count - 1; j >= 0; j--)
            {
                item = Leader.Inventory.Items.GetItem(j);
                if (item != null)
                {
                    copyItem = (Item)item.Copy(GetEngine());
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    copyItem.Release();
                }
            }

            Leader.Inventory.Clear();
            for (int j = Leader.HeroItems.Items.Count - 1; j >= 0; j--)
            {
                item = Leader.HeroItems.Items.GetItem(j);
                if (item != null)
                {
                    copyItem = (Item)item.Copy(GetEngine());
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    copyItem.Release();
                }
            }

            Leader.HeroItems.Clear();
            for (int i = 0, last = StrategicMap.Current.UnitControl.GetCount() - 1; i <= last; i++)
            {
                troop = StrategicMap.Current.UnitControl.GetUnit(i);
                if (troop.Player == player && troop.GetUnitType() != UnitType.Machine && (troop.GetMoveTypes() & Set<MoveType>.Of(MoveType.Walking, MoveType.Flying)) != Set<MoveType>.Of() && !(troop is Leader) && !troop.GetAbilityEnabled(155))
                {
                    if (troop is Hero)
                    {
                        hero = troop.Copy<Hero>(GetEngine());
                        hero.SetOwner(Heroes);
                        hero.RemoveEnchantments();
                        hero.RemoveHandicaps();
                        hero.ExecuteCancelCasting();
                        for (int j = hero.Inventory.Items.Count - 1; j >= 0; j--)
                        {
                            item = hero.Inventory.Items.GetItem(j);
                            if (item != null)
                            {
                                copyItem = (Item)item.Copy(GetEngine());
                                copyItem.SetOwner(Items);
                                copyItem.ItemId = hero.UnitId;
                                copyItem.Release();
                            }
                        }

                        hero.Inventory.Clear();
                        for (int j = hero.HeroItems.Items.Count - 1; j >= 0; j--)
                        {
                            item = hero.HeroItems.Items.GetItem(j);
                            if (item != null)
                            {
                                copyItem = (Item)item.Copy(GetEngine());
                                copyItem.SetOwner(Items);
                                copyItem.ItemId = hero.UnitId;
                                copyItem.Release();
                            }
                        }

                        hero.HeroItems.Clear();
                        hero.Release();
                    }
                    else
                    {
                        newUnit = (Troop)troop.Copy(GetEngine());
                        newUnit.RemoveEnchantments();
                        newUnit.RemoveHandicaps();
                        units.AddUnit(newUnit);
                        newUnit.Release();
                    }
                }
            }

            _researchPercentage = StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchPercentage;
            var players = StrategicMap.Current.Players.GetPlayers(player);
            _spells.Assign(players.MagicControl.Spells);
            _researchSpellId = StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId;
            _researchPointsLeft = StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchPointsLeft;
            RandSeed = StrategicMap.Current.Random(16777215);
            GenerateTransferUnits(units);
            units?.Dispose();
        }
    }

    // Diplomacy.pas:1944
    public void ClearSelection()
    {
        SelectedHeroes.Clear();
        SelectedItems.Clear();
        SelectedUnits.Clear();
        TransferPointsLeft = TransferPointsMax;
        SelectedTransferMana = 0;
        SelectedTransferGold = 0;
        SelectedSpheres.Clear();
    }

    // Diplomacy.pas:1955
    public bool AddItem(int index)
    {
        bool result;
        if (index >= 0 && index < Items.Count && SelectedItems.IndexOf(index) == -1 && Items.GetItem(index).GetCampaignTransferPoints() <= TransferPointsLeft)
        {
            var campaignTransferPoints = Items.GetItem(index).GetCampaignTransferPoints();
            TransferPointsLeft -= campaignTransferPoints;
            SelectedItems.Add(index);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:1968
    public bool DeleteItem(int index)
    {
        bool result;
        if (SelectedItems.Remove(index) != -1)
        {
            var campaignTransferPoints = Items.GetItem(index).GetCampaignTransferPoints();
            TransferPointsLeft += campaignTransferPoints;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:1978
    public bool AddHero(int index)
    {
        bool result;
        if (index >= 0 && index < Heroes.Count && SelectedHeroes.IndexOf(index) == -1 && Heroes.GetHero(index).GetCampaignTransferPoints() <= TransferPointsLeft)
        {
            var campaignTransferPoints = Heroes.GetHero(index).GetCampaignTransferPoints();
            TransferPointsLeft -= campaignTransferPoints;
            SelectedHeroes.Add(index);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:1991
    public bool DeleteHero(int index)
    {
        bool result;
        if (SelectedHeroes.Remove(index) != -1)
        {
            var campaignTransferPoints = Heroes.GetHero(index).GetCampaignTransferPoints();
            TransferPointsLeft += campaignTransferPoints;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:2001
    public bool AddUnit(int index)
    {
        bool result;
        if (index >= 0 && index < Units.Count && SelectedUnits.IndexOf(index) == -1 && Units.GetUnit(index).GetCampaignTransferPoints() <= TransferPointsLeft)
        {
            var campaignTransferPoints = Units.GetUnit(index).GetCampaignTransferPoints();
            TransferPointsLeft -= campaignTransferPoints;
            SelectedUnits.Add(index);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:2014
    public bool DeleteUnit(int index)
    {
        bool result;
        if (SelectedUnits.Remove(index) != -1)
        {
            var campaignTransferPoints = Units.GetUnit(index).GetCampaignTransferPoints();
            TransferPointsLeft += campaignTransferPoints;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:2024
    public void SetSelectedTransferGold(int value)
    {
        int max;
        max = SelectedTransferGold + TransferPointsLeft;
        if (max > TransferGoldMax)
        {
            max = TransferGoldMax;
        }

        if (value > max)
        {
            value = max;
        }

        TransferPointsLeft -= value - SelectedTransferGold;
        SelectedTransferGold = value;
    }

    // Diplomacy.pas:2034
    public void SetSelectedTransferMana(int value)
    {
        int max;
        max = SelectedTransferMana + TransferPointsLeft;
        if (max > TransferManaMax)
        {
            max = TransferManaMax;
        }

        if (value > max)
        {
            value = max;
        }

        TransferPointsLeft -= value - SelectedTransferMana;
        SelectedTransferMana = value;
    }

    // Diplomacy.pas:2044
    public void SetupSelection(string fileName)
    {
        StrategicMap map;
        ClearSelection();
        map = (StrategicMap)GetEngine().CreatePreviewHsm(fileName, GameProfile.WorldMap);
        TransferPointsMax = map.CampaignSettings.TransferPointsMax;
        if (GetEngine().Campaign.DifficultyLevel == CampaignDifficultyLevel.Easy)
        {
            TransferPointsMax = Round(TransferPointsMax * 1.5);
        }

        if (GetEngine().Campaign.DifficultyLevel == CampaignDifficultyLevel.Hard)
        {
            TransferPointsMax = Round(TransferPointsMax * 0.5);
        }

        TransferGoldMax = map.CampaignSettings.TransferGoldMax;
        TransferManaMax = map.CampaignSettings.TransferManaMax;
        TransferPointsLeft = TransferPointsMax;
        map?.Dispose();
    }

    // Diplomacy.pas:2065
    public void AutoSelect()
    {
        int points = default;
        bool done = default;
        QuadItemList candidates = default;
        ClearSelection();
        candidates = new QuadItemList();
        done = false;
        while (!done)
        {
            candidates.Clear();
            for (int i = 0, last = Heroes.Count - 1; i <= last; i++)
            {
                if (SelectedHeroes.IndexOf(i) == -1)
                {
                    points = Heroes.GetHero(i).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(0, i, points, 0);
                    }
                }
            }

            for (int i = 0, last = Items.Count - 1; i <= last; i++)
            {
                if (SelectedItems.IndexOf(i) == -1)
                {
                    points = Items.GetItem(i).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(1, i, points, 0);
                    }
                }
            }

            for (int i = 0, last = Units.Count - 1; i <= last; i++)
            {
                if (SelectedUnits.IndexOf(i) == -1)
                {
                    points = Units.GetUnit(i).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(2, i, points, 0);
                    }
                }
            }

            candidates.Sort(CompareCampaignTransfer);
            if (candidates.Count != 0)
            {
                switch (candidates[0].First)
                {
                    case 0:
                        AddHero(candidates[0].Second);
                        break;
                    case 1:
                        AddItem(candidates[0].Second);
                        break;
                    case 2:
                        AddUnit(candidates[0].Second);
                        break;
                }
            }
            else
            {
                done = true;
            }
        }

        SetSelectedTransferGold(TransferPointsMax);
        SetSelectedTransferMana(TransferPointsMax);
        candidates?.Dispose();
    }

    // Diplomacy.pas:2109
    public void ExecuteSelection()
    {
        sbyte player;
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
        Leader leader;
        Troop troop;
        Hero hero;
        Item itemItem;
        if (StrategicMap.Current != null)
        {
            StrategicMap.Current.SynchronizeBegin();
            StrategicMap.Current.Players.Update();
            player = (sbyte)(StrategicMap.Current.Players.Count - 1);
            while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                player = (sbyte)(player - 1);
            }

            if (GetEngine().Campaign.CompletedScenarios.Count != 0)
            {
                StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchPercentage = _researchPercentage;
            }

            StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchSpellId = _researchSpellId;
            StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchPointsLeft = _researchPointsLeft;
            StrategicMap.Current.Players.GetPlayers(player).MagicControl.Spells.Assign(_spells);
            var players = StrategicMap.Current.Players.GetPlayers(player);
            players.InitialGold += SelectedTransferGold;
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            fPlayersPlayers.MagicControl.ExternalMana += SelectedTransferMana;
            leader = StrategicMap.Current.Players.GetPlayers(player).Leader;
            var coordinate23 = leader.GetLocation();
            x = coordinate23.X;
            y = coordinate23.Y;
            l = coordinate23.MapLevel;
            if (Leader != null)
            {
                leader.Deactivate();
                leader.Assign(Leader, GetEngine());
                leader.SetSpherePicksMax((byte)(leader.GetSphereCount() + SelectedSpheres.Count));
                for (int i = 0, last = SelectedSpheres.Count - 1; i <= last; i++)
                {
                    leader.AddSphere((MagicSphere)SelectedSpheres[i]);
                }

                for (int j = SelectedItems.Count - 1; j >= 0; j--)
                {
                    var item = SelectedItems[j];
                    itemItem = Items.GetItem(item);
                    if (leader.UnitId == itemItem.ItemId)
                    {
                        var fSelectedItemsItem = SelectedItems[j];
                        itemItem = (Item)Items.GetItem(fSelectedItemsItem).Copy(GetEngine());
                        if (leader.AutoPlaceItem(itemItem))
                        {
                            SelectedItems.Delete(j);
                        }

                        itemItem.Release();
                    }
                }

                leader.UnitId = -1;
                leader.SetPlayer(player);
                leader.Activate();
            }

            for (int i = SelectedUnits.Count - 1; i >= 0; i--)
            {
                var item = SelectedUnits[i];
                troop = (Troop)Units.GetUnit(item).Copy(GetEngine());
                troop.UnitId = -1;
                troop.Place(x, y, l, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                troop.Release();
            }

            for (int i = SelectedHeroes.Count - 1; i >= 0; i--)
            {
                var item = SelectedHeroes[i];
                hero = Heroes.GetHero(item).Copy<Hero>(GetEngine());
                for (int j = SelectedItems.Count - 1; j >= 0; j--)
                {
                    var fSelectedItemsItem = SelectedItems[j];
                    itemItem = Items.GetItem(fSelectedItemsItem);
                    if (hero.UnitId == itemItem.ItemId)
                    {
                        var item2 = SelectedItems[j];
                        itemItem = (Item)Items.GetItem(item2).Copy(GetEngine());
                        if (hero.AutoPlaceItem(itemItem))
                        {
                            SelectedItems.Delete(j);
                        }

                        itemItem.Release();
                    }
                }

                hero.UnitId = -1;
                hero.Mode = hero.Mode + Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                hero.Place(x, y, l, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                hero.Mode = hero.Mode - Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                hero.Release();
            }

            for (int i = SelectedItems.Count - 1; i >= 0; i--)
            {
                var item = SelectedItems[i];
                itemItem = (Item)Items.GetItem(item).Copy(GetEngine());
                StrategicMap.Current.ItemControl.RegisterItem(itemItem);
                itemItem.PlaceOnMap(StrategicMap.Current, x, y, l);
                itemItem.Release();
            }

            StrategicMap.Current.SynchronizeEnd();
        }
    }

    private static int CompareCampaignTransfer(in QuadItemList.QuadInteger item1, in QuadItemList.QuadInteger item2) => item2.Third - item1.Third;

    // intf_P5_08.inc:43

    // intf_P5_08.inc:43

}
