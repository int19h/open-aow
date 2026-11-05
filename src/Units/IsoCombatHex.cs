// Translated from IsoCombatHex.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.IsoCombatHex
{
    public partial class TIsometricCombatHexagon : TUpperIsometricHexagon
    {
        public TIsometricCombatHexagon(TECustomNode owner) : base(owner)
        {}

        // IsoCombatHex.pas:40
        public new const int ClassID = 2229514;

        // IsoCombatHex.pas:43
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // IsoCombatHex.pas:46
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TCustomImageLibrary IL = default;
            int Xhp = default;
            int Yhp = default;
            int Variant = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf;
            var call__1 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__1.ImageLibrary;
            Variant = ((int)(((uint)ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            var call__2 = IL[(int)(((uint)FSides) >> 6) + 40];
            call__2.Show(Xhp, Yhp, SurfaceDesc);
            if (FOpenSides != 0)
            {
                Yhp -= 38;
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        var call__3 = IL[18];
                        call__3.Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 2) == 0)
                    {
                        var call__4 = IL[19];
                        call__4.Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 34) == 34)
                    {
                        var call__5 = IL[23];
                        call__5.Show(Xhp - 20, Yhp - 48 - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 34) == 0)
                        {
                            var call__6 = IL[20];
                            call__6.Show(Xhp - 20, Yhp - 50 - 16, SurfaceDesc);
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
                            call__7.Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__8 = IL[Variant + 3];
                            call__8.Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) != 0)
                        {
                            var call__9 = IL[16];
                            call__9.Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            var call__10 = IL[Variant + 0];
                            call__10.Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
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
                            call__13.Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__14 = IL[15];
                                call__14.Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__15 = IL[Variant + 10];
                                call__15.Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) == 0)
                        {
                            var call__16 = IL[24];
                            call__16.Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                var call__17 = IL[14];
                                call__17.Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                var call__18 = IL[Variant + 7];
                                call__18.Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }
                }
            }
        }

    }

    public partial class TIsometricCombatHexagonResource : TUpperIsometricHexagonResource
    {
        public TIsometricCombatHexagonResource(TECustomNode owner) : base(owner)
        {}

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
        public override THexagonControl GetHexagonControl()
        {
            THexagonControl Result = default;
            Result = ((TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        // IsoCombatHex.pas:111
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat ISO-Hex";
            return Result;
        }

    }

    public static partial class IsoCombatHex
    {
        // IsoCombatHex.pas:120
        public static void RegisterIsometricCombatHexagon(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TIsometricCombatHexagon>(), EClass.Of<TIsometricCombatHexagonResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
