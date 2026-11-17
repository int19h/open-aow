#nullable disable
using Aow.Game.Combat;

using Aow.Game.Draw.Images;

using Aow.Game.Host;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

public partial class TacticalBattleUnitHexSprite
{
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int index;
        int deltaX = default;
        int deltaY = default;
        double progress;
        int hexX = default;
        int hexY = default;
        int flyingOffset;
        int race;
        CityWall wall;
        CityWall nextWall;
        LockedSurface lockedSurfaceLocal;
        try
        {
            if (!(Direction < UnitGraphicsResource.Sequences.Count))
            {
                OriginalUnitAssert("Invalid Unit Index", "D:\\AoWDev\\AoWTC\\inc\\TCUnitHS.pas", 4262);
            }

            HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
            pixelXSurface = x - pixelXSurface + OffsetX;
            pixelYSurface = y - pixelYSurface - 8 + OffsetY;
            if (Unit.GetPlayer() > 0)
            {
                sbyte player = Unit.GetPlayer();
                race = StrategicMap.Current.Players.GetPlayers(player).PlayerRace;
            }
            else
            {
                race = 32;
            }

            flyingOffset = Unit.GetAbilityEnabled(1) ? -8 : 0;

            if (Unit.GetAbilityEnabled(36))
            {
                wall = Owner.FindOwnedChild<CityWall>(2229508) ?? Owner.FindOwnedChild(2229510) as CityWall;
                if (MoveStep == -1)
                {
                    if (wall != null && wall.HitPoints > 0)
                    {
                        pixelYSurface -= 32;
                    }
                }
                else
                {
                    sbyte yhx = GetHexY();
                    HexGeometry.CenterHexNumberToHex(GetHexX(), yhx, Direction, ref hexX, ref hexY);
                    nextWall = CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild<CityWall>(2229508) ?? CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2229510) as CityWall;
                    int moveSteps = GetMoveSteps();
                    if (MoveStep < moveSteps / 2)
                    {
                        if (wall != null && wall.HitPoints > 0)
                        {
                            if (nextWall != null && nextWall.HitPoints > 0)
                            {
                                pixelYSurface -= 32;
                            }
                            else
                            {
                                int moveSteps2 = GetMoveSteps();
                                int moveSteps3 = GetMoveSteps();
                                pixelYSurface -= (moveSteps2 - MoveStep) * 32 / moveSteps3;
                            }
                        }
                    }
                    else
                    {
                        if (nextWall != null && nextWall.HitPoints > 0)
                        {
                            if (wall != null && wall.HitPoints > 0)
                            {
                                pixelYSurface -= 32;
                            }
                            else
                            {
                                int moveSteps2 = GetMoveSteps();
                                pixelYSurface -= MoveStep * 32 / moveSteps2;
                            }
                        }
                    }
                }
            }

