// Translated from IsoCombatHex.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class IsometricCombatHexagon : UpperIsometricHexagon
{
    public IsometricCombatHexagon(ECustomNode owner) : base(owner)
    { }

    // IsoCombatHex.pas:40
    public new const int ClassID = 2229514;

    // IsoCombatHex.pas:43
    public override MapObjectResourceList GetResourceList(Engine.Engine AEngine) => AoWHSSet.Current.FCombatResources;

    // IsoCombatHex.pas:46
    public override void ShowDynamic(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        CustomImageLibrary IL;
        int Xhp = default;
        int Yhp = default;
        int Variant;
        Aow.Game.World.Map.HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        Xhp = Xhp - XhpSurf - 8;
        Yhp = Yhp - YhpSurf;
        IL = FResource.ImageSequenceList.ImageSequences[0].ImageLibrary;
        Variant = FrameFixtureInputs.CombatTextureVariant(this);
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

    public new partial class Resource : UpperIsometricHexagon.Resource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // IsoCombatHex.pas:108
        public new const int ClassID = 2229515;

        // IsoCombatHex.pas:114
        public override int ResourceUserClassID() => 2229514;

        // IsoCombatHex.pas:117
        public override HexagonControl GetHexagonControl() => ((AoWHSSet)Engine.HSSet).FCombatHexagons;

        // IsoCombatHex.pas:111
        public override string ResourceEditName() => "Combat ISO-Hex";

    }

}
