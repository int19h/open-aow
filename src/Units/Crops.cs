// Translated from Crops.pas.
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;

namespace Aow.Game.World.Structures
{

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
        public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Info)
        {
            if (Set<HSMapStatusElement>.Of(HSMapStatusElement.Editting, HSMapStatusElement.Activated).IsSubsetOf(Aow.Game.World.Map.AoWHSMap.Current.Status) && !FChangingCrops)
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
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).RegisterIncomeSource(FIncomeSource);
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
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).UnregisterIncomeSource(FIncomeSource);
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
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(Xhx, Yhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
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
                        Aow.Game.World.Map.HexGeometry.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
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
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).UnregisterIncomeSource(FIncomeSource);
                }

                base.SetPlayer(Player);
                UpdateCrops();
                if (FPlayer != -1)
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).RegisterIncomeSource(FIncomeSource);
                }
            }
        }

        // Crops.pas:440
        public virtual bool CanPlaceCrop(MapField Field)
        {
            bool Result;
            ArmyHS Army;
            HexagonSprite HS;
            if (!GetRazed())
            {
                Army = (ArmyHS)Field.FindOwnedChild(131607);
                Result = Army == null || Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Army.FArmy.FPlayer, FPlayer) != DiplomaticRelation.War;
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
            if (!GetEditMode() && FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Activated))
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
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
                Result = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
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
                Race = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FPlayerRace;
                var playerRelationValue = Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).GetPlayerRelationValue(Player);
                Aow.Game.World.Map.AoWHSMap.Current.FRaces.GetRace(Race).SetPlayerRelationValue(Player, playerRelationValue - -5);
            }
        }

        // Crops.pas:302
        public void SetIncome(byte Value)
        {
            sbyte Player;
            if (Value != this.FIncome && this.FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Activated))
            {
                this.FIncome = Value;
                Player = this.FPlayer;
                if (Player != -1)
                {
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).UpdateIncome();
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
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
            {
                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                {
                    Crop = (global::Aow.Game.World.Terrain.Crop)MapLevel.GetField(X, Y).FindOwnedChild(131946);
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
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
            {
                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                {
                    Crop = (global::Aow.Game.World.Terrain.Crop)MapLevel.GetField(X, Y).FindChild(131946);
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
            Aow.Game.World.Terrain.Crop.Resource Resource;
            MapField Field;
            int X = default;
            int Y = default;
            DelphiList Crops;
            Crop Crop;
            WordList Locations;
            if (FStatus.Contains(Aow.Game.World.MapObjectStatusElement.PlacedOnMap) && !FChangingCrops)
            {
                FChangingCrops = true;
                MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
                Crops = new DelphiList();
                Locations = new WordList();
                Locations.Assign(((PlayerCropStructureResource)FResource).FCropLocations);
                I = Locations.Count - 1;
                for (; I >= 0; I -= 1)
                {
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                    if (X >= 0 && X < MapLevel.Width && Y >= 0 && Y < MapLevel.Height)
                    {
                        Field = MapLevel.GetField(X, Y);
                        Crop = (global::Aow.Game.World.Terrain.Crop)Field.FindOwnedChild(131946);
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
                    Resource = (Aow.Game.World.Terrain.Crop.Resource)FResource.Engine.HSSet.MapObjectResourceList.FindChild(131947);
                    if (Resource != null)
                    {
                        while (Crops.Count != Value && Locations.Count != 0)
                        {
                            I = Aow.Game.World.Map.AoWHSMap.Current.Random(Locations.Count);
                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
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
                            I = Aow.Game.World.Map.AoWHSMap.Current.Random(Crops.Count);
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

            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            for (int I = ((PlayerCropStructureResource)FResource).FCropLocations.Count - 1; I >= 0; I--)
            {
                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FXhx, FYhx, ((PlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                {
                    Crop = (global::Aow.Game.World.Terrain.Crop)MapLevel.GetField(X, Y).FindChild(131946);
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
    public partial class PlayerCropStructureResource : PlayerStructureResource
    {
        public PlayerCropStructureResource(ECustomNode owner) : base(owner)
        { }

        // Crops.pas:599
        public new const int ClassID = 131919;

        public WordList FCropLocations;

        // Crops.pas:592
        public override void Dispose()
        {
            FCropLocations?.Dispose();
            FCropLocations = null;
            base.Dispose();
        }

        // Crops.pas:605
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                if (FCropLocations != null)
                {
                    FCropLocations?.Dispose();
                }

                FCropLocations = TerrainList.CreateSurroundingHNTable();
            }
        }

        // Crops.pas:602
        public override int ResourceUserClassID() => 131918;

    }
}

namespace Aow.Game.World.Terrain
{

    // FCropOwner
    public partial class Crop : ILTerrainMO
    {
        public Crop(ECustomNode owner) : base(owner)
        {
        }

        // Crops.pas:119
        public new const int ClassID = 131946;

        public PlayerCropStructure FCropOwner;

        // Crops.pas:109

        // Crops.pas:151
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001991)
            {
                EndTurn((sbyte)Msg.Payload.Int);
            }
        }

        // Crops.pas:203
        public override void TerrainChanged(in HSTerrainInfo Info)
        {
            HexagonSprite HS;
            if (Info.HN != 0)
            {
                return;
            }

            if (FStatus.Contains(Aow.Game.World.MapObjectStatusElement.Activated))
            {
                if (ValidTerrainType(Info.TerrainType))
                {
                    FTerrainType = Info.TerrainType;
                    FImageIndex = GetTerrainTypeImage(Info.TerrainType);
                    HS = Info.MapField.GetMaxLevelNoneTransparentHS();
                    if ((HS != null && HS.GetLevel() > GetLevel() && !(HS is AbstractRoad)) || (Info.OverlayType >= 0 && Info.OverlayType != 4))
                    {
                        Dispose();
                    }
                }
                else
                {
                    Dispose();
                }
            }
            else
            {
                FTerrainType = Info.TerrainType;
            }
        }

        // Crops.pas:157
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            if (Msg.MsgID != 16)
            {
                base.MapFieldMsgProc(ref Msg);
            }

            if (Msg.MsgID == 4357)
            {
                Dispose();
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
            if (FCropOwner != null && !Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Destroying))
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
        public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            MapField Field;
            HexagonSprite HS;
            Field = MapContainer.GetField(Xhx, Yhx, Lhx);
            HS = Field.GetMaxLevelNoneTransparentHS();
            return Field.FindOwnedHS(typeof(Fire)) == null && (HS == null || HS.GetLevel() < 10 || HS is AbstractRoad);
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
        public void SetCropOwner(PlayerCropStructure Value)
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
                Dispose();
            }
        }

        // Crops.pas:136
        public void EndTurn(sbyte Player)
        {
            sbyte PlayerIndex;
            ArmyHS Army;
            Army = ((AoWMapField)Owner).GetArmyHS();
            PlayerIndex = Player;
            if (Army != null && Army.FArmy.FPlayer == PlayerIndex && FCropOwner.FPlayer > 0 && PlayerIndex != FCropOwner.FPlayer)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FCropOwner.FPlayer, PlayerIndex) == DiplomaticRelation.War)
                {
                    FCropOwner.CropTrampled(PlayerIndex);
                    Dispose();
                }
            }
        }

        // Crops.pas:163
        public void LinkToCropOwner()
        {
            PlayerCropStructure Found;
            MapLevel MapLevel;
            HNWalkData Walk = default;
            Found = null;
            MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
            Aow.Game.World.Movement.HNWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 1);
            while (Walk.Radius <= 1 && Found == null)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    Found = (PlayerCropStructure)MapLevel.GetField(Walk.X, Walk.Y).FindNoneTransparentHS(typeof(PlayerCropStructure));
                }

                Aow.Game.World.Movement.HNWalkData.WalkToNextHN(ref Walk);
            }

            SetCropOwner(Found);
        }

        public new partial class Resource : ILTerrainMOR
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

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
            public Crop PlaceCrop(sbyte X, sbyte Y, sbyte L, PlayerCropStructure CropOwner)
            {
                Crop Result = new Crop(null);
                Result.SetResource(this);
                Result.ResourceLinked = true;
                Result.FCropOwner = CropOwner;
                if (!Aow.Game.World.Map.AoWHSMap.Current.PlaceHX(Result, X, Y, L))
                {
                    Result?.Dispose();
                    Result = null;
                }

                return Result;
            }

            // Crops.pas:254
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.Crop.Resource.CropRStr);

            private static readonly string CropRStr = "Crops";

        }

        // Crops.pas:68

    }

    [StructLayout(LayoutKind.Explicit, Size = 25)]
    public struct CropILFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public int ImageIndex;
        [FieldOffset(24)]
        public sbyte TerrainType;
    }
}
