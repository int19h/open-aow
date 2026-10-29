// Translated from ILTer.pas.
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
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.ILTer
{
    // { The ILTer unit of HSEPack.dpl: image library terrain, multi-hexagon objects that show one image
//   per terrain type (TILTerrainMO, ClassID $20186; resource TILTerrainMOR, $20187), and the fixed
//   variant that keeps its terrain type and updates the map terrain instead
//   (TFixedILTerrainMO, $20188; TFixedILTerrainMOR, $20189). }
    // { ILTer.TILTerrainMO (instance size 28): +$14 FImageIndex, +$18 FTerrainType }
    public partial class TILTerrainMO : Aow.Units.HSEngine.TMultiHexMO
    {
        // ILTer.pas:79
        public new const int ClassID = 131462;

        protected internal int FImageIndex;
        protected internal sbyte FTerrainType;
        // ILTer.pas:73
        // { TILTerrainMO }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainType = unchecked((sbyte)-1);
        }

        // ILTer.pas:164
        public override sbyte GetTerrain(int Index)
        {
            sbyte Result = default;
            var call__1 = FResource.TerrainList.GetTerrain(Index);
            Result = unchecked((sbyte)call__1);
            if (Result >= 0)
            {
                Result = unchecked((sbyte)FTerrainType);
            }

            return Result;
        }

        // ILTer.pas:131
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            bool Result = default;
            var call__2 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__2 >= 0)
            {
                var call__3 = GetMostFrequentMapFieldTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)FTerrainType));
                Result = call__3 == Info.MapField.TerrainType;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // ILTer.pas:152
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__4 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__4 >= 0)
            {
                var call__5 = ValidTerrainType(Info.TerrainType);
                if (call__5)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
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
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__7 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__7 >= 0)
            {
                var call__8 = ValidTerrainType(Info.TerrainType);
                if (call__8)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
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
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            // { Tried index shift/addition and receiver/Self casts; argument load order still differs. }
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
            if (this.FResource.ImageSequenceList == null)
            {
                var call__11 = this.FResource.ImageLibrary[FImageIndex];
                call__11.Show((Xhp - XhpSurf) + ((Aow.Units.HSEngine.TMultiHexMOR)this.FResource).dXImage, (Yhp - YhpSurf) + ((Aow.Units.HSEngine.TMultiHexMOR)this.FResource).dYImage, SurfaceDesc);
            }
            else
            {
                this.FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, FImageIndex, unchecked((uint)(Xhp + Yhp)));
            }
        }

        // ILTer.pas:184
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__12 = GetValidTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx));
                FTerrainType = unchecked((sbyte)call__12);
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
            var call__13 = GetValidTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx));
            FTerrainType = unchecked((sbyte)call__13);
            var call__14 = GetTerrainTypeImage(FTerrainType);
            FImageIndex = call__14;
        }

        // ILTer.pas:196
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            bool CanPlaceHere = default;
            int Count = default;
            sbyte TerrainType = default;
            int I = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            sbyte Overlay = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            Aow.Units.HSEngine.TTerrainList TerrainList = default;
            CanPlaceHere = true;
            Count = FResource.TerrainList.Count;
            var call__15 = MapContainer.GetMapLevel(Lhx);
            var call__16 = GetValidTerrainType(call__15, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx));
            FTerrainType = unchecked((sbyte)call__16);
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (CanPlaceHere && (Walk.HN < Count))
            {
                var call__17 = GetTerrain(Walk.HN);
                TerrainType = unchecked((sbyte)call__17);
                var call__18 = GetOverlay(Walk.HN);
                Overlay = unchecked((sbyte)call__18);
                if ((((((TerrainType >= 0) || (Overlay >= 0)) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < MapContainer.Width)) && (Walk.Y < MapContainer.Height))
                {
                    var call__19 = MapContainer.GetField(Walk.X, Walk.Y, Lhx);
                    var call__20 = call__19.GetMaxLevelNoneTransparentHS();
                    HS = call__20;
                    CanPlaceHere = (HS == null) || CanPlaceOnHS(HS);
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            if (CanPlaceHere)
            {
                TerrainList = new Aow.Units.HSEngine.TTerrainList();
                TerrainList.Assign(FResource.TerrainList);
                {
                    I = 0;
                    int end__21 = TerrainList.Count - 1;
                    for (; I <= end__21; I += 1)
                    {
                        if (TerrainList.GetTerrain(I) >= 0)
                        {
                            TerrainList.SetTerrain(I, unchecked((sbyte)FTerrainType));
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
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            if (Result)
            {
                Result = false;
                var call__24 = MapContainer.GetMapLevel(Lhx);
                var call__25 = GetValidTerrainType(call__24, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx));
                FTerrainType = unchecked((sbyte)call__25);
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
            Aow.Units.ImageLib.TImageSequenceList Sequences = default;
            // { Tried local order, index identity/shift, receiver cast and indexed property; argument order still differs. }
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
        public virtual sbyte ForceTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // ILTer.pas:107
        public virtual sbyte GetValidTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            int Count = default;
            Result = unchecked((sbyte)GetMostFrequentMapFieldTerrainType(MapLevel, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)-1)));
            if (!ValidTerrainType(Result))
            {
                Result = unchecked((sbyte)ForceTerrainType(MapLevel, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx)));
                if (GetEditMode() || (Result != -2))
                {
                    if (!ValidTerrainType(Result))
                    {
                        Result = 0;
                        var call__28 = FResource.Engine.HSSet.TerrainTypes.Count;
                        Count = call__28;
                        while ((Result < Count) && !ValidTerrainType(Result))
                        {
                            Result = (sbyte)(((int)Result) + 1);
                        }

                        if (Result == Count)
                        {
                            Result = unchecked((sbyte)-1);
                        }
                    }
                }
            }

            return Result;
        }

        public int ImageIndex { get => FImageIndex; set => FImageIndex = value; }
        public sbyte TerrainType { get => FTerrainType; }

        // ILTer.pas:20
        public static new Aow.Units.ILTer.TILTerrainMO Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ILTer.TILTerrainMO o = new Aow.Units.ILTer.TILTerrainMO();
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

        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // ILTer.pas:79
            public override int ClassID() => Aow.Units.ILTer.TILTerrainMO.ClassID;

            // ILTer.pas:20
            public new Aow.Units.ILTer.TILTerrainMO Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ILTer.TILTerrainMO)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.ILTer.TILTerrainMO => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ILTer.TILTerrainMOR (instance size 68) }
    public partial class TILTerrainMOR : Aow.Units.HSEngine.TMultiHexMOR
    {
        // ILTer.pas:269
        public new const int ClassID = 131463;

        // ILTer.pas:279
        public override void SetCellImage(Aow.Units.ImageLib.TLibraryImage Value)
        {
        }

        // ILTer.pas:274
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131462;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMultiHexMOR.Meta
        {
            // ILTer.pas:269
            public override int ClassID() => Aow.Units.ILTer.TILTerrainMOR.ClassID;

            // ILTer.pas:264
            // { TILTerrainMOR }
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"ImageLibrary Terrain";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ILTer.TILTerrainMOR => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.ILTer.TILTerrainMOR Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ILTer.TILTerrainMOR)base.Create(AOwner);
        }
    }

    // { ILTer.TFixedILTerrainMO (instance size 28) }
    public partial class TFixedILTerrainMO : Aow.Units.ILTer.TILTerrainMO
    {
        public new const int ClassID = 131464;

        // ILTer.pas:295
        public override bool CanChangeTerrain(in Aow.Units.HSEngine.THSSenderInfo Info)
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
                    var call__33 = GetMostFrequentMapFieldTerrainType((Aow.Units.HSEngine.TMapLevel)Owner.Owner, unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)call__32));
                    bool__31 = (call__33 == Info.MapField.TerrainType);
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
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__34 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__34 >= 0)
            {
                var call__35 = ValidTerrainType(Info.TerrainType);
                if (call__35)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
                    var call__36 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__36;
                }
            }
        }

        // ILTer.pas:315
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            var call__37 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__37 >= 0)
            {
                var call__38 = ValidTerrainType(Info.TerrainType);
                if (call__38)
                {
                    FTerrainType = unchecked((sbyte)Info.TerrainType);
                    UpdateTerrain((Aow.Units.HSEngine.TMapLevel)Owner.Owner, FXhx, FYhx);
                    var call__39 = GetTerrainTypeImage(Info.TerrainType);
                    FImageIndex = call__39;
                }
                else
                {
                    try
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"TILFixedTerrain Failure");
                    }
                    catch (System.Exception)
                    {
                        Destroy();
                    }
                }
            }
        }

        // ILTer.pas:290
        public override sbyte ForceTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // ILTer.pas:285
            // { TFixedILTerrainMO }
            public override int ClassID() => Aow.Units.ILTer.TFixedILTerrainMO.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.ILTer.TFixedILTerrainMO => (Meta)MetaRegistry.Of(typeof(T));
            // ILTer.pas:20
            public new Aow.Units.ILTer.TFixedILTerrainMO Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ILTer.TFixedILTerrainMO)base.Create(AOwner);
        }
    }

    // { ILTer.TFixedILTerrainMOR (instance size 68) }
    public partial class TFixedILTerrainMOR : Aow.Units.ILTer.TILTerrainMOR
    {
        // ILTer.pas:339
        public new const int ClassID = 131465;

        // ILTer.pas:344
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131464;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // ILTer.pas:339
            public override int ClassID() => Aow.Units.ILTer.TFixedILTerrainMOR.ClassID;

            // ILTer.pas:334
            // { TFixedILTerrainMOR }
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Fixed ImageLibrary Terrain";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ILTer.TFixedILTerrainMOR => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.ILTer.TFixedILTerrainMOR Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ILTer.TFixedILTerrainMOR)base.Create(AOwner);
        }
    }

    public static partial class ILTer
    {
        // ILTer.pas:349
        public static void RegisterILTerrain(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.ILTer.TILTerrainMO>(), Aow.Units.Engine.EClass.Of<Aow.Units.ILTer.TILTerrainMOR>()]);
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.ILTer.TFixedILTerrainMO>(), Aow.Units.Engine.EClass.Of<Aow.Units.ILTer.TFixedILTerrainMOR>()]);
        }
    }
}
