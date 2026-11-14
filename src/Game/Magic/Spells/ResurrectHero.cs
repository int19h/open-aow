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
    public ResurrectHero(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_resurrectHeroText);
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
            source.GetLocation(out X, out Y, out layer);
            StrategicMap.Current.ValidateObservation(source.Player, X, Y, layer, 0);
            animation = new TwinkleAnimation(null) { Holder = CreationHolder() };
            animation.Setup(CastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            StrategicMap.Current.PlaceHex(animation, X, Y, layer);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(X, Y, layer, 0))
            {
                CastSounds.Play(0);
            }
        }

        if (executeCount == 20)
        {
            heroes = new HeroList(null) { Holder = CreationHolder() };
            StrategicMap.Current.HeroControl.ListDeadHeroes(heroes);
            for (int I = heroes.Count - 1; I >= 0; I--)
            {
                if (heroes.GetHero(I) is Leader || heroes.GetHero(I).GetLevel() > StrategicMap.Current.Players.GetPlayers(source.Player).BaseHeroJoinLevel + 2)
                {
                    heroes.RemoveChild(heroes.GetHero(I));
                }
            }

            if (heroes.Count != 0)
            {
                source.GetLocation(out X, out Y, out layer);
                var count = heroes.Count;
                hero = heroes.GetHero(StrategicMap.Current.Random(count));
                hero.Resurrect();
                army = hero.Place(X, Y, layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                log = new LocationTextEventLog(null) { Holder = CreationHolder() };
                log._player = source.Player;
                log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_resurrectedFormat), [hero.HeroName]);
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
                    emergeLog = new HeroEmergeEventLog(null) { Holder = CreationHolder() };
                    emergeLog.Setup(hero, source.Player, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                    emergeLog.Execute(true, ExecuteTrigger.Event, null);
                    emergeLog.Release();
                }
                else
                {
                    infoLog = new ArmyInfoEventLog(null) { Holder = CreationHolder() };
                    var indexOf = army.Army.Units.IndexOf(hero);
                    infoLog.ArmyView.Setup(army.Army, (byte)(1 << indexOf));
                    infoLog.Selection = 255;
                    infoLog._player = source.Player;
                    var noJoinResurrection = AbilityRules.TranslateResourceString(GetEngine(), s_noJoinResurrectionText);
                    infoLog.Messages.Text = noJoinResurrection;
                    infoLog.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                    infoLog.Execute(true, ExecuteTrigger.Event, null);
                    infoLog.Release();
                }
            }
            else
            {
                var noHeroAnswersYourCall = AbilityRules.TranslateResourceString(GetEngine(), GameText.NoHeroAnswersYourCallText);
                StrategicMap.Current.ShowMessage(noHeroAnswersYourCall);
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
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
