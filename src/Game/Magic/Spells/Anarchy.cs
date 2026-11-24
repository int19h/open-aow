#nullable disable warnings
// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class Anarchy : CityTargetSpell
{
    // CitySpells.pas:849
    public Anarchy(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_anarchyText);
        SpellId = 72;
        CityTargetMode = CityTargetModes.Enemy;
    }

    // CitySpells.pas:875
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        City targetCity;
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            LocationTextEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Player = source.Player,
                Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [SpellName])
            };
            log.SetLocation(level: targetCity.HexLevel, y: targetCity.HexY, x: targetCity.HexX);
            StrategicMap.Current.Players.GetPlayers(targetCity.Player).EventLogbook.AddEvent(log);
            _ = log.Release();
            PlayDefaultCityEffects(source, targetCity);
            _ = targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[16] is not null)
            {
                ImageSequenceAnimation a = new(null, show: ShowAnimation) { Holder = CreationHolder() };
                a.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[16]);
                a.SetPixelXOffset(a.PixelXOffset - 16);
                a.SetPixelYOffset(a.PixelYOffset - 16);
                a.Looped = true;
                a.FrameCount = 120;
                int targetCityHexLevel = targetCity.HexLevel;
                int targetCityHexY = targetCity.HexY;
                int hexX = targetCity.HexX;
                _ = StrategicMap.Current.PlaceHex(a, hexX, targetCityHexY, L: targetCityHexLevel);
            }

            return true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = eventValue.GetSelectedCity();
                targetCity.ExecuteAnarchy();
                CastingDone(source, mana: eventValue.EventMana);
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    // CitySpells.pas:858
    public override bool ValidTargetCity(Troop source, City? targetCity)
    {
        bool result = base.ValidTargetCity(source, targetCity);
        return result && targetCity.Player != PlayerId.Independent && !targetCity.Enchantments.Contains(CityEnchantmentIds.AntiMagicShell);
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
