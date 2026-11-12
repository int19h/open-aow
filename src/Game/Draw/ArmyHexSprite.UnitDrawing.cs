#nullable disable
using System.Runtime.CompilerServices;

using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class ArmyHexSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int frame;
        int pixelX = default;
        int pixelY = default;
        int deltaX = default;
        int deltaY = default;
        double part;
        if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Owner.FieldFlags).Hi).Lo != 0 || !StrategicMap.Current.FogEnabled)
        {
            if (Direction >= UnitGraphicsResource.Sequences.Count)
            {
                /* TODO: Implement the inline assembly operation. */
                throw new NotImplementedException();
            }

            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
            pixelXSurface = pixelX - pixelXSurface;
            pixelYSurface = pixelY - pixelYSurface;
            if (MoveStep != -1)
            {
                HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
                if ((Status & 16) == 0)
                {
                    part = (double)MoveStep / MoveSteps;
                    pixelXSurface += Round((double)part * deltaX);
                    pixelYSurface += Round((double)part * deltaY);
                }

                if ((Status & 8) == 0)
                {
                    if (Direction == 0)
                    {
                        sequence = UnitGraphicsResource.Sequences.List[9];
                    }
                    else
                    {
                        sequence = UnitGraphicsResource.Sequences.List[Direction + 5];
                    }
                }
                else
                {
                    if (Direction == 0)
                    {
                        sequence = UnitGraphicsResource.Sequences.List[3];
                    }
                    else
                    {
                        sequence = UnitGraphicsResource.Sequences.List[Direction - 1];
                    }
                }
            }
            else
            {
                if (Direction == 0)
                {
                    sequence = UnitGraphicsResource.Sequences.List[3];
                }
                else
                {
                    sequence = UnitGraphicsResource.Sequences.List[Direction - 1];
                }
            }

            if (!Army.Status.Contains(ArmyStatusElement.Concealed))
            {
                if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || StrategicMap.Current.SeatedPlayer != Army.Player || Army.Count != Army.ConcealedUnitsCount())
                {
                    while (sequence != null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    ShowEnchantments(pixelXSurface, pixelYSurface + _unitYOffset, lockedSurface);
                    if (!Army.Status.Contains(ArmyStatusElement.Selected) || (StrategicMap.Current.Frame & 6) != 6)
                    {
                        if (FlagId == 32)
                        {
                            var count = Army.Count;
                            GamePack.Current.FlagControl.Flags[count + 39].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                        }
                        else
                        {
                            var count = Army.Count;
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
                    while (sequence != null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    ShowEnchantments(pixelXSurface, pixelYSurface + _unitYOffset, lockedSurface);
                    if (Army.Status.Contains(ArmyStatusElement.Selected))
                    {
                        if ((StrategicMap.Current.Frame & 6) != 6)
                        {
                            if (FlagId != 32)
                            {
                                var count = Army.Count;
                                GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count + 29].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[50]);
                                if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                                {
                                    GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                                }
                            }
                            else
                            {
                                var count = Army.Count;
                                GamePack.Current.FlagControl.Flags[count + 39].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[50]);
                            }
                        }
                    }
                    else
                    {
                        if (FlagId != 32)
                        {
                            var count = Army.Count;
                            GamePack.Current.RaceResources.GetRaceResource(FlagId).ImageLibrary[count + 29].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[Math.Abs((StrategicMap.Current.Frame << 2) % 60 - 30) + 40]);
                            if (Army.Status.Contains(ArmyStatusElement.ShowCrown))
                            {
                                GamePack.Current.FlagControl.Flags[FlagId + 80].Show(pixelXSurface - 5, pixelYSurface - 14, lockedSurface);
                            }
                        }
                        else
                        {
                            var count = Army.Count;
                            GamePack.Current.FlagControl.Flags[count + 39].ShowBlended(pixelXSurface - 5, pixelYSurface - 12, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[Math.Abs((StrategicMap.Current.Frame << 2) % 60 - 30) + 40]);
                        }
                    }
                }
            }
            else
            {
                if (!Army.Status.Contains(ArmyStatusElement.FullyConcealed))
                {
                    while (sequence != null)
                    {
                        frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (frame != -1)
                        {
                            sequence.ImageLibrary[frame].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + _unitYOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    if (!Army.Status.Contains(ArmyStatusElement.Selected) || (StrategicMap.Current.Frame & 6) != 6)
                    {
                        if (FlagId == 32)
                        {
                            var count = Army.Count;
                            var concealedUnitsCount = Army.ConcealedUnitsCount();
                            GamePack.Current.FlagControl.Flags[count - concealedUnitsCount + 39].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                        }
                        else
                        {
                            var count = Army.Count;
                            var concealedUnitsCount = Army.ConcealedUnitsCount();
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
        ImageSequence sequence;
        float part;
        int pixelX = default;
        int pixelY = default;
        int deltaX = default;
        int deltaY = default;
        int frame;
        if ((Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref Owner.FieldFlags).Hi).Lo != 0 || !StrategicMap.Current.FogEnabled) && !Army.Status.Contains(ArmyStatusElement.FullyConcealed))
        {
            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, ref pixelX, ref pixelY);
            X = pixelX - X;
            Y = pixelY - Y;
            if (MoveStep != -1)
            {
                HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
                if ((Status & 16) == 0)
                {
                    var moveSteps = GetMoveSteps();
                    part = (float)((double)MoveStep / moveSteps);
                    X += Round((double)part * deltaX);
                    Y += Round((double)part * deltaY);
                }
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
                frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                sequence.ImageLibrary[frame].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
                sequence = sequence.Next;
            }
        }
    }

    public void PlayMoveSfx()
    {
        ImageSequence sequence;
        int frame;
        if (MoveStep != -1 && (Status & 8) == 0)
        {
            if (Direction == 0)
            {
                sequence = UnitGraphicsResource.Sequences.List[9];
            }
            else
            {
                sequence = UnitGraphicsResource.Sequences.List[Direction + 5];
            }

            if (sequence != null)
            {
                frame = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                if (sequence.SpeedCount == sequence.SequenceSpeed)
                {
                    UnitGraphicsResource.SfxLibraries[1].Play(frame % 100);
                }
            }
        }
    }
}
