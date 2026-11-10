// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// FDaysActive
public partial class HarmonyEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:921
    public new const int ClassID = 135958;

    public int FDaysActive;

    // GlobalEnchantmentSpells.pas:931
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FDaysActive, 20);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("DaysActive", ref FDaysActive);
    }

    // GlobalEnchantmentSpells.pas:913
    public HarmonyEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.HarmonyRStr);
        FEnchantmentType = GlobalEnchantmentType.Harmony;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:943
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001986 && Msg.Payload.Int == FPlayerIndex)
        {
            NewDay();
        }
    }

    // GlobalEnchantmentSpells.pas:926
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:956
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        if (FDaysActive > 10)
        {
            AoWHSMap.Current.FRaces.FRelationOffset -= 40;
        }
        else
        {
            AoWHSMap.Current.FRaces.FRelationOffset -= FDaysActive * 4;
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:949
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        NewDay();
        return true;
    }

    // GlobalEnchantmentSpells.pas:937
    public void NewDay()
    {
        FDaysActive += 1;
        if (FDaysActive <= 10)
        {
            AoWHSMap.Current.FRaces.FRelationOffset += 4;
        }
    }

}
