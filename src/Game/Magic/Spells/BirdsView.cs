#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, spell type
public class BirdsView : Spell
{
    // GlobalSpells.pas:489
    public BirdsView(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_birdsViewText);
        SpellId = 2;
        SpellType = SpellType.Global;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Air;
    }

    // GlobalSpells.pas:505
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        CastingDone(source, mana: eventValue.EventMana);
        MapCoordinate coordinate53 = source.GetLocation();
        sbyte x = coordinate53.X;
        sbyte y = coordinate53.Y;
        sbyte layer = coordinate53.MapLevel;
        if (StrategicMap.Current.SeatedPlayer == source.Player)
        {
            BirdsViewHexSprite birdsViewHexSprite = new(null) { Holder = CreationHolder() };
            birdsViewHexSprite.StartAnimation(x, y, layer, radius: 14);
        }

        _ = StrategicMap.Current.ExploreArea(x, y, layer, source.Player, range: 14);
        return false;
    }

    // GlobalSpells.pas:500
    public override bool Enabled => StrategicMap.Current.ExplorationEnabled;

    // GlobalSpells.pas:518
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            MapCoordinate coordinate54 = source.GetLocation();
            _ = coordinate54.X;
            _ = coordinate54.Y;
            sbyte layer = coordinate54.MapLevel;
            if (layer != 0)
            {
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastSpellInUndergroundText);
                return false;
            }
        }

        return result;
    }

    // GlobalSpells.pas:534
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

    private static readonly string s_birdsViewText = "Bird's View";
}
