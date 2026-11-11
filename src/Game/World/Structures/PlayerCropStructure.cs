// Translated from Crops.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.World.Structures;

// FIncome, FChangingCrops, FIncomeSource
public partial class PlayerCropStructure : PlayerStructure
{

    public PlayerCropStructure(ECustomNode owner) : base(owner)
    {
        FChangingCrops = false;
        FIncomeSource = new PlayerStructureIncomeSource(null);
        FIncomeSource.Setup(this);
    }

    // Crops.pas:284
    public new const int ClassID = 131918;

    public byte FIncome;
    public bool FChangingCrops;
    public PlayerStructureIncomeSource FIncomeSource;

    // Crops.pas:278
    public override void Dispose()
    {
        base.Dispose();
        FIncomeSource?.Dispose();
    }

    // Crops.pas:270

    // Crops.pas:528
    public override void NeighbourTerrainChanged(int HN, in HexSpriteTerrainInfo Info)
    {
        if (Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting, HexSpriteMapStatusElement.Activated).IsSubsetOf(AoWHSMap.Current.Status) && !FChangingCrops)
        {
            UpdateCrops();
        }
    }

    // Crops.pas:534
    public override void Remove()
    {
        if (!GetEditMode())
        {
            SetCropCount(0);
        }

        base.Remove();
    }

    // Crops.pas:357
    public override void Activate()
    {
        sbyte Player;
        base.Activate();
        Player = FPlayer;
        if (Player != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).RegisterIncomeSource(FIncomeSource);
        }

        if (!GetEditMode())
        {
            LinkToCrops();
            UpdateCrops();
        }
    }

    // Crops.pas:372
    public override void Deactivate()
    {
        sbyte Player;
        Player = FPlayer;
        if (Player != -1)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(Player).UnregisterIncomeSource(FIncomeSource);
        }

        base.Deactivate();
    }

    // Crops.pas:560
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapLevel MapLevel;
        int I;
        int HN;
        int X = default;
        int Y = default;
        int NX = default;
        int NY = default;
        bool Result = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
        if (Result && !GetEditMode())
        {
            MapLevel = MapContainer.GetMapLevel(Lhx);
            I = ((PlayerCropStructureResource)FResource).FCropLocations.Count;
            while (Result && I != 0)
            {
                I -= 1;
                HexGeometry.CenterHNtoHX(Xhx, Yhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                NX = X;
                NY = Y;
                HN = 0;
                while (Result && HN != 7)
                {
                    if (NX >= 0 && NX < MapLevel.Width && NY >= 0 && NY < MapLevel.Height)
                    {
                        Result = MapLevel.GetField(NX, NY).FindNoneTransparentHS(typeof(PlayerCropStructure)) == null;
                    }

                    HN += 1;
                    HexGeometry.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
                }
            }
        }

        return Result;
    }

    // Crops.pas:586
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        if (Result)
        {
            if (!GetEditMode())
            {
                UpdateCrops();
            }
        }

        return Result;
    }

    // Crops.pas:325
    public override void NewTurn(sbyte Player)
    {
        base.NewTurn(Player);
        if (FPlayer == Player)
        {
            if (CropsEnabled())
            {
                RegrowCrops();
            }
        }
    }

    // Crops.pas:342
    public override void SetPlayer(sbyte Player)
    {
        if (Player != FPlayer)
        {
            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
            }

            base.SetPlayer(Player);
            UpdateCrops();
            if (FPlayer != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
            }
        }
    }

    // Crops.pas:440
    public virtual bool CanPlaceCrop(MapField Field)
    {
        bool Result;
        ArmyHexSprite Army;
        HexSprite HS;
        if (!GetRazed())
        {
            Army = (ArmyHexSprite)Field.FindOwnedChild(131607);
            Result = Army == null || AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Army.FArmy.FPlayer, FPlayer) != DiplomaticRelation.War;
            if (Result)
            {
                HS = Field.GetMaxLevelNoneTransparentHS();
                Result = HS == null || HS.GetLevel() < 26;
            }

            if (Result)
            {
                Result = Set<byte>.WithBounds(0, 255).With(12).WithRange(1, 4).Contains((byte)Field.TerrainType) || Field.TerrainType == 5 && GetRace() == 11;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Crops.pas:320
    public virtual bool CropsEnabled() => !GetRazed() && !GetBuilding();

    // Crops.pas:314
    public virtual void RegrowCrops()
    {
        if (GetCropCount() < ((PlayerCropStructureResource)FResource).FCropLocations.Count)
        {
            SetCropCount(GetCropCount() + 1);
        }
    }

    // Crops.pas:332
    public virtual void UpdateIncome()
    {
        if (!GetRazed() && !GetBuilding())
        {
            SetIncome((byte)(GetCropCount() * 2));
        }
        else
        {
            SetIncome(0);
        }
    }

    // Crops.pas:401
    public virtual void UpdateCrops()
    {
        if (!GetEditMode() && FStatus.Contains(MapObjectStatusElement.Activated))
        {
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                if (CropsEnabled())
                {
                    SetCropCount(((PlayerCropStructureResource)FResource).FCropLocations.Count);
                }
            }

            UpdateCropSettings();
            UpdateIncome();
        }
    }

    // Crops.pas:413
    public virtual sbyte GetRace()
    {
        sbyte Result;
        if (FPlayer > 0)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // Crops.pas:339
    public virtual void CropsChanged()
    {
        UpdateIncome();
    }

    // Crops.pas:290
    public virtual void CropTrampled(sbyte Player)
    {
        int Race;
        if (Player != FPlayer && FPlayer > 0 && Player > 0)
        {
            Race = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
            var playerRelationValue = AoWHSMap.Current.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
            AoWHSMap.Current.FRaces.GetRace(Race).SetPlayerRelationValue(Player, playerRelationValue - -5);
        }
    }

    // Crops.pas:302
    public void SetIncome(byte Value)
    {
        sbyte Player;
        if (Value != this.FIncome && this.FStatus.Contains(MapObjectStatusElement.Activated))
        {
            this.FIncome = Value;
            Player = this.FPlayer;
            if (Player != -1)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(Player).UpdateIncome();
            }
        }
    }

    // Crops.pas:382
    public void UpdateCropSettings()
    {
        MapLevel MapLevel;
        int X = default;
        int Y = default;
        Crop Crop;
        MapLevel = (MapLevel)Owner.Owner;
        for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (Crop)MapLevel.GetField(X, Y).FindOwnedChild(131946);
                if (Crop != null)
                {
                    Crop.UpdateSettings();
                }
            }
        }
    }

    // Crops.pas:420
    public int GetCropCount()
    {
        MapLevel MapLevel;
        int X = default;
        int Y = default;
        Crop Crop;
        int Result = 0;
        MapLevel = (MapLevel)Owner.Owner;
        for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (Crop)MapLevel.GetField(X, Y).FindChild(131946);
                if (Crop != null)
                {
                    if (Crop.FCropOwner == this)
                    {
                        Result += 1;
                    }
                }
            }
        }

        return Result;
    }

    // Crops.pas:463
    public void SetCropCount(int Value)
    {
        MapLevel MapLevel;
        int I;
        Crop.Resource Resource;
        MapField Field;
        int X = default;
        int Y = default;
        DelphiList Crops;
        Crop Crop;
        UInt16List Locations;
        if (FStatus.Contains(MapObjectStatusElement.PlacedOnMap) && !FChangingCrops)
        {
            FChangingCrops = true;
            MapLevel = (MapLevel)Owner.Owner;
            Crops = new DelphiList();
            Locations = new UInt16List();
            Locations.Assign(((PlayerCropStructureResource)FResource).FCropLocations);
            I = Locations.Count - 1;
            for (; I >= 0; I -= 1)
            {
                HexGeometry.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                if (X >= 0 && X < MapLevel.Width && Y >= 0 && Y < MapLevel.Height)
                {
                    Field = MapLevel.GetField(X, Y);
                    Crop = (Crop)Field.FindOwnedChild(131946);
                    if (Crop != null || !CanPlaceCrop(Field))
                    {
                        if (Crop != null && Crop.FCropOwner == this)
                        {
                            Crops.Add(Crop);
                        }

                        Locations.Delete(I);
                    }
                }
                else
                {
                    Locations.Delete(I);
                }
            }

            if (Crops.Count < Value)
            {
                Resource = (Crop.Resource)FResource.Engine.HSSet.MapObjectResourceList.FindChild(131947);
                if (Resource != null)
                {
                    while (Crops.Count != Value && Locations.Count != 0)
                    {
                        I = AoWHSMap.Current.Random(Locations.Count);
                        HexGeometry.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                        Crop = Resource.PlaceCrop((sbyte)X, (sbyte)Y, (sbyte)MapLevel.Lhx, this);
                        if (Crop != null)
                        {
                            Crops.Add(Crop);
                            Crop.SetCropOwner(this);
                        }

                        Locations.Delete(I);
                    }
                }
            }
            else
            {
                if (Crops.Count > Value)
                {
                    while (Crops.Count != Value && Crops.Count != 0)
                    {
                        I = AoWHSMap.Current.Random(Crops.Count);
                        ((Crops[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                        Crops.Delete(I);
                    }
                }
            }

            // { Original frees and clears the recursion guard only on normal completion. }
            Crops?.Dispose();
            Locations?.Dispose();
            FChangingCrops = false;
            CropsChanged();
        }
    }

    // Crops.pas:540
    public void LinkToCrops()
    {
        MapLevel MapLevel;
        int X = default;
        int Y = default;
        Crop Crop;
        if (GetEditMode())
        {
            return;
        }

        MapLevel = (MapLevel)Owner.Owner;
        for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (Crop)MapLevel.GetField(X, Y).FindChild(131946);
                if (Crop != null)
                {
                    Crop.SetCropOwner(this);
                }
            }
        }
    }

    // Crops.pas:27
    public override int GetIncome() => FIncome;
}
