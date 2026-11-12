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
        SpellName = AbilityRules.TranslateResourceString(s_anarchyText);
        SpellId = 72;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:875
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        LocationTextEventLog log = default;
        ImageSequenceAnimation A = default;
        if (executeCount == 0)
        {
            targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
            log = new LocationTextEventLog(null);
            log._player = source.Player;
            log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.CastFormat), [SpellName]);
            var hexLevel = targetCity.GetHexLevel();
            var hexY = targetCity.GetHexY();
            log.SetLocation(targetCity.GetHexX(), hexY, hexLevel);
            StrategicMap.Current.Players.GetPlayers(targetCity.Player).EventLogbook.AddEvent(log);
            log.Release();
            PlayDefaultCityEffects(source, targetCity);
            targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[16] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[16]);
                A.SetPixelXOffset(A.PixelXOffset - 16);
                A.SetPixelYOffset(A.PixelYOffset - 16);
                A.OnShow += ShowAnimation;
                A.Looped = true;
                A.FrameCount = 120;
                var targetCityHexLevel = targetCity.GetHexLevel();
                var targetCityHexY = targetCity.GetHexY();
                var hexX = targetCity.GetHexX();
                StrategicMap.Current.PlaceHex(A, hexX, targetCityHexY, targetCityHexLevel);
            }

            result = true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
                targetCity.ExecuteAnarchy();
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

    // CitySpells.pas:858
    public override bool ValidTargetCity(Troop source, City targetCity)
    {
        bool result = base.ValidTargetCity(source, targetCity);
        if (!result || targetCity.Player == 0 || targetCity.Enchantments.Contains(CityEnchantmentId.AntiMagicShell))
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // CitySpells.pas:867
    public void ShowAnimation(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        var imageSequenceAnimation = (ImageSequenceAnimation)animation;
        if (imageSequenceAnimation.ImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FrameCounter) == 0 && GameTables.GameEngine.AnimationDetail != 0)
        {
            var random = Random(2);
            AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_anarchyText = "Anarchy";
}
