// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;

using Aow.Game.Units;
using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// AIBasePriority, UnitIndices
public partial class SummonSpell : Spell
{

    public int AIBasePriority { get; protected set; }
    public UnitIndexList UnitIndices { get; }

    // SummonSpells.pas:249
    public override void Dispose()
    {
        UnitIndices?.Dispose();
        base.Dispose();
    }

    // SummonSpells.pas:241
    public SummonSpell(ECustomNode owner) : base(owner)
    {
        UnitIndices = new UnitIndexList();
        SpellType = SpellType.Global;
        AIBasePriority = -1;
    }

    // SummonSpells.pas:288
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        bool canCast;
        AICastSpellAction action;
        string reason = "";
        if (AIBasePriority != -1)
        {
            var aiUpkeepPriority = GetAIUpkeepPriority();
            canCast = AIRequestCastSpellExpense(source, BaseManaCost, ManaUpkeep, aiUpkeepPriority);
            canCast = canCast && ((Hero)source).CanCastSpell(SpellId, ref reason);
            if (canCast)
            {
                action = CreateCastSpellAction();
                SetupCastSpellAction(source, action);
                action.Priority = AIBasePriority;
                action.SetOwner(actions);
            }
        }
    }

    // SummonSpells.pas:308
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        bool result;
        string reason = "";
        var hero = (Hero)source;
        if (hero.CanCastSpell(SpellId, ref reason))
        {
            var aiUpkeepPriority = GetAIUpkeepPriority();
            AICastSpellExpenseMade(source, BaseManaCost, ManaUpkeep, aiUpkeepPriority);
            if (hero.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
            {
                result = this.Activate(source);
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

    // SummonSpells.pas:266
    public override bool Activate(Troop source)
    {
        bool result;
        SummonSpellTokenEvent eventValue;
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            eventValue = new SummonSpellTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            SetupSummonSpellTokenEvent(eventValue);
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

    // SummonSpells.pas:261
    public virtual void SetupSummonSpellTokenEvent(SummonSpellTokenEvent eventValue)
    {
        var random = Random(UnitIndices.Count);
        var unitIndex = UnitIndices[random];
        eventValue.UnitIndices.Add(unitIndex);
    }

    // SummonSpells.pas:283
    public int GetAIUpkeepPriority()
    {
        var unitIndex = UnitIndices[0];
        return ((SummonedAbility)AoWHSSet.Current.AbilityControl.GetAbility(155)).UnitIndexToAIUpkeepPriority(unitIndex);
    }

    // SummonSpells.pas:255
    public int GetDispelResistance(Troop troop) => SummonedAbility.GetDefaultDispelResistance(troop);
}
