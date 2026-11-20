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
    public LifeMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), GameText.LifeMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1398
    public override int GetAIUpkeepPriority() => 1250;

    // GlobalEnchantmentSpells.pas:1428
    public override bool Deactivate(byte player)
    {
        _ = base.Deactivate(player);
        // { Original doubles Death. }
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Death);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments - Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));
        EngineMessage.InitializeMessage(out EngineMessage message, 537002240, this);
        StrategicMap.Current.SendMessage(ref message, this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1401
    public override bool CanActivate(sbyte player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentType.LifeMastery))
            {
                string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), GameText.IsAlreadyActiveFormat);
                string name = GetName();
                reason = DelphiFormat.Format(xIsAlreadyActive, [name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1412
    public override bool Activate(sbyte player)
    {
        _ = base.Activate(player);
        // { Original doubles Death. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Death);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments + Set<PlayerEnchantmentType>.Of(PlayerEnchantmentType.LifeMastery));

        for (int i = playerLocal.Armies.Count - 1; i >= 0; i--)
        {
            for (int j = playerLocal.Armies.GetArmy(i).Units.Count - 1; j >= 0; j--)
            {
                playerLocal.Armies.GetArmy(i).Units.GetUnit(j).SetLifeMasteryBlessing();
            }
        }

        return true;
    }

}
