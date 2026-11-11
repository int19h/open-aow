#nullable disable
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Abilities;

public partial class Troop
{
    public virtual int ShowEx(int X, int Y, in LockedSurface SD, in UnitShowInfo Info)
    {
        ImageSequence Sequence;
        int Frame;
        int TotalFrames;
        int ResourceIndex;
        int Result = 0;
        ResourceIndex = GetUnitGFXResourceIndex();
        AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).AddRef();
        if (Info.Animation != UnitAnimation.None)
        {
            Sequence = (ImageSequence)AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).FSequences.List[(int)Info.Animation * 6 + Info.Direction - 1];
            if (Sequence != null)
            {
                if (!Info.Looped)
                {
                    TotalFrames = Sequence.GetTotalSequenceFrames();
                    Frame = Sequence.GetUnloopedFrameIndex(Info.Frame);
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }
                    else
                    {
                        AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Show(Info.Direction, X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    if (TotalFrames - Info.Frame > Result)
                    {
                        Result = TotalFrames - Info.Frame;
                    }
                }
                else
                {
                    Frame = Sequence.GetFrameIndex(Info.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    Result = 1;
                }

                Sequence = Sequence.Next;
                while (Sequence != null)
                {
                    Frame = Sequence.GetFrameIndex(Info.Frame, 0);
                    if (Frame >= 0)
                    {
                        Sequence.ImageLibrary[Frame].Show(X + Sequence.dXshift + 16, Y + Sequence.dYshift + 20, SD);
                    }

                    Sequence = Sequence.Next;
                }
            }
            else
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        if (GetAbilityEnabled(96))
        {
            if (Info.Looped)
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
            }
            else
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(96).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (GetAbilityEnabled(98))
        {
            if (Info.Looped)
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
            }
            else
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(98).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (GetAbilityEnabled(93))
        {
            if (Info.Looped)
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
            }
            else
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(93).FImageSequences.Show(0, X + 32, Y + 32, 4, SD);
            }
        }

        if (Info.Overlays.Contains(UnitOverlayModesElement.HitPoints))
        {
            var hits = GetHits();
            var hitPointsToHitPcnt = (byte)HitChance.HitPointsToHitPcnt(GetHitPoints(), hits);
            HitChance.ShowHitPcnt(X, Y, SD, hitPointsToHitPcnt);
        }

        if (Info.Overlays.Contains(UnitOverlayModesElement.Combat))
        {
            if (GetHitPoints() == 0)
            {
                AoWHSSet.Current.FCombatImages[4].Show(X + 18, Y + 10, SD);
            }

            if (GetAbilityEnabled(48))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(48).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(150))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(150).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(149))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(149).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(97))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(97).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(92))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(92).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(95))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(95).FImageSequences.ShowLooped(0, X + 32, Y + 32, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(94))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(94).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(127))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(127).FImageSequences.ShowLooped(0, X + 32, Y + 35, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(107))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(107).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }

            if (GetAbilityEnabled(34))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(34).FImageSequences.ShowLooped(0, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }
        }

        if (FUnitStatus.Contains(UnitStatusElement.Enchanted))
        {
            if (Info.Looped)
            {
                AoWHSSet.Current.FImageSequences.ShowLooped(70, X + 32, Y + 40, AoWHSMap.Current.Frame, SD);
            }
            else
            {
                AoWHSSet.Current.FImageSequences.Show(70, X + 32, Y + 40, 5, SD);
            }
        }

        AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(ResourceIndex).Release();
        return Result;
    }
}
