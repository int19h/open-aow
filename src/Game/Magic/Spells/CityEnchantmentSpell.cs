#nullable disable
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
public class CityEnchantmentSpell(CustomNode owner) : CityTargetSpell(owner)
{

    public CityEnchantmentId EnchantmentId { get; protected set; }
    public int EnchantmentSpellId { get; protected set; }
    public int AIPriority { get; protected set; } = -1;
    public int AIBudget { get; protected set; }
    public Set<CityEnchantmentCastSpellSituationElement> AICastSituation { get; protected set; }

    // CitySpells.pas:576
    protected override bool ExecuteTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int executeCount)
    {
        City targetCity = eventValue.GetSelectedCity();
        CityEnchantment enchantment = new(null)
        {
            Holder = CreationHolder(),
            CityEnchantmentId = EnchantmentId,
            EnchantmentManaUpkeep = ManaUpkeep
        };
        MapCoordinate mapCoordinate = targetCity.Owner.GetMapCoordinate();
        enchantment.EnchantmentLocation = mapCoordinate;
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
        if (AIPriority != -1)
        {
            MapCoordinate coordinate56 = source.GetLocation();
            sbyte x = coordinate56.X;
            sbyte y = coordinate56.Y;
            sbyte l = coordinate56.MapLevel;
            City targetCity = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<City>();
            bool condition = targetCity is not null;
            if (condition)
            {
                condition = AIRequestCastSpellExpense(upkeep: targetCity.AICityEnchantmentUpkeepPriority(EnchantmentId), source: source, budget: BaseManaCost, mana: ManaUpkeep);
            }

            bool valid = condition;
            // { Original uses mana upkeep for mana and city priority for upkeep. }
            valid = valid && ValidTargetCity(source, targetCity);
            string reason = "";
            valid = valid && source.CanCastSpell(SpellId, ref reason);
            if (valid)
            {
                if ((AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.LeaderInCity) && source is Leader) || (AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex4City) && targetCity.GetSize() == 4) || (AICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex3City) && targetCity.GetSize() == 3))
                {
                    AICastSpellAction action = CreateCastSpellAction();
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
        string reason = "";
        MapCoordinate coordinate57 = source.GetLocation();
        sbyte x = coordinate57.X;
        sbyte y = coordinate57.Y;
        sbyte l = coordinate57.MapLevel;
        City targetCity = StrategicMap.Current.MapContainer.GetField(x, y, l).FindNoneTransparentHexSprite<City>();
        if (targetCity is not null && ValidTargetCity(source, targetCity) && source.CanCastSpell(SpellId, ref reason))
        {
            _ = AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, AIBudget);
            if (source.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                return TargetSelected(source, x, y, l);
            }
            else
            {
                if (source.GetCastingSpellId() != SpellId)
                {
                    _ = source.ExecuteStartCasting(SpellId);
                    return true;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
    }

    // CitySpells.pas:570
    public override bool ValidTargetCity(Troop source, City targetCity) => base.ValidTargetCity(source, targetCity) && !targetCity.Enchantments.Contains(EnchantmentId);

    // CitySpells.pas:560
    public override CityTargetSpellTokenEvent CreateTokenEvent() => new CityEnchantmentSpellTokenEvent(null) { Holder = CreationHolder() };

    // CitySpells.pas:565
    public override bool SetupTokenEvent(CityTargetSpellTokenEvent eventValue, Troop source, int X, int Y, int L) => base.SetupTokenEvent(eventValue, source, X, Y, L);
}
