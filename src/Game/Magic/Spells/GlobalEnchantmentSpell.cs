#nullable disable
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// AIBasePriority
public partial class GlobalEnchantmentSpell : Spell
{
    public int AIBasePriority { get; protected set; }

    // GlobalEnchantmentSpells.pas:413
    public GlobalEnchantmentSpell(CustomNode owner) : base(owner)
    {
        AIBasePriority = -1;
    }

    // GlobalEnchantmentSpells.pas:429
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        if (executeCount == 0)
        {
            ((GlobalEnchantmentSpellTokenEvent)eventValue).Enchantment.Cast(source.Player);
        }

        return false;
    }

    // GlobalEnchantmentSpells.pas:494
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        AICastSpellAction action;
        bool aiCanCastSpell = AICanCastSpell(source);
        if (aiCanCastSpell)
        {
            aiCanCastSpell = AIRequestCastSpellExpense(upkeep: AIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
        }

        if (aiCanCastSpell)
        {
            action = CreateCastSpellAction();
            SetupCastSpellAction(source, action);
            action.Priority = AIBasePriority;
            action.SetOwner(actions);
        }
    }

    // GlobalEnchantmentSpells.pas:507
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        bool result;
        string reason = "";
        if (AICanCastSpell(source))
        {
            AICastSpellExpenseMade(upkeep: AIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
            if (source.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                result = this.Activate(source);
            }
            else
            {
                if (source.GetCastingSpellId() != SpellId)
                {
                    source.ExecuteStartCasting(SpellId);
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

    // GlobalEnchantmentSpells.pas:435
    public override bool CanActivate(Troop source, ref string messageText)
    {
        GlobalEnchantment enchantment;
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            if (!StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                enchantment = CreateEnchantment();
                if (enchantment != null)
                {
                    SetupEnchantment(source, enchantment);
                    result = enchantment.CanCast(source.Player, ref messageText);
                    enchantment.Release();
                }
            }
            else
            {
                result = false;
                messageText = AbilityRules.TranslateResourceString(GetEngine(), GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveText);
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:458
    public override bool Activate(Troop source)
    {
        bool result;
        GlobalEnchantmentSpellTokenEvent eventValue;
        string reason = "";
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new GlobalEnchantmentSpellTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            eventValue.Enchantment = CreateEnchantment();
            SetupEnchantment(source, eventValue.Enchantment);
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

    // GlobalEnchantmentSpells.pas:476
    public virtual bool AICanCastSpell(Troop source)
    {
        string reason = "";
        bool result = AIBasePriority != -1 && source.CanCastSpell(SpellId, ref reason);
        return result && CanActivate(source, ref reason);
    }

    // GlobalEnchantmentSpells.pas:419
    public virtual GlobalEnchantment CreateEnchantment() => null;

    // GlobalEnchantmentSpells.pas:424
    public virtual void SetupEnchantment(Troop source, GlobalEnchantment enchantment)
    {
        enchantment.EnchantmentManaUpkeep = ManaUpkeep;
    }

    // GlobalEnchantmentSpells.pas:484
    public int AIUpkeepPriority()
    {
        GlobalEnchantment enchantment;
        // { Original creates the base enchantment here. }
        enchantment = new GlobalEnchantment(null) { Holder = CreationHolder() };
        int result = enchantment.GetAIUpkeepPriority();
        enchantment.Release();
        return result;
    }
}
