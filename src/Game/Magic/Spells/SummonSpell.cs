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

// FAIBasePriority, FUnitIndices
public partial class SummonSpell : Spell
{

    public int FAIBasePriority;
    public UnitIndexList FUnitIndices;

    // SummonSpells.pas:249
    public override void Dispose()
    {
        FUnitIndices?.Dispose();
        base.Dispose();
    }

    // SummonSpells.pas:241
    public SummonSpell(ECustomNode owner) : base(owner)
    {
        FUnitIndices = new UnitIndexList();
        FSpellType = SpellType.Global;
        FAIBasePriority = -1;
    }

    // SummonSpells.pas:288
    public override void AIPrefetchCastSpellActions(Troop Source, AICastSpellActionList Actions)
    {
        bool CanCast;
        AICastSpellAction Action;
        string Reason = "";
        if (FAIBasePriority != -1)
        {
            var aiUpkeepPriority = GetAIUpkeepPriority();
            CanCast = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
            CanCast = CanCast && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
            if (CanCast)
            {
                Action = CreateCastSpellAction();
                SetupCastSpellAction(Source, Action);
                Action.FPriority = FAIBasePriority;
                Action.SetOwner(Actions);
            }
        }
    }

    // SummonSpells.pas:308
    public override bool AIExecuteCastSpellAction(Troop Source)
    {
        bool Result;
        string Reason = "";
        var hero = (Hero)Source;
        if (hero.CanCastSpell(FSpellID, ref Reason))
        {
            var aiUpkeepPriority = GetAIUpkeepPriority();
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
            if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Result = this.Activate(Source);
            }
            else
            {
                if (hero.FCastingSpellID != FSpellID)
                {
                    hero.ExecuteStartCasting(FSpellID);
                    Result = true;
                }
                else
                {
                    Result = true;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // SummonSpells.pas:266
    public override bool Activate(Troop Source)
    {
        bool Result;
        SummonSpellTokenEvent Event;
        string Reason = "";
        if (CanActivate(Source, ref Reason))
        {
            Event = new SummonSpellTokenEvent(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            SetupSummonSpellTE(Event);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // SummonSpells.pas:261
    public virtual void SetupSummonSpellTE(SummonSpellTokenEvent Event)
    {
        var random = Random(FUnitIndices.Count);
        var unitIndice = FUnitIndices[random];
        Event.FUnitIndices.Add(unitIndice);
    }

    // SummonSpells.pas:283
    public int GetAIUpkeepPriority()
    {
        var unitIndice = FUnitIndices[0];
        return ((SummonedAbility)AoWHSSet.Current.FAbilityControl.GetAbility(155)).UnitIndexToAIUpkeepPriority(unitIndice);
    }

    // SummonSpells.pas:255
    public int GetDispelResistance(Troop unit) => SummonedAbility.GetDefaultDispelResistance(unit);
}
