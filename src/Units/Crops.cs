// Translated from Crops.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.ILTer;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Crops
{
    // FIncome, FChangingCrops, FIncomeSource
    public partial class TPlayerCropStructure : TPlayerStructure
    {

        public TPlayerCropStructure(TECustomNode owner) : base(owner)
        {
            FChangingCrops = false;
            var call__1 = new TPlayerStructureIncomeSource(null);
            FIncomeSource = call__1;
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
            sbyte Player = default;
            base.Activate();
            Player = FPlayer;
            if (Player != -1)
            {
                var call__2 = AoWHSMap.FPlayers.GetPlayers(Player);
                call__2.RegisterIncomeSource(FIncomeSource);
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
            sbyte Player = default;
            Player = FPlayer;
            if (Player != -1)
            {
                var call__3 = AoWHSMap.FPlayers.GetPlayers(Player);
                call__3.UnregisterIncomeSource(FIncomeSource);
            }

            base.Deactivate();
        }

        // Crops.pas:560
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TMapLevel MapLevel = default;
            int I = default;
            int HN = default;
            int X = default;
            int Y = default;
            int NX = default;
            int NY = default;
            Result = base.CanPlace(MapContainer, Xhx, Yhx, Lhx);
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
                            var call__4 = MapLevel.GetField(NX, NY);
                            var call__5 = call__4.FindNoneTransparentHS(typeof(TPlayerCropStructure));
                            Result = call__5 == null;
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
            bool Result = default;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
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
                    var call__6 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__6.UnregisterIncomeSource(FIncomeSource);
                }

                base.SetPlayer(Player);
                UpdateCrops();
                if (FPlayer != -1)
                {
                    var call__7 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__7.RegisterIncomeSource(FIncomeSource);
                }
            }
        }

        // Crops.pas:440
        public virtual bool CanPlaceCrop(TMapField Field)
        {
            bool Result = default;
            TArmyHS Army = default;
            THexagonSprite HS = default;
            if (!GetRazed())
            {
                Army = (TArmyHS)Field.FindOwnedChild(131607);
                bool bool__8 = Army == null;
                if (!bool__8)
                {
                    var call__9 = AoWHSMap.FPlayers.GetDiplomaticRelation(Army.FArmy.FPlayer, FPlayer);
                    bool__8 = call__9 != TDiplomaticRelation.drWar;
                }

                Result = bool__8;
                if (Result)
                {
                    HS = Field.GetMaxLevelNoneTransparentHS();
                    Result = HS == null || HS.GetLevel() < 26;
                }

                if (Result)
                {
                    bool bool__10 = TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 4).Contains((byte)Field.TerrainType);
                    if (!bool__10)
                    {
                        bool bool__11 = Field.TerrainType == 5;
                        if (bool__11)
                        {
                            var call__12 = GetRace();
                            bool__11 = call__12 == 11;
                        }

                        bool__10 = bool__11;
                    }

                    Result = bool__10;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Crops.pas:320
        public virtual bool CropsEnabled()
        {
            bool Result = default;
            var call__13 = GetRazed();
            bool bool__14 = !call__13;
            if (bool__14)
            {
                var call__15 = GetBuilding();
                bool__14 = !call__15;
            }

            Result = bool__14;
            return Result;
        }

        // Crops.pas:314
        public virtual void RegrowCrops()
        {
            var call__16 = GetCropCount();
            if (call__16 < ((TPlayerCropStructureResource)FResource).FCropLocations.Count)
            {
                var call__17 = GetCropCount();
                SetCropCount(call__17 + 1);
            }
        }

        // Crops.pas:332
        public virtual void UpdateIncome()
        {
            var call__18 = GetRazed();
            bool bool__19 = !call__18;
            if (bool__19)
            {
                var call__20 = GetBuilding();
                bool__19 = !call__20;
            }

            if (bool__19)
            {
                var call__21 = GetCropCount();
                SetIncome((byte)(call__21 * 2));
            }
            else
            {
                SetIncome(0);
            }
        }

        // Crops.pas:401
        public virtual void UpdateCrops()
        {
            var call__22 = GetEditMode();
            if (!call__22 && FStatus.Contains(TMapObjectStatusElement.moActivated))
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
            sbyte Result = default;
            if (FPlayer > 0)
            {
                var call__23 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Result = call__23.FPlayerRace;
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
            int Race = default;
            if (Player != FPlayer && FPlayer > 0 && Player > 0)
            {
                var call__24 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Race = call__24.FPlayerRace;
                var call__25 = AoWHSMap.FRaces.GetRace(Race);
                var call__26 = call__25.GetPlayerRelationValue(Player);
                var call__27 = AoWHSMap.FRaces.GetRace(Race);
                call__27.SetPlayerRelationValue(Player, call__26 - -5);
            }
        }

        // Crops.pas:302
        public void SetIncome(byte A1)
        {
            sbyte Player = default;
            if (A1 != this.FIncome && this.FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                this.FIncome = A1;
                Player = this.FPlayer;
                if (Player != -1)
                {
                    var call__29 = AoWHSMap.FPlayers.GetPlayers(Player);
                    call__29.UpdateIncome();
                }
            }
        }

        // Crops.pas:382
        public void UpdateCropSettings()
        {
            TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            TCrop Crop = default;
            MapLevel = (TMapLevel)Owner.Owner;
            {
                I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__30 = 0;
                for (; I >= end__30; I -= 1)
                {
                    CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                    {
                        var call__31 = MapLevel.GetField(X, Y);
                        var call__32 = call__31.FindOwnedChild(131946);
                        Crop = (TCrop)call__32;
                        if (Crop != null)
                        {
                            Crop.UpdateSettings();
                        }
                    }
                }
            }
        }

        // Crops.pas:420
        public int GetCropCount()
        {
            int Result = default;
            TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            TCrop Crop = default;
            Result = 0;
            MapLevel = (TMapLevel)Owner.Owner;
            {
                I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__33 = 0;
                for (; I >= end__33; I -= 1)
                {
                    CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                    {
                        var call__34 = MapLevel.GetField(X, Y);
                        var call__35 = call__34.FindChild(131946);
                        Crop = (TCrop)call__35;
                        if (Crop != null)
                        {
                            if (Crop.FCropOwner == this)
                            {
                                Result += 1;
                            }
                        }
                    }
                }
            }

            return Result;
        }

        // Crops.pas:463
        public void SetCropCount(int A1)
        {
            TMapLevel MapLevel = default;
            int I = default;
            TCropResource Resource = default;
            TMapField Field = default;
            int X = default;
            int Y = default;
            TList Crops = default;
            TCrop Crop = default;
            TWordList Locations = default;
            if (FStatus.Contains(TMapObjectStatusElement.moPlacedOnMap) && !FChangingCrops)
            {
                FChangingCrops = true;
                MapLevel = (TMapLevel)Owner.Owner;
                Crops = new TList();
                Locations = new TWordList();
                Locations.Assign(((TPlayerCropStructureResource)FResource).FCropLocations);
                {
                    I = Locations.Count - 1;
                    byte end__36 = 0;
                    for (; I >= end__36; I -= 1)
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
                }

                if (Crops.Count < A1)
                {
                    var call__37 = FResource.Engine.HSSet.MapObjectResourceList.FindChild(131947);
                    Resource = (TCropResource)call__37;
                    if (Resource != null)
                    {
                        while (Crops.Count != A1 && Locations.Count != 0)
                        {
                            var call__38 = AoWHSMap.Random(Locations.Count);
                            I = call__38;
                            CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                            var call__39 = Resource.PlaceCrop((sbyte)X, (sbyte)Y, (sbyte)MapLevel.Lhx, this);
                            Crop = call__39;
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
                    if (Crops.Count > A1)
                    {
                        while (Crops.Count != A1 && Crops.Count != 0)
                        {
                            var call__40 = AoWHSMap.Random(Crops.Count);
                            I = call__40;
                            var call__41 = Crops[I];
                            ((TCrop)call__41).Destroy();
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
            TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            TCrop Crop = default;
            if (GetEditMode())
            {
                return;
            }

            MapLevel = (TMapLevel)Owner.Owner;
            {
                I = ((TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__42 = 0;
                for (; I >= end__42; I -= 1)
                {
                    CenterHNtoHX(FXhx, FYhx, ((TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if (X >= 0 && MapLevel.Width > X && Y >= 0 && MapLevel.Height > Y)
                    {
                        var call__43 = MapLevel.GetField(X, Y);
                        var call__44 = call__43.FindChild(131946);
                        Crop = (TCrop)call__44;
                        if (Crop != null)
                        {
                            Crop.SetCropOwner(this);
                        }
                    }
                }
            }
        }

        // Crops.pas:27


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

                var call__45 = TerrainList.CreateSurroundingHNTable();
                FCropLocations = call__45;
            }
        }

        // Crops.pas:602
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131918;
            return Result;
        }

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
                EndTurn((sbyte)Msg.Payload.Value);
            }
        }

        // Crops.pas:203
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            THexagonSprite HS = default;
            if (Info.HN != 0)
            {
                return;
            }

            if (FStatus.Contains(TMapObjectStatusElement.moActivated))
            {
                var call__46 = ValidTerrainType(Info.TerrainType);
                if (call__46)
                {
                    FTerrainType = Info.TerrainType;
                    var call__47 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__47;
                    var call__48 = Info.MapField.GetMaxLevelNoneTransparentHS();
                    HS = call__48;
                    bool bool__49 = HS != null;
                    if (bool__49)
                    {
                        var call__50 = HS.GetLevel();
                        var call__51 = GetLevel();
                        bool__49 = call__50 > call__51;
                    }

                    if (bool__49 && !(HS is TAbstractRoad) || Info.OverlayType >= 0 && Info.OverlayType != 4)
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
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // Crops.pas:234
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TMapField Field = default;
            THexagonSprite HS = default;
            Field = MapContainer.GetField(Xhx, Yhx, Lhx);
            HS = Field.GetMaxLevelNoneTransparentHS();
            var call__53 = Field.FindOwnedHS(typeof(TFire));
            bool bool__54 = call__53 == null;
            if (bool__54)
            {
                bool bool__55 = HS == null;
                if (!bool__55)
                {
                    var call__56 = HS.GetLevel();
                    bool__55 = call__56 < 10;
                }

                bool__54 = bool__55 || HS is TAbstractRoad;
            }

            Result = bool__54;
            return Result;
        }

        // Crops.pas:228
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            bool bool__57 = FCropOwner != null;
            if (bool__57)
            {
                var call__58 = FCropOwner.GetRace();
                bool__57 = call__58 == 11;
            }

            if (bool__57)
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
            int Index = default;
            var call__59 = GetTerrainTypeImage(TerrainType);
            Index = call__59;
            this.FImageIndex = Index;
            var call__60 = FResource.ImageLibrary.ImageExist(Index);
            if (!call__60)
            {
                Destroy();
            }
        }

        // Crops.pas:136
        public void EndTurn(sbyte Player)
        {
            sbyte PlayerIndex = default;
            TArmyHS Army = default;
            var call__61 = ((TAoWMapField)Owner).GetArmyHS();
            Army = call__61;
            PlayerIndex = Player;
            if (Army != null && Army.FArmy.FPlayer == PlayerIndex && FCropOwner.FPlayer > 0 && PlayerIndex != FCropOwner.FPlayer)
            {
                var call__62 = AoWHSMap.FPlayers.GetDiplomaticRelation(FCropOwner.FPlayer, PlayerIndex);
                if (call__62 == TDiplomaticRelation.drWar)
                {
                    FCropOwner.CropTrampled(PlayerIndex);
                    Destroy();
                }
            }
        }

        // Crops.pas:163
        public void LinkToCropOwner()
        {
            TPlayerCropStructure Found = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            Found = null;
            MapLevel = (TMapLevel)Owner.Owner;
            InitHNWalk(ref Walk, FXhx, FYhx, 1);
            while (Walk.Radius <= 1 && Found == null)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var call__63 = MapLevel.GetField(Walk.X, Walk.Y);
                    var call__64 = call__63.FindNoneTransparentHS(typeof(TPlayerCropStructure));
                    Found = (TPlayerCropStructure)call__64;
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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131946;
            return Result;
        }

        // Crops.pas:248
        public override void SetLevel(byte Value)
        {
            base.SetLevel(5);
        }

        // Crops.pas:257
        public TCrop PlaceCrop(sbyte X, sbyte Y, sbyte L, TPlayerCropStructure CropOwner)
        {
            TCrop Result = default;
            Result = new TCrop(null);
            Result.SetResource(this);
            Result.ResourceLinked = true;
            Result.FCropOwner = CropOwner;
            var call__65 = AoWHSMap.PlaceHX(Result, X, Y, L);
            if (!call__65)
            {
                Result.Free();
                Result = null;
            }

            return Result;
        }

        // Crops.pas:254
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(CropRStr);
            return Result;
        }

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

    public static partial class Crops
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
}
