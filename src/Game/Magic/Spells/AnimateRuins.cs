// Translated from CitySpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class AnimateRuins : CityTargetSpell
{
    // CitySpells.pas:915
    public AnimateRuins(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(AnimateRuinsRStr);
        FSpellID = 76;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy, CityTargetModeElement.Razed);
    }

    // CitySpells.pas:938
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        bool Result = default;
        City TargetCity = default;
        LocationTextEventLog Log = default;
        ImageSequenceAnimation A = default;
        int Count = default;
        Unit AUnit = default;
        if (ExecuteCount == 0)
        {
            TargetCity = ((CityTargetSpellTokenEvent)Event).GetSelectedCity();
            Log = new LocationTextEventLog(null);
            Log.FPlayer = Source.FPlayer;
            var animateRuins = AbilityRules.TranslateRStr(AnimateRuinsRStr);
            Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [animateRuins]);
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
            var targetCityYhx = TargetCity.GetYhx();
            var targetCityLhx = TargetCity.GetLhx();
            var xhx = TargetCity.GetXhx();
            EventLog.DistributeLocationEventLog(Log, Source.FPlayer, xhx, targetCityYhx, targetCityLhx, false);
            Log.Release();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.ValidateObservation(Source.FPlayer);
            if (TargetCity.Resource.ImageSequenceList.ImageSequences[20] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[20]);
                A.SetXhpOffset(A.FXhpOffset - 16);
                A.SetYhpOffset(A.FYhpOffset - 16);
                A.FOnShow = ShowAnimation;
                var lhx2 = TargetCity.GetLhx();
                var yhx2 = TargetCity.GetYhx();
                var targetCityXhx = TargetCity.GetXhx();
                AoWHSMap.Current.PlaceHX(A, targetCityXhx, yhx2, lhx2);
            }

            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((CityTargetSpellTokenEvent)Event).GetSelectedCity();
                TargetCity.SetRace(AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FPlayerRace);
                TargetCity.SetRazed(false);
                var size = TargetCity.GetSize();
                Count = AoWHSMap.Current.Random(size * 2) + 1;
                for (int I = 0; I <= Count; I++)
                {
                    AUnit = new Unit(null);
                    var random = AoWHSMap.Current.Random(2);
                    AUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TAnimateRuins_ExecuteTE__UndeadUnits[random]));
                    var lhx = TargetCity.GetLhx();
                    var yhx = TargetCity.GetYhx();
                    AUnit.Place(TargetCity.GetXhx(), yhx, lhx, Source.FPlayer, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    AUnit.Release();
                }

                CastingDone(Source, Event.FEventMana);
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // CitySpells.pas:924
    public override bool ValidTargetCity(Troop Source, City TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        return Result && TargetCity.GetRazed();
    }

    // CitySpells.pas:930
    public void ShowAnimation(Animation Animation, int X, int Y, in LockedSurface SD)
    {
        var imageSequenceAnimation = (ImageSequenceAnimation)Animation;
        if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && GameTables.AoWEngine.FAnimationDetail != 0)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[random]);
        }
    }

    private static readonly string AnimateRuinsRStr = "Animate Ruins";
    private static Int32Array2 TAnimateRuins_ExecuteTE__UndeadUnits = Int32Array2.From([198, 201]);
}
