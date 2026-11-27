#nullable disable warnings
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public partial class Troop
{
    public virtual int Show(int X, int Y, in LockedSurface lockedSurface, in UnitShowInfo info)
    {
        int result = 0;
        int resourceIndex = UnitGraphicsResourceIndex;
        _ = GetEngine().GamePack.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).AddRef();
        if (info.Animation != UnitAnimation.None)
        {
            GamePack gamePack = GetEngine().GamePack;
            ImageSequence? sequence = gamePack.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Sequences.List[((int)info.Animation * 6) + info.Direction - 1];
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
                        gamePack.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Show(info.Direction, X + sequence.OffsetX + 16, Y + sequence.OffsetY + 20, lockedSurface);
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
                GetEngine().GamePack.AbilityControl[96].ImageSequences.ShowLooped(0, X + 32, Y + 32, GetEngine().StrategicMap.Frame, lockedSurface);
            }
            else
            {
                _ = GetEngine().GamePack.AbilityControl[96].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(98))
        {
            if (info.Looped)
            {
                GetEngine().GamePack.AbilityControl[98].ImageSequences.ShowLooped(0, X + 32, Y + 32, GetEngine().StrategicMap.Frame, lockedSurface);
            }
            else
            {
                _ = GetEngine().GamePack.AbilityControl[98].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (GetAbilityEnabled(93))
        {
            if (info.Looped)
            {
                GetEngine().GamePack.AbilityControl[93].ImageSequences.ShowLooped(0, X + 32, Y + 32, GetEngine().StrategicMap.Frame, lockedSurface);
            }
            else
            {
                _ = GetEngine().GamePack.AbilityControl[93].ImageSequences.Show(0, X + 32, Y + 32, frame: 4, lockedSurface);
            }
        }

        if (info.Overlays.Contains(UnitOverlayModes.HitPoints))
        {
            int hitPointsToHitPercent = HitChance.HitPointsToHitPercent(hits: GetHits(), hitPoints: GetHitPoints()).TruncateToByte();
            HitChance.ShowHitPercent(X, Y, surface: lockedSurface, percent: hitPointsToHitPercent, engine: GetEngine()!);
        }

        if (info.Overlays.Contains(UnitOverlayModes.Combat))
        {
            if (GetHitPoints() == 0)
            {
                GetEngine().GamePack.CombatImages[4].Show(X + 18, Y + 10, lockedSurface);
            }

            if (GetAbilityEnabled(48))
            {
                GetEngine().GamePack.AbilityControl[48].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(150))
            {
                GetEngine().GamePack.AbilityControl[150].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(149))
            {
                GetEngine().GamePack.AbilityControl[149].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(97))
            {
                GetEngine().GamePack.AbilityControl[97].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(92))
            {
                GetEngine().GamePack.AbilityControl[92].ImageSequences.ShowLooped(0, X + 32, Y + 35, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(95))
            {
                GetEngine().GamePack.AbilityControl[95].ImageSequences.ShowLooped(0, X + 32, Y + 32, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(94))
            {
                GetEngine().GamePack.AbilityControl[94].ImageSequences.ShowLooped(0, X + 32, Y + 35, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(127))
            {
                GetEngine().GamePack.AbilityControl[127].ImageSequences.ShowLooped(0, X + 32, Y + 35, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(107))
            {
                GetEngine().GamePack.AbilityControl[107].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }

            if (GetAbilityEnabled(34))
            {
                GetEngine().GamePack.AbilityControl[34].ImageSequences.ShowLooped(0, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }
        }

        if (UnitStatus.Contains(UnitStatuses.Enchanted))
        {
            if (info.Looped)
            {
                GetEngine().GamePack.ImageSequences.ShowLooped(70, X + 32, Y + 40, GetEngine().StrategicMap.Frame, lockedSurface);
            }
            else
            {
                _ = GetEngine().GamePack.ImageSequences.Show(70, X + 32, Y + 40, frame: 5, lockedSurface);
            }
        }

        _ = GetEngine().GamePack.UnitGraphicsResources.GetUnitGraphicsResource(resourceIndex).Release();
        return result;
    }
}
