// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited multiple targets, target range, dispel resistance, AI value
public partial class DispelMagic : UnitSpell
{
    // UnitSpells.pas:1000
    public DispelMagic(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.DispelMagicText);
        SpellId = 8;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 4;
        AllowMultipleTargets = true;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:1023
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        DispelMagicSpellCombatAction action;
        action = new DispelMagicSpellCombatAction(null);
        action.SpellId = SpellId;
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.Generate(source, target, GetDispelMana(((CombatUnit)source).Troop));
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        return action;
    }

    // UnitSpells.pas:1074
    public override void GetCombatInfo(Troop source, ref SpellCombatInfo info)
    {
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend, CombatTargetTypeElement.Enemy);
    }

    // UnitSpells.pas:1068
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && target.CanDispelEnchantment(source.Player);
    }

    // UnitSpells.pas:1018
    public override bool ValidTargetParty(Troop source, ArmyHexSprite party) => party.Army.Player == source.Player;

    // UnitSpells.pas:1036
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        int count;
        EnchantmentList enchantments;
        int chance;
        if (ValidTargetUnit(source, target))
        {
            count = 0;
            enchantments = new EnchantmentList(null);
            target.ListEnchantments(enchantments);
            for (int I = enchantments.GetCount() - 1; I >= 0; I--)
            {
                var dispelMana = GetDispelMana(source);
                chance = enchantments.GetEnchantments(I).DispelChance(dispelMana);
                if (chance > AoWHSMap.Current.Random(100))
                {
                    count += 1;
                    enchantments.GetEnchantments(I).Dispel();
                }
            }

            if (source.Player == AoWHSMap.Current.SeatedPlayer)
            {
                if (count > 0)
                {
                    var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.SpellsSuccessfullyDispelledFormat), [IntToStr(count)]);
                    AoWHSMap.Current.ShowMessage(format);
                }
                else
                {
                    var dispelMagicFailed = AbilityRules.TranslateResourceString(GameText.DispelMagicFailedText);
                    AoWHSMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            result = enchantments.GetCount() > 0;
            enchantments?.Dispose();
        }
        else
        {
            result = false;
        }

        return result;
    }

    // UnitSpells.pas:1013
    public int GetDispelMana(Troop source) => source.GetAbLevel(52) * 15 + 25;
}
