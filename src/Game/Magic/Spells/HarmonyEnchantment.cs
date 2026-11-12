// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// DaysActive
public partial class HarmonyEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:921
    public new const int ClassId = 135958;

    public int DaysActive;

    // GlobalEnchantmentSpells.pas:931
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref DaysActive, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("DaysActive", ref DaysActive);
    }

    // GlobalEnchantmentSpells.pas:913
    public HarmonyEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.HarmonyText);
        EnchantmentType = GlobalEnchantmentType.Harmony;
        DispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:943
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIndex)
        {
            NewDay();
        }
    }

    // GlobalEnchantmentSpells.pas:926
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:956
    public override bool Deactivate(byte player)
    {
        base.Deactivate(player);
        if (DaysActive > 10)
        {
            AoWHSMap.Current.Races.RelationOffset -= 40;
        }
        else
        {
            AoWHSMap.Current.Races.RelationOffset -= DaysActive * 4;
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:949
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        NewDay();
        return true;
    }

    // GlobalEnchantmentSpells.pas:937
    public void NewDay()
    {
        DaysActive += 1;
        if (DaysActive <= 10)
        {
            AoWHSMap.Current.Races.RelationOffset += 4;
        }
    }

}
