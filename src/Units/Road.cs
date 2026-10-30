// Translated from Road.pas.
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

namespace Aow.Units.Road
{
    // { The Road unit of HSEPack.dpl: a road on a hexagon (TRoad, ClassID $20164) whose image is chosen
//   from the roads on the six neighbour hexagons (TRoadResource, ClassID $20165). }
    // { Road.TRoad (instance size 20): +$10 FTransitions (bit HN-1: a road on the neighbour HN; the
//     pairs 2+6 and 3+5 become bits 7 and 6) }
    public partial class TRoad : Aow.Units.HSEngine.TAbstractRoad
    {
        public TRoad(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // Road.pas:47
        public new const int ClassID = 131428;

        protected internal byte FTransitions;
        // Road.pas:42
        // { TRoad }


        // Road.pas:52
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            ((Aow.Units.Road.TRoadResource)FResource).Show(((Xhp - XhpSurf) - 8) - 6, (Yhp - YhpSurf) - 9, SurfaceDesc, this);
        }

        // Road.pas:60
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            Aow.Units.HSEngine.THexagonSprite OldRoad = default;
            // { Tried Boolean staging/cast, receiver scope and TObject road; result allocation still differs. }
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindOwnedHS(typeof(Aow.Units.HSEngine.TAbstractRoad));
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
            Aow.Units.HSEngine.TMapLevel Level = default;
            int HN = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            bool HasRoad = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            FTransitions = 0;
            {
                HN = 1;
                byte end__1 = 6;
                for (; HN <= end__1; HN += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HNtoHX(HN, ref Xhx, ref Yhx);
                    Aow.Units.HSEngine.HSEngine.TranslateHX(0, 0, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
                    if ((((Xhx >= 0) && (Yhx >= 0)) && (Level.Width > Xhx)) && (Level.Height > Yhx))
                    {
                        MapField = Level.GetField(Xhx, Yhx);
                        HasRoad = MapField.FindOwnedHS(typeof(Aow.Units.HSEngine.TAbstractRoad)) != null;
                        if (HasRoad)
                        {
                            FTransitions = (byte)(((int)FTransitions) + (1 << (HN - 1)));
                        }
                    }
                }
            }

            if ((FTransitions & 34) == 34)
            {
                FTransitions = unchecked((byte)((FTransitions & 221) + 128));
            }

            if ((FTransitions & 20) == 20)
            {
                FTransitions = unchecked((byte)((FTransitions & 235) + 64));
            }
        }

        // Road.pas:101
        public void UpdateNeighbourTransitions()
        {
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            int HN = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            Aow.Units.HSEngine.TAbstractRoad Road = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            {
                HN = 1;
                byte end__2 = 6;
                for (; HN <= end__2; HN += 1)
                {
                    Aow.Units.HSEngine.HSEngine.HNtoHX(HN, ref Xhx, ref Yhx);
                    Aow.Units.HSEngine.HSEngine.TranslateHX(0, 0, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Xhx, Yhx, ref Xhx, ref Yhx);
                    if ((((Xhx >= 0) && (Yhx >= 0)) && (Xhx < Level.Width)) && (Yhx < Level.Height))
                    {
                        MapField = Level.GetField(Xhx, Yhx);
                        Road = (Aow.Units.HSEngine.TAbstractRoad)MapField.FindChild(131428);
                        if (Road != null)
                        {
                            Road.UpdateTransitions();
                        }
                    }
                }
            }
        }

        // Road.pas:18


        public new partial class Meta : Aow.Units.HSEngine.TAbstractRoad.Meta
        {
            // Road.pas:47
            public override int ClassID() => Aow.Units.Road.TRoad.ClassID;

            // Road.pas:18
            public new Aow.Units.Road.TRoad Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Road.TRoad)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Road.TRoad => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Road.TRoadResource (instance size 52) }
    public partial class TRoadResource : Aow.Units.HSEngine.TAbstractRoadResource
    {
        public TRoadResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // Road.pas:136
        public new const int ClassID = 131429;

        // Road.pas:126
        // { TRoadResource }
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131428;
            return Result;
        }

        // Road.pas:141
        public void Show(int Xhp, int Yhp, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc, Aow.Units.Road.TRoad Road)
        {
            byte Transitions = default;
            byte Base = default;
            Transitions = unchecked((byte)Road.FTransitions);
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
        public override Aow.Units.Engine.TEObjectEditor.Meta Editor()
        {
            Aow.Units.Engine.TEObjectEditor.Meta Result = default;
            var call__12 = FEngine.Editors.FindEditor(131429);
            Result = call__12;
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // Road.pas:131
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Road";
                return Result;
            }

            // Road.pas:136
            public override int ClassID() => Aow.Units.Road.TRoadResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Road.TRoadResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1124
            public new Aow.Units.Road.TRoadResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Road.TRoadResource)base.Create(AOwner);
        }
    }

    public static partial class Road
    {
        // Road.pas:177
        public static void RegisterRoad(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Road.TRoad>(), Aow.Units.Engine.EClass.Of<Aow.Units.Road.TRoadResource>()]);
        }
    }
}
