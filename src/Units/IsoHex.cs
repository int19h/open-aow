// Translated from IsoHex.pas.
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

namespace Aow.Units.IsoHex
{
    // { IsoHex.TIsometricHexagon (instance size 24): +$10 FDynamic, +$14 FOpenSides (bit HN-1: the
//     neighbour HN is no hexagon of this kind), +$15 FSides (bits 0..5: the neighbour HN has another
//     terrain, bits 6..7: the image variant) }
    public partial class TIsometricHexagon : Aow.Units.HSEngine.TAbstractHexagon
    {
        public TIsometricHexagon(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__1 = Aow.Rtl.Rtl.Random(4);
            FSides = unchecked((byte)(call__1 << 6));
        }


        // IsoHex.pas:214
        public new const int ClassID = 131437;

        protected internal Aow.Units.IsoHex.TDynamicIsometricHexagon FDynamic;
        protected internal byte FOpenSides;
        protected internal byte FSides;
        // IsoHex.pas:204
        public override void Destroy()
        {
            if (FDynamic != null)
            {
                FDynamic.Free();
                FDynamic = null;
            }

            base.Destroy();
        }

        // IsoHex.pas:198
        // { TIsometricHexagon }


        // IsoHex.pas:224
        public override void Disconnect()
        {
            if (FDynamic != null)
            {
                FDynamic.Free();
                FDynamic = null;
            }

            base.Disconnect();
        }

        // IsoHex.pas:234
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            UpdateTransitions();
        }

