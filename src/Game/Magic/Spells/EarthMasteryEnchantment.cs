// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class EarthMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1828
    public new const int ClassId = 135_970;

    // GlobalEnchantmentSpells.pas:1820
    public EarthMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EarthMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1831
    public override int GetAIUpkeepPriority() => 1_250;

    // GlobalEnchantmentSpells.pas:1861
    public override bool Deactivate(byte player)
    {
        _ = base.Deactivate(player);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).ResetEarthMasteryStoneSkin();
            }
        }

        // { Original doubles Air. }
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Air);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1834
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.EarthMastery))
            {
                reason = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat), args: [Name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1845
    public override bool Activate(sbyte player)
    {
        _ = base.Activate(player);
        // { Original doubles Air. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Air);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).SetEarthMasteryStoneSkin();
            }
        }

        return true;
    }
}
