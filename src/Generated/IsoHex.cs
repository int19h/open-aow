// Generated from IsoHex.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__1 = Aow.Rtl.Rtl.Random(4);
            FSides = unchecked((byte)(call__1 << 6));
        }

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
        public static new Aow.Units.IsoHex.TIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.IsoHex.TIsometricHexagon o = new Aow.Units.IsoHex.TIsometricHexagon();
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

        public new partial class Meta : Aow.Units.HSEngine.TAbstractHexagon.Meta
        {
            // IsoHex.pas:214
            public override int ClassID()
            {
                int Result = default;
                Result = 131437;
                return Result;
            }

            // IsoHex.pas:219
            public virtual Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta DynamicIsometricHexagonClass()
            {
                Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta Result = default;
                Result = Aow.Units.IsoHex.TDynamicIsometricHexagon.Meta.Of<Aow.Units.IsoHex.TDynamicIsometricHexagon>();
                return Result;
            }

            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TIsometricHexagon o = (Aow.Units.IsoHex.TIsometricHexagon)NewInstance();
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
                where T : Aow.Units.IsoHex.TIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { IsoHex.TDynamicIsometricHexagon (instance size 8): owned by its TIsometricHexagon (FOwner set
//     directly, so it is not added as a child) and shown from the map field of that hexagon }
    public partial class TDynamicIsometricHexagon : Aow.Units.HSEngine.THexagonSprite
    {
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
            public override int ClassID()
            {
                int Result = default;
                Result = 131439;
                return Result;
            }

            // IsoHex.pas:141
            public override TSet<Aow.Units.HSEngine.TControlModeElement> ControlMode()
            {
                TSet<Aow.Units.HSEngine.TControlModeElement> Result = default;
                Result = TSet<Aow.Units.HSEngine.TControlModeElement>.Of();
                return Result;
            }

            // IsoHex.pas:24
            public new Aow.Units.IsoHex.TDynamicIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TDynamicIsometricHexagon o = (Aow.Units.IsoHex.TDynamicIsometricHexagon)NewInstance();
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
                where T : Aow.Units.IsoHex.TDynamicIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { IsoHex.TIsometricHexagonResource (instance size 56): +$34 FDynamicLevel (26) }
    public partial class TIsometricHexagonResource : Aow.Units.HSEngine.TAbstractHexagonResource
    {
        protected internal byte FDynamicLevel;
        // IsoHex.pas:262
        // { TIsometricHexagonResource }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FDynamicLevel = 26;
        }

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
        public static new Aow.Units.IsoHex.TIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.IsoHex.TIsometricHexagonResource o = new Aow.Units.IsoHex.TIsometricHexagonResource();
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

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // IsoHex.pas:268
            public override int ClassID()
            {
                int Result = default;
                Result = 131438;
                return Result;
            }

            // IsoHex.pas:64
            public new Aow.Units.IsoHex.TIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TIsometricHexagonResource o = (Aow.Units.IsoHex.TIsometricHexagonResource)NewInstance();
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
                where T : Aow.Units.IsoHex.TIsometricHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { IsoHex.TUpperIsometricHexagon (instance size 24) }
    public partial class TUpperIsometricHexagon : Aow.Units.IsoHex.TIsometricHexagon
    {
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

        // IsoHex.pas:375
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__15 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__15.ImageLibrary;
            Variant = ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            var call__16 = IL[((int)(((uint)FSides) >> 6)) + 40];
            call__16.Show(Xhp, Yhp, SurfaceDesc);
            if (FOpenSides != 0)
            {
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        var call__17 = IL[18];
                        call__17.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 2) == 0)
                    {
                        var call__18 = IL[19];
                        call__18.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 34) == 34)
                    {
                        var call__19 = IL[23];
                        call__19.Show(Xhp - 20, (Yhp - 48) - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 34) == 0)
                        {
                            var call__20 = IL[20];
                            call__20.Show(Xhp - 20, (Yhp - 50) - 16, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) != 0)
                        {
                            var call__21 = IL[17];
                            call__21.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__22 = IL[Variant + 3];
                            call__22.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) != 0)
                        {
                            var call__23 = IL[16];
                            call__23.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__24 = IL[Variant];
                            call__24.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 20) == 20)
                    {
                        var call__25 = IL[13];
                        call__25.Show(Xhp - 19, Yhp, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 20) == 0)
                        {
                            var call__26 = IL[6];
                            call__26.Show(Xhp - 19, Yhp, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) == 0)
                        {
                            var call__27 = IL[25];
                            call__27.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__28 = IL[15];
                                call__28.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__29 = IL[Variant + 10];
                                call__29.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) == 0)
                        {
                            var call__30 = IL[24];
                            call__30.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__31 = IL[14];
                                call__31.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__32 = IL[Variant + 7];
                                call__32.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagon.Meta
        {
            // IsoHex.pas:310
            // { TUpperIsometricHexagon }
            public override int ClassID()
            {
                int Result = default;
                Result = 131440;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TUpperIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TUpperIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TUpperIsometricHexagon o = (Aow.Units.IsoHex.TUpperIsometricHexagon)NewInstance();
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

    // { IsoHex.TUpperIsometricHexagonResource (instance size 56) }
    public partial class TUpperIsometricHexagonResource : Aow.Units.IsoHex.TIsometricHexagonResource
    {
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
            public override int ClassID()
            {
                int Result = default;
                Result = 131441;
                return Result;
            }

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
            public new Aow.Units.IsoHex.TUpperIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TUpperIsometricHexagonResource o = (Aow.Units.IsoHex.TUpperIsometricHexagonResource)NewInstance();
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

    // { IsoHex.TLowerIsometricHexagon (instance size 24) }
    public partial class TLowerIsometricHexagon : Aow.Units.IsoHex.TIsometricHexagon
    {
        // IsoHex.pas:501
        // { IL[Variant + 0] reproduces the original's lea edx, [esi] for the index. }
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__33 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__33.ImageLibrary;
            Variant = ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            if (FOpenSides != 0)
            {
                X = Xhp - 19;
                Y = Yhp - 32;
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 35) == 35)
                    {
                        var call__34 = IL[6];
                        call__34.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 35) == 1)
                        {
                            var call__35 = IL[13];
                            call__35.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 7) == 7)
                    {
                        var call__36 = IL[25];
                        call__36.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 3) == 3)
                        {
                            var call__37 = IL[Variant + 3];
                            call__37.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 49) == 49)
                    {
                        var call__38 = IL[24];
                        call__38.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 33) == 33)
                        {
                            var call__39 = IL[Variant + 0];
                            call__39.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 6) == 6)
                    {
                        var call__40 = IL[Variant + 10];
                        call__40.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 2) == 2)
                        {
                            var call__41 = IL[17];
                            call__41.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 48) == 48)
                    {
                        var call__42 = IL[Variant + 7];
                        call__42.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 32) == 32)
                        {
                            var call__43 = IL[16];
                            call__43.Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 14) == 14)
                    {
                        var call__44 = IL[22];
                        call__44.Show(X, Y + 1, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 12) == 12)
                        {
                            if (Variant == 2)
                            {
                                var call__45 = IL[19];
                                call__45.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__46 = IL[Variant + 30];
                                call__46.Show(X, Y + 1, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 56) == 56)
                    {
                        var call__47 = IL[21];
                        call__47.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 24) == 24)
                        {
                            if (Variant == 2)
                            {
                                var call__48 = IL[18];
                                call__48.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__49 = IL[Variant + 28];
                                call__49.Show(X, Y, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 28) == 8)
                    {
                        var call__50 = IL[23];
                        call__50.Show(X, Y, SurfaceDesc);
                    }
                }
                else
                {
                    if ((FOpenSides & 14) == 4)
                    {
                        var call__51 = IL[15];
                        call__51.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 56) == 16)
                        {
                            var call__52 = IL[14];
                            call__52.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
            }
        }

        public new partial class Meta : Aow.Units.IsoHex.TIsometricHexagon.Meta
        {
            // IsoHex.pas:455
            // { TLowerIsometricHexagon }
            public override int ClassID()
            {
                int Result = default;
                Result = 131442;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.IsoHex.TLowerIsometricHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:50
            public new Aow.Units.IsoHex.TLowerIsometricHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TLowerIsometricHexagon o = (Aow.Units.IsoHex.TLowerIsometricHexagon)NewInstance();
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

    // { IsoHex.TLowerIsometricHexagonResource (instance size 56) }
    public partial class TLowerIsometricHexagonResource : Aow.Units.IsoHex.TIsometricHexagonResource
    {
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
            public override int ClassID()
            {
                int Result = default;
                Result = 131443;
                return Result;
            }

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
            public new Aow.Units.IsoHex.TLowerIsometricHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.IsoHex.TLowerIsometricHexagonResource o = (Aow.Units.IsoHex.TLowerIsometricHexagonResource)NewInstance();
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

    public static partial class IsoHex
    {
        // IsoHex.pas:586
        public static void RegisterIsometricHexagon(Aow.Units.Engine.TEngine Engine)
        {
            Engine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TIsometricHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TIsometricHexagonResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TUpperIsometricHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TUpperIsometricHexagonResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TLowerIsometricHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TLowerIsometricHexagonResource>()]);
        }
    }
}
