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
        public override int GetShowPriority() => (FObstacle.GetShowPriority() & 67108863) - 1677721600;

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
        public override int GetShowPriority() => (FObstacle.GetShowPriority() & 67108863) + 393216;

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
            bool Result = base.PlaceOnMap();
            if (Result)
            {
                var yhx = Yhx;
                var xhx = Xhx;
                UpdateMapFields((TMapLevel)Owner.Owner, xhx, yhx);
            }

            return Result;
        }

        // CombatObstacle.pas:116
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // CombatObstacle.pas:229
        public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
        {
            if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(59))
            {
                if (FResource.TerrainList.GetOverlay(Info.HN) == 8)
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
                    var showPriority = FShadow.GetShowPriority();
                    VisibilityList.AddSortChild(FShadow, showPriority);
                }

                if (FOverlay != null)
                {
                    var showPriority = FOverlay.GetShowPriority();
                    VisibilityList.AddSortChild(FOverlay, showPriority);
                }
            }
        }

        // CombatObstacle.pas:107
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

        // CombatObstacle.pas:104
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

        // CombatObstacle.pas:203
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);

        // CombatObstacle.pas:125
        public void CreateShadow()
        {
            if (FResource.ImageSequenceList.GetImageSequence(40) != null)
            {
                if (FShadow != null)
                {
                    DestroyShadow();
                }

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    FShadow = new TCombatObstacleShadow(null);
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
            if (FResource.ImageSequenceList.GetImageSequence(41) != null)
            {
                if (FOverlay != null)
                {
                    DestroyOverlay();
                }

                if (AoWHSMap == null || AoWHSMap.FShowUnitShadows)
                {
                    FOverlay = new TCombatObstacleOverlay(null);
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
        public override int ResourceUserClassID() => 2229546;

        // CombatObstacle.pas:242
        public override string ResourceEditName() => "Combat " + TranslateRStr(ObstacleRStr);

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
