#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        Player playerLocal = GetEngine().StrategicMap.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.EarthMastery);

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).ResetEarthMasteryStoneSkin();
            }
        }

        // { Original doubles Air. }
        GetEngine().StrategicMap.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Air);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1834
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.EarthMastery))
            {
                reason = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat), args: [Name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1845
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        // { Original doubles Air. }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Air);
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments | PlayerEnchantmentTypes.EarthMastery);

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
