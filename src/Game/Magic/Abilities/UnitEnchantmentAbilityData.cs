// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

// (ability ID, duration, player, enchantment)
public partial class UnitEnchantmentAbilityData : AbilityData
{
    public new const int ClassID = 131756;

    // AoWAb.PAS:555
    public UnitEnchantmentAbilityData(ECustomNode owner) : base(owner)
    {
        FEnchantmentPlayerIndex = -1;
        FEnchantmentDuration = -1;
    }

    public ushort FEnchantmentAbilityID;
    public sbyte FEnchantmentDuration;
    // -1 is permanent
    public sbyte FEnchantmentPlayerIndex;
    public UnitEnchantment FEnchantment;

    // AoWAb.PAS:567
    public override void Dispose()
    {
        UnitEnchantment Enchantment;
        Enchantment = FEnchantment;
        if (Enchantment != null)
        {
            Enchantment?.Dispose();
        }

        base.Dispose();
    }

    // AoWAb.PAS:592
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwWord(ref FEnchantmentAbilityID, 15);
        Stream.rwCreateEObject(ref FEnchantment, null, 16);
        Stream.rwShortintEx(ref FEnchantmentPlayerIndex, -1, 17);
        Stream.rwShortintEx(ref FEnchantmentDuration, -1, 18);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.UShort("EnchantmentAbilityID", ref FEnchantmentAbilityID);
        persister.Created("Enchantment", ref FEnchantment);
        persister.SByte("EnchantmentPlayerIndex", ref FEnchantmentPlayerIndex, -1);
        persister.SByte("EnchantmentDuration", ref FEnchantmentDuration, -1);
    }

    // AoWAb.PAS:577
    public override int GetAbilityID() => FEnchantmentAbilityID;

    // AoWAb.PAS:582
    public void SetEnchantment(UnitEnchantment Enchantment)
    {
        UnitEnchantment OldEnchantment;
        OldEnchantment = FEnchantment;
        if (OldEnchantment != null)
        {
            OldEnchantment?.Dispose();
        }

        FEnchantment = Enchantment;
    }

    // intf_P4_02.inc:789

}
