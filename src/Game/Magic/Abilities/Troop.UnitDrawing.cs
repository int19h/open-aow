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
        ImageSequence sequence;
        int frame;
        int totalFrames;
        int resourceIndex;
        int result = 0;
        resourceIndex = GetUnitGraphicsResourceIndex();
        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).AddRef();
        if (info.Animation != UnitAnimation.None)
        {
            sequence = (ImageSequence)AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Sequences.List[(int)info.Animation * 6 + info.Direction - 1];
            if (sequence != null)
            {
                if (!info.Looped)
                {
                    totalFrames = sequence.GetTotalSequenceFrames();
                    frame = sequence.GetUnloopedFrameIndex(info.Frame);
                    if (frame >= 0)
                    {
                        sequence.ImageLibrary[frame].Show(X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }
                    else
                    {
                        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Show(info.Direction, X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }

                    if (totalFrames - info.Frame > result)
                    {
                        result = totalFrames - info.Frame;
                    }
                }
                else
                {
                    frame = sequence.GetFrameIndex(info.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (frame >= 0)
                    {
                        sequence.ImageLibrary[frame].Show(X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
                    }

                    result = 1;
                }

                sequence = sequence.Next;
                while (sequence != null)
                {
                    frame = sequence.GetFrameIndex(info.Frame, 0);
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
                AoWHSSet.Current.AbilityControl.GetAbility(96).ImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, lockedSurface);
            }
            else
            {
                AoWHSSet.Current.AbilityControl.GetAbility(96).ImageSequences.Show(0, X + 32, Y + 32, 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(98))
        {
            if (info.Looped)
            {
                AoWHSSet.Current.AbilityControl.GetAbility(98).ImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, lockedSurface);
            }
            else
            {
                AoWHSSet.Current.AbilityControl.GetAbility(98).ImageSequences.Show(0, X + 32, Y + 32, 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(93))
        {
            if (info.Looped)
            {
                AoWHSSet.Current.AbilityControl.GetAbility(93).ImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, lockedSurface);
            }
            else
            {
                AoWHSSet.Current.AbilityControl.GetAbility(93).ImageSequences.Show(0, X + 32, Y + 32, 4, lockedSurface);
            }
        }

        if (info.Overlays.Contains(UnitOverlayModesElement.HitPoints))
        {
            var hits = GetHits();
            var hitPointsToHitPercent = (byte)HitChance.HitPointsToHitPercent(GetHitPoints(), hits);
            HitChance.ShowHitPercent(X, Y, lockedSurface, hitPointsToHitPercent);
        }

        if (info.Overlays.Contains(UnitOverlayModesElement.Combat))
        {
            if (GetHitPoints() == 0)
            {
                AoWHSSet.Current.CombatImages[4].Show(X + 18, Y + 10, lockedSurface);
            }

            if (GetAbilityEnabled(48))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(48).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(150))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(150).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(149))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(149).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(97))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(97).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(92))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(92).ImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(95))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(95).ImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(94))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(94).ImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(127))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(127).ImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(107))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(107).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(34))
            {
                AoWHSSet.Current.AbilityControl.GetAbility(34).ImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }
        }

        if (UnitStatus.Contains(UnitStatusElement.Enchanted))
        {
            if (info.Looped)
            {
                AoWHSSet.Current.ImageSequences.ShowLooped(70, X + 32, Y + 40, AoWHSMap.Current.Frame, lockedSurface);
            }
            else
            {
                AoWHSSet.Current.ImageSequences.Show(70, X + 32, Y + 40, 5, lockedSurface);
            }
        }

        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Release();
        return result;
    }
}
