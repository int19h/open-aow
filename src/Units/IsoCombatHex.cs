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
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

        // IsoCombatHex.pas:46
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TCustomImageLibrary IL;
            int Xhp = default;
            int Yhp = default;
            int Variant;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = Xhp - XhpSurf - 8;
            Yhp = Yhp - YhpSurf;
            IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
            Variant = ((int)(((uint)ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            IL[(int)(((uint)FSides) >> 6) + 40].Show(Xhp, Yhp, SurfaceDesc);
            if (FOpenSides != 0)
            {
                Yhp -= 38;
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 32) == 0)
                    {
                        IL[18].Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 2) == 0)
                    {
                        IL[19].Show(Xhp - 19, Yhp - 48 - 16, SurfaceDesc);
                    }

                    if ((FOpenSides & 34) == 34)
                    {
                        IL[23].Show(Xhp - 20, Yhp - 48 - 16, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 34) == 0)
                        {
                            IL[20].Show(Xhp - 20, Yhp - 50 - 16, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) != 0)
                        {
                            IL[17].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 3].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) != 0)
                        {
                            IL[16].Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            IL[Variant + 0].Show(Xhp - 19 + 16 + 16, Yhp - 16, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 20) == 20)
                    {
                        IL[13].Show(Xhp - 19, Yhp, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 20) == 0)
                        {
                            IL[6].Show(Xhp - 19, Yhp, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 16) != 0)
                    {
                        if ((FOpenSides & 32) == 0)
                        {
                            IL[25].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                IL[15].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                IL[Variant + 10].Show(Xhp - 19 - 32, Yhp - 16, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 4) != 0)
                    {
                        if ((FOpenSides & 2) == 0)
                        {
                            IL[24].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                        }
                        else
                        {
                            if ((FOpenSides & 1) != 0)
                            {
                                IL[14].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
                            }
                            else
                            {
                                IL[Variant + 7].Show(Xhp - 19 + 32, Yhp - 16, SurfaceDesc);
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
        public override int ResourceUserClassID() => 2229514;

        // IsoCombatHex.pas:117
        public override THexagonControl GetHexagonControl() => ((TAoWHSSet)Engine.HSSet).FCombatHexagons;

        // IsoCombatHex.pas:111
        public override AnsiString ResourceEditName() => (AnsiString)"Combat ISO-Hex";

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
