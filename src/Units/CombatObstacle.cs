// Translated from CombatObstacle.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatObstacle
{
    public partial class TCombatObstacleShadow : THexagonSprite
    {
        public TCombatObstacleShadow(TECustomNode owner) : base(owner)
        {}

        public TCombatObstacle FObstacle;
        // CombatObstacle.pas:74
        public override int GetShowPriority()
        {
            int Result = default;
            var call__1 = FObstacle.GetShowPriority();
            Result = (call__1 & 67108863) - 1677721600;
            return Result;
        }

        // CombatObstacle.pas:79
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FObstacle.ShowShadow(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    public partial class TCombatObstacleOverlay : THexagonSprite
    {
        public TCombatObstacleOverlay(TECustomNode owner) : base(owner)
        {}

        public TCombatObstacle FObstacle;
        // CombatObstacle.pas:84
        public override int GetShowPriority()
        {
            int Result = default;
            var call__2 = FObstacle.GetShowPriority();
            Result = (call__2 & 67108863) + 393216;
            return Result;
        }

        // CombatObstacle.pas:89
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            FObstacle.ShowOverlay(XhpSurf, YhpSurf, SurfaceDesc);
        }

        // Engine.pas:524

    }

    public partial class TCombatObstacle : TMultiHexMO
    {
        public TCombatObstacle(TECustomNode owner) : base(owner)
        {}

        // CombatObstacle.pas:94
        public new const int ClassID = 2229546;

        public TCombatObstacleShadow FShadow;
        public TCombatObstacleOverlay FOverlay;
        // CombatObstacle.pas:97
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
                UpdateMapFields((TMapLevel)Owner.Owner, call__4, call__3);
            }

            return Result;
        }

        // CombatObstacle.pas:116
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel = default;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatObstacle.pas:229
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            var call__5 = ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59);
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
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            if (Info.HN < FResource.TerrainList.Count)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }

        // CombatObstacle.pas:193
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
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
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatObstacle.pas:104
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // CombatObstacle.pas:203
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
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

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    var call__10 = new TCombatObstacleShadow(null);
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
                    AoWCombatMap.DynamicVisibleHS.RemoveChild(FShadow);
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

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    var call__12 = new TCombatObstacleOverlay(null);
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
                    AoWCombatMap.DynamicVisibleHS.RemoveChild(FOverlay);
                }

                FOverlay.Free();
                FOverlay = null;
            }
        }

        // CombatObstacle.pas:171
        public void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 40, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
        }

        // CombatObstacle.pas:182
        public void ShowOverlay(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 41, (uint)(((TMapField)Owner).Xhx + (((TMapField)Owner).Yhx << 4)));
        }

    }

    public partial class TCombatObstacleResource : TMultiHexMOR
    {
        public TCombatObstacleResource(TECustomNode owner) : base(owner)
        {}

        // CombatObstacle.pas:239
        public new const int ClassID = 2229547;

        // CombatObstacle.pas:245
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229546;
            return Result;
        }

        // CombatObstacle.pas:242
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat " + TranslateRStr(ObstacleRStr);
            return Result;
        }

    }

    public static partial class CombatObstacle
    {
        // CombatObstacle.pas:248
        public static void RegisterCombatObstacle(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatObstacleResource>(), EClass.Of<TCombatObstacle>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
