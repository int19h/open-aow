#nullable disable warnings
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

namespace Aow.Game.Magic.Spells;

// AIBasePriority
public class GlobalEnchantmentSpell(CustomNode? owner, EngineObject holder) : Spell(owner, holder: holder)
{

    public int AIBasePriority { get; protected init; } = -1;

    // GlobalEnchantmentSpells.pas:429
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        if (executeCount == 0)
        {
            // GlobalEnchantmentSpell.Activate constructs the GlobalEnchantmentSpellTokenEvent this spell executes.
            _ = ((GlobalEnchantmentSpellTokenEvent)eventValue).Enchantment.Cast(source.Player);
        }

        return false;
    }

    // GlobalEnchantmentSpells.pas:494
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        bool aiCanCastSpell = AICanCastSpell(source);
        if (aiCanCastSpell)
        {
            aiCanCastSpell = AIRequestCastSpellExpense(upkeep: AIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
        }

        if (aiCanCastSpell)
        {
            AICastSpellAction action = CreateCastSpellAction();
            SetupCastSpellAction(source, action);
            action.Priority = AIBasePriority;
            action.SetOwner(actions);
        }
    }

    // GlobalEnchantmentSpells.pas:507
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        if (AICanCastSpell(source))
        {
            _ = AICastSpellExpenseMade(upkeep: AIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
            string reason = "";
            if (source.CanCastSpellInstantly(SpellId, mana: BaseManaCost, ref reason))
            {
                return Activate(source);
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

    // GlobalEnchantmentSpells.pas:435
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            if (!GetEngine().StrategicMap.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentTypes.SpellWard))
            {
                GlobalEnchantment? enchantment = CreateEnchantment();
                if (enchantment is not null)
                {
                    SetupEnchantment(source, enchantment);
                    result = enchantment.CanCast(source.Player, reason: ref messageText);
                    _ = enchantment.Release();
                }
            }
            else
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:458
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, messageText: ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            GlobalEnchantmentSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            eventValue.Enchantment = CreateEnchantment();
            SetupEnchantment(source, eventValue.Enchantment);
            GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // GlobalEnchantmentSpells.pas:476
    public virtual bool AICanCastSpell(Troop source)
    {
        string reason = "";
        bool result = AIBasePriority != -1 && source.CanCastSpell(SpellId, ref reason);
        return result && CanActivate(source, messageText: ref reason);
    }

    // GlobalEnchantmentSpells.pas:419
    public virtual GlobalEnchantment? CreateEnchantment() => null;

    // GlobalEnchantmentSpells.pas:424
    public virtual void SetupEnchantment(Troop source, GlobalEnchantment? enchantment) => enchantment.EnchantmentManaUpkeep = ManaUpkeep;

    // GlobalEnchantmentSpells.pas:484
    public int AIUpkeepPriority()
    {
        // { Original creates the base enchantment here. }
        GlobalEnchantment enchantment = new(null) { Holder = CreationHolder() };
        int result = enchantment.GetAIUpkeepPriority();
        _ = enchantment.Release();
        return result;
    }
}
