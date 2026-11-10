// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class Anarchy : CityTargetSpell
{
    // CitySpells.pas:849
    public Anarchy(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(AnarchyRStr);
        FSpellID = 72;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:875
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        City TargetCity = default;
        LocationTextEventLog Log = default;
        ImageSequenceAnimation A = default;
        if (ExecuteCount == 0)
        {
            TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
            Log = new LocationTextEventLog(null);
            Log.FPlayer = Source.FPlayer;
            Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [FSpellName]);
            var lhx = TargetCity.GetLhx();
            var yhx = TargetCity.GetYhx();
            Log.SetLocation(TargetCity.GetXhx(), yhx, lhx);
            AoWHSMap.Current.FPlayers.GetPlayers(TargetCity.FPlayer).FEventLogbook.AddEvent(Log);
            Log.Release();
            PlayDefaultCityEffects(Source, TargetCity);
            TargetCity.ValidateObservation(Source.FPlayer);
            if (TargetCity.Resource.ImageSequenceList.ImageSequences[16] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(TargetCity.Resource.ImageSequenceList.ImageSequences[16]);
                A.SetXhpOffset(A.FXhpOffset - 16);
                A.SetYhpOffset(A.FYhpOffset - 16);
                A.FOnShow = ShowAnimation;
                A.FLooped = true;
                A.FFrameCount = 120;
                var targetCityLhx = TargetCity.GetLhx();
                var targetCityYhx = TargetCity.GetYhx();
                var xhx = TargetCity.GetXhx();
                AoWHSMap.Current.PlaceHX(A, xhx, targetCityYhx, targetCityLhx);
            }

            Result = true;
        }
        else
        {
            if (ExecuteCount == 50)
            {
                TargetCity = ((CityTargetSpellTE)Event).GetSelectedCity();
                TargetCity.ExecuteAnarchy();
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

    // CitySpells.pas:858
    public override bool ValidTargetCity(AbstractUnit Source, City TargetCity)
    {
        bool Result = base.ValidTargetCity(Source, TargetCity);
        if (!Result || TargetCity.FPlayer == 0 || TargetCity.FEnchantments.Contains(CityEnchantmentID.AntiMagicShell))
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // CitySpells.pas:867
    public void ShowAnimation(Animation Animation, int X, int Y, in SurfaceDesc SD)
    {
        var imageSequenceAnimation = (ImageSequenceAnimation)Animation;
        if (imageSequenceAnimation.FImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FFrameCounter) == 0 && GameTables.AoWEngine.FAnimationDetail != 0)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSFX[random]);
        }
    }

    private static readonly string AnarchyRStr = "Anarchy";
}
