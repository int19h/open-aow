#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class LifeMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1395
    public new const int ClassId = 135_967;

    // GlobalEnchantmentSpells.pas:1387
    public LifeMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LifeMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:1398
    public override int GetAIUpkeepPriority() => 1_250;

    // GlobalEnchantmentSpells.pas:1428
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        // { Original doubles Death. }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Death);
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        strategicMap.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.LifeMastery);
        EngineMessage.InitializeMessage(out EngineMessage message, messageId: 537002240, sender: this);
        strategicMap.SendMessage(ref message, sender: this);
        return true;
    }

    // GlobalEnchantmentSpells.pas:1401
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.LifeMastery))
            {
                string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat);
                string name = GetName();
                reason = DelphiFormat.Format(xIsAlreadyActive, args: [name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:1412
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        // { Original doubles Death. }
        StrategicMap strategicMap = GetEngine().StrategicMap;
        strategicMap.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Death);
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        PlayerMagicControl playerMagicControl = playerLocal.MagicControl;
        playerMagicControl.SetActiveEnchantments(playerMagicControl.ActiveEnchantments | PlayerEnchantmentTypes.LifeMastery);

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
