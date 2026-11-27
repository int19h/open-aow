#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
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
        int x = coordinate55.X;
        int y = coordinate55.Y;
        int layer = coordinate55.MapLevel;
        Hero? hero = FindCallHero(source);
        if (hero is not null)
        {
            if (hero.Owner is null)
            {
                hero.SetHitPoints((sbyte)hero.GetHits());
            }

            ArmyHexSprite? army = hero.Place(x, y, L: layer, player: PlayerId.Independent, mode: UnitPlaceModes.ScanArea | UnitPlaceModes.CreateNewArmy);
            StrategicMap strategicMap = GetEngine().StrategicMap;
            bool showSource = strategicMap.SeatedPlayer == source.Player || strategicMap.VisibleForPlayer(x, y, layer, player: strategicMap.SeatedPlayer);
            StrategicMap engineStrategicMap = GetEngine().StrategicMap;
            bool condition = engineStrategicMap.SeatedPlayer == source.Player;
            if (!condition)
            {
                int hexLevel = army.HexLevel;
                int hexY = army.HexY;
                int hexX = army.HexX;
                condition = engineStrategicMap.VisibleForPlayer(hexX, hexY, hexLevel, player: engineStrategicMap.SeatedPlayer);
            }

            bool showTarget = condition;
            ImageSequenceAnimation animation;
            if (showSource)
            {
                GamePack gamePack = GetEngine().GamePack;
                if (gamePack.SpellControl[SpellId].CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(gamePack.SpellControl[SpellId].CastImages.GetImageSequence(0));
                    _ = GetEngine().StrategicMap.PlaceHex(animation, x, y, L: layer);
                }
            }

            if (showTarget)
            {
                GamePack engineGamePack = GetEngine().GamePack;
                if (engineGamePack.SpellControl[SpellId].CastImages.GetImageSequence(0) is not null)
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(engineGamePack.SpellControl[SpellId].CastImages.GetImageSequence(0));
                    int hexLevel = army.HexLevel;
                    int hexY = army.HexY;
                    int hexX = army.HexX;
                    _ = GetEngine().StrategicMap.PlaceHex(animation, hexX, hexY, L: hexLevel);
                }
            }

            StrategicMap centerViewToStrategicMap = GetEngine().StrategicMap;
            if (centerViewToStrategicMap.SeatedPlayer == source.Player)
            {
                int hexLevel = army.HexLevel;
                int hexY = army.HexY;
                int hexX = army.HexX;
                centerViewToStrategicMap.CenterViewTo(hexX, hexY, hexLevel);
            }
            else
            {
                int hexLevel = army.HexLevel;
                int hexY = army.HexY;
                int hexX = army.HexX;
                _ = GetEngine().StrategicMap.ValidateObservation(source.Player, hexX, hexY, hexLevel, radius: 0);
            }

            StrategicMap watchingStrategicMap = GetEngine().StrategicMap;
            bool test = watchingStrategicMap.SeatedPlayer == source.Player || watchingStrategicMap.Watching(x, y, L: layer, radius: 0);
            if (!test)
            {
                int hexLevel = army.HexLevel;
                int hexY = army.HexY;
                int hexX = army.HexX;
                test = watchingStrategicMap.Watching(hexX, hexY, L: hexLevel, radius: 0);
            }

            if (test)
            {
                _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(0);
            }
        }
        else
        {
            if (GetEngine().StrategicMap.SeatedPlayer == source.Player)
            {
                string noHeroAnsweredYourCall = AbilityRules.TranslateResourceString(GetEngine(), S: s_noHeroAnsweredYourCallText);
                GetEngine().StrategicMap.ShowMessage(noHeroAnsweredYourCall);
                _ = GetEngine().GamePack.SpellControl[SpellId].CastSounds.Play(0);
            }
        }

        GamePack castingDoneGamePack = GetEngine().GamePack;
        Spell? spell = castingDoneGamePack.SpellControl[SpellId];
        castingDoneGamePack.SpellControl[SpellId].CastingDone(source, mana: spell.BaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, messageText: ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
        DoubleItemList<Hero> heroes = [];
        Hero hero;
        for (int i = GetEngine().StrategicMap.HeroControl.GetCount() - 1; i >= 0; i--)
        {
            hero = GetEngine().StrategicMap.HeroControl[i];
            if (hero.Player == PlayerId.Independent && hero.Owner is Army && hero.ArmyOwner.Location.X != -1)
            {
                _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
            }
        }

        if (heroes.Count == 0)
        {
            for (int i = GetEngine().StrategicMap.HeroControl.GetCount() - 1; i >= 0; i--)
            {
                hero = GetEngine().StrategicMap.HeroControl[i];
                if (!hero.UnitStatus.Contains(UnitStatuses.Killed) && PlayerIdExtensions.ToOriginal(hero.Player) <= 0 && hero.Owner is null)
                {
                    _ = hero.OfferToJoin(source.Player) ? heroes.AddAbility(hero, 10) : heroes.AddAbility(hero, 1);
                }
            }
        }

        Hero? result;
        if (heroes.Count != 0)
        {
            RandSeed = GetEngine().StrategicMap.Random(16777215);
            int index = heroes.RandomChanceItemB(0);
            result = heroes[index].First;
        }
        else
        {
            result = null;
        }
        return result;
    }

    private static readonly string s_callHeroText = "Call Hero";
    private static readonly string s_noHeroAnsweredYourCallText = "No hero answered your call";
}
