// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class WaterMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:760
    public new const int ClassId = 135956;

    // GlobalEnchantmentSpells.pas:752
    public WaterMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.WaterMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:765
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:791
    public override bool Deactivate(byte player)
    {
        base.Deactivate(player);
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        AoWHSMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
        AoWHSMap.Current.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Fire);
        return true;
    }

    // GlobalEnchantmentSpells.pas:770
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.WaterMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateResourceString(GameText.IsAlreadyActiveFormat);
                var name = GetName();
                reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                result = false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:781
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        AoWHSMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.WaterMastery));
        AoWHSMap.Current.UpdateVisibility();
        // { Original uses Fire for Water Mastery. }
        AoWHSMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Fire);
        return true;
    }

}
