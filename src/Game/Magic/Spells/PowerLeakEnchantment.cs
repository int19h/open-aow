// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public partial class PowerLeakEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1001
    public new const int ClassID = 135959;

    // GlobalEnchantmentSpells.pas:993
    public PowerLeakEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.PowerLeakRStr);
        FEnchantmentType = GlobalEnchantmentType.PowerLeak;
        FDispelMana = 25;
    }

    // GlobalEnchantmentSpells.pas:1006
    public override int GetAIUpkeepPriority() => 750;

    // GlobalEnchantmentSpells.pas:1021
    public override bool Deactivate(byte Player)
    {
        base.Deactivate(Player);
        for (int I = AoWHSMap.Current.FPlayers.Count - 1; I >= 0; I--)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.Update();
        }

        return true;
    }

    // GlobalEnchantmentSpells.pas:1011
    public override bool Activate(sbyte Player)
    {
        base.Activate(Player);
        for (int I = AoWHSMap.Current.FPlayers.Count - 1; I >= 0; I--)
        {
            AoWHSMap.Current.FPlayers.GetPlayers(I).FMagicControl.Update();
        }

        return true;
    }

}
