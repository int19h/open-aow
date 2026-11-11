// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// FAIBasePriority
public partial class GlobalEnchantmentSpell : Spell
{
    public int FAIBasePriority;

    // GlobalEnchantmentSpells.pas:413
    public GlobalEnchantmentSpell(ECustomNode owner) : base(owner)
    {
        FAIBasePriority = -1;
    }

    // GlobalEnchantmentSpells.pas:429
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        if (ExecuteCount == 0)
        {
            ((GlobalEnchantmentSpellTokenEvent)Event).FEnchantment.Cast(Source.FPlayer);
        }

        return false;
    }

    // GlobalEnchantmentSpells.pas:494
    public override void AIPrefetchCastSpellActions(Troop Source, AICastSpellActionList Actions)
    {
        AICastSpellAction Action;
        bool aiCanCastSpell = AICanCastSpell(Source);
        if (aiCanCastSpell)
        {
            var aiUpkeepPriority = AIUpkeepPriority();
            aiCanCastSpell = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
        }

        if (aiCanCastSpell)
        {
            Action = CreateCastSpellAction();
            SetupCastSpellAction(Source, Action);
            Action.FPriority = FAIBasePriority;
            Action.SetOwner(Actions);
        }
    }

    // GlobalEnchantmentSpells.pas:507
    public override bool AIExecuteCastSpellAction(Troop Source)
    {
        bool Result;
        string Reason = "";
        if (AICanCastSpell(Source))
        {
            var aiUpkeepPriority = AIUpkeepPriority();
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, aiUpkeepPriority);
            var hero = (Hero)Source;
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

    // GlobalEnchantmentSpells.pas:435
    public override bool CanActivate(Troop Source, ref string MessageText)
    {
        GlobalEnchantment Enchantment;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            if (!AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard))
            {
                Enchantment = CreateEnchantment();
                if (Enchantment != null)
                {
                    SetupEnchantment(Source, Enchantment);
                    Result = Enchantment.CanCast(Source.FPlayer, ref MessageText);
                    Enchantment.Release();
                }
            }
            else
            {
                Result = false;
                MessageText = AbilityRules.TranslateRStr(GameText.CannotCastGlobalEnchantmentWhenSpellWardIsActiveRStr);
            }
        }

        return Result;
    }

    // GlobalEnchantmentSpells.pas:458
    public override bool Activate(Troop Source)
    {
        bool Result;
        GlobalEnchantmentSpellTokenEvent Event;
        string Reason = "";
        if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new GlobalEnchantmentSpellTokenEvent(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.FEnchantment = CreateEnchantment();
            SetupEnchantment(Source, Event.FEnchantment);
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

    // GlobalEnchantmentSpells.pas:476
    public virtual bool AICanCastSpell(Troop Source)
    {
        string Reason = "";
        bool Result = FAIBasePriority != -1 && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
        return Result && CanActivate(Source, ref Reason);
    }

    // GlobalEnchantmentSpells.pas:419
    public virtual GlobalEnchantment CreateEnchantment() => null;

    // GlobalEnchantmentSpells.pas:424
    public virtual void SetupEnchantment(Troop Source, GlobalEnchantment Enchantment)
    {
        Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
    }

    // GlobalEnchantmentSpells.pas:484
    public int AIUpkeepPriority()
    {
        GlobalEnchantment Enchantment;
        // { Original creates the base enchantment here. }
        Enchantment = new GlobalEnchantment(null);
        int Result = Enchantment.GetAIUpkeepPriority();
        Enchantment.Release();
        return Result;
    }
}
