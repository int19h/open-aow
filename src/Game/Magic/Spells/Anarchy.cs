#nullable disable
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
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public partial class Anarchy : CityTargetSpell
{
    // CitySpells.pas:849
    public Anarchy(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_anarchyText);
        SpellId = 72;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy);
    }

    // CitySpells.pas:875
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        City targetCity = default;
        LocationTextEventLog log = default;
        ImageSequenceAnimation a = default;
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            log = new LocationTextEventLog(null)
            {
                Holder = CreationHolder(),
                Player = source.Player,
                Text = Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [SpellName])
            };
            log.SetLocation(level: targetCity.GetHexLevel(), y: targetCity.GetHexY(), x: targetCity.GetHexX());
            StrategicMap.Current.Players.GetPlayers(targetCity.Player).EventLogbook.AddEvent(log);
            _ = log.Release();
            PlayDefaultCityEffects(source, targetCity);
            _ = targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[16] is not null)
            {
                a = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                a.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[16]);
                a.SetPixelXOffset(a.PixelXOffset - 16);
                a.SetPixelYOffset(a.PixelYOffset - 16);
                a.OnShow += ShowAnimation;
                a.Looped = true;
                a.FrameCount = 120;
                sbyte targetCityHexLevel = targetCity.GetHexLevel();
                sbyte targetCityHexY = targetCity.GetHexY();
                sbyte hexX = targetCity.GetHexX();
                _ = StrategicMap.Current.PlaceHex(a, hexX, targetCityHexY, targetCityHexLevel);
            }

            result = true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = eventValue.GetSelectedCity();
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
        result = result && targetCity.Player != 0 && !targetCity.Enchantments.Contains(CityEnchantmentId.AntiMagicShell);

        return result;
    }

    // CitySpells.pas:867
    public void ShowAnimation(Animation animation, int X, int Y, in LockedSurface lockedSurface)
    {
        // The spell builds an image sequence. Another animation fails. Null stays null.
        ImageSequenceAnimation imageSequenceAnimation = (ImageSequenceAnimation)animation;
        if (imageSequenceAnimation.ImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FrameCounter) == 0 && GetEngine().AnimationDetail != 0)
        {
            int random = Random(2);
            _ = AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_anarchyText = "Anarchy";
}
