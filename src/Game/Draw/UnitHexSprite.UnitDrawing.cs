#nullable disable
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class UnitHexSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int index;
        int pixelX = default;
        int pixelY = default;
        int deltaX = default;
        int deltaY = default;
        float progress;
        Assert(Direction < UnitGraphicsResource.Sequences.Count, "Invalid Unit Index", "UnitHS.PAS", 853);
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        if (MoveStep != -1)
        {
            HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
            if ((Status & 16) == 0)
            {
                var moveSteps = GetMoveSteps();
                progress = (float)((double)MoveStep / moveSteps);
                pixelXSurface += Round((double)deltaX * progress);
                pixelYSurface += Round((double)deltaY * progress);
            }

            if ((Status & 8) == 0)
            {
                sequence = (ImageSequence)UnitGraphicsResource.Sequences.List[Direction + 5];
                if (sequence != null && GetSelected())
                {
                    index = sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                    // sound timing gate
                    if (sequence.SpeedCount == sequence.SequenceSpeed)
                    {
                        UnitGraphicsResource.SfxLibraries[1].Play(index % 100);
                    }
                }
            }
            else
            {
                sequence = (ImageSequence)UnitGraphicsResource.Sequences.List[Direction - 1];
            }
        }
        else
        {
            sequence = (ImageSequence)UnitGraphicsResource.Sequences.List[Direction - 1];
        }

        if (!GetSelected() || (AoWHSMap.Current.Frame & 6) != 6)
        {
            while (sequence != null)
            {
                index = sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (index != -1)
                {
                    sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                }

                sequence = sequence.Next;
            }
        }
    }

    public virtual void ShowShadow(int X, int Y, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int index;
        float progress;
        int pixelX = default;
        int pixelY = default;
        int deltaX = default;
        int deltaY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
        X = pixelX - X;
        Y = pixelY - Y;
        if (MoveStep != -1)
        {
            HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
            var moveSteps = GetMoveSteps();
            progress = (float)((double)MoveStep / moveSteps);
            X += Round((double)deltaX * progress);
            Y += Round((double)deltaY * progress);
        }

        if (Direction == 0)
        {
            sequence = UnitGraphicsResource.Sequences.GetImageSequence(27);
        }
        else
        {
            sequence = UnitGraphicsResource.Sequences.GetImageSequence(Direction + 23);
        }

        while (sequence != null)
        {
            index = sequence.GetFrameIndex(AoWHSMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
            sequence.ImageLibrary[index].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
            sequence = sequence.Next;
        }
    }
}
