// Translated from IsoCombatHex.pas.
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

namespace Aow.Units.IsoCombatHex
{
    // { TIsometricCombatHexagon (instance size 24, VMT 0043386C) }
    public partial class TIsometricCombatHexagon : Aow.Units.IsoHex.TUpperIsometricHexagon
    {
        // IsoCombatHex.pas:40
        public new const int ClassID = 2229514;

        // IsoCombatHex.pas:43
        public override Aow.Units.HSEngine.TMapObjectResourceList GetResourceList(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.HSEngine.TMapObjectResourceList Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources;
            return Result;
        }

        // IsoCombatHex.pas:46
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__1 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__1.ImageLibrary;
            Variant = ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            var call__2 = IL[((int)(((uint)FSides) >> 6)) + 40];
            call__2.Show(Xhp, Yhp, SurfaceDesc);
            if (FOpenSides != 0)
            {
                Yhp -= 38;
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        var call__3 = IL[18];
                        call__3.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 2) == 0)
                    {
                        var call__4 = IL[19];
                        call__4.Show(Xhp - 19, (Yhp - 48) - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 34) == 34)
                    {
                        var call__5 = IL[23];
                        call__5.Show(Xhp - 20, (Yhp - 48) - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 34) == 0)
                        {
                            var call__6 = IL[20];
                            call__6.Show(Xhp - 20, (Yhp - 50) - 16, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) != 0)
                        {
                            var call__7 = IL[17];
                            call__7.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__8 = IL[Variant + 3];
                            call__8.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) != 0)
                        {
                            var call__9 = IL[16];
                            call__9.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__10 = IL[Variant + 0];
                            call__10.Show(((Xhp - 19) + 16) + 16, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 20) == 20)
                    {
                        var call__11 = IL[13];
                        call__11.Show(Xhp - 19, Yhp, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 20) == 0)
                        {
                            var call__12 = IL[6];
                            call__12.Show(Xhp - 19, Yhp, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) == 0)
                        {
                            var call__13 = IL[25];
                            call__13.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__14 = IL[15];
                                call__14.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__15 = IL[Variant + 10];
                                call__15.Show((Xhp - 19) - 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) == 0)
                        {
                            var call__16 = IL[24];
                            call__16.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__17 = IL[14];
                                call__17.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__18 = IL[Variant + 7];
                                call__18.Show((Xhp - 19) + 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }

        public new partial class Meta : Aow.Units.IsoHex.TUpperIsometricHexagon.Meta
        {
            // IsoCombatHex.pas:40
            public override int ClassID() => Aow.Units.IsoCombatHex.TIsometricCombatHexagon.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.IsoCombatHex.TIsometricCombatHexagon => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:50
            public new Aow.Units.IsoCombatHex.TIsometricCombatHexagon Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoCombatHex.TIsometricCombatHexagon)base.Create(AOwner);
        }
    }

    // { TIsometricCombatHexagonResource (instance size 56, VMT 00433A24) }
    public partial class TIsometricCombatHexagonResource : Aow.Units.IsoHex.TUpperIsometricHexagonResource
    {
        // IsoCombatHex.pas:108
        public new const int ClassID = 2229515;

        // IsoCombatHex.pas:114
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229514;
            return Result;
        }

        // IsoCombatHex.pas:117
        public override Aow.Units.HSEngine.THexagonControl GetHexagonControl()
        {
            Aow.Units.HSEngine.THexagonControl Result = default;
            Result = ((Aow.Units.AoWE.TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        public new partial class Meta : Aow.Units.IsoHex.TUpperIsometricHexagonResource.Meta
        {
            // IsoCombatHex.pas:108
            public override int ClassID() => Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource.ClassID;

            // IsoCombatHex.pas:111
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Combat ISO-Hex";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
            // IsoHex.pas:64
            public new Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource)base.Create(AOwner);
        }
    }

    public static partial class IsoCombatHex
    {
        // IsoCombatHex.pas:120
        public static void RegisterIsometricCombatHexagon(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.IsoCombatHex.TIsometricCombatHexagon>(), Aow.Units.Engine.EClass.Of<Aow.Units.IsoCombatHex.TIsometricCombatHexagonResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
