#nullable disable warnings
// Translated from UnitSpells.pas.
using System.Globalization;
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.Magic.Spells;

// inherited multiple targets, target range, dispel resistance, AI value
public class DispelMagic : UnitSpell
{
    // UnitSpells.pas:1000
    public DispelMagic(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DispelMagicText);
        SpellId = 8;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 4;
        AllowMultipleTargets = true;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
    }

    // UnitSpells.pas:1023
    public override CombatAction CreateCombatAction(CombatObject? source, CombatObject target, bool chargeMana)
    {
        DispelMagicSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SpellId = SpellId,
            SourceId = source.Id,
            TargetId = target.Id
        };
        if (source is CombatUnit sourceUnit)
        {
            action.Generate(source, target, dispelMana: GetDispelMana(sourceUnit.Troop));
        }
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        return action;
    }

    // UnitSpells.pas:1074
    public override void GetCombatInfo(Troop? source, ref SpellCombatInfo info)
    {
        base.GetCombatInfo(source, ref info);
        info.TargetTypes = CombatTargetTypes.Friend | CombatTargetTypes.Enemy;
    }

    // UnitSpells.pas:1068
    public override bool ValidTargetUnit(Troop? source, Troop? target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && target.CanDispelEnchantment(source.Player);
    }

    // UnitSpells.pas:1018
    public override bool ValidTargetParty(Troop source, ArmyHexSprite party) => party.Army.Player == source.Player;

    // UnitSpells.pas:1036
    public override bool ExecuteSpell(Troop? source, Troop? target, int mana, Span<int> data)
    {
        bool result;
        if (ValidTargetUnit(source, target))
        {
            int count = 0;
            EnchantmentList enchantments = new(null) { Holder = CreationHolder() };
            target.ListEnchantments(enchantments);
            for (int i = enchantments.GetCount() - 1; i >= 0; i--)
            {
                int dispelMana = GetDispelMana(source);
                int chance = enchantments.GetEnchantments(i).DispelChance(dispelMana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    count += 1;
                    _ = enchantments.GetEnchantments(i).Dispel();
                }
            }

            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                if (count > 0)
                {
                    string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SpellsSuccessfullyDispelledFormat), args: [count.ToString(CultureInfo.InvariantCulture)]);
                    StrategicMap.Current.ShowMessage(format);
                }
                else
                {
                    string dispelMagicFailed = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DispelMagicFailedText);
                    StrategicMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            result = enchantments.GetCount() > 0;
            enchantments?.Dispose();
        }
        else
        {
            return false;
        }

        return result;
    }

    // UnitSpells.pas:1013
    public int GetDispelMana(Troop? source) => (source.GetOwnAbilityLevel(52) * 15) + 25;
}
