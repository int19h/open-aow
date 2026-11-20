#nullable disable
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
    public ResurrectHero(CustomNode owner, EngineObject holder) : base(owner)
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
        sbyte x;
        sbyte y;
        sbyte layer;
        if (executeCount == 0)
        {
            MapCoordinate coordinate75 = source.GetLocation();
            x = coordinate75.X;
            y = coordinate75.Y;
            layer = coordinate75.MapLevel;
            _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, layer, 0);
            TwinkleAnimation animation = new(null) { Holder = CreationHolder() };
            animation.Setup(CastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            _ = StrategicMap.Current.PlaceHex(animation, x, y, layer);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(x, y, layer, 0))
            {
                _ = CastSounds.Play(0);
            }
        }
        if (executeCount == 20)
        {
            HeroList heroes = new(null) { Holder = CreationHolder() };
            StrategicMap.Current.HeroControl.ListDeadHeroes(heroes);
            for (int i = heroes.Count - 1; i >= 0; i--)
            {
                if (heroes.GetHero(i) is Leader || heroes.GetHero(i).GetLevel() > StrategicMap.Current.Players.GetPlayers(source.Player).BaseHeroJoinLevel + 2)
                {
                    _ = heroes.RemoveChild(heroes.GetHero(i));
                }
            }

            if (heroes.Count != 0)
            {
                MapCoordinate coordinate76 = source.GetLocation();
                x = coordinate76.X;
                y = coordinate76.Y;
                layer = coordinate76.MapLevel;
                int count = heroes.Count;
                Hero hero = heroes.GetHero(StrategicMap.Current.Random(count));
                hero.Resurrect();
                ArmyHexSprite army = hero.Place(x, y, layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                LocationTextEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    Player = source.Player,
                    Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), s_resurrectedFormat), [hero.HeroName])
                };
                log.SetLocation(level: army.GetHexLevel(), y: army.GetHexY(), x: army.GetHexX());
                EventLog.DistributeLocationEventLog(y: army.GetHexY(), level: army.GetHexLevel(), x: army.GetHexX(), log: log, player: source.Player, execute: false);
                _ = log.Release();
                if (hero.OfferToJoin(source.Player))
                {
                    HeroEmergeEventLog emergeLog = new(null) { Holder = CreationHolder() };
                    emergeLog.Setup(hero, source.Player, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                    _ = emergeLog.Execute(true, ExecuteTrigger.Event, null);
                    _ = emergeLog.Release();
                }
                else
                {
                    ArmyInfoEventLog infoLog = new(null) { Holder = CreationHolder() };
                    int indexOf = army.Army.Units.IndexOf(hero);
                    infoLog.ArmyView.Setup(army.Army, (byte)(1 << indexOf));
                    infoLog.Selection = 255;
                    infoLog.Player = source.Player;
                    string noJoinResurrection = AbilityRules.TranslateResourceString(GetEngine(), s_noJoinResurrectionText);
                    infoLog.Messages.Text = noJoinResurrection;
                    infoLog.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                    _ = infoLog.Execute(true, ExecuteTrigger.Event, null);
                    _ = infoLog.Release();
                }
            }
            else
            {
                string noHeroAnswersYourCall = AbilityRules.TranslateResourceString(GetEngine(), GameText.NoHeroAnswersYourCallText);
                StrategicMap.Current.ShowMessage(noHeroAnswersYourCall);
            }

            CastingDone(source, eventValue.EventMana);
            heroes?.Dispose();
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalSpells.pas:1062
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_noJoinResurrectionText = "I needed that... Goodbye.";
    private static readonly string s_resurrectHeroText = "Resurrect Hero";
    private static readonly string s_resurrectedFormat = "%s resurrected";
}
