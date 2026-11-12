// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// (target-unit users, selection bits)
public partial class UnitSpellCaster : SpellCaster
{
    // Spells.PAS:859
    public UnitSpellCaster(ECustomNode owner) : base(owner)
    {
        TargetUnits = new UnitUserList(null);
    }

    public UnitUserList TargetUnits;
    public byte SelectionMask;
    public bool AllowMultipleTargets;
    public byte ValidSelection;

    // Spells.PAS:864
    public override void Dispose()
    {
        TargetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:947
    public override bool CanCast(ref string reason)
    {
        bool result;
        Hero hero;
        int mana;
        hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(CasterSourceId);
        if (hero != null && !hero.UnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (SelectionMask != 0)
            {
                if (!AoWHSMap.Current.Players.GetPlayers(hero.Player).MagicControl.GetCheatEnabled())
                {
                    mana = GetRequiredMana();
                    if (hero.CastingReady() && hero.CastingSpellId == CasterSpellId)
                    {
                        mana -= AoWHSSet.Current.SpellControl.GetSpell(CasterSpellId).BaseManaCost;
                    }

                    if (mana <= hero.GetCastingPoints())
                    {
                        if (mana <= AoWHSMap.Current.Players.GetPlayers(hero.Player).MagicControl.Mana)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughManaText);
                        }
                    }
                    else
                    {
                        result = false;
                        reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughChannelingPointsText);
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GameText.s_noUnitSelectedText);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:981
    public override bool Cast()
    {
        bool result;
        UnitUserList targets;
        string reason = "";
        Troop source;
        if (CanCast(ref reason))
        {
            source = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
            if (source != null)
            {
                targets = new UnitUserList(null);
                for (int I = 0, last = TargetUnits.Count - 1; I <= last; I++)
                {
                    if (((1 << I) & SelectionMask) != 0)
                    {
                        targets.AddUnit(TargetUnits.GetUnit(I));
                    }
                }

                result = ((UnitSpell)AoWHSSet.Current.SpellControl.GetSpell(CasterSpellId)).CastSpell(source, targets);
                targets?.Dispose();
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Spells.PAS:896
    public int GetSelectionCount()
    {
        int bits;
        int result = 0;
        bits = SelectionMask;
        while (bits != 0)
        {
            if ((bits & 1) != 0)
            {
                result += 1;
            }

            bits = (int)(((uint)bits) >> 1);
        }

        return result;
    }

    // Spells.PAS:1085
    public bool Valid()
    {
        bool result;
        Troop source;
        source = AoWHSMap.Current.UnitControl.FindUnit<Troop>(CasterSourceId);
        if (source == null || (((byte)source.UnitStatus.Image) & 16) != 0)
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // intf_P4_02.inc:1038
    public void SetSelection(byte selection)
    {
        byte value = selection;
        var source = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
        ((UnitSpell)AoWHSSet.Current.SpellControl.GetSpell(CasterSpellId)).ValidateUnitSelection(source, TargetUnits, ref value);
        SelectionMask = value;
    }
}
