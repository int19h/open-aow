#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public partial class Troop
{
    public virtual int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = 0;
        int resourceIndex = UnitGraphicsResourceIndex;
        _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).AddRef();
        if (info.Animation != UnitAnimation.None)
        {
            ImageSequence sequence = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Sequences.List[((int)info.Animation * 6) + info.Direction - 1];
            if (sequence is not null)
            {
                int frame;
                if (!info.Looped)
                {
                    int totalFrames = sequence.TotalSequenceFrames;
                    frame = sequence.GetUnloopedFrameIndex(info.Frame);
                    if (frame >= 0)
                    {
                        sequence.ImageLibrary[frame].Show(X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }
                    else
                    {
                        GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Show(info.Direction, X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }

                    if (totalFrames - info.Frame > result)
                    {
                        result = totalFrames - info.Frame;
                    }
                }
                else
                {
                    frame = sequence.GetFrameIndex(info.Frame, offset: FrameFixtureInputs.AnimationOffset(this));
                    if (frame >= 0)
                    {
                        sequence.ImageLibrary[frame].Show(X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }

                    result = 1;
                }

                sequence = sequence.Next;
                while (sequence is not null)
                {
                    frame = sequence.GetFrameIndex(info.Frame, offset: 0);
                    if (frame >= 0)
                    {
                        sequence.ImageLibrary[frame].Show(X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        if (GetAbilityEnabled(96))
        {
            if (info.Looped)
            {
                GamePack.Current.AbilityControl[96].ImageSequences.ShowLooped(0, X + 32, Y + 32, StrategicMap.Current.Frame, lockedSurface);
            }
            else
            {
                _ = GamePack.Current.AbilityControl[96].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(98))
        {
            if (info.Looped)
            {
                GamePack.Current.AbilityControl[98].ImageSequences.ShowLooped(0, X + 32, Y + 32, StrategicMap.Current.Frame, lockedSurface);
            }
            else
            {
                _ = GamePack.Current.AbilityControl[98].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(93))
        {
            if (info.Looped)
            {
                GamePack.Current.AbilityControl[93].ImageSequences.ShowLooped(0, X + 32, Y + 32, StrategicMap.Current.Frame, lockedSurface);
            }
            else
            {
                _ = GamePack.Current.AbilityControl[93].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (info.Overlays.Contains(UnitOverlayModesElement.HitPoints))
        {
            byte hitPointsToHitPercent = (byte)HitChance.HitPointsToHitPercent(hits: GetHits(), hitPoints: GetHitPoints());
            HitChance.ShowHitPercent(X, Y, surface: lockedSurface, percent: hitPointsToHitPercent);
        }

        if (info.Overlays.Contains(UnitOverlayModesElement.Combat))
        {
            if (GetHitPoints() == 0)
            {
                GamePack.Current.CombatImages[4].Show(X + 18, Y + 10, lockedSurface);
            }

            if (GetAbilityEnabled(48))
            {
                GamePack.Current.AbilityControl[48].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(150))
            {
                GamePack.Current.AbilityControl[150].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(149))
            {
                GamePack.Current.AbilityControl[149].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(97))
            {
                GamePack.Current.AbilityControl[97].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(92))
            {
                GamePack.Current.AbilityControl[92].ImageSequences.ShowLooped(0, X + 32, Y + 35, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(95))
            {
                GamePack.Current.AbilityControl[95].ImageSequences.ShowLooped(0, X + 32, Y + 32, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(94))
            {
                GamePack.Current.AbilityControl[94].ImageSequences.ShowLooped(0, X + 32, Y + 35, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(127))
            {
                GamePack.Current.AbilityControl[127].ImageSequences.ShowLooped(0, X + 32, Y + 35, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(107))
            {
                GamePack.Current.AbilityControl[107].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(34))
            {
                GamePack.Current.AbilityControl[34].ImageSequences.ShowLooped(0, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }
        }

        if (UnitStatus.Contains(UnitStatusElement.Enchanted))
        {
            if (info.Looped)
            {
                GamePack.Current.ImageSequences.ShowLooped(70, X + 32, Y + 40, StrategicMap.Current.Frame, lockedSurface);
            }
            else
            {
                _ = GamePack.Current.ImageSequences.Show(70, X + 32, Y + 40, frame: 5, lockedSurface);
            }
        }

        _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Release();
        return result;
    }
}
