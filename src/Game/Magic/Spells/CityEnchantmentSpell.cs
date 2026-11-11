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
    public CityEnchantmentId FEnchantmentID;
    public int FEnchantmentSpellID;
    public int FAIPriority;
    public int FAIBudget;
    public Set<CityEnchantmentCastSpellSituationElement> FAICastSituation;

    // CitySpells.pas:554
    public CityEnchantmentSpell(ECustomNode owner) : base(owner)
    {
        FAIPriority = -1;
    }

    // CitySpells.pas:576
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        City TargetCity;
        CityEnchantment Enchantment;
        TargetCity = ((CityTargetSpellTokenEvent)Event).GetSelectedCity();
        Enchantment = new CityEnchantment(null);
        Enchantment.FEnchantmentID = FEnchantmentID;
        Enchantment.FEnchantmentManaUpkeep = FManaUpkeep;
        var xyl = ((MapField)TargetCity.Owner).GetXYL();
        Enchantment.FEnchantmentLocation.Packed = xyl;
        Enchantment.FDispelMana = FEnchantmentSpellID;
        Enchantment.FPlayerIndex = Source.FPlayer;
        TargetCity.ActivateEnchantment(Enchantment);
        PlayDefaultCityEffects(Event.GetSource(), TargetCity);
        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // CitySpells.pas:594
    public override void AIPrefetchCastSpellActions(Troop Source, AICastSpellActionList Actions)
    {
        bool Valid;
        AICastSpellAction Action;
        string Reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City TargetCity;
        if (FAIPriority != -1)
        {
            Source.GetLocation(ref X, ref Y, ref L);
            TargetCity = (City)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(City));
            bool condition = TargetCity != null;
            if (condition)
            {
                var aiCityEnchantmentUpkeepPriority = TargetCity.AICityEnchantmentUpkeepPriority(FEnchantmentID);
                condition = AIRequestCastSpellExpense(Source, FBaseManaCost, FManaUpkeep, aiCityEnchantmentUpkeepPriority);
            }

            Valid = condition;
            // { Original uses mana upkeep for mana and city priority for upkeep. }
            Valid = Valid && ValidTargetCity(Source, TargetCity);
            Valid = Valid && ((Hero)Source).CanCastSpell(FSpellID, ref Reason);
            if (Valid)
            {
                if (FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.LeaderInCity) && Source is Leader || FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex4City) && TargetCity.GetSize() == 4 || FAICastSituation.Contains(CityEnchantmentCastSpellSituationElement.Hex3City) && TargetCity.GetSize() == 3)
                {
                    Action = CreateCastSpellAction();
                    SetupCastSpellAction(Source, Action);
                    Action.FPriority = FAIPriority;
                    Action.SetOwner(Actions);
                }
            }
        }
    }

    // CitySpells.pas:623
    public override bool AIExecuteCastSpellAction(Troop Source)
    {
        bool Result;
        string Reason = "";
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        City TargetCity;
        Source.GetLocation(ref X, ref Y, ref L);
        TargetCity = (City)AoWHSMap.Current.MapContainer.GetField(X, Y, L).FindNoneTransparentHS(typeof(City));
        if (TargetCity != null && ValidTargetCity(Source, TargetCity) && ((Hero)Source).CanCastSpell(FSpellID, ref Reason))
        {
            AICastSpellExpenseMade(Source, FBaseManaCost, FManaUpkeep, FAIBudget);
            var hero = (Hero)Source;
            if (hero.CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
            {
                Result = TargetSelected(Source, X, Y, L);
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

    // CitySpells.pas:570
    public override bool ValidTargetCity(Troop Source, City TargetCity) => base.ValidTargetCity(Source, TargetCity) && !TargetCity.FEnchantments.Contains(FEnchantmentID);

    // CitySpells.pas:560
    public override CityTargetSpellTokenEvent CreateTE() => new CityEnchantmentSpellTokenEvent(null);

    // CitySpells.pas:565
    public override bool SetupTE(CityTargetSpellTokenEvent Event, Troop Source, int X, int Y, int L) => base.SetupTE(Event, Source, X, Y, L);
}
