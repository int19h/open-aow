// Translated from AoWE.pas.
using Aow.Engine.Buffers;
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
        FHeroes = new HeroList(null);
        FItems = new ItemList(null);
        FUnits = new UnitList(null);
        FSpells = new Int32List();
        FResearchPercentage = 50;
        FSelectedHeroes = new Int32List();
        FSelectedItems = new Int32List();
        FSelectedUnits = new Int32List();
        FSelectedSpheres = new ByteList();
    }

    public Leader FLeader;
    public HeroList FHeroes;
    public ItemList FItems;
    public UnitList FUnits;
    public int FResearchPercentage;
    public Int32List FSpells;
    public int FResearchSpellID;
    public int FResearchPointsLeft;
    public Int32List FSelectedHeroes;
    public Int32List FSelectedItems;
    public Int32List FSelectedUnits;
    public int FSelectedTransferGold;
    public int FSelectedTransferMana;
    public ByteList FSelectedSpheres;
    public int FTransferPointsMax;
    public int FTransferGoldMax;
    public int FTransferManaMax;
    public int FTransferPointsLeft;

    // Diplomacy.pas:1750
    public override void Dispose()
    {
        Clear();
        FHeroes?.Dispose();
        FItems?.Dispose();
        FUnits?.Dispose();
        FSelectedSpheres?.Dispose();
        FSpells?.Dispose();
        // { Original bug: the three selected-index lists are never freed. }
        base.Dispose();
    }

    // Diplomacy.pas:1776
    public override void ReadWrite(EStorageStream Stream)
    {
        Leader Leader = default;
        base.ReadWrite(Stream);
        Stream.rwEObject(FHeroes, 20);
        Stream.rwEObject(FItems, 21);
        Stream.rwEObject(FUnits, 22);
        Stream.rwStreamProc(23, FSpells.LoadFromStream, FSpells.SaveToStream);
        Stream.rwInteger(ref FResearchSpellID, 24);
        Stream.rwInteger(ref FResearchPointsLeft, 25);
        Stream.rwInteger(ref FResearchPercentage, 26);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Translate)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Leader = null;
            Stream.rwCreateEObject(ref Leader, null, 27);
            SetLeader(Leader);
            if (Leader != null)
            {
                Leader.Release();
            }
        }
        else
        {
            Stream.rwCreateEObject(ref FLeader, null, 27);
        }
    }

    // Diplomacy.pas:1762
    public override void Clear()
    {
        base.Clear();
        if (FLeader != null)
        {
            FLeader.Release();
            FLeader = null;
        }

        FHeroes.Clear();
        FItems.Clear();
        FUnits.Clear();
        FSpells.Clear();
    }

    // Diplomacy.pas:1797
    public void SetLeader(Leader Leader)
    {
        if (FLeader != null)
        {
            FLeader.Release();
        }

        FLeader = Leader;
        if (FLeader != null)
        {
            FLeader.AddRef();
        }
    }

    // Diplomacy.pas:1804
    public void GenerateTransferUnits(UnitList Units)
    {
        int Index;
        int MaxUnits;
        Int32Array4 LevelCounts = default;
        DelphiList Candidates;
        Troop AUnit;
        FUnits.Clear();
        Candidates = new DelphiList();
        MaxUnits = 10;
        LevelCounts[1 - 1] = 8;
        LevelCounts[2 - 1] = 4;
        LevelCounts[3 - 1] = 1;
        if (GameTables.AoWEngine.FCampaign.FRaceHistory.Count > 2)
        {
            LevelCounts[4 - 1] = 1;
        }
        else
        {
            LevelCounts[4 - 1] = 0;
        }

        for (int Level = 4; Level >= 1; Level--)
        {
            if (LevelCounts[Level - 1] != 0)
            {
                Candidates.Clear();
                for (int I = 0, last = Units.Count - 1; I <= last; I++)
                {
                    AUnit = Units.GetUnit(I);
                    if (AUnit.GetUnitLevel() == Level && FUnits.IndexOf(AUnit) == -1)
                    {
                        Candidates.Add(AUnit);
                    }
                }

                // { Original bug: the per-level quota is tested but never decremented. }
                while (LevelCounts[Level - 1] != 0 && Candidates.Count != 0 && FUnits.Count != MaxUnits)
                {
                    Index = Random(Candidates.Count);
                    var abstractUnit = Candidates[Index];
                    FUnits.AddUnit((Troop)abstractUnit);
                    Candidates.Delete(Index);
                }
            }
        }

        Candidates?.Dispose();
    }

    // Diplomacy.pas:1842
    public void Retrieve()
    {
        UnitUserList Units;
        sbyte Player;
        Troop AUnit;
        Troop NewUnit;
        Hero Hero;
        Item Item;
        Item CopyItem;
        Clear();
        if (AoWHSMap.Current != null)
        {
            Units = new UnitUserList(null);
            Player = (sbyte)AoWHSMap.Current.FPlayers.FLocalPlayers[0];
            FLeader = (Leader)AoWHSMap.Current.FPlayers.GetPlayers(Player).FLeader.Copy(GameTables.AoWEngine);
            FLeader.RemoveEnchantments();
            FLeader.RemoveHandicaps();
            FLeader.ExecuteCancelCasting();
            for (int J = FLeader.FInventory.Count - 1; J >= 0; J--)
            {
                Item = FLeader.FInventory.GetItem(J);
                if (Item != null)
                {
                    CopyItem = (Item)Item.Copy(GameTables.AoWEngine);
                    CopyItem.SetOwner(FItems);
                    CopyItem.FItemID = FLeader.FUnitID;
                    CopyItem.Release();
                }
            }

            FLeader.FInventory.Clear();
            for (int J = FLeader.FHeroItems.Count - 1; J >= 0; J--)
            {
                Item = FLeader.FHeroItems.GetItem(J);
                if (Item != null)
                {
                    CopyItem = (Item)Item.Copy(GameTables.AoWEngine);
                    CopyItem.SetOwner(FItems);
                    CopyItem.FItemID = FLeader.FUnitID;
                    CopyItem.Release();
                }
            }

            FLeader.FHeroItems.Clear();
            for (int I = 0, last = AoWHSMap.Current.FUnitControl.GetCount() - 1; I <= last; I++)
            {
                AUnit = AoWHSMap.Current.FUnitControl.GetUnit(I);
                if (AUnit.FPlayer == Player && AUnit.GetUnitType() != UnitType.Machine && (AUnit.GetMoveTypes() & Set<MoveType>.Of(MoveType.Walking, MoveType.Flying)) != Set<MoveType>.Of() && !(AUnit is Leader) && !AUnit.GetAbilityEnabled(155))
                {
                    if (AUnit is Hero)
                    {
                        Hero = (Hero)AUnit.Copy(GameTables.AoWEngine);
                        Hero.SetOwner(FHeroes);
                        Hero.RemoveEnchantments();
                        Hero.RemoveHandicaps();
                        Hero.ExecuteCancelCasting();
                        for (int J = Hero.FInventory.Count - 1; J >= 0; J--)
                        {
                            Item = Hero.FInventory.GetItem(J);
                            if (Item != null)
                            {
                                CopyItem = (Item)Item.Copy(GameTables.AoWEngine);
                                CopyItem.SetOwner(FItems);
                                CopyItem.FItemID = Hero.FUnitID;
                                CopyItem.Release();
                            }
                        }

                        Hero.FInventory.Clear();
                        for (int J = Hero.FHeroItems.Count - 1; J >= 0; J--)
                        {
                            Item = Hero.FHeroItems.GetItem(J);
                            if (Item != null)
                            {
                                CopyItem = (Item)Item.Copy(GameTables.AoWEngine);
                                CopyItem.SetOwner(FItems);
                                CopyItem.FItemID = Hero.FUnitID;
                                CopyItem.Release();
                            }
                        }

                        Hero.FHeroItems.Clear();
                        Hero.Release();
                    }
                    else
                    {
                        NewUnit = (Troop)AUnit.Copy(GameTables.AoWEngine);
                        NewUnit.RemoveEnchantments();
                        NewUnit.RemoveHandicaps();
                        Units.AddUnit(NewUnit);
                        NewUnit.Release();
                    }
                }
            }

            FResearchPercentage = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchPercentage;
            var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            FSpells.Assign(players.FMagicControl.FSpells);
            FResearchSpellID = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchSpellID;
            FResearchPointsLeft = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchPointsLeft;
            RandSeed = AoWHSMap.Current.Random(16777215);
            GenerateTransferUnits(Units);
            Units?.Dispose();
        }
    }

    // Diplomacy.pas:1944
    public void ClearSelection()
    {
        FSelectedHeroes.Clear();
        FSelectedItems.Clear();
        FSelectedUnits.Clear();
        FTransferPointsLeft = FTransferPointsMax;
        FSelectedTransferMana = 0;
        FSelectedTransferGold = 0;
        FSelectedSpheres.Clear();
    }

    // Diplomacy.pas:1955
    public bool AddItem(int Index)
    {
        bool Result;
        if (Index >= 0 && Index < FItems.Count && FSelectedItems.IndexOf(Index) == -1 && FItems.GetItem(Index).GetCampaignTransferPoints() <= FTransferPointsLeft)
        {
            var campaignTransferPoints = FItems.GetItem(Index).GetCampaignTransferPoints();
            FTransferPointsLeft -= campaignTransferPoints;
            FSelectedItems.Add(Index);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:1968
    public bool DeleteItem(int Index)
    {
        bool Result;
        if (FSelectedItems.Remove(Index) != -1)
        {
            var campaignTransferPoints = FItems.GetItem(Index).GetCampaignTransferPoints();
            FTransferPointsLeft += campaignTransferPoints;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:1978
    public bool AddHero(int Index)
    {
        bool Result;
        if (Index >= 0 && Index < FHeroes.Count && FSelectedHeroes.IndexOf(Index) == -1 && FHeroes.GetHero(Index).GetCampaignTransferPoints() <= FTransferPointsLeft)
        {
            var campaignTransferPoints = FHeroes.GetHero(Index).GetCampaignTransferPoints();
            FTransferPointsLeft -= campaignTransferPoints;
            FSelectedHeroes.Add(Index);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:1991
    public bool DeleteHero(int Index)
    {
        bool Result;
        if (FSelectedHeroes.Remove(Index) != -1)
        {
            var campaignTransferPoints = FHeroes.GetHero(Index).GetCampaignTransferPoints();
            FTransferPointsLeft += campaignTransferPoints;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:2001
    public bool AddUnit(int Index)
    {
        bool Result;
        if (Index >= 0 && Index < FUnits.Count && FSelectedUnits.IndexOf(Index) == -1 && FUnits.GetUnit(Index).GetCampaignTransferPoints() <= FTransferPointsLeft)
        {
            var campaignTransferPoints = FUnits.GetUnit(Index).GetCampaignTransferPoints();
            FTransferPointsLeft -= campaignTransferPoints;
            FSelectedUnits.Add(Index);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:2014
    public bool DeleteUnit(int Index)
    {
        bool Result;
        if (FSelectedUnits.Remove(Index) != -1)
        {
            var campaignTransferPoints = FUnits.GetUnit(Index).GetCampaignTransferPoints();
            FTransferPointsLeft += campaignTransferPoints;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:2024
    public void SetSelectedTransferGold(int Value)
    {
        int Max;
        Max = FSelectedTransferGold + FTransferPointsLeft;
        if (Max > FTransferGoldMax)
        {
            Max = FTransferGoldMax;
        }

        if (Value > Max)
        {
            Value = Max;
        }

        FTransferPointsLeft -= Value - FSelectedTransferGold;
        FSelectedTransferGold = Value;
    }

    // Diplomacy.pas:2034
    public void SetSelectedTransferMana(int Value)
    {
        int Max;
        Max = FSelectedTransferMana + FTransferPointsLeft;
        if (Max > FTransferManaMax)
        {
            Max = FTransferManaMax;
        }

        if (Value > Max)
        {
            Value = Max;
        }

        FTransferPointsLeft -= Value - FSelectedTransferMana;
        FSelectedTransferMana = Value;
    }

    // Diplomacy.pas:2044
    public void SetupSelection(string FileName)
    {
        AoWHSMap Map;
        ClearSelection();
        Map = (AoWHSMap)GameTables.AoWEngine.CreatePreviewHSM(FileName, GameProfile.WorldMap);
        FTransferPointsMax = Map.FCampaignSettings.FTransferPointsMax;
        if (GameTables.AoWEngine.FCampaign.FDifficultyLevel == Campaign.DifficultyLevel.Easy)
        {
            FTransferPointsMax = Round((double)FTransferPointsMax * 1.5);
        }

        if (GameTables.AoWEngine.FCampaign.FDifficultyLevel == Campaign.DifficultyLevel.Hard)
        {
            FTransferPointsMax = Round((double)FTransferPointsMax * 0.5);
        }

        FTransferGoldMax = Map.FCampaignSettings.FTransferGoldMax;
        FTransferManaMax = Map.FCampaignSettings.FTransferManaMax;
        FTransferPointsLeft = FTransferPointsMax;
        Map?.Dispose();
    }

    // Diplomacy.pas:2065
    public void AutoSelect()
    {
        int Points = default;
        bool Done = default;
        QuadItemList Candidates = default;
        ClearSelection();
        Candidates = new QuadItemList();
        Done = false;
        while (!Done)
        {
            Candidates.Clear();
            for (int I = 0, last = FHeroes.Count - 1; I <= last; I++)
            {
                if (FSelectedHeroes.IndexOf(I) == -1)
                {
                    Points = FHeroes.GetHero(I).GetCampaignTransferPoints();
                    if (Points <= FTransferPointsLeft)
                    {
                        Candidates.AddABCD(0, I, Points, 0);
                    }
                }
            }

            for (int I = 0, last = FItems.Count - 1; I <= last; I++)
            {
                if (FSelectedItems.IndexOf(I) == -1)
                {
                    Points = FItems.GetItem(I).GetCampaignTransferPoints();
                    if (Points <= FTransferPointsLeft)
                    {
                        Candidates.AddABCD(1, I, Points, 0);
                    }
                }
            }

            for (int I = 0, last = FUnits.Count - 1; I <= last; I++)
            {
                if (FSelectedUnits.IndexOf(I) == -1)
                {
                    Points = FUnits.GetUnit(I).GetCampaignTransferPoints();
                    if (Points <= FTransferPointsLeft)
                    {
                        Candidates.AddABCD(2, I, Points, 0);
                    }
                }
            }

            Candidates.Sort(CompareCampaignTransfer);
            if (Candidates.Count != 0)
            {
                switch (Candidates[0].A)
                {
                    case 0:
                        AddHero(Candidates[0].B);
                        break;
                    case 1:
                        AddItem(Candidates[0].B);
                        break;
                    case 2:
                        AddUnit(Candidates[0].B);
                        break;
                }
            }
            else
            {
                Done = true;
            }
        }

        SetSelectedTransferGold(FTransferPointsMax);
        SetSelectedTransferMana(FTransferPointsMax);
        Candidates?.Dispose();
    }

    // Diplomacy.pas:2109
    public void ExecuteSelection()
    {
        sbyte Player;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Leader Leader;
        Troop AUnit;
        Hero Hero;
        Item Item;
        if (AoWHSMap.Current != null)
        {
            AoWHSMap.Current.SynchroniseBegin();
            AoWHSMap.Current.FPlayers.Update();
            Player = (sbyte)(AoWHSMap.Current.FPlayers.Count - 1);
            while (Player > 1 && AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
            {
                Player = (sbyte)(Player - 1);
            }

            if (GameTables.AoWEngine.FCampaign.FCompletedScenarios.Count != 0)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchPercentage = FResearchPercentage;
            }

            AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchSpellID = FResearchSpellID;
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FResearchPointsLeft = FResearchPointsLeft;
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.FSpells.Assign(FSpells);
            var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            players.FInitialGold += FSelectedTransferGold;
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            fPlayersPlayers.FMagicControl.FExternalMana += FSelectedTransferMana;
            Leader = AoWHSMap.Current.FPlayers.GetPlayers(Player).FLeader;
            Leader.GetLocation(ref X, ref Y, ref L);
            if (FLeader != null)
            {
                Leader.Deactivate();
                Leader.Assign(FLeader, GameTables.AoWEngine);
                Leader.SetSpherePicksMax((byte)(Leader.GetSphereCount() + FSelectedSpheres.Count));
                for (int I = 0, last = FSelectedSpheres.Count - 1; I <= last; I++)
                {
                    Leader.AddSphere((MagicSphere)FSelectedSpheres[I]);
                }

                for (int J = FSelectedItems.Count - 1; J >= 0; J--)
                {
                    var item = FSelectedItems[J];
                    Item = FItems.GetItem(item);
                    if (Leader.FUnitID == Item.FItemID)
                    {
                        var fSelectedItemsItem = FSelectedItems[J];
                        Item = (Item)FItems.GetItem(fSelectedItemsItem).Copy(GameTables.AoWEngine);
                        if (Leader.AutoPlaceItem(Item))
                        {
                            FSelectedItems.Delete(J);
                        }

                        Item.Release();
                    }
                }

                Leader.FUnitID = -1;
                Leader.SetPlayer(Player);
                Leader.Activate();
            }

            for (int I = FSelectedUnits.Count - 1; I >= 0; I--)
            {
                var item = FSelectedUnits[I];
                AUnit = (Troop)FUnits.GetUnit(item).Copy(GameTables.AoWEngine);
                AUnit.FUnitID = -1;
                AUnit.Place(X, Y, L, Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                AUnit.Release();
            }

            for (int I = FSelectedHeroes.Count - 1; I >= 0; I--)
            {
                var item = FSelectedHeroes[I];
                Hero = (Hero)FHeroes.GetHero(item).Copy(GameTables.AoWEngine);
                for (int J = FSelectedItems.Count - 1; J >= 0; J--)
                {
                    var fSelectedItemsItem = FSelectedItems[J];
                    Item = FItems.GetItem(fSelectedItemsItem);
                    if (Hero.FUnitID == Item.FItemID)
                    {
                        var item2 = FSelectedItems[J];
                        Item = (Item)FItems.GetItem(item2).Copy(GameTables.AoWEngine);
                        if (Hero.AutoPlaceItem(Item))
                        {
                            FSelectedItems.Delete(J);
                        }

                        Item.Release();
                    }
                }

                Hero.FUnitID = -1;
                Hero.FMode = Hero.FMode + Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                Hero.Place(X, Y, L, Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                Hero.FMode = Hero.FMode - Set<HeroModeElement>.Of(HeroModeElement.SkipLibraryUpdate);
                Hero.Release();
            }

            for (int I = FSelectedItems.Count - 1; I >= 0; I--)
            {
                var item = FSelectedItems[I];
                Item = (Item)FItems.GetItem(item).Copy(GameTables.AoWEngine);
                AoWHSMap.Current.FItemControl.RegisterItem(Item);
                Item.PlaceOnMap(AoWHSMap.Current, X, Y, L);
                Item.Release();
            }

            AoWHSMap.Current.SynchroniseEnd();
        }
    }

    private static int CompareCampaignTransfer(in QuadItemList.QuadInteger Item1, in QuadItemList.QuadInteger Item2)
    {
        return Item2.C - Item1.C;
    }

    // intf_P5_08.inc:43

    // intf_P5_08.inc:43

}
