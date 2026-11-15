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
    public AnimateRuins(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_animateRuinsText);
        SpellId = 76;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy, CityTargetModeElement.Razed);
    }

    // CitySpells.pas:938
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        LocationTextEventLog log = default;
        ImageSequenceAnimation a = default;
        int count = default;
        Unit unit = default;
        if (executeCount == 0)
        {
            targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
            log = new LocationTextEventLog(null) { Holder = CreationHolder() };
            log._player = source.Player;
            var animateRuins = AbilityRules.TranslateResourceString(GetEngine(), s_animateRuinsText);
            log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [animateRuins]);
            log.SetLocation(level: targetCity.GetHexLevel(), y: targetCity.GetHexY(), x: targetCity.GetHexX());
            EventLog.DistributeLocationEventLog(y: targetCity.GetHexY(), level: targetCity.GetHexLevel(), x: targetCity.GetHexX(), log: log, player: source.Player, execute: false);
            log.Release();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[20] != null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[20]);
                a.SetPixelXOffset(a.PixelXOffset - 16);
                a.SetPixelYOffset(a.PixelYOffset - 16);
                a.OnShow += ShowAnimation;
                var hexLevel2 = targetCity.GetHexLevel();
                var hexY2 = targetCity.GetHexY();
                var targetCityHexX = targetCity.GetHexX();
                StrategicMap.Current.PlaceHex(a, targetCityHexX, hexY2, hexLevel2);
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
                for (int i = 0; i <= count; i++)
                {
                    unit = new Unit(null) { Holder = CreationHolder() };
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
        if (imageSequenceAnimation.ImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FrameCounter) == 0 && GetEngine().AnimationDetail != 0)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_animateRuinsText = "Animate Ruins";

    private static ReadOnlySpan<int> UndeadUnitResourceIds => [198, 201];
}
