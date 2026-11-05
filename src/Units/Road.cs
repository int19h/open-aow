// Translated from Road.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.Road
{
    // { The Road unit of HSEPack.dpl: a road on a hexagon (TRoad, ClassID $20164) whose image is chosen
//   from the roads on the six neighbour hexagons (TRoadResource, ClassID $20165). }
    // FTransitions (bit HN-1: a road on the neighbour HN. the pairs 2+6 and 3+5 become bits 7 and
    // 6)
    public partial class TRoad : TAbstractRoad
    {
        public TRoad(TECustomNode owner) : base(owner)
        {
        }

        // Road.pas:47
        public new const int ClassID = 131428;

        protected internal byte FTransitions;
        // Road.pas:42


        // Road.pas:52
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            ((TRoadResource)FResource).Show(Xhp - XhpSurf - 8 - 6, Yhp - YhpSurf - 9, SurfaceDesc, this);
        }

        // Road.pas:60
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            TMapField MapField = default;
            THexagonSprite OldRoad = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindOwnedHS(typeof(TAbstractRoad));
            if (OldRoad != null)
            {
                OldRoad.Destroy();
            }

            Owner = MapField;
            Result = PlaceOnMap();
            UpdateTransitions();
            UpdateNeighbourTransitions();
            return Result;
        }

        // Road.pas:74
        public override void UpdateTransitions()
        {
            int Xhx = default;
            int Yhx = default;
            TMapLevel Level = default;
            int HN = default;
            TMapField MapField = default;
            bool HasRoad = default;
            Level = (TMapLevel)Owner.Owner;
            FTransitions = 0;
            {
                HN = 1;
                byte end__1 = 6;
                for (; HN <= end__1; HN += 1)
                {
                    HNtoHX(HN, ref Xhx, ref Yhx);
                    TranslateHX(0, 0, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
                    if (Xhx >= 0 && Yhx >= 0 && Level.Width > Xhx && Level.Height > Yhx)
                    {
                        MapField = Level.GetField(Xhx, Yhx);
                        HasRoad = MapField.FindOwnedHS(typeof(TAbstractRoad)) != null;
                        if (HasRoad)
                        {
                            FTransitions = (byte)(FTransitions + (1 << (HN - 1)));
                        }
                    }
                }
            }

            if ((FTransitions & 34) == 34)
            {
                FTransitions = (byte)((FTransitions & 221) + 128);
            }

            if ((FTransitions & 20) == 20)
            {
                FTransitions = (byte)((FTransitions & 235) + 64);
            }
        }

        // Road.pas:101
        public void UpdateNeighbourTransitions()
        {
            int Xhx = default;
            int Yhx = default;
            TMapLevel Level = default;
            int HN = default;
            TMapField MapField = default;
            TAbstractRoad Road = default;
            Level = (TMapLevel)Owner.Owner;
            {
                HN = 1;
                byte end__2 = 6;
                for (; HN <= end__2; HN += 1)
                {
                    HNtoHX(HN, ref Xhx, ref Yhx);
                    TranslateHX(0, 0, ((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
                    if (Xhx >= 0 && Yhx >= 0 && Xhx < Level.Width && Yhx < Level.Height)
                    {
                        MapField = Level.GetField(Xhx, Yhx);
                        Road = (TAbstractRoad)MapField.FindChild(131428);
                        if (Road != null)
                        {
                            Road.UpdateTransitions();
                        }
                    }
                }
            }
        }

        // Road.pas:18


    }

    public partial class TRoadResource : TAbstractRoadResource
    {
        public TRoadResource(TECustomNode owner) : base(owner)
        {}

        // Road.pas:136
        public new const int ClassID = 131429;

        // Road.pas:126
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131428;
            return Result;
        }

        // Road.pas:141
        public void Show(int Xhp, int Yhp, in TSurfaceDesc SurfaceDesc, TRoad Road)
        {
            byte Transitions = default;
            byte Base = default;
            Transitions = Road.FTransitions;
            Base = 1;
            if (Transitions != 0)
            {
                if ((Transitions & 64) == 0)
                {
                    if ((Transitions & 16) != 0)
                    {
                        var call__3 = FImageLibrary[Base + 4];
                        call__3.Show(Xhp, Yhp, SurfaceDesc);
                    }

                    if ((Transitions & 4) != 0)
                    {
                        var call__4 = FImageLibrary[Base + 2];
                        call__4.Show(Xhp, Yhp, SurfaceDesc);
                    }
                }
                else
                {
                    var call__5 = FImageLibrary[Base + 6];
                    call__5.Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 128) == 0)
                {
                    if ((Transitions & 2) != 0)
                    {
                        var call__6 = FImageLibrary[Base + 1];
                        call__6.Show(Xhp, Yhp, SurfaceDesc);
                    }

                    if ((Transitions & 32) != 0)
                    {
                        var call__7 = FImageLibrary[Base + 5];
                        call__7.Show(Xhp, Yhp, SurfaceDesc);
                    }
                }
                else
                {
                    var call__8 = FImageLibrary[Base + 7];
                    call__8.Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 1) != 0)
                {
                    var call__9 = FImageLibrary[Base];
                    call__9.Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((Transitions & 8) != 0)
                {
                    var call__10 = FImageLibrary[Base + 3];
                    call__10.Show(Xhp, Yhp, SurfaceDesc);
                }
            }
            else
            {
                var call__11 = FImageLibrary[Base + 6];
                call__11.Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // Road.pas:171
        public override EditorClass Editor()
        {
            EditorClass Result = default;
            var call__12 = FEngine.Editors.FindEditor(131429);
            Result = call__12;
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // Road.pas:131
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Road";
            return Result;
        }

    }

    public static partial class Road
    {
        // Road.pas:177
        public static void RegisterRoad(TEngine Engine)
        {
            Engine.RegisterEClasses([EClass.Of<TRoad>(), EClass.Of<TRoadResource>()]);
        }
    }
}
