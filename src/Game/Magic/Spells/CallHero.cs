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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class CallHero : Spell
{
    // GlobalSpells.pas:1184
    public CallHero(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_callHeroText);
        SpellId = 69;
        SpellType = SpellType.Global;
        BaseManaCost = 0;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:1250
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        bool showSource;
        bool showTarget;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        Hero hero;
        ArmyHexSprite army;
        ImageSequenceAnimation animation;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        source.GetLocation(ref X, ref Y, ref layer);
        hero = FindCallHero(source);
        if (hero != null)
        {
            if (hero.Owner == null)
            {
                hero.SetHitPoints(hero.GetHits());
            }

            army = hero.Place(X, Y, layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
            showSource = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.VisibleForPlayer(X, Y, layer, StrategicMap.Current.SeatedPlayer);
            bool condition = StrategicMap.Current.SeatedPlayer == source.Player;
            if (!condition)
            {
                var hexLevel = army.GetHexLevel();
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                condition = StrategicMap.Current.VisibleForPlayer(hexX, hexY, hexLevel, StrategicMap.Current.SeatedPlayer);
            }

            showTarget = condition;
            if (showSource)
            {
                if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0) != null)
                {
                    animation = new ImageSequenceAnimation(null);
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0));
                    StrategicMap.Current.PlaceHex(animation, X, Y, layer);
                }
            }

            if (showTarget)
            {
                if (GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0) != null)
                {
                    animation = new ImageSequenceAnimation(null);
                    animation.SetImageSequence(GamePack.Current.SpellControl.GetSpell(SpellId).CastImages.GetImageSequence(0));
                    var hexLevel = army.GetHexLevel();
                    var hexY = army.GetHexY();
                    var hexX = army.GetHexX();
                    StrategicMap.Current.PlaceHex(animation, hexX, hexY, hexLevel);
                }
            }

            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                var hexLevel = army.GetHexLevel();
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                StrategicMap.Current.CenterViewTo(hexX, hexY, hexLevel);
            }
            else
            {
                var hexLevel = army.GetHexLevel();
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                StrategicMap.Current.ValidateObservation(source.Player, hexX, hexY, hexLevel, 0);
            }

            bool test = StrategicMap.Current.SeatedPlayer == source.Player || StrategicMap.Current.Watching(X, Y, layer, 0);
            if (!test)
            {
                var hexLevel = army.GetHexLevel();
                var hexY = army.GetHexY();
                var hexX = army.GetHexX();
                test = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
            }

            if (test)
            {
                GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
            }
        }
        else
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                var noHeroAnsweredYourCall = AbilityRules.TranslateResourceString(s_noHeroAnsweredYourCallText);
                StrategicMap.Current.ShowMessage(noHeroAnsweredYourCall);
                GamePack.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(0);
            }
        }

        var spell = GamePack.Current.SpellControl.GetSpell(SpellId);
        GamePack.Current.SpellControl.GetSpell(SpellId).CastingDone(source, spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        SpellTokenEvent eventValue;
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null);
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

    // GlobalSpells.pas:1212
    public Hero FindCallHero(Troop source)
    {
        Hero result;
        Hero hero;
        DoubleItemList<Hero> heroes;
        int index;
        heroes = new DoubleItemList<Hero>();
        for (int I = StrategicMap.Current.HeroControl.GetCount() - 1; I >= 0; I--)
        {
            hero = StrategicMap.Current.HeroControl.GetHero(I);
            if (hero.Player == 0 && hero.Owner is Army && hero.ArmyOwner.X != -1)
            {
                if (hero.OfferToJoin(source.Player))
                {
                    heroes.AddAbility(hero, 10);
                }
                else
                {
                    heroes.AddAbility(hero, 1);
                }
            }
        }

        if (heroes.Count == 0)
        {
            for (int I = StrategicMap.Current.HeroControl.GetCount() - 1; I >= 0; I--)
            {
                hero = StrategicMap.Current.HeroControl.GetHero(I);
                if (!hero.UnitStatus.Contains(UnitStatusElement.Killed) && hero.Player <= 0 && hero.Owner == null)
                {
                    if (hero.OfferToJoin(source.Player))
                    {
                        heroes.AddAbility(hero, 10);
                    }
                    else
                    {
                        heroes.AddAbility(hero, 1);
                    }
                }
            }
        }

        if (heroes.Count != 0)
        {
            RandSeed = StrategicMap.Current.Random(16777215);
            index = heroes.RandomChanceItemB(0);
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
