#nullable disable warnings
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

using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Session;

// leader. heroes. items. units. research percentage. spells. research spell ID. research points
// left. selected heroes/items/units. gold/mana. spheres. points max. gold max. mana max. points
// left
public sealed class CampaignTransferSettings : EngineObject
{
    // Diplomacy.pas:1736
    public CampaignTransferSettings(CustomNode? owner) : base(owner)
    {
        Heroes = new HeroList(null) { Holder = CreationHolder() };
        Items = new ItemList(null) { Holder = CreationHolder() };
        Units = new UnitList(null) { Holder = CreationHolder() };
        _spells = [];
        _researchPercentage = 50;
        SelectedHeroes = [];
        SelectedItems = [];
        SelectedUnits = [];
        SelectedSpheres = [];
    }

    public Leader? Leader { get; private set; }

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
        // { Original bug: the three selected-index lists are never freed. }
        base.Dispose();
    }

    // Diplomacy.pas:1776
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("Heroes", objectValue: Heroes);
        persister.Object("Items", objectValue: Items);
        persister.Object("Units", objectValue: Units);
        persister.Int32List(name: "Spells", list: _spells);
        _researchSpellId = persister.Int32("ResearchSpellId", value: _researchSpellId);
        _researchPointsLeft = persister.Int32("ResearchPointsLeft", value: _researchPointsLeft);
        _researchPercentage = persister.Int32("ResearchPercentage", value: _researchPercentage);
        if (persister.Loading)
        {
            Leader? leader = null;
            leader = persister.Created("Leader", objectValue: leader);
            SetLeader(leader);
            _ = (leader?.Release());
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
        _ = (Leader?.Release());
        Leader = null;

        Heroes.Clear();
        Items.Clear();
        Units.Clear();
        _spells.Clear();
    }

    // Diplomacy.pas:1797
    public void SetLeader(Leader? leader)
    {
        _ = (Leader?.Release());

        Leader = leader;
        _ = (Leader?.AddRef());
    }

    // Diplomacy.pas:1804
    public void GenerateTransferUnits(UnitUserList units)
    {
        Span<int> levelCounts = stackalloc int[4];
        Units.Clear();
        DelphiList<Troop> candidates = [];
        int maxUnits = 10;
        levelCounts[1 - 1] = 8;
        levelCounts[2 - 1] = 4;
        levelCounts[3 - 1] = 1;
        levelCounts[4 - 1] = GetEngine().Campaign.RaceHistory.Count > 2 ? 1 : 0;

        for (int level = 4; level >= 1; level--)
        {
            if (levelCounts[level - 1] != 0)
            {
                candidates.Clear();
                for (int i = 0, last = units.Count - 1; i <= last; i++)
                {
                    Troop? troop = units.GetUnit(i);
                    if (troop.UnitLevel == level && Units.IndexOf(troop) == -1)
                    {
                        _ = candidates.Add(troop);
                    }
                }

                // { Original bug: the per-level quota is tested but never decremented. }
                while (levelCounts[level - 1] != 0 && candidates.Count != 0 && Units.Count != maxUnits)
                {
                    int index = Random(candidates.Count);
                    Troop? abstractUnit = candidates[index];
                    _ = Units.AddUnit(abstractUnit);
                    candidates.Delete(index);
                }
            }
        }
    }

    // Diplomacy.pas:1842
    public void Retrieve()
    {
        Clear();
        if (StrategicMap.Current is not null)
        {
            UnitUserList units = new(null) { Holder = CreationHolder() };
            sbyte player = (sbyte)StrategicMap.Current.Players.LocalPlayers[0];
            Leader = StrategicMap.Current.Players.GetPlayers(player).Leader.Copy<Leader>(GetEngine());
            Leader.RemoveEnchantments();
            Leader.RemoveHandicaps();
            _ = Leader.ExecuteCancelCasting();
            Item? item;
            Item copyItem;
            for (int j = Leader.Inventory.Items.Count - 1; j >= 0; j--)
            {
                item = Leader.Inventory.Items.GetItem(j);
                if (item is not null)
                {
                    copyItem = item.Copy<Item>(GetEngine());
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    _ = copyItem.Release();
                }
            }

            Leader.Inventory.Clear();
            for (int j = Leader.HeroItems.Items.Count - 1; j >= 0; j--)
            {
                item = Leader.HeroItems.Items.GetItem(j);
                if (item is not null)
                {
                    copyItem = item.Copy<Item>(GetEngine());
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    _ = copyItem.Release();
                }
            }

            Leader.HeroItems.Clear();
            for (int i = 0, last = StrategicMap.Current.UnitControl.GetCount() - 1; i <= last; i++)
            {
                Troop troop = StrategicMap.Current.UnitControl[i];
                if (troop.Player == player && troop.GetUnitType() != UnitType.Machine && (troop.GetMoveTypes() & (MoveTypes.Walking | MoveTypes.Flying)) != MoveTypes.None && troop is not Magic.Abilities.Leader && !troop.GetAbilityEnabled(155))
                {
                    if (troop is Hero)
                    {
                        Hero hero = troop.Copy<Hero>(GetEngine());
                        hero.SetOwner(Heroes);
                        hero.RemoveEnchantments();
                        hero.RemoveHandicaps();
                        _ = hero.ExecuteCancelCasting();
                        for (int j = hero.Inventory.Items.Count - 1; j >= 0; j--)
                        {
                            item = hero.Inventory.Items.GetItem(j);
                            if (item is not null)
                            {
                                copyItem = item.Copy<Item>(GetEngine());
                                copyItem.SetOwner(Items);
                                copyItem.ItemId = hero.UnitId;
                                _ = copyItem.Release();
                            }
                        }

                        hero.Inventory.Clear();
                        for (int j = hero.HeroItems.Items.Count - 1; j >= 0; j--)
                        {
                            item = hero.HeroItems.Items.GetItem(j);
                            if (item is not null)
                            {
                                copyItem = item.Copy<Item>(GetEngine());
                                copyItem.SetOwner(Items);
                                copyItem.ItemId = hero.UnitId;
                                _ = copyItem.Release();
                            }
                        }

                        hero.HeroItems.Clear();
                        _ = hero.Release();
                    }
                    else
                    {
                        Troop newUnit = troop.Copy<Troop>(GetEngine());
                        newUnit.RemoveEnchantments();
                        newUnit.RemoveHandicaps();
                        _ = units.AddUnit(newUnit);
                        _ = newUnit.Release();
                    }
                }
            }

            _researchPercentage = StrategicMap.Current.Players.GetPlayers(player).MagicControl.ResearchPercentage;
            Player? players = StrategicMap.Current.Players.GetPlayers(player);
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
        if (index >= 0 && index < Items.Count && SelectedItems.IndexOf(index) == -1 && Items.GetItem(index).CampaignTransferPoints <= TransferPointsLeft)
        {
            int campaignTransferPoints = Items.GetItem(index).CampaignTransferPoints;
            TransferPointsLeft -= campaignTransferPoints;
            _ = SelectedItems.Add(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1968
    public bool DeleteItem(int index)
    {
        if (SelectedItems.Remove(index) != -1)
        {
            int campaignTransferPoints = Items.GetItem(index).CampaignTransferPoints;
            TransferPointsLeft += campaignTransferPoints;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1978
    public bool AddHero(int index)
    {
        if (index >= 0 && index < Heroes.Count && SelectedHeroes.IndexOf(index) == -1 && Heroes.GetHero(index).GetCampaignTransferPoints() <= TransferPointsLeft)
        {
            int campaignTransferPoints = Heroes.GetHero(index).GetCampaignTransferPoints();
            TransferPointsLeft -= campaignTransferPoints;
            _ = SelectedHeroes.Add(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1991
    public bool DeleteHero(int index)
    {
        if (SelectedHeroes.Remove(index) != -1)
        {
            int campaignTransferPoints = Heroes.GetHero(index).GetCampaignTransferPoints();
            TransferPointsLeft += campaignTransferPoints;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:2001
    public bool AddUnit(int index)
    {
        if (index >= 0 && index < Units.Count && SelectedUnits.IndexOf(index) == -1 && Units.GetUnit(index).GetCampaignTransferPoints() <= TransferPointsLeft)
        {
            int campaignTransferPoints = Units.GetUnit(index).GetCampaignTransferPoints();
            TransferPointsLeft -= campaignTransferPoints;
            _ = SelectedUnits.Add(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:2014
    public bool DeleteUnit(int index)
    {
        if (SelectedUnits.Remove(index) != -1)
        {
            int campaignTransferPoints = Units.GetUnit(index).GetCampaignTransferPoints();
            TransferPointsLeft += campaignTransferPoints;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:2024
    public void SetSelectedTransferGold(int value)
    {
        int max = SelectedTransferGold + TransferPointsLeft;
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
        int max = SelectedTransferMana + TransferPointsLeft;
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
        ClearSelection();
        StrategicMap map = GetEngine().CreatePreviewHsm<StrategicMap>(fileName, profile: GameProfile.WorldMap);
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
        ClearSelection();
        QuadItemList candidates = [];
        bool done = false;
        while (!done)
        {
            candidates.Clear();
            int points;
            for (int i = 0, last = Heroes.Count - 1; i <= last; i++)
            {
                if (SelectedHeroes.IndexOf(i) == -1)
                {
                    points = Heroes.GetHero(i).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        _ = candidates.AddABCD(0, i, C: points, D: 0);
                    }
                }
            }

            for (int i = 0, last = Items.Count - 1; i <= last; i++)
            {
                if (SelectedItems.IndexOf(i) == -1)
                {
                    points = Items.GetItem(i).CampaignTransferPoints;
                    if (points <= TransferPointsLeft)
                    {
                        _ = candidates.AddABCD(1, i, C: points, D: 0);
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
                        _ = candidates.AddABCD(2, i, C: points, D: 0);
                    }
                }
            }

            candidates.Sort(CompareCampaignTransfer);
            if (candidates.Count != 0)
            {
                switch (candidates[0].First)
                {
                    case 0:
                        _ = AddHero(candidates[0].Second);
                        break;
                    case 1:
                        _ = AddItem(candidates[0].Second);
                        break;
                    case 2:
                        _ = AddUnit(candidates[0].Second);
                        break;
                    default:
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
    }

    // Diplomacy.pas:2109
    public void ExecuteSelection()
    {
        if (StrategicMap.Current is not null)
        {
            StrategicMap.Current.SynchronizeBegin();
            StrategicMap.Current.Players.Update();
            sbyte player = (sbyte)(StrategicMap.Current.Players.Count - 1);
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
            Player? players = StrategicMap.Current.Players.GetPlayers(player);
            players.InitialGold += SelectedTransferGold;
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(player);
            fPlayersPlayers.MagicControl.ExternalMana += SelectedTransferMana;
            Leader? leader = StrategicMap.Current.Players.GetPlayers(player).Leader;
            MapCoordinate coordinate23 = leader.GetLocation();
            int x = coordinate23.X;
            int y = coordinate23.Y;
            int l = coordinate23.MapLevel;
            Item? itemItem;
            if (Leader is not null)
            {
                leader.Deactivate();
                leader.Assign(Leader, engine: GetEngine());
                leader.SetSpherePicksMax((leader.SphereCount + SelectedSpheres.Count).TruncateToByte());
                for (int i = 0, last = SelectedSpheres.Count - 1; i <= last; i++)
                {
                    _ = leader.AddSphere((MagicSphere)SelectedSpheres[i]);
                }

                for (int j = SelectedItems.Count - 1; j >= 0; j--)
                {
                    int item = SelectedItems[j];
                    itemItem = Items.GetItem(item);
                    if (leader.UnitId == itemItem.ItemId)
                    {
                        int fSelectedItemsItem = SelectedItems[j];
                        itemItem = Items.GetItem(fSelectedItemsItem).Copy<Item>(GetEngine());
                        if (leader.AutoPlaceItem(itemItem))
                        {
                            SelectedItems.Delete(j);
                        }

                        _ = itemItem.Release();
                    }
                }

                leader.UnitId = -1;
                leader.SetPlayer(player);
                leader.Activate();
            }

            for (int i = SelectedUnits.Count - 1; i >= 0; i--)
            {
                int item = SelectedUnits[i];
                Troop troop = Units.GetUnit(item).Copy<Troop>(GetEngine());
                troop.UnitId = -1;
                _ = troop.Place(x, y, l, player, mode: UnitPlaceModes.ScanArea);
                _ = troop.Release();
            }

            for (int i = SelectedHeroes.Count - 1; i >= 0; i--)
            {
                int item = SelectedHeroes[i];
                Hero hero = Heroes.GetHero(item).Copy<Hero>(GetEngine());
                for (int j = SelectedItems.Count - 1; j >= 0; j--)
                {
                    int fSelectedItemsItem = SelectedItems[j];
                    itemItem = Items.GetItem(fSelectedItemsItem);
                    if (hero.UnitId == itemItem.ItemId)
                    {
                        int item2 = SelectedItems[j];
                        itemItem = Items.GetItem(item2).Copy<Item>(GetEngine());
                        if (hero.AutoPlaceItem(itemItem))
                        {
                            SelectedItems.Delete(j);
                        }

                        _ = itemItem.Release();
                    }
                }

                hero.UnitId = -1;
                hero.Mode |= HeroModes.SkipLibraryUpdate;
                _ = hero.Place(x, y, l, player, mode: UnitPlaceModes.ScanArea);
                hero.Mode &= ~HeroModes.SkipLibraryUpdate;
                _ = hero.Release();
            }

            for (int i = SelectedItems.Count - 1; i >= 0; i--)
            {
                int item = SelectedItems[i];
                itemItem = Items.GetItem(item).Copy<Item>(GetEngine());
                StrategicMap.Current.ItemControl.RegisterItem(itemItem);
                _ = itemItem.PlaceOnMap(StrategicMap.Current, (sbyte)x, (sbyte)y, (sbyte)l);
                _ = itemItem.Release();
            }

            StrategicMap.Current.SynchronizeEnd();
        }
    }

    private static int CompareCampaignTransfer(QuadItemList.QuadInteger item1, QuadItemList.QuadInteger item2) => item2.Third - item1.Third;
}
