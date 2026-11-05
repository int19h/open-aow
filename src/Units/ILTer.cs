// Translated from ILTer.pas.
using Aow.Rtl;
using Exception = Aow.Rtl.Exception;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.ILTer
{
    // { The ILTer unit of HSEPack.dpl: image library terrain, multi-hexagon objects that show one image
//   per terrain type (TILTerrainMO, ClassID $20186; resource TILTerrainMOR, $20187), and the fixed
//   variant that keeps its terrain type and updates the map terrain instead
//   (TFixedILTerrainMO, $20188; TFixedILTerrainMOR, $20189). }
    // FImageIndex, FTerrainType
    public partial class TILTerrainMO : TMultiHexMO
    {

        public TILTerrainMO(TECustomNode owner) : base(owner)
        {
            FTerrainType = -1;
        }

        // ILTer.pas:79
        public new const int ClassID = 131462;

        protected internal int FImageIndex;
        protected internal sbyte FTerrainType;


        // ILTer.pas:164
        public override sbyte GetTerrain(int Index)
        {
            sbyte Result = default;
            var call__1 = FResource.TerrainList.GetTerrain(Index);
            Result = call__1;
            if (Result >= 0)
            {
                Result = FTerrainType;
            }

            return Result;
        }

        // ILTer.pas:131
        public override bool CanChangeTerrain(in THSSenderInfo Info)
        {
            bool Result = default;
            var call__2 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__2 >= 0)
            {
                var call__3 = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, FTerrainType);
                Result = call__3 == Info.MapField.TerrainType;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // ILTer.pas:152
        public override void ChangeTerrain(in THSTerrainInfo Info)
        {
            var call__4 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__4 >= 0)
            {
                var call__5 = ValidTerrainType(Info.TerrainType);
                if (call__5)
                {
                    FTerrainType = Info.TerrainType;
                    var call__6 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__6;
                }
                else
                {
                    Destroy();
                }
            }
        }

        // ILTer.pas:140
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            var call__7 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__7 >= 0)
            {
                var call__8 = ValidTerrainType(Info.TerrainType);
                if (call__8)
                {
                    FTerrainType = Info.TerrainType;
                    var call__9 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__9;
                }
                else
                {
                    Destroy();
                }
            }
        }

        // ILTer.pas:170
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            if (this.FResource.ImageSequenceList == null)
            {
                var call__11 = this.FResource.ImageLibrary[FImageIndex];
                call__11.Show(Xhp - XhpSurf + ((TMultiHexMOR)this.FResource).dXImage, Yhp - YhpSurf + ((TMultiHexMOR)this.FResource).dYImage, SurfaceDesc);
            }
            else
            {
                this.FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, FImageIndex, (uint)(Xhp + Yhp));
            }
        }

        // ILTer.pas:184
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__12 = GetValidTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx);
                FTerrainType = call__12;
            }

            return Result;
        }

        // ILTer.pas:191
        public override bool Loaded()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            return Result;
        }

        // ILTer.pas:255
        public override void Activate()
        {
            base.Activate();
            var call__13 = GetValidTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx);
            FTerrainType = call__13;
            var call__14 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__14;
        }

        // ILTer.pas:196
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            bool CanPlaceHere = default;
            int Count = default;
            sbyte TerrainType = default;
            int I = default;
            THNWalkData Walk = default;
            sbyte Overlay = default;
            THexagonSprite HS = default;
            TTerrainList TerrainList = default;
            CanPlaceHere = true;
            Count = FResource.TerrainList.Count;
            var call__15 = MapContainer.GetMapLevel(Lhx);
            var call__16 = GetValidTerrainType(call__15, Xhx, Yhx);
            FTerrainType = call__16;
            InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (CanPlaceHere && Walk.HN < Count)
            {
                var call__17 = GetTerrain(Walk.HN);
                TerrainType = call__17;
                var call__18 = GetOverlay(Walk.HN);
                Overlay = call__18;
                if ((TerrainType >= 0 || Overlay >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapContainer.Width && Walk.Y < MapContainer.Height)
                {
                    var call__19 = MapContainer.GetField(Walk.X, Walk.Y, Lhx);
                    var call__20 = call__19.GetMaxLevelNoneTransparentHS();
                    HS = call__20;
                    CanPlaceHere = HS == null || CanPlaceOnHS(HS);
                }

                WalkToNextHN(ref Walk);
            }

            if (CanPlaceHere)
            {
                TerrainList = new TTerrainList();
                TerrainList.Assign(FResource.TerrainList);
                {
                    I = 0;
                    int end__21 = TerrainList.Count - 1;
                    for (; I <= end__21; I += 1)
                    {
                        if (TerrainList.GetTerrain(I) >= 0)
                        {
                            TerrainList.SetTerrain(I, FTerrainType);
                        }
                    }
                }

                bool bool__22 = CanPlaceHere;
                if (bool__22)
                {
                    var call__23 = MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, TerrainList);
                    bool__22 = call__23;
                }

                CanPlaceHere = bool__22;
                TerrainList.Free();
            }

            Result = CanPlaceHere;
            return Result;
        }

        // ILTer.pas:236
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            if (Result)
            {
                Result = false;
                var call__24 = MapContainer.GetMapLevel(Lhx);
                var call__25 = GetValidTerrainType(call__24, Xhx, Yhx);
                FTerrainType = call__25;
                if (FTerrainType >= 0)
                {
                    var call__26 = GetTerrainTypeImage(FTerrainType);
                    FImageIndex = call__26;
                    if (FImageIndex != -1)
                    {
                        if (!GetEditMode())
                        {
                            OverrideTerrain();
                        }

                        Result = true;
                    }
                }
            }

            return Result;
        }

        // ILTer.pas:89
        public virtual bool ValidTerrainType(int TerrainType)
        {
            bool Result = default;
            int Index = default;
            TImageSequenceList Sequences = default;
            if (TerrainType >= 0)
            {
                Index = GetTerrainTypeImage(TerrainType);
                Sequences = FResource.ImageSequenceList;
                if (Sequences == null)
                {
                    var call__27 = FResource.ImageLibrary.ImageExist(Index);
                    Result = call__27;
                }
                else
                {
                    Result = Sequences.GetImageSequence(Index) != null;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // ILTer.pas:126
        public virtual int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            return Result;
        }

        // ILTer.pas:84
        public virtual sbyte ForceTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // ILTer.pas:107
        public virtual sbyte GetValidTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            int Count = default;
            Result = GetMostFrequentMapFieldTerrainType(MapLevel, Xhx, Yhx, -1);
            if (!ValidTerrainType(Result))
            {
                Result = ForceTerrainType(MapLevel, Xhx, Yhx);
                if (GetEditMode() || Result != -2)
                {
                    if (!ValidTerrainType(Result))
                    {
                        Result = 0;
                        var call__28 = FResource.Engine.HSSet.TerrainTypes.Count;
                        Count = call__28;
                        while (Result < Count && !ValidTerrainType(Result))
                        {
                            Result = (sbyte)(Result + 1);
                        }

                        if (Result == Count)
                        {
                            Result = -1;
                        }
                    }
                }
            }

            return Result;
        }

        public int ImageIndex { get => FImageIndex; set => FImageIndex = value; }
        public sbyte TerrainType { get => FTerrainType; }

        // ILTer.pas:20


    }

    public partial class TILTerrainMOR : TMultiHexMOR
    {
        public TILTerrainMOR(TECustomNode owner) : base(owner)
        {}

        // ILTer.pas:269
        public new const int ClassID = 131463;

        // ILTer.pas:279
        public override void SetCellImage(ImageLib.TLibraryImage Value)
        {
        }

        // ILTer.pas:274
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131462;
            return Result;
        }

        // ILTer.pas:264
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"ImageLibrary Terrain";
            return Result;
        }

    }

    public partial class TFixedILTerrainMO : TILTerrainMO
    {

        public TFixedILTerrainMO(TECustomNode owner) : base(owner)
        {}


        public new const int ClassID = 131464;

        // ILTer.pas:295
        public override bool CanChangeTerrain(in THSSenderInfo Info)
        {
            bool Result = default;
            var call__29 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__29 >= 0)
            {
                var call__30 = ValidTerrainType(Info.MapField.TerrainType);
                bool bool__31 = call__30;
                if (bool__31)
                {
                    var call__32 = FResource.TerrainList.GetTerrain(Info.HN);
                    var call__33 = GetMostFrequentMapFieldTerrainType((TMapLevel)Owner.Owner, FXhx, FYhx, call__32);
                    bool__31 = call__33 == Info.MapField.TerrainType;
                }

                Result = bool__31;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // ILTer.pas:305
        public override void ChangeTerrain(in THSTerrainInfo Info)
        {
            var call__34 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__34 >= 0)
            {
                var call__35 = ValidTerrainType(Info.TerrainType);
                if (call__35)
                {
                    FTerrainType = Info.TerrainType;
                    var call__36 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__36;
                }
            }
        }

        // ILTer.pas:315
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            var call__37 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__37 >= 0)
            {
                var call__38 = ValidTerrainType(Info.TerrainType);
                if (call__38)
                {
                    FTerrainType = Info.TerrainType;
                    UpdateTerrain((TMapLevel)Owner.Owner, FXhx, FYhx);
                    var call__39 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__39;
                }
                else
                {
                    try
                    {
                        throw Exception.Create((AnsiString)"TILFixedTerrain Failure");
                    }
                    catch (System.Exception)
                    {
                        Destroy();
                    }
                }
            }
        }

        // ILTer.pas:290
        public override sbyte ForceTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

    }

    public partial class TFixedILTerrainMOR : TILTerrainMOR
    {
        public TFixedILTerrainMOR(TECustomNode owner) : base(owner)
        {}


        // ILTer.pas:339
        public new const int ClassID = 131465;

        // ILTer.pas:344
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131464;
            return Result;
        }

        // ILTer.pas:334
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Fixed ImageLibrary Terrain";
            return Result;
        }

    }

    public static partial class ILTer
    {
        // ILTer.pas:349
        public static void RegisterILTerrain(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TILTerrainMO>(), EClass.Of<TILTerrainMOR>()]);
            Engine.RegisterEClasses([EClass.Of<TFixedILTerrainMO>(), EClass.Of<TFixedILTerrainMOR>()]);
        }
    }
}
