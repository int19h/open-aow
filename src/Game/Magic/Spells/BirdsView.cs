// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, spell type
public partial class BirdsView : Spell
{
    // GlobalSpells.pas:489
    public BirdsView(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_birdsViewText);
        SpellId = 2;
        SpellType = SpellType.Global;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Air;
    }

    // GlobalSpells.pas:505
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        CastingDone(source, eventValue.EventMana);
        source.GetLocation(ref X, ref Y, ref layer);
        if (AoWHSMap.Current.SeatedPlayer == source.Player)
        {
            var birdsViewHexSprite = new BirdsViewHexSprite(null);
            birdsViewHexSprite.StartAnimation(X, Y, layer, 14);
        }

        AoWHSMap.Current.ExploreArea(X, Y, layer, source.Player, 14);
        return false;
    }

    // GlobalSpells.pas:500
    public override bool GetEnabled() => AoWHSMap.Current.ExplorationEnabled;

    // GlobalSpells.pas:518
    public override bool CanActivate(Troop source, ref string messageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            source.GetLocation(ref X, ref Y, ref layer);
            if (layer != 0)
            {
                messageText = AbilityRules.TranslateResourceString(GameText.CannotCastSpellInUndergroundText);
                result = false;
            }
        }

        return result;
    }

    // GlobalSpells.pas:534
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

    private static readonly string s_birdsViewText = "Bird's View";
}
