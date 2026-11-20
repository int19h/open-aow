#nullable disable

using Aow.Game.Draw.Images;
using Aow.Game.Host;
using Aow.Game.Units;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw;

public partial class ArmyHexSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (Owner.FogCount != 0 || !StrategicMap.Current.FogEnabled)
        {
            if (Direction >= UnitGraphicsResource.Sequences.Count)
            {
                /* TODO: Implement the inline assembly operation. */
                throw new NotImplementedException();
            }

            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            pixelXSurface = pixelX - pixelXSurface;
            pixelYSurface = pixelY - pixelYSurface;
            ImageSequence sequence;
            if (MoveStep != -1)
            {
                int deltaX = default;
                int deltaY = default;
                HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
                if ((Status & 0b0001_0000) == 0)
                {
                    double part = (double)MoveStep / MoveSteps;
                    pixelXSurface += Round((double)part * deltaX);
                    pixelYSurface += Round((double)part * deltaY);
                }

                sequence = (Status & 0b1000) == 0
                    ? Direction == 0 ? UnitGraphicsResource.Sequences.List[9] : UnitGraphicsResource.Sequences.List[Direction + 5]
                    : Direction == 0 ? UnitGraphicsResource.Sequences.List[3] : UnitGraphicsResource.Sequences.List[Direction - 1];
            }
            else
            {
                sequence = Direction == 0 ? UnitGraphicsResource.Sequences.List[3] : UnitGraphicsResource.Sequences.List[Direction - 1];
            }

            int frame;
            if (!Army.Status.Contains(ArmyStatusElement.Concealed))
            {
                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || StrategicMap.Current.SeatedPlayer != Army.Player || Army.Units.Count != Army.ConcealedUnitsCount())
                {
                    while (sequence is not null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    ShowEnchantments(pixelXSurface, pixelYSurface + _unitYOffset, lockedSurface);
                    if (!Army.Status.Contains(ArmyStatusElement.Selected) || (StrategicMap.Current.Frame & 0b0110) != 0b0110)
                    {
                        if (FlagId == 32)
                        {
                            int count = Army.Units.Count;
                            GamePack.Current.FlagControl.Flags[count + 39].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                        }
                        else
                        {
                            int count = Army.Units.Count;
                            GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count + 29].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                            if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                            {
                                GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                            }
                        }
                    }
                }
                else
                {
                    while (sequence is not null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    ShowEnchantments(pixelXSurface, pixelYSurface + _unitYOffset, lockedSurface);
                    if (Army.Status.Contains(ArmyStatusElement.Selected))
                    {
                        if ((StrategicMap.Current.Frame & 0b0110) != 0b0110)
                        {
                            if (FlagId != 32)
                            {
                                int count = Army.Units.Count;
                                GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count + 29].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[50]);
                                if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                                }
                            }
                            else
                            {
                                int count = Army.Units.Count;
                                GamePack.Current.FlagControl.Flags[count + 39].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[50]);
                            }
                        }
                    }
                    else
                    {
                        if (FlagId != 32)
                        {
                            int count = Army.Units.Count;
                            GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count + 29].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[Math.Abs(((StrategicMap.Current.Frame << 2) % 60) - 30) + 40]);
                            if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                            {
                                GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                            }
                        }
                        else
                        {
                            int count = Army.Units.Count;
                            GamePack.Current.FlagControl.Flags[count + 39].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[Math.Abs(((StrategicMap.Current.Frame << 2) % 60) - 30) + 40]);
                        }
                    }
                }
            }
            else
            {
                if (!Army.Status.Contains(ArmyStatusElement.FullyConcealed))
                {
                    while (sequence is not null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    if (!Army.Status.Contains(ArmyStatusElement.Selected) || (StrategicMap.Current.Frame & 0b0110) != 0b0110)
                    {
                        if (FlagId == 32)
                        {
                            int count = Army.Units.Count;
                            int concealedUnitsCount = Army.ConcealedUnitsCount();
                            GamePack.Current.FlagControl.Flags[count - concealedUnitsCount + 39].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                        }
                        else
                        {
                            int count = Army.Units.Count;
                            int concealedUnitsCount = Army.ConcealedUnitsCount();
                            GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count - concealedUnitsCount + 29].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                            if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                            {
                                GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                            }
                        }
                    }
                }
            }
        }
    }

    public override void ShowShadow(int X, int Y, in LockedSurface lockedSurface)
    {
        if ((Owner.FogCount != 0 || !StrategicMap.Current.FogEnabled) && !Army.Status.Contains(ArmyStatusElement.FullyConcealed))
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
            X = pixelX - X;
            Y = pixelY - Y;
            if (MoveStep != -1)
            {
                int deltaX = default;
                int deltaY = default;
                HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
                if ((Status & 0b0001_0000) == 0)
                {
                    int moveSteps = GetMoveSteps();
                    float part = (float)((double)MoveStep / moveSteps);
                    X += Round((double)part * deltaX);
                    Y += Round((double)part * deltaY);
                }
            }

            ImageSequence sequence = Direction == 0
                ? UnitGraphicsResource.Sequences.GetImageSequence(27)
                : UnitGraphicsResource.Sequences.GetImageSequence(Direction + 23);

            while (sequence is not null)
            {
                int frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                sequence.ImageLibrary[frame].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                sequence = sequence.Next;
            }
        }
    }

    public void PlayMoveSfx()
    {
        if (MoveStep != -1 && (Status & 0b1000) == 0)
        {
            ImageSequence sequence = Direction == 0 ? UnitGraphicsResource.Sequences.List[9] : UnitGraphicsResource.Sequences.List[Direction + 5];

            if (sequence is not null)
            {
                int frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                if (sequence.SpeedCount == sequence.SequenceSpeed)
                {
                    _ = UnitGraphicsResource.SfxLibraries[1].Play(frame % 100);
                }
            }
        }
    }
}
