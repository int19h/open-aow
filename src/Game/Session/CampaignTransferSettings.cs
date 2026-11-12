// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

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
    public CampaignTransferSettings(ECustomNode owner) : base(owner)
    {
        Heroes = new HeroList(null);
        Items = new ItemList(null);
        Units = new UnitList(null);
        _spells = new Int32List();
        _researchPercentage = 50;
        SelectedHeroes = new Int32List();
        SelectedItems = new Int32List();
        SelectedUnits = new Int32List();
        SelectedSpheres = new ByteList();
    }

    public Leader Leader;

    public HeroList Heroes { get; }
    public ItemList Items { get; }
    public UnitList Units { get; }

    private int _researchPercentage;
    private Int32List _spells;
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
    public override void ReadWrite(EStorageStream stream)
    {
        Leader leader = default;
        base.ReadWrite(stream);
        stream.rwEObject(Heroes, 20);
        stream.rwEObject(Items, 21);
        stream.rwEObject(Units, 22);
        stream.rwStreamProc(23, _spells.LoadFromStream, _spells.SaveToStream);
        stream.rwInteger(ref _researchSpellId, 24);
        stream.rwInteger(ref _researchPointsLeft, 25);
        stream.rwInteger(ref _researchPercentage, 26);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            leader = null;
            stream.rwCreateEObject(ref leader, null, 27);
            SetLeader(leader);
            if (leader != null)
            {
                leader.Release();
            }
        }
        else
        {
            stream.rwCreateEObject(ref Leader, null, 27);
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
    public void GenerateTransferUnits(UnitList units)
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
        if (GameTables.GameEngine.Campaign.RaceHistory.Count > 2)
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
                for (int I = 0, last = units.Count - 1; I <= last; I++)
                {
                    troop = units.GetUnit(I);
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
            units = new UnitUserList(null);
            player = (sbyte)StrategicMap.Current.Players.LocalPlayers[0];
            Leader = (Leader)StrategicMap.Current.Players.GetPlayers(player).Leader.Copy(GameTables.GameEngine);
            Leader.RemoveEnchantments();
            Leader.RemoveHandicaps();
            Leader.ExecuteCancelCasting();
            for (int J = Leader.Inventory.Count - 1; J >= 0; J--)
            {
                item = Leader.Inventory.GetItem(J);
                if (item != null)
                {
                    copyItem = (Item)item.Copy(GameTables.GameEngine);
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    copyItem.Release();
                }
            }

            Leader.Inventory.Clear();
            for (int J = Leader.HeroItems.Count - 1; J >= 0; J--)
            {
                item = Leader.HeroItems.GetItem(J);
                if (item != null)
                {
                    copyItem = (Item)item.Copy(GameTables.GameEngine);
                    copyItem.SetOwner(Items);
                    copyItem.ItemId = Leader.UnitId;
                    copyItem.Release();
                }
            }

            Leader.HeroItems.Clear();
            for (int I = 0, last = StrategicMap.Current.UnitControl.GetCount() - 1; I <= last; I++)
            {
                troop = StrategicMap.Current.UnitControl.GetUnit(I);
                if (troop.Player == player && troop.GetUnitType() != UnitType.Machine && (troop.GetMoveTypes() & Set<MoveType>.Of(MoveType.Walking, MoveType.Flying)) != Set<MoveType>.Of() && !(troop is Leader) && !troop.GetAbilityEnabled(155))
                {
                    if (troop is Hero)
                    {
                        hero = (Hero)troop.Copy(GameTables.GameEngine);
                        hero.SetOwner(Heroes);
                        hero.RemoveEnchantments();
                        hero.RemoveHandicaps();
                        hero.ExecuteCancelCasting();
                        for (int J = hero.Inventory.Count - 1; J >= 0; J--)
                        {
                            item = hero.Inventory.GetItem(J);
                            if (item != null)
                            {
                                copyItem = (Item)item.Copy(GameTables.GameEngine);
                                copyItem.SetOwner(Items);
                                copyItem.ItemId = hero.UnitId;
                                copyItem.Release();
                            }
                        }

                        hero.Inventory.Clear();
                        for (int J = hero.HeroItems.Count - 1; J >= 0; J--)
                        {
                            item = hero.HeroItems.GetItem(J);
                            if (item != null)
                            {
                                copyItem = (Item)item.Copy(GameTables.GameEngine);
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
                        newUnit = (Troop)troop.Copy(GameTables.GameEngine);
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
        map = (StrategicMap)GameTables.GameEngine.CreatePreviewHsm(fileName, GameProfile.WorldMap);
        TransferPointsMax = map.CampaignSettings.TransferPointsMax;
        if (GameTables.GameEngine.Campaign.DifficultyLevel == CampaignDifficultyLevel.Easy)
        {
            TransferPointsMax = Round((double)TransferPointsMax * 1.5);
        }

        if (GameTables.GameEngine.Campaign.DifficultyLevel == CampaignDifficultyLevel.Hard)
        {
            TransferPointsMax = Round((double)TransferPointsMax * 0.5);
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
            for (int I = 0, last = Heroes.Count - 1; I <= last; I++)
            {
                if (SelectedHeroes.IndexOf(I) == -1)
                {
                    points = Heroes.GetHero(I).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(0, I, points, 0);
                    }
                }
            }

            for (int I = 0, last = Items.Count - 1; I <= last; I++)
            {
                if (SelectedItems.IndexOf(I) == -1)
                {
                    points = Items.GetItem(I).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(1, I, points, 0);
                    }
                }
            }

            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                if (SelectedUnits.IndexOf(I) == -1)
                {
                    points = Units.GetUnit(I).GetCampaignTransferPoints();
                    if (points <= TransferPointsLeft)
                    {
                        candidates.AddABCD(2, I, points, 0);
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
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Leader leader;
        Troop troop;
        Hero hero;
        Item Item;
        if (StrategicMap.Current != null)
        {
            StrategicMap.Current.SynchronizeBegin();
            StrategicMap.Current.Players.Update();
            player = (sbyte)(StrategicMap.Current.Players.Count - 1);
            while (player > 1 && StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
            {
                player = (sbyte)(player - 1);
            }

            if (GameTables.GameEngine.Campaign.CompletedScenarios.Count != 0)
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
            leader.GetLocation(ref X, ref Y, ref L);
            if (Leader != null)
            {
                leader.Deactivate();
                leader.Assign(Leader, GameTables.GameEngine);
                leader.SetSpherePicksMax((byte)(leader.GetSphereCount() + SelectedSpheres.Count));
                for (int I = 0, last = SelectedSpheres.Count - 1; I <= last; I++)
                {
                    leader.AddSphere((MagicSphere)SelectedSpheres[I]);
                }

                for (int J = SelectedItems.Count - 1; J >= 0; J--)
                {
                    var item = SelectedItems[J];
                    Item = Items.GetItem(item);
                    if (leader.UnitId == Item.ItemId)
                    {
                        var fSelectedItemsItem = SelectedItems[J];
                        Item = (Item)Items.GetItem(fSelectedItemsItem).Copy(GameTables.GameEngine);
                        if (leader.AutoPlaceItem(Item))
                        {
                            SelectedItems.Delete(J);
                        }

                        Item.Release();
                    }
                }

                leader.UnitId = -1;
                leader.SetPlayer(player);
                leader.Activate();
            }

            for (int I = SelectedUnits.Count - 1; I >= 0; I--)
            {
                var item = SelectedUnits[I];
                troop = (Troop)Units.GetUnit(item).Copy(GameTables.GameEngine);
                troop.UnitId = -1;
                troop.Place(X, Y, L, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                troop.Release();
            }

            for (int I = SelectedHeroes.Count - 1; I >= 0; I--)
            {
                var item = SelectedHeroes[I];
                hero = (Hero)Heroes.GetHero(item).Copy(GameTables.GameEngine);
                for (int J = SelectedItems.Count - 1; J >= 0; J--)
                {
                    var fSelectedItemsItem = SelectedItems[J];
                    Item = Items.GetItem(fSelectedItemsItem);
                    if (hero.UnitId == Item.ItemId)
                    {
                        var item2 = SelectedItems[J];
                        Item = (Item)Items.GetItem(item2).Copy(GameTables.GameEngine);
                        if (hero.AutoPlaceItem(Item))
                        {
                            SelectedItems.Delete(J);
                        }

                        Item.Release();
                    }
                }

                hero.UnitId = -1;
                hero.Mode = hero.Mode + Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                hero.Place(X, Y, L, player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                hero.Mode = hero.Mode - Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                hero.Release();
            }

            for (int I = SelectedItems.Count - 1; I >= 0; I--)
            {
                var item = SelectedItems[I];
                Item = (Item)Items.GetItem(item).Copy(GameTables.GameEngine);
                StrategicMap.Current.ItemControl.RegisterItem(Item);
                Item.PlaceOnMap(StrategicMap.Current, X, Y, L);
                Item.Release();
            }

            StrategicMap.Current.SynchronizeEnd();
        }
    }

    private static int CompareCampaignTransfer(in QuadItemList.QuadInteger item1, in QuadItemList.QuadInteger item2)
    {
        return item2.Third - item1.Third;
    }

    // intf_P5_08.inc:43

    // intf_P5_08.inc:43

}
