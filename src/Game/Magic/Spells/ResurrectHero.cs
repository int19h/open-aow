// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class ResurrectHero : Spell
{
    // GlobalSpells.pas:980
    public ResurrectHero(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_resurrectHeroText);
        SpellId = 66;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:991
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        TwinkleAnimation animation = default;
        HeroList heroes;
        Hero hero;
        ArmyHexSprite army;
        LocationTextEventLog log;
        HeroEmergeEventLog emergeLog = default;
        ArmyInfoEventLog infoLog = default;
        if (executeCount == 0)
        {
            source.GetLocation(ref X, ref Y, ref layer);
            AoWHSMap.Current.ValidateObservation(source.Player, X, Y, layer, 0);
            animation = new TwinkleAnimation(null);
            animation.Setup(CastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            AoWHSMap.Current.PlaceHex(animation, X, Y, layer);
            if (source.Player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(X, Y, layer, 0))
            {
                CastSounds.Play(0);
            }
        }

        if (executeCount == 20)
        {
            heroes = new HeroList(null);
            AoWHSMap.Current.HeroControl.ListDeadHeroes(heroes);
            for (int I = heroes.Count - 1; I >= 0; I--)
            {
                if (heroes.GetHero(I) is Leader || heroes.GetHero(I).GetLevel() > AoWHSMap.Current.Players.GetPlayers(source.Player).BaseHeroJoinLevel + 2)
                {
                    heroes.RemoveChild(heroes.GetHero(I));
                }
            }

            if (heroes.Count != 0)
            {
                source.GetLocation(ref X, ref Y, ref layer);
                var count = heroes.Count;
                hero = heroes.GetHero(AoWHSMap.Current.Random(count));
                hero.Resurrect();
                army = hero.Place(X, Y, layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                log = new LocationTextEventLog(null);
                log._player = source.Player;
                log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(s_resurrectedFormat), [hero.HeroName]);
                var hexLevel = army.GetHexLevel();
                var hexY = army.GetHexY();
                log.SetLocation(army.GetHexX(), hexY, hexLevel);
                var armyHexY = army.GetHexY();
                var armyHexLevel = army.GetHexLevel();
                var hexX = army.GetHexX();
                EventLog.DistributeLocationEventLog(log, source.Player, hexX, armyHexY, armyHexLevel, false);
                log.Release();
                if (hero.OfferToJoin(source.Player))
                {
                    emergeLog = new HeroEmergeEventLog(null);
                    emergeLog.Setup(hero, source.Player, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                    emergeLog.Execute(true, ExecuteTrigger.Event, null);
                    emergeLog.Release();
                }
                else
                {
                    infoLog = new ArmyInfoEventLog(null);
                    var indexOf = army.Army.IndexOf(hero);
                    infoLog.ArmyView.Setup(army.Army, (byte)(1 << indexOf));
                    infoLog.Selection = 255;
                    infoLog._player = source.Player;
                    var noJoinResurrection = AbilityRules.TranslateResourceString(s_noJoinResurrectionText);
                    infoLog.Messages.Text = noJoinResurrection;
                    infoLog.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                    infoLog.Execute(true, ExecuteTrigger.Event, null);
                    infoLog.Release();
                }
            }
            else
            {
                var noHeroAnswersYourCall = AbilityRules.TranslateResourceString(GameText.s_noHeroAnswersYourCallText);
                AoWHSMap.Current.ShowMessage(noHeroAnswersYourCall);
            }

            CastingDone(source, eventValue.EventMana);
            heroes?.Dispose();
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // GlobalSpells.pas:1062
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        SpellTokenEvent eventValue;
        base.Activate(source);
        if (CanActivate(source, ref reason) && !AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_noJoinResurrectionText = "I needed that... Goodbye.";
    private static readonly string s_resurrectHeroText = "Resurrect Hero";
    private static readonly string s_resurrectedFormat = "%s resurrected";
}
