#nullable disable warnings
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw;

public partial class UnitHexSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        Assert(Direction < UnitGraphicsResource.Sequences.Count, message: "Invalid Unit Index", file: "UnitHS.PAS", line: 853);
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        pixelXSurface = pixelX - pixelXSurface;
        pixelYSurface = pixelY - pixelYSurface;
        ImageSequence? sequence;
        int index;
        if (MoveStep != -1)
        {
            int deltaX = default;
            int deltaY = default;
            HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
            if ((Status & 0b0001_0000) == 0)
            {
                int moveSteps = GetMoveSteps();
                float progress = (float)((double)MoveStep / moveSteps);
                pixelXSurface += Round((double)deltaX * progress);
                pixelYSurface += Round((double)deltaY * progress);
            }

            if ((Status & 0b1000) == 0)
            {
                sequence = UnitGraphicsResource.Sequences.List[Direction + 5];
                if (sequence is not null && GetSelected())
                {
                    index = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                    // sound timing gate
                    if (sequence.SpeedCount == sequence.SequenceSpeed)
                    {
                        _ = UnitGraphicsResource.SfxLibraries[1].Play(index % 100);
                    }
                }
            }
            else
            {
                sequence = UnitGraphicsResource.Sequences.List[Direction - 1];
            }
        }
        else
        {
            sequence = UnitGraphicsResource.Sequences.List[Direction - 1];
        }

        if (!GetSelected() || (StrategicMap.Current.Frame & 0b0110) != 0b0110)
        {
            while (sequence is not null)
            {
                index = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
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
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        X = pixelX - X;
        Y = pixelY - Y;
        if (MoveStep != -1)
        {
            int deltaX = default;
            int deltaY = default;
            HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
            int moveSteps = GetMoveSteps();
            float progress = (float)((double)MoveStep / moveSteps);
            X += Round((double)deltaX * progress);
            Y += Round((double)deltaY * progress);
        }

        ImageSequence? sequence = Direction == 0
            ? UnitGraphicsResource.Sequences.GetImageSequence(27)
            : UnitGraphicsResource.Sequences.GetImageSequence(Direction + 23);

        while (sequence is not null)
        {
            int index = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
            sequence.ImageLibrary[index].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
            sequence = sequence.Next;
        }
    }
}
