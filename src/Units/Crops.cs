// Translated from Crops.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.ILTer;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Crops;

// FIncome, FChangingCrops, FIncomeSource
public partial class TPlayerCropStructure : TPlayerStructure
{

    public TPlayerCropStructure(TECustomNode owner) : base(owner)
    {
        FChangingCrops = false;
        FIncomeSource = new TPlayerStructureIncomeSource(null);
        FIncomeSource.Setup(this);
    }

    // Crops.pas:284
    public new const int ClassID = 131918;

    public byte FIncome;
    public bool FChangingCrops;
    public TPlayerStructureIncomeSource FIncomeSource;
    // Crops.pas:278
    public override void Destroy()
    {
        base.Destroy();
        FIncomeSource.Free();
    }

    // Crops.pas:270


    // Crops.pas:528
    public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
    {
        if (TSet<THSMapStatusElement>.Of(THSMapStatusElement.msEditting, THSMapStatusElement.msActivated).IsSubsetOf(AoWHSMap.Status) && !FChangingCrops)
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
            AoWHSMap.FPlayers.GetPlayers(Player).RegisterIncomeSource(FIncomeSource);
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
            AoWHSMap.FPlayers.GetPlayers(Player).UnregisterIncomeSource(FIncomeSource);
        }

        base.Deactivate();
    }

    // Crops.pas:560
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        TMapLevel MapLevel;
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
            I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count;
            while (Result && I != 0)
            {
                I -= 1;
                CenterHNtoHX(Xhx, Yhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                NX = X;
                NY = Y;
                HN = 0;
                while (Result && HN != 7)
                {
                    if (NX >= 0 && NX < MapLevel.Width && NY >= 0 && NY < MapLevel.Height)
                    {
                        Result = MapLevel.GetField(NX, NY).FindNoneTransparentHS(typeof(TPlayerCropStructure)) == null;
                    }

                    HN += 1;
                    CenterHNtoHX(X, Y, HN, ref NX, ref NY);
                }
            }
        }

        return Result;
    }

    // Crops.pas:586
    public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
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
                AoWHSMap.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
            }

            base.SetPlayer(Player);
            UpdateCrops();
            if (FPlayer != -1)
            {
                AoWHSMap.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
            }
        }
    }

    // Crops.pas:440
    public virtual bool CanPlaceCrop(TMapField Field)
    {
        bool Result;
        TArmyHS Army;
        THexagonSprite HS;
        if (!GetRazed())
        {
            Army = (TArmyHS)Field.FindOwnedChild(131607);
            Result = Army == null || AoWHSMap.FPlayers.GetDiplomaticRelation(Army.FArmy.FPlayer, FPlayer) != TDiplomaticRelation.drWar;
            if (Result)
            {
                HS = Field.GetMaxLevelNoneTransparentHS();
                Result = HS == null || HS.GetLevel() < 26;
            }

            if (Result)
            {
                Result = TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 4).Contains((byte)Field.TerrainType) || Field.TerrainType == 5 && GetRace() == 11;
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
        if (GetCropCount() < ((TPlayerCropStructureResource)FResource).FCropLocations.Count)
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
        if (!GetEditMode() && FStatus.Contains(TMapObjectStatusElement.moActivated))
        {
            if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                if (CropsEnabled())
                {
                    SetCropCount(((TPlayerCropStructureResource)FResource).FCropLocations.Count);
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
            Result = AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerRace;
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
            Race = AoWHSMap.FPlayers.GetPlayers(FPlayer).FPlayerRace;
            var playerRelationValue = AoWHSMap.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
            AoWHSMap.FRaces.GetRace(Race).SetPlayerRelationValue(Player, playerRelationValue - -5);
        }
    }

    // Crops.pas:302
    public void SetIncome(byte Value)
    {
        sbyte Player;
        if (Value != this.FIncome && this.FStatus.Contains(TMapObjectStatusElement.moActivated))
        {
            this.FIncome = Value;
            Player = this.FPlayer;
            if (Player != -1)
            {
                AoWHSMap.FPlayers.GetPlayers(Player).UpdateIncome();
            }
        }
    }

    // Crops.pas:382
    public void UpdateCropSettings()
    {
        TMapLevel MapLevel;
        int X = default;
        int Y = default;
        TCrop Crop;
        MapLevel = (TMapLevel)Owner.Owner;
        for (int I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (TCrop)MapLevel.GetField(X, Y).FindOwnedChild(131946);
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
        TMapLevel MapLevel;
        int X = default;
        int Y = default;
        TCrop Crop;
        int Result = 0;
        MapLevel = (TMapLevel)Owner.Owner;
        for (int I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (TCrop)MapLevel.GetField(X, Y).FindChild(131946);
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
        TMapLevel MapLevel;
        int I;
        TCropResource Resource;
        TMapField Field;
        int X = default;
        int Y = default;
        TList Crops;
        TCrop Crop;
        TWordList Locations;
        if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap) && !FChangingCrops)
        {
            FChangingCrops = true;
            MapLevel = (TMapLevel)Owner.Owner;
            Crops = new TList();
            Locations = new TWordList();
            Locations.Assign(((TPlayerCropStructureResource)FResource).FCropLocations);
            I = Locations.Count - 1;
            for (; I >= 0; I -= 1)
            {
                CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                if (X >= 0 && X < MapLevel.Width && Y >= 0 && Y < MapLevel.Height)
                {
                    Field = MapLevel.GetField(X, Y);
                    Crop = (TCrop)Field.FindOwnedChild(131946);
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
                Resource = (TCropResource)FResource.Engine.HSSet.MapObjectResourceList.FindChild(131947);
                if (Resource != null)
                {
                    while (Crops.Count != Value && Locations.Count != 0)
                    {
                        I = AoWHSMap.Random(Locations.Count);
                        CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
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
                        I = AoWHSMap.Random(Crops.Count);
                        ((TCrop)Crops[I]).Destroy();
                        Crops.Delete(I);
                    }
                }
            }

            // { Original frees and clears the recursion guard only on normal completion. }
            Crops.Free();
            Locations.Free();
            FChangingCrops = false;
            CropsChanged();
        }
    }

    // Crops.pas:540
    public void LinkToCrops()
    {
        TMapLevel MapLevel;
        int X = default;
        int Y = default;
        TCrop Crop;
        if (GetEditMode())
        {
            return;
        }

        MapLevel = (TMapLevel)Owner.Owner;
        for (int I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
        {
            CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
            if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
            {
                Crop = (TCrop)MapLevel.GetField(X, Y).FindChild(131946);
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

// FCropLocations
public partial class TPlayerCropStructureResource : TPlayerStructureResource
{
    public TPlayerCropStructureResource(TECustomNode owner) : base(owner)
    {}

    // Crops.pas:599
    public new const int ClassID = 131919;

    public TWordList FCropLocations;
    // Crops.pas:592
    public override void Destroy()
    {
        FCropLocations.Free();
        FCropLocations = null;
        base.Destroy();
    }

    // Crops.pas:605
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            if (FCropLocations != null)
            {
                FCropLocations.Free();
            }

            FCropLocations = TerrainList.CreateSurroundingHNTable();
        }
    }

    // Crops.pas:602
    public override int ResourceUserClassID() => 131918;

}

// FCropOwner
public partial class TCrop : TILTerrainMO
{
    public TCrop(TECustomNode owner) : base(owner)
    {
    }

    // Crops.pas:119
    public new const int ClassID = 131946;

    public TPlayerCropStructure FCropOwner;
    // Crops.pas:114
    public override void Destroy()
    {
        base.Destroy();
    }

    // Crops.pas:109


    // Crops.pas:151
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001991)
        {
            EndTurn((sbyte)Msg.Payload.Int);
        }
    }

    // Crops.pas:203
    public override void TerrainChanged(in THSTerrainInfo Info)
    {
        THexagonSprite HS;
        if (Info.HN != 0)
        {
            return;
        }

        if (FStatus.Contains(TMapObjectStatusElement.moActivated))
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
                HS = Info.MapField.GetMaxLevelNoneTransparentHS();
                if ((HS != null && HS.GetLevel() > GetLevel() && !(HS is TAbstractRoad)) || (Info.OverlayType >= 0 && Info.OverlayType != 4))
                {
                    Destroy();
                }
            }
            else
            {
                Destroy();
            }
        }
        else
        {
            FTerrainType = Info.TerrainType;
        }
    }

    // Crops.pas:157
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        if (Msg.MsgID != 16)
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (Msg.MsgID == 4357)
        {
            Destroy();
        }
    }

    // Crops.pas:183
    public override void Activate()
    {
        LinkToCropOwner();
        base.Activate();
    }

    // Crops.pas:189
    public override void Deactivate()
    {
        if (FCropOwner != null && !AoWHSMap.Status.Contains(THSMapStatusElement.msDestroying))
        {
            var playerCropStructure = FCropOwner;
            this.FCropOwner = null;
            playerCropStructure.CropsChanged();
        }

        base.Deactivate();
    }

    // Crops.pas:200
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // Crops.pas:234
    public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        TMapField Field;
        THexagonSprite HS;
        Field = MapContainer.GetField(Xhx, Yhx, Lhx);
        HS = Field.GetMaxLevelNoneTransparentHS();
        return Field.FindOwnedHS(typeof(TFire)) == null && (HS == null || HS.GetLevel() < 10 || HS is TAbstractRoad);
    }

    // Crops.pas:228
    public override int GetTerrainTypeImage(int TerrainType)
    {
        int Result = TerrainType;
        if (FCropOwner != null && FCropOwner.GetRace() == 11)
        {
            Result += 30;
        }

        return Result;
    }

    // Crops.pas:122
    public void SetCropOwner(TPlayerCropStructure Value)
    {
        FCropOwner = Value;
        UpdateSettings();
    }

    // Crops.pas:128
    public void UpdateSettings()
    {
        int Index;
        Index = GetTerrainTypeImage(TerrainType);
        this.FImageIndex = Index;
        if (!FResource.ImageLibrary.ImageExist(Index))
        {
            Destroy();
        }
    }

    // Crops.pas:136
    public void EndTurn(sbyte Player)
    {
        sbyte PlayerIndex;
        TArmyHS Army;
        Army = ((TAoWMapField)Owner).GetArmyHS();
        PlayerIndex = Player;
        if (Army != null && Army.FArmy.FPlayer == PlayerIndex && FCropOwner.FPlayer > 0 && PlayerIndex != FCropOwner.FPlayer)
        {
            if (AoWHSMap.FPlayers.GetDiplomaticRelation(FCropOwner.FPlayer, PlayerIndex) == TDiplomaticRelation.drWar)
            {
                FCropOwner.CropTrampled(PlayerIndex);
                Destroy();
            }
        }
    }

    // Crops.pas:163
    public void LinkToCropOwner()
    {
        TPlayerCropStructure Found;
        TMapLevel MapLevel;
        THNWalkData Walk = default;
        Found = null;
        MapLevel = (TMapLevel)Owner.Owner;
        InitHNWalk(ref Walk, FXhx, FYhx, 1);
        while (Walk.Radius <= 1 && Found == null)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Found = (TPlayerCropStructure)MapLevel.GetField(Walk.X, Walk.Y).FindNoneTransparentHS(typeof(TPlayerCropStructure));
            }

            WalkToNextHN(ref Walk);
        }

        SetCropOwner(Found);
    }

    // Crops.pas:68


}

