#nullable disable
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class UnitHS
{
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int Index;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        float Progress;
        Assert(FDirection < FUnitGFXResource.FSequences.Count, "Invalid Unit Index", "UnitHS.PAS", 853);
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        XhpSurf = Xhp - XhpSurf;
        YhpSurf = Yhp - YhpSurf;
        if (FMoveStep != -1)
        {
            HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
            if ((FStatus & 16) == 0)
            {
                var moveSteps = GetMoveSteps();
                Progress = (float)((double)FMoveStep / moveSteps);
                XhpSurf += Round((double)DX * Progress);
                YhpSurf += Round((double)DY * Progress);
            }

            if ((FStatus & 8) == 0)
            {
                Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection + 5];
                if (Sequence != null && GetSelected())
                {
                    Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    // sound timing gate
                    if (Sequence.SpeedCount == Sequence.SequenceSpeed)
                    {
                        FUnitGFXResource.FSFXLibraries[1].Play(Index % 100);
                    }
                }
            }
            else
            {
                Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
            }
        }
        else
        {
            Sequence = (ImageSequence)FUnitGFXResource.FSequences.List[FDirection - 1];
        }

        if (!GetSelected() || (AoWHSMap.Current.Frame & 6) != 6)
        {
            while (Sequence != null)
            {
                Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (Index != -1)
                {
                    Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }
    }

    public virtual void ShowShadow(int X, int Y, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int Index;
        float Progress;
        int Xhp = default;
        int Yhp = default;
        int DX = default;
        int DY = default;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        X = Xhp - X;
        Y = Yhp - Y;
        if (FMoveStep != -1)
        {
            HexGeometry.HNtoHP(FDirection, ref DX, ref DY);
            var moveSteps = GetMoveSteps();
            Progress = (float)((double)FMoveStep / moveSteps);
            X += Round((double)DX * Progress);
            Y += Round((double)DY * Progress);
        }

        if (FDirection == 0)
        {
            Sequence = FUnitGFXResource.FSequences.GetImageSequence(27);
        }
        else
        {
            Sequence = FUnitGFXResource.FSequences.GetImageSequence(FDirection + 23);
        }

        while (Sequence != null)
        {
            Index = Sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
            Sequence.ImageLibrary[Index].Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
            Sequence = Sequence.Next;
        }
    }
}
