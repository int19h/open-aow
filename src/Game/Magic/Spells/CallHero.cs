#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public class CallHero : Spell
{
    // GlobalSpells.pas:1184
    public CallHero(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_callHeroText);
        SpellId = 69;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:1250
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        MapCoordinate coordinate55 = source.GetLocation();
        sbyte x = coordinate55.X;
        sbyte y = coordinate55.Y;
        sbyte layer = coordinate55.MapLevel;
        Hero? hero = FindCallHero(source);
        if (hero is not null)
        {
            if (hero.Owner is null)
            {
                hero.SetHitPoints((sbyte)hero.GetHits());
            }

            ArmyHexSprite? army = hero.Place(x, y, L: layer, player: 0, mode: UnitPlaceModes.ScanArea | UnitPlaceModes.CreateNewArmy);
            bool showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(x, y, layer, player: StrategicMap.Current.SeatedPlayer);
            bool condition = StrategicMap.Current.SeatedPlayer == source.Player;
            if (!condition)
            {
                sbyte hexLevel = army.HexLevel;
                sbyte hexY = army.HexY;
                sbyte hexX = army.HexX;
                condition = StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, player: StrategicMap.Current.SeatedPlayer);
            }

            bool showTarget = condition;
            ImageSequenceAnimation animation;
            if (showSource)
            {
                if (GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0));
                    _ = StrategicMap.Current.PlaceHex(animation, x, y, L: layer);
                }
            }

            if (showTarget)
            {
                if (GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl[SpellId].CastImages.GetImageSequence(0));
                    sbyte hexLevel = army.HexLevel;
                    sbyte hexY = army.HexY;
                    sbyte hexX = army.HexX;
                    _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY, L: hexLevel);
                }
            }

            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                sbyte hexLevel = army.HexLevel;
                sbyte hexY = army.HexY;
                sbyte hexX = army.HexX;
                StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            }
            else
            {
                sbyte hexLevel = army.HexLevel;
                sbyte hexY = army.HexY;
                sbyte hexX = army.HexX;
                _ = StrategicMap.Current.ValidateObservation(source.Player, hexX, hexY, hexLevel, radius: 0);
            }

            bool test = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Watching(x, y, L: layer, radius: 0);
            if (!test)
            {
                sbyte hexLevel = army.HexLevel;
                sbyte hexY = army.HexY;
                sbyte hexX = army.HexX;
                test = StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0);
            }

            if (test)
            {
                _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0);
            }
        }
        else
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                string noHeroAnsweredYourCall = AbilityRules.TranslateResourceString(GetEngine(), S: s_noHeroAnsweredYourCallText);
                StrategicMap.Current.ShowMessage(noHeroAnsweredYourCall);
                _ = GamePack.Current.SpellControl[SpellId].CastSounds.Play(0);
            }
        }

        Spell? spell = GamePack.Current.SpellControl[SpellId];
        GamePack.Current.SpellControl[SpellId].CastingDone(source, mana: spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(Troop source)
    {
        string reason = "";
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

    // GlobalSpells.pas:1212
    public Hero? FindCallHero(Troop source)
    {
        DoubleItemList<Hero> heroes = new();
        Hero hero;
        for (int i = StrategicMap.Current.HeroControl.GetCount() - 1; i >= 0; i--)
        {
            hero = StrategicMap.Current.HeroControl[i];
            if (hero.Player == 0 && hero.Owner is Army && hero.ArmyOwner.Location.X != -1)
            {
                _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
            }
        }

        if (heroes.Count == 0)
        {
            for (int i = StrategicMap.Current.HeroControl.GetCount() - 1; i >= 0; i--)
            {
                hero = StrategicMap.Current.HeroControl[i];
                if (!hero.UnitStatus.Contains(UnitStatuses.Killed) && hero.Player <= 0 && hero.Owner is null)
                {
                    _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
                }
            }
        }

        Hero? result;
        if (heroes.Count != 0)
        {
            RandSeed = StrategicMap.Current.Random(16777215);
            int index = heroes.RandomChanceItemB(0);
            result = heroes[index].First;
        }
        else
        {
            result = null;
        }

        heroes?.Dispose();
        return result;
    }

    private static readonly string s_callHeroText = "Call Hero";
    private static readonly string s_noHeroAnsweredYourCallText = "No hero answered your call";
}
