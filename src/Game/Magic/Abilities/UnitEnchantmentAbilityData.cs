// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

// (ability ID, duration, player, enchantment)
public partial class UnitEnchantmentAbilityData : AbilityData
{
    public new const int ClassId = 131756;

    // AoWAb.PAS:555
    public UnitEnchantmentAbilityData(ECustomNode owner) : base(owner)
    {
        EnchantmentPlayerIndex = -1;
        EnchantmentDuration = -1;
    }

    public ushort EnchantmentAbilityId { get; set; }
    public sbyte EnchantmentDuration { get; set; }
    // -1 is permanent
    public sbyte EnchantmentPlayerIndex { get; set; }
    public UnitEnchantment Enchantment { get; set; }

    // AoWAb.PAS:567
    public override void Dispose()
    {
        UnitEnchantment enchantment;
        enchantment = Enchantment;
        if (enchantment != null)
        {
            enchantment?.Dispose();
        }

        base.Dispose();
    }

    // AoWAb.PAS:592
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentAbilityId = persister.UInt16("EnchantmentAbilityId", value: EnchantmentAbilityId);
        Enchantment = persister.Created("Enchantment", objectValue: Enchantment);
        EnchantmentPlayerIndex = persister.SByte("EnchantmentPlayerIndex", value: EnchantmentPlayerIndex, fallback: -1);
        EnchantmentDuration = persister.SByte("EnchantmentDuration", value: EnchantmentDuration, fallback: -1);
    }

    // AoWAb.PAS:577
    public override int GetAbilityId() => EnchantmentAbilityId;

    // AoWAb.PAS:582
    public void SetEnchantment(UnitEnchantment enchantment)
    {
        UnitEnchantment oldEnchantment;
        oldEnchantment = Enchantment;
        if (oldEnchantment != null)
        {
            oldEnchantment?.Dispose();
        }

        Enchantment = enchantment;
    }

    // intf_P4_02.inc:789

}
