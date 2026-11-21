#nullable disable
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
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

public class AnimateRuins : CityTargetSpell
{
    // CitySpells.pas:915
    public AnimateRuins(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_animateRuinsText);
        SpellId = 76;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Enemy, CityTargetModeElement.Razed);
    }

    // CitySpells.pas:938
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        City targetCity;
        if (executeCount == 0)
        {
            targetCity = eventValue.GetSelectedCity();
            LocationTextEventLog log = new(null)
            {
                Holder = CreationHolder(),
                Player = source.Player
            };
            string animateRuins = AbilityRules.TranslateResourceString(GetEngine(), S: s_animateRuinsText);
            log.Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [animateRuins]);
            log.SetLocation(level: targetCity.HexLevel, y: targetCity.HexY, x: targetCity.HexX);
            EventLog.DistributeLocationEventLog(y: targetCity.HexY, level: targetCity.HexLevel, x: targetCity.HexX, log: log, player: source.Player, execute: false);
            _ = log.Release();
            PlayDefaultCityEffects(source, targetCity);
            _ = targetCity.ValidateObservation(source.Player);
            if (targetCity.Resource.ImageSequenceList.ImageSequences[20] is not null)
            {
                ImageSequenceAnimation a = new(null) { Holder = CreationHolder() };
                a.SetImageSequence(targetCity.Resource.ImageSequenceList.ImageSequences[20]);
                a.SetPixelXOffset(a.PixelXOffset - 16);
                a.SetPixelYOffset(a.PixelYOffset - 16);
                a.SetShowHandler(ShowAnimation);
                sbyte hexLevel2 = targetCity.HexLevel;
                sbyte hexY2 = targetCity.HexY;
                sbyte targetCityHexX = targetCity.HexX;
                _ = StrategicMap.Current.PlaceHex(a, targetCityHexX, hexY2, L: hexLevel2);
            }

            return true;
        }
        else
        {
            if (executeCount == 50)
            {
                targetCity = eventValue.GetSelectedCity();
                targetCity.SetRace(StrategicMap.Current.Players.GetPlayers(source.Player).PlayerRace);
                targetCity.SetRazed(false);
                int size = targetCity.Size;
                int count = StrategicMap.Current.Random(size * 2) + 1;
                for (int i = 0; i <= count; i++)
                {
                    Unit unit = new(null) { Holder = CreationHolder() };
                    int random = StrategicMap.Current.Random(2);
                    unit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(UndeadUnitResourceIds[random]));
                    sbyte hexLevel = targetCity.HexLevel;
                    sbyte hexY = targetCity.HexY;
                    _ = unit.Place(targetCity.HexX, hexY, L: hexLevel, source.Player, mode: Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea));
                    _ = unit.Release();
                }

                CastingDone(source, mana: eventValue.EventMana);
                return false;
            }
            else
            {
                return true;
            }
        }
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
        // The spell builds an image sequence. Another animation fails. Null stays null.
        ImageSequenceAnimation imageSequenceAnimation = (ImageSequenceAnimation)animation;
        if (imageSequenceAnimation.ImageSequence.GetUnloopedFrameIndex(imageSequenceAnimation.FrameCounter) == 0 && GetEngine().AnimationDetail != 0)
        {
            int random = Random(2);
            _ = AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[random]);
        }
    }

    private static readonly string s_animateRuinsText = "Animate Ruins";

    private static ReadOnlySpan<int> UndeadUnitResourceIds => [198, 201];
}
