// Translated from CombatObstacle.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
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
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatObstacle
{
    // { TCombatObstacleShadow (instance size 12, VMT 00431888) }
    public partial class TCombatObstacleShadow : Aow.Units.HSEngine.THexagonSprite
    {
        public Aow.Units.CombatObstacle.TCombatObstacle FObstacle;
        // CombatObstacle.pas:74
        // { $08 }
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FObstacle.GetShowPriority();
            Result = (call__1 & 67108863) - 1677721600;
            return Result;
        }

        // CombatObstacle.pas:79
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            FObstacle.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524
        public static new Aow.Units.CombatObstacle.TCombatObstacleShadow Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatObstacle.TCombatObstacleShadow o = new Aow.Units.CombatObstacle.TCombatObstacleShadow();
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

    // { TCombatObstacleOverlay (instance size 12, VMT 00431A20) }
    public partial class TCombatObstacleOverlay : Aow.Units.HSEngine.THexagonSprite
    {
        public Aow.Units.CombatObstacle.TCombatObstacle FObstacle;
        // CombatObstacle.pas:84
        // { $08 }
        public override int GetShowPriority()
        {
            int Result = default;
            var call__2 = FObstacle.GetShowPriority();
            Result = (call__2 & 67108863) + 393216;
            return Result;
        }

        // CombatObstacle.pas:89
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            FObstacle.ShowOverlay(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524
        public static new Aow.Units.CombatObstacle.TCombatObstacleOverlay Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.CombatObstacle.TCombatObstacleOverlay o = new Aow.Units.CombatObstacle.TCombatObstacleOverlay();
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

    // { TCombatObstacle (instance size 28, VMT 00431BB8) }
    public partial class TCombatObstacle : Aow.Units.HSEngine.TMultiHexMO
    {
        public Aow.Units.CombatObstacle.TCombatObstacleShadow FShadow;
        // { $14 }
        public Aow.Units.CombatObstacle.TCombatObstacleOverlay FOverlay;
        // CombatObstacle.pas:97
        // { $18 }
        public override void Destroy()
        {
            DestroyShadow();
            DestroyOverlay();
            base.Destroy();
        }

        // CombatObstacle.pas:110
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__3 = Yhx;
                var call__4 = Xhx;
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__4, call__3);
            }

            return Result;
        }

        // CombatObstacle.pas:116
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatObstacle.pas:229
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            var call__5 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
            if (call__5)
            {
                var call__6 = FResource.TerrainList.GetOverlay(Info.HN);
                if (call__6 == 8)
                {
                    MoveResult.Code = -1;
                    MoveResult.Done = true;
                }
            }
        }

        // CombatObstacle.pas:208
        public override void Activate()
        {
            base.Activate();
            CreateShadow();
            CreateOverlay();
        }

        // CombatObstacle.pas:215
        public override void Deactivate()
        {
            DestroyShadow();
            DestroyOverlay();
            base.Deactivate();
        }

        // CombatObstacle.pas:222
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN < FResource.TerrainList.Count)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }

        // CombatObstacle.pas:193
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if (FShadow != null)
                {
                    var call__7 = FShadow.GetShowPriority();
                    VisibilityList.AddSortChild(FShadow, call__7);
                }

                if (FOverlay != null)
                {
                    var call__8 = FOverlay.GetShowPriority();
                    VisibilityList.AddSortChild(FOverlay, call__8);
                }
            }
        }

        // CombatObstacle.pas:107
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatObstacle.pas:104
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CombatObstacle.pas:203
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)Lhx));
            return Result;
        }

        // CombatObstacle.pas:125
        public void CreateShadow()
        {
            var call__9 = FResource.ImageSequenceList.GetImageSequence(40);
            if (call__9 != null)
            {
                if (FShadow != null)
                {
                    DestroyShadow();
                }

                if ((Aow.Units.AoWE.AoWE.AoWHSMap == null) || Aow.Units.AoWE.AoWE.AoWHSMap.FShowUnitShadows)
                {
                    var call__10 = Aow.Units.CombatObstacle.TCombatObstacleShadow.Create(null);
                    FShadow = call__10;
                    FShadow.FObstacle = this;
                }
            }
        }

        // CombatObstacle.pas:138
        public void DestroyShadow()
        {
            if (FShadow != null)
            {
                if (GetVisible())
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicVisibleHS.RemoveChild(FShadow);
                }

                FShadow.Free();
                FShadow = null;
            }
        }

        // CombatObstacle.pas:148
        public void CreateOverlay()
        {
            var call__11 = FResource.ImageSequenceList.GetImageSequence(41);
            if (call__11 != null)
            {
                if (FOverlay != null)
                {
                    DestroyOverlay();
                }

                if ((Aow.Units.AoWE.AoWE.AoWHSMap == null) || Aow.Units.AoWE.AoWE.AoWHSMap.FShowUnitShadows)
                {
                    var call__12 = Aow.Units.CombatObstacle.TCombatObstacleOverlay.Create(null);
                    FOverlay = call__12;
                    FOverlay.FObstacle = this;
                }
            }
        }

        // CombatObstacle.pas:161
        public void DestroyOverlay()
        {
            if (FOverlay != null)
            {
                if (GetVisible())
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicVisibleHS.RemoveChild(FOverlay);
                }

                FOverlay.Free();
                FOverlay = null;
            }
        }

        // CombatObstacle.pas:171
        public void ShowShadow(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, unchecked((uint)(((Aow.Units.HSEngine.TMapField)Owner).Xhx + (((Aow.Units.HSEngine.TMapField)Owner).Yhx << 4))));
        }

        // CombatObstacle.pas:182
        public void ShowOverlay(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 41, unchecked((uint)(((Aow.Units.HSEngine.TMapField)Owner).Xhx + (((Aow.Units.HSEngine.TMapField)Owner).Yhx << 4))));
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // CombatObstacle.pas:94
            public override int ClassID()
            {
                int Result = default;
                Result = 2229546;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatObstacle.TCombatObstacle => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.CombatObstacle.TCombatObstacle Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatObstacle.TCombatObstacle o = (Aow.Units.CombatObstacle.TCombatObstacle)NewInstance();
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

    // { TCombatObstacleResource (instance size 68, VMT 00431D60) }
    public partial class TCombatObstacleResource : Aow.Units.HSEngine.TMultiHexMOR
    {
        // CombatObstacle.pas:245
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229546;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMultiHexMOR.Meta
        {
            // CombatObstacle.pas:239
            public override int ClassID()
            {
                int Result = default;
                Result = 2229547;
                return Result;
            }

            // CombatObstacle.pas:242
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = ((AnsiString)"Combat ") + Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ObstacleRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.CombatObstacle.TCombatObstacleResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.CombatObstacle.TCombatObstacleResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.CombatObstacle.TCombatObstacleResource o = (Aow.Units.CombatObstacle.TCombatObstacleResource)NewInstance();
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

    public static partial class CombatObstacle
    {
        // CombatObstacle.pas:248
        public static void RegisterCombatObstacle(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatObstacle.TCombatObstacleResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.CombatObstacle.TCombatObstacle>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
