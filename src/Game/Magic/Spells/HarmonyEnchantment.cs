#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// DaysActive
public class HarmonyEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:921
    public new const int ClassId = 135_958;

    public int DaysActive { get; set; }

    // GlobalEnchantmentSpells.pas:931
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        DaysActive = persister.Int32("DaysActive", value: DaysActive);
    }

    // GlobalEnchantmentSpells.pas:913
    public HarmonyEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HarmonyText);
        EnchantmentType = GlobalEnchantmentType.Harmony;
        DispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:943
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(PlayerIndex))
        {
            NewDay();
        }
    }

    // GlobalEnchantmentSpells.pas:926
    public override int GetAIUpkeepPriority() => 500;

    // GlobalEnchantmentSpells.pas:956
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        if (DaysActive > 10)
        {
            StrategicMap.Current.Races.RelationOffset -= 40;
        }
        else
        {
            StrategicMap.Current.Races.RelationOffset -= DaysActive * 4;
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:949
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        NewDay();
        return true;
    }

    // GlobalEnchantmentSpells.pas:937
    public void NewDay()
    {
        DaysActive += 1;
        if (DaysActive <= 10)
        {
            StrategicMap.Current.Races.RelationOffset += 4;
        }
    }
}