        // IsoHex.pas:239
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__2 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__2.ImageLibrary;
            var call__3 = IL[((int)(((uint)FSides) >> 6)) + 40];
            call__3.Show(Xhp, Yhp, SurfaceDesc);
            Xhp -= 6;
            Yhp -= 9;
            if ((FSides & 1) != 0)
            {
                var call__4 = IL[31];
                call__4.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 2) != 0)
            {
                var call__5 = IL[32];
                call__5.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 32) != 0)
            {
                var call__6 = IL[30];
                call__6.Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // IsoHex.pas:256
        public virtual void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
        }

        // IsoHex.pas:50


        public new partial class Meta : Aow.Units.HSEngine.TAbstractHexagon.Meta
        {
            // IsoHex.pas:214
            public override int ClassID() => Aow.Units.IsoHex.TIsometricHexagon.ClassID;

            // IsoHex.pas:219
            public virtual Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta DynamicIsometricHexagonClass()
            {
                Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta Result = default;
                Result = Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta.Of<Aow.Units.IsoHex.TDynamicIsometricHexagon>();
                return Result;
            }

            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TIsometricHexagon)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { IsoHex.TDynamicIsometricHexagon (instance size 8): owned by its TIsometricHexagon (FOwner set
//     directly, so it is not added as a child) and shown from the map field of that hexagon }
    public partial class TDynamicIsometricHexagon : Aow.Units.HSEngine.THexagonSprite
    {
        // IsoHex.pas:126
        public new const int ClassID = 131439;

        // IsoHex.pas:136
        public override TSet<Aow.Units.Engine.TEControlStyleElement> GetControlStyle()
        {
            TSet<Aow.Units.Engine.TEControlStyleElement> Result = default;
            Result = TSet<Aow.Units.Engine.TEControlStyleElement>.Of();
            return Result;
        }

        // IsoHex.pas:109
        // { TDynamicIsometricHexagon }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(null);
            FOwner = AOwner;
            ((Aow.Units.HSEngine.TMapField)AOwner.Owner).AddHS(this, 0);
        }

        // IsoHex.pas:161
        public override void SetVisible(bool Value)
        {
        }

        // IsoHex.pas:165
        public override bool GetVisible()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // IsoHex.pas:131
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // IsoHex.pas:24
        public static new Aow.Units.IsoHex.TDynamicIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.IsoHex.TDynamicIsometricHexagon o = new Aow.Units.IsoHex.TDynamicIsometricHexagon();
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

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // IsoHex.pas:126
            public override int ClassID() => Aow.Units.IsoHex.TDynamicIsometricHexagon.ClassID;

            // IsoHex.pas:141
            public override TSet<Aow.Units.HSEngine.TControlModeElement> ControlMode()
            {
                TSet<Aow.Units.HSEngine.TControlModeElement> Result = default;
                Result = TSet<Aow.Units.HSEngine.TControlModeElement>.Of();
                return Result;
            }

            // IsoHex.pas:24
            public new Aow.Units.IsoHex.TDynamicIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TDynamicIsometricHexagon)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TDynamicIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { IsoHex.TIsometricHexagonResource (instance size 56): +$34 FDynamicLevel (26) }
    public partial class TIsometricHexagonResource : Aow.Units.HSEngine.TAbstractHexagonResource
    {
        public TIsometricHexagonResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FDynamicLevel = 26;
        }


        // IsoHex.pas:268
        public new const int ClassID = 131438;

        protected internal byte FDynamicLevel;
        // IsoHex.pas:262
        // { TIsometricHexagonResource }


        // IsoHex.pas:302
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // IsoHex.pas:273
        public override Aow.Units.HSEngine.TMapObject PlaceResourceUser(Aow.Units.HSEngine.THSMap Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            Aow.Units.HSEngine.TMapObject Result = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            var call__7 = Map.MapContainer.GetField(Xhx, Yhx, Lhx);
            MapField = call__7;
            Result = (Aow.Units.HSEngine.TMapObject)MapField.FindOwnedHS(typeof(Aow.Units.IsoHex.TIsometricHexagon));
            if (Result != null)
            {
                var call__8 = Result.GetTerrain(0);
                var call__9 = FTerrainList.GetTerrain(0);
                if (call__8 != call__9)
                {
                    Result.SetResource(this);
                    MapField.UpdateTerrainType();
                    ((Aow.Units.IsoHex.TIsometricHexagon)Result).UpdateTransitions();
                }
                else
                {
                    Result.SetResource(this);
                }

                Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
            }
            else
            {
                Result = (Aow.Units.HSEngine.TMapObject)CreateResourceUser();
                if (!Map.PlaceHX(Result, Xhx, Yhx, Lhx))
                {
                    Result.Free();
                    Result = null;
                }
            }

            return Result;
        }

        // IsoHex.pas:64


        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // IsoHex.pas:268
            public override int ClassID() => Aow.Units.IsoHex.TIsometricHexagonResource.ClassID;

            // IsoHex.pas:64
            public new Aow.Units.IsoHex.TIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TIsometricHexagonResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TIsometricHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { IsoHex.TUpperIsometricHexagon (instance size 24) }
    public partial class TUpperIsometricHexagon : Aow.Units.IsoHex.TIsometricHexagon
    {
        public TUpperIsometricHexagon(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}


        public new const int ClassID = 131440;

        // IsoHex.pas:355
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            if (FDynamic == null)
            {
                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                Xhp = (Xhp - XhpSurf) - 8;
                Yhp = Yhp - YhpSurf;
                var call__10 = FResource.ImageSequenceList.ImageSequences[0];
                IL = call__10.ImageLibrary;
                var call__11 = IL[((int)(((uint)FSides) >> 6)) + 40];
                call__11.Show(Xhp, Yhp, SurfaceDesc);
                Xhp -= 6;
                Yhp -= 9;
                if ((FSides & 1) != 0)
                {
                    var call__12 = IL[31];
                    call__12.Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((FSides & 2) != 0)
                {
                    var call__13 = IL[32];
                    call__13.Show(Xhp, Yhp, SurfaceDesc);
                }

                if ((FSides & 32) != 0)
                {
                    var call__14 = IL[30];
                    call__14.Show(Xhp, Yhp, SurfaceDesc);
                }
            }
        }

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagon.Meta
        {
            // IsoHex.pas:310
            // { TUpperIsometricHexagon }
            public override int ClassID() => Aow.Units.IsoHex.TUpperIsometricHexagon.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TUpperIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TUpperIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TUpperIsometricHexagon)base.Create(AOwner);
        }
    }

    // { IsoHex.TUpperIsometricHexagonResource (instance size 56) }
    public partial class TUpperIsometricHexagonResource : Aow.Units.IsoHex.TIsometricHexagonResource
    {
        public TUpperIsometricHexagonResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}


        public new const int ClassID = 131441;

        // IsoHex.pas:443
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131440;
            return Result;
        }

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagonResource.Meta
        {
            // IsoHex.pas:438
            // { TUpperIsometricHexagonResource }
            public override int ClassID() => Aow.Units.IsoHex.TUpperIsometricHexagonResource.ClassID;

            // IsoHex.pas:448
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.HSEngine.HSEngine.UpperIsometricHexagonStr;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TUpperIsometricHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:64
            public new Aow.Units.IsoHex.TUpperIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TUpperIsometricHexagonResource)base.Create(AOwner);
        }
    }

    // { IsoHex.TLowerIsometricHexagon (instance size 24) }
    public partial class TLowerIsometricHexagon : Aow.Units.IsoHex.TIsometricHexagon
    {
        public TLowerIsometricHexagon(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}


        public new const int ClassID = 131442;

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagon.Meta
        {
            // IsoHex.pas:455
            // { TLowerIsometricHexagon }
            public override int ClassID() => Aow.Units.IsoHex.TLowerIsometricHexagon.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TLowerIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TLowerIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TLowerIsometricHexagon)base.Create(AOwner);
        }
    }

    // { IsoHex.TLowerIsometricHexagonResource (instance size 56) }
    public partial class TLowerIsometricHexagonResource : Aow.Units.IsoHex.TIsometricHexagonResource
    {
        public TLowerIsometricHexagonResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}


        public new const int ClassID = 131443;

        // IsoHex.pas:581
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131442;
            return Result;
        }

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagonResource.Meta
        {
            // IsoHex.pas:571
            // { TLowerIsometricHexagonResource }
            public override int ClassID() => Aow.Units.IsoHex.TLowerIsometricHexagonResource.ClassID;

            // IsoHex.pas:576
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.HSEngine.HSEngine.LowerIsometricHexagonStr;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TLowerIsometricHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:64
            public new Aow.Units.IsoHex.TLowerIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoHex.TLowerIsometricHexagonResource)base.Create(AOwner);
        }
    }

    public static partial class IsoHex
    {
        // IsoHex.pas:586
        public static void RegisterIsometricHexagon(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TIsometricHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TIsometricHexagonResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TUpperIsometricHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TUpperIsometricHexagonResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TLowerIsometricHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoHex.TLowerIsometricHexagonResource>()]);
        }
    }
}
