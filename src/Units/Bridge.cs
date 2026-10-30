// Translated from Bridge.pas.
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
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;

namespace Aow.Units.Bridge
{
    // { The Bridge unit of HSEPack.dpl: a bridge over water (TBridge, ClassID $20174, an overlay at level
//   10) whose image is chosen from the roads and shores around it (TBridgeResource, ClassID $20175). }
    // { Bridge.TBridge (instance size 20): +$10 FTransitions (two bits per side HN 1..6: 0 none, 1 a
//     road, 2 the shore the bridge reaches) }
    public partial class TBridge : Aow.Units.HSEngine.TAbstractRoad
    {
        public TBridge(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // Bridge.pas:48
        public new const int ClassID = 131444;

        internal ushort FTransitions;
        // Bridge.pas:43
        // { TBridge }


        // Bridge.pas:53
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            ((Aow.Units.Bridge.TBridgeResource)FResource).Show(((Xhp - XhpSurf) - 8) - 6, (Yhp - YhpSurf) - 9, SurfaceDesc, this);
        }

        // Bridge.pas:63
        // { Original oddity: the overlay of the owner of the previous owner (a map level, not a map field)
//   is updated. }
        public override bool PlaceHX(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            Aow.Units.HSEngine.THexagonSprite OldRoad = default;
            Aow.Units.HSEngine.TMapField OldOwner = default;
            // { Tried identity assignment, receiver scope and object/result casts; road register still differs. }
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindHS(typeof(Aow.Units.HSEngine.TAbstractRoad));
            if (OldRoad != null)
            {
                OldRoad.Destroy();
            }

            OldOwner = (Aow.Units.HSEngine.TMapField)Owner;
            Owner = MapField;
            FStatus = FStatus + TSet<Aow.Units.HSEngine.TMapObjectStatusElement>.Of(Aow.Units.HSEngine.TMapObjectStatusElement.moPlacedOnMap);
            if (OldOwner != null)
            {
                ((Aow.Units.HSEngine.TMapField)OldOwner.Owner).UpdateOverlayType();
            }

            MapField.UpdateOverlayType();
            UpdateTransitions();
            MapContainer.Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
            Result = true;
            return Result;
        }

