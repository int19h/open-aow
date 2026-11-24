#nullable disable warnings
// Translated from SummonSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Spells;

// AIBasePriority, UnitIndices
public class SummonSpell : Spell
{

    public int AIBasePriority { get; protected init; }
    public UnitIndexList UnitIndices { get; }

    // SummonSpells.pas:249
    public override void Dispose() => base.Dispose();

    // SummonSpells.pas:241
    public SummonSpell(CustomNode? owner) : base(owner)
    {
        UnitIndices = [];
        SpellType = SpellType.Global;
        AIBasePriority = -1;
    }

    // SummonSpells.pas:288
    public override void AIPrefetchCastSpellActions(Troop source, AICastSpellActionList actions)
    {
        if (AIBasePriority != -1)
        {
            bool canCast = AIRequestCastSpellExpense(upkeep: AIUpkeepPriority, source: source, budget: BaseManaCost, mana: ManaUpkeep);
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
            _ = AICastSpellExpenseMade(upkeep: AIUpkeepPriority, source: source, budget: BaseManaCost, mana: ManaUpkeep);
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

    // SummonSpells.pas:266
    public override bool Activate(Troop source)
    {
        string reason = "";
        if (CanActivate(source, messageText: ref reason))
        {
            SummonSpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
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
    public int AIUpkeepPriority
    {
        get
        {
            int unitIndex = UnitIndices[0];
            return GamePack.Current.AbilityControl.GetAbility<SummonedAbility>(155).UnitIndexToAIUpkeepPriority(unitIndex);
        }
    }

    // SummonSpells.pas:255
    public int GetDispelResistance(Troop? troop) => SummonedAbility.GetDefaultDispelResistance(troop);
}
