// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EarthMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1828
    public new const int ClassId = 135970;

    // GlobalEnchantmentSpells.pas:1820
    public EarthMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.EarthMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1831
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1861
    public override bool Deactivate(byte player)
    {
        Player playerLocal;
        base.Deactivate(player);
        playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        var playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int I = playerLocal.Armies.Count - 1; I >= 0; I--)
        {
            for (int J = playerLocal.Armies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                playerLocal.Armies.GetArmy(I).GetUnit(J).ResetEarthMasteryStoneSkin();
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
                reason = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.IsAlreadyActiveFormat), [Name]);
                result = false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1845
    public override bool Activate(sbyte player)
    {
        Player playerLocal;
        base.Activate(player);
        // { Original doubles Air. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Air);
        playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        var playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.EarthMastery));

        for (int I = playerLocal.Armies.Count - 1; I >= 0; I--)
        {
            for (int J = playerLocal.Armies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                playerLocal.Armies.GetArmy(I).GetUnit(J).SetEarthMasteryStoneSkin();
            }
        }

        return true;
    }

}
