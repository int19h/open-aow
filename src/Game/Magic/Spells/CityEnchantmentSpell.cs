// Translated from CitySpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// enchantment ID, enchantment setting, AI priority, AI budget, AI cast situations
public partial class CityEnchantmentSpell : CityTargetSpell
{
    public CityEnchantmentId EnchantmentId { get; protected set; }
    public int EnchantmentSpellId { get; protected set; }
    public int AIPriority { get; protected set; }
    public int AIBudget { get; protected set; }
    public Set<CityEnchantmentCastSpellSituationElement> AICastSituation { get; protected set; }

    // CitySpells.pas:554
    public CityEnchantmentSpell(ECustomNode owner) : base(owner)
    {
        AIPriority = -1;
    }

    // CitySpells.pas:576
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        City targetCity;
        CityEnchantment enchantment;
        targetCity = ((CityTargetSpellTokenEvent)eventValue).GetSelectedCity();
        enchantment = new CityEnchantment(null);
        enchantment.CityEnchantmentId = EnchantmentId;
        enchantment.EnchantmentManaUpkeep = ManaUpkeep;
        var mapCoordinate = targetCity.Owner.GetMapCoordinate();
        enchantment.EnchantmentLocation.Packed = mapCoordinate;
        enchantment.DispelMana = EnchantmentSpellId;
        enchantment.PlayerIndex = source.Player;
        targetCity.ActivateEnchantment(enchantment);
        PlayDefaultCityEffects(eventValue.GetSource(), targetCity);
        CastingDone(source, eventValue.EventMana);
        return false;
    }

    // CitySpells.pas:594
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        bool valid;
        AICastSpellAction action;
        string reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City targetCity;
        if (AIPriority != -1)
        {
            source.GetLocation(out X, out Y, out L);
            targetCity = (City)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(City));
            bool condition = targetCity != null;
            if (condition)
            {
                var aiCityEnchantmentUpkeepPriority = targetCity.AICityEnchantmentUpkeepPriority(EnchantmentId);
                condition = AIRequestCastSpellExpense(source, BaseManaCost, ManaUpkeep, aiCityEnchantmentUpkeepPriority);
            }

            valid = condition;
            // { Original uses mana upkeep for mana and city priority for upkeep. }
            valid = valid && ValidTargetCity(source, targetCity);
            valid = valid && ((Hero)source).CanCastSpell(SpellId, ref reason);
            if (valid)
            {
                if (AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.LeaderInCity) && source is Leader || AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex4City) && targetCity.GetSize() == 4 || AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex3City) && targetCity.GetSize() == 3)
                {
                    action = CreateCastSpellAction();
                    SetupCastSpellAction(source, action);
                    action.Priority = AIPriority;
                    action.SetOwner(actions);
                }
            }
        }
    }

    // CitySpells.pas:623
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        bool result;
        string reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City targetCity;
        source.GetLocation(out X, out Y, out L);
        targetCity = (City)StrategicMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHexSprite(typeof(City));
        if (targetCity != null && ValidTargetCity(source, targetCity) && ((Hero)source).CanCastSpell(SpellId, ref reason))
        {
            AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, AIBudget);
            var hero = (Hero)source;
            if (hero.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                result = TargetSelected(source, X, Y, L);
            }
            else
            {
                if (hero.CastingSpellId != SpellId)
                {
                    hero.ExecuteStartCasting(SpellId);
                    result = true;
                }
                else
                {
                    result = true;
                }
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // CitySpells.pas:570
    public override bool ValidTargetCity(Troop source, City targetCity) => base.ValidTargetCity(source, targetCity) && !targetCity.Enchantments.Contains(EnchantmentId);

    // CitySpells.pas:560
    public override CityTargetSpellTokenEvent CreateTokenEvent() => new CityEnchantmentSpellTokenEvent(null);

    // CitySpells.pas:565
    public override bool SetupTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int X, int Y, int L) => base.SetupTokenEvent(eventValue, source, X, Y, L);
}
