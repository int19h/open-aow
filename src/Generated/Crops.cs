// Generated from Crops.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Crops
{
    // { TPlayerCropStructure (instance size 68, VMT 557A5634): +$3C FIncome,
//     +$3D FChangingCrops, +$40 FIncomeSource. }
    public partial class TPlayerCropStructure : Aow.Units.AoWE.TPlayerStructure
    {
        public byte FIncome;
        public bool FChangingCrops;
        public Aow.Units.FixedArray_byte_2 FReserved3E;
        public Aow.Units.AoWE.TPlayerStructureIncomeSource FIncomeSource;
        // Crops.pas:278
        public override void Destroy()
        {
            base.Destroy();
            FIncomeSource.Free();
        }

        // Crops.pas:270
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FChangingCrops = false;
            var call__1 = Aow.Units.AoWE.TPlayerStructureIncomeSource.Create(null);
            FIncomeSource = call__1;
            FIncomeSource.Setup(this);
        }

        // Crops.pas:528
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            if (TSet<Aow.Units.HSEngine.THSMapStatusElement>.Of(Aow.Units.HSEngine.THSMapStatusElement.msEditting, Aow.Units.HSEngine.THSMapStatusElement.msActivated).IsSubsetOf(Aow.Units.AoWE.AoWE.AoWHSMap.Status) && !FChangingCrops)
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
            Player = unchecked((sbyte)FPlayer);
            if (Player != -1)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
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
            Player = unchecked((sbyte)FPlayer);
            if (Player != -1)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                call__3.UnregisterIncomeSource(FIncomeSource);
            }

            base.Deactivate();
        }

        // Crops.pas:560
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int I = default;
            int HN = default;
            int X = default;
            int Y = default;
            int NX = default;
            int NY = default;
            Result = base.CanPlace(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            if (Result && !GetEditMode())
            {
                MapLevel = MapContainer.GetMapLevel(Lhx);
                I = ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count;
                while (Result && (I != 0))
                {
                    I -= 1;
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    NX = X;
                    NY = Y;
                    HN = 0;
                    while (Result && (HN != 7))
                    {
                        if ((((NX >= 0) && (NX < MapLevel.Width)) && (NY >= 0)) && (NY < MapLevel.Height))
                        {
                            var call__4 = MapLevel.GetField(NX, NY);
                            var call__5 = call__4.FindNoneTransparentHS(typeof(Aow.Units.Crops.TPlayerCropStructure));
                            Result = call__5 == null;
                        }

                        HN += 1;
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
                    }
                }
            }

            return Result;
        }

        // Crops.pas:586
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
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
            base.NewTurn(unchecked((sbyte)Player));
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
                    var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__6.UnregisterIncomeSource(FIncomeSource);
                }

                base.SetPlayer(unchecked((sbyte)Player));
                UpdateCrops();
                if (FPlayer != -1)
                {
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__7.RegisterIncomeSource(FIncomeSource);
                }
            }
        }

        // Crops.pas:440
        public virtual bool CanPlaceCrop(Aow.Units.HSEngine.TMapField Field)
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            if (!GetRazed())
            {
                Army = (Aow.Units.AoWE.TArmyHS)Field.FindOwnedChild(131607);
                bool bool__8 = (Army == null);
                if (!bool__8)
                {
                    var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)Army.FArmy.FPlayer), unchecked((sbyte)FPlayer));
                    bool__8 = (call__9 != Aow.Units.AoWE.TDiplomaticRelation.drWar);
                }

                Result = bool__8;
                if (Result)
                {
                    HS = Field.GetMaxLevelNoneTransparentHS();
                    Result = (HS == null) || (HS.GetLevel() < 26);
                }

                if (Result)
                {
                    bool bool__10 = TSet<byte>.WithBounds(0, 255).With(12).WithRange(1, 4).Contains((byte)Field.TerrainType);
                    if (!bool__10)
                    {
                        bool bool__11 = (Field.TerrainType == 5);
                        if (bool__11)
                        {
                            var call__12 = GetRace();
                            bool__11 = (call__12 == 11);
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
            if (call__16 < ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count)
            {
                var call__17 = GetCropCount();
                SetCropCount(call__17 + 1);
            }
        }

        // Crops.pas:332
        // { +22C draft }
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
                SetIncome(unchecked((byte)(call__21 * 2)));
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
            if (!call__22 && FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                {
                    if (CropsEnabled())
                    {
                        SetCropCount(((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count);
                    }
                }

                UpdateCropSettings();
                UpdateIncome();
            }
        }

        // Crops.pas:413
        // { +234 draft }
        public virtual sbyte GetRace()
        {
            sbyte Result = default;
            if (FPlayer > 0)
            {
                var call__23 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Result = unchecked((sbyte)call__23.FPlayerRace);
            }
            else
            {
                Result = unchecked((sbyte)-1);
            }

            return Result;
        }

        // Crops.pas:339
        public virtual void CropsChanged()
        {
            UpdateIncome();
        }

        // Crops.pas:290
        // { +23C draft }
        public virtual void CropTrampled(sbyte Player)
        {
            int Race = default;
            if (((Player != FPlayer) && (FPlayer > 0)) && (Player > 0))
            {
                var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Race = call__24.FPlayerRace;
                var call__25 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                var call__26 = call__25.GetPlayerRelationValue(unchecked((sbyte)Player));
                var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                call__27.SetPlayerRelationValue(unchecked((sbyte)Player), call__26 - -5);
            }
        }

        // Crops.pas:302
        public void SetIncome(byte A1)
        {
            sbyte Player = default;
            {
                var w__28 = this;
                if ((A1 != w__28.FIncome) && w__28.FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
                {
                    w__28.FIncome = unchecked((byte)A1);
                    Player = unchecked((sbyte)w__28.FPlayer);
                    if (Player != -1)
                    {
                        var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        call__29.UpdateIncome();
                    }
                }
            }
        }

        // Crops.pas:382
        // { draft }
        public void UpdateCropSettings()
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            Aow.Units.Crops.TCrop Crop = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            {
                I = ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__30 = 0;
                for (; I >= end__30; I -= 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if ((((X >= 0) && (MapLevel.Width > X)) && (Y >= 0)) && (MapLevel.Height > Y))
                    {
                        var call__31 = MapLevel.GetField(X, Y);
                        var call__32 = call__31.FindOwnedChild(131946);
                        Crop = (Aow.Units.Crops.TCrop)call__32;
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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            Aow.Units.Crops.TCrop Crop = default;
            Result = 0;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            {
                I = ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__33 = 0;
                for (; I >= end__33; I -= 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if ((((X >= 0) && (MapLevel.Width > X)) && (Y >= 0)) && (MapLevel.Height > Y))
                    {
                        var call__34 = MapLevel.GetField(X, Y);
                        var call__35 = call__34.FindChild(131946);
                        Crop = (Aow.Units.Crops.TCrop)call__35;
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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int I = default;
            Aow.Units.Crops.TCropResource Resource = default;
            Aow.Units.HSEngine.TMapField Field = default;
            int X = default;
            int Y = default;
            Aow.Rtl.TList Crops = default;
            Aow.Units.Crops.TCrop Crop = default;
            Aow.Units.Engine.TWordList Locations = default;
            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap) && !FChangingCrops)
            {
                FChangingCrops = true;
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                Crops = new Aow.Rtl.TList();
                Locations = Aow.Units.Engine.TWordList.Create();
                Locations.Assign(((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations);
                {
                    I = Locations.Count - 1;
                    byte end__36 = 0;
                    for (; I >= end__36; I -= 1)
                    {
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                        if ((((X >= 0) && (X < MapLevel.Width)) && (Y >= 0)) && (Y < MapLevel.Height))
                        {
                            Field = MapLevel.GetField(X, Y);
                            Crop = (Aow.Units.Crops.TCrop)Field.FindOwnedChild(131946);
                            if ((Crop != null) || !CanPlaceCrop(Field))
                            {
                                if ((Crop != null) && (Crop.FCropOwner == this))
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
                    Resource = (Aow.Units.Crops.TCropResource)call__37;
                    if (Resource != null)
                    {
                        while ((Crops.Count != A1) && (Locations.Count != 0))
                        {
                            var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Locations.Count);
                            I = call__38;
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, Locations.List[I], ref X, ref Y);
                            var call__39 = Resource.PlaceCrop(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)MapLevel.Lhx), this);
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
                        while ((Crops.Count != A1) && (Crops.Count != 0))
                        {
                            var call__40 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Crops.Count);
                            I = call__40;
                            var call__41 = Crops[I];
                            ((Aow.Units.Crops.TCrop)call__41).Destroy();
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
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int I = default;
            int X = default;
            int Y = default;
            Aow.Units.Crops.TCrop Crop = default;
            if (GetEditMode())
            {
                return;
            }

            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            {
                I = ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.Count - 1;
                byte end__42 = 0;
                for (; I >= end__42; I -= 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FXhx, FYhx, ((Aow.Units.Crops.TPlayerCropStructureResource)FResource).FCropLocations.List[I], ref X, ref Y);
                    if ((((X >= 0) && (MapLevel.Width > X)) && (Y >= 0)) && (MapLevel.Height > Y))
                    {
                        var call__43 = MapLevel.GetField(X, Y);
                        var call__44 = call__43.FindChild(131946);
                        Crop = (Aow.Units.Crops.TCrop)call__44;
                        if (Crop != null)
                        {
                            Crop.SetCropOwner(this);
                        }
                    }
                }
            }
        }

        // Crops.pas:27
        public static new Aow.Units.Crops.TPlayerCropStructure Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Crops.TPlayerCropStructure o = new Aow.Units.Crops.TPlayerCropStructure();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructure.Meta
        {
            // Crops.pas:284
            public override int ClassID()
            {
                int Result = default;
                Result = 131918;
                return Result;
            }

            // Crops.pas:27
            public new Aow.Units.Crops.TPlayerCropStructure Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Crops.TPlayerCropStructure o = (Aow.Units.Crops.TPlayerCropStructure)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Crops.TPlayerCropStructure => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { draft }
    // { TPlayerCropStructureResource (instance size 120, VMT 557A58FC): +$74 FCropLocations. }
    public partial class TPlayerCropStructureResource : Aow.Units.AoWE.TPlayerStructureResource
    {
        public Aow.Units.Engine.TWordList FCropLocations;
        // Crops.pas:592
        public override void Destroy()
        {
            FCropLocations.Free();
            FCropLocations = null;
            base.Destroy();
        }

        // Crops.pas:605
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
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

        public new partial class Meta : Aow.Units.AoWE.TPlayerStructureResource.Meta
        {
            // Crops.pas:599
            public override int ClassID()
            {
                int Result = default;
                Result = 131919;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Crops.TPlayerCropStructureResource => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:300
            public new Aow.Units.Crops.TPlayerCropStructureResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Crops.TPlayerCropStructureResource o = (Aow.Units.Crops.TPlayerCropStructureResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TCrop (instance size 32, VMT 557A5A10): +$1C FCropOwner. }
    public partial class TCrop : Aow.Units.ILTer.TILTerrainMO
    {
        public Aow.Units.Crops.TPlayerCropStructure FCropOwner;
        // Crops.pas:114
        public override void Destroy()
        {
            base.Destroy();
        }

        // Crops.pas:109
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // Crops.pas:151
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001991)
            {
                EndTurn(unchecked((sbyte)((sbyte)Msg.Payload.Value)));
            }
        }

        // Crops.pas:203
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            Aow.Units.HSEngine.THexagonSprite HS = default;
            if (Info.HN != 0)
            {
                return;
            }

            if (FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
            {
                var call__46 = ValidTerrainType(Info.TerrainType);
                if (call__46)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
                    var call__47 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__47;
                    var call__48 = Info.MapField.GetMaxLevelNoneTransparentHS();
                    HS = call__48;
                    bool bool__49 = (HS != null);
                    if (bool__49)
                    {
                        var call__50 = HS.GetLevel();
                        var call__51 = GetLevel();
                        bool__49 = (call__50 > call__51);
                    }

                    if ((bool__49 && !(HS is Aow.Units.HSEngine.TAbstractRoad)) || ((unchecked((short)Info.OverlayType) >= 0) && (unchecked((short)Info.OverlayType) != 4)))
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
                FTerrainType = unchecked((sbyte)Info.TerrainType);
            }
        }

        // Crops.pas:157
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
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
            if ((FCropOwner != null) && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msDestroying))
            {
                {
                    var w__52 = FCropOwner;
                    this.FCropOwner = null;
                    w__52.CropsChanged();
                }
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
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapField Field = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            Field = MapContainer.GetField(Xhx, Yhx, Lhx);
            HS = Field.GetMaxLevelNoneTransparentHS();
            var call__53 = Field.FindOwnedHS(typeof(Aow.Units.AoWE.TFire));
            bool bool__54 = (call__53 == null);
            if (bool__54)
            {
                bool bool__55 = (HS == null);
                if (!bool__55)
                {
                    var call__56 = HS.GetLevel();
                    bool__55 = (call__56 < 10);
                }

                bool__54 = (bool__55 || (HS is Aow.Units.HSEngine.TAbstractRoad));
            }

            Result = bool__54;
            return Result;
        }

        // Crops.pas:228
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            bool bool__57 = (FCropOwner != null);
            if (bool__57)
            {
                var call__58 = FCropOwner.GetRace();
                bool__57 = (call__58 == 11);
            }

            if (bool__57)
            {
                Result += 30;
            }

            return Result;
        }

        // Crops.pas:122
        public void SetCropOwner(Aow.Units.Crops.TPlayerCropStructure Value)
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
        // { draft }
        public void EndTurn(sbyte Player)
        {
            sbyte PlayerIndex = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            var call__61 = ((Aow.Units.AoWE.TAoWMapField)Owner).GetArmyHS();
            Army = call__61;
            PlayerIndex = unchecked((sbyte)Player);
            if ((((Army != null) && (Army.FArmy.FPlayer == PlayerIndex)) && (FCropOwner.FPlayer > 0)) && (PlayerIndex != FCropOwner.FPlayer))
            {
                var call__62 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)FCropOwner.FPlayer), unchecked((sbyte)PlayerIndex));
                if (call__62 == Aow.Units.AoWE.TDiplomaticRelation.drWar)
                {
                    FCropOwner.CropTrampled(unchecked((sbyte)PlayerIndex));
                    Destroy();
                }
            }
        }

        // Crops.pas:163
        public void LinkToCropOwner()
        {
            Aow.Units.Crops.TPlayerCropStructure Found = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Found = null;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 1);
            while ((Walk.Radius <= 1) && (Found == null))
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                {
                    var call__63 = MapLevel.GetField(Walk.X, Walk.Y);
                    var call__64 = call__63.FindNoneTransparentHS(typeof(Aow.Units.Crops.TPlayerCropStructure));
                    Found = (Aow.Units.Crops.TPlayerCropStructure)call__64;
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            SetCropOwner(Found);
        }

        // Crops.pas:68
        public static new Aow.Units.Crops.TCrop Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Crops.TCrop o = new Aow.Units.Crops.TCrop();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // Crops.pas:119
            public override int ClassID()
            {
                int Result = default;
                Result = 131946;
                return Result;
            }

            // Crops.pas:68
            public new Aow.Units.Crops.TCrop Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Crops.TCrop o = (Aow.Units.Crops.TCrop)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Crops.TCrop => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { draft }
    // { TCropResource (instance size 68, VMT 557A5BB0) }
    public partial class TCropResource : Aow.Units.ILTer.TILTerrainMOR
    {
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
        public Aow.Units.Crops.TCrop PlaceCrop(sbyte X, sbyte Y, sbyte L, Aow.Units.Crops.TPlayerCropStructure CropOwner)
        {
            Aow.Units.Crops.TCrop Result = default;
            Result = Aow.Units.Crops.TCrop.Create(null);
            Result.SetResource(this);
            Result.ResourceLinked = true;
            Result.FCropOwner = CropOwner;
            var call__65 = Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Result, X, Y, L);
            if (!call__65)
            {
                Result.Free();
                Result = null;
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // Crops.pas:245
            public override int ClassID()
            {
                int Result = default;
                Result = 131947;
                return Result;
            }

            // Crops.pas:254
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CropRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Crops.TCropResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.Crops.TCropResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Crops.TCropResource o = (Aow.Units.Crops.TCropResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 25)]
    public struct TCropILFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_20 Reserved;
        [FieldOffset(20)]
        public int ImageIndex;
        [FieldOffset(24)]
        public sbyte TerrainType;
    }

    public static partial class Crops
    {
        // Crops.pas:615
        public static void RegisterCrops(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Crops.TCrop>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Crops.TCropResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Crops.TPlayerCropStructure>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Crops.TPlayerCropStructureResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
