#nullable disable
// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;

using Aow.Game.Units;
using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// AIBasePriority, UnitIndices
public class SummonSpell : Spell
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
    public SummonSpell(CustomNode owner) : base(owner)
    {
        UnitIndices = new UnitIndexList();
        SpellType = SpellType.Global;
        AIBasePriority = -1;
    }

    // SummonSpells.pas:288
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        if (AIBasePriority != -1)
        {
            bool canCast = AIRequestCastSpellExpense(upkeep: GetAIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
            string reason = "";
            canCast = canCast && source.CanCastSpell(SpellId, ref reason);
            if (canCast)
            {
                AICastSpellAction action = CreateCastSpellAction();
                SetupCastSpellAction(source, action);
                action.Priority = AIBasePriority;
                action.SetOwner(actions);
            }
        }
    }

    // SummonSpells.pas:308
    public override bool AIExecuteCastSpellAction(Troop source)
    {
        string reason = "";
        if (source.CanCastSpell(SpellId, ref reason))
        {
            _ = AICastSpellExpenseMade(upkeep: GetAIUpkeepPriority(), source: source, budget: BaseManaCost, mana: ManaUpkeep);
            if (source.CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
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

    // SummonSpells.pas:266
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, ref reason))
        {
            SummonSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            SetupSummonSpellTokenEvent(eventValue);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    // SummonSpells.pas:261
    public virtual void SetupSummonSpellTokenEvent(SummonSpellTokenEvent eventValue)
    {
        int random = Random(UnitIndices.Count);
        int unitIndex = UnitIndices[random];
        _ = eventValue.UnitIndices.Add(unitIndex);
    }

    // SummonSpells.pas:283
    public int GetAIUpkeepPriority()
    {
        int unitIndex = UnitIndices[0];
        return GamePack.Current.AbilityControl.GetAbility<SummonedAbility>(155).UnitIndexToAIUpkeepPriority(unitIndex);
    }

    // SummonSpells.pas:255
    public int GetDispelResistance(Troop troop) => SummonedAbility.GetDefaultDispelResistance(troop);
}