            switch (Action)
            {
                case SpriteAction.Move:
                case SpriteAction.Busy:
                case SpriteAction.FreeStruck:
                    if (Action == SpriteAction.Move && MoveStep != -1)
                    {
                        HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
                        int moveSteps = GetMoveSteps();
                        progress = (double)MoveStep / moveSteps;
                        pixelXSurface += Round(deltaX * progress);
                        pixelYSurface += Round(deltaY * progress);
                        sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 5];
                    }
                    else
                    {
                        sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction - 1];
                    }

                    while (sequence != null)
                    {
                        index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (index != -1)
                        {
                            sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + flyingOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }

                    break;
                case SpriteAction.MeleeHit:
                case SpriteAction.MeleeMiss:
                case SpriteAction.Ability:
                case SpriteAction.BreathAttack:
                    sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 11];
                    if (sequence != null)
                    {
                        while (sequence != null)
                        {
                            index = sequence.GetUnloopedFrameIndex(ActionFrame);
                            if (index != -1)
                            {
                                sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + flyingOffset, lockedSurface);
                            }

                            sequence = sequence.Next;
                        }
                    }

                    break;
                case SpriteAction.Hit:
                    sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 17];
                    if (sequence != null)
                    {
                        while (sequence != null)
                        {
                            index = sequence.GetUnloopedFrameIndex(ActionFrame);
                            if (index != -1)
                            {
                                sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + flyingOffset, lockedSurface);
                            }

                            sequence = sequence.Next;
                        }
                    }

                    break;
                case SpriteAction.StepUp:
                case SpriteAction.StepBack:
                    sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 5];
                    if (sequence != null)
                    {
                        while (sequence != null)
                        {
                            index = sequence.GetFrameIndex(StrategicMap.Current.Frame + ActionFrame, FrameFixtureInputs.AnimationOffset(this));
                            if (index != -1)
                            {
                                sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY + flyingOffset, lockedSurface);
                            }

                            sequence = sequence.Next;
                        }
                    }

                    break;
                case SpriteAction.RoundAttack:
                case SpriteAction.RoundAttackHit:
                    sequence = GamePack.Current.ImageSequences.ImageSequences[Direction + 15];
                    if (sequence != null)
                    {
                        while (sequence != null)
                        {
                            index = sequence.GetUnloopedFrameIndex(ActionFrame);
                            if (index != -1)
                            {
                                if (sequence.Layer == 0)
                                {
                                    while (!UnitGraphicsResource.Sequences.ImageSequences[Direction + 11].ImageLibrary.ImageExist(index))
                                    {
                                        index -= 1;
                                    }

                                    UnitGraphicsResource.Sequences.ImageSequences[Direction + 11].ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX + 16, pixelYSurface + flyingOffset + sequence.OffsetY + 16, lockedSurface);
                                }
                                else
                                {
                                    sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX + 16, pixelYSurface + sequence.OffsetY + flyingOffset + 16, lockedSurface);
                                }
                            }

                            sequence = sequence.Next;
                        }
                    }

                    break;
            }

            if (Unit.Troop.UnitStatus.Contains(UnitStatusElement.Enchanted))
            {
                GamePack.Current.ImageSequences.ShowLooped(70, pixelXSurface + 16, pixelYSurface + 18 + flyingOffset, StrategicMap.Current.Frame, lockedSurface);
            }

            hexX = 0;
            for (; hexX <= 13; hexX += 1)
            {
                if (Unit.GetAbilityEnabled(StatusAbilities[hexX]))
                {
                    sequence = GamePack.Current.AbilityControl.GetAbility(StatusAbilities[hexX]).ImageSequences.ImageSequences[0];
                    while (sequence != null)
                    {
                        index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        if (index != -1)
                        {
                            sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX + 15, pixelYSurface + sequence.OffsetY + 10 + flyingOffset, lockedSurface);
                        }

                        sequence = sequence.Next;
                    }
                }
            }

            if (!GetSelected() || (CombatMap.Current.Frame & 6) != 6)
            {
                if (race == 32)
                {
                    if (Unit.MovePoints == 0 || Unit.CombatPlayer != CombatMap.Current.SeatedCombatPlayer)
                    {
                        GamePack.Current.FlagControl.Flags[70].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                    }
                    else
                    {
                        GamePack.Current.FlagControl.Flags[_moveIndicator + 69].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                    }
                }
                else
                {
                    if (Unit.MovePoints == 0 || Unit.CombatPlayer != CombatMap.Current.SeatedCombatPlayer)
                    {
                        GamePack.Current.RaceResources.GetRaceResource(race).ImageLibrary[70].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                    }
                    else
                    {
                        GamePack.Current.RaceResources.GetRaceResource(race).ImageLibrary[_moveIndicator + 69].Show(pixelXSurface - 5, pixelYSurface - 12, lockedSurface);
                    }
                }

                lockedSurfaceLocal = lockedSurface;
                hexX = Unit.GetHitPoints() * 10 / Unit.GetHits();
                if (hexX == 0)
                {
                    hexX = 1;
                }

                if (pixelXSurface - 4 + hexX < lockedSurfaceLocal.ClipRectangle.Right)
                {
                    Rect clipRectangle = lockedSurfaceLocal.ClipRectangle;
                    clipRectangle.Right = pixelXSurface - 4 + hexX;
                    lockedSurfaceLocal.ClipRectangle = clipRectangle;
                }

                GamePack.Current.CombatImages[1].ShowClipped(pixelXSurface - 4, pixelYSurface - 4, lockedSurfaceLocal);
            }
        }
        catch
        {
            _ = MessageBox(0, "Error during ShowUnit", NullTerminatedText("Error - " + Unit.Troop.GetName()), 0);
        }
    }

    public override void ShowShadow(int X, int Y, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        float progress;
        int index;
        int deltaX = default;
        int deltaY = default;
        HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int pixelX, out int pixelY);
        X = pixelX - X + OffsetX;
        Y = Busy ? pixelY - Y - 8 : pixelY - Y - 8 + OffsetY;

        if (MoveStep != -1)
        {
            HexGeometry.HexNumberToPixel(Direction, ref deltaX, ref deltaY);
            int moveSteps = GetMoveSteps();
            progress = (float)((double)MoveStep / moveSteps);
            X += Round((double)deltaX * progress);
            Y += Round((double)deltaY * progress);
        }

        sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 23];
        while (sequence != null)
        {
            index = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
            if (index != -1)
            {
                sequence.ImageLibrary[index].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }
    }

    public void NewFrame(object sender)
    {
        ImageSequence sequence;
        if (ActionDone)
        {
            SetAction(SpriteAction.Move);
        }

        if (_moveIndicator == 0)
        {
            _moveIndicator = (short)(Random(5) + 1);
            _indicatorStep = 1;
        }
        else
        {
            _moveIndicator = (short)(_moveIndicator + _indicatorStep);
            if (_moveIndicator == 1)
            {
                _indicatorStep = 1;
            }
            else
            {
                if (_moveIndicator == 6)
                {
                    _indicatorStep = -1;
                }
            }
        }

        switch (Action)
        {
            case SpriteAction.StepUp:
                int moveSteps = GetMoveSteps();
                if (ActionFrame <= moveSteps)
                {
                    int offsetX = OffsetX;
                    int offsetY = OffsetY;
                    HexGeometry.HexNumberToPixel(Direction, ref offsetX, ref offsetY);
                    OffsetX = offsetX;
                    OffsetY = offsetY;
                    int moveSteps2 = GetMoveSteps();
                    OffsetX = ActionFrame * OffsetX / (moveSteps2 * 2);
                    int moveSteps3 = GetMoveSteps();
                    OffsetY = ActionFrame * OffsetY / (moveSteps3 * 2);
                    ActionFrame = (short)(ActionFrame + 2);
                }
                else
                {
                    int offsetX2 = OffsetX;
                    int offsetY2 = OffsetY;
                    HexGeometry.HexNumberToPixel(Direction, ref offsetX2, ref offsetY2);
                    OffsetX = offsetX2;
                    OffsetY = offsetY2;
                    OffsetX /= 2;
                    OffsetY /= 2;
                    ActionDone = true;
                }

                break;
            case SpriteAction.StepBack:
                int moveSteps4 = GetMoveSteps();
                if (ActionFrame <= moveSteps4)
                {
                    int offsetX3 = OffsetX;
                    int offsetY3 = OffsetY;
                    HexGeometry.HexNumberToPixel(Direction, ref offsetX3, ref offsetY3);
                    OffsetX = offsetX3;
                    OffsetY = offsetY3;
                    int moveSteps2 = GetMoveSteps();
                    int moveSteps3 = GetMoveSteps();
                    OffsetX = (moveSteps2 - ActionFrame) * OffsetX / (moveSteps3 * 2);
                    int moveSteps5 = GetMoveSteps();
                    int moveSteps6 = GetMoveSteps();
                    OffsetY = (moveSteps5 - ActionFrame) * OffsetY / (moveSteps6 * 2);
                    ActionFrame = (short)(ActionFrame + 2);
                }
                else
                {
                    OffsetX = 0;
                    OffsetY = 0;
                    ActionDone = true;
                }

                break;
            case SpriteAction.MeleeHit:
            case SpriteAction.MeleeMiss:
            case SpriteAction.Ability:
                sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 11];
                if (sequence != null)
                {
                    ActionFrame = (short)(ActionFrame + 1);
                    int totalSequenceFrames = sequence.GetTotalSequenceFrames();
                    if (ActionFrame >= totalSequenceFrames - 1)
                    {
                        ActionFrame = (short)(sequence.GetTotalSequenceFrames() - 1);
                        ActionDone = true;
                    }
                }

                break;
            case SpriteAction.BreathAttack:
                sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 11];
                if (sequence != null)
                {
                    int totalSequenceFrames = sequence.GetTotalSequenceFrames();
                    if (ActionFrame < totalSequenceFrames - 1)
                    {
                        ActionFrame = (short)(ActionFrame + 1);
                    }
                    else
                    {
                        ActionDone = true;
                    }
                }

                break;
            case SpriteAction.Hit:
                sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 17];
                if (sequence != null)
                {
                    ActionFrame = (short)(ActionFrame + 1);
                    int totalSequenceFrames = sequence.GetTotalSequenceFrames();
                    if (ActionFrame >= totalSequenceFrames - 1)
                    {
                        ActionFrame = (short)(sequence.GetTotalSequenceFrames() - 1);
                        ActionDone = true;
                    }
                }

                break;
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                sequence = GamePack.Current.ImageSequences.ImageSequences[Direction + 16];
                if (sequence != null)
                {
                    ActionFrame = (short)(ActionFrame + 1);
                    int totalSequenceFrames = sequence.GetTotalSequenceFrames();
                    if (ActionFrame >= totalSequenceFrames - 1)
                    {
                        ActionFrame = (short)(sequence.GetTotalSequenceFrames() - 1);
                        ActionDone = true;
                    }
                }

                break;
            default:
                ActionFrame = -1;
                break;
        }

        switch (Action)
        {
            case SpriteAction.Move:
            case SpriteAction.StepUp:
            case SpriteAction.StepBack:
                if (MoveStep != -1)
                {
                    sequence = UnitGraphicsResource.Sequences.ImageSequences[Direction + 5];
                    if (sequence != null && sequence.SpeedCount == sequence.SequenceSpeed && CombatMap.Current.PlaySounds)
                    {
                        int frameIndex = sequence.GetFrameIndex(StrategicMap.Current.Frame, FrameFixtureInputs.AnimationOffset(this));
                        _ = UnitGraphicsResource.SfxLibraries[1].Play(frameIndex % 100);
                    }
                }

                break;
            case SpriteAction.MeleeHit:
            case SpriteAction.MeleeMiss:
            case SpriteAction.Ability:
            case SpriteAction.BreathAttack:
                if (ActionFrame == 0 && CombatMap.Current.PlaySounds)
                {
                    if (SelectedAbility > -1)
                    {
                        if (SelectedAbility == 52)
                        {
                            if (SelectedSpell is not 112 and not 122 and not 119)
                            {
                                _ = GamePack.Current.SpellControl.GetSpell(SelectedSpell).CastSounds.Play(0);
                            }
                        }
                        else
                        {
                            _ = GamePack.Current.AbilityControl.GetAbility(SelectedAbility).SfxLibrary.Play(0);
                        }
                    }
                    else
                    {
                        _ = Action == SpriteAction.MeleeMiss ? UnitGraphicsResource.SfxLibraries[2].Play(1) : UnitGraphicsResource.SfxLibraries[2].Play(0);
                    }
                }

                break;
            case SpriteAction.Hit:
            case SpriteAction.Killed:
                if (ActionFrame == 0 && CombatMap.Current.PlaySounds)
                {
                    _ = Action == SpriteAction.Hit ? UnitGraphicsResource.SfxLibraries[3].Play(0) : UnitGraphicsResource.SfxLibraries[4].Play(0);
                }

                break;
            case SpriteAction.RoundAttack:
            case SpriteAction.RoundAttackHit:
                if (ActionFrame < 18 && CombatMap.Current.PlaySounds && TacticalTables.RangedAttackDirection[ActionFrame] > 0)
                {
                    _ = Action == SpriteAction.RoundAttack
                        ? UnitGraphicsResource.SfxLibraries[2].Play(1)
                        : UnitGraphicsResource.SfxLibraries[2].Play(0);
                }

                break;
        }
    }
}
