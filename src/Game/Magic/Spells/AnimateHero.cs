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
public class AnimateHero : Spell
{
    // GlobalSpells.pas:1080
    public AnimateHero(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_animateHeroText);
        SpellId = 67;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:1091
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte x;
        sbyte y;
        sbyte layer;
        if (executeCount == 0)
        {
            MapCoordinate coordinate51 = source.GetLocation();
            x = coordinate51.X;
            y = coordinate51.Y;
            layer = coordinate51.MapLevel;
            _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, layer, radius: 0);
            TwinkleAnimation animation = new(null) { Holder = CreationHolder() };
            animation.Setup(CastImages.GetImageSequence(0), mode: Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), count: 50, radius: 64, duration: 50, startDelay: 30);
            _ = StrategicMap.Current.PlaceHex(animation, x, y, L: layer);
            if (source.Player == StrategicMap.Current.SeatedPlayer || StrategicMap.Current.Watching(x, y, L: layer, radius: 0))
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
                MapCoordinate coordinate52 = source.GetLocation();
                x = coordinate52.X;
                y = coordinate52.Y;
                layer = coordinate52.MapLevel;
                int count = heroes.Count;
                Hero hero = heroes.GetHero(StrategicMap.Current.Random(count));
                hero.Animate();
                ArmyHexSprite army = hero.Place(x, y, L: layer, player: 0, mode: Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                LocationTextEventLog log = new(null)
                {
                    Holder = CreationHolder(),
                    Player = source.Player,
                    Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_animatedFormat), args: [hero.HeroName])
                };
                log.SetLocation(level: army.HexLevel, y: army.HexY, x: army.HexX);
                EventLog.DistributeLocationEventLog(y: army.HexY, level: army.HexLevel, x: army.HexX, log: log, player: source.Player, execute: false);
                _ = log.Release();
                if (hero.OfferToJoin(source.Player))
                {
                    HeroEmergeEventLog emergeLog = new(null) { Holder = CreationHolder() };
                    emergeLog.Setup(hero, source.Player, mode: HeroEmergeEventLog.HeroJoinMode.Resurrected);
                    _ = emergeLog.Execute(true, trigger: ExecuteTrigger.Event, done: null);
                    _ = emergeLog.Release();
                }
                else
                {
                    ArmyInfoEventLog infoLog = new(null) { Holder = CreationHolder() };
                    int indexOf = army.Army.Units.IndexOf(hero);
                    infoLog.ArmyView.Setup(army.Army, selection: (byte)(1 << indexOf));
                    infoLog.Selection = 255;
                    infoLog.Player = source.Player;
                    string noJoinAnimate = AbilityRules.TranslateResourceString(GetEngine(), S: s_noJoinAnimateText);
                    infoLog.Messages.Text = noJoinAnimate;
                    infoLog.Triggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                    _ = infoLog.Execute(true, trigger: ExecuteTrigger.Event, done: null);
                    _ = infoLog.Release();
                }
            }
            else
            {
                if (source.Player == StrategicMap.Current.SeatedPlayer)
                {
                    string noHeroAnswersYourCall = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NoHeroAnswersYourCallText);
                    StrategicMap.Current.ShowMessage(noHeroAnswersYourCall);
                    _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0);
                }
            }

            CastingDone(source, mana: eventValue.EventMana);
            heroes?.Dispose();
            return false;
        }
        else
        {
            return true;
        }
    }

    // GlobalSpells.pas:1166
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, messageText: ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_animateHeroText = "Animate Hero";
    private static readonly string s_noJoinAnimateText = "Who dares to call me! Do you really think I would serve you?!";
    private static readonly string s_animatedFormat = "%s animated";
}
