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
    public PowerLeakEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.PowerLeakText);
        EnchantmentType = GlobalEnchantmentType.PowerLeak;
        DispelMana = 25;
    }

    // GlobalEnchantmentSpells.pas:1006
    public override int GetAIUpkeepPriority() => 750;

    // GlobalEnchantmentSpells.pas:1021
    public override bool Deactivate(byte player)
    {
        base.Deactivate(player);
        for (int I = StrategicMap.Current.Players.Count - 1; I >= 0; I--)
        {
            StrategicMap.Current.Players.GetPlayers(I).MagicControl.Update();
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:1011
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        for (int I = StrategicMap.Current.Players.Count - 1; I >= 0; I--)
        {
            StrategicMap.Current.Players.GetPlayers(I).MagicControl.Update();
        }

        return true;
    }

}
