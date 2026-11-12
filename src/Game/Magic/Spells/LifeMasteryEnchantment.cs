// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LifeMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1395
    public new const int ClassId = 135967;

    // GlobalEnchantmentSpells.pas:1387
    public LifeMasteryEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.s_lifeMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1398
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1428
    public override bool Deactivate(byte player)
    {
        EngineMessage message = default;
        base.Deactivate(player);
        // { Original doubles Death. }
        AoWHSMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Death);
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        AoWHSMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));
        EngineMessage.InitializeMessage(ref message, 537002240, this);
        AoWHSMap.Current.SendMessage(ref message, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1401
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (AoWHSMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                var xIsAlreadyActive = AbilityRules.TranslateResourceString(GameText.s_isAlreadyActiveFormat);
                var name = GetName();
                reason = RtlUnit.Format(xIsAlreadyActive, [name]);
                result = false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1412
    public override bool Activate(sbyte player)
    {
        Player playerLocal;
        base.Activate(player);
        // { Original doubles Death. }
        AoWHSMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Death);
        playerLocal = AoWHSMap.Current.Players.GetPlayers(PlayerIndex);
        var playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));

        for (int I = playerLocal.Armies.Count - 1; I >= 0; I--)
        {
            for (int J = playerLocal.Armies.GetArmy(I).Count - 1; J >= 0; J--)
            {
                playerLocal.Armies.GetArmy(I).GetUnit(J).SetLifeMasteryBlessing();
            }
        }

        return true;
    }

}
