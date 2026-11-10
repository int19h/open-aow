// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

namespace Aow.Game.Draw;

// FCity, FModeStartFrame, FMode, FShellAnimation, FShellEndFrame
public partial class CityOverlayHS : SingleHS
{
    public CityOverlayHS(ECustomNode owner) : base(owner)
    { }

    public City FCity;
    public int FModeStartFrame;
    public CityOverlayMode FMode;
    public AnonymousEnum_saSetup FShellAnimation;
    public int FShellEndFrame;

    // City.pas:718
    public override int GetShowPriority() => FCity.GetShowPriority() + 134217728;

    // City.pas:733
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        int Intensity;
        int Index;
        int Frame;
        ImageSequence Sequence;
        HexGeometry.HXtoHP(FCity.FXhx, FCity.FYhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FCity.FEnchantmentsVisible)
        {
            if (FCity.FEnchantments.Contains(CityEnchantmentID.Warmonger))
            {
                Intensity = Math.Abs((int)(((uint)AoWHSMap.Current.Frame) >> 1) % 20 - 10) + 50;
                Sequence = FCity.FResource.ImageSequenceList.ImageSequences[17];
                while (Sequence != null)
                {
                    Index = Sequence.GetFrameIndex(0, 0);
                    if (Sequence == FCity.FResource.ImageSequenceList.ImageSequences[17])
                    {
                        Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, GfxState.IntensityBlendTable.LookupTables[Intensity]);
                    }
                    else
                    {
                        Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, GfxState.AlphaBlendTable.LookupTables[Intensity]);
                    }

                    Sequence = Sequence.Next;
                }
            }

            if (FCity.FEnchantments.Contains(CityEnchantmentID.GoldRush))
            {
                Intensity = Math.Abs((int)(((uint)AoWHSMap.Current.Frame) >> 1) % 20 - 10) + 70;
                Sequence = FCity.FResource.ImageSequenceList.ImageSequences[18];
                while (Sequence != null)
                {
                    if (Sequence == FCity.FResource.ImageSequenceList.ImageSequences[18])
                    {
                        Index = Sequence.GetFrameIndex(0, 0);
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, GfxState.IntensityBlendTable.LookupTables[Intensity]);
                        }
                    }
                    else
                    {
                        Index = Sequence.GetFrameIndex(0, AoWHSMap.Current.Frame);
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }

            if (FCity.FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
            {
                Sequence = FCity.FResource.ImageSequenceList.ImageSequences[19];
                if (Sequence != null)
                {
                    Intensity = 50;
                    Index = Sequence.GetFrameIndex(0, 0);
                    if (FShellEndFrame != -1 && AoWHSMap.Current.Frame < FShellEndFrame)
                    {
                        switch (FShellAnimation)
                        {
                            case AnonymousEnum_saSetup.Setup:
                                Frame = FShellEndFrame - AoWHSMap.Current.Frame;
                                if (Frame <= 50)
                                {
                                    Intensity = 50 - Frame;
                                }
                                else
                                {
                                    FShellEndFrame = -1;
                                }

                                break;
                            case AnonymousEnum_saSetup.Fade:
                                Frame = FShellEndFrame - AoWHSMap.Current.Frame;
                                if (Frame <= 20)
                                {
                                    Intensity = Intensity + (10 - Math.Abs(Frame - 10)) * 3;
                                }
                                else
                                {
                                    FShellEndFrame = -1;
                                }

                                break;
                        }
                    }

                    Sequence.ImageLibrary[Index].ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, GfxState.IntensityBlendTable.LookupTables[Intensity]);
                }
            }

            if (FMode == CityOverlayMode.Looting)
            {
                FCity.FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 22, (uint)(((MapField)FCity.Owner).Xhx + ((MapField)FCity.Owner).Yhx * 16));
            }
        }
    }

    // City.pas:721
    public void PlaySetupShellAnimation()
    {
        FShellAnimation = AnonymousEnum_saSetup.Setup;
        FShellEndFrame = AoWHSMap.Current.Frame + 50;
    }

    // City.pas:727
    public void PlayFadeShellAnimation()
    {
        FShellAnimation = AnonymousEnum_saSetup.Fade;
        FShellEndFrame = AoWHSMap.Current.Frame + 20;
    }

    // City.pas:815
    public void SetMode(CityOverlayMode Mode)
    {
        if (Mode != FMode)
        {
            FModeStartFrame = AoWHSMap.Current.Frame;
            FMode = Mode;
        }
    }

    public enum AnonymousEnum_saSetup : byte
    {
        Setup = 0, Fade = 1
    }

    public enum CityOverlayMode : byte
    {
        Idle = 0, ShowRazeAnimation = 1, Looting = 2
    }

    // Engine.pas:524

}
