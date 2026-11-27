#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class EnchantedRoadsEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:535
    public new const int ClassId = 135_955;

    // GlobalEnchantmentSpells.pas:527
    public EnchantedRoadsEnchantment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Name = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.EnchantedRoadsText);
        EnchantmentType = GlobalEnchantmentType.None;
        DispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:540
    public override int GetAIUpkeepPriority() => 900;

    // GlobalEnchantmentSpells.pas:563
    public override bool Deactivate(PlayerId? player)
    {
        _ = base.Deactivate(player);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        strategicMap.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments & ~PlayerEnchantmentTypes.EnchantedRoads);
        return true;
    }

    // GlobalEnchantmentSpells.pas:545
    public override bool CanActivate(PlayerId? player, ref string reason)
    {
        bool result = base.CanActivate(player, ref reason);
        if (result && GetEngine().StrategicMap.Players.GetPlayers(player).MagicControl.ActiveEnchantments.Contains(PlayerEnchantmentTypes.EnchantedRoads))
        {
            string xIsAlreadyActive = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.IsAlreadyActiveFormat);
            string name = GetName();
            reason = DelphiFormat.Format(xIsAlreadyActive, args: [name]);
            return false;
        }

        return result;
    }

    // GlobalEnchantmentSpells.pas:555
    public override bool Activate(PlayerId? player)
    {
        _ = base.Activate(player);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Player playerLocal = strategicMap.Players.GetPlayers(PlayerIndex);
        strategicMap.Players.GetPlayers(PlayerIndex).MagicControl.SetActiveEnchantments(playerLocal.MagicControl.ActiveEnchantments | PlayerEnchantmentTypes.EnchantedRoads);
        return true;
    }
}