public partial class TCropResource : TILTerrainMOR
{
    public TCropResource(TECustomNode owner) : base(owner)
    {}

    // Crops.pas:245
    public new const int ClassID = 131947;

    // Crops.pas:251
    public override int ResourceUserClassID() => 131946;

    // Crops.pas:248
    public override void SetLevel(byte Value)
    {
        base.SetLevel(5);
    }

    // Crops.pas:257
    public TCrop PlaceCrop(sbyte X, sbyte Y, sbyte L, TPlayerCropStructure CropOwner)
    {
        TCrop Result = new TCrop(null);
        Result.SetResource(this);
        Result.ResourceLinked = true;
        Result.FCropOwner = CropOwner;
        if (!AoWHSMap.PlaceHX(Result, X, Y, L))
        {
            Result.Free();
            Result = null;
        }

        return Result;
    }

    // Crops.pas:254
    public override string ResourceEditName() => TranslateRStr(CropRStr);

}

[StructLayout(LayoutKind.Explicit, Size = 25)]
public struct TCropILFields
{
    [FieldOffset(0)]
    public FixedArray_byte_20 Reserved;
    [FieldOffset(20)]
    public int ImageIndex;
    [FieldOffset(24)]
    public sbyte TerrainType;
}

public static partial class CropsUnit
{
    // Crops.pas:615
    public static void RegisterCrops(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TCrop>(), EClass.Of<TCropResource>()]);
        AEngine.RegisterEClasses([EClass.Of<TPlayerCropStructure>(), EClass.Of<TPlayerCropStructureResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
