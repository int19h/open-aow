// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class PowerLeakEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1001
    public new const int ClassId = 135959;

    // GlobalEnchantmentSpells.pas:993
    public PowerLeakEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), GameText.PowerLeakText);
        EnchantmentType = GlobalEnchantmentType.PowerLeak;
        DispelMana = 25;
    }

    // GlobalEnchantmentSpells.pas:1006
    public override int GetAIUpkeepPriority() => 750;

    // GlobalEnchantmentSpells.pas:1021
    public override bool Deactivate(byte player)
    {
        _ = base.Deactivate(player);
        for (int i = StrategicMap.Current.Players.Count - 1; i >= 0; i--)
        {
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Update();
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:1011
    public override bool Activate(sbyte player)
    {
        _ = base.Activate(player);
        for (int i = StrategicMap.Current.Players.Count - 1; i >= 0; i--)
        {
            StrategicMap.Current.Players.GetPlayers(i).MagicControl.Update();
        }

        return true;
    }

}
