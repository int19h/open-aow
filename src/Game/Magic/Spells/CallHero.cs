#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public class CallHero : Spell
{
    // GlobalSpells.pas:1184
    public CallHero(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_callHeroText);
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
        Hero hero = FindCallHero(source);
        if (hero is not null)
        {
            if (hero.Owner is null)
            {
                hero.SetHitPoints(hero.GetHits());
            }

            ArmyHexSprite army = hero.Place(x, y, layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
            bool showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(x, y, layer, StrategicMap.Current.SeatedPlayer);
            bool condition = StrategicMap.Current.SeatedPlayer == source.Player;
            if (!condition)
            {
                sbyte hexLevel = army.GetHexLevel();
                sbyte hexY = army.GetHexY();
                sbyte hexX = army.GetHexX();
                condition = StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, StrategicMap.Current.SeatedPlayer);
            }

            bool showTarget = condition;
            ImageSequenceAnimation animation;
            if (showSource)
            {
                if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0));
                    _ = StrategicMap.Current.PlaceHex(animation, x, y, layer);
                }
            }

            if (showTarget)
            {
                if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0));
                    sbyte hexLevel = army.GetHexLevel();
                    sbyte hexY = army.GetHexY();
                    sbyte hexX = army.GetHexX();
                    _ = StrategicMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
                }
            }

            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                sbyte hexLevel = army.GetHexLevel();
                sbyte hexY = army.GetHexY();
                sbyte hexX = army.GetHexX();
                StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            }
            else
            {
                sbyte hexLevel = army.GetHexLevel();
                sbyte hexY = army.GetHexY();
                sbyte hexX = army.GetHexX();
                _ = StrategicMap.Current.ValidateObservation(source.Player, hexX, hexY, hexLevel, 0);
            }

            bool test = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Watching(x, y, layer, 0);
            if (!test)
            {
                sbyte hexLevel = army.GetHexLevel();
                sbyte hexY = army.GetHexY();
                sbyte hexX = army.GetHexX();
                test = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
            }

            if (test)
            {
                _ = GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
            }
        }
        else
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                string noHeroAnsweredYourCall = AbilityRules.TranslateResourceString(GetEngine(), s_noHeroAnsweredYourCallText);
                StrategicMap.Current.ShowMessage(noHeroAnsweredYourCall);
                _ = GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
            }
        }

        Spell spell = GamePack.Current.SpellControl.GetSpell(SpellId);
        GamePack.Current.SpellControl.GetSpell(SpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(Troop source)
    {
        string reason = "";
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

    // GlobalSpells.pas:1212
    public Hero FindCallHero(Troop source)
    {
        DoubleItemList<Hero> heroes = new();
        Hero hero;
        for (int i = StrategicMap.Current.HeroControl.GetCount() - 1; i >= 0; i--)
        {
            hero = StrategicMap.Current.HeroControl.GetHero(i);
            if (hero.Player == 0 && hero.Owner is Army && hero.ArmyOwner.Location.X != -1)
            {
                _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
            }
        }

        if (heroes.Count == 0)
        {
            for (int i = StrategicMap.Current.HeroControl.GetCount() - 1; i >= 0; i--)
            {
                hero = StrategicMap.Current.HeroControl.GetHero(i);
                if (!hero.UnitStatus.Contains(UnitStatusElement.Killed) && hero.Player <= 0 && hero.Owner is null)
                {
                    _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
                }
            }
        }

        Hero result;
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