        // Bridge.pas:82
        public override void UpdateTransitions()
        {
            Aow.Units.FixedArray_byte_6 Sides = default;
            int Transitions = default;
            int MaxDistance = default;
            int MaxSide = default;
            bool HasRoad = default;
            bool InShore = default;
            int Xhx = default;
            int Yhx = default;
            int Shores = default;
            sbyte TerrainType = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            int HN = default;
            int Start = default;
            int Shore = default;
            int Distance = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            bool IsShore = default;
            Transitions = 0;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Shores = 0;
            InShore = false;
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
                        TerrainType = unchecked((sbyte)MapField.TerrainType);
                        HasRoad = MapField.FindHS(typeof(Aow.Units.HSEngine.TAbstractRoad)) != null;
                        bool bool__2 = (TerrainType != 0);
                        if (bool__2)
                        {
                            var call__3 = FResource.TerrainList.GetTerrain(0);
                            bool__2 = (call__3 != TerrainType);
                        }

                        bool bool__4 = bool__2;
                        if (bool__4)
                        {
                            var call__5 = MapField.FindHS(typeof(Aow.Units.BorderHx.TBorderHexagon));
                            bool__4 = (call__5 == null);
                        }

                        IsShore = bool__4;
                        if (HasRoad)
                        {
                            Sides[HN - 1] = 1;
                            InShore = false;
                        }
                        else
                        {
                            if (IsShore)
                            {
                                if (!InShore)
                                {
                                    Shores += 1;
                                }

                                Sides[HN - 1] = unchecked((byte)(Shores + 2));
                                InShore = true;
                            }
                            else
                            {
                                Sides[HN - 1] = 0;
                                InShore = false;
                            }
                        }
                    }
                    else
                    {
                        Sides[HN - 1] = 0;
                    }
                }
            }

            // { the shores on side 1 and side 6 are one shore }
            if (((Sides[1 - 1] > 2) && (Sides[6 - 1] > 2)) && (Sides[1 - 1] != Sides[6 - 1]))
            {
                {
                    HN = 3;
                    byte end__6 = 6;
                    for (; HN <= end__6; HN += 1)
                    {
                        if (Sides[HN - 1] == Sides[6 - 1])
                        {
                            Sides[HN - 1] = unchecked((byte)Sides[1 - 1]);
                        }
                    }
                }

                Shores -= 1;
            }

            // { the side the bridge starts from: a road, else the first side without water }
            Start = 1;
            while ((Start <= 6) && (Sides[Start - 1] != 1))
            {
                Start += 1;
            }

            if (Start == 1)
            {
                if (Sides[2 - 1] == 1)
                {
                    Start = 2;
                }
                else
                {
                    if (Sides[3 - 1] == 1)
                    {
                        Start = 3;
                    }
                }
            }

            if (Start == 7)
            {
                Start = 1;
                while ((Start <= 6) && (Sides[Start - 1] == 0))
                {
                    Start += 1;
                }

                if (Start == 1)
                {
                    if (Sides[2 - 1] != 0)
                    {
                        Start = 2;
                    }
                    else
                    {
                        if (Sides[3 - 1] == 0)
                        {
                            Start = 3;
                        }
                    }
                }
            }

            if (Start <= 6)
            {
                // { of every shore keep the side farthest from the start }
                {
                    Shore = 3;
                    int end__7 = Shores + 2;
                    for (; Shore <= end__7; Shore += 1)
                    {
                        MaxDistance = 0;
                        MaxSide = 0;
                        if ((Shore != Sides[Start - 1]) || (Shores == 1))
                        {
                            {
                                HN = 1;
                                byte end__8 = 6;
                                for (; HN <= end__8; HN += 1)
                                {
                                    if (Shore == Sides[HN - 1])
                                    {
                                        Distance = System.Math.Abs(HN - Start);
                                        if (Distance > 3)
                                        {
                                            Distance = System.Math.Abs(Distance - 6);
                                        }

                                        if (Distance > MaxDistance)
                                        {
                                            MaxDistance = Distance;
                                            MaxSide = HN;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MaxSide = Start;
                        }

                        {
                            HN = 1;
                            byte end__9 = 6;
                            for (; HN <= end__9; HN += 1)
                            {
                                if ((HN != Start) && (Shore == Sides[HN - 1]))
                                {
                                    if (HN != MaxSide)
                                    {
                                        Sides[HN - 1] = 0;
                                    }
                                    else
                                    {
                                        Sides[HN - 1] = 2;
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    HN = 1;
                    byte end__10 = 6;
                    for (; HN <= end__10; HN += 1)
                    {
                        Transitions += Sides[HN - 1] << ((HN - 1) * 2);
                    }
                }
            }

            FTransitions = unchecked((ushort)Transitions);
        }

        // Bridge.pas:18


        public new partial class Meta : Aow.Units.HSEngine.TAbstractRoad.Meta
        {
            // Bridge.pas:48
            public override int ClassID() => Aow.Units.Bridge.TBridge.ClassID;

            // Bridge.pas:18
            public new Aow.Units.Bridge.TBridge Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Bridge.TBridge)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Bridge.TBridge => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Bridge.TBridgeResource (instance size 52) }
    public partial class TBridgeResource : Aow.Units.HSEngine.TAbstractRoadResource
    {
        public TBridgeResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            SetLevel(10);
        }

        // Bridge.pas:205
        public new const int ClassID = 131445;

        // Bridge.pas:189
        // { TBridgeResource }


        // Bridge.pas:195
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131444;
            return Result;
        }

        // Bridge.pas:210
        public void Show(int Xhp, int Yhp, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc, Aow.Units.Bridge.TBridge Bridge)
        {
            int Transitions = default;
            int Base = default;
            Transitions = Bridge.FTransitions;
            Base = 1;
            var call__11 = FImageLibrary[Base + 18];
            call__11.Show(Xhp, Yhp, SurfaceDesc);
            if ((Transitions & 3) != 0)
            {
                if ((Transitions & 3) == 1)
                {
                    var call__12 = FImageLibrary[Base + 6];
                    call__12.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__13 = FImageLibrary[Base + 12];
                call__13.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 12) != 0)
            {
                if ((Transitions & 12) == 4)
                {
                    var call__14 = FImageLibrary[Base + 7];
                    call__14.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__15 = FImageLibrary[Base + 13];
                call__15.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 48) != 0)
            {
                if ((Transitions & 48) == 16)
                {
                    var call__16 = FImageLibrary[Base + 8];
                    call__16.Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    var call__17 = FImageLibrary[Base + 2];
                    call__17.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__18 = FImageLibrary[Base + 14];
                call__18.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 192) != 0)
            {
                if ((Transitions & 192) == 64)
                {
                    var call__19 = FImageLibrary[Base + 9];
                    call__19.Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    var call__20 = FImageLibrary[Base + 3];
                    call__20.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__21 = FImageLibrary[Base + 15];
                call__21.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 768) != 0)
            {
                if ((Transitions & 768) == 256)
                {
                    var call__22 = FImageLibrary[Base + 10];
                    call__22.Show(Xhp, Yhp, SurfaceDesc);
                }
                else
                {
                    var call__23 = FImageLibrary[Base + 4];
                    call__23.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__24 = FImageLibrary[Base + 16];
                call__24.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((Transitions & 3072) != 0)
            {
                if ((Transitions & 3072) == 1024)
                {
                    var call__25 = FImageLibrary[Base + 11];
                    call__25.Show(Xhp, Yhp, SurfaceDesc);
                }

                var call__26 = FImageLibrary[Base + 17];
                call__26.Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // Bridge.pas:253
        public override Aow.Units.Engine.TEObjectEditor.Meta Editor()
        {
            Aow.Units.Engine.TEObjectEditor.Meta Result = default;
            var call__27 = FEngine.Editors.FindEditor(131445);
            Result = call__27;
            if (Result == null)
            {
                Result = base.Editor();
            }

            return Result;
        }

        // Bridge.pas:259
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if (Msg.MsgID != 65556)
            {
                base.MsgProc(ref Msg, Sender);
            }
        }

        // Bridge.pas:28


        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // Bridge.pas:200
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.HSEngine.HSEngine.BridgeStr;
                return Result;
            }

            // Bridge.pas:205
            public override int ClassID() => Aow.Units.Bridge.TBridgeResource.ClassID;

            // Bridge.pas:28
            public new Aow.Units.Bridge.TBridgeResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Bridge.TBridgeResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Bridge.TBridgeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class Bridge
    {
        // Bridge.pas:264
        public static void RegisterBridge(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Bridge.TBridge>(), Aow.Units.Engine.EClass.Of<Aow.Units.Bridge.TBridgeResource>()]);
        }
    }
}
