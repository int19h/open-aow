#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class AirMasteryEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:649
    public new const int ClassId = 135_966;

    // GlobalEnchantmentSpells.pas:641
    public AirMasteryEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.AirMasteryText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 175;
    }

    // GlobalEnchantmentSpells.pas:654
    public override int GetAIUpkeepPriority() => 1_250;

    // GlobalEnchantmentSpells.pas:679
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.AirMastery);
        // { Original uses Earth. }
        StrategicMap.Current.GlobalMagicControl.DecrementDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }

    // GlobalEnchantmentSpells.pas:659
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result)
        {
            if (StrategicMap.Current.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.AirMastery))
            {
                string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat);
                string name = GetName();
                reason = DelphiFormat.Format(xIsAlreadyActive, args: [name]);
                return false;
            }
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:670
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(PlayerIndex);
        StrategicMap.Current.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments | PlayerEnchantmentTypes.AirMastery);
        // { Original uses Earth. }
        StrategicMap.Current.GlobalMagicControl.IncrementDoubleSphereManaCount(MagicSphere.Earth);
        return true;
    }
}
