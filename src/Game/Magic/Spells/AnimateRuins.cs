// Translated from CitySpells.pas.
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
        SpellName = AbilityRules.TranslateResourceString(s_animateRuinsText);
        SpellId = 76;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy, CityTargetModeElement.Razed);
    }

    // CitySpells.pas:938
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        LocationTextEventLog log = default;
        ImageSequenceAnimation A = default;
        int count = default;
        Unit unit = default;
        if (executeCount == 0)
        {
            targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
            log = new LocationTextEventLog(null);
            log._player = source.Player;
            var animateRuins = AbilityRules.TranslateResourceString(s_animateRuinsText);
            log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.CastFormat), [animateRuins]);
            var hexLevel = targetCity.GetHexLevel();
            var hexY = targetCity.GetHexY();
            log.SetLocation(targetCity.GetHexX(), hexY, hexLevel);
            var targetCityHexY = targetCity.GetHexY();
            var targetCityHexLevel = targetCity.GetHexLevel();
            var hexX = targetCity.GetHexX();
            EventLog.DistributeLocationEventLog(log, source.Player, hexX, targetCityHexY, targetCityHexLevel, false);
            log.Release();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[20] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[20]);
                A.SetPixelXOffset(A.PixelXOffset - 16);
                A.SetPixelYOffset(A.PixelYOffset - 16);
                A.OnShow = ShowAnimation;
                var hexLevel2 = targetCity.GetHexLevel();
                var hexY2 = targetCity.GetHexY();
                var targetCityHexX = targetCity.GetHexX();
                StrategicMap.Current.PlaceHex(A, targetCityHexX, hexY2, hexLevel2);
            }

            result = true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
                targetCity.SetRace(StrategicMap.Current.Players.GetPlayers(source.Player).PlayerRace);
                targetCity.SetRazed(false);
                var size = targetCity.GetSize();
                count = StrategicMap.Current.Random(size * 2) + 1;
                for (int I = 0; I <= count; I++)
                {
                    unit = new Unit(null);
                    var random = StrategicMap.Current.Random(2);
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(UndeadUnitResourceIds[random]));
                    var hexLevel = targetCity.GetHexLevel();
                    var hexY = targetCity.GetHexY();
                    unit.Place(targetCity.GetHexX(), hexY, hexLevel, source.Player, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    unit.Release();
                }

                CastingDone(source, eventValue.EventMana);
                result = false;
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // CitySpells.pas:924
    public override bool ValidTargetCity(Troop source, City targetCity)
    {
        bool result = base.ValidTargetCity(source, targetCity);
        return result && targetCity.GetRazed();
    }

    // CitySpells.pas:930
    public void ShowAnimation(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        var imageSequenceAnimation = (ImageSequenceAnimation)animation;
        if (imageSequenceAnimation.ImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FrameCounter) == 0 && GameTables.GameEngine.AnimationDetail != 0)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_animateRuinsText = "Animate Ruins";

    private static ReadOnlySpan<int> UndeadUnitResourceIds => [198, 201];
}
