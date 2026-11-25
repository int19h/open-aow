#nullable disable warnings
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
    public UnitSpellCaster(CustomNode? owner) : base(owner) => TargetUnits = new UnitUserList(null) { Holder = CreationHolder() };

    public override SpellCasterView InputView => SpellCasterView.UnitTargets;

    public UnitUserList TargetUnits { get; }
    public int SelectionMask { get; private set; }
    public bool AllowMultipleTargets { get; set; }
    public int ValidSelection { get; set; }

    // Spells.PAS:864
    public override void Dispose()
    {
        TargetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:947
    public override bool CanCast(ref string reason)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Hero hero = strategicMap.UnitControl.FindUnit<Hero>(CasterSourceId);
        bool result;
        if (hero is not null && !hero.UnitStatus.Contains(UnitStatuses.Killed))
        {
            if (SelectionMask != 0)
            {
                if (!strategicMap.Players.GetPlayers(hero.Player).MagicControl.GetCheatEnabled())
                {
                    int mana = GetRequiredMana();
                    if (hero.CastingReady() && hero.CastingSpellId == CasterSpellId)
                    {
                        mana -= GetEngine().GamePack.SpellControl[CasterSpellId].BaseManaCost;
                    }

                    if (mana <= hero.GetCastingPoints())
                    {
                        if (mana <= strategicMap.Players.GetPlayers(hero.Player).MagicControl.Mana)
                        {
                            return true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughManaText);
                        }
                    }
                    else
                    {
                        result = false;
                        reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughChannelingPointsText);
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NoUnitSelectedText);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Spells.PAS:981
    public override bool Cast()
    {
        bool result;
        string reason = "";
        if (CanCast(ref reason))
        {
            Troop? source = GetEngine().StrategicMap.UnitControl.FindUnit(CasterSourceId);
            if (source is not null)
            {
                UnitUserList targets = new(null) { Holder = CreationHolder() };
                for (int i = 0, last = TargetUnits.Count - 1; i <= last; i++)
                {
                    if (((1 << i) & SelectionMask) != 0)
                    {
                        _ = targets.AddUnit(TargetUnits.GetUnit(i));
                    }
                }

                result = GetEngine().GamePack.SpellControl.GetSpell<UnitSpell>(CasterSpellId).CastSpell(source, targets);
                targets?.Dispose();
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Spells.PAS:896
    public int GetSelectionCount()
    {
        int result = 0;
        int bits = SelectionMask;
        while (bits != 0)
        {
            if ((bits & 1) != 0)
            {
                result += 1;
            }

            bits = bits >>> 1;
        }

        return result;
    }

    // Spells.PAS:1085
    public bool Valid()
    {
        Troop source = GetEngine().StrategicMap.UnitControl.FindUnit<Troop>(CasterSourceId);
        return source is not null && (((byte)source.UnitStatus) & 16) == 0;
    }

    // intf_P4_02.inc:1038
    public void SetSelection(int selection)
    {
        int value = selection;
        Troop? source = GetEngine().StrategicMap.UnitControl.FindUnit(CasterSourceId);
        value = GetEngine().GamePack.SpellControl.GetSpell<UnitSpell>(CasterSpellId).ValidateUnitSelection(source, targets: TargetUnits, selection: value);
        SelectionMask = value;
    }
}
