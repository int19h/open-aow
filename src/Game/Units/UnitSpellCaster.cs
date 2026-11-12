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
        TargetUnits = new UnitUserList(null) { Holder = CreationHolder() };
    }

    public UnitUserList TargetUnits { get; }
    public byte SelectionMask { get; private set; }
    public bool AllowMultipleTargets { get; set; }
    public byte ValidSelection { get; set; }

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
        hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(CasterSourceId);
        if (hero != null && !hero.UnitStatus.Contains(UnitStatusElement.Killed))
        {
            if (SelectionMask != 0)
            {
                if (!StrategicMap.Current.Players.GetPlayers(hero.Player).MagicControl.GetCheatEnabled())
                {
                    mana = GetRequiredMana();
                    if (hero.CastingReady() && hero.CastingSpellId == CasterSpellId)
                    {
                        mana -= GamePack.Current.SpellControl.GetSpell(CasterSpellId).BaseManaCost;
                    }

                    if (mana <= hero.GetCastingPoints())
                    {
                        if (mana <= StrategicMap.Current.Players.GetPlayers(hero.Player).MagicControl.Mana)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughManaText);
                        }
                    }
                    else
                    {
                        result = false;
                        reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughChannelingPointsText);
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
                reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NoUnitSelectedText);
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
            source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
            if (source != null)
            {
                targets = new UnitUserList(null) { Holder = CreationHolder() };
                for (int I = 0, last = TargetUnits.Count - 1; I <= last; I++)
                {
                    if (((1 << I) & SelectionMask) != 0)
                    {
                        targets.AddUnit(TargetUnits.GetUnit(I));
                    }
                }

                result = ((UnitSpell)GamePack.Current.SpellControl.GetSpell(CasterSpellId)).CastSpell(source, targets);
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
        source = StrategicMap.Current.UnitControl.FindUnit<Troop>(CasterSourceId);
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
        var source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        ((UnitSpell)GamePack.Current.SpellControl.GetSpell(CasterSpellId)).ValidateUnitSelection(source, TargetUnits, ref value);
        SelectionMask = value;
    }
}
