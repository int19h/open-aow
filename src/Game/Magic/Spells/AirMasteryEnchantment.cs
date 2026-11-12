// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class AirMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:649
    public new const int ClassId = 135966;

    // GlobalEnchantmentSpells.pas:641
    public AirMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.AirMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:654
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:679
    public override bool Deactivate(byte player)
    {
        base.Deactivate(player);
        var playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
        // { Original uses Earth. }
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }

    // GlobalEnchantmentSpells.pas:659
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.AirMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateResourceString(GameText.IsAlreadyActiveFormat);
                var name = GetName();
                reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                result = false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:670
    public override bool Activate(sbyte player)
    {
        base.Activate(player);
        var playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.AirMastery));
        // { Original uses Earth. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }

}
